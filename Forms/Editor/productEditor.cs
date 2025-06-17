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
using System.Globalization;
using System.Threading;
using TeklifHazirlayici.Properties;

namespace Teklif_Hazırlayıcı.Forms.Editor
{
    public partial class productEditor : Form
    {
        string editor_mode;
        int? product_id;
        private readonly ProductManager _productManager;
        public productEditor(int? productId, string editorMode, ProductManager productManager)
        {
            InitializeComponent();
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            editor_mode = editorMode;
            product_id = productId;
            _productManager = productManager;
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

                var data = _productManager.GetProductById(product_id);

                if (data.Any())
                {
                    var product = data.First();
                    textBox2.Text = product["kalip_no"];

                    textBox1.Text = product["urun"];
                    textBox3.Text = product["gramaj"].ToString();
                    int index2 = comboBox2.FindStringExact(product["kategori"]);
                    if (index2 >= 0)
                    {
                        comboBox2.SelectedIndex = index2;
                    }
                }
                else
                {
                    MessageHelper.ShowError("Ürün bulunamadı.");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (editor_mode == "Add")
            {
                if (!StringValidator.IsValid(textBox1.Text))
                {
                    MessageHelper.ShowError("Ürün isim alanı boş bırakılamaz.");
                }
                else if (!StringValidator.IsValid(comboBox2.Text))
                {
                    MessageHelper.ShowError("Kategori alanı boş bırakılamaz.");
                }
                else
                {
                    if (StringValidator.IsValid(textBox1.Text))
                    {
                        decimal gramaj;
                        ExtractGramaj(out gramaj);
                        _productManager.AddProduct(textBox2.Text, textBox1.Text, gramaj, comboBox2.Text);
                        DialogResult = DialogResult.OK;
                    }
                }
            }
            else if (editor_mode == "Edit")
            {
                if (MessageHelper.ShowQuestion("Kaydetmek istediğinize emin misiniz? Bu işlem geri alınamaz.") == DialogResult.Yes)
                {
                    decimal gramaj;
                    ExtractGramaj(out gramaj);
                    _productManager.UpdateProduct(product_id, textBox2.Text, textBox1.Text, gramaj, comboBox2.Text);
                    DialogResult = DialogResult.OK;
                }
            }

        }

        private void ExtractGramaj(out decimal gramaj)
        {
            if(comboBox2.Text == "Aksesuar")
            {
                textBox3.Text = "0"; // Aksesuar için gramaj 0 olarak ayarlanır
                gramaj = 0; // Aksesuar için gramaj 0 olarak ayarlanır
                return;
            }

            string gramajMetin = textBox3.Text.Trim();
            gramajMetin = gramajMetin.Replace(',', '.');
            bool isValid = decimal.TryParse(
                gramajMetin,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, // . = ondalık
                out gramaj
            );

            if (!isValid || gramaj <= 0)
            {
                MessageHelper.ShowError("Lütfen geçerli ve pozitif bir gramaj değeri giriniz. Örnek: 1,1 veya 1.1");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar))
            {
                e.Handled = true; // karakter girişini iptal eder
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Aksesuar")
            {
                textBox3.Enabled = false;
                textBox3.Text = "0"; // Aksesuar için gramaj 0 olarak ayarlanır
            }
            else
            {
                textBox3.Enabled = true;
                textBox3.Text = ""; // Diğer kategoriler için gramaj alanı boş bırakılır
            }
        }
    }
}
