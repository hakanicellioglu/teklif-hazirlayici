using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teklif_Hazırlayıcı.Forms
{
    public class ColumnItem
    {
        public string Name { get; set; } // gerçek sütun adı
        public string DisplayName { get; set; } // kullanıcıya görünen ad

        public ColumnItem(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName; // UI'de sadece görünen ad görünsün
        }
    }

}
