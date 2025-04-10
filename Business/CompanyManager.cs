using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Helpers;

namespace Teklif_Hazırlayıcı.Business
{
    public class CompanyManager
    {
        private readonly DataAccess.DbConnection _connection;

        public CompanyManager()
        {
            _connection = new DataAccess.DbConnection();
        }

        public DataTable GetCompany()
        {
            string query = "SELECT * FROM Firmalar";
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
            string query = "SELECT * FROM firmalar WHERE adi LIKE @ad";
            DataTable dt = new DataTable();

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, _connection.GetConnection()))
                {
                    // Parametreleri ekle
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ad", $"%{search}%");
                    //cmd.Parameters.AddWithValue("@adres", $"%{search}%");
                    //cmd.Parameters.AddWithValue("@telefon", $"%{search}%");
                    //cmd.Parameters.AddWithValue("@eposta", $"%{search}%");

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                conn.Close();
            }

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            return dt;
        }
        public void AddCompany(string name, string address, string phone_number, string email)
        {
            if (!CompanyExistsName(name))
            {
                string query = "INSERT INTO firmalar(adi, adres, telefon, eposta) VALUES(@Name, @Address, @PhoneNumber, @Email)";
                using (OleDbConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@PhoneNumber", phone_number);
                        cmd.Parameters.AddWithValue("@Email", email);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageHelper.ShowSuccess("Firma başarıyla eklendi");
                        }
                        else
                        {
                            MessageHelper.ShowError("Firma eklenirken hata oluştu.");
                        }
                    }
                }
            }
            else
            {
                MessageHelper.ShowWarning("Bu isimde bir firma zaten var.");
            }
        }
        public void UpdateCompany(int? id, string name, string address, string phone_number, string email)
        {
            if (!id.HasValue)
            {
                MessageHelper.ShowError("Geçersiz firma ID.");
                return;
            }

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();

                // Mevcut veriyi çekiyoruz
                string selectQuery = "SELECT adi, adres, telefon, eposta FROM firmalar WHERE firma_id = @CompanyId";
                using (OleDbCommand selectCmd = new OleDbCommand(selectQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@CompanyId", id);

                    using (OleDbDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string currentName = reader["adi"].ToString();
                            string currentAddress = reader["adres"].ToString();
                            string currentPhone = reader["telefon"].ToString();
                            string currentEmail = reader["eposta"].ToString();

                            // Farklılık var mı kontrol et
                            if (currentName == name &&
                                currentAddress == address &&
                                currentPhone == phone_number &&
                                currentEmail == email)
                            {
                                MessageHelper.ShowInfo("Hiçbir değişiklik yapılmadı.");
                                return;
                            }
                        }
                        else
                        {
                            MessageHelper.ShowError("Firma bulunamadı.");
                            return;
                        }
                    }
                }

                // Güncelleme işlemi
                string updateQuery = "UPDATE firmalar SET adi = @Name, adres = @Address, telefon = @PhoneNumber, eposta = @Email WHERE firma_id = @CompanyId";
                using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@Name", name);
                    updateCmd.Parameters.AddWithValue("@Address", address);
                    updateCmd.Parameters.AddWithValue("@PhoneNumber", phone_number);
                    updateCmd.Parameters.AddWithValue("@Email", email);
                    updateCmd.Parameters.AddWithValue("@CompanyId", id);

                    int result = updateCmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageHelper.ShowSuccess("Firma başarıyla güncellendi.");
                    }
                    else
                    {
                        MessageHelper.ShowError("Firma güncellenirken hata oluştu.");
                    }
                }
            }
        }
        public bool CompanyExistsName(string parameter)
        {
            string query = "SELECT adi FROM firmalar WHERE adi = @Name";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", parameter);
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string company_name = reader["adi"].ToString().ToLower();
                            return company_name == parameter.ToLower();
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
        public List<Dictionary<string, string>> GetCompanyById(int? companyId)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            string query = "SELECT adi,adres,telefon,eposta FROM firmalar WHERE firma_id = @CompanyId";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId ?? (object)DBNull.Value);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, string> row = new Dictionary<string, string>();
                            row["adi"] = reader["adi"].ToString();
                            row["adres"] = reader["adres"].ToString();
                            row["telefon"] = reader["telefon"].ToString();
                            row["eposta"] = reader["eposta"].ToString();
                            result.Add(row);
                        }
                    }
                }
            }
            return result;
        }

    }
}
