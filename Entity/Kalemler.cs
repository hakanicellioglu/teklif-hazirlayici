using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teklif_Hazırlayıcı.Entity
{
    public class Kalemler
    {
        public int KalemID { get; set; }
        public int TeklifID { get; set; }
        public int UrunID { get; set; }
        public string Yuzey { get; set; }
        public string YuzeyKodu { get; set; }
        public int Boy { get; set; }
        public int Adet { get; set; }
        public float Kg { get; set; }
        public float BirimFiyat { get; set; }
        public float ToplamFiyat { get; set; }
    }
}
