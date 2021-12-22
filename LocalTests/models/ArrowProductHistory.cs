
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTests
{
    public class ArrowProductHistory
    {
        public long Status { get; set; }

        public ProductHistoryData Data { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Status: {Status}");

            foreach(ProductHistoryAction action in Data.Actions)
            {
                builder.AppendLine($" Action: {action.Action} - CreatedAt: {action.CreatedAt.ToString()} - treated: {action.treated}");

                foreach (ProductHistoryNote note in action.Note)
                {
                    builder.AppendLine($"   BEFORE: {note.Before.State}, {note.Before.ActiveSeats} ///  AFTER: {note.After.State}, {note.After.Seats}");
                }
                

            }


            return builder.ToString();
        }
    }
}
