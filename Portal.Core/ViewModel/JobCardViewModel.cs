using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class JobCardViewModel:IModel
    {
      
        public long JobCardID { get; set; }
        public string JobCardNo { get; set; }       
        public string JobCardDate { get; set; }
        public string TimeIn { get; set; }
        public string TimeInMinute { get; set; }
        public string DeliveryTime { get; set; }
        public string DeliveryTimeMinute { get; set; }
        public long CustomerID { get; set; }

        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ModelName { get; set; }
        public string RegNo { get; set; }
        public string FrameNo { get; set; }

        public string EngineNo { get; set; }

        public string DateOfSale { get; set; }
        public string KMSCovered { get; set; }
        public string CouponNo { get; set; }

        public string FuelLevel { get; set; }
        public string EngineOilLevel { get; set; }
        public string KeyNo { get; set; }

        public string BatteryMakeNo { get; set; }

        public string Damage { get; set; }
        public string Accessories { get; set; }
        public string TypeOfService { get; set; }

        public string EstimationDeliveryTime { get; set; }
        public string EstimationDeliveryTimeMinute { get; set; }
        public decimal EstimationCostOfRepair { get; set; }
        public decimal EstimationCostOfParts { get; set; }

        public string PreJobCardNo { get; set; }

        public string PreSeviceDate { get; set; }
        public string PreKey { get; set; }
        public string MahenicName { get; set; }
        public string VehicleNo { get; set; }
        public string ChassisNo { get; set; }

        public string StartTime { get; set; }
        public string StartTimeMinute { get; set; }
        public string ClosingTime { get; set; }
        public string ClosingTimeMinute { get; set; }
        public int CompanyId { get; set; }

        public string ApprovalStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedUsername { get; set; }

        public string ModifiedUsername { get; set; }
    }

    public class JobCardProductDetailViewModel : IModel
    {

        public int SequenceNo { get; set; }
        public long JobCardDetailID { get; set; }      
        public long JobCardID { get; set; }
        public long ProductID { get; set; }
        public long ServiceItemID { get; set; }
        public string ProductName { get; set; }
        public string ServiceItemName { get; set; }        
        public string CustComplaintObservation { get; set; }
        public string SupervisorAdvice { get; set; }
        public decimal AmountEstimated { get; set; }
       

    }

    public class JobCardDetailViewModel : IModel
    {

        public int SequenceNo { get; set; }
        public long JobCardDetailID { get; set; }
        public long JobCardID { get; set; }
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        
        public decimal Quantity { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CGST_Perc { get; set; }
        public decimal CGST_Amount { get; set; }
        public decimal SGST_Perc { get; set; }
        public decimal SGST_Amount { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
