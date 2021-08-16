using Boren.TWComponentPricing.Worker;
using Boren.TWComponentPricing.Worker.Service;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Boren.TWComponentPricing.Tests
{
    [TestFixture]
    public class CrawlerTests
    {
        // url : 
        private readonly ICrawlerService _crawlerService;
        public CrawlerTests()
        {
            _crawlerService = new CrawlerService();
        }


        [Test]
        public async Task Test1()
        {
            var list = await _crawlerService.GetAsync();
        }
    }
}
