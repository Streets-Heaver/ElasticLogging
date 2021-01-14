using ElasticLogging.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ElasticLogging.Tests
{
    [TestClass]
    public class LoggingServiceTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var settings = new LoggingSettings();
            settings.ElasticSearchUrl = "http://kingston.office-ad.streets-heaver.com:9200";
            settings.Index = "dev-sla-monitor";
            using var service = new LoggingService(settings, "test", false);

            await service.InfoAsync("async test");
        }
    }
}
