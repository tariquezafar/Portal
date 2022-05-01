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
  public  class ProjectBL
    {
        DBInterface dbInterface;
        public ProjectBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditProject(ProjectViewModel projectViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {

                Project project = new Project
                {

                   ProjectId = projectViewModel.ProjectId,
                    ProjectName= projectViewModel.ProjectName,
                    ProjectCode = projectViewModel.ProjectCode,
                    CustomerBranchId = projectViewModel.CustomerBranchId,
                    CustomerId = projectViewModel.CustomerId,
                    CreatedBy = projectViewModel.CreatedBy,
                    ProjectStatus = projectViewModel.ProjectStatus,
                    CompanyBranchId=projectViewModel.CompanyBranchId,

                };
                responseOut = dbInterface.AddEditProject(project);
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

        public List<CityViewModel> GetCityList(int stateId = 0)
        {
            List<CityViewModel> cityList = new List<CityViewModel>();
            try
            {
                List<Portal.DAL.City> cities = dbInterface.GetCityList(stateId);
                if (cities != null && cities.Count > 0)
                {
                    foreach (Portal.DAL.City city in cities)
                    {
                        cityList.Add(new CityViewModel {CityId = city.CityId,CityName=city.CityName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return cityList;
        }
        
        public List<ProjectViewModel> GetProjectList(string projectName = "",int projectCode = 0,int customerId = 0 , int customerBranchId=0,string projectStatus="",int companyBranchId=0)
        {
            List<ProjectViewModel> projects = new List<ProjectViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                
                DataTable dtProjects = sqlDbInterface.GetProjectList(projectName, projectCode, customerId, customerBranchId, projectStatus, companyBranchId);
                if (projects != null && dtProjects.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProjects.Rows)
                    {

                        projects.Add(new ProjectViewModel {
                            ProjectId = Convert.ToInt32(dr["ProjectId"]),
                            ProjectName = Convert.ToString(dr["ProjectName"]),
                            ProjectCode = Convert.ToString(dr["ProjectCode"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerBranchId = Convert.ToInt32(dr["CustomerBranchId"]),
                            CustomerBranchName = Convert.ToString(dr["CustomerBranchName"]),
                            ProjectStatus = Convert.ToString(dr["ProjectStatus"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"]),
                        });
                       
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return projects;
        }

        public ProjectViewModel GetProjectDetail(int projectId = 0)
        {
           
            ProjectViewModel project = new ProjectViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtprojects = sqlDbInterface.GetProjectDetail(projectId);
                if (dtprojects != null && dtprojects.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtprojects.Rows)
                    {
                        project = new ProjectViewModel
                        {
                            ProjectId = Convert.ToInt32(dr["ProjectId"]),
                            ProjectName = Convert.ToString(dr["ProjectName"]),
                            ProjectCode = Convert.ToString(dr["ProjectCode"]),
                            CustomerId = Convert.ToInt32(dr["CustomerId"]),
                            CustomerName = Convert.ToString(dr["CustomerName"]),
                            CustomerBranchId = Convert.ToInt32(dr["CustomerBranchId"]),
                            CustomerBranchName = Convert.ToString(dr["CustomerBranchName"]),
                            ProjectStatus = Convert.ToString(dr["ProjectStatus"]),
                            CompanyBranchName= Convert.ToString(dr["CompanyBranchName"]),
                            CompanyBranchId= Convert.ToInt32(dr["CompanyBranchId"])


                        };

                       
                    }
                }
            
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return project;
        }



    }
}
