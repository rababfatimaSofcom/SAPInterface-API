namespace SAPInterface.API.Model.Domain
{
    public class TestSpecification
    {
        public string SectionCode { get; set; }
        public string ProductCode { get; set; }

        public string TestCode { get; set; }
        public string LimitDescription { get; set; }
        public double MinLimit { get; set; }
        public double MaxLimit { get; set; }
    }
}
