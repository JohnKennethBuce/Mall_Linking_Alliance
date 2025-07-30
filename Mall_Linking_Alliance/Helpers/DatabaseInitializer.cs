using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Mall_Linking_Alliance.Helpers
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseStructure(string dbPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dbPath))
                    throw new ArgumentNullException(nameof(dbPath), "Database path cannot be null.");

                bool dbExists = File.Exists(dbPath);
                if (!dbExists)
                {
                    SQLiteConnection.CreateFile(dbPath);
                }

                using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    connection.Open();

                    void CreateIfMissing(string tableName, string fullSql)
                    {
                        string checkSql = "SELECT name FROM sqlite_master WHERE type='table' AND name=@table";
                        using (var cmd = new SQLiteCommand(checkSql, connection))
                        {
                            cmd.Parameters.AddWithValue("@table", tableName);
                            var result = cmd.ExecuteScalar();

                            if (result == null)
                            {
                                string[] commands = fullSql.Split(new[] { ";\r\n", ";\n", ";" }, StringSplitOptions.RemoveEmptyEntries);
                                using (var transaction = connection.BeginTransaction())
                                {
                                    foreach (var sql in commands)
                                    {
                                        using (var createCmd = new SQLiteCommand(sql.Trim(), connection, transaction))
                                        {
                                            createCmd.ExecuteNonQuery();
                                        }
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }

                    // tblsales
                    CreateIfMissing("tblsales", @"
CREATE TABLE tblsales (
    receiptno       TEXT PRIMARY KEY,
    date            TEXT,
    void            INTEGER DEFAULT 0,
    cash            NUMERIC DEFAULT 0,
    credit          NUMERIC DEFAULT 0,
    charge          NUMERIC DEFAULT 0,
    giftcheck       NUMERIC DEFAULT 0,
    othertender     NUMERIC DEFAULT 0,
    linedisc        NUMERIC DEFAULT 0,
    linesenior      NUMERIC DEFAULT 0,
    linepwd         NUMERIC DEFAULT 0,
    evat            NUMERIC DEFAULT 0,
    linediplomat    NUMERIC DEFAULT 0,
    subtotal        NUMERIC DEFAULT 0,
    disc            NUMERIC DEFAULT 0,
    senior          NUMERIC DEFAULT 0,
    pwd             NUMERIC DEFAULT 0,
    diplomat        NUMERIC DEFAULT 0,
    vat             NUMERIC DEFAULT 0,
    exvat           NUMERIC DEFAULT 0,
    incvat          NUMERIC DEFAULT 0,
    localtax        NUMERIC DEFAULT 0,
    amusement       NUMERIC DEFAULT 0,
    ewt             NUMERIC DEFAULT 0,
    service         NUMERIC DEFAULT 0,
    taxsale         NUMERIC DEFAULT 0,
    notaxsale       NUMERIC DEFAULT 0,
    taxexsale       NUMERIC DEFAULT 0,
    taxincsale      NUMERIC DEFAULT 0,
    zerosale        NUMERIC DEFAULT 0,
    vatexempt       NUMERIC DEFAULT 0,
    customercount   INTEGER DEFAULT 0,
    gross           NUMERIC DEFAULT 0,
    taxrate         NUMERIC DEFAULT 0,
    posted          INTEGER DEFAULT 0,
    memo            TEXT,
    qty             INTEGER

);
CREATE INDEX IF NOT EXISTS idx_sales_date ON tblsales(date);
");

                    // tblsalesline
                    CreateIfMissing("tblsalesline", @"
CREATE TABLE tblsalesline (
    receiptno       TEXT,
    sku             TEXT,
    qty             NUMERIC DEFAULT 0,
    unitprice       NUMERIC DEFAULT 0,
    disc            NUMERIC DEFAULT 0,
    senior          NUMERIC DEFAULT 0,
    pwd             NUMERIC DEFAULT 0,
    diplomat        NUMERIC DEFAULT 0,
    taxtype         NUMERIC DEFAULT 0,
    tax             NUMERIC DEFAULT 0,
    memo            TEXT,
    total           NUMERIC DEFAULT 0,

    FOREIGN KEY (receiptno) REFERENCES tblsales (receiptno)
);
CREATE INDEX IF NOT EXISTS idx_salesline_sku ON tblsalesline(sku);
");

                    // tblmaster
                    CreateIfMissing("tblmaster", @"
CREATE TABLE tblmaster (
    sku             TEXT,
    name            TEXT,
    inventory       INTEGER,
    price           NUMERIC DEFAULT 0,
    category        TEXT
);
CREATE INDEX IF NOT EXISTS idx_master_sku ON tblmaster(sku);
");

                    // tblsettings
                    CreateIfMissing("tblsettings", @"
CREATE TABLE tblsettings (
    tenantid        INTEGER DEFAULT 0,
    tenantkey       TEXT,
    tmid            INTEGER DEFAULT 0,
    doc             TEXT,
    browsedb        TEXT,
    savedb          TEXT
);
");

                    // tbleod
                    CreateIfMissing("tbleod", @"
CREATE TABLE tbleod (
    date                TEXT,
    zcounter            INTEGER DEFAULT 0,
    previousnrgt        NUMERIC DEFAULT 0,
    nrgt                NUMERIC DEFAULT 0,
    previoustax         NUMERIC DEFAULT 0,
    newtax              NUMERIC DEFAULT 0,
    previoustaxsale     NUMERIC DEFAULT 0,
    newtaxsale          NUMERIC DEFAULT 0,
    previousnotaxsale   NUMERIC DEFAULT 0,
    newnotaxsale        NUMERIC DEFAULT 0,
    opentime            INTEGER DEFAULT 0,
    closetime           INTEGER DEFAULT 0,
    gross               NUMERIC DEFAULT 0,
    vat                 NUMERIC DEFAULT 0,
    localtax            NUMERIC DEFAULT 0,
    amusement           NUMERIC DEFAULT 0,
    ewt                 NUMERIC DEFAULT 0,
    taxsale             NUMERIC DEFAULT 0,
    notaxsale           NUMERIC DEFAULT 0,
    zerosale            NUMERIC DEFAULT 0,
    vatexempt           NUMERIC DEFAULT 0,
    void                NUMERIC DEFAULT 0,
    voidcnt             INTEGER DEFAULT 0,
    disc                NUMERIC DEFAULT 0,
    disccnt             INTEGER DEFAULT 0,
    refund              NUMERIC DEFAULT 0,
    refundcnt           INTEGER DEFAULT 0,
    senior              NUMERIC DEFAULT 0,
    seniorcnt           INTEGER DEFAULT 0,
    pwd                 NUMERIC DEFAULT 0,
    pwdcnt              INTEGER DEFAULT 0,
    diplomat            NUMERIC DEFAULT 0,
    diplomatcnt         INTEGER DEFAULT 0,
    service             NUMERIC DEFAULT 0,
    servicecnt          INTEGER DEFAULT 0,
    receiptstart        TEXT,
    receiptend          TEXT,
    trxcnt              INTEGER DEFAULT 0,
    cash                NUMERIC DEFAULT 0,
    cashcnt             INTEGER DEFAULT 0,
    credit              NUMERIC DEFAULT 0,
    creditcnt           INTEGER DEFAULT 0,
    charge              NUMERIC DEFAULT 0,
    chargecnt           INTEGER DEFAULT 0,
    giftcheck           NUMERIC DEFAULT 0,
    giftcheckcnt        INTEGER DEFAULT 0,
    othertender         NUMERIC DEFAULT 0,
    othertendercnt      INTEGER DEFAULT 0,
    FOREIGN KEY (date) REFERENCES tblsales (date)
);
");

                    // tbllogs
                    CreateIfMissing("tbllogs", @"
CREATE TABLE tbllogs (
    logid       INTEGER PRIMARY KEY AUTOINCREMENT,
    logdate     TEXT DEFAULT (datetime('now')),
    loglevel    TEXT,
    source      TEXT,
    message     TEXT
);
CREATE INDEX IF NOT EXISTS idx_logs_date ON tbllogs(logdate);
");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error ensuring database structure:\n\n{ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
