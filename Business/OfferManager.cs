using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Forms;
using Teklif_Hazırlayıcı.Helpers;

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
        public OfferManager()
        {
            /*
             *
             * DbConnection sınıfından yeni bir örnek oluşturularak 
             * _connection alanına atanır. Veritabanı bağlantısını başlatmak için kullanılır.
             *
             */
            _connection = new DataAccess.SqlDbConnection();
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
        public int AddOffer(int firma_id, int yetkili_id, DateTime teklif_tarih, string teslim_sekli, string odeme_sekli, int odeme_vadesi, int teklif_suresi, string doviz_kuru, char doviz_birimi, string vade, float vadefarki, string lme, string iscilik, string iskonto_orani, string kdv_orani, bool tevkifat, string durum)
        {
            try
            {
                /*
                     *
                     * Yeni bir teklif kaydını "teklifler" tablosuna ekler.
                     * Firma, yetkili, tarih, ödeme ve teslim bilgileri ile birlikte kur, iskonto, KDV, tevkifat ve durum gibi detaylar veritabanına yazılır.
                     * Ekleme işlemi başarılı olursa, veritabanında oluşturulan teklifin ID'si alınarak döndürülür.
                     * Aksi durumda -1 değeri döndürülür.
                     *
                     */

                // İskonto
                decimal iskontoDecimal = 0;
                if (!decimal.TryParse(iskonto_orani, NumberStyles.Any, CultureInfo.InvariantCulture, out iskontoDecimal))
                    iskontoDecimal = 0; // Hatalıysa 0 olarak ayarla
                string iskontoStr = iskontoDecimal.ToString("0.##", new CultureInfo("tr-TR"));

                // KDV
                decimal kdvDecimal = 20;
                if (!decimal.TryParse(kdv_orani, NumberStyles.Any, CultureInfo.InvariantCulture, out kdvDecimal))
                    kdvDecimal = 20;
                string kdvStr = kdvDecimal.ToString("0.##", new CultureInfo("tr-TR"));



                // LME
                decimal lmeDecimal = 0;
                if (!decimal.TryParse(lme, NumberStyles.Any, CultureInfo.InvariantCulture, out lmeDecimal))
                    lmeDecimal = 0;
                string lmeStr = lmeDecimal.ToString("0.##", new CultureInfo("tr-TR"));

                // İşçilik
                decimal iscilikDecimal = 0;
                if (!decimal.TryParse(iscilik, NumberStyles.Any, CultureInfo.InvariantCulture, out iscilikDecimal))
                    iscilikDecimal = 0;
                string iscilikStr = iscilikDecimal.ToString("0.##", new CultureInfo("tr-TR"));


                int teklifId = -1;

                string query = "INSERT INTO teklifler (firma_id, yetkili_id, teklif_tarih, teslim_sekli, odeme_sekli, odeme_vade, teklif_sure, doviz_kuru, doviz_birimi, vade, vade_farki, lme, iscilik, iskonto_orani, kdv_orani, tevkifat, tevkifat_orani, durum) " +
                               "VALUES (@CompanyId, @AuthorizedPersonId, @OfferDate, @DeliveryMethod, @PaymentMethod, @PaymentDue, @OfferValidity, @ExchangeRate, @CurrencyUnit, @Term, @TermRate, @Lme, @Workmanship, @DiscountRate, @VatRate, @Withholding, @WithholdingRate, @Status);";

                string getIdQuery = "SELECT @@IDENTITY;";

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = firma_id;
                        cmd.Parameters.Add("@AuthorizedPersonId", SqlDbType.Int).Value = yetkili_id;
                        cmd.Parameters.Add("@OfferDate", SqlDbType.Date).Value = teklif_tarih;
                        cmd.Parameters.Add("@DeliveryMethod", SqlDbType.NVarChar).Value = teslim_sekli;
                        cmd.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar).Value = odeme_sekli;
                        cmd.Parameters.Add("@PaymentDue", SqlDbType.Int).Value = odeme_vadesi;
                        cmd.Parameters.Add("@OfferValidity", SqlDbType.Int).Value = teklif_suresi;
                        cmd.Parameters.Add("@ExchangeRate", SqlDbType.Float).Value = doviz_kuru;
                        cmd.Parameters.Add("@CurrencyUnit", SqlDbType.NVarChar).Value = doviz_birimi.ToString();
                        cmd.Parameters.Add("@Term", SqlDbType.NVarChar).Value = vade;
                        cmd.Parameters.Add("@TermRate", SqlDbType.Float).Value = vadefarki;
                        cmd.Parameters.Add("@Lme", SqlDbType.Decimal).Value = Convert.ToDecimal(lmeStr);
                        cmd.Parameters.Add("@Workmanship", SqlDbType.Decimal).Value = Convert.ToDecimal(iscilikStr);
                        cmd.Parameters.Add("@DiscountRate", SqlDbType.Decimal).Value = Convert.ToDecimal(iskontoStr);
                        cmd.Parameters.Add("@VatRate", SqlDbType.Decimal).Value = Convert.ToDecimal(kdvStr);
                        cmd.Parameters.Add("@Withholding", SqlDbType.Bit).Value = tevkifat;
                        cmd.Parameters.AddWithValue("@WithholdingRate", tevkifat ? 70 : 0);
                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = durum;


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
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        #endregion

        #region Teklif Güncelleme

        public void UpdateOffer(int? teklif_id, int firma_id, int yetkili_id, DateTime teklif_tarih, string teslim_sekli, string odeme_sekli, int odeme_vadesi, int teklif_suresi, string doviz_kuru, char doviz_birimi, string vade, float vade_farki, string lme, string iscilik, string iskonto_orani, string kdv_orani, bool tevkifat, string durum)
        {
            try
            {
                // İskonto
                decimal iskontoDecimal = 0;
                if (!decimal.TryParse(iskonto_orani, NumberStyles.Any, CultureInfo.InvariantCulture, out iskontoDecimal))
                    iskontoDecimal = 0; // Hatalıysa 0 olarak ayarla
                string iskontoStr = iskontoDecimal.ToString("0.##", new CultureInfo("tr-TR"));

                // KDV
                decimal kdvDecimal = 0;
                if (!decimal.TryParse(kdv_orani, NumberStyles.Any, CultureInfo.InvariantCulture, out kdvDecimal))
                    kdvDecimal = 0;
                string kdvStr = kdvDecimal.ToString("0.##", new CultureInfo("tr-TR"));

                // LME
                decimal lmeDecimal = 0;
                if (!decimal.TryParse(lme, NumberStyles.Any, CultureInfo.InvariantCulture, out lmeDecimal))
                    lmeDecimal = 0;
                string lmeStr = lmeDecimal.ToString("0.##", new CultureInfo("tr-TR"));

                // İşçilik
                decimal iscilikDecimal = 0;
                if (!decimal.TryParse(iscilik, NumberStyles.Any, CultureInfo.InvariantCulture, out iscilikDecimal))
                    iscilikDecimal = 0;
                string iscilikStr = iscilikDecimal.ToString("0.##", new CultureInfo("tr-TR"));





                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();


                    // Mevcut teklifi çek
                    string selectQuery = "SELECT * FROM teklifler WHERE teklif_id = @TeklifId";
                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@TeklifId", teklif_id);
                        bool isDifferent = false;
                        using (SqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isDifferent =
                                    (int)reader["firma_id"] != firma_id ||
                                    (int)reader["yetkili_id"] != yetkili_id ||
                                    Convert.ToDateTime(reader["teklif_tarih"]) != teklif_tarih ||
                                    reader["teslim_sekli"].ToString() != teslim_sekli ||
                                    reader["odeme_sekli"].ToString() != odeme_sekli ||
                                    Convert.ToInt32(reader["odeme_vade"]) != odeme_vadesi ||
                                    Convert.ToInt32(reader["teklif_sure"]) != teklif_suresi ||
                                    reader["doviz_kuru"].ToString() != doviz_kuru ||
                                    Convert.ToChar(reader["doviz_birimi"]) != doviz_birimi ||
                                    reader["vade"].ToString() != vade ||
                                    reader["vade_farki"].ToString() != vade_farki.ToString() ||
                                    reader["lme"].ToString() != lme ||
                                    reader["iscilik"].ToString() != iscilik ||
                                    reader["iskonto_orani"].ToString() != iskonto_orani ||
                                    reader["kdv_orani"].ToString() != kdv_orani ||
                                    Convert.ToBoolean(reader["tevkifat"]) != tevkifat ||
                                    reader["durum"].ToString() != durum;
                                if (!isDifferent)
                                {
                                    MessageHelper.ShowInfo("Hiçbir değişiklik yapılmadı.");
                                    return;
                                }
                            }
                            else
                            {
                                MessageHelper.ShowError("Teklif bulunamadı.");
                                return;
                            }
                        }

                        if (isDifferent)
                        {
                            string updateQuery = @"
                        UPDATE teklifler SET 
                            firma_id = @FirmaId,
                            yetkili_id = @YetkiliId,
                            teklif_tarih = @TeklifTarih,
                            teslim_sekli = @TeslimSekli,
                            odeme_sekli = @OdemeSekli,
                            odeme_vade = @OdemeVadesi,
                            teklif_sure = @TeklifSuresi,
                            doviz_kuru = @DovizKuru,
                            doviz_birimi = @DovizBirimi,
                            vade = @Vade,
                            vade_farki = @TermRate,
                            lme = @Lme,
                            iscilik = @Workmanship,
                            iskonto_orani = @IskontoOrani,
                            kdv_orani = @KdvOrani,
                            tevkifat = @Tevkifat,
                            tevkifat_orani = @TevkifatOrani,
                            durum = @Durum
                        WHERE teklif_id = @TeklifId";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.Add(new SqlParameter("@FirmaId", SqlDbType.Int) { Value = firma_id });
                                updateCmd.Parameters.Add(new SqlParameter("@YetkiliId", SqlDbType.Int) { Value = yetkili_id });
                                updateCmd.Parameters.Add(new SqlParameter("@TeklifTarih", SqlDbType.Date) { Value = teklif_tarih });
                                updateCmd.Parameters.Add(new SqlParameter("@TeslimSekli", SqlDbType.VarChar) { Value = teslim_sekli });
                                updateCmd.Parameters.Add(new SqlParameter("@OdemeSekli", SqlDbType.VarChar) { Value = odeme_sekli });
                                updateCmd.Parameters.Add(new SqlParameter("@OdemeVadesi", SqlDbType.Int) { Value = odeme_vadesi });
                                updateCmd.Parameters.Add(new SqlParameter("@TeklifSuresi", SqlDbType.Int) { Value = teklif_suresi });
                                updateCmd.Parameters.Add(new SqlParameter("@DovizKuru", SqlDbType.VarChar) { Value = doviz_kuru });
                                updateCmd.Parameters.Add(new SqlParameter("@DovizBirimi", SqlDbType.VarChar) { Value = doviz_birimi.ToString() });
                                updateCmd.Parameters.Add(new SqlParameter("@Vade", SqlDbType.VarChar) { Value = vade });
                                updateCmd.Parameters.Add(new SqlParameter("@TermRate", SqlDbType.Float) { Value = vade_farki });
                                updateCmd.Parameters.AddWithValue("@Lme", lmeStr);
                                updateCmd.Parameters.AddWithValue("@Workmanship", iscilikStr);
                                updateCmd.Parameters.AddWithValue("@IskontoOrani", iskontoStr);
                                updateCmd.Parameters.AddWithValue("@KdvOrani", kdvStr);
                                updateCmd.Parameters.Add(new SqlParameter("@Tevkifat", SqlDbType.Bit) { Value = tevkifat });
                                // "tevkifat" parametresi teklif üzerinde tevkifat uygulandığını belirtir.
                                // Tevkifat oranı sadece bu parametre true ise 70 olarak ayarlanmalıdır.
                                updateCmd.Parameters.AddWithValue("@TevkifatOrani", tevkifat ? 70 : 0);
                                updateCmd.Parameters.Add(new SqlParameter("@Durum", SqlDbType.VarChar) { Value = durum });
                                updateCmd.Parameters.Add(new SqlParameter("@TeklifId", SqlDbType.Int) { Value = teklif_id });


                                int result = updateCmd.ExecuteNonQuery();
                                if (result > 0)
                                {
                                    MessageHelper.ShowSuccess("Teklif başarıyla güncellendi.");
                                }
                                else
                                {
                                    MessageHelper.ShowError("Teklif güncellenirken bir hata oluştu.");
                                }
                            }
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
                                string oranStr = reader["iskonto_orani"]?.ToString();
                                iskontoOrani = !string.IsNullOrEmpty(oranStr) ? Convert.ToDecimal(oranStr) : 0;

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

                                toplamKgStr = reader["ToplamKg"] != DBNull.Value ? reader["ToplamKg"].ToString().Replace(".", ",") : "0";
                                toplamKg = Convert.ToDecimal(reader["ToplamKg"].ToString());
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
                                    toplamAdet = reader["ToplamAdet"] != DBNull.Value ? Convert.ToDecimal(reader["ToplamAdet"]) : 0;
                                    toplamTutar = reader["ToplamTutar"] != DBNull.Value ? Convert.ToDecimal(reader["ToplamTutar"]) : 0;

                                    toplamAdetStr = toplamAdet.ToString().Replace(".", ",");
                                    toplamTutarStr = toplamTutar.ToString().Replace(".", ",");
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
    }
}