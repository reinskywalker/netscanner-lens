namespace netscan_lens
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            chkOwnIp = new CheckBox();
            lblStartIp = new Label();
            txtStartIp = new TextBox();
            lblEndIp = new Label();
            txtEndIp = new TextBox();
            lblScanType = new Label();
            cmbScanType = new ComboBox();
            lblTimeout = new Label();
            numTimeout = new NumericUpDown();
            lblPortStart = new Label();
            numPortStart = new NumericUpDown();
            lblPortEnd = new Label();
            numPortEnd = new NumericUpDown();
            btnScan = new Button();
            btnCancel = new Button();
            btnExport = new Button();
            btnCredits = new Button();
            progressBar = new ProgressBar();
            dgvResults = new DataGridView();
            colIp = new DataGridViewTextBoxColumn();
            colHost = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colLatency = new DataGridViewTextBoxColumn();
            colPorts = new DataGridViewTextBoxColumn();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            toolTip = new ToolTip(components);
            topPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)numTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPortStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPortEnd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();

            chkOwnIp.AutoSize = true;
            chkOwnIp.Location = new Point(13, 12);
            chkOwnIp.Name = "chkOwnIp";
            chkOwnIp.Size = new Size(104, 24);
            chkOwnIp.TabIndex = 0;
            chkOwnIp.Text = "Scan own IP";
            chkOwnIp.CheckedChanged += chkOwnIp_CheckedChanged;

            lblStartIp.AutoSize = true;
            lblStartIp.Location = new Point(13, 44);
            lblStartIp.Name = "lblStartIp";
            lblStartIp.Size = new Size(100, 23);
            lblStartIp.TabIndex = 0;
            lblStartIp.Text = "Start IP";

            txtStartIp.Location = new Point(80, 40);
            txtStartIp.Name = "txtStartIp";
            txtStartIp.PlaceholderText = "e.g. 192.168.1.1";
            txtStartIp.Size = new Size(160, 23);
            txtStartIp.TabIndex = 0;

            lblEndIp.AutoSize = true;
            lblEndIp.Location = new Point(250, 44);
            lblEndIp.Name = "lblEndIp";
            lblEndIp.Size = new Size(100, 23);
            lblEndIp.TabIndex = 0;
            lblEndIp.Text = "End IP";

            txtEndIp.Location = new Point(305, 40);
            txtEndIp.Name = "txtEndIp";
            txtEndIp.PlaceholderText = "e.g. 192.168.1.254";
            txtEndIp.Size = new Size(160, 23);
            txtEndIp.TabIndex = 0;

            lblScanType.AutoSize = true;
            lblScanType.Location = new Point(480, 44);
            lblScanType.Name = "lblScanType";
            lblScanType.Size = new Size(100, 23);
            lblScanType.TabIndex = 0;
            lblScanType.Text = "Scan Type";

            cmbScanType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbScanType.Items.AddRange(new object[] { "Ping", "Port Sweep" });
            cmbScanType.Location = new Point(548, 40);
            cmbScanType.Name = "cmbScanType";
            cmbScanType.Size = new Size(140, 23);
            cmbScanType.TabIndex = 0;

            lblTimeout.AutoSize = true;
            lblTimeout.Location = new Point(700, 44);
            lblTimeout.Name = "lblTimeout";
            lblTimeout.Size = new Size(100, 23);
            lblTimeout.TabIndex = 0;
            lblTimeout.Text = "Timeout (ms)";

            numTimeout.Location = new Point(796, 40);
            numTimeout.Maximum = new decimal(new int[] { 60000, 0, 0, 0 });
            numTimeout.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            numTimeout.Name = "numTimeout";
            numTimeout.Size = new Size(100, 23);
            numTimeout.TabIndex = 0;
            numTimeout.Value = new decimal(new int[] { 200, 0, 0, 0 });

            lblPortStart.AutoSize = true;
            lblPortStart.Location = new Point(13, 84);
            lblPortStart.Name = "lblPortStart";
            lblPortStart.Size = new Size(100, 23);
            lblPortStart.TabIndex = 0;
            lblPortStart.Text = "Start Port";

            numPortStart.Location = new Point(80, 80);
            numPortStart.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numPortStart.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPortStart.Name = "numPortStart";
            numPortStart.Size = new Size(100, 23);
            numPortStart.TabIndex = 0;
            numPortStart.Value = new decimal(new int[] { 1, 0, 0, 0 });
   
            lblPortEnd.AutoSize = true;
            lblPortEnd.Location = new Point(190, 84);
            lblPortEnd.Name = "lblPortEnd";
            lblPortEnd.Size = new Size(100, 23);
            lblPortEnd.TabIndex = 0;
            lblPortEnd.Text = "End Port";

            numPortEnd.Location = new Point(250, 80);
            numPortEnd.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numPortEnd.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPortEnd.Name = "numPortEnd";
            numPortEnd.Size = new Size(100, 23);
            numPortEnd.TabIndex = 0;
            toolTip.SetToolTip(numPortEnd, "If only end is set, start defaults to 1");
            numPortEnd.Value = new decimal(new int[] { 1, 0, 0, 0 });
 
            btnScan.Location = new Point(370, 79);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(90, 27);
            btnScan.TabIndex = 0;
            btnScan.Text = "Scan";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
 
            btnCancel.Enabled = false;
            btnCancel.Location = new Point(466, 79);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 27);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;

            btnExport.Location = new Point(562, 79);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(90, 27);
            btnExport.TabIndex = 0;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;

            btnCredits.Location = new Point(658, 79);
            btnCredits.Name = "btnCredits";
            btnCredits.Size = new Size(90, 27);
            btnCredits.TabIndex = 0;
            btnCredits.Text = "Credits";
            btnCredits.UseVisualStyleBackColor = true;
            btnCredits.Click += btnCredits_Click;

            progressBar.Dock = DockStyle.Top;
            progressBar.Location = new Point(0, 150);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(1744, 10);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 1;

            dgvResults.AllowUserToAddRows = false;
            dgvResults.AllowUserToDeleteRows = false;
            dgvResults.AllowUserToResizeRows = false;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.BackgroundColor = SystemColors.Window;
            dgvResults.Columns.AddRange(new DataGridViewColumn[] { colIp, colHost, colStatus, colLatency, colPorts });
            dgvResults.Dock = DockStyle.Fill;
            dgvResults.Location = new Point(0, 160);
            dgvResults.MultiSelect = false;
            dgvResults.Name = "dgvResults";
            dgvResults.ReadOnly = true;
            dgvResults.RowHeadersVisible = false;
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResults.Size = new Size(1744, 605);
            dgvResults.TabIndex = 0;

            colIp.HeaderText = "IP Address";
            colIp.Name = "colIp";
            colIp.ReadOnly = true;

            colHost.HeaderText = "Hostname";
            colHost.Name = "colHost";
            colHost.ReadOnly = true;

            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;

            colLatency.HeaderText = "Latency (ms)";
            colLatency.Name = "colLatency";
            colLatency.ReadOnly = true;

            colPorts.HeaderText = "Open Ports";
            colPorts.Name = "colPorts";
            colPorts.ReadOnly = true;

            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip.Location = new Point(0, 765);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1744, 22);
            statusStrip.SizingGrip = false;
            statusStrip.TabIndex = 3;

            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(83, 17);
            lblStatus.Text = "Ready to scan.";

            topPanel.BackColor = SystemColors.ControlLight;
            topPanel.Controls.Add(chkOwnIp);
            topPanel.Controls.Add(lblStartIp);
            topPanel.Controls.Add(txtStartIp);
            topPanel.Controls.Add(lblEndIp);
            topPanel.Controls.Add(txtEndIp);
            topPanel.Controls.Add(lblScanType);
            topPanel.Controls.Add(cmbScanType);
            topPanel.Controls.Add(lblTimeout);
            topPanel.Controls.Add(numTimeout);
            topPanel.Controls.Add(lblPortStart);
            topPanel.Controls.Add(numPortStart);
            topPanel.Controls.Add(lblPortEnd);
            topPanel.Controls.Add(numPortEnd);
            topPanel.Controls.Add(btnScan);
            topPanel.Controls.Add(btnCancel);
            topPanel.Controls.Add(btnExport);
            topPanel.Controls.Add(btnCredits);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Padding = new Padding(10);
            topPanel.Size = new Size(1744, 150);
            topPanel.TabIndex = 2;

            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1744, 787);
            Controls.Add(dgvResults);
            Controls.Add(progressBar);
            Controls.Add(topPanel);
            Controls.Add(statusStrip);
            MaximizeBox = false;
            MinimumSize = new Size(900, 500);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Network Scanner Lens";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)numTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPortStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPortEnd).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblStartIp;
        private TextBox txtStartIp;
        private Label lblEndIp;
        private TextBox txtEndIp;
        private Label lblScanType;
        private ComboBox cmbScanType;
        private Label lblTimeout;
        private NumericUpDown numTimeout;
        private Label lblPortStart;
        private NumericUpDown numPortStart;
        private Label lblPortEnd;
        private NumericUpDown numPortEnd;
        private Button btnScan;
        private Button btnCancel;
        private Button btnExport;
        private Button btnCredits;
        private ProgressBar progressBar;
        private DataGridView dgvResults;
        private DataGridViewTextBoxColumn colIp;
        private DataGridViewTextBoxColumn colHost;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colLatency;
        private DataGridViewTextBoxColumn colPorts;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolTip toolTip;
        private CheckBox chkOwnIp;
        private Panel topPanel;
    }
}
