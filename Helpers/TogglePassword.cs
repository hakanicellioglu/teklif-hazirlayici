using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teklif_Hazırlayıcı.Helpers
{
    public class TogglePassword
    {
        public TogglePassword(TextBox textBox, Button button) 
        {
            if (textBox.UseSystemPasswordChar)
            {
                textBox.UseSystemPasswordChar = false;
                button.ImageIndex = 0;
            }
            else
            {
                textBox.UseSystemPasswordChar = true;
                button.ImageIndex = 1;

            }
        }
    }
}
