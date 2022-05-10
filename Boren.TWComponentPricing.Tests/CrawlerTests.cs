using Boren.TWComponentPricing.Worker;
using Boren.TWComponentPricing.Worker.Service;
using Newtonsoft.Json;
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
        public async Task GetAsyncTest()
        {
            var list = await _crawlerService.GetAsync();
            var json = JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        [Test]
        public async Task SetAsyncTest()
        {
            var list = await _crawlerService.GetAsync();
            await _crawlerService.SetAsync(list);
        }
    }
}
