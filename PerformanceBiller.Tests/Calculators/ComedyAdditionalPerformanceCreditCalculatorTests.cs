using PerformanceBiller.Calculators;
using PerformanceBiller.Model;
using Xunit;

namespace PerformanceBiller.Tests.Calculators
{
    public class ComedyAdditionalPerformanceCreditCalculatorTests
    {
        private readonly ComedyAdditionalPerformanceCreditCalculator _calculator;

        public ComedyAdditionalPerformanceCreditCalculatorTests()
        {
            _calculator = new ComedyAdditionalPerformanceCreditCalculator();
        }

        [Fact]
        public void Returns_one_credit_to_each_5_attendants()
        {
            const int ATTENDANCE = 15;

            var expectedCredits = 3;

            var amount = _calculator.Calculate(new Performance
            {
                Audience = ATTENDANCE
            });

            Assert.Equal(expectedCredits, amount);
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
