using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teklif_Hazırlayıcı.DataAccess;

namespace Teklif_Hazırlayıcı.Business
{
    public class UserManager
    {
        private readonly DataAccess.DbConnection _connection;

        public UserManager()
        {
            _connection = new DataAccess.DbConnection();
        }

        public bool UserExists(string username, string password)
        {
            try
            {
                _connection.Open();

                string query = "SELECT COUNT(*) FROM kullanicilar WHERE kullanici_adi = @Username AND parola = @Password";
                using (OleDbCommand command = new OleDbCommand(query, _connection.GetConnection()))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
