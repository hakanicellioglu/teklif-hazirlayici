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

namespace Teklif_Hazırlayıcı.Forms.Editor
{
    public partial class itemEditor : Form
    {
        int? teklif_id;
        string kategori;
        itemManager itemManager = new itemManager();

        public itemEditor(int? teklifId)
        {
            InitializeComponent();
            teklif_id = teklifId;
            LoadProduct();
        }


        private bool LoadProduct()
        {
            var dt = itemManager.GetProduct(); // DataTable

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageHelper.ShowError("Ürün verileri yüklenemedi.");
                return false;
            }

            chkUrunler.DataSource = dt;
            chkUrunler.DisplayMember = "urun"; // Görünen
            chkUrunler.ValueMember = "urun_id";    // Firma ID (veritabanı ID'si)
            chkUrunler.SelectedIndex = -1;
            return true;
        }

        private void chkUrunler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkUrunler.SelectedValue == null || chkUrunler.SelectedValue is DataRowView)
                return;

            if (int.TryParse(chkUrunler.SelectedValue.ToString(), out int item_id))
            {
                string kategoriBilgisi = itemManager.GetCategory(item_id);
                kategori = kategoriBilgisi;

                if (kategoriBilgisi == "Alüminyum")
                {
                    lblBoy.Visible = true;
                    txtBoy.Visible = true;
                    lblYuzey.Visible = true;
                    chkYuzey.Visible = true;
                }
                else
                {
                    lblBoy.Visible = false;
                    txtBoy.Visible = false;
                    lblYuzey.Visible = false;
                    chkYuzey.Visible = false;
                    lblYuzeyKodu.Visible = false;
                    txtYuzeyKodu.Visible = false;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            /*
             * 
             * Toplam Kg Hesaplama
             * Ürün kimlik numarasından gramaj değerini al.
             * Boy değerini milimetre değerini metre değerine dönüştür.
             * Gramaj değerini, yeni boy değeri ile çarpıp %10 daha fazlasını al.
             * 
             */

            /*
             * 
             * Toplam Tutar Hesaplama
             * LME(kg) değeri ile birim fiyat değerini çarp.
             * 
             */
            // ✅ Ürün seçimi kontrolü
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

            // ✅ Adet kontrolü
            if (!int.TryParse(txtAdet.Text, out int adet))
            {
                MessageHelper.ShowError("Adet değeri geçersiz.");
                return;
            }

            // ✅ Boy kontrolü (virgül yerine nokta çevir)
            string boyText = txtBoy.Text.Replace(",", ".");
            if (!decimal.TryParse(boyText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal boy_mm))
            {
                MessageHelper.ShowError("Boy değeri geçersiz.");
                return;
            }

            // ✅ LME çek → birim fiyat hesapla
            decimal lmeTon = itemManager.GetLMEFromTeklif(teklif_id.Value);
            if (lmeTon <= 0)
            {
                MessageHelper.ShowError("Teklif için geçerli bir LME değeri bulunamadı.");
                return;
            }

            decimal birimFiyat = lmeTon / 1000m;

            // ✅ Hesaplamalar
            decimal gramaj = itemManager.GetGramaj(urun_id); // ürünün gramajı
            decimal boy_m = boy_mm / 1000m; // milimetreden metreye
            decimal toplamKg = gramaj * boy_m * adet * 1.1m; // %10 fazla
            decimal toplamTutar = toplamKg * birimFiyat;

            // ✅ Diğer veriler
            string yuzey = chkYuzey.Text;
            string yuzey_kodu = txtYuzeyKodu.Text;

            // ✅ Bilgilendirme (opsiyonel)
            MessageBox.Show($"Debug Bilgisi:\n" +
            $"Adet: {adet}\n" +
            $"Boy (mm): {boy_mm}\n" +
            $"Boy (m): {boy_m}\n" +
            $"Gramaj: {gramaj}\n" +
            $"LME (ton): {lmeTon}\n" +
            $"Birim Fiyat (kg): {birimFiyat}\n" +
            $"Toplam KG: {toplamKg}\n" +
            $"Toplam Tutar: {toplamTutar}",
            "DEBUG",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);


            // ✅ Veritabanına kayıt
            itemManager.AddProduct(teklif_id, urun_id, yuzey, yuzey_kodu, adet, (int)boy_mm, toplamKg, birimFiyat, toplamTutar);

            DialogResult = DialogResult.OK;
        }

        private void chkYuzey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(chkYuzey.Text == "Press")
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
