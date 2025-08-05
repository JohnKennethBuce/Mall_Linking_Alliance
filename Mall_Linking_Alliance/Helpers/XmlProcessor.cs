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
        //Parsing Formats
        private static string FormatDate(string raw)
        {
            if (!string.IsNullOrWhiteSpace(raw))
            {
                raw = raw.Trim();
                Console.WriteLine($"[FormatDate] Trying to parse: '{raw}'");

                if (DateTime.TryParseExact(raw, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var dt))
                {
                    Console.WriteLine($"[FormatDate] Parsed OK: {dt:yyyy-MM-dd}");
                    return dt.ToString("yyyy-MM-dd");
                }

                Console.WriteLine($"⚠️ [FormatDate] Failed to parse: '{raw}'");
            }
            else
            {
                Console.WriteLine($"⚠️ [FormatDate] Input was null or whitespace.");
            }

            return string.Empty;
        }


        private static int ParseInt(string s)
        {
            return int.TryParse(s, out var val) ? val : 0;
        }

        public static long ParseLong(string value)
        {
            long.TryParse(value, out var result);
            return result;
        }

        private static decimal ParseDecimal(string s)
        {
            return decimal.TryParse(s, out var val) ? val : 0m;
        }

        public static bool Process(string xmlContent, string fileName, TblSettings settings)
        {
            XmlProcessingResult result = null;

            try
            {
                result = ProcessXml(xmlContent, fileName, settings);
            }
            catch (Exception ex)
            {
                result = new XmlProcessingResult
                {
                    Success = false,
                    HasErrors = true,
                    Message = $"Unhandled exception: {ex.Message}"
                };
            }

            string targetFolder = result.Success
                ? Path.Combine(settings.BrowseDb, "ProcessedFiles")
                : Path.Combine(settings.BrowseDb, "DeniedFiles");

            Directory.CreateDirectory(targetFolder);

            string destinationPath = Path.Combine(targetFolder, Path.GetFileName(fileName));

            if (File.Exists(destinationPath))
                File.Delete(destinationPath);

            File.Move(fileName, destinationPath);

            if (!result.Success || result.HasErrors)
            {
                WriteDeniedLog(destinationPath, result);  // log next to moved file
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
        private static XmlProcessingResult ProcessXml(string xmlContent, string fileName, TblSettings settings)
        {
            var result = new XmlProcessingResult();

            try
            {
                var sanitizedXml = SanitizeXmlEntities(xmlContent);
                var doc = XDocument.Parse(sanitizedXml); // <- use sanitized content

                string dbPath = settings.SaveDb;
                if (string.IsNullOrWhiteSpace(dbPath) || !File.Exists(dbPath))
                {
                    result.Success = false;
                    result.HasErrors = true;
                    result.Message = "SaveDb path is invalid or does not exist.";
                    return result;
                }

                // 🔷 ID / SETTINGS
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

                foreach (var master in doc.Descendants("master"))
                {
                    foreach (var prod in master.Elements("product"))
                    {
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
                }

                // 🔷 SALES
                var sales = doc.Root.Element("sales");
                if (sales != null)
                {
                    foreach (var trx in sales.Elements("trx"))
                    {
                        try
                        {
                            // Validate and extract ReceiptNo
                            string receiptNo = trx.Element("receiptno")?.Value?.Trim();
                            if (string.IsNullOrWhiteSpace(receiptNo))
                                throw new Exception("Missing or empty <receiptno> in <trx> block.");

                            if (DatabaseHelper.ReceiptExists(receiptNo, dbPath))
                                throw new Exception($"Duplicate ReceiptNo already exists in database: {receiptNo}");

                            // Extract and format date from parent <sales> tag
                            string rawDate = sales.Element("date")?.Value;
                            Console.WriteLine($"[DEBUG] Raw Date from <sales>: '{rawDate}'");

                            string formattedDate = FormatDate(rawDate);
                            Console.WriteLine($"[DEBUG] Formatted Date: '{formattedDate}'");

                            // Create TblSales object
                            var tblSales = new TblSales
                            {
                                ReceiptNo = receiptNo,
                                Date = formattedDate,
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
                                Posted = ParseLong(trx.Element("posted")?.Value),
                                Memo = trx.Element("memo")?.Value?.Trim() ?? "N/A",
                                Qty = ParseInt(trx.Element("qty")?.Value)
                            };

                            // Insert into database
                            DatabaseHelper.InsertSales(tblSales, dbPath);

                // 🔷 SALES LINE
                var lines = trx.Elements("line");
                            foreach (var line in lines)
                            {
                                var item = new TblSalesLine
                                {
                                    ReceiptNo = receiptNo,
                                    Sku = line.Element("sku")?.Value,
                                    Qty = ParseDecimal(line.Element("qty")?.Value),
                                    UnitPrice = ParseDecimal(line.Element("unitprice")?.Value),
                                    Disc = ParseDecimal(line.Element("disc")?.Value),
                                    Senior = ParseDecimal(line.Element("senior")?.Value),
                                    Pwd = ParseDecimal(line.Element("pwd")?.Value),
                                    Diplomat = ParseDecimal(line.Element("diplomat")?.Value),
                                    TaxType = ParseDecimal(line.Element("taxtype")?.Value),
                                    Tax = ParseDecimal(line.Element("tax")?.Value),
                                    Memo = line.Element("memo")?.Value,
                                    Total = ParseDecimal(line.Element("total")?.Value)
                                };

                                DatabaseHelper.InsertSalesLine(item, dbPath);

                                Console.WriteLine($"[SalesLine] Added line SKU={item.Sku}, Qty={item.Qty}, Total={item.Total}");
                            }
                        }
                        catch (Exception ex)
                        {
                            result.HasErrors = true;

                            string shortFileName = Path.GetFileName(fileName);
                            string trxId = trx.Element("receiptno")?.Value ?? "Unknown";

                            string errorDetails = $"❌ Error in <trx> with ReceiptNo [{trxId}] in file [{shortFileName}]: {ex.Message}\n{ex.StackTrace}";
                            Logger.Error(errorDetails, "Sales XML Processor", dbPath);

                            result.Message += $"{Environment.NewLine}- {errorDetails}";
                        }
                    }
                }

                // 🔚 Final result
                result.Success = !result.HasErrors;
                result.Message = result.HasErrors ? "One or more <trx> entries failed." : "Processed and saved to database successfully.";
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


        private static void WriteDeniedLog(string xmlFilePath, XmlProcessingResult result)
        {
            string fileName = Path.GetFileName(xmlFilePath);
            string logPath = Path.ChangeExtension(xmlFilePath, ".txt"); // Log goes beside moved XML

            // Write the .txt log with reason
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"🗓️ Date: {DateTime.Now}");
            sb.AppendLine($"📄 File: {fileName}");
            sb.AppendLine($"❌ Denied Reason:");
            sb.AppendLine($"  - {result.Message}");

            File.WriteAllText(logPath, sb.ToString());

            Logger.Warn($"File denied: {fileName} - {result.Message}", "XML Processor");
        }
    }
}