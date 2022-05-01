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
    public class GLBL
    {
        DBInterface dbInterface;
        public GLBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditGLMapping(GLDetailViewModel gLDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {

                GLDetail glDetail = new GLDetail
                {
                    FinYearId = gLDetailViewModel.FinYearId,
                    OpeningBalanceDebit = gLDetailViewModel.OpeningBalanceDebit,
                    OpeningBalanceCredit = gLDetailViewModel.OpeningBalanceCredit,
                    OpeningBalance = gLDetailViewModel.OpeningBalance,
                    CompanyBranchId = gLDetailViewModel.CompanyBranchId,
                    GLId = gLDetailViewModel.GLId,
                    Status =true,
                    CompanyId= gLDetailViewModel.CompanyId,
                    CreatedBy= gLDetailViewModel.CreatedBy
                   

                };

                responseOut = sqlDbInterface.AddEditGLDetailMappingOpeningStock(glDetail);

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
        public ResponseOut AddEditGL(GLViewModel glViewModel,GLDetailViewModel gLDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                GL gl = new GL {
                    GLId = glViewModel.GLId,
                    GLCode = glViewModel.GLCode,
                    GLHead=glViewModel.GLHead,
                    GLType = glViewModel.GLType,
                    GLSubGroupId = glViewModel.GLSubGroupId,
                    SLTypeId=glViewModel.SLTypeId,
                    IsBookGL=glViewModel.IsBookGL,
                    IsBranchGL=glViewModel.IsBranchGL,
                    IsDebtorGL=glViewModel.IsDebtorGL,
                    IsCreditorGL=glViewModel.IsCreditorGL,
                    IsTaxGL= glViewModel.IsTaxGL,
                    IsPostGL=glViewModel.IsPostGL,
                    CompanyId=glViewModel.CompanyId,
                    CreatedBy = glViewModel.CreatedBy,
                    Status =glViewModel.GLStatus
                   };

                GLDetail glDetail = new GLDetail {
                    FinYearId= gLDetailViewModel.FinYearId,
                    OpeningBalanceDebit= gLDetailViewModel.OpeningBalanceDebit,
                    OpeningBalanceCredit= gLDetailViewModel.OpeningBalanceCredit,
                    OpeningBalance= gLDetailViewModel.OpeningBalance,
                    CompanyBranchId = gLDetailViewModel.CompanyBranchId
                };
            
                responseOut = sqlDbInterface.AddEditGL(gl,glDetail);

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

        public List<GLViewModel> GetGLList(string GLHead = "", string GLCode = "", string GLType = "", int GLMainGroupId = 0, int GLSubGroupId = 0, int SLTypeId = 0, int companyId = 0,int finYear=0,int companyBranchId=0)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLs = sqlDbInterface.GetGLList(GLHead,GLCode, GLType,GLMainGroupId,GLSubGroupId,SLTypeId,companyId, finYear, companyBranchId);
                if (dtGLs != null && dtGLs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLs.Rows)
                    {
                        gls.Add(new GLViewModel
                        {
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            GLType = Convert.ToString(dr["GLType"]),
                            GLSubGroupId = Convert.ToInt32(dr["GLSubGroupId"]),
                            GLSubGroupName=Convert.ToString(dr["GLSubGroupName"]),
                            GLMainGroupId=Convert.ToInt32(dr["GLMainGroupId"]),
                            GLMainGroupName=Convert.ToString(dr["GLMainGroupName"]),
                            SLTypeId = Convert.ToInt32(dr["SLTypeId"]),
                            SLTypeName=Convert.ToString(dr["SLTypeName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ModifiedByUserName= Convert.ToString(dr["ModifiedByName"]),
                            GLStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"]),
                            OpeningBalanceCredit = Convert.ToDecimal(dr["OpeningBalanceCredit"]),
                            OpeningBalanceDebit = Convert.ToDecimal(dr["OpeningBalanceDebit"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gls;
        }

        public List<GLViewModel> GetGLOpeningList(string GLHead = "", string GLCode = "", string GLType = "", int GLMainGroupId = 0, int GLSubGroupId = 0, int SLTypeId = 0, int companyId = 0, int finYear = 0, int companyBranchId = 0)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLs = sqlDbInterface.GetGLOpeningList(GLHead, GLCode, GLType, GLMainGroupId, GLSubGroupId, SLTypeId, companyId, finYear, companyBranchId);
                if (dtGLs != null && dtGLs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLs.Rows)
                    {
                        gls.Add(new GLViewModel
                        {
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            GLType = Convert.ToString(dr["GLType"]),
                            GLSubGroupId = Convert.ToInt32(dr["GLSubGroupId"]),
                            GLSubGroupName = Convert.ToString(dr["GLSubGroupName"]),
                            GLMainGroupId = Convert.ToInt32(dr["GLMainGroupId"]),
                            GLMainGroupName = Convert.ToString(dr["GLMainGroupName"]),
                            SLTypeId = Convert.ToInt32(dr["SLTypeId"]),
                            SLTypeName = Convert.ToString(dr["SLTypeName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            GLStatus = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                            OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"]),
                            OpeningBalanceCredit = Convert.ToDecimal(dr["OpeningBalanceCredit"]),
                            OpeningBalanceDebit = Convert.ToDecimal(dr["OpeningBalanceDebit"]),
                            FinyearDesc = Convert.ToString(dr["FinYearDesc"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gls;
        }

        public GLViewModel GetGLDetail(int glId = 0,int finYearId=2017)
        {
            GLViewModel glViewModel = new GLViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLs = sqlDbInterface.GetGLDetail(glId, finYearId);
                if (dtGLs != null && dtGLs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLs.Rows)
                    {
                        glViewModel = new GLViewModel
                        {
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            GLType = Convert.ToString(dr["GLType"]),
                            GLSubGroupId= Convert.ToInt32(dr["GLSubGroupId"]),
                            GLSubGroupName = Convert.ToString(dr["GLSubGroupName"]),
                            GLMainGroupId =Convert.ToInt32(dr["GLMainGroupId"]),
                            GLMainGroupName = Convert.ToString(dr["GLMainGroupName"]),
                            SLTypeId =Convert.ToInt32(dr["SLTypeId"]),
                            IsBookGL=Convert.ToBoolean(dr["IsBookGL"]),

                            IsBranchGL = Convert.ToBoolean(dr["IsBranchGL"]),
                            IsCreditorGL = Convert.ToBoolean(dr["IsCreditorGL"]),
                            IsDebtorGL = Convert.ToBoolean(dr["IsDebtorGL"]),
                            IsPostGL = Convert.ToBoolean(dr["IsPostGL"]),
                            IsTaxGL = Convert.ToBoolean(dr["IsTaxGL"]),

                            GLStatus=Convert.ToBoolean(dr["Status"]),
                            OpeningBalanceDebit = Convert.ToDecimal(dr["OpeningBalanceDebit"]),
                            OpeningBalanceCredit = Convert.ToDecimal(dr["OpeningBalanceCredit"]),
                            OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"])



                            //CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            //ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glViewModel;
        }

        public GLViewModel GetGLDetailOpening(int glId = 0, int finYearId = 2017, long companyBranchId = 0)
        {
            GLViewModel glViewModel = new GLViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLs = sqlDbInterface.GetGLDetailOpening(glId, finYearId, companyBranchId);
                if (dtGLs != null && dtGLs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLs.Rows)
                    {
                        glViewModel = new GLViewModel
                        {
                            GLId = Convert.ToInt32(dr["GLId"]),                          
                            OpeningBalanceDebit = Convert.ToDecimal(dr["OpeningBalanceDebit"]),
                            OpeningBalanceCredit = Convert.ToDecimal(dr["OpeningBalanceCredit"]),
                            OpeningBalance = Convert.ToDecimal(dr["OpeningBalance"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"])                           
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glViewModel;
        }

        public List<GLSubGroupViewModel> GetGLSubGroupList(int mainGroupid)
        {
            List<GLSubGroupViewModel> glSubGroups = new List<GLSubGroupViewModel>();
            try
            {
                List<GLSubGroup> glSubGroupList = dbInterface.GetGLSubGroupList(mainGroupid);
                if (glSubGroupList != null && glSubGroupList.Count > 0)
                {
                    foreach (GLSubGroup glSubGroup in glSubGroupList)
                    {
                        glSubGroups.Add(new GLSubGroupViewModel { GLSubGroupId = glSubGroup.GLSubGroupId, GLSubGroupName = glSubGroup.GLSubGroupName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glSubGroups;
        }

        public List<GLViewModel> GetGLAutoCompleteList(string searchTerm, int companyId)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            try
            {
                List<GL> glList = dbInterface.GetGLAutoCompleteList(searchTerm, companyId);
                if (glList != null && glList.Count > 0)
                {
                    foreach (GL gl in glList)
                    {
                        gls.Add(new GLViewModel { GLId = gl.GLId, GLHead = gl.GLHead, GLCode = gl.GLCode, SLTypeId =Convert.ToInt16(gl.SLTypeId) });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gls;
        }


        public List<GLViewModel> GetPostingGLAutoCompleteList(string term, int slTypeId,  int companyId)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            try
            {
                List<GL> glList = dbInterface.GetPostingGLAutoCompleteList(term, slTypeId, companyId);
                if (glList != null && glList.Count > 0)
                {
                    foreach (GL gl in glList)
                    {
                        gls.Add(new GLViewModel { GLId = gl.GLId, GLHead = gl.GLHead, GLCode = gl.GLCode});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gls;
        }

        public List<GLViewModel> GetGLAutoCompleteListForTax(string term, int companyId)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            try
            {
                List<GL> glList = dbInterface.GetGLAutoCompleteListsForTax(term, companyId);
                if (glList != null && glList.Count > 0)
                {
                    foreach (GL gl in glList)
                    {
                        gls.Add(new GLViewModel { GLId = gl.GLId, GLHead = gl.GLHead, GLCode = gl.GLCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gls;
        }
        public List<GLViewModel> GetGLAutoCompleteListForVoucher(string searchTerm,int bookId, int companyId)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLList = sqlDbInterface.GetGLAutoCompleteList(searchTerm,bookId, companyId);
                if (dtGLList != null && dtGLList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLList.Rows)
                    {
                        gls.Add(new GLViewModel {
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            SLTypeId = Convert.ToInt16(dr["SLTypeId"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gls;
        }
        public List<GLViewModel> GetGLAutoCompleteListForJournalVoucher(string searchTerm, int companyId)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtGLList = sqlDbInterface.GetJVGLAutoCompleteList(searchTerm, companyId);
                if (dtGLList != null && dtGLList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGLList.Rows)
                    {
                        gls.Add(new GLViewModel
                        {
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            SLTypeId = Convert.ToInt16(dr["SLTypeId"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gls;
        }
        public List<GLViewModel> GetGLAutoCompleteList(string searchTerm, int slTypeId, int companyId)
        {
            List<GLViewModel> sls = new List<GLViewModel>();
            try
            {
                List<GL> glList = dbInterface.GetGLAutoCompleteList(searchTerm, slTypeId, companyId);
                if (glList != null && glList.Count > 0)
                {
                    foreach (GL gl in glList)
                    {
                        sls.Add(new GLViewModel { GLId = gl.GLId, GLHead = gl.GLHead, GLCode = gl.GLCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sls;
        }
        public List<GLViewModel> GetGLAutoCompleteListForProductGLMapping(string term, int companyId)
        {
            List<GLViewModel> gls = new List<GLViewModel>();
            try
            {
                List<GL> glList = dbInterface.GetGLAutoCompleteListForProductGLMapping(term, companyId);
                if (glList != null && glList.Count > 0)
                {
                    foreach (GL gl in glList)
                    {
                        gls.Add(new GLViewModel { GLId = gl.GLId, GLHead = gl.GLHead, GLCode = gl.GLCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gls;
        }
        public ResponseOut ImportGL(GLViewModel glViewModel, GLDetailViewModel gLDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                GL gl = new GL
                {
                    GLId = glViewModel.GLId,
                    GLCode = glViewModel.GLCode,
                    GLHead = glViewModel.GLHead,
                    GLType = glViewModel.GLType,
                    CompanyId = glViewModel.CompanyId,
                    GLSubGroupId = glViewModel.GLSubGroupId,
                    IsBookGL = glViewModel.IsBookGL,
                    IsBranchGL = glViewModel.IsBranchGL,
                    IsCreditorGL = glViewModel.IsCreditorGL,
                    IsDebtorGL = glViewModel.IsDebtorGL,
                    IsPostGL = glViewModel.IsPostGL,
                    IsTaxGL = glViewModel.IsTaxGL,
                    SLTypeId = glViewModel.SLTypeId,
                    CreatedBy = glViewModel.CreatedBy,
                    Status = glViewModel.GLStatus
                };
                GLDetail glDetail = new GLDetail
                {
                    FinYearId = gLDetailViewModel.FinYearId,
                    OpeningBalanceDebit = gLDetailViewModel.OpeningBalanceDebit,
                    OpeningBalanceCredit = gLDetailViewModel.OpeningBalanceCredit,
                    OpeningBalance = gLDetailViewModel.OpeningBalance
                };

                responseOut = sqlDbInterface.AddEditGL(gl, glDetail);
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
        public ResponseOut ImportGLDetail(GLDetailViewModel gLDetailViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                GLDetail glDetail = new GLDetail
                {
                    GLId = gLDetailViewModel.GLId,
                    FinYearId = gLDetailViewModel.FinYearId,
                    OpeningBalanceDebit = gLDetailViewModel.OpeningBalanceDebit,
                    OpeningBalanceCredit = gLDetailViewModel.OpeningBalanceCredit, 
                    CompanyId =gLDetailViewModel.CompanyId,
                    CreatedBy = gLDetailViewModel.CreatedBy,
                    Status = gLDetailViewModel.GLDetailStatus,
                    OpeningBalance = gLDetailViewModel.OpeningBalance
                    
                };

                responseOut = sqlDbInterface.AddEditGLDetail(glDetail);
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
        public DataTable GLReport(string GLHead = "", string GLCode = "", string GLType = "", int GLMainGroupId = 0, int GLSubGroupId = 0, int SLTypeId = 0, int companyId = 0, int finYear = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtGlReport = new DataTable();
            try
            {
                dtGlReport = sqlDbInterface.GlReport(GLHead, GLCode, GLType, GLMainGroupId, GLSubGroupId, SLTypeId, companyId, finYear);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtGlReport;
        }



    }
}
