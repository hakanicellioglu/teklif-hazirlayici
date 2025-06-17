using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Business;
using Teklif_Hazırlayıcı.Forms.Custom_Item;
using Teklif_Hazırlayıcı.Forms.Editor;
using Teklif_Hazırlayıcı.Helpers;
using Teklif_Hazırlayıcı.Validation;
using TeklifHazirlayici.Properties;

namespace Teklif_Hazırlayıcı.Forms
{
    public partial class auth : Form
    {
        private readonly CompanyManager _companyManager;
        private readonly AuthManager _authManager;
        private readonly ColumnForm _columnForm;

        public auth(CompanyManager companyManager, AuthManager authManager, ColumnForm columnForm)
        {
            InitializeComponent();
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            _companyManager = companyManager;
            _authManager = authManager;
            _columnForm = columnForm;
            LoadAuth();
        }

        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            txtSearch.Focus();
        }

        PlaceHolder placeHolder = new PlaceHolder("Yetkili arayın...");
        private void txt_Enter(object sender, EventArgs e)
        {
            placeHolder.EnterPlaceHolder(txtSearch);
        }

        private void txt_Leave(object sender, EventArgs e)
        {
            placeHolder.LeavePlaceHolder(txtSearch);
        }






        /*
        private void button1_Click(object sender, EventArgs e)
        {

            txtSearch.Clear();
            placeHolder.LeavePlaceHolder(txtSearch);
            dataGridView1.DataSource = null;
            LoadAuth();
        }
        */
        private void LoadAuth()
        {
            dataGridView1.DataSource = _authManager.GetAuthWithCompanyName();
            SetupGridColumnProperties();
            SetupAuthGridColumns();
        }

        private void SetupGridColumnProperties()
        {
            DataGridHelper.SetupGridColumnProperties(dataGridView1);
        }

        private void SetupAuthGridColumns()
        {
            // Gereksiz sütunları gizle
            if (dataGridView1.Columns["yetkili_id"] != null)
                dataGridView1.Columns["yetkili_id"].Visible = false;

            if (dataGridView1.Columns["firma_id"] != null)
                dataGridView1.Columns["firma_id"].Visible = false;

            // İstenilen sıraya göre DisplayIndex ayarla
            if (dataGridView1.Columns["Firma"] != null)
            {
                dataGridView1.Columns["Firma"].DisplayIndex = 0;
                dataGridView1.Columns["Firma"].HeaderText = "Firma Adı";
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

        private void button4_Click(object sender, EventArgs e)
        {
            Point globalPos = button4.Parent.PointToScreen(button4.Location);
            Point formPos = this.PointToClient(globalPos);

            Rectangle buttonBounds = button4.RectangleToScreen(button4.ClientRectangle);
            Point location = new Point(buttonBounds.X, buttonBounds.Bottom);

            _columnForm.StartPosition = FormStartPosition.Manual;
            _columnForm.Location = location;


            int xWitdh = button4.Location.X;
            int newSize = panel2.Width - xWitdh;
            _columnForm.Size = new Size(newSize, _columnForm.Size.Height);


            // Dialog olarak göster
            if (_columnForm.ShowDialog(this) == DialogResult.OK)
            {
                List<ColumnItem> secilenler = _columnForm.SelectedColumns;

                // Sütun görünürlüğünü güncelle
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.Visible = secilenler.Any(s => s.Name == col.Name);
                }
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
                var result = _authManager.Search(txtSearch.Text);


                if (result != null && result.Rows.Count > 0)

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
            using (var authEditor = new authEditor(null, "Add", new CompanyManager(), new AuthManager()))
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
                    using (var authEditor = new authEditor(value.Value, "Edit", new CompanyManager(), new AuthManager()))
                    {
                        authEditor.ShowDialog();
                    }

                    LoadAuth();
                }
                else if (result == CustomMessageBox.CustomResult.Sil)
                {
                    if (MessageHelper.ShowQuestion("Bu yetkiliyi silmek istediğinize emin misiniz?") == DialogResult.Yes)
                    {
                        _authManager.DeleteAuth(value.Value);
                        LoadAuth();
                    }
                }
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnSearch_Click(sender, e);

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _authManager.GetAuthWithCompanyName();
            SetupGridColumnProperties();
            SetupAuthGridColumns();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
