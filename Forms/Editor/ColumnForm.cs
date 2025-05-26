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
    public partial class ColumnForm : Form
    {
        private readonly AuthManager _authManager;
        public ColumnForm(AuthManager authManager)
        {
            InitializeComponent();
            _authManager = authManager;
            GetColumns();
        }

        

        public List<ColumnItem> SelectedColumns
        {
            get
            {
                var selected = new List<ColumnItem>();

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i) &&
                        checkedListBox1.Items[i] is ColumnItem item)
                    {
                        selected.Add(item);
                    }
                }

                return selected;
            }
        }


        private void GetColumns()
        {
            var columns = _authManager.GetColumnDisplayNames("yetkililer");
            checkedListBox1.Items.Clear();

            foreach (var col in columns)
            {
                checkedListBox1.Items.Add(new ColumnItem(col.Name, col.DisplayName), true);
            }
        }
        public void SetColumnList(List<(string Name, string DisplayName)> columns)
        {
            checkedListBox1.Items.Clear();
            foreach (var col in columns)
            {
                checkedListBox1.Items.Add(new ColumnItem(col.Name, col.DisplayName), true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }


        private void ColumnForm_Deactivate(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }
    }
}
