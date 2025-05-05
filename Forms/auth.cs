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
    public partial class auth : Form
    {
        AuthManager authManager = new AuthManager();

        public auth()
        {
            InitializeComponent();
            LoadAuth();
        }


        PlaceHolder placeHolder = new PlaceHolder("Yetkili arayın...");

        private void button1_Click(object sender, EventArgs e)
        {

            txtSearch.Clear();
            placeHolder.LeavePlaceHolder(txtSearch);
            dataGridView1.DataSource = null;
            LoadAuth();
        }

        private void LoadAuth()
        {
            dataGridView1.DataSource = authManager.GetAuthWithCompanyName();
            SetupGridColumnProperties();
            SetupAuthGridColumns();
        }

        private void SetupGridColumnProperties()
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.Resizable = DataGridViewTriState.False;
            }
        }

        private void SetupAuthGridColumns()
        {
            // Gereksiz sütunları gizle
            if (dataGridView1.Columns["yetkili_id"] != null)
                dataGridView1.Columns["yetkili_id"].Visible = false;

            if (dataGridView1.Columns["firma_id"] != null)
                dataGridView1.Columns["firma_id"].Visible = false;

            // İstenilen sıraya göre DisplayIndex ayarla
            if (dataGridView1.Columns["adi"] != null)
            {
                dataGridView1.Columns["adi"].DisplayIndex = 0;
                dataGridView1.Columns["adi"].HeaderText = "Deneme";
            }

            if (dataGridView1.Columns["isim"] != null)
            {
                dataGridView1.Columns["isim"].DisplayIndex = 1;
                dataGridView1.Columns["isim"].HeaderText = "İsim";
            }

            if (dataGridView1.Columns["soyisim"] != null)
            {
                dataGridView1.Columns["soyisim"].DisplayIndex = 2;
                dataGridView1.Columns["soyisim"].HeaderText = "Soyisim";
            }

            if (dataGridView1.Columns["hitap"] != null)
            {
                dataGridView1.Columns["hitap"].DisplayIndex = 3;
                dataGridView1.Columns["hitap"].HeaderText = "Hitap";
            }

            if (dataGridView1.Columns["adres"] != null)
            {
                dataGridView1.Columns["adres"].DisplayIndex = 4;
                dataGridView1.Columns["adres"].HeaderText = "Adres";
            }

            if (dataGridView1.Columns["telefon"] != null)
            {
                dataGridView1.Columns["telefon"].DisplayIndex = 5;
                dataGridView1.Columns["telefon"].HeaderText = "Telefon";
            }

            if (dataGridView1.Columns["eposta"] != null)
            {
                dataGridView1.Columns["eposta"].DisplayIndex = 6;
                dataGridView1.Columns["eposta"].HeaderText = "E-posta";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            if (TextboxValidator.IsNullOrWhiteSpace(txtSearch) || txtSearch.Text == "Yetkili arayın...")
            {
                MessageHelper.ShowError("Arama alanı boş bırakılamaz. Tüm yetkililer listeleniyor.");
                LoadAuth();
            }
            else
            {
                var result = authManager.Search(txtSearch.Text);

                if (result != null)
                {
                    dataGridView1.DataSource = result;
                    SetupGridColumnProperties();
                    SetupAuthGridColumns();
                }
                else
                {
                    MessageHelper.ShowError("Aramaya uygun yetkili bulunamadı.");
                }
            }
        }

        private void btnAddAuth_Click(object sender, EventArgs e)
        {
            using (var authEditor = new authEditor(null, "Add"))
            {
                authEditor.ShowDialog();
            }
            LoadAuth();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int? value = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString());
                var result = CustomMessageBox.Show("Bu yetkili düzenlemek veya silmek istiyor musunuz?");

                if (result == CustomMessageBox.CustomResult.Duzenle)
                {
                    using (var editor = new authEditor(value, "Edit"))
                    {
                        editor.ShowDialog();
                    }
                    LoadAuth();
                }
                else if (result == CustomMessageBox.CustomResult.Sil)
                {
                    if (MessageHelper.ShowQuestion("Bu yetkiliyi silmek istediğinize emin misiniz?") == DialogResult.Yes)
                    {
                        authManager.DeleteAuth(value.Value);
                        LoadAuth();
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

        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            txtSearch.Focus();
        }
    }
}
