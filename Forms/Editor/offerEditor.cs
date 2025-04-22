using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Business;
using Teklif_Hazırlayıcı.Forms.Custom_Item;
using Teklif_Hazırlayıcı.Helpers;
using Teklif_Hazırlayıcı.Validation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Teklif_Hazırlayıcı.Forms.Editor
{
    public partial class offerEditor : Form
    {
        string editor_mode;
        int? offer_id;
        CompanyManager CompanyManager = new CompanyManager();
        AuthManager AuthManager = new AuthManager();

        public offerEditor(int? offerId, string editMode)
        {
            InitializeComponent();
            offer_id = offerId;
            editor_mode = editMode;
            LoadOffer();
            SelectionMode();
        }

        private void SelectionMode()
        {
            if (editor_mode == "Add")
            {
                button1.Text = "Ekle";
                btnCancel.Visible = false;
                txtIskonto.Text = "0";
                txtTevkifat.Text = "0";
                btnEdit.Visible = false;
            }
            else if (editor_mode == "Edit")
            {
                CenterToScreen();
                button1.Text = "Kaydet";
                btnCancel.Visible = true;
                btnEdit.Visible = true;

                OfferManager manager = new OfferManager();
                var data = manager.GetOfferById(offer_id);

                if (data != null && data.Rows.Count > 0)
                {
                    var offer = data.Rows[0];

                    // Firma ComboBox eşleşmesi
                    string firmaAdi = offer["firma_adi"].ToString();
                    int firmaIndex = chkFirmalar.FindStringExact(firmaAdi);
                    if (firmaIndex >= 0) chkFirmalar.SelectedIndex = firmaIndex;

                    // Yetkili ComboBox eşleşmesi
                    string yetkiliAdi = offer["yetkili_adi"].ToString().Trim().ToLower();

                    for (int i = 0; i < chkYetkililer.Items.Count; i++)
                    {
                        string itemText = chkYetkililer.Items[i].ToString().Trim().ToLower();

                        if (itemText.Contains(yetkiliAdi))
                        {
                            chkYetkililer.SelectedIndex = i;
                            break;
                        }
                    }

                    // Temel alanlar
                    dateTimePicker1.Value = Convert.ToDateTime(offer["teklif_tarih"]);
                    chkTeslimSekli.SelectedItem = offer["teslim_sekli"].ToString();
                    chkOdemeSekli.SelectedItem = offer["odeme_sekli"].ToString();
                    txtOdemeVadesi.Text = offer["odeme_vadesi"].ToString();
                    chkDovizBirimi.SelectedItem = offer["doviz_birimi"].ToString();
                    txtDovizKuru.Text = offer["doviz_kuru"].ToString();
                    txtTeklifSuresi.Text = offer["teklif_suresi"].ToString();
                    txtLME.Text = offer["lme"].ToString();
                    txtİscilik.Text = offer["iscilik"].ToString();

                    int iskonto_orani = Convert.ToInt32(offer["iskonto_orani"]);

                    if (iskonto_orani > 0)
                    {
                        chkİskonto.Checked = true;
                        txtIskonto.Text = offer["iskonto_orani"].ToString();
                    }
                    else

                    {
                        txtIskonto.Text = "0";
                        chkİskonto.Checked = false;
                    }

                    // Tevkifat alanı
                    txtTevkifat.Text = offer["tevkifat_orani"].ToString();

                    decimal oran = 0;
                    decimal.TryParse(txtTevkifat.Text, out oran);

                    // Checkbox işaretleniyor mu?
                    chkTevkifat.Checked = oran > 0;

                    // TextBox aktif/pasif ayarlanıyor
                    txtTevkifat.Enabled = chkTevkifat.Checked;

                    // Eğer sıfırsa görünür olarak da "0" yazabiliriz, garantiye almak için
                    if (!chkTevkifat.Checked)
                    {
                        txtTevkifat.Text = "0";
                    }


                    // Diğer alanlar
                    chkDurum.SelectedItem = offer["durum"].ToString();
                    chkVade.SelectedItem = offer["vade"].ToString();
                    LoadProducts();

                }
                else
                {
                    MessageHelper.ShowError("Teklif bulunamadı.");
                }
            }
        }

        private void LoadProducts()
        {
            itemManager itemMgr = new itemManager();
            DataTable dtKalemler = itemMgr.GetItemsByTeklifId(offer_id);

            if (dtKalemler != null && dtKalemler.Rows.Count > 0)
            {

                dataGridView1.DataSource = dtKalemler;
                dataGridView1.Visible = true;
                dataGridView1.Columns["kalem_id"].Visible = false;
                dataGridView1.Columns["teklif_id"].Visible = false;
                dataGridView1.Columns["urun_id"].Visible = false;


                // İsteğe bağlı: kolon başlıklarını özelleştirebilirsin
                dataGridView1.Columns["urun"].HeaderText = "Ürün";
                dataGridView1.Columns["kalip_no"].HeaderText = "Kalıp No";
                dataGridView1.Columns["yuzey"].HeaderText = "Yüzey";
                dataGridView1.Columns["yuzey_kodu"].HeaderText = "Yüzey Kodu";
                dataGridView1.Columns["gramaj"].HeaderText = "Gramaj";
                dataGridView1.Columns["kategori"].HeaderText = "Kategori";
                dataGridView1.Columns["adet"].HeaderText = "Adet";
                dataGridView1.Columns["boy"].HeaderText = "Boy";
                dataGridView1.Columns["kg"].HeaderText = "KG";
                dataGridView1.Columns["birim_fiyat"].HeaderText = "Birim Fiyat";
                dataGridView1.Columns["toplam_tutar"].HeaderText = "Toplam Tutar";
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        private bool ValidateForm()
        {
            // ComboBox kontrolleri
            if (chkFirmalar.SelectedIndex == -1)
            {
                MessageHelper.ShowError("Lütfen firma seçiniz.");
                return false;
            }

            if (chkYetkililer.SelectedIndex == -1)
            {
                MessageHelper.ShowError("Lütfen yetkili seçiniz.");
                return false;
            }

            if (chkTeslimSekli.SelectedIndex == -1)
            {
                MessageHelper.ShowError("Lütfen teslim şekli seçiniz.");
                return false;
            }

            if (chkOdemeSekli.SelectedIndex == -1)
            {
                MessageHelper.ShowError("Lütfen ödeme şekli seçiniz.");
                return false;
            }

            if (chkDovizBirimi.SelectedIndex == -1)
            {
                MessageHelper.ShowError("Lütfen döviz birimi seçiniz.");
                return false;
            }

            if (chkDurum.SelectedIndex == -1)
            {
                MessageHelper.ShowError("Lütfen durum seçiniz.");
                return false;
            }

            if (chkVade.SelectedIndex == -1)
            {
                MessageHelper.ShowError("Lütfen vade seçiniz.");
                return false;
            }

            string iskontoStr = txtIskonto.Text.Trim().Replace(",", ".");
            if (!decimal.TryParse(iskontoStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal iskontoDecimal))
            {
                MessageHelper.ShowError("İskonto oranı geçerli bir sayı değil.");
                return false;
            }

            if (iskontoDecimal < 0 || iskontoDecimal > 100)
            {
                MessageHelper.ShowError("İskonto oranı 0 ile 100 arasında olmalıdır.");
                return false;
            }



            // CheckBox kontrolü
            if (chkTevkifat.Checked)
            {
                if (!IsValidInt(txtTevkifat, "Tevkifat"))
                    return false;

                if (!decimal.TryParse(txtTevkifat.Text.Trim(), out decimal oran))
                {
                    MessageHelper.ShowError("Tevkifat oranı geçerli bir sayı olmalıdır.");
                    return false;
                }

                if (oran <= 0 || oran > 100)
                {
                    MessageHelper.ShowError("Tevkifat oranı 0'dan büyük ve 100'e eşit veya daha küçük olmalıdır.");
                    return false;
                }
            }
            else
            {
                if (MessageHelper.ShowQuestion("Tevkifat seçilmedi. Devam etmek istiyor musunuz?") != DialogResult.Yes)
                    return false;
            }


            return true;
        }

        private bool IsValidInt(System.Windows.Forms.TextBox tb, string fieldName)
        {
            if (TextboxValidator.IsNullOrWhiteSpace(tb))
            {
                MessageHelper.ShowError($"{fieldName} alanı boş bırakılamaz.");
                return false;
            }
            if (!int.TryParse(tb.Text, out _))
            {
                MessageHelper.ShowError($"{fieldName} alanı sayısal bir değer içermelidir.");
                return false;
            }
            return true;
        }

        private void LoadOffer()
        {
            if (LoadCompany())
            {
                long SelectedCompany = Convert.ToInt64(chkFirmalar.SelectedValue);
                chkYetkililer.Enabled = true;
                if (LoadAuth(SelectedCompany))
                {
                }
                else
                {
                    chkYetkililer.Items.Add("Sayın Yetkili");
                    return;
                }
            }
            else
            {
                chkYetkililer.Enabled = false;
            }
        }
        private bool LoadAuth(long firma_id)
        {
            var authList = AuthManager.GetAuthByCompanyId(firma_id);
            chkYetkililer.DataSource = null; // eski kaynakları sıfırla

            if (authList == null || authList.Count == 0)
                return false;

            var authComboList = new List<KeyValuePair<string, string>>();

            foreach (var auth in authList)
            {
                if (auth.ContainsKey("isim") && auth.ContainsKey("yetkili_id"))
                {
                    string displayText = auth["isim"];

                    if (auth.ContainsKey("hitap") && !string.IsNullOrWhiteSpace(auth["hitap"]))
                        displayText = $"{auth["isim"]} {auth["hitap"]}";

                    authComboList.Add(new KeyValuePair<string, string>(auth["yetkili_id"], displayText));
                }
                else
                {
                    MessageHelper.ShowError("Eksik veri: " + string.Join(", ", auth.Keys));
                }
            }

            chkYetkililer.DisplayMember = "Value";
            chkYetkililer.ValueMember = "Key";
            chkYetkililer.DataSource = authComboList;

            return true;
        }

        private bool LoadCompany()
        {
            var dt = CompanyManager.GetCompany(); // DataTable

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageHelper.ShowError("Şirket verileri yüklenemedi.");
                return false;
            }

            chkFirmalar.DataSource = dt;
            chkFirmalar.DisplayMember = "adi"; // Görünen
            chkFirmalar.ValueMember = "firma_id";    // Firma ID (veritabanı ID'si)
            return true;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkFirmalar.SelectedIndex == -1 || chkFirmalar.SelectedItem == null)
            {
                chkYetkililer.Enabled = false;
                chkYetkililer.Items.Clear();
                return;
            }

            try
            {
                var selectedRow = chkFirmalar.SelectedItem as DataRowView;
                if (selectedRow != null && selectedRow["firma_id"] != DBNull.Value)
                {
                    long selectedCompany = Convert.ToInt64(selectedRow["firma_id"]);
                    LoadAuth(selectedCompany); // aşağıda tanımlı
                    chkYetkililer.Enabled = true;
                }
                else
                {
                    MessageHelper.ShowError("Firma ID'si alınamadı.");
                    chkYetkililer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata: " + ex.Message);
                chkYetkililer.Enabled = false;
            }
        }

        string exTevkifatValue;
        private void chkTevkifat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTevkifat.Checked)
            {
                txtTevkifat.Enabled = true;

                if (!string.IsNullOrEmpty(exTevkifatValue))
                    txtTevkifat.Text = exTevkifatValue; // saklanan değeri geri getir
            }
            else
            {
                exTevkifatValue = txtTevkifat.Text;

                txtTevkifat.Enabled = false;

                // Eğer tevkifat değeri zaten 0 değilse ve sıfırlamak istiyorsan
                if (string.IsNullOrEmpty(txtTevkifat.Text) || txtTevkifat.Text != "0")
                    txtTevkifat.Text = "0";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (editor_mode == "Add")
            {
                if (ValidateForm() == true)
                {

                    OfferManager offerManager = new OfferManager();
                    string tevkifat = string.IsNullOrWhiteSpace(txtTevkifat.Text) ? "0" : txtTevkifat.Text;
                    int yetkiliId;

                    if (chkYetkililer.SelectedItem is KeyValuePair<string, string> selectedAuth)
                    {
                        yetkiliId = Convert.ToInt32(selectedAuth.Key);
                    }
                    else
                    {
                        MessageHelper.ShowError("Lütfen bir yetkili seçiniz.");
                        return;
                    }

                    decimal kdvDecimal = 20m;
                    string kdvStr = kdvDecimal.ToString("0.##", new CultureInfo("tr-TR"));

                    int odemeVadesi = 0;
                    if (!int.TryParse(txtOdemeVadesi.Text.Trim(), out odemeVadesi))
                        odemeVadesi = 0;

                    decimal dovizKuruDecimal = 1.00m;
                    string dovizKuruStr = "1.00"; // varsayılan

                    if (decimal.TryParse(txtDovizKuru.Text.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out dovizKuruDecimal))
                    {
                        dovizKuruStr = dovizKuruDecimal.ToString("0.##", new CultureInfo("tr-TR"));
                    }



                    int teklifId = offerManager.AddOffer(
                        Convert.ToInt32(chkFirmalar.SelectedValue),
                        yetkiliId,
                        dateTimePicker1.Value,
                        chkTeslimSekli.Text,
                        chkOdemeSekli.Text,
                        odemeVadesi,
                        Convert.ToInt32(txtTeklifSuresi.Text.Trim()),
                        dovizKuruStr,
                        Convert.ToChar(chkDovizBirimi.Text.Trim().Substring(0, 1)),
                        chkVade.Text,
                        txtLME.Text,
                        txtİscilik.Text,
                        txtIskonto.Text,
                        kdvStr,
                        chkTevkifat.Checked,
                        tevkifat,
                        chkDurum.Text
                    );

                    if (teklifId > 0)
                    {
                        if (MessageHelper.ShowQuestion("Teklif başarıyla oluşturuldu. Ürün eklemek ister misiniz?") == DialogResult.Yes)
                        {
                            Hide();
                            itemEditor itemEditor = new itemEditor(teklifId, null, "Add");
                            if (itemEditor.ShowDialog() == DialogResult.OK)
                            {
                                offerManager.UpdateOfferById(teklifId);
                            }
                        }
                        else Close();

                    }
                    else
                    {
                        MessageHelper.ShowError("Teklif eklenirken bir hata oluştu.");
                    }
                }
            }
            else if (editor_mode == "Edit")
            {

                OfferManager offerManager = new OfferManager();

                int yetkiliId;
                if (chkYetkililer.SelectedIndex >= 0 && chkYetkililer.SelectedValue != null)
                {
                    yetkiliId = Convert.ToInt32(chkYetkililer.SelectedValue);
                }
                else
                {
                    MessageHelper.ShowError("Lütfen bir yetkili seçiniz.");
                    return;
                }


                offerManager.UpdateOffer(offer_id, Convert.ToInt32(chkFirmalar.SelectedValue), yetkiliId, dateTimePicker1.Value, chkTeslimSekli.Text, chkOdemeSekli.Text, Convert.ToInt32(txtOdemeVadesi.Text), Convert.ToInt32(txtTeklifSuresi.Text), txtDovizKuru.Text, Convert.ToChar(chkDovizBirimi.Text), chkVade.Text, txtLME.Text, txtİscilik.Text, txtIskonto.Text, "20", chkTevkifat.Checked, txtTevkifat.Text, chkDurum.Text);
                Close();
            }
        }

        private void txtIskonto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
            {
                e.Handled = true;
                txtIskonto.SelectedText = ".";
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int kalemId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["kalem_id"].Value);
                var result = CustomMessageBox.Show("Bu kalemi düzenlemek veya silmek istiyor musunuz?");

                if (result == CustomMessageBox.CustomResult.Duzenle)
                {
                    Hide();
                    itemEditor editor = new itemEditor(null, kalemId, "Edit");
                    editor.Width = Screen.PrimaryScreen.WorkingArea.Width;
                    editor.Height = Screen.PrimaryScreen.WorkingArea.Height;
                    editor.ShowDialog();
                    LoadProducts();
                    Show();
                }
                else if (result == CustomMessageBox.CustomResult.Sil)
                {
                    var confirm = MessageHelper.ShowQuestion("Bu kalemi silmek istediğinize emin misiniz?");
                    if (confirm == DialogResult.Yes)
                    {
                        itemManager manager = new itemManager();
                        if (manager.DeleteProductByKalemId(kalemId))
                        {
                            MessageHelper.ShowInfo("Kalem başarıyla silindi.");
                            LoadProducts();
                        }
                        else
                        {
                            MessageHelper.ShowError("Silme işlemi başarısız oldu.");
                        }
                    }
                }
            }
        }

        private void chkOdemeSekli_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkOdemeSekli.SelectedIndex == 0) { chkVade.Items.Clear(); chkVade.Items.Add("Peşin"); chkVade.Items.Add("Vadeli"); }
            else if (chkOdemeSekli.SelectedIndex == 1) { chkVade.Items.Clear(); chkVade.Items.Add("Vade"); }
            else { chkVade.Items.Clear(); chkVade.Items.Add("Peşin"); chkVade.Items.Add("Taksit"); }
        }

        private void chkVade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkOdemeSekli.Text == "Nakit" && chkVade.Text == "Peşin")
            {
                txtOdemeVadesi.Enabled = false;
            }
            else
                txtOdemeVadesi.Enabled = true;

        }

        private void chkDovizBirimi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkDovizBirimi.Text == "₺") txtDovizKuru.Enabled = false;
            else txtDovizKuru.Enabled = true;
        }


        string exİskontoValue;
        private void chkİskonto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkİskonto.Checked)
            {
                txtIskonto.Enabled = true;

                if (!string.IsNullOrEmpty(exİskontoValue))
                    txtIskonto.Text = exİskontoValue;
            }
            else
            {
                exİskontoValue = txtIskonto.Text;
                txtIskonto.Enabled = false;
                if (string.IsNullOrEmpty(txtIskonto.Text) || txtIskonto.Text != "0")
                    txtIskonto.Text = "0";
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            itemEditor itemEditor = new itemEditor(offer_id, null, "Add");
            itemEditor.ShowDialog();
            LoadProducts();
        }
    }
}