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
using TeklifHazirlayici.Forms.Custom_Item;
using TeklifHazirlayici.Properties;

namespace Teklif_Hazırlayıcı.Forms
{
    public partial class offer : Form
    {
        private readonly OfferManager _offerManager;
        private readonly offerEditor _offerEditor;
        private Button btnManageColumns;

        public offer(OfferManager offerManager, offerEditor offerEditor)
        {
            InitializeComponent();
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            _offerManager = offerManager;
            _offerEditor = offerEditor;
            AddManageColumnsButton();
            cmbStatus.SelectedIndex = 0;
            dtStartDate.Value = DateTime.Today.AddMonths(-1);
            dtEndDate.Value = DateTime.Today;
            LoadOffer();
        }

        private void AddManageColumnsButton()
        {
            btnManageColumns = new Button
            {
                Text = "Sütunlar",
                Dock = DockStyle.Right,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };
            btnManageColumns.FlatAppearance.BorderSize = 0;
            btnManageColumns.Click += btnManageColumns_Click;
            panel2.Controls.Add(btnManageColumns);
        }

        private async void LoadOffer()
        {
            dataGridView1.DataSource = await _offerManager.GetOfferAsync();
            SetupGridColumnProperties();
            SetupOfferGridColumns();
        }

        private void SetupGridColumnProperties()
        {
            DataGridHelper.SetupGridColumnProperties(dataGridView1);
        }

        private void SetupOfferGridColumns()
        {
            if (dataGridView1.Columns["teklif_id"] != null)
            {
                dataGridView1.Columns["teklif_id"].HeaderText = "Teklif No";
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
            using (var editor = new offerEditor(null, 
                "Add", 
                new AuthManager(), 
                new CompanyManager(), 
                new OfferManager()))
            {
                Width = Screen.PrimaryScreen.WorkingArea.Width;
                Height = Screen.PrimaryScreen.WorkingArea.Height;
                editor.ShowDialog();
            }
            LoadOffer();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            var result = await _offerManager.SearchAsync(
                txtSearch.Text,
                cmbStatus.SelectedItem?.ToString(),
                dtStartDate.Value,
                dtEndDate.Value);

            if (result != null)
            {
                dataGridView1.DataSource = result;
                SetupGridColumnProperties();
                SetupOfferGridColumns();
            }
            else
            {
                MessageHelper.ShowError("Aramaya uygun teklif bulunamadı.");
            }
        }

        private async void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            var result = await _offerManager.SearchAsync(
                txtSearch.Text,
                cmbStatus.SelectedItem?.ToString(),
                dtStartDate.Value,
                dtEndDate.Value);

            if (result != null)
            {
                dataGridView1.DataSource = result;
                SetupGridColumnProperties();
                SetupOfferGridColumns();
            }
        }

        private async void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            var result = await _offerManager.SearchAsync(
                txtSearch.Text,
                cmbStatus.SelectedItem?.ToString(),
                dtStartDate.Value,
                dtEndDate.Value);

            if (result != null)
            {
                dataGridView1.DataSource = result;
                SetupGridColumnProperties();
                SetupOfferGridColumns();
            }
        }

        private async void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            var result = await _offerManager.SearchAsync(
                txtSearch.Text,
                cmbStatus.SelectedItem?.ToString(),
                dtStartDate.Value,
                dtEndDate.Value);

            if (result != null)
            {
                dataGridView1.DataSource = result;
                SetupGridColumnProperties();
                SetupOfferGridColumns();
            }
        }

        PlaceHolder placeHolder = new PlaceHolder("Teklif arayın...");
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            placeHolder.LeavePlaceHolder(txtSearch);
            cmbStatus.SelectedIndex = 0;
            dtStartDate.Value = DateTime.Today.AddMonths(-1);
            dtEndDate.Value = DateTime.Today;
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
                    using (var editor = new offerEditor(teklifId, "Edit", new AuthManager(), new CompanyManager(), new OfferManager()))
                    {
                        //editor.Width = Screen.PrimaryScreen.WorkingArea.Width;
                        //editor.Height = Screen.PrimaryScreen.WorkingArea.Height;
                        editor.ShowDialog();
                    }
                    Parent.Parent.Show();
                    LoadOffer();

                }
                else if (result == CustomMessageBox.CustomResult.Sil)
                {
                    var confirm = MessageHelper.ShowQuestion("Bu teklifi ve ilişkili tüm ürünleri silmek istediğinize emin misiniz?");
                    if (confirm == DialogResult.Yes)
                    {
                        
                        if (_offerManager.DeleteOffer(teklifId))
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
            using (var offerEditor = new offerEditor(null, "Add", new AuthManager(), new CompanyManager(), new OfferManager()))
            {
                offerEditor.ShowDialog();
            }
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOffer();
        }

        private void btnManageColumns_Click(object sender, EventArgs e)
        {
            ColumnForm columnForm = new ColumnForm("teklifler", dataGridView1);
            columnForm.TopLevel = false;
            columnForm.Parent = panel1;

            columnForm.Location = new Point(btnManageColumns.Left, btnManageColumns.Bottom);

            // Parent ilişkisini kaldır
            columnForm.Parent = null;
            columnForm.TopLevel = true;

            // Ana forma göre konumla
            // Formun ekran üzerindeki konumunu al
            Point formScreenLocation = this.PointToScreen(Point.Empty);

            // Formun sağ kenarına hizalama
            int x = formScreenLocation.X + this.Width - columnForm.Width;
            int y = btnManageColumns.PointToScreen(new Point(0, btnManageColumns.Height + 20)).Y;

            columnForm.Location = new Point(x, y);
            columnForm.StartPosition = FormStartPosition.Manual;
            columnForm.ShowDialog();
        }
    }
}
