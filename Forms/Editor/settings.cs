using System;
using System.Configuration;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Helpers;

namespace Teklif_Hazırlayıcı.Forms.Editor
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
            LoadConnectionString();
        }

        private void LoadConnectionString()
        {
            string conn = Environment.GetEnvironmentVariable("SQL_CONN_STRING") ??
                           ConfigurationManager.ConnectionStrings["SqlConnectionString"]?.ConnectionString;
            txtConnectionString.Text = conn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConnectionString.Text))
            {
                MessageHelper.ShowError("Bağlantı dizesi boş olamaz.");
                return;
            }

            Environment.SetEnvironmentVariable("SQL_CONN_STRING", txtConnectionString.Text,
                EnvironmentVariableTarget.User);
            MessageHelper.ShowInfo("Bağlantı dizesi kaydedildi.");
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
