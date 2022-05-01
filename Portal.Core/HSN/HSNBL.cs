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

namespace Portal.Core
{
    public class HSNBL
    {
        DBInterface dbInterface;
        public HSNBL()
        {
            dbInterface = new DBInterface();
        }
        public List<HSNViewModel> GetHSNAutoCompleteList(string searchTerm)
        {
            List<HSNViewModel> hSNLists = new List<HSNViewModel>();
            try
            {
                List<HSNCode> hSNList = dbInterface.GetHSNAutoCompleteList(searchTerm);

                if (hSNList != null && hSNList.Count > 0)
                {
                    foreach (HSNCode hsn in hSNList)
                    {
                        hSNLists.Add(new HSNViewModel
                        {
                            HSNID = Convert.ToInt32(hsn.HSNID),
                            HSNCode = hsn.HSNCode1,
                            CGST_Perc =Convert.ToDecimal(hsn.CGST_Perc),
                            SGST_Perc = Convert.ToDecimal(hsn.SGST_Perc),
                            IGST_Perc = Convert.ToDecimal(hsn.IGST_Perc),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return hSNLists;
        }

        public ResponseOut AddEditHSN(HSNViewModel hSNViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HSNCode hSNCode = new HSNCode
                {
                    HSNID = hSNViewModel.HSNID,
                    HSNCode1 = hSNViewModel.HSNCode,
                    CGST_Perc = hSNViewModel.CGST_Perc,
                    SGST_Perc = hSNViewModel.SGST_Perc,
                    IGST_Perc = hSNViewModel.IGST_Perc,
                    Status = hSNViewModel.HSN_Status,
                   
                };
                responseOut = dbInterface.AddEditHSN(hSNCode);
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

        public List<HSNViewModel> GetHSNList(string hsnCode = "", string Status = "")
        {
            List<HSNViewModel> hSNViewModeLIst = new List<HSNViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCLaimTypes = sqlDbInterface.GetHSNList(hsnCode, Status);
                if (dtCLaimTypes != null && dtCLaimTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCLaimTypes.Rows)
                    {
                        hSNViewModeLIst.Add(new HSNViewModel
                        {
                            HSNID = Convert.ToInt32(dr["HSNID"]),
                            HSNCode = Convert.ToString(dr["HSNCode"]),
                            CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]),
                            HSN_Status = Convert.ToBoolean(dr["Status"]),
                            SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]),
                            IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return hSNViewModeLIst;
        }

        public HSNViewModel GetHSNDetail(int hSNID = 0)
        {
            HSNViewModel hSNViewModel = new HSNViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtClaimTypes = sqlDbInterface.GetHSNDetail(hSNID);
                if (dtClaimTypes != null && dtClaimTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtClaimTypes.Rows)
                    {
                        hSNViewModel = new HSNViewModel
                        {
                            HSNID = Convert.ToInt32(dr["HSNID"]),
                            HSNCode = Convert.ToString(dr["HSNCode"]),
                            CGST_Perc = Convert.ToDecimal(dr["CGST_Perc"]),
                            HSN_Status = Convert.ToBoolean(dr["Status"]),
                            SGST_Perc = Convert.ToDecimal(dr["SGST_Perc"]),
                            IGST_Perc = Convert.ToDecimal(dr["IGST_Perc"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return hSNViewModel;
        }


    }
}
