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
using Teklif_Hazırlayıcı.Validation;

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
            //SelectionMode();
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

            // TextBox kontrolleri
            if (!IsValidInt(txtTeklifSuresi, "Teklif süresi")) return false;
            if (!IsValidInt(txtLME, "LME")) return false;
            if (!IsValidInt(txtIskonto, "İskonto")) return false;
            //if (!IsValidInt(txtTevkifat, "Tevkifat")) return false;
            if (!IsValidInt(txtDovizKuru, "Döviz kuru")) return false;
            if (!IsValidInt(txtOdemeVadesi, "Ödeme vadesi")) return false;
            if (!IsValidInt(txtKDV, "KDV")) return false;

            // CheckBox kontrolü
            if (chkTevkifat.Checked)
            {
                if (!IsValidInt(txtTevkifat, "Tevkifat"))
                    return false;
            }
            else
            {
                if (MessageHelper.ShowQuestion("Tevkifat seçilmedi. Devam etmek istiyor musunuz?") != DialogResult.Yes)
                    return false;
            }

            return true;
        }
        private bool IsValidInt(TextBox tb, string fieldName)
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
                    MessageHelper.ShowError("Yetkili bulunamadı.");
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


            chkYetkililer.Items.Clear();

            if (authList == null || authList.Count == 0)
                return false;

            chkYetkililer.DisplayMember = "Value"; // Çok önemli!

            foreach (var auth in authList)
            {
                if (auth.ContainsKey("isim") && auth.ContainsKey("yetkili_id"))
                {
                    string displayText = auth["isim"];

                    if (auth.ContainsKey("hitap") && !string.IsNullOrWhiteSpace(auth["hitap"]))
                        displayText = $"{auth["isim"]} {auth["hitap"]}";

                    chkYetkililer.Items.Add(new KeyValuePair<string, string>(auth["yetkili_id"], displayText));
                }
                else
                {
                    MessageBox.Show("Eksik veri: " + string.Join(", ", auth.Keys));
                }
            }

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
                    MessageBox.Show("Firma ID'si alınamadı.");
                    chkYetkililer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                chkYetkililer.Enabled = false;
            }
        }
        private void chkTevkifat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTevkifat.Checked) txtTevkifat.Visible = true;
            else txtTevkifat.Visible = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateForm() == true)
            {

                OfferManager offerManager = new OfferManager();
                int tevkifat = string.IsNullOrWhiteSpace(txtTevkifat.Text) ? 0 : Convert.ToInt32(txtTevkifat.Text);
                bool tevkifatVarMi = !string.IsNullOrWhiteSpace(txtTevkifat.Text) && Convert.ToDecimal(txtTevkifat.Text) > 0;
                decimal tevkifatOrani = tevkifatVarMi ? Convert.ToDecimal(txtTevkifat.Text.Trim()) : 0;
                int yetkiliId;

                if (chkYetkililer.SelectedItem is KeyValuePair<string, string> selectedAuth)
                {
                    yetkiliId = Convert.ToInt32(selectedAuth.Key);
                }
                else
                {
                    MessageBox.Show("Lütfen bir yetkili seçiniz.");
                    return;
                }


                int teklifId = offerManager.AddOffer(
                    Convert.ToInt32(chkFirmalar.SelectedValue),
                    yetkiliId,
                    dateTimePicker1.Value,
                    chkTeslimSekli.Text,
                    chkOdemeSekli.Text,
                    Convert.ToInt32(txtOdemeVadesi.Text.Trim()),
                    Convert.ToInt32(txtTeklifSuresi.Text.Trim()),
                    txtDovizKuru.Text.Trim(),
                    Convert.ToChar(chkDovizBirimi.Text.Trim().Substring(0, 1)),
                    chkVade.Text,
                    Convert.ToInt32(txtLME.Text.Trim()),
                    Convert.ToDecimal(txtIskonto.Text.Trim()),
                    Convert.ToDecimal(txtKDV.Text.Trim()),
                    tevkifatVarMi,
                    tevkifatOrani,
                    chkDurum.Text
                );

                if (teklifId > 0)
                {
                    if (MessageHelper.ShowQuestion("Teklif başarıyla oluşturuldu. Ürün eklemek ister misiniz?") == DialogResult.Yes)
                    {
                        Hide();
                        itemEditor itemEditor = new itemEditor(teklifId);
                        itemEditor.ShowDialog();
                    }
                }
                else
                {
                    MessageHelper.ShowError("Teklif eklenirken bir hata oluştu.");
                }
            }
        }
    }
}
