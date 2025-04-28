using System;
using System.Collections.Generic;
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
            }
        }

        public void LeavePlaceHolder(TextBox textBox)
        {
            if (textBox.Text == "")
            {
                textBox.Text = _text;
            }
        }
    }
}
