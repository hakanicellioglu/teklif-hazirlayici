using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Business;
using Teklif_Hazırlayıcı.Forms.Editor;
using Teklif_Hazırlayıcı.Helpers;
using Teklif_Hazırlayıcı.Validation;

namespace Teklif_Hazırlayıcı.Forms
{
    public partial class company : Form
    {
        CompanyManager CompanyManager = new CompanyManager();
        public company()
        {
            InitializeComponent();
            LoadCompany();
        }

        private void LoadCompany()
        {
            dataGridView1.DataSource = CompanyManager.GetCompany();
            SetupGridColumnProperties();
            SetupCompanyGridColumns();
        }


        private void SetupGridColumnProperties()
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.Resizable = DataGridViewTriState.False;
            }
        }

        private void SetupCompanyGridColumns()
        {
            if (dataGridView1.Columns["firma_id"] != null)
            {
                dataGridView1.Columns["firma_id"].HeaderText = "Firma No";
                dataGridView1.Columns["firma_id"].Visible = false; // gizlemek istiyorsan
            }

            if (dataGridView1.Columns["adi"] != null)
                dataGridView1.Columns["adi"].HeaderText = "Firma Adı";

            if (dataGridView1.Columns["adres"] != null)
                dataGridView1.Columns["adres"].HeaderText = "Adres";

            if (dataGridView1.Columns["telefon"] != null)
                dataGridView1.Columns["telefon"].HeaderText = "Telefon";

            if (dataGridView1.Columns["eposta"] != null)
                dataGridView1.Columns["eposta"].HeaderText = "E-posta";
        }

        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            companyEditor companyEditor = new companyEditor("Add");
            companyEditor.ShowDialog();
            LoadCompany();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (TextboxValidator.IsNullOrWhiteSpace(txtSearch))
            {
                MessageHelper.ShowError("Arama alanı boş bırakılamaz.");
            }
            else
            {
                if (CompanyManager.Search(txtSearch.Text) != null)
                {
                    CompanyManager.Search(txtSearch.Text);
                    SetupGridColumnProperties();
                    SetupCompanyGridColumns();
                }
                else
                {
                    dataGridView1.DataSource = null;
                    MessageHelper.ShowError("Aramaya uygun firma bulunamadı.");
                }
            }


        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            dataGridView1.DataSource = null;
            LoadCompany();
        }
    }
}
