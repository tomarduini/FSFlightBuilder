namespace FSFlightBuilder
{
    partial class BGLImporter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BGLImporter));
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            pbImporter = new AWDControls.ProgressBarWithCaption();
            groupBox1 = new System.Windows.Forms.GroupBox();
            lblComms = new System.Windows.Forms.Label();
            lblParkings = new System.Windows.Forms.Label();
            lblRunways = new System.Windows.Forms.Label();
            lblAirports = new System.Windows.Forms.Label();
            radLabel4 = new System.Windows.Forms.Label();
            radLabel3 = new System.Windows.Forms.Label();
            radLabel2 = new System.Windows.Forms.Label();
            radLabel1 = new System.Windows.Forms.Label();
            groupBox3 = new System.Windows.Forms.GroupBox();
            lstAircraft = new System.Windows.Forms.ListBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            lblRoutes = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            lblWaypoints = new System.Windows.Forms.Label();
            lblNavaids = new System.Windows.Forms.Label();
            radLabel7 = new System.Windows.Forms.Label();
            radLabel8 = new System.Windows.Forms.Label();
            btnCancel = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            // 
            // pbImporter
            // 
            pbImporter.CustomText = null;
            pbImporter.DisplayType = AWDControls.ProgressBarDisplayText.CustomText;
            pbImporter.Location = new System.Drawing.Point(12, 155);
            pbImporter.Name = "pbImporter";
            pbImporter.Size = new System.Drawing.Size(715, 23);
            pbImporter.TabIndex = 13;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblComms);
            groupBox1.Controls.Add(lblParkings);
            groupBox1.Controls.Add(lblRunways);
            groupBox1.Controls.Add(lblAirports);
            groupBox1.Controls.Add(radLabel4);
            groupBox1.Controls.Add(radLabel3);
            groupBox1.Controls.Add(radLabel2);
            groupBox1.Controls.Add(radLabel1);
            groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            groupBox1.Location = new System.Drawing.Point(12, 10);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(182, 126);
            groupBox1.TabIndex = 14;
            groupBox1.TabStop = false;
            groupBox1.Text = "Airport Data";
            // 
            // lblComms
            // 
            lblComms.AutoSize = true;
            lblComms.Location = new System.Drawing.Point(91, 94);
            lblComms.Name = "lblComms";
            lblComms.Size = new System.Drawing.Size(13, 15);
            lblComms.TabIndex = 14;
            lblComms.Text = "0";
            // 
            // lblParkings
            // 
            lblParkings.AutoSize = true;
            lblParkings.Location = new System.Drawing.Point(91, 70);
            lblParkings.Name = "lblParkings";
            lblParkings.Size = new System.Drawing.Size(13, 15);
            lblParkings.TabIndex = 13;
            lblParkings.Text = "0";
            // 
            // lblRunways
            // 
            lblRunways.AutoSize = true;
            lblRunways.Location = new System.Drawing.Point(91, 46);
            lblRunways.Name = "lblRunways";
            lblRunways.Size = new System.Drawing.Size(13, 15);
            lblRunways.TabIndex = 12;
            lblRunways.Text = "0";
            // 
            // lblAirports
            // 
            lblAirports.AutoSize = true;
            lblAirports.Location = new System.Drawing.Point(91, 22);
            lblAirports.Name = "lblAirports";
            lblAirports.Size = new System.Drawing.Size(13, 15);
            lblAirports.TabIndex = 11;
            lblAirports.Text = "0";
            // 
            // radLabel4
            // 
            radLabel4.Location = new System.Drawing.Point(6, 70);
            radLabel4.Name = "radLabel4";
            radLabel4.Size = new System.Drawing.Size(85, 18);
            radLabel4.TabIndex = 10;
            radLabel4.Text = "Parking Spots:";
            // 
            // radLabel3
            // 
            radLabel3.Location = new System.Drawing.Point(6, 94);
            radLabel3.Name = "radLabel3";
            radLabel3.Size = new System.Drawing.Size(66, 18);
            radLabel3.TabIndex = 9;
            radLabel3.Text = "Comms:";
            // 
            // radLabel2
            // 
            radLabel2.Location = new System.Drawing.Point(6, 46);
            radLabel2.Name = "radLabel2";
            radLabel2.Size = new System.Drawing.Size(66, 18);
            radLabel2.TabIndex = 8;
            radLabel2.Text = "Runways:";
            // 
            // radLabel1
            // 
            radLabel1.Location = new System.Drawing.Point(6, 22);
            radLabel1.Name = "radLabel1";
            radLabel1.Size = new System.Drawing.Size(66, 18);
            radLabel1.TabIndex = 7;
            radLabel1.Text = "Airports:";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(lstAircraft);
            groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            groupBox3.Location = new System.Drawing.Point(411, 10);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(316, 130);
            groupBox3.TabIndex = 16;
            groupBox3.TabStop = false;
            groupBox3.Text = "Aircraft Found";
            // 
            // lstAircraft
            // 
            lstAircraft.FormattingEnabled = true;
            lstAircraft.ItemHeight = 15;
            lstAircraft.Location = new System.Drawing.Point(6, 22);
            lstAircraft.Name = "lstAircraft";
            lstAircraft.Size = new System.Drawing.Size(304, 94);
            lstAircraft.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lblRoutes);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(lblWaypoints);
            groupBox2.Controls.Add(lblNavaids);
            groupBox2.Controls.Add(radLabel7);
            groupBox2.Controls.Add(radLabel8);
            groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            groupBox2.Location = new System.Drawing.Point(212, 10);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(182, 126);
            groupBox2.TabIndex = 15;
            groupBox2.TabStop = false;
            groupBox2.Text = "Navigation Data";
            // 
            // lblRoutes
            // 
            lblRoutes.AutoSize = true;
            lblRoutes.Location = new System.Drawing.Point(85, 70);
            lblRoutes.Name = "lblRoutes";
            lblRoutes.Size = new System.Drawing.Size(13, 15);
            lblRoutes.TabIndex = 15;
            lblRoutes.Text = "0";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(6, 70);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(73, 18);
            label2.TabIndex = 14;
            label2.Text = "Routes:";
            // 
            // lblWaypoints
            // 
            lblWaypoints.AutoSize = true;
            lblWaypoints.Location = new System.Drawing.Point(85, 46);
            lblWaypoints.Name = "lblWaypoints";
            lblWaypoints.Size = new System.Drawing.Size(13, 15);
            lblWaypoints.TabIndex = 13;
            lblWaypoints.Text = "0";
            // 
            // lblNavaids
            // 
            lblNavaids.AutoSize = true;
            lblNavaids.Location = new System.Drawing.Point(85, 22);
            lblNavaids.Name = "lblNavaids";
            lblNavaids.Size = new System.Drawing.Size(13, 15);
            lblNavaids.TabIndex = 12;
            lblNavaids.Text = "0";
            // 
            // radLabel7
            // 
            radLabel7.Location = new System.Drawing.Point(6, 46);
            radLabel7.Name = "radLabel7";
            radLabel7.Size = new System.Drawing.Size(73, 18);
            radLabel7.TabIndex = 11;
            radLabel7.Text = "Waypoints:";
            // 
            // radLabel8
            // 
            radLabel8.Location = new System.Drawing.Point(6, 22);
            radLabel8.Name = "radLabel8";
            radLabel8.Size = new System.Drawing.Size(62, 18);
            radLabel8.TabIndex = 10;
            radLabel8.Text = "Navaids:";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(652, 194);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // BGLImporter
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(739, 229);
            Controls.Add(btnCancel);
            Controls.Add(groupBox2);
            Controls.Add(groupBox3);
            Controls.Add(groupBox1);
            Controls.Add(pbImporter);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "BGLImporter";
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Flight Simulator Scenery Importer";
            Shown += BGLImporter_Shown;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private AWDControls.ProgressBarWithCaption pbImporter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label radLabel4;
        private System.Windows.Forms.Label radLabel3;
        private System.Windows.Forms.Label radLabel2;
        private System.Windows.Forms.Label radLabel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label radLabel7;
        private System.Windows.Forms.Label radLabel8;
        private System.Windows.Forms.ListBox lstAircraft;
        private System.Windows.Forms.Label lblRunways;
        private System.Windows.Forms.Label lblAirports;
        private System.Windows.Forms.Label lblComms;
        private System.Windows.Forms.Label lblParkings;
        private System.Windows.Forms.Label lblWaypoints;
        private System.Windows.Forms.Label lblNavaids;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRoutes;
        private System.Windows.Forms.Label label2;
    }
}