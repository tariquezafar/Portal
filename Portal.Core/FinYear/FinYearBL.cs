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
    public class FinYearBL
    {
        DBInterface dbInterface;
        public FinYearBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditFinYear(FinYearViewModel finYearViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                //01-Jan-2016

                finYearViewModel.FinYearId = Convert.ToInt16(finYearViewModel.StartDate.Substring(7, 4)) ;
                finYearViewModel.FinYearCode = finYearViewModel.StartDate.Substring(7, 4) + "-" + finYearViewModel.EndDate.Substring(9, 2) ;
                finYearViewModel.FinYearDesc = finYearViewModel.StartDate + " - " + finYearViewModel.EndDate;
                FinancialYear finYear = new FinancialYear
                {
                    FinYearId=finYearViewModel.FinYearId,
                    StartDate = Convert.ToDateTime(finYearViewModel.StartDate),
                    EndDate = Convert.ToDateTime(finYearViewModel.EndDate),
                    FinYearCode=finYearViewModel.FinYearCode,
                    FinYearDesc=finYearViewModel.FinYearDesc,
                    Status= finYearViewModel.FinYearStatus
                };
                responseOut = dbInterface.AddEditFinYear(finYear);
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
        public List<FinYearViewModel> GetFinYearList()
        {
            List<FinYearViewModel> finyears = new List<FinYearViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtFinYears = sqlDbInterface.GetFinYearList();
                if (dtFinYears != null && dtFinYears.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinYears.Rows)
                    {
                        finyears.Add(new FinYearViewModel
                        {
                            FinYearId = Convert.ToInt32(dr["FinYearId"]),
                            FinYearDesc = Convert.ToString(dr["FinYearDesc"]),
                            StartDate = Convert.ToString(dr["StartDate"]),
                            EndDate = Convert.ToString(dr["EndDate"]),
                            FinYearCode = Convert.ToString(dr["FinYearCode"]),
                            FinYearStatus = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finyears;
        }
        public List<FinYearViewModel> GetFinancialYearList()
        {
            List<FinYearViewModel> finYears = new List<FinYearViewModel>();
            try
            {
                List<FinancialYear> finYearList = dbInterface.GetFinYearList();
                if (finYearList != null && finYearList.Count > 0)
                {
                    foreach (FinancialYear finYear in finYearList)
                    {
                        finYears.Add(new FinYearViewModel { FinYearId = finYear.FinYearId, FinYearDesc= finYear.FinYearDesc});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finYears;
        }
        public FinYearViewModel GetCurrentFinancialYear(int finYearid=0)
        {
            FinYearViewModel finYears = new FinYearViewModel();
            try
            {
                FinancialYear finYear = dbInterface.GetCurrentFinYear(finYearid);
                if (finYear != null )
                {
                    finYears=new FinYearViewModel { FinYearId = finYear.FinYearId, FinYearDesc = finYear.FinYearDesc,FinYearCode=finYear.FinYearCode,StartDate=Convert.ToDateTime(finYear.StartDate).ToString("dd-MMM-yyyy"), EndDate = Convert.ToDateTime(finYear.EndDate).ToString("dd-MMM-yyyy") };
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finYears;
        }

        public List<FinYearViewModel> GetFinancialYearForEmployeeAppraisalTemplateList()
        {
            List<FinYearViewModel> finYears = new List<FinYearViewModel>();
            try
            {
                List<FinancialYear> finyearList = dbInterface.GetFinancialYearForEmployeeAppraisalTemplateList();
                if (finyearList != null && finyearList.Count > 0)
                {
                    foreach (FinancialYear advance in finyearList)
                    {
                        finYears.Add(new FinYearViewModel { FinYearId = advance.FinYearId, FinYearDesc = advance.FinYearDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finYears;
        }

        public FinYearViewModel GetFinYearDetail(int finYearId = 0)
        {
            FinYearViewModel finYear = new FinYearViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtfinYears = sqlDbInterface.GetFinYearDetail(finYearId);
                if (dtfinYears != null && dtfinYears.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtfinYears.Rows)
                    {
                        finYear = new FinYearViewModel
                        {
                            FinYearId = Convert.ToInt32(dr["FinYearId"]),
                            StartDate = Convert.ToString(dr["StartDate"]),
                            EndDate = Convert.ToString(dr["EndDate"]),
                            FinYearCode = Convert.ToString(dr["FinYearCode"]),
                            FinYearDesc = Convert.ToString(dr["FinYearDesc"]),
                            FinYearStatus = Convert.ToBoolean(dr["status"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return finYear;
        }

    }
}
