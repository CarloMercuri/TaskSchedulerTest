// See https://aka.ms/new-console-template for more information
using LocalTests;
using System.Data.SqlClient;
using System.Diagnostics;


string time = "07:43";
DateTime now = DateTime.Parse("2021-12-21 08:23");

Console.WriteLine(now);

//ScheduleManager.InitializeScheduler();

string[] s = time.Split(':');
int hour = Convert.ToInt32(s[0]);
int minute = Convert.ToInt32(s[1]);

DateTime next = DateTime.MinValue;

if (now.Minute < minute)
{
    next = now.AddMinutes(minute - now.Minute).AddSeconds(-now.Second);
}
else
{
    next = now.AddHours(1).AddMinutes(minute - now.Minute).AddSeconds(-now.Second);
}

Console.WriteLine(next);



//while (true)
//{
//    Console.ReadKey();

//    DateTime nowTime = DateTime.Now;

//    DateTime newMinute = DateTime.Now.AddMinutes(1).AddSeconds(-DateTime.Now.Second).AddSeconds(2);

//    Console.WriteLine("now: " + nowTime + " ------- new: " +  newMinute);
//}




//DateTime nowTime = DateTime.Parse("2021-12-16 23:50:00");

//// Get the next quarter
//int nextTickMinutes = 15 * (nowTime.Minute / 15) + 15;

//DateTime newTime = nowTime.AddMinutes(nextTickMinutes - nowTime.Minute);
//newTime = newTime.AddSeconds(-newTime.Second);

//Console.WriteLine(newTime);

//TimeSpan difference = newTime - nowTime;

//double result = difference.TotalMilliseconds;
//if (result <= 0)
//    result += TimeSpan.FromHours(24).TotalMilliseconds;



//string[] s = time.Split(':');
//int hour = Convert.ToInt32(s[0]);
//int minute = Convert.ToInt32(s[1]);

//TimeSpan ts = new TimeSpan(hour, minute, 0);

//Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd" + "  kl. " + "HH:mm")}");



//Timer timer = new Timer((e) =>
//{
//    TickTock();
//}, null, 0, 5000);



//while (true)
//{
//    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
//    ConsoleKeyInfo key = Console.ReadKey(true);

//    if(key.KeyChar == 'c')
//    {
//        timer.Change(0, 1000);
//    }


//}

void TickTock()
{

    Console.WriteLine($"TickTock on thread: {Thread.CurrentThread.ManagedThreadId}");



}


double CalculateMillisecondsToTime()
{
    DateTime nowTime = DateTime.Now;
    TimeSpan difference = DateTime.Now.AddDays(2).TimeOfDay - nowTime.TimeOfDay; // _shutdownTime.TimeOfDay - nowTime.TimeOfDay;
    double result = difference.TotalMilliseconds;
    if (result <= 0)
        result += TimeSpan.FromHours(24).TotalMilliseconds;

    return result;
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
