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
    public class TargetTypeBL
    {
        DBInterface dbInterface;
        public TargetTypeBL()
        {
            dbInterface = new DBInterface();
        }
     
        public ResponseOut AddEditTargetType(TargetTypeViewModel targetTypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                TargetType targettype=new TargetType
               {
                    TargetTypeId = targetTypeViewModel.TargetTypeId,
                    TargetName = targetTypeViewModel.TargetName,
                    TargetDesc = targetTypeViewModel.TargetDesc,
                    Status = targetTypeViewModel.TargetType_Status,
                    CreatedBy=targetTypeViewModel.CreatedBy,
                    CreatedDate=Convert.ToDateTime(targetTypeViewModel.CreatedDate),
                    Modifiedby=targetTypeViewModel.Modifiedby,
                    ModifiedDate=Convert.ToDateTime(targetTypeViewModel.ModifiedDate),
                    CompanyBranchId=Convert.ToInt32(targetTypeViewModel.CompanyBranchId)
                };
                responseOut = dbInterface.AddEditTargetType(targettype);
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


      
        public List<TargetTypeViewModel> GetTargetTypeList(string targettypeName, string targettypeDesc, int status, int userid,int companyBranchId)
        {
            List<TargetTypeViewModel> targetTypeViewModel = new List<TargetTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
               
                DataTable dttargetType = sqlDbInterface.GetTargetTypeList(targettypeName, targettypeDesc, status, userid, companyBranchId);
                if (dttargetType != null && dttargetType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dttargetType.Rows)
                    {
                        targetTypeViewModel.Add(new TargetTypeViewModel
                        {
                            TargetName = Convert.ToString(dr["TargetName"]),
                            TargetDesc = Convert.ToString(dr["TargetDesc"]),
                            CreatedByUser = Convert.ToString(dr["CreatedBy"]),
                            ModifyByUser = Convert.ToString(dr["Modifiedby"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            TargetTypeId = Convert.ToInt32(dr["TargetTypeId"]),
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
            return targetTypeViewModel;
        }

       
        public TargetTypeViewModel GetTargetTypeDetail(int targettypeId = 0)
        {
            TargetTypeViewModel targetTypeViewModel = new TargetTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTargetType = sqlDbInterface.GetTargetTypeDetails(targettypeId);
                if (dtTargetType != null && dtTargetType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTargetType.Rows)
                    {
                        targetTypeViewModel = new TargetTypeViewModel
                        {
                           TargetName= Convert.ToString(dr["TargetName"]),
                           TargetDesc = Convert.ToString(dr["TargetDesc"]),
                           Status = Convert.ToBoolean(dr["Status"]),
                           CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"])
                         
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return targetTypeViewModel;
        }

        public List<TargetTypeViewModel> GetTargetTypeList()
        {
            List<TargetTypeViewModel> targetTypeList = new List<TargetTypeViewModel>();
            try
            {
                List<TargetType> states = dbInterface.GetTargetTypeList();
                if (states != null && states.Count > 0)
                {
                    foreach (Portal.DAL.TargetType targetType in states)
                    {
                        targetTypeList.Add(new TargetTypeViewModel { TargetTypeId = targetType.TargetTypeId, TargetName = targetType.TargetName});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return targetTypeList;
        }
    }
}
