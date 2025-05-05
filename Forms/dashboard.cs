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
            panel1.Visible = false;
            btnHome.Visible = true;

            if (pnlForm.Controls.Count > 0)
            {
                var oldForm = pnlForm.Controls[0] as Form;
                oldForm?.Dispose();
                pnlForm.Controls.Clear();
            }

            formToLoad.TopLevel = false;
            formToLoad.Dock = DockStyle.Fill;
            pnlForm.Controls.Add(formToLoad);
            formToLoad.Show();
        }


        private void btnHome_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            btnHome.Visible = false;
            pnlForm.Controls.Clear();
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

        private void btnOffer_Click(object sender, EventArgs e)
        {
            LoadForm(new offer());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
