﻿using Mall_Linking_Alliance.Model;
using System;
using System.Data.SQLite;

namespace Mall_Linking_Alliance.Helpers
{
    public static class DatabaseHelper
    {
        private static string GetConnectionString(string dbPath)
        {
            return $"Data Source={dbPath};Version=3;";
        }

        public static void InsertLog(TblLog log, string dbPath)
        {
            try
            {
                using (var conn = new SQLiteConnection(GetConnectionString(dbPath)))
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO tbllogs (LogDate, LogLevel, Source, Message) 
                        VALUES (@LogDate, @LogLevel, @Source, @Message);";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@LogDate", log.LogDate);
                        cmd.Parameters.AddWithValue("@LogLevel", log.LogLevel);
                        cmd.Parameters.AddWithValue("@Source", log.Source);
                        cmd.Parameters.AddWithValue("@Message", log.Message);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to save log: {ex.Message}");
            }
        }

        public static void InsertSettings(TblSettings s, string dbPath)
        {
            try
            {
                using (var conn = new SQLiteConnection(GetConnectionString(dbPath)))
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO tblsettings (tenantid, tenantkey, tmid, doc, browsedb, savedb) 
                        VALUES (@tenantid, @tenantkey, @tmid, @doc, @browsedb, @savedb);";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@tenantid", s.TenantId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tenantkey", s.TenantKey ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tmid", s.TmId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@doc", s.Doc ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@browsedb", s.BrowseDb ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@savedb", s.SaveDb ?? (object)DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to save settings: {ex.Message}");
            }
        }

        public static void InsertSales(TblSales s, string dbPath)
        {
            try
            {
                using (var conn = new SQLiteConnection(GetConnectionString(dbPath)))
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO tblsales (
                            receiptno, date, void, cash, credit, charge, giftcheck, othertender,
                            linedisc, linesenior, linepwd, evat, linediplomat, subtotal, disc, senior, pwd,
                            diplomat, vat, exvat, incvat, localtax, amusement, ewt, service, taxsale, notaxsale,
                            taxexsale, taxincsale, zerosale, vatexempt, customercount, gross, taxrate, posted,
                            memo, qty
                        ) VALUES (
                            @receiptno, @date, @void, @cash, @credit, @charge, @giftcheck, @othertender,
                            @linedisc, @linesenior, @linepwd, @evat, @linediplomat, @subtotal, @disc, @senior, @pwd,
                            @diplomat, @vat, @exvat, @incvat, @localtax, @amusement, @ewt, @service, @taxsale, @notaxsale,
                            @taxexsale, @taxincsale, @zerosale, @vatexempt, @customercount, @gross, @taxrate, @posted,
                            @memo, @qty
                        );";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@receiptno", s.ReceiptNo);
                        cmd.Parameters.AddWithValue("@date", s.Date);
                        cmd.Parameters.AddWithValue("@void", s.Void ?? 0);
                        cmd.Parameters.AddWithValue("@cash", s.Cash ?? 0);
                        cmd.Parameters.AddWithValue("@credit", s.Credit ?? 0);
                        cmd.Parameters.AddWithValue("@charge", s.Charge ?? 0);
                        cmd.Parameters.AddWithValue("@giftcheck", s.GiftCheck ?? 0);
                        cmd.Parameters.AddWithValue("@othertender", s.OtherTender ?? 0);
                        cmd.Parameters.AddWithValue("@linedisc", s.LineDisc ?? 0);
                        cmd.Parameters.AddWithValue("@linesenior", s.LineSenior ?? 0);
                        cmd.Parameters.AddWithValue("@linepwd", s.LinePwd ?? 0);
                        cmd.Parameters.AddWithValue("@evat", s.Evat ?? 0);
                        cmd.Parameters.AddWithValue("@linediplomat", s.LineDiplomat ?? 0);
                        cmd.Parameters.AddWithValue("@subtotal", s.Subtotal ?? 0);
                        cmd.Parameters.AddWithValue("@disc", s.Disc ?? 0);
                        cmd.Parameters.AddWithValue("@senior", s.Senior ?? 0);
                        cmd.Parameters.AddWithValue("@pwd", s.Pwd ?? 0);
                        cmd.Parameters.AddWithValue("@diplomat", s.Diplomat ?? 0);
                        cmd.Parameters.AddWithValue("@vat", s.Vat ?? 0);
                        cmd.Parameters.AddWithValue("@exvat", s.ExVat ?? 0);
                        cmd.Parameters.AddWithValue("@incvat", s.IncVat ?? 0);
                        cmd.Parameters.AddWithValue("@localtax", s.LocalTax ?? 0);
                        cmd.Parameters.AddWithValue("@amusement", s.Amusement ?? 0);
                        cmd.Parameters.AddWithValue("@ewt", s.Ewt ?? 0);
                        cmd.Parameters.AddWithValue("@service", s.Service ?? 0);
                        cmd.Parameters.AddWithValue("@taxsale", s.TaxSale ?? 0);
                        cmd.Parameters.AddWithValue("@notaxsale", s.NoTaxSale ?? 0);
                        cmd.Parameters.AddWithValue("@taxexsale", s.TaxExSale ?? 0);
                        cmd.Parameters.AddWithValue("@taxincsale", s.TaxIncSale ?? 0);
                        cmd.Parameters.AddWithValue("@zerosale", s.ZeroSale ?? 0);
                        cmd.Parameters.AddWithValue("@vatexempt", s.VatExempt ?? 0);
                        cmd.Parameters.AddWithValue("@customercount", s.CustomerCount ?? 0);
                        cmd.Parameters.AddWithValue("@gross", s.Gross ?? 0);
                        cmd.Parameters.AddWithValue("@taxrate", s.TaxRate ?? 0);
                        cmd.Parameters.AddWithValue("@posted", s.Posted ?? 0);
                        cmd.Parameters.AddWithValue("@memo", s.Memo ?? "");
                        cmd.Parameters.AddWithValue("@qty", s.Qty ?? 0);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to save sales: {ex.Message}");
            }
        }

        public static void InsertSalesLine(TblSalesLine l, string dbPath)
        {
            try
            {
                using (var conn = new SQLiteConnection(GetConnectionString(dbPath)))
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO tblsalesline (
                            receiptno, sku, qty, unitprice, disc, senior, pwd, diplomat,
                            taxtype, tax, memo, total
                        ) VALUES (
                            @receiptno, @sku, @qty, @unitprice, @disc, @senior, @pwd, @diplomat,
                            @taxtype, @tax, @memo, @total
                        );";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@receiptno", l.ReceiptNo);
                        cmd.Parameters.AddWithValue("@sku", l.Sku);
                        cmd.Parameters.AddWithValue("@qty", l.Qty ?? 0);
                        cmd.Parameters.AddWithValue("@unitprice", l.UnitPrice ?? 0);
                        cmd.Parameters.AddWithValue("@disc", l.Disc ?? 0);
                        cmd.Parameters.AddWithValue("@senior", l.Senior ?? 0);
                        cmd.Parameters.AddWithValue("@pwd", l.Pwd ?? 0);
                        cmd.Parameters.AddWithValue("@diplomat", l.Diplomat ?? 0);
                        cmd.Parameters.AddWithValue("@taxtype", l.TaxType ?? 0);
                        cmd.Parameters.AddWithValue("@tax", l.Tax ?? 0);
                        cmd.Parameters.AddWithValue("@memo", l.Memo ?? "");
                        cmd.Parameters.AddWithValue("@total", l.Total ?? 0);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to save salesline: {ex.Message}");
            }
        }

        public static void InsertMaster(TblMaster m, string dbPath)
        {
            try
            {
                using (var conn = new SQLiteConnection(GetConnectionString(dbPath)))
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO tblmaster (receiptno, sku, name, inventory, price, category) 
                        VALUES (@receiptno, @sku, @name, @inventory, @price, @category);";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@receiptno", m.ReceiptNo);
                        cmd.Parameters.AddWithValue("@sku", m.Sku);
                        cmd.Parameters.AddWithValue("@name", m.Name ?? "");
                        cmd.Parameters.AddWithValue("@inventory", m.Inventory ?? 0);
                        cmd.Parameters.AddWithValue("@price", m.Price ?? 0);
                        cmd.Parameters.AddWithValue("@category", m.Category ?? "");

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to save master: {ex.Message}");
            }
        }
    }
}
