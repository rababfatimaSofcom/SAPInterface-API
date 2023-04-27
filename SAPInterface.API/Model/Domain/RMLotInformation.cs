namespace SAPInterface.API.Model.Domain
{
    public class RMLotInformation
    {
        //splinewbatchtrn
        public string SectionCode { get; set; }
        public string ProductCode { get; set; }

        public string GRN { get; set; }
        public double Sub { get; set; }
        public string SampleQuantity { get; set; }
        public string Unit { get; set; }
        public string SampleDate { get; set; }
        public string SampleBy { get; set; }

        public string SamplePoint { get; set; }
        public DateTime MfgDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ManufacturerCode { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierLot { get; set; }

        public DateTime RecieveDate { get; set; }
        public double RequiredLeadTimeDays { get; set; }
        public double RequiredLeadTimeHours { get; set; }
        public double RequiredLeadTimeMinutes { get; set; }
        public decimal BatchSize { get; set; }
        public string BatchUnit { get; set; }

        public string BatchType { get; set; }
        public string BatchStatus { get; set; }
        public string AnalyticalNo { get; set; }
        public string RT_PO_No { get; set; }
        public string NoOfContainers { get; set; }
        public string SubLot { get; set; }
    }
}
