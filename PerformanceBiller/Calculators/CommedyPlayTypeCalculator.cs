using PerformanceBiller.Model;

namespace PerformanceBiller.Calculators
{
    public class CommedyPlayTypeCalculator
    {
        public decimal Calculate(Performance performance)
        {
            var thisAmount = 30000m;

            if (performance.Audience > 20)
                thisAmount += 10000 + 500 * (performance.Audience - 20);

            thisAmount += 300 * performance.Audience;

            return thisAmount;
        }
    }
}
