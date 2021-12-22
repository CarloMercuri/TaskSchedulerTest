using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTests
{
    public class ActionCheckTask : IScheduledTask
    {
        public string TaskName { get; set; }
        public bool isRunning { get; set; }
        public bool isCanceled { get; set; }
        public int PercentCompletion { get; }
        public char ScheduleType { get; set; }
        public string ScheduleInterval { get; set; }
        public DateTime StartDate { get; set; }
        public string ExecutionTimeOfDay { get; set; }
        public DateTime LastRun { get; set; }
        public DateTime NextExecution { get; set; }

        private int testint = 0;

        private ArrowApi arrow = new ArrowApi();

        public string NextCycleTime()
        {
            throw new NotImplementedException();
        }

        public TaskResults StartTask()
        {
            isRunning = true;
            TaskResults results = new TaskResults();
            results.TaskName = TaskName;
            results.TimeStarted = DateTime.Now;

            Stopwatch taskTimer = Stopwatch.StartNew();

            try
            {
                CheckCustomerHistory();
            }
            catch (Exception ex)
            {
                taskTimer.Stop();
                results.Success = false;
                results.ExecutionTime = taskTimer.Elapsed.TotalSeconds;
                results.Message = $"Task '{TaskName}' failed after {taskTimer.Elapsed.TotalSeconds} seconds. Error: {ex.Message}";
                isRunning = false;
                return results;
            }

            taskTimer.Stop();
            results.ExecutionTime = taskTimer.Elapsed.TotalSeconds;
            results.Message = $"Task '{TaskName}' executed successfuly. Elapsed time: {results.ExecutionTime}";
            results.Success = true;
            isRunning = false;
            return results;
        }

        public List<Customers> GetCustomersArrow()
        {
            List<Customers> filteredList = new List<Customers>();

            try
            {
                SqlDbConnections con = new SqlDbConnections(true); // TEST mode
                List<Customers> customers = con.GetCustomersFromDb();

                foreach (var customer in customers)
                {
                    if (customer.EconomicsId != "" && customer.ArrowId != "")
                    {
                        if (customer.ArrowId != "XSP565181") // Test customer
                        {
                            filteredList.Add(customer);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }

            return filteredList;
        }

        public void CheckCustomerHistory()
        {
            Console.WriteLine();


            HistoryCheckLog mainLog = new HistoryCheckLog();

            List<Customers> customers = GetCustomersArrow();

            DateTime lastCheck = DateTime.Now;




            SqlDbConnections con = new SqlDbConnections(true); // TEST mode

            DataTable data = con.GetDailyLicenceCheckSettings();

            Dictionary<string, bool> settings = new Dictionary<string, bool>();


            if (data.Rows.Count > 0)
            {
                settings["updated_assigned_seats"] = Convert.ToBoolean(data.Rows[0]["updated_assigned_seats"]);
                settings["updated_seats_increase"] = Convert.ToBoolean(data.Rows[0]["updated_seats_increase"]);
                settings["updated_seats_decrease"] = Convert.ToBoolean(data.Rows[0]["updated_seats_decrease"]);
                settings["updated"] = Convert.ToBoolean(data.Rows[0]["updated"]);
                settings["activation_success"] = Convert.ToBoolean(data.Rows[0]["activation_success"]);
                settings["in_progress"] = Convert.ToBoolean(data.Rows[0]["in_progress"]);
                settings["created"] = Convert.ToBoolean(data.Rows[0]["created"]);
                settings["renewal"] = Convert.ToBoolean(data.Rows[0]["renewal"]);
                settings["suspended"] = Convert.ToBoolean(data.Rows[0]["suspended"]);
                settings["activation_failure"] = Convert.ToBoolean(data.Rows[0]["activation_failure"]);
                settings["updated_friendly_name"] = Convert.ToBoolean(data.Rows[0]["updated_friendly_name"]);
                lastCheck = DateTime.Parse(data.Rows[0]["last_check"].ToString());

            }


            MultiThreading.For(0, customers.Count, i =>
            {
                CustomerNoAzureProducts customerNoAzureProducts = new CustomerNoAzureProducts();
                customers[i].Products = GetCustomerProducts(customers[i].ArrowId, customerNoAzureProducts).Products;
                //customers[i].Products = GetProductsForCustomer(customers[i].ArrowId);

                MultiThreading.For(0, customers[i].Products.Count, j =>
                {
                    ArrowProductHistory history = arrow.GetHistory(customers[i].Products[j].referance);
                    //customers[i].Products[j].productHistory = arrow.GetHistory(customers[i].Products[j].referance);

                    foreach (var action in history.Data.Actions)
                    {
                        if (action.CreatedAt >= lastCheck)
                        {
                            HistoryCheckLogElement el = new HistoryCheckLogElement();
                            el.Action = action;
                            el.CustomerId = customers[i].ArrowId;
                            el.CustomerName = customers[i].Name;
                            el.ProductId = customers[i].Products[j].referance;
                            el.ProductName = customers[i].Products[j].name;
                            mainLog.Elements.Add(el);
                        }
                    }
                });

            });

            List<HistoryCheckLogElement> logElements = new List<HistoryCheckLogElement>();

            foreach (HistoryCheckLogElement element in mainLog.Elements)
            {
                if (settings.ContainsKey(element.Action.Action))
                {
                    if (settings[element.Action.Action] == true)
                    {
                        logElements.Add(element);
                    }
                }
            }



            Console.WriteLine();

        }

        public ICustomerProductsModel GetCustomerProducts(string arrowId, ICustomerProductsModel model)
        {
            try
            {
                // Get the raw licence data from Arrow
                ArrowCustomerLicences customerLicences = arrow.GetCustomerLicences(arrowId);
                model.FillProductsList(customerLicences);
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public void StopTask()
        {
            isCanceled = true;
            isRunning = false;
        }

        public void UpdateNextTick(DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
