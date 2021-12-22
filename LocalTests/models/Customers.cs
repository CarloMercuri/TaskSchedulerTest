using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalTests
{
    /// <summary>
    /// Used to define Customers in the system. 
    /// </summary>
    public class Customers
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ArrowId { get; set; }
        public string EconomicsId { get; set; }
        public CustomerSettings Settings { get; set; }
        public string IpvisionDomain { get; set; }
        public string Self { get; set; }
        public string Rekv { get; set; }
        public string Note { get; set; }
        public string M365DraftNote { get; set; }
        public string RekvNumberM365 { get; set; }
        public string TelepoDraftNote { get; set; }
        public string RekvNumberTelepo { get; set; }
        public bool InProgress { get; set; }
        public bool ArrowDeleted { get; set; }
        public bool TelepoDeleted { get; set; }
        public bool Barred { get; set; }
        public List<Products> Products { get; set; }
    }
}
