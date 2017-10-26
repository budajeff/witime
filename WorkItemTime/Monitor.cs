using System;
using System.Data;
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
        }

        private void SystemEventsOnSessionSwitch(object sender, SessionSwitchEventArgs sessionSwitchEventArgs)
		{
            var row = this._activityTable.NewRow();
            row.SetField(Data.ActivityDateTime, DateTime.Now);
            row.SetField(Data.ActivityDescription, Enum.GetName(typeof(SessionSwitchReason), sessionSwitchEventArgs.Reason));
            row.SetField(Data.ActivityStatus, Data.ActivityStatusUnapproved);
            this._activityTable.Rows.Add(row);
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
        public const string ActivityStatus = "status";
        public const string ActivityStatusUnapproved = "unapproved";

        public const string LogTableName = "log";
        public const string LogMessage = "message";

        public void Load()
        {
            this.UberSet = new DataSet();
            try
            {
                this.UberSet.ReadXml("settings.xml");
            }
            catch(Exception ex)
            {             
                this.UberSet = this.CreateDataSet();
            }
        }

		public DataSet CreateDataSet()
		{
			var dataSet = new DataSet();
			var activityTable = dataSet.Tables.Add(ActivityTableName);
			activityTable.Columns.Add(ActivityDateTime, typeof(DateTime));
            activityTable.Columns.Add(ActivityDescription, typeof(string));
			activityTable.Columns.Add(ActivityStatus, typeof(string));//approved, posting, posted

			var settingsTable = dataSet.Tables.Add(SettingsTableName);
			settingsTable.Columns.Add(SettingsTableKey, typeof(string));
			settingsTable.Columns.Add(SettingsTableValue, typeof(string));
			settingsTable.Columns.Add(SettingsTableComment, typeof(string));

			settingsTable.Rows.Add(SettingsTfptPathAndFileName, @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\tfpt.exe", "the exe");
			settingsTable.Rows.Add(SettingsTfsCollectionName, "http://tfstta.int.thomson.com:8080/tfs/DefaultCollection", "the collection");
			settingsTable.Rows.Add(SettingsTfsWorkHoursFieldName, "Actual Work", "TFS WI field name to increment");

			return dataSet;
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

