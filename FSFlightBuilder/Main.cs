using AutoUpdaterDotNET;
using FSFlightBuilder.Components;
using FSFlightBuilder.Data.Models;
using FSFlightBuilder.Entities;
using FSFlightBuilder.Enums;
using MsgBox;
using RTB_ToolTip;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace FSFlightBuilder
{
    public partial class Main : Form
    {
        public const string updaterPrefix = "AWD_";

        private List<Airport> _airports = new List<Airport>();
        private List<Waypoint> _waypoints = new List<Waypoint>();
        private List<Comm> _comms = new List<Comm>();
        private List<Navaid> _navaids = new List<Navaid>();
        private List<Parking> _parkings = new List<Parking>();
        private List<Aircraft> _aircraft = new List<Aircraft>();
        private readonly Dictionary<string, string> _airways = new Dictionary<string, string>();
        private string _info = string.Empty;
        private List<string> _routepoints = new List<string>();
        private List<RoutePoint> _points = new List<RoutePoint>();
        private Airport _departure;
        private Airport _destination;
        private string appPath;
        private string _fsPath;
        private string _fileName = string.Empty;
        private string _briefing;
        private string _path;
        private bool _isIFR;
        private BGLImporter bglImporter;
        private RichTextBoxToolTip rtb;
        public static List<string> updateInfo = new List<string>();
        private bool updatesFromMenu;
        private int routeBearing = 0;

        public Main()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.SetData("DataDirectory", Common.DataPath);

            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            if (!isElevated)
            {
                MessageBox.Show(@"FS Flight Builder must be run with administrator privileges.  This can be accomplished by right-clicking on the desktop shortcut, select properties, select the Compatibility tab, select the 'Run this program as an administrator' checkbox.", "Administrator Privileges Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;

            try
            {
                if (File.Exists($"{Common.DataPath}\\navdata.db"))
                {
                    File.Delete($"{Common.DataPath}\\navdata.db");
                }
            }
            catch { }

            SimStartup();
        }

        private void UpdateButtons()
        {
            if (string.IsNullOrEmpty(Common.ChartService))
            {
                Common.ChartService = "AVIATIONAPI";
                Properties.Settings.Default.Service = "AVIATIONAPI";
                Properties.Settings.Default.Save();
            }

            SetFlightSim(Common.FlightSim);
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSkyVector.Text))
            {
                MessageBox.Show(@"Please paste the ""Link"" value from SkyVector.com and try again.",
                    @"No Input from SkyVector", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var txt = txtSkyVector.Text;
            ClearData();
            txtSkyVector.Text = txt;

            //    '* Skyvector: https://skyvector.com/?ll=40.38975664582873,-84.10212706924007+chart=301+zoom=3+fpl=N0161F195%20KUCP%20ACO%20MFD%20ROD%20KPLD
            //    '* fpl: N = Airspeed, F = flight level %20 = space
            //Make sure route starts and ends at an airport
            // Search input string for ampersand character, split and exit on 1st occurence
            //replace %20 with space
            var svInput = txtSkyVector.Text.Replace("%20", " ");
            //https://skyvector.com/?ll=40.38975664582873,-84.10212706924007+chart=301+zoom=3+fpl=N0161F195 KUCP ACO MFD ROD KPLD
            var amps = svInput.Split('&');
            //https://skyvector.com/?ll=40.38975664582873,-84.10212706924007   chart=301   zoom=3   fpl=N0161F195 KUCP ACO MFD ROD KPLD
            if (amps.Length < 4)
            {
            }

            var equals = amps[0].Split('=');
            if (equals.Length < 2)
            {
            }

            if (amps.Length > 3 && amps[1].Contains("chart") && amps[2].Contains("zoom"))
            {
                var tmp = amps[3].Split('=');
                if (tmp.Length > 1)
                {
                    _info = tmp[1];
                }
            }

            try
            {
                if (!string.IsNullOrEmpty(_info))
                {
                    _info = _info.Replace("undefined ", "");
                    var tmp = _info.Split(' ');
                    if (tmp.Length < 3)
                    {
                    }
                    var idx = tmp[0].IndexOf('F');
                    if (idx > -1)
                    {
                        txtSpeed.Value = tmp[0].Contains("N") ? AWDConvert.ToDecimal(tmp[0].Substring(1, idx - 1)) : 0;
                        txtAltitude.Value = AWDConvert.ToDecimal(tmp[0].Substring(idx + 1));
                        for (var j = 1; j < tmp.Length; j++)
                        {
                            _routepoints.Add(tmp[j]);
                        }
                    }
                    else
                    {
                        idx = tmp[0].IndexOf('A');
                        if (idx > -1)
                        {
                            txtSpeed.Value = tmp[0].Contains("N") ? AWDConvert.ToDecimal(tmp[0].Substring(1, idx - 1)) : 0;
                            txtAltitude.Value = AWDConvert.ToDecimal(tmp[0].Substring(idx + 1));
                            for (var j = 1; j < tmp.Length; j++)
                            {
                                _routepoints.Add(tmp[j].ToUpper());
                            }
                        }
                        else
                        {
                            idx = tmp[0].IndexOf('N');
                            if (idx > -1)
                            {
                                txtSpeed.Value = AWDConvert.ToDecimal(tmp[0].Substring(1)); //, idx - 1));
                                                                                            //                            txtAltitude.Value = AWDConvert.ToDecimal(tmp[0].Substring(idx + 1));
                                for (var j = 1; j < tmp.Length; j++)
                                {
                                    _routepoints.Add(tmp[j].ToUpper());
                                }
                            }
                            else
                            {
                                for (var j = 1; j < tmp.Length; j++)
                                {
                                    _routepoints.Add(tmp[j].ToUpper());
                                }
                            }
                        }
                    }
                    txtAltitude.Value = txtAltitude.Value * 100;
                }

                if (BuildRoute())
                {

                    var route = new StringBuilder();
                    foreach (var routepoint in _routepoints)
                    {
                        if (route.Length > 0)
                        {
                            route.Append(" ");
                        }
                        route.Append(routepoint);
                    }
                    txtRoute.Text = route.ToString();

                    ResetParking();
                    UpdateWeather();
                    if (_routepoints.Count == 2 && cmbRouteTypes.Items.Count > 0)
                    {
                        cmbRouteTypes.SelectedIndex = 0;
                    }
                }
            }
            catch
            {

            }
        }

        private async void btnBuildPlan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRoute.Text))
            {
                MessageBox.Show(
                    @"Please enter a route for your flight.",
                    @"Missing Route Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtRoute.Text = txtRoute.Text.Replace("TOD ", "");
            var parts = txtRoute.Text.Split(' ');
            if (_departure == null || _departure.AirportId != _routepoints[0])
            {
                _departure = DataHelper.GetAirport(_routepoints[0].Trim());
            }

            if (_departure == null)
            {
                MessageBox.Show(
                    @"Please check the departure ICAO for your flight.  " + _routepoints[0] +
                    @" was not found in the database." + Environment.NewLine + Environment.NewLine +
                    @"If it was added recently, you may need to update the database.",
                    @"Missing Departure Airport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_destination == null || _destination.AirportId != _routepoints[_routepoints.Count - 1])
            {
                _destination = DataHelper.GetAirport(_routepoints[_routepoints.Count - 1].Trim());
            }

            if (_destination == null)
            {
                MessageBox.Show(
                    @"Please check the destination ICAO for your flight.  " + _routepoints[_routepoints.Count - 1] +
                    @" was not found in the database." + Environment.NewLine + Environment.NewLine +
                    @"If it was added recently, you may need to update the database.",
                    @"Missing Destination Airport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtAltitude.Value == 0 || txtSpeed.Value == 0)
            {
                if (
                    MessageBox.Show(
                        @"Missing information found.  Please confirm that your altitude, speed, and starting position are set." +
                        Environment.NewLine + @"Do you wish to continue?",
                        @"Missing Information Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                    DialogResult.Yes)
                {
                    return;
                }
            }

            if (cmbParking.SelectedItem == null)
            {
                MessageBox.Show(
                    @"Please select a starting position.",
                    @"Missing Information Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbWeatherTypes.SelectedIndex == -1 && Common.FlightSim != FlightSimType.MSFS)
            {
                MessageBox.Show(
                    @"Please select a weather type.",
                    @"Missing Information Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbWeatherTypes.SelectedIndex == 0 && cmbThemes.SelectedIndex == -1)
            {
                MessageBox.Show(
                    @"You have selected ""Theme"" for the Weather Type, but have not selected a theme.",
                    @"Missing Information Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbRouteTypes.SelectedIndex == -1)
            {
                MessageBox.Show(
                    @"Please select a Route Type for your flight.",
                    @"Missing Route Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbAircraft.SelectedIndex == -1)
            {
                MessageBox.Show(
                    @"Please select an aircraft for your flight.",
                    @"Missing Route Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var acft = _aircraft.FirstOrDefault(a => a.Name == ((FSFlightBuilder.Data.Models.Aircraft)cmbAircraft.SelectedItem).Name);
            if (acft != null)
            {
                if (chkIncludeTOC.Checked && (acft.ClimbRate == 0 || acft.ClimbSpeed == 0))
                {
                    MessageBox.Show(
                        @"The selected aircraft does not have the Climb Rate or Climb Speed set.  Edit the aircaft using the File:Aircraft Editor menu option, or uncheck the ""Include TOC"" checkbox.",
                        @"Missing Aircraft Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (chkIncludeTOD.Checked && (AWDConvert.ToInt32(acft.DescentRate) == 0 || AWDConvert.ToInt32(acft.DescentSpeed) == 0))
                {
                    if (MessageBox.Show(
                        @"The selected aircraft does not have the Descent Rate or Descent Speed set.  Would you like to edit the selected aircaft's descent information?",
                        @"Cannot Calculate TOD", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        var frm = new AircraftEditor(_aircraft);
                        frm.DataPath = Common.DataPath;
                        frm.FS_Path = _fsPath;
                        frm.SelectedAircraft = acft;
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            _aircraft = await DataHelper.GetAircraft();
                            acft = frm.SelectedAircraft;
                            if (chkIncludeTOD.Checked && (acft.DescentRate == 0 || acft.DescentSpeed == 0))
                            {
                                MessageBox.Show(
                                    @"The selected aircraft does not have the Descent Rate or Descent Speed set.  Please use the File: Aircraft Editor menu option enter the information, or uncheck the ""Include TOD"" checkbox.",
                                    @"Cannot Calculate TOD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            GetDepDestData();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true
            };

            var fpName = !string.IsNullOrEmpty(_fileName)
                ? _fileName
                : (btnVFR.Checked ? "VFR" : "IFR") + " " + _departure.AirportId + " to " + _destination.AirportId;
            var ctls = new List<ControlTypes>(); // {InputBox.Type.TextBox, InputBox.Type.ComboBox};
            var tpe = new ControlTypes { Label = new Label { Text = @"Flight Name: " }, Type = InputBox.Type.TextBox };
            ctls.Add(tpe);
            tpe = new ControlTypes { Label = new Label { Text = @"Flight Type: " }, Type = InputBox.Type.ComboBox };
            ctls.Add(tpe);
            var cmbItems = new List<string> { Common.FlightSim.ToString().StartsWith("FSX") ? "FSX (flt)" : Common.FlightSim.ToString().StartsWith("MS") ? "MSFS (pln)" : "Prepar3D (fxml)" };
            var defaultValue = Common.FlightSim.ToString().StartsWith("FSX")
                ? "FSX (flt)"
                : Common.FlightSim.ToString().StartsWith("MS")
                ? "MSFS (pln)" : "Prepar3D (fxml)";

            //Set the FP Path
            string fpPath = Common.flightSimPaths[Common.FlightSim].FPPath;
            if (
                InputBox.ShowDialog("Please name your flight", "Name Your Flight", InputBox.Icon.Information,
                    InputBox.Buttons.OkCancel, ctls, fpName, defaultValue, cmbItems,
                    fpPath) == DialogResult.OK)
            {

                var fileParts = InputBox.ResultValue.Split('~');
                _fileName = fileParts[0];

                var selectedCharts = new List<Chart>();
                using (var form = new ChartChooser(_departure.AirportId, _destination.AirportId, Common.DataPath.TrimEnd('\\') + @"\Routes\" + _fileName + ".awd", Common.DataPath, Common.ChartService, _departure.Country == "United States" && _destination.Country == "United States"))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        selectedCharts = form.selectedCharts;
                    }
                }

                using (new WaitCursor())
                {
                    await GenerateBriefing(fileParts, xmlWriterSettings, acft, selectedCharts);
                    GetDepDestData();
                }
            }
        }

        private async Task<Action> GenerateBriefing(string[] fileParts, XmlWriterSettings xmlWriterSettings, Aircraft acft, List<Chart> selectedCharts)
        {
            try
            {

                wbBriefing.Navigate("about:blank");

                var flightType = fileParts[1];
                string path = string.Empty;
                switch (flightType)
                {
                    case "FSX (flt)":
                        path = Common.FlightSim == FlightSimType.FSXSE ? Common.flightSimPaths[FlightSimType.FSXSE].FPPath : Common.flightSimPaths[FlightSimType.FSX].FPPath;
                        break;
                    case "Prepar3D (fxml)":
                        path = Common.flightSimPaths[FlightSimType.P3D].FPPath;
                        break;
                    case "MSFS (pln)":
                        path = Common.flightSimPaths[FlightSimType.MSFS].FPPath;
                        break;
                }

                if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                {
                    //If not, set to the "My Documents" folder
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                using (var writer = XmlWriter.Create(path + @"\" + _fileName + ".pln", xmlWriterSettings))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("SimBase.Document");
                    writer.WriteAttributeString("Type", "AceXML");
                    writer.WriteAttributeString("version", "3,4");
                    writer.WriteStartElement("Descr");
                    writer.WriteString("AceXML Document");
                    writer.WriteEndElement();

                    writer.WriteStartElement("FlightPlan.FlightPlan");

                    writer.WriteStartElement("Title");
                    writer.WriteString(btnVFR.Checked
                        ? "VFR " + _departure.IcaoName + " to " + _destination.IcaoName
                        : "IFR " + _departure.IcaoName + " to " + _destination.IcaoName);
                    writer.WriteEndElement();
                    writer.WriteStartElement("FPType");
                    writer.WriteString(btnVFR.Checked ? "VFR" : "IFR");
                    writer.WriteEndElement();
                    if (cmbRouteTypes.SelectedIndex > 0)
                    {
                        writer.WriteStartElement("RouteType");
                        writer.WriteString(cmbRouteTypes.SelectedValue.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteStartElement("CruisingAlt");
                    writer.WriteString(txtAltitude.Text);
                    writer.WriteEndElement();
                    writer.WriteStartElement("DepartureID");
                    writer.WriteString(_departure.AirportId);
                    writer.WriteEndElement();
                    writer.WriteStartElement("DepartureLLA");
                    var elev = (int)_departure.Elevation;
                    writer.WriteString(Common.C2Dms(_departure.Latitude, true) + "," +
                                       Common.C2Dms(_departure.Longitude, false) + "," +
                                       elev.ToString("+000000.00"));
                    writer.WriteEndElement();
                    writer.WriteStartElement("DestinationID");
                    writer.WriteString(_destination.AirportId);
                    writer.WriteEndElement();

                    elev = (int)_destination.Elevation;
                    writer.WriteStartElement("DestinationLLA");
                    writer.WriteString(Common.C2Dms(_destination.Latitude, true) + "," +
                                       Common.C2Dms(_destination.Longitude, false) + "," +
                                       elev.ToString("+000000.00"));
                    writer.WriteEndElement();
                    writer.WriteStartElement("DeparturePosition");
                    var pos = cmbParking.Text.Replace("Runway ", string.Empty);
                    var idx = cmbParking.Text.IndexOf('(');
                    writer.WriteString(idx > -1 ? pos.Substring(0, idx - 1) : pos);
                    writer.WriteEndElement();
                    writer.WriteStartElement("DepartureName");
                    writer.WriteString(_departure.IcaoName);
                    writer.WriteEndElement();
                    writer.WriteStartElement("DestinationName");
                    writer.WriteString(_destination.IcaoName);
                    writer.WriteEndElement();
                    writer.WriteStartElement("AppVersion");
                    writer.WriteStartElement("AppVersionMajor");
                    writer.WriteString("10");
                    writer.WriteEndElement();
                    writer.WriteStartElement("AppVersionBuild");
                    writer.WriteString("3");
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    _points.Clear();

                    //TODO: TOC Update
                    //Remove TOC and TOD and check the boxes appropriately
                    for (var i = _routepoints.Count - 1; i >= 0; i--)
                    {
                        if (_routepoints[i] == "TOD")
                        {
                            chkIncludeTOD.Checked = true;
                            _routepoints.Remove("TOD");
                        }
                    }

                    //Get the TOC
                    _points = Common.GetRoutePoints(_routepoints, _departure, _destination, acft,
                        txtAltitude.Value, chkIncludeTOC.Checked, chkIncludeTOD.Checked);
                    var start = _airports.FirstOrDefault(a => a.AirportId == _routepoints[0]);
                    var stop = _airports.FirstOrDefault(a => a.AirportId == _routepoints[_routepoints.Count - 1]);
                    foreach (var routepoint in _points)
                    {
                        writer.WriteStartElement("ATCWaypoint");
                        writer.WriteAttributeString("id", routepoint.Id);

                        //See if it's an airport
                        if (routepoint.Type == "Airport")
                        {
                            writer.WriteStartElement("ATCWaypointType");
                            writer.WriteString("Airport");
                            writer.WriteEndElement();
                            writer.WriteStartElement("WorldPosition");
                            writer.WriteString(Common.C2Dms(routepoint.Latitude, true, true) + "," +
                                               Common.C2Dms(routepoint.Longitude, false, true) + "," +
                                               routepoint.Elevation.ToString("+000000.00"));
                            writer.WriteEndElement();

                            writer.WriteStartElement("ICAO");
                            writer.WriteStartElement("ICAOIdent");
                            writer.WriteString(routepoint.Id);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        else if (routepoint.Type.StartsWith("VOR") || routepoint.Type.StartsWith("NDB"))
                        {
                            writer.WriteStartElement("ATCWaypointType");
                            writer.WriteString(routepoint.Type.StartsWith("VOR") ? "VOR" : routepoint.Type);
                            writer.WriteEndElement();
                            writer.WriteStartElement("WorldPosition");
                            writer.WriteString(Common.C2Dms(routepoint.Latitude, true, true) + "," +
                                               Common.C2Dms(routepoint.Longitude, false, true) + ", " +
                                               routepoint.Elevation.ToString("+000000.00"));
                            writer.WriteEndElement();

                            writer.WriteStartElement("ICAO");
                            writer.WriteStartElement("ICAORegion");
                            writer.WriteString(routepoint.Region);
                            writer.WriteEndElement();
                            writer.WriteStartElement("ICAOIdent");
                            writer.WriteString(routepoint.Id);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        else if (routepoint.Id == "TOD")
                        {
                            writer.WriteStartElement("ATCWaypointType");
                            writer.WriteString("User");
                            writer.WriteEndElement();
                            writer.WriteStartElement("WorldPosition");
                            writer.WriteString(Common.C2Dms(routepoint.Latitude, true, true) + "," +
                                               Common.C2Dms(routepoint.Longitude, false, true) + ", " +
                                               routepoint.Elevation.ToString("+000000.00"));
                            writer.WriteEndElement();

                            writer.WriteStartElement("ICAO");
                            writer.WriteStartElement("ICAORegion");
                            writer.WriteString(routepoint.Region);
                            writer.WriteEndElement();
                            writer.WriteStartElement("ICAOIdent");
                            writer.WriteString(routepoint.Id);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        else if (routepoint.Region != string.Empty)
                        {
                            writer.WriteStartElement("ATCWaypointType");
                            writer.WriteString("Intersection");
                            writer.WriteEndElement();
                            writer.WriteStartElement("WorldPosition");
                            writer.WriteString(Common.C2Dms(routepoint.Latitude, true, true) + "," +
                                               Common.C2Dms(routepoint.Longitude, false, true) +
                                               "+000000.00");
                            writer.WriteEndElement();
                            if (_airways.ContainsKey(routepoint.Id))
                            {
                                writer.WriteStartElement("ATCAirway");
                                writer.WriteString(_airways.FirstOrDefault(a => a.Key == routepoint.Id).Value);
                                writer.WriteEndElement();
                            }

                            writer.WriteStartElement("ICAO");
                            writer.WriteStartElement("ICAORegion");
                            writer.WriteString(routepoint.Region);
                            writer.WriteEndElement();
                            writer.WriteStartElement("ICAOIdent");
                            writer.WriteString(routepoint.Id);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                    if (path == Common.flightSimPaths[FlightSimType.FSX].FPPath && !string.IsNullOrEmpty(Common.flightSimPaths[FlightSimType.MSFS].FPPath))
                    {
                        //Copy the plan to the P3D folder as well.
                        if (Directory.Exists(Common.flightSimPaths[FlightSimType.MSFS].FPPath))
                        {
                            File.Copy(path + @"\" + _fileName + ".pln", Common.flightSimPaths[FlightSimType.MSFS].FPPath + @"\" + _fileName + ".pln", true);
                        }
                    }
                    else if (path == Common.flightSimPaths[FlightSimType.FSX].FPPath && !string.IsNullOrEmpty(Common.flightSimPaths[FlightSimType.P3D].FPPath))
                    {
                        if (Directory.Exists(Common.flightSimPaths[FlightSimType.P3D].FPPath))
                        {
                            //Copy the plan to the P3D folder as well.
                            File.Copy(path + @"\" + _fileName + ".pln", Common.flightSimPaths[FlightSimType.P3D].FPPath + @"\" + _fileName + ".pln", true);
                        }
                    }
                }

                //Do not create the flt file for MSFS
                if (!string.IsNullOrEmpty(Common.DataPath))
                {
                    //Mission Brief Xml
                    var helper = new ResourceHelpers();
                    XDocument xmlDoc;
                    if (Common.FlightSim == FlightSimType.P3D)
                    {
                        xmlDoc = XDocument.Parse(helper.GetResourceTextFile("TemplateP3D.xml"));
                    }
                    else
                    {
                        xmlDoc = XDocument.Parse(helper.GetResourceTextFile("Template.xml"));
                    }
                    var items =
                        from item in
                        xmlDoc.Elements("SimBase.Document")
                            .Elements("WorldBase.Flight")
                            .Elements("SimMissionUI.ScenarioMetadata")
                        select item;
                    foreach (var item in items)
                    {
                        //assign new value to the sub-element author
                        item.Element("AbbreviatedMissionBrief").Value = _fileName + ".htm";
                        item.Element("MissionBrief").Value = _fileName + ".htm";
                        item.Element("Descr").Value = (btnVFR.Checked ? "VFR" : "IFR") + " flight from " + _departure.IcaoName + " to " + _destination.IcaoName;
                        break;
                    }
                    items =
                        from item in
                        xmlDoc.Elements("SimBase.Document")
                        select item;
                    foreach (var item in items)
                    {
                        item.Element("Title").Value = _fileName;
                        break;
                    }

                    xmlDoc.Save(path + @"\" + _fileName + ".xml");


                    //Build the Flight file
                    var radioMsg = new StringBuilder(btnVFR.Checked ? "VFR " : "IFR ");
                    radioMsg.Append(_departure.IcaoName + " to " + _destination.IcaoName + ", ");
                    radioMsg.Append(_departure.AirportId + " to " + _destination.AirportId + ", ");
                    if (dtFlight.Value > DateTime.MinValue)
                    {
                        radioMsg.Append(dtFlight.Value.TimeOfDay + " " + dtFlight.Value.Date);
                    }
                    if (Common.FlightSim != FlightSimType.MSFS)
                    {
                        if (!flightType.Contains("(flt)"))
                        {
                            //Flight Fxml
                            xmlDoc = XDocument.Parse(helper.GetResourceTextFile("Template.fxml"));
                            //Root Section
                            items = from item in xmlDoc.Elements("SimBase.Document")
                                    where item.Element("Filename") != null
                                    select item;

                            foreach (var item in items)
                            {
                                item.Attribute("id").Value = _fileName;
                                var versionInfo = Common.p3dAppVersion.Split('.');
                                if (versionInfo.Length > 1)
                                {
                                    item.Attribute("version").Value = versionInfo[0] + "," + versionInfo[1];
                                }
                                //assign new value to the sub-element author
                                item.Element("Filename").Value = _fileName + ".fxml";
                                break;
                            }

                            items = from item in xmlDoc.Elements("SimBase.Document").Elements("Flight.Sections")
                                    select item;

                            var xElements = items as IList<XElement> ?? items.ToList();
                            xElements.Elements("Section")
                                .Elements("Property")
                                .Where(x => !x.HasAttributes)
                                .Remove();

                            xElements.Elements("Section")
                                .Elements("Property").Where(el => ((string)el.Attribute("Name")).StartsWith("WpInfo"))
                                .Remove();

                            foreach (var item in xElements)
                            {
                                foreach (var section in item.Elements())
                                {
                                    switch (section.Attribute("Name").Value)
                                    {
                                        case "Main":
                                            foreach (var elem in section.Elements())
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "Title":
                                                        elem.Attribute("Value").Value = _fileName;
                                                        break;
                                                    case "Description":
                                                        elem.Attribute("Value").Value = _departure.IcaoName + " to " +
                                                                                        _destination.IcaoName;
                                                        break;
                                                    case "AppVersion":
                                                        elem.Attribute("Value").Value = Common.p3dAppVersion;
                                                        break;
                                                }
                                            }
                                            break;
                                        case "Kneeboard":
                                            foreach (var elem in section.Elements())
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "RadioMsg0":
                                                        elem.Attribute("Value").Value = radioMsg.ToString();
                                                        break;
                                                }
                                            }
                                            break;
                                        case "GPS_Engine":
                                            //                                        gpsFound = true;
                                            foreach (var elem in section.Elements())
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "Filename":
                                                        elem.Attribute("Value").Value = _fileName;
                                                        break;
                                                    case "position":
                                                        elem.Attribute("Value").Value =
                                                            @"N0&#xB0; 0.00&apos;, E0&#xB0; 0.00&apos;, +000000.00";
                                                        break;
                                                    case "CountWP":
                                                        elem.Attribute("Value").Value = _points.Count.ToString();
                                                        break;
                                                }
                                            }
                                            break;
                                        case "DateTimeSeason":
                                            if (dtFlight.Value > DateTime.MinValue)
                                            {
                                                foreach (var elem in section.Elements())
                                                {
                                                    switch (elem.Attribute("Name").Value)
                                                    {
                                                        case "Season":
                                                            elem.Attribute("Value").Value =
                                                                Common.Season(dtFlight.Value).ToString();
                                                            break;
                                                        case "Year":
                                                            elem.Attribute("Value").Value = dtFlight.Value.Year.ToString();
                                                            break;
                                                        case "Day":
                                                            elem.Attribute("Value").Value =
                                                                dtFlight.Value.DayOfYear.ToString();
                                                            break;
                                                        case "Hours":
                                                            elem.Attribute("Value").Value = dtFlight.Value.Hour.ToString();
                                                            break;
                                                        case "Minutes":
                                                            elem.Attribute("Value").Value = dtFlight.Value.Minute.ToString();
                                                            break;
                                                        case "Seconds":
                                                            elem.Attribute("Value").Value = dtFlight.Value.Second.ToString();
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case "Sim.0":
                                            foreach (var elem in section.Elements())
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "Sim":
                                                        //Get the aircraft text
                                                        var idx = cmbAircraft.Text.LastIndexOf('(');
                                                        elem.Attribute("Value").Value = idx > -1
                                                            ? cmbAircraft.Text.Substring(0, idx - 1).Trim()
                                                            : cmbAircraft.Text;
                                                        break;
                                                }
                                            }
                                            break;
                                        case "SimVars.0":
                                            Parking spot = null;
                                            if (cmbParking.Text.StartsWith("Runway"))
                                            {
                                                var rwyParts = cmbParking.Text.Split(' ');
                                                if (rwyParts.Length > 1)
                                                {
                                                    var rwy = _departure.Runways.FirstOrDefault(r => r.Number == rwyParts[1]);
                                                    if (rwy != null)
                                                    {
                                                        spot = new Parking
                                                        {
                                                            Latitude = rwy.Latitude,
                                                            Longitude = rwy.Longitude,
                                                            Heading = (long)rwy.Heading
                                                        };
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var park = Common.ParseParking(cmbParking.Text);
                                                var park1 = AWDConvert.ToInt32(park[1]);

                                                spot = _departure.Parkings.FirstOrDefault(
                                                    p =>
                                                        //p.Type.Replace("_", " ") == park[0] &&
                                                        p.Name == park[0] &&
                                                        p.Number == park1);
                                                if (park1 == 0)
                                                {
                                                    Common.SendMail(
                                                        "There's an issue with the selected parking.  The parking text is " +
                                                        cmbParking.Text + ", departure airport is " + _departure.AirportId);
                                                }
                                            }

                                            foreach (var elem in section.Elements())
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "Latitude":
                                                        elem.Attribute("Value").Value = Common.LatLon(spot.Latitude,
                                                            true);
                                                        break;
                                                    case "Longitude":
                                                        elem.Attribute("Value").Value = Common.LatLon(spot.Longitude,
                                                            false);
                                                        break;
                                                    case "Altitude":
                                                        elem.Attribute("Value").Value = "+000000.00";
                                                        break;
                                                    case "Heading":
                                                        if (spot != null)
                                                        {
                                                            elem.Attribute("Value").Value =
                                                                ((int)spot.Heading).ToString(CultureInfo.InvariantCulture);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "Weather":
                                            foreach (var elem in section.Elements())
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "WeatherType":
                                                        switch (cmbWeatherTypes.SelectedIndex)
                                                        {
                                                            case 0:
                                                                elem.Attribute("Value").Value = "0";
                                                                break;
                                                            case 1:
                                                                if (cmbWeatherTypes.Items.Count == 2)
                                                                {
                                                                    elem.Attribute("Value").Value = "2";
                                                                }
                                                                else
                                                                {
                                                                    elem.Attribute("Value").Value = "1";
                                                                }
                                                                break;
                                                            default:
                                                                elem.Attribute("Value").Value = "3";
                                                                break;
                                                        }
                                                        break;
                                                    case "ThemeName":
                                                        switch (cmbWeatherTypes.SelectedIndex)
                                                        {
                                                            case 0:
                                                                elem.Attribute("Value").Value = @"weather\themes\" + cmbThemes.SelectedValue;
                                                                break;
                                                            default:
                                                                elem.Attribute("Value").Value = "";
                                                                break;
                                                        }
                                                        break;
                                                    case "ThemeTime":
                                                        switch (cmbWeatherTypes.SelectedIndex)
                                                        {
                                                            case 0:
                                                                elem.Attribute("Value").Value = cmbThemes.SelectedValue.ToString().StartsWith("as_dynamic") ? "26" : "20";
                                                                break;
                                                            default:
                                                                elem.Attribute("Value").Value = "";
                                                                break;
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case "ResourcePath":
                                            foreach (var elem in section.Elements())
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "HintPath":
                                                        elem.Attribute("Value").Value = path;
                                                        break;
                                                }
                                            }
                                            break;
                                        case "ObjectFile":
                                            foreach (var elem in section.Elements())
                                            {
                                                switch (elem.Attribute("Name").Value)
                                                {
                                                    case "File":
                                                        elem.Attribute("Value").Value = _fileName;
                                                        break;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }

                            var gps =
                                xElements.Elements("Section")
                                    .FirstOrDefault(i => (string)i.Attribute("Name") == "GPS_Engine");
                            if (gps != null)
                            {
                                for (var i = 0; i < _points.Count; i++)
                                {
                                    //3=height in meters
                                    double height;
                                    if (i == 0)
                                    {
                                        height = (double)_departure.Elevation * 0.3048;
                                    }
                                    else if (i == _routepoints.Count - 1)
                                    {
                                        height = (double)_destination.Elevation * 0.3048;
                                    }
                                    else
                                    {
                                        height = AWDConvert.ToInt32(txtAltitude.Value) * 0.3048;
                                    }
                                    var xl = new XElement("Property");
                                    xl.Add(new XAttribute("Name", "WpInfo" + i));
                                    xl.Add(new XAttribute("Value",
                                        txtSpeed.Text + ", 0, " + AWDConvert.ToInt32(height) + ", 0, 0, 0.0, 0.0, 0.0"));
                                    gps.Add(xl);
                                }
                            }
                            else
                            {
                                var section = new XElement("Section");
                                section.Add(new XAttribute("Name", "GPS_Engine"));
                                var xl = new XElement("Property");
                                xl.Add(new XAttribute("Name", "Filename"));
                                xl.Add(new XAttribute("Value", _fileName));
                                section.Add(xl);
                                xl = new XElement("Property");
                                xl.Add(new XAttribute("Name", "position"));
                                xl.Add(new XAttribute("Value",
                                    @"N0&#xB0; 0.00&apos;, E0&#xB0; 0.00&apos;, +000000.00"));
                                section.Add(xl);
                                xl = new XElement("Property");
                                xl.Add(new XAttribute("Name", "Time"));
                                xl.Add(new XAttribute("Value", "0"));
                                section.Add(xl);
                                xl = new XElement("Property");
                                xl.Add(new XAttribute("Name", "TimeWP"));
                                xl.Add(new XAttribute("Value", "0"));
                                section.Add(xl);
                                xl = new XElement("Property");
                                xl.Add(new XAttribute("Name", "ArriveTime"));
                                xl.Add(new XAttribute("Value", "0"));
                                section.Add(xl);
                                xl = new XElement("Property");
                                xl.Add(new XAttribute("Name", "CountWP"));
                                xl.Add(new XAttribute("Value",
                                    _routepoints.Count.ToString()));
                                section.Add(xl);
                                for (var i = 0; i < _points.Count; i++)
                                {
                                    //3=height in meters
                                    double height;
                                    if (i == 0)
                                    {
                                        height = (double)_departure.Elevation * 0.3048;
                                    }
                                    else if (i == _points.Count - 1)
                                    {
                                        height = (double)_destination.Elevation * 0.3048;
                                    }
                                    else
                                    {
                                        height = AWDConvert.ToInt32(txtAltitude.Value) * 0.3048;
                                    }
                                    xl = new XElement("Property");
                                    xl.Add(new XAttribute("Name", "WpInfo" + i));
                                    xl.Add(new XAttribute("Value",
                                        txtSpeed.Text + ", 0, " + AWDConvert.ToInt32(height) + ", 0, 0, 0.0, 0.0, 0.0"));
                                    section.Add(xl);
                                }
                                xl = new XElement("Property");
                                xl.Add(new XAttribute("Name", "NextWP"));
                                xl.Add(new XAttribute("Value", "1"));
                                section.Add(xl);
                                xl = new XElement("Property");
                                xl.Add(new XAttribute("Name", "PlaneStarted"));
                                xl.Add(new XAttribute("Value", "False"));
                                section.Add(xl);
                                xl = new XElement("Property");
                                xl.Add(new XAttribute("Name", "CountFP"));
                                xl.Add(new XAttribute("Value", "0"));
                                section.Add(xl);

                                xElements.FirstOrDefault(i => i.Name == "Flight.Sections").Add(section);
                            }

                            xmlDoc.Save(path + @"\" + _fileName + ".fxml");
                            var reader = new StreamReader(path + @"\" + _fileName + ".fxml");
                            var fxml = reader.ReadToEnd();
                            reader.Close();

                            fxml = Regex.Replace(fxml, "&amp;", "&");
                            var writer = new StreamWriter(path + @"\" + _fileName + ".fxml");
                            writer.Write(fxml);
                            writer.Close();
                        }
                        else if (!flightType.Contains("fxml)"))
                        {
                            var result = new StringBuilder();
                            Parking spot = null;
                            if (cmbParking.Text.StartsWith("Runway"))
                            {
                                var rwyParts = cmbParking.Text.Split(' ');
                                if (rwyParts.Length > 1)
                                {
                                    var rwy = _departure.Runways.FirstOrDefault(r => r.Number == rwyParts[1]);
                                    if (rwy != null)
                                    {
                                        spot = new Parking
                                        {
                                            Latitude = rwy.Latitude,
                                            Longitude = rwy.Longitude,
                                            Heading = (long)rwy.Heading
                                        };
                                    }
                                }
                            }
                            else
                            {
                                var prk = Common.ParseParking(cmbParking.Text);
                                var prk1 = AWDConvert.ToInt32(prk[1]);

                                if (prk1 == 0)
                                {
                                    Common.SendMail(
                                        "There's an issue with the selected parking.  The parking text is " +
                                        cmbParking.Text + ", departure airport is " + _departure.AirportId);
                                }

                                spot = _departure.Parkings.FirstOrDefault(
                                    p =>
                                        p.Name == prk[0] &&
                                        p.Number == prk1);
                            }

                            //See if the Previous Flight file is found.  Otherwise use the template
                            string flt;
                            var stream = helper.GetResourceFile("Template.flt");
                            var sr = new StreamReader(stream);
                            // Read the file and display it line by line.
                            while ((flt = sr.ReadLine()) != null)
                            {
                                var fltParts = flt.Split('=');
                                switch (fltParts[0])
                                {
                                    case "Title":
                                        flt = fltParts[0] + "=" + _fileName;
                                        break;
                                    case "Description":
                                        flt = fltParts[0] + "=" + _departure.IcaoName + " to " + _destination.IcaoName;
                                        break;
                                    case "AppVersion":
                                        flt = fltParts[0] + "=" + Common.fsxAppVersion;
                                        break;
                                    case "RadioMsg0":
                                        flt = fltParts[0] + "=" + radioMsg;
                                        break;
                                    case "Filename":
                                        flt = fltParts[0] + "=" + _fileName;
                                        break;
                                    case "position":
                                        var elev = ((int)_departure.Elevation).ToString("N2").Replace(",", "");
                                        while (elev.Length < 9)
                                        {
                                            elev = "0" + elev;
                                        }
                                        flt = fltParts[0] + @"=" + Common.LatLon(_departure.Latitude, true, true) +
                                              ", " + Common.LatLon(_departure.Longitude, false, true) + ", +" + elev;
                                        break;
                                    case "CountWP":
                                        flt = fltParts[0] + "=" + _points.Count;
                                        break;
                                    case "WpInfo0":
                                        var wypts = new StringBuilder();
                                        for (var i = 0; i < _points.Count; i++)
                                        {
                                            //3=height in meters
                                            double height;
                                            if (i == 0)
                                            {
                                                height = (double)_departure.Elevation * 0.3048;
                                            }
                                            else if (i == _points.Count - 1)
                                            {
                                                height = (double)_destination.Elevation * 0.3048;
                                            }
                                            else
                                            {
                                                height = AWDConvert.ToInt32(txtAltitude.Value) * 0.3048;
                                            }
                                            wypts.Append("WpInfo" + i + "=" + txtSpeed.Text + ", 0, " +
                                                         AWDConvert.ToInt32(height) + ", 0, 0, 0.0, 0.0, 0.0" +
                                                         Environment.NewLine);
                                        }
                                        flt = wypts.ToString();
                                        break;
                                    case "WpInfo1":
                                    case "WpInfo2":
                                    case "WpInfo3":
                                    case "WpInfo4":
                                    case "WpInfo5":
                                    case "WpInfo6":
                                    case "WpInfo7":
                                    case "WpInfo8":
                                    case "WpInfo9":
                                    case "WpInfo10":
                                    case "WpInfo11":
                                    case "WpInfo12":
                                    case "WpInfo13":
                                    case "WpInfo14":
                                    case "WpInfo15":
                                    case "WpInfo16":
                                    case "WpInfo17":
                                    case "WpInfo18":
                                    case "WpInfo19":
                                    case "WpInfo20":
                                        flt = string.Empty;
                                        break;
                                    case "Season":
                                        if (dtFlight.Value > DateTime.MinValue)
                                        {
                                            flt = fltParts[0] + "=" + Common.Season(dtFlight.Value);
                                        }
                                        break;
                                    case "Year":
                                        if (dtFlight.Value > DateTime.MinValue)
                                        {
                                            flt = fltParts[0] + "=" + dtFlight.Value.Year;
                                        }
                                        break;
                                    case "Day":
                                        if (dtFlight.Value > DateTime.MinValue)
                                        {
                                            flt = fltParts[0] + "=" + dtFlight.Value.DayOfYear;
                                        }
                                        break;
                                    case "Hours":
                                        if (dtFlight.Value > DateTime.MinValue)
                                        {
                                            flt = fltParts[0] + "=" + dtFlight.Value.Hour;
                                        }
                                        break;
                                    case "Minutes":
                                        if (dtFlight.Value > DateTime.MinValue)
                                        {
                                            flt = fltParts[0] + "=" + dtFlight.Value.Minute;
                                        }
                                        break;
                                    case "Seconds":
                                        if (dtFlight.Value > DateTime.MinValue)
                                        {
                                            flt = fltParts[0] + "=" + dtFlight.Value.Second;
                                        }
                                        break;
                                    case "Sim":
                                        var idx = cmbAircraft.Text.LastIndexOf('(');
                                        flt = idx > -1
                                            ? fltParts[0] + "=" + cmbAircraft.Text.Substring(0, idx - 1).Trim()
                                            : fltParts[0] + "=" + cmbAircraft.Text;
                                        break;
                                    case "Latitude":
                                        flt = fltParts[0] + "=" + Common.C2Dms((double)spot.Latitude, true);
                                        break;
                                    case "Longitude":
                                        flt = fltParts[0] + "=" + Common.C2Dms((double)spot.Longitude, false);
                                        break;
                                    case "Altitude":
                                        flt = fltParts[0] + "=" + "+000000.00";
                                        break;
                                    case "Heading":
                                        flt = fltParts[0] + "=" +
                                              ((int)spot.Heading).ToString(CultureInfo.InvariantCulture);
                                        break;
                                    case "WeatherType":
                                        switch (cmbWeatherTypes.SelectedIndex)
                                        {
                                            case 0:
                                                flt = fltParts[0] + "=0";
                                                break;
                                            case 1:
                                                flt = fltParts[0] + "=1";
                                                break;
                                            default:
                                                flt = fltParts[0] + "=3";
                                                break;
                                        }
                                        break;
                                    case "ThemeName":
                                        switch (cmbWeatherTypes.SelectedIndex)
                                        {
                                            case 0:
                                                flt = fltParts[0] + @"=weather\themes\" + cmbThemes.SelectedValue;
                                                break;
                                            default:
                                                flt = fltParts[0] + "=";
                                                break;
                                        }
                                        break;
                                    case "ThemeTime":
                                        switch (cmbWeatherTypes.SelectedIndex)
                                        {
                                            case 0:
                                                flt = fltParts[0] + "=" + (cmbThemes.SelectedValue.ToString().StartsWith("as_dynamic") ? "26" : "20");
                                                break;
                                            default:
                                                flt = fltParts[0] + "=";
                                                break;
                                        }
                                        break;
                                    case "File":
                                        flt = fltParts[0] + "=" + _fileName;
                                        break;
                                }
                                result.Append(flt + Environment.NewLine);
                            }

                            sr.Close();
                            var val = result.ToString();
                            val = Regex.Replace(val, "&amp;", "&");
                            var writer = new StreamWriter(path + @"\" + _fileName + ".flt", false, Encoding.Default);
                            writer.Write(val);
                            writer.Close();
                        }
                    }

                    //Briefing Html
                    _departure.IcaoName = _departure.IcaoName ?? _departure.AirportId;
                    _destination.IcaoName = _destination.IcaoName ?? _destination.AirportId;
                    _briefing = helper.GetResourceTextFile("Template.htm");
                    _briefing = Regex.Replace(_briefing,
                        "!BRIEFING TITLE!",
                        _departure.AirportId + " to " + _destination.AirportId + " Briefing");
                    _briefing = Regex.Replace(_briefing,
                        "!FLIGHTPLAN!",
                        _departure.AirportId + " to " + _destination.AirportId + ", " + (btnVFR.Checked ? "VFR" : "IFR"));
                    _briefing = Regex.Replace(_briefing,
                        "!DEPARTURE!",
                        _departure.IcaoName);
                    _briefing = Regex.Replace(_briefing,
                        "!DESTINATION!",
                        _destination.IcaoName);
                    _briefing = Regex.Replace(_briefing,
                        "!ALTITUDE!",
                        txtAltitude.Text);

                    //Route
                    var route = string.Empty;
                    foreach (var routepoint in _routepoints)
                    {
                        if (!string.IsNullOrEmpty(route))
                        {
                            route += " ";
                        }
                        route += routepoint;
                    }
                    _briefing = Regex.Replace(_briefing, "!ROUTE!", route);

                    var routeDetail = new StringBuilder("<table style=\"font-size:11px;\">");
                    foreach (var point in _points)
                    {
                        string type = point.Type +
                                      (!string.IsNullOrEmpty(point.Frequency) ? " " + point.Frequency : string.Empty);
                        routeDetail.Append("<tr><td>" + type + "</td><td>" + point.Id + "</td></tr>");
                    }
                    routeDetail.Append("</table>");
                    _briefing = Regex.Replace(_briefing, "!ROUTEDETAIL!", routeDetail.ToString());

                    //***Parking 4
                    _briefing = Regex.Replace(_briefing, "!PARKING!", cmbParking.Text);

                    //Departure/Destination
                    for (var i = 0; i < 2; i++)
                    {
                        var apt = i == 0 ? _departure : _destination;
                        var txt = new StringBuilder();
                        txt.Append(apt.AirportId + ", " + apt.IcaoName + Environment.NewLine);
                        txt.Append("Elevation: " + apt.Elevation + " ft" + Environment.NewLine);
                        var lighting = new StringBuilder();
                        var num = 0;
                        var aptRunways = apt.Runways.OrderBy(r => r.Number);
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
                        _briefing = Regex.Replace(_briefing, i == 0 ? "!DEPINFO!" : "!DESTINFO!", txt.ToString());

                        //Comm
                        //Update PF3 frequency if approach and departure are on same frequency
                        if (!string.IsNullOrEmpty(Common.pf3Path))
                        {
                            var depFreq = apt.Comms.FirstOrDefault(f => f.Type == (long)CommType.DEPARTURE); //9
                            var appFreq = apt.Comms.FirstOrDefault(f => f.Type == (long)CommType.APPROACH); //8

                            if (depFreq.Frequency == appFreq.Frequency)
                            {
                                if (apt.AirportId == _departure.AirportId)
                                {
                                    //Open the PF3 file
                                    if (File.Exists(Common.pf3Path))
                                    {
                                        var line = string.Empty;
                                        var reader = new StreamReader(File.OpenRead(Common.pf3Path));
                                        var pf3File = reader.ReadToEnd();
                                        reader.Close();
                                        reader = new StreamReader(File.OpenRead(Common.pf3Path));
                                        while (!reader.EndOfStream)
                                        {
                                            line = reader.ReadLine();
                                            //Find departure
                                            if (line.StartsWith(apt.AirportId + ",8"))
                                            {
                                                var lineParts = line.Split(',');
                                                if (lineParts.Length > 2)
                                                {
                                                    lineParts[2] = "123.6";
                                                }
                                                var newLine = string.Join(",", lineParts);
                                                pf3File = Regex.Replace(pf3File, line, newLine);
                                            }
                                            else if (line.StartsWith(apt.AirportId + ",9"))
                                            {
                                                var lineParts = line.Split(',');
                                                if (lineParts.Length > 2)
                                                {
                                                    lineParts[2] = depFreq.Frequency.ToString();
                                                }
                                                var newLine = string.Join(",", lineParts);
                                                pf3File = Regex.Replace(pf3File, line, newLine);
                                            }
                                        }
                                        reader.Close();

                                        //Save the file
                                        var txtPF3 = new StreamWriter(Common.pf3Path);
                                        txtPF3.Write(pf3File.ToString());
                                        txtPF3.Close();
                                    }
                                }
                                else if (apt.AirportId == _destination.AirportId)
                                {
                                    //Open the PF3 file
                                    if (File.Exists(Common.pf3Path))
                                    {
                                        var line = string.Empty;
                                        var reader = new StreamReader(File.OpenRead(Common.pf3Path));
                                        var pf3File = reader.ReadToEnd();
                                        reader.Close();
                                        reader = new StreamReader(File.OpenRead(Common.pf3Path));
                                        while (!reader.EndOfStream)
                                        {
                                            line = reader.ReadLine();
                                            //Find departure
                                            if (line.StartsWith(apt.AirportId + ",8"))
                                            {
                                                var lineParts = line.Split(',');
                                                if (lineParts.Length > 2)
                                                {
                                                    lineParts[2] = appFreq.Frequency.ToString();
                                                }
                                                var newLine = string.Join(",", lineParts);
                                                pf3File = Regex.Replace(pf3File, line, newLine);
                                            }
                                            else if (line.StartsWith(apt.AirportId + ",9"))
                                            {
                                                var lineParts = line.Split(',');
                                                if (lineParts.Length > 2)
                                                {
                                                    lineParts[2] = "123.6";
                                                }
                                                var newLine = string.Join(",", lineParts);
                                                pf3File = Regex.Replace(pf3File, line, newLine);
                                            }
                                        }
                                        reader.Close();

                                        //Save the file
                                        var txtPF3 = new StreamWriter(Common.pf3Path);
                                        txtPF3.Write(pf3File.ToString());
                                        txtPF3.Close();
                                    }
                                }
                            }
                        }

                        txt = new StringBuilder();

                        foreach (var comm in apt.Comms)
                        {
                            if (!string.IsNullOrEmpty(comm.Frequency))
                            {
                                var commName = !string.IsNullOrEmpty(comm.Name) ? ", " + comm.Name : string.Empty;
                                txt.Append((CommType)comm.Type + commName + ", " + comm.Frequency + " MHz" +
                                           Environment.NewLine);
                            }
                        }
                        _briefing = Regex.Replace(_briefing, i == 0 ? "!DEPCOMMS!" : "!DESTCOMMS!", txt.ToString());

                        _briefing = Regex.Replace(_briefing, i == 0 ? "!DEPRUNWAYLIGHTING!" : "!DESTRUNWAYLIGHTING!",
                            lighting.ToString());
                    }

                    //Weather
                    if (cmbWeatherTypes.SelectedIndex < 1)
                    {
                        _briefing = Regex.Replace(_briefing, "!DEPMETAR!", "Theme: " + cmbThemes.Text);
                        _briefing = Regex.Replace(_briefing, "!DESTMETAR!", "Theme: " + cmbThemes.Text);
                    }
                    else
                    {
                        try
                        {
                            var weatherParts = UpdateWeather(_departure.AirportId, _destination.AirportId).Split('|');
                            if (weatherParts.Length > 1)
                            {
                                _briefing = Regex.Replace(_briefing, "!DEPMETAR!", _departure.AirportId + " WEATHER:" + Environment.NewLine + weatherParts[0]);
                                _briefing = Regex.Replace(_briefing, "!DESTMETAR!", _destination.AirportId + " WEATHER:" + Environment.NewLine + weatherParts[1]);
                            }
                            else if (weatherParts[0].Contains(_departure.AirportId))
                            {
                                _briefing = Regex.Replace(_briefing, "!DEPMETAR!", _departure.AirportId + " WEATHER:" + Environment.NewLine + weatherParts[0]);
                            }
                            else
                            {
                                _briefing = Regex.Replace(_briefing, "!DESTMETAR!", _destination.AirportId + " WEATHER:" + Environment.NewLine + weatherParts[0]);
                            }

                            if (_isIFR && cmbWeatherTypes.Items.Count > 0 && cmbWeatherTypes.SelectedIndex != 0 && btnVFR.Checked)
                            {
                                timer1.Interval = 1000; // 1 second interval
                                timer1.Start();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Unable to connect to the weather service.  Please check your internet connection and try again.");
                        }
                    }

                    _briefing = Regex.Replace(_briefing, "!DEPMETAR!", "METAR Not Found.");
                    _briefing = Regex.Replace(_briefing, "!DESTMETAR!", "METAR Not Found.");
                    _path = path;

                    //Diagrams
                    var charts = new StringBuilder();
                    if (File.Exists(Common.DataPath + @"\tmp.pdf"))
                    {
                        File.Delete(Common.DataPath + @"\tmp.pdf");
                    }
                    if (File.Exists(Common.DataPath + @"\tmp.txt"))
                    {
                        File.Delete(Common.DataPath + @"\tmp.txt");
                    }

                    var txtCharts = new StringBuilder();
                    foreach (var chart in selectedCharts)
                    {
                        if (chart.ChartId != "error")
                        {
                            try
                            {

                                using (var client = new HttpClient())
                                {
                                    var response = await client.GetAsync(chart.PDFUrl);
                                    response.EnsureSuccessStatusCode();
                                    await using var ms = await response.Content.ReadAsStreamAsync();
                                    var images = PDFtoImage.Conversion.ToImages(ms, "", 96);
                                    var page = 1;
                                    foreach (var image in images)
                                    {
                                        var encImage = image.Encode(SKEncodedImageFormat.Png, 96);
                                        var imgFile = $"{Common.DataPath}\\ChartImages\\{chart.ChartId}_{page}.png";
                                        File.WriteAllBytes(imgFile, encImage.ToArray());
                                        charts.Append($"<img src=\"{Common.DataPath}\\ChartImages\\{chart.ChartId}_{page}.png\"><hr />{Environment.NewLine}");
                                        page++;
                                    }
                                    txtCharts.Append(chart.ChartId + Environment.NewLine);
                                }
                            }
                            catch
                            {
                                MessageBox.Show(@"An error occured while loading chart """ +
                                                chart.ChartName +
                                                @""".  The chart could not be found.  The requested url is """ +
                                                chart.PDFUrl + @"""." + Environment.NewLine + Environment.NewLine +
                                                @"You can press CTRL-C to copy this text.");
                                if (File.Exists(Common.DataPath + @"\tmp.txt"))
                                {
                                    File.Delete(Common.DataPath + @"\tmp.txt");
                                }
                            }
                        }
                        else
                        {
                            charts.Append("An error occurred while loading the charts for " + chart.ChartName + Environment.NewLine);
                        }
                    }

                    if (!Directory.Exists(Common.DataPath.TrimEnd('\\') + "\\Routes"))
                    {
                        Directory.CreateDirectory(Common.DataPath.TrimEnd('\\') + "\\Routes");
                    }
                    var txtWriter = new StreamWriter(Common.DataPath.TrimEnd('\\') + @"\Routes\" + _fileName + ".awd");
                    txtWriter.Write(txtCharts.ToString());
                    txtWriter.Close();

                    var chartsText = string.Empty;
                    switch (Common.ChartService)
                    {
                        //case "AUTO": //Future feature
                        //    if (_departure.Country == "United States" && _destination.Country == "United States")
                        //    {
                        //        chartsText = "Charts (courtesy of FAA.org)";
                        //    }
                        //    else
                        //    {
                        //        chartsText = "Charts (courtesy of Aircharts.org)";
                        //    }
                        //    break;
                        case "FAA":
                            chartsText = "Charts (courtesy of FAA.org)";
                            break;
                        case "AVIATIONAPI":
                            chartsText = "Charts (courtesy of AviationAPI.com)";
                            break;
                        default:
                            chartsText = "Charts";
                            break;
                    }

                    _briefing = Regex.Replace(_briefing, "!CHARTTEXT!", chartsText);
                    _briefing = Regex.Replace(_briefing, "!CHARTS!", charts.ToString());

                    txtWriter = new StreamWriter(_path + @"\" + _fileName + ".htm");
                    txtWriter.Write(_briefing);
                    txtWriter.Close();

                    wbBriefing.Navigate(_path + @"\" + _fileName + ".htm");
                }
                if (dtFlight.Value > DateTime.MinValue)
                {
                    Properties.Settings.Default.LastFlight = dtFlight.Value.ToString(CultureInfo.InvariantCulture);
                    Properties.Settings.Default.Save();
                }
                btnBuildPlan.FlatStyle = FlatStyle.System;
                btnBuildPlan.UseVisualStyleBackColor = true;
                if (Common.FlightSim != FlightSimType.MSFS)
                {
                    btnLaunchFS.BackColor = Color.LightBlue;
                    btnLaunchFS.Enabled = true;
                }
                else
                {
                    btnLaunchFS.Enabled = false;
                }
                btnPrintBriefing.Enabled = true;
            }
            catch (Exception ex)
            {
                var message = Common.BuildExceptionMessage(ex);
                Common.SendMail($"An error occurred while building the Briefing. {message}");
                MessageBox.Show(@"An error occurred while generating the briefing. The developer has been notified. Please try again later. We apologize for the problem.");
            }

            return null;
        }

        private void OnTimerTick(Object sender, EventArgs eventargs)
        {
            lblIFRConditions.Visible = !lblIFRConditions.Visible;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Common.DataUpdateInProgress)
            {
                Close();
            }
            else if (MessageBox.Show(@"Are you sure you want to exit?", @"Are you sure?", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private async void FSFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FrmOptions();
            UseWaitCursor = true;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //If they changed the flight sim, updated the app data
                if (Common.PriorFlightSim != Common.FlightSim)
                {
                    Application.DoEvents();
                    SimStartup();
                    await FillLists();
                    ClearData();

                    if (Properties.Settings.Default.UpdateOnStart)
                    {
                        CheckForUpdates(true);
                    }

                    Common.PriorFlightSim = Common.FlightSim;
                }
            }
            UseWaitCursor = false;
        }

        private void cmbAircraft_SelectedValueChanged(object sender, EventArgs e)
        {
            //Show the image
            if (cmbAircraft.SelectedItem != null)
            {
                picPreview.ImageLocation = ((Aircraft)cmbAircraft.SelectedItem).Texture + @"\thumbnail.jpg";
                picPreview.Visible = File.Exists(picPreview.ImageLocation);
            }
            else
            {
                picPreview.Visible = false;
            }
            if (cmbAircraft.SelectedIndex > -1)
            {
                var acft = _aircraft.FirstOrDefault(a => a.Name == ((Aircraft)cmbAircraft.SelectedItem).Name);
                if (acft != null && acft.Airspeed > 0 && acft.Airspeed >= txtSpeed.Minimum && acft.Airspeed <= txtSpeed.Maximum)
                {
                    txtSpeed.Value = (decimal)acft.Airspeed;
                }
                else
                {
                    txtSpeed.Value = 0;
                }
            }
        }

        private async Task<bool> FillLists()
        {
            if (string.IsNullOrEmpty(Common.DataPath))
            {
                Common.DataPath =
                Path.GetDirectoryName(Application.ExecutablePath).ToLower().Replace(@"\bin\debug", string.Empty).Replace(@"\bin\release", string.Empty) +
                @"\Data";
            }
            //Check the database to see if there are airports
            if (!Directory.Exists(Common.DataPath))
            {
                Directory.CreateDirectory(Common.DataPath);
            }
            bool dbOk = false;
            if (!File.Exists(Common.DataPath + @"\fsflightbuilder.db"))
            {
                //Create new database
                var helper = new ResourceHelpers();
                var db = helper.GetResourceFile("FSFlightBuilder.db");
                using (var file = new FileStream(Common.DataPath + @"\fsflightbuilder.db", FileMode.Create, FileAccess.Write))
                {
                    db.CopyTo(file);
                }
                if (!File.Exists(Common.DataPath + @"\fsflightbuilder.db"))
                {
                    if (MessageBox.Show("An error occured while creating the database. Unfortunately, FS Flight Builder cannot continue. Please try again.", "FS Flight Builder Closing", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    {
                        Environment.Exit(-1);
                    }
                }

            }
            else
            {
                if (!DataHelper.CheckDatabase())
                {
                    MessageBox.Show("The FS Flight Builder database has been modified in this version. An update is required." + Environment.NewLine + Environment.NewLine +
                        "Once the database is updated, the data will be refreshed using the Scenery Import tool.", "Database Update Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Add the missing columns
                    DataHelper.AddDatabaseColumns();
                    //Run the BGL
                    bglImporter = new BGLImporter();
                    UseWaitCursor = true;
                    if (bglImporter.ShowDialog() == DialogResult.OK)
                    {
                        using (var context = new FSFBDbConn(Common.DBConnName, Common.DataPath))
                        {
                            dbOk = context.Airports.Any(fs => fs.LongestRwyLength > 0 && fs.FSType == (int)Common.FlightSim) && context.Runways.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Parkings.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Comms.Any(fs => fs.FSType == (int)Common.FlightSim) &&
                            context.Aircraft.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Navaids.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Waypoints.Any(w => w.FSType == (int)Common.FlightSim) && context.Routes.Any(fs => fs.FSType == (int)Common.FlightSim);
                        }
                    }
                    UseWaitCursor = false;
                }
                else
                {
                    using (var context = new FSFBDbConn(Common.DBConnName, Common.DataPath))
                    {
                        try
                        {
                            dbOk = context.Airports.Any(fs => fs.LongestRwyLength > 0 && fs.FSType == (int)Common.FlightSim) && context.Runways.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Parkings.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Comms.Any(fs => fs.FSType == (int)Common.FlightSim) &&
                                context.Aircraft.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Navaids.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Waypoints.Any(w => w.FSType == (int)Common.FlightSim) && context.Routes.Any(fs => fs.FSType == (int)Common.FlightSim);
                        }
                        catch
                        {
                            dbOk = false;
                        }
                    }
                }
            }

            if (!dbOk)
            {
                if (!Directory.Exists(Common.flightSimPaths[Common.FlightSim].DefaultFSPath))
                {
                    MessageBox.Show($"The flight sim path ({Common.flightSimPaths[Common.FlightSim].DefaultFSPath}) for {Common.FlightSim} could not be found.{Environment.NewLine + Environment.NewLine}Unfortunately, FS Flight Builder cannot continue. Please check the directory and try again.", "Flight Sim Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(-1);
                }

                DataHelper.AddDatabaseColumns();
                //If the xml files exist
                if (File.Exists(Common.DataPath + @"\runways.xml") && File.Exists(Common.DataPath + @"\Parking.csv") &&
                File.Exists(Common.DataPath + @"\Navaids.xml") && File.Exists(Common.DataPath + @"\Comms.csv") &&
                File.Exists(Common.DataPath + @"\Waypoints.xml"))
                {
                    //show message about updates in the background
                    MessageBox.Show("FS Flight Builder has been redesigned to use a database to store the data, instead of requiring MakeRunways to generate it." + Environment.NewLine + Environment.NewLine +
                        "The database will be now be created using the Scenery Import screen.", "FS Flight Builder Redesign", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (bglImporter.ShowDialog() == DialogResult.OK)
                    {
                        if (MessageBox.Show(this, "The new database has been created. FS Flight Builder will now use the database and the MakeRunways utility is no longered required."
                            + Environment.NewLine + Environment.NewLine +
                            "It's recommended that we restart FS Flight Builder.  Would you like to restart?", "Restart FS Flight Builder?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            //re-run the app
                            Application.Restart();
                        }
                        else
                        {
                            await FillLists();
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("Some data files were not found. The database will need to be updated to restore the missing data. This process may take several" +
                    " minutes to finish. Do you want to continue?", "Database Update Required", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        if (Common.FlightSim == FlightSimType.Unknown)
                        {
                            //If multiple flight simulators were found, get selected sim
                            var txt = "FS Flight Builder found multiple flight simulators." + Environment.NewLine + Environment.NewLine + "Please select the flight simulator you would like to load.";
                            List<FlightSimType> flightSims = new List<FlightSimType>();
                            foreach (var fs in Common.flightSimPaths)
                            {
                                if (!string.IsNullOrEmpty(fs.Value.DefaultFSPath))
                                {
                                    flightSims.Add(fs.Key);
                                }
                            }

                            //Select the flight sim from the list
                            if (flightSims.Count > 1)
                            {
                                var popup = new PopupMessage("", txt, flightSims);
                                if (popup.ShowDialog() == DialogResult.Yes)
                                {
                                    if (popup.SelectedFlightSim == FlightSimType.Unknown)
                                    {
                                        //Close the app
                                        if (MessageBox.Show("FS Flight Builder cannot determine the flight sim and will exit.", "FS Flight Builder Closing", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                                        {
                                            Environment.Exit(-1);
                                        }
                                    }
                                    switch (popup.SelectedFlightSim)
                                    {
                                        case FlightSimType.MSFS:
                                            _fsPath = Common.flightSimPaths[FlightSimType.MSFS].DefaultFSPath;
                                            Common.FlightSim = FlightSimType.MSFS;
                                            btnLaunchFS.Text = @"MSFS - Disabled";
                                            btnLaunchFS.Enabled = false; //.Text = @"Launch Prepar3D";
                                            Text = "FS Flight Builder - Microsoft Flight Simulator";
                                            break;
                                        case FlightSimType.P3D:
                                            _fsPath = Common.flightSimPaths[FlightSimType.P3D].DefaultFSPath;
                                            Common.FlightSim = FlightSimType.P3D;
                                            btnLaunchFS.Text = @"Launch Prepar3D";
                                            Text = "FS Flight Builder - Prepar3D";
                                            break;
                                        case FlightSimType.FSXSE:
                                            _fsPath = Common.flightSimPaths[FlightSimType.FSXSE].DefaultFSPath;
                                            Common.FlightSim = FlightSimType.FSXSE;
                                            btnLaunchFS.Text = @"Launch FSX Steam Edition";
                                            Text = "FS Flight Builder - FSX Steam Edition";
                                            break;
                                        case FlightSimType.FSX:
                                            _fsPath = Common.flightSimPaths[FlightSimType.FSX].DefaultFSPath;
                                            Common.FlightSim = FlightSimType.FSX;
                                            btnLaunchFS.Text = @"Launch FSX";
                                            Text = "FS Flight Builder - FSX";
                                            break;
                                    }
                                    Properties.Settings.Default.FlightSim = Common.FlightSim.ToString();
                                    Properties.Settings.Default.Save();
                                    //Make sure the selected flight sim paths are ok
                                    Common.CheckPaths();
                                }
                                else
                                {
                                    //Close the app
                                    if (MessageBox.Show("FS Flight Builder is unable to determine the installed flight sim and will exit.", "FS Flight Builder Closing", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                                    {
                                        Environment.Exit(-1);
                                    }
                                }
                            }
                        }

                        Enabled = false;
                        if (Common.CheckPaths())
                        {
                            if (string.IsNullOrEmpty(_fsPath))
                            {
                                _fsPath = Common.flightSimPaths[Common.FlightSim].DefaultFSPath;
                            }

                            bglImporter = new BGLImporter();
                            try
                            {
                                if (bglImporter.ShowDialog() == DialogResult.OK)
                                {
                                    await FillLists();
                                }
                            }
                            catch (OperationCanceledException)
                            {
                            }
                        }
                        Enabled = true;
                    }
                    else
                    {
                        if (MessageBox.Show("Unfortunately, FS Flight Builder is missing data files and cannot continue.", "FS Flight Builder Closing", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                        {
                            Environment.Exit(-1);
                        }
                    }
                }

            }

            await FillAsyncLists();

            switch (Common.FlightSim)
            {
                case FlightSimType.MSFS:
                    btnLaunchFS.Text = @"MSFS - Disabled";
                    btnLaunchFS.Enabled = false;
                    Text = "FS Flight Builder - Microsoft Flight Simulator";
                    break;
                case FlightSimType.P3D:
                    btnLaunchFS.Text = @"Launch Prepar3D";
                    Text = "FS Flight Builder - Prepar3D";
                    break;
                case FlightSimType.FSXSE:
                    btnLaunchFS.Text = @"Launch FSX Steam Edition";
                    Text = "FS Flight Builder - FSX Steam Edition";
                    break;
                case FlightSimType.FSX:
                    btnLaunchFS.Text = @"Launch FSX";
                    Text = "FS Flight Builder - FSX";
                    break;
            }

            if (string.IsNullOrEmpty(Common.DataPath))
            {
                Common.DataPath =
                    Path.GetDirectoryName(Application.ExecutablePath).ToLower().Replace(@"\bin\debug", string.Empty).Replace(@"\bin\release", string.Empty) +
                    @"\Data";
            }
            if (!Directory.Exists(Common.DataPath))
            {
                Directory.CreateDirectory(Common.DataPath);
            }

            txtRoute.BackColor = !string.IsNullOrEmpty(txtRoute.Text) ? Color.White : Color.LightCoral;
            txtSpeed.BackColor = txtSpeed.Value > 0 ? Color.White : Color.LightCoral;
            txtAltitude.BackColor = txtAltitude.Value > 0 ? Color.White : Color.LightCoral;

            Components.Update.updateMe(updaterPrefix, Application.StartupPath + @"\");
            if (cmbWeatherTypes.Items.Count > 2)
            {
                cmbWeatherTypes.SelectedIndex = 2;
            }

            //txtRoutes Tooltip for weather
            rtb = new RichTextBoxToolTip { RichTextBox = txtRoute };

            if (File.Exists(Application.StartupPath + @"\update.awd"))
            {
                MessageBox.Show("FS Flight Builder has been updated.  Please select the \"Help : Change Log\" menu option to see what's been changed.", "Application Updated");
                try
                {
                    File.Delete(Application.StartupPath + @"\update.awd");
                }
                catch
                {
                }
            }

            try
            {
                //Aircraft
                SetAppPath();
                for (var i = _aircraft.Count; i > 0; i--)
                {
                    if (_aircraft[i - 1].Texture.Contains(".AirTraffic"))
                    {
                        _aircraft.RemoveAt(i - 1);
                    }
                }

                cmbAircraft.DataSource = _aircraft;
                cmbAircraft.DisplayMember = "Name";
                cmbAircraft.ValueMember = "Texture";
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting aircraft.", ex);
            }

            //Weather Themes
            if (File.Exists(Common.DataPath + @"\themes.txt"))
            {
                Dictionary<string, string> themes = new Dictionary<string, string>();
                var wtReader = new StreamReader(File.OpenRead(Common.DataPath + @"\themes.txt"));
                while (!wtReader.EndOfStream)
                {
                    var parts = wtReader.ReadLine().Split('|');
                    themes.Add(parts[0], parts[1]);
                }
                wtReader.Close();
                cmbThemes.DataSource = new BindingSource(themes, null);
                cmbThemes.DisplayMember = "Value";
                cmbThemes.ValueMember = "Key";
            }
            else
            {
                if (_fsPath == null)
                {
                    _fsPath = Common.flightSimPaths[Common.FlightSim].DefaultFSPath;
                }

                if (string.IsNullOrEmpty(_fsPath))
                {
                    _fsPath = Common.CheckInstalls();
                }

                if (_fsPath != null && Directory.Exists(_fsPath) && Directory.Exists(_fsPath.TrimEnd('\\') + @"\Weather\Themes"))
                {
                    var files = new List<string>();
                    var fileContents =
                        from file in Directory.EnumerateFiles(_fsPath.TrimEnd('\\') + @"\Weather\Themes", "*.wt")
                        select new { File = file };
                    foreach (var fc in fileContents)
                    {
                        files.Add(fc.File);
                    }

                    Dictionary<string, string> themes = new Dictionary<string, string>();
                    var sb = new StringBuilder();
                    foreach (var wt in files)
                    {
                        var lines = File.ReadAllLines(wt);
                        foreach (string line in lines)
                        {
                            var cfgLine = line.ToLower();
                            if (cfgLine.StartsWith("title="))
                            {
                                var idx = wt.LastIndexOf('\\');
                                var wtb = idx > -1 ? wt.Substring(idx + 1) : wt;
                                themes.Add(wtb.Replace(".wt", ".WTB"), line.Substring(6).Trim('\"'));
                                sb.Append(wtb.Replace(".wt", ".WTB") + "|" + line.Substring(6).Trim('\"') + Environment.NewLine);
                                break;
                            }
                        }
                    }
                    File.WriteAllText(Common.DataPath + @"\Themes.txt", sb.ToString());

                    cmbThemes.DataSource = new BindingSource(themes, null);
                    cmbThemes.DisplayMember = "Value";
                    cmbThemes.ValueMember = "Key";
                }
                else if (Common.FlightSim != FlightSimType.MSFS)
                {
                    MessageBox.Show(
                        @"The weather themes folder was not found.  Searching in the " + _fsPath.TrimEnd('\\') +
                        @"\Weather\Themes folder.");
                    throw new Exception(@"The weather themes folder was not found.  Searching in the " +
                                     _fsPath.TrimEnd('\\') + @"\Weather\Themes folder.");
                }
            }

            try
            {
                var dict = new Dictionary<string, string>
                {
                    {"", "Direct"},
                    {"VOR", "VOR to VOR"},
                    {"LowAlt", "Low Altitude Airways"},
                    {"HighAlt", "High Altitude Airways"}
                };

                cmbRouteTypes.DataSource = new BindingSource(dict, null);
                cmbRouteTypes.DisplayMember = "Value";
                cmbRouteTypes.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                throw new Exception("Error filling Route Types.", ex);
            }
            return true;
        }

        private async Task<bool> FillAsyncLists()
        {
            _airports.Clear();
            _parkings.Clear();
            _comms.Clear();
            _waypoints.Clear();
            _navaids.Clear();
            _aircraft.Clear();

            //Check the database for the waypointid column in the waypoints table.
            //If not there, show message that the database needs to be updated
            //and send them to the bglimporter
            //show message about updates in the background
            if (!DataHelper.CheckDatabase())
            {
                MessageBox.Show("FS Flight Builder has been redesigned to use a database to store the data, instead of requiring MakeRunways to generate it." + Environment.NewLine + Environment.NewLine +
                    "The database will be now be created using the Scenery Import screen.", "FS Flight Builder Redesign", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Add the missing columns
                DataHelper.AddDatabaseColumns();
                //Run the BGL
                bglImporter = new BGLImporter();
                if (bglImporter.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show(this, "The new database has been created. FS Flight Builder will now use the database and the MakeRunways utility is no longered required."
                        + Environment.NewLine + Environment.NewLine +
                        "It's recommended that we restart FS Flight Builder.  Would you like to restart?", "Restart FS Flight Builder?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //re-run the app
                        Application.Restart();
                    }
                    else
                    {
                        FillLists();
                    }
                }
            }

            _waypoints = await DataHelper.GetWaypoints();
            _navaids = await DataHelper.GetNavaids();
            _parkings = DataHelper.GetParkings();
            _comms = DataHelper.GetComms();
            _airports = DataHelper.GetAirports();
            _aircraft = await DataHelper.GetAircraft();
            return true;
        }

        private void ClearData()
        {
            try
            {
                txtRoute.Clear();
                timer1.Stop();
                _isIFR = false;
                lblIFRConditions.Visible = false;
                if (_airways != null)
                {
                    _airways.Clear();
                }
                _fileName = null;
                _routepoints = new List<string>();
                txtSkyVector.Text = string.Empty;
                txtSpeed.Value = 0;
                txtAltitude.Value = 0;
                if (wbBriefing != null)
                {
                    wbBriefing.DocumentText = string.Empty;
                }
                cmbParking.Items.Clear();
                cmbRouteTypes.SelectedIndex = -1;
                cmbAircraft.SelectedIndex = -1;
                btnVFR.Checked = true;
                btnIFR.Checked = false;
                lblParking.Text = @"Starting Position";

                cmbParking.Enabled =
                    cmbRouteTypes.Enabled = cmbWeatherTypes.Enabled =
                        btnVFR.Enabled =
                            btnIFR.Enabled = btnBuildPlan.Enabled = cmbAircraft.Enabled = dtFlight.Enabled =
                                btnSystem.Enabled = chkSystem.Enabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error clearing data.", ex);
            }

        }

        private void updateAircraftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                if (UpdateAircraft())
                {
                    MessageBox.Show(@"Aircraft update complete.");
                }
            }
        }

        private void fullDatabaseUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FullUpdate();
        }

        internal bool BuildRoute()
        {
            if (_routepoints != null && _routepoints.Count > 0)
            {
                if (_routepoints.Count == 1)
                {
                    MessageBox.Show(@"Please identify a departure and destination airport for your route.");
                    return false;
                }
                var tmpRoutes = _routepoints.GetRange(0, _routepoints.Count);

                //Iterate through the routepoints to replace any victor airways with actual waypoints
                for (var i = 1; i < _routepoints.Count - 1; i++)
                {
                    var prev = i > 1 ? _routepoints[i - 1] : string.Empty;
                    var next = i < _routepoints.Count - 2 ? _routepoints[i + 1] : string.Empty;
                    var okToRemove = false;
                    for (int a = 0; a < 2; a++)
                    {
                        var dir = a == 0 ? prev : next;
                        var wypts = DataHelper.GetWaypointsById(dir).ToList();
                        if (wypts.Count > 0)
                        {
                            foreach (var wypt in wypts)
                            {
                                foreach (var rte in wypt.Routes)
                                {
                                    if (rte.Name == _routepoints[i])
                                    {
                                        okToRemove = true;
                                        if (wypt.NavId == dir)
                                        {
                                            var val = a == 0 ? rte.Next : rte.Previous;
                                            var idx = tmpRoutes.IndexOf(_routepoints[i]);
                                            if (!string.IsNullOrEmpty(val) && !tmpRoutes.Contains(val))
                                            {
                                                if (idx > -1)
                                                {
                                                    tmpRoutes.Insert(idx, val);
                                                    _airways.Add(val, _routepoints[i]);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    if (okToRemove)
                    {
                        tmpRoutes.Remove(_routepoints[i]);
                    }
                }

                _routepoints = tmpRoutes;

                var departureChanged = _departure == null || _departure.AirportId != _routepoints[0];
                if (departureChanged)
                {
                    _departure = DataHelper.GetAirport(_routepoints[0].Trim());
                }

                if (_departure == null)
                {
                    MessageBox.Show(
                        @"Please check the departure ICAO for your flight.  " + _routepoints[0] +
                        @" was not found in the database." + Environment.NewLine + Environment.NewLine +
                        @"If it was added recently, you may need to update the database.",
                        @"Missing Departure Airport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (_destination == null || _destination.AirportId != _routepoints[_routepoints.Count - 1])
                {
                    _destination = DataHelper.GetAirport(_routepoints[_routepoints.Count - 1].Trim());
                }

                if (_destination == null)
                {
                    MessageBox.Show(
                        @"Please check the destination ICAO for your flight.  " + _routepoints[_routepoints.Count - 1] +
                        @" was not found in the database." + Environment.NewLine + Environment.NewLine +
                        @"If it was added recently, you may need to update the database.",
                        @"Missing Destination Airport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //Fill the page controls
                lblParking.Text = @"Starting Position at " + _departure.AirportId;
                if (departureChanged)
                {
                    //var parkingSpots = _parkings.Where(p => p.AirportId == _departure.AirportId);
                    var parkingSpots = _departure.Parkings;
                    cmbParking.Items.Clear();
                    //Add the runways
                    foreach (var runway in _departure.Runways.OrderBy(r => r.Number))
                    {
                        cmbParking.Items.Add("Runway " + runway.Number);
                    }

                    //Add the parking spots
                    foreach (var parking in parkingSpots)
                    {
                        var desc = GenerateParkingString(parking);
                        cmbParking.Items.Add(desc);
                    }
                }

                cmbParking.Enabled =
                    cmbRouteTypes.Enabled =
                        btnVFR.Enabled =
                            btnIFR.Enabled = btnBuildPlan.Enabled = cmbAircraft.Enabled = dtFlight.Enabled =
                                btnSystem.Enabled = chkSystem.Enabled = true;

                cmbWeatherTypes.Enabled = Common.FlightSim != FlightSimType.MSFS;
                double fromLat = 0;
                double fromLon = 0;
                double fromMag = 0;
                double toLat = 0;
                double toLon = 0;
                double toMag = 0;
                var apt = DataHelper.GetAirport(_routepoints[0]);
                if (apt != null)
                {
                    fromLat = apt.Latitude;
                    fromLon = apt.Longitude;
                    fromMag = apt.MagVar;
                }

                apt = DataHelper.GetAirport(_routepoints[1]);
                if (apt != null)
                {
                    toLat = apt.Latitude;
                    toLon = apt.Longitude;
                    toMag = apt.MagVar;
                }
                else
                {
                    var wpt = DataHelper.GetWaypointById(_routepoints[1]);
                    if (wpt != null)
                    {
                        toLat = wpt.Latitude;
                        toLon = wpt.Longitude;
                        toMag = wpt.MagVar * -1;
                    }
                    else
                    {
                        var nav = DataHelper.GetNavaidById(_routepoints[1]);
                        if (nav != null)
                        {
                            toLat = nav.Latitude;
                            toLon = nav.Longitude;
                            toMag = (double)nav.MagVar * -1;
                        }
                    }

                }

                if (fromLat != 0 && fromLon != 0 && toLat != 0 && toLon != 0)
                {
                    var dir = Pos.BearingTo(fromLat, fromLon, toLat, toLon);
                    dir = dir + Math.Round((fromMag + toMag) / 2);
                    dir = dir > 360 ? dir - 360 : dir;
                    dir = dir < 0 ? dir + 360 : dir;
                    routeBearing = Convert.ToInt32(dir);
                    UpdateAltTooltip();
                }

                return true;
            }
            //Calculate Direction between the first route point and the second
            return false;
        }

        private void UpdateAltTooltip()
        {
            if (btnIFR.Checked)
            {
                if (routeBearing < 180)
                {
                    ttSkyVector.SetToolTip(this.txtAltitude, "Odd");
                }
                else
                {
                    ttSkyVector.SetToolTip(this.txtAltitude, "Even");
                }
            }
            else
            {
                if (routeBearing < 180)
                {
                    ttSkyVector.SetToolTip(this.txtAltitude, "Odd + 500");
                }
                else
                {
                    ttSkyVector.SetToolTip(this.txtAltitude, "Even + 500");
                }
            }
        }

        private void importFlightFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fileName = null;
            var path = Common.flightSimPaths[Common.FlightSim].FPPath; // == FlightSimType.FSX
                                                                       //                ? _fsxFPPath
                                                                       //                : Common.FlightSim == FlightSimType.FSXSE ? _fsxSEFPPath : _p3dFPPath;
            fdFlight.InitialDirectory = path;
            fdFlight.Filter = Common.FlightSim.ToString().StartsWith("FSX")
                ? "Flight Files|*.flt|Flight Plans|*.pln"
                : Common.FlightSim.ToString().StartsWith("MS")
                ? "Flight Plans|*.pln"
                : "Flight Files|*.fxml|Flight Plans|*.pln";

            DialogResult result = fdFlight.ShowDialog();
            if (result != DialogResult.Cancel && !string.IsNullOrEmpty(fdFlight.FileName))
            {
                using (new WaitCursor())
                {
                    ImportData();
                    GetDepDestData();
                }
            }
        }

        private async void ImportData()
        {
            ClearData();
            if (fdFlight.SafeFileName != null)
            {
                try
                {
                    var idx = fdFlight.SafeFileName.LastIndexOf(".", StringComparison.Ordinal);
                    _fileName = idx > -1 ? fdFlight.SafeFileName.Substring(0, idx) : fdFlight.SafeFileName;
                    var info = Common.ParseFlightFile(fdFlight.FileName, _aircraft);
                    if (info != null)
                    {
                        string fs;
                        if (fdFlight.SafeFileName.ToLower().EndsWith(".pln"))
                        {
                            fs = Common.FlightSim.ToString();
                        }
                        else
                        {
                            fs = fdFlight.SafeFileName.ToLower().EndsWith(".flt")
                            ? Common.FlightSim == FlightSimType.FSXSE ? "FSXSE" : "FSX"
                            : "P3D";
                        }

                        if (fs != Common.FlightSim.ToString())
                        {
                            SetAppPath();

                            cmbAircraft.DataSource = _aircraft;
                            switch (fs)
                            {
                                case "FSX":
                                    Common.FlightSim = FlightSimType.FSX;
                                    break;
                                case "FSXSE":
                                    Common.FlightSim = FlightSimType.FSXSE;
                                    break;
                                case "P3D":
                                    Common.FlightSim = FlightSimType.P3D;
                                    break;
                                default:
                                    Common.FlightSim = FlightSimType.Unknown;
                                    break;
                            }
                        }
                        if (info.Aircraft != null)
                        {
                            cmbAircraft.SelectedItem = info.Aircraft;
                        }

                        if (chkSystem.Checked)
                        {
                            dtFlight.Value = System.DateTime.Now;
                        }
                        else if (AWDConvert.ToDateTime(info.FlightTime) > DateTime.MinValue)
                        {
                            dtFlight.Value = AWDConvert.ToDateTime(info.FlightTime);
                        }

                        _routepoints = info.Route ?? new List<string>();
                        //TODO: TOC Update
                        //Check for TOC and TOD and check the boxes appropriately
                        for (var i = _routepoints.Count - 1; i >= 0; i--)
                        {
                            if (_routepoints[i] == "TOD")
                            {
                                chkIncludeTOD.Checked = true;
                            }
                        }

                        if (!string.IsNullOrEmpty(info.CruiseAltitude))
                        {
                            txtAltitude.Value = AWDConvert.ToDecimal(info.CruiseAltitude);
                        }
                        else
                        {
                            txtAltitude.Enabled = true;
                        }
                        if (!string.IsNullOrEmpty(info.CruiseSpeed))
                        {
                            txtSpeed.Value = AWDConvert.ToDecimal(info.CruiseSpeed);
                        }
                        else
                        {
                            txtSpeed.Enabled = true;
                        }
                        if (BuildRoute())
                        {
                            var route = new StringBuilder();
                            foreach (var rte in info.Route)
                            {
                                if (route.Length > 0)
                                {
                                    route.Append(" ");
                                }
                                route.Append(rte);
                            }

                            txtRoute.Text = route.ToString();
                            ResetParking();
                            if (!string.IsNullOrEmpty(info.Parking))
                            {
                                idx =
                                    cmbParking.FindString(info.Parking);

                                if (idx > -1 && cmbParking.Items.Count > idx)
                                {
                                    cmbParking.SelectedIndex = idx;
                                }
                            }
                            else if (info.Runway > 0)
                            {
                                var rwy = info.Runway < 10 ? "0" + info.Runway : info.Runway.ToString();
                                idx =
                                    cmbParking.FindString("Runway " + rwy);
                                if (idx > -1 && cmbParking.Items.Count > idx)
                                {
                                    cmbParking.SelectedIndex = idx;
                                }
                            }

                            idx = cmbRouteTypes.FindString(info.RouteType);
                            if (idx > -1 && cmbRouteTypes.Items.Count > idx)
                            {
                                cmbRouteTypes.SelectedIndex = idx;
                            }

                            if (!string.IsNullOrEmpty(info.WeatherType) && cmbWeatherTypes.Items.Count > 2)
                            {
                                switch (info.WeatherType)
                                {
                                    case "0":
                                        cmbWeatherTypes.SelectedIndex = 0;
                                        break;
                                    case "1":
                                        cmbWeatherTypes.SelectedIndex = 1;
                                        break;
                                    case "3":
                                        cmbWeatherTypes.SelectedIndex = 2;
                                        break;
                                }
                            }

                            if (!string.IsNullOrEmpty(info.WeatherType) && !string.IsNullOrEmpty(info.WeatherTheme) && info.WeatherType == "0")
                            {
                                idx = info.WeatherTheme.LastIndexOf('\\');
                                cmbThemes.SelectedValue = idx > -1 ? info.WeatherTheme.Substring(idx + 1) : info.WeatherTheme;
                            }

                            if (!string.IsNullOrEmpty(info.FpType))
                            {
                                if (info.FpType == "VFR")
                                {
                                    btnVFR.Checked = true;
                                }
                                else
                                {
                                    btnIFR.Checked = true;
                                }
                            }
                            _isIFR = false;

                            if (!string.IsNullOrEmpty(info.BriefingFile) && File.Exists(info.BriefingFile))
                            {
                                //Check for flight conditions
                                var reader = new StreamReader(File.OpenRead(info.BriefingFile));
                                var apt = string.Empty;
                                while (!reader.EndOfStream)
                                {
                                    var line = reader.ReadLine();
                                    if (line.Contains(" WEATHER"))
                                    {
                                        apt = line.Substring(0, line.IndexOf(" ", StringComparison.Ordinal));
                                        //Determine if it's departure or destination 
                                    }
                                    else if (line.Contains("Flight Category"))
                                    {
                                        //Get the flight type
                                        if (!string.IsNullOrEmpty(apt))
                                        {
                                            txtRoute.Find(apt, RichTextBoxFinds.MatchCase);
                                            if (line.Contains("LIFR"))
                                            {
                                                _isIFR = true;
                                                txtRoute.SelectionBackColor = Color.Magenta;
                                                txtRoute.SelectionColor = Color.White;
                                            }
                                            else if (line.Contains("IFR"))
                                            {
                                                _isIFR = true;
                                                txtRoute.SelectionBackColor = Color.Red;
                                                txtRoute.SelectionColor = Color.White;
                                            }
                                            else if (line.Contains("MVFR"))
                                            {
                                                txtRoute.SelectionBackColor = Color.Blue;
                                                txtRoute.SelectionColor = Color.White;
                                            }
                                            else if (line.Contains("VFR"))
                                            {
                                                txtRoute.SelectionBackColor = Color.Green;
                                                txtRoute.SelectionColor = Color.White;
                                            }
                                            else
                                            {
                                                txtRoute.SelectionBackColor = Color.White;
                                                txtRoute.SelectionColor = Color.Black;
                                            }
                                            apt = string.Empty;
                                        }
                                    }
                                }
                                reader.Close();

                                wbBriefing.Navigate(info.BriefingFile);
                            }

                            if (_isIFR && cmbWeatherTypes.Items.Count > 0 && (cmbWeatherTypes.SelectedIndex != 0 || Common.FlightSim == FlightSimType.MSFS) && btnVFR.Checked)
                            {
                                timer1.Interval = 1000;
                                timer1.Start();
                            }

                            if (cmbAircraft.SelectedItem != null && txtSpeed.Value == 0 &&
                                cmbAircraft.Items.Count > 0)
                            {
                                var acft =
                                    _aircraft.FirstOrDefault(
                                        a => a.Name == ((Aircraft)cmbAircraft.SelectedItem).Name);
                                if (acft != null && acft.Airspeed > 0)
                                {
                                    txtSpeed.Value = (decimal)acft.Airspeed;
                                }
                            }

                            //Rebuild the chart images
                            var airacMessageShown = false;
                            if (!Directory.Exists(Common.DataPath + @"\ChartImages"))
                            {
                                Directory.CreateDirectory(Common.DataPath + @"\ChartImages");
                            }
                            var chartPath = Common.DataPath.TrimEnd('\\') + "\\Routes\\" +
                                            fdFlight.SafeFileName.ToLower().Replace(".flt", "").Replace("fxml", "") +
                                            ".awd";
                            if (File.Exists(chartPath))
                            {
                                var txtReader = new StreamReader(chartPath);
                                while (!txtReader.EndOfStream)
                                {
                                    var chart = txtReader.ReadLine();
                                    if (!File.Exists(Common.DataPath + "\\ChartImages\\" + chart + "_1.png"))
                                    {
                                        if (!string.IsNullOrEmpty(Properties.Settings.Default.AIRAC))
                                        {
                                            try
                                            {
                                                //Get and save the file
                                                using (var client = new HttpClient())
                                                {
                                                    var url = Common.ChartService == "FAA" ? $"http://aeronav.faa.gov/d-tpp/{Common.CheckAirac(true)}/{chart}" : $"https://charts.aviationapi.com/AIRAC_{Common.CheckAirac(true)}/{chart}";
                                                    //var response = await client.GetAsync($"https://www.aircharts.org/data/view.php?id={chart}");
                                                    if (!string.IsNullOrEmpty(Properties.Settings.Default.AIRAC))
                                                    {
                                                        var response = await client.GetAsync(url);
                                                        response.EnsureSuccessStatusCode();
                                                        await using var ms = await response.Content.ReadAsStreamAsync();
                                                        var images = PDFtoImage.Conversion.ToImages(ms, "", 96);
                                                        var page = 1;
                                                        foreach (var image in images)
                                                        {
                                                            var encImage = image.Encode(SKEncodedImageFormat.Png, 96);
                                                            var imgFile = $"{Common.DataPath}\\ChartImages\\{chart}_{page}.png";
                                                            File.WriteAllBytes(imgFile, encImage.ToArray());
                                                            page++;
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                if (!string.IsNullOrEmpty(Properties.Settings.Default.AIRAC))
                                                {
                                                    Common.logger.Error($"Error loading a chart image. Error is {ex.Message}.");
                                                    MessageBox.Show($"An error occured while loading a chart.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!airacMessageShown)
                                            {
                                                MessageBox.Show($"Could not load charts because the AIRAC version could not be found. Please click the \"Build Flight\" button to reload the charts.");
                                                airacMessageShown = true;
                                            }
                                        }
                                    }
                                }
                                txtReader.Close();
                            }
                            else
                            {
                                MessageBox.Show(@"Select ""Build Flight Files"" to create the flight charts.");
                            }
                            txtRoute.BackColor = !string.IsNullOrEmpty(txtRoute.Text)
                                ? Color.White
                                : Color.LightCoral;
                            btnBuildPlan.Enabled = info.Route.Count > 1 && txtAltitude.Value > 0 &&
                                                   txtSpeed.Value > 0;
                            btnLaunchFS.Enabled = btnBuildPlan.Enabled;
                            btnPrintBriefing.Enabled = !string.IsNullOrEmpty(wbBriefing.DocumentText);

                            if (cmbRouteTypes.SelectedIndex == -1 && cmbRouteTypes.Items.Count > 0)
                            {
                                cmbRouteTypes.SelectedIndex = 0;
                            }
                        }
                    }
                    wbBriefing.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error ocurred while reading the flight plan.");
                    Common.logger.Error($"Error reading flight plan. Error is {ex.Message}.");
                }
            }
        }

        private void txtSpeed_ValueChanged(object sender, EventArgs e)
        {
            txtSpeed.BackColor = txtSpeed.Value > 0 ? Color.White : Color.LightCoral;
            btnBuildPlan.Enabled = BuildRoute() && txtAltitude.Value > 0 && txtSpeed.Value > 0;
            if (btnBuildPlan.Enabled)
            {
                btnBuildPlan.BackColor = Color.LightBlue;
            }
        }

        private void txtAltitude_ValueChanged(object sender, EventArgs e)
        {
            txtAltitude.BackColor = txtAltitude.Value > 0 ? Color.White : Color.LightCoral;
            btnBuildPlan.Enabled = BuildRoute() && txtAltitude.Value > 0 && txtSpeed.Value > 0;
            if (btnBuildPlan.Enabled)
            {
                //Change the color to blue
                btnBuildPlan.BackColor = Color.LightBlue;
            }
        }

        private void btnSystem_Click(object sender, EventArgs e)
        {
            dtFlight.Value = AWDConvert.ToDateTime(DateTime.Now);
        }

        private void chkSystem_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseSystem = chkSystem.Checked;
            Properties.Settings.Default.Save();
        }

        private void newFlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(@"Are you sure you want to create a new flight?", @"Are you sure?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClearData();
            }
        }

        private async void btnLaunchFS_Click(object sender, EventArgs e)
        {
            UpdateConfig();
            Common.GetAllFSPaths();
            if (Common.FlightSim == FlightSimType.FSX && string.IsNullOrEmpty(Common.flightSimPaths[FlightSimType.FSX].DefaultFSPath))
            {
                MessageBox.Show(@"The FSX folder was not found.  Please select the folder and try again.",
                    @"Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                var frm = new FrmOptions();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    //If they changed the flight sim, updated the app data
                    if (Common.PriorFlightSim != Common.FlightSim)
                    {
                        UseWaitCursor = true;
                        SimStartup();
                        await FillLists();
                        ClearData();

                        if (Properties.Settings.Default.UpdateOnStart)
                        {
                            CheckForUpdates(true);
                        }

                        Common.PriorFlightSim = Common.FlightSim;
                        UseWaitCursor = false;
                    }
                }

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Common.flightSimPaths[FlightSimType.FSX].DefaultFSPath + @"\fsx.exe",
                        WorkingDirectory = Common.flightSimPaths[FlightSimType.FSX].DefaultFSPath,
                        Arguments = "-flt:\"" + Common.flightSimPaths[FlightSimType.FSX].FPPath.TrimEnd('\\') + "\\" + _fileName + ".flt\""
                    }
                };
                process.Start();
            }
            else if (Common.FlightSim == FlightSimType.FSXSE && string.IsNullOrEmpty(Common.flightSimPaths[FlightSimType.FSXSE].DefaultFSPath))
            {
                MessageBox.Show(
                    @"The FSX Steam Edition folder was not found.  Please select the folder and try again.",
                    @"Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                var frm = new FrmOptions();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    //If they changed the flight sim, updated the app data
                    if (Common.PriorFlightSim != Common.FlightSim)
                    {
                        UseWaitCursor = true;
                        SimStartup();
                        await FillLists();
                        ClearData();

                        if (Properties.Settings.Default.UpdateOnStart)
                        {
                            CheckForUpdates(true);
                        }

                        Common.PriorFlightSim = Common.FlightSim;
                        UseWaitCursor = false;
                    }
                }

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Common.flightSimPaths[FlightSimType.FSXSE].DefaultFSPath + @"\fsx.exe",
                        WorkingDirectory = Common.flightSimPaths[FlightSimType.FSXSE].DefaultFSPath,
                        Arguments = "-flt:\"" + Common.flightSimPaths[FlightSimType.FSXSE].FPPath.TrimEnd('\\') + "\\" + _fileName + ".flt\""
                    }
                };
                process.Start();
            }
            else if (Common.FlightSim == FlightSimType.P3D && string.IsNullOrEmpty(Common.flightSimPaths[FlightSimType.P3D].DefaultFSPath))
            {
                MessageBox.Show(@"The Prepar3D folder was not found.  Please select the folder and try again.",
                    @"Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                var frm = new FrmOptions();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    //If they changed the flight sim, updated the app data
                    if (Common.PriorFlightSim != Common.FlightSim)
                    {
                        UseWaitCursor = true;
                        SimStartup();
                        await FillLists();
                        ClearData();

                        if (Properties.Settings.Default.UpdateOnStart)
                        {
                            CheckForUpdates(true);
                        }

                        Common.PriorFlightSim = Common.FlightSim;
                        UseWaitCursor = false;
                    }
                }

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Common.flightSimPaths[FlightSimType.P3D].DefaultFSPath + @"\prepar3d.exe",
                        WorkingDirectory = Common.flightSimPaths[FlightSimType.P3D].DefaultFSPath,
                        Arguments = "-fxml:\"" + Common.flightSimPaths[FlightSimType.P3D].FPPath + "\\" + _fileName + ".fxml\""
                    }
                };
                process.Start();
            }
        }

        private void txtRoute_Leave(object sender, EventArgs e)
        {
            GetDepDestData();
        }

        private void txtRoute_TextChanged(object sender, EventArgs e)
        {
            txtRoute.BackColor = !string.IsNullOrEmpty(txtRoute.Text) ? Color.White : Color.LightCoral;
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        private void CheckForUpdates(bool fromStart = false)
        {
            updatesFromMenu = !fromStart;
            AutoUpdater.Start("http://www.arduiniwebdevelopment.com/updates/fsflightbuilderupdater.xml");
        }

        private void Main_Load(object sender, EventArgs e)
        {
        }

        private async void Main_Shown(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            Application.DoEvents();

            await FillLists();
            ClearData();

            if (Properties.Settings.Default.UpdateOnStart)
            {
                CheckForUpdates(true);
            }
            UseWaitCursor = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new About();
            frm.ShowDialog();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Common.DataUpdateInProgress)
            {
                if (MessageBox.Show("The new database is in the process of being created.  If you exit, the database will need to be rebuilt the next time the application is run." + Environment.NewLine + Environment.NewLine + "Do you want to exit?", "Database generation in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            //Restore the flight sim config file
            string path = string.Empty;
            string file = string.Empty;
            switch (Common.FlightSim)
            {
                case FlightSimType.FSX:
                    path = @"Microsoft\FSX\";
                    file = "FSX.CFG";
                    break;
                case FlightSimType.FSXSE:
                    path = @"Microsoft\FSX-SE\";
                    file = "FSX_SE.CFG";
                    break;
                case FlightSimType.P3D:
                    path = @"Lockheed Martin\Prepar3D v4\";
                    file = "prepar3d.cfg";
                    break;
            }
            try
            {
                var appPath = Common.GetAppPath(path, file);
            }
            catch (Exception)
            {
                if (!File.Exists(appPath + file) && File.Exists(appPath + "AWD.cfg"))
                {
                    File.Move(appPath + "AWD.cfg", appPath + file);
                }

                if (File.Exists(appPath + "AWD.cfg"))
                {
                    File.Delete(appPath + "AWD.config");
                }
                if (File.Exists(appPath + "AWD_tmp.cfg"))
                {
                    File.Delete(appPath + "AWD_tmp.config");
                }
            }

            //Delete the chart png files
            //Diagrams
            if (Properties.Settings.Default.DeleteImagesOnClose && Directory.Exists(Common.DataPath + @"\ChartImages"))
            {
                foreach (string f in Directory.EnumerateFiles(Common.DataPath + @"\ChartImages", "*.png"))
                {
                    File.Delete(f);
                }
            }

        }

        private void UpdateConfig()
        {
            string path = string.Empty;
            string search = string.Empty;
            string file = string.Empty;
            switch (Common.FlightSim)
            {
                case FlightSimType.FSX:
                    path = @"Microsoft\FSX\";
                    search = @"SHOW_OPENING_SCREEN";
                    file = "FSX.CFG";
                    break;
                case FlightSimType.FSXSE:
                    path = @"Microsoft\FSX-SE\";
                    search = @"SHOW_OPENING_SCREEN";
                    file = "FSX_SE.CFG";
                    break;
                case FlightSimType.P3D:
                    path = @"Lockheed Martin\Prepar3D v4\";
                    search = @"SHOW_SCENARIO_WINDOW";
                    file = "prepar3d.cfg";
                    break;
            }
            try
            {
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //Find the config file and change the setting to not show the Initial Dialog
                if (!string.IsNullOrEmpty(appDataFolder))
                {
                    appPath = appDataFolder + @"\" + path;
                    if (Common.FlightSim == FlightSimType.P3D)
                    {
                        if (!Directory.Exists(appPath))
                        {
                            appPath = appDataFolder + @"\Lockheed Martin\Prepar3D v3\";
                            if (!Directory.Exists(appPath))
                            {
                                appPath = appDataFolder + @"\Lockheed Martin\Prepar3D v2\";
                                if (!Directory.Exists(appPath))
                                {
                                    appPath = appDataFolder + @"\Lockheed Martin\Prepar3D\";
                                }
                            }
                        }
                    }

                    if (!File.Exists(appPath + "AWD.cfg") && File.Exists(appPath + file))
                    {
                        //Copy it to AWD_tmp.config
                        File.Copy(appPath + file, appPath + "AWD_tmp.cfg");

                        var reader = new StreamReader(appPath + "AWD_tmp.cfg");
                        var fs = reader.ReadToEnd();
                        reader.Close();

                        if (fs.Contains(search + "=1"))
                        {
                            //Find the Opening line in AWD_tmp.config and replace it
                            fs = Regex.Replace(fs, search + "=1", search + "=0");
                            var writer = new StreamWriter(appPath + "AWD_tmp.cfg");
                            writer.Write(fs);
                            writer.Close();
                            //Rename the config to AWD.config
                            File.Move(appPath + file, appPath + "AWD.cfg");
                            //Rename the AWD_tmp.config to config
                            File.Move(appPath + "AWD_tmp.cfg", appPath + file);
                        }
                        else
                        {
                            File.Delete(appPath + "AWD_tmp.cfg");
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (!File.Exists(appPath + file) && File.Exists(appPath + "AWD.cfg"))
                {
                    File.Move(appPath + "AWD.cfg", appPath + file);
                }

                if (File.Exists(appPath + "AWD.cfg"))
                {
                    File.Delete(appPath + "AWD.config");
                }
                if (File.Exists(appPath + "AWD_tmp.cfg"))
                {
                    File.Delete(appPath + "AWD_tmp.config");
                }
            }
        }

        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + @"\fs flight builder manual.pdf");
        }

        private void changeLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Application.StartupPath + @"\changelog.txt"))
            {
                MessageBox.Show("Could not find the Change Log.", "File Not Found");
                return;
            }
            Process.Start(Application.StartupPath + @"\changelog.txt");
        }

        private void txtRoute_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLower(e.KeyChar))
            {
                txtRoute.SelectedText = Char.ToUpper(e.KeyChar).ToString();

                e.Handled = true;
            }
        }

        private void btnVFR_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAltTooltip();
            if (_isIFR && cmbWeatherTypes.Items.Count > 0 && (cmbWeatherTypes.SelectedIndex != 0 || Common.FlightSim == FlightSimType.MSFS) && btnVFR.Checked)
            {
                timer1.Interval = 1000; // 1 second interval
                timer1.Start();
            }
            else
            {
                lblIFRConditions.Visible = false;
                timer1.Stop();
            }
        }

        private void btnIFR_CheckedChanged(object sender, EventArgs e)
        {
            lblIFRConditions.Visible = false;
            UpdateAltTooltip();
            timer1.Stop();
        }

        private void ResetParking(string icao = null)
        {
            var idx = cmbParking.SelectedIndex;
            var spot = cmbParking.SelectedItem ?? string.Empty;
            if (icao == null)
            {
                if (_departure == null)
                {
                    return;
                }
                else
                {
                    icao = _departure.AirportId;
                }
            }
            cmbParking.Items.Clear();
            var parkingSpots = _departure.Parkings; // _parkings.Where(p => p.AirportId == icao);
                                                    //Add the runways
            foreach (var runway in _departure.Runways.OrderBy(r => r.Number))
            {
                cmbParking.Items.Add("Runway " + runway.Number);
            }

            //Add the parking spots
            foreach (var parking in parkingSpots)
            {
                var desc = GenerateParkingString(parking);
                cmbParking.Items.Add(desc);
                if (desc == spot.ToString())
                {
                    cmbParking.SelectedItem = desc; // parking;
                }
            }
        }

        private string GenerateParkingString(Parking parking)
        {
            return $"{ParkingUtil.parkingNameToStr(parking.Name)} {parking.Number} ({parking.Type})";
        }

        private void UpdateWeather()
        {
            if (_departure != null && _destination != null)
            {
                UpdateWeather(_departure.AirportId, _destination.AirportId);
            }
        }

        private string UpdateWeather(string dep, string dest)
        {
            lblIFRConditions.Visible = false;
            timer1.Stop();

            var dict = new eDictionary();
            var weather = Common.UpdateWeather(dep, dest, _departure, _destination, ref dict, ref txtRoute);

            // Assign the dictionary to the RichTextBoxToolTip
            rtb.Dictionary = dict;
            _isIFR = weather.Contains("Flight Category: IFR");

            if (_isIFR && cmbWeatherTypes.Items.Count > 0 && (cmbWeatherTypes.SelectedIndex != 0 || Common.FlightSim == FlightSimType.MSFS) && btnVFR.Checked)
            {
                timer1.Interval = 1000; // 1 second interval
                timer1.Start();
            }
            return weather;
        }

        private void cmbWeatherTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbThemes.Enabled = cmbWeatherTypes.SelectedIndex == 0;
            if (cmbThemes.Enabled && cmbThemes.Items.Count == 0)
            {
                if (_fsPath != null && Directory.Exists(_fsPath) && Directory.Exists(_fsPath.TrimEnd('\\') + @"\Weather\Themes"))
                {
                    var files = new List<string>();
                    var fileContents =
                        from file in Directory.EnumerateFiles(_fsPath.TrimEnd('\\') + @"\Weather\Themes", "*.wt")
                        select new { File = file };
                    foreach (var fc in fileContents)
                    {
                        files.Add(fc.File);
                    }

                    Dictionary<string, string> themes = new Dictionary<string, string>();
                    var db = new StringBuilder();
                    foreach (var wt in files)
                    {
                        var lines = File.ReadAllLines(wt);
                        foreach (string line in lines)
                        {
                            var cfgLine = line.ToLower();
                            if (cfgLine.StartsWith("title="))
                            {
                                var idx = wt.LastIndexOf('\\');
                                var wtb = idx > -1 ? wt.Substring(idx + 1) : wt;
                                themes.Add(wtb.Replace(".wt", ".WTB"), line.Substring(6).Trim('\"'));
                                db.Append(wtb.Replace(".wt", ".WTB") + "|" + line.Substring(6).Trim('\"') + Environment.NewLine);
                                break;
                            }
                        }
                    }
                    File.WriteAllText(Common.DataPath + @"\Themes.txt", db.ToString());

                    cmbThemes.DataSource = new BindingSource(themes, null);
                    cmbThemes.DisplayMember = "Value";
                    cmbThemes.ValueMember = "Key";
                }
            }
            if (!cmbThemes.Enabled)
            {
                cmbThemes.SelectedIndex = -1;
            }
            if (cmbWeatherTypes.SelectedIndex > 0)
            {
                UpdateWeather();
            }
            else if (cmbThemes.SelectedIndex == -1)
            {
                cmbThemes.SelectedIndex = 0;
            }
            if (cmbWeatherTypes.SelectedIndex == 0)
            {
                timer1.Stop();
                lblIFRConditions.Visible = false;
            }
        }

        private void cmbThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbThemes.Enabled && cmbThemes.SelectedIndex > -1)
            {
                txtRoute.Find(_departure.AirportId, RichTextBoxFinds.MatchCase);
                txtRoute.SelectionBackColor = Color.White;
                txtRoute.SelectionColor = Color.Black;

                txtRoute.Find(_destination.AirportId, RichTextBoxFinds.MatchCase);
                txtRoute.SelectionBackColor = Color.White;
                txtRoute.SelectionColor = Color.Black;
            }
        }

        private void chkIncludeTOC_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.IncludeTOC = chkIncludeTOC.Checked;
            Properties.Settings.Default.Save();
        }

        private void chkIncludeTOD_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.IncludeTOD = chkIncludeTOD.Checked;
            Properties.Settings.Default.Save();
        }

        private async void aircraftEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var idx = cmbAircraft.SelectedIndex;
            var frm = new AircraftEditor(_aircraft);
            frm.DataPath = Common.DataPath;
            frm.FS_Path = _fsPath;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                SetAppPath();

                _aircraft = await DataHelper.GetAircraft();

                cmbAircraft.DataSource = _aircraft;
                cmbAircraft.DisplayMember = "Name";
                cmbAircraft.ValueMember = "Texture";
                cmbAircraft.SelectedIndex = idx;
            }
        }

        private void SetAppPath(bool reset = false)
        {
            if (reset || string.IsNullOrEmpty(appPath))
            {
                var path = string.Empty;
                var file = string.Empty;
                switch (Common.FlightSim)
                {
                    case FlightSimType.FSX:
                        path = @"Microsoft\FSX\";
                        file = "FSX.CFG";
                        break;
                    case FlightSimType.FSXSE:
                        path = @"Microsoft\FSX-SE\";
                        file = "FSX_SE.CFG";
                        break;
                    case FlightSimType.P3D:
                        path = @"Lockheed Martin\Prepar3D v4\";
                        file = "prepar3d.cfg";
                        break;
                }
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                //Find the config file and change the setting to not show the Initial Dialog
                appPath = string.Empty;
                if (!string.IsNullOrEmpty(appDataFolder))
                {
                    appPath = appDataFolder + @"\" + path;
                    if (Common.FlightSim == FlightSimType.P3D)
                    {
                        if (!Directory.Exists(appPath))
                        {
                            appPath = appDataFolder + @"\Prepar3D v3\";
                            if (!Directory.Exists(appPath))
                            {
                                appPath = appDataFolder + @"\Prepar3D v2\";
                                if (!Directory.Exists(appPath))
                                {
                                    appPath = appDataFolder + @"\Prepar3D\";
                                }
                            }
                        }
                    }
                }
            }
        }

        private async void FullUpdate()
        {
            var dbOk = false;
            var message = string.Empty;
            var lbl = string.Empty;
            using (var context = new FSFBDbConn(Common.DBConnName, Common.DataPath))
            {
                try
                {
                    dbOk = context.Airports.Any(fs => fs.LongestRwyLength > 0 && fs.FSType == (int)Common.FlightSim) && context.Runways.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Parkings.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Comms.Any(fs => fs.FSType == (int)Common.FlightSim) &&
                        context.Aircraft.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Navaids.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Waypoints.Any(w => w.FSType == (int)Common.FlightSim) && context.Routes.Any(fs => fs.FSType == (int)Common.FlightSim);
                }
                catch
                {
                    dbOk = false;
                }
            }

            if (dbOk)
            {
                message = "The database update process may take several minutes.  Are you sure you want to continue?";
                lbl = "Update Full Database";
                Common.PriorFlightSim = FlightSimType.Unknown;
            }
            else
            {
                message = "Some data files were not found. The database will need to be updated to restore the missing data. This process may take several" +
                    " minutes to finish. Do you want to continue?";
                lbl = "Database Update Required";
            }
            if (MessageBox.Show(message, lbl, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Enabled = false;
                if (Common.CheckPaths())
                {
                    bglImporter = new BGLImporter();
                    if (bglImporter.ShowDialog() != DialogResult.OK)
                    {
                        UseWaitCursor = true;

                        //Try to delete the navdata database
                        try
                        {
                            if (File.Exists($"{Common.DataPath}\\navdata.db"))
                            {
                                File.Delete($"{Common.DataPath}\\navdata.db");
                            }
                        }
                        catch { }

                        using (var context = new FSFBDbConn(Common.DBConnName, Common.DataPath))
                        {
                            dbOk = context.Airports.Any(fs => fs.LongestRwyLength > 0 && fs.FSType == (int)Common.FlightSim) && context.Runways.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Parkings.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Comms.Any(fs => fs.FSType == (int)Common.FlightSim) &&
                                context.Aircraft.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Navaids.Any(fs => fs.FSType == (int)Common.FlightSim) && context.Waypoints.Any(w => w.FSType == (int)Common.FlightSim) && context.Routes.Any(fs => fs.FSType == (int)Common.FlightSim);
                        }
                        if (!dbOk)
                        {
                            if (Common.PriorFlightSim != FlightSimType.Unknown)
                            {
                                //Change the Flight Sim back to the original one
                                SetFlightSim(Common.PriorFlightSim);
                                switch (Common.FlightSim)
                                {
                                    case FlightSimType.P3D:
                                        MessageBox.Show("Selected flight simulator changed back to Prepar3D.");
                                        break;
                                    case FlightSimType.FSXSE:
                                        MessageBox.Show("Selected flight simulator changed back to FSX Steam Edition.");
                                        break;
                                    case FlightSimType.FSX:
                                        MessageBox.Show("Selected flight simulator changed back to FSX.");
                                        break;
                                }
                            }
                            else
                            {
                                //Exit
                                MessageBox.Show(@"An error occured while building the database. Please re-launch the application and try again." +
                                    Environment.NewLine + Environment.NewLine + "If you believe this is an error, please contact the developer at tarduini@arduiniwebdevelopment.com",
                                     @"Database Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (File.Exists(Common.appPath + @"\FSFlightBuilder.ini"))
                                {
                                    File.Delete(Common.appPath + @"\FSFlightBuilder.ini");
                                }
                                Environment.Exit(-1);
                            }

                        }
                        else
                        {
                            await FillLists();
                        }
                        UseWaitCursor = false;
                    }
                    else
                    {
                        UseWaitCursor = true;
                        await FillLists();
                        UseWaitCursor = false;
                    }
                }

                Enabled = true;
            }
            else if (Common.PriorFlightSim != FlightSimType.Unknown)
            {
                //Change the Flight Sim back to the original one
                SetFlightSim(Common.PriorFlightSim);
                switch (Common.FlightSim)
                {
                    case FlightSimType.P3D:
                        MessageBox.Show("Selected flight simulator changed back to Prepar3D.");
                        break;
                    case FlightSimType.FSXSE:
                        MessageBox.Show("Selected flight simulator changed back to FSX Steam Edition.");
                        break;
                    case FlightSimType.FSX:
                        MessageBox.Show("Selected flight simulator changed back to FSX.");
                        break;
                }
            }
        }

        private void SetFlightSim(FlightSimType priorFlightSim)
        {
            Properties.Settings.Default.FlightSim = priorFlightSim.ToString();
            Common.FlightSim = priorFlightSim;
            Common.PriorFlightSim = FlightSimType.Unknown;
            Common.CheckPaths();

            SetAppPath(true);
            switch (Common.FlightSim)
            {
                case FlightSimType.MSFS:
                    btnLaunchFS.Text = @"MSFS Disabled";
                    Text = "FS Flight Builder - Microsoft Flight Simulator";
                    break;
                case FlightSimType.P3D:
                    btnLaunchFS.Text = @"Launch Prepar3D";
                    Text = "FS Flight Builder - Prepar3D";
                    break;
                case FlightSimType.FSXSE:
                    btnLaunchFS.Text = @"Launch FSX Steam Edition";
                    Text = "FS Flight Builder - FSX Steam Edition";
                    break;
                case FlightSimType.FSX:
                    btnLaunchFS.Text = @"Launch FSX";
                    Text = "FS Flight Builder - FSX";
                    break;
            }

        }

        private bool UpdateAircraft()
        {
            if (string.IsNullOrEmpty(appPath))
            {
                SetAppPath();
            }

            if (!string.IsNullOrEmpty(appPath))
            {
                List<string> aircraftFiles = new List<string>();
                Common.GetAllAircraft(ref aircraftFiles);
                _aircraft = Common.ParseAircraft(aircraftFiles);
                if (Common.SaveAircraft(_aircraft))
                {
                    cmbAircraft.DataSource = _aircraft;
                    cmbAircraft.DisplayMember = "Name";
                    cmbAircraft.ValueMember = "Texture";
                }
                return true;
            }
            else
            {
                MessageBox.Show(
                    @"Something happened to prevent me from determining the Flight Simulator.  Please select the Flight Simulator from the Options menu.");
                return false;
            }
        }

        private void destinationChooserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new DestinationChooser();
            frm.DataPath = Common.DataPath;
            frm.FS_Path = _fsPath;

            if (_departure != null && !string.IsNullOrEmpty(txtRoute.Text))
            {
                frm.Departure = _departure.AirportId;
            }
            else if (!string.IsNullOrEmpty(txtRoute.Text))
            {
                var idx = txtRoute.Text.IndexOf(" ");
                if (idx > -1)
                {
                    frm.Departure = txtRoute.Text.Substring(0, idx);
                }
                else
                {
                    frm.Departure = txtRoute.Text;
                }
            }

            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(frm.Departure) && !string.IsNullOrEmpty(frm.Destination))
                {
                    txtRoute.Text = frm.Departure + " " + frm.Destination;
                    txtRoute_Leave(null, null);
                    txtAltitude.Select();
                }
            }
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    DialogResult dialogResult;
                    if (args.Mandatory.Value)
                    {
                        dialogResult =
                            MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. This is a required update. Click Ok to begin updating the application.", @"Update Available",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                    }
                    else
                    {
                        dialogResult =
                            MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. Would you like to update the application now?", @"Update Available",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);
                    }

                    if (dialogResult.Equals(DialogResult.Yes))
                    {
                        try
                        {
                            if (AutoUpdater.DownloadUpdate(args))
                            {
                                Application.Exit();
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
                else if (updatesFromMenu)
                {
                    MessageBox.Show(@"You have the most recent version of FS Flight Builder.", @"No update available",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    updatesFromMenu = false;
                }
            }
            else
            {
                MessageBox.Show(
                        @"There is a problem reaching update server. Please check your internet connection and try again later.",
                        @"Update check failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            destinationChooserToolStripMenuItem.Enabled = false;
            destinationChooserToolStripMenuItem.ToolTipText = "Reading airports.  The Destination Chooser will be available in a few seconds.";
            _ = DataHelper.GetAllAirports();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            destinationChooserToolStripMenuItem.ToolTipText = "";
            destinationChooserToolStripMenuItem.Enabled = true;
        }

        private void txtAltitude_Enter(object sender, EventArgs e)
        {
            txtAltitude.Select(0, txtAltitude.Text.Length);
        }

        private void txtSpeed_Enter(object sender, EventArgs e)
        {
            txtSpeed.Select(0, txtSpeed.Text.Length);

        }

        private async void FinishUp()
        {
            await FillLists();

            ClearData();

            backgroundWorker1.RunWorkerAsync();
        }

        private void btnSkyVector_Click(object sender, EventArgs e)
        {
            var txtAlt = string.Empty;
            if (txtAltitude.Value > 0)
            {
                txtAlt = "A" + Common.FormatAltitude(txtAltitude.Value) + " ";
            }
            var txtSpd = string.Empty;
            if (txtSpeed.Value > 0)
            {
                txtSpd = "N" + Common.FormatSpeed(txtSpeed.Value);
            }
            Process.Start(new ProcessStartInfo($"https://skyvector.com/?fpl={txtSpd + txtAlt + txtRoute.Text.Replace("TOD ", "")}") { UseShellExecute = true });
        }

        private void btnPrintBriefing_Click(object sender, EventArgs e)
        {
            wbBriefing.ShowPrintDialog();
        }

        private void btnBuildPlan_StyleChanged(object sender, EventArgs e)
        {
            var st = btnBuildPlan.FlatStyle;
        }

        private void GetDepDestData()
        {
            _airways.Clear();
            _routepoints = txtRoute.Text.Split(' ').ToList();
            if (_routepoints.Count < 2)
            {
                return;
            }

            using (new WaitCursor())
            {
                _departure = DataHelper.GetAirport(_routepoints[0].Trim()); // _airports.FirstOrDefault(a => a.AirportId == );
                                                                            //_departure.Runways = runs.ToList();
                if (_departure == null)
                {
                    MessageBox.Show(
                        @"Please check the departure ICAO for your flight.  " + _routepoints[0] +
                        @" was not found in the database." + Environment.NewLine + Environment.NewLine +
                        @"If it was added recently, you may need to update the database.",
                        @"Missing Departure Airport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _destination = DataHelper.GetAirport(_routepoints[_routepoints.Count - 1].Trim());
                if (_destination == null)
                {
                    MessageBox.Show(
                        @"Please check the destination ICAO for your flight.  " + _routepoints[_routepoints.Count - 1] +
                        @" was not found in the database." + Environment.NewLine + Environment.NewLine +
                        @"If it was added recently, you may need to update the database.",
                        @"Missing Destination Airport", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ResetParking(_routepoints[0]);
                UpdateWeather(_routepoints[0], _routepoints[_routepoints.Count - 1]);
                btnBuildPlan.Enabled = BuildRoute() && txtAltitude.Value > 0 && txtSpeed.Value > 0;
                if (btnBuildPlan.Enabled)
                {
                    //Change the color to blue
                    btnBuildPlan.BackColor = Color.LightBlue;
                }
            }
        }

        private void SimStartup()
        {
            var useSystem = Properties.Settings.Default.UseSystem;
            var flightTime = Properties.Settings.Default.LastFlight;
            chkIncludeTOC.Checked = chkIncludeTOD.Checked = false;
            timer1.Tick += OnTimerTick;
            var opts = Common.SimStartup();
            if (opts == null)
            {
                return;
            }
            chkSystem.Checked = useSystem || string.IsNullOrEmpty(flightTime);
            var dt = chkSystem.Checked ? DateTime.Now : opts.FlightDate;
            dtFlight.Value = dt;

            if (opts.WeatherTypes.Count > 0)
            {
                cmbWeatherTypes.Items.AddRange(opts.WeatherTypes.ToArray());
            }

            chkIncludeTOD.Checked = opts.IncludeTOD;

            UpdateButtons();
            Common.logger.Info("Config complete.Flight sim is {0}", Common.FlightSim);
        }
    }
}
