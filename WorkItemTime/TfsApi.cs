using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkItemTime
{
	public class TfsApi
	{
		readonly DataTable _tfsEdits;
		readonly DataTable _settings;

		public TfsApi(System.Data.DataSet uberSet)
		{
			this._tfsEdits = uberSet.Tables[Data.TfsEditsTableName];
			this._settings = uberSet.Tables[Data.SettingsTableName];
		}

		public void Send()
		{

		}

		public void IncrementWorkHours(DataRow tfsEditRow)
		{
			var tfptPathAndFileName = this._settings.Rows.Find(Data.SettingsTfptPathAndFileName).Field<string>(Data.SettingsTableValue);
			var tfsCollectionName = this._settings.Rows.Find(Data.SettingsTfsCollectionName).Field<string>(Data.SettingsTableValue);
			var workHoursFieldName = this._settings.Rows.Find(Data.SettingsTfsWorkHoursFieldName).Field<string>(Data.SettingsTableValue);
			var workItemNumber = tfsEditRow.Field<Int32>(Data.TfsEditsWorkItem);

			//read the current hours
			var startInfo = new ProcessStartInfo();
			startInfo.FileName = tfptPathAndFileName;
			startInfo.Arguments = $"workitem /collection{tfsCollectionName} {workItemNumber} /fields:\"{workHoursFieldName}\" = {workHoursFieldName}";
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardInput = false;

			using (var process = Process.Start(startInfo))
			{
				process.Start();
				var output = process.StandardOutput.ReadToEnd();
				var error = process.StandardError.ReadToEnd();
				tfsEditRow.SetField(Data.TfsEditsApiOutput, output);
				tfsEditRow.SetField(Data.TfsEditsApiError, error);
			}


			//write the updated hours
			startInfo = new ProcessStartInfo();
			startInfo.FileName = tfptPathAndFileName;
			startInfo.Arguments = $"workitem /update /collection{tfsCollectionName} {workItemNumber} /fields:\"{workHoursFieldName}\" = {workHoursFieldName}";
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardInput = false;

			using (var process = Process.Start(startInfo))
			{
				process.Start();
				var output = process.StandardOutput.ReadToEnd();
				var error = process.StandardError.ReadToEnd();
				tfsEditRow.SetField(Data.TfsEditsApiOutput, output);
				tfsEditRow.SetField(Data.TfsEditsApiError, error);
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


		//   }
	}
}
