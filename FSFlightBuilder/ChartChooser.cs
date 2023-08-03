using FSFlightBuilder.Components;
using FSFlightBuilder.Enums;
using FSFlightBuilder.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FSFlightBuilder
{
    public partial class ChartChooser : Form
    {
        private readonly string _dep;
        private readonly string _dest;
        private readonly string _fileName;
        private string _chartService;
        private string _dataPath;

        public List<Chart> selectedCharts = new List<Chart>();
        public IEnumerable<Chart> allCharts { get; set; }

        public ChartChooser(string departure, string destination, string fName, string path, string chartService, bool usOnly)
        {
            InitializeComponent();
            _dep = departure;
            _dest = destination;
            _fileName = fName;
            _chartService = chartService;
            if (chartService == "FAA")
            {
                Text = "FAA Charts (USA Only)";
            }
            _dataPath = path;
        }

        private void ChartChooser_Load(object sender, EventArgs e)
        {
            IChartProvider provider;
            if (_chartService == "AVIATIONAPI")
            {
                provider = new AviationAPIProvider();
            }
            else
            {
                provider = new FAAChartProvider(_dataPath);
            }
            allCharts = provider.GetCharts(_dep, _dest);

            for (var i = 0; i < 4; i++)
            {
                TreeView tree;
                string apt;
                if (i < 2)
                {
                    tree = treeDeparture;
                    apt = _dep;
                }
                else
                {
                    tree = treeDestination;
                    apt = _dest;
                }

                if (Char.IsNumber(apt[0]))
                {
                    apt = "K" + apt;
                }

                if (i == 0 || i == 2)
                {
                    tree.Nodes.Clear();
                }
                IEnumerable<Chart> airportCharts;
                if (i == 0 || i == 2)
                {
                    airportCharts = allCharts.Where(c => c.ICAO == apt && c.ChartType != ChartTypes.AFD).OrderBy(o => o.ChartType).OrderBy(o => o.Runway);
                }
                else
                {
                    airportCharts = allCharts.Where(c => c.ICAO == apt && c.ChartType == ChartTypes.AFD);
                }

                var icao = string.Empty;
                ChartTypes chartType = ChartTypes.None;
                var runway = string.Empty;
                TreeNode icaoNode = null;
                TreeNode typeNode = null;
                TreeNode runwayNode = null;
                foreach (var chart in airportCharts)
                {
                    if (icao != chart.ICAO)
                    {
                        if (tree.Nodes.Count == 0)
                        {
                            tree.Nodes.Add(
                            new TreeNode(chart.AirportName + " (" + chart.ICAO + ")"));
                        }
                        icao = chart.ICAO;
                        chartType = ChartTypes.None;
                        runway = string.Empty;
                        if (tree.Nodes.Count > 0)
                        {
                            icaoNode = tree.Nodes[0];
                        }
                        if (string.IsNullOrEmpty(chart.ChartId))
                        {
                            icaoNode.TreeView.Enabled = false;
                        }
                    }
                    if (!string.IsNullOrEmpty(chart.ChartId))
                    {

                        if (chartType != chart.ChartType)
                        {
                            var tNode = new TreeNode(chart.ChartType.ToString().Replace("_", " "));
                            if (icaoNode != null)
                            {
                                icaoNode.Nodes.Add(tNode);
                            }
                            typeNode = tNode;
                            chartType = chart.ChartType;
                            runway = string.Empty;
                        }
                        if (string.IsNullOrEmpty(chart.Runway))
                        {
                            var tn = new TreeNode
                            {
                                Name = chart.ChartId,
                                Text = chart.ChartName
                            };
                            typeNode.Nodes.Add(tn);
                        }
                        else if (chart.Runway != runway)
                        {
                            var tNode = new TreeNode(chart.Runway);
                            typeNode.Nodes.Add(tNode);
                            runwayNode = tNode;
                            runway = chart.Runway;

                            var tn = new TreeNode
                            {
                                Name = chart.ChartId,
                                Text = chart.ChartName
                            };
                            runwayNode.Nodes.Add(tn);
                        }
                        else
                        {
                            var tn = new TreeNode
                            {
                                Name = chart.ChartId,
                                Text = chart.ChartName
                            };
                            runwayNode.Nodes.Add(tn);
                        }
                    }
                }

            }

            //automatically check the selected charts
            if (!string.IsNullOrEmpty(_fileName) && File.Exists(_fileName))
            {
                var txtReader = new StreamReader(_fileName);
                while (!txtReader.EndOfStream)
                {
                    var chart = txtReader.ReadLine();
                    for (var i = 0; i < 2; i++)
                    {
                        var tree = i == 0 ? treeDeparture : treeDestination;
                        foreach (TreeNode node in tree.Nodes)
                        {
                            foreach (TreeNode n in node.Nodes)
                            {
                                if (n.Nodes.Count > 0)
                                {
                                    foreach (TreeNode n1 in n.Nodes)
                                    {
                                        if (n1.Nodes.Count > 0)
                                        {
                                            foreach (TreeNode n2 in n1.Nodes)
                                            {
                                                if (n2.Nodes.Count > 0)
                                                {
                                                    foreach (TreeNode n3 in n2.Nodes)
                                                    {
                                                        if (n3.Name == chart)
                                                        {
                                                            n3.Checked = true;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (n2.Name == chart)
                                                    {
                                                        n2.Checked = true;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (n1.Name == chart)
                                            {
                                                n1.Checked = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (n.Name == chart)
                                    {
                                        n.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
                txtReader.Close();
            }

            treeDeparture.ExpandAll();
            treeDestination.ExpandAll();
            treeDeparture.Enabled = treeDeparture.Nodes.Count > 0;
            treeDestination.Enabled = treeDestination.Nodes.Count > 0;
        }

        private void CheckTreeViewNode(TreeNode node, bool isChecked)
        {
            foreach (TreeNode item in node.Nodes)
            {
                item.Checked = isChecked;

                if (item.Nodes.Count > 0)
                {
                    CheckTreeViewNode(item, isChecked);
                }
            }
        }

        private void treeDeparture_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckTreeViewNode(e.Node, e.Node.Checked);
        }

        private void treeDestination_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckTreeViewNode(e.Node, e.Node.Checked);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < 2; i++)
            {
                var tree = i == 0 ? treeDeparture : treeDestination;
                foreach (TreeNode node in tree.Nodes)
                {

                    foreach (TreeNode n in node.Nodes)
                    {
                        if (n.Nodes.Count > 0)
                        {
                            foreach (TreeNode n1 in n.Nodes)
                            {
                                if (n1.Nodes.Count > 0)
                                {
                                    foreach (TreeNode n2 in n1.Nodes)
                                    {
                                        if (n2.Checked)
                                        {
                                            selectedCharts.Add(allCharts.FirstOrDefault(c => c.ChartId == n2.Name));
                                        }
                                    }
                                }
                                else
                                {
                                    if (n1.Checked)
                                    {
                                        selectedCharts.Add(allCharts.FirstOrDefault(c => c.ChartId == n1.Name));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (n.Checked)
                            {
                                selectedCharts.Add(allCharts.FirstOrDefault(c => c.ChartId == n.Name));
                            }
                        }
                    }
                }
            }
            DialogResult = DialogResult.OK;
            if (selectedCharts.Count > 0)
            {
                //Set the default AIRAC
                var airacUrl = selectedCharts[0].PDFUrl;
                var idx = airacUrl.IndexOf("AIRAC_");
                var idx2 = airacUrl.LastIndexOf("/");
                if (idx > -1)
                {
                    var airac = airacUrl.Substring(idx + 6, airacUrl.Length - idx2 - 4);
                    Properties.Settings.Default.AIRAC = airac;
                }
            }

            Close();
        }
    }
}
