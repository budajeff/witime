using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WorkItemTime
{
	public class Monitor
	{
		public void Start()
		{
			SystemEvents.SessionSwitch += SystemEventsOnSessionSwitch;
		}

		private void SystemEventsOnSessionSwitch(object sender, SessionSwitchEventArgs sessionSwitchEventArgs)
		{
			throw new NotImplementedException();
		}


	}

	public class Data
	{
		public const string SettingsTfptPathAndFileName = "TFPT Path and File Name";
		public const string SettingsTfsCollectionName = "TFS Collection Name";
		public const string SettingsTfsWorkHoursFieldName = "TFS Work Hours Field Name";

		public DataSet CreateDataSet()
		{
			var dataSet = new DataSet();
			var activityTable = dataSet.Tables.Add("activity");
			activityTable.Columns.Add("start", typeof(DateTime));
			activityTable.Columns.Add("status", typeof(string));//approved, posting, posted

			var settingsTable = dataSet.Tables.Add("settings");
			settingsTable.Columns.Add("key", typeof(string));
			settingsTable.Columns.Add("value", typeof(string));
			settingsTable.Columns.Add("comment", typeof(string));

			settingsTable.Rows.Add(SettingsTfptPathAndFileName, @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\tfpt.exe", "the exe");
			settingsTable.Rows.Add(SettingsTfsCollectionName, "http://tfstta.int.thomson.com:8080/tfs/DefaultCollection", "the collection");
			settingsTable.Rows.Add(SettingsTfsWorkHoursFieldName, "Actual Work", "TFS WI field name to increment");

			return dataSet;
		}

		public void LoadOrCreate()
		{
			
		}
	}

	/// <summary>
	/// Writes TFS WI data via tfpt.exe
	/// </summary>
	/// <remarks>
	/// 
	/// C:\Program Files (x86)\Microsoft Visual Studio 12.0>tfpt workitem /collection:http://tfstta.int.thomson.com:8080/tfs/DefaultCollection 1245593
	///
	///-------------------------------------------------------------------------------
	///Work Item: 1245593
	///Onvio Country = All
	///Sprint Team = Time & Billing
	///Actual Work = 33
	///Estimated Work = 40
	///Backlog Priority =
	///Activity =
	///Blocked =
	///Remaining Work =
	///Integration Build =
	///Closed Date = 9/18/2017 2:35:16 PM
	///Board Lane =
	///Board Column Done =
	///Board Column =
	///Tags =
	///Related Link Count = 1
	///History =
	///Description = Enchantments to the Activity Log Grid(perviously user stories 1
	///-7 but now made into one for planning purposes)
	///Created By = Day, Derek E. (TR Technology & Ops)
	///	Created Date = 9 / 11 / 2017 1:49:06 PM
	///Work Item Type = Task
	///Assigned To = Buda, Jeff (TR Technology & Ops)
	///	Reason = Work finished
	///Changed By = Buda, Jeff (TR Technology & Ops)
	///Rev = 9
	///Watermark = 5360536
	///Authorized Date = 9 / 18 / 2017 2:35:16 PM
	///State = Done
	///Title = Activity Log Grid Enhancements Front End
	///Authorized As = Buda, Jeff (TR Technology & Ops)
	///Area Id = 32445
	///ID = 1245593
	///Changed Date = 9 / 18 / 2017 2:35:16 PM
	///Revised Date = 1/1/9999 12:00:00 AM
	///Area Path = BlueMoonCore
	///Node Name = BlueMoonCore
	///Attached File Count = 0
	///Hyperlink Count = 0
	///Team Project = BlueMoonCore
	///External Link Count = 0
	///Iteration ID = 48900
	///Iteration Path = BlueMoonCore\Current\Sprint 17.10.05r
	///Links
	///
	///Related Workitem    Work Item: 1245592

	///	C:\Program Files(x86)\Microsoft Visual Studio 12.0>tfpt workitem /update /collection:http://tfstta.int.thomson.com:8080/tfs/DefaultCollection 1245593 /fields:"Actual Work = 34"
	///Work item 1245593 updated.
	/// </remarks>
	public class TfsPoster
	{
		public TfsPoster(
			string tfptPathAndFileName,
			string tfsCollectionName)
		{
		}

		public void ReadHours()
		{
			
		}

		public void IncrementHours(
			Int32 workItemNumber, 
			string workHoursFieldName,
			Decimal incrementAmount)
		{


			var startInfo = new ProcessStartInfo();
			startInfo.FileName = tfptPathAndFileName;
			startInfo.Arguments = $"workitem /update /collection{tfsCollectionName} {workItemNumber} /fields:\"{workHoursFieldName}\" = {}";
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardInput = false;

			using (var process = Process.Start(startInfo))
			{
				process.Start();
				var output = process.StandardOutput.ReadToEnd();
				var error = process.StandardError.ReadToEnd();
			}
			
		}
	}
}

