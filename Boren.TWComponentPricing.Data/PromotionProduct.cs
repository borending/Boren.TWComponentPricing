using System;
using System.Collections.Generic;

#nullable disable

namespace Boren.TWComponentPricing.Data
{
    public partial class PromotionProduct
    {
        public int PromotionId { get; set; }
        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
