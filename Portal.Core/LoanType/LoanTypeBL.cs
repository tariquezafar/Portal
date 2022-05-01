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
using Portal.DAL.Infrastructure;
namespace Portal.Core
{
    public class LoanTypeBL
    {
        HRMSDBInterface dbInterface;
        public LoanTypeBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditLoanType(HR_LoanTypeViewModel loantypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_LoanType loantype = new HR_LoanType
                {
                    LoanTypeId = loantypeViewModel.LoanTypeId,
                    LoanTypeName = loantypeViewModel.LoanTypeName,
                    LoanInterestRate = loantypeViewModel.LoanInterestRate,
                    InterestCalcOn = loantypeViewModel.InterestCalcOn,
                    Status = loantypeViewModel.LoanType_Status
                };
                responseOut = dbInterface.AddEditLoanType(loantype);
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

        public List<HR_LoanTypeViewModel> GetLoanTypeList(string loantypeName = "",  string loaninterestcalcOn = "", string Status = "")
        {
            List<HR_LoanTypeViewModel> loantypes = new List<HR_LoanTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtLoanTypes = sqlDbInterface.GetLoanTypeList(loantypeName, loaninterestcalcOn, Status);
                if (dtLoanTypes != null && dtLoanTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLoanTypes.Rows)
                    {
                        loantypes.Add(new HR_LoanTypeViewModel
                        {
                            LoanTypeId = Convert.ToInt32(dr["LoanTypeId"]),
                            LoanTypeName = Convert.ToString(dr["LoanTypeName"]),
                            LoanInterestRate = Convert.ToDecimal(dr["LoanInterestRate"]),
                            InterestCalcOn = Convert.ToString(dr["InterestCalcOn"]),
                            LoanType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return loantypes;
        }

        public HR_LoanTypeViewModel GetLoanTypeDetail(int loantypeId = 0)
        {
            HR_LoanTypeViewModel loantype = new HR_LoanTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtLoanTypes = sqlDbInterface.GetLoanTypeDetail(loantypeId);
                if (dtLoanTypes != null && dtLoanTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLoanTypes.Rows)
                    {
                        loantype = new HR_LoanTypeViewModel
                        {
                            LoanTypeId = Convert.ToInt32(dr["LoanTypeId"]),
                            LoanTypeName = Convert.ToString(dr["LoanTypeName"]),
                            LoanInterestRate = Convert.ToDecimal(dr["LoanInterestRate"]),
                            InterestCalcOn = Convert.ToString(dr["InterestCalcOn"]),
                            LoanType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return loantype;
        }


        public List<HR_LoanTypeViewModel> GetEmployeeLoanApplicationTypeList()
        {
            List<HR_LoanTypeViewModel> loanTypeViewModel = new List<HR_LoanTypeViewModel>();
            try
            {
                List<HR_LoanType> loanTypeList = dbInterface.GetEmployeeLoanApplicationTypeList();
                if (loanTypeList != null && loanTypeList.Count > 0)
                {
                    foreach (HR_LoanType advance in loanTypeList)
                    {
                        loanTypeViewModel.Add(new HR_LoanTypeViewModel { LoanTypeId = advance.LoanTypeId, LoanTypeName = advance.LoanTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return loanTypeViewModel;
        }

        public List<EmployeeLoanApplicationViewModel> GetEmployeeLoanApplicationDetails(int companyId, int userId, int reportingUserId, int reportingRoleId)
        {

            List<EmployeeLoanApplicationViewModel> loanApplicationList = new List<EmployeeLoanApplicationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtList = sqlDbInterface.GetEmployeeLoanApplicationDetails(companyId, userId, reportingUserId, reportingRoleId);
                if (dtList != null && dtList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtList.Rows)
                    {
                        loanApplicationList.Add(new EmployeeLoanApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            LoanTypeName = Convert.ToString(dr["LoanTypeName"]),
                            LoanReason = Convert.ToString(dr["LoanReason"]),
                            LoanStatus = Convert.ToString(dr["LoanStatus"]),
                            LoanAmount = Convert.ToDecimal(dr["LoanAmount"]),
                            LoanInstallmentAmount = Convert.ToDecimal(dr["LoanInstallmentAmount"]),
                            EmployeeId = Convert.ToInt64(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"])
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return loanApplicationList;
        }
    }
}
