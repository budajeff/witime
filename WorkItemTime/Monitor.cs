using System;
using System.Data;
using System.Linq;
using Microsoft.Win32;

namespace WorkItemTime
{
    public class Monitor : IDisposable
	{
        readonly DataSet _uberSet;
        readonly DataTable _activityTable;
        public Monitor(DataSet uberSet)
        {
            this._uberSet = uberSet;
            this._activityTable = this._uberSet.Tables[Data.ActivityTableName];
        }

        public void Start()
        {
            SystemEvents.SessionSwitch -= SystemEventsOnSessionSwitch;
            SystemEvents.SessionSwitch += SystemEventsOnSessionSwitch;

			SystemEvents.SessionEnded -= SystemEventsOnSessionEnded;
	        SystemEvents.SessionEnded += SystemEventsOnSessionEnded;
        }

		private void SystemEventsOnSessionEnded(object sender, SessionEndedEventArgs sessionEndedEventArgs)
		{
			var description = Enum.GetName(typeof(SessionSwitchReason), sessionEndedEventArgs.Reason);
			this.Log(description, Data.ActivityKindStop);
		}

		public void Log(string description, string activityKind)
        {
            var row = this._activityTable.NewRow();
            row.SetField(Data.ActivityDateTime, DateTime.Now);
            row.SetField(Data.ActivityDescription, description);
			row.SetField(Data.ActivityKind, activityKind);
            row.SetField(Data.ActivityStatus, Data.ActivityStatusUnapproved);
            this._activityTable.Rows.Add(row);
   
        }

        private void SystemEventsOnSessionSwitch(object sender, SessionSwitchEventArgs sessionSwitchEventArgs)
		{
			var description = Enum.GetName(typeof(SessionSwitchReason), sessionSwitchEventArgs.Reason);
			if (sessionSwitchEventArgs.Reason == SessionSwitchReason.SessionLock ||
			    sessionSwitchEventArgs.Reason == SessionSwitchReason.SessionLogoff)
			{
				this.Log(description, Data.ActivityKindStop);
			}
			else if (sessionSwitchEventArgs.Reason == SessionSwitchReason.SessionUnlock||
			         sessionSwitchEventArgs.Reason == SessionSwitchReason.SessionLogon)
			{
				this.Log(description, Data.ActivityKindStart);
			}
		}

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SystemEvents.SessionSwitch -= SystemEventsOnSessionSwitch; 
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Monitor() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }

	public class Data
	{
        public DataSet UberSet { get; private set; }

        public const string SettingsTableName = "settings";
        public const string SettingsTableKey = "key";
        public const string SettingsTableValue = "value";
        public const string SettingsTableComment = "comment";
		public const string SettingsTfptPathAndFileName = "TFPT Path and File Name";
		public const string SettingsTfsCollectionName = "TFS Collection Name";
		public const string SettingsTfsWorkHoursFieldName = "TFS Work Hours Field Name";

        public const string ActivityTableName = "activity";
        public const string ActivityDateTime = "datetime";
        public const string ActivityDescription = "description";
		public const string ActivityComment = "comment";
        public const string ActivityStatus = "status";
		public const string ActivityKind = "kind";
		public const string ActivityWorkItem = "workitem";
        public const string ActivityStatusUnapproved = "unapproved";

		public const string ActivityKindStart = "start";
		public const string ActivityKindStop = "stop";

        public const string LogTableName = "log";
        public const string LogMessage = "message";

		public const string TfsEditsTableName = "tfsEdits";
		public const string TfsEditsWorkItem = ActivityWorkItem;
		public const string TfsEditsDuration = "duration";
		public const string TfsEditsComment = ActivityComment;

        public void Load()
        {
            this.UberSet = new DataSet();
            try
            {
                this.UberSet.ReadXml("data.xml");
            }
            catch(Exception ex)
            {             
                this.UberSet = this.CreateDataSet();
            }
        }

        public void Save()
        {
            this.UberSet.WriteXml("data.xml", XmlWriteMode.WriteSchema);
        }

