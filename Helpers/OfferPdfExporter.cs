using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teklif_Hazırlayıcı.Business;
using Teklif_Hazırlayıcı.Helpers;

namespace Teklif_Hazırlayıcı.Helpers
{
    public static class OfferPdfExporter
    {
        public static async Task ExportOfferToPdfAsync(int offerId, bool tevkifat)
        {
            try
            {
                OfferManager offerManager = new OfferManager();
                DataTable teklifDetay = await offerManager.GetOfferDetailByIdAsync(offerId);
                if (teklifDetay == null || teklifDetay.Rows.Count == 0)
                {
                    MessageBox.Show("Teklif verisi bulunamadı.");
                    return;
                }

                DataTable teklifGenel = await offerManager.GetOfferByIdAsync(offerId);
                decimal lmeExport = 0;
                if (teklifGenel != null && teklifGenel.Rows.Count > 0)
                {
                    decimal.TryParse(teklifGenel.Rows[0]["lme"].ToString(), NumberStyles.Any, new CultureInfo("tr-TR"), out lmeExport);
                }

                var row = teklifDetay.Rows[0];
                string firmaAdi = row["FirmaIsim"].ToString();
                string yetkiliAdi = row["YetkiliIsim"].ToString();
                string yetkiliSoyadi = row["YetkiliSoyisim"].ToString();
                string firmaAdres = row["FirmaAdres"].ToString();
                string yetkiliTelefon = row["YetkiliTelefon"].ToString();
                string yetkiliEmail = row["YetkiliEposta"].ToString();
                string teklifTarih = Convert.ToDateTime(row["teklif_tarih"]).ToString("dd.MM.yyyy");
                string teslimSekli = row["teslim_sekli"]?.ToString() ?? "-";
                string odemeSekli = row["odeme_sekli"]?.ToString() ?? "-";
                string odemeVadesi = row["odeme_vade"]?.ToString() ?? "-";
                string teklifSuresi = row["teklif_sure"]?.ToString() ?? "-";
                string dovizKuru = Convert.ToDecimal(row["doviz_kuru"], CultureInfo.InvariantCulture).ToString("N2", new CultureInfo("tr-TR"));
                string vade = row["vade"]?.ToString() ?? "-";
                int toplamAdet = Convert.ToInt32(row["toplam_adet"].ToString());
                decimal toplamKg = Convert.ToDecimal(row["toplam_kg"], CultureInfo.InvariantCulture);
                string toplamKgStr = toplamKg.ToString("N3", new CultureInfo("tr-TR"));
                decimal malHizmetTutari = Convert.ToDecimal(row["mal_hizmet_bedeli"], CultureInfo.InvariantCulture);
                string malHizmetTutariStr = malHizmetTutari.ToString("N2", new CultureInfo("tr-TR"));
                decimal iskontoOrani = Convert.ToDecimal(row["iskonto_orani"], CultureInfo.InvariantCulture);
                string iskontoOraniStr = iskontoOrani.ToString("N2", new CultureInfo("tr-TR"));
                decimal iskontoTutari = malHizmetTutari * iskontoOrani / 100;
                string iskontoTutariStr = iskontoTutari.ToString("N2", new CultureInfo("tr-TR"));
                decimal iskontoSonrasiTutar = malHizmetTutari - iskontoTutari;
                string iskontoSonrasiTutarStr = iskontoSonrasiTutar.ToString("N2", new CultureInfo("tr-TR"));
                decimal kdv = iskontoSonrasiTutar * 0.20m;
                string kdvStr = kdv.ToString("N2", new CultureInfo("tr-TR"));
                decimal toplamAluminyumTutari = offerManager.GetToplamAluminyumTutari(offerId);
                decimal kdvaluminyum = toplamAluminyumTutari * 0.20m;
                string kdvaluminyumStr = kdvaluminyum.ToString("N2", new CultureInfo("tr-TR"));
                decimal tevkifatTutar = 0;
                if (tevkifat)
                {
                    tevkifatTutar = kdvaluminyum * 0.70m;
                }
                string tevkifatStr = tevkifatTutar.ToString("N2", new CultureInfo("tr-TR"));
                decimal vergiliToplam = iskontoSonrasiTutar + kdv;
                string vergiliToplamStr = vergiliToplam.ToString("N2", new CultureInfo("tr-TR"));
                decimal odenecekTutar = vergiliToplam - tevkifatTutar;
                string odenecekTutarStr = odenecekTutar.ToString("N2", new CultureInfo("tr-TR"));
                char doviz_birimi = row["doviz_birimi"] != DBNull.Value ? Convert.ToChar(row["doviz_birimi"]) : '₺';

                SaveFileDialog saveFile = new SaveFileDialog
                {
                    Filter = "PDF dosyası (*.pdf)|*.pdf",
                    FileName = $"Teklif_{offerId}.pdf"
                };
                if (saveFile.ShowDialog() != DialogResult.OK)
                    return;

                BaseFont baseFont;
                try
                {
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    if (!File.Exists(fontPath))
                        throw new FileNotFoundException("Arial font bulunamadı.", fontPath);
                    baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                catch (Exception ex)
                {
                    Logger.Log("Yazı tipi yükleme hatası: " + ex.Message);
                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
                }

                var normalFont = new iTextSharp.text.Font(baseFont, 5);
                var titleFont = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD);
                var smallFont = new iTextSharp.text.Font(baseFont, 5);

                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, new FileStream(saveFile.FileName, FileMode.Create));
                doc.Open();

                string logoPath = Path.Combine(Application.StartupPath, "Forms", "Resources", "logo.jpeg");
                if (File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(150f, 150f);
                    logo.Alignment = Element.ALIGN_LEFT;
                    logo.SpacingAfter = 10f;
                    doc.Add(logo);
                }
                if (!File.Exists(logoPath))
                {
                    MessageBox.Show("Logo bulunamadı: " + logoPath);
                }

                doc.Add(new Paragraph("TEKLİF FORMU", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 15
                });

                UserManager userManager = new UserManager();
                string hazirlayanAdSoyad = userManager.GetUserFullName(TeklifHazirlayici.Properties.Settings.Default.kullanici_id);

                PdfPTable ustBilgiTable = new PdfPTable(2);
                ustBilgiTable.WidthPercentage = 100;
                ustBilgiTable.SetWidths(new float[] { 70f, 30f });
                Paragraph solParagraf = new Paragraph();
                solParagraf.Add(new Chunk($"Firma Adı  : {firmaAdi}\n", normalFont));
                solParagraf.Add(new Chunk($"Yetkili    : {yetkiliAdi + " " + yetkiliSoyadi}\n", normalFont));
                solParagraf.Add(new Chunk($"Telefon    : {yetkiliTelefon}\n", normalFont));
                solParagraf.Add(new Chunk($"E-Mail     : {yetkiliEmail}\n", normalFont));
                solParagraf.Add(new Chunk($"Adres      : {firmaAdres}\n", normalFont));
                PdfPCell solCell2 = new PdfPCell(solParagraf)
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_TOP
                };
                ustBilgiTable.AddCell(solCell2);

