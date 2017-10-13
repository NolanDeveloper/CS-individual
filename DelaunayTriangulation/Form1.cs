using System.Windows.Forms;

namespace DelaunayTriangulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

		private void btnDemo_Click(object sender, System.EventArgs e)
		{
			triangluationPlotter1.Demo();
		}
	}
}
