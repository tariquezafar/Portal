using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_CTCViewModel : IModel
    {
        public int CTCId { get; set; }
        public int CompanyId { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public decimal Basic { get; set; }
        public decimal HRAPerc { get; set; }
        public decimal HRAAmount { get; set; }
        public decimal Conveyance { get; set; }
        public decimal Medical { get; set; }
        public decimal ChildEduAllow { get; set; }


        public decimal LTA { get; set; }
        public decimal SpecialAllow { get; set; }
        public decimal OtherAllow { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal EmployeePF { get; set; }

        public decimal EmployeeESI { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal NetSalary { get; set; }
        public decimal EmployerPF { get; set; }
        public decimal EmployerESI { get; set; }

        public decimal MonthlyCTC { get; set; }
        public decimal YearlyCTC { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public bool CTC_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
