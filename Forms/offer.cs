using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class offer: Form
    {
        OfferManager offerManager= new OfferManager();
        public offer()
        {
            InitializeComponent();
            LoadCompany();
        }

        private void LoadCompany()
        {
            dataGridView1.DataSource = offerManager.GetOffer();
            SetupGridColumnProperties();
            SetupOfferGridColumns();
        }

        private void SetupGridColumnProperties()
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.Resizable = DataGridViewTriState.False;
            }
        }

        private void SetupOfferGridColumns()
        {
            if (dataGridView1.Columns["teklif_id"] != null)
            {
                dataGridView1.Columns["teklif_id"].HeaderText = "Teklif No";
                dataGridView1.Columns["teklif_id"].Visible = false;
            }

            if (dataGridView1.Columns["adi"] != null)
                dataGridView1.Columns["adi"].HeaderText = "Firma";

            if (dataGridView1.Columns["isim"] != null)
                dataGridView1.Columns["isim"].HeaderText = "Yetkili İsmi";

            if (dataGridView1.Columns["soyisim"] != null)
                dataGridView1.Columns["soyisim"].HeaderText = "Yetkili Soyismi";

            if (dataGridView1.Columns["hitap"] != null)
                dataGridView1.Columns["hitap"].HeaderText = "Hitap";

            if (dataGridView1.Columns["teklif_tarih"] != null)
                dataGridView1.Columns["teklif_tarih"].HeaderText = "Teklif Tarihi";

            if (dataGridView1.Columns["durum"] != null)
                dataGridView1.Columns["durum"].HeaderText = "Durum";
        }

        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            //offerEditor offerEditor = new offerEditor(null, "Add");
            //offerEditor.ShowDialog();
            //LoadCompany();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            if (TextboxValidator.IsNullOrWhiteSpace(txtSearch))
            {
                MessageHelper.ShowError("Arama alanı boş bırakılamaz.");
            }
            else
            {
                if (offerManager.Search(txtSearch.Text) != null)
                {
                    dataGridView1.DataSource = offerManager.Search(txtSearch.Text);
                    SetupGridColumnProperties();
                    SetupOfferGridColumns();
                }
                else
                {
                    dataGridView1.DataSource = null;
                    MessageHelper.ShowError("Aramaya uygun yetkili bulunamadı.");
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
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
                    offerEditor editor = new offerEditor(value, "Edit");
                    editor.ShowDialog();
                    LoadCompany();
                }
                else if (result == CustomMessageBox.CustomResult.Sil)
                {
                    var confirm = MessageBox.Show("Bu şirketi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        //offerEditor.DeleteCompany(value.Value);
                        LoadCompany();
                    }
                }
            }
        }
    }
}
