using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalTests
{
    public class ProductsSettings
    {
        public bool VmSupport { get; set; }
        public bool VpnSupport { get; set; }
        public bool UserSupport { get; set; }
        public bool BackupSupport { get; set; }
        public bool PrintxSupport { get; set; }
        public bool SignatureSupport { get; set; }
        public bool ownProduct { get; set; }
        public bool MailSupport { get; set; }
        public bool SupportProd { get; set; }
        public string draftSelected { get; set; }

        public ProductsSettings copy()
        {
            ProductsSettings settings = new ProductsSettings();
            settings.VmSupport = VmSupport;
            settings.VpnSupport = VpnSupport;
            settings.UserSupport = UserSupport;
            settings.BackupSupport = BackupSupport;
            settings.PrintxSupport = PrintxSupport;
            settings.SignatureSupport = SignatureSupport;
            settings.ownProduct = ownProduct;
            settings.MailSupport = MailSupport;
            settings.SupportProd = SupportProd;
            return settings;
        }
    }
}
