using PerformanceBiller.Model;

namespace PerformanceBiller.Calculators
{
    public interface IPlayTypeCalculator
    {
        string PlayType { get; }
        bool IsApplicable(Play play);
        decimal Calculate(Performance performance);
    }
}
