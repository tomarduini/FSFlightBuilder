using FSFlightBuilder.Components;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
//using System.Windows.Controls;
using System.Windows.Forms;
//using System.Windows.Shapes;

namespace FSFlightBuilder
{
    public partial class FrmOptions : Form
    {
        private string initValue = string.Empty;
        public FrmOptions()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            cmbFS.Items.Clear();

            TextBox fsFolder;
            TextBox fsCustomFolder = null;
            TextBox fpFolder;
            TextBox airplanesFolder;
            TabPage tab;
            string cmbText;
            foreach (var sim in Common.flightSimPaths)
            {
                switch (sim.Key)
                {
                    case FlightSimType.FSXSE:
                        fsFolder = txtFSXSEPath;
                        fpFolder = txtFSXSEFlightPlanFolder;
                        airplanesFolder = txtFSXSEAirplanesFolder;
                        tab = tabFSXSE;
                        cmbText = "FSX Steam Edition";
                        break;
                    case FlightSimType.P3D:
                        fsFolder = txtP3DPath;
                        fsCustomFolder = txtP3DCustomPath;
                        fpFolder = txtP3DFlightPlanFolder;
                        airplanesFolder = txtP3DAirplanesFolder;
                        tab = tabP3D;
                        cmbText = "Prepar3D";
                        break;
                    case FlightSimType.MSFS:
                        fsFolder = txtMSFSFolder;
                        fsCustomFolder = txtMSFSCustomFolder;
                        fpFolder = txtMSFSFlightPlanFolder;
                        airplanesFolder = txtMSFSAirplanesFolder;
                        tab = tabMSFS;
                        cmbText = "Microsoft Flight Simulator";
                        break;
                    default:
                        fsFolder = txtFSXPath;
                        fpFolder = txtFSXFlightPlanFolder;
                        airplanesFolder = txtFSXAirplanesFolder;
                        tab = tabFSX;
                        cmbText = "FSX";
                        break;
                }

                var isInstalled = sim.Value.Installed;
                fsFolder.Text = sim.Value.DefaultFSPath;
                if (!isInstalled)
                {
                    tabFolders.TabPages.Remove(tab);
                }
                else
                {
                    fpFolder.Text = sim.Value.FPPath;
                    airplanesFolder.Text = sim.Value.AirplanesPath;
                    var fsOk = true;
                    for (var i = 0; i < 4; i++)
                    {
                        string path = string.Empty;
                        TextBox ctrl = null;
                        switch (i)
                        {
                            case 0:
                                path = fsFolder.Text;
                                ctrl = fsFolder;
                                break;
                            case 1:
                                path = fpFolder.Text;
                                ctrl = fpFolder;
                                break;
                            case 2:
                                path = airplanesFolder.Text;
                                ctrl = airplanesFolder;
                                break;
                            case 3:
                                if (fsCustomFolder != null)
                                {
                                    fsCustomFolder.Text = sim.Value.CustomFSPath;
                                    path = fsCustomFolder.Text;
                                    ctrl = fsCustomFolder;
                                }
                                break;
                        }

                        if (ctrl != null && !Directory.Exists(path))
                        {
                            ctrl.BackColor = Color.Red;
                            ctrl.ForeColor = Color.White;
                            toolTip1.SetToolTip(ctrl, "Path not found.");
                            if (i != 1)
                            {
                                fsOk = false;
                            }
                            //var parts = tabFSX.Controls;
                            tab.Text = $"{sim.Key} Folders (Missing Data)";
                            toolTip1.SetToolTip(tab, "Missing Data");
                        }
                    }

                    if (fsOk)
                    {
                        cmbFS.Items.Add(cmbText);
                    }
                }
            }

            UpdateCombo();
            UpdateChartOptions();
            chkDeleteImages.Checked = Properties.Settings.Default.DeleteImagesOnClose;
            chkUpdates.Checked = Properties.Settings.Default.UpdateOnStart;
            btnCancel.Focus();
            cmbFS.Enabled = !Common.DataUpdateInProgress;
        }

        private void UpdateChartOptions()
        {
            var chartService = Properties.Settings.Default.Service;
            if (!string.IsNullOrEmpty(chartService))
            {
                switch (chartService)
                {
                    case "FAA":
                        rdoFAACharts.Checked = true;
                        break;
                    case "AVIATIONAPI":
                        rdoAviationAPI.Checked = true;
                        break;
                    default:
                        rdoAuto.Checked = true;
                        break;
                }
            }
        }

        private void btnChooseFSX_Click(object sender, EventArgs e)
        {

        }

