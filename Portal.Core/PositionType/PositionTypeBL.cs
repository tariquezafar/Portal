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
using Portal.DAL.Infrastructure;
namespace Portal.Core
{
    public class PositionTypeBL
    {
        HRMSDBInterface dbInterface;
        public PositionTypeBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditPositionType(PositionTypeViewModel positiontypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_PositionType positiontype = new HR_PositionType
                {
                    PositionTypeId = positiontypeViewModel.PositionTypeId,
                    PositionTypeName = positiontypeViewModel.PositionTypeName,
                    PositionTypeCode = positiontypeViewModel.PositionTypeCode,
                    Status = positiontypeViewModel.PositionType_Status
                };
                responseOut = dbInterface.AddEditPositionType(positiontype);
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

        public List<PositionTypeViewModel> GetPositionTypeList(string positiontypeName = "", string positiontypeCode = "", string Status = "")
        {
            List<PositionTypeViewModel> positiontypes = new List<PositionTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPositionTypes = sqlDbInterface.GetPositionTypeList(positiontypeName, positiontypeCode, Status);
                if (dtPositionTypes != null && dtPositionTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPositionTypes.Rows)
                    {
                        positiontypes.Add(new PositionTypeViewModel
                        {
                            PositionTypeId = Convert.ToInt32(dr["PositionTypeId"]),
                            PositionTypeName = Convert.ToString(dr["PositionTypeName"]),
                            PositionTypeCode = Convert.ToString(dr["PositionTypeCode"]),
                            PositionType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return positiontypes;
        }

        public PositionTypeViewModel GetPositionTypeDetail(int positiontypeId = 0)
        {
            PositionTypeViewModel positiontype = new PositionTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtPositionTypes = sqlDbInterface.GetPositionTypeDetail(positiontypeId);
                if (dtPositionTypes != null && dtPositionTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtPositionTypes.Rows)
                    {
                        positiontype = new PositionTypeViewModel
                        {
                            PositionTypeId = Convert.ToInt32(dr["PositionTypeId"]),
                            PositionTypeName = Convert.ToString(dr["PositionTypeName"]),
                            PositionTypeCode = Convert.ToString(dr["PositionTypeCode"]),
                            PositionType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return positiontype;
        }

      

    }
}
