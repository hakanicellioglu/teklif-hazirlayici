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
using Teklif_Hazırlayıcı.Helpers;
using TeklifHazirlayici.Properties;

namespace Teklif_Hazırlayıcı.Forms.Editor
{
    public partial class itemEditor : Form
    {
        int? teklif_id, kalem_id;
        string kategori, editor_mode;
        private readonly ItemManager _itemManager;

        public itemEditor(int? teklifId, int? kalemId, string editMode, ItemManager itemManager)
        {
            InitializeComponent();
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            teklif_id = teklifId;
            kalem_id = kalemId;
            editor_mode = editMode;
            _itemManager = itemManager;
            LoadProduct();
            SelectionMode();
        }

        private void SelectionMode()
        {
            if (editor_mode == "Add")
            {
                button1.Text = "Ekle";
                btnCancel.Visible = false;
            }
            else if (editor_mode == "Edit")
            {
                /*
                 * 
                 * Alüminyum mu Aksesuar mı?
                 * 
                 * Alüminyum ise boy, yuzey, yuzey_kodu görünüsn.
                 * 
                 */


                button1.Text = "Kaydet";
                btnCancel.Visible = true;

                var data = _itemManager.GetProductById(kalem_id);

                if (data != null && data.Rows.Count > 0)
                {
                    var offer = data.Rows[0];


                    string hedefUrun = offer["urun"].ToString().Trim();

                    for (int i = 0; i < chkUrunler.Items.Count; i++)
                    {
                        var item = (DataRowView)chkUrunler.Items[i];
                        string urun = item["urun"].ToString().Trim();

                        if (urun.Equals(hedefUrun, StringComparison.OrdinalIgnoreCase))
                        {
                            chkUrunler.SelectedIndex = i;
                            break;
                        }
                    }


                    txtAdet.Text = offer["adet"].ToString();

                    if (offer["kategori"].ToString() == "Alüminyum")
                    {
                        txtBoy.Visible = true;
                        chkYuzey.Visible = true;
                        txtYuzeyKodu.Visible = true;
                        txtBoy.Text = offer["boy"].ToString();
                        chkYuzey.SelectedItem = offer["yuzey"].ToString();
                        txtYuzeyKodu.Text = offer["yuzey_kodu"].ToString();
                    }
                    else
                    {
                        txtBoy.Visible = false;
                        chkYuzey.Visible = false;
                        txtYuzeyKodu.Visible = false;
                    }
                }
                else
                {
                    MessageHelper.ShowError("Kalem bilgisi bulunamadı.");
                }
            }
        }


        private bool LoadProduct()
        {
            var dt = _itemManager.GetProduct(); // DataTable

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageHelper.ShowError("Ürün verileri yüklenemedi.");
                return false;
            }

            // Yeni sütun oluştur: "kalıp no - ürün"
            dt.Columns.Add("urun_adi_gosterim", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string kalipNo = row["kalip_no"].ToString();
                string urun = row["urun"].ToString();
                row["urun_adi_gosterim"] = $"{kalipNo} - {urun}";
            }

            chkUrunler.DataSource = dt;
            chkUrunler.DisplayMember = "urun_adi_gosterim"; // Görüntülenen alan
            chkUrunler.ValueMember = "urun_id";             // Değer alanı
            chkUrunler.SelectedIndex = -1;

            return true;
        }


