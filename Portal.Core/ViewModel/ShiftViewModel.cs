using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;

namespace Portal.Core.ViewModel
{
    public class ShiftViewModel : IModel
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string ShiftDescription { get; set; }
        public int CompanyId { get; set; }
        public int ShiftTypeId { get; set; }
        public string ShiftTypeName { get; set; }
        public int NormalPaidHours { get; set; }
        public int Allowance { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public string LateArrivalLimit { get; set; }
        public string EarlyGoLimit { get; set; }
        public string OvertimeStartTime { get; set; }
        public string ValidTill { get; set; }
        public decimal FullDayWorkHour { get; set; }
        public decimal HalfDayWorkHour { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public bool  Shift_Status { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
}
