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
            label2.Text = $"Teklif Sayısı: {_offerManager.GetOfferCount()}";
            label3.Text = $"Ürün Sayısı: {_productManager.GetProductCount()}";
            label4.Text = $"Müşteri Sayısı: {_companyManager.GetCompanyCount()}";
            decimal toplamTl = _offerManager.GetTotalAmount('₺');
            decimal toplamDolar = _offerManager.GetTotalAmount('$');
            int onaylanan = _offerManager.GetApprovedOfferCount();
            label5.Text = $"Tutar (₺): {toplamTl:N2}\nTutar ($): {toplamDolar:N2}\nOnaylanan Teklif: {onaylanan}";
        }
    }
}
