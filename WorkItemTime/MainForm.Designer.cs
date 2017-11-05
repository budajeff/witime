namespace WorkItemTime
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._tabControl = new System.Windows.Forms.TabControl();
			this._activityTab = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._activityGrid = new System.Windows.Forms.DataGridView();
			this._clearActivityLogs = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this._tfsEditsGrid = new System.Windows.Forms.DataGridView();
			this._submitToTfsButton = new System.Windows.Forms.Button();
			this._settingsTab = new System.Windows.Forms.TabPage();
			this._settingsGrid = new System.Windows.Forms.DataGridView();
			this._toggleButton = new System.Windows.Forms.Button();
			this._tabControl.SuspendLayout();
			this._activityTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._activityGrid)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._tfsEditsGrid)).BeginInit();
			this._settingsTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._settingsGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// _tabControl
			// 
			this._tabControl.Controls.Add(this._activityTab);
			this._tabControl.Controls.Add(this._settingsTab);
			this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tabControl.Location = new System.Drawing.Point(0, 0);
			this._tabControl.Name = "_tabControl";
			this._tabControl.SelectedIndex = 0;
			this._tabControl.Size = new System.Drawing.Size(850, 475);
			this._tabControl.TabIndex = 0;
			// 
			// _activityTab
			// 
			this._activityTab.Controls.Add(this.splitContainer1);
			this._activityTab.Location = new System.Drawing.Point(4, 22);
			this._activityTab.Name = "_activityTab";
			this._activityTab.Padding = new System.Windows.Forms.Padding(3);
			this._activityTab.Size = new System.Drawing.Size(842, 449);
			this._activityTab.TabIndex = 0;
			this._activityTab.Text = "Activity";
			this._activityTab.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
			this.splitContainer1.Size = new System.Drawing.Size(836, 443);
			this.splitContainer1.SplitterDistance = 221;
			this.splitContainer1.TabIndex = 1;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._activityGrid, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._clearActivityLogs, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this._toggleButton, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(836, 221);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _activityGrid
			// 
			this._activityGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.tableLayoutPanel1.SetColumnSpan(this._activityGrid, 2);
			this._activityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._activityGrid.Location = new System.Drawing.Point(3, 3);
			this._activityGrid.Name = "_activityGrid";
			this._activityGrid.Size = new System.Drawing.Size(830, 186);
			this._activityGrid.TabIndex = 2;
			// 
			// _clearActivityLogs
			// 
			this._clearActivityLogs.AutoSize = true;
			this._clearActivityLogs.Location = new System.Drawing.Point(729, 195);
			this._clearActivityLogs.Name = "_clearActivityLogs";
			this._clearActivityLogs.Size = new System.Drawing.Size(104, 23);
			this._clearActivityLogs.TabIndex = 1;
			this._clearActivityLogs.Text = "Clear Activity Logs";
			this._clearActivityLogs.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this._tfsEditsGrid, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this._submitToTfsButton, 1, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(836, 218);
			this.tableLayoutPanel2.TabIndex = 1;
			// 
			// _tfsEditsGrid
			// 
			this._tfsEditsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.tableLayoutPanel2.SetColumnSpan(this._tfsEditsGrid, 2);
			this._tfsEditsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tfsEditsGrid.Location = new System.Drawing.Point(3, 3);
			this._tfsEditsGrid.Name = "_tfsEditsGrid";
			this._tfsEditsGrid.Size = new System.Drawing.Size(830, 183);
			this._tfsEditsGrid.TabIndex = 0;
			// 
			// _submitToTfsButton
			// 
			this._submitToTfsButton.AutoSize = true;
			this._submitToTfsButton.Location = new System.Drawing.Point(421, 192);
			this._submitToTfsButton.Name = "_submitToTfsButton";
			this._submitToTfsButton.Size = new System.Drawing.Size(84, 23);
			this._submitToTfsButton.TabIndex = 1;
			this._submitToTfsButton.Text = "Submit to TFS";
			this._submitToTfsButton.UseVisualStyleBackColor = true;
			this._submitToTfsButton.Click += new System.EventHandler(this._submitToTfsButton_Click);
			// 
			// _settingsTab
			// 
			this._settingsTab.Controls.Add(this._settingsGrid);
			this._settingsTab.Location = new System.Drawing.Point(4, 22);
			this._settingsTab.Name = "_settingsTab";
			this._settingsTab.Padding = new System.Windows.Forms.Padding(3);
			this._settingsTab.Size = new System.Drawing.Size(842, 449);
			this._settingsTab.TabIndex = 1;
			this._settingsTab.Text = "Settings";
			this._settingsTab.UseVisualStyleBackColor = true;
			// 
			// _settingsGrid
			// 
			this._settingsGrid.AllowUserToAddRows = false;
			this._settingsGrid.AllowUserToDeleteRows = false;
			this._settingsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._settingsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._settingsGrid.Location = new System.Drawing.Point(3, 3);
			this._settingsGrid.Name = "_settingsGrid";
			this._settingsGrid.Size = new System.Drawing.Size(836, 443);
			this._settingsGrid.TabIndex = 0;
			// 
			// _toggleButton
			// 
			this._toggleButton.Location = new System.Drawing.Point(3, 195);
			this._toggleButton.Name = "_toggleButton";
			this._toggleButton.Size = new System.Drawing.Size(106, 23);
			this._toggleButton.TabIndex = 3;
			this._toggleButton.Text = "Start/Stop Current";
			this._toggleButton.UseVisualStyleBackColor = true;
			this._toggleButton.Click += new System.EventHandler(this._toggleButton_Click);
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
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._activityGrid)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._tfsEditsGrid)).EndInit();
			this._settingsTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._settingsGrid)).EndInit();
			this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.TabPage _activityTab;
        private System.Windows.Forms.TabPage _settingsTab;
        private System.Windows.Forms.DataGridView _settingsGrid;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button _clearActivityLogs;
		private System.Windows.Forms.Button _submitToTfsButton;
		private System.Windows.Forms.DataGridView _tfsEditsGrid;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.DataGridView _activityGrid;
		private System.Windows.Forms.Button _toggleButton;
	}
}

