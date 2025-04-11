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
    public class ProductManager
    {
        private readonly DataAccess.DbConnection _connection;
        public ProductManager()
        {
            _connection = new DataAccess.DbConnection();
        }
        public DataTable GetProduct()
        {
            string query = "SELECT * FROM urunler";
            using (OleDbCommand cmd = new OleDbCommand(query, _connection.GetConnection()))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        public DataTable Search(string search)
        {
            string query = @"SELECT * FROM urunler WHERE urun LIKE @Product OR kalip_no LIKE @MoldNumber";

            DataTable dt = new DataTable();

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    string likeValue = $"%{search}%";
                    cmd.Parameters.AddWithValue("@Product", likeValue);
                    cmd.Parameters.AddWithValue("@MoldNumber", likeValue);

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt.Rows.Count > 0 ? dt : null;
        }
        public void AddProduct(string mold_number, string product, decimal weight, string category)
        {
            string query = "INSERT INTO urunler(kalip_no, urun, gramaj, kategori) VALUES(@MoldNumber, @Product, @Weight, @Category)";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MoldNumber", mold_number);
                    cmd.Parameters.AddWithValue("@Product", product);
                    cmd.Parameters.Add("@Weight", OleDbType.Double).Value = weight;
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
        public void UpdateProduct(int? product_id, string mold_number, string product, decimal weight, string category)
        {
            if (!product_id.HasValue)
            {
                MessageHelper.ShowError("Geçersiz yetkili kimlik numarası.");
                return;
            }

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();

                // Mevcut veriyi çekiyoruz
                string selectQuery = "SELECT urun_id, kalip_no, urun, gramaj, kategori FROM urunler WHERE urun_id = @AuthId";
                using (OleDbCommand selectCmd = new OleDbCommand(selectQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@AuthId", product_id);

                    using (OleDbDataReader reader = selectCmd.ExecuteReader())
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
                using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@MoldNumber", mold_number);
                    updateCmd.Parameters.AddWithValue("@Product", product);
                    updateCmd.Parameters.Add("@Weight", OleDbType.Double).Value = weight;
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
        public void DeleteProduct(int product_id)
        {
            string query = "DELETE FROM urunler WHERE urun_id = @ProductId";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", product_id);
                    int result = cmd.ExecuteNonQuery();

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
        public List<Dictionary<string, string>> GetProductById(int? product_id)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            string query = "SELECT urun_id, kalip_no, urun, gramaj, kategori FROM urunler WHERE urun_id = @ProductId";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", product_id ?? (object)DBNull.Value);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
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

    }
}
