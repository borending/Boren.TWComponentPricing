using System;
using System.Collections.Generic;

#nullable disable

namespace Boren.TWComponentPricing.Data
{
    public partial class Price
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price1 { get; set; }
        public DateTime DateTime { get; set; }

        public virtual Product Product { get; set; }
    }
}
