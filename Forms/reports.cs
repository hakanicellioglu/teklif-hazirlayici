using System;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Helpers;
using TeklifHazirlayici.Properties;

namespace Teklif_Hazırlayıcı.Forms
{
    public partial class reports : Form
    {
        public reports()
        {
            InitializeComponent();
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
        }
    }
}
