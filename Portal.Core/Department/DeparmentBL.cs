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
    public class DepartmentBL
    {
        DBInterface dbInterface;
        public DepartmentBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditDepartment(DepartmentViewModel departmentViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Department department = new Department
                {
                    DepartmentId = departmentViewModel.DepartmentId,
                    DepartmentName = departmentViewModel.DepartmentName,
                    DepartmentCode = departmentViewModel.DepartmentCode,   
                    CompanyId = departmentViewModel.CompanyId,
                    CreatedBy = departmentViewModel.CreatedBy,
                    CreatedDate = DateTime.Now, 
                    Status= departmentViewModel.DepartmentStatus,
                    CompanyBranchId = departmentViewModel.CompanyBranchId
                };
                responseOut = dbInterface.AddEditDepartment(department);
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

        public List<DepartmentViewModel> GetDepartmentList(string departmentName = "", string departmentCode = "", string Status="", int companyId = 0,int companyBranchId=0)
        {
            List<DepartmentViewModel> departments = new List<DepartmentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtdepartments = sqlDbInterface.GetDepartmentList(departmentName, departmentCode, Status, companyId, companyBranchId);
                if (dtdepartments != null && dtdepartments.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtdepartments.Rows)
                    {
                        departments.Add(new DepartmentViewModel
                        {
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            DepartmentCode = Convert.ToString(dr["DepartmentCode"]),
                            CompanyId = Convert.ToInt32(dr["CompanyId"]), 
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]), 
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()), 
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]), 
                            DepartmentStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return departments;
        }
        public DepartmentViewModel GetDepartmentDetail(int departmentId = 0)
        {
           DepartmentViewModel department = new DepartmentViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDepartments = sqlDbInterface.GetDepartmentDetail(departmentId);
                if (dtDepartments != null && dtDepartments.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDepartments.Rows)
                    {
                        department = new DepartmentViewModel
                        {
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            DepartmentCode = Convert.ToString(dr["DepartmentCode"]),
                            CompanyId = Convert.ToInt32(dr["CompanyId"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            DepartmentStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return department;
        }


        public List<DepartmentViewModel> GetDepartmentList(int companyId)
        {
            List<DepartmentViewModel> departments = new List<DepartmentViewModel>();
            try
            {
                List<Department> departmentList = dbInterface.GetDepartmentList(companyId);
                if (departmentList != null && departmentList.Count > 0)
                {
                    foreach (Department department in departmentList)
                    {
                        departments.Add(new DepartmentViewModel { DepartmentId = department.DepartmentId, DepartmentName = department.DepartmentName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return departments;
        }

        public ResponseOut ImportDepartment(DepartmentViewModel departmentViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Department department = new Department
                {
                    DepartmentId = departmentViewModel.DepartmentId,
                    DepartmentName = departmentViewModel.DepartmentName,
                    DepartmentCode = departmentViewModel.DepartmentCode,
                    CompanyId = departmentViewModel.CompanyId,
                    CreatedBy = departmentViewModel.CreatedBy,
                    CreatedDate = DateTime.Now,
                    Status = departmentViewModel.DepartmentStatus
                };
                responseOut = dbInterface.AddEditDepartment(department);
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

        public List<DepartmentViewModel> GetDepartmentsList(int departmentID)
        {
            List<DepartmentViewModel> departmentViewModel = new List<DepartmentViewModel>();
            try
            {
                List<Department> locationList = dbInterface.GetDepartmentsList(departmentID);
                if (locationList != null && locationList.Count > 0)
                {
                    foreach (Department location in locationList)
                    {
                        departmentViewModel.Add(new DepartmentViewModel { DepartmentId = location.DepartmentId, DepartmentName = location.DepartmentName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return departmentViewModel;
        }
    }
}
