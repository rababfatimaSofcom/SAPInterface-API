namespace SAPInterface.API.Model.Domain
{
    public class UsageDecisionAndStockTransfer
    {
        public string SectionCode { get; set; }
        public string ProductCode { get; set; }

        public double Batch { get; set; }
        public double Sub { get; set; }
        public string DecisionCode { get; set; }
        public string Description { get; set; }
        public string EorS { get; set; }
    }
}
