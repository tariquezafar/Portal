using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class TargetViewModel
    {
        public long TargetId { get; set; }
        public string TargetNo { get; set; }
        public string TargetDate { get; set; }
        public string TargetFromDate { get; set; }
        public string TargetToDate { get; set; }
        public int CompanyId { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        
        public string Frequency { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string TargetStatus { get; set; }
        public int TargetSequence { get; set; }
        public int FinYearId { get; set; }
        public bool Status { get; set; }
    }

    public class TargetDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long TargetDetailId { get; set; }
        public long TargetId { get; set; }

        public int TargetTypeId { get; set; }
        public string TargetTypeName { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int Vehicles { get; set; }
        public int TargetQty { get; set; }
        public decimal TargetAmount { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string Frequency { get; set; }
        public decimal Amount { get; set; }

        public int PerDealar { get; set; }
        public int DealershipsNos { get; set; }
        public int TargetDealershipsNos { get; set; }
    }
}
