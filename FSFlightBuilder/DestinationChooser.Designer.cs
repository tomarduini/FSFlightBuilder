namespace FSFlightBuilder
{
    partial class DestinationChooser
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            wbDepAirportInfo = new System.Windows.Forms.WebBrowser();
            wbDestAirportInfo = new System.Windows.Forms.WebBrowser();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            btnGo = new System.Windows.Forms.Button();
            cbAirports = new AWDControls.SearchableComboBox(components);
            departureBindingSource = new System.Windows.Forms.BindingSource(components);
            lblMaxDist = new System.Windows.Forms.Label();
            lblMinDist = new System.Windows.Forms.Label();
            cbSettings = new System.Windows.Forms.CheckBox();
            btnKilometers = new System.Windows.Forms.RadioButton();
            btnNauticalMiles = new System.Windows.Forms.RadioButton();
            btnSearch = new System.Windows.Forms.Button();
            numMaxDistance = new System.Windows.Forms.NumericUpDown();
            numMinDistance = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            gbOptions = new System.Windows.Forms.GroupBox();
            chkJetFuel = new System.Windows.Forms.CheckBox();
            numMinRunLength = new System.Windows.Forms.NumericUpDown();
            chkMinRunLength = new System.Windows.Forms.CheckBox();
            chkAvGas = new System.Windows.Forms.CheckBox();
            chkUncontrolled = new System.Windows.Forms.CheckBox();
            chkILS = new System.Windows.Forms.CheckBox();
            chkHardOnly = new System.Windows.Forms.CheckBox();
            chkTower = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            lblDeparture = new System.Windows.Forms.Label();
            gridAirports = new System.Windows.Forms.DataGridView();
            iCAODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            elevationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            distanceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            maxRunLengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            maxRunWidthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            toweredDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            hasHardRunwayDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            hasILSDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            hasAvGasDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            hasJetFuelDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            parkingsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            commsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            gridBindingSource = new System.Windows.Forms.BindingSource(components);
            btnCancel = new System.Windows.Forms.Button();
            btnSelect = new System.Windows.Forms.Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            btnSkyVector = new System.Windows.Forms.Button();
            airportBindingSource = new System.Windows.Forms.BindingSource(components);
            tabAirportInfo = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            tabPage2 = new System.Windows.Forms.TabPage();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)departureBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxDistance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMinDistance).BeginInit();
            gbOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMinRunLength).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridAirports).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)airportBindingSource).BeginInit();
            tabAirportInfo.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // wbDepAirportInfo
            // 
            wbDepAirportInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            wbDepAirportInfo.Location = new System.Drawing.Point(3, 3);
            wbDepAirportInfo.Margin = new System.Windows.Forms.Padding(2);
            wbDepAirportInfo.MinimumSize = new System.Drawing.Size(15, 16);
            wbDepAirportInfo.Name = "wbDepAirportInfo";
            wbDepAirportInfo.Size = new System.Drawing.Size(848, 178);
            wbDepAirportInfo.TabIndex = 1;
            // 
            // wbDestAirportInfo
            // 
            wbDestAirportInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            wbDestAirportInfo.Location = new System.Drawing.Point(3, 3);
            wbDestAirportInfo.Margin = new System.Windows.Forms.Padding(2);
            wbDestAirportInfo.MinimumSize = new System.Drawing.Size(15, 16);
            wbDestAirportInfo.Name = "wbDestAirportInfo";
            wbDestAirportInfo.Size = new System.Drawing.Size(848, 178);
            wbDestAirportInfo.TabIndex = 1;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(btnGo);
            splitContainer1.Panel1.Controls.Add(cbAirports);
            splitContainer1.Panel1.Controls.Add(lblMaxDist);
            splitContainer1.Panel1.Controls.Add(lblMinDist);
            splitContainer1.Panel1.Controls.Add(cbSettings);
            splitContainer1.Panel1.Controls.Add(btnKilometers);
            splitContainer1.Panel1.Controls.Add(btnNauticalMiles);
            splitContainer1.Panel1.Controls.Add(btnSearch);
            splitContainer1.Panel1.Controls.Add(numMaxDistance);
            splitContainer1.Panel1.Controls.Add(numMinDistance);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(gbOptions);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(label3);
            splitContainer1.Panel1.Controls.Add(lblDeparture);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(gridAirports);
            splitContainer1.Size = new System.Drawing.Size(886, 293);
            splitContainer1.SplitterDistance = 322;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // btnGo
            // 
            btnGo.Location = new System.Drawing.Point(287, 39);
            btnGo.Name = "btnGo";
            btnGo.Size = new System.Drawing.Size(34, 23);
            btnGo.TabIndex = 17;
            btnGo.Text = "Go";
            btnGo.UseVisualStyleBackColor = true;
            btnGo.Click += btnGo_Click;
            // 
            // cbAirports
            // 
            cbAirports.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            cbAirports.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cbAirports.DataSource = departureBindingSource;
            cbAirports.DisplayMember = "Name";
            cbAirports.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbAirports.FormattingEnabled = true;
            cbAirports.Location = new System.Drawing.Point(110, 39);
            cbAirports.Name = "cbAirports";
            cbAirports.Size = new System.Drawing.Size(171, 23);
            cbAirports.TabIndex = 16;
            cbAirports.ValueMember = "ICAO";
            cbAirports.KeyDown += cbAirports_KeyDown;
            cbAirports.Leave += cbAirports_Leave;
            // 
            // departureBindingSource
            // 
            departureBindingSource.DataSource = typeof(Entities.AirportInfo);
            // 
            // lblMaxDist
            // 
            lblMaxDist.AutoSize = true;
            lblMaxDist.Location = new System.Drawing.Point(254, 98);
            lblMaxDist.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMaxDist.Name = "lblMaxDist";
            lblMaxDist.Size = new System.Drawing.Size(27, 15);
            lblMaxDist.TabIndex = 15;
            lblMaxDist.Text = "NM";
            // 
            // lblMinDist
            // 
            lblMinDist.AutoSize = true;
            lblMinDist.Location = new System.Drawing.Point(254, 68);
            lblMinDist.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMinDist.Name = "lblMinDist";
            lblMinDist.Size = new System.Drawing.Size(27, 15);
            lblMinDist.TabIndex = 15;
            lblMinDist.Text = "NM";
            // 
            // cbSettings
            // 
            cbSettings.AutoSize = true;
            cbSettings.Location = new System.Drawing.Point(16, 239);
            cbSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cbSettings.Name = "cbSettings";
            cbSettings.Size = new System.Drawing.Size(148, 19);
            cbSettings.TabIndex = 13;
            cbSettings.Text = "Remember my settings";
            cbSettings.UseVisualStyleBackColor = true;
            // 
            // btnKilometers
            // 
            btnKilometers.AutoSize = true;
            btnKilometers.Location = new System.Drawing.Point(225, 14);
            btnKilometers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnKilometers.Name = "btnKilometers";
            btnKilometers.Size = new System.Drawing.Size(81, 19);
            btnKilometers.TabIndex = 2;
            btnKilometers.Text = "Kilometers";
            btnKilometers.UseVisualStyleBackColor = true;
            btnKilometers.CheckedChanged += btnKilometers_CheckedChanged;
            // 
            // btnNauticalMiles
            // 
            btnNauticalMiles.AutoSize = true;
            btnNauticalMiles.Checked = true;
            btnNauticalMiles.Location = new System.Drawing.Point(118, 14);
            btnNauticalMiles.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnNauticalMiles.Name = "btnNauticalMiles";
            btnNauticalMiles.Size = new System.Drawing.Size(100, 19);
            btnNauticalMiles.TabIndex = 1;
            btnNauticalMiles.TabStop = true;
            btnNauticalMiles.Text = "Nautical Miles";
            btnNauticalMiles.UseVisualStyleBackColor = true;
            btnNauticalMiles.CheckedChanged += btnNauticalMiles_CheckedChanged;
            // 
            // btnSearch
            // 
            btnSearch.Location = new System.Drawing.Point(230, 238);
            btnSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(88, 27);
            btnSearch.TabIndex = 14;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // numMaxDistance
            // 
            numMaxDistance.Location = new System.Drawing.Point(110, 96);
            numMaxDistance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numMaxDistance.Maximum = new decimal(new int[] { 1316134911, 2328, 0, 0 });
            numMaxDistance.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxDistance.Name = "numMaxDistance";
            numMaxDistance.Size = new System.Drawing.Size(138, 23);
            numMaxDistance.TabIndex = 5;
            numMaxDistance.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxDistance.Enter += numMaxDistance_Enter;
            // 
            // numMinDistance
            // 
            numMinDistance.Location = new System.Drawing.Point(110, 66);
            numMinDistance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numMinDistance.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            numMinDistance.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMinDistance.Name = "numMinDistance";
            numMinDistance.Size = new System.Drawing.Size(138, 23);
            numMinDistance.TabIndex = 4;
            numMinDistance.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numMinDistance.Enter += numMinDistance_Enter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(13, 97);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 15);
            label2.TabIndex = 1;
            label2.Text = "Max Distance";
            // 
            // gbOptions
            // 
            gbOptions.Controls.Add(chkJetFuel);
            gbOptions.Controls.Add(numMinRunLength);
            gbOptions.Controls.Add(chkMinRunLength);
            gbOptions.Controls.Add(chkAvGas);
            gbOptions.Controls.Add(chkUncontrolled);
            gbOptions.Controls.Add(chkILS);
            gbOptions.Controls.Add(chkHardOnly);
            gbOptions.Controls.Add(chkTower);
            gbOptions.Location = new System.Drawing.Point(16, 123);
            gbOptions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbOptions.Name = "gbOptions";
            gbOptions.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbOptions.Size = new System.Drawing.Size(301, 107);
            gbOptions.TabIndex = 6;
            gbOptions.TabStop = false;
            gbOptions.Text = "Destination Options";
            // 
            // chkJetFuel
            // 
            chkJetFuel.AutoSize = true;
            chkJetFuel.Location = new System.Drawing.Point(79, 46);
            chkJetFuel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkJetFuel.Name = "chkJetFuel";
            chkJetFuel.Size = new System.Drawing.Size(65, 19);
            chkJetFuel.TabIndex = 13;
            chkJetFuel.Text = "Jet Fuel";
            chkJetFuel.UseVisualStyleBackColor = true;
            // 
            // numMinRunLength
            // 
            numMinRunLength.Enabled = false;
            numMinRunLength.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numMinRunLength.Location = new System.Drawing.Point(179, 71);
            numMinRunLength.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            numMinRunLength.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            numMinRunLength.Name = "numMinRunLength";
            numMinRunLength.Size = new System.Drawing.Size(100, 23);
            numMinRunLength.TabIndex = 12;
            numMinRunLength.Enter += numMinRunLength_Enter;
            // 
            // chkMinRunLength
            // 
            chkMinRunLength.AutoSize = true;
            chkMinRunLength.Location = new System.Drawing.Point(7, 75);
            chkMinRunLength.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkMinRunLength.Name = "chkMinRunLength";
            chkMinRunLength.Size = new System.Drawing.Size(164, 19);
            chkMinRunLength.TabIndex = 11;
            chkMinRunLength.Text = "Minimum Runway Length";
            chkMinRunLength.UseVisualStyleBackColor = true;
            chkMinRunLength.CheckedChanged += chkMinRunLength_CheckedChanged;
            // 
            // chkAvGas
            // 
            chkAvGas.AutoSize = true;
            chkAvGas.Location = new System.Drawing.Point(7, 46);
            chkAvGas.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkAvGas.Name = "chkAvGas";
            chkAvGas.Size = new System.Drawing.Size(62, 19);
            chkAvGas.TabIndex = 9;
            chkAvGas.Text = "Av Gas";
            chkAvGas.UseVisualStyleBackColor = true;
            // 
            // chkUncontrolled
            // 
            chkUncontrolled.AutoSize = true;
            chkUncontrolled.Location = new System.Drawing.Point(158, 22);
            chkUncontrolled.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkUncontrolled.Name = "chkUncontrolled";
            chkUncontrolled.Size = new System.Drawing.Size(95, 19);
            chkUncontrolled.TabIndex = 7;
            chkUncontrolled.Text = "Uncontrolled";
            chkUncontrolled.UseVisualStyleBackColor = true;
            // 
            // chkILS
            // 
            chkILS.AutoSize = true;
            chkILS.Location = new System.Drawing.Point(79, 22);
            chkILS.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkILS.Name = "chkILS";
            chkILS.Size = new System.Drawing.Size(41, 19);
            chkILS.TabIndex = 8;
            chkILS.Text = "ILS";
            chkILS.UseVisualStyleBackColor = true;
            // 
            // chkHardOnly
            // 
            chkHardOnly.AutoSize = true;
            chkHardOnly.Location = new System.Drawing.Point(158, 46);
            chkHardOnly.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkHardOnly.Name = "chkHardOnly";
            chkHardOnly.Size = new System.Drawing.Size(99, 19);
            chkHardOnly.TabIndex = 10;
            chkHardOnly.Text = "Hard Surfaces";
            toolTip1.SetToolTip(chkHardOnly, "Show airports with hard surfaces");
            chkHardOnly.UseVisualStyleBackColor = true;
            // 
            // chkTower
            // 
            chkTower.AutoSize = true;
            chkTower.Location = new System.Drawing.Point(7, 22);
            chkTower.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkTower.Name = "chkTower";
            chkTower.Size = new System.Drawing.Size(57, 19);
            chkTower.TabIndex = 6;
            chkTower.Text = "Tower";
            chkTower.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(13, 67);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(76, 15);
            label1.TabIndex = 1;
            label1.Text = "Min Distance";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(13, 16);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(82, 15);
            label3.TabIndex = 1;
            label3.Text = "Distance Units";
            // 
            // lblDeparture
            // 
            lblDeparture.AutoSize = true;
            lblDeparture.Location = new System.Drawing.Point(13, 39);
            lblDeparture.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblDeparture.Name = "lblDeparture";
            lblDeparture.Size = new System.Drawing.Size(90, 15);
            lblDeparture.TabIndex = 1;
            lblDeparture.Text = "Departure ICAO";
            // 
            // gridAirports
            // 
            gridAirports.AllowUserToAddRows = false;
            gridAirports.AllowUserToDeleteRows = false;
            gridAirports.AllowUserToOrderColumns = true;
            gridAirports.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            gridAirports.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            gridAirports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridAirports.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { iCAODataGridViewTextBoxColumn, nameDataGridViewTextBoxColumn, elevationDataGridViewTextBoxColumn, distanceDataGridViewTextBoxColumn, maxRunLengthDataGridViewTextBoxColumn, maxRunWidthDataGridViewTextBoxColumn, toweredDataGridViewCheckBoxColumn, hasHardRunwayDataGridViewCheckBoxColumn, hasILSDataGridViewCheckBoxColumn, hasAvGasDataGridViewCheckBoxColumn, hasJetFuelDataGridViewCheckBoxColumn, parkingsDataGridViewTextBoxColumn, commsDataGridViewTextBoxColumn });
            gridAirports.DataSource = gridBindingSource;
            gridAirports.Dock = System.Windows.Forms.DockStyle.Fill;
            gridAirports.Location = new System.Drawing.Point(0, 0);
            gridAirports.Name = "gridAirports";
            gridAirports.ReadOnly = true;
            gridAirports.RowTemplate.Height = 25;
            gridAirports.Size = new System.Drawing.Size(559, 293);
            gridAirports.TabIndex = 0;
            gridAirports.CellClick += gridAirports_CellClick;
            gridAirports.ColumnHeaderMouseClick += gridAirports_ColumnHeaderMouseClick;
            gridAirports.RowEnter += gridAirports_RowEnter;
            // 
            // iCAODataGridViewTextBoxColumn
            // 
            iCAODataGridViewTextBoxColumn.DataPropertyName = "ICAO";
            iCAODataGridViewTextBoxColumn.HeaderText = "ICAO";
            iCAODataGridViewTextBoxColumn.Name = "iCAODataGridViewTextBoxColumn";
            iCAODataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            nameDataGridViewTextBoxColumn.HeaderText = "Name";
            nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // elevationDataGridViewTextBoxColumn
            // 
            elevationDataGridViewTextBoxColumn.DataPropertyName = "Elevation";
            elevationDataGridViewTextBoxColumn.HeaderText = "Elevation";
            elevationDataGridViewTextBoxColumn.Name = "elevationDataGridViewTextBoxColumn";
            elevationDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // distanceDataGridViewTextBoxColumn
            // 
            distanceDataGridViewTextBoxColumn.DataPropertyName = "Distance";
            distanceDataGridViewTextBoxColumn.HeaderText = "Distance";
            distanceDataGridViewTextBoxColumn.Name = "distanceDataGridViewTextBoxColumn";
            distanceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // maxRunLengthDataGridViewTextBoxColumn
            // 
            maxRunLengthDataGridViewTextBoxColumn.DataPropertyName = "MaxRunLength";
            maxRunLengthDataGridViewTextBoxColumn.HeaderText = "MaxRunLength";
            maxRunLengthDataGridViewTextBoxColumn.Name = "maxRunLengthDataGridViewTextBoxColumn";
            maxRunLengthDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // maxRunWidthDataGridViewTextBoxColumn
            // 
            maxRunWidthDataGridViewTextBoxColumn.DataPropertyName = "MaxRunWidth";
            maxRunWidthDataGridViewTextBoxColumn.HeaderText = "MaxRunWidth";
            maxRunWidthDataGridViewTextBoxColumn.Name = "maxRunWidthDataGridViewTextBoxColumn";
            maxRunWidthDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // toweredDataGridViewCheckBoxColumn
            // 
            toweredDataGridViewCheckBoxColumn.DataPropertyName = "Towered";
            toweredDataGridViewCheckBoxColumn.HeaderText = "Towered";
            toweredDataGridViewCheckBoxColumn.Name = "toweredDataGridViewCheckBoxColumn";
            toweredDataGridViewCheckBoxColumn.ReadOnly = true;
            toweredDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // hasHardRunwayDataGridViewCheckBoxColumn
            // 
            hasHardRunwayDataGridViewCheckBoxColumn.DataPropertyName = "HasHardRunway";
            hasHardRunwayDataGridViewCheckBoxColumn.HeaderText = "Hard Runways";
            hasHardRunwayDataGridViewCheckBoxColumn.Name = "hasHardRunwayDataGridViewCheckBoxColumn";
            hasHardRunwayDataGridViewCheckBoxColumn.ReadOnly = true;
            hasHardRunwayDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // hasILSDataGridViewCheckBoxColumn
            // 
            hasILSDataGridViewCheckBoxColumn.DataPropertyName = "HasILS";
            hasILSDataGridViewCheckBoxColumn.HeaderText = "Has ILS";
            hasILSDataGridViewCheckBoxColumn.Name = "hasILSDataGridViewCheckBoxColumn";
            hasILSDataGridViewCheckBoxColumn.ReadOnly = true;
            hasILSDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // hasAvGasDataGridViewCheckBoxColumn
            // 
            hasAvGasDataGridViewCheckBoxColumn.DataPropertyName = "HasAvGas";
            hasAvGasDataGridViewCheckBoxColumn.HeaderText = "Has Av Gas";
            hasAvGasDataGridViewCheckBoxColumn.Name = "hasAvGasDataGridViewCheckBoxColumn";
            hasAvGasDataGridViewCheckBoxColumn.ReadOnly = true;
            hasAvGasDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // hasJetFuelDataGridViewCheckBoxColumn
            // 
            hasJetFuelDataGridViewCheckBoxColumn.DataPropertyName = "HasJetFuel";
            hasJetFuelDataGridViewCheckBoxColumn.HeaderText = "Has Jet Fuel";
            hasJetFuelDataGridViewCheckBoxColumn.Name = "hasJetFuelDataGridViewCheckBoxColumn";
            hasJetFuelDataGridViewCheckBoxColumn.ReadOnly = true;
            hasJetFuelDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // parkingsDataGridViewTextBoxColumn
            // 
            parkingsDataGridViewTextBoxColumn.DataPropertyName = "Parkings";
            parkingsDataGridViewTextBoxColumn.HeaderText = "Parkings";
            parkingsDataGridViewTextBoxColumn.Name = "parkingsDataGridViewTextBoxColumn";
            parkingsDataGridViewTextBoxColumn.ReadOnly = true;
            parkingsDataGridViewTextBoxColumn.Visible = false;
            // 
            // commsDataGridViewTextBoxColumn
            // 
            commsDataGridViewTextBoxColumn.DataPropertyName = "Comms";
            commsDataGridViewTextBoxColumn.HeaderText = "Comms";
            commsDataGridViewTextBoxColumn.Name = "commsDataGridViewTextBoxColumn";
            commsDataGridViewTextBoxColumn.ReadOnly = true;
            commsDataGridViewTextBoxColumn.Visible = false;
            // 
            // gridBindingSource
            // 
            gridBindingSource.AllowNew = false;
            gridBindingSource.DataSource = typeof(Entities.AirportData);
            gridBindingSource.Sort = "";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Location = new System.Drawing.Point(784, 517);
            btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(88, 27);
            btnCancel.TabIndex = 19;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnClose_Click;
            // 
            // btnSelect
            // 
            btnSelect.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSelect.Location = new System.Drawing.Point(481, 517);
            btnSelect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new System.Drawing.Size(88, 27);
            btnSelect.TabIndex = 17;
            btnSelect.Text = "Select";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnSkyVector
            // 
            btnSkyVector.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSkyVector.Location = new System.Drawing.Point(575, 517);
            btnSkyVector.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSkyVector.Name = "btnSkyVector";
            btnSkyVector.Size = new System.Drawing.Size(198, 27);
            btnSkyVector.TabIndex = 18;
            btnSkyVector.Text = "Select && Launch SkyVector";
            btnSkyVector.UseVisualStyleBackColor = true;
            btnSkyVector.Click += btnSelect_Click;
            // 
            // airportBindingSource
            // 
            airportBindingSource.DataSource = typeof(Data.Models.Airport);
            // 
            // tabAirportInfo
            // 
            tabAirportInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabAirportInfo.Controls.Add(tabPage1);
            tabAirportInfo.Controls.Add(tabPage2);
            tabAirportInfo.Location = new System.Drawing.Point(12, 299);
            tabAirportInfo.Name = "tabAirportInfo";
            tabAirportInfo.SelectedIndex = 0;
            tabAirportInfo.Size = new System.Drawing.Size(862, 212);
            tabAirportInfo.TabIndex = 20;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(wbDepAirportInfo);
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(854, 184);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Departure Airport";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(wbDestAirportInfo);
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(854, 184);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Destination Airport";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // DestinationChooser
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ClientSize = new System.Drawing.Size(886, 547);
            Controls.Add(tabAirportInfo);
            Controls.Add(btnSkyVector);
            Controls.Add(btnSelect);
            Controls.Add(btnCancel);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DestinationChooser";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Destination Chooser";
            Load += AirportChooser_Load;
            Shown += DestinationChooser_Shown;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)departureBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxDistance).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMinDistance).EndInit();
            gbOptions.ResumeLayout(false);
            gbOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMinRunLength).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridAirports).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)airportBindingSource).EndInit();
            tabAirportInfo.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDeparture;
        private System.Windows.Forms.RadioButton btnKilometers;
        private System.Windows.Forms.RadioButton btnNauticalMiles;
        private System.Windows.Forms.NumericUpDown numMaxDistance;
        private System.Windows.Forms.NumericUpDown numMinDistance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.NumericUpDown numMinRunLength;
        private System.Windows.Forms.CheckBox chkMinRunLength;
        private System.Windows.Forms.CheckBox chkAvGas;
        private System.Windows.Forms.CheckBox chkUncontrolled;
        private System.Windows.Forms.CheckBox chkILS;
        private System.Windows.Forms.CheckBox chkTower;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.BindingSource gridBindingSource;
        private System.Windows.Forms.BindingSource airportBindingSource;
        private System.Windows.Forms.WebBrowser wbDepAirportInfo;
        private System.Windows.Forms.WebBrowser wbDestAirportInfo;
        private System.Windows.Forms.BindingSource departureBindingSource;
        private System.Windows.Forms.CheckBox chkHardOnly;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnSkyVector;
        private System.Windows.Forms.CheckBox cbSettings;
        private System.Windows.Forms.Label lblMaxDist;
        private System.Windows.Forms.Label lblMinDist;
        private AWDControls.SearchableComboBox cbAirports;
        private System.Windows.Forms.DataGridView gridAirports;
        private System.Windows.Forms.DataGridViewTextBoxColumn iCAODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn elevationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn distanceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxRunLengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxRunWidthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn toweredDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hasHardRunwayDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hasAvGasDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hasJetFuelDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hasILSDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parkingsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commsDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabControl tabAirportInfo;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.CheckBox chkJetFuel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}