using System;
using System.Collections.Generic;

namespace LocalTests
{
    public static class TaskScheduler
    {
        private static Dictionary<string, IScheduledTask> ActiveTasks = new Dictionary<string, IScheduledTask>();

        private static List<TaskData> Tasks = new List<TaskData>();

        private static List<IScheduledTask> TaskQueue = new List<IScheduledTask>();

        private static DateTime nextTickTime = DateTime.MinValue;


        private static void InitializeScheduler()
        {
            nextTickTime = DateTime.Now;
        }

        public static DateTime RoundQuarterUp(DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, 0)
                .AddMinutes(input.Minute % 15 == 0 ? 15 : 15 - input.Minute % 15);
        }

        public static DateTime RoundQuarterUp2(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month,
                                dateTime.Day, dateTime.Hour, (dateTime.Minute / 15) * 15 + 15, 0);
        }

        private static void TickTock()
        {
            foreach (TaskData task in Tasks)
            {
                if (task.Task.SHOULDRUN)
                {
                    TaskQueue.Add(task.Task);
                }
            }

            CheckQueue();
        }

        private static void RecalcuateTimer()
        {
            DateTime currentTime = DateTime.Now;
            nextTickTime = nextTickTime.AddMinutes(15);
        }

        private static void TaskEnded()
        {
            CheckQueue();
        }

        private static void CheckQueue()
        {
            if (TaskQueue.Count > 0)
            {
                // check if isCanceled == true
                StartTask(TaskQueue[0].TaskName);
                //removeat [0]
            }
        }

        public static TaskResults StartTask(string name)
        {

            // if running, ignore

            return new TaskResults() { Message = "Hi", Success = true };
        }

        public static void ScheduleTask(Action action, int days, int hours, int minutes, int seconds)
        {

        }


    }
}
