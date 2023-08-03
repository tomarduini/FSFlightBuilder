//using atools.fs.utils;
using FSFlightBuilder.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FSFlightBuilder
{
    public partial class AircraftEditor : Form
    {
        private Data.Models.Aircraft acft = null;

        internal Data.Models.Aircraft SelectedAircraft { get; set; }

        public string FS_Path { get; set; }
        public string DataPath { get; set; }
        private List<Data.Models.Aircraft> _aircraft;
        public List<Data.Models.Aircraft> Aircraft { get; set; }

        public AircraftEditor(List<Data.Models.Aircraft> aircraft)
        {
            InitializeComponent();
            _aircraft = aircraft.GroupBy(a => new { a.UIType })
                                                      .Select(a => a.First())
                                                      .OrderBy(a => a.UIType).ToList();
            Aircraft = new List<Data.Models.Aircraft>(_aircraft);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //if (_aircraft != null)
            //{
            //    //get all aircraft/liveries
            //    var aircraft = await DataHelper.GetAircraft();
            //    foreach (var acft in _aircraft)
            //    {
            //        //Update all aircraft
            //        var dbAircraft = aircraft.Where(a => a.UIType.ToLower() == acft.UIType.ToLower()).ToList();
            //        dbAircraft.ForEach(s => s.Airspeed = acft.Airspeed);
            //        dbAircraft.ForEach(s => s.ClimbSpeed = acft.ClimbSpeed);
            //        dbAircraft.ForEach(s => s.ClimbRate = acft.ClimbRate);
            //        dbAircraft.ForEach(s => s.DescentSpeed = acft.DescentSpeed);
            //        dbAircraft.ForEach(s => s.DescentRate = acft.DescentRate);
            //        aircraft.
            //    }









            //    foreach (var acft in aircraft)
            //    {
            //        var a = _aircraft.FirstOrDefault(ac => (int)ac.Id == (int)acft.Id);
            //        if (a != null)
            //        {
            //            if (AWDConvert.ToDecimal(a.DescentRate) > 0 || AWDConvert.ToDecimal(a.DescentSpeed) > 0)
            //            {
            //                acft.DescentRate = a.DescentRate;
            //                acft.DescentSpeed = a.DescentSpeed;
            //            }
            //            acft.Airspeed = a.Airspeed;
            //        }
            //    }
            DataHelper.SaveAircraft(_aircraft);
            Aircraft = _aircraft; // = aircraft;
            //}
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AircraftEditor_Load(object sender, EventArgs e)
        {
            string uiType = string.Empty;
            treeAircraft.Nodes.Clear();
            foreach (var acft in _aircraft)
            {
                //var acftName = acft.UIType;
                //if (acftName != name)
                //{
                //    name = acftName;
                treeAircraft.Nodes.Add(new TreeNode(acft.UIType));
                //}
            }

            //automatically check the selected charts
            if (SelectedAircraft != null)
            {
                foreach (TreeNode node in treeAircraft.Nodes)
                {
                    if (node.Text == SelectedAircraft.UIType)
                    {
                        treeAircraft.SelectedNode = node;
                        break;
                    }
                }
            }

            treeAircraft.ExpandAll();
        }

        private void treeAircraft_AfterSelect(object sender, TreeViewEventArgs e)
        {
            acft = _aircraft.FirstOrDefault(a => a.UIType.ToLower() == treeAircraft.SelectedNode.Text.ToLower());
            if (acft != null)
            {
                numClimbRate.Value = AWDConvert.ToDecimal(acft.ClimbRate);
                numClimbSpeed.Value = AWDConvert.ToDecimal(acft.ClimbSpeed);
                numDescentRate.Value = AWDConvert.ToDecimal(acft.DescentRate);
                numDescentSpeed.Value = AWDConvert.ToDecimal(acft.DescentSpeed);
                numCruiseSpeed.Value = AWDConvert.ToDecimal(acft.Airspeed);
            }

        }

        private void numClimbSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (treeAircraft.SelectedNode != null)
            {
                var items = _aircraft.Where(a => a.UIType.ToLower() == treeAircraft.SelectedNode.Text.ToLower());
                foreach (var acft in items)
                {
                    acft.ClimbSpeed = numClimbSpeed.Value;
                }
            }
        }

        private void numClimbRate_ValueChanged(object sender, EventArgs e)
        {
            if (treeAircraft.SelectedNode != null)
            {
                var items = _aircraft.Where(a => a.UIType.ToLower() == treeAircraft.SelectedNode.Text.ToLower());
                foreach (var acft in items)
                {
                    acft.ClimbRate = numClimbRate.Value;
                }
            }
        }

        private void numDescentSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (treeAircraft.SelectedNode != null)
            {
                var items = _aircraft.Where(a => a.UIType.ToLower() == treeAircraft.SelectedNode.Text.ToLower());
                foreach (var acft in items)
                {
                    acft.DescentSpeed = numDescentSpeed.Value;
                }
            }
        }

        private void numDescentRate_ValueChanged(object sender, EventArgs e)
        {
            if (treeAircraft.SelectedNode != null)
            {
                var items = _aircraft.Where(a => a.UIType.ToLower() == treeAircraft.SelectedNode.Text.ToLower());
                foreach (var acft in items)
                {
                    acft.DescentRate = numDescentRate.Value;
                }
            }
        }

        private void numCruiseSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (treeAircraft.SelectedNode != null)
            {
                var items = _aircraft.Where(a => a.UIType.ToLower() == treeAircraft.SelectedNode.Text.ToLower());
                foreach (var acft in items)
                {
                    acft.Airspeed = numCruiseSpeed.Value;
                }
            }
        }

        private void AircraftEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_aircraft != Aircraft)
            {
                if (MessageBox.Show("Changes will be lost.  Do you want to exit with saving your changes?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void numClimbRate_Enter(object sender, EventArgs e)
        {
            numClimbRate.Select(0, numClimbRate.Text.Length);
        }

        private void numClimbSpeed_Enter(object sender, EventArgs e)
        {
            numClimbSpeed.Select(0, numClimbSpeed.Text.Length);
        }

        private void numDescentRate_Enter(object sender, EventArgs e)
        {
            numDescentRate.Select(0, numDescentRate.Text.Length);
        }

        private void numDescentSpeed_Enter(object sender, EventArgs e)
        {
            numDescentSpeed.Select(0, numDescentSpeed.Text.Length);
        }

        private string GetUIType(string uiType)
        {
            var idx = uiType.IndexOf(" (");
            return uiType;
        }
    }
}
