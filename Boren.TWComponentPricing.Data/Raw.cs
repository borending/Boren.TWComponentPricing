using System;
using System.Collections.Generic;

#nullable disable

namespace Boren.TWComponentPricing.Data
{
    public partial class Raw
    {
        public Raw()
        {
            InverseRawNavigation = new HashSet<Raw>();
        }

        public int Id { get; set; }
        public int? RawId { get; set; }
        public int CategroyId { get; set; }
        public DateTime Time { get; set; }
        public bool Done { get; set; }
        public string FixedText { get; set; }

        public virtual Categroy Categroy { get; set; }
        public virtual Raw RawNavigation { get; set; }
        public virtual ICollection<Raw> InverseRawNavigation { get; set; }
    }
}
