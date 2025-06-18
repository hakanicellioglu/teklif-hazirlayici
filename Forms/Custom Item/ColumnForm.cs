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

        public ColumnForm(string tableName)
        {
            InitializeComponent();
            _tableName = tableName;
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
                checkedListBox1.Items.Add(new ColumnItem(name, display), true);
            }
        }

        public static void Show(string tableName)
        {
            using (var form = new ColumnForm(tableName))
            {
                form.ShowDialog();
            }
        }
    }
}
