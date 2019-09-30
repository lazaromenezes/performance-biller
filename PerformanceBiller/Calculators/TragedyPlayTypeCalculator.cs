using PerformanceBiller.Model;

namespace PerformanceBiller.Calculators
{
    public class TragedyPlayTypeCalculator
    {
        public decimal Calculate(Performance performance)
        {
            var thisAmount = 40000;

            if (performance.Audience > 30)
                thisAmount += 1000 * (performance.Audience - 30);

            return thisAmount;
        }
    }
}
