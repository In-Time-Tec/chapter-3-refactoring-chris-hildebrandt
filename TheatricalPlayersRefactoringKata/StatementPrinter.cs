using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string PrintPlayersStatement(Dictionary<string, Play> plays, Invoice invoice)
        {
            var invoiceTotal = getInvoiceTotal(plays, invoice);
            var volumeCredits = getVolumeCredits(plays, invoice);
            CultureInfo cultureInfo = new CultureInfo("en-US");
            var statement = string.Format("Statement for {0}\n", invoice.Customer);
            statement += getInvoicePerformances(plays, invoice);
            statement += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(invoiceTotal / 100));
            statement += String.Format("You earned {0} credits\n", volumeCredits);
            return statement;
        }

        public int getInvoiceTotal(Dictionary<string, Play> plays, Invoice invoice)
        {
            int invoiceTotal = 0;
            foreach (var performance in invoice.Performances)
            {
                var play = plays[performance.PlayID];
                invoiceTotal += getBilledAmountByPerformance(play.Type, performance.Audience);
            }

            return invoiceTotal;
        }
        public int getBilledAmountByPerformance(string type, int audience)
        {
            int performanceTotal = 0;
            switch (type)
            {
                case "tragedy":
                    performanceTotal += calculateTragedyAmount(audience);
                    break;
                case "comedy":
                    performanceTotal += calculateComedyAmount(audience);
                    break;
                default:
                    throw new Exception("unknown type: " + type);
            }
            return performanceTotal;
        }

        public int calculateTragedyAmount(int audience)
        {

            int performanceCost = 40000;
            if (audience > 30)
            {
                performanceCost = 1000 * (audience - 30);
            }
            return performanceCost;
        }

        public int calculateComedyAmount(int audience)
        {
            int performanceCost = 30000;
            if (audience > 20)
            {
                performanceCost += 10000 + 500 * (audience - 20);
            }
            performanceCost += 300 * audience;
            return performanceCost;
        }
        public int getVolumeCredits(Dictionary<string, Play> plays, Invoice invoice)
        {
            int volumeCredits = 0;
            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                if (play.Type == "comedy")
                {
                    volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);
                }
            }

            return volumeCredits;
        }
        public string getInvoicePerformances(Dictionary<string, Play> plays, Invoice invoice)
        {
            string performances = "";
            foreach (var perf in invoice.Performances)
            {
                CultureInfo cultureInfo = new CultureInfo("en-US");
                var play = plays[perf.PlayID];
                int performanceCost = getBilledAmountByPerformance(play.Type, perf.Audience);
                performances += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(performanceCost / 100), perf.Audience);
            }

            return performances;
        }
    }
}