using PerformanceBiller.Calculators;
using PerformanceBiller.Model;
using Xunit;

namespace PerformanceBiller.Tests.Calculators
{
    public class TragedyPlayTypeCalculatorTests
    {
        [Fact]
        public void Calculate_Test()
        {
            var calculator = new TragedyPlayTypeCalculator();

            var ammount = calculator.Calculate(new Performance());

            Assert.Equal(40000, ammount);
        }
    }
}
