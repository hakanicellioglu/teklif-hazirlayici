using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Teklif_Hazırlayıcı.Helpers;
using Teklif_Hazırlayıcı.Forms;
using Teklif_Hazırlayıcı.DataAccess;

namespace Teklif_Hazırlayıcı.Business
{
    public class AuthManager
    {

        /*
        *
        * Veritabanı işlemleri için kullanılan bağlantı nesnesi. 
        * Uygulama boyunca yalnızca okunabilir (readonly) olarak tanımlanmıştır.
        *
        */
        private readonly DataAccess.SqlDbConnection _connection;


        public AuthManager()
        {
            /*
             *
             * DbConnection sınıfından yeni bir örnek oluşturularak 
             * _connection alanına atanır. Veritabanı bağlantısını başlatmak için kullanılır.
             *
             */
            _connection = new DataAccess.SqlDbConnection();

        }

        public DataTable GetAuth()
        {
            /*
            *
            * Veritabanındaki "yetkililer" tablosundaki tüm kayıtları getirir.
            * OleDbCommand ile sorgu hazırlanır ve OleDbDataAdapter ile DataTable nesnesine doldurulur.
            * Sonuç olarak doldurulmuş DataTable döndürülür.
            *
            */
            string query = "SELECT * FROM yetkililer";
            using (SqlCommand cmd = new SqlCommand(query, _connection.GetConnection()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        public DataTable GetAuthWithCompanyName()
        {
            /*
             *
             * "yetkililer" tablosundaki kayıtları firma adı ile birlikte getirir.
             * LEFT JOIN ile "firmalar" tablosundaki firma adları eşleştirilir.
             * Sorgu sonucunda yetkili bilgileri ve ilişkili firma adı DataTable'a doldurularak döndürülür.
             *
             */
            string query = @"
            SELECT y.yetkili_id, f.isim, y.isim, y.soyisim, y.hitap, y.adres, y.telefon, y.eposta
            FROM yetkililer y
            LEFT JOIN firmalar f ON y.firma_id = f.firma_id";

            using (SqlCommand cmd = new SqlCommand(query, _connection.GetConnection()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable Search(string search)
        {
            /*
             *
             * Girilen arama terimine göre "yetkililer" ve "firmalar" tablolarında arama yapar.
             * Arama; isim, soyisim, telefon ve e-posta alanlarında LIKE operatörü ile gerçekleştirilir.
             * Elde edilen sonuçlar bir DataTable içine doldurulur.
             * Sonuç bulunamazsa null döndürülür.
             *
             */
            string query = @"
            SELECT y.*, f.isim AS Firma 
            FROM yetkililer y 
            LEFT JOIN firmalar f ON y.firma_id = f.firma_id 
            WHERE y.isim LIKE @Isim 
               OR y.soyisim LIKE @Soyisim 
               OR y.telefon LIKE @Telefon 
               OR y.eposta LIKE @Eposta";

            DataTable dt = new DataTable();

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    string likeValue = $"%{search}%";
                    cmd.Parameters.AddWithValue("@Isim", likeValue);
                    cmd.Parameters.AddWithValue("@Soyisim", likeValue);
                    cmd.Parameters.AddWithValue("@Telefon", likeValue);
                    cmd.Parameters.AddWithValue("@Eposta", likeValue);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt;

        }
        public void AddAuth(int company_id, string name, string surname, string honorific, string address, string phone_number, string email)
        {
            string query = "INSERT INTO yetkililer(firma_id, isim, soyisim, hitap, adres, telefon, eposta) VALUES(@CompanyId, @Name, @Surname, @Honorific, @Address, @PhoneNumber, @Email)";
            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    AddAuthParameters(cmd, company_id, name, surname, honorific, address, phone_number, email);
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

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();

                // Mevcut veriyi çekiyoruz
                string selectQuery = "SELECT firma_id, isim, soyisim, hitap, adres, telefon, eposta FROM yetkililer WHERE yetkili_id = @AuthId";
                using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("@AuthId", auth_id);

                    using (SqlDataReader reader = selectCmd.ExecuteReader())
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
                            if (currentCompanyId == company_id &&
                                currentName == name &&
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
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                {
                    AddAuthParameters(updateCmd, company_id, name, surname, honorific, address, phone_number, email);
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
            // Tekliflere bağlı mı kontrol et (örnek: teklifler tablosunda yetkili_id kolonu varsa)
            string checkQuery = "SELECT COUNT(*) FROM teklifler WHERE yetkili_id = @AuthId";
            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@AuthId", auth_id);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageHelper.ShowWarning("Bu yetkiliye ait teklifler mevcut. Önce teklifleri silmelisiniz.");
                        return;
                    }
                }

                // Silme işlemi
                string query = "DELETE FROM yetkililer WHERE yetkili_id = @AuthId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AuthId", auth_id);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                        MessageHelper.ShowSuccess("Yetkili başarıyla silindi.");
                    else
                        MessageHelper.ShowError("Yetkili silinirken hata oluştu.");
                }
            }
        }

        public List<Dictionary<string, string>> GetAuthById(int? authId)
        {
            /*
             *
             * Belirtilen `authId` değerine sahip yetkili kaydını getirir.
             * Sorgu sonucunda elde edilen bilgiler bir sözlük (Dictionary) yapısında toplanır ve listeye eklenir.
             * Her kayıt için firma_id, isim, soyisim, hitap, adres, telefon ve eposta alanları alınır.
             * Sonuç olarak bu bilgileri içeren sözlük listesini döndürür.
             *
             */
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            string query = "SELECT firma_id, isim, soyisim, hitap, adres, telefon, eposta FROM yetkililer WHERE yetkili_id = @AuthId";
            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AuthId", authId ?? (object)DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
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
        public List<Dictionary<string, string>> GetAuthByCompanyId(long? companyId)
        {
            /*
             *
             * Belirtilen `companyId` değerine sahip firmanın yetkililerini getirir.
             * "yetkililer" tablosundan yalnızca isim ve hitap alanları seçilir.
             * Her kayıt bir sözlük olarak oluşturulup listeye eklenir.
             * Hitap alanı null ise boş string atanır.
             * Sonuç olarak bu bilgileri içeren sözlük listesi döndürülür.
             *
             */
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            string query = "SELECT yetkili_id, isim, hitap FROM yetkililer WHERE firma_id = @CompanyId";
            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", companyId ?? (object)DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, string> row = new Dictionary<string, string>();
                            row["yetkili_id"] = reader["yetkili_id"].ToString();  // BU satır ekleniyor!
                            row["isim"] = reader["isim"].ToString();
                            row["hitap"] = reader["hitap"] != DBNull.Value ? reader["hitap"].ToString() : "";
                            result.Add(row);
                        }
                    }
                }
            }
            return result;
        }

        public List<(string Name, string DisplayName)> GetColumnDisplayNames(string tableName)
        {
            var list = new List<(string, string)>();

            string query = @"
    SELECT 
        c.name AS ColumnName,
        ISNULL(ep.value, c.name) AS DisplayName
    FROM sys.columns c
    LEFT JOIN sys.extended_properties ep 
        ON ep.major_id = c.object_id 
        AND ep.minor_id = c.column_id 
        AND ep.name = 'MS_Description'
    WHERE c.object_id = OBJECT_ID(@TableName)
    ORDER BY c.column_id";

            using (var conn = new SqlDbConnection().GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableName", "dbo." + tableName);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string display = reader.GetString(1);
                        list.Add((name, display));
                    }
                }
            }

            return list;
        }

        private void AddAuthParameters(SqlCommand cmd, int? company_id, string name, string surname, string honorific, string address, string phone_number, string email)
        {
            cmd.Parameters.AddWithValue("@CompanyId", company_id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Surname", surname);
            cmd.Parameters.AddWithValue("@Honorific", honorific);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@PhoneNumber", phone_number);
            cmd.Parameters.AddWithValue("@Email", email);
        }
    }
}


