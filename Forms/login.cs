using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Business;
using Teklif_Hazırlayıcı.Helpers;
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
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (!Teklif_Hazırlayıcı.Validation.StringValidator.IsValid(txtUsername.Text))
            {
                MessageHelper.ShowError("Kullanıcı adı geçersiz.");
                return;
            }
            else if(!Teklif_Hazırlayıcı.Validation.StringValidator.IsValid(txtPassword.Text))
            {
                MessageHelper.ShowError("Şifre geçersiz.");
                return;
            }

            if (UserManager.UserExists(txtUsername.Text, txtPassword.Text))
            {
                MessageHelper.ShowInfo("Giriş Başarılı");
            }
            else
            {
                MessageHelper.ShowError("Kullanıcı adı veya şifre hatalı");
            }

        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            if(txtPassword.PasswordChar == '*')
            {
                txtPassword.PasswordChar = '\n';
                btnTogglePassword.Text = "Gizle";
            }
            else
            {
                txtPassword.PasswordChar = '*';
                btnTogglePassword.Text = "Göster";
            }
        }
    }
}
