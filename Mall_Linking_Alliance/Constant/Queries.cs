using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall_Linking_Alliance.Constant
{
    public static class Queries
    {
        public const string EodCheckerMismatchQuery = @"
SELECT 
    c.date,
    c.pwdcnt AS checker_pwdcnt,
    e.pwdcnt AS eod_pwdcnt,
    c.trxcnt AS checker_trxcnt,
    e.trxcnt AS eod_trxcnt,                     -- 🔸 include this
    c.cash AS checker_cash,
    e.cash AS eod_cash,
    c.receiptstart AS checker_receiptstart,
    e.receiptstart AS eod_receiptstart,
    c.receiptend AS checker_receiptend,
    e.receiptend AS eod_receiptend
FROM tblchecker c
JOIN tbleod e ON c.date = e.date
WHERE 
    c.pwdcnt != e.pwdcnt OR
    c.trxcnt != e.trxcnt OR                     -- 🔸 compare here
    c.cash != e.cash OR
    c.receiptstart != e.receiptstart OR
    c.receiptend != e.receiptend;
";

        public const string InsertCheckerFromEodAndSales = @"
INSERT INTO tblchecker (date, pwdcnt, trxcnt, cash, receiptstart, receiptend)
SELECT 
    e.date,
    COALESCE((SELECT COUNT(*) FROM tblsales s WHERE s.date = e.date AND s.pwd > 0), 0) AS pwdcnt,
    COALESCE((SELECT COUNT(*) FROM tblsales s WHERE s.date = e.date), 0) AS trxcnt,
    COALESCE((SELECT SUM(s.cash) FROM tblsales s WHERE s.date = e.date), 0) AS cash,
    (SELECT s.receiptno FROM tblsales s WHERE s.date = e.date ORDER BY s.receiptno ASC LIMIT 1) AS receiptstart,
    (SELECT s.receiptno FROM tblsales s WHERE s.date = e.date ORDER BY s.receiptno DESC LIMIT 1) AS receiptend
FROM tbleod e
WHERE NOT EXISTS (
    SELECT 1 FROM tblchecker c WHERE c.date = e.date
);
";
    }
}

