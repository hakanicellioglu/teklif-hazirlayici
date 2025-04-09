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

namespace Teklif_Hazırlayıcı.Forms
{
    public partial class company: Form
    {
        public company()
        {
            InitializeComponent();
            LoadCompany();
        }

        private void LoadCompany()
        {
            CompanyManager manager = new CompanyManager();
            dataGridView1.DataSource = manager.GetCompany();
        }
    }
}
