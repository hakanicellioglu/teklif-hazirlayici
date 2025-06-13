using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teklif_Hazırlayıcı.DataAccess
{
    public class SqlDbConnection
    {
        /*
         * SQL Server bağlantısını yöneten sınıftır.
         * ConnectionString, App.config üzerinden "SqlConnectionString" anahtarıyla alınır.
         */
        private readonly string _connectionString;

        public SqlDbConnection()
        {
            _connectionString = Environment.GetEnvironmentVariable("SQL_CONN_STRING") ??
                               ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        }

        /// <summary>
        /// Yeni bir SQL bağlantısı döner.
        /// </summary>
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
