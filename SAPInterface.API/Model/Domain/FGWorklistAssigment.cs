namespace SAPInterface.API.Model.Domain
{
    public class FGWorklistAssigment
    {
        public string SectionCode { get; set; }
        public string ProductCode { get; set; }

        public double Batch { get; set; }
        public double Sub { get; set; }
        public string TestCode { get; set; }
        public string Description { get; set; }
        public string Analyst { get; set; }
        public DateTime TestDate { get; set; }
    }
}
