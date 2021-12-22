using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace LocalTests
{

    /// <summary>
    /// This class holds all the api calls to Arrows XSP api. 
    /// </summary>

    public class ArrowApi
    {
        //Data used for getting access to Arrows, api. 
        private string defaultUri = "https://xsp.arrow.com";
        private string apiSecret = "DanL33d010203";
        private string apiKey = "wSmewivOVUmWSvnOHHXJviUYobByYQYe";
        private string s = "";

        //private List<ProductData> productList = new List<ProductData>();
        private List<Products> finalProductList = new List<Products>();
        private List<Products> UpdateProductList = new List<Products>();
        List<Customers> usedCustomersFound = new List<Customers>();

        private List<Products> listBelowCost = new List<Products>();

        //TODO - Remove when transistion from Legacy to Azure plan is done
        private string AzureSku = "MS-AZR-0145P";
        public string AzurePlanSku = "DZH318Z0BPS6:0001";

        private static bool RunningBool = false;

        //Class definition for sqldbconnections. 

        private IRestResponse GetCall(RestClient clientData)
        {
            try
            {

                //Setting up request. Change method if needed. 
                var requestDraftData = new RestRequest(Method.GET);
                requestDraftData.AddHeader("Host", "xsp.arrow.com");
                requestDraftData.AddHeader("Apikey", apiKey);
                requestDraftData.AddHeader("Signature", s);
                requestDraftData.AddHeader("Content-Type", "application/json");


                return clientData.Execute(requestDraftData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ArrowCustomerLicences GetCustomerLicences(string customerArrowId)
        {
            try
            {
                int amount = 0;
                ArrowCustomerLicences producList = new ArrowCustomerLicences();

                //Get products for single customer. 
                while (amount != 3)
                {
                    RestClient clientData = new RestClient(defaultUri + $"/index.php/api/customers/{customerArrowId}/licenses");
                    var customerResponce = GetCall(clientData);
                    producList = JsonConvert.DeserializeObject<ArrowCustomerLicences>(customerResponce.Content);

                    if (customerResponce.StatusCode == HttpStatusCode.OK)
                    {
                        if (customerResponce.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception();
                        }

                        break;
                    }
                    amount++;
                    Thread.Sleep(5000);
                }
                if (producList.Status != 200) //in case that the app failed to get data when it gets to here.
                    throw new HttpListenerException((int)producList.Status, "Failed to fetch data from Arrow.");

                return producList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ArrowProductHistory GetHistory(string prodRef)
        {
            try
            {
                //Get History for a customers product. 

                int amount = 0;
                ArrowProductHistory hist = new ArrowProductHistory();

                while (amount != 3)
                {
                    RestClient Client = new RestClient(defaultUri + $"/index.php/api/licenses/{prodRef}/history");
                    var productHistory = GetCall(Client);
                    if (productHistory.StatusCode == HttpStatusCode.OK)
                    {
                        hist = JsonConvert.DeserializeObject<ArrowProductHistory>(productHistory.Content);
                        break;
                    }

                    amount++;
                    Thread.Sleep(5000);
                }
                if (hist.Status != 200) //in case that the app failed to get data when it gets to here.
                    throw new HttpListenerException((int)hist.Status, "Failed to fetch data from Arrow.");
                else
                    return hist;

            }
            catch (HttpListenerException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
              
                throw;
            }
        }
    }
}