        private void btnChooseFSXFP_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtFSXFlightPlanFolder.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtFSXFlightPlanFolder.Text = fbDialog.SelectedPath;
            }
        }

        private void btnChooseFSXSE_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtFSXSEPath.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtFSXSEPath.Text = fbDialog.SelectedPath;
                UpdateCombo();
                if (File.Exists(txtFSXSEPath.Text + @"\fsx.exe"))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(txtFSXSEPath.Text + @"\fsx.exe");
                    var idx = versionInfo.ProductVersion.IndexOf(" ", StringComparison.Ordinal);
                    Common.fsxAppVersion = idx > -1
                        ? versionInfo.ProductVersion.Substring(0, idx)
                        : versionInfo.ProductVersion;
                }
            }
        }

        private void btnChooseFSXSEFP_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtFSXSEFlightPlanFolder.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtFSXSEFlightPlanFolder.Text = fbDialog.SelectedPath;
            }
        }

        private void btnChooseP3D_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtP3DPath.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtP3DPath.Text = fbDialog.SelectedPath;
                UpdateCombo();
                if (File.Exists(txtP3DPath.Text + @"\Prepar3D.exe"))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(txtP3DPath.Text + @"\Prepar3D.exe");
                    var idx = versionInfo.ProductVersion.IndexOf(" ", StringComparison.Ordinal);
                    Common.p3dAppVersion = idx > -1
                        ? versionInfo.ProductVersion.Substring(0, idx)
                        : versionInfo.ProductVersion;
                }
            }
        }

        private void btnChooseP3DFP_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtP3DFlightPlanFolder.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtP3DFlightPlanFolder.Text = fbDialog.SelectedPath;
            }
        }

        private void btnChooseP3DAirplanes_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtP3DAirplanesFolder.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtP3DAirplanesFolder.Text = fbDialog.SelectedPath;
            }
        }

        private void btnChooseFSXSEAirplanes_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtFSXSEAirplanesFolder.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtFSXSEAirplanesFolder.Text = fbDialog.SelectedPath;
            }
        }

        private void btnChooseFSXAirplane_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtFSXAirplanesFolder.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtFSXAirplanesFolder.Text = fbDialog.SelectedPath;
            }
        }

        private void btnChooseMSFSClient_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtMSFSFolder.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtMSFSFolder.Text = fbDialog.SelectedPath;
            }

        }

        private void btnChooseMSFSFP_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtMSFSFlightPlanFolder.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtMSFSFlightPlanFolder.Text = fbDialog.SelectedPath;
            }
        }

        private void btnChooseMSFSAirplane_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtMSFSAirplanesFolder.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtMSFSAirplanesFolder.Text = fbDialog.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbFS.Text))
            {
                MessageBox.Show(@"Please select a Flight Simulator");
                return;
            }
            if (!string.IsNullOrEmpty(txtFSXFlightPlanFolder.Text) && txtFSXFlightPlanFolder.Text.ToLower().EndsWith("missions"))
            {
                MessageBox.Show(@"Flight plans are not allowed to be saved in the root ""Missions"" folder for FSX.  Please select a sub-folder.");
                return;
            }
            else if (!string.IsNullOrEmpty(txtFSXSEFlightPlanFolder.Text) && txtFSXSEFlightPlanFolder.Text.ToLower().EndsWith("missions"))
            {
                MessageBox.Show(@"Flight plans are not allowed to be saved in the root ""Missions"" folder for FSX SE.  Please select a sub-folder.");
                return;
            }

            TextBox ctrl = null;
            string sim = string.Empty;
            for (var i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0: //FSX
                        ctrl = txtFSXFlightPlanFolder;
                        sim = "FSX Missions Folder";
                        break;
                    case 1: //FSXSE
                        ctrl = txtFSXSEFlightPlanFolder;
                        sim = "FSX SE Missions Folder";
                        break;
                    case 2: //P3D
                        ctrl = txtP3DFlightPlanFolder;
                        sim = "Prepar3D Flight Plan Folder";
                        break;
                    case 3: //MSFS
                        ctrl = txtMSFSFlightPlanFolder;
                        sim = "MSFS Flight Plan Folder";
                        break;
                }
                if (!string.IsNullOrEmpty(ctrl.Text) && !Directory.Exists(ctrl.Text))
                {
                    ctrl.BackColor = Color.Red;
                    ctrl.ForeColor = Color.White;
                    toolTip1.SetToolTip(ctrl, "Path not found.");
                    var result = MessageBox.Show("The " + sim + " was not found. Would you like me to create it?", "Folder Not Found", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.Yes:
                            Directory.CreateDirectory(ctrl.Text);
                            if (Directory.Exists(ctrl.Text))
                            {
                                ctrl.BackColor = Color.White;
                                ctrl.ForeColor = Color.Black;
                                toolTip1.SetToolTip(ctrl, "");
                            }
                            break;
                        case DialogResult.No:
                            break;
                        default:
                            return;
                    }

                }
            }
            Properties.Settings.Default.FSXDefaultFSPath = txtFSXPath.Text;
            Properties.Settings.Default.FSXFPPath = txtFSXFlightPlanFolder.Text;
            Properties.Settings.Default.FSXAirplanesPath = txtFSXAirplanesFolder.Text;
            Properties.Settings.Default.FSXSEDefaultFSPath = txtFSXSEPath.Text;
            Properties.Settings.Default.FSXSEFPPath = txtFSXSEFlightPlanFolder.Text;
            Properties.Settings.Default.FSXSEAirplanesPath = txtFSXSEAirplanesFolder.Text;
            Properties.Settings.Default.P3DDefaultFSPath = txtP3DPath.Text;
            Properties.Settings.Default.P3DCustomFSPath = txtP3DCustomPath.Text;
            Properties.Settings.Default.P3DFPPath = txtP3DFlightPlanFolder.Text;
            Properties.Settings.Default.P3DAirplanesPath = txtP3DAirplanesFolder.Text;
            Properties.Settings.Default.MSFSDefaultFSPath = txtMSFSFolder.Text;
            Properties.Settings.Default.MSFSCustomFSPath = txtMSFSCustomFolder.Text;
            Properties.Settings.Default.MSFSFPPath = txtMSFSFlightPlanFolder.Text;
            Properties.Settings.Default.MSFSAirplanesPath = txtMSFSAirplanesFolder.Text;
            Properties.Settings.Default.FSXVersion = Common.fsxAppVersion;
            Properties.Settings.Default.FSXSEVersion = Common.fsxSEAppVersion;
            Properties.Settings.Default.P3DVersion = Common.p3dAppVersion;
            Properties.Settings.Default.MSFSVersion = Common.msfsAppVersion;
            if (rdoFAACharts.Checked)
            {
                Properties.Settings.Default.Service = "FAA";
            }
            else
            {
                Properties.Settings.Default.Service = "AVIATIONAPI";
            }

            FlightSimType fs = FlightSimType.Unknown;
            switch (cmbFS.Text)
            {
                case "FSX":
                    fs = FlightSimType.FSX;
                    break;
                case "FSX Steam Edition":
                    fs = FlightSimType.FSXSE;
                    break;
                case "Prepar3D":
                    fs = FlightSimType.P3D;
                    break;
                case "Microsoft Flight Simulator":
                    fs = FlightSimType.MSFS;
                    break;
            }
            if (fs != Common.FlightSim)
            {
                Common.PriorFlightSim = Common.FlightSim;
                MessageBox.Show(
                    @"Changing the Flight Simulator may also require an update to the application databases.  To perform an update, select ""File:Database:Update Full database"" from the menu.");
                Properties.Settings.Default.FlightSim = fs.ToString();
                Common.FlightSim = fs;
                Common.CheckPaths();
            }
            Properties.Settings.Default.DeleteImagesOnClose = chkDeleteImages.Checked;
            Properties.Settings.Default.UpdateOnStart = chkUpdates.Checked;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
        }

        private void UpdateCombo()
        {
            var val = cmbFS.Text;
            if (cmbFS.Items.Count == 1)
            {
                cmbFS.SelectedIndex = 0;
                cmbFS.Enabled = false;
            }
            else if (!string.IsNullOrEmpty(val))
            {
                cmbFS.Text = val;
            }
            else
            {
                switch (Common.FlightSim)
                {
                    case FlightSimType.MSFS:
                        cmbFS.Text = @"Microsoft Flight Simulator";
                        break;
                    case FlightSimType.P3D:
                        cmbFS.Text = @"Prepar3D";
                        break;
                    case FlightSimType.FSXSE:
                        cmbFS.Text = @"FSX Steam Edition";
                        break;
                    case FlightSimType.FSX:
                        cmbFS.Text = @"FSX";
                        break;
                }
            }
        }

        private void lnkFindFS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Common.CheckInstalls();
            InitializeForm();
        }

        private void btnChooseCustomP3D_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtP3DCustomPath.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtP3DCustomPath.Text = fbDialog.SelectedPath;
            }
        }

        private void btnChooseMSFSCustom_Click(object sender, EventArgs e)
        {
            fbDialog.RootFolder = Environment.SpecialFolder.Desktop;
            fbDialog.SelectedPath = txtP3DCustomPath.Text;
            DialogResult result = fbDialog.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fbDialog.SelectedPath))
            {
                txtMSFSCustomFolder.Text = fbDialog.SelectedPath;
            }
        }
    }
}
