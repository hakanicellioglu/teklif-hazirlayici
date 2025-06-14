using System;
using System.Data;
using System.Data.SqlClient;
using Teklif_Hazırlayıcı.Models;

namespace Teklif_Hazırlayıcı.DataAccess
{
    /// <summary>
    /// Provides database operations for companies.
    /// </summary>
    public class CompanyRepository : IRepository<Company>
    {
        private readonly SqlDbConnection _connection;

        public CompanyRepository()
        {
            _connection = new SqlDbConnection();
        }

        public int Insert(Company company)
        {
            string query = "INSERT INTO firmalar (isim, adres, telefon, eposta) " +
                           "VALUES (@Name, @Address, @Phone, @Email); SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", company.Isim);
                    cmd.Parameters.AddWithValue("@Address", company.Adres);
                    cmd.Parameters.AddWithValue("@Phone", company.Telefon);
                    cmd.Parameters.AddWithValue("@Email", company.Eposta);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(Company company)
        {
            string updateQuery = @"UPDATE firmalar SET
                                    isim = @Name,
                                    adres = @Address,
                                    telefon = @Phone,
                                    eposta = @Email
                                  WHERE firma_id = @CompanyId";

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", company.FirmaId);
                    cmd.Parameters.AddWithValue("@Name", company.Isim);
                    cmd.Parameters.AddWithValue("@Address", company.Adres);
                    cmd.Parameters.AddWithValue("@Phone", company.Telefon);
                    cmd.Parameters.AddWithValue("@Email", company.Eposta);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
