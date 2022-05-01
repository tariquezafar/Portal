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
    public class TermTemplateBL
    {
        DBInterface dbInterface;
        public TermTemplateBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditTermTemplate(TermTemplateViewModel termtemplateViewModel, List<TermTemplateDetailViewModel> termtemplateDetail)
        {
            ResponseOut responseOutBranch = new ResponseOut();
            ResponseOut responseOutProduct = new ResponseOut();
            ResponseOut responseOut = new ResponseOut();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {

                    TermsTemplate termtemplate = new TermsTemplate
                    {
                        TermTemplateId = termtemplateViewModel.TermTemplateId,
                        TermTempalteName = termtemplateViewModel.TermTempalteName, 
                        CompanyId = termtemplateViewModel.CompanyId,
                        CreatedBy = termtemplateViewModel.CreatedBy, 
                        Status = termtemplateViewModel.TermTemplate_Status,
                        CompanyBranchId=termtemplateViewModel.CompanyBranchId,
                    };


                    int termtemplateId = 0;
                    responseOut = dbInterface.AddEditTermTemplate(termtemplate, out termtemplateId);
                    if (responseOut.status == ActionStatus.Success)
                    {
                         
                        if (termtemplateDetail != null && termtemplateDetail.Count > 0)
                        {
                            foreach (TermTemplateDetailViewModel termTemplateDetailViewModel in termtemplateDetail)
                            {
                                TermTemplateDetail termtemplatesDetail = new TermTemplateDetail
                                { 
                                    TrnId = termTemplateDetailViewModel.TrnId,
                                    TermTemplateId = termtemplateId,
                                    TermsDesc = termTemplateDetailViewModel.TermsDesc,
                                    Status = termTemplateDetailViewModel.Term_Status
                                };
                                responseOutProduct = dbInterface.AddEditTermTemplateDetail(termtemplatesDetail);
                            }
                        }

                    }
                    transactionscope.Complete();
                }
                catch (TransactionException ex1)
                {
                    transactionscope.Dispose();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex1);
                    throw ex1;
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                    throw ex;
                }
            }
            return responseOut;
        }
      

        public ResponseOut RemoveTermTemplate(long trnId)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                responseOut = dbInterface.RemoveTermTemplate(trnId);
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

        public List<TermTemplateViewModel> GetTermTemplateList(string termtemplateName, int companyId, string status,int companyBranchId)
        {
            List<TermTemplateViewModel> termtemplates = new List<TermTemplateViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTermTemplates = sqlDbInterface.GetTermTemplateList(termtemplateName, companyId, status, companyBranchId);
                if (dtTermTemplates != null && dtTermTemplates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTermTemplates.Rows)
                    {
                        termtemplates.Add(new TermTemplateViewModel
                        {
                            TermTemplateId = Convert.ToInt32(dr["TermTemplateId"]),
                            TermTempalteName = Convert.ToString(dr["TermTempalteName"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            TermTemplate_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName=Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return termtemplates;
        }
        public TermTemplateViewModel GetTermTemplateDetail(int termtemplateId = 0)
        {
            TermTemplateViewModel termtemplate = new TermTemplateViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTermTemplates = sqlDbInterface.GetTermTemplateDetail(termtemplateId);
                if (dtTermTemplates != null && dtTermTemplates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTermTemplates.Rows)
                    {
                        termtemplate = new TermTemplateViewModel
                        { 
                            TermTemplateId = Convert.ToInt32(dr["TermTemplateId"]),
                            TermTempalteName = Convert.ToString(dr["TermTempalteName"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            TermTemplate_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                            
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return termtemplate;
        }

        public List<TermTemplateDetailViewModel> GetTermTemplateDetailLists(int termtemplateId)
        {
            List<TermTemplateDetailViewModel> termTemplates = new List<TermTemplateDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTermTemplates = sqlDbInterface.GetTermTemplateDetailLists(termtemplateId);
                if (dtTermTemplates != null && dtTermTemplates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTermTemplates.Rows)
                    {
                        termTemplates.Add(new TermTemplateDetailViewModel
                        {
                            TrnId = Convert.ToInt32(dr["TrnId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            TermTemplateId = Convert.ToInt32(dr["TermTemplateId"]),
                            TermsDesc = Convert.ToString(dr["TermsDesc"]),
                            Term_Status = Convert.ToBoolean(dr["Status"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return termTemplates;
        }

        public List<TermTemplateDetailViewModel> GetTermTemplateDetailList(int termtemplateId)
        {
            List<TermTemplateDetailViewModel> termTemplates = new List<TermTemplateDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTermTemplates= sqlDbInterface.GetTermTemplateDetailList(termtemplateId);
                if (dtTermTemplates != null && dtTermTemplates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTermTemplates.Rows)
                    {
                        termTemplates.Add(new TermTemplateDetailViewModel
                        {
                            TrnId = Convert.ToInt32(dr["TrnId"]),
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            TermTemplateId = Convert.ToInt32(dr["TermTemplateId"]),
                            TermsDesc = Convert.ToString(dr["TermsDesc"]),
                            Term_Status= Convert.ToBoolean(dr["Status"]),
                           
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return termTemplates;
        }

        public List<TermTemplateViewModel> GetTermTemplateList(int companyId)
        {
            List<TermTemplateViewModel> termList = new List<TermTemplateViewModel>();
            try
            {
                List<Portal.DAL.TermsTemplate> terms = dbInterface.GetTermTemplateList(companyId);
                if (terms != null && terms.Count > 0)
                {
                    foreach (Portal.DAL.TermsTemplate term in terms)
                    {
                        termList.Add(new TermTemplateViewModel { TermTemplateId = term.TermTemplateId,TermTempalteName=term.TermTempalteName });
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return termList;
        }


    }
}
