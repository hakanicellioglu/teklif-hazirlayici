using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Teklif_Hazırlayıcı.Helpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Teklif_Hazırlayıcı.Business
{
    public class CompanyManager
    {
        /*
        *
        * Veritabanı işlemleri için kullanılan bağlantı nesnesi. 
        * Uygulama boyunca yalnızca okunabilir (readonly) olarak tanımlanmıştır.
        *
        */
        private readonly DataAccess.SqlDbConnection _connection;

        public CompanyManager()
        {
            /*
             *
             * DbConnection sınıfından yeni bir örnek oluşturularak 
             * _connection alanına atanır. Veritabanı bağlantısını başlatmak için kullanılır.
             *
             */
            _connection = new DataAccess.SqlDbConnection();
        }

        public DataTable GetCompany()
        {
            /*
             *
             * Veritabanındaki "firmalar" tablosundaki tüm kayıtları getirir.
             * OleDbCommand ile sorgu hazırlanır ve OleDbDataAdapter ile DataTable nesnesine doldurulur.
             * Doldurulmuş DataTable nesnesi döndürülür.
             *
             */
            try
            {
                string query = "SELECT * FROM firmalar";
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
                return null;
            }
        }
        public DataTable Search(string search)
        {
            /*
             *
             * Firma adını içeren arama terimine göre "firmalar" tablosunda arama yapar.
             * Sadece firma adı (adi) alanı üzerinden LIKE operatörü ile eşleşme sağlanır.
             * Elde edilen sonuçlar bir DataTable nesnesine aktarılır.
             * Sonuç bulunamazsa null döndürülür, aksi halde doldurulmuş DataTable döndürülür.
             *
             */
            try
            {
                string query = "SELECT * FROM firmalar WHERE isim LIKE @ad";
                DataTable dt = new DataTable();

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, _connection.GetConnection()))
                    {
                        // Parametreleri ekle
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@ad", $"%{search}%");
                        //cmd.Parameters.AddWithValue("@adres", $"%{search}%");
                        //cmd.Parameters.AddWithValue("@telefon", $"%{search}%");
                        //cmd.Parameters.AddWithValue("@eposta", $"%{search}%");

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
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
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                return null;
            }
        }
        public void AddCompany(string name, string address, string phone_number, string email)
        {
            /*
             *
             * Verilen firma bilgilerini kullanarak "firmalar" tablosuna yeni bir kayıt ekler.
             * Öncesinde aynı ada sahip bir firma olup olmadığı `CompanyExistsName` fonksiyonu ile kontrol edilir.
             * Firma zaten varsa uyarı mesajı gösterilir.
             * Kayıt işlemi başarılı olursa kullanıcı bilgilendirilir, aksi durumda hata mesajı gösterilir.
             *
             */
            try
            {
                if (!CompanyExistsName(name))
                {
                    string query = "INSERT INTO firmalar(isim, adres, telefon, eposta) VALUES(@Name, @Address, @PhoneNumber, @Email)";
                    using (SqlConnection conn = _connection.GetConnection())
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
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
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }
        public void UpdateCompany(int? id, string name, string address, string phone_number, string email)
        {
            /*
             *
             * Belirtilen `id` değerine sahip firmanın bilgilerini günceller.
             * Güncelleme öncesinde mevcut kayıtlar veritabanından alınır ve gelen parametrelerle karşılaştırılır.
             * Eğer bilgilerde bir değişiklik yoksa işlem yapılmaz, kullanıcı bilgilendirilir.
             * Değişiklik varsa veritabanı güncellenir ve işlem sonucu kullanıcıya bildirilir.
             *
             */
            try
            {
                if (!id.HasValue)
                {
                    MessageHelper.ShowError("Geçersiz firma ID.");
                    return;
                }

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();

                    // Mevcut veriyi çekiyoruz
                    string selectQuery = "SELECT isim, adres, telefon, eposta FROM firmalar WHERE firma_id = @CompanyId";
                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@CompanyId", id);

                        using (SqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string currentName = reader["isim"].ToString();
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
                    string updateQuery = "UPDATE firmalar SET isim = @Name, adres = @Address, telefon = @PhoneNumber, eposta = @Email WHERE firma_id = @CompanyId";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
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
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }
        public void DeleteCompany(int id)
        {
            try
            {
                // Önce firmaya bağlı yetkili var mı kontrol edelim
                AuthManager authManager = new AuthManager();
                var authList = authManager.GetAuthByCompanyId(id);

                if (authList != null && authList.Count > 0)
                {
                    MessageHelper.ShowWarning("Bu firmaya bağlı yetkililer var. Önce bağlı yetkilileri silmelisiniz.");
                    return;
                }

                // Daha sonra sil
                string query = "DELETE FROM firmalar WHERE firma_id = @CompanyId";
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyId", id);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                            MessageHelper.ShowSuccess("Firma başarıyla silindi.");
                        else
                            MessageHelper.ShowError("Firma silerken hata oluştu.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public bool CompanyExistsName(string parameter)
        {
            /*
             *
             * Verilen firma adının "firmalar" tablosunda zaten mevcut olup olmadığını kontrol eder.
             * Sorgu ile eşleşen bir kayıt varsa true, yoksa false döndürülür.
             * Karşılaştırma büyük/küçük harf duyarsız yapılır.
             *
             */
            try
            {
                string query = "SELECT isim FROM firmalar WHERE isim = @Name";
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", parameter);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string company_name = reader["isim"].ToString().ToLower();
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
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                return false;
            }
        }
        public List<Dictionary<string, string>> GetCompanyById(int? companyId)
        {
            /*
             *
             * Belirtilen `companyId` değerine sahip firmanın bilgilerini getirir.
             * Sorgu sonucunda firma adı, adres, telefon ve e-posta bilgileri çekilir.
             * Her kayıt bir sözlük (Dictionary) olarak oluşturulup listeye eklenir.
             * Elde edilen sözlük listesi döndürülür.
             *
             */
            try
            {
                List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

                string query = "SELECT isim,adres,telefon,eposta FROM firmalar WHERE firma_id = @CompanyId";
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
                                row["isim"] = reader["isim"].ToString();
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
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

    }
}
