using Mall_Linking_Alliance.Model;
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

        public static bool ReceiptExists(string receiptNo, string dbPath)
        {
            try
            {
                using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM tblsales WHERE receiptno = @receiptno";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@receiptno", receiptNo);
                        var count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ReceiptExists failed: {ex.Message}");
                return false;
            }
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
                    Console.WriteLine($"[DEBUG] Inserting Sale: Receipt={s.ReceiptNo}, Date={s.Date}");

                    conn.Open();

                    string sql = @"
                INSERT INTO tblsales (
                    receiptno, [date], void, cash, credit, charge, giftcheck, othertender,
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
                        cmd.Parameters.AddWithValue("@void", s.Void);
                        cmd.Parameters.AddWithValue("@cash", s.Cash);
                        cmd.Parameters.AddWithValue("@credit", s.Credit);
                        cmd.Parameters.AddWithValue("@charge", s.Charge);
                        cmd.Parameters.AddWithValue("@giftcheck", s.GiftCheck);
                        cmd.Parameters.AddWithValue("@othertender", s.OtherTender);
                        cmd.Parameters.AddWithValue("@linedisc", s.LineDisc);
                        cmd.Parameters.AddWithValue("@linesenior", s.LineSenior);
                        cmd.Parameters.AddWithValue("@linepwd", s.LinePwd);
                        cmd.Parameters.AddWithValue("@evat", s.Evat);
                        cmd.Parameters.AddWithValue("@linediplomat", s.LineDiplomat);
                        cmd.Parameters.AddWithValue("@subtotal", s.Subtotal);
                        cmd.Parameters.AddWithValue("@disc", s.Disc);
                        cmd.Parameters.AddWithValue("@senior", s.Senior);
                        cmd.Parameters.AddWithValue("@pwd", s.Pwd);
                        cmd.Parameters.AddWithValue("@diplomat", s.Diplomat);
                        cmd.Parameters.AddWithValue("@vat", s.Vat);
                        cmd.Parameters.AddWithValue("@exvat", s.ExVat);
                        cmd.Parameters.AddWithValue("@incvat", s.IncVat);
                        cmd.Parameters.AddWithValue("@localtax", s.LocalTax);
                        cmd.Parameters.AddWithValue("@amusement", s.Amusement);
                        cmd.Parameters.AddWithValue("@ewt", s.Ewt);
                        cmd.Parameters.AddWithValue("@service", s.Service);
                        cmd.Parameters.AddWithValue("@taxsale", s.TaxSale);
                        cmd.Parameters.AddWithValue("@notaxsale", s.NoTaxSale);
                        cmd.Parameters.AddWithValue("@taxexsale", s.TaxExSale);
                        cmd.Parameters.AddWithValue("@taxincsale", s.TaxIncSale);
                        cmd.Parameters.AddWithValue("@zerosale", s.ZeroSale);
                        cmd.Parameters.AddWithValue("@vatexempt", s.VatExempt);
                        cmd.Parameters.AddWithValue("@customercount", s.CustomerCount);
                        cmd.Parameters.AddWithValue("@gross", s.Gross);
                        cmd.Parameters.AddWithValue("@taxrate", s.TaxRate);
                        cmd.Parameters.AddWithValue("@posted", s.Posted);
                        cmd.Parameters.AddWithValue("@memo", s.Memo ?? "");
                        cmd.Parameters.AddWithValue("@qty", s.Qty);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to save sale. Receipt={s.ReceiptNo}, Date={s.Date}, Error={ex.Message}");
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
                        cmd.Parameters.AddWithValue("@receiptno", l.ReceiptNo ?? string.Empty);
                        cmd.Parameters.AddWithValue("@sku", l.Sku ?? string.Empty);
                        cmd.Parameters.AddWithValue("@qty", l.Qty.HasValue ? (object)l.Qty.Value : 0m);
                        cmd.Parameters.AddWithValue("@unitprice", l.UnitPrice.HasValue ? (object)l.UnitPrice.Value : 0m);
                        cmd.Parameters.AddWithValue("@disc", l.Disc.HasValue ? (object)l.Disc.Value : 0m);
                        cmd.Parameters.AddWithValue("@senior", l.Senior.HasValue ? (object)l.Senior.Value : 0m);
                        cmd.Parameters.AddWithValue("@pwd", l.Pwd.HasValue ? (object)l.Pwd.Value : 0m);
                        cmd.Parameters.AddWithValue("@diplomat", l.Diplomat.HasValue ? (object)l.Diplomat.Value : 0m);
                        cmd.Parameters.AddWithValue("@taxtype", l.TaxType.HasValue ? (object)l.TaxType.Value : 0m);
                        cmd.Parameters.AddWithValue("@tax", l.Tax.HasValue ? (object)l.Tax.Value : 0m);
                        cmd.Parameters.AddWithValue("@memo", l.Memo ?? string.Empty);
                        cmd.Parameters.AddWithValue("@total", l.Total.HasValue ? (object)l.Total.Value : 0m);

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
                INSERT INTO tblmaster (sku, name, inventory, price, category) 
                VALUES (@sku, @name, @inventory, @price, @category);";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@sku", m.Sku);
                        cmd.Parameters.AddWithValue("@name", m.Name ?? "");
                        cmd.Parameters.AddWithValue("@price", m.Price);
                        cmd.Parameters.AddWithValue("@inventory", m.Inventory);
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