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
using Teklif_Hazırlayıcı.Forms.Custom_Item;
using Teklif_Hazırlayıcı.Forms.Editor;
using Teklif_Hazırlayıcı.Helpers;
using Teklif_Hazırlayıcı.Validation;

namespace Teklif_Hazırlayıcı.Forms
{
    public partial class product: Form
    {
        ProductManager productManager = new ProductManager();
        public product()
        {
            InitializeComponent();
            LoadProduct();
        }

        PlaceHolder placeHolder = new PlaceHolder("Ürün arayın...");
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            placeHolder.LeavePlaceHolder(txtSearch);
            dataGridView1.DataSource = null;
            LoadProduct();
        }

        private void LoadProduct()
        {
            dataGridView1.DataSource = productManager.GetProduct();
            SetupGridColumnProperties();
            SetupProductGridColumns();
        }

        private void SetupGridColumnProperties()
        {
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.Resizable = DataGridViewTriState.False;
            }
        }

        private void SetupProductGridColumns()
        {
            // Gereksiz sütunları gizle
            if (dataGridView1.Columns["urun_id"] != null)
                dataGridView1.Columns["urun_id"].Visible = false;

            if (dataGridView1.Columns["kalip_no"] != null)
            {
                dataGridView1.Columns["kalip_no"].DisplayIndex = 1;
                dataGridView1.Columns["kalip_no"].HeaderText = "Kalıp No";
            }

            if (dataGridView1.Columns["urun"] != null)
            {
                dataGridView1.Columns["urun"].DisplayIndex = 2;
                dataGridView1.Columns["urun"].HeaderText = "Ürün";
            }


            if (dataGridView1.Columns["gramaj"] != null)
            {
                dataGridView1.Columns["gramaj"].DisplayIndex = 3;
                dataGridView1.Columns["gramaj"].HeaderText = "Gramaj";
            }

            if (dataGridView1.Columns["kategori"] != null)
            {
                dataGridView1.Columns["kategori"].DisplayIndex = 4;
                dataGridView1.Columns["kategori"].HeaderText = "Kategori";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            if (TextboxValidator.IsNullOrWhiteSpace(txtSearch))
            {
                MessageHelper.ShowError("Arama alanı boş bırakılamaz.");
            }
            else
            {
                var result = productManager.Search(txtSearch.Text);

                if (result != null)
                {
                    dataGridView1.DataSource = result;
                    SetupGridColumnProperties();
                    SetupProductGridColumns();
                }
                else
                {
                    MessageHelper.ShowError("Aramaya uygun ürün bulunamadı.");
                }
            }

        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            using (var productEditor = new productEditor(null, "Add"))
            {
                productEditor.ShowDialog();
            }
            LoadProduct();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int? value = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString());
                var result = CustomMessageBox.Show("Bu yetkili düzenlemek veya silmek istiyor musunuz?");

                if (result == CustomMessageBox.CustomResult.Duzenle)
                {
                    using (var editor = new productEditor(value, "Edit"))
                    {
                        editor.ShowDialog();
                    }
                    LoadProduct();
                }
                else if (result == CustomMessageBox.CustomResult.Sil)
                {
                    var confirm = MessageBox.Show("Bu yetkiliyi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        productManager.DeleteProduct(value.Value);
                        LoadProduct();
                    }
                }
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnSearch_Click(sender, e);

        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            placeHolder.EnterPlaceHolder(txtSearch);
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            placeHolder.LeavePlaceHolder(txtSearch);
        }
    }
}
