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
    public class EmployeeTravelAppBL
    {
        HRMSDBInterface dbInterface;
        public EmployeeTravelAppBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       

        public ResponseOut AddEditEmployeeTravelApp(HR_EmployeeTravelApplicationViewModel employeetravelapplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_EmployeeTravelApplication employeetravelapplication = new HR_EmployeeTravelApplication
                {
                    ApplicationId = employeetravelapplicationViewModel.ApplicationId, 
                    ApplicationDate =Convert.ToDateTime(employeetravelapplicationViewModel.ApplicationDate),
                    EmployeeId = employeetravelapplicationViewModel.EmployeeId,
                    TravelTypeId = employeetravelapplicationViewModel.TravelTypeId, 
                    TravelReason = employeetravelapplicationViewModel.TravelReason, 
                    TravelStartDate = Convert.ToDateTime(employeetravelapplicationViewModel.TravelStartDate),
                    TravelEndDate = Convert.ToDateTime(employeetravelapplicationViewModel.TravelEndDate),
                    TravelDestination = employeetravelapplicationViewModel.TravelDestination,
                    TravelStatus = employeetravelapplicationViewModel.TravelStatus,
                    CompanyId = employeetravelapplicationViewModel.CompanyId,
                     

                };
                responseOut = sqlDbInterface.AddEditEmployeeTravelApp(employeetravelapplication);
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


        public List<HR_EmployeeTravelApplicationViewModel> GetEmployeeTravelAppList(string applicationNo, int employeeId , string travelTypeId,  string travelStatus, string travelDestination,string fromDate, string toDate, int companyId,string companyBranch)
        {
            List<HR_EmployeeTravelApplicationViewModel> employeetravelApplications = new List<HR_EmployeeTravelApplicationViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeTravelApplications = sqlDbInterface.GetEmployeeTravelAppList(applicationNo, employeeId, travelTypeId, travelStatus, travelDestination, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, companyBranch);
                if (dtEmployeeTravelApplications != null && dtEmployeeTravelApplications.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeTravelApplications.Rows)
                    {
                        employeetravelApplications.Add(new HR_EmployeeTravelApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            TravelTypeId = Convert.ToInt16(dr["TravelTypeId"]),
                            TravelTypeName = Convert.ToString(dr["TravelTypeName"]), 
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]), 
                            TravelStartDate = Convert.ToString(dr["TravelStartDate"]),
                            TravelEndDate = Convert.ToString(dr["TravelEndDate"]), 
                            TravelReason = Convert.ToString(dr["TravelReason"]),
                            TravelDestination = Convert.ToString(dr["TravelDestination"]),
                            TravelStatus = Convert.ToString(dr["TravelStatus"]),
                         

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeetravelApplications;
        }



        public HR_EmployeeTravelApplicationViewModel GetEmployeeTravelAppDetail(long applicationId = 0)
        {
            HR_EmployeeTravelApplicationViewModel employeetravelApplications = new HR_EmployeeTravelApplicationViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeTravelApplication = sqlDbInterface.GetEmployeeTravelAppDetail(applicationId);
                if (dtEmployeeTravelApplication != null && dtEmployeeTravelApplication.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeTravelApplication.Rows)
                    {
                        employeetravelApplications = new HR_EmployeeTravelApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            TravelTypeId = Convert.ToInt16(dr["TravelTypeId"]),
                            TravelDestination = Convert.ToString(dr["TravelDestination"]), 
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            TravelStartDate = Convert.ToString(dr["TravelStartDate"]),
                            TravelEndDate = Convert.ToString(dr["TravelEndDate"]),
                            TravelReason = Convert.ToString(dr["TravelReason"]),
                            TravelStatus = Convert.ToString(dr["TravelStatus"]),
                            RejectDate = Convert.ToString(dr["RejectDate"]),
                            RejectReason = Convert.ToString(dr["RejectReason"])

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeetravelApplications;
        }


        public List<HR_EmployeeTravelApplicationViewModel> GetEmployeeTravelAppApprovalList(string applicationNo, int employeeId, string travelTypeId, string travelStatus, string travelDestination, string fromDate, string toDate, int companyId)
        {
            List<HR_EmployeeTravelApplicationViewModel> employeetravelApplications = new List<HR_EmployeeTravelApplicationViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployeeTravelApplications = sqlDbInterface.GetEmployeeTravelAppApprovalList(applicationNo, employeeId, travelTypeId, travelStatus, travelDestination,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtEmployeeTravelApplications != null && dtEmployeeTravelApplications.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployeeTravelApplications.Rows)
                    {
                        employeetravelApplications.Add(new HR_EmployeeTravelApplicationViewModel
                        {
                            ApplicationId = Convert.ToInt32(dr["ApplicationId"]),
                            ApplicationNo = Convert.ToString(dr["ApplicationNo"]), 
                            TravelTypeId = Convert.ToInt16(dr["TravelTypeId"]),
                            TravelTypeName = Convert.ToString(dr["TravelTypeName"]),
                            EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            TravelDestination = Convert.ToString(dr["TravelDestination"]),
                            TravelStartDate = Convert.ToString(dr["TravelStartDate"]),
                            TravelEndDate = Convert.ToString(dr["TravelEndDate"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            TravelReason = Convert.ToString(dr["TravelReason"]),
                            TravelStatus = Convert.ToString(dr["TravelStatus"]),
                            ApprovedByUserName = Convert.ToString(dr["ApprovedByUserName"]),
                            ApproveDate = Convert.ToString(dr["ApproveDate"]),
                            RejectDate = Convert.ToString(dr["RejectDate"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeetravelApplications;
        }


        public ResponseOut ApproveRejectEmployeeTravelApp(HR_EmployeeTravelApplicationViewModel employeetravelapplicationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                HR_EmployeeTravelApplication employeeApplication = new HR_EmployeeTravelApplication
                {
                    ApplicationId = employeetravelapplicationViewModel.ApplicationId,
                    RejectReason = employeetravelapplicationViewModel.RejectReason,
                    ApproveBy = employeetravelapplicationViewModel.ApproveBy,
                    TravelStatus = employeetravelapplicationViewModel.TravelStatus

                };

                responseOut = dbInterface.ApproveRejectEmployeeTravelApp(employeeApplication);
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
