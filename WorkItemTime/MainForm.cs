using System;
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

            _monitor = new Monitor(_data.UberSet);
            this._monitor.Start();
        }
    }
}
