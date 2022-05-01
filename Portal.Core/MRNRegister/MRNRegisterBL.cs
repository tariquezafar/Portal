using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.Common;
using System.Reflection;
using System.Data;
using System.Transactions;
namespace Portal.Core
{
    public class MRNRegisterBL
    {
        DBInterface dbInterface;
        public MRNRegisterBL()
        {
            dbInterface = new DBInterface();
        }
         
     
        public List<MRNViewModel> GetMRNRegisterList(int vendorId, int shippingstateId, string fromDate, string toDate, int companyId, int createdBy, string sortBy, string sortOrder,string companyBranch)
        {
            List<MRNViewModel> mrns = new List<MRNViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtMRNs = sqlDbInterface.GetMRNRegisterList(vendorId, shippingstateId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, createdBy, sortBy, sortOrder, companyBranch);
                if (dtMRNs != null && dtMRNs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMRNs.Rows)
                    {

                        mrns.Add(new MRNViewModel
                        {
                            MRNId = Convert.ToInt32(dr["MRNId"]),
                            MRNNo = Convert.ToString(dr["MRNNo"]),
                            MRNDate = Convert.ToString(dr["MRNDate"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]), 
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),

                            POId = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),

                            QCId = Convert.ToInt32(dr["QualityCheckId"]),
                            QCNo = Convert.ToString(dr["QualityCheckNo"]),

                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorCode = Convert.ToString(dr["VendorCode"]),
                            VendorName = Convert.ToString(dr["VendorName"]), 
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            CompanyBranchAddress = Convert.ToString(dr["CompanyBranchAddress"]),
                            CompanyBranchCity = Convert.ToString(dr["CompanyBranchCity"]),
                            CompanyBranchStateName = Convert.ToString(dr["CompanyBranchStateName"]),
                            CompanyBranchPinCode = Convert.ToString(dr["CompanyBranchPinCode"]),
                            CompanyBranchTINNo = Convert.ToString(dr["CompanyBranchTINNo"]),
                            CompanyBranchCSTNo = Convert.ToString(dr["CompanyBranchCSTNo"]), 
                            Remarks1 = Convert.ToString(dr["Remarks1"]),
                            Remarks2 = Convert.ToString(dr["Remarks2"]),
                            DispatchRefNo = Convert.ToString(dr["DispatchRefNo"]),
                            DispatchRefDate = Convert.ToString(dr["DispatchRefDate"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return mrns;
        }
         




    }
}
