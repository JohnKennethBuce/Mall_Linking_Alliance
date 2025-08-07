using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Mall_Linking_Alliance.Helpers
{
    public static class EodXmlProcessor
    {
        private static string CompareField(string label, object checkerVal, object eodVal)
        {
            bool match = Equals(checkerVal?.ToString(), eodVal?.ToString());
            string status = match ? "✅" : "❌";
            return $"   - {label}: checker={checkerVal}, eod={eodVal} {status}";
        }

        public static string ProcessDeniedEodFiles(string deniedFolderPath, string dbPath)
        {
            if (!Directory.Exists(deniedFolderPath))
            {
                Logger.Warn($"❌ DeniedFiles folder not found: {deniedFolderPath}");
                return "NA";
            }

            var xmlFiles = Directory.GetFiles(deniedFolderPath, "*.xml");
            int insertedCount = 0;
            int skippedCount = 0;
            int totalVoidedSkipped = 0;
            StringBuilder logBuilder = new StringBuilder();

            foreach (var filePath in xmlFiles)
            {
                try
                {
                    string xmlContent = File.ReadAllText(filePath);
                    var doc = XDocument.Parse(xmlContent);
                    var root = doc.Root;
                    var docType = root?.Element("id")?.Element("doc")?.Value;

                    // Only process SALES_EOD XMLs
                    if (docType != "SALES_EOD") continue;

                    var trxList = root.Element("sales")?.Elements("trx")?.ToList();

                    if (trxList == null || trxList.Count < 2)
                    {
                        logBuilder.AppendLine($"⚠️ Skipped (less than 2 trx): {Path.GetFileName(filePath)}");
                        skippedCount++;
                        continue;
                    }

                    // Count voided transactions before inserting
                    int voidedCount = trxList.Count(trx => trx.Element("void")?.Value == "1");
                    totalVoidedSkipped += voidedCount;

                    // Generate TblEod object from <sales>
                    var eod = EodGenerator.GenerateFromXml(xmlContent);

                    if (eod == null)
                    {
                        logBuilder.AppendLine($"❌ Failed to generate EOD: {Path.GetFileName(filePath)}");
                        skippedCount++;
                        continue;
                    }

                    // Check if EOD already exists
                    if (DatabaseHelper.EodExists(eod.Date, eod.ZCounter ?? 0, dbPath))
                    {
                        logBuilder.AppendLine($"⚠️ Duplicate EOD skipped: Date={eod.Date}, ZCounter={eod.ZCounter}");
                        skippedCount++;
                        continue;
                    }

                    // Save to DB
                    DatabaseHelper.InsertEod(eod, dbPath);
                    insertedCount++;
                    logBuilder.AppendLine($"✅ Inserted EOD: {Path.GetFileName(filePath)}");

                    if (voidedCount > 0)
                    {
                        logBuilder.AppendLine($"⏩ Voided transactions skipped: {voidedCount}");
                    }
                }
                catch (Exception ex)
                {
                    logBuilder.AppendLine($"❌ Error processing file {Path.GetFileName(filePath)}: {ex.Message}");
                    skippedCount++;
                }
            }

            // 📌 Populate tblchecker from tbleod + tblsales
            DatabaseHelper.PopulateCheckerFromEodAndSales(dbPath);

            // 📌 Compare tblchecker vs tbleod and log mismatch
            logBuilder.AppendLine();
            logBuilder.AppendLine("🔍 EOD vs Checker Mismatch Report:");

            var mismatches = DatabaseHelper.GetEodCheckerMismatchData(dbPath);
            if (mismatches != null && mismatches.Rows.Count > 0)
            {
                logBuilder.AppendLine("❌ Mismatches found between tblchecker and tbleod:");
                logBuilder.AppendLine("-----------------------------------------------------");

                foreach (var row in mismatches.AsEnumerable())
                {
                    string date = row["date"].ToString();
                    logBuilder.AppendLine($"📅 Date: {date}");
                    logBuilder.AppendLine(CompareField("pwdcnt", row["checker_pwdcnt"], row["eod_pwdcnt"]));
                    logBuilder.AppendLine(CompareField("trxcnt", row["checker_trxcnt"], row["eod_trxcnt"]));
                    logBuilder.AppendLine(CompareField("cash", row["checker_cash"], row["eod_cash"]));
                    logBuilder.AppendLine(CompareField("receiptstart", row["checker_receiptstart"], row["eod_receiptstart"]));
                    logBuilder.AppendLine(CompareField("receiptend", row["checker_receiptend"], row["eod_receiptend"]));
                    logBuilder.AppendLine();
                }
            }
            else
            {
                logBuilder.AppendLine("✅ No mismatches found between tblchecker and tbleod.");
            }

            // 📌 Summary log
            logBuilder.AppendLine("\n📋 EOD Processing Summary:");
            logBuilder.AppendLine($"Inserted={insertedCount}, Skipped={skippedCount}, Total={xmlFiles.Length}");

            if (totalVoidedSkipped > 0)
                logBuilder.AppendLine($"⏩ Total voided transactions skipped: {totalVoidedSkipped}");

            // 📌 Write to .txt
            string summaryLogPath = Path.Combine(deniedFolderPath, $"EOD_Summary_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            File.WriteAllText(summaryLogPath, logBuilder.ToString());

            Logger.Info($"📋 EOD summary written to: {summaryLogPath}");
            return summaryLogPath;
        }
    }
}
