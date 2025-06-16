using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Forms;
using Teklif_Hazırlayıcı.Helpers;
using Teklif_Hazırlayıcı.DataAccess; // Assuming DataAccess namespace contains SqlDbConnection and OfferRepository

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
        private readonly DataAccess.SqlDbConnection _connection;
        private readonly DataAccess.OfferRepository _offerRepository;
        public OfferManager()
        {
            /*
             *
             * DbConnection sınıfından yeni bir örnek oluşturularak 
             * _connection alanına atanır. Veritabanı bağlantısını başlatmak için kullanılır.
             *
             */
            _connection = new DataAccess.SqlDbConnection();
            _offerRepository = new DataAccess.OfferRepository();
        }


        #region Teklif Listeleme
        public DataTable GetOffer()
        {
            try
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
            SELECT t.teklif_id, t.yetkili_id, y.isim, y.soyisim, y.hitap, f.isim, t.teklif_tarih, t.durum
            FROM (teklifler AS t
            LEFT JOIN firmalar AS f ON t.firma_id = f.firma_id)
            LEFT JOIN yetkililer AS y ON t.yetkili_id = y.yetkili_id;";

                using (SqlCommand cmd = new SqlCommand(query, _connection.GetConnection()))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }
        public DataTable GetOfferById(int? offer_id)
        {
            try
            {
                string query = @"
    SELECT 
        t.teklif_id,
        t.firma_id,
        t.yetkili_id,
        f.isim AS isim,
        y.isim AS yetkili_adi,
        y.soyisim AS yetkili_soyadi,
        y.hitap,
        t.teklif_tarih,
        t.teslim_sekli,
        t.odeme_sekli,
        t.odeme_vade,
        t.teklif_sure,
        t.doviz_kuru,
        t.doviz_birimi,
        t.vade,
        t.vade_farki,
        t.lme,
        t.iscilik,
        t.toplam_adet,
        t.toplam_kg,
        t.mal_hizmet_bedeli,
        t.iskonto_orani,
        t.iskonto_tutari,
        t.kdv_orani,
        t.kdv_tutari,
        t.tevkifat,
        t.tevkifat_orani,
        t.tevkifat_tutari,
        t.genel_toplam,
        t.odenecek,
        t.durum
    FROM 
        (teklifler AS t
        LEFT JOIN firmalar AS f ON t.firma_id = f.firma_id)
        LEFT JOIN yetkililer AS y ON t.yetkili_id = y.yetkili_id
    WHERE t.teklif_id = @OfferId";

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OfferId", offer_id); // OleDb: parametre adı değil, sırası önemli

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt.Rows.Count > 0 ? dt : null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Teklif Arama
        public DataTable Search(string search)
        {
            try
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
            SELECT y.isim, y.soyisim, y.hitap, f.isim, t.teklif_tarih, t.durum
            FROM (teklifler t 
            LEFT JOIN firmalar f ON t.firma_id = f.firma_id)
            LEFT JOIN yetkililer y ON y.firma_id = f.firma_id
            WHERE y.isim LIKE @Name 
               OR y.soyisim LIKE @Surname
               OR f.isim LIKE @CompanyName";

                DataTable dt = new DataTable();

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        string likeValue = $"%{search}%";
                        cmd.Parameters.AddWithValue("@Name", likeValue);
                        cmd.Parameters.AddWithValue("@Surname", likeValue);
                        cmd.Parameters.AddWithValue("@CompanyName", likeValue);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message); 
                throw;
            }
        }
        #endregion

        #region Teklif Ekleme
        /// <summary>
        /// Adds a new offer and returns its identifier.
        /// </summary>
        public int AddOffer(int firma_id, int yetkili_id, DateTime teklif_tarih, string teslim_sekli, string odeme_sekli, int odeme_vadesi, int teklif_suresi, string doviz_kuru, char doviz_birimi, string vade, float vadefarki, string lme, string iscilik, string iskonto_orani, string kdv_orani, bool tevkifat, string durum)
        {
            try
            {
                decimal iskontoDecimal = ParseDecimal(iskonto_orani, 0);
                decimal kdvDecimal = ParseDecimal(kdv_orani, 20);
                decimal lmeDecimal = ParseDecimal(lme, 0);
                decimal iscilikDecimal = ParseDecimal(iscilik, 0);

                return _offerRepository.InsertOffer(
                    firma_id,
                    yetkili_id,
                    teklif_tarih,
                    teslim_sekli,
                    odeme_sekli,
                    odeme_vadesi,
                    teklif_suresi,
                    doviz_kuru,
                    doviz_birimi,
                    vade,
                    vadefarki,
                    lmeDecimal,
                    iscilikDecimal,
                    iskontoDecimal,
                    kdvDecimal,
                    tevkifat,
                    durum);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        #endregion

        #region Teklif Güncelleme
        /// <summary>
        /// Updates an offer with the provided values.
        /// </summary>
        public void UpdateOffer(int? teklif_id, int firma_id, int yetkili_id, DateTime teklif_tarih, string teslim_sekli, string odeme_sekli, int odeme_vadesi, int teklif_suresi, string doviz_kuru, char doviz_birimi, string vade, float vade_farki, string lme, string iscilik, string iskonto_orani, string kdv_orani, bool tevkifat, string durum)
        {
            try
            {
                if (!teklif_id.HasValue)
                {
                    MessageHelper.ShowError("Teklif bulunamadı.");
                    return;
                }

                decimal iskontoDecimal = ParseDecimal(iskonto_orani, 0);
                decimal kdvDecimal = ParseDecimal(kdv_orani, 0);
                decimal lmeDecimal = ParseDecimal(lme, 0);
                decimal iscilikDecimal = ParseDecimal(iscilik, 0);

                _offerRepository.UpdateOffer(
                    teklif_id.Value,
                    firma_id,
                    yetkili_id,
                    teklif_tarih,
                    teslim_sekli,
                    odeme_sekli,
                    odeme_vadesi,
                    teklif_suresi,
                    doviz_kuru,
                    doviz_birimi,
                    vade,
                    vade_farki,
                    lmeDecimal,
                    iscilikDecimal,
                    iskontoDecimal,
                    kdvDecimal,
                    tevkifat,
                    durum);

                MessageHelper.ShowSuccess("Teklif başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }



        public bool UpdateOfferById(int? teklifId)
        {

            try
            {
                /*
                     * 
                     * İskonto oranını al.
                     * Toplamları hesapla.
                     * Finansal hesaplamaları yap.
                     * Güncelleme işlemini yap.
                     * 
                     */
                decimal iskontoOrani = 0;
                bool tevkifatUygulanacak = false;


                string offerQuery = "SELECT iskonto_orani, tevkifat FROM teklifler WHERE teklif_id = @teklifId";

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(offerQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@teklifId", teklifId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["iskonto_orani"] != DBNull.Value)
                                {
                                    iskontoOrani = Convert.ToDecimal(reader["iskonto_orani"]);
                                }
                                else
                                {
                                    iskontoOrani = 0;
                                }

                                tevkifatUygulanacak = reader["tevkifat"] != DBNull.Value && Convert.ToBoolean(reader["tevkifat"]);
                            }
                        }
                    }

                    decimal toplamAdet = 0, toplamKg = 0, toplamTutar = 0;
                    string toplamAdetStr = "0", toplamKgStr = "0", toplamTutarStr = "0";

                    // 2.1.  Kg - aksesuar hariç
                    string selectAdetKgQuery = @"
                SELECT                    
                    ISNULL(SUM(k.kg), 0) AS ToplamKg
                FROM kalemler k
                INNER JOIN urunler u ON k.urun_id = u.urun_id
                WHERE k.teklif_id = @teklifId AND (u.kategori IS NULL OR u.kategori <> 'aksesuar')";

                    using (SqlCommand cmd = new SqlCommand(selectAdetKgQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@teklifId", teklifId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                if (reader["ToplamKg"] != DBNull.Value)
                                {
                                    toplamKg = Convert.ToDecimal(reader["ToplamKg"]);
                                    toplamKgStr = toplamKg.ToString().Replace(".", ",");
                                }
                                else
                                {
                                    toplamKgStr = "0";
                                }
                            }
                        }
                    }
                    try
                    {


                        string selectTutarQuery = @"
                SELECT 
                    ISNULL(SUM(k.toplam_tutar), 0) AS ToplamTutar,
                    ISNULL(SUM(k.adet), 0) AS ToplamAdet
                FROM kalemler k
                WHERE k.teklif_id = @teklifId";

                        using (SqlCommand cmd = new SqlCommand(selectTutarQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@teklifId", teklifId);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (reader["ToplamAdet"] != DBNull.Value)
                                    {
                                        toplamAdet = Convert.ToDecimal(reader["ToplamAdet"]);
                                        toplamAdetStr = toplamAdet.ToString().Replace(".", ",");
                                    }
                                    if (reader["ToplamTutar"] != DBNull.Value)
                                    {
                                        toplamTutar = Convert.ToDecimal(reader["ToplamTutar"]);
                                        toplamTutarStr = toplamTutar.ToString().Replace(".", ",");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageHelper.ShowError("Toplam tutar hesaplanırken bir hata oluştu: " + ex.Message);
                        return false;
                    }


                    // 3. Finansal hesaplamalar
                    decimal iskontoTutar = ((toplamTutar * (iskontoOrani / 100)));
                    decimal iskontoSonrasi = toplamTutar - iskontoTutar;
                    decimal kdv = iskontoSonrasi * 0.20m;
                    decimal aluminyumTutar = GetToplamAluminyumTutari(teklifId.Value);
                    decimal tevkifat = ((((aluminyumTutar * 20) / 100) * 70) / 100);
                    decimal genelToplam = iskontoSonrasi + kdv;
                    decimal odenecek = genelToplam - tevkifat;

                    // 4. Güncelleme
                    string updateQuery = @"
                UPDATE teklifler SET 
                    toplam_adet = @toplamAdet,
                    toplam_kg = @toplamKg,
                    mal_hizmet_bedeli = @toplamTutar,
                    iskonto_tutari = @iskontoTutar,
                    kdv_tutari = @kdv,
                    tevkifat_tutari = @tevkifat,
                    genel_toplam = @genelToplam,
                    odenecek = @odenecek
                WHERE teklif_id = @teklifId";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@toplamAdet", toplamAdet);
                        cmd.Parameters.AddWithValue("@toplamKg", toplamKg);
                        cmd.Parameters.AddWithValue("@toplamTutar", toplamTutar);
                        cmd.Parameters.AddWithValue("@iskontoTutar", iskontoTutar);
                        cmd.Parameters.AddWithValue("@kdv", kdv);
                        cmd.Parameters.AddWithValue("@tevkifat", tevkifat);
                        cmd.Parameters.AddWithValue("@genelToplam", genelToplam);
                        cmd.Parameters.AddWithValue("@odenecek", odenecek);
                        cmd.Parameters.AddWithValue("@teklifId", teklifId);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Teklif Silme
        public bool DeleteOffer(int teklif_id)
        {
            try
            {
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();

                    // Önce kalemleri sil
                    var deleteItemsCmd = new SqlCommand("DELETE FROM kalemler WHERE teklif_id = @TeklifId", conn);
                    deleteItemsCmd.Parameters.AddWithValue("@TeklifId", teklif_id);
                    deleteItemsCmd.ExecuteNonQuery();

                    // Sonra teklifi sil
                    var deleteOfferCmd = new SqlCommand("DELETE FROM teklifler WHERE teklif_id = @TeklifId", conn);
                    deleteOfferCmd.Parameters.AddWithValue("@TeklifId", teklif_id);
                    int result = deleteOfferCmd.ExecuteNonQuery();

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        #endregion
        
        #region Teklif Detay Getirme
        public DataTable GetOfferDetailById(int teklif_id)
        {
            try
            {
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"
                SELECT 
                    f.isim AS FirmaIsim, 
                    y.isim AS YetkiliIsim,
                    y.soyisim AS YetkiliSoyisim,
                    f.adres AS FirmaAdres,
                    y.telefon AS YetkiliTelefon,    
                    y.eposta AS YetkiliEposta,
                    t.teklif_tarih, 
                    t.toplam_adet, 
                    t.toplam_kg, 
                    t.mal_hizmet_bedeli, 
                    t.iskonto_orani, 
                    t.iskonto_tutari, 
                    t.kdv_tutari, 
                    t.tevkifat_tutari, 
                    t.genel_toplam, 
                    t.odenecek, 
                    t.doviz_birimi,
                    t.teslim_sekli,
                    t.odeme_sekli,
                    t.odeme_vade,
                    t.teklif_sure,
                    t.doviz_kuru,
                    t.vade,
                    t.vade_farki
                FROM ((teklifler t
                LEFT JOIN firmalar f ON f.firma_id = t.firma_id)
                LEFT JOIN yetkililer y ON y.yetkili_id = t.yetkili_id)
                WHERE t.teklif_id = @TeklifId", conn);

                    cmd.Parameters.AddWithValue("@TeklifId", teklif_id);

                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Alüminyum Tutarı Getirme
        public decimal GetToplamAluminyumTutari(int teklif_id)
        {
            try
            {
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"
            SELECT u.kategori, k.toplam_tutar
            FROM kalemler k
            INNER JOIN urunler u ON k.urun_id = u.urun_id
            WHERE k.teklif_id = @TeklifId", conn);

                    cmd.Parameters.AddWithValue("@TeklifId", teklif_id);

                    decimal toplamAluminyum = 0;
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string kategori = rdr["kategori"].ToString().Trim().ToLower();
                            if (kategori == "alüminyum")
                            {
                                decimal.TryParse(rdr["toplam_tutar"].ToString(), out decimal tutar);
                                toplamAluminyum += tutar;
                            }
                        }
                    }
                    return toplamAluminyum;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Teklif Kalemleri Getirme
        public DataTable GetTeklifKalemleri(int teklif_id)
        {
            try
            {
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"
            SELECT u.kalip_no, u.urun, k.yuzey, k.yuzey_kodu, k.boy, k.adet, k.kg, k.birim_fiyat, k.toplam_tutar
            FROM kalemler k
            INNER JOIN urunler u ON k.urun_id = u.urun_id
            WHERE k.teklif_id = @TeklifId", conn);

                    cmd.Parameters.AddWithValue("@TeklifId", teklif_id);

                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Parses a decimal value from string and falls back to the provided default.
        /// </summary>
        private decimal ParseDecimal(string value, decimal defaultValue)
        {
            if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                result = defaultValue;
            }
            return result;
        }

        public int GetOfferCount()
        {
            try
            {
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM teklifler", conn))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public int GetApprovedOfferCount()
        {
            try
            {
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM teklifler WHERE durum = 'Bitti'", conn))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public decimal GetTotalAmount()
        {
            try
            {
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(odenecek), 0) FROM teklifler", conn))
                    {
                        object result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public decimal GetTotalAmount(char currency)
        {
            try
            {
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(odenecek), 0) FROM teklifler WHERE doviz_birimi = @Currency", conn))
                    {
                        cmd.Parameters.AddWithValue("@Currency", currency.ToString());
                        object result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }
    }
}