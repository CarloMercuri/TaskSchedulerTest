using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTests
{
    public interface IScheduledTask
    {
        public string TaskName { get; set; }

        public bool isRunning { get; set; }
        public bool isCanceled { get; set; }

        public int PercentCompletion { get; }

        /// <summary>
        /// 'O' = Once. 'T' = Timed Interval. 'D' = Daily Hourly Schedule. 'M' = Monthly execution
        /// </summary>
        public char ScheduleType { get; set; }

        public string ScheduleInterval { get; set; }

        public DateTime StartDate { get; set; }

        /// <summary>
        /// hh:mm
        /// </summary>
        public string ExecutionTimeOfDay {get; set;}

        public DateTime LastRun { get; set; }
        public DateTime NextExecution { get; set; }

        public string NextCycleTime();

        public TaskResults StartTask();

        public void StopTask();

        public void UpdateNextTick(DateTime time);



        //public override string ToString()
        //{
        //    return $"Task '{TaskName}', ";
        //}

    }
}
