
namespace PWStats
{
    partial class frmMain
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTerm = new System.Windows.Forms.Button();
            this.txtTerm = new System.Windows.Forms.TextBox();
            this.chkTerm = new System.Windows.Forms.CheckBox();
            this.txtDist = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblMMSI = new System.Windows.Forms.Label();
            this.cbxOptions = new System.Windows.Forms.ComboBox();
            this.cbxShip = new System.Windows.Forms.ComboBox();
            this.cbxDir = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkLabel = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nudBins = new System.Windows.Forms.NumericUpDown();
            this.cbxStat = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.txtGap = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnTrack = new System.Windows.Forms.Button();
            this.btnShip = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnTransect = new System.Windows.Forms.Button();
            this.btnPlot = new System.Windows.Forms.Button();
            this.txtTrackFile = new System.Windows.Forms.TextBox();
            this.txtShipFile = new System.Windows.Forms.TextBox();
            this.txtTransect = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBins)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMessage,
            this.lblInfo,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 667);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(686, 26);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblMessage
            // 
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(225, 21);
            this.lblMessage.Spring = true;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = false;
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(400, 21);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.AutoSize = false;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(50, 21);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnHelp);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnTrack);
            this.panel1.Controls.Add(this.btnShip);
            this.panel1.Controls.Add(this.btnProcess);
            this.panel1.Controls.Add(this.btnTransect);
            this.panel1.Controls.Add(this.btnPlot);
            this.panel1.Controls.Add(this.txtTrackFile);
            this.panel1.Controls.Add(this.txtShipFile);
            this.panel1.Controls.Add(this.txtTransect);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(686, 305);
            this.panel1.TabIndex = 1;
            // 
            // btnHelp
            // 
            this.btnHelp.Enabled = false;
            this.btnHelp.Location = new System.Drawing.Point(550, 271);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(2);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(56, 22);
            this.btnHelp.TabIndex = 22;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTerm);
            this.groupBox1.Controls.Add(this.txtTerm);
            this.groupBox1.Controls.Add(this.chkTerm);
            this.groupBox1.Controls.Add(this.txtDist);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtTime);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtSpeed);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Location = new System.Drawing.Point(8, 137);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(536, 67);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Docking Options";
            // 
            // btnTerm
            // 
            this.btnTerm.Enabled = false;
            this.btnTerm.Location = new System.Drawing.Point(500, 39);
            this.btnTerm.Margin = new System.Windows.Forms.Padding(2);
            this.btnTerm.Name = "btnTerm";
            this.btnTerm.Size = new System.Drawing.Size(27, 22);
            this.btnTerm.TabIndex = 21;
            this.btnTerm.Text = "...";
            this.btnTerm.UseVisualStyleBackColor = true;
            this.btnTerm.Click += new System.EventHandler(this.btnTerm_Click);
            // 
            // txtTerm
            // 
            this.txtTerm.Location = new System.Drawing.Point(152, 41);
            this.txtTerm.Margin = new System.Windows.Forms.Padding(2);
            this.txtTerm.Name = "txtTerm";
            this.txtTerm.ReadOnly = true;
            this.txtTerm.Size = new System.Drawing.Size(344, 20);
            this.txtTerm.TabIndex = 20;
            // 
            // chkTerm
            // 
            this.chkTerm.AutoSize = true;
            this.chkTerm.Location = new System.Drawing.Point(7, 42);
            this.chkTerm.Margin = new System.Windows.Forms.Padding(2);
            this.chkTerm.Name = "chkTerm";
            this.chkTerm.Size = new System.Drawing.Size(146, 17);
            this.chkTerm.TabIndex = 19;
            this.chkTerm.Text = "User Specified Terminals:";
            this.chkTerm.UseVisualStyleBackColor = true;
            this.chkTerm.CheckedChanged += new System.EventHandler(this.chkTerm_CheckedChanged);
            // 
            // txtDist
            // 
            this.txtDist.Location = new System.Drawing.Point(475, 14);
            this.txtDist.Margin = new System.Windows.Forms.Padding(2);
            this.txtDist.Name = "txtDist";
            this.txtDist.Size = new System.Drawing.Size(52, 20);
            this.txtDist.TabIndex = 12;
            this.txtDist.Text = "100";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(349, 15);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(121, 13);
            this.label13.TabIndex = 10;
            this.label13.Text = "Grouping Distance (m) ≤";
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(274, 14);
            this.txtTime.Margin = new System.Windows.Forms.Padding(2);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(52, 20);
            this.txtTime.TabIndex = 11;
            this.txtTime.Text = "0.0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(195, 16);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Time (hours) ≥";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(123, 13);
            this.txtSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(52, 20);
            this.txtSpeed.TabIndex = 10;
            this.txtSpeed.Text = "0.01";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(46, 16);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Speed (m/s) ≤";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblMMSI);
            this.groupBox3.Controls.Add(this.cbxOptions);
            this.groupBox3.Controls.Add(this.cbxShip);
            this.groupBox3.Controls.Add(this.cbxDir);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.chkLabel);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.nudBins);
            this.groupBox3.Controls.Add(this.cbxStat);
            this.groupBox3.Location = new System.Drawing.Point(8, 209);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(536, 91);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output Options";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // lblMMSI
            // 
            this.lblMMSI.AutoSize = true;
            this.lblMMSI.Location = new System.Drawing.Point(256, 65);
            this.lblMMSI.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMMSI.Name = "lblMMSI";
            this.lblMMSI.Size = new System.Drawing.Size(35, 13);
            this.lblMMSI.TabIndex = 20;
            this.lblMMSI.Text = "MMSI";
            // 
            // cbxOptions
            // 
            this.cbxOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOptions.FormattingEnabled = true;
            this.cbxOptions.Items.AddRange(new object[] {
            "MMSI",
            "Ship Name"});
            this.cbxOptions.Location = new System.Drawing.Point(15, 64);
            this.cbxOptions.Margin = new System.Windows.Forms.Padding(2);
            this.cbxOptions.Name = "cbxOptions";
            this.cbxOptions.Size = new System.Drawing.Size(76, 21);
            this.cbxOptions.TabIndex = 19;
            this.cbxOptions.SelectedIndexChanged += new System.EventHandler(this.cbxOptions_SelectedIndexChanged);
            // 
            // cbxShip
            // 
            this.cbxShip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxShip.FormattingEnabled = true;
            this.cbxShip.Items.AddRange(new object[] {
            "<All Ships>",
            "<All Ships>"});
            this.cbxShip.Location = new System.Drawing.Point(101, 64);
            this.cbxShip.Margin = new System.Windows.Forms.Padding(2);
            this.cbxShip.Name = "cbxShip";
            this.cbxShip.Size = new System.Drawing.Size(151, 21);
            this.cbxShip.TabIndex = 17;
            this.cbxShip.SelectedIndexChanged += new System.EventHandler(this.cbxShip_SelectedIndexChanged);
            // 
            // cbxDir
            // 
            this.cbxDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDir.FormattingEnabled = true;
            this.cbxDir.Items.AddRange(new object[] {
            "Both",
            "Up only",
            "Down only"});
            this.cbxDir.Location = new System.Drawing.Point(101, 11);
            this.cbxDir.Margin = new System.Windows.Forms.Padding(2);
            this.cbxDir.Name = "cbxDir";
            this.cbxDir.Size = new System.Drawing.Size(151, 21);
            this.cbxDir.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Direction:";
            // 
            // chkLabel
            // 
            this.chkLabel.AutoSize = true;
            this.chkLabel.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkLabel.Checked = true;
            this.chkLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLabel.Location = new System.Drawing.Point(420, 13);
            this.chkLabel.Margin = new System.Windows.Forms.Padding(2);
            this.chkLabel.Name = "chkLabel";
            this.chkLabel.Size = new System.Drawing.Size(112, 17);
            this.chkLabel.TabIndex = 18;
            this.chkLabel.Text = "Show Value Label";
            this.chkLabel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 40);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Statistics:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(437, 40);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "# Bins:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(256, 40);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Data Range";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // nudBins
            // 
            this.nudBins.Enabled = false;
            this.nudBins.Location = new System.Drawing.Point(481, 38);
            this.nudBins.Margin = new System.Windows.Forms.Padding(2);
            this.nudBins.Name = "nudBins";
            this.nudBins.Size = new System.Drawing.Size(51, 20);
            this.nudBins.TabIndex = 16;
            this.nudBins.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // cbxStat
            // 
            this.cbxStat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStat.FormattingEnabled = true;
            this.cbxStat.Items.AddRange(new object[] {
            "Ship Count",
            "By MMSI",
            "By Name",
            "By Type",
            "By Length",
            "By Breadth",
            "By Hull Depth",
            "By Draft Max",
            "By AIS To Stern",
            "By Horse Power",
            "By Num. Propellers",
            "By Propeller Blades",
            "By Propeller Diameter",
            "By Ship Speed",
            "By Docking Duration"});
            this.cbxStat.Location = new System.Drawing.Point(101, 37);
            this.cbxStat.Margin = new System.Windows.Forms.Padding(2);
            this.cbxStat.Name = "cbxStat";
            this.cbxStat.Size = new System.Drawing.Size(151, 21);
            this.cbxStat.TabIndex = 15;
            this.cbxStat.SelectedIndexChanged += new System.EventHandler(this.cbxStat_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Controls.Add(this.dateTimePicker2);
            this.groupBox2.Controls.Add(this.txtGap);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(8, 89);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(536, 43);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Track Processing Options";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(80, 17);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(95, 20);
            this.dateTimePicker1.TabIndex = 7;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(248, 17);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(95, 20);
            this.dateTimePicker2.TabIndex = 8;
            // 
            // txtGap
            // 
            this.txtGap.Location = new System.Drawing.Point(475, 15);
            this.txtGap.Margin = new System.Windows.Forms.Padding(2);
            this.txtGap.Name = "txtGap";
            this.txtGap.Size = new System.Drawing.Size(52, 20);
            this.txtGap.TabIndex = 9;
            this.txtGap.Text = "1.0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(369, 17);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Track Gaps (hours):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "End Date:";
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(618, 218);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(56, 22);
            this.btnExport.TabIndex = 21;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(618, 35);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(56, 22);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(550, 244);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(56, 22);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(618, 271);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(56, 22);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnTrack
            // 
            this.btnTrack.Enabled = false;
            this.btnTrack.Location = new System.Drawing.Point(550, 35);
            this.btnTrack.Margin = new System.Windows.Forms.Padding(2);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(56, 22);
            this.btnTrack.TabIndex = 4;
            this.btnTrack.Text = "Browse";
            this.btnTrack.UseVisualStyleBackColor = true;
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);
            // 
            // btnShip
            // 
            this.btnShip.Location = new System.Drawing.Point(550, 8);
            this.btnShip.Margin = new System.Windows.Forms.Padding(2);
            this.btnShip.Name = "btnShip";
            this.btnShip.Size = new System.Drawing.Size(56, 22);
            this.btnShip.TabIndex = 1;
            this.btnShip.Text = "Browse";
            this.btnShip.UseVisualStyleBackColor = true;
            this.btnShip.Click += new System.EventHandler(this.btnShip_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Enabled = false;
            this.btnProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.Location = new System.Drawing.Point(550, 153);
            this.btnProcess.Margin = new System.Windows.Forms.Padding(2);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(126, 22);
            this.btnProcess.TabIndex = 13;
            this.btnProcess.Text = "Process Tracks";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnTransect
            // 
            this.btnTransect.Enabled = false;
            this.btnTransect.Location = new System.Drawing.Point(550, 62);
            this.btnTransect.Margin = new System.Windows.Forms.Padding(2);
            this.btnTransect.Name = "btnTransect";
            this.btnTransect.Size = new System.Drawing.Size(56, 22);
            this.btnTransect.TabIndex = 6;
            this.btnTransect.Text = "Browse";
            this.btnTransect.UseVisualStyleBackColor = true;
            this.btnTransect.Click += new System.EventHandler(this.btnTransect_Click);
            // 
            // btnPlot
            // 
            this.btnPlot.Enabled = false;
            this.btnPlot.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlot.Location = new System.Drawing.Point(550, 218);
            this.btnPlot.Margin = new System.Windows.Forms.Padding(2);
            this.btnPlot.Name = "btnPlot";
            this.btnPlot.Size = new System.Drawing.Size(56, 22);
            this.btnPlot.TabIndex = 19;
            this.btnPlot.Text = "Plot";
            this.btnPlot.UseVisualStyleBackColor = true;
            this.btnPlot.Click += new System.EventHandler(this.btnPlot_Click);
            // 
            // txtTrackFile
            // 
            this.txtTrackFile.Location = new System.Drawing.Point(89, 37);
            this.txtTrackFile.Margin = new System.Windows.Forms.Padding(2);
            this.txtTrackFile.Name = "txtTrackFile";
            this.txtTrackFile.ReadOnly = true;
            this.txtTrackFile.Size = new System.Drawing.Size(456, 20);
            this.txtTrackFile.TabIndex = 3;
            // 
            // txtShipFile
            // 
            this.txtShipFile.Location = new System.Drawing.Point(89, 10);
            this.txtShipFile.Margin = new System.Windows.Forms.Padding(2);
            this.txtShipFile.Name = "txtShipFile";
            this.txtShipFile.ReadOnly = true;
            this.txtShipFile.Size = new System.Drawing.Size(456, 20);
            this.txtShipFile.TabIndex = 0;
            // 
            // txtTransect
            // 
            this.txtTransect.Location = new System.Drawing.Point(89, 63);
            this.txtTransect.Margin = new System.Windows.Forms.Padding(2);
            this.txtTransect.Name = "txtTransect";
            this.txtTransect.ReadOnly = true;
            this.txtTransect.Size = new System.Drawing.Size(456, 20);
            this.txtTransect.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 39);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Ship Track File:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 12);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Ship Info. File:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 66);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Transect File:";
            // 
            // chart1
            // 
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Angle = -90;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 305);
            this.chart1.Margin = new System.Windows.Forms.Padding(2);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(686, 362);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 693);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.Text = "Ship Track Statistics";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBins)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbxStat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbxDir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        //private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Button btnTransect;
        private System.Windows.Forms.TextBox txtTransect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudBins;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btnTrack;
        private System.Windows.Forms.Button btnShip;
        private System.Windows.Forms.TextBox txtTrackFile;
        private System.Windows.Forms.TextBox txtShipFile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ToolStripStatusLabel lblMessage;
        private System.Windows.Forms.ToolStripStatusLabel lblInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPlot;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkLabel;
        private System.Windows.Forms.TextBox txtGap;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDist;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbxShip;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnTerm;
        private System.Windows.Forms.TextBox txtTerm;
        private System.Windows.Forms.CheckBox chkTerm;
        private System.Windows.Forms.ComboBox cbxOptions;
        private System.Windows.Forms.Label lblMMSI;
    }
}
