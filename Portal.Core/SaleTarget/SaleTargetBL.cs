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
  public  class SaleTargetBL
    {
        DBInterface dbInterface;
        public SaleTargetBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditTarget(TargetViewModel targetViewModel, List<TargetDetailViewModel> targetDetails)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                Target target = new Target
                {
                    TargetId = targetViewModel.TargetId,
                    TargetDate = Convert.ToDateTime(targetViewModel.TargetDate),
                    TargetFromDate = Convert.ToDateTime(targetViewModel.TargetFromDate),
                    TargetToDate = Convert.ToDateTime(targetViewModel.TargetToDate),
                    CompanyId = targetViewModel.CompanyId,
                    CompanyBranchId = targetViewModel.CompanyBranchId,
                    Frequency= targetViewModel.Frequency,
                    Remarks = targetViewModel.Remarks,
                    FinYearId=targetViewModel.FinYearId,
                    CreatedBy = targetViewModel.CreatedBy,
                    TargetStatus = targetViewModel.TargetStatus,
                    Status= targetViewModel.Status
                };
               
                List<TargetDetail> targetDetailList = new List<TargetDetail>();
                if (targetDetails != null && targetDetails.Count > 0)
                {
                    foreach (TargetDetailViewModel item in targetDetails)
                    {
                        targetDetailList.Add(new TargetDetail
                        {
                            EmpId=item.EmpId,
                            DesignationId=item.DesignationId,
                            ProductId = item.ProductId,
                            StateId=item.StateId,
                            CityId=item.CityId,
                            Vehicles=item.Vehicles,
                            TargetQty=item.TargetQty,
                            TargetAmount=item.Amount,
                            TargetTypeId = item.TargetTypeId,
                            TargetDealershipsNos=item.TargetDealershipsNos
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditTarget(target, targetDetailList);

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
        public TargetViewModel GetTargetDetail(long targetId = 0)
        {
            TargetViewModel target = new TargetViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTargets = sqlDbInterface.GetTargetDetail(targetId);
                if (dtTargets != null && dtTargets.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTargets.Rows)
                    {
                        target = new TargetViewModel
                        {
                            TargetId = Convert.ToInt32(dr["TargetId"]),
                            TargetNo = Convert.ToString(dr["TargetNo"]),
                            TargetDate = Convert.ToString(dr["TargetDate"]),
                            TargetFromDate = Convert.ToString(dr["TargetFromDate"]),
                            TargetToDate = Convert.ToString(dr["TargetToDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            Frequency = Convert.ToString(dr["Frequency"]),
                            TargetStatus = Convert.ToString(dr["TargetStatus"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
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
            return target;
        }
        public List<TargetDetailViewModel> GetTargetDetailList(long targetId)
        {
            List<TargetDetailViewModel> targetDetails = new List<TargetDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTargets = sqlDbInterface.GetTargetDetailList(Convert.ToInt32(targetId));
                if (dtTargets != null && dtTargets.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTargets.Rows)
                    {
                        targetDetails.Add(new TargetDetailViewModel
                        {

                            TargetDetailId = Convert.ToInt32(dr["TargetDetailId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            EmpId = Convert.ToInt32(dr["EmpId"]),
                            EmployeeName=Convert.ToString(dr["EmployeeName"]),
                            DesignationId= Convert.ToInt32(dr["DesignationId"]),
                            DesignationName=Convert.ToString(dr["DesignationName"]),
                            TargetTypeId=Convert.ToInt32(dr["TargetTypeId"]),
                            TargetTypeName=Convert.ToString(dr["TargetTypeName"]),
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName=Convert.ToString(dr["ProductName"]),
                            StateId =Convert.ToInt32(dr["StateId"]),
                            StateName=Convert.ToString(dr["StateName"]),
                            CityId = Convert.ToInt32(dr["CityId"]),
                            CityName=Convert.ToString(dr["CityName"]),
                            Vehicles =Convert.ToInt32(dr["Vehicles"]),
                            TargetQty=Convert.ToInt32(dr["TargetQty"]),
                            Amount = Convert.ToInt32(dr["TargetAmount"]),
                            PerDealar=Convert.ToInt32(dr["PerDealar"]),
                            DealershipsNos= Convert.ToInt32(dr["DealershipsNos"]),
                            TargetDealershipsNos= Convert.ToInt32(dr["TargetDealershipsNos"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return targetDetails;
        }
        public List<EmployeeViewModel> GetEmployeeAutoCompleteList(string searchTerm,long companyBranchId)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            try
            {
                List<Employee> employeeList = dbInterface.GetEmployeeAutoCompleteList(searchTerm, companyBranchId);

                if (employeeList != null && employeeList.Count > 0)
                {
                    foreach (Employee employee in employeeList)
                    {
                        employees.Add(new EmployeeViewModel {
                            EmployeeId = employee.EmployeeId,
                            FirstName = employee.FirstName + " " + employee.LastName,
                            EmployeeCode = employee.EmployeeCode,
                            MobileNo = employee.MobileNo,
                            DepartmentId = Convert.ToInt32(employee.DepartmentId),
                            DesignationId = Convert.ToInt32(employee.DesignationId) });
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

        public List<CityViewModel> GetCityAutoCompleteList(string searchTerm, int stateId)
        {
            List<CityViewModel> cities = new List<CityViewModel>();
            try
            {
                List<City> cityList = dbInterface.GetCityAutoCompleteList(searchTerm, stateId);

                if (cityList != null && cityList.Count > 0)
                {
                    foreach (City city in cityList)
                    {
                        cities.Add(new CityViewModel { CityId = city.CityId,CityName=city.CityName, Vehicles=Convert.ToInt32(city.Vehicles),PerDealar=Convert.ToInt32(city.PerDealar),DealershipsNos=Convert.ToInt32(city.DealershipsNos)});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return cities;
        }

        public List<TargetViewModel> GetTargetList(string targetNo, int companyBranchId, string fromDate, string toDate, int companyId, string displayType = "", string approvalStatus = "")
        {
            List<TargetViewModel> targets = new List<TargetViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTargets = sqlDbInterface.GetTargetList(targetNo, companyBranchId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, displayType, approvalStatus);
                if (dtTargets != null && dtTargets.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTargets.Rows)
                    {
                        targets.Add(new TargetViewModel
                        {
                            TargetId = Convert.ToInt32(dr["TargetId"]),
                            TargetNo = Convert.ToString(dr["TargetNo"]),
                            TargetDate = Convert.ToString(dr["TargetDate"]),
                            TargetFromDate = Convert.ToString(dr["TargetFromDate"]),
                            TargetToDate = Convert.ToString(dr["TargetToDate"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            TargetStatus = Convert.ToString(dr["TargetStatus"]),
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
            return targets;
        }

        public DataTable GetTargetDataTable(long targetId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTarget = new DataTable();
            try
            {
                dtTarget = sqlDbInterface.GetTargetDetail(targetId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTarget;
        }

        public DataTable GetTargetDetailDataTable(int targetId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtTargetDetails = new DataTable();
            try
            {
                dtTargetDetails = sqlDbInterface.GetTargetDetailList(targetId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtTargetDetails;
        }
        public DataTable GetSaleTargetTypeDataTable(int salesEmpid,int userId, string fromDate, string toDate, int companyId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtSaleTargetType = new DataTable();
            try
            {
                dtSaleTargetType = sqlDbInterface.GetSaleTargetTypeDataTable(salesEmpid, userId,Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate),companyId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtSaleTargetType;
        }


        public List<UserViewModel> GetEmployeeList(int userid)
        {
            SQLDbInterface sQLDbInterface = new SQLDbInterface();
            List<UserViewModel> User = new List<UserViewModel>();
            try
            {
                
                DataTable dtSalesEmployee = sQLDbInterface.GetEmployeeList(userid);
                if (dtSalesEmployee != null && dtSalesEmployee.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSalesEmployee.Rows)
                    {
                        User.Add(new UserViewModel
                        {
                            UserId = Convert.ToInt32(dr["userid"]),
                            UserName = Convert.ToString(dr["UserName"])
                          
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return User;
        }

    }
}
