using Boren.TWComponentPricing.Data;
using Boren.TWComponentPricing.Worker;
using Boren.TWComponentPricing.Worker.Service;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;
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

        [Test]
        public void GetFixedTextTest()
        {
            var builder = new StringBuilder();

            var dbContext = new PricingDbContext();
            var entities = dbContext.Products.Select(x => x).ToList();
            foreach (var entity in entities)
                builder.AppendLine(entity.GetFixedText());

            Console.Write(builder.ToString());
        }
    }
}
