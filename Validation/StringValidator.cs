using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teklif_Hazırlayıcı.Validation
{
    public class StringValidator
    {
        public static bool IsValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}