                PdfPTable sagIciTable = new PdfPTable(2);
                sagIciTable.WidthPercentage = 100;
                sagIciTable.SetWidths(new float[] { 10f, 20f });
                sagIciTable.AddCell(CreateLeftCell("Teklif No:", smallFont));
                sagIciTable.AddCell(CreateRightCell(offerId.ToString("D6"), smallFont));
                sagIciTable.AddCell(CreateLeftCell("Hazırlayan:", smallFont));
                sagIciTable.AddCell(CreateRightCell(hazirlayanAdSoyad, smallFont));
                sagIciTable.AddCell(CreateLeftCell("E-Mail:", smallFont));
                sagIciTable.AddCell(CreateRightCell("siparis@alumannaluminyum.com.tr", smallFont));
                sagIciTable.AddCell(CreateLeftCell("Tarih:", smallFont));
                sagIciTable.AddCell(CreateRightCell(DateTime.Now.ToString("dd.MM.yyyy"), smallFont));
                PdfPCell sagCell2 = new PdfPCell(sagIciTable)
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_TOP
                };
                ustBilgiTable.AddCell(sagCell2);
                doc.Add(ustBilgiTable);
                doc.Add(new Paragraph(" ") { SpacingAfter = 2f });

                var kalemler = offerManager.GetTeklifKalemleri(offerId);
                PdfPTable table = new PdfPTable(10)
                {
                    WidthPercentage = 100
                };
                table.SetWidths(new float[] { 4, 9, 25, 10, 11, 8, 8, 8, 12, 12 });
                table.KeepTogether = true;
                string[] headers = { "NO", "KOD", "ÜRÜN", "YÜZEY", "YÜZEY KODU", "BOY", "ADET", "KG", "BİRİM FİYAT", "TOPLAM TUTAR" };
                foreach (var h in headers)
                {
                    var cell = new PdfPCell(new Phrase(h, smallFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }
                int sira = 1;
                foreach (DataRow kalem in kalemler.Rows)
                {
                    table.AddCell(CreateCell(sira.ToString(), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(kalem["kalip_no"].ToString(), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(kalem["urun"].ToString(), smallFont, Element.ALIGN_LEFT));
                    table.AddCell(CreateCell(kalem["yuzey"].ToString(), smallFont, Element.ALIGN_CENTER));
                    table.AddCell(CreateCell(kalem["yuzey_kodu"].ToString(), smallFont, Element.ALIGN_CENTER));
                    table.AddCell(CreateCell(kalem["boy"].ToString(), smallFont, Element.ALIGN_CENTER));
                    table.AddCell(CreateCell(kalem["adet"].ToString(), smallFont, Element.ALIGN_CENTER));
                    table.AddCell(CreateCell(Math.Round(Convert.ToDecimal(kalem["kg"]), 3).ToString("N3", new CultureInfo("tr-TR")), smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(FormatDecimalTr(ParseDecimalTr(kalem["birim_fiyat"].ToString())) + " " + doviz_birimi, smallFont, Element.ALIGN_RIGHT));
                    table.AddCell(CreateCell(Math.Round(Convert.ToDecimal(kalem["toplam_tutar"]), 2).ToString("N2", new CultureInfo("tr-TR")) + " " + doviz_birimi, smallFont, Element.ALIGN_RIGHT));
                    sira++;
                }
                doc.Add(table);

                PdfPTable toplamTable = new PdfPTable(2)
                {
                    WidthPercentage = 40,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                };
                toplamTable.SetWidths(new float[] { 60, 40 });
                PdfPTable spaceTable = new PdfPTable(1);
                PdfPCell emptyCell = new PdfPCell(new Phrase(""))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    FixedHeight = 200f
                };
                spaceTable.AddCell(emptyCell);
                doc.Add(spaceTable);

                PdfPTable teslimBilgiTable = new PdfPTable(2);
                teslimBilgiTable.KeepTogether = true;
                teslimBilgiTable.WidthPercentage = 40;
                teslimBilgiTable.SetWidths(new float[] { 50, 50 });
                teslimBilgiTable.AddCell(CreateCell("TESLİM ŞEKLİ", smallFont));
                teslimBilgiTable.AddCell(CreateCell(teslimSekli, smallFont));
                teslimBilgiTable.AddCell(CreateCell("ÖDEME ŞEKLİ ve VADESİ", smallFont));
                teslimBilgiTable.AddCell(CreateCell($"{odemeSekli} / {odemeVadesi} gün", smallFont));
                teslimBilgiTable.AddCell(CreateCell("TEKLİF GEÇERLİLİK SÜRESİ", smallFont));
                teslimBilgiTable.AddCell(CreateCell(teklifSuresi + " gün", smallFont));
                teslimBilgiTable.AddCell(CreateCell("DÖVİZ KURU (Merkez Bankası)", smallFont));
                teslimBilgiTable.AddCell(CreateCell(dovizKuru, smallFont));
                teslimBilgiTable.AddCell(CreateCell("VADE", smallFont));
                teslimBilgiTable.AddCell(CreateCell(vade, smallFont));
                teslimBilgiTable.AddCell(CreateCell("LME (" + doviz_birimi + "/ton)", smallFont));
                teslimBilgiTable.AddCell(CreateCell(lmeExport.ToString("N2", new CultureInfo("tr-TR")), smallFont));
                teslimBilgiTable.HorizontalAlignment = Element.ALIGN_LEFT;

                string[,] toplamlar = {
                    { "TOPLAM ADET", toplamAdet.ToString() },
                    { "TOPLAM KG", toplamKgStr },
                    { "MAL ve HİZMET TUTARI", malHizmetTutariStr + " " + doviz_birimi },
                    { $"HESAPLANAN İSKONTO - %{iskontoOrani}", iskontoTutariStr + " " + doviz_birimi },
                    { "İSKONTOLU TUTAR", iskontoSonrasiTutarStr + " " + doviz_birimi },
                    { "HESAPLANAN KDV", kdvStr + " " + doviz_birimi },
                    { "TEVKİFAT (bakır, çinko ve alüminyum ürünlerinin teslimi %70)", tevkifatStr + " " + doviz_birimi },
                    { "VERGİLER DAHİL GENEL TOPLAM", vergiliToplamStr + " " + doviz_birimi },
                    { "ÖDENECEK TUTAR", odenecekTutarStr + " " + doviz_birimi }
                };
                for (int i = 0; i < toplamlar.GetLength(0); i++)
                {
                    PdfPCell solCell = new PdfPCell(new Phrase(toplamlar[i, 0], smallFont))
                    {
                        Border = iTextSharp.text.Rectangle.BOX,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };
                    toplamTable.AddCell(solCell);
                    PdfPCell sagCell = new PdfPCell(new Phrase(toplamlar[i, 1], smallFont))
                    {
                        Border = iTextSharp.text.Rectangle.BOX,
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                    };
                    toplamTable.AddCell(sagCell);
                }

                PdfPTable yanYanaTable = new PdfPTable(2);
                yanYanaTable.WidthPercentage = 100;
                yanYanaTable.SetWidths(new float[] { 50, 50 });
                PdfPCell teslimCell = new PdfPCell(teslimBilgiTable)
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    PaddingRight = 10f,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                PdfPCell toplamCell = new PdfPCell(toplamTable)
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    PaddingLeft = 10f,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                yanYanaTable.AddCell(teslimCell);
                yanYanaTable.AddCell(toplamCell);
                doc.Add(yanYanaTable);

                PdfPTable aciklamaTable = new PdfPTable(1);
                aciklamaTable.WidthPercentage = 100;
                aciklamaTable.KeepTogether = true;
                PdfPCell aciklamaBaslik = new PdfPCell(new Phrase("AÇIKLAMALAR", smallFont));
                aciklamaBaslik.BackgroundColor = BaseColor.LIGHT_GRAY;
                aciklamaBaslik.HorizontalAlignment = Element.ALIGN_CENTER;
                aciklamaBaslik.Border = iTextSharp.text.Rectangle.NO_BORDER;
                aciklamaBaslik.FixedHeight = 12f;
                aciklamaTable.AddCell(aciklamaBaslik);
                aciklamaTable.AddCell(CreateCell("• ÖDEMESİ YAPILMAMIŞ SİPARİŞLER, SEVK TARİHİNDEKİ FİYATTAN FATURA EDİLİR.", smallFont));
                aciklamaTable.AddCell(CreateCell("• VADELİ ÖDEMELERDE  %5 FİYAT FARKI EKLENECEKTİR.", smallFont));
                aciklamaTable.AddCell(CreateCell("• SİPARİŞLERDE FATURA KANTARDAKİ KG ÜZERİNDEN DÜZENLENİR. TEKLİFTEKİ KG BİLGİLERİ KATALOG BİLGİLERİ OLUP GERÇEK MİKTAR İLE FARKLILIK GÖSTEREBİLİR.", smallFont));
                aciklamaTable.AddCell(CreateCell("• ÖZEL BOY TÜM ÜRÜNLERDE  ±%10 ÜRETİLEBİLİR. BU DURUMDA ÜRETİLEN MAL MÜŞTERİYE SEVK EDİLİR.", smallFont));
                aciklamaTable.AddCell(CreateCell("• SİPARİŞLER MÜŞTERİ TARAFINDAN KONTROL EDİLİP ONAYLANDIKTAN SONRA PLANLAMAYA ALINIR.", smallFont));
                aciklamaTable.AddCell(CreateCell("• SİPARİŞLERDE NAKLİYE ÜCRETİ MÜŞTERİYE AİTTİR.", smallFont));
                doc.Add(aciklamaTable);

                PdfPTable bankaTable = new PdfPTable(3);
                bankaTable.WidthPercentage = 100;
                bankaTable.SetWidths(new float[] { 30, 40, 30 });
                PdfPCell bankaBaslik = new PdfPCell(new Phrase("BANKA HESAP BİLGİLERİ", smallFont));
                bankaBaslik.BackgroundColor = BaseColor.LIGHT_GRAY;
                bankaBaslik.Colspan = 3;
                bankaBaslik.HorizontalAlignment = Element.ALIGN_CENTER;
                bankaBaslik.Border = iTextSharp.text.Rectangle.NO_BORDER;
                bankaBaslik.FixedHeight = 12f;
                bankaTable.AddCell(bankaBaslik);
                bankaTable.AddCell(CreateCell("VAKIFBANK", smallFont));
                bankaTable.AddCell(CreateCell("TR44 0001 5001 5800 7321 3983 24", smallFont));
                bankaTable.AddCell(CreateCell("Alumann Alüminyum Sanayi ve Ticaret A.Ş", smallFont));
                bankaTable.AddCell(CreateCell("ALBARAKA", smallFont));
                bankaTable.AddCell(CreateCell("TR33 0020 3000 0956 2368 0000 01", smallFont));
                bankaTable.AddCell(CreateCell("Alumann Alüminyum Sanayi ve Ticaret A.Ş", smallFont));
                bankaTable.AddCell(CreateCell("VAKIF KATILIM", smallFont));
                bankaTable.AddCell(CreateCell("TR55 0021 0000 0008 3591 5000 01", smallFont));
                bankaTable.AddCell(CreateCell("Alumann Alüminyum Sanayi ve Ticaret A.Ş", smallFont));
                doc.Add(bankaTable);
                doc.Add(new Paragraph(" ") { SpacingBefore = 5f, SpacingAfter = 5f });

                PdfPTable onayTable = new PdfPTable(2);
                onayTable.WidthPercentage = 100;
                onayTable.KeepTogether = true;
                onayTable.SetWidths(new float[] { 50, 50 });
                PdfPCell tedarikciCell = new PdfPCell(new Phrase("", normalFont));
                tedarikciCell.Border = iTextSharp.text.Rectangle.BOX;
                tedarikciCell.FixedHeight = 40;
                PdfPCell tedarikciHeader = new PdfPCell(new Phrase("TEDARİKÇİ ONAYI", smallFont));
                tedarikciHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                tedarikciHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tedarikciHeader.Colspan = 1;
                tedarikciCell.HorizontalAlignment = Element.ALIGN_LEFT;
                tedarikciCell.Border = iTextSharp.text.Rectangle.BOX;
                tedarikciCell.FixedHeight = 40;
                PdfPCell musteriCell = new PdfPCell(new Phrase("", normalFont));
                musteriCell.Border = iTextSharp.text.Rectangle.BOX;
                musteriCell.FixedHeight = 40;
                PdfPCell musteriHeader = new PdfPCell(new Phrase("MÜŞTERİ ONAYI", smallFont));
                musteriHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                musteriHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                musteriHeader.Colspan = 1;
                musteriCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                musteriCell.Border = iTextSharp.text.Rectangle.BOX;
                musteriCell.FixedHeight = 40;
                onayTable.AddCell(tedarikciHeader);
                onayTable.AddCell(musteriHeader);
                onayTable.AddCell(tedarikciCell);
                onayTable.AddCell(musteriCell);
                doc.Add(onayTable);
                doc.Close();
                MessageBox.Show("PDF başarıyla oluşturuldu.", "PDF Çıktısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex.Message);
            }
        }

        private static PdfPCell CreateCell(string text, iTextSharp.text.Font font, int alignment = Element.ALIGN_LEFT)
        {
            return new PdfPCell(new Phrase(text, font))
            {
                Border = iTextSharp.text.Rectangle.NO_BORDER,
                HorizontalAlignment = alignment
            };
        }

        private static decimal ParseDecimalTr(string value)
        {
            if (decimal.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            return 0;
        }

        private static string FormatDecimalTr(decimal value, int precision = 2)
        {
            return value.ToString($"N{precision}", new CultureInfo("tr-TR"));
        }
    }
}
