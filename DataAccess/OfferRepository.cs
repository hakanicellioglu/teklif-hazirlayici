using System;
using System.Data;
using System.Data.SqlClient;

namespace Teklif_Hazırlayıcı.DataAccess
{
    /// <summary>
    /// Provides database operations for offers.
    /// </summary>
    public class OfferRepository
    {
        private readonly SqlDbConnection _connection;

        public OfferRepository()
        {
            _connection = new SqlDbConnection();
        }

        /// <summary>
        /// Inserts a new offer and returns the generated identifier.
        /// </summary>
        public int InsertOffer(int firmaId, int yetkiliId, DateTime teklifTarih, string teslimSekli,
            string odemeSekli, int odemeVade, int teklifSure, string dovizKuru, char dovizBirimi,
            string vade, float vadeFarki, decimal lme, decimal iscilik, decimal iskontoOrani,
            decimal kdvOrani, bool tevkifat, string durum)
        {
            string query = "INSERT INTO teklifler (firma_id, yetkili_id, teklif_tarih, teslim_sekli, odeme_sekli, odeme_vade, teklif_sure, doviz_kuru, doviz_birimi, vade, vade_farki, lme, iscilik, iskonto_orani, kdv_orani, tevkifat, tevkifat_orani, durum) " +
                           "VALUES (@CompanyId, @AuthorizedPersonId, @OfferDate, @DeliveryMethod, @PaymentMethod, @PaymentDue, @OfferValidity, @ExchangeRate, @CurrencyUnit, @Term, @TermRate, @Lme, @Workmanship, @DiscountRate, @VatRate, @Withholding, @WithholdingRate, @Status); SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int).Value = firmaId;
                    cmd.Parameters.Add("@AuthorizedPersonId", SqlDbType.Int).Value = yetkiliId;
                    cmd.Parameters.Add("@OfferDate", SqlDbType.Date).Value = teklifTarih;
                    cmd.Parameters.Add("@DeliveryMethod", SqlDbType.NVarChar).Value = teslimSekli;
                    cmd.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar).Value = odemeSekli;
                    cmd.Parameters.Add("@PaymentDue", SqlDbType.Int).Value = odemeVade;
                    cmd.Parameters.Add("@OfferValidity", SqlDbType.Int).Value = teklifSure;
                    cmd.Parameters.Add("@ExchangeRate", SqlDbType.Float).Value = dovizKuru;
                    cmd.Parameters.Add("@CurrencyUnit", SqlDbType.NVarChar).Value = dovizBirimi.ToString();
                    cmd.Parameters.Add("@Term", SqlDbType.NVarChar).Value = vade;
                    cmd.Parameters.Add("@TermRate", SqlDbType.Float).Value = vadeFarki;
                    cmd.Parameters.Add("@Lme", SqlDbType.Decimal).Value = lme;
                    cmd.Parameters.Add("@Workmanship", SqlDbType.Decimal).Value = iscilik;
                    cmd.Parameters.Add("@DiscountRate", SqlDbType.Decimal).Value = iskontoOrani;
                    cmd.Parameters.Add("@VatRate", SqlDbType.Decimal).Value = kdvOrani;
                    cmd.Parameters.Add("@Withholding", SqlDbType.Bit).Value = tevkifat;
                    cmd.Parameters.Add("@WithholdingRate", SqlDbType.Float).Value = tevkifat ? 70 : 0;
                    cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = durum;

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Updates an existing offer record.
        /// </summary>
        public void UpdateOffer(int teklifId, int firmaId, int yetkiliId, DateTime teklifTarih, string teslimSekli,
            string odemeSekli, int odemeVade, int teklifSure, string dovizKuru, char dovizBirimi,
            string vade, float vadeFarki, decimal lme, decimal iscilik, decimal iskontoOrani,
            decimal kdvOrani, bool tevkifat, string durum)
        {
            string updateQuery = @"UPDATE teklifler SET
                            firma_id = @FirmaId,
                            yetkili_id = @YetkiliId,
                            teklif_tarih = @TeklifTarih,
                            teslim_sekli = @TeslimSekli,
                            odeme_sekli = @OdemeSekli,
                            odeme_vade = @OdemeVadesi,
                            teklif_sure = @TeklifSuresi,
                            doviz_kuru = @DovizKuru,
                            doviz_birimi = @DovizBirimi,
                            vade = @Vade,
                            vade_farki = @TermRate,
                            lme = @Lme,
                            iscilik = @Workmanship,
                            iskonto_orani = @IskontoOrani,
                            kdv_orani = @KdvOrani,
                            tevkifat = @Tevkifat,
                            tevkifat_orani = @TevkifatOrani,
                            durum = @Durum
                        WHERE teklif_id = @TeklifId";

            using (SqlConnection conn = _connection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FirmaId", firmaId);
                    cmd.Parameters.AddWithValue("@YetkiliId", yetkiliId);
                    cmd.Parameters.AddWithValue("@TeklifTarih", teklifTarih);
                    cmd.Parameters.AddWithValue("@TeslimSekli", teslimSekli);
                    cmd.Parameters.AddWithValue("@OdemeSekli", odemeSekli);
                    cmd.Parameters.AddWithValue("@OdemeVadesi", odemeVade);
                    cmd.Parameters.AddWithValue("@TeklifSuresi", teklifSure);
                    cmd.Parameters.AddWithValue("@DovizKuru", dovizKuru);
                    cmd.Parameters.AddWithValue("@DovizBirimi", dovizBirimi.ToString());
                    cmd.Parameters.AddWithValue("@Vade", vade);
                    cmd.Parameters.AddWithValue("@TermRate", vadeFarki);
                    cmd.Parameters.AddWithValue("@Lme", lme);
                    cmd.Parameters.AddWithValue("@Workmanship", iscilik);
                    cmd.Parameters.AddWithValue("@IskontoOrani", iskontoOrani);
                    cmd.Parameters.AddWithValue("@KdvOrani", kdvOrani);
                    cmd.Parameters.AddWithValue("@Tevkifat", tevkifat);
                    cmd.Parameters.AddWithValue("@TevkifatOrani", tevkifat ? 70 : 0);
                    cmd.Parameters.AddWithValue("@Durum", durum);
                    cmd.Parameters.AddWithValue("@TeklifId", teklifId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
