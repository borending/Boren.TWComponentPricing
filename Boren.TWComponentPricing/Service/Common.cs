using Boren.TWComponentPricing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boren.TWComponentPricing.Service
{
    public class Common
    {
        public static FeatureType? GetType(string className)
        {
            FeatureType? type = null;
            switch (className)
            {
                case "r":
                    type = FeatureType.TopSelling;
                    break;
                case "g":
                    type = FeatureType.PriceChange;
                    break;
                case "b":
                    type = FeatureType.Both;
                    break;
                default:
                    break;
            }

            return type;
        }
    }
}
