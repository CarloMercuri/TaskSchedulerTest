using System.Collections.Generic;

namespace LocalTests
{
    public interface ICustomerProductsModel
    {
        public List<Products> Products { get; set; }

        public void FillProductsList(ArrowCustomerLicences licences);
    }
}
