using System;
using System.Data;
using System.Data.SqlClient;
using Teklif_Hazırlayıcı.Models;

namespace Teklif_Hazırlayıcı.DataAccess
{
    /// <summary>
    /// Provides database operations for authorized people.
    /// </summary>
    public class AuthRepository : IRepository<Auth>
    {
        private readonly SqlDbConnection _connection;

        public AuthRepository()
        {
            _connection = new SqlDbConnection();
        }

        public int InsertAuth(int? firmaId, string isim, string soyisim, string hitap,
                              string adres, string telefon, string eposta)
        {
            string query = "INSERT INTO yetkililer(firma_id, isim, soyisim, hitap, adres, telefon, eposta) " +
                           "VALUES(@CompanyId, @Name, @Surname, @Honorific, @Address, @Phone, @Email); SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", (object)firmaId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", isim);
                    cmd.Parameters.AddWithValue("@Surname", soyisim);
                    cmd.Parameters.AddWithValue("@Honorific", hitap);
                    cmd.Parameters.AddWithValue("@Address", adres);
                    cmd.Parameters.AddWithValue("@Phone", telefon);
                    cmd.Parameters.AddWithValue("@Email", eposta);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void UpdateAuth(int yetkiliId, int? firmaId, string isim, string soyisim, string hitap,
                               string adres, string telefon, string eposta)
        {
            string query = @"UPDATE yetkililer SET
                                firma_id = @CompanyId,
                                isim = @Name,
                                soyisim = @Surname,
                                hitap = @Honorific,
                                adres = @Address,
                                telefon = @Phone,
                                eposta = @Email
                             WHERE yetkili_id = @AuthId";

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", (object)firmaId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", isim);
                    cmd.Parameters.AddWithValue("@Surname", soyisim);
                    cmd.Parameters.AddWithValue("@Honorific", hitap);
                    cmd.Parameters.AddWithValue("@Address", adres);
                    cmd.Parameters.AddWithValue("@Phone", telefon);
                    cmd.Parameters.AddWithValue("@Email", eposta);
                    cmd.Parameters.AddWithValue("@AuthId", yetkiliId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAuth(int yetkiliId)
        {
            string query = "DELETE FROM yetkililer WHERE yetkili_id = @AuthId";
            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AuthId", yetkiliId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int Insert(Auth entity)
        {
            return InsertAuth(entity.FirmaId, entity.Isim, entity.Soyisim, entity.Hitap,
                              entity.Adres, entity.Telefon, entity.Eposta);
        }

        public void Update(Auth entity)
        {
            UpdateAuth(entity.YetkiliId, entity.FirmaId, entity.Isim, entity.Soyisim, entity.Hitap,
                       entity.Adres, entity.Telefon, entity.Eposta);
        }
    }
}
