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
    public class EmployeeLoanApplicationBL
    {
        HRMSDBInterface dbInterface;
        public EmployeeLoanApplicationBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        #region EmployeeLoanApplication

        public ResponseOut AddEditEmployeeLoanApplication(EmployeeLoanApplicationViewModel employeeLoanApplicationViewModel)
        {

            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();              
            try
            {
                HR_EmployeeLoanApplication employeeLoanApplication = new HR_EmployeeLoanApplication
                {
                    ApplicationId = employeeLoanApplicationViewModel.ApplicationId,
                    ApplicationNo = employeeLoanApplicationViewModel.ApplicationNo,
                    ApplicationDate =Convert.ToDateTime(employeeLoanApplicationViewModel.ApplicationDate),
                    CompanyId = employeeLoanApplicationViewModel.CompanyId,
                    EmployeeId = employeeLoanApplicationViewModel.EmployeeId,
                    LoanTypeId = employeeLoanApplicationViewModel.LoanTypeId,
                    LoanInterestRate = employeeLoanApplicationViewModel.LoanInterestRate,
                    InterestCalcOn = employeeLoanApplicationViewModel.InterestCalcOn,
                    LoanAmount = employeeLoanApplicationViewModel.LoanAmount,
                    LoanStartDate =Convert.ToDateTime(employeeLoanApplicationViewModel.LoanStartDate),
                    LoanEndDate = Convert.ToDateTime(employeeLoanApplicationViewModel.LoanEndDate),
                    LoanInstallmentAmount = employeeLoanApplicationViewModel.LoanInstallmentAmount,
                    LoanReason = employeeLoanApplicationViewModel.LoanReason,
                    LoanStatus = employeeLoanApplicationViewModel.LoanStatus,
                    //ApproveBy = employeeLoanApplicationViewModel.ApproveBy,
                    //ApproveDate =Convert.ToDateTime(employeeLoanApplicationViewModel.ApproveDate),
                    //RejectBy = employeeLoanApplicationViewModel.RejectBy,
                    //RejectDate = Convert.ToDateTime(employeeLoanApplicationViewModel.RejectDate),
                    //RejectReason = employeeLoanApplicationViewModel.RejectReason,




                };
                responseOut = hRSQLDBInterface.AddEditEmployeeLoanApplication(employeeLoanApplication);

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
        public List<EmployeeLoanApplicationViewModel> GetEmployeeLoanApplicationList(string applicationNo , int employeeId , string loanTypeName , string loanStatus , string fromDate , string toDate , int companyId)
        {
            List<EmployeeLoanApplicationViewModel> employeeLoanApplicationViewModel = new List<EmployeeLoanApplicationViewModel>();          
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtLoanTypeApplication = hRSQLDBInterface.GetEmployeeLoanApplicationList(applicationNo, employeeId, loanTypeName, loanStatus,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),companyId);
                if (dtLoanTypeApplication != null && dtLoanTypeApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLoanTypeApplication.Rows)
                    {
                        employeeLoanApplicationViewModel.Add(new EmployeeLoanApplicationViewModel
                        {
                            ApplicationId=Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            EmployeeId=Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName=Convert.ToString(dr["EmployeeName"]),
                            LoanTypeId = Convert.ToInt32(dr["LoanTypeId"]),
                            LoanTypeName = Convert.ToString(dr["LoanTypeName"]),
                            LoanInterestRate = Convert.ToDecimal(dr["LoanInterestRate"]),
                            InterestCalcOn = Convert.ToString(dr["InterestCalcOn"]),
                            LoanAmount = Convert.ToDecimal(dr["LoanAmount"]),
                            LoanStartDate = Convert.ToString(dr["LoanStartDate"]),
                            LoanEndDate = Convert.ToString(dr["LoanEndDate"]),
                            LoanInstallmentAmount = Convert.ToDecimal(dr["LoanInstallmentAmount"]),
                            LoanReason = Convert.ToString(dr["LoanReason"]),
                            LoanStatus = Convert.ToString(dr["LoanStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeLoanApplicationViewModel;
        }
        public EmployeeLoanApplicationViewModel GetEmployeeLoanApplicationDetail(long applicationId = 0)
        {
            EmployeeLoanApplicationViewModel employeeLoanApplicationViewModel = new EmployeeLoanApplicationViewModel();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtLoanTypeApplication = hRSQLDBInterface.GetEmployeeLoanApplicationDetail(applicationId);
                if (dtLoanTypeApplication != null && dtLoanTypeApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLoanTypeApplication.Rows)
                    {
                        employeeLoanApplicationViewModel = new EmployeeLoanApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            LoanTypeId = Convert.ToInt32(dr["LoanTypeId"]),
                            LoanTypeName = Convert.ToString(dr["LoanTypeName"]),
                            LoanInterestRate = Convert.ToDecimal(dr["LoanInterestRate"]),
                            InterestCalcOn = Convert.ToString(dr["InterestCalcOn"]),
                            LoanAmount = Convert.ToDecimal(dr["LoanAmount"]),
                            LoanStartDate = Convert.ToString(dr["LoanStartDate"]),
                            LoanEndDate = Convert.ToString(dr["LoanEndDate"]),
                            LoanInstallmentAmount = Convert.ToDecimal(dr["LoanInstallmentAmount"]),
                            LoanReason = Convert.ToString(dr["LoanReason"]),
                            LoanStatus = Convert.ToString(dr["LoanStatus"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeLoanApplicationViewModel;
        }
        #endregion



        #region EmployeeLoanApplicationApproval
        public ResponseOut ApproveRejectEmployeeLoanApplication(EmployeeLoanApplicationViewModel employeeLoanApplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                HR_EmployeeLoanApplication employeeApplication = new HR_EmployeeLoanApplication
                {
                    ApplicationId = employeeLoanApplicationViewModel.ApplicationId,
                    RejectReason = employeeLoanApplicationViewModel.RejectReason,
                    ApproveBy = employeeLoanApplicationViewModel.ApproveBy,
                    LoanStatus = employeeLoanApplicationViewModel.LoanStatus

                };

                responseOut = dbInterface.ApproveRejectEmployeeLoanApplication(employeeApplication);
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
        public List<EmployeeLoanApplicationViewModel> GetEmployeeLoanApplicationApprovalList(string applicationNo, int employeeId, string loanTypeName, string loanStatus, string fromDate, string toDate, int companyId)
        {
            List<EmployeeLoanApplicationViewModel> employeeLoanApplicationViewModel = new List<EmployeeLoanApplicationViewModel>();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtLoanTypeApplication = hRSQLDBInterface.GetEmployeeLoanApplicationApprovalList(applicationNo, employeeId, loanTypeName, loanStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtLoanTypeApplication != null && dtLoanTypeApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLoanTypeApplication.Rows)
                    {
                        employeeLoanApplicationViewModel.Add(new EmployeeLoanApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            LoanTypeId = Convert.ToInt32(dr["LoanTypeId"]),
                            LoanTypeName = Convert.ToString(dr["LoanTypeName"]),
                            LoanInterestRate = Convert.ToDecimal(dr["LoanInterestRate"]),
                            InterestCalcOn = Convert.ToString(dr["InterestCalcOn"]),
                            LoanAmount = Convert.ToDecimal(dr["LoanAmount"]),
                            LoanStartDate = Convert.ToString(dr["LoanStartDate"]),
                            LoanEndDate = Convert.ToString(dr["LoanEndDate"]),
                            LoanInstallmentAmount = Convert.ToDecimal(dr["LoanInstallmentAmount"]),
                            LoanReason = Convert.ToString(dr["LoanReason"]),
                            LoanStatus = Convert.ToString(dr["LoanStatus"]),
                            ApprovedByUserName = Convert.ToString(dr["ApprovedByUserName"]),
                            ApproveDate = Convert.ToString(dr["ApproveDate"]),
                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeLoanApplicationViewModel;
        }
        public EmployeeLoanApplicationViewModel GetEmployeeLoanApplicationApprovalDetail(long applicationId = 0)
        {
            EmployeeLoanApplicationViewModel employeeLoanApplicationViewModel = new EmployeeLoanApplicationViewModel();
            HRSQLDBInterface hRSQLDBInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtLoanTypeApplication = hRSQLDBInterface.GetEmployeeLoanApplicationApprovalDetail(applicationId);
                if (dtLoanTypeApplication != null && dtLoanTypeApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLoanTypeApplication.Rows)
                    {
                        employeeLoanApplicationViewModel = new EmployeeLoanApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            LoanTypeId = Convert.ToInt32(dr["LoanTypeId"]),
                            LoanTypeName = Convert.ToString(dr["LoanTypeName"]),
                            LoanInterestRate = Convert.ToDecimal(dr["LoanInterestRate"]),
                            InterestCalcOn = Convert.ToString(dr["InterestCalcOn"]),
                            LoanAmount = Convert.ToDecimal(dr["LoanAmount"]),
                            LoanStartDate = Convert.ToString(dr["LoanStartDate"]),
                            LoanEndDate = Convert.ToString(dr["LoanEndDate"]),
                            LoanInstallmentAmount = Convert.ToDecimal(dr["LoanInstallmentAmount"]),
                            LoanReason = Convert.ToString(dr["LoanReason"]),
                            LoanStatus = Convert.ToString(dr["LoanStatus"]),
                            RejectReason = Convert.ToString(dr["RejectReason"]),
                            ApproveDate = Convert.ToString(dr["RejectReason"]),
                            RejectDate = Convert.ToString(dr["RejectReason"]),                          
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeLoanApplicationViewModel;
        }
        #endregion
    }
}
