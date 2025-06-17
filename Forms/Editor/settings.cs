using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using TeklifHazirlayici.Properties;
using Teklif_Hazırlayıcı.Helpers;

namespace Teklif_Hazırlayıcı.Forms.Editor
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
        }

        private void settings_Load(object sender, EventArgs e)
        {
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            txtDefaultNote.Text = Settings.Default.DefaultNote;
            cmbTheme.SelectedItem = Settings.Default.Theme;
            chkEmailApproval.Checked = Settings.Default.NotifyOnApprovalEmail;
            chkEmailNewOffer.Checked = Settings.Default.NotifyOnNewOfferEmail;
            chkSmsApproval.Checked = Settings.Default.NotifyOnApprovalSMS;
            chkSmsNewOffer.Checked = Settings.Default.NotifyOnNewOfferSMS;
            txtSignature.Text = Settings.Default.DigitalSignature;
            txtName.Text = Settings.Default.DigitalName;
            txtTitle.Text = Settings.Default.DigitalTitle;
            txtLogDirectory.Text = Settings.Default.LogDirectory;

            if (File.Exists(Settings.Default.CompanyLogoPath))
                picLogo.ImageLocation = Settings.Default.CompanyLogoPath;
        }

        private void btnBrowseLogo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picLogo.ImageLocation = ofd.FileName;
                }
            }
        }

        private void btnBrowseLogDir_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtLogDirectory.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Settings.Default.DefaultNote = txtDefaultNote.Text;
            Settings.Default.Theme = cmbTheme.SelectedItem?.ToString() ?? "Light";
            Settings.Default.NotifyOnApprovalEmail = chkEmailApproval.Checked;
            Settings.Default.NotifyOnNewOfferEmail = chkEmailNewOffer.Checked;
            Settings.Default.NotifyOnApprovalSMS = chkSmsApproval.Checked;
            Settings.Default.NotifyOnNewOfferSMS = chkSmsNewOffer.Checked;
            Settings.Default.DigitalSignature = txtSignature.Text;
            Settings.Default.DigitalName = txtName.Text;
            Settings.Default.DigitalTitle = txtTitle.Text;
            Settings.Default.CompanyLogoPath = picLogo.ImageLocation ?? string.Empty;
            Settings.Default.LogDirectory = txtLogDirectory.Text;

            TeklifHazirlayici.Properties.Settings.Default.Save();

            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.ApplyThemeToAllOpenForms(dark);

            MessageBox.Show("Ayarlar kaydedildi.");
            Close();
        }
    }
}
