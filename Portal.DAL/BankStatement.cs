//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class BankStatement
    {
        public long BankStatementID { get; set; }
        public string BankStatementNo { get; set; }
        public Nullable<System.DateTime> BankStatementDate { get; set; }
        public Nullable<long> BankBookId { get; set; }
        public string BankBranch { get; set; }
        public Nullable<System.DateTime> BankStatementFromDate { get; set; }
        public Nullable<System.DateTime> BankStatementToDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CompanyBranchId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string BankStatementStatus { get; set; }
        public Nullable<int> BankStatementSequence { get; set; }
    }
}
