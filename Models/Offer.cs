using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teklif_Hazırlayıcı.Models
{
    public class Offer
    {
        public int TeklifId { get; set; }
        public int FirmaId { get; set; }
        public int YetkiliId { get; set; }
        public string Hazirlayan { get; set; }
        public DateTime TeklifTarih { get; set; }
        public string TeslimSekli { get; set; }
        public string OdemeSekli { get; set; }
        public int OdemeVade { get; set; }
        public int TeklifSure { get; set; }
        public float DovizKuru { get; set; }
        public string DovizBirimi { get; set; }
        public string Vade { get; set; }
        public float VadeFarki { get; set; }
        public float Lme { get; set; }
        public float Iscilik { get; set; }
        public float ToplamAdet { get; set; }
        public float ToplamKg { get; set; }
        public float MalHizmetBedeli { get; set; }
        public float IskontoOrani { get; set; }
        public float IskontoTutari { get; set; }
        public float KdvOrani { get; set; }
        public float KdvTutari { get; set; }
        public bool Tevkifat { get; set; }
        public float TevkifatOrani { get; set; }
        public float TevkifatTutari { get; set; }
        public float GenelToplam { get; set; }
        public float Odenecek { get; set; }
        public string Durum { get; set; }
    }
}
