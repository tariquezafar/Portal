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
    
    public partial class City
    {
        public long CityId { get; set; }
        public string CityName { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<decimal> Population { get; set; }
        public Nullable<decimal> Area { get; set; }
        public Nullable<int> Railway { get; set; }
        public Nullable<int> Airport { get; set; }
        public Nullable<int> PointOfInterest { get; set; }
        public string CityClass { get; set; }
        public Nullable<decimal> PopFactor { get; set; }
        public Nullable<decimal> EffectivePopulation { get; set; }
        public Nullable<int> Vehicles { get; set; }
        public Nullable<int> PerDealar { get; set; }
        public Nullable<int> DealershipsNos { get; set; }
    }
}