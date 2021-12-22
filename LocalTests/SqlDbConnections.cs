using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTests
{
    public class SqlDbConnections
    {
        string connString = "Server = tdkbilling.database.windows.net; Database = Test_M365_Billing; User Id = tdkbillinguser; Password = LRiy_V45psNM9wO; MultipleActiveResultSets=True;";
        string customerConnString = "Server = tdkbilling.database.windows.net; Database = Test_BillingCustomers; User Id = tdkbillinguser; Password = LRiy_V45psNM9wO; MultipleActiveResultSets=True;";

        public SqlDbConnections(bool testMode)
        {

        }

        public DataTable GetDailyLicenceCheckSettings()
        {
            DataTable data = new DataTable();

            try
            {
                data = Query(GetM365Connection(), "SELECT * FROM DailyActionsCheckSettings");
            }
            catch (Exception ex)
            {

               
                throw;
            }

            return data;


        }

        public List<Customers> GetCustomersFromDb()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(customerConnString))
                {
                    con.Open();

                    List<Customers> customers = new List<Customers>();
                    SqlCommand com = new SqlCommand("Select * from dbo.Customers ORDER BY CustomerName");

                    com.Connection = con;

                    using (SqlDataAdapter da = new SqlDataAdapter(com))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            for (int e = 0; e < dt.Rows.Count; e++)
                            {
                                Customers item = new Customers();

                                item.Id = Convert.ToString(dt.Rows[e]["Id"]);
                                item.Name = Convert.ToString(dt.Rows[e]["CustomerName"]);
                                item.ArrowId = Convert.ToString(dt.Rows[e]["ArrowId"]);
                                item.EconomicsId = Convert.ToString(dt.Rows[e]["EconomicsId"]);
                                item.Rekv = Convert.ToString(dt.Rows[e]["RekvNumber"]);
                                item.Note = Convert.ToString(dt.Rows[e]["Note"]);
                                item.IpvisionDomain = Convert.ToString(dt.Rows[e]["IpvisibleDomain"]);
                                item.M365DraftNote = Convert.ToString(dt.Rows[e]["M365DraftNote"]);
                                item.RekvNumberM365 = Convert.ToString(dt.Rows[e]["RekvNumberM365"]);
                                item.TelepoDraftNote = Convert.ToString(dt.Rows[e]["TelepoDraftNote"]);
                                item.RekvNumberTelepo = Convert.ToString(dt.Rows[e]["RekvNumberTelepo"]);
                                item.ArrowDeleted = dt.Rows[e]["ArrowDeleted"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[e]["ArrowDeleted"]) : false;
                                item.TelepoDeleted = dt.Rows[e]["TelepoDeleted"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[e]["TelepoDeleted"]) : false;
                                item.Barred = Convert.ToBoolean(dt.Rows[e]["Barred"]);

                                customers.Add(item);
                            }
                        }
                    }
                    return customers;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable Query(SqlConnection connection, string query, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                foreach (SqlParameter parameter in parameters)
                    command.Parameters.Add(parameter);

                connection.Open();

                DataTable dataTable = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(command))
                    da.Fill(dataTable);

                connection.Close();
                return dataTable;
            }
        }

        public SqlConnection GetM365Connection()
        {
            return new SqlConnection(connString);
        }

        public void InsertSchedule(IScheduledTask task)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Schedules (task_name, startdate, schedule_type, schedule_interval, execution_timeofday, last_run, next_execution)" +
                        " VALUES (@name, @startdate, @scheduleType, @scheduleInterval, @executionTimeOfDay, @lastRun, @nextExecution)");
                    cmd.Parameters.AddWithValue("@name", task.TaskName);
                    cmd.Parameters.AddWithValue("@startdate", task.StartDate);
                    cmd.Parameters.AddWithValue("@scheduleType", task.ScheduleType);
                    cmd.Parameters.AddWithValue("@scheduleInterval", task.ScheduleInterval);
                    cmd.Parameters.AddWithValue("@executionTimeOfDay", task.ExecutionTimeOfDay);
                    cmd.Parameters.AddWithValue("@lastRun", task.LastRun);
                    cmd.Parameters.AddWithValue("@nextExecution", task.NextExecution);
                    cmd.Connection = con;

                    cmd.ExecuteNonQuery();


                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetSchedules()
        {
            try
            {
                using(SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Schedules");
                    cmd.Connection = con;

                    using(SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
