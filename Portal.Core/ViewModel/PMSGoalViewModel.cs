using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class PMSGoalViewModel
    {
        public int GoalId { get; set; }       
        public string GoalName { get; set; }
        public string GoalDescription { get; set; }

        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int GoalCategoryId { get; set; } 
        public string GoalCategoryName { get; set; }

        public int PerformanceCycleId { get; set; } 
        public string PerformanceCycleName { get; set; }
        public int FinYearId { get; set; }

        public int CompanyId { get; set; }
       

        public string StartDate { get; set; }
        public string DueDate { get; set; }
        public decimal Weight { get; set; }
        public bool GoalStatus { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; } 
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }
   
}
