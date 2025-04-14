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

            if (authList == null || authList.Count == 0)
            {
                return false;
            }

            chkYetkililer.Items.Clear();
            foreach (var auth in authList)
            {
                if (auth.ContainsKey("isim"))
                {
                    string displayText = auth["isim"];

                    if (auth.ContainsKey("hitap") && !string.IsNullOrWhiteSpace(auth["hitap"]))
                    {
                        displayText = $"{auth["isim"]} {auth["hitap"]}";
                    }
                    chkYetkililer.Items.Add(displayText);

                }
            }

            if (chkYetkililer.Items.Count > 0)
                chkYetkililer.SelectedIndex = 0;

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

            chkYetkililer.Enabled = true;

            try
            {
                var selectedRow = chkFirmalar.SelectedItem as DataRowView;
                if (selectedRow != null && selectedRow["firma_id"] != DBNull.Value)
                {
                    long selectedCompany = Convert.ToInt64(selectedRow["firma_id"]);
                    LoadAuth(selectedCompany);
                }
                else
                {
                    MessageBox.Show("Şirket ID'si alınamadı (satır null veya boş).");
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
            if(ValidateForm() == true)
            {
                MessageBox.Show("Firma: " + chkFirmalar.SelectedValue.ToString());
                MessageBox.Show("Yetkili: " + chkYetkililer.SelectedValue.ToString()); 
                //OfferManager offerManager = new OfferManager();
                //offerManager.AddOffer(Convert.ToInt32(chkFirmalar.SelectedValue), Convert.ToInt32(chkYetkililer.SelectedValue), Convert.ToDateTime(dateTimePicker1.Text), chkTeslimSekli.Text, chkOdemeSekli.Text, Convert.ToInt32(txtOdemeVadesi.Text), Convert.ToInt32(txtTeklifSuresi.Text), txtDovizKuru.Text, Convert.ToChar(chkDovizBirimi.Text), chkVade.Text, Convert.ToInt32(txtLME.Text), Convert.ToInt32(txtIskonto.Text), Convert.ToInt32(txtKDV.Text), true, Convert.ToInt32(txtTevkifat.Text), chkDurum.Text);
            }
            
        }
    }
}
