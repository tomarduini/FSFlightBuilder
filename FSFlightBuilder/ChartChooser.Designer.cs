namespace FSFlightBuilder
{
    partial class ChartChooser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartChooser));
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            treeDeparture = new System.Windows.Forms.TreeView();
            treeDestination = new System.Windows.Forms.TreeView();
            btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
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
            splitContainer1.Panel1.Controls.Add(treeDeparture);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(treeDestination);
            splitContainer1.Size = new System.Drawing.Size(612, 488);
            splitContainer1.SplitterDistance = 310;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // treeDeparture
            // 
            treeDeparture.CheckBoxes = true;
            treeDeparture.Dock = System.Windows.Forms.DockStyle.Fill;
            treeDeparture.Location = new System.Drawing.Point(0, 0);
            treeDeparture.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeDeparture.Name = "treeDeparture";
            treeDeparture.Size = new System.Drawing.Size(310, 488);
            treeDeparture.TabIndex = 1;
            treeDeparture.AfterCheck += treeDeparture_AfterCheck;
            // 
            // treeDestination
            // 
            treeDestination.CheckBoxes = true;
            treeDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            treeDestination.Location = new System.Drawing.Point(0, 0);
            treeDestination.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeDestination.Name = "treeDestination";
            treeDestination.Size = new System.Drawing.Size(297, 488);
            treeDestination.TabIndex = 1;
            treeDestination.AfterCheck += treeDestination_AfterCheck;
            // 
            // btnClose
            // 
            btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnClose.Location = new System.Drawing.Point(511, 494);
            btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(88, 27);
            btnClose.TabIndex = 1;
            btnClose.Text = "Save/Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // ChartChooser
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(612, 526);
            Controls.Add(btnClose);
            Controls.Add(splitContainer1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ChartChooser";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Charts (courtesy of AviationAPI.com)";
            Load += ChartChooser_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeDeparture;
        private System.Windows.Forms.TreeView treeDestination;
        private System.Windows.Forms.Button btnClose;
    }
}