		public DataSet CreateDataSet()
		{
			var dataSet = new DataSet();
			var activityTable = dataSet.Tables.Add(ActivityTableName);
			activityTable.Columns.Add(ActivityDateTime, typeof(DateTime));
            activityTable.Columns.Add(ActivityDescription, typeof(string));
			activityTable.Columns.Add(ActivityKind, typeof(string));
			activityTable.Columns.Add(ActivityComment, typeof(string));
			activityTable.Columns.Add(ActivityStatus, typeof(string));//approved, posting, posted
			var wi = activityTable.Columns.Add(ActivityWorkItem, typeof(Int32));
            wi.AllowDBNull = true;
            
			var settingsTable = dataSet.Tables.Add(SettingsTableName);
			settingsTable.Columns.Add(SettingsTableKey, typeof(string));
			settingsTable.Columns.Add(SettingsTableValue, typeof(string)); 
			settingsTable.Columns.Add(SettingsTableComment, typeof(string));

			settingsTable.Rows.Add(SettingsTfptPathAndFileName, @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\tfpt.exe", "the exe");
			settingsTable.Rows.Add(SettingsTfsCollectionName, "http://tfstta.int.thomson.com:8080/tfs/DefaultCollection", "the collection");
			settingsTable.Rows.Add(SettingsTfsWorkHoursFieldName, "Actual Work", "TFS WI field name to increment");

			var tfsEditsTable = dataSet.Tables.Add(TfsEditsTableName);
            wi = tfsEditsTable.Columns.Add(TfsEditsWorkItem, typeof(Int32));
            wi.AllowDBNull = true;
            //tfsEditsTable.PrimaryKey = new[] { tfsEditsTable.Columns.Add(TfsEditsWorkItem)};
			tfsEditsTable.Columns.Add(TfsEditsDuration);
			tfsEditsTable.Columns.Add(TfsEditsComment);

			return dataSet;
		}

		public void CalculateDurations(DataSet uberSet)
		{
			var activityTable = uberSet.Tables[Data.ActivityTableName];
			var tfsEditsTable = uberSet.Tables[Data.TfsEditsTableName];
			tfsEditsTable.Rows.Clear();

			DataRow previousActivity = null;
			DateTime? currentDateTimeStart = null;

			DataRow tfsEdit = null;

			foreach (DataRow currentActivity in activityTable.Rows.OfType<DataRow>().OrderBy(row=>row.Field<DateTime>(Data.ActivityDateTime)))
			{
				if (currentActivity.Field<string>(Data.ActivityKind) == ActivityKindStart && previousActivity == null)
				{
					//make the tfs row
					tfsEdit = tfsEditsTable.NewRow();
					tfsEdit.SetField(Data.TfsEditsDuration, 0);

					//iterate to second row
					previousActivity = currentActivity;
					continue;
				}
                if (currentActivity.Field<string>(Data.ActivityKind) == ActivityKindStart
                    && previousActivity.Field<string>(Data.ActivityKind) == ActivityKindStop)
                {
                    if (currentActivity.Field<Int32?>(Data.ActivityWorkItem).HasValue)
                    {
                        //see if we have already encountered this WI
                        tfsEdit = tfsEditsTable.AsEnumerable()
                            .FirstOrDefault(
                            row =>
                            {
                                return
                                row.Field<Int32?>(Data.ActivityWorkItem).HasValue
                                && row.Field<Int32?>(Data.ActivityWorkItem).Value ==
                                currentActivity.Field<Int32?>(Data.ActivityWorkItem).Value;
                            });

                        if (tfsEdit == null)
                        {
                            //make the tfs row
                            tfsEdit = tfsEditsTable.NewRow();
                            tfsEdit.SetField(Data.TfsEditsDuration, 0);
                        }
                    }
                }
                else if (currentActivity.Field<string>(Data.ActivityKind) == ActivityKindStop
                    && previousActivity.Field<string>(Data.ActivityKind) == ActivityKindStart)
                {
                    //accumulate the time
                    var duration = currentActivity.Field<DateTime>(Data.ActivityDateTime)
                        - previousActivity.Field<DateTime>(Data.ActivityDateTime);

                    tfsEdit.SetField(Data.TfsEditsDuration, duration.Minutes);
                    tfsEdit.SetField(Data.TfsEditsWorkItem, previousActivity.Field<Int32?>(Data.ActivityWorkItem));
                    tfsEdit.SetField(Data.TfsEditsComment, currentActivity.Field<string>(Data.ActivityComment));

                    if(tfsEditsTable.Rows.IndexOf(tfsEdit) == -1)
                    {
                        tfsEditsTable.Rows.Add(tfsEdit);
                    }
                }
				previousActivity = currentActivity;
			}
		}
	}

