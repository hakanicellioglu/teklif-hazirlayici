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
using Teklif_Hazırlayıcı.Properties;

namespace Teklif_Hazırlayıcı.Forms.Editor
{
    public partial class companyEditor : Form
    {
        string editor_mode;
        int? company_id;
        private readonly CompanyManager _companyManager;
        public companyEditor(int? companyId, string editorMode, CompanyManager companyManager)
        {
            InitializeComponent();
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            editor_mode = editorMode;
            company_id = companyId;
            _companyManager = companyManager;
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
                button1.Text = "Kaydet";
                btnCancel.Visible = true;

                var company = _companyManager.GetCompanyById(company_id);

                if (company != null)
                {
                    textBox1.Text = company.Isim;
                    textBox2.Text = company.Adres;
                    textBox3.Text = company.Telefon;
                    textBox4.Text = company.Eposta;
                }
                else
                {
                    MessageHelper.ShowError("Firma bulunamadı.");
                }

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (editor_mode == "Add")
            {
                if (TextboxValidator.IsNullOrWhiteSpace(textBox1))
                {
                    MessageHelper.ShowError("Firma isim alanı boş bırakılamaz.");
                }
                else
                {
                    _companyManager.AddCompany(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                    DialogResult = DialogResult.OK;
                }
            }
            else if(editor_mode == "Edit")
            {
                if(MessageHelper.ShowQuestion("Kaydetmek istediğinize emin misiniz? Bu işlem geri alınamaz.") == DialogResult.Yes)
                {
                    _companyManager.UpdateCompany(company_id, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                    DialogResult = DialogResult.OK;
                }
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
