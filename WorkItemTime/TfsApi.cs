using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
			this._tfsEdits.ForEachRow(row => row.SetField(Data.TfsEditsStatus, Data.TfsEditsStatusPending));
			this._tfsEdits.ForEachRow(this.WriteToTfs);
		}

		public void WriteToTfs(DataRow tfsEditRow)
		{
			var tfptPathAndFileName = this._settings.Rows.Find(Data.SettingsTfptPathAndFileName).Field<string>(Data.SettingsTableValue);
			var tfsCollectionName = this._settings.Rows.Find(Data.SettingsTfsCollectionName).Field<string>(Data.SettingsTableValue);
			var workHoursFieldName = this._settings.Rows.Find(Data.SettingsTfsWorkHoursFieldName).Field<string>(Data.SettingsTableValue);
			var workItemNumber = tfsEditRow.Field<Int32>(Data.TfsEditsWorkItem);
			var durationMinutes = tfsEditRow.Field<Int32>(Data.TfsEditsDurationMinutes);

			//read the current hours
			var startInfo = new ProcessStartInfo();
			startInfo.FileName = tfptPathAndFileName;
			startInfo.Arguments = $"workitem /collection:{tfsCollectionName} {workItemNumber}";
			startInfo.UseShellExecute = false;
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardInput = false;
			startInfo.CreateNoWindow = true;

			var standardOutputBuilder = new StringBuilder();
			var standardErrorBuilder = new StringBuilder();

			var tfsGetResult = "";
			var error = "";
			var returnText = "";
			try
			{
				using (var process = Process.Start(startInfo))
				{
					tfsGetResult = process.StandardOutput.ReadToEnd();
					error = process.StandardError.ReadToEnd();
					standardErrorBuilder.AppendLine(error);
					process.WaitForExit();
				}
				if (!string.IsNullOrWhiteSpace(error))
				{
					tfsEditRow.SetField(Data.TfsEditsStatus, Data.TfsEditsStatusError);
					return;
				}

				//read current WI hours value
				tfsEditRow.SetField(Data.TfsEditsStatus, Data.TfsEditsStatusReading);

				var fields = tfsGetResult.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
				var workHoursField =
					fields.Where(f => f.Trim().StartsWith(workHoursFieldName, StringComparison.InvariantCultureIgnoreCase))
						.FirstOrDefault() ?? "0";
				var workHours = workHoursField.Replace(workHoursFieldName + " = ", "").Trim();
				var currentHours = Double.Parse(workHours);

				//update WI hours w/ the amount from the current row
				currentHours += new TimeSpan(0, 0, durationMinutes, 0).TotalHours;

				//create a WI history entry for this edit based on the comment
				var history = tfsEditRow.Field<string>(Data.TfsEditsComment) ?? "";
				history += Environment.NewLine + Environment.NewLine + "Updated via witime";

				//write the updated hours
				tfsEditRow.SetField(Data.TfsEditsStatus, Data.TfsEditsStatusWriting);

				startInfo = new ProcessStartInfo();
				startInfo.UseShellExecute = false;
				startInfo.FileName = tfptPathAndFileName;
				startInfo.Arguments =
					$"workitem /collection:{tfsCollectionName} /update {workItemNumber} /fields:\"{workHoursFieldName}={currentHours:F2};History={history}\"";
				startInfo.RedirectStandardError = true;
				startInfo.RedirectStandardOutput = true;
				startInfo.RedirectStandardInput = false;
				startInfo.CreateNoWindow = true;

				using (var process = Process.Start(startInfo))
				{
					var output = process.StandardOutput.ReadToEnd();
					error = process.StandardError.ReadToEnd();
					tfsEditRow.SetField(Data.TfsEditsApiOutput, output);
					tfsEditRow.SetField(Data.TfsEditsApiError, error);
					process.WaitForExit();
				}
			}
			catch (Exception ex)
			{
				standardErrorBuilder.AppendLine(ex.ToString());
				tfsEditRow.SetField(Data.TfsEditsStatus, Data.TfsEditsStatusError);
			}
			finally
			{
				var standardOutput = standardOutputBuilder.ToString();
				tfsEditRow.SetField(Data.TfsEditsApiOutput, standardOutput);
				var standardError = standardErrorBuilder.ToString();
				tfsEditRow.SetField(Data.TfsEditsApiError, standardError);
				returnText = string.IsNullOrWhiteSpace(standardError) ? standardOutput : standardError;

				tfsEditRow.SetField(Data.TfsEditsStatus, !string.IsNullOrWhiteSpace(standardError) ? Data.TfsEditsStatusError :  Data.TfsEditsStatusSuccess);

			}
		}
	}
}
