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
            LoadOffer();
        }

        private void LoadOffer()
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

            if (dataGridView1.Columns["yetkili_id"] != null)
            {
                dataGridView1.Columns["yetkili_id"].HeaderText = "Yetkili No";
                dataGridView1.Columns["yetkili_id"].Visible = false;
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

        PlaceHolder placeHolder = new PlaceHolder("Teklif arayın...");
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            placeHolder.LeavePlaceHolder(txtSearch);
            dataGridView1.DataSource = null;
            LoadOffer();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int teklifId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["teklif_id"].Value);
                var result = CustomMessageBox.Show("Bu teklifi düzenlemek veya silmek istiyor musunuz?");

                if (result == CustomMessageBox.CustomResult.Duzenle)
                {
                    Parent.Parent.Hide();
                    offerEditor editor = new offerEditor(teklifId, "Edit");
                    editor.Width = Screen.PrimaryScreen.WorkingArea.Width;
                    editor.Height = Screen.PrimaryScreen.WorkingArea.Height;
                    editor.ShowDialog();
                    Parent.Parent.Show();
                    LoadOffer();
                    
                }
                else if (result == CustomMessageBox.CustomResult.Sil)
                {
                    var confirm = MessageHelper.ShowQuestion("Bu teklifi ve ilişkili tüm ürünleri silmek istediğinize emin misiniz?");
                    if (confirm == DialogResult.Yes)
                    {
                        OfferManager manager = new OfferManager();
                        if (manager.DeleteOffer(teklifId))
                        {
                            MessageHelper.ShowInfo("Teklif başarıyla silindi.");
                            LoadOffer();
                        }
                        else
                        {
                            MessageHelper.ShowError("Silme işlemi başarısız oldu.");
                        }
                    }
                }
            }
        }

        private void btnAddOffer_Click(object sender, EventArgs e)
        {
            offerEditor offerEditor = new offerEditor(null, "Add");
            offerEditor.ShowDialog();
            LoadOffer();

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {

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

        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            txtSearch.Focus();
        }
    }
}
