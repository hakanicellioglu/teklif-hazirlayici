using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teklif_Hazırlayıcı.DataAccess
{
    public static class DatabaseInitializer
    {
        // SQL Server'daki "master" veritabanına bağlanmak için bağlantı dizesi
        private static readonly string masterConnectionString = "Server=.;Database=master;Trusted_Connection=True;";

        // Uygulamanın çalışacağı veritabanı adı
        private static readonly string targetDatabaseName = "TeklifHazirlayiciDB";

        // Hedef veritabanına bağlanmak için bağlantı dizesi
        private static readonly string targetDbConnectionString = $"Server=.;Database={targetDatabaseName};Trusted_Connection=True;";

        /// <summary>
        /// Veritabanı yoksa oluşturur ve gerekli tabloları kurar.
        /// </summary>
        public static void Initialize()
        {
            if (!DatabaseExists())
            {
                CreateDatabase();
                CreateTables();
            }
        }

        /// <summary>
        /// SQL Server'da veritabanı var mı kontrol eder.
        /// </summary>
        private static bool DatabaseExists()
        {
            using (var connection = new SqlConnection(masterConnectionString))
            {
                string query = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{targetDatabaseName}'";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Yeni veritabanı oluşturur.
        /// </summary>
        private static void CreateDatabase()
        {
            using (var connection = new SqlConnection(masterConnectionString))
            {
                string query = $"CREATE DATABASE {targetDatabaseName}";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Veritabanı içinde gerekli tabloları oluşturur.
        /// Bu örnekte temel bir Kullanici tablosu yer almakta.
        /// </summary>
        private static void CreateTables()
        {
            using (var connection = new SqlConnection(targetDbConnectionString))
            {
                connection.Open();

                var commandText = @"
            CREATE TABLE firmalar (
                firma_id INT IDENTITY(1,1) PRIMARY KEY,
                isim NVARCHAR(255),
                adres NVARCHAR(500),
                telefon NVARCHAR(50),
                eposta NVARCHAR(100)
            );

            CREATE TABLE yetkililer (
                yetkili_id INT IDENTITY(1,1) PRIMARY KEY,
                firma_id INT,
                isim NVARCHAR(100),
                soyisim NVARCHAR(100),
                hitap NVARCHAR(50),
                adres NVARCHAR(255),
                telefon NVARCHAR(50),
                eposta NVARCHAR(100),
                FOREIGN KEY(firma_id) REFERENCES firmalar(firma_id)
            );

            CREATE TABLE urunler (
                urun_id INT IDENTITY(1,1) PRIMARY KEY,
                kalip_no NVARCHAR(100),
                urun NVARCHAR(255),
                gramaj FLOAT,
                kategori NVARCHAR(100)
            );

            CREATE TABLE kullanicilar (
                kullanici_id INT IDENTITY(1,1) PRIMARY KEY,
                isim NVARCHAR(100),
                soyisim NVARCHAR(100),
                kullanici_adi NVARCHAR(100),
                parola NVARCHAR(100),
                eposta NVARCHAR(100)
            );

            CREATE TABLE teklifler (
                teklif_id INT IDENTITY(1,1) PRIMARY KEY,
                firma_id INT,
                yetkili_id INT,
                hazirlayan NVARCHAR(255),
                teklif_tarih DATE,
                teslim_sekli NVARCHAR(100),
                odeme_sekli NVARCHAR(100),
                odeme_vade INT,
                teklif_sure INT,
                vade NVARCHAR(100),
                vade_farki FLOAT,
                doviz_birimi NVARCHAR(50),
                doviz_kuru FLOAT,
                iscilik FLOAT,
                lme FLOAT,
                toplam_adet FLOAT,
                toplam_kg FLOAT,
                mal_hizmet_bedeli FLOAT,
                iskonto_orani FLOAT,
                iskonto_tutari FLOAT,
                kdv_orani FLOAT,
                kdv_tutari FLOAT,
                tevkifat BIT,
                onay_durumu NVARCHAR(100),
                tevkifat_orani FLOAT,
                tevkifat_tutari FLOAT,
                genel_toplam FLOAT,
                odencek FLOAT,
                durum NVARCHAR(50),
                FOREIGN KEY(firma_id) REFERENCES firmalar(firma_id),
                FOREIGN KEY(yetkili_id) REFERENCES yetkililer(yetkili_id)
            );

            CREATE TABLE kalemler (
                kalem_id INT IDENTITY(1,1) PRIMARY KEY,
                teklif_id INT,
                urun_id INT,
                yuzey NVARCHAR(100),
                yuzey_kodu NVARCHAR(100),
                boy NVARCHAR(100),
                adet INT,
                kg FLOAT,
                birim_fiyat FLOAT,
                toplam_tutar FLOAT,
                FOREIGN KEY(teklif_id) REFERENCES teklifler(teklif_id),
                FOREIGN KEY(urun_id) REFERENCES urunler(urun_id)
            );
        ";

                using (var command = new SqlCommand(commandText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
