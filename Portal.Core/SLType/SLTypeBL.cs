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
    public class SLTypeBL
    {
        DBInterface dbInterface;
        public SLTypeBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditSLType(SLTypeViewModel sLTypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                SLType sLTypeId = new SLType
                {
                    SLTypeId = sLTypeViewModel.SLTypeId,
                    SLTypeName = sLTypeViewModel.SLTypeName,
                    Status = sLTypeViewModel.SLType_Status,
                };
                responseOut = dbInterface.AddEditSLType(sLTypeId);
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

        public List<SLTypeViewModel> GetSLTypeList(string sLTypeName = "", string Status = "")
        {
            List<SLTypeViewModel> sLTypeList = new List<SLTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtsLTypes = sqlDbInterface.GetSLTypeList(sLTypeName, Status);
                if (dtsLTypes != null && dtsLTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtsLTypes.Rows)
                    {
                        sLTypeList.Add(new SLTypeViewModel
                        {
                            SLTypeId = Convert.ToInt32(dr["SLTypeId"]),
                            SLTypeName = Convert.ToString(dr["SLTypeName"]),
                            SLType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sLTypeList;
        }

        public SLTypeViewModel GetSLTypeDetail(int sLTypeId = 0)
        {
            SLTypeViewModel sLType = new SLTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtSLTypes = sqlDbInterface.GetSLTypeDetail(sLTypeId);
                if (dtSLTypes != null && dtSLTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSLTypes.Rows)
                    {
                        sLType = new SLTypeViewModel
                        {
                            SLTypeId = Convert.ToInt32(dr["SLTypeId"]),
                            SLTypeName = Convert.ToString(dr["SLTypeName"]),
                            SLType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sLType;
        }
        public List<SLTypeViewModel> GetSLTypeList()
        {
            List<SLTypeViewModel> slTypes = new List<SLTypeViewModel>();
            try
            {
                List<SLType> slTypeList = dbInterface.GetSLTypeList();
                if (slTypeList != null && slTypeList.Count > 0)
                {
                    foreach (SLType slType in slTypeList)
                    {
                        slTypes.Add(new SLTypeViewModel { SLTypeId = slType.SLTypeId, SLTypeName = slType.SLTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return slTypes;
        }

    }
}
