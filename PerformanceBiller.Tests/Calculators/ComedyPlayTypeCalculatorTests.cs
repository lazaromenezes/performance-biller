using PerformanceBiller.Calculators;
using PerformanceBiller.Model;
using Xunit;

namespace PerformanceBiller.Tests.Calculators
{
    public class ComedyPlayTypeCalculatorTests
    {
        private const decimal BASE_AMOUNT = 30000m;

        private readonly ComedyPlayTypeCalculator _calculator;

        public ComedyPlayTypeCalculatorTests()
        {
            _calculator = new ComedyPlayTypeCalculator();
        }

        [Fact]
        public void Returns_base_amount_plus_300_for_each_attendant_if_up_to_20()
        {
            const int ATTENDANCE = 15;
            var expectedAmount = BASE_AMOUNT + 300 * ATTENDANCE;

            var amount = _calculator.Calculate(new Performance
            {
                Audience = ATTENDANCE
            });

            Assert.Equal(expectedAmount, amount);
        }

        [Fact]
        public void Returns_base_amount_plus_audience_bonus_for_each_after_threshold_attendant()
        {
            const int ATTENDANCE = 25;
            const int BONUS_BASE = 10000;
            const int PER_EXTRA_ATTENDANT_BONUS = 500;

            var baseAmmount = BASE_AMOUNT + 300 * ATTENDANCE;
            var bonusAmmount = BONUS_BASE + PER_EXTRA_ATTENDANT_BONUS * 5;

            var expectedAmount = baseAmmount + bonusAmmount;

            var amount = _calculator.Calculate(new Performance
            {
                Audience = ATTENDANCE
            });

            Assert.Equal(expectedAmount, amount);
        }

        [Fact]
        public void IsApplicable_if_play_type_is_calculators_playType()
        {
            var play = new Play
            {
                Type = _calculator.PlayType
            };

            Assert.True(_calculator.IsApplicable(play));
        }

        [Fact]
        public void Is_not_applicable_if_play_type_is_different_from_comedy()
        {
            var play = new Play
            {
                Type = "anything but comedy"
            };

            Assert.False(_calculator.IsApplicable(play));
        }

        [Fact]
        public void PlayType_is_set_properly()
        {
            Assert.Equal("comedy", _calculator.PlayType);
        }
    }
}
