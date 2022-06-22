using System;
using System.Collections.Generic;

#nullable disable

namespace Boren.TWComponentPricing.Data
{
    public partial class Categroy
    {
        public Categroy()
        {
            Products = new HashSet<Product>();
            Raws = new HashSet<Raw>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Raw> Raws { get; set; }
    }
}
