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
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            var page = (await browser.PagesAsync()).First();
            //await page.SetViewportAsync(new ViewPortOptions { Width = 1024, Height = 768 });
            await page.GoToAsync(COOLPC_URL);
            await page.WaitForTimeoutAsync(3000);

            return (browser, page);
        }

        public async Task<IList<object>> GetAsync()
        {
            // DOM 操作
            var web = await this.GetWebAsync();
            await web.Page.WaitForTimeoutAsync(1000);

            // 商品類別 #tbdy tr
            var categories = await web.Page.QuerySelectorAllAsync("#tbdy tr");
            foreach (var category in categories)
            {
                // 類別名稱 tr.t
                var categoryName = await category.QuerySelectorAsync(".t").EvaluateFunctionAsync<string>("e => e.innerHTML");
                // 商品 td select.s optgroup option
                var items = await category.QuerySelectorAllAsync("td select.s optgroup option");

                // 2021.08.17 
                // todo: 把列表轉換成物件
                // todo: 資料庫結構規劃
                var itemNames = new List<string>();
                foreach (var item in items)
                {
                    var text = await item.EvaluateFunctionAsync<string>("e => e.innerHTML");
                    var clss = await (await item.GetPropertyAsync("className")).JsonValueAsync<string>();
                    itemNames.Add(text);
                }
            }

            // table id : Tfix
            throw new NotImplementedException();
        }
    }
}
