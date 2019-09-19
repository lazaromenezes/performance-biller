using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Xunit;

namespace PerformanceBiller.Tests
{
    public class StatementTests
    {
        [Fact]
        public void Statement_can_run()
        {
            var expectedOutput = "Statement for BigCo\n" +
                " Hamlet: $650.00 (55 seats)\n" +
                " As You Like It: $580.00 (35 seats)\n" +
                " Othello: $500.00 (40 seats)\n" +
                "Amount owed is $1,730.00\n" +
                "You earned 47 credits\n";

            var statement = new Statement();

            using (var invoicesFile = File.OpenText("..\\..\\..\\invoices.json"))
            using (var invoicesReader = new JsonTextReader(invoicesFile))
            using (var playsFile = File.OpenText("..\\..\\..\\plays.json"))
            using (var playsReader = new JsonTextReader(playsFile)) {
                var invoices = (JArray) JToken.ReadFrom(invoicesReader);

                var invoice = (JObject) invoices.First;

                var plays = (JObject) JToken.ReadFrom(playsReader);

                var actualResult = statement.Run(invoice, plays);

                Assert.Equal(expectedOutput, actualResult);
            }
        }
    }
}
