using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class BookViewModel:IModel
    {
        public int BookId { get; set; }

        public long CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
        public string BookName { get; set; }
        public string BookCode { get; set; }
        public string BookType { get; set; }
        public string GLCode { get; set; }
        public string GLHead { get; set; }
        public int CompanyId { get; set; }
        public string BankBranch { get; set; }
        public string BankAccountNo { get; set; }  
        public string IFSC { get; set; }
        public decimal OverDraftLimit { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }
        public bool Book_Status { get; set; }
        public string message { get; set; }
        public bool status { get; set; } 
        public string AdCode { get; set; }
    }
    
}
