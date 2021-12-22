using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalTests
{
    public class CustomerLicenseActions
    {
        public Uri Update { get; set; }

        public Uri History { get; set; }

        public Uri AddonsCatalog { get; set; }

        public Uri Suspend { get; set; }

        public Uri Reactivate { get; set; }
    }
}