	///// <summary>
	///// Writes TFS WI data via tfpt.exe
	///// </summary>
	///// <remarks>
	///// 
	///// C:\Program Files (x86)\Microsoft Visual Studio 12.0>tfpt workitem /collection:http://tfstta.int.thomson.com:8080/tfs/DefaultCollection 1245593
	/////
	/////-------------------------------------------------------------------------------
	/////Work Item: 1245593
	/////Onvio Country = All
	/////Sprint Team = Time & Billing
	/////Actual Work = 33
	/////Estimated Work = 40
	/////Backlog Priority =
	/////Activity =
	/////Blocked =
	/////Remaining Work =
	/////Integration Build =
	/////Closed Date = 9/18/2017 2:35:16 PM
	/////Board Lane =
	/////Board Column Done =
	/////Board Column =
	/////Tags =
	/////Related Link Count = 1
	/////History =
	/////Description = Enchantments to the Activity Log Grid(perviously user stories 1
	/////-7 but now made into one for planning purposes)
	/////Created By = Day, Derek E. (TR Technology & Ops)
	/////	Created Date = 9 / 11 / 2017 1:49:06 PM
	/////Work Item Type = Task
	/////Assigned To = Buda, Jeff (TR Technology & Ops)
	/////	Reason = Work finished
	/////Changed By = Buda, Jeff (TR Technology & Ops)
	/////Rev = 9
	/////Watermark = 5360536
	/////Authorized Date = 9 / 18 / 2017 2:35:16 PM
	/////State = Done
	/////Title = Activity Log Grid Enhancements Front End
	/////Authorized As = Buda, Jeff (TR Technology & Ops)
	/////Area Id = 32445
	/////ID = 1245593
	/////Changed Date = 9 / 18 / 2017 2:35:16 PM
	/////Revised Date = 1/1/9999 12:00:00 AM
	/////Area Path = BlueMoonCore
	/////Node Name = BlueMoonCore
	/////Attached File Count = 0
	/////Hyperlink Count = 0
	/////Team Project = BlueMoonCore
	/////External Link Count = 0
	/////Iteration ID = 48900
	/////Iteration Path = BlueMoonCore\Current\Sprint 17.10.05r
	/////Links
	/////
	/////Related Workitem    Work Item: 1245592
	/////	C:\Program Files(x86)\Microsoft Visual Studio 12.0>tfpt workitem /update /collection:http://tfstta.int.thomson.com:8080/tfs/DefaultCollection 1245593 /fields:"Actual Work = 34"
	/////Work item 1245593 updated.
	///// </remarks>
	//public class TfsIntegration
	//{
 //       readonly DataSet _uberSet;
 //       readonly DataTable _settings;
	//	public TfsIntegration(DataSet uberSet)
	//	{
 //           this._uberSet = uberSet;
 //           this._settings = uberSet.Tables[Data.SettingsTableName];
	//	}

 //       public ReadWorkHours()
 //       {

 //       }

 //       public IncrementWorkHours(
 //           Int32 workItemNumber,
 //           Decimal incrementAmount)
 //       {
 //           var tfptPathAndFileName = this._settings.Rows.Find(Data.SettingsTfptPathAndFileName).Field<string>(Data.SettingsTableValue);
 //           var tfsCollectionName = this._settings.Rows.Find(Data.SettingsTfsCollectionName).Field<string>(Data.SettingsTableValue);
 //           var workHoursFieldName = this._settings.Rows.Find(Data.SettingsTfsWorkHoursFieldName).Field<string>(Data.SettingsTableValue);
 //           return this.PerformTfsOperation($"workitem {operation} /collection{tfsCollectionName} {workItemNumber} /fields:\"{workHoursFieldName}\" = {workHoursFieldName}");

 //           var startInfo = new ProcessStartInfo();
 //           startInfo.FileName = tfptPathAndFileName;
 //           startInfo.Arguments = arguments;
 //           startInfo.RedirectStandardError = true;
 //           startInfo.RedirectStandardOutput = true;
 //           startInfo.RedirectStandardInput = false;

 //           using (var process = Process.Start(startInfo))
 //           {
 //               process.Start();
 //               var output = process.StandardOutput.ReadToEnd();
 //               var error = process.StandardError.ReadToEnd();
 //           }

 //       }

 //   }
}

