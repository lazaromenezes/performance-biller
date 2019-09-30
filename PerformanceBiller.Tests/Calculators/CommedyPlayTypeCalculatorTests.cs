using PerformanceBiller.Calculators;
using PerformanceBiller.Model;
using Xunit;

namespace PerformanceBiller.Tests.Calculators
{
    public class CommedyPlayTypeCalculatorTests
    {
        [Fact]
        public void Returns_base_amount()
        {
            var calculator = new CommedyPlayTypeCalculator();

            var ammount = calculator.Calculate(new Performance());

            Assert.Equal(30000, ammount);
        }
    }
}
