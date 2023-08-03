using FSFlightBuilder.Components;
using System;
using System.Windows.Forms;

namespace FSFlightBuilder
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            var about = new ResourceHelpers().GetResourceTextFile("About.html");
            wbAbout.DocumentText = about;

            lblVersion.Text = @"Version " + Application.ProductVersion;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
