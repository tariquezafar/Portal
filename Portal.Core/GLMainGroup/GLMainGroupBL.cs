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
    public class GLMainGroupBL
    {
        DBInterface dbInterface;
        public GLMainGroupBL()
        {
            dbInterface = new DBInterface();
        }


        public ResponseOut AddEditGLMainGroup(GLMainGroupViewModel glmaingroupViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                GLMainGroup glmaingroup = new GLMainGroup
                {
                   GLMainGroupId = glmaingroupViewModel.GLMainGroupId,
                   GLMainGroupName = glmaingroupViewModel.GLMainGroupName,
                    CompanyId = glmaingroupViewModel.CompanyId,
                    GLType  = glmaingroupViewModel.GLType,
                   SequenceNo = glmaingroupViewModel.SequenceNo,
                    CreatedBy = glmaingroupViewModel.CreatedBy,
                    Status = glmaingroupViewModel.GLMainGroup_Status
                };
                responseOut = dbInterface.AddEditGLMainGroup(glmaingroup);
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



        public List<GLMainGroupViewModel> GetGLMainGroupList(string glmaingroupName = "", string glType = "", int sequenceNo = 0, int companyId=0, string status = "")
        {
            List<GLMainGroupViewModel> glmaingroups = new List<GLMainGroupViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            { 
                DataTable dtGLMainGroups = sqlDbInterface.GetGLMainGroupList(glmaingroupName, glType, sequenceNo, companyId, status);
                if (dtGLMainGroups != null && dtGLMainGroups.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLMainGroups.Rows)
                    {
                        glmaingroups.Add(new GLMainGroupViewModel
                        {
                            GLMainGroupId = Convert.ToInt32(dr["GLMainGroupId"]),
                            GLMainGroupName = Convert.ToString(dr["GLMainGroupName"]),
                            GLType = Convert.ToString(dr["GLType"]),
                            SequenceNo = Convert.ToInt32(dr["SequenceNo"].ToString() == "" ? "0" : dr["SequenceNo"].ToString()),
                            GLMainGroup_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
             return glmaingroups;
        }

        public GLMainGroupViewModel GetGLMainGroupDetail(int glmaingroupId = 0)
        {
            GLMainGroupViewModel glmaingroup = new GLMainGroupViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLMainGroups = sqlDbInterface.GetGLMainGroupDetail(glmaingroupId);
                if (dtGLMainGroups != null && dtGLMainGroups.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLMainGroups.Rows)
                    {
                        glmaingroup = new GLMainGroupViewModel
                        {
                            GLMainGroupId = Convert.ToInt32(dr["GLMainGroupId"]),
                            GLMainGroupName = Convert.ToString(dr["GLMainGroupName"]),
                            GLType = Convert.ToString(dr["GLType"]),
                            SequenceNo = Convert.ToInt32(dr["SequenceNo"]),
                            GLMainGroup_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
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
            return glmaingroup;
        }



        public ResponseOut ImportGlMainGroup(GLMainGroupViewModel glmaingroupViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                GLMainGroup glmaingroup = new GLMainGroup
                {
                    GLMainGroupId = glmaingroupViewModel.GLMainGroupId,
                    GLMainGroupName = glmaingroupViewModel.GLMainGroupName,
                    CompanyId = glmaingroupViewModel.CompanyId,
                    GLType = glmaingroupViewModel.GLType,
                    SequenceNo = glmaingroupViewModel.SequenceNo,
                    CreatedBy = glmaingroupViewModel.CreatedBy,
                    Status = glmaingroupViewModel.GLMainGroup_Status
                };
                responseOut = dbInterface.AddEditGLMainGroup(glmaingroup);
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

        //public List<GLMainGroupViewModel> GetGLMainGroupList()
        //{
        //    List<GLMainGroupViewModel> glMainGroups = new List<GLMainGroupViewModel>();
        //    try
        //    {
        //        List<GLMainGroup> glMainGroupList = dbInterface.GetGLMainGroupList();
        //        if (glMainGroupList != null && glMainGroupList.Count > 0)
        //        {
        //            foreach (GLMainGroup glMainGroup in glMainGroupList)
        //            {
        //                glMainGroups.Add(new GLMainGroupViewModel { GLMainGroupId = glMainGroup.GLMainGroupId, GLMainGroupName = glMainGroup.GLMainGroupName });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
        //        throw ex;
        //    }
        //    return glMainGroups;
        //} 

    }
}
