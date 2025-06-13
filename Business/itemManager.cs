using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teklif_Hazırlayıcı.Helpers;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Teklif_Hazırlayıcı.Business
{
    public class itemManager
    {
        /*
        *
        * Veritabanı işlemleri için kullanılan bağlantı nesnesi. 
        * Uygulama boyunca yalnızca okunabilir (readonly) olarak tanımlanmıştır.
        *
        */
        private readonly DataAccess.SqlDbConnection _connection;
        public itemManager()
        {
            /*
             *
             * DbConnection sınıfından yeni bir örnek oluşturularak 
             * _connection alanına atanır. Veritabanı bağlantısını başlatmak için kullanılır.
             *
             */
            _connection = new DataAccess.SqlDbConnection();

        }

        #region CRUD İşlemleri

        public void AddProduct(int? teklif_id, int urun_id, string yuzey, string yuzey_kodu, int adet, int boy, decimal kg, decimal birim_fiyat, decimal toplam_tutar)
        {
            try
            {
                /*
                     *
                     * Belirtilen teklif ve ürün bilgileri ile "kalemler" tablosuna yeni bir ürün kalemi ekler.
                     * Teklif ID boş ise işlem yapılmaz ve kullanıcıya hata mesajı gösterilir.
                     * Yüzey ve yüzey kodu boşsa veritabanına NULL olarak gönderilir.
                     * Kg, birim fiyat ve toplam tutar parametreleri manuel olarak OleDbType ile tanımlanarak eklenir.
                     * İşlem başarıyla gerçekleşirse bilgi mesajı, aksi takdirde hata mesajı gösterilir.
                     *
                     */
                if (teklif_id == null)
                {
                    MessageHelper.ShowError("Teklif ID boş olamaz.");
                    return;
                }

                string query = @"INSERT INTO kalemler 
                     (teklif_id, urun_id, yuzey, yuzey_kodu, adet, boy, kg, birim_fiyat, toplam_tutar) 
                     VALUES 
                     (@TeklifId, @UrunId, @Yuzey, @YuzeyKodu, @Adet, @Boy, @Kg, @BirimFiyat, @ToplamTutar)";

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        var kgParam = new SqlParameter("@kg", SqlDbType.Float);
                        kgParam.Value = kg;

                        var fiyatParam = new SqlParameter("@birim_fiyat", SqlDbType.Float);
                        fiyatParam.Value = birim_fiyat;

                        var tutarParam = new SqlParameter("@toplam_tutar", SqlDbType.Float);
                        tutarParam.Value = toplam_tutar;


                        cmd.Parameters.Add(new SqlParameter("@TeklifId", SqlDbType.Int) { Value = teklif_id });
                        cmd.Parameters.Add(new SqlParameter("@UrunId", SqlDbType.Int) { Value = urun_id });
                        cmd.Parameters.Add(new SqlParameter("@Yuzey", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(yuzey) ? DBNull.Value : (object)yuzey });
                        cmd.Parameters.Add(new SqlParameter("@YuzeyKodu", SqlDbType.VarChar) { Value = string.IsNullOrEmpty(yuzey_kodu) ? DBNull.Value : (object)yuzey_kodu });
                        cmd.Parameters.Add(new SqlParameter("@Adet", SqlDbType.Int) { Value = adet });
                        cmd.Parameters.Add(new SqlParameter("@Boy", SqlDbType.Int) { Value = boy });
                        cmd.Parameters.Add(new SqlParameter("@Kg", SqlDbType.Float) { Value = kg });
                        cmd.Parameters.Add(new SqlParameter("@BirimFiyat", SqlDbType.Float) { Value = birim_fiyat });
                        cmd.Parameters.Add(new SqlParameter("@ToplamTutar", SqlDbType.Float) { Value = toplam_tutar });


                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageHelper.ShowInfo("Ürün kalemi başarıyla eklendi.");
                        }
                        else
                        {
                            MessageHelper.ShowError("Kalem eklenirken bir hata oluştu.");
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

        public bool DeleteProductByKalemId(int? kalem_id)
        {
            try
            {
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();

                    var deleteItemsCmd = new SqlCommand("DELETE FROM kalemler WHERE kalem_id = @KalemId", conn);
                    deleteItemsCmd.Parameters.AddWithValue("@KalemId", kalem_id);

                    int result = deleteItemsCmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public bool UpdateProductByKalemId(int kalem_id, string yuzey, string yuzey_kodu, int adet, int boy, decimal kg, decimal birim_fiyat, decimal toplam_tutar)
        {
            try
            {
                string query = @"
UPDATE kalemler SET 
    yuzey = @Yuzey,
    yuzey_kodu = @YuzeyKodu,
    adet = @Adet,
    boy = @Boy,
    kg = @Kg,
    birim_fiyat = @BirimFiyat,
    toplam_tutar = @ToplamTutar
WHERE kalem_id = @KalemId";

                try
                {
                    using (SqlConnection conn = _connection.GetConnection())
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Yuzey", string.IsNullOrEmpty(yuzey) ? DBNull.Value : (object)yuzey);
                            cmd.Parameters.AddWithValue("@YuzeyKodu", string.IsNullOrEmpty(yuzey_kodu) ? DBNull.Value : (object)yuzey_kodu);
                            cmd.Parameters.AddWithValue("@Adet", adet);
                            cmd.Parameters.AddWithValue("@Boy", boy);
                            cmd.Parameters.AddWithValue("@Kg", kg);
                            cmd.Parameters.AddWithValue("@BirimFiyat", birim_fiyat);
                            cmd.Parameters.AddWithValue("@ToplamTutar", toplam_tutar);
                            cmd.Parameters.AddWithValue("@KalemId", kalem_id);

                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log dosyasına yaz
                    File.AppendAllText("log.txt", $"[{DateTime.Now}] UpdateProductByKalemId ERROR: {ex.Message}{Environment.NewLine}");

                    // Kullanıcıya bilgi vermek için (isteğe bağlı): MessageBox.Show kullanılabilir
                    MessageBox.Show("Ürün güncellenirken bir hata oluştu. Detaylar log dosyasına kaydedildi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }


        #endregion

        public string GetCategory(int? urun_id)
        {
            try
            {
                /*
                     *
                     * Belirtilen `urun_id` değerine sahip ürünün kategori bilgisini getirir.
                     * Eğer `urun_id` null ise veya kategori bilgisi bulunamazsa null döndürülür.
                     * Kategori bilgisi, "urunler" tablosundan çekilir ve string olarak döndürülür.
                     *
                     */
                if (urun_id == null)
                    return null;

                string query = "SELECT kategori FROM urunler WHERE urun_id = @ProductId";
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", urun_id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader["kategori"]?.ToString();
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public DataTable GetProduct()
        {
            try
            {
                /*
                     *
                     * Veritabanındaki "urunler" tablosundaki tüm ürün kayıtlarını getirir.
                     * OleDbCommand ile hazırlanan sorgu, OleDbDataAdapter kullanılarak bir DataTable nesnesine aktarılır.
                     * Doldurulan DataTable döndürülür.
                     *
                     */
                string query = "SELECT * FROM urunler";
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

        public decimal GetLMEFromTeklif(int? teklif_id)
        {
            try
            {
                /*
                     *
                     * Belirtilen `teklif_id` değerine ait teklif kaydından LME (London Metal Exchange) değerini getirir.
                     * LME değeri `teklifler` tablosundan alınır ve decimal türünde döndürülür.
                     * Eğer değer bulunamazsa veya dönüşüm başarısız olursa varsayılan olarak 0 döndürülür.
                     *
                     */
                string query = "SELECT lme FROM teklifler WHERE teklif_id = @TeklifId";

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TeklifId", teklif_id);
                        object result = cmd.ExecuteScalar();

                        if (result != null && decimal.TryParse(result.ToString(), out decimal lme))
                            return lme;

                        return 0m;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public decimal Getİscilik(int? teklif_id)
        {
            try
            {
                string query = "SELECT iscilik FROM teklifler WHERE teklif_id = @TeklifId";

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TeklifId", teklif_id);
                        object result = cmd.ExecuteScalar();

                        if (result != null && decimal.TryParse(result.ToString(), out decimal lme))
                            return lme;

                        return 0m;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public decimal GetGramaj(int urun_id)
        {
            try
            {
                /*
                     *
                     * Belirtilen `urun_id` değerine sahip ürünün gramaj bilgisini getirir.
                     * "urunler" tablosundan alınan gramaj değeri decimal türüne çevrilerek döndürülür.
                     * Eğer değer bulunamazsa varsayılan olarak 0 döndürülür.
                     *
                     */
                string query = "SELECT gramaj FROM urunler WHERE urun_id = @ProductId";
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", urun_id);
                        var result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToDecimal(result) : 0m;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public DataTable GetItemsByTeklifId(int? teklif_id)
        {
            try
            {
                string query = @"SELECT k.kalem_id, k.teklif_id, k.urun_id, u.urun, u.kalip_no, u.gramaj, u.kategori, 
                            k.yuzey, k.yuzey_kodu, k.adet, k.boy, k.kg, k.birim_fiyat, k.toplam_tutar
                     FROM kalemler k
                     INNER JOIN urunler u ON k.urun_id = u.urun_id
                     WHERE k.teklif_id = @TeklifId";

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TeklifId", teklif_id);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public DataTable GetProductById(int? kalem_id)
        {
            try
            {
                string query =
                    "SELECT u.urun_id, u.kalip_no, u.urun, u.gramaj, u.kategori, k.adet, k.boy, k.yuzey, k.yuzey_kodu " +
                    "FROM urunler u " +
                    "INNER JOIN kalemler k ON u.urun_id = k.urun_id " +
                    "WHERE k.kalem_id = @KalemId";


                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@KalemId", kalem_id);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        internal decimal GetVadeliFiyat(int? value)
        {
            try
            {
                /*
                     *
                     * Belirtilen `value` değerine göre vadeli fiyatı getirir.
                     * "vadeli_fiyat" tablosundan alınan fiyat değeri decimal türüne çevrilerek döndürülür.
                     * Eğer değer bulunamazsa varsayılan olarak 0 döndürülür.
                     *
                     */
                string query = "SELECT vade_farki FROM teklifler WHERE teklif_id = @Id";
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", value);
                        var result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToDecimal(result) : 0m;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        internal int GetVadeAy(int? value)
        {
            try
            {
                /*
                     *
                     * Belirtilen `value` değerine göre vade ayını getirir.
                     * "vadeli_fiyat" tablosundan alınan vade ayı değeri int türüne çevrilerek döndürülür.
                     * Eğer değer bulunamazsa varsayılan olarak 0 döndürülür.
                     *
                     */
                string query = "SELECT odeme_vade FROM teklifler WHERE teklif_id = @Id";
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", value);
                        var result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) / 30 : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public void UpdateItemPricesByOffer(int? teklifId)
        {
            try
            {
                decimal lme = GetLMEFromTeklif(teklifId);
                decimal iscilik = Getİscilik(teklifId);
                decimal vadeFarki = GetVadeliFiyat(teklifId);
                int ay = GetVadeAy(teklifId);

                decimal basePrice = (lme / 1000m) + (iscilik / 1000m);
                decimal factor = ay > 0 ? 1 + (vadeFarki / 100m) * ay : 1m;

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();

                    string selectQuery = @"SELECT k.kalem_id, k.kg, u.kategori
                        FROM kalemler k
                        INNER JOIN urunler u ON k.urun_id = u.urun_id
                        WHERE k.teklif_id = @TeklifId";

                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@TeklifId", teklifId);

                        List<(int KalemId, decimal Kg)> kalemler = new List<(int, decimal)>();

                        using (SqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string kategori = reader["kategori"].ToString().Trim().ToLower();
                                if (kategori == "alüminyum" || kategori == "aluminyum")
                                {
                                    int kalemId = Convert.ToInt32(reader["kalem_id"]);
                                    decimal kg = reader["kg"] != DBNull.Value ? Convert.ToDecimal(reader["kg"]) : 0m;
                                    kalemler.Add((kalemId, kg));
                                }
                            }
                        }

                        foreach (var k in kalemler)
                        {
                            decimal yeniFiyat = Math.Round(basePrice * factor, 2);
                            decimal yeniTutar = Math.Round(yeniFiyat * k.Kg, 2);

                            string updateQuery = @"UPDATE kalemler SET birim_fiyat = @BirimFiyat, toplam_tutar = @ToplamTutar WHERE kalem_id = @KalemId";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@BirimFiyat", yeniFiyat);
                                updateCmd.Parameters.AddWithValue("@ToplamTutar", yeniTutar);
                                updateCmd.Parameters.AddWithValue("@KalemId", k.KalemId);
                                updateCmd.ExecuteNonQuery();
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
    }
}
