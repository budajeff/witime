namespace WorkItemTime
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this._tabControl = new System.Windows.Forms.TabControl();
            this._activityTab = new System.Windows.Forms.TabPage();
            this._settingsTab = new System.Windows.Forms.TabPage();
            this._settingsGrid = new System.Windows.Forms.DataGridView();
            this._activityGrid = new System.Windows.Forms.DataGridView();
            this._tabControl.SuspendLayout();
            this._activityTab.SuspendLayout();
            this._settingsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._settingsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._activityGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._activityTab);
            this._tabControl.Controls.Add(this._settingsTab);
            this._tabControl.Location = new System.Drawing.Point(44, 29);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(762, 411);
            this._tabControl.TabIndex = 0;
            // 
            // _activityTab
            // 
            this._activityTab.Controls.Add(this._activityGrid);
            this._activityTab.Location = new System.Drawing.Point(4, 22);
            this._activityTab.Name = "_activityTab";
            this._activityTab.Padding = new System.Windows.Forms.Padding(3);
            this._activityTab.Size = new System.Drawing.Size(754, 385);
            this._activityTab.TabIndex = 0;
            this._activityTab.Text = "Activity";
            this._activityTab.UseVisualStyleBackColor = true;
            // 
            // _settingsTab
            // 
            this._settingsTab.Controls.Add(this._settingsGrid);
            this._settingsTab.Location = new System.Drawing.Point(4, 22);
            this._settingsTab.Name = "_settingsTab";
            this._settingsTab.Padding = new System.Windows.Forms.Padding(3);
            this._settingsTab.Size = new System.Drawing.Size(754, 385);
            this._settingsTab.TabIndex = 1;
            this._settingsTab.Text = "Settings";
            this._settingsTab.UseVisualStyleBackColor = true;
            // 
            // _settingsGrid
            // 
            this._settingsGrid.AllowUserToAddRows = false;
            this._settingsGrid.AllowUserToDeleteRows = false;
            this._settingsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._settingsGrid.Location = new System.Drawing.Point(57, 25);
            this._settingsGrid.Name = "_settingsGrid";
            this._settingsGrid.Size = new System.Drawing.Size(533, 324);
            this._settingsGrid.TabIndex = 0;
            // 
            // _activityGrid
            // 
            this._activityGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._activityGrid.Location = new System.Drawing.Point(49, 32);
            this._activityGrid.Name = "_activityGrid";
            this._activityGrid.Size = new System.Drawing.Size(520, 277);
            this._activityGrid.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 475);
            this.Controls.Add(this._tabControl);
            this.Name = "MainForm";
            this.Text = "TFS Work Item Time Tracker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this._tabControl.ResumeLayout(false);
            this._activityTab.ResumeLayout(false);
            this._settingsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._settingsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._activityGrid)).EndInit();
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _activityTab;
        private System.Windows.Forms.TabPage _settingsTab;
        private System.Windows.Forms.DataGridView _settingsGrid;
        private System.Windows.Forms.DataGridView _activityGrid;
    }
}

