using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
 public class CRMDashboardViewModel
    {
        public List<LeadStatusCountViewModel> leadStatusCountList { get; set; }
    }
    public class LeadStatusCountViewModel
    {
        public int LeadStatusId { get; set; }
        public string LeadStatusName { get; set; }
        public int TotalLeadCount{ get; set; }
        public int TodayCount { get; set; }
        public int MTDCount { get; set; }
        public int YTDCount { get; set; }
    }
    public class LeadSourceCountViewModel
    {
        public int LeadSourceId { get; set; }
        public string LeadSourceName { get; set; }
        public int TotalLeadCount { get; set; }
        public int TodayCount { get; set; }
        public int MTDCount { get; set; }
        public int YTDCount { get; set; }
    }
    public class LeadFollowUpCountViewModel
    {
        public int FollowUpActivityTypeId { get; set; }
        public string FollowUpActivityTypeName { get; set; }
        public int TodayCount { get; set; }
        public int TommorowCount { get; set; }
        public int WeekCount { get; set; }
        public int MonthCount { get; set; }
    }
    public class LeadFollowUpReminderDueDateCountViewModel
    {
        public int FollowUpActivityTypeId { get; set; }
        public string FollowUpActivityTypeName { get; set; }
        public int ReminderCount { get; set; }
        public int DueDateCount { get; set; }
        
    }

    public class LeadFollowUpReminderDueDateListViewModel
    {
        public int LeadId { get; set; }
        public string LeadCode { get; set; }
        public string CompanyName { get; set; }

    }

    public class LeadConversionCountViewModel
    {
        public string DateValue { get; set; }
        
        public int TotalLead { get; set; }
        public int NewOpertunity { get; set; }
        public int Quotation { get; set; }
        
    }
}
