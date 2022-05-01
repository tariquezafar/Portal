using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class ProductBOMViewModel:IModel
    {
        public long BOMId { get; set; }
        public string AssemblyType { get; set; }
        public long AssemblyId { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyCode { get; set; }
        public string AssemblyShortDesc { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public decimal BOMQty { get; set; }
        public decimal ScrapPercentage { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public bool BOM_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string  ProcessType { get; set; }
        public string SaleUOMName { get; set; }

        public string Cancelstatus { get; set; }

        public int CompanyBranchId { get; set; }

        public string CompanyBranchName { get; set; }
    }

    public class ProductBomManufacturingExpenseViewModel
    {
        public long ProductBomManufacturingExpenseId { get; set; }
        public long AssemblyId { get; set; }
        public decimal Electricityexpenses { get; set; }
        public decimal LabourExpense { get; set; }
        public decimal OtherExpense { get; set; }
    }


}
