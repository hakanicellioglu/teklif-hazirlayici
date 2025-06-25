using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Teklif_Hazırlayıcı.Business;
using Teklif_Hazırlayıcı.Forms.Custom_Item;
using Teklif_Hazırlayıcı.Helpers;
using Teklif_Hazırlayıcı.Models;
using Teklif_Hazırlayıcı.Validation;
using TeklifHazirlayici.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Teklif_Hazırlayıcı.Forms.Editor
{
    public partial class offerEditor : Form
    {
        string editor_mode;
        int? offer_id;
        private readonly CompanyManager _companyManager = new CompanyManager();
        private readonly AuthManager _authManager = new AuthManager();
        private readonly OfferManager _offerManager = new OfferManager();


        public offerEditor(int? offerId, string editMode, AuthManager authManager, CompanyManager companyManager, OfferManager offerManager)
        {
            InitializeComponent();
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            offer_id = offerId;
            editor_mode = editMode;
            LoadOffer();
            SelectionMode();
            _authManager = authManager;
            _companyManager = companyManager;
            _offerManager = offerManager;
        }

        private async void SelectionMode()
        {
            if (editor_mode == "Add")
            {
                button1.Text = "Ekle";

                txtIskonto.Text = "0";

                btnCancel.Visible = false;
                btnEdit.Visible = false;
                button2.Visible = false;
            }
            else if (editor_mode == "Edit")
            {
                CenterToScreen();
                button1.Text = "Kaydet";

                btnCancel.Visible = true;
                btnEdit.Visible = true;
                button2.Visible = true;

                var data = await _offerManager.GetOfferByIdAsync(offer_id);

                if (data != null && data.Rows.Count > 0)
                {
                    var offer = data.Rows[0];

                    // Firma ComboBox eşleşmesi
                    string firmaAdi = offer["isim"].ToString();
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
                    txtOdemeVadesi.Text = offer["odeme_vade"].ToString();
                    chkDovizBirimi.SelectedItem = offer["doviz_birimi"].ToString();
                    txtDovizKuru.Text = offer["doviz_kuru"].ToString();
                    txtTeklifSuresi.Text = offer["teklif_sure"].ToString();
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

                    // Tevkifat alanı - veritabanından gelen değere göre işaretlenmeli
                    if (offer.Table.Columns.Contains("tevkifat"))
                    {
                        bool tevkifat = false;

                        if (offer["tevkifat"] != DBNull.Value)
                            tevkifat = Convert.ToBoolean(offer["tevkifat"]);

                        chkTevkifat.Checked = tevkifat;
                    }
                    else
                    {
                        // Kolon yoksa loglanabilir veya varsayılan olarak kapatılabilir
                        chkTevkifat.Checked = false;
                    }



                    // Diğer alanlar
                    chkDurum.SelectedItem = offer["durum"].ToString();
                    chkVade.SelectedItem = offer["vade"].ToString();
                    if (offer.Table.Columns.Contains("vade_farki"))
                        txtVadeFarki.Text = offer["vade_farki"].ToString();
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
            ItemManager itemMgr = new ItemManager();
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
            var authList = _authManager.GetAuthByCompanyId(firma_id);
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
            var dt = _companyManager.GetCompany();

            if (dt == null)
            {
                MessageHelper.ShowError("GetCompany null döndü!");
                return false;
            }



            // Sütun adı doğruysa devam et
            // List<Company> kullanıldığı için DisplayMember ve ValueMember
            // Company sınıfındaki property adları ile eşleşmeli
            chkFirmalar.DataSource = dt;
            chkFirmalar.DisplayMember = "Isim";
            chkFirmalar.ValueMember = "FirmaId";

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
                var selectedCompany = chkFirmalar.SelectedItem as Company;

                if (selectedCompany != null)
                {
                    LoadAuth(selectedCompany.FirmaId);
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


        private void button1_Click(object sender, EventArgs e)
        {
            if (editor_mode == "Add")
            {
                if (ValidateForm() == true)
                {
                    OfferManager offerManager = new OfferManager();
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

                    var turkish = new CultureInfo("tr-TR");


                    // Eğer çıktıyı Türkçe formatta göstermek isterseniz bu gerekli olur.
                    // Sadece giriş için InvariantCulture kullanacağız.
                    var turkishOutputCulture = new CultureInfo("tr-TR");

                    string input = txtDovizKuru.Text.Trim();
                    decimal dovizKuruDecimal;
                    string dovizKuruStr; // Başlangıç değeri atamak yerine sonunda güncelleyeceğiz

                    // Adım 1: Kullanıcının girdiği noktayı virgülle değiştirmemek için
                    // ve ondalık ayırıcı olarak noktayı kabul etmek için InvariantCulture kullanın.
                    // Ancak, kullanıcı yanlışlıkla binlik ayırıcı olarak virgül kullanmışsa
                    // veya ondalık ayırıcı olarak virgül kullanmışsa (Türkçe alışkanlıkla),
                    // bu durumda input'u temizlememiz gerekebilir.

                    // En güvenli yol: Gelen string'deki tüm virgülleri noktaya çevirip
                    // (eğer varsa, binlik ayırıcı olarak değil ondalık ayırıcı olarak kullanılmışsa)
                    // sonra InvariantCulture ile denemek. Bu, kullanıcının "123,45" veya "123.45"
                    // girmesi durumunda da doğru çalışmasını sağlar.
                    string cleanedInput = input.Replace(",", ".");

                    // Şimdi, temizlenmiş input'u InvariantCulture kullanarak decimal'e dönüştürmeyi dene.
                    // NumberStyles.Any, hem binlik ayırıcıları hem de ondalık ayırıcıları esnek bir şekilde yorumlamaya çalışır.
                    bool parseSuccess = decimal.TryParse(cleanedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out dovizKuruDecimal);

                    // Eğer dönüştürme başarısız olursa (örneğin, "abc" girilirse)
                    if (!parseSuccess)
                    {
                        // Burada uygun bir varsayılan değer atayabilir veya kullanıcıya hata mesajı gösterebilirsiniz.
                        dovizKuruDecimal = 1.00m; // Örneğin, varsayılan olarak 1.00
                                                  // Loglama veya hata bildirimi yapılabilir: Console.WriteLine("Dönüştürme başarısız oldu, varsayılan değer kullanıldı.");
                    }

                    // Adım 2: Elde ettiğiniz decimal değeri, istediğiniz string formatına çevirin.
                    // Eğer çıktıda ondalık ayırıcının virgül olmasını istiyorsanız, burada "tr-TR" kültürünü kullanın.
                    // Eğer çıktıda da ondalık ayırıcının nokta olmasını istiyorsanız, burada da InvariantCulture kullanın.
                    dovizKuruStr = dovizKuruDecimal.ToString("0.##", turkishOutputCulture); // Çıktıyı Türkçe formatta (virgüllü)

                    // Eğer çıktıda da nokta olmasını istiyorsanız:
                    // dovizKuruStr = dovizKuruDecimal.ToString("0.##", CultureInfo.InvariantCulture);

                    float vadeFarki = 0;
                    if (chkVade.Text.Contains("Vade"))
                    {
                        if (!float.TryParse(txtVadeFarki.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out vadeFarki))
                        {
                            MessageHelper.ShowError("Vade farkı geçerli bir sayı değil.");
                            return;
                        }
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
                        vadeFarki,
                        txtLME.Text,
                        txtİscilik.Text,
                        txtIskonto.Text,
                        kdvStr,
                        chkTevkifat.Checked,
                        chkDurum.Text
                    );

                    if (teklifId > 0)
                    {
                        if (MessageHelper.ShowQuestion("Teklif başarıyla oluşturuldu. Ürün eklemek ister misiniz?") == DialogResult.Yes)
                        {
                            Hide();
                            itemEditor itemEditor = new itemEditor(teklifId, null, "Add", new ItemManager());
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

                float vadeFarki = 0;
                if (chkVade.Text.Contains("Vade"))
                {
                    if (!float.TryParse(txtVadeFarki.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out vadeFarki))
                    {
                        MessageHelper.ShowError("Vade farkı geçerli bir sayı değil.");
                        return;
                    }
                }

                _offerManager.UpdateOffer(
                    offer_id,
                    Convert.ToInt32(chkFirmalar.SelectedValue),
                    yetkiliId,
                    dateTimePicker1.Value,
                    chkTeslimSekli.Text,
                    chkOdemeSekli.Text,
                    Convert.ToInt32(txtOdemeVadesi.Text),
                    Convert.ToInt32(txtTeklifSuresi.Text),
                    txtDovizKuru.Text,
                    Convert.ToChar(chkDovizBirimi.Text),
                    chkVade.Text,
                    vadeFarki,
                    txtLME.Text,
                    txtİscilik.Text,
                    txtIskonto.Text,
                    "20",
                    chkTevkifat.Checked,
                    chkDurum.Text);

                ItemManager itemMgr = new ItemManager();
                itemMgr.UpdateItemPricesByOffer(offer_id);
                _offerManager.UpdateOfferById(offer_id);

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
                    itemEditor editor = new itemEditor(offer_id, kalemId, "Edit", new ItemManager());
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
                        ItemManager manager = new ItemManager();
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
            else if (chkOdemeSekli.SelectedIndex == 1) { chkVade.Items.Clear(); chkVade.Items.Add("Vadeli"); }
            else { chkVade.Items.Clear(); chkVade.Items.Add("Peşin"); chkVade.Items.Add("Taksit"); }
        }

        private void chkVade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkOdemeSekli.Text == "Nakit" && chkVade.Text == "Peşin")
            {
                txtOdemeVadesi.Enabled = false;
                txtVadeFarki.Enabled = false;
            }
            else
            {
                txtOdemeVadesi.Enabled = true;
                txtVadeFarki.Enabled = true;
            }


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
            itemEditor itemEditor = new itemEditor(offer_id, null, "Add", new ItemManager());
            itemEditor.ShowDialog();
            LoadProducts();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            OfferManager offerManager = new OfferManager();

            if (offerManager.UpdateOfferById(offer_id))
            {
                await OfferPdfExporter.ExportOfferToPdfAsync(offer_id.Value, chkTevkifat.Checked);
            }
            else
            {
                MessageBox.Show("Teklif güncellenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogError(string message)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
                File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
            }
            catch
            {
                // loglama başarısızsa sessiz geç
            }
        }



        private void txtİscilik_TextChanged(object sender, EventArgs e)
        {
            HesaplaVeGoster();
        }

        private void txtLME_TextChanged(object sender, EventArgs e)
        {
            HesaplaVeGoster();
        }

        private void HesaplaVeGoster()
        {
            decimal iscilik = 0, lme = 0, sonuc = 0;

            // TryParse ile güvenli dönüşüm
            decimal.TryParse(txtİscilik.Text, NumberStyles.Any, new CultureInfo("tr-TR"), out iscilik);
            decimal.TryParse(txtLME.Text, NumberStyles.Any, new CultureInfo("tr-TR"), out lme);

            sonuc = (iscilik / 1000) + (lme / 1000);
            label12.Text = "Birim Fiyat (₺/kg): " + sonuc.ToString("N2", new CultureInfo("tr-TR"));
        }
    }
}
