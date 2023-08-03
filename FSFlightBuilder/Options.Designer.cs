namespace FSFlightBuilder
{
    partial class FrmOptions
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
            components = new System.ComponentModel.Container();
            fbDialog = new System.Windows.Forms.FolderBrowserDialog();
            btnSave = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            cmbFS = new System.Windows.Forms.ComboBox();
            label5 = new System.Windows.Forms.Label();
            chkUpdates = new System.Windows.Forms.CheckBox();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            groupBox1 = new System.Windows.Forms.GroupBox();
            tabFolders = new System.Windows.Forms.TabControl();
            tabFSX = new System.Windows.Forms.TabPage();
            btnChooseFSXAirplane = new System.Windows.Forms.Button();
            btnChooseFSXFP = new System.Windows.Forms.Button();
            btnChooseFSX = new System.Windows.Forms.Button();
            txtFSXAirplanesFolder = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            txtFSXFlightPlanFolder = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            txtFSXPath = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            tabFSXSE = new System.Windows.Forms.TabPage();
            btnChooseFSXSEAirplane = new System.Windows.Forms.Button();
            btnChooseFSXSEFP = new System.Windows.Forms.Button();
            btnChooseFSXSE = new System.Windows.Forms.Button();
            txtFSXSEPath = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            txtFSXSEFlightPlanFolder = new System.Windows.Forms.TextBox();
            txtFSXSEAirplanesFolder = new System.Windows.Forms.TextBox();
            label9 = new System.Windows.Forms.Label();
            tabP3D = new System.Windows.Forms.TabPage();
            btnChooseCustomP3D = new System.Windows.Forms.Button();
            txtP3DCustomPath = new System.Windows.Forms.TextBox();
            label11 = new System.Windows.Forms.Label();
            btnChooseP3DAirplane = new System.Windows.Forms.Button();
            btnChooseP3DFP = new System.Windows.Forms.Button();
            btnChooseP3D = new System.Windows.Forms.Button();
            txtP3DAirplanesFolder = new System.Windows.Forms.TextBox();
            label10 = new System.Windows.Forms.Label();
            txtP3DPath = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            txtP3DFlightPlanFolder = new System.Windows.Forms.TextBox();
            tabMSFS = new System.Windows.Forms.TabPage();
            btnChooseMSFSCustom = new System.Windows.Forms.Button();
            txtMSFSCustomFolder = new System.Windows.Forms.TextBox();
            label12 = new System.Windows.Forms.Label();
            btnChooseMSFSAirplane = new System.Windows.Forms.Button();
            btnChooseMSFSFP = new System.Windows.Forms.Button();
            btnChooseMSFS = new System.Windows.Forms.Button();
            txtMSFSFolder = new System.Windows.Forms.TextBox();
            label14 = new System.Windows.Forms.Label();
            label15 = new System.Windows.Forms.Label();
            txtMSFSFlightPlanFolder = new System.Windows.Forms.TextBox();
            txtMSFSAirplanesFolder = new System.Windows.Forms.TextBox();
            label16 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            rdoAuto = new System.Windows.Forms.RadioButton();
            rdoAviationAPI = new System.Windows.Forms.RadioButton();
            rdoFAACharts = new System.Windows.Forms.RadioButton();
            chkDeleteImages = new System.Windows.Forms.CheckBox();
            lnkFindFS = new System.Windows.Forms.LinkLabel();
            groupBox1.SuspendLayout();
            tabFolders.SuspendLayout();
            tabFSX.SuspendLayout();
            tabFSXSE.SuspendLayout();
            tabP3D.SuspendLayout();
            tabMSFS.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // fbDialog
            // 
            fbDialog.Description = "Please select the folder that contains your Flight Plans";
            // 
            // btnSave
            // 
            btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSave.Location = new System.Drawing.Point(588, 395);
            btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(88, 27);
            btnSave.TabIndex = 11;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(706, 395);
            btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(88, 27);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // cmbFS
            // 
            cmbFS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbFS.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cmbFS.FormattingEnabled = true;
            cmbFS.Items.AddRange(new object[] { "FSX", "FSX Steam Edition", "Prepar3D" });
            cmbFS.Location = new System.Drawing.Point(235, 13);
            cmbFS.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbFS.Name = "cmbFS";
            cmbFS.Size = new System.Drawing.Size(321, 32);
            cmbFS.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label5.Location = new System.Drawing.Point(9, 16);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(218, 24);
            label5.TabIndex = 17;
            label5.Text = "Selected Flight Simulator";
            // 
            // chkUpdates
            // 
            chkUpdates.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            chkUpdates.AutoSize = true;
            chkUpdates.Location = new System.Drawing.Point(14, 400);
            chkUpdates.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkUpdates.Name = "chkUpdates";
            chkUpdates.Size = new System.Drawing.Size(165, 19);
            chkUpdates.TabIndex = 10;
            chkUpdates.Text = "Check for updates on start";
            chkUpdates.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tabFolders);
            groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            groupBox1.Location = new System.Drawing.Point(12, 66);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(782, 209);
            groupBox1.TabIndex = 88;
            groupBox1.TabStop = false;
            groupBox1.Text = "Flight Sim Folders";
            // 
            // tabFolders
            // 
            tabFolders.Controls.Add(tabFSX);
            tabFolders.Controls.Add(tabFSXSE);
            tabFolders.Controls.Add(tabP3D);
            tabFolders.Controls.Add(tabMSFS);
            tabFolders.Location = new System.Drawing.Point(7, 22);
            tabFolders.Name = "tabFolders";
            tabFolders.SelectedIndex = 0;
            tabFolders.Size = new System.Drawing.Size(769, 161);
            tabFolders.TabIndex = 1;
            // 
            // tabFSX
            // 
            tabFSX.Controls.Add(btnChooseFSXAirplane);
            tabFSX.Controls.Add(btnChooseFSXFP);
            tabFSX.Controls.Add(btnChooseFSX);
            tabFSX.Controls.Add(txtFSXAirplanesFolder);
            tabFSX.Controls.Add(label8);
            tabFSX.Controls.Add(txtFSXFlightPlanFolder);
            tabFSX.Controls.Add(label2);
            tabFSX.Controls.Add(txtFSXPath);
            tabFSX.Controls.Add(label1);
            tabFSX.Location = new System.Drawing.Point(4, 24);
            tabFSX.Name = "tabFSX";
            tabFSX.Padding = new System.Windows.Forms.Padding(3);
            tabFSX.Size = new System.Drawing.Size(761, 133);
            tabFSX.TabIndex = 0;
            tabFSX.Text = "FSX Folders";
            tabFSX.UseVisualStyleBackColor = true;
            // 
            // btnChooseFSXAirplane
            // 
            btnChooseFSXAirplane.Location = new System.Drawing.Point(664, 65);
            btnChooseFSXAirplane.Name = "btnChooseFSXAirplane";
            btnChooseFSXAirplane.Size = new System.Drawing.Size(91, 23);
            btnChooseFSXAirplane.TabIndex = 89;
            btnChooseFSXAirplane.Text = "Choose";
            btnChooseFSXAirplane.UseVisualStyleBackColor = true;
            btnChooseFSXAirplane.Click += btnChooseFSXAirplane_Click;
            // 
            // btnChooseFSXFP
            // 
            btnChooseFSXFP.Location = new System.Drawing.Point(664, 34);
            btnChooseFSXFP.Name = "btnChooseFSXFP";
            btnChooseFSXFP.Size = new System.Drawing.Size(91, 23);
            btnChooseFSXFP.TabIndex = 88;
            btnChooseFSXFP.Text = "Choose";
            btnChooseFSXFP.UseVisualStyleBackColor = true;
            btnChooseFSXFP.Click += btnChooseFSXFP_Click;
            // 
            // btnChooseFSX
            // 
            btnChooseFSX.Location = new System.Drawing.Point(664, 5);
            btnChooseFSX.Name = "btnChooseFSX";
            btnChooseFSX.Size = new System.Drawing.Size(91, 23);
            btnChooseFSX.TabIndex = 87;
            btnChooseFSX.Text = "Choose";
            btnChooseFSX.UseVisualStyleBackColor = true;
            btnChooseFSX.Click += btnChooseFSX_Click;
            // 
            // txtFSXAirplanesFolder
            // 
            txtFSXAirplanesFolder.Location = new System.Drawing.Point(122, 66);
            txtFSXAirplanesFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtFSXAirplanesFolder.Name = "txtFSXAirplanesFolder";
            txtFSXAirplanesFolder.Size = new System.Drawing.Size(535, 23);
            txtFSXAirplanesFolder.TabIndex = 86;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(7, 70);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(92, 15);
            label8.TabIndex = 85;
            label8.Text = "Airplanes Folder";
            // 
            // txtFSXFlightPlanFolder
            // 
            txtFSXFlightPlanFolder.Location = new System.Drawing.Point(122, 36);
            txtFSXFlightPlanFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtFSXFlightPlanFolder.Name = "txtFSXFlightPlanFolder";
            txtFSXFlightPlanFolder.Size = new System.Drawing.Size(535, 23);
            txtFSXFlightPlanFolder.TabIndex = 84;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(7, 40);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(89, 15);
            label2.TabIndex = 83;
            label2.Text = "Missions Folder";
            // 
            // txtFSXPath
            // 
            txtFSXPath.Location = new System.Drawing.Point(122, 6);
            txtFSXPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtFSXPath.Name = "txtFSXPath";
            txtFSXPath.Size = new System.Drawing.Size(535, 23);
            txtFSXPath.TabIndex = 82;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 10);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(63, 15);
            label1.TabIndex = 81;
            label1.Text = "Sim Folder";
            // 
            // tabFSXSE
            // 
            tabFSXSE.Controls.Add(btnChooseFSXSEAirplane);
            tabFSXSE.Controls.Add(btnChooseFSXSEFP);
            tabFSXSE.Controls.Add(btnChooseFSXSE);
            tabFSXSE.Controls.Add(txtFSXSEPath);
            tabFSXSE.Controls.Add(label7);
            tabFSXSE.Controls.Add(label6);
            tabFSXSE.Controls.Add(txtFSXSEFlightPlanFolder);
            tabFSXSE.Controls.Add(txtFSXSEAirplanesFolder);
            tabFSXSE.Controls.Add(label9);
            tabFSXSE.Location = new System.Drawing.Point(4, 24);
            tabFSXSE.Name = "tabFSXSE";
            tabFSXSE.Padding = new System.Windows.Forms.Padding(3);
            tabFSXSE.Size = new System.Drawing.Size(761, 133);
            tabFSXSE.TabIndex = 1;
            tabFSXSE.Text = "FSX Steam Edition Folders";
            tabFSXSE.UseVisualStyleBackColor = true;
            // 
            // btnChooseFSXSEAirplane
            // 
            btnChooseFSXSEAirplane.Location = new System.Drawing.Point(664, 66);
            btnChooseFSXSEAirplane.Name = "btnChooseFSXSEAirplane";
            btnChooseFSXSEAirplane.Size = new System.Drawing.Size(91, 23);
            btnChooseFSXSEAirplane.TabIndex = 92;
            btnChooseFSXSEAirplane.Text = "Choose";
            btnChooseFSXSEAirplane.UseVisualStyleBackColor = true;
            btnChooseFSXSEAirplane.Click += btnChooseFSXSEAirplanes_Click;
            // 
            // btnChooseFSXSEFP
            // 
            btnChooseFSXSEFP.Location = new System.Drawing.Point(664, 35);
            btnChooseFSXSEFP.Name = "btnChooseFSXSEFP";
            btnChooseFSXSEFP.Size = new System.Drawing.Size(91, 23);
            btnChooseFSXSEFP.TabIndex = 91;
            btnChooseFSXSEFP.Text = "Choose";
            btnChooseFSXSEFP.UseVisualStyleBackColor = true;
            btnChooseFSXSEFP.Click += btnChooseFSXSEFP_Click;
            // 
            // btnChooseFSXSE
            // 
            btnChooseFSXSE.Location = new System.Drawing.Point(664, 6);
            btnChooseFSXSE.Name = "btnChooseFSXSE";
            btnChooseFSXSE.Size = new System.Drawing.Size(91, 23);
            btnChooseFSXSE.TabIndex = 90;
            btnChooseFSXSE.Text = "Choose";
            btnChooseFSXSE.UseVisualStyleBackColor = true;
            btnChooseFSXSE.Click += btnChooseFSXSE_Click;
            // 
            // txtFSXSEPath
            // 
            txtFSXSEPath.Location = new System.Drawing.Point(133, 6);
            txtFSXSEPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtFSXSEPath.Name = "txtFSXSEPath";
            txtFSXSEPath.Size = new System.Drawing.Size(524, 23);
            txtFSXSEPath.TabIndex = 76;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(8, 10);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(63, 15);
            label7.TabIndex = 75;
            label7.Text = "Sim Folder";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(8, 40);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(89, 15);
            label6.TabIndex = 77;
            label6.Text = "Missions Folder";
            // 
            // txtFSXSEFlightPlanFolder
            // 
            txtFSXSEFlightPlanFolder.Location = new System.Drawing.Point(133, 36);
            txtFSXSEFlightPlanFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtFSXSEFlightPlanFolder.Name = "txtFSXSEFlightPlanFolder";
            txtFSXSEFlightPlanFolder.Size = new System.Drawing.Size(524, 23);
            txtFSXSEFlightPlanFolder.TabIndex = 78;
            // 
            // txtFSXSEAirplanesFolder
            // 
            txtFSXSEAirplanesFolder.Location = new System.Drawing.Point(133, 66);
            txtFSXSEAirplanesFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtFSXSEAirplanesFolder.Name = "txtFSXSEAirplanesFolder";
            txtFSXSEAirplanesFolder.Size = new System.Drawing.Size(524, 23);
            txtFSXSEAirplanesFolder.TabIndex = 80;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(8, 70);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(92, 15);
            label9.TabIndex = 79;
            label9.Text = "Airplanes Folder";
            // 
            // tabP3D
            // 
            tabP3D.Controls.Add(btnChooseCustomP3D);
            tabP3D.Controls.Add(txtP3DCustomPath);
            tabP3D.Controls.Add(label11);
            tabP3D.Controls.Add(btnChooseP3DAirplane);
            tabP3D.Controls.Add(btnChooseP3DFP);
            tabP3D.Controls.Add(btnChooseP3D);
            tabP3D.Controls.Add(txtP3DAirplanesFolder);
            tabP3D.Controls.Add(label10);
            tabP3D.Controls.Add(txtP3DPath);
            tabP3D.Controls.Add(label4);
            tabP3D.Controls.Add(label3);
            tabP3D.Controls.Add(txtP3DFlightPlanFolder);
            tabP3D.Location = new System.Drawing.Point(4, 24);
            tabP3D.Name = "tabP3D";
            tabP3D.Padding = new System.Windows.Forms.Padding(3);
            tabP3D.Size = new System.Drawing.Size(761, 133);
            tabP3D.TabIndex = 2;
            tabP3D.Text = "Prepar3D Folders";
            tabP3D.UseVisualStyleBackColor = true;
            // 
            // btnChooseCustomP3D
            // 
            btnChooseCustomP3D.Location = new System.Drawing.Point(664, 95);
            btnChooseCustomP3D.Name = "btnChooseCustomP3D";
            btnChooseCustomP3D.Size = new System.Drawing.Size(91, 23);
            btnChooseCustomP3D.TabIndex = 95;
            btnChooseCustomP3D.Text = "Choose";
            btnChooseCustomP3D.UseVisualStyleBackColor = true;
            btnChooseCustomP3D.Click += btnChooseCustomP3D_Click;
            // 
            // txtP3DCustomPath
            // 
            txtP3DCustomPath.Location = new System.Drawing.Point(133, 95);
            txtP3DCustomPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtP3DCustomPath.Name = "txtP3DCustomPath";
            txtP3DCustomPath.Size = new System.Drawing.Size(524, 23);
            txtP3DCustomPath.TabIndex = 94;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(13, 99);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(85, 15);
            label11.TabIndex = 93;
            label11.Text = "Custom Folder";
            // 
            // btnChooseP3DAirplane
            // 
            btnChooseP3DAirplane.Location = new System.Drawing.Point(664, 66);
            btnChooseP3DAirplane.Name = "btnChooseP3DAirplane";
            btnChooseP3DAirplane.Size = new System.Drawing.Size(91, 23);
            btnChooseP3DAirplane.TabIndex = 92;
            btnChooseP3DAirplane.Text = "Choose";
            btnChooseP3DAirplane.UseVisualStyleBackColor = true;
            btnChooseP3DAirplane.Click += btnChooseP3DAirplanes_Click;
            // 
            // btnChooseP3DFP
            // 
            btnChooseP3DFP.Location = new System.Drawing.Point(664, 35);
            btnChooseP3DFP.Name = "btnChooseP3DFP";
            btnChooseP3DFP.Size = new System.Drawing.Size(91, 23);
            btnChooseP3DFP.TabIndex = 91;
            btnChooseP3DFP.Text = "Choose";
            btnChooseP3DFP.UseVisualStyleBackColor = true;
            btnChooseP3DFP.Click += btnChooseP3DFP_Click;
            // 
            // btnChooseP3D
            // 
            btnChooseP3D.Location = new System.Drawing.Point(664, 6);
            btnChooseP3D.Name = "btnChooseP3D";
            btnChooseP3D.Size = new System.Drawing.Size(91, 23);
            btnChooseP3D.TabIndex = 90;
            btnChooseP3D.Text = "Choose";
            btnChooseP3D.UseVisualStyleBackColor = true;
            btnChooseP3D.Click += btnChooseP3D_Click;
            // 
            // txtP3DAirplanesFolder
            // 
            txtP3DAirplanesFolder.Location = new System.Drawing.Point(133, 66);
            txtP3DAirplanesFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtP3DAirplanesFolder.Name = "txtP3DAirplanesFolder";
            txtP3DAirplanesFolder.Size = new System.Drawing.Size(524, 23);
            txtP3DAirplanesFolder.TabIndex = 77;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(13, 70);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(92, 15);
            label10.TabIndex = 76;
            label10.Text = "Airplanes Folder";
            // 
            // txtP3DPath
            // 
            txtP3DPath.Location = new System.Drawing.Point(133, 6);
            txtP3DPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtP3DPath.Name = "txtP3DPath";
            txtP3DPath.Size = new System.Drawing.Size(524, 23);
            txtP3DPath.TabIndex = 73;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(13, 10);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(63, 15);
            label4.TabIndex = 72;
            label4.Text = "Sim Folder";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(13, 40);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(99, 15);
            label3.TabIndex = 74;
            label3.Text = "Flight Plan Folder";
            // 
            // txtP3DFlightPlanFolder
            // 
            txtP3DFlightPlanFolder.Location = new System.Drawing.Point(133, 36);
            txtP3DFlightPlanFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtP3DFlightPlanFolder.Name = "txtP3DFlightPlanFolder";
            txtP3DFlightPlanFolder.Size = new System.Drawing.Size(524, 23);
            txtP3DFlightPlanFolder.TabIndex = 75;
            // 
            // tabMSFS
            // 
            tabMSFS.Controls.Add(btnChooseMSFSCustom);
            tabMSFS.Controls.Add(txtMSFSCustomFolder);
            tabMSFS.Controls.Add(label12);
            tabMSFS.Controls.Add(btnChooseMSFSAirplane);
            tabMSFS.Controls.Add(btnChooseMSFSFP);
            tabMSFS.Controls.Add(btnChooseMSFS);
            tabMSFS.Controls.Add(txtMSFSFolder);
            tabMSFS.Controls.Add(label14);
            tabMSFS.Controls.Add(label15);
            tabMSFS.Controls.Add(txtMSFSFlightPlanFolder);
            tabMSFS.Controls.Add(txtMSFSAirplanesFolder);
            tabMSFS.Controls.Add(label16);
            tabMSFS.Location = new System.Drawing.Point(4, 24);
            tabMSFS.Name = "tabMSFS";
            tabMSFS.Padding = new System.Windows.Forms.Padding(3);
            tabMSFS.Size = new System.Drawing.Size(761, 133);
            tabMSFS.TabIndex = 3;
            tabMSFS.Text = "Microsoft Flight Simulator Folders";
            tabMSFS.UseVisualStyleBackColor = true;
            // 
            // btnChooseMSFSCustom
            // 
            btnChooseMSFSCustom.Location = new System.Drawing.Point(660, 95);
            btnChooseMSFSCustom.Name = "btnChooseMSFSCustom";
            btnChooseMSFSCustom.Size = new System.Drawing.Size(91, 23);
            btnChooseMSFSCustom.TabIndex = 101;
            btnChooseMSFSCustom.Text = "Choose";
            btnChooseMSFSCustom.UseVisualStyleBackColor = true;
            btnChooseMSFSCustom.Click += btnChooseMSFSCustom_Click;
            // 
            // txtMSFSCustomFolder
            // 
            txtMSFSCustomFolder.Location = new System.Drawing.Point(136, 95);
            txtMSFSCustomFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtMSFSCustomFolder.Name = "txtMSFSCustomFolder";
            txtMSFSCustomFolder.Size = new System.Drawing.Size(517, 23);
            txtMSFSCustomFolder.TabIndex = 100;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(9, 99);
            label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(107, 15);
            label12.TabIndex = 99;
            label12.Text = "Community Folder";
            // 
            // btnChooseMSFSAirplane
            // 
            btnChooseMSFSAirplane.Location = new System.Drawing.Point(660, 66);
            btnChooseMSFSAirplane.Name = "btnChooseMSFSAirplane";
            btnChooseMSFSAirplane.Size = new System.Drawing.Size(91, 23);
            btnChooseMSFSAirplane.TabIndex = 98;
            btnChooseMSFSAirplane.Text = "Choose";
            btnChooseMSFSAirplane.UseVisualStyleBackColor = true;
            btnChooseMSFSAirplane.Click += btnChooseMSFSAirplane_Click;
            // 
            // btnChooseMSFSFP
            // 
            btnChooseMSFSFP.Location = new System.Drawing.Point(660, 35);
            btnChooseMSFSFP.Name = "btnChooseMSFSFP";
            btnChooseMSFSFP.Size = new System.Drawing.Size(91, 23);
            btnChooseMSFSFP.TabIndex = 97;
            btnChooseMSFSFP.Text = "Choose";
            btnChooseMSFSFP.UseVisualStyleBackColor = true;
            btnChooseMSFSFP.Click += btnChooseMSFSFP_Click;
            // 
            // btnChooseMSFS
            // 
            btnChooseMSFS.Location = new System.Drawing.Point(660, 6);
            btnChooseMSFS.Name = "btnChooseMSFS";
            btnChooseMSFS.Size = new System.Drawing.Size(91, 23);
            btnChooseMSFS.TabIndex = 96;
            btnChooseMSFS.Text = "Choose";
            btnChooseMSFS.UseVisualStyleBackColor = true;
            btnChooseMSFS.Click += btnChooseMSFSClient_Click;
            // 
            // txtMSFSFolder
            // 
            txtMSFSFolder.Location = new System.Drawing.Point(136, 6);
            txtMSFSFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtMSFSFolder.Name = "txtMSFSFolder";
            txtMSFSFolder.Size = new System.Drawing.Size(517, 23);
            txtMSFSFolder.TabIndex = 91;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(9, 10);
            label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(74, 15);
            label14.TabIndex = 90;
            label14.Text = "Client Folder";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(9, 70);
            label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(92, 15);
            label15.TabIndex = 94;
            label15.Text = "Airplanes Folder";
            // 
            // txtMSFSFlightPlanFolder
            // 
            txtMSFSFlightPlanFolder.Location = new System.Drawing.Point(136, 36);
            txtMSFSFlightPlanFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtMSFSFlightPlanFolder.Name = "txtMSFSFlightPlanFolder";
            txtMSFSFlightPlanFolder.Size = new System.Drawing.Size(517, 23);
            txtMSFSFlightPlanFolder.TabIndex = 93;
            // 
            // txtMSFSAirplanesFolder
            // 
            txtMSFSAirplanesFolder.Location = new System.Drawing.Point(136, 66);
            txtMSFSAirplanesFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtMSFSAirplanesFolder.Name = "txtMSFSAirplanesFolder";
            txtMSFSAirplanesFolder.Size = new System.Drawing.Size(517, 23);
            txtMSFSAirplanesFolder.TabIndex = 95;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(9, 40);
            label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(99, 15);
            label16.TabIndex = 92;
            label16.Text = "Flight Plan Folder";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rdoAuto);
            groupBox2.Controls.Add(rdoAviationAPI);
            groupBox2.Controls.Add(rdoFAACharts);
            groupBox2.Controls.Add(chkDeleteImages);
            groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            groupBox2.Location = new System.Drawing.Point(12, 281);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(782, 82);
            groupBox2.TabIndex = 89;
            groupBox2.TabStop = false;
            groupBox2.Text = "Charts";
            // 
            // rdoAuto
            // 
            rdoAuto.AutoSize = true;
            rdoAuto.Location = new System.Drawing.Point(409, 49);
            rdoAuto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rdoAuto.Name = "rdoAuto";
            rdoAuto.Size = new System.Drawing.Size(190, 19);
            rdoAuto.TabIndex = 9;
            rdoAuto.Tag = "ChartType";
            rdoAuto.Text = "Automatic (FAA for U.S. routes)";
            rdoAuto.UseVisualStyleBackColor = true;
            rdoAuto.Visible = false;
            // 
            // rdoAviationAPI
            // 
            rdoAviationAPI.AutoSize = true;
            rdoAviationAPI.Checked = true;
            rdoAviationAPI.Location = new System.Drawing.Point(175, 49);
            rdoAviationAPI.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rdoAviationAPI.Name = "rdoAviationAPI";
            rdoAviationAPI.Size = new System.Drawing.Size(204, 19);
            rdoAviationAPI.TabIndex = 8;
            rdoAviationAPI.TabStop = true;
            rdoAviationAPI.Tag = "ChartType";
            rdoAviationAPI.Text = "AviationAPI Charts (Includes AFD)";
            rdoAviationAPI.UseVisualStyleBackColor = true;
            // 
            // rdoFAACharts
            // 
            rdoFAACharts.AutoSize = true;
            rdoFAACharts.Location = new System.Drawing.Point(8, 49);
            rdoFAACharts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            rdoFAACharts.Name = "rdoFAACharts";
            rdoFAACharts.Size = new System.Drawing.Size(144, 19);
            rdoFAACharts.TabIndex = 7;
            rdoFAACharts.Tag = "ChartType";
            rdoFAACharts.Text = "FAA Charts (USA Only)";
            rdoFAACharts.UseVisualStyleBackColor = true;
            // 
            // chkDeleteImages
            // 
            chkDeleteImages.AutoSize = true;
            chkDeleteImages.Checked = true;
            chkDeleteImages.CheckState = System.Windows.Forms.CheckState.Checked;
            chkDeleteImages.Location = new System.Drawing.Point(7, 22);
            chkDeleteImages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkDeleteImages.Name = "chkDeleteImages";
            chkDeleteImages.Size = new System.Drawing.Size(279, 19);
            chkDeleteImages.TabIndex = 6;
            chkDeleteImages.Text = "Delete chart images when the application closes";
            chkDeleteImages.UseVisualStyleBackColor = true;
            // 
            // lnkFindFS
            // 
            lnkFindFS.AutoSize = true;
            lnkFindFS.Location = new System.Drawing.Point(585, 30);
            lnkFindFS.Name = "lnkFindFS";
            lnkFindFS.Size = new System.Drawing.Size(193, 15);
            lnkFindFS.TabIndex = 90;
            lnkFindFS.TabStop = true;
            lnkFindFS.Text = "My flight sim is not listed. Recheck.";
            lnkFindFS.LinkClicked += lnkFindFS_LinkClicked;
            // 
            // FrmOptions
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(807, 435);
            Controls.Add(lnkFindFS);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(cmbFS);
            Controls.Add(label5);
            Controls.Add(chkUpdates);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FrmOptions";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "FS Flight Builder Options";
            groupBox1.ResumeLayout(false);
            tabFolders.ResumeLayout(false);
            tabFSX.ResumeLayout(false);
            tabFSX.PerformLayout();
            tabFSXSE.ResumeLayout(false);
            tabFSXSE.PerformLayout();
            tabP3D.ResumeLayout(false);
            tabP3D.PerformLayout();
            tabMSFS.ResumeLayout(false);
            tabMSFS.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog fbDialog;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbFS;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkUpdates;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabFolders;
        private System.Windows.Forms.TabPage tabFSX;
        private System.Windows.Forms.TabPage tabFSXSE;
        private System.Windows.Forms.TabPage tabP3D;
        private System.Windows.Forms.TabPage tabMSFS;
        private System.Windows.Forms.TextBox txtFSXAirplanesFolder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtFSXFlightPlanFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFSXPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFSXSEPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFSXSEFlightPlanFolder;
        private System.Windows.Forms.TextBox txtFSXSEAirplanesFolder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtP3DAirplanesFolder;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtP3DPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtP3DFlightPlanFolder;
        private System.Windows.Forms.TextBox txtMSFSFolder;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtMSFSFlightPlanFolder;
        private System.Windows.Forms.TextBox txtMSFSAirplanesFolder;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnChooseFSX;
        private System.Windows.Forms.Button btnChooseFSXAirplane;
        private System.Windows.Forms.Button btnChooseFSXFP;
        private System.Windows.Forms.Button btnChooseFSXSEAirplane;
        private System.Windows.Forms.Button btnChooseFSXSEFP;
        private System.Windows.Forms.Button btnChooseFSXSE;
        private System.Windows.Forms.Button btnChooseP3DAirplane;
        private System.Windows.Forms.Button btnChooseP3DFP;
        private System.Windows.Forms.Button btnChooseP3D;
        private System.Windows.Forms.Button btnChooseMSFSAirplane;
        private System.Windows.Forms.Button btnChooseMSFSFP;
        private System.Windows.Forms.Button btnChooseMSFS;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoAuto;
        private System.Windows.Forms.RadioButton rdoAviationAPI;
        private System.Windows.Forms.RadioButton rdoFAACharts;
        private System.Windows.Forms.CheckBox chkDeleteImages;
        private System.Windows.Forms.LinkLabel lnkFindFS;
        private System.Windows.Forms.Button btnChooseCustomP3D;
        private System.Windows.Forms.TextBox txtP3DCustomPath;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnChooseMSFSCustom;
        private System.Windows.Forms.TextBox txtMSFSCustomFolder;
        private System.Windows.Forms.Label label12;
    }
}