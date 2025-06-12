using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teklif_Hazırlayıcı.Helpers;
using System.Data.SqlClient;

namespace Teklif_Hazırlayıcı.Business
{
    public class ProductManager
    {
        /*
        *
        * Veritabanı işlemleri için kullanılan bağlantı nesnesi. 
        * Uygulama boyunca yalnızca okunabilir (readonly) olarak tanımlanmıştır.
        *
        */
        private readonly DataAccess.SqlDbConnection _connection;
        public ProductManager()
        {
            /*
             *
             * DbConnection sınıfından yeni bir örnek oluşturularak 
             * _connection alanına atanır. Veritabanı bağlantısını başlatmak için kullanılır.
             *
             */
            _connection = new DataAccess.SqlDbConnection();
        }
        public DataTable GetProduct()
        {
            try
            {
                /*
                     *
                     * "urunler" tablosundaki tüm ürün kayıtlarını getirir.
                     * OleDbCommand ile oluşturulan sorgu OleDbDataAdapter ile çalıştırılır.
                     * Elde edilen veriler bir DataTable nesnesine aktarılır ve döndürülür.
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
        public DataTable Search(string search)
        {
            try
            {
                /*
                     *
                     * "urunler" tablosunda ürün adı (`urun`) veya kalıp numarası (`kalip_no`) arama terimiyle eşleşen kayıtları getirir.
                     * LIKE operatörü ile hem ürün adı hem de kalıp numarası alanlarında filtreleme yapılır.
                     * Uygun kayıtlar bir DataTable nesnesine doldurulur.
                     * Sonuç bulunamazsa null, varsa doldurulmuş DataTable döndürülür.
                     *
                     */
                string query = @"SELECT * FROM urunler WHERE urun LIKE @Product OR kalip_no LIKE @MoldNumber";

                DataTable dt = new DataTable();

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        string likeValue = $"%{search}%";
                        cmd.Parameters.AddWithValue("@Product", likeValue);
                        cmd.Parameters.AddWithValue("@MoldNumber", likeValue);

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
        public void AddProduct(string mold_number, string product, decimal weight, string category)
        {
            try
            {
                /*
                     *
                     * Yeni bir ürün kaydını "urunler" tablosuna ekler.
                     * Parametre olarak verilen kalıp numarası, ürün adı, gramaj (decimal) ve kategori bilgileri sorguya eklenir.
                     * Gramaj alanı `OleDbType.Double` olarak tanımlanarak veri türü uyumluluğu sağlanır.
                     * Ekleme işlemi sonucunda başarı ya da hata mesajı kullanıcıya gösterilir.
                     *
                     */
                string query = "INSERT INTO urunler(kalip_no, urun, gramaj, kategori) VALUES(@MoldNumber, @Product, @Weight, @Category)";
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MoldNumber", mold_number);
                        cmd.Parameters.AddWithValue("@Product", product);
                        cmd.Parameters.Add("@Weight", SqlDbType.Decimal).Value = weight;
                        cmd.Parameters.AddWithValue("@Category", category);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageHelper.ShowSuccess("Ürün başarıyla eklendi");
                        }
                        else
                        {
                            MessageHelper.ShowError("Ürün eklenirken hata oluştu.");
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
        public void UpdateProduct(int? product_id, string mold_number, string product, decimal weight, string category)
        {
            try
            {
                /*
                     *
                     * Belirtilen `product_id` değerine sahip ürün kaydını günceller.
                     * Önce veritabanından mevcut ürün bilgileri çekilir ve yeni parametrelerle karşılaştırılır.
                     * Eğer herhangi bir değişiklik yoksa güncelleme yapılmaz ve kullanıcı bilgilendirilir.
                     * Değişiklik varsa kalıp numarası, ürün adı, gramaj ve kategori alanları güncellenir.
                     * İşlem sonucuna göre başarı veya hata mesajı gösterilir.
                     *
                     */
                if (!product_id.HasValue)
                {
                    MessageHelper.ShowError("Geçersiz yetkili kimlik numarası.");
                    return;
                }

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();

                    // Mevcut veriyi çekiyoruz
                    string selectQuery = "SELECT urun_id, kalip_no, urun, gramaj, kategori FROM urunler WHERE urun_id = @AuthId";
                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@AuthId", product_id);

                        using (SqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int currentProductId = (int)reader["urun_id"];
                                string currentMoldNumber = reader["kalip_no"].ToString();
                                string currentProduct = reader["urun"].ToString();
                                string currentWeight = reader["gramaj"].ToString();
                                string currentCategory = reader["kategori"].ToString();

                                // Farklılık var mı kontrol et
                                if (currentMoldNumber == mold_number &&
                                    currentProduct == product &&
                                    currentWeight == weight.ToString() &&
                                    currentCategory == category)
                                {
                                    MessageHelper.ShowInfo("Hiçbir değişiklik yapılmadı.");
                                    return;
                                }
                            }
                            else
                            {
                                MessageHelper.ShowError("Ürün bulunamadı.");
                                return;
                            }
                        }
                    }

                    // Güncelleme işlemi
                    string updateQuery = "UPDATE urunler SET kalip_no = @MoldNumber, urun = @Product, gramaj = @Weight, kategori = @Category WHERE urun_id = @ProductId";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@MoldNumber", mold_number);
                        updateCmd.Parameters.AddWithValue("@Product", product);
                        updateCmd.Parameters.Add("@Weight", SqlDbType.Decimal).Value = weight;
                        updateCmd.Parameters.AddWithValue("@Category", category);
                        updateCmd.Parameters.AddWithValue("@ProductId", product_id);


                        int result = updateCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageHelper.ShowSuccess("Ürün başarıyla güncellendi.");
                        }
                        else
                        {
                            MessageHelper.ShowError("Ürün güncellenirken hata oluştu.");
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
        public void DeleteProduct(int product_id)
        {
            try
            {
                /*
                     *
                     * Ürünün başka bir teklifte kullanılıp kullanılmadığını kontrol eder.
                     * Eğer varsa hata mesajı verir, yoksa silme işlemini yapar.
                     *
                     */
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();

                    // Önce ürünü başka tabloda kullanan kayıt var mı diye kontrol et
                    string checkQuery = "SELECT COUNT(*) FROM kalemler WHERE urun_id = @ProductId";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@ProductId", product_id);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // Ürün başka bir teklifte kullanılmış
                            MessageHelper.ShowError("Bu ürün herhangi bir teklifte kullanıldığı için silinemez.");
                            return;
                        }
                    }

                    // Silme işlemi
                    string deleteQuery = "DELETE FROM urunler WHERE urun_id = @ProductId";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@ProductId", product_id);
                        int result = deleteCmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageHelper.ShowSuccess("Ürün başarıyla silindi");
                        }
                        else
                        {
                            MessageHelper.ShowError("Ürün silerken hata oluştu.");
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

        public List<Dictionary<string, string>> GetProductById(int? product_id)
        {
            try
            {
                /*
                     *
                     * Belirtilen `product_id` değerine sahip ürün bilgilerini getirir.
                     * "urunler" tablosundan urun_id, kalip_no, urun, gramaj ve kategori alanları çekilir.
                     * Her sonuç bir sözlük (Dictionary) olarak oluşturulup listeye eklenir.
                     * Liste doluysa ürün bilgileri, boşsa boş liste döndürülür.
                     *
                     */
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                string query = "SELECT urun_id, kalip_no, urun, gramaj, kategori FROM urunler WHERE urun_id = @ProductId";
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", product_id ?? (object)DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, string> row = new Dictionary<string, string>();
                                row["urun_id"] = reader["urun_id"].ToString();
                                row["kalip_no"] = reader["kalip_no"].ToString();
                                row["urun"] = reader["urun"].ToString();
                                row["gramaj"] = reader["gramaj"].ToString();
                                row["kategori"] = reader["kategori"].ToString();
                                result.Add(row);
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

    }
}
