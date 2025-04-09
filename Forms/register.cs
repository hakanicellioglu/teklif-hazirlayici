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

namespace Teklif_Hazırlayıcı
{
    public partial class register: Form
    {
        public register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
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
    }
}
