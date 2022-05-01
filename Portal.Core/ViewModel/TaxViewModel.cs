using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class TaxViewModel:IModel
    {  
        public int TaxId { get; set; }
        public string TaxName { get; set; }
        public string TaxType { get; set; }
        public string TaxSubType { get; set; }
        public Nullable<decimal> TaxPercentage { get; set; }
        public int FormTypeId { get; set; }
        public string FormTypeDesc { get; set; }
        
        public bool CFormApplicable { get; set; }
        public decimal WithOutCFormTaxPercentae { get; set; }
        public int TaxGLId { get; set; }

        public string TaxGLCode { get; set; }
        public int TaxSLId { get; set; }
        public string TaxSLCode { get; set; } 
        public string TaxGLHead { get; set; }
        public string TaxSLHead { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedName { get; set; }
        public string ModifiedDate { get; set; }

        public int SurchargeGLId_1 { get; set; }
        public string SurchargeGLCode_1 { get; set; }
        public int SurchargeSLId_1 { get; set; }
        public string SurchargeSLCode_1 { get; set; }
        public string SurchargeName_1 { get; set; }
        public Nullable<decimal> SurchargePercentage_1 { get; set; }
        public int SurchargeGLId_2 { get; set; }
        public string SurchargeGLCode_2 { get; set; }
        public int SurchargeSLId_2 { get; set; }
        public string SurchargeSLCode_2 { get; set; }
        public string SurchargeName_2 { get; set; }
        public Nullable<decimal> SurchargePercentage_2 { get; set; }
        public int SurchargeGLId_3 { get; set; }
        public string SurchargeGLCode_3 { get; set; }
        public int SurchargeSLId_3 { get; set; }
        public string SurchargeSLCode_3 { get; set; }
        public string SurchargeName_3 { get; set; }
        public Nullable<decimal> SurchargePercentage_3 { get; set; }

        public bool Tax_Status { get; set; }
        public string message { get; set; }
        public bool Status { get; set; } 
    }
    
}
