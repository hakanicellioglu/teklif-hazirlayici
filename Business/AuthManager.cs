using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Teklif_Hazırlayıcı.Helpers;

namespace Teklif_Hazırlayıcı.Business
{
    public class AuthManager
    {
        private readonly DataAccess.DbConnection _connection;

        public AuthManager()
        {
            _connection = new DataAccess.DbConnection();
        }


        public DataTable GetAuth()
        {
            string query = "SELECT * FROM yetkililer";
            using (OleDbCommand cmd = new OleDbCommand(query, _connection.GetConnection()))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable GetAuthWithCompanyName()
        {
            string query = @"
        SELECT y.yetkili_id, f.adi AS Firma, y.isim, y.soyisim, y.hitap, y.adres, y.telefon, y.eposta
        FROM yetkililer y
        LEFT JOIN firmalar f ON y.firma_id = f.firma_id";

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
            string query = @"
        SELECT y.*, f.adi AS Firma 
        FROM yetkililer y 
        LEFT JOIN firmalar f ON y.firma_id = f.firma_id 
        WHERE y.isim LIKE @Isim 
           OR y.soyisim LIKE @Soyisim 
           OR y.telefon LIKE @Telefon 
           OR y.eposta LIKE @Eposta";

            DataTable dt = new DataTable();

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    string likeValue = $"%{search}%";
                    cmd.Parameters.AddWithValue("@Isim", likeValue);
                    cmd.Parameters.AddWithValue("@Soyisim", likeValue);
                    cmd.Parameters.AddWithValue("@Telefon", likeValue);
                    cmd.Parameters.AddWithValue("@Eposta", likeValue);

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt.Rows.Count > 0 ? dt : null;
        }


        public void AddAuth(int company_id, string name, string surname, string honorific, string address, string phone_number, string email)
        {
            string query = "INSERT INTO yetkililer(firma_id, isim, soyisim, hitap, adres, telefon, eposta) VALUES(@CompanyId, @Name, @Surname, @Honorific, @Address, @PhoneNumber, @Email)";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@firma_id", company_id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Surname", surname);
                    cmd.Parameters.AddWithValue("@Honorific", honorific);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phone_number);
                    cmd.Parameters.AddWithValue("@Email", email);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageHelper.ShowSuccess("Yetkili başarıyla eklendi");
                    }
                    else
                    {
                        MessageHelper.ShowError("Yetkili eklenirken hata oluştu.");
                    }
                }
            }
        }

        public void UpdateAuth(int? auth_id, int? company_id, string name, string surname, string honorific, string address, string phone_number, string email)
        {
            if (!auth_id.HasValue)
            {
                MessageHelper.ShowError("Geçersiz yetkili kimlik numarası.");
                return;
            }

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();

                // Mevcut veriyi çekiyoruz
                string selectQuery = "SELECT firma_id, isim, soyisim, hitap, adres, telefon, eposta FROM yetkililer WHERE yetkili_id = @AuthId";
                using (OleDbCommand selectCmd = new OleDbCommand(selectQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@AuthId", auth_id);

                    using (OleDbDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int currentCompanyId = (int)reader["firma_id"];
                            string currentName = reader["isim"].ToString();
                            string currentSurname = reader["soyisim"].ToString();
                            string currentHonorific = reader["hitap"].ToString();
                            string currentAddress = reader["adres"].ToString();
                            string currentPhone = reader["telefon"].ToString();
                            string currentEmail = reader["eposta"].ToString();

                            // Farklılık var mı kontrol et
                            if (currentName == name &&
                                currentSurname == surname &&
                                currentHonorific == honorific &&
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
                            MessageHelper.ShowError("Yetkili bulunamadı.");
                            return;
                        }
                    }
                }

                // Güncelleme işlemi
                string updateQuery = "UPDATE yetkililer SET firma_id = @CompanyId, isim = @Name, soyisim = @Surname, hitap = @Honorific, adres = @Address, telefon = @PhoneNumber, eposta = @Email WHERE yetkili_id = @AuthId";
                using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@CompanyId", company_id);
                    updateCmd.Parameters.AddWithValue("@Name", name);
                    updateCmd.Parameters.AddWithValue("@Surname", surname);
                    updateCmd.Parameters.AddWithValue("@Honorific", honorific);
                    updateCmd.Parameters.AddWithValue("@Address", address);
                    updateCmd.Parameters.AddWithValue("@PhoneNumber", phone_number);
                    updateCmd.Parameters.AddWithValue("@Email", email);
                    updateCmd.Parameters.AddWithValue("@AuthId", auth_id);


                    int result = updateCmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageHelper.ShowSuccess("Yetkili başarıyla güncellendi.");
                    }
                    else
                    {
                        MessageHelper.ShowError("Yetkili güncellenirken hata oluştu.");
                    }
                }
            }
        }

        public void DeleteAuth(int auth_id)
        {
            string query = "DELETE FROM yetkililer WHERE yetkili_id = @AuthId";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AuthId", auth_id);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageHelper.ShowSuccess("Yetkili başarıyla silindi");
                    }
                    else
                    {
                        MessageHelper.ShowError("Yetkili silerken hata oluştu.");
                    }
                }
            }
        }

        public List<Dictionary<string, string>> GetAuthById(int? authId)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            string query = "SELECT firma_id, isim, soyisim, hitap, adres, telefon, eposta FROM yetkililer WHERE yetkili_id = @AuthId";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AuthId", authId ?? (object)DBNull.Value);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, string> row = new Dictionary<string, string>();
                            row["firma_id"] = reader["firma_id"].ToString();
                            row["isim"] = reader["isim"].ToString();
                            row["soyisim"] = reader["soyisim"].ToString();
                            row["hitap"] = reader["hitap"].ToString();
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
