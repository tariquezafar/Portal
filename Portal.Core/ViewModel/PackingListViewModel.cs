using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Core.ViewModel
{
    public class PackingListViewModel
    {
        public int SequenceNo { get; set; }
        public int PackingListID { get; set; }
        public string PackingListName { get; set; }
        public string PackingListDescription { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string PackingListStatus { get; set; }

        public int ProductSubGroupId { get; set; }
        public string ProductSubGroupName { get; set; }

        public long PackingListTypeID { get; set; }
        public string PackingListTypeName { get; set; }
        public int CompanyId { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedByUserName { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }
    }

    public class PackingListDetailViewModel
    {
        public int PackingListDetailedID { get; set; }
        public int PackingListID { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductShortDesc { get; set; }
        public string UOMName { get; set; }
        public string IsComplimentary { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public int SequenceNo { get; set; }
        public decimal Quantity { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}
