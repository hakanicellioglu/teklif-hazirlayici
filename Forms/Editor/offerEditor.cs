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
using iTextSharp.text;
using iTextSharp.text.pdf;
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
        private readonly CompanyManager _companyManager = new CompanyManager();
        private readonly AuthManager _authManager = new AuthManager();
        private readonly OfferManager _offerManager = new OfferManager();


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

                txtIskonto.Text = "0";
                txtTevkifat.Text = "0";

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

                var data = _offerManager.GetOfferById(offer_id);

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

            foreach (DataColumn col in dt.Columns)
            {
                Console.WriteLine("Kolon: " + col.ColumnName);
            }

            // Sütun adı doğruysa devam et
            chkFirmalar.DataSource = dt;
            chkFirmalar.DisplayMember = "isim";  // ← isim yerine firma_adi olabilir
            chkFirmalar.ValueMember = "firma_id";

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


                _offerManager.UpdateOffer(offer_id, Convert.ToInt32(chkFirmalar.SelectedValue), yetkiliId, dateTimePicker1.Value, chkTeslimSekli.Text, chkOdemeSekli.Text, Convert.ToInt32(txtOdemeVadesi.Text), Convert.ToInt32(txtTeklifSuresi.Text), txtDovizKuru.Text, Convert.ToChar(chkDovizBirimi.Text), chkVade.Text, txtLME.Text, txtİscilik.Text, txtIskonto.Text, "20", chkTevkifat.Checked, txtTevkifat.Text, chkDurum.Text);
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
                    itemEditor editor = new itemEditor(offer_id, kalemId, "Edit");
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

        private void button2_Click(object sender, EventArgs e)
        {
            OfferManager offerManager = new OfferManager();

            if (offerManager.UpdateOfferById(offer_id))
            {
                ExportOfferToPdf();
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


        private void ExportOfferToPdf()
        {
            try
            {
                OfferManager offerManager = new OfferManager();
                DataTable teklifDetay = offerManager.GetOfferDetailById(offer_id.Value);

                if (teklifDetay == null || teklifDetay.Rows.Count == 0)
                {
                    MessageBox.Show("Teklif verisi bulunamadı.");
                    return;
                }

                var row = teklifDetay.Rows[0];
                string firmaAdi = row["isim"].ToString();
                string yetkiliAdi = row["isim"].ToString();
                string teklifTarih = Convert.ToDateTime(row["teklif_tarih"]).ToString("dd.MM.yyyy");


                string teslimSekli = row["teslim_sekli"]?.ToString() ?? "-";
                string odemeSekli = row["odeme_sekli"]?.ToString() ?? "-";
                string odemeVadesi = row["odeme_vadesi"]?.ToString() ?? "-";
                string teklifSuresi = row["teklif_suresi"]?.ToString() ?? "-";
                string dovizKuru = Convert.ToDecimal(row["doviz_kuru"], CultureInfo.InvariantCulture).ToString("N2", new CultureInfo("tr-TR"));
                string vade = row["vade"]?.ToString() ?? "-";


                int toplamAdet = Convert.ToInt32(row["toplam_adet"].ToString());

                decimal toplamKg = Convert.ToDecimal(row["toplam_kg"], CultureInfo.InvariantCulture);
                string toplamKgStr = toplamKg.ToString("N3", new CultureInfo("tr-TR"));



                decimal malHizmetTutari = Convert.ToDecimal(row["mal_hizmet_tutari"], CultureInfo.InvariantCulture);
                string malHizmetTutariStr = malHizmetTutari.ToString("N2", new CultureInfo("tr-TR"));

                decimal iskontoOrani = Convert.ToDecimal(row["iskonto_orani"], CultureInfo.InvariantCulture);
                string iskontoOraniStr = iskontoOrani.ToString("N2", new CultureInfo("tr-TR"));


                decimal iskontoTutari = malHizmetTutari * iskontoOrani / 100;
                string iskontoTutariStr = iskontoTutari.ToString("N2", new CultureInfo("tr-TR"));

                decimal iskontoSonrasiTutar = malHizmetTutari - iskontoTutari;
                string iskontoSonrasiTutarStr = iskontoSonrasiTutar.ToString("N2", new CultureInfo("tr-TR"));

                decimal kdv = iskontoSonrasiTutar * 0.20m;
                string kdvStr = kdv.ToString("N2", new CultureInfo("tr-TR"));

                decimal toplamAluminyumTutari = offerManager.GetToplamAluminyumTutari(offer_id.Value);
                decimal kdvaluminyum = toplamAluminyumTutari * 0.20m;
                string kdvaluminyumStr = kdvaluminyum.ToString("N2", new CultureInfo("tr-TR"));

                decimal tevkifat = kdv * 0.70m;
                string tevkifatStr = tevkifat.ToString("N2", new CultureInfo("tr-TR"));

                decimal vergiliToplam = iskontoSonrasiTutar + kdv;
                string vergiliToplamStr = vergiliToplam.ToString("N2", new CultureInfo("tr-TR"));

                decimal odenecekTutar = iskontoSonrasiTutar + tevkifat;
                string odenecekTutarStr = odenecekTutar.ToString("N2", new CultureInfo("tr-TR"));

                char doviz_birimi = row["doviz_birimi"] != DBNull.Value ? Convert.ToChar(row["doviz_birimi"]) : '₺';


                SaveFileDialog saveFile = new SaveFileDialog
                {
                    Filter = "PDF dosyası (*.pdf)|*.pdf",
                    FileName = $"Teklif_{offer_id}.pdf"
                };

                if (saveFile.ShowDialog() != DialogResult.OK)
                    return;
                //string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");


                BaseFont baseFont;
                try
                {
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    if (!File.Exists(fontPath))
                        throw new FileNotFoundException("Arial font bulunamadı.", fontPath);

                    baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                catch (Exception ex)
                {
                    LogError("Yazı tipi yükleme hatası: " + ex.Message);
                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED); // fallback
                }


                //BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var normalFont = new iTextSharp.text.Font(baseFont, 5);
                var titleFont = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD);
                var smallFont = new iTextSharp.text.Font(baseFont, 5);

                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, new FileStream(saveFile.FileName, FileMode.Create));
                doc.Open();



                // Logo ekleniyor
                string logoPath = Path.Combine(Application.StartupPath, "Forms", "Resources", "logo.jpeg");
                if (File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(150f, 150f);
                    logo.Alignment = Element.ALIGN_LEFT;
                    logo.SpacingAfter = 10f;
                    doc.Add(logo);
                }
                if (!File.Exists(logoPath))
                {
                    MessageBox.Show("Logo bulunamadı: " + logoPath);
                }

                // TEKLİF FORMU BAŞLIĞI
                doc.Add(new Paragraph("TEKLİF FORMU", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 15
                });

                UserManager userManager = new UserManager();
                string hazirlayanAdSoyad = userManager.GetUserFullName(Properties.Settings.Default.kullanici_id);

                // Ana üst tablo: 2 ana sütun
                PdfPTable ustBilgiTable = new PdfPTable(2);
                ustBilgiTable.WidthPercentage = 100;
                ustBilgiTable.SetWidths(new float[] { 70f, 30f }); // %50 sol - %50 sağ

                // SOL içerik: Firma Bilgileri
                Paragraph solParagraf = new Paragraph();
                solParagraf.Add(new Chunk($"Firma Adı  : {firmaAdi}\n", normalFont));
                solParagraf.Add(new Chunk($"Yetkili    : {yetkiliAdi}\n", normalFont));
                solParagraf.Add(new Chunk($"Tarih      : {teklifTarih}\n", normalFont));

                PdfPCell solCell2 = new PdfPCell(solParagraf)
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_TOP
                };
                ustBilgiTable.AddCell(solCell2);

                // SAĞ içerik: Hazırlayan Bilgileri - 2 sütunluk küçük tablo
                PdfPTable sagIciTable = new PdfPTable(2);
                sagIciTable.WidthPercentage = 100;
                sagIciTable.SetWidths(new float[] { 10f, 20f });

                // Sağ iç tabloya her satırı ekleyelim
                sagIciTable.AddCell(CreateLeftCell("Teklif No:", smallFont));
                sagIciTable.AddCell(CreateRightCell(Convert.ToInt32(offer_id).ToString("D6"), smallFont));

                sagIciTable.AddCell(CreateLeftCell("Hazırlayan:", smallFont));
                sagIciTable.AddCell(CreateRightCell(hazirlayanAdSoyad, smallFont));

                sagIciTable.AddCell(CreateLeftCell("E-Mail:", smallFont));
                sagIciTable.AddCell(CreateRightCell("siparis@alumannaluminyum.com.tr", smallFont));

                sagIciTable.AddCell(CreateLeftCell("Tarih:", smallFont));
                sagIciTable.AddCell(CreateRightCell(DateTime.Now.ToString("dd.MM.yyyy"), smallFont));

                // Sağ ana hücre
                PdfPCell sagCell2 = new PdfPCell(sagIciTable)
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_TOP
                };
                ustBilgiTable.AddCell(sagCell2);

                // ANA üst tabloyu PDF'e ekle
                doc.Add(ustBilgiTable);

                // Sonra boşluk bırakalım
                doc.Add(new Paragraph(" ") { SpacingAfter = 2f });

                // Yardımcı fonksiyonlar: hücre üreticiler
                PdfPCell CreateLeftCell(string text, iTextSharp.text.Font font)
                {
                    return new PdfPCell(new Phrase(text, font))
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                }

                PdfPCell CreateRightCell(string text, iTextSharp.text.Font font)
                {
                    return new PdfPCell(new Phrase(text, font))
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                }


                var kalemler = offerManager.GetTeklifKalemleri(offer_id.Value);

                PdfPTable table = new PdfPTable(10)
                {
                    WidthPercentage = 100
                };
                table.SetWidths(new float[] { 4, 9, 25, 10, 11, 8, 8, 8, 12, 12 });
                table.KeepTogether = true;

                string[] headers = { "NO", "KOD", "ÜRÜN", "YÜZEY", "YÜZEY KODU", "BOY", "ADET", "KG", "BİRİM FİYAT", "TOPLAM TUTAR" };
                foreach (var h in headers)
                {
                    var cell = new PdfPCell(new Phrase(h, smallFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                int sira = 1;
                foreach (DataRow kalem in kalemler.Rows)
                {
                    table.AddCell(CreateCell(sira.ToString(), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(kalem["kalip_no"].ToString(), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(kalem["urun"].ToString(), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(kalem["yuzey"].ToString(), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(kalem["yuzey_kodu"].ToString(), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(kalem["boy"].ToString(), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(kalem["adet"].ToString(), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(Math.Round(Convert.ToDecimal(kalem["kg"]), 3).ToString("N3", new CultureInfo("tr-TR")), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(FormatDecimalTr(ParseDecimalTr(kalem["birim_fiyat"].ToString())) + " " + doviz_birimi, smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(Math.Round(Convert.ToDecimal(kalem["toplam_tutar"]), 2).ToString("N2", new CultureInfo("tr-TR")) + " " + doviz_birimi, smallFont, Element.ALIGN_RIGHT));

                    sira++;
                }
                doc.Add(table);

                PdfPTable toplamTable = new PdfPTable(2)
                {
                    WidthPercentage = 40,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                };
                toplamTable.SetWidths(new float[] { 60, 40 });

                PdfPTable spaceTable = new PdfPTable(1);
                PdfPCell emptyCell = new PdfPCell(new Phrase(""))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    FixedHeight = 200f
                };
                spaceTable.AddCell(emptyCell);
                doc.Add(spaceTable);

                PdfPTable teslimBilgiTable = new PdfPTable(2);
                teslimBilgiTable.KeepTogether = true;
                teslimBilgiTable.WidthPercentage = 40;
                teslimBilgiTable.SetWidths(new float[] { 50, 50 });


                teslimBilgiTable.AddCell(CreateCell("TESLİM ŞEKLİ", smallFont));
                teslimBilgiTable.AddCell(CreateCell(teslimSekli, smallFont));

                teslimBilgiTable.AddCell(CreateCell("ÖDEME ŞEKLİ ve VADESİ", smallFont));
                teslimBilgiTable.AddCell(CreateCell($"{odemeSekli} / {odemeVadesi} gün", smallFont));

                teslimBilgiTable.AddCell(CreateCell("TEKLİF GEÇERLİLİK SÜRESİ", smallFont));
                teslimBilgiTable.AddCell(CreateCell(teklifSuresi + " gün", smallFont));

                teslimBilgiTable.AddCell(CreateCell("DÖVİZ KURU (Merkez Bankası)", smallFont));
                teslimBilgiTable.AddCell(CreateCell(dovizKuru, smallFont));

                teslimBilgiTable.AddCell(CreateCell("VADE", smallFont));
                teslimBilgiTable.AddCell(CreateCell(vade, smallFont));
                teslimBilgiTable.HorizontalAlignment = Element.ALIGN_LEFT;
                //doc.Add(teslimBilgiTable);




                string[,] toplamlar = {
                    { "TOPLAM ADET", toplamAdet.ToString() },
                    { "TOPLAM KG", toplamKgStr },
                    { "MAL ve HİZMET TUTARI", malHizmetTutariStr + " " + doviz_birimi },
                    { $"HESAPLANAN İSKONTO - %{iskontoOrani}", iskontoTutariStr + " " + doviz_birimi },
                    { "İSKONTOLU TUTAR", iskontoSonrasiTutarStr + " " + doviz_birimi },
                    { "HESAPLANAN KDV", kdvStr + " " + doviz_birimi },
                    { "TEVKİFAT (bakır, çinko ve alüminyum ürünlerinin teslimi %70)", tevkifatStr + " " + doviz_birimi },
                    { "VERGİLER DAHİL GENEL TOPLAM", vergiliToplamStr + " " + doviz_birimi },
                    { "ÖDENECEK TUTAR", odenecekTutarStr + " " + doviz_birimi }
                };

                for (int i = 0; i < toplamlar.GetLength(0); i++)
                {
                    PdfPCell solCell = new PdfPCell(new Phrase(toplamlar[i, 0], smallFont))
                    {
                        Border = iTextSharp.text.Rectangle.BOX,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };
                    toplamTable.AddCell(solCell);

                    PdfPCell sagCell = new PdfPCell(new Phrase(toplamlar[i, 1], smallFont))
                    {
                        Border = iTextSharp.text.Rectangle.BOX,
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                    };
                    toplamTable.AddCell(sagCell);
                }
                //doc.Add(toplamTable);

                // 1. Ana tablo: 2 sütunlu (sol: teslim, sağ: toplam)
                PdfPTable yanYanaTable = new PdfPTable(2);
                yanYanaTable.WidthPercentage = 100;
                yanYanaTable.SetWidths(new float[] { 50, 50 }); // sol %50, sağ %50 alan kaplasın

                // 2. Teslim tablosunu hücreye sar
                PdfPCell teslimCell = new PdfPCell(teslimBilgiTable)
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    PaddingRight = 10f,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };

                // 3. Toplam tablosunu hücreye sar
                PdfPCell toplamCell = new PdfPCell(toplamTable)
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    PaddingLeft = 10f,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };

                // 4. Hücreleri yan yana tabloya ekle
                yanYanaTable.AddCell(teslimCell);
                yanYanaTable.AddCell(toplamCell);

                // 5. PDF'e ekle
                doc.Add(yanYanaTable);




                PdfPTable aciklamaTable = new PdfPTable(1);
                aciklamaTable.WidthPercentage = 100;
                aciklamaTable.KeepTogether = true;

                PdfPCell aciklamaBaslik = new PdfPCell(new Phrase("AÇIKLAMALAR", smallFont));
                aciklamaBaslik.BackgroundColor = BaseColor.LIGHT_GRAY;
                aciklamaBaslik.HorizontalAlignment = Element.ALIGN_CENTER;
                aciklamaBaslik.Border = iTextSharp.text.Rectangle.NO_BORDER;
                aciklamaBaslik.FixedHeight = 12f;
                aciklamaTable.AddCell(aciklamaBaslik);
                aciklamaTable.AddCell(CreateCell("• ÖDEMESİ YAPILMAMIŞ SİPARİŞLER, SEVK TARİHİNDEKİ FİYATTAN FATURA EDİLİR.", smallFont));
                aciklamaTable.AddCell(CreateCell("• VADELİ ÖDEMELERDE  %5 FİYAT FARKI EKLENECEKTİR.", smallFont));
                aciklamaTable.AddCell(CreateCell("• SİPARİŞLERDE FATURA KANTARDAKİ KG ÜZERİNDEN DÜZENLENİR. TEKLİFTEKİ KG BİLGİLERİ KATALOG BİLGİLERİ OLUP GERÇEK MİKTAR İLE FARKLILIK GÖSTEREBİLİR.", smallFont));
                aciklamaTable.AddCell(CreateCell("• ÖZEL BOY TÜM ÜRÜNLERDE  ±%10 ÜRETİLEBİLİR. BU DURUMDA ÜRETİLEN MAL MÜŞTERİYE SEVK EDİLİR.", smallFont));
                aciklamaTable.AddCell(CreateCell("• SİPARİŞLER MÜŞTERİ TARAFINDAN KONTROL EDİLİP ONAYLANDIKTAN SONRA PLANLAMAYA ALINIR.", smallFont));
                aciklamaTable.AddCell(CreateCell("• SİPARİŞLERDE NAKLİYE ÜCRETİ MÜŞTERİYE AİTTİR.", smallFont));



                doc.Add(aciklamaTable);

                PdfPTable bankaTable = new PdfPTable(3);
                bankaTable.WidthPercentage = 100;
                bankaTable.SetWidths(new float[] { 30, 40, 30 });
                PdfPCell bankaBaslik = new PdfPCell(new Phrase("BANKA HESAP BİLGİLERİ", smallFont));
                bankaBaslik.BackgroundColor = BaseColor.LIGHT_GRAY;
                bankaBaslik.Colspan = 3;
                bankaBaslik.HorizontalAlignment = Element.ALIGN_CENTER;
                bankaBaslik.Border = iTextSharp.text.Rectangle.NO_BORDER;
                bankaBaslik.FixedHeight = 12f;
                bankaTable.AddCell(bankaBaslik);
                bankaTable.AddCell(CreateCell("VAKIFBANK", smallFont));
                bankaTable.AddCell(CreateCell("TR44 0001 5001 5800 7321 3983 24", smallFont));
                bankaTable.AddCell(CreateCell("Alumann Alüminyum Sanayi ve Ticaret A.Ş", smallFont));

                bankaTable.AddCell(CreateCell("ALBARAKA", smallFont));
                bankaTable.AddCell(CreateCell("TR33 0020 3000 0956 2368 0000 01", smallFont));
                bankaTable.AddCell(CreateCell("Alumann Alüminyum Sanayi ve Ticaret A.Ş", smallFont));

                bankaTable.AddCell(CreateCell("VAKIF KATILIM", smallFont));
                bankaTable.AddCell(CreateCell("TR55 0021 0000 0008 3591 5000 01", smallFont));
                bankaTable.AddCell(CreateCell("Alumann Alüminyum Sanayi ve Ticaret A.Ş", smallFont));

                doc.Add(bankaTable);
                doc.Add(new Paragraph(" ") { SpacingBefore = 5f, SpacingAfter = 5f });


                PdfPTable onayTable = new PdfPTable(2);
                onayTable.WidthPercentage = 100;
                onayTable.KeepTogether = true;
                onayTable.SetWidths(new float[] { 50, 50 });
                PdfPCell tedarikciCell = new PdfPCell(new Phrase("", normalFont));
                tedarikciCell.Border = iTextSharp.text.Rectangle.BOX;
                tedarikciCell.FixedHeight = 40;
                PdfPCell tedarikciHeader = new PdfPCell(new Phrase("TEDARİKÇİ ONAYI", smallFont));
                tedarikciHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                tedarikciHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tedarikciHeader.Colspan = 1;
                tedarikciCell.HorizontalAlignment = Element.ALIGN_LEFT;
                tedarikciCell.Border = iTextSharp.text.Rectangle.BOX;
                tedarikciCell.FixedHeight = 40;
                PdfPCell musteriCell = new PdfPCell(new Phrase("", normalFont));
                musteriCell.Border = iTextSharp.text.Rectangle.BOX;
                musteriCell.FixedHeight = 40;
                PdfPCell musteriHeader = new PdfPCell(new Phrase("MÜŞTERİ ONAYI", smallFont));
                musteriHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                musteriHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                musteriHeader.Colspan = 1;
                musteriCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                musteriCell.Border = iTextSharp.text.Rectangle.BOX;
                musteriCell.FixedHeight = 40;
                onayTable.AddCell(tedarikciHeader);
                onayTable.AddCell(musteriHeader);
                onayTable.AddCell(tedarikciCell);
                onayTable.AddCell(musteriCell);
                doc.Add(onayTable);
                doc.Close();
                MessageBox.Show("PDF başarıyla oluşturuldu.", "PDF Çıktısı", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex.Message);
            }
        }

        private PdfPCell CreateCell(string text, iTextSharp.text.Font font, int alignment = Element.ALIGN_LEFT, int border = iTextSharp.text.Rectangle.NO_BORDER)
        {
            return new PdfPCell(new Phrase(text, font))
            {
                Border = border,
                HorizontalAlignment = alignment
            };
        }


        private decimal ParseDecimalTr(string value)
        {
            if (decimal.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            return 0;
        }

        public string FormatDecimalTr(decimal value, int precision = 2)
        {
            return value.ToString($"N{precision}", new CultureInfo("tr-TR"));
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
            label12.Text = sonuc.ToString("N3", new CultureInfo("tr-TR"));
        }
    }
}
