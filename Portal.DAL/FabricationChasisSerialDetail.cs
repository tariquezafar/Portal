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
    
    public partial class FabricationChasisSerialDetail
    {
        public long FabricationChasisSerialDetailId { get; set; }
        public Nullable<long> FabricationId { get; set; }
        public Nullable<long> FabricationDetailId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public string ChasisSerialNo { get; set; }
        public string MotorNo { get; set; }
    }
}
