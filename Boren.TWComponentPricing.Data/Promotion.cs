using System;
using System.Collections.Generic;

#nullable disable

namespace Boren.TWComponentPricing.Data
{
    public partial class Promotion
    {
        public int Id { get; set; }
        public int? DetailId { get; set; }
        public string OriginText { get; set; }
        public string FixedText { get; set; }

        public virtual Detail Detail { get; set; }
    }
}
