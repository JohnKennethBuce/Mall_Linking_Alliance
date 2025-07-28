using Mall_Linking_Alliance.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Mall_Linking_Alliance.Helpers
{
    public static class XmlProcessor
    {
        public static bool Process(string xmlContent, string fileName, TblSettings settings)
        {
            var result = ProcessXml(xmlContent, fileName, settings);

            string targetFolder = result.Success
                ? Path.Combine(settings.BrowseDb, "ProcessedFiles")
                : Path.Combine(settings.BrowseDb, "DeniedFiles");

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            string destinationPath = Path.Combine(targetFolder, Path.GetFileName(fileName));

            if (File.Exists(destinationPath))
                File.Delete(destinationPath);

            File.Move(fileName, destinationPath);

            if (!result.Success || result.HasErrors)
            {
                WriteDeniedLog(destinationPath, result);
                Logger.Error($"❌ {result.Message}", "XmlProcessor");
                return false;
            }

            Logger.Info($"✅ Processed: {Path.GetFileName(fileName)}", "XmlProcessor");
            return true;
        }
        private static string SanitizeXmlEntities(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return xml;

            // Replace raw ampersands not part of valid entity references
            return System.Text.RegularExpressions.Regex.Replace(
                xml,
                @"&(?![a-zA-Z]+;|#\d+;)", // matches & not followed by valid entity
                "&amp;"
            );
        }
        private static string GetValueOrNA(XElement element)
        {
            return string.IsNullOrWhiteSpace(element?.Value) ? "NA" : element.Value;
        }
        private static XmlProcessingResult ProcessXml(string xmlContent, string fileName, TblSettings settings)
        {
            var result = new XmlProcessingResult();

            try
            {
                var sanitizedXml = SanitizeXmlEntities(xmlContent);
                var doc = XDocument.Parse(xmlContent);

                string dbPath = settings.SaveDb;
                if (string.IsNullOrWhiteSpace(dbPath) || !File.Exists(dbPath))
                {
                    result.Success = false;
                    result.HasErrors = true;
                    result.Message = "SaveDb path is invalid or does not exist.";
                    return result;
                }

                // 🔷 ID
                var id = doc.Root.Element("id");
                var tblSettings = new TblSettings
                {
                    TenantId = (int?)ParseInt(id.Element("tenantid")?.Value),
                    TenantKey = id.Element("key")?.Value,
                    TmId = (int?)ParseInt(id.Element("tmid")?.Value),
                    Doc = id.Element("doc")?.Value,
                    BrowseDb = settings.BrowseDb,
                    SaveDb = settings.SaveDb
                };
                DatabaseHelper.InsertSettings(tblSettings, dbPath);

                // 🔷 SALES + SALES LINE

                var sales = doc.Root.Element("sales");
                if (sales != null)
                {
                    foreach (var trx in sales.Elements("trx"))
                    {
                        var tblSales = new TblSales
                        {
                            ReceiptNo = trx.Element("receiptno")?.Value,
                            Date = trx.Element("date")?.Value,
                            Void = ParseInt(trx.Element("void")?.Value),
                            Cash = ParseDecimal(trx.Element("cash")?.Value),
                            Credit = ParseDecimal(trx.Element("credit")?.Value),
                            Charge = ParseDecimal(trx.Element("charge")?.Value),
                            GiftCheck = ParseDecimal(trx.Element("giftcheck")?.Value),
                            OtherTender = ParseDecimal(trx.Element("othertender")?.Value),
                            LineDisc = ParseDecimal(trx.Element("linedisc")?.Value),
                            LineSenior = ParseDecimal(trx.Element("linesenior")?.Value),
                            Evat = ParseDecimal(trx.Element("evat")?.Value),
                            LinePwd = ParseDecimal(trx.Element("linepwd")?.Value),
                            LineDiplomat = ParseDecimal(trx.Element("linediplomat")?.Value),
                            Subtotal = ParseDecimal(trx.Element("subtotal")?.Value),
                            Disc = ParseDecimal(trx.Element("disc")?.Value),
                            Senior = ParseDecimal(trx.Element("senior")?.Value),
                            Pwd = ParseDecimal(trx.Element("pwd")?.Value),
                            Diplomat = ParseDecimal(trx.Element("diplomat")?.Value),
                            Vat = ParseDecimal(trx.Element("vat")?.Value),
                            ExVat = ParseDecimal(trx.Element("exvat")?.Value),
                            IncVat = ParseDecimal(trx.Element("incvat")?.Value),
                            LocalTax = ParseDecimal(trx.Element("localtax")?.Value),
                            Amusement = ParseDecimal(trx.Element("amusement")?.Value),
                            Ewt = ParseDecimal(trx.Element("ewt")?.Value),
                            Service = ParseDecimal(trx.Element("service")?.Value),
                            TaxSale = ParseDecimal(trx.Element("taxsale")?.Value),
                            NoTaxSale = ParseDecimal(trx.Element("notaxsale")?.Value),
                            TaxExSale = ParseDecimal(trx.Element("taxexsale")?.Value),
                            TaxIncSale = ParseDecimal(trx.Element("taxincsale")?.Value),
                            ZeroSale = ParseDecimal(trx.Element("zerosale")?.Value),
                            VatExempt = ParseDecimal(trx.Element("vatexempt")?.Value),
                            CustomerCount = ParseInt(trx.Element("customercount")?.Value),
                            Gross = ParseDecimal(trx.Element("gross")?.Value),
                            TaxRate = ParseDecimal(trx.Element("taxrate")?.Value),
                            Posted = ParseInt(trx.Element("posted")?.Value),
                            Memo = GetValueOrNA(trx.Element("memo")),
                            Qty = ParseInt(trx.Element("qty")?.Value)
                        };
                        DatabaseHelper.InsertSales(tblSales, dbPath);

                        var line = trx.Element("line");
                        if (line != null)
                        {
                            var lineItems = ParseSalesLines(line, tblSales.ReceiptNo);
                            foreach (var item in lineItems)
                            {
                                DatabaseHelper.InsertSalesLine(item, dbPath);
                            }
                        }
                    }
                }

                // 🔷 MASTER
                foreach (var master in doc.Descendants("master"))
                {
                    var prod = master.Element("product");
                    var tblMaster = new TblMaster
                    {
                        Sku = prod.Element("sku")?.Value,
                        Name = prod.Element("name")?.Value,
                        Inventory = ParseInt(prod.Element("inventory")?.Value),
                        Price = ParseDecimal(prod.Element("price")?.Value),
                        Category = prod.Element("category")?.Value
                    };
                    DatabaseHelper.InsertMaster(tblMaster, dbPath);
                }

                // 🔷 EOD
                var eod = doc.Root.Element("eod");
                if (eod != null)
                {
                    var tblEod = new TblEod
                    {
                        Date = eod.Element("date")?.Value,
                        ZCounter = ParseInt(eod.Element("zcounter")?.Value),
                        PreviousNrgt = ParseDecimal(eod.Element("previousnrgt")?.Value),
                        Nrgt = ParseDecimal(eod.Element("nrgt")?.Value),
                        PreviousTax = ParseDecimal(eod.Element("previoustax")?.Value),
                        NewTax = ParseDecimal(eod.Element("newtax")?.Value),
                        PreviousTaxSale = ParseDecimal(eod.Element("previoustaxsale")?.Value),
                        NewTaxSale = ParseDecimal(eod.Element("newtaxsale")?.Value),
                        PreviousNoTaxSale = ParseDecimal(eod.Element("previousnotaxsale")?.Value),
                        NewNoTaxSale = ParseDecimal(eod.Element("newnotaxsale")?.Value),
                        OpenTime = ParseInt(eod.Element("opentime")?.Value) ?? 0,
                        CloseTime = ParseInt(eod.Element("closetime")?.Value) ?? 0,
                        Gross = ParseDecimal(eod.Element("gross")?.Value),
                        Vat = ParseDecimal(eod.Element("vat")?.Value),
                        LocalTax = ParseDecimal(eod.Element("localtax")?.Value),
                        Amusement = ParseDecimal(eod.Element("amusement")?.Value),
                        Ewt = ParseDecimal(eod.Element("ewt")?.Value),
                        TaxSale = ParseDecimal(eod.Element("taxsale")?.Value),
                        NoTaxSale = ParseDecimal(eod.Element("notaxsale")?.Value),
                        ZeroSale = ParseDecimal(eod.Element("zerosale")?.Value),
                        VatExempt = ParseDecimal(eod.Element("vatexempt")?.Value),
                        Void = ParseDecimal(eod.Element("void")?.Value),
                        VoidCnt = ParseInt(eod.Element("voidcnt")?.Value),
                        Disc = ParseDecimal(eod.Element("disc")?.Value),
                        DiscCnt = ParseInt(eod.Element("disccnt")?.Value),
                        Refund = ParseDecimal(eod.Element("refund")?.Value),
                        RefundCnt = ParseInt(eod.Element("refundcnt")?.Value),
                        Senior = ParseDecimal(eod.Element("senior")?.Value),
                        SeniorCnt = ParseInt(eod.Element("seniorcnt")?.Value),
                        Pwd = ParseDecimal(eod.Element("pwd")?.Value),
                        PwdCnt = ParseInt(eod.Element("pwdcnt")?.Value),
                        Diplomat = ParseDecimal(eod.Element("diplomat")?.Value),
                        DiplomatCnt = ParseInt(eod.Element("diplomatcnt")?.Value),
                        Service = ParseDecimal(eod.Element("service")?.Value),
                        ServiceCnt = ParseInt(eod.Element("servicecnt")?.Value),
                        ReceiptStart = eod.Element("receiptstart")?.Value,
                        ReceiptEnd = eod.Element("receiptend")?.Value,
                        TrxCnt = ParseInt(eod.Element("trxcnt")?.Value),
                        Cash = ParseDecimal(eod.Element("cash")?.Value),
                        CashCnt = ParseInt(eod.Element("cashcnt")?.Value),
                        Credit = ParseDecimal(eod.Element("credit")?.Value),
                        CreditCnt = ParseInt(eod.Element("creditcnt")?.Value),
                        Charge = ParseDecimal(eod.Element("charge")?.Value),
                        ChargeCnt = ParseInt(eod.Element("chargecnt")?.Value),
                        GiftCheck = ParseDecimal(eod.Element("giftcheck")?.Value),
                        GiftCheckCnt = ParseInt(eod.Element("giftcheckcnt")?.Value),
                        OtherTender = ParseDecimal(eod.Element("othertender")?.Value),
                        OtherTenderCnt = ParseInt(eod.Element("othertendercnt")?.Value)
                    };

                    DatabaseHelper.InsertEod(tblEod, dbPath); // You’ll implement this next
                }


                result.Message = "Processed and saved to database successfully.";
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.HasErrors = true;
                result.Message = $"Exception: {ex.Message}";
                return result;
            }


        }

        private static List<TblSalesLine> ParseSalesLines(XElement line, string receiptNo)
        {
            var children = line.Elements().ToList();
            var result = new List<TblSalesLine>();

            const int FieldsPerItem = 11;

            if (children.Count % FieldsPerItem != 0)
            {
                Logger.Error($"Invalid number of <salesline> child elements: Expected multiples of {FieldsPerItem}, but got {children.Count}.");
                return result;
            }

            for (int i = 0; i < children.Count; i += FieldsPerItem)
            {
                try
                {
                    var item = new TblSalesLine
                    {
                        ReceiptNo = receiptNo,
                        Sku = children[i]?.Value,
                        Qty = ParseDecimal(children[i + 1]?.Value),
                        UnitPrice = ParseDecimal(children[i + 2]?.Value),
                        Disc = ParseDecimal(children[i + 3]?.Value),
                        Senior = ParseDecimal(children[i + 4]?.Value),
                        Pwd = ParseDecimal(children[i + 5]?.Value),
                        Diplomat = ParseDecimal(children[i + 6]?.Value),
                        TaxType = ParseDecimal(children[i + 7]?.Value),
                        Tax = ParseDecimal(children[i + 8]?.Value),
                        Memo = children[i + 9]?.Value,
                        Total = ParseDecimal(children[i + 10]?.Value)
                    };
                    result.Add(item);
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error parsing salesline block at index {i}: {ex.Message}");
                }
            }

            return result;
        }


        private static void WriteDeniedLog(string deniedFilePath, XmlProcessingResult result)
        {
            string logPath = Path.ChangeExtension(deniedFilePath, ".txt");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"🗓️ Date: {DateTime.Now}");
            sb.AppendLine($"📄 File: {Path.GetFileName(deniedFilePath)}");
            sb.AppendLine($"❌ Reason: {result.Message}");

            File.WriteAllText(logPath, sb.ToString());
        }

        private static int? ParseInt(string s)
        {
            if (int.TryParse(s, out var val))
                return val;
            return null;
        }

        private static decimal? ParseDecimal(string s)
        {
            if (decimal.TryParse(s, out var val))
                return val;
            return null;
        }
    }
}
