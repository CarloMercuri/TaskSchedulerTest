

namespace LocalTests
{
    public class HistoryCheckLogElement
    {
        public ProductHistoryAction Action { get; set; }
        public string CustomerId { get; set; } = "N/A";
        public string CustomerName { get; set; } = "N/A";
        public string ProductId { get; set; } = "N/A";
        public string ProductName { get; set; } = "N/A";

        public override string ToString()
        {
            return $"Action: {Action.Action}, on Product ID: {ProductId} ({ProductName}) -- Customer ID: {CustomerId}, Customer Name: {CustomerName}";
        }
    }
}
