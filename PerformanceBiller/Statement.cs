using PerformanceBiller.Calculators;
using PerformanceBiller.Exceptions;
using PerformanceBiller.Model;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PerformanceBiller
{
    public class Statement
    {
        public IPlayTypeCalculator[] _playTypeCalculators;

        public Statement(IPlayTypeCalculator[] playTypeCalculators)
        {
            _playTypeCalculators = playTypeCalculators;
        }

        public string BuildStatement(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0m;
            var volumeCredits = 0;
            var result = $"Statement for {invoice.Customer}\n";
            var cultureInfo = new CultureInfo("en-US");

            foreach (var performance in invoice.Performances)
            {
                var play = plays[performance.PlayID];

                var performanceAmount = CalculatePerformanceAmmount(performance, play);
                
                // add volume credits
                volumeCredits += Math.Max(performance.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += performance.Audience / 5;
                // print line for this order
                result += $" {play.Name}: {(performanceAmount / 100).ToString("C", cultureInfo)} ({performance.Audience} seats)\n";
                totalAmount += performanceAmount;
            }

            result += $"Amount owed is {(totalAmount/100).ToString("C", cultureInfo)}\n";
            result += $"You earned {volumeCredits} credits\n";

            return result;
        }

        private decimal CalculatePerformanceAmmount(Performance perf, Play play)
        {
            foreach (var calculator in _playTypeCalculators)
                if (calculator.IsApplicable(play))
                    return calculator.Calculate(perf);

            throw new UnkonwPlayTypeException(play);
        }
    }
}
