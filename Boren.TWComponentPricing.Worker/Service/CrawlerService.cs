using Boren.TWComponentPricing.Model;
using Boren.TWComponentPricing.Service;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public async Task<IList<Data.Product>> GetAsync()
        {
            // DOM 操作
            var web = await this.GetWebAsync();
            await web.Page.WaitForTimeoutAsync(1000);

            // 商品類別 #tbdy tr
            var entities = new List<Data.Product>();
            var categories = await web.Page.QuerySelectorAllAsync("#tbdy tr");
            try
            {
                foreach (var category in categories)
                {
                    // 類別名稱 tr.t
                    var categoryName = await category.QuerySelectorAsync(".t").EvaluateFunctionAsync<string>("e => e.innerHTML");
                    // 商品群組 td select.s optgroup
                    var groups = await category.QuerySelectorAllAsync("td select.s optgroup");

                    foreach (var group in groups)
                    {
                        Data.Detail pointer = null;
                        var items = await group.QuerySelectorAllAsync("option");
                        foreach (var item in items)
                        {
                            var text = await item.EvaluateFunctionAsync<string>("e => e.innerHTML");
                            // 如果沒縮排且有寫金額，視為商品
                            if (text.IndexOf("↪") == -1 && Regex.IsMatch(text, @"$\d+"))
                            {
                                var attr = await item.EvaluateFunctionAsync<string>("e => e.className");
                                var className = string.IsNullOrEmpty(attr) ? null : attr;

                                var detail = new Data.Detail
                                {
                                    Price = Convert.ToDecimal(Regex.Matches(text, @"$\d+").Last()),
                                    DateTime = DateTime.Today,
                                    FeatureType = Common.GetType(className)
                                };
                                pointer = detail;
                                var product = new Data.Product
                                {
                                    OriginText = text,
                                    Categroy = new Data.Categroy { Name = categoryName },
                                    FixedText = text.Substring(0, text.IndexOf(",")),
                                    Details = new List<Data.Detail> { detail }
                                };

                                entities.Add(product);
                            }
                            else
                            {
                                IList<string> list = pointer.Remarks != null ? pointer.Remarks.ToList() : new List<string>();
                                list.Add(text);
                                pointer.Remarks = list.ToArray();
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                var tt = e.Message;
            }

            return entities;
        }
    }
}
