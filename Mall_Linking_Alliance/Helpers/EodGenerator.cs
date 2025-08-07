using System;
using System.Xml.Linq;
using Mall_Linking_Alliance.Model;

namespace Mall_Linking_Alliance.Helpers
{
    public static class EodGenerator
    {
        public static TblEod GenerateFromXml(string xmlContent)
        {
            var doc = XDocument.Parse(xmlContent);
            var sales = doc.Root.Element("sales");
            if (sales == null) return null;

            return new TblEod
            {
                Date = sales.Element("date")?.Value,
                ZCounter = ToInt(sales.Element("zcounter")?.Value),
                PreviousNrgt = ToDecimal(sales.Element("previousnrgt")?.Value),
                Nrgt = ToDecimal(sales.Element("nrgt")?.Value),
                PreviousTax = ToDecimal(sales.Element("previoustax")?.Value),
                NewTax = ToDecimal(sales.Element("newtax")?.Value),
                PreviousTaxSale = ToDecimal(sales.Element("previoustaxsale")?.Value),
                NewTaxSale = ToDecimal(sales.Element("newtaxsale")?.Value),
                PreviousNoTaxSale = ToDecimal(sales.Element("prevousnotaxsale")?.Value),
                NewNoTaxSale = ToDecimal(sales.Element("newnotaxsale")?.Value),
                OpenTime = ToInt(sales.Element("opentime")?.Value),
                CloseTime = ToInt(sales.Element("closetime")?.Value),
                Gross = ToDecimal(sales.Element("gross")?.Value),
                Vat = ToDecimal(sales.Element("vat")?.Value),
                LocalTax = ToDecimal(sales.Element("localtax")?.Value),
                Amusement = ToDecimal(sales.Element("amusement")?.Value),
                Ewt = ToDecimal(sales.Element("ewt")?.Value),
                TaxSale = ToDecimal(sales.Element("taxsale")?.Value),
                NoTaxSale = ToDecimal(sales.Element("notaxsale")?.Value),
                ZeroSale = ToDecimal(sales.Element("zerosale")?.Value),
                VatExempt = ToDecimal(sales.Element("vatexempt")?.Value),
                Void = ToDecimal(sales.Element("void")?.Value),
                VoidCnt = ToInt(sales.Element("voidcnt")?.Value),
                Disc = ToDecimal(sales.Element("disc")?.Value),
                DiscCnt = ToInt(sales.Element("disccnt")?.Value),
                Refund = ToDecimal(sales.Element("refund")?.Value),
                RefundCnt = ToInt(sales.Element("refundcnt")?.Value),
                Senior = ToDecimal(sales.Element("senior")?.Value),
                SeniorCnt = ToInt(sales.Element("seniorcnt")?.Value),
                Pwd = ToDecimal(sales.Element("pwd")?.Value),
                PwdCnt = ToInt(sales.Element("pwdcnt")?.Value),
                Diplomat = ToDecimal(sales.Element("diplomat")?.Value),
                DiplomatCnt = ToInt(sales.Element("diplomatcnt")?.Value),
                Service = ToDecimal(sales.Element("service")?.Value),
                ServiceCnt = ToInt(sales.Element("servicecnt")?.Value),
                ReceiptStart = sales.Element("receiptstart")?.Value,
                ReceiptEnd = sales.Element("receiptend")?.Value,
                TrxCnt = ToInt(sales.Element("trxcnt")?.Value),
                Cash = ToDecimal(sales.Element("cash")?.Value),
                CashCnt = ToInt(sales.Element("cashcnt")?.Value),
                Credit = ToDecimal(sales.Element("credit")?.Value),
                CreditCnt = ToInt(sales.Element("creditcnt")?.Value),
                Charge = ToDecimal(sales.Element("charge")?.Value),
                ChargeCnt = ToInt(sales.Element("chargecnt")?.Value),
                GiftCheck = ToDecimal(sales.Element("giftcheck")?.Value),
                GiftCheckCnt = ToInt(sales.Element("giftcheckcnt")?.Value),
                OtherTender = ToDecimal(sales.Element("othertender")?.Value),
                OtherTenderCnt = ToInt(sales.Element("othertendercnt")?.Value)
            };
        }

        private static int ToInt(string value)
        {
            return int.TryParse(value, out var result) ? result : 0;
        }

        private static decimal ToDecimal(string value)
        {
            return decimal.TryParse(value, out var result) ? result : 0;
        }
    }
}
