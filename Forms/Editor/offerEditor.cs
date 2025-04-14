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

        private void LoadOffer()
        {
            if (LoadCompany())
            {
                long SelectedCompany = Convert.ToInt64(comboBox1.SelectedValue);
                comboBox2.Enabled = true;
                if (LoadAuth(SelectedCompany))
                {

                }
                else
                {
                    MessageHelper.ShowError("Yetkili bulunamadı.");
                }
            }
            else
            {
                comboBox2.Enabled = false;
            }
        }

        private bool LoadAuth(long firma_id)
        {
            var authList = AuthManager.GetAuthByCompanyId(firma_id); // FirmaId artık long? alıyor varsayıyorum

            if (authList == null || authList.Count == 0)
            {
                return false;
            }

            comboBox2.Items.Clear();
            foreach (var auth in authList)
            {
                if (auth.ContainsKey("isim"))
                    comboBox2.Items.Add(auth["isim"]);
            }

            if (comboBox2.Items.Count > 0)
                comboBox2.SelectedIndex = 0;

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

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "adi"; // Görünen
            comboBox1.ValueMember = "firma_id";    // Firma ID (veritabanı ID'si)
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox1.SelectedItem == null)
            {
                comboBox2.Enabled = false;
                comboBox2.Items.Clear();
                return;
            }

            comboBox2.Enabled = true;

            try
            {
                var selectedRow = comboBox1.SelectedItem as DataRowView;
                if (selectedRow != null && selectedRow["firma_id"] != DBNull.Value)
                {
                    long selectedCompany = Convert.ToInt64(selectedRow["firma_id"]);
                    LoadAuth(selectedCompany);
                }
                else
                {
                    MessageBox.Show("Şirket ID'si alınamadı (satır null veya boş).");
                    comboBox2.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                comboBox2.Enabled = false;
            }
        }



        private void chkTevkifat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTevkifat.Checked) textBox5.Visible = true;
            else textBox5.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
