using System;
using System.Data;
using System.Data.SqlClient;
using Teklif_Hazırlayıcı.Models;

namespace Teklif_Hazırlayıcı.DataAccess
{
    /// <summary>
    /// Provides database operations for products.
    /// </summary>
    public class ProductRepository : IRepository<Product>
    {
        private readonly SqlDbConnection _connection;

        public ProductRepository()
        {
            _connection = new SqlDbConnection();
        }

        public int Insert(Product product)
        {
            string query = "INSERT INTO urunler (kalip_no, urun, gramaj, kategori) " +
                           "VALUES (@MoldNumber, @Product, @Weight, @Category); SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MoldNumber", product.KalipNo);
                    cmd.Parameters.AddWithValue("@Product", product.Urun);
                    cmd.Parameters.Add("@Weight", SqlDbType.Decimal).Value = product.Gramaj;
                    cmd.Parameters.AddWithValue("@Category", product.Kategori);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(Product product)
        {
            string updateQuery = @"UPDATE urunler SET
                                    kalip_no = @MoldNumber,
                                    urun = @Product,
                                    gramaj = @Weight,
                                    kategori = @Category
                                  WHERE urun_id = @ProductId";

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", product.UrunId);
                    cmd.Parameters.AddWithValue("@MoldNumber", product.KalipNo);
                    cmd.Parameters.AddWithValue("@Product", product.Urun);
                    cmd.Parameters.Add("@Weight", SqlDbType.Decimal).Value = product.Gramaj;
                    cmd.Parameters.AddWithValue("@Category", product.Kategori);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
