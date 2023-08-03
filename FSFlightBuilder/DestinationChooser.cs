//using atools.fs.utils;
using FSFlightbuilder;
using FSFlightBuilder.Components;
using FSFlightBuilder.Data.Models;
using FSFlightBuilder.Entities;
using FSFlightBuilder.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FSFlightBuilder
{
    internal partial class DestinationChooser : Form
    {
        public string FS_Path { get; set; }
        public string DataPath { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        private Airport _departure;
        private List<AirportData> gridAirportList = new List<AirportData>();
        private int selectedColumn = 3;

        public DestinationChooser()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (cbAirports.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(Departure) && !string.IsNullOrEmpty(Destination))
                {
                    if (((Button)sender).Name == "btnSkyVector")
                    {
                        Process.Start(new ProcessStartInfo($"https://skyvector.com/?fpl={Departure}%20{Destination}") { UseShellExecute = true });
                    }

                    //Save the settings
                    if (!cbSettings.Checked)
                    {
                        Properties.Settings.Default.ChooserSettings = string.Empty;
                        Properties.Settings.Default.Save();
                    }
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Please select a destination airport.");
                }
            }
            else
            {
                MessageBox.Show("Please select a departure airport.");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AirportChooser_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            new WaitDialog(RunSearch, "searching...", this);
        }

        private void RunSearch()
        {
            if (cbAirports.SelectedItem != null)
            {
                var dep = DataHelper.GetAirport(cbAirports.SelectedValue.ToString());
                var minDistance = Convert.ToInt32(numMinDistance.Value);
                var maxDistance = Convert.ToInt32(numMaxDistance.Value);
                var rwyLength = Convert.ToInt32(numMinRunLength.Value);
                var hasAvGas = chkAvGas.Checked;
                var hasjetFuel = chkJetFuel.Checked;
                var hasILS = chkILS.Checked;
                var hasTower = chkTower.Checked;
                var hasUncontrolled = chkUncontrolled.Checked;
                var hasHardOnly = chkHardOnly.Checked;
                var minRunway = chkMinRunLength.Checked;
                var isNautical = btnNauticalMiles.Checked;

                if (DataHelper.AllAirports == null || DataHelper.AllAirports.Count == 0)
                {
                    DataHelper.airports = DataHelper.GetAirports();
                }

                var apts = DataHelper.AllAirports.Where(a => a.AirportId != dep.AirportId)
                .Select(a => new AirportData()
                {
                    ICAO = a.AirportId,
                    Name = a.IcaoName,
                    Elevation = (int)(a.Elevation),
                    MaxRunLength = isNautical ? (int)(a.LongestRwyLength * 3.28084) : (int)(a.LongestRwyLength),
                    MaxRunWidth = isNautical ? (int)(a.LongestRwyWidth * 3.28084) : (int)(a.LongestRwyWidth),
                    Distance = GetDistance(isNautical, a.Latitude, a.Longitude, dep.Latitude, dep.Longitude),
                    HasHardRunway = a.Runways.Any(sfc => RunwayUtil.surfaceTypeToStr(sfc.Surface).ToUpper() == "BRICK" || RunwayUtil.surfaceTypeToStr(sfc.Surface).ToUpper() == "TARMAC" || RunwayUtil.surfaceTypeToStr(sfc.Surface).ToUpper() == "ASPHALT" || RunwayUtil.surfaceTypeToStr(sfc.Surface).ToUpper() == "CONCRETE"),
                    HasILS = a.Runways.Any(rwy => !string.IsNullOrEmpty(rwy.IlsFrequency)),
                    Towered = a.HasTower == 1,
                    HasAvGas = a.HasAvGas == 1,
                    HasJetFuel = a.HasJetFuel == 1
                }).ToList();

                if (numMinDistance.Value > 0)
                {
                    apts = apts.Where(apt => apt.Distance >= (double)numMinDistance.Value).ToList();
                }
                if (numMaxDistance.Value > 0)
                {
                    apts = apts.Where(apt => apt.Distance <= (double)numMaxDistance.Value).ToList();
                }

                if (chkAvGas.Checked)
                {
                    apts = apts.Where(apt => apt.HasAvGas == true).ToList();
                }

                if (chkJetFuel.Checked)
                {
                    apts = apts.Where(apt => apt.HasJetFuel == true).ToList();
                }

                if (chkILS.Checked)
                {
                    apts = apts.Where(apt => apt.HasILS == true).ToList();
                }

                if (chkTower.Checked && !chkUncontrolled.Checked)
                {
                    apts = apts.Where(apt => apt.Towered == true).ToList();
                }

                if (chkUncontrolled.Checked && !chkTower.Checked)
                {
                    apts = apts.Where(apt => apt.Towered == false).ToList();
                }

                if (chkHardOnly.Checked)
                {
                    apts = apts.Where(apt => apt.HasHardRunway == true).ToList();
                }

                gridAirportList = apts.OrderBy(a => a.Distance).ToList();

                if (gridAirports.InvokeRequired)
                {
                    gridAirports.Invoke(new MethodInvoker(() => gridAirports.DataSource = gridAirportList));
                }
                else
                {
                    gridAirports.DataSource = gridAirportList;
                }
                gridAirports.Columns[3].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                if (btnNauticalMiles.Checked)
                {
                    gridAirports.Columns[3].HeaderCell.Value = "Distance (NM)";
                    gridAirports.Columns[4].HeaderCell.Value = "Max Runway Length (ft)";
                    gridAirports.Columns[5].HeaderCell.Value = "Max Runway Width (ft)";
                }
                else
                {
                    gridAirports.Columns[3].HeaderCell.Value = "Distance (KM)";
                    gridAirports.Columns[4].HeaderCell.Value = "Max Runway Length (m)";
                    gridAirports.Columns[5].HeaderCell.Value = "Max Runway Width (m)";
                }

                //Save the settings
                if (cbSettings.Checked)
                {
                    //save the selected airport, and other settings to the ini
                    Properties.Settings.Default.ChooserSettings = BuildSettings();
                }
                else
                {
                    Properties.Settings.Default.ChooserSettings = string.Empty;
                }
                Properties.Settings.Default.Save();
            }
        }

        private void chkMinRunLength_CheckedChanged(object sender, EventArgs e)
        {
            numMinRunLength.Enabled = chkMinRunLength.Checked;
        }

        private void UpdateAirportInfo(string apt)
        {
            var _airportInfo = CreateAirportInfo(apt);
            var txtWriter = new StreamWriter(DataPath + @"\dep.htm");
            txtWriter.Write(_airportInfo);
            txtWriter.Close();
            Departure = apt;

            wbDepAirportInfo.Navigate(DataPath.Replace("\\\\", "\\") + @"\dep.htm");
        }

        private string CreateAirportInfo(string icao)
        {
            _departure = DataHelper.GetAirport(icao);
            //Briefing Html
            var helper = new ResourceHelpers();
            var _briefing = helper.GetResourceTextFile("AirportInfo.htm");
            if (_departure != null)
            {
                _briefing = Regex.Replace(_briefing,
                    "!BRIEFING TITLE!",
                    "(" + _departure.AirportId + ") " + _departure.IcaoName);
                var txt = new StringBuilder();
                txt.Append("Elevation: " + _departure.Elevation + " ft" + Environment.NewLine);
                var lighting = new StringBuilder();
                var num = 0;
                var aptRunways = _departure.Runways.OrderBy(r => r.Number);
                foreach (var runway in aptRunways)
                {
                    //Check for L, R, C
                    var rwy = runway.Number.Replace("L", "").Replace("R", "").Replace("C", "");
                    var rwyInt = AWDConvert.ToInt32(rwy);
                    if (rwyInt < 19 && rwyInt != num)
                    {
                        txt.Append("Runways " + runway.Number + "/" + (rwyInt + 18) + ": " + runway.Surface +
                                   ", Dimensions " + Convert.ToInt32(runway.Length * 3.281) + " x " + Convert.ToInt32(runway.Width * 3.281) + " feet" +
                                   " / " + runway.Length + " x " + runway.Width + " meters" + Environment.NewLine);
                    }
                }
                foreach (var runway in aptRunways)
                {
                    txt.Append("Runway " + runway.Number + ": Heading " + AWDConvert.ToDecimal(runway.Heading) +
                               "&#176;, Pattern: " + runway.PatternLanding + Environment.NewLine);
                    if (!string.IsNullOrEmpty(runway.IlsFrequency))
                    {
                        txt.Append("\tILS Frequency:" + runway.IlsFrequency + ", Heading: " +
                                   Convert.ToInt32(runway.IlsHeading) + "&#176;" +
                                   Environment.NewLine);
                    }

                    if (runway.ApproachLights != "NONE" ||
                        (!string.IsNullOrEmpty(runway.Glideslope) && runway.Glideslope != "NONE" && runway.Glideslope != "0"))
                    {
                        lighting.Append("Runway " + runway.Number + ": Approach " + runway.ApproachLights +
                                        ", Glideslope " + runway.Glideslope + Environment.NewLine);
                    }
                }
                _briefing = Regex.Replace(_briefing, "!INFO!", txt.ToString());

                //Comms
                txt = new StringBuilder();

                foreach (var comm in _departure.Comms)
                {
                    if (!string.IsNullOrEmpty(comm.Frequency))
                    {
                        var commName = !string.IsNullOrEmpty(comm.Name) ? ", " + comm.Name : string.Empty;
                        txt.Append((CommType)comm.Type + commName + ", " + comm.Frequency + " MHz" +
                                   Environment.NewLine);
                    }
                }
                _briefing = Regex.Replace(_briefing, "!COMMS!", txt.ToString());

                _briefing = Regex.Replace(_briefing, "!RUNWAYLIGHTING!",
                    lighting.ToString());

                //Weather
                try
                {
                    var weather = Common.UpdateWeather(_departure.AirportId);
                    _briefing = Regex.Replace(_briefing, "!METAR!", "WEATHER:" + Environment.NewLine + weather);
                    return _briefing;
                }
                catch
                {
                    MessageBox.Show("Could not download the weather information.  Please check your internet connection.");
                }
            }

            return Regex.Replace(_briefing, "!METAR!", "METAR Not Found.");
        }

        private string BuildSettings()
        {
            var sb = new StringBuilder();
            if (btnNauticalMiles.Checked)
            {
                sb.Append("Feet~");
            }
            else
            {
                sb.Append("Meters~");
            }
            //sb.Append(cbAirports.ActiveRow.Cells[0].Value + "~");
            sb.Append(cbAirports.SelectedValue + "~");
            sb.Append(numMinDistance.Value + "~");
            sb.Append(numMaxDistance.Value + "~");
            sb.Append(chkTower.Checked + "~");
            sb.Append(chkUncontrolled.Checked + "~");
            sb.Append(chkILS.Checked + "~");
            sb.Append(chkAvGas.Checked + "~");
            sb.Append(chkHardOnly.Checked + "~");
            sb.Append(chkMinRunLength.Checked + "~");
            sb.Append(numMinRunLength.Value + "~");
            sb.Append(chkJetFuel.Checked);

            return sb.ToString();
        }

        private void btnNauticalMiles_CheckedChanged(object sender, EventArgs e)
        {
            var lbl = btnNauticalMiles.Checked ? "NM" : "KM";
            lblMinDist.Text = lblMaxDist.Text = lbl;
        }

        private void btnKilometers_CheckedChanged(object sender, EventArgs e)
        {
            var lbl = btnNauticalMiles.Checked ? "NM" : "KM";
            lblMinDist.Text = lblMaxDist.Text = lbl;
        }

        private void DestinationChooser_Shown(object sender, EventArgs e)
        {
            this.Refresh();

            //Read the settings
            var chooserSettings = Properties.Settings.Default.ChooserSettings;
            if (!string.IsNullOrEmpty(chooserSettings))
            {
                string[] settings = chooserSettings.Split('~');
                if (settings.Length > 10)
                {
                    if (settings[0] == "Feet")
                    {
                        btnNauticalMiles.Select();
                        lblMinDist.Text = lblMaxDist.Text = "NM";
                    }
                    else
                    {
                        btnKilometers.Select();
                        lblMinDist.Text = lblMaxDist.Text = "KM";
                    }
                    //cbAirports.SelectedText = settings[1];
                    cbAirports.SelectedValue = settings[1];
                    numMinDistance.Value = Convert.ToDecimal(settings[2]);
                    numMaxDistance.Value = Convert.ToDecimal(settings[3]);
                    chkTower.Checked = Convert.ToBoolean(settings[4]);
                    chkUncontrolled.Checked = Convert.ToBoolean(settings[5]);
                    chkILS.Checked = Convert.ToBoolean(settings[6]);
                    chkAvGas.Checked = Convert.ToBoolean(settings[7]);
                    chkHardOnly.Checked = Convert.ToBoolean(settings[8]);
                    chkMinRunLength.Checked = Convert.ToBoolean(settings[9]);
                    numMinRunLength.Value = Convert.ToDecimal(settings[10]);
                    if (settings.Length == 12)
                    {
                        chkJetFuel.Checked = Convert.ToBoolean(settings[11]);
                    }
                    cbSettings.Checked = true;
                    Departure = settings[1]; // airports.FirstOrDefault(a => a.AirportId == settings[1]);
                }
            }
            var frm = new WaitDialog(LoadData, "Reading airport data...", this);

            if (Common.FlightSim == FlightSimType.MSFS)
            {
                //Disable the hard runways option for MSFS
                chkHardOnly.Checked = false;
                chkHardOnly.Enabled = false;
                toolTip1.SetToolTip(chkHardOnly, "Runway surfaces are not currently available in Microsoft Flight Simulator");
                toolTip1.Active = true;
            }
            else
            {
                toolTip1.RemoveAll();
            }

            cbAirports.Select();

            if (!string.IsNullOrEmpty(Departure))
            {
                cbAirports.SelectedValue = Departure;
                UpdateAirportInfo(Departure);
            }
            cbAirports.Visible = true;
        }

        private void LoadData()
        {
            List<Airport> airports;
            if (DataHelper.airports == null || DataHelper.airports.Count == 0)
            {
                airports = DataHelper.GetAirports();
            }
            else
            {
                airports = DataHelper.airports;
            }
            var apts = airports.Select(a => new AirportInfo()
            {
                ICAO = a.AirportId,
                Name = a.IcaoName + " (" + a.AirportId + ")",
                Elevation = (int)(a.Elevation),
            }).OrderBy(o => o.Name);
            departureBindingSource.DataSource = apts;
        }

        private double GetDistance(bool isNautical, double apLatitude, double apLongitude, double depLatitude, double depLongitude)
        {
            double res = 0;
            if (isNautical)
            {
                res = Math.Round(6367 * Math.Acos(Math.Sin(DegreeToRadian(depLatitude)) * (Math.Sin(DegreeToRadian(apLatitude))) + ((Math.Cos(DegreeToRadian(depLatitude))) * (Math.Cos(DegreeToRadian(apLatitude))) * (Math.Cos((DegreeToRadian(apLongitude) - DegreeToRadian(depLongitude)))))) * 0.54, 1);
            }
            else
            {
                res = Math.Round(6367 * Math.Acos(Math.Sin(DegreeToRadian(depLatitude)) * (Math.Sin(DegreeToRadian(apLatitude))) + ((Math.Cos(DegreeToRadian(depLatitude))) * (Math.Cos(DegreeToRadian(apLatitude))) * (Math.Cos((DegreeToRadian(apLongitude) - DegreeToRadian(depLongitude)))))), 1);
            }
            return res;

        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private void numMinDistance_Enter(object sender, EventArgs e)
        {
            numMinDistance.Select(0, numMinDistance.Text.Length);
        }

        private void numMaxDistance_Enter(object sender, EventArgs e)
        {
            numMaxDistance.Select(0, numMaxDistance.Text.Length);
        }

        private void numMinRunLength_Enter(object sender, EventArgs e)
        {
            numMinRunLength.Select(0, numMinRunLength.Text.Length);
        }

        private void gridAirports_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = gridAirports.Columns[e.ColumnIndex];
            DataGridViewColumn oldColumn = selectedColumn > -1 ? gridAirports.Columns[selectedColumn] : null;
            ListSortDirection direction;

            if (oldColumn == newColumn)
            {
                // Sorting by the same column: toggle between ASC and DESC:
                direction = gridAirports.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }
            else
            {
                // Sorting by a new column.
                direction = ListSortDirection.Descending;
                selectedColumn = e.ColumnIndex;
                // Remove the sorting glyph from the old column, if any:
                if (oldColumn != null)
                {
                    oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }

            switch (e.ColumnIndex)
            {
                case 0: //ICAO
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.ICAO).ToList() : gridAirportList.OrderBy(a => a.ICAO).ToList();
                    break;
                case 1: //Name
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.Name).ToList() : gridAirportList.OrderBy(a => a.Name).ToList();
                    break;
                case 2: //Elevation
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.Elevation).ToList() : gridAirportList.OrderBy(a => a.Elevation).ToList();
                    break;
                case 3: //Distance
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.Distance).ToList() : gridAirportList.OrderBy(a => a.Distance).ToList();
                    break;
                case 4: //MaxRunLength
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.MaxRunLength).ToList() : gridAirportList.OrderBy(a => a.MaxRunLength).ToList();
                    break;
                case 5: //MaxRunWidth
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.MaxRunWidth).ToList() : gridAirportList.OrderBy(a => a.MaxRunWidth).ToList();
                    break;
                case 6: //Towered
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.Towered).ToList() : gridAirportList.OrderBy(a => a.Towered).ToList();
                    break;
                case 7: //HardRunways
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.HasHardRunway).ToList() : gridAirportList.OrderBy(a => a.HasHardRunway).ToList();
                    break;
                case 8: //HasILS
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.HasILS).ToList() : gridAirportList.OrderBy(a => a.HasILS).ToList();
                    break;
                case 9: //HasAvGas
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.HasAvGas).ToList() : gridAirportList.OrderBy(a => a.HasAvGas).ToList();
                    break;
                case 10: //HasJetFuel
                    gridAirportList = direction == ListSortDirection.Ascending ? gridAirportList.OrderByDescending(a => a.HasJetFuel).ToList() : gridAirportList.OrderBy(a => a.HasJetFuel).ToList();
                    break;
            }

            gridAirports.DataSource = gridAirportList; // gridBindingSource;

            gridAirports.Refresh();

            newColumn.HeaderCell.SortGlyphDirection = direction == ListSortDirection.Ascending
                ? SortOrder.Ascending
                : SortOrder.Descending;
        }

        private void gridAirports_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                UseWaitCursor = true;
                var apt = DataHelper.GetAirport(gridAirports.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (apt != null)
                {
                    if (apt.AirportId != Destination)
                    {
                        var _airportInfo = CreateAirportInfo(apt.AirportId);
                        var txtWriter = new StreamWriter(DataPath + @"\dest.htm");
                        txtWriter.Write(_airportInfo);
                        txtWriter.Close();

                        Destination = apt.AirportId;

                        wbDestAirportInfo.Navigate(DataPath.Replace("\\\\", "\\") + @"\dest.htm");
                        tabAirportInfo.SelectedTab = tabAirportInfo.TabPages[1]; // .Tabs[1].Selected = true;
                    }
                }
                UseWaitCursor = false;
            }
        }

        private void gridAirports_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (gridAirports.SelectedRows.Count > 0)
            {
                UseWaitCursor = true;
                var apt = DataHelper.GetAirport(gridAirports.SelectedRows[0].Cells[0].Value.ToString());
                if (apt != null)
                {
                    var _airportInfo = CreateAirportInfo(apt.AirportId);
                    var txtWriter = new StreamWriter(DataPath + @"\dest.htm");
                    txtWriter.Write(_airportInfo);
                    txtWriter.Close();

                    Destination = apt.AirportId;

                    wbDestAirportInfo.Navigate(DataPath.Replace("\\\\", "\\") + @"\dest.htm");
                    tabAirportInfo.SelectedTab = tabAirportInfo.TabPages[1]; // .Tabs[1].Selected = true;
                }
                UseWaitCursor = false;
            }
        }

        private void cbAirports_Leave(object sender, EventArgs e)
        {
            SelectDeparture();
        }

        private void cbAirports_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectDeparture();
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            SelectDeparture();
        }

        private void SelectDeparture()
        {
            if (cbAirports.SelectedIndex > -1)
            {
                UseWaitCursor = true;
                var apt = DataHelper.GetAirport(cbAirports.SelectedValue.ToString());
                if (apt != null)
                {
                    UpdateAirportInfo(apt.AirportId);
                    gridBindingSource.DataSource = new List<AirportData>();
                    //gridAirports..DataBind();
                    wbDestAirportInfo.Navigate("about:blank");
                }
                UseWaitCursor = false;
            }
        }
    }

}
