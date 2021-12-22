using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTests
{
    public class MultiThreading
    {
        public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int> body)
        {
            try
            {
                

                return Parallel.For(fromInclusive, toExclusive, i =>
                {
                    body(i);
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
