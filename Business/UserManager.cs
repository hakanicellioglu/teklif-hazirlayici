using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.DataAccess;
using Teklif_Hazırlayıcı.Helpers;
using System.Security.Cryptography;


namespace Teklif_Hazırlayıcı.Business
{
    public class UserManager
    {
        /*
        *
        * Veritabanı işlemleri için kullanılan bağlantı nesnesi. 
        * Uygulama boyunca yalnızca okunabilir (readonly) olarak tanımlanmıştır.
        *
        */
        private readonly DataAccess.SqlDbConnection _connection;

        public UserManager()
        {
            /*
             *
             * DbConnection sınıfından yeni bir örnek oluşturularak 
             * _connection alanına atanır. Veritabanı bağlantısını başlatmak için kullanılır.
             *
             */
            _connection = new DataAccess.SqlDbConnection();
        }


        private string ComputeSha256Hash(string rawData)
        {
            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                    StringBuilder builder = new StringBuilder();
                    foreach (var b in bytes)
                        builder.Append(b.ToString("x2")); // hex format
                    return builder.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }


        public bool AddUser(string name, string surname, string username, string email, string password)
        {
            try
            {
                /*
                     *
                     * Yeni bir kullanıcıyı "kullanicilar" tablosuna ekler.
                     * Önce kullanıcı adı ve e-posta adresinin veritabanında olup olmadığı kontrol edilir.
                     * Eğer bu bilgiler mevcutsa işlem iptal edilir ve kullanıcıya hata mesajı gösterilir.
                     * Bilgiler benzersizse veritabanına isim, soyisim, kullanıcı adı, e-posta ve parola bilgileri kaydedilir.
                     * İşlem sonucuna göre kullanıcıya bilgi veya hata mesajı verilir.
                     *
                     */
                if (isThere(username))
                {
                    MessageHelper.ShowError($"\"{username}\" kullanıcı adı zaten kullanılmaktadır.");
                    return false;
                }
                else
                {
                    if (isThere(email))
                    {
                        MessageHelper.ShowError($"\"{email}\" e-posta adresi zaten kullanılmaktadır.");
                        return false;
                    }
                    else
                    {
                        try
                        {
                            using (SqlConnection conn = _connection.GetConnection())
                            {

                                conn.Open();

                                string query = "INSERT INTO kullanicilar(isim,soyisim,kullanici_adi, eposta, parola) VALUES (@Name, @Surname, @Username, @Email, @Password)";
                                using (SqlCommand command = new SqlCommand(query, conn))
                                {
                                    string hashedPassword = ComputeSha256Hash(password);
                                    command.Parameters.AddWithValue("@Name", name);
                                    command.Parameters.AddWithValue("@Surname", surname);
                                    command.Parameters.AddWithValue("@Username", username);
                                    command.Parameters.AddWithValue("@Email", email);
                                    command.Parameters.AddWithValue("@Password", hashedPassword);

                                    int result = command.ExecuteNonQuery();
                                    if (result > 0)
                                    {
                                        MessageHelper.ShowSuccess("Başarıyla kayıt oldunuz!");
                                        return true;
                                    }
                                    else
                                    {
                                        MessageHelper.ShowError("Kayıt işlemi sırasında bilinmeyen bir hatayla karşılaşıldı.");
                                        return false;
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageHelper.ShowError("Hata: " + ex.Message);
                            return false;
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

        private bool isThere(string parameter)
        {
            try
            {
                /*
                     *
                     * Verilen `parameter` değerine sahip kullanıcı adının "kullanicilar" tablosunda olup olmadığını kontrol eder.
                     * Sorguda COUNT(*) kullanılarak eşleşen kayıt sayısı alınır.
                     * Sonuç 0'dan büyükse true (mevcut), değilse false (yok) döndürülür.
                     *
                     */
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM kullanicilar WHERE kullanici_adi = @Parameter";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Parameter", parameter);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public bool UserExists(string username, string password)
        {
            try
            {
                /*
                     *
                     * Verilen kullanıcı adı ve parola bilgilerine sahip bir kullanıcının olup olmadığını kontrol eder.
                     * "kullanicilar" tablosunda kullanıcı adı ve parola eşleşmesi aranır.
                     * Eşleşme varsa true, yoksa false döndürülür.
                     *
                     */
                using (SqlConnection conn = _connection.GetConnection())
                {

                    conn.Open();
                    string query = "SELECT COUNT(*) AS KayıtSayisi FROM kullanicilar WHERE kullanici_adi = @Username AND parola = @Password";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {

                        string hashedPassword = ComputeSha256Hash(password);


                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", hashedPassword);



                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }

        public void SelectUserId(string username)
        {
            try
            {
                int kullanici_id = -1;
                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    string queryId = "SELECT kullanici_id FROM kullanicilar WHERE kullanici_adi = @Username";
                    using (SqlCommand command2 = new SqlCommand(queryId, conn))
                    {
                        command2.Parameters.AddWithValue("@Username", username);
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                kullanici_id = Convert.ToInt32(reader["kullanici_id"].ToString());
                                Properties.Settings.Default.kullanici_id = kullanici_id;
                            }
                            else
                            {
                                Properties.Settings.Default.kullanici_id = kullanici_id;
                            }
                            Properties.Settings.Default.Save();
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

        public string GetUserFullName(int kullaniciId)
        {
            try
            {
                string isimSoyisim = "Sayın Yetkili";

                string query = "SELECT isim, soyisim FROM Kullanicilar WHERE kullanici_id = @kullaniciId";

                using (SqlConnection conn = _connection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@kullaniciId", kullaniciId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string isim = reader["isim"].ToString();
                                string soyisim = reader["soyisim"].ToString();

                                isimSoyisim = $"{isim} {soyisim}".Trim();
                            }
                        }
                    }
                }

                return isimSoyisim;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("Hata oluştu: " + ex.Message);
                throw;
            }
        }
    }
}