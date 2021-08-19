using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boren.TWComponentPricing.Model
{
    public class ItemViewModel
    {
        public string Categroy { get; set; }

        public int Value { get; set; }

        public int? ParentValue { get; set; }

        public string Text { get; set; }

        public string ClassName { get; set; }
    }
}
