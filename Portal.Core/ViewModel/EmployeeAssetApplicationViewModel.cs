using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class EmployeeAssetApplicationViewModel
    {
        public long ApplicationId { get; set; }       
        public string ApplicationNo { get; set; }
        public string ApplicationDate { get; set; }
        public int CompanyId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string AssetTypeName{ get; set; }      
        public int AssetTypeId { get; set; }           
        public string AssetReason { get; set; }

        public string ApprovedByUserName { get; set; }
        public bool AssetStatus { get; set; }
        public string ApplicationStatus { get; set; }
        public int ApproveBy { get; set; }
        public string ApproveDate { get; set; }
        public int RejectBy { get; set; }
        public string RejectDate { get; set; }
        public string RejectReason { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }

    }
   
}
