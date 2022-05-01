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
  public  class SLDetailBL
    {
        DBInterface dbInterface;
        public SLDetailBL()
        {
            dbInterface = new DBInterface();
        }

        
        //public ResponseOut AddEditSLDetail(SLDetail sLDetail)
        //{
        //    ResponseOut responseOut = new ResponseOut();
        //    try
        //    {
        //        responseOut = dbInterface.AddEditSLDetail(sLDetail);
        //    }
        //    catch (Exception ex)
        //    {
        //        responseOut.status = ActionStatus.Fail;
        //        responseOut.message = ActionMessage.ApplicationException;
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //        throw ex;
        //    }
        //    return responseOut;
        //}

        public ResponseOut AddEditSLDetail(List<SLDetailViewModel> sLDetail ,int companyId,int finYearID,int createdBY)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                List<SLDetail> sLDetailList = new List<SLDetail>();
                if (sLDetail != null && sLDetail.Count > 0)
                {
                    foreach (SLDetailViewModel item in sLDetail)
                    {
                        sLDetailList.Add(new SLDetail
                        {
                            GLId = item.GLId,
                            SLId = item.SLId,
                            CompanyId= companyId,
                            FinYearId= finYearID,
                            CompanyBranchId=item.CompanyBranchId,
                            OpeningBalanceDebit=item.OpeningBalanceDebit,
                            OpeningBalanceCredit=item.OpeningBalanceCredit,
                            Status=item.SLDetailStatus,
                            CreatedBy = createdBY,
                        });
                    }
                    responseOut = sqlDbInterface.AddEditSLDetail(sLDetailList);
                }
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

        public List<SLDetailViewModel> GetSLDetailList(int SLTypeId = 0, int GLId = 0,int SLId=0,int FinYearId=0, int CompanyId = 0,int companyBranchId=0)
        {
            List<SLDetailViewModel> sLDetailList = new List<SLDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtsL = sqlDbInterface.GetSLDetailList(SLTypeId,GLId,SLId,FinYearId,CompanyId, companyBranchId);
                if (dtsL != null && dtsL.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtsL.Rows)
                    {
                        sLDetailList.Add(new SLDetailViewModel
                        {
                            SLDetailId= string.IsNullOrEmpty(dr["SLDetailId"].ToString()) ? 0 : Convert.ToInt32(dr["SLDetailId"]),
                            GLId = string.IsNullOrEmpty(dr["GLId"].ToString()) ? 0 : Convert.ToInt32(dr["GLId"]),
                            SLId = string.IsNullOrEmpty(dr["SLId"].ToString()) ? 0 : Convert.ToInt32(dr["SLId"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            SLHead = Convert.ToString(dr["SLHead"]),
                            OpeningBalanceDebit= string.IsNullOrEmpty(dr["OpeningBalanceDebit"].ToString())?Convert.ToDecimal("0.0"):Convert.ToDecimal(dr["OpeningBalanceDebit"]),
                            OpeningBalanceCredit = string.IsNullOrEmpty(dr["OpeningBalanceCredit"].ToString()) ? Convert.ToDecimal("0.0"):Convert.ToDecimal(dr["OpeningBalanceCredit"]),
                            SLDetailStatus = string.IsNullOrEmpty(Convert.ToString(dr["Status"])) ?false:Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sLDetailList;
        }

        public ResponseOut ImportSLDetail(SLDetail sLDetail)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                responseOut = dbInterface.AddEditSLDetail(sLDetail);
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
    }
}
