using HtmlAgilityPack;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boren.TWComponentPricing.Worker.Service
{
    public class CrawlerService : ICrawlerService
    {
        private const string COOLPC_URL = "https://www.coolpc.com.tw/evaluate.php";

        private async Task<(Browser Browser, Page Page)> GetWebAsync()
        {
            await new BrowserFetcher().DownloadAsync();
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false });
            var page = (await browser.PagesAsync()).First();
            await page.SetViewportAsync(new ViewPortOptions { Width = 1024, Height = 768 });
            await page.GoToAsync(COOLPC_URL);
            await page.WaitForTimeoutAsync(3000);

            return (browser, page);
        }

        public async Task<IList<object>> GetAsync()
        {
            var web = await this.GetWebAsync();
            await web.Page.WaitForTimeoutAsync(3000);

            var html = await web.Page.GetContentAsync();
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var trs = document.DocumentNode.SelectNodes("#tbdy tr");
            foreach(var tr in trs)
            {
                var title = tr.ChildNodes.SingleOrDefault(c => c.HasClass("t"));
                //var title = (await tr.("td.t"));
            }

            // table id : Tfix
            throw new NotImplementedException();
        }
    }
}
