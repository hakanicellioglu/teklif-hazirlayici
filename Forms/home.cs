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
using TeklifHazirlayici.Properties;

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
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            _offerManager = new OfferManager();
            _productManager = new ProductManager();
            _companyManager = new CompanyManager();
            LoadStatistics();
        }

        private async void LoadStatistics()
        {
            int onaylanan = await _offerManager.GetApprovedOfferCountAsync();
            label6.Text = $"{await _offerManager.GetOfferCountAsync()}";
            label7.Text = $"{_productManager.GetProductCount()}";
            label8.Text = $"{_companyManager.GetCompanyCount()}";
            decimal toplamTl = await _offerManager.GetTotalAmountAsync('₺');
            decimal toplamDolar = await _offerManager.GetTotalAmountAsync('$');
            label9.Text = $"{toplamTl:N2} | {toplamDolar:N2}";
        }
    }
}
