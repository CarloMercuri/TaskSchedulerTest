using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTests
{
    public class ActionCheckTask : IScheduledTask
    {
        public string TaskName { get; set; }
        public Action RequestedAction { get; set; }
        public bool isRunning { get; set; }
        public int PercentCompletion { get; set; }

        public string NextCycleTime()
        {
            throw new NotImplementedException();
        }

        public void StartTask()
        {
            throw new NotImplementedException();
        }

        public void StopTask()
        {
            throw new NotImplementedException();
        }

        public void UpdateNextTick(DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
