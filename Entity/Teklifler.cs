using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teklif_Hazırlayıcı.Entity
{
    public class Teklifler
    {
        public int TeklifId { get; set; }
        public string TeklifNo { get; set; }
        public DateTime TeklifTarihi { get; set; }
        public string TeklifVerenFirma { get; set; }
        public string TeklifAlanFirma { get; set; }
        public decimal ToplamTutar { get; set; }
        public string Durum { get; set; } // Örnek: "Beklemede", "Kabul Edildi", "Reddedildi"
        public string Aciklama { get; set; } // Opsiyonel açıklama alanı

        
    }
}
