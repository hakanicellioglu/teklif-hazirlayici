using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string query = "SELECT * FROM Company ORDER BY Name";
            using (OleDbCommand cmd = new OleDbCommand(query, _connection.GetConnection()))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }

        }
    }
}
