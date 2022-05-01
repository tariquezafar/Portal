using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class LeadViewModel:IModel
    {
        public int UserId { get; set; }
        public int LeadId { get; set; }
        public string LeadCode { get; set; } 
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ContactPersonName { get; set; }
        public string Designation { get; set; } 
        public string Email { get; set; }
        public string AlternateEmail { get; set; }  
        public string ContactNo { get; set; }
        public string AlternateContactNo { get; set; } 
        public string Fax { get; set; }
        public string CompanyAddress { get; set; }
        public string BranchAddress { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; } 
        public string CountryName { get; set; }
        public String PinCode { get; set; } 
        public string CompanyCity { get; set; }
        public int CompanyStateId { get; set; } 
        public int CompanyCountryId { get; set; }
        public string CompanyPinCode { get; set; }
        public int LeadStatusId { get; set; }
        public string LeadStatusName { get; set; }
        public int LeadSourceId { get; set; }
        public string LeadSourceName { get; set; }
        public string OtherLeadSourceDescription { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; } 
        public bool Lead_Status { get; set; }
        public string message { get; set; } 
        public bool status { get; set; }
        public int LeadTypeId { get; set; }
        public string LeadTypeName { get; set; }

        public int CompanyBranch { get; set; }
        public string CompanyBranchName { get; set; }


    }
    public class LeadFollowUpViewModel
    {
        public int SequenceNo { get; set; }
        public long LeadFollowUpId { get; set; }
        public long LeadId { get; set; }
        public int FollowUpActivityTypeId { get; set; }
        public string FollowUpActivityTypeName { get; set; }
        public string FollowUpDueDateTime { get; set; }
        public string FollowUpReminderDateTime { get; set; }
        public string FollowUpRemarks { get; set; }
        public int Priority { get; set; }
        public string PriorityName { get; set; }
        public int LeadStatusId { get; set; }
        public string LeadStatusName { get; set; }
        public string LeadStatusReason { get; set; }
        public int FollowUpByUserId { get; set; }
        public string FollowUpByUserName { get; set; }
        public int CreatedBy { get; set; }
        public string  CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }
}
