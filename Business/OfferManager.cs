using System;
using System.Data;
using System.Data.OleDb;
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
            SELECT t.teklif_id, t.yetkili_id, y.isim, y.soyisim, y.hitap, f.adi, t.teklif_tarih, t.durum
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
        t.iscilik,
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
        public int AddOffer(int firma_id, int yetkili_id, DateTime teklif_tarih, string teslim_sekli, string odeme_sekli, int odeme_vadesi, int teklif_suresi, string doviz_kuru, char doviz_birimi, string vade, string lme, string iscilik, string iskonto_orani, string kdv_orani, bool tevkifat, string tevkifat_orani, string durum)
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

            // Tevkifat
            decimal tevkifatDecimal = 0;
            if (!decimal.TryParse(tevkifat_orani, NumberStyles.Any, CultureInfo.InvariantCulture, out tevkifatDecimal))
                tevkifatDecimal = 0;
            string tevkifatStr = tevkifatDecimal.ToString("0.##", new CultureInfo("tr-TR"));

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

            string query = "INSERT INTO teklifler (firma_id, yetkili_id, teklif_tarih, teslim_sekli, odeme_sekli, odeme_vadesi, teklif_suresi, doviz_kuru, doviz_birimi, vade, lme, iscilik, iskonto_orani, kdv_orani, tevkifat, tevkifat_orani, durum) " +
                           "VALUES (@CompanyId, @AuthorizedPersonId, @OfferDate, @DeliveryMethod, @PaymentMethod, @PaymentDue, @OfferValidity, @ExchangeRate, @CurrencyUnit, @Term, @Lme, @Workmanship, @DiscountRate, @VatRate, @Withholding, @WithholdingRate, @Status);";

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
                    cmd.Parameters.AddWithValue("@Lme", lmeStr);
                    cmd.Parameters.AddWithValue("@Workmanship", iscilikStr);
                    cmd.Parameters.AddWithValue("@IskontoOrani", iskontoStr);
                    cmd.Parameters.AddWithValue("@KdvOrani", kdvStr);
                    cmd.Parameters.Add("@Withholding", OleDbType.Boolean).Value = tevkifat;
                    cmd.Parameters.AddWithValue("@TevkifatOrani", tevkifatStr);
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

        #region Teklif Güncelleme
        public void UpdateOffer(int? teklif_id, int firma_id, int yetkili_id, DateTime teklif_tarih, string teslim_sekli, string odeme_sekli, int odeme_vadesi, int teklif_suresi, string doviz_kuru, char doviz_birimi, string vade, string lme, string iscilik, string iskonto_orani, string kdv_orani, bool tevkifat, string tevkifat_orani, string durum)
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

            // Tevkifat
            decimal tevkifatDecimal = 0;
            if (!decimal.TryParse(tevkifat_orani, NumberStyles.Any, CultureInfo.InvariantCulture, out tevkifatDecimal))
                tevkifatDecimal = 0;
            string tevkifatStr = tevkifatDecimal.ToString("0.##", new CultureInfo("tr-TR"));

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





            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();


                // Mevcut teklifi çek
                string selectQuery = "SELECT * FROM teklifler WHERE teklif_id = @TeklifId";
                using (OleDbCommand selectCmd = new OleDbCommand(selectQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@TeklifId", teklif_id);

                    using (OleDbDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool isDifferent =
                                (int)reader["firma_id"] != firma_id ||
                                (int)reader["yetkili_id"] != yetkili_id ||
                                Convert.ToDateTime(reader["teklif_tarih"]) != teklif_tarih ||
                                reader["teslim_sekli"].ToString() != teslim_sekli ||
                                reader["odeme_sekli"].ToString() != odeme_sekli ||
                                Convert.ToInt32(reader["odeme_vadesi"]) != odeme_vadesi ||
                                Convert.ToInt32(reader["teklif_suresi"]) != teklif_suresi ||
                                reader["doviz_kuru"].ToString() != doviz_kuru ||
                                Convert.ToChar(reader["doviz_birimi"]) != doviz_birimi ||
                                reader["vade"].ToString() != vade ||
                                reader["lme"].ToString() != lme ||
                                reader["iscilik"].ToString() != iscilik ||
                                reader["iskonto_orani"].ToString() != iskonto_orani ||
                                reader["kdv_orani"].ToString() != kdv_orani ||
                                Convert.ToBoolean(reader["tevkifat"]) != tevkifat ||
                                reader["tevkifat_orani"].ToString() != tevkifat_orani ||
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

                        string updateQuery = @"
                        UPDATE teklifler SET 
                            firma_id = @FirmaId,
                            yetkili_id = @YetkiliId,
                            teklif_tarih = @TeklifTarih,
                            teslim_sekli = @TeslimSekli,
                            odeme_sekli = @OdemeSekli,
                            odeme_vadesi = @OdemeVadesi,
                            teklif_suresi = @TeklifSuresi,
                            doviz_kuru = @DovizKuru,
                            doviz_birimi = @DovizBirimi,
                            vade = @Vade,
                            lme = @Lme,
                            iscilik = @Workmanship,
                            iskonto_orani = @IskontoOrani,
                            kdv_orani = @KdvOrani,
                            tevkifat = @Tevkifat,
                            tevkifat_orani = @TevkifatOrani,
                            durum = @Durum
                        WHERE teklif_id = @TeklifId";

                        using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.Add(new OleDbParameter("@FirmaId", OleDbType.Integer) { Value = firma_id });
                            updateCmd.Parameters.Add(new OleDbParameter("@YetkiliId", OleDbType.Integer) { Value = yetkili_id });
                            updateCmd.Parameters.Add(new OleDbParameter("@TeklifTarih", OleDbType.Date) { Value = teklif_tarih });
                            updateCmd.Parameters.Add(new OleDbParameter("@TeslimSekli", OleDbType.VarChar) { Value = teslim_sekli });
                            updateCmd.Parameters.Add(new OleDbParameter("@OdemeSekli", OleDbType.VarChar) { Value = odeme_sekli });
                            updateCmd.Parameters.Add(new OleDbParameter("@OdemeVadesi", OleDbType.Integer) { Value = odeme_vadesi });
                            updateCmd.Parameters.Add(new OleDbParameter("@TeklifSuresi", OleDbType.Integer) { Value = teklif_suresi });
                            updateCmd.Parameters.Add(new OleDbParameter("@DovizKuru", OleDbType.VarChar) { Value = doviz_kuru });
                            updateCmd.Parameters.Add(new OleDbParameter("@DovizBirimi", OleDbType.VarChar) { Value = doviz_birimi.ToString() });
                            updateCmd.Parameters.Add(new OleDbParameter("@Vade", OleDbType.VarChar) { Value = vade });
                            updateCmd.Parameters.AddWithValue("@Lme", lmeStr);
                            updateCmd.Parameters.AddWithValue("@Workmanship", iscilikStr);
                            updateCmd.Parameters.AddWithValue("@IskontoOrani", iskontoStr);
                            updateCmd.Parameters.AddWithValue("@KdvOrani", kdvStr);
                            updateCmd.Parameters.Add(new OleDbParameter("@Tevkifat", OleDbType.Boolean) { Value = tevkifat });
                            updateCmd.Parameters.AddWithValue("@TevkifatOrani", tevkifatStr);
                            updateCmd.Parameters.Add(new OleDbParameter("@Durum", OleDbType.VarChar) { Value = durum });
                            updateCmd.Parameters.Add(new OleDbParameter("@TeklifId", OleDbType.Integer) { Value = teklif_id });


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

        public bool UpdateOfferById(int? teklifId)
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

            string offerQuery = "SELECT iskonto_orani FROM teklifler WHERE teklif_id = @teklifId";

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(offerQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@teklifId", teklifId);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string oranStr = result.ToString();
                        iskontoOrani = Convert.ToDecimal(oranStr);
                    }
                }

                decimal toplamAdet = 0, toplamKg = 0, toplamTutar = 0;
                string toplamAdetStr = "0", toplamKgStr = "0", toplamTutarStr = "0";

                // 2. Toplamları hesapla
                string selectQuery = @"
            SELECT 
                IIF(ISNULL(SUM(k.adet)), 0, SUM(k.adet)) AS ToplamAdet,
                IIF(ISNULL(SUM(k.kg)), 0, SUM(k.kg)) AS ToplamKg,
                IIF(ISNULL(SUM(k.toplam_tutar)), 0, SUM(k.toplam_tutar)) AS ToplamTutar
            FROM kalemler k
            INNER JOIN urunler u ON k.urun_id = u.urun_id
            WHERE k.teklif_id = @teklifId AND (u.kategori IS NULL OR u.kategori <> 'aksesuar')";
                using (OleDbCommand cmd = new OleDbCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@teklifId", teklifId);
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            toplamAdetStr = reader["ToplamAdet"] != DBNull.Value ? reader["ToplamAdet"].ToString().Replace(".",",") : "0";
                            toplamAdet = Convert.ToDecimal(reader["ToplamAdet"].ToString());

                            toplamKgStr = reader["ToplamKg"] != DBNull.Value ? reader["ToplamKg"].ToString().Replace(".",",") : "0";
                            toplamKg = Convert.ToDecimal(reader["ToplamKg"].ToString());

                            toplamTutarStr = reader["ToplamTutar"] != DBNull.Value ? reader["ToplamTutar"].ToString().Replace(".", ",") : "0";
                            toplamTutar = Convert.ToDecimal(reader["ToplamTutar"].ToString());
                        }
                    }
                }

                // 3. Finansal hesaplamalar
                decimal iskontoTutar = ((toplamTutar * (iskontoOrani/100)));
                decimal iskontoSonrasi = toplamTutar - iskontoTutar;
                decimal kdv = iskontoSonrasi * 0.20m;
                decimal aluminyumTutar = GetToplamAluminyumTutari(teklifId.Value);
                decimal tevkifat = ((((aluminyumTutar * 20) / 100) * 70) / 100);
                decimal genelToplam = iskontoSonrasi + kdv;
                decimal odenecek = genelToplam - tevkifat;

                string iskontoTutarStr = iskontoTutar.ToString().Replace(".", ",");
                string kdvStr = kdv.ToString().Replace(".", ",");
                string tevkifatStr = tevkifat.ToString().Replace(".", ",");
                string genelToplamStr = genelToplam.ToString().Replace(".", ",");
                string odenecekStr = odenecek.ToString().Replace(".", ",");



                // 4. Güncelleme - türler decimal kalıyor
                string updateQuery = @"
                UPDATE teklifler SET 
                    toplam_adet = @toplamAdet,
                    toplam_kg = @toplamKg,
                    mal_hizmet_tutari = @toplamTutar,
                    iskonto_tutari = @iskontoTutar,
                    kdv_tutari = @kdv,
                    tevkifat_tutari = @tevkifat,
                    genel_toplam = @genelToplam,
                    odenecek_tutar = @odenecek
                WHERE teklif_id = @teklifId";

                using (OleDbCommand cmd = new OleDbCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@toplamAdet", toplamAdetStr);
                    cmd.Parameters.AddWithValue("@toplamKg", toplamKgStr);
                    cmd.Parameters.AddWithValue("@toplamTutar", toplamTutarStr);
                    cmd.Parameters.AddWithValue("@iskontoTutar", iskontoTutarStr);
                    cmd.Parameters.AddWithValue("@kdv", kdvStr);
                    cmd.Parameters.AddWithValue("@tevkifat", tevkifatStr);
                    cmd.Parameters.AddWithValue("@genelToplam", genelToplamStr);
                    cmd.Parameters.AddWithValue("@odenecek", odenecekStr);
                    cmd.Parameters.AddWithValue("@teklifId", teklifId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        #endregion

        #region Teklif Silme
        public bool DeleteOffer(int teklif_id)
        {
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();

                // Önce kalemleri sil
                var deleteItemsCmd = new OleDbCommand("DELETE FROM kalemler WHERE teklif_id = @TeklifId", conn);
                deleteItemsCmd.Parameters.AddWithValue("@TeklifId", teklif_id);
                deleteItemsCmd.ExecuteNonQuery();

                // Sonra teklifi sil
                var deleteOfferCmd = new OleDbCommand("DELETE FROM teklifler WHERE teklif_id = @TeklifId", conn);
                deleteOfferCmd.Parameters.AddWithValue("@TeklifId", teklif_id);
                int result = deleteOfferCmd.ExecuteNonQuery();

                return result > 0;
            }
        }

        #endregion

        public DataTable GetOfferDetailById(int? teklif_id)
        {
            string query = @"
        SELECT f.adi, y.isim, t.teklif_tarih, t.toplam_adet, t.toplam_kg, 
               t.mal_hizmet_tutari, t.iskonto_orani, t.iskonto_tutari, t.kdv_tutari, 
               t.tevkifat_tutari, t.genel_toplam, t.odenecek_tutar, t.doviz_birimi
        FROM ((teklifler t
        LEFT JOIN firmalar f ON f.firma_id = t.firma_id)
        LEFT JOIN yetkililer y ON y.yetkili_id = t.yetkili_id)
        WHERE t.teklif_id = ?";

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", teklif_id);

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt.Rows.Count > 0 ? dt : null;
                    }
                }
            }
        }
        public decimal GetToplamAluminyumTutari(int? teklif_id)
        {
            decimal toplamAluminyumTutari = 0;

            string query = @"
        SELECT u.kategori, k.toplam_tutar
        FROM kalemler k
        INNER JOIN urunler u ON k.urun_id = u.urun_id
        WHERE k.teklif_id = ?";

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    command.Parameters.AddWithValue("?", teklif_id);

                    using (OleDbDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string kategori = rdr["kategori"].ToString().Trim().ToLower();
                            if (kategori == "alüminyum")
                            {
                                decimal.TryParse(rdr["toplam_tutar"].ToString(), out decimal tutar);
                                toplamAluminyumTutari += tutar;
                            }
                        }
                    }
                }
            }

            return toplamAluminyumTutari;
        }

        public DataTable GetTeklifKalemleri(int? teklif_id)
        {
            string query = @"
        SELECT u.kalip_no, u.urun, k.yuzey, k.yuzey_kodu, k.boy, k.adet, k.kg, k.birim_fiyat, k.toplam_tutar
        FROM kalemler k
        INNER JOIN urunler u ON k.urun_id = u.urun_id
        WHERE k.teklif_id = ?";

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    command.Parameters.AddWithValue("?", teklif_id);

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt.Rows.Count > 0 ? dt : null;
                    }
                }
            }
        }



        /*
         * 
         * PDF Çıktısı
         * 
         */
        #region Teklif Detay Getirme
        public DataTable GetOfferDetailById(int teklif_id)
        {
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                var cmd = new OleDbCommand(@"
                SELECT 
                    f.adi, 
                    y.isim, 
                    t.teklif_tarih, 
                    t.toplam_adet, 
                    t.toplam_kg, 
                    t.mal_hizmet_tutari, 
                    t.iskonto_orani, 
                    t.iskonto_tutari, 
                    t.kdv_tutari, 
                    t.tevkifat_tutari, 
                    t.genel_toplam, 
                    t.odenecek_tutar, 
                    t.doviz_birimi,
                    t.teslim_sekli,
                    t.odeme_sekli,
                    t.odeme_vadesi,
                    t.teklif_suresi,
                    t.doviz_kuru,
                    t.vade
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
        #endregion
        #region Alüminyum Tutarı Getirme
        public decimal GetToplamAluminyumTutari(int teklif_id)
        {
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                var cmd = new OleDbCommand(@"
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
        #endregion
        #region Teklif Kalemleri Getirme
        public DataTable GetTeklifKalemleri(int teklif_id)
        {
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                var cmd = new OleDbCommand(@"
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
        #endregion




    }
}