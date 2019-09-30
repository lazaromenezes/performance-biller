using PerformanceBiller.Model;

namespace PerformanceBiller.Calculators
{
    public interface IAdditionalPerformanceCreditCalculator
    {
        string PlayType { get; }
        bool IsApplicable(Play play);
        int Calculate(Performance performance);
    }
}
