using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teklif_Hazırlayıcı.Forms.Custom_Item
{
    public partial class CustomMessageBox : Form
    {
        public enum CustomResult
        {
            Iptal,
            Duzenle,
            Sil
        }

        public CustomResult Result { get; private set; }

        public CustomMessageBox(string mesaj)
        {
            InitializeComponent();
            lblMessage.Text = mesaj;
            Result = CustomResult.Iptal;    
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            Result = CustomResult.Duzenle;
            this.Close();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            Result = CustomResult.Sil;
            this.Close();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            Result = CustomResult.Iptal;
            this.Close();
        }

        public static CustomResult Show(string mesaj)
        {
            using (var msgBox = new CustomMessageBox(mesaj))
            {
                msgBox.ShowDialog();
                return msgBox.Result;
            }
        }
    }
}
