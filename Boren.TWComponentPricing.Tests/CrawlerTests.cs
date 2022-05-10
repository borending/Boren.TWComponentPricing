﻿using Boren.TWComponentPricing.Worker;
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
        public async Task GetAsyncTest()
        {
            var list = await _crawlerService.GetAsync();
        }

        [Test]
        public async Task SetAsyncTest()
        {
            var list = await _crawlerService.GetAsync();
            //var list = await _crawlerService.GetAsync();
        }
    }
}
