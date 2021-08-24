using System;
using System.Collections.Generic;

#nullable disable

namespace Boren.TWComponentPricing.Data
{
    public partial class Detail
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
        public string[] Remarks { get; set; }

        public virtual Product Product { get; set; }
    }
}
