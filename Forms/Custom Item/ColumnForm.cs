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
using Teklif_Hazırlayıcı.Forms;

namespace TeklifHazirlayici.Forms.Custom_Item
{
    public partial class ColumnForm : Form
    {
        private readonly string _tableName;
        private readonly DataGridView _dataGridView;

        public ColumnForm(string tableName, DataGridView dataGridView)
        {
            InitializeComponent();
            _tableName = tableName;
            _dataGridView = dataGridView;
            LoadColumns();
        }

        private void LoadColumns()
        {
            var manager = new AuthManager();
            var columns = manager.GetColumnDisplayNames(_tableName);

            if (columns == null)
                return;

            foreach (var (name, display) in columns)
            {
                bool visible = true;
                if (_dataGridView.Columns[name] != null)
                {
                    visible = _dataGridView.Columns[name].Visible;
                }

                checkedListBox1.Items.Add(new ColumnItem(name, display), visible);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                var item = (ColumnItem)checkedListBox1.Items[i];
                bool isChecked = checkedListBox1.GetItemChecked(i);
                if (_dataGridView.Columns[item.Name] != null)
                {
                    _dataGridView.Columns[item.Name].Visible = isChecked;
                }
            }

            this.Close();
        }

        public static void Open(string tableName, DataGridView dgv)
        {
            using (var form = new ColumnForm(tableName, dgv))
            {
                form.ShowDialog();
            }
        }
    }
}
