using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WorkItemTime
{
	public static class DataTableUtilities
	{
		public static void ForEachRow(this DataTable table, Action<DataRow> rowAction)
		{
			foreach (var row in table.AsEnumerable())
			{
				rowAction(row);
			}
		}

		public static void ForEach<TItem>(this IEnumerable<TItem> enumerable, Action<TItem> action)
		{
			foreach (var item in enumerable)
				action(item);
		}
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
		public const string ActivityStatusDateTime = "statusDateTime";
		public const string ActivityKind = "kind";
		public const string ActivityWorkItem = "workitem";
		public const string ActivityStatusNotSentToTfs = "Not Set To TFS";
		public const string ActivityStatusSendingToTfs = "Sending to TFS";
		public const string ActivityStatusSentToTfs = "Sent To TFS";

		public const string ActivityKindStart = "Start";
		public const string ActivityKindStop = "Stop";

		public const string LogTableName = "log";
		public const string LogMessage = "message";

		public const string TfsEditsTableName = "tfsEdits";
		public const string TfsEditsWorkItem = ActivityWorkItem;
		public const string TfsEditsDurationMinutes = "duration";
		public const string TfsEditsComment = ActivityComment;
		public const string TfsEditsApiOutput = "apiOutput";
		public const string TfsEditsApiError = "apiError";
		public const string TfsEditsStatus = "status";
		public const string TfsEditsStatusNone = "none";
		public const string TfsEditsStatusPending = "pending";
		public const string TfsEditsStatusReading = "reading";
		public const string TfsEditsStatusWriting = "writing";
		public const string TfsEditsStatusSuccess = "success";
		public const string TfsEditsStatusError = "error";




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
			activityTable.Columns.Add(ActivityStatusDateTime, typeof(DateTime));
			var wi = activityTable.Columns.Add(ActivityWorkItem, typeof(Int32));
			wi.AllowDBNull = true;
            
			var settingsTable = dataSet.Tables.Add(SettingsTableName);
			var keyColumn = settingsTable.Columns.Add(SettingsTableKey, typeof(string));
			settingsTable.Columns.Add(SettingsTableValue, typeof(string)); 
			settingsTable.Columns.Add(SettingsTableComment, typeof(string));
			settingsTable.PrimaryKey = new[] {keyColumn};

			settingsTable.Rows.Add(SettingsTfptPathAndFileName, @"C:\Program Files (x86)\Microsoft Team Foundation Server 2013 Power Tools\tfpt.exe", "the exe");
			settingsTable.Rows.Add(SettingsTfsCollectionName, "http://tfstta.int.thomson.com:8080/tfs/DefaultCollection", "the collection");
			settingsTable.Rows.Add(SettingsTfsWorkHoursFieldName, "Actual Work", "TFS WI field name to increment");

			var tfsEditsTable = dataSet.Tables.Add(TfsEditsTableName);
			wi = tfsEditsTable.Columns.Add(TfsEditsWorkItem, typeof(Int32));
			wi.AllowDBNull = true;
			//tfsEditsTable.PrimaryKey = new[] { tfsEditsTable.Columns.Add(TfsEditsWorkItem)};
			tfsEditsTable.Columns.Add(TfsEditsDurationMinutes, typeof(Int32));
			tfsEditsTable.Columns.Add(TfsEditsComment);
			tfsEditsTable.Columns.Add(TfsEditsStatus);
			tfsEditsTable.Columns.Add(TfsEditsApiOutput);
			tfsEditsTable.Columns.Add(TfsEditsApiError);

			return dataSet;
		}

		public static void SetActivityStatus(DataRow activityRow, string status)
		{
			activityRow.SetField(Data.ActivityStatus, status);
			activityRow.SetField(Data.ActivityStatusDateTime, DateTime.Now);
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
					tfsEdit.SetField(Data.TfsEditsDurationMinutes, 0);

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
							tfsEdit.SetField(Data.TfsEditsDurationMinutes, 0);
						}
					}
				}
				else if (currentActivity.Field<string>(Data.ActivityKind) == ActivityKindStop
				         && previousActivity.Field<string>(Data.ActivityKind) == ActivityKindStart)
				{
					//accumulate the time
					var duration = currentActivity.Field<DateTime>(Data.ActivityDateTime)
					               - previousActivity.Field<DateTime>(Data.ActivityDateTime);

					tfsEdit.SetField(Data.TfsEditsDurationMinutes, duration.Minutes);
					tfsEdit.SetField(Data.TfsEditsWorkItem, previousActivity.Field<Int32?>(Data.ActivityWorkItem));

					//append comment
					var activityComment = currentActivity.Field<string>(Data.ActivityComment);
					if (!string.IsNullOrWhiteSpace(activityComment))
					{
						var tfsEditComment = tfsEdit.Field<string>(Data.TfsEditsComment) ?? "";
						tfsEditComment += Environment.NewLine + activityComment;
						tfsEdit.SetField(Data.TfsEditsComment, tfsEditComment);
					}

					if (tfsEditsTable.Rows.IndexOf(tfsEdit) == -1)
					{
						tfsEditsTable.Rows.Add(tfsEdit);
					}
				}
				previousActivity = currentActivity;
			}
		}
	}
}