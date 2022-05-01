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
    public class VendorFormBL
    {
        DBInterface dbInterface;
        public VendorFormBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditVendorForm(VendorFormViewModel vendorFormViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                VendorForm vendorForm = new VendorForm
                {
                    VendorFormTrnId = vendorFormViewModel.VendorFormTrnId,
                    VendorId = vendorFormViewModel.VendorId,
                    InvoiceId = vendorFormViewModel.InvoiceId,
                    FormTypeId = vendorFormViewModel.FormTypeId,
                    FormStatus = vendorFormViewModel.FormStatus,
                    RefNo = vendorFormViewModel.RefNo,
                    RefDate = string.IsNullOrEmpty(vendorFormViewModel.RefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(vendorFormViewModel.RefDate),
                    Amount = vendorFormViewModel.Amount,
                    Remarks = vendorFormViewModel.Remarks,
                    CompanyId = vendorFormViewModel.CompanyId,
                    CreatedBy = vendorFormViewModel.CreatedBy


                };
                responseOut = dbInterface.AddEditVendorForm(vendorForm);

            }

            catch (Exception ex)
            {
                responseOut.status = ActionStatus.Fail;
                responseOut.message = ActionMessage.ApplicationException;
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

            return responseOut;
        }

        public List<VendorFormViewModel> GetVendorFormList(string FormStatus, string vendorName, string invoiceNo, string refNo, string fromDate, string toDate, int companyId)
        {
            List<VendorFormViewModel> vendorFormViewModel = new List<VendorFormViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVendorPayments = sqlDbInterface.GetVendorFormList(FormStatus, vendorName, invoiceNo, refNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtVendorPayments != null && dtVendorPayments.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVendorPayments.Rows)
                    {
                        vendorFormViewModel.Add(new VendorFormViewModel
                        {
                            VendorFormTrnId = Convert.ToInt32(dr["VendorFormTrnId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            FormTypeId = Convert.ToInt32(dr["FormTypeId"]),
                            FormStatus = Convert.ToString(dr["FormStatus"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
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
            return vendorFormViewModel;
        }
        public VendorFormViewModel GetVendorFormDetail(long vendorFormTrnId = 0)
        {
            VendorFormViewModel vendorFormViewModel = new VendorFormViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVendors = sqlDbInterface.GetVendorFormDetail(vendorFormTrnId);
                if (dtVendors != null && dtVendors.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVendors.Rows)
                    {
                        vendorFormViewModel = new VendorFormViewModel
                        {
                            VendorFormTrnId = Convert.ToInt32(dr["VendorFormTrnId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            FormTypeId = Convert.ToInt32(dr["FormTypeId"]),
                            FormStatus = Convert.ToString(dr["FormStatus"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            InvoiceId = Convert.ToInt32(dr["InvoiceId"]),
                            InvoiceNo = Convert.ToString(dr["InvoiceNo"]),
                            InvoiceDate = Convert.ToString(dr["InvoiceDate"]),
                            RefNo = Convert.ToString(dr["RefNo"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),
                            RefDate = Convert.ToString(dr["RefDate"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return vendorFormViewModel;
        }
    }
}