        private void chkUrunler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkUrunler.SelectedValue == null || chkUrunler.SelectedValue is DataRowView)
                return;

            if (int.TryParse(chkUrunler.SelectedValue.ToString(), out int item_id))
            {
                string kategoriBilgisi = _itemManager.GetCategory(item_id);
                kategori = kategoriBilgisi;

                if (kategoriBilgisi == "Alüminyum")
                {
                    lblBoy.Visible = true;
                    txtBoy.Visible = true;
                    lblYuzey.Visible = true;
                    chkYuzey.Visible = true;
                    label3.Visible = false;
                    txtBirimFiyat.Visible = false;
                }
                else
                {
                    lblBoy.Visible = false;
                    txtBoy.Visible = false;
                    lblYuzey.Visible = false;
                    chkYuzey.Visible = false;
                    lblYuzeyKodu.Visible = false;
                    txtYuzeyKodu.Visible = false;
                    label3.Visible = true;
                    txtBirimFiyat.Visible = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OfferManager offerManager = new OfferManager();
            if (editor_mode == "Add")
            {
                InitializeOfferManager(out offerManager);
                if (offerManager == null) return;

                if (offerManager.UpdateOfferById(teklif_id.Value) == true)
                {
                    chkUrunler.SelectedItem = -1;
                    chkYuzey.SelectedIndex = -1;
                    txtAdet.Text = "";
                    txtBoy.Text = "";
                    txtYuzeyKodu.Text = "";
                }
            }

            else if (editor_mode == "Edit")
            {
                // ürün güncellemesi
                if (kalem_id == null)
                {
                    MessageHelper.ShowError("Kalem ID eksik.");
                    return;
                }

                if (!int.TryParse(txtAdet.Text, out int adet))
                {
                    MessageHelper.ShowError("Adet değeri geçersiz.");
                    return;
                }

                string boyText = txtBoy.Text.Replace(",", ".");
                if (!decimal.TryParse(boyText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal boy_mm))
                {
                    boy_mm = 0;
                }

                decimal lmeTon = _itemManager.GetLMEFromTeklif(teklif_id.Value);
                decimal iscilikTon = _itemManager.Getİscilik(teklif_id.Value);
                decimal birimFiyat = (lmeTon / 1000m) + (iscilikTon / 1000m);

                decimal gramaj = _itemManager.GetGramaj((int)chkUrunler.SelectedValue);
                decimal boy_m = boy_mm / 1000m;
                decimal toplamKg = Math.Round(gramaj * boy_m * adet * 1.1m, 3);
                decimal toplamTutar = Math.Round(toplamKg * birimFiyat, 2);

                string yuzey = chkYuzey.Text;
                string yuzey_kodu = txtYuzeyKodu.Text;

                if (kategori == "Aksesuar")
                {
                    boy_mm = 0;
                    yuzey = null;
                    yuzey_kodu = null;
                }

                // Aksesuar ise birim fiyatı manuel olarak gir (veya sabit bir değerden çek)
                if (kategori == "Aksesuar")
                {
                    // Basit bir örnek: Birim fiyat textbox'ı eklenebilir, burada 100 varsayılan gibi.
                    if (!decimal.TryParse(txtBirimFiyat.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out birimFiyat))
                    {
                        MessageHelper.ShowError("Birim fiyat geçersiz.");
                        return;
                    }

                    // Toplam tutarı da buna göre güncelle
                    toplamKg = adet; // Aksesuarlar kg yerine adet bazlı olabilir
                    toplamTutar = adet * birimFiyat;
                }


                if (_itemManager.UpdateProductByKalemId(kalem_id.Value, yuzey, yuzey_kodu, adet, (int)boy_mm, toplamKg, birimFiyat, toplamTutar))
                {
                    offerManager.UpdateOfferById(teklif_id.Value);
                    MessageHelper.ShowInfo("Kalem başarıyla güncellendi.");
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageHelper.ShowError("Kalem güncellenemedi.");
                }
            }
        }

        private void InitializeOfferManager(out OfferManager offerManager)
        {
            offerManager = null;

            if (chkUrunler.SelectedValue == null || chkUrunler.SelectedValue is DataRowView)
            {
                MessageHelper.ShowError("Lütfen bir ürün seçiniz.");
                return;
            }

            if (!int.TryParse(chkUrunler.SelectedValue.ToString(), out int urun_id))
            {
                MessageHelper.ShowError("Ürün ID geçersiz.");
                return;
            }

            if (!int.TryParse(txtAdet.Text, out int adet))
            {
                MessageHelper.ShowError("Adet değeri geçersiz.");
                return;
            }

            decimal boy_mm = 0;
            decimal birimFiyat = 0;
            decimal toplamTutar = 0;
            decimal toplamKg = 0;
            decimal gramaj = 0;
            string yuzey = null;
            string yuzey_kodu = null;

            if (kategori == "Alüminyum")
            {
                string boyText = txtBoy.Text.Replace(",", ".");
                if (!decimal.TryParse(boyText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out boy_mm))
                {
                    MessageHelper.ShowError("Boy değeri geçersiz.");
                    return;
                }

                decimal lmeTon = _itemManager.GetLMEFromTeklif(teklif_id.Value);
                if (lmeTon <= 0)
                {
                    MessageHelper.ShowError("Teklif için geçerli bir LME değeri bulunamadı.");
                    return;
                }

                decimal iscilikTon = _itemManager.Getİscilik(teklif_id.Value);
                if (iscilikTon <= 0)
                {
                    MessageHelper.ShowError("Teklif için geçerli bir işçilik değeri bulunamadı.");
                    return;
                }

                decimal sonuc = (lmeTon / 1000) + (iscilikTon / 1000);
                decimal vade = _itemManager.GetVadeliFiyat(teklif_id.Value);
                int ay = _itemManager.GetVadeAy(teklif_id.Value);
                decimal vadeliFiyat = vade == 0 ? birimFiyat = sonuc : birimFiyat = sonuc * (1 + (vade / 100) * ay);


                gramaj = _itemManager.GetGramaj(urun_id);
                decimal boy_m = boy_mm / 1000m;
                toplamKg = Math.Round(gramaj * boy_m * adet * 1.1m, 3);
                toplamTutar = Math.Round(toplamKg * birimFiyat, 2);

                yuzey = chkYuzey.Text;
                yuzey_kodu = txtYuzeyKodu.Text;
            }
            else if (kategori == "Aksesuar")
            {
                boy_mm = 0;
                yuzey = null;
                yuzey_kodu = null;

                if (!decimal.TryParse(txtBirimFiyat.Text.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out birimFiyat))
                {
                    MessageHelper.ShowError("Birim fiyat geçersiz.");
                    return;
                }

                toplamKg = adet;
                toplamTutar = adet * birimFiyat;
            }

            _itemManager.AddProduct(teklif_id, urun_id, yuzey, yuzey_kodu, adet, (int)boy_mm, toplamKg, birimFiyat, toplamTutar);
            offerManager = new OfferManager();
        }



        private void chkYuzey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkYuzey.Text == "Press")
            {
                lblYuzeyKodu.Visible = false;
                txtYuzeyKodu.Visible = false;
            }
            else
            {
                lblYuzeyKodu.Visible = true;
                txtYuzeyKodu.Visible = true;
            }
        }
    }
}
