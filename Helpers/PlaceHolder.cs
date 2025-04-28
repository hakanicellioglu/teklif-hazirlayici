using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teklif_Hazırlayıcı.Helpers
{
    public class PlaceHolder
    {
        private string _text;

        public PlaceHolder(string text) 
        {
            _text = text;
        }

        public void EnterPlaceHolder(TextBox textBox)
        {
            if (textBox.Text == _text)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black; // Kullanıcı yazacak: Siyah yap
            }
        }

        public void LeavePlaceHolder(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = _text;
                textBox.ForeColor = Color.Gray; // Placeholder gösteriliyor: Gri yap
            }
        }
    }
}
