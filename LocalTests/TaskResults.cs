using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTests
{
    public class TaskResults
    {
        public string TaskName { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime TimeStarted { get; set; }

        /// <summary>
        /// Execution time in seconds
        /// </summary>
        public double ExecutionTime { get; set; }
        

    }
}
