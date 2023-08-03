using FSFlightBuilder.Components;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FSFlightBuilder
{
    public partial class PopupMessage : Form
    {
        public Components.FlightSimType SelectedFlightSim { get; set; }

        public PopupMessage(string title, string label, List<FlightSimType> flightSims)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(title))
            {
                Text = title;
            }
            label1.Text = label;
            comboBox1.Items.Clear();
            foreach (var fs in flightSims)
            {
                switch (fs)
                {
                    case FlightSimType.MSFS:
                        comboBox1.Items.Add("Microsoft Flight Simulator");
                        break;
                    case FlightSimType.P3D:
                        comboBox1.Items.Add("Prepar3D");
                        break;
                    case FlightSimType.FSXSE:
                        comboBox1.Items.Add("Microsoft FSX Steam Edition");
                        break;
                    case FlightSimType.FSX:
                        comboBox1.Items.Add("Microsoft FSX");
                        break;
                }
            }
            SelectedFlightSim = FlightSimType.Unknown;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void PopupMessage_Load(object sender, EventArgs e)
        {
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "Microsoft Flight Simulator":
                    SelectedFlightSim = FlightSimType.MSFS;
                    break;
                case "Prepar3D":
                    SelectedFlightSim = FlightSimType.P3D;
                    break;
                case "Microsoft FSX Steam Edition":
                    SelectedFlightSim = FlightSimType.FSXSE;
                    break;
                case "Microsoft FSX":
                    SelectedFlightSim = FlightSimType.FSX;
                    break;
            }
            if (SelectedFlightSim != FlightSimType.Unknown)
            {
                this.DialogResult = DialogResult.Yes;
                Close();
            }
            else
            {
                MessageBox.Show(this, "Please select a flight simulator from the list");
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
