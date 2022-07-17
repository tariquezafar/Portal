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
    
    public partial class Dispatch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dispatch()
        {
            this.DispatchProductDetail = new HashSet<DispatchProductDetail>();
        }
    
        public int DispatchID { get; set; }
        public string DispatchNo { get; set; }
        public Nullable<System.DateTime> DispatchDate { get; set; }
        public int DispatchPlanID { get; set; }
        public Nullable<int> CompanyBranchID { get; set; }
        public Nullable<int> DispatchSequence { get; set; }
        public string ApprovalStatus { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual DispatchPlan DispatchPlan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DispatchProductDetail> DispatchProductDetail { get; set; }
    }
}