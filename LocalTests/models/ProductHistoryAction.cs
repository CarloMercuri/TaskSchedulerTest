
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalTests
{
    public partial class ProductHistoryAction
    {
        public string Action{ get; set; }

        public List<ProductHistoryNote> Note { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public bool treated { get; set; }
    }

}
