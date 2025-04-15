using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teklif_Hazırlayıcı.Helpers;

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
        private readonly DataAccess.DbConnection _connection;
        public itemManager()
        {
            /*
             *
             * DbConnection sınıfından yeni bir örnek oluşturularak 
             * _connection alanına atanır. Veritabanı bağlantısını başlatmak için kullanılır.
             *
             */
            _connection = new DataAccess.DbConnection();

        }

        public string GetCategory(int? urun_id)
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
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", urun_id);
                    using (OleDbDataReader reader = cmd.ExecuteReader())
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

        public DataTable GetProduct()
        {
            /*
             *
             * Veritabanındaki "urunler" tablosundaki tüm ürün kayıtlarını getirir.
             * OleDbCommand ile hazırlanan sorgu, OleDbDataAdapter kullanılarak bir DataTable nesnesine aktarılır.
             * Doldurulan DataTable döndürülür.
             *
             */
            string query = "SELECT * FROM urunler";
            using (OleDbCommand cmd = new OleDbCommand(query, _connection.GetConnection()))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void AddProduct(int? teklif_id, int urun_id, string yuzey, string yuzey_kodu, int adet, int boy, decimal kg, decimal birim_fiyat, decimal toplam_tutar)
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

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    var kgParam = new OleDbParameter("@kg", OleDbType.Double);
                    kgParam.Value = kg;

                    var fiyatParam = new OleDbParameter("@birim_fiyat", OleDbType.Double);
                    fiyatParam.Value = birim_fiyat;

                    var tutarParam = new OleDbParameter("@toplam_tutar", OleDbType.Double);
                    tutarParam.Value = toplam_tutar;


                    cmd.Parameters.AddWithValue("@TeklifId", teklif_id);
                    cmd.Parameters.AddWithValue("@UrunId", urun_id);
                    cmd.Parameters.AddWithValue("@Yuzey", string.IsNullOrEmpty(yuzey) ? DBNull.Value : (object)yuzey);
                    cmd.Parameters.AddWithValue("@YuzeyKodu", string.IsNullOrEmpty(yuzey_kodu) ? DBNull.Value : (object)yuzey_kodu);
                    cmd.Parameters.AddWithValue("@Adet", adet);
                    cmd.Parameters.AddWithValue("@Boy", boy);

                    // ❗ Hedef nokta: Bu üçü manuel ekleniyor
                    cmd.Parameters.Add(kgParam);
                    cmd.Parameters.Add(fiyatParam);
                    cmd.Parameters.Add(tutarParam);

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

        public decimal GetLMEFromTeklif(int teklif_id)
        {
            /*
             *
             * Belirtilen `teklif_id` değerine ait teklif kaydından LME (London Metal Exchange) değerini getirir.
             * LME değeri `teklifler` tablosundan alınır ve decimal türünde döndürülür.
             * Eğer değer bulunamazsa veya dönüşüm başarısız olursa varsayılan olarak 0 döndürülür.
             *
             */
            string query = "SELECT lme FROM teklifler WHERE teklif_id = @TeklifId";

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TeklifId", teklif_id);
                    object result = cmd.ExecuteScalar();

                    if (result != null && decimal.TryParse(result.ToString(), out decimal lme))
                        return lme;

                    return 0m;
                }
            }
        }

        public decimal GetGramaj(int urun_id)
        {
            /*
             *
             * Belirtilen `urun_id` değerine sahip ürünün gramaj bilgisini getirir.
             * "urunler" tablosundan alınan gramaj değeri decimal türüne çevrilerek döndürülür.
             * Eğer değer bulunamazsa varsayılan olarak 0 döndürülür.
             *
             */
            string query = "SELECT gramaj FROM urunler WHERE urun_id = @ProductId";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", urun_id);
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) : 0m;
                }
            }
        }

    }
}
