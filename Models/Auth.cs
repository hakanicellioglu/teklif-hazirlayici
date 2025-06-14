using System;

namespace Teklif_Hazırlayıcı.Models
{
    public class Auth
    {
        public int YetkiliId { get; set; }
        public int? FirmaId { get; set; }
        public string Isim { get; set; }
        public string Soyisim { get; set; }
        public string Hitap { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string Eposta { get; set; }
    }
}
