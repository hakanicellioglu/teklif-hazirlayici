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
    public partial class home : Form
    {
        private readonly OfferManager _offerManager;
        private readonly ProductManager _productManager;
        private readonly CompanyManager _companyManager;

        public home()
        {
            InitializeComponent();
            _offerManager = new OfferManager();
            _productManager = new ProductManager();
            _companyManager = new CompanyManager();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            int onaylanan = _offerManager.GetApprovedOfferCount();
            label6.Text = $"{_offerManager.GetOfferCount()}";
            label7.Text = $"{_productManager.GetProductCount()}";
            label8.Text = $"{_companyManager.GetCompanyCount()}";
            decimal toplamTl = _offerManager.GetTotalAmount('₺');
            decimal toplamDolar = _offerManager.GetTotalAmount('$');
            label9.Text = $"{toplamTl:N2} | {toplamDolar:N2}";
        }
    }
}
