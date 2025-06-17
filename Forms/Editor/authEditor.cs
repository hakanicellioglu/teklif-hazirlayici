using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Teklif_Hazırlayıcı.Business;
using Teklif_Hazırlayıcı.Helpers;
using Teklif_Hazırlayıcı.Validation;
using TeklifHazirlayici.Properties;

namespace Teklif_Hazırlayıcı.Forms.Editor
{
    public partial class authEditor : Form
    {
        string editor_mode;
        int? auth_id;
        
        private readonly CompanyManager _companyManager;
        private readonly AuthManager _authManager;
        //CompanyManager CompanyManager = new CompanyManager();
        //uthManager AuthManager = new AuthManager();

        public authEditor(int? authId, string editorMode, CompanyManager companyManager, AuthManager authManager)
        {
            InitializeComponent();
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            auth_id = authId;
            editor_mode = editorMode;
            _companyManager = companyManager;
            _authManager = authManager;
            LoadCompany();
            SelectionMode();
        }

        private void LoadCompany()
        {
            var companyList = _companyManager.GetCompany();

            if (companyList == null || companyList.Count == 0)
            {
                MessageHelper.ShowError("Şirket verileri yüklenemedi.");
                return;
            }

            comboBox1.DataSource = null;
            comboBox1.DisplayMember = "Isim";        // Company sınıfındaki property
            comboBox1.ValueMember = "FirmaId";       // Company sınıfındaki property
            comboBox1.DataSource = companyList;
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
                button1.Text = "Kaydet";
                btnCancel.Visible = true;

                AuthManager manager = new AuthManager();
                CompanyManager companyManager = new CompanyManager();
                var data = manager.GetAuthById(auth_id);

                if (data.Any())
                {
                    var auth = data.First();

                    var company = companyManager.GetCompanyById(Convert.ToInt32(auth["firma_id"]));
                    if (company != null)
                    {
                        string firmaAdi = company.Isim;
                        int index = comboBox1.FindStringExact(firmaAdi);
                        if (index >= 0)
                        {
                            comboBox1.SelectedIndex = index;
                        }
                    }

                    textBox1.Text = auth["isim"];
                    textBox2.Text = auth["soyisim"];

                    int index2 = comboBox2.FindStringExact(auth["hitap"]);
                    if (index2 >= 0)
                    {
                        comboBox2.SelectedIndex = index2;
                    }

                    textBox3.Text = auth["adres"];
                    textBox4.Text = auth["telefon"];
                    textBox5.Text = auth["eposta"];
                }
                else
                {
                    MessageHelper.ShowError("Firma bulunamadı.");
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int companyId;
            if (editor_mode == "Add")
            {
                if (TextboxValidator.IsNullOrWhiteSpace(textBox1))
                {
                    MessageHelper.ShowError("Firma isim alanı boş bırakılamaz.");
                }
                else
                {
                    companyId = Convert.ToInt32(comboBox1.SelectedValue);
                    _authManager.AddAuth(companyId, textBox1.Text, textBox2.Text, comboBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
                    DialogResult = DialogResult.OK;
                }
            }
            else if (editor_mode == "Edit")
            {
                if (MessageHelper.ShowQuestion("Kaydetmek istediğinize emin misiniz? Bu işlem geri alınamaz.") == DialogResult.Yes)
                {
                    if (comboBox1.SelectedValue == null)
                    {
                        MessageHelper.ShowError("Lütfen bir firma seçin.");
                        return;
                    }

                    // Ardından çağrı
                    companyId = Convert.ToInt32(comboBox1.SelectedValue);

                    _authManager.UpdateAuth(
                        auth_id,
                        companyId,
                        textBox1.Text,
                        textBox2.Text,
                        comboBox2.Text,
                        textBox3.Text,
                        textBox4.Text,
                        textBox5.Text
                    );
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
