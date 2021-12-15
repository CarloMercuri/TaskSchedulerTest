// See https://aka.ms/new-console-template for more information
using LocalTests;
using System.Data.SqlClient;
using System.Diagnostics;

string connString = "Server = tdkbilling.database.windows.net; Database = Test_M365_Billing; User Id = tdkbillinguser; Password = LRiy_V45psNM9wO; MultipleActiveResultSets=True;";

string time = "07:00";

//string[] s = time.Split(':');
//int hour = Convert.ToInt32(s[0]);
//int minute = Convert.ToInt32(s[1]);

//TimeSpan ts = new TimeSpan(hour, minute, 0);

//Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd" + "  kl. " + "HH:mm")}");



Timer timer = new Timer((e) =>
{
    TickTock();
}, null, 1000, 1000);



while (true)
{
    Console.WriteLine("OutsideTimer");
    Thread.Sleep(500);
    Console.WriteLine($"Main thread, id: {Thread.CurrentThread.ManagedThreadId}");
}

void TickTock()
{

    Console.WriteLine($"TickTock on thread: {Thread.CurrentThread.ManagedThreadId}");



}


double CalculateMillisecondsToTime()
{
    DateTime nowTime = DateTime.Now;
    TimeSpan difference = _shutdownTime.TimeOfDay - nowTime.TimeOfDay;
    double result = difference.TotalMilliseconds;
    if (result <= 0)
        result += TimeSpan.FromHours(24).TotalMilliseconds;
}



object QueryScalar(SqlConnection connection, string query, bool logChanges, bool isStoredProcedure, params SqlParameter[] parameters)
{

    using (SqlCommand command = new SqlCommand(query, connection))
    {
        foreach (SqlParameter parameter in parameters)
        {
            if (parameter.Value != null)
                command.Parameters.Add(parameter);
        }

        connection.Open();
        object result = command.ExecuteScalar();
        connection.Close();
        return result;
    }
}

Console.ReadLine();
