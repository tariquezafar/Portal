using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core
{
public  class ChasisSerialMappingViewModel
    {
        public long MappingId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ChasisSerialNo { get; set; }
        public string MotorNo { get; set; }
        public string ControllerNo { get; set; }
        public string Color { get; set; }
        public string BatteryPower { get; set; }
        public string BatterySerialNo1 { get; set; }
        public string BatterySerialNo2 { get; set; }
        public string BatterySerialNo3 { get; set; }
        public string BatterySerialNo4 { get; set; }
        public string Tier { get; set; }
        public bool FrontGlassAvailable { get; set; }
        public bool ViperAvailable { get; set; }
        public bool RearShockerAvailable { get; set; }
        public bool ChargerAvailable { get; set; }       
        public bool FM { get; set; }
        public bool FmAvailable { get; set; }
        public bool ChasisSerialMapping_Status { get; set; }
        public bool Status { get; set; }
        public int CreatedBy { get; set; }

        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }

        public int CompanyBranchId { get; set; }
        public string ComapnyBranchName { get; set; }

        public string ProductSerialStatus { get; set; }
    }
}
