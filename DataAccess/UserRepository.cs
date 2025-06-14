using System;
using System.Data;
using System.Data.SqlClient;
using Teklif_Hazırlayıcı.Models;

namespace Teklif_Hazırlayıcı.DataAccess
{
    /// <summary>
    /// Provides database operations for users.
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private readonly SqlDbConnection _connection;

        public UserRepository()
        {
            _connection = new SqlDbConnection();
        }

        public int Insert(User user)
        {
            string query = "INSERT INTO kullanicilar (isim, soyisim, kullanici_adi, eposta) " +
                           "VALUES (@Name, @Surname, @Username, @Email); SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", user.Isim);
                    cmd.Parameters.AddWithValue("@Surname", user.Soyisim);
                    cmd.Parameters.AddWithValue("@Username", user.KullaniciAdi);
                    cmd.Parameters.AddWithValue("@Email", user.Eposta);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(User user)
        {
            string updateQuery = @"UPDATE kullanicilar SET
                                    isim = @Name,
                                    soyisim = @Surname,
                                    kullanici_adi = @Username,
                                    eposta = @Email
                                  WHERE kullanici_id = @UserId";

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", user.KullaniciId);
                    cmd.Parameters.AddWithValue("@Name", user.Isim);
                    cmd.Parameters.AddWithValue("@Surname", user.Soyisim);
                    cmd.Parameters.AddWithValue("@Username", user.KullaniciAdi);
                    cmd.Parameters.AddWithValue("@Email", user.Eposta);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
