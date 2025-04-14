using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teklif_Hazırlayıcı.Helpers;

namespace Teklif_Hazırlayıcı.Business
{
    public class OfferManager
    {
        private readonly DataAccess.DbConnection _connection;
        public OfferManager()
        {
            _connection = new DataAccess.DbConnection();

        }
        public DataTable GetOffer()
        {
            string query = @"
            SELECT y.isim, y.soyisim, y.hitap, f.adi, t.teklif_tarih, t.durum
            FROM (teklifler AS t
            LEFT JOIN firmalar AS f ON t.firma_id = f.firma_id)
            LEFT JOIN yetkililer AS y ON t.yetkili_id = y.yetkili_id;";

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
            string query = @"
            SELECT y.isim, y.soyisim, y.hitap, f.adi, t.teklif_tarih, t.durum
            FROM (teklifler t 
            LEFT JOIN firmalar f ON t.firma_id = f.firma_id)
            LEFT JOIN yetkililer y ON y.firma_id = f.firma_id
            WHERE y.isim LIKE @Name 
               OR y.soyisim LIKE @Surname
               OR f.adi LIKE @CompanyName";

            DataTable dt = new DataTable();

            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    string likeValue = $"%{search}%";
                    cmd.Parameters.AddWithValue("@Name", likeValue);
                    cmd.Parameters.AddWithValue("@Surname", likeValue);
                    cmd.Parameters.AddWithValue("@CompanyName", likeValue);
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            return dt.Rows.Count > 0 ? dt : null;
        }
        public void AddOffer(int firma_id, int yetkili_id, DateTime teklif_tarih, string teslim_sekli, string odeme_sekli, int odeme_vadesi, int teklif_suresi, string doviz_kuru, char doviz_birimi, string vade, int lme, decimal iskonto_orani, decimal kdv_orani, bool tevkifat, decimal tevkifat_orani, string durum)
        {
            string query = "INSERT INTO teklifler (firma_id, yetkili_id, teklif_tarih, teslim_sekli, odeme_sekli, odeme_vadesi, teklif_suresi, doviz_kuru, doviz_birimi, vade, lme, iskonto_orani, kdv_orani, tevkifat, tevkifat_orani, durum) VALUES (@CompanyId, @AuthorizedPersonId, @OfferDate, @DeliveryMethod, @PaymentMethod, @PaymentDue, @OfferValidity, @ExchangeRate, @CurrencyUnit, @Term, @Lme, @DiscountRate, @VatRate, @Withholding, @WithholdingRate, @Status);";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", firma_id);
                    cmd.Parameters.AddWithValue("@AuthorizedPersonId", yetkili_id);
                    cmd.Parameters.AddWithValue("@OfferDate", teklif_tarih);
                    cmd.Parameters.AddWithValue("@DeliveryMethod", teslim_sekli);
                    cmd.Parameters.AddWithValue("@PaymentMethod", odeme_sekli);
                    cmd.Parameters.AddWithValue("@PaymentDue", odeme_vadesi);
                    cmd.Parameters.AddWithValue("@OfferValidity", teklif_suresi);
                    cmd.Parameters.AddWithValue("@ExchangeRate", doviz_kuru);
                    cmd.Parameters.AddWithValue("@CurrencyUnit", doviz_birimi);
                    cmd.Parameters.AddWithValue("@Term", vade);
                    cmd.Parameters.AddWithValue("@Lme", lme);
                    cmd.Parameters.AddWithValue("@DiscountRate", iskonto_orani);
                    cmd.Parameters.AddWithValue("@VatRate", kdv_orani);
                    cmd.Parameters.AddWithValue("@Withholding", tevkifat);
                    cmd.Parameters.AddWithValue("@WithholdingRate", tevkifat_orani);
                    cmd.Parameters.AddWithValue("@Status", durum);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        if(MessageHelper.ShowQuestion("Teklif başarıyla eklendi. Ürün eklemek ister misiniz?") == System.Windows.Forms.DialogResult.Yes)
                        {
                            // Yönlendirme.
                        }
                    }
                    else
                    {
                        MessageHelper.ShowError("Teklif eklenirken hata oluştu.");

                    }
                }
            }

        }

        public void AddProduct(string mold_number, string product, decimal weight, string category)
        {
            string query = "INSERT INTO urunler(kalip_no, urun, gramaj, kategori) VALUES(@MoldNumber, @Product, @Weight, @Category)";
            using (OleDbConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MoldNumber", mold_number);
                    cmd.Parameters.AddWithValue("@Product", product);
                    cmd.Parameters.Add("@Weight", OleDbType.Double).Value = weight;
                    cmd.Parameters.AddWithValue("@Category", category);
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageHelper.ShowSuccess("Ürün başarıyla eklendi");
                    }
                    else
                    {
                        MessageHelper.ShowError("Ürün eklenirken hata oluştu.");
                    }
                }
            }
        }
    }
}
