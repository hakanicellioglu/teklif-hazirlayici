using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teklif_Hazırlayıcı.DataAccess
{
    public class DbConnection
    {
        private readonly OleDbConnection _connection;
        private readonly string _connectionString;

        public DbConnection()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            _connection = new OleDbConnection(_connectionString);
        }

        public void Open()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();
        }

        public void Close()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
                _connection.Close();
        }

        public OleDbConnection GetConnection()
        {
            return new OleDbConnection(_connectionString);
        }
    }
}
