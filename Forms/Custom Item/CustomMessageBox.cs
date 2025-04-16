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
            /*
             *
             * Uygulamada özel kullanıcı eylemlerini temsil eden sonuç durumlarını belirtir.
             * `Iptal`: İşlemin iptal edildiğini belirtir.
             * `Duzenle`: Düzenleme işlemini temsil eder.
             * `Sil`: Silme işlemini temsil eder.
             *
             */
            Iptal,
            Duzenle,
            Sil
        }

        
        public CustomResult Result 
        {
            /*
             *
             * Kullanıcının yaptığı işlemin sonucunu tutan özelliktir.
             * Sadece sınıf içinden atanabilir (`private set`), dış sınıflar tarafından sadece okunabilir.
             * `CustomResult` enum türündedir ve işlem sonucunu (İptal, Düzenle, Sil) yansıtır.
             *
             */
            get;
            private set; 
        }


        public CustomMessageBox(string mesaj)
        {
            /*
             *
             * CustomMessageBox sınıfının kurucusudur.
             * Parametre olarak alınan mesaj, kullanıcıya gösterilecek olan `lblMessage` bileşenine aktarılır.
             * Varsayılan işlem sonucu `CustomResult.Iptal` olarak ayarlanır.
             *
             */
            InitializeComponent();
            lblMessage.Text = mesaj;
            Result = CustomResult.Iptal;    
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            /*
             *
             * "Düzenle" butonuna tıklandığında çalışır.
             * Sonuç olarak `Result` özelliği `CustomResult.Duzenle` olarak ayarlanır.
             * Ardından pencere kapatılır.
             *
             */
            Result = CustomResult.Duzenle;
            this.Close();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            /*
             *
             * "Sil" butonuna tıklandığında çalışır.
             * `Result` özelliği `CustomResult.Sil` olarak ayarlanır.
             * Ardından ileti kutusu kapatılır.
             *
             */
            Result = CustomResult.Sil;
            this.Close();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            /*
             *
             * "İptal" butonuna tıklandığında çalışır.
             * `Result` değeri `CustomResult.Iptal` olarak ayarlanır.
             * Ardından pencere kapatılır.
             *
             */
            Result = CustomResult.Iptal;
            this.Close();
        }

        public static CustomResult Show(string mesaj)
        {
            /*
             *
             * `CustomMessageBox` penceresini gösteren statik metottur.
             * Parametre olarak verilen mesaj, ileti kutusunda görüntülenir.
             * Diyalog kutusu kapandığında, kullanıcı tarafından seçilen işlem sonucu (`CustomResult`) döndürülür.
             *
             */
            using (var msgBox = new CustomMessageBox(mesaj))
            {
                msgBox.ShowDialog();
                return msgBox.Result;
            }
        }
    }
}
