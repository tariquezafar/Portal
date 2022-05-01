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
    public class SeparationOrderBL
    {
        HRMSDBInterface dbInterface;
        public SeparationOrderBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditSeparationOrder(SeparationOrderViewModel separationorderViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_SeparationOrder separationOrder = new HR_SeparationOrder
                {
                    SeparationOrderId = separationorderViewModel.SeparationOrderId, 
                    SeparationOrderDate=Convert.ToDateTime(separationorderViewModel.SeparationOrderDate),
                    EmployeeId = separationorderViewModel.EmployeeId,
                    ExitInterviewId = separationorderViewModel.ExitInterviewId,
                    EmployeeClearanceId = separationorderViewModel.EmployeeClearanceId,
                    ExperienceLetter = separationorderViewModel.ExperienceLetter,
                    SepartionRemarks = separationorderViewModel.SepartionRemarks,  
                    SeparationStatus = separationorderViewModel.SeparationStatus,
                    CreatedBy = separationorderViewModel.CreatedBy

                };
                responseOut = sqlDbInterface.AddEditSeparationOrder(separationOrder);
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
        

        public List<SeparationOrderViewModel> GetSeparationOrderList(string separationorderNo, int employeeId, string employeeClearanceNo, string exitInterviewNo, string separationStatus,  string fromDate, string toDate)
        {
            List<SeparationOrderViewModel> separationOrders = new List<SeparationOrderViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtSeparationOrders = sqlDbInterface.GetSeparationOrderList(separationorderNo, employeeId, employeeClearanceNo, exitInterviewNo, separationStatus,Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
                if (dtSeparationOrders != null && dtSeparationOrders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationOrders.Rows)
                    {
                        separationOrders.Add(new SeparationOrderViewModel
                        {
                            SeparationOrderId = Convert.ToInt32(dr["SeparationOrderId"]),
                            SeparationOrderDate = Convert.ToString(dr["SeparationOrderDate"]),
                            SeparationOrderNo = Convert.ToString(dr["SeparationOrderNo"]),
                            ExitInterviewId = Convert.ToInt16(dr["ExitInterviewId"]),
                            ExitInterviewNo = Convert.ToString(dr["ExitInterviewNo"]),
                            EmployeeId = Convert.ToInt16(dr["EmployeeId"]),
                            EmployeeClearanceId = Convert.ToInt32(dr["EmployeeClearanceId"]),
                            EmployeeClearanceNo= Convert.ToString(dr["EmployeeClearanceNo"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            ExperienceLetter = Convert.ToString(dr["ExperienceLetter"]),
                            SepartionRemarks = Convert.ToString(dr["SepartionRemarks"]),
                            SeparationStatus = Convert.ToString(dr["SeparationStatus"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
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
            return separationOrders;
        }



        public SeparationOrderViewModel GetSeparationOrderDetail(long separationorderId = 0)
        {
            SeparationOrderViewModel separationOrders = new SeparationOrderViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
              DataTable dtSeparationOrders = sqlDbInterface.GetSeparationOrderDetail(separationorderId);
              if (dtSeparationOrders != null && dtSeparationOrders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationOrders.Rows)
                    {
                        separationOrders = new SeparationOrderViewModel
                        {
                            SeparationOrderId = Convert.ToInt32(dr["SeparationOrderId"]),
                            SeparationOrderDate = Convert.ToString(dr["SeparationOrderDate"]),
                            SeparationOrderNo = Convert.ToString(dr["SeparationOrderNo"]),
                            ExitInterviewId = Convert.ToInt16(dr["ExitInterviewId"]),
                            ExitInterviewNo = Convert.ToString(dr["ExitInterviewNo"]),
                            EmployeeId = Convert.ToInt16(dr["EmployeeId"]),
                            EmployeeClearanceId = Convert.ToInt32(dr["EmployeeClearanceId"]),
                            EmployeeClearanceNo= Convert.ToString(dr["EmployeeClearanceNo"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            ExperienceLetter = Convert.ToString(dr["ExperienceLetter"]),
                            SepartionRemarks = Convert.ToString(dr["SepartionRemarks"]),
                            SeparationStatus = Convert.ToString(dr["SeparationStatus"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                          
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationOrders;
        }
        public SeparationOrderViewModel GetEmployeeInterviewClearanceDetail(long employeeId = 0)
        {
            SeparationOrderViewModel separationOrder = new SeparationOrderViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtSeparationOrders = sqlDbInterface.GetEmployeeInterviewClearanceDetail(employeeId);
                if (dtSeparationOrders != null && dtSeparationOrders.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSeparationOrders.Rows)
                    {
                        separationOrder = new SeparationOrderViewModel
                        {
                            ExitInterviewId = Convert.ToInt32(dr["ExitInterviewId"]),
                            ExitInterviewNo = Convert.ToString(dr["ExitInterviewNo"]),
                            EmployeeClearanceId = Convert.ToInt32(dr["EmployeeClearanceId"]),
                            EmployeeClearanceNo= Convert.ToString(dr["EmployeeClearanceNo"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationOrder;
        }
    }
}
