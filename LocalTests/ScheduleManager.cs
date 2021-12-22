using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace LocalTests
{
    public static class ScheduleManager
    {
        private static Dictionary<string, IScheduledTask> ActiveTasks = new Dictionary<string, IScheduledTask>();

        private static List<TaskData> Tasks = new List<TaskData>();

        private static List<IScheduledTask> TaskQueue = new List<IScheduledTask>();

        private static DateTime nextTickTime = DateTime.MinValue;
        private static int msToNextTick = 0;

        private static Timer TickTimer;
        private static int tickInterval = 1; // minutes
        private static int tickIntervalMs;
        private static int tickIntervalExtraMargin = 3000; // 3 seconds margin just in case
        private static int lastTickMinute;

        // TEST
        private static bool runOnce = false;

        public static void InitializeScheduler()
        {
            tickIntervalMs = tickInterval * 60 * 1000;


            // Get the tasks from the database, and fill the tasks list
            FillTasksList();

            // Check if any task should run?



            // ALWAYS LAST

            //nextTickTime = RoundMinuteUp();
            //int ms = (int)GetMillisecondsDifference(DateTime.Now, nextTickTime);
            //Console.WriteLine(ms);
            //TickTimer = new Timer((e) =>
            //{
            //    TickTock();
            //}, null, ms, tickIntervalMs); // Set it to default to repeat after the equivalent in ms of 15 minutes
        }

        private static void FillTasksList()
        {
            SqlDbConnections sql = new SqlDbConnections(true);
            DataTable data = sql.GetSchedules();

            if (data.Rows.Count > 0)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string taskName = data.Rows[i]["task_name"].ToString();

                    IScheduledTask task = GetCorrectTask(taskName);

                    if (task == null)
                    {
                        // Somehow wrong name. TODO: Add to error log?
                        continue;
                    }

                    task.TaskName = taskName;
                    task.StartDate = DateTime.Parse(data.Rows[i]["startdate"].ToString());
                    task.ScheduleType = data.Rows[i]["schedule_type"].ToString().Substring(0, 1)[0]; // what in the world did i just write. TODO: fix
                    task.ScheduleInterval = data.Rows[i]["schedule_interval"].ToString();
                    task.ExecutionTimeOfDay = data.Rows[i]["execution_timeofday"].ToString();
                    task.LastRun = DateTime.Parse(data.Rows[i]["last_run"].ToString());
                    task.NextExecution = DateTime.Parse(data.Rows[i]["next_execution"].ToString());

                    TaskData td = new TaskData();
                    td.Name = taskName;
                    td.Task = task;

                    Tasks.Add(td);

                }
            }

            foreach(TaskData tData in Tasks)
            {
                RescheduleTask(tData.Task);
            }
        }

        private static void TestPrintTasks()
        {
            foreach (TaskData td in Tasks)
            {
                Console.WriteLine("-------------------------------------------------------------------------");
                Console.WriteLine($"Task Name: {td.Task.TaskName}");
                Console.WriteLine($"Task StartDate: {td.Task.StartDate}");
                Console.WriteLine($"Task ScheduleType: {td.Task.ScheduleType}");
                Console.WriteLine($"Task Schedule Interval: {td.Task.ScheduleInterval}");
                Console.WriteLine($"Task Execution Time Of Day: {td.Task.ExecutionTimeOfDay}");
                Console.WriteLine($"Task LastRun: {td.Task.LastRun}");
                Console.WriteLine($"Task NextExecution: {td.Task.NextExecution}");

            }
        }

        private static void FillFakeTasks()
        {
            IScheduledTask task = new ActionCheckTask();
            task.TaskName = "ActionCheck";
            task.StartDate = DateTime.Now.AddMinutes(5);
            task.ScheduleType = 'D';
            task.ScheduleInterval = "00:00";
            task.ExecutionTimeOfDay = "07:00";
            task.LastRun = DateTime.Now.AddDays(-3);
            task.NextExecution = DateTime.Now.AddMinutes(30);

            IScheduledTask task2 = new ActionCheckTask();
            task2.TaskName = "ActionCheck";
            task2.StartDate = DateTime.Now.AddMinutes(2);
            task2.ScheduleType = 'D';
            task2.ScheduleInterval = "00:00";
            task2.ExecutionTimeOfDay = "10:32";
            task2.LastRun = DateTime.Now.AddDays(-12);
            task2.NextExecution = DateTime.Now.AddMinutes(30);

            IScheduledTask task3 = new ActionCheckTask();
            task3.TaskName = "ActionCheck";
            task3.StartDate = DateTime.Now.AddMinutes(32);
            task3.ScheduleType = 'D';
            task3.ScheduleInterval = "00:00";
            task3.ExecutionTimeOfDay = "17:12";
            task3.LastRun = DateTime.Now.AddDays(-5);
            task3.NextExecution = DateTime.Now.AddMinutes(60);

            SqlDbConnections con = new SqlDbConnections(true);

            con.InsertSchedule(task);
            con.InsertSchedule(task2);
            con.InsertSchedule(task3);
        }

        private static IScheduledTask GetCorrectTask(string taskName)
        {
            switch (taskName)
            {
                case "ActionCheck":
                    return new ActionCheckTask();

                default:
                    return null; 
                    
            }
        }

        /// <summary>
        /// Returns a DateTime object set to the next quarter hour
        /// </summary>
        /// <returns></returns>
        public static DateTime RoundQuarterUp()
        {
            DateTime nowTime = DateTime.Now;

            // Get the next quarter
            int nextTickSeconds = 5 * (nowTime.Second / 5) + 5; 
            //int nextTickMinutes = 15 * (nowTime.Minute / 15) + 15;

            DateTime newTime = nowTime.AddSeconds(nextTickSeconds - nowTime.Second);

            newTime = newTime.AddMilliseconds(-newTime.Millisecond);

            return newTime;
        }

        public static DateTime RoundMinuteUp()
        {
            return DateTime.Now.AddMinutes(1).AddSeconds(-DateTime.Now.Second).AddSeconds(2);
        }

        public static DateTime RoundQuarterUpWorking()
        {
            DateTime nowTime = DateTime.Now;

            // Get the next quarter
            int nextTickMinutes = 15 * (nowTime.Minute / 15) + 15;

            DateTime newTime = nowTime.AddMinutes(nextTickMinutes - nowTime.Minute);
            newTime = newTime.AddSeconds(-newTime.Second);

            return newTime;
        }


        private static void TickTock()
        {
            if (runOnce)
            {
                return;
            }

            foreach (TaskData task in Tasks)
            {


                if (task.Task.NextExecution.ToString("yyyy-MM-dd-HH-mm").Equals(DateTime.Now.AddSeconds(DateTime.Now.Second).ToString("yyyy-MM-dd-HH-mm")))
                {
                    TaskQueue.Add(task.Task);
                }

                TaskQueue.Add(task.Task);
            }

            UpdateTimerInterval();
            CheckQueue();

 

        }

        private static void RecalculateTimer()
        {
            DateTime currentTime = DateTime.Now;
            nextTickTime = RoundQuarterUp();
            int ms = (int)GetMillisecondsDifference(currentTime, nextTickTime);
        }

        private static void UpdateTimerInterval()
        {
            nextTickTime = RoundQuarterUp();
            int ms = (int)GetMillisecondsDifference(DateTime.Now, nextTickTime);
            
            TickTimer.Change(ms, tickIntervalMs);
        }

        public static double GetMillisecondsDifference(DateTime d1, DateTime d2)
        {
            TimeSpan difference = new TimeSpan();
            if (d1 > d2) difference = d1 - d2;
            if (d2 > d1) difference = d2 - d1;

            double result = difference.TotalMilliseconds;
            if (result <= 0)
                result += TimeSpan.FromHours(24).TotalMilliseconds;

            return result;
        }

        private static void TaskEnded()
        {
            CheckQueue();

        }


        private static void CheckQueue() // TODO: Better system
        {
            Console.WriteLine($"CheckQueue. Thread: {Thread.CurrentThread.ManagedThreadId}");

            foreach(IScheduledTask task in TaskQueue)
            {
                if (task.isRunning)
                {
                    RescheduleTask(task);
                    continue; // skip if already running e.g., forced run
                }

                if (task.isCanceled)
                {
                    continue; // skip if cancelled
                }

                StartTask(task);
                RescheduleTask(task);



            }

            // After all the tasks have run, clear the queue
            TaskQueue.Clear();
        }

        private static void ScheduleTaskOnLoad(IScheduledTask task)
        {
            switch (task.ScheduleType)
            {
                case 'O':

                    if(task.StartDate > DateTime.Now)
                    {
                        task.NextExecution = task.StartDate;
                    } else
                    {
                        task.NextExecution = DateTime.MinValue;
                    }

                    break;

                case 'T': // Timed interval
                    string[] s = task.ScheduleInterval.Split(':');
                    int hour = Convert.ToInt32(s[0]);
                    int minute = Convert.ToInt32(s[1]);

                    DateTime now = DateTime.Now;
                    DateTime next = DateTime.MinValue;

                    if (now.Minute < minute)
                    {
                        next = now.AddMinutes(minute - now.Minute).AddSeconds(-now.Second);
                    }
                    else
                    {
                        next = now.AddHours(1).AddMinutes(minute - now.Minute).AddSeconds(-now.Second);
                    }

                    task.NextExecution = next;

                    break;

                case 'D': // Daily hourly schedule
                    
                    break;

                case 'M':
                    task.NextExecution = task.NextExecution.AddMonths(1);
                    break;
            }
        }

        private static void RescheduleTask(IScheduledTask task)
        {
            switch (task.ScheduleType)
            {
                case 'O':
                    // Does not need to run again.
                    task.NextExecution = DateTime.MinValue;
                    task.StartDate = DateTime.MinValue;
                    break;

                case 'T': // Timed interval
                    string[] s = task.ScheduleInterval.Split(':');
                    int hour = Convert.ToInt32(s[0]);
                    int minute = Convert.ToInt32(s[1]);
                    task.NextExecution = task.NextExecution.AddHours(hour).AddMinutes(minute);
                    break;

                case 'D': // Daily hourly schedule
                    task.NextExecution = task.NextExecution.AddDays(1);
                    break;

                case 'M':
                    task.NextExecution =  task.NextExecution.AddMonths(1);
                    break;
            }
        }

        public static TaskResults StartTask(IScheduledTask task)
        {

            TaskResults result = task.StartTask();
            task.isRunning = false;
            task.LastRun = result.TimeStarted;
 
            return result;
        }

        public static void StopTask(string taskName)
        {
            foreach(TaskData task in Tasks)
            {
                if (task.Name.Equals(taskName))
                {
                    if (task.Task.isRunning)
                    {
                        task.Task.StopTask();
                    }
                }               
            }
        }

        private static TaskData GetTask(string name)
        {
            return Tasks.Find(x => x.Task.TaskName.Equals(name));
        }

        public static int GetTaskProgress(string taskName)
        {
            TaskData task = GetTask(taskName);

            if (task == null)
            {
                return -1;
            }
            else
            {
                return task.Task.PercentCompletion;
            }
        }

        public static bool IsTaskRunning(string taskName)
        {
            TaskData task = GetTask(taskName);

            return task.Task.isRunning;
        }

        public static void ScheduleTask(Action action, int days, int hours, int minutes, int seconds)
        {

        }


    }
}
