using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Business;
using Teklif_Hazırlayıcı.Forms;
using Teklif_Hazırlayıcı.Helpers;
using TeklifHazirlayici.Properties;
using Teklif_Hazırlayıcı.Validation;

namespace Teklif_Hazırlayıcı
{
    public partial class login : Form
    {
        UserManager UserManager = new UserManager();
        MessageHelper MessageHelper = new MessageHelper();

        public login()
        {
            InitializeComponent();
            bool dark = TeklifHazirlayici.Properties.Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
            CenterToScreen();
            WindowState = FormWindowState.Maximized;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (!Teklif_Hazırlayıcı.Validation.StringValidator.IsValid(txtUsername.Text))
            {
                MessageHelper.ShowError("Kullanıcı adı geçersiz.");
                return;
            }
            else if (!Teklif_Hazırlayıcı.Validation.StringValidator.IsValid(txtPassword.Text))
            {
                MessageHelper.ShowError("Şifre geçersiz.");
                return;
            }

            if (UserManager.UserExists(txtUsername.Text, txtPassword.Text))
            {
                Hide();
                UserManager.SelectUserId(txtUsername.Text);
                dashboard dashboard = new dashboard();
                dashboard.ShowDialog();
                Close();
            }
            else
            {
                MessageHelper.ShowError("Kullanıcı adı veya şifre hatalı");
            }

        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            TogglePassword togglePassword = new TogglePassword(txtPassword, btnTogglePassword);
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            register register = new register();
            register.ShowDialog();
            Show();
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btnLogin_Click(sender, e);
        }
    }
}