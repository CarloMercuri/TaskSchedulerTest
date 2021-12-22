using System.Collections.Generic;

namespace LocalTests
{
    public class HistoryCheckLog
    {
        public List<HistoryCheckLogElement> Elements { get; set; }

        public HistoryCheckLog()
        {
            Elements = new List<HistoryCheckLogElement>();
        }
    }
}
