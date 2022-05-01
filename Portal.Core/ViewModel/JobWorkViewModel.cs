using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class JobOrderViewModel
    {


        public long JobWorkId { get; set; }
        public string JobWorkNo { get; set; }
        public string JobWorkDate { get; set; }
        public string JobWorkTime { get; set; }
        public int VendorId { get; set; }
        public string Destination { get; set; }
        public string MotorVehicleNo { get; set; }
        public string TransportName { get; set; }
        public int  CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }

        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }

        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string JobWorkStatus { get; set; }
        public int JobWorkSequence { get; set; }
        public int FinYearId { get; set; }

    }
    public class JobWorkProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long JobWorkProductDetailId { get; set; }
        public long JobWorkId { get; set; }
        public long ProductId { get; set; }
        public string NatureOfProcessing { get; set; }

        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string ProductHSNCode { get; set; }
        public string UOMName { get; set; }

        public string IdentificationMarks { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalValue { get; set; }
        public decimal ScrapPerc { get; set; }

    }

    public partial class JobWorkINProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long JobWorkProductInDetailId { get; set; }
        public long JobWorkId { get; set; }
        public long ProductId { get; set; }
         public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public int UomId { get; set; }
        public string UOMName { get; set; }

        public string ProductHSNCode { get; set; }
    }

}
