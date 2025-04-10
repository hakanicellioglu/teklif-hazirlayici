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
        public companyEditor(string editorMode)
        {
            InitializeComponent();
            editor_mode = editorMode;
            SelectionMode();
        }

        private void SelectionMode()
        {
            if (editor_mode == "Add")
            {
                button1.Text = "Ekle";
                button2.Visible = false;
            }
            else if (editor_mode == "Edit")
            {
                button1.Text = "Kaydet";
                button2.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (editor_mode == "Add")
            {
                if (TextboxValidator.IsNullOrWhiteSpace(textBox1) || TextboxValidator.IsNullOrWhiteSpace(textBox2) || TextboxValidator.IsNullOrWhiteSpace(textBox3) || TextboxValidator.IsNullOrWhiteSpace(textBox4))
                {
                    MessageHelper.ShowError("Zorunlu alan(lar) boş bırakılamaz. Lütfen alan(ları) doldurunuz.");
                }
                else
                {
                    CompanyManager companyManager = new CompanyManager();
                    companyManager.AddCompany(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                }
            }
            else if(editor_mode == "Edit")
            {
                if(MessageHelper.ShowQuestion("Kaydetmek istediğinize emin misiniz? Bu işlem geri alınamaz.") == DialogResult.Yes)
                {
                    // Güncelleme İşlemi
                }
            }

        }
    }
}
