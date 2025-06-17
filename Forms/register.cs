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
using Teklif_Hazırlayıcı.Validation;
using TeklifHazirlayici.Properties;

namespace Teklif_Hazırlayıcı
{
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
            bool dark = Settings.Default.Theme.Equals("Dark", StringComparison.OrdinalIgnoreCase);
            ThemeManager.SetTheme(this, dark);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (TextboxValidator.IsNullOrWhiteSpace(txtName) ||
                TextboxValidator.IsNullOrWhiteSpace(txtSurname) ||
                TextboxValidator.IsNullOrWhiteSpace(txtUsername) ||
                TextboxValidator.IsNullOrWhiteSpace(txtEmail) ||
                TextboxValidator.IsNullOrWhiteSpace(txtPassword))
            {
                MessageHelper.ShowError("Lütfen tüm alanları eksiksiz doldurun.");
                return;
            }

            if (!StringValidator.IsValidUsername(txtUsername.Text))
            {
                MessageHelper.ShowError("Kullanıcı adı 3-20 karakter olmalı ve harf, rakam veya alt çizgi içerebilir.");
                return;
            }

            if (!StringValidator.IsValidPassword(txtPassword.Text))
            {
                MessageHelper.ShowError("Şifre 6-20 karakter olmalı ve geçersiz karakter içermemelidir.");
                return;
            }

            UserManager userManager = new UserManager();
            userManager.AddUser(txtName.Text, txtSurname.Text, txtUsername.Text, txtEmail.Text, txtPassword.Text);
        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            TogglePassword togglePassword = new TogglePassword(txtPassword, btnTogglePassword);
        }

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void register_Load(object sender, EventArgs e)
        {

        }
    }
}
