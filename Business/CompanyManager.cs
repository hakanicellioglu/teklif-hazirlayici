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
            string query = @"SELECT * FROM firmalar 
                     WHERE adi LIKE @ad";

            DataTable dt = new DataTable();

            _connection.Open();

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

            _connection.Close();

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            return dt;
        }





        public bool CompanyExists(string parameter)
        {
            string query = "SELECT adi FROM firmalar WHERE name = @Name";
            using (OleDbCommand cmd = new OleDbCommand(query, _connection.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@Name", parameter);
                OleDbDataReader reader = cmd.ExecuteReader();
                string company_name = reader["adi"].ToString().ToLower();
                if (company_name == parameter.ToLower())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void AddCompany(string name, string address, string phone_number, string email)
        {
            if (!CompanyExists(name))
            {
                string query = "INSERT INTO firmalar(adi,adres,telefon,eposta) VALUES(@Name, @Address, @PhoneNumber, @Email)";
                using (OleDbCommand cmd = new OleDbCommand(query, _connection.GetConnection()))
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
    }
}
