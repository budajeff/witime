using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WorkItemTime
{
    public partial class MainForm : Form
	{
        WorkItemTime.Data _data = new Data();
        WorkItemTime.Monitor _monitor;

        public MainForm()
		{
			InitializeComponent();
		}

        private void MainForm_Load(object sender, EventArgs e)
        {
            _data.Load();

            var settingsBinding = new BindingSource();
            settingsBinding.DataSource = this._data.UberSet.Tables[Data.SettingsTableName];
            this._settingsGrid.DataSource = settingsBinding;

            var activityBinding = new BindingSource();
            activityBinding.DataSource = this._data.UberSet.Tables[Data.ActivityTableName];
            this._activityGrid.DataSource = activityBinding;


	        var dateColumn = this._activityGrid.Columns[Data.ActivityDateTime];
	        dateColumn.DefaultCellStyle.Format = "g";
	        this._activityGrid.Sort(dateColumn, ListSortDirection.Descending);

            _monitor = new Monitor(_data.UberSet);
            _monitor.Log(Application.ProductName + " started", Data.ActivityKindStart);
            this._monitor.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._monitor.Log(Application.ProductName + " closing", Data.ActivityKindStop);
            this._data.Save();
        }

		private void _submitToTfsButton_Click(object sender, EventArgs e)
		{

		}
	}
}
