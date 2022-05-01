using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{

    public class TBDrillDownViewModel : IModel
    {
        public List<TBDrillDown_GLTypeViewModel> GLTypeList { get; set; }
        public List<TBDrillDown_MainGroupViewModel> MainGroupList { get; set; }
        public List<TBDrillDown_SubGroupViewModel> SubGroupList { get; set; }
        public List<TBDrillDown_GLWiseViewModel> GLWiseList { get; set; }
        public List<TBDrillDown_SLWiseViewModel> SLWiseList { get; set; }

    }
    public class GLDrillDownViewModel:IModel
    {
        public List<GLDrillDown_GLOpeningViewModel> GLOpeningList { get; set; }
        public List<GLDrillDown_GLLedgerViewModel> GLLedgerList { get; set; }
    }
    public class SLDrillDownViewModel : IModel
    {
        public List<SLDrillDown_SLOpeningViewModel> SLOpeningList { get; set; }
        public List<SLDrillDown_SLLedgerViewModel> SLLedgerList { get; set; }
    }

    public class TBDrillDown_GLTypeViewModel:IModel
    {
        public string GLTYPE { get; set; }
        public int GLTYPE_ORDER { get; set; }

        public decimal CLOSING { get; set; }
        public int CompanyBranchId { get; set; }
        public decimal DEBIT { get; set; }
        public decimal CREDIT { get; set; }
        public decimal YEAROPENINGBALANCEDEBIT { get; set; }
        public decimal YEAROPENINGBALANCECREDIT { get; set; }
        public decimal CLOSINGBALANCEDEBIT { get; set; }
        public decimal CLOSINGBALANCECREDIT { get; set; }
    
    }
    public class TBDrillDown_MainGroupViewModel : IModel
    {
        public string GLTYPE { get; set; }
        public int GLMainGroupId { get; set; }
        public decimal CLOSING { get; set; }
        public int CompanyBranchId { get; set; }
       
        public string GLMainGroupName { get; set; }
        public decimal DEBIT { get; set; }
        public decimal CREDIT { get; set; }
        public decimal YEAROPENINGBALANCEDEBIT { get; set; }
        public decimal YEAROPENINGBALANCECREDIT { get; set; }
        public decimal CLOSINGBALANCEDEBIT { get; set; }
        public decimal CLOSINGBALANCECREDIT { get; set; }

    }
    public class TBDrillDown_SubGroupViewModel : IModel
    {
        public decimal CLOSING { get; set; }
      
        public int GLMainGroupId { get; set; }
        public int CompanyBranchId { get; set; }

        public int GLSubGroupId { get; set; }
        public int ScheduleNo { get; set; }
        public string GLSubGroupName { get; set; }
        public decimal DEBIT { get; set; }
        public decimal CREDIT { get; set; }
        public decimal YEAROPENINGBALANCEDEBIT { get; set; }
        public decimal YEAROPENINGBALANCECREDIT { get; set; }
        public decimal CLOSINGBALANCEDEBIT { get; set; }
        public decimal CLOSINGBALANCECREDIT { get; set; }

    }
    public class TBDrillDown_GLWiseViewModel : IModel
    {

        public int GLMainGroupId { get; set; }
        public decimal CLOSING { get; set; }
        public int CompanyBranchId { get; set; }
        
        public int GLSubGroupId { get; set; }
        public int SLTypeId { get; set; }
        public int BookId { get; set; }
        public Int32 GLId { get; set; }
        public string GLCode { get; set; }
        public string GLHead { get; set; }
        public decimal DEBIT { get; set; }
        public decimal CREDIT { get; set; }
        public decimal YEAROPENINGBALANCEDEBIT { get; set; }
        public decimal YEAROPENINGBALANCECREDIT { get; set; }
        public decimal CLOSINGBALANCEDEBIT { get; set; }
        public decimal CLOSINGBALANCECREDIT { get; set; }

    }
    public class TBDrillDown_SLWiseViewModel : IModel
    {

        public Int32 GLId { get; set; }
        public Int64 SLId { get; set; }
        public decimal CLOSING { get; set; }
        public int CompanyBranchId { get; set; }
        public string SLCode { get; set; }
        public string SLHead { get; set; }
        public decimal DEBIT { get; set; }
        public decimal CREDIT { get; set; }
        public decimal YEAROPENINGBALANCEDEBIT { get; set; }
        public decimal YEAROPENINGBALANCECREDIT { get; set; }
        public decimal CLOSINGBALANCEDEBIT { get; set; }
        public decimal CLOSINGBALANCECREDIT { get; set; }

    }

    public class GLDrillDown_GLOpeningViewModel : IModel
    {

        public Int32 GLId { get; set; }
        public string GLCode { get; set; }
        public string GLHead { get; set; }
        public decimal DBAmount { get; set; }
        public decimal CRAmount { get; set; }
        public decimal Balance { get; set; }
        public string BalanceDRCR { get; set; }
        

    }
    public class GLDrillDown_GLLedgerViewModel : IModel
    {
        public Int64 VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string VoucherType { get; set; }
        public string Narration { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string PayeeName { get; set; }
        public decimal DBAmount { get; set; }
        public decimal CRAmount { get; set; }
        public Int32 BookId { get; set; }
        public string VoucherBookType { get; set; }
        public string VoucherViewPagePath { get; set; }
        public Int32 GLId { get; set; }
        public string GLCode { get; set; }
        public string GLHead { get; set; }
        public decimal RunningBalance { get; set; }
        public string RunningBalanceDrCr { get; set; }


    }

    public class SLDrillDown_SLOpeningViewModel : IModel
    {

        public Int32 GLId { get; set; }
        public string GLCode { get; set; }
        public string GLHead { get; set; }
        public Int32 SLId { get; set; }
        public string SLCode { get; set; }
        public string SLHead { get; set; }
        public decimal DBAmount { get; set; }
        public decimal CRAmount { get; set; }
        public decimal Balance { get; set; }
        public string BalanceDRCR { get; set; }


    }
    public class SLDrillDown_SLLedgerViewModel : IModel
    {
        public Int64 VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string VoucherType { get; set; }
        public string Narration { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string PayeeName { get; set; }
        public string ChequeRefNo { get; set; }
        public string ChequeRefDate { get; set; }
        public string ValueDate { get; set; }
        public decimal DBAmount { get; set; }
        public decimal CRAmount { get; set; }
        public Int32 BookId { get; set; }
        public string VoucherBookType { get; set; }
        public string VoucherViewPagePath { get; set; }
        public Int32 GLId { get; set; }
        public string GLCode { get; set; }
        public string GLHead { get; set; }
        public Int32 SLId { get; set; }
        public string SLCode { get; set; }
        public string SLHead { get; set; }
        public decimal RunningBalance { get; set; }
        public string RunningBalanceDrCr { get; set; }


    }

}
