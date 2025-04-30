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
        int? teklif_id, kalem_id;
        string kategori, editor_mode;
        itemManager itemManager = new itemManager();

        public itemEditor(int? teklifId, int? kalemId, string editMode)
        {
            InitializeComponent();
            teklif_id = teklifId;
            kalem_id = kalemId;
            editor_mode = editMode;
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

                var data = itemManager.GetProductById(kalem_id);

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

        private void button2_Click(object sender, EventArgs e)
        {
            OfferManager offerManager = new OfferManager();
            InitializeOfferManager(out offerManager);
            offerManager.UpdateOfferById(teklif_id.Value);
            DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OfferManager offerManager;
            InitializeOfferManager(out offerManager);
            if (offerManager == null) return;

            if (editor_mode == "Add")
            {
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

                decimal lmeTon = itemManager.GetLMEFromTeklif(teklif_id.Value);
                decimal iscilikTon = itemManager.Getİscilik(teklif_id.Value);
                decimal birimFiyat = (lmeTon / 1000m) + (iscilikTon / 1000m);

                decimal gramaj = itemManager.GetGramaj((int)chkUrunler.SelectedValue);
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


                if (itemManager.UpdateProductByKalemId(kalem_id.Value, yuzey, yuzey_kodu, adet, (int)boy_mm, toplamKg, birimFiyat, toplamTutar))
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
            if (chkUrunler.SelectedValue == null || chkUrunler.SelectedValue is DataRowView)
            {
                MessageHelper.ShowError("Lütfen bir ürün seçiniz.");
                offerManager = null;
                return;
            }

            if (!int.TryParse(chkUrunler.SelectedValue.ToString(), out int urun_id))
            {
                MessageHelper.ShowError("Ürün ID geçersiz.");
                offerManager = null;
                return;
            }

            if (!int.TryParse(txtAdet.Text, out int adet))
            {
                MessageHelper.ShowError("Adet değeri geçersiz.");
                offerManager = null;
                return;
            }

            decimal boy_mm = 0;

            if (kategori == "Alüminyum")
            {
                string boyText = txtBoy.Text.Replace(",", ".");
                if (!decimal.TryParse(boyText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out boy_mm))
                {
                    MessageHelper.ShowError("Boy değeri geçersiz.");
                    offerManager = null;
                    return;
                }
            }


            decimal lmeTon = itemManager.GetLMEFromTeklif(teklif_id.Value);
            if (lmeTon <= 0)
            {
                MessageHelper.ShowError("Teklif için geçerli bir LME değeri bulunamadı.");
                offerManager = null;
                return;
            }

            decimal iscilikTon = itemManager.Getİscilik(teklif_id.Value);
            if (iscilikTon <= 0)
            {
                MessageHelper.ShowError("Teklif için geçerli bir işçilik değeri bulunamadı.");
                offerManager = null;
                return;
            }

            decimal birimFiyat = (lmeTon / 1000m) + (iscilikTon / 1000m);

            decimal gramaj = itemManager.GetGramaj(urun_id);
            decimal boy_m = boy_mm / 1000m;
            decimal toplamKg = Math.Round(gramaj * boy_m * adet * 1.1m, 3);
            decimal toplamTutar = Math.Round(toplamKg * birimFiyat, 2);

            string yuzey = chkYuzey.Text;
            string yuzey_kodu = txtYuzeyKodu.Text;

            itemManager.AddProduct(teklif_id, urun_id, yuzey, yuzey_kodu, adet, (int)boy_mm, toplamKg, birimFiyat, toplamTutar);
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
