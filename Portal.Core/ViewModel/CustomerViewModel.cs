using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class CustomerViewModel
    {

        public int CustomerId { get; set; }
        public int LeadID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ContactPersonName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string ContactNo { get; set; }
        public string Fax { get; set; }
        public string PrimaryAddress { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string PinCode { get; set; }
        public string CSTNo { get; set; }
        public string TINNo { get; set; }
        public string PANNo { get; set; }
        public string GSTNo { get; set; }
        public string ExciseNo { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerTypeDesc { get; set; }
        public decimal CreditLimit { get; set; }
        public int CreditDays { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public bool Customer_Status { get; set; }
        public decimal AnnualTurnover { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public bool GST_Exempt { get; set; }

        public bool IsComposition { get; set; }
        public bool IsUIN { get; set; }
        public string UINNo { get; set; }
        public int CompanyBranchId { get; set; }

        public string CompanyBranchName { get; set; }
        public int SaleEmpId { get; set; }
        public string SaleEmployeeName { get; set; }

    }
    public class CustomerBranchViewModel
    {

        public long CustomerBranchId { get; set; } 
        public int SequenceNo { get; set; }
        public int CustomerId { get; set; }
        public string BranchName { get; set; }
        public string ContactPersonName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string ContactNo { get; set; }
        public string Fax { get; set; }
        public string PrimaryAddress { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string PinCode { get; set; }
        public string CSTNo { get; set; }
        public string TINNo { get; set; }
        public string PANNo { get; set; }
        public string GSTNo { get; set; } 
        public decimal AnnualTurnover { get; set; }
        public bool CustomerBranch_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    public class CustomerProductViewModel
    {
        public int SequenceNo { get; set; }
        public long MappingId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }

    public class CustomerFollowUpViewModel
    {
        public long CustomerFollowUpId { get; set; }
        public int FollowUpSequenceNo { get; set; } 
        public long CustomerId { get; set; }
        public int FollowUpActivityTypeId { get; set; }
        public string FollowUpActivityTypeName { get; set; }
        public string FollowUpDueDateTime { get; set; }
        public string FollowUpReminderDateTime { get; set; }
        public string FollowUpRemarks { get; set; }
        public int Priority { get; set; }
        public string PriorityName { get; set; }
        public int FollowUpStatusId { get; set; }
        public string FollowUpStatusName { get; set; }
        public string FollowUpStatusReason { get; set; }
        public int FollowUpByUserId { get; set; }
        public string FollowUpByUserName { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }

    public class CustomerCountViewModel
    {
        public long NewCustomer { get; set; }
        public long TotalCustomer { get; set; }
    }

    public class SaleDashboardItemsViewModel
    {
        public int SrNo { get; set; }
        public string ContainerItemKey { get; set; }
        public string ContainerItemValue { get; set; }
        public string BoxNumber { get; set; }
    }
    public class InventoryDashboardItemsViewModel
    {
        public int SrNo { get; set; }
        public string ContainerItemKey { get; set; }
        public string ContainerItemValue { get; set; }
        public string BoxNumber { get; set; }
    }

    public class ProductionDashboardItemsViewModel
    {
        public int SrNo { get; set; }
        public string ContainerItemKey { get; set; }
        public string ContainerItemValue { get; set; }
        public string BoxNumber { get; set; }
    }
}
