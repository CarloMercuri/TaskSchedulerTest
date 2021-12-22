using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalTests
{
    public class CustomerLicences
    {
        public int dbId { get; set; }

        public string LicenseId { get; set; }

        public object ParentLicenseId { get; set; }

        public string FriendlyName { get; set; }

        public string CustomerRef { get; set; }

        public string ServiceRef { get; set; }

        public string Sku { get; set; }

        public string Name { get; set; }

        //[JsonConverter(typeof(ParseStringConverter))]
        public long Seats { get; set; }

        //[JsonProperty("activeSeats")]
        //public ActiveSeats ActiveSeats { get; set; }

        public DateTimeOffset ActivationDatetime { get; set; }

        public DateTimeOffset ExpiryDatetime { get; set; }

        public string State { get; set; }

        public string Periodicity { get; set; }

        public CustomerLicenseActions Actions { get; set; }

        public string Category { get; set; }

        public string Program { get; set; }

        //[JsonProperty("order")]
        //public Order Order { get; set; }
    }
}
