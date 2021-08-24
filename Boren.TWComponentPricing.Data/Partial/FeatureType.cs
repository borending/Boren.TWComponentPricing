namespace Boren.TWComponentPricing.Data
{
    public enum FeatureType : int
    {
        /// <summary>
        /// 熱賣
        /// </summary>
        TopSelling = 1,

        /// <summary>
        /// 價格異動
        /// </summary>
        PriceChange = 2,

        /// <summary>
        /// 熱賣+價格異動
        /// </summary>
        Both = 3
    }
}
