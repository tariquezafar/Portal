using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
   public class SINViewModel
    {
        public long SINId { get; set; }
        public string SINNo { get; set; }
        public string SINDate { get; set; }
        public long JobId { get; set; }
        public string JobNo { get; set; }
        public string JobDate { get; set; }
        public long RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public string RequisitionDate { get; set; }
        public int CompanyBranchId { get; set; }       
        public string BranchName { get; set; }
        public int  FromLocationId { get; set; }
        public int ToLocationId { get; set; }  
        public string RefNo { get; set; } 
        public string RefDate { get; set; }

        public string EmployeeName { get; set; }

        public string FromLocationName { get; set; }

        public string ToLocationName { get; set; }
        public string Remarks1 { get; set; } 
        public string Remarks2 { get; set; }
        public int  ReceivedByUserId { get; set; } 
        public int  FinYearId { get; set; } 
        public int  CompanyId { get; set; }
        public string SINStatus { get; set; }
        public int SINSequence { get; set; }          
        public int CreatedBy { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedDate { get; set; }        
       public decimal TotalQuantity { get; set; }

        public string CancelReason { get; set; }
    }

    public class SINProductDetailViewModel
    {
        public int SequenceNo { get; set; }
        public long SINProductDetailId { get; set; }
        public long SINId { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }       
        public string UOMName { get; set; }
        public decimal Quantity { get; set; }
        public decimal IssuedQuantity { get; set; }
        public decimal BalanceQuantity { get; set; }
        public decimal IssueQuantity { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal IndentQuantity { get; set; }

    }


    public class SINSupportingDocumentViewModel
    {
        public int DocumentSequenceNo { get; set; }
        public long SINDocId { get; set; }
        public int SINId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }


    public class SINCountViewModel
    {
        public string SINTodayCount { get; set; }
        public string SINMtdCount { get; set; }
        public string SINYtdCount { get; set; }
        public string SINCount_Head { get; set; }


    }


}
