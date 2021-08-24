using System;
using System.Collections.Generic;

#nullable disable

namespace Boren.TWComponentPricing.Data
{
    public partial class Product
    {
        public Product()
        {
            Details = new HashSet<Detail>();
        }

        public int Id { get; set; }
        public string OriginText { get; set; }
        public int CategroyId { get; set; }
        public int? BrandId { get; set; }
        public string FixedText { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Categroy Categroy { get; set; }
        public virtual ICollection<Detail> Details { get; set; }
    }
}
