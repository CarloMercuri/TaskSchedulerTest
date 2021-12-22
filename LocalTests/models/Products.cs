using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalTests
{
    public class Products
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sku { get; set; }
        public DateTime LastUpdated { get; set; }
        public string specialPrice { get; set; }
        public string costPrice { get; set; }
        public string listPrice { get; set; }
        public string costYearPrice { get; set; }
        public string listYearPrice { get; set; }
        public string ownPrice { get; set; }
        public string totalPrice { get; set; }
        public ProductsSettings productsSettings { get; set; }
        public string referance { get; set; }
        public string friendlyName { get; set; }
        public string startValue { get; set; } //value at the start of the period. 
        public string peak { get; set; } //Peak is the highest value achieved in the period. 
        public string current { get; set; } //Current reprecents the total value at the end of the period.
        public string amount { get; set; } //amount is the number of products that is for the current line.
        public string startDate { get; set; } //Used for history check.
        public string endDate { get; set; } //Used for history check.
        public string note { get; set; }
        public string description { get; set; }
        public string tenantId { get; set; }
        public bool IsAzureData { get; set; }
        public bool IsCustomLicense { get; set; }

        public ArrowProductHistory productHistory { get; set; }

        public Products copy()
        {
            Products prod = new Products();
            prod.id = id;
            prod.name = name;
            prod.sku = name;
            prod.LastUpdated = LastUpdated;
            prod.specialPrice = specialPrice;
            prod.costPrice = costPrice;
            prod.listPrice = listPrice;
            prod.costYearPrice = costYearPrice;
            prod.listYearPrice = listYearPrice;
            prod.ownPrice = ownPrice;
            prod.totalPrice = totalPrice;
            prod.productsSettings = productsSettings.copy();
            prod.referance = referance;
            prod.friendlyName = friendlyName;
            prod.startValue = startValue;
            prod.peak = peak;
            prod.current = current;
            prod.amount = amount;
            prod.startDate = startDate;
            prod.endDate = endDate;
            prod.note = note;
            prod.description = description;
            prod.tenantId = tenantId;
            prod.IsCustomLicense = IsCustomLicense;
            prod.IsAzureData = IsAzureData;
            prod.productHistory = productHistory;

            return prod;
        }
    }
}
