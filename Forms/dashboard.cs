using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Forms.Editor;


namespace Teklif_Hazırlayıcı.Forms
{
    public partial class dashboard : Form
    {
        public dashboard()
        {
            InitializeComponent();
            ConfigureDashboard();
        }

        private void ConfigureDashboard()
        {
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;
            CenterToScreen();
        }

        private void LoadForm(Form formToLoad)
        {
            panel1.Visible = false;
            panel1.Enabled = false;
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
            panel1.Enabled = true;
            btnHome.Visible = false;
            pnlForm.Controls.Clear();
        }

        private void btnCompany_Click(object sender, EventArgs e)
        {
            LoadForm(new company(new Business.CompanyManager()));
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            LoadForm(new auth(new Business.CompanyManager(), new Business.AuthManager(), new ColumnForm(new Business.AuthManager())));
        }

        private void dashboard_Resize(object sender, EventArgs e)
        {

        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            LoadForm(new product(new Business.ProductManager()));
        }

        private void btnOffer_Click(object sender, EventArgs e)
        {
            LoadForm(new offer(new Business.OfferManager(), new offerEditor(null, "Add", new Business.AuthManager(), new Business.CompanyManager(), new Business.OfferManager())));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}
