using Newtonsoft.Json.Linq;
using PerformanceBiller.Model;
using System;
using System.Globalization;

namespace PerformanceBiller
{
    public class Statement
    {
        public string BuildStatement(Invoice invoice, JObject plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = $"Statement for {invoice.Customer}\n";
            var cultureInfo = new CultureInfo("en-US");

            foreach (var perf in invoice.Performances) {
                var play = (JObject) plays.GetValue(perf.PlayID);
                var thisAmount = 0;
                switch (play.GetValue("type").ToString()) {
                    case "tragedy":
                        thisAmount = 40000;
                        if (Convert.ToInt32(perf.Audience) > 30) {
                            thisAmount += 1000 * (Convert.ToInt32(perf.Audience) - 30);
                        }
                        break;
                    case "comedy":
                        thisAmount = 30000;
                        if (Convert.ToInt32(perf.Audience) > 20) {
                            thisAmount += 10000 + 500 * (Convert.ToInt32(perf.Audience) - 20);
                        }
                        thisAmount += 300 * Convert.ToInt32(perf.Audience);
                        break;
                    default:
                        throw new Exception($"unknown type: { play.GetValue("type").ToString()}");
                }
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.GetValue("type").ToString()) volumeCredits += Convert.ToInt32(perf.Audience) / 5;
                // print line for this order
                result += $" {play.GetValue("name")}: {(thisAmount/100).ToString("C", cultureInfo)} ({perf.Audience} seats)\n";
                totalAmount += thisAmount;
             }
             result += $"Amount owed is {(totalAmount/100).ToString("C", cultureInfo)}\n";
             result += $"You earned {volumeCredits} credits\n";

             return result;
        }
    }
}
