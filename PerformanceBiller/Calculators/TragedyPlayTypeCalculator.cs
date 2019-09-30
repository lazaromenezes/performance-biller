using PerformanceBiller.Model;

namespace PerformanceBiller.Calculators
{
    public class TragedyPlayTypeCalculator : IPlayTypeCalculator
    {
        private const decimal BASE_AMOUNT = 40000m;
        private const decimal AUDIENCE_PERFORMANCE_AMOUNT = 1000m;
        private const int AUDIENCE_PERFORMANCE_THRESHOLD = 30;

        public string PlayType => "tragedy";

        public decimal Calculate(Performance performance)
        {
            var amount = BASE_AMOUNT;

            if (performance.Audience > AUDIENCE_PERFORMANCE_THRESHOLD)
                amount += AUDIENCE_PERFORMANCE_AMOUNT * (performance.Audience - AUDIENCE_PERFORMANCE_THRESHOLD);

            return amount;
        }

        public bool IsApplicable(Play play)
        {
            return PlayType.Equals(play.Type);
        }
    }
}
