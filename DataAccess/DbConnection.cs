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
        /*
         *
         * Veritabanı bağlantısını ve bağlantı dizesini saklayan özel alanlardır.
         * `_connection`: OleDb üzerinden veritabanına bağlantı sağlar.
         * `_connectionString`: App.config içinden alınan bağlantı dizesidir.
         *
         */
        private readonly OleDbConnection _connection;
        private readonly string _connectionString;

        public DbConnection()
        {
            /*
             *
             * DbConnection sınıfının kurucusudur.
             * Uygulama yapılandırma dosyasından bağlantı dizesi okunur ve `_connectionString` alanına atanır.
             * Bu bağlantı dizesi ile yeni bir `OleDbConnection` nesnesi oluşturularak `_connection` alanına atanır.
             *
             */
            _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            _connection = new OleDbConnection(_connectionString);
        }

        public OleDbConnection GetConnection()
        {
            return new OleDbConnection(_connectionString);
        }
    }
}
