namespace FSFlightBuilder
{
    partial class AircraftEditor
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
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            treeAircraft = new System.Windows.Forms.TreeView();
            numCruiseSpeed = new System.Windows.Forms.NumericUpDown();
            label5 = new System.Windows.Forms.Label();
            numDescentRate = new System.Windows.Forms.NumericUpDown();
            numDescentSpeed = new System.Windows.Forms.NumericUpDown();
            numClimbRate = new System.Windows.Forms.NumericUpDown();
            numClimbSpeed = new System.Windows.Forms.NumericUpDown();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCruiseSpeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDescentRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDescentSpeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numClimbRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numClimbSpeed).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeAircraft);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(numCruiseSpeed);
            splitContainer1.Panel2.Controls.Add(label5);
            splitContainer1.Panel2.Controls.Add(numDescentRate);
            splitContainer1.Panel2.Controls.Add(numDescentSpeed);
            splitContainer1.Panel2.Controls.Add(numClimbRate);
            splitContainer1.Panel2.Controls.Add(numClimbSpeed);
            splitContainer1.Panel2.Controls.Add(label4);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Size = new System.Drawing.Size(716, 488);
            splitContainer1.SplitterDistance = 461;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // treeAircraft
            // 
            treeAircraft.Dock = System.Windows.Forms.DockStyle.Fill;
            treeAircraft.Location = new System.Drawing.Point(0, 0);
            treeAircraft.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeAircraft.Name = "treeAircraft";
            treeAircraft.Size = new System.Drawing.Size(461, 488);
            treeAircraft.TabIndex = 1;
            treeAircraft.AfterSelect += treeAircraft_AfterSelect;
            // 
            // numCruiseSpeed
            // 
            numCruiseSpeed.Location = new System.Drawing.Point(159, 160);
            numCruiseSpeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numCruiseSpeed.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numCruiseSpeed.Name = "numCruiseSpeed";
            numCruiseSpeed.Size = new System.Drawing.Size(74, 23);
            numCruiseSpeed.TabIndex = 7;
            numCruiseSpeed.ValueChanged += numCruiseSpeed_ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(21, 162);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(105, 15);
            label5.TabIndex = 6;
            label5.Text = "Cruise Speed (KTS)";
            // 
            // numDescentRate
            // 
            numDescentRate.Location = new System.Drawing.Point(159, 83);
            numDescentRate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numDescentRate.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numDescentRate.Name = "numDescentRate";
            numDescentRate.Size = new System.Drawing.Size(74, 23);
            numDescentRate.TabIndex = 4;
            numDescentRate.ValueChanged += numDescentRate_ValueChanged;
            numDescentRate.Enter += numDescentRate_Enter;
            // 
            // numDescentSpeed
            // 
            numDescentSpeed.Location = new System.Drawing.Point(159, 113);
            numDescentSpeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numDescentSpeed.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numDescentSpeed.Name = "numDescentSpeed";
            numDescentSpeed.Size = new System.Drawing.Size(74, 23);
            numDescentSpeed.TabIndex = 5;
            numDescentSpeed.ValueChanged += numDescentSpeed_ValueChanged;
            numDescentSpeed.Enter += numDescentSpeed_Enter;
            // 
            // numClimbRate
            // 
            numClimbRate.Location = new System.Drawing.Point(159, 8);
            numClimbRate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numClimbRate.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numClimbRate.Name = "numClimbRate";
            numClimbRate.Size = new System.Drawing.Size(74, 23);
            numClimbRate.TabIndex = 2;
            numClimbRate.ValueChanged += numClimbRate_ValueChanged;
            numClimbRate.Enter += numClimbRate_Enter;
            // 
            // numClimbSpeed
            // 
            numClimbSpeed.Location = new System.Drawing.Point(159, 38);
            numClimbSpeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numClimbSpeed.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numClimbSpeed.Name = "numClimbSpeed";
            numClimbSpeed.Size = new System.Drawing.Size(74, 23);
            numClimbSpeed.TabIndex = 3;
            numClimbSpeed.ValueChanged += numClimbSpeed_ValueChanged;
            numClimbSpeed.Enter += numClimbSpeed_Enter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(21, 85);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(110, 15);
            label4.TabIndex = 3;
            label4.Text = "Descent Rate (FPM)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(21, 115);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(114, 15);
            label3.TabIndex = 2;
            label3.Text = "Descent Speed (KTS)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Enabled = false;
            label2.Location = new System.Drawing.Point(21, 10);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(100, 15);
            label2.TabIndex = 1;
            label2.Text = "Climb Rate (FPM)";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Enabled = false;
            label1.Location = new System.Drawing.Point(21, 40);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(104, 15);
            label1.TabIndex = 0;
            label1.Text = "Climb Speed (KTS)";
            // 
            // btnClose
            // 
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnClose.Location = new System.Drawing.Point(615, 495);
            btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(88, 27);
            btnClose.TabIndex = 6;
            btnClose.Text = "Save/Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // AircraftEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(716, 526);
            Controls.Add(btnClose);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "AircraftEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Aircraft Climb/Descent Editor";
            FormClosing += AircraftEditor_FormClosing;
            Load += AircraftEditor_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numCruiseSpeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDescentRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDescentSpeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)numClimbRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)numClimbSpeed).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeAircraft;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.NumericUpDown numDescentRate;
        private System.Windows.Forms.NumericUpDown numDescentSpeed;
        private System.Windows.Forms.NumericUpDown numClimbRate;
        private System.Windows.Forms.NumericUpDown numClimbSpeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numCruiseSpeed;
        private System.Windows.Forms.Label label5;
    }
}