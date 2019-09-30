using PerformanceBiller.Model;

namespace PerformanceBiller.Calculators
{
    public class ComedyPlayTypeCalculator: IPlayTypeCalculator
    {
        private const decimal BASE_AMMOUNT = 30000m;
        private const decimal PER_ATTENDANT_BILL = 300m;
        private const decimal BASE_ATTENDANCE_BONUS = 10000m;
        private const decimal PER_EXTRA_ATTENDANT_BONUS = 500m;
        private const int AUDIENCE_PERFORMANCE_THRESHOLD = 20;

        public string PlayType => "comedy";

        public decimal Calculate(Performance performance)
        {
            var thisAmount = BASE_AMMOUNT;

            if (performance.Audience > AUDIENCE_PERFORMANCE_THRESHOLD)
                thisAmount += CalculateAttendanceBonus(performance);

            return thisAmount + PER_ATTENDANT_BILL * performance.Audience;
        }

        private static decimal CalculateAttendanceBonus(Performance performance)
        {
            var bonusAttendants = performance.Audience - AUDIENCE_PERFORMANCE_THRESHOLD;
            return BASE_ATTENDANCE_BONUS + PER_EXTRA_ATTENDANT_BONUS * bonusAttendants;
        }

        public bool IsApplicable(Play play)
        {
            return PlayType.Equals(play.Type);
        }
    }
}
