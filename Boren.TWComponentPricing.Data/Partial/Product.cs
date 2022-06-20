using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Boren.TWComponentPricing.Data
{
    public partial class Product
    {
        public string GetFixedText()
        {
            string fixedText = Regex.Replace(this.OriginText, @"【.*?】", " ");
            fixedText = Regex.Replace(fixedText, @"\(.*?\)", "");
            fixedText = Regex.Replace(fixedText, @"(?:[\w\.]+\/)+[\w\.]+", "");

            var trims = new List<string> { ",", "贈", "+" };
            trims.ForEach(target =>
            {
                if (fixedText.Contains(target))
                    fixedText = fixedText.Substring(0, fixedText.LastIndexOf(target));
            });

            return fixedText.Trim();
        }
    }
}
