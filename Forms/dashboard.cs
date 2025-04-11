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
            pnlForm.Controls.Clear();
            formToLoad.TopLevel = false;
            formToLoad.Dock = DockStyle.Fill;
            pnlForm.Controls.Add(formToLoad);
            formToLoad.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {

        }

        private void btnCompany_Click(object sender, EventArgs e)
        {
            LoadForm(new company());
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            LoadForm(new auth());
        }

        private void dashboard_Resize(object sender, EventArgs e)
        {

        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            LoadForm(new product());
        }
    }
}
