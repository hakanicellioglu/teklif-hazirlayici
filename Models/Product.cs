using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teklif_Hazırlayıcı.Models
{
    public class Product
    {
        public int UrunId { get; set; }
        public string KalipNo { get; set; }
        public string Urun { get; set; }
        public decimal Gramaj { get; set; }
        public string Kategori { get; set; }
    }
}
