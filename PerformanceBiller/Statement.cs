using PerformanceBiller.Calculators;
using PerformanceBiller.Exceptions;
using PerformanceBiller.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PerformanceBiller
{
    public class Statement
    {
        private readonly IPlayTypeCalculator[] _playTypeCalculators;
        private readonly IAdditionalPerformanceCreditCalculator[] _creditsCalculators;

        public Statement(IPlayTypeCalculator[] playTypeCalculators,
            IAdditionalPerformanceCreditCalculator[] creditsCalculators)
        {
            _playTypeCalculators = playTypeCalculators;
            _creditsCalculators = creditsCalculators;
        }

        public string BuildStatement(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = CalculateTotalAmount(invoice, plays);
            var volumeCredits = CalculateVolumeCredits(invoice, plays);

            return BuildStatementOutput(invoice, plays, totalAmount, volumeCredits);
        }

        private decimal CalculateTotalAmount(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0m;

            foreach (var performance in invoice.Performances)
            {
                var play = plays[performance.PlayID];

                var performanceAmount = CalculatePerformanceAmmount(performance, play);

                totalAmount += performanceAmount;
            }

            return totalAmount;
        }

        private int CalculateVolumeCredits(Invoice invoice, Dictionary<string, Play> plays)
        {
            var volumeCredits = 0;

            foreach (var performance in invoice.Performances)
            {
                var play = plays[performance.PlayID];

                volumeCredits += CalculatePerformanceCredits(performance, play);
            }

            return volumeCredits;
        }

        private string BuildStatementOutput(Invoice invoice, Dictionary<string, Play> plays, decimal totalAmount, int volumeCredits)
        {
            var statement = InitializeStatement(invoice);

            AddStatementPlayLines(invoice, plays, statement);
            AddStatementBottomLine(totalAmount, volumeCredits, statement);

            return statement.ToString();
        }

        private StringBuilder InitializeStatement(Invoice invoice)
        {
            return new StringBuilder($"Statement for {invoice.Customer}\n");
        }

        private void AddStatementPlayLines(Invoice invoice, Dictionary<string, Play> plays, StringBuilder statement)
        {
            foreach (var performance in invoice.Performances)
                AddPlayLine(plays, statement, performance);
        }

        private void AddPlayLine(Dictionary<string, Play> plays, StringBuilder statement, Performance performance)
        {
            var play = plays[performance.PlayID];

            var performanceAmount = CalculatePerformanceAmmount(performance, play);

            statement.Append(BuildPerformanceLine(performance, play, performanceAmount));
        }

        private void AddStatementBottomLine(decimal totalAmount, int volumeCredits, StringBuilder result)
        {
            result.Append($"Amount owed is {FormatCurrency(totalAmount)}\n");
            result.Append($"You earned {volumeCredits} credits\n");
        }

        private string FormatCurrency(decimal totalAmount)
        {
            var cultureInfo = new CultureInfo("en-US");

            return (totalAmount / 100).ToString("C", cultureInfo);
        }

        private string BuildPerformanceLine(Performance performance, Play play, decimal performanceAmount)
        {
            return $" {play.Name}: {FormatCurrency(performanceAmount)} ({performance.Audience} seats)\n";
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
