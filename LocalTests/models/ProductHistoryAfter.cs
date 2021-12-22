
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalTests
{
    public partial class ProductHistoryAfter
    {

        public long? ActiveSeats { get; set; }

        public long? Seats { get; set; }

        public string State { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }

        public Guid? Sku { get; set; }

        public long? BaseSeats { get; set; }

        public DateTimeOffset? ActivationDate { get; set; }
    }
}
