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
        public Action RequestedAction { get; set; }

        public bool isRunning { get; set; }

        public int PercentCompletion { get; set; }

        public string NextCycleTime();

        public void StartTask();

        public void StopTask();

        public void UpdateNextTick(DateTime time);



        //public override string ToString()
        //{
        //    return $"Task '{TaskName}', ";
        //}

    }
}
