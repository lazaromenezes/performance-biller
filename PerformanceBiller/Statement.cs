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
        public readonly IPlayTypeCalculator[] _playTypeCalculators;
        public readonly IAdditionalPerformanceCreditCalculator[] _creditsCalculators;

        public Statement(IPlayTypeCalculator[] playTypeCalculators,
            IAdditionalPerformanceCreditCalculator[] creditsCalculators)
        {
            _playTypeCalculators = playTypeCalculators;
            _creditsCalculators = creditsCalculators;
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

                volumeCredits += CalculatePerformanceCredits(performance, play);

                // print line for this order
                result += $" {play.Name}: {(performanceAmount / 100).ToString("C", cultureInfo)} ({performance.Audience} seats)\n";
                totalAmount += performanceAmount;
            }

            result += $"Amount owed is {(totalAmount/100).ToString("C", cultureInfo)}\n";
            result += $"You earned {volumeCredits} credits\n";

            return result;
        }

        private int CalculatePerformanceCredits(Performance performance, Play play)
        {
            const int PERFORMANCE_CREDIT_TRHESHOLD = 30;

            var credits = Math.Max(performance.Audience - PERFORMANCE_CREDIT_TRHESHOLD, 0);

            foreach (var calculator in _creditsCalculators)
                if (calculator.IsApplicable(play))
                    credits += calculator.Calculate(performance);

            return credits;
        }

        private decimal CalculatePerformanceAmmount(Performance performance, Play play)
        {
            foreach (var calculator in _playTypeCalculators)
                if (calculator.IsApplicable(play))
                    return calculator.Calculate(performance);

            throw new UnkonwPlayTypeException(play);
        }
    }
}
