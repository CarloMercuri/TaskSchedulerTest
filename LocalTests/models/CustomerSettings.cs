using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalTests
{
    public class CustomerSettings
    {
        public bool VmSupport { get; set; }
        public bool VpnSupport { get; set; }
        public bool UserSupport { get; set; }
        public bool BackupSupport { get; set; }
        public bool PrintxSupport { get; set; }
        public bool SignatureSupport { get; set; }
        public bool AzureDetails { get; set; }
        public bool Include365 { get; set; } = true;
        public bool IncludeAzure { get; set; } = true;
        public bool MailSupport { get; set; }
        public bool WVDSupport { get; set; }
        public bool DraftSplit { get; set; }
        public string LegacyAzureId { get; set; } = ""; //Defaults to empty string.
    }
}
