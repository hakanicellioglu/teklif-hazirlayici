using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teklif_Hazırlayıcı.DataAccess;
using Teklif_Hazırlayıcı.Helpers;

namespace Teklif_Hazırlayıcı.Business
{
    public class UserManager
    {
        private readonly DataAccess.DbConnection _connection;

        public UserManager()
        {
            _connection = new DataAccess.DbConnection();
        }

        public bool AddUser(string name, string surname, string username, string email, string password)
        {
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
                        using (OleDbConnection conn = _connection.GetConnection())
                        {
                            string query = "INSERT INTO kullanicilar(isim,soyisim,kullanici_adi, eposta, parola) VALUES (@Name, @Surname, @Username, @Email, @Password)";
                            using (OleDbCommand command = new OleDbCommand(query, _connection.GetConnection()))
                            {
                                command.Parameters.AddWithValue("@Name", username);
                                command.Parameters.AddWithValue("@Surname", surname);
                                command.Parameters.AddWithValue("@Username", username);
                                command.Parameters.AddWithValue("@Email", email);
                                command.Parameters.AddWithValue("@Password", password);
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

        private bool isThere(string parameter)
        {
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM kullanicilar WHERE kullanici_adi = @Parameter";
                using (OleDbCommand command = new OleDbCommand(query, _connection.GetConnection()))
                {
                    command.Parameters.AddWithValue("@Parameter", parameter);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool UserExists(string username, string password)
        {
            using (OleDbConnection conn = _connection.GetConnection())
            {

                conn.Open();
                string query = "SELECT COUNT(*) FROM kullanicilar WHERE kullanici_adi = @Username AND parola = @Password";
                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
