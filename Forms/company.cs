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
using Teklif_Hazırlayıcı.Forms.Custom_Item;
using Teklif_Hazırlayıcı.Forms.Editor;
using Teklif_Hazırlayıcı.Helpers;
using Teklif_Hazırlayıcı.Validation;
using Teklif_Hazırlayıcı.Models;
using Teklif_Hazırlayıcı.Properties;

namespace Teklif_Hazırlayıcı.Forms
{
    public partial class company : Form
    {
        private readonly CompanyManager _companyManager;
        public company(CompanyManager companyManager)
        {
            InitializeComponent();
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            _companyManager = companyManager;
            LoadCompany();
        }

        private void LoadCompany()
        {
            dataGridView1.DataSource = _companyManager.GetCompany();
            SetupGridColumnProperties();
            SetupCompanyGridColumns();
        }

        private void SetupGridColumnProperties()
        {
            DataGridHelper.SetupGridColumnProperties(dataGridView1);
        }

        private void SetupCompanyGridColumns()
        {
            if (dataGridView1.Columns["FirmaId"] != null)
            {
                dataGridView1.Columns["FirmaId"].HeaderText = "Firma No";
                dataGridView1.Columns["FirmaId"].Visible = false;
            }

            if (dataGridView1.Columns["Isim"] != null)
                dataGridView1.Columns["Isim"].HeaderText = "Firma Adı";

            if (dataGridView1.Columns["Adres"] != null)
                dataGridView1.Columns["Adres"].HeaderText = "Adres";

            if (dataGridView1.Columns["Telefon"] != null)
                dataGridView1.Columns["Telefon"].HeaderText = "Telefon";

            if (dataGridView1.Columns["Eposta"] != null)
                dataGridView1.Columns["Eposta"].HeaderText = "E-posta";
        }
        

        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            using (var companyEditor = new companyEditor(null, "Add", new CompanyManager())) 
            {
                companyEditor.ShowDialog();
            }
            LoadCompany();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            if (TextboxValidator.IsNullOrWhiteSpace(txtSearch) || txtSearch.Text == "Firma arayın...")
            {
                MessageHelper.ShowError("Arama alanı boş bırakılamaz. Tüm firmalar yükleniyor.");
                LoadCompany();
            }
            else
            {
                var result = _companyManager.Search(txtSearch.Text);

                if (result != null && result.Count > 0)
                {
                    dataGridView1.DataSource = result;
                    SetupGridColumnProperties();
                    SetupCompanyGridColumns();
                }
                else
                {
                    MessageHelper.ShowError("Aramaya uygun firma bulunamadı.");
                }

            }
        }

        PlaceHolder placeHolder = new PlaceHolder("Firma arayın...");

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            placeHolder.LeavePlaceHolder(txtSearch);
            dataGridView1.DataSource = null;
            LoadCompany();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int? value = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["FirmaId"].Value?.ToString());
                var result = CustomMessageBox.Show("Bu şirketi düzenlemek veya silmek istiyor musunuz?");

                if (result == CustomMessageBox.CustomResult.Duzenle)
                {
                    using (var editor = new companyEditor(value, "Edit", new CompanyManager())) 
                    {
                        editor.ShowDialog();
                    }
                    LoadCompany();
                }
                else if (result == CustomMessageBox.CustomResult.Sil)
                {
                    var confirm = MessageBox.Show("Bu şirketi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        _companyManager.DeleteCompany(value.Value);
                        LoadCompany();
                    }
                }
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnSearch_Click(sender, e);

        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            placeHolder.EnterPlaceHolder(txtSearch);

        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            placeHolder.LeavePlaceHolder(txtSearch);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadCompany();
        }
    }
}
