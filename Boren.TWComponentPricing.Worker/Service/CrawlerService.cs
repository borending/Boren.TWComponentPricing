using AngleSharp;
using AngleSharp.Html.Parser;
using Boren.TWComponentPricing.Data;
using Boren.TWComponentPricing.Service;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
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
            // todo: 決定要取哪一個分類

            // DOM 操作
            var web = await this.GetWebAsync();
            await web.Page.WaitForTimeoutAsync(1000);

            // 商品類別 #tbdy tr
            var entities = new List<Data.Product>();

            var context = BrowsingContext.New(Configuration.Default);
            var parser = context.GetService<IHtmlParser>();
            var document = parser.ParseDocument(await web.Page.GetContentAsync());

            var builder = new StringBuilder();
            var categories = document.QuerySelectorAll("#tbdy > tr");
            foreach (var category in categories)
            {
                builder.Clear();

                try
                {
                    var categoryNumber = Convert.ToInt16(category.QuerySelector(".w").TextContent);
                    var categoryName = category.QuerySelector(".t").TextContent;
                    builder.AppendLine($"categoryName: {categoryName}");

                    if (Convert.ToInt32(categoryNumber) != 4)
                        continue;

                    var categroy = new Categroy { Number = categoryNumber, Name = categoryName };

                    // 商品群組 td select.s optgroup
                    var groups = category.QuerySelectorAll("td select.s optgroup");
                    foreach (var group in groups)
                    {
                        Data.Detail pointer = null;
                        var items = group.QuerySelectorAll("option");
                        foreach (var item in items)
                        {
                            var disabled = item.HasAttribute("disabled");
                            string text = item.TextContent;
                            builder.AppendLine($"item OriginText: {text}");
                            // 如果沒箭頭且有寫金額，視為商品
                            if (!text.Contains("↪") && Regex.IsMatch(text, @"\$\d+"))
                            {
                                if (disabled)
                                {
                                    // 如果disabled 就是無效商品，要把pointer 清掉，並繼續下一步驟
                                    pointer = null;
                                    continue;
                                }

                                var className = string.IsNullOrEmpty(item.ClassName) ? null : item.ClassName;
                                // 可能有多個價錢，只記錄最後一個
                                var price = Regex.Matches(text, @"\$\d+").Last().Value;
                                var detail = new Detail
                                {
                                    Price = Convert.ToDecimal(price.Replace("$", "")),
                                    DateTime = DateTime.Today,
                                    FeatureType = Common.GetType(className)
                                };
                                pointer = detail;
                                var product = new Data.Product
                                {
                                    OriginText = text,
                                    Categroy = categroy,
                                    //FixedText = text.Substring(0, text.IndexOf(",")),
                                    Details = new List<Detail> { detail }
                                };

                                entities.Add(product);
                            }
                            else if (text.IndexOf("↪") != -1 && disabled && pointer != null)
                            {
                                // 如果有箭頭又是disabled 就是為贈品，加入Remarks
                                IList<string> list = pointer.Remarks != null ? pointer.Remarks.ToList() : new List<string>();
                                text = text.TrimStart().Replace("↪", "");
                                list.Add(text);
                                pointer.Remarks = list.ToArray();
                            }
                            else if (pointer == null)
                            {
                                // 如果沒有pointer ，又是disable，那應該是促銷內容
                                // 再看要怎麼紀錄
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    builder.AppendLine(e.Message);
                    builder.AppendLine(e.StackTrace);
                    if (e.InnerException != null)
                        builder.AppendLine(e.InnerException.Message);

                    return null;
                }
            }

            return entities;
        }

        public async Task SetAsync(IList<Data.Product> products)
        {
            var dbContext = new PricingDbContext();
            var existProducts = dbContext.Products.Include("Categroy").Include("Details").Select(x => x).ToList();

            foreach (var product in products)
            {
                var exists = existProducts.Any(x => x.OriginText == product.OriginText && x.Categroy.Name == product.Categroy.Name);
                if (exists)
                {
                    // update
                }
                else
                {
                    dbContext.Add(product);
                }
            }

            var result = false;
            try
            {
                await dbContext.SaveChangesAsync();
                result = true;
            }
            catch (Exception e)
            {

            }
        }

        public async Task GetV2Async()
        {
            // todo: 決定要取哪一個分類

            // DOM 操作
            var web = await this.GetWebAsync();
            await web.Page.WaitForTimeoutAsync(1000);

            // 商品類別 #tbdy tr
            var entities = new List<Data.Product>();

            var context = BrowsingContext.New(Configuration.Default);
            var parser = context.GetService<IHtmlParser>();
            var document = parser.ParseDocument(await web.Page.GetContentAsync());

            var builder = new StringBuilder();
            var categories = document.QuerySelectorAll("#tbdy > tr");
            foreach (var category in categories)
            {
                builder.Clear();

                try
                {
                    var categoryNumber = Convert.ToInt16(category.QuerySelector(".w").TextContent);
                    var categoryName = category.QuerySelector(".t").TextContent;
                    builder.AppendLine($"categoryName: {categoryName}");

                    if (Convert.ToInt32(categoryNumber) != 4)
                        continue;

                    var categroy = new Categroy { Number = categoryNumber, Name = categoryName };

                    // 商品群組 td select.s optgroup
                    var groups = category.QuerySelectorAll("td select.s optgroup");
                    foreach (var group in groups)
                    {
                        Data.Detail pointer = null;
                        var items = group.QuerySelectorAll("option");
                        foreach (var item in items)
                        {
                            var disabled = item.HasAttribute("disabled");
                            string text = item.TextContent;
                            builder.AppendLine($"item OriginText: {text}");
                            // 如果沒箭頭且有寫金額，視為商品
                            if (!text.Contains("↪") && Regex.IsMatch(text, @"\$\d+"))
                            {
                                if (disabled)
                                {
                                    // 如果disabled 就是無效商品，要把pointer 清掉，並繼續下一步驟
                                    pointer = null;
                                    continue;
                                }

                                var className = string.IsNullOrEmpty(item.ClassName) ? null : item.ClassName;
                                // 可能有多個價錢，只記錄最後一個
                                var price = Regex.Matches(text, @"\$\d+").Last().Value;
                                var detail = new Detail
                                {
                                    Price = Convert.ToDecimal(price.Replace("$", "")),
                                    DateTime = DateTime.Today,
                                    FeatureType = Common.GetType(className)
                                };
                                pointer = detail;
                                var product = new Data.Product
                                {
                                    OriginText = text,
                                    Categroy = categroy,
                                    //FixedText = text.Substring(0, text.IndexOf(",")),
                                    Details = new List<Detail> { detail }
                                };

                                entities.Add(product);
                            }
                            else if (text.IndexOf("↪") != -1 && disabled && pointer != null)
                            {
                                // 如果有箭頭又是disabled 就是為贈品，加入Remarks
                                IList<string> list = pointer.Remarks != null ? pointer.Remarks.ToList() : new List<string>();
                                text = text.TrimStart().Replace("↪", "");
                                list.Add(text);
                                pointer.Remarks = list.ToArray();
                            }
                            else if (pointer == null)
                            {
                                // 如果沒有pointer ，又是disable，那應該是促銷內容
                                // 再看要怎麼紀錄
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    builder.AppendLine(e.Message);
                    builder.AppendLine(e.StackTrace);
                    if (e.InnerException != null)
                        builder.AppendLine(e.InnerException.Message);

                    return;
                }
            }
        }
    }
}
