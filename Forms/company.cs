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
            companyEditor companyEditor = new companyEditor(null, "Add");
            companyEditor.ShowDialog();
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
                var result = CompanyManager.Search(txtSearch.Text);

                if (result != null)
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
                int? value = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString());
                var result = CustomMessageBox.Show("Bu şirketi düzenlemek veya silmek istiyor musunuz?");

                if (result == CustomMessageBox.CustomResult.Duzenle)
                {
                    companyEditor editor = new companyEditor(value, "Edit");
                    editor.ShowDialog();
                    LoadCompany();
                }
                else if (result == CustomMessageBox.CustomResult.Sil)
                {
                    var confirm = MessageBox.Show("Bu şirketi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        CompanyManager.DeleteCompany(value.Value);
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
    }
}
