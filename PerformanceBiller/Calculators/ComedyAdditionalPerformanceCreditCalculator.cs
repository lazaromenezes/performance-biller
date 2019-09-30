using PerformanceBiller.Model;

namespace PerformanceBiller.Calculators
{
    public class ComedyAdditionalPerformanceCreditCalculator : IAdditionalPerformanceCreditCalculator
    {
        public string PlayType => "comedy";

        public int Calculate(Performance performance)
        {
            return performance.Audience / 5;
        }

        public bool IsApplicable(Play play)
        {
            return PlayType.Equals(play.Type);
        }
    }
}
