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
    public partial class companyEditor : Form
    {

        string editor_mode;
        int? company_id;
        CompanyManager companyManager = new CompanyManager();

        public companyEditor(int? companyId, string editorMode)
        {
            InitializeComponent();
            editor_mode = editorMode;
            company_id = companyId;
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

                CompanyManager manager = new CompanyManager();
                var data = manager.GetCompanyById(company_id);

                if (data.Any())
                {
                    var company = data.First();
                    textBox1.Text = company["adi"];
                    textBox2.Text = company["adres"];
                    textBox3.Text = company["telefon"];
                    textBox4.Text = company["eposta"];
                }
                else
                {
                    MessageBox.Show("Firma bulunamadı.");
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
                    companyManager.AddCompany(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                    DialogResult = DialogResult.OK;
                }
            }
            else if(editor_mode == "Edit")
            {
                if(MessageHelper.ShowQuestion("Kaydetmek istediğinize emin misiniz? Bu işlem geri alınamaz.") == DialogResult.Yes)
                {
                    companyManager.UpdateCompany(company_id, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
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
