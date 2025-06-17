using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Teklif_Hazırlayıcı.Validation
{
    public class StringValidator
    {
        public static bool IsValid(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public static bool IsValidUsername(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return Regex.IsMatch(input, "^[a-zA-Z0-9_]{3,20}$");
        }

        public static bool IsValidPassword(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return Regex.IsMatch(input, "^[a-zA-Z0-9!@#$%^&*()_+-=]{6,20}$");
        }
    }
}
