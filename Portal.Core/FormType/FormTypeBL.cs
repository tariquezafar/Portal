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
    public class FormTypeBL
    {
        DBInterface dbInterface;
        public FormTypeBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditFormType(FormTypeViewModel formTypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {             
              FormType formType = new FormType
              {
                    FormTypeId = formTypeViewModel.FormTypeId,
                    FormTypeDesc = formTypeViewModel.FormTypeDesc,
                    CompanyId = formTypeViewModel.CompanyId,                                    
                    Status = formTypeViewModel.FormType_Status                                                  
                };
                responseOut = dbInterface.AddEditFormType(formType);
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
        public List<FormTypeViewModel> GetFormTypeList(string formTypeDesc = "", int companyId=0, string status = "")
        {
            List<FormTypeViewModel> formTypeViewModel = new List<FormTypeViewModel>();
          
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            { 
                DataTable dtSchedules = sqlDbInterface.GetFormTypeList(formTypeDesc, companyId, status);
                if (dtSchedules != null && dtSchedules.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSchedules.Rows)
                    {
                        formTypeViewModel.Add(new FormTypeViewModel
                        {
                            FormTypeId = Convert.ToInt32(dr["FormTypeId"]),
                            FormTypeDesc = Convert.ToString(dr["FormTypeDesc"]),                                                     
                            FormType_Status = Convert.ToBoolean(dr["Status"]),                          
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
             return formTypeViewModel;
        }
        public FormTypeViewModel GetScheduleDetail(int formTypeId = 0)
        {

            FormTypeViewModel formTypeViewModel = new FormTypeViewModel();          
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSchedules = sqlDbInterface.GetFormTypeDetail(formTypeId);
                if (dtSchedules != null && dtSchedules.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSchedules.Rows)
                    {
                        formTypeViewModel = new FormTypeViewModel
                        {
                            FormTypeId = Convert.ToInt32(dr["FormTypeId"]),
                            FormTypeDesc = Convert.ToString(dr["FormTypeDesc"]),
                            FormType_Status = Convert.ToBoolean(dr["Status"]),

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return formTypeViewModel;
        }

        public List<FormTypeViewModel> GetFormTypeList()
        {
         
            List<FormTypeViewModel> formTypeViewModel = new List<FormTypeViewModel>();
            try
            {
                List<Portal.DAL.FormType> formTypemodes = dbInterface.GetFormTypeList();
                if (formTypemodes != null && formTypemodes.Count > 0)
                {
                    foreach (Portal.DAL.FormType formTypemode in formTypemodes)
                    {
                        formTypeViewModel.Add(new FormTypeViewModel { FormTypeId = formTypemode.FormTypeId, FormTypeDesc = formTypemode.FormTypeDesc });
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return formTypeViewModel;
        }


    }
}
