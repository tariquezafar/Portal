using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class CompanyViewModel:IModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ContactPersonName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Logo { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? State { get; set; }
        public int? CountryId { get; set; }
        public string ZipCode { get; set; }
        public string CompanyDesc { get; set; }
        public string PANNo { get; set; }
        public string TINNo { get; set; }
        public string TanNo { get; set; }
        public string ServiceTaxNo { get; set; }
        public string CompanyCode { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public decimal AnnualTurnover { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        


    }


    public class CompanyBranchViewModel
    {

        public long CompanyBranchId { get; set; }
        public int CompanyId { get; set; }
        public string BranchName { get; set; }
        public string ContactPersonName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string ContactNo { get; set; }
        public string Fax { get; set; }
        public string PrimaryAddress { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string PinCode { get; set; }
        public string CSTNo { get; set; }
        public string TINNo { get; set; }
        public string PANNo { get; set; }
        public string GSTNo { get; set; }
        public bool CompanyBranch_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public decimal LoadingCGST_Perc { get; set; }
        public decimal LoadingSGST_Perc { get; set; }
        public decimal LoadingIGST_Perc { get; set; }
        public decimal FreightCGST_Perc { get; set; }
        public decimal FreightSGST_Perc { get; set; }
        public decimal FreightIGST_Perc { get; set; }
        public decimal InsuranceCGST_Perc { get; set; }
        public decimal InsuranceSGST_Perc { get; set; }
        public decimal InsuranceIGST_Perc { get; set; }
        public decimal AnnualTurnover { get; set; }
        public string CompanyBranchCode { get; set; }
        public string ManufactorLocationCode { get; set; }
        public string BranchType { get; set; }
       
    }
}
