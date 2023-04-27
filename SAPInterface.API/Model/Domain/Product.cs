using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SApInterface.API.Model.Domain
{
    public class Product
    {
        public string ItemCode { get; set; }
        public string ShortName { get; set; }

        public string LongName { get; set; }
        public string ProductCategory { get; set; }
        public string  BatchSize { get; set; }
        public double Leadtime { get; set; }
        public string StorageCondition { get; set; }
        public string RegistrationNo { get; set; }

        public string Unit { get; set; }

        public DataSet ds { get; set; }


    }
}
