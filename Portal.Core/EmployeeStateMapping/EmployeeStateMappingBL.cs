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
  public class EmployeeStateMappingBL
    {

        DBInterface dbInterface;
        public EmployeeStateMappingBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditEmpStateMapping(EmployeeStateMapping employeeStateMapping)
        {
          ResponseOut responseOut = new ResponseOut();
            try
            {
                responseOut = dbInterface.AddEditEmpStateMapping(employeeStateMapping);
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

        public List<EmployeeViewModel> GetEmployeeAutoCompleteList(string searchTerm)
        {
            
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            try
            {
                List<Employee> employeeList = dbInterface.GetEmployeeAutoCompleteList(searchTerm,0);

                if (employeeList != null && employeeList.Count > 0)
                {
                    foreach (Employee employee in employeeList)
                    {
                        employees.Add(new EmployeeViewModel { EmployeeId=employee.EmployeeId,FirstName=employee.FirstName+" "+employee.LastName,EmployeeCode=employee.EmployeeCode,MobileNo=employee.MobileNo });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employees;
        }

        public List<EmployeeStateMappingViewModel> GetStateList(int employeeId)
        {
            List<EmployeeStateMappingViewModel> states = new List<EmployeeStateMappingViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtStates = sqlDbInterface.GetStateMap(employeeId);
                if (dtStates != null && dtStates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStates.Rows)
                    {
                        states.Add(new EmployeeStateMappingViewModel
                        {
                            EmployeeStateMappingId=Convert.ToInt32(dr["EmployeeStateMappingId"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            StateCode = Convert.ToString(dr["StateCode"]),
                            SelectState= Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return states;
        }
    }
}
