namespace FSFlightBuilder
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            txtSkyVector = new System.Windows.Forms.TextBox();
            btnParse = new System.Windows.Forms.Button();
            cmbParking = new System.Windows.Forms.ComboBox();
            lblParking = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            cmbRouteTypes = new System.Windows.Forms.ComboBox();
            btnVFR = new System.Windows.Forms.RadioButton();
            btnIFR = new System.Windows.Forms.RadioButton();
            btnBuildPlan = new System.Windows.Forms.Button();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            updateAircraftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fullDatabaseUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            FSFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aircraftEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newFlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            importFlightFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            utilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            destinationChooserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            userManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            changeLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            label2 = new System.Windows.Forms.Label();
            dtFlight = new System.Windows.Forms.DateTimePicker();
            label5 = new System.Windows.Forms.Label();
            cmbAircraft = new System.Windows.Forms.ComboBox();
            picPreview = new System.Windows.Forms.PictureBox();
            btnLaunchFS = new System.Windows.Forms.Button();
            lblAltitude = new System.Windows.Forms.Label();
            lblAirspeed = new System.Windows.Forms.Label();
            txtAltitude = new System.Windows.Forms.NumericUpDown();
            txtSpeed = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            fdFlight = new System.Windows.Forms.OpenFileDialog();
            label7 = new System.Windows.Forms.Label();
            wbBriefing = new System.Windows.Forms.WebBrowser();
            chkSystem = new System.Windows.Forms.CheckBox();
            btnSystem = new System.Windows.Forms.Button();
            txtRoute = new System.Windows.Forms.RichTextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            lblIFRConditions = new System.Windows.Forms.Label();
            cmbWeatherTypes = new System.Windows.Forms.ComboBox();
            cmbThemes = new System.Windows.Forms.ComboBox();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            chkIncludeTOC = new System.Windows.Forms.CheckBox();
            chkIncludeTOD = new System.Windows.Forms.CheckBox();
            ttSkyVector = new System.Windows.Forms.ToolTip(components);
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            btnSkyVector = new System.Windows.Forms.Button();
            btnPrintBriefing = new System.Windows.Forms.Button();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtAltitude).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtSpeed).BeginInit();
            SuspendLayout();
            // 
            // txtSkyVector
            // 
            txtSkyVector.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtSkyVector.Location = new System.Drawing.Point(98, 33);
            txtSkyVector.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtSkyVector.Name = "txtSkyVector";
            txtSkyVector.Size = new System.Drawing.Size(699, 23);
            txtSkyVector.TabIndex = 0;
            // 
            // btnParse
            // 
            btnParse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnParse.Location = new System.Drawing.Point(808, 31);
            btnParse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnParse.Name = "btnParse";
            btnParse.Size = new System.Drawing.Size(170, 27);
            btnParse.TabIndex = 1;
            btnParse.Text = "Parse SkyVector Input";
            btnParse.UseVisualStyleBackColor = true;
            btnParse.Click += btnParse_Click;
            // 
            // cmbParking
            // 
            cmbParking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbParking.FormattingEnabled = true;
            cmbParking.Location = new System.Drawing.Point(168, 116);
            cmbParking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbParking.Name = "cmbParking";
            cmbParking.Size = new System.Drawing.Size(279, 23);
            cmbParking.TabIndex = 6;
            // 
            // lblParking
            // 
            lblParking.AutoSize = true;
            lblParking.Location = new System.Drawing.Point(5, 119);
            lblParking.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblParking.Name = "lblParking";
            lblParking.Size = new System.Drawing.Size(94, 15);
            lblParking.TabIndex = 5;
            lblParking.Text = "Starting Position";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(5, 269);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(98, 15);
            label3.TabIndex = 6;
            label3.Text = "Flight Route Type";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(5, 299);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(90, 15);
            label4.TabIndex = 7;
            label4.Text = "Flight Plan Type";
            // 
            // cmbRouteTypes
            // 
            cmbRouteTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbRouteTypes.FormattingEnabled = true;
            cmbRouteTypes.Location = new System.Drawing.Point(168, 266);
            cmbRouteTypes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbRouteTypes.Name = "cmbRouteTypes";
            cmbRouteTypes.Size = new System.Drawing.Size(279, 23);
            cmbRouteTypes.TabIndex = 12;
            // 
            // btnVFR
            // 
            btnVFR.AutoSize = true;
            btnVFR.Checked = true;
            btnVFR.Location = new System.Drawing.Point(230, 298);
            btnVFR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnVFR.Name = "btnVFR";
            btnVFR.Size = new System.Drawing.Size(45, 19);
            btnVFR.TabIndex = 13;
            btnVFR.TabStop = true;
            btnVFR.Tag = "PlanType";
            btnVFR.Text = "VFR";
            btnVFR.UseVisualStyleBackColor = true;
            btnVFR.CheckedChanged += btnVFR_CheckedChanged;
            // 
            // btnIFR
            // 
            btnIFR.AutoSize = true;
            btnIFR.Location = new System.Drawing.Point(290, 298);
            btnIFR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnIFR.Name = "btnIFR";
            btnIFR.Size = new System.Drawing.Size(41, 19);
            btnIFR.TabIndex = 14;
            btnIFR.Tag = "PlanType";
            btnIFR.Text = "IFR";
            btnIFR.UseVisualStyleBackColor = true;
            btnIFR.CheckedChanged += btnIFR_CheckedChanged;
            // 
            // btnBuildPlan
            // 
            btnBuildPlan.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnBuildPlan.Location = new System.Drawing.Point(847, 201);
            btnBuildPlan.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnBuildPlan.Name = "btnBuildPlan";
            btnBuildPlan.Size = new System.Drawing.Size(122, 70);
            btnBuildPlan.TabIndex = 18;
            btnBuildPlan.Text = "Build Flight Files";
            btnBuildPlan.UseVisualStyleBackColor = true;
            btnBuildPlan.Click += btnBuildPlan_Click;
            btnBuildPlan.StyleChanged += btnBuildPlan_StyleChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, newFlightToolStripMenuItem, importFlightFileToolStripMenuItem, utilitiesToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(983, 24);
            menuStrip1.TabIndex = 12;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { databaseToolStripMenuItem, FSFolderToolStripMenuItem, aircraftEditorToolStripMenuItem, checkForUpdatesToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // databaseToolStripMenuItem
            // 
            databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { updateAircraftToolStripMenuItem, fullDatabaseUpdateToolStripMenuItem });
            databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            databaseToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            databaseToolStripMenuItem.Text = "Database";
            // 
            // updateAircraftToolStripMenuItem
            // 
            updateAircraftToolStripMenuItem.Name = "updateAircraftToolStripMenuItem";
            updateAircraftToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            updateAircraftToolStripMenuItem.Text = "Update Aircraft Only";
            updateAircraftToolStripMenuItem.Click += updateAircraftToolStripMenuItem_Click;
            // 
            // fullDatabaseUpdateToolStripMenuItem
            // 
            fullDatabaseUpdateToolStripMenuItem.Name = "fullDatabaseUpdateToolStripMenuItem";
            fullDatabaseUpdateToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            fullDatabaseUpdateToolStripMenuItem.Text = "Update Full Database";
            fullDatabaseUpdateToolStripMenuItem.Click += fullDatabaseUpdateToolStripMenuItem_Click;
            // 
            // FSFolderToolStripMenuItem
            // 
            FSFolderToolStripMenuItem.Name = "FSFolderToolStripMenuItem";
            FSFolderToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            FSFolderToolStripMenuItem.Text = "Options";
            FSFolderToolStripMenuItem.Click += FSFolderToolStripMenuItem_Click;
            // 
            // aircraftEditorToolStripMenuItem
            // 
            aircraftEditorToolStripMenuItem.Name = "aircraftEditorToolStripMenuItem";
            aircraftEditorToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            aircraftEditorToolStripMenuItem.Text = "Aircraft Editor";
            aircraftEditorToolStripMenuItem.Click += aircraftEditorToolStripMenuItem_Click;
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            checkForUpdatesToolStripMenuItem.Click += checkForUpdatesToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // newFlightToolStripMenuItem
            // 
            newFlightToolStripMenuItem.Name = "newFlightToolStripMenuItem";
            newFlightToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            newFlightToolStripMenuItem.Text = "New Flight";
            newFlightToolStripMenuItem.Click += newFlightToolStripMenuItem_Click;
            // 
            // importFlightFileToolStripMenuItem
            // 
            importFlightFileToolStripMenuItem.Name = "importFlightFileToolStripMenuItem";
            importFlightFileToolStripMenuItem.Size = new System.Drawing.Size(128, 20);
            importFlightFileToolStripMenuItem.Text = "Import Flight or Plan";
            importFlightFileToolStripMenuItem.Click += importFlightFileToolStripMenuItem_Click;
            // 
            // utilitiesToolStripMenuItem
            // 
            utilitiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { destinationChooserToolStripMenuItem });
            utilitiesToolStripMenuItem.Name = "utilitiesToolStripMenuItem";
            utilitiesToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            utilitiesToolStripMenuItem.Text = "Utilities";
            // 
            // destinationChooserToolStripMenuItem
            // 
            destinationChooserToolStripMenuItem.Name = "destinationChooserToolStripMenuItem";
            destinationChooserToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            destinationChooserToolStripMenuItem.Text = "Destination Chooser";
            destinationChooserToolStripMenuItem.Click += destinationChooserToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { userManualToolStripMenuItem, changeLogToolStripMenuItem, aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // userManualToolStripMenuItem
            // 
            userManualToolStripMenuItem.Name = "userManualToolStripMenuItem";
            userManualToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            userManualToolStripMenuItem.Text = "User Manual";
            userManualToolStripMenuItem.Click += userManualToolStripMenuItem_Click;
            // 
            // changeLogToolStripMenuItem
            // 
            changeLogToolStripMenuItem.Name = "changeLogToolStripMenuItem";
            changeLogToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            changeLogToolStripMenuItem.Text = "Change Log";
            changeLogToolStripMenuItem.Click += changeLogToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 153);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(95, 15);
            label2.TabIndex = 14;
            label2.Text = "Flight Date/Time";
            // 
            // dtFlight
            // 
            dtFlight.CustomFormat = "MM/dd/yyyy hh:mm tt";
            dtFlight.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtFlight.Location = new System.Drawing.Point(168, 148);
            dtFlight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dtFlight.Name = "dtFlight";
            dtFlight.ShowUpDown = true;
            dtFlight.Size = new System.Drawing.Size(212, 23);
            dtFlight.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(6, 327);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(46, 15);
            label5.TabIndex = 16;
            label5.Text = "Aircraft";
            // 
            // cmbAircraft
            // 
            cmbAircraft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbAircraft.FormattingEnabled = true;
            cmbAircraft.Location = new System.Drawing.Point(155, 324);
            cmbAircraft.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbAircraft.Name = "cmbAircraft";
            cmbAircraft.Size = new System.Drawing.Size(292, 23);
            cmbAircraft.TabIndex = 15;
            cmbAircraft.SelectedValueChanged += cmbAircraft_SelectedValueChanged;
            // 
            // picPreview
            // 
            picPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            picPreview.Location = new System.Drawing.Point(455, 115);
            picPreview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            picPreview.Name = "picPreview";
            picPreview.Size = new System.Drawing.Size(385, 227);
            picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            picPreview.TabIndex = 18;
            picPreview.TabStop = false;
            // 
            // btnLaunchFS
            // 
            btnLaunchFS.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnLaunchFS.Enabled = false;
            btnLaunchFS.Location = new System.Drawing.Point(847, 278);
            btnLaunchFS.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnLaunchFS.Name = "btnLaunchFS";
            btnLaunchFS.Size = new System.Drawing.Size(122, 69);
            btnLaunchFS.TabIndex = 19;
            btnLaunchFS.Text = "Launch FS";
            btnLaunchFS.UseVisualStyleBackColor = true;
            btnLaunchFS.Click += btnLaunchFS_Click;
            // 
            // lblAltitude
            // 
            lblAltitude.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblAltitude.AutoSize = true;
            lblAltitude.Location = new System.Drawing.Point(755, 67);
            lblAltitude.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblAltitude.Name = "lblAltitude";
            lblAltitude.Size = new System.Drawing.Size(85, 15);
            lblAltitude.TabIndex = 23;
            lblAltitude.Text = "Cruise Altitude";
            // 
            // lblAirspeed
            // 
            lblAirspeed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblAirspeed.AutoSize = true;
            lblAirspeed.Location = new System.Drawing.Point(869, 67);
            lblAirspeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblAirspeed.Name = "lblAirspeed";
            lblAirspeed.Size = new System.Drawing.Size(102, 15);
            lblAirspeed.TabIndex = 24;
            lblAirspeed.Text = "Cruise Speed (Kts)";
            // 
            // txtAltitude
            // 
            txtAltitude.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            txtAltitude.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            txtAltitude.Location = new System.Drawing.Point(758, 85);
            txtAltitude.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtAltitude.Maximum = new decimal(new int[] { 75000, 0, 0, 0 });
            txtAltitude.Name = "txtAltitude";
            txtAltitude.Size = new System.Drawing.Size(107, 23);
            txtAltitude.TabIndex = 4;
            txtAltitude.ValueChanged += txtAltitude_ValueChanged;
            txtAltitude.Enter += txtAltitude_Enter;
            // 
            // txtSpeed
            // 
            txtSpeed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            txtSpeed.Location = new System.Drawing.Point(873, 85);
            txtSpeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtSpeed.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            txtSpeed.Name = "txtSpeed";
            txtSpeed.Size = new System.Drawing.Size(106, 23);
            txtSpeed.TabIndex = 5;
            txtSpeed.ValueChanged += txtSpeed_ValueChanged;
            txtSpeed.Enter += txtSpeed_Enter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(5, 67);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(38, 15);
            label1.TabIndex = 28;
            label1.Text = "Route";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(6, 37);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(83, 15);
            label6.TabIndex = 30;
            label6.Text = "SkyVector Link";
            ttSkyVector.SetToolTip(label6, resources.GetString("label6.ToolTip"));
            // 
            // fdFlight
            // 
            fdFlight.Filter = "Flight Files|*.fxml;*.flt|Flight Plans|*.pln";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(5, 355);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(81, 15);
            label7.TabIndex = 32;
            label7.Text = "Flight Briefing";
            // 
            // wbBriefing
            // 
            wbBriefing.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            wbBriefing.Location = new System.Drawing.Point(5, 373);
            wbBriefing.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            wbBriefing.MinimumSize = new System.Drawing.Size(23, 23);
            wbBriefing.Name = "wbBriefing";
            wbBriefing.Size = new System.Drawing.Size(841, 370);
            wbBriefing.TabIndex = 15;
            wbBriefing.TabStop = false;
            // 
            // chkSystem
            // 
            chkSystem.AutoSize = true;
            chkSystem.Location = new System.Drawing.Point(168, 177);
            chkSystem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkSystem.Name = "chkSystem";
            chkSystem.Size = new System.Drawing.Size(151, 19);
            chkSystem.TabIndex = 9;
            chkSystem.Text = "Always use system time";
            chkSystem.UseVisualStyleBackColor = true;
            chkSystem.CheckedChanged += chkSystem_CheckedChanged;
            // 
            // btnSystem
            // 
            btnSystem.Location = new System.Drawing.Point(388, 145);
            btnSystem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSystem.Name = "btnSystem";
            btnSystem.Size = new System.Drawing.Size(59, 27);
            btnSystem.TabIndex = 8;
            btnSystem.Text = "System";
            btnSystem.UseVisualStyleBackColor = true;
            btnSystem.Click += btnSystem_Click;
            // 
            // txtRoute
            // 
            txtRoute.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtRoute.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtRoute.DetectUrls = false;
            txtRoute.Location = new System.Drawing.Point(9, 85);
            txtRoute.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtRoute.Multiline = false;
            txtRoute.Name = "txtRoute";
            txtRoute.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            txtRoute.Size = new System.Drawing.Size(622, 24);
            txtRoute.TabIndex = 2;
            txtRoute.Text = "";
            txtRoute.TextChanged += txtRoute_TextChanged;
            txtRoute.KeyPress += txtRoute_KeyPress;
            txtRoute.Leave += txtRoute_Leave;
            // 
            // lblIFRConditions
            // 
            lblIFRConditions.AutoSize = true;
            lblIFRConditions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblIFRConditions.ForeColor = System.Drawing.Color.Red;
            lblIFRConditions.Location = new System.Drawing.Point(337, 300);
            lblIFRConditions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblIFRConditions.Name = "lblIFRConditions";
            lblIFRConditions.Size = new System.Drawing.Size(94, 13);
            lblIFRConditions.TabIndex = 33;
            lblIFRConditions.Text = "IFR Conditions!";
            lblIFRConditions.Visible = false;
            // 
            // cmbWeatherTypes
            // 
            cmbWeatherTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbWeatherTypes.Enabled = false;
            cmbWeatherTypes.FormattingEnabled = true;
            cmbWeatherTypes.Items.AddRange(new object[] { "Theme", "Real World (Static)", "Real World (15 Min.)" });
            cmbWeatherTypes.Location = new System.Drawing.Point(168, 204);
            cmbWeatherTypes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbWeatherTypes.Name = "cmbWeatherTypes";
            cmbWeatherTypes.Size = new System.Drawing.Size(279, 23);
            cmbWeatherTypes.TabIndex = 10;
            cmbWeatherTypes.SelectedIndexChanged += cmbWeatherTypes_SelectedIndexChanged;
            // 
            // cmbThemes
            // 
            cmbThemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbThemes.Enabled = false;
            cmbThemes.FormattingEnabled = true;
            cmbThemes.Location = new System.Drawing.Point(168, 235);
            cmbThemes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cmbThemes.Name = "cmbThemes";
            cmbThemes.Size = new System.Drawing.Size(279, 23);
            cmbThemes.TabIndex = 11;
            cmbThemes.SelectedIndexChanged += cmbThemes_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(5, 207);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(78, 15);
            label8.TabIndex = 36;
            label8.Text = "Weather Type";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(5, 238);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(90, 15);
            label9.TabIndex = 37;
            label9.Text = "Weather Theme";
            // 
            // chkIncludeTOC
            // 
            chkIncludeTOC.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            chkIncludeTOC.AutoSize = true;
            chkIncludeTOC.Location = new System.Drawing.Point(847, 144);
            chkIncludeTOC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkIncludeTOC.Name = "chkIncludeTOC";
            chkIncludeTOC.Size = new System.Drawing.Size(90, 19);
            chkIncludeTOC.TabIndex = 17;
            chkIncludeTOC.Text = "Include TOC";
            chkIncludeTOC.UseVisualStyleBackColor = true;
            chkIncludeTOC.Visible = false;
            chkIncludeTOC.CheckedChanged += chkIncludeTOC_CheckedChanged;
            // 
            // chkIncludeTOD
            // 
            chkIncludeTOD.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            chkIncludeTOD.AutoSize = true;
            chkIncludeTOD.Location = new System.Drawing.Point(847, 118);
            chkIncludeTOD.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkIncludeTOD.Name = "chkIncludeTOD";
            chkIncludeTOD.Size = new System.Drawing.Size(90, 19);
            chkIncludeTOD.TabIndex = 16;
            chkIncludeTOD.Text = "Include TOD";
            chkIncludeTOD.UseVisualStyleBackColor = true;
            chkIncludeTOD.CheckedChanged += chkIncludeTOD_CheckedChanged;
            // 
            // ttSkyVector
            // 
            ttSkyVector.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            // 
            // btnSkyVector
            // 
            btnSkyVector.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSkyVector.Location = new System.Drawing.Point(639, 83);
            btnSkyVector.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSkyVector.Name = "btnSkyVector";
            btnSkyVector.Size = new System.Drawing.Size(82, 27);
            btnSkyVector.TabIndex = 3;
            btnSkyVector.Text = "SkyVector";
            btnSkyVector.UseVisualStyleBackColor = true;
            btnSkyVector.Click += btnSkyVector_Click;
            // 
            // btnPrintBriefing
            // 
            btnPrintBriefing.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPrintBriefing.Enabled = false;
            btnPrintBriefing.Location = new System.Drawing.Point(847, 372);
            btnPrintBriefing.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnPrintBriefing.Name = "btnPrintBriefing";
            btnPrintBriefing.Size = new System.Drawing.Size(122, 69);
            btnPrintBriefing.TabIndex = 41;
            btnPrintBriefing.Text = "Print Briefing";
            btnPrintBriefing.UseVisualStyleBackColor = true;
            btnPrintBriefing.Click += btnPrintBriefing_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(983, 749);
            Controls.Add(btnPrintBriefing);
            Controls.Add(btnSkyVector);
            Controls.Add(chkIncludeTOD);
            Controls.Add(chkIncludeTOC);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(cmbThemes);
            Controls.Add(cmbWeatherTypes);
            Controls.Add(lblIFRConditions);
            Controls.Add(txtRoute);
            Controls.Add(btnSystem);
            Controls.Add(chkSystem);
            Controls.Add(wbBriefing);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label1);
            Controls.Add(txtSpeed);
            Controls.Add(txtAltitude);
            Controls.Add(lblAirspeed);
            Controls.Add(lblAltitude);
            Controls.Add(btnLaunchFS);
            Controls.Add(picPreview);
            Controls.Add(cmbAircraft);
            Controls.Add(label5);
            Controls.Add(dtFlight);
            Controls.Add(label2);
            Controls.Add(btnBuildPlan);
            Controls.Add(btnIFR);
            Controls.Add(btnVFR);
            Controls.Add(cmbRouteTypes);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(lblParking);
            Controls.Add(cmbParking);
            Controls.Add(btnParse);
            Controls.Add(txtSkyVector);
            Controls.Add(menuStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(999, 398);
            Name = "Main";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "FS Flight Builder";
            FormClosing += Main_FormClosing;
            Load += Main_Load;
            Shown += Main_Shown;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtAltitude).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtSpeed).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtSkyVector;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.ComboBox cmbParking;
        private System.Windows.Forms.Label lblParking;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbRouteTypes;
        private System.Windows.Forms.RadioButton btnVFR;
        private System.Windows.Forms.RadioButton btnIFR;
        private System.Windows.Forms.Button btnBuildPlan;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtFlight;
        private System.Windows.Forms.ToolStripMenuItem FSFolderToolStripMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbAircraft;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnLaunchFS;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateAircraftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullDatabaseUpdateToolStripMenuItem;
        private System.Windows.Forms.Label lblAltitude;
        private System.Windows.Forms.Label lblAirspeed;
        private System.Windows.Forms.NumericUpDown txtAltitude;
        private System.Windows.Forms.NumericUpDown txtSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem importFlightFileToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog fdFlight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.WebBrowser wbBriefing;
        private System.Windows.Forms.CheckBox chkSystem;
        private System.Windows.Forms.Button btnSystem;
        private System.Windows.Forms.ToolStripMenuItem newFlightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManualToolStripMenuItem;
        private System.Windows.Forms.RichTextBox txtRoute;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblIFRConditions;
        private System.Windows.Forms.ComboBox cmbWeatherTypes;
        private System.Windows.Forms.ComboBox cmbThemes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkIncludeTOC;
        private System.Windows.Forms.CheckBox chkIncludeTOD;
        private System.Windows.Forms.ToolStripMenuItem aircraftEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem utilitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem destinationChooserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeLogToolStripMenuItem;
        private System.Windows.Forms.ToolTip ttSkyVector;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnSkyVector;
        private System.Windows.Forms.Button btnPrintBriefing;
    }
}

