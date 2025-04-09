using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teklif_Hazırlayıcı.Forms
{
    public partial class dashboard: Form
    {
        public dashboard()
        {
            InitializeComponent();
        }

        private void LoadForm(Form formToLoad)
        {
            panel2.Controls.Clear();
            formToLoad.TopLevel = false;
            formToLoad.Dock = DockStyle.Fill;
            panel2.Controls.Add(formToLoad);
            formToLoad.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {

        }

        private void btnCompany_Click(object sender, EventArgs e)
        {
            LoadForm(new company());
        }
    }
}
