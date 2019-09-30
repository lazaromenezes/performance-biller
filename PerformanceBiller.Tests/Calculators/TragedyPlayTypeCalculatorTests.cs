using PerformanceBiller.Calculators;
using PerformanceBiller.Model;
using System;
using Xunit;

namespace PerformanceBiller.Tests.Calculators
{
    public class TragedyPlayTypeCalculatorTests
    {
        private const decimal BASE_AMOUNT = 40000m;
        private const decimal AUDIENCE_PERFORMANCE_AMOUNT = 1000m;

        private readonly TragedyPlayTypeCalculator _calculator;

        public TragedyPlayTypeCalculatorTests()
        {
            _calculator = new TragedyPlayTypeCalculator();
        }

        [Fact]
        public void Returns_base_amount_if_audience_is_less_than_30()
        {
            var expectedAmount = BASE_AMOUNT;

            var amount = _calculator.Calculate(new Performance {
                Audience = 25
            });

            Assert.Equal(expectedAmount, amount);
        }

        [Fact]
        public void Returns_base_amount_plus_audience_bonus_for_each_after_threshold_attendant()
        {
            var exectedAmount = BASE_AMOUNT + 5 * AUDIENCE_PERFORMANCE_AMOUNT;

            var amount = _calculator.Calculate(new Performance {
                Audience = 35
            });

            Assert.Equal(exectedAmount, amount);
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
        public void Is_not_applicable_if_play_type_is_different_from_tragedy()
        {
            var play = new Play
            {
                Type = "anything but tragedy"
            };

            Assert.False(_calculator.IsApplicable(play));
        }

        [Fact]
        public void PlayType_is_set_properly()
        {
            Assert.Equal("tragedy", _calculator.PlayType);
        }
    }
}
