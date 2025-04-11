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

namespace Teklif_Hazırlayıcı.Forms.Editor
{
    public partial class productEditor: Form
    {
        string editor_mode;
        int? product_id;
        ProductManager productManager = new ProductManager();
        public productEditor(int? productId, string editorMode)
        {
            InitializeComponent();
            editor_mode = editorMode;
            product_id = productId;
            LoadProduct();
            SelectionMode();
        }

        private void LoadProduct()
        {
            var dt = productManager.GetProduct(); // DataTable

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Ürün verileri yüklenemedi.");
                return;
            }

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "urun"; // Görünen
            comboBox1.ValueMember = "urun";    // Firma ID (veritabanı ID'si)
        }

        private void SelectionMode()
        {
            if (editor_mode == "Add")
            {
                button1.Text = "Ekle";
                btnCancel.Visible = false;
                comboBox1.DataSource = null;
            }
            else if (editor_mode == "Edit")
            {
                button1.Text = "Kaydet";
                btnCancel.Visible = true;



                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

                ProductManager manager = new ProductManager();
                var data = manager.GetProductById(product_id);

                if (data.Any())
                {
                    var product = data.First();
                    textBox2.Text = product["kalip_no"];

                    int index = comboBox1.FindStringExact(product["urun"]);
                    if (index >= 0)
                    {
                        comboBox1.SelectedIndex = index;
                    }

                    textBox3.Text = product["gramaj"].ToString();
                    int index2 = comboBox2.FindStringExact(product["kategori"]);
                    if (index2 >= 0)
                    {
                        comboBox2.SelectedIndex = index2;
                    }
                }
                else
                {
                    MessageBox.Show("Ürün bulunamadı.");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (editor_mode == "Add")
            {
                if (StringValidator.IsValid(comboBox1.Text))
                {
                    MessageHelper.ShowError("Ürün isim alanı boş bırakılamaz.");
                }
                else if(StringValidator.IsValid(comboBox2.Text))
                {
                    MessageHelper.ShowError("Kategori alanı boş bırakılamaz.");
                }
                else
                {
                    productManager.AddProduct(textBox2.Text, comboBox1.Text, Convert.ToDecimal(textBox3.Text), comboBox2.Text);
                    DialogResult = DialogResult.OK;
                }
            }
            else if (editor_mode == "Edit")
            {
                if (MessageHelper.ShowQuestion("Kaydetmek istediğinize emin misiniz? Bu işlem geri alınamaz.") == DialogResult.Yes)
                {
                    productManager.UpdateProduct(product_id, textBox2.Text, comboBox1.Text, Convert.ToDecimal(textBox3.Text), comboBox2.Text);
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
