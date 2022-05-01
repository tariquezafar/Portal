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
    public class DesignationBL
    {
        DBInterface dbInterface;
        public DesignationBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditDesignation(DesignationViewModel designationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Designation designation = new Designation
                {
                    DesignationId = designationViewModel.DesignationId,
                    DesignationName = designationViewModel.DesignationName,
                    DesignationCode = designationViewModel.DesignationCode,
                    DepartmentId = designationViewModel.DepartmentId,
                    CreatedBy = designationViewModel.CreatedBy,
                    CreatedDate = DateTime.Now, 
                    Status= designationViewModel.DesignationStatus
                };
                responseOut = dbInterface.AddEditDesignation(designation);
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

        public List<DesignationViewModel> GetDesignationList(string designationName = "", string designationCode = "", int departmentId = 0, string Status = "")
        {
            List<DesignationViewModel> designations = new List<DesignationViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtdesignations = sqlDbInterface.GetDesignationList(designationName, designationCode, departmentId, Status );
                if (dtdesignations != null && dtdesignations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtdesignations.Rows)
                    {
                        designations.Add(new DesignationViewModel
                        {
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),
                            DesignationCode = Convert.ToString(dr["DesignationCode"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            DesignationStatus = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return designations;
        }
        public DesignationViewModel GetDesignationDetail(int designationId = 0)
        {
            DesignationViewModel designation = new DesignationViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDesignations = sqlDbInterface.GetDesignationDetail(designationId);
                if (dtDesignations != null && dtDesignations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDesignations.Rows)
                    {
                        designation = new DesignationViewModel
                        {
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),
                            DesignationCode = Convert.ToString(dr["DesignationCode"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]), 
                            DesignationStatus = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return designation;
        }
        public List<DesignationViewModel> GetDesignationList(int departmentId)
        {
            List<DesignationViewModel> designations = new List<DesignationViewModel>();
            try
            {
                List<Designation> designationList = dbInterface.GetDesignationList(departmentId);
                if (designationList != null && designationList.Count > 0)
                {
                    foreach (Designation designation in designationList)
                    {
                        designations.Add(new DesignationViewModel { DesignationId = designation.DesignationId, DesignationName = designation.DesignationName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return designations;
        }
        public List<DesignationViewModel> GetAllDesignationList()
        {
            List<DesignationViewModel> designations = new List<DesignationViewModel>();
            try
            {
                List<Designation> designationList = dbInterface.GetAllDesignationList();
                if (designationList != null && designationList.Count > 0)
                {
                    foreach (Designation designation in designationList)
                    {
                        designations.Add(new DesignationViewModel { DesignationId = designation.DesignationId, DesignationName = designation.DesignationName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return designations;
        }

        public List<DesignationViewModel> GetDesignationList()
        {
            List<DesignationViewModel> designations = new List<DesignationViewModel>();
            try
            {
                List<Designation> designationList = dbInterface.GetDesignationList();
                if (designationList != null && designationList.Count > 0)
                {
                    foreach (Designation designation in designationList)
                    {
                        designations.Add(new DesignationViewModel { DesignationId = designation.DesignationId, DesignationName = designation.DesignationName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return designations;
        }


        public ResponseOut ImportDesignation(DesignationViewModel designationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Designation designation = new Designation
                {
                    DesignationId = designationViewModel.DesignationId,
                    DesignationName = designationViewModel.DesignationName,
                    DesignationCode = designationViewModel.DesignationCode,
                    DepartmentId = designationViewModel.DepartmentId,
                    CreatedBy = designationViewModel.CreatedBy,
                    CreatedDate = DateTime.Now,
                    Status = designationViewModel.DesignationStatus
                };
                responseOut = dbInterface.AddEditDesignation(designation);
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
