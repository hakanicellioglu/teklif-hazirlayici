using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Teklif_Hazırlayıcı.Business
{
    public class OfferManager
    {
        /*
        *
        * Veritabanı işlemleri için kullanılan bağlantı nesnesi. 
        * Uygulama boyunca yalnızca okunabilir (readonly) olarak tanımlanmıştır.
        *
        */
        private readonly DataAccess.DbConnection _connection;
        public OfferManager()
        {
            /*
             *
             * DbConnection sınıfından yeni bir örnek oluşturularak 
             * _connection alanına atanır. Veritabanı bağlantısını başlatmak için kullanılır.
             *
             */
            _connection = new DataAccess.DbConnection();

        }

        #region Teklif Güncelleme
        public bool UpdateOffer(int teklifId)
        {
            int toplamAdet = 0;
            decimal toplamKg = 0;

            string selectQuery = @"
        SELECT 
            IIF(ISNULL(SUM(k.adet)), 0, SUM(k.adet)) AS ToplamAdet,
            IIF(ISNULL(SUM(k.kg)), 0, SUM(k.kg)) AS ToplamKg,
            IIF(ISNULL(SUM(k.toplam_tutar)), 0, SUM(k.toplam_tutar)) AS ToplamTutar
        FROM kalemler k
        INNER JOIN urunler u ON k.urun_id = u.urun_id
        WHERE k.teklif_id = @teklifId AND (u.kategori IS NULL OR u.kategori <> 'aksesuar')";

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();

                // 1. Seçim (Toplamlar)
                using (OleDbCommand cmd = new OleDbCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@teklifId", teklifId);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            toplamAdet = reader["ToplamAdet"] != DBNull.Value ? Convert.ToInt32(reader["ToplamAdet"]) : 0;
                            toplamKg = reader["ToplamKg"] != DBNull.Value ? Convert.ToDecimal(reader["ToplamKg"]) : 0;
                        }
                    }
                }

                // 2. Güncelleme
                string updateQuery = "UPDATE teklifler SET toplam_adet = @toplamAdet, toplam_kg = @toplamKg WHERE teklif_id = @teklifId";
                using (OleDbCommand cmd = new OleDbCommand(updateQuery, conn))
                {
                    cmd.Parameters.Add("@toplamAdet", OleDbType.Integer).Value = toplamAdet;
                    cmd.Parameters.Add("@toplamKg", OleDbType.Double).Value = toplamKg;
                    cmd.Parameters.Add("@teklifId", OleDbType.Integer).Value = teklifId;

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }

            }
        }
        #endregion

        #region Teklif Listeleme
        public DataTable GetOffer()
        {
            /*
             *
             * "teklifler" tablosundaki teklifleri, ilişkili firma ve yetkili bilgileriyle birlikte getirir.
             * LEFT JOIN ile firmalar ve yetkililer tabloları birleştirilir.
             * Sonuç olarak isim, soyisim, hitap, firma adı, teklif tarihi ve teklif durumu bilgileri alınır.
             * Bu veriler bir DataTable nesnesine doldurularak döndürülür.
             *
             */
            string query = @"
            SELECT t.teklif_id, t.yetkili_id, y.isim, y.hitap, f.adi, t.teklif_tarih, t.durum
            FROM (teklifler AS t
            LEFT JOIN firmalar AS f ON t.firma_id = f.firma_id)
            LEFT JOIN yetkililer AS y ON t.yetkili_id = y.yetkili_id;";

            using (OleDbCommand cmd = new OleDbCommand(query, _connection.GetConnection()))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable GetOfferById(int? offer_id)
        {
            string query = @"
    SELECT 
        t.teklif_id,
        t.firma_id,
        t.yetkili_id,
        f.adi AS firma_adi,
        y.isim AS yetkili_adi,
        y.soyisim AS yetkili_soyadi,
        y.hitap,
        t.teklif_tarih,
        t.teslim_sekli,
        t.odeme_sekli,
        t.odeme_vadesi,
        t.teklif_suresi,
        t.doviz_kuru,
        t.doviz_birimi,
        t.vade,
        t.lme,
        t.toplam_adet,
        t.toplam_kg,
        t.mal_hizmet_tutari,
        t.iskonto_orani,
        t.iskonto_tutari,
        t.kdv_orani,
        t.kdv_tutari,
        t.tevkifat,
        t.tevkifat_orani,
        t.tevkifat_tutari,
        t.genel_toplam,
        t.odenecek_tutar,
        t.durum
    FROM 
        (teklifler AS t
        LEFT JOIN firmalar AS f ON t.firma_id = f.firma_id)
        LEFT JOIN yetkililer AS y ON t.yetkili_id = y.yetkili_id
    WHERE t.teklif_id = ?";

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", offer_id); // OleDb: parametre adı değil, sırası önemli

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt.Rows.Count > 0 ? dt : null;
                    }
                }
            }
        }



        #endregion



        #region Teklif Arama
        public DataTable Search(string search)
        {
            /*
             *
             * Arama terimine göre teklifleri, ilişkili firma ve yetkili bilgileri ile birlikte filtreleyerek getirir.
             * Arama; yetkili adı, soyadı veya firma adı üzerinden gerçekleştirilir.
             * LEFT JOIN ile "firmalar" ve "yetkililer" tabloları "teklifler" ile birleştirilir.
             * Elde edilen sonuçlar DataTable içerisine doldurulur.
             * Sonuç yoksa null döndürülür, varsa doldurulmuş DataTable döndürülür.
             *
             */
            string query = @"
            SELECT y.isim, y.soyisim, y.hitap, f.adi, t.teklif_tarih, t.durum
            FROM (teklifler t 
            LEFT JOIN firmalar f ON t.firma_id = f.firma_id)
            LEFT JOIN yetkililer y ON y.firma_id = f.firma_id
            WHERE y.isim LIKE @Name 
               OR y.soyisim LIKE @Surname
               OR f.adi LIKE @CompanyName";

            DataTable dt = new DataTable();

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    string likeValue = $"%{search}%";
                    cmd.Parameters.AddWithValue("@Name", likeValue);
                    cmd.Parameters.AddWithValue("@Surname", likeValue);
                    cmd.Parameters.AddWithValue("@CompanyName", likeValue);
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt.Rows.Count > 0 ? dt : null;
        }
        #endregion

        #region Teklif Ekleme
        public int AddOffer(int firma_id, int yetkili_id, DateTime teklif_tarih, string teslim_sekli, string odeme_sekli, int odeme_vadesi, int teklif_suresi, string doviz_kuru, char doviz_birimi, string vade, int lme, decimal iskonto_orani, decimal kdv_orani, bool tevkifat, decimal tevkifat_orani, string durum)
        {
            /*
             *
             * Yeni bir teklif kaydını "teklifler" tablosuna ekler.
             * Firma, yetkili, tarih, ödeme ve teslim bilgileri ile birlikte kur, iskonto, KDV, tevkifat ve durum gibi detaylar veritabanına yazılır.
             * Ekleme işlemi başarılı olursa, veritabanında oluşturulan teklifin ID'si alınarak döndürülür.
             * Aksi durumda -1 değeri döndürülür.
             *
             */
            int teklifId = -1;

            string query = "INSERT INTO teklifler (firma_id, yetkili_id, teklif_tarih, teslim_sekli, odeme_sekli, odeme_vadesi, teklif_suresi, doviz_kuru, doviz_birimi, vade, lme, iskonto_orani, kdv_orani, tevkifat, tevkifat_orani, durum) " +
                           "VALUES (@CompanyId, @AuthorizedPersonId, @OfferDate, @DeliveryMethod, @PaymentMethod, @PaymentDue, @OfferValidity, @ExchangeRate, @CurrencyUnit, @Term, @Lme, @DiscountRate, @VatRate, @Withholding, @WithholdingRate, @Status);";

            string getIdQuery = "SELECT @@IDENTITY;";

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("@CompanyId", OleDbType.Integer).Value = firma_id;
                    cmd.Parameters.Add("@AuthorizedPersonId", OleDbType.Integer).Value = yetkili_id;
                    cmd.Parameters.Add("@OfferDate", OleDbType.Date).Value = teklif_tarih;
                    cmd.Parameters.Add("@DeliveryMethod", OleDbType.VarChar).Value = teslim_sekli;
                    cmd.Parameters.Add("@PaymentMethod", OleDbType.VarChar).Value = odeme_sekli;
                    cmd.Parameters.Add("@PaymentDue", OleDbType.Integer).Value = odeme_vadesi;
                    cmd.Parameters.Add("@OfferValidity", OleDbType.Integer).Value = teklif_suresi;
                    cmd.Parameters.Add("@ExchangeRate", OleDbType.VarChar).Value = doviz_kuru;
                    cmd.Parameters.Add("@CurrencyUnit", OleDbType.VarChar).Value = doviz_birimi.ToString(); // char -> string
                    cmd.Parameters.Add("@Term", OleDbType.VarChar).Value = vade;
                    cmd.Parameters.Add("@Lme", OleDbType.Integer).Value = lme;
                    cmd.Parameters.Add("@DiscountRate", OleDbType.Decimal).Value = iskonto_orani;
                    cmd.Parameters.Add("@VatRate", OleDbType.Decimal).Value = kdv_orani;
                    cmd.Parameters.Add("@Withholding", OleDbType.Boolean).Value = tevkifat;
                    cmd.Parameters.Add("@WithholdingRate", OleDbType.Decimal).Value = tevkifat_orani;
                    cmd.Parameters.Add("@Status", OleDbType.VarChar).Value = durum;


                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        // Şimdi ID'yi al
                        cmd.CommandText = getIdQuery;
                        teklifId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }

            return teklifId;
        }

        #endregion

    }
}
