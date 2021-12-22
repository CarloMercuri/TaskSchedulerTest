using System;
using System.Collections.Generic;

namespace LocalTests
{
    public class CustomerNoAzureProducts : ICustomerProductsModel
    {
        public List<Products> Products { get; set; }
        private string AzureSku = "MS-AZR-0145P";
        private string AzurePlanSku = "DZH318Z0BPS6:0001";

        public void FillProductsList(ArrowCustomerLicences licences)
        {
            Products = new List<Products>();

            foreach (var license in licences.Data.Licenses)
            {
                //Simple solution to remove unused Azure products. 
                if (license.Sku == AzureSku || license.Sku == AzurePlanSku)
                {
                    // Dont count azure products?

                }
                else
                {
                    // Keep products that have been canceled or deleted
                    // Need to check if they have been deleted or canceled the day before

                    Products prod = new Products() { productsSettings = new ProductsSettings() };
                    prod.name = license.Name.Replace("(Legacy)", "");
                    prod.sku = license.Sku;
                    prod.LastUpdated = DateTime.Now.ToLocalTime();
                    prod.productsSettings.ownProduct = false;
                    prod.startValue = license.Seats.ToString();
                    prod.referance = license.LicenseId;
                    prod.friendlyName = license.FriendlyName;

                    //Defining default product values to be set later

                    prod.costPrice = "N/A";
                    prod.costYearPrice = "N/A";
                    prod.listPrice = "N/A";
                    prod.listYearPrice = "N/A";
                    prod.ownPrice = "";
                    prod.specialPrice = "";
                    prod.totalPrice = "";
                    Products.Add(prod);

                }
            }

        }
    }
}
