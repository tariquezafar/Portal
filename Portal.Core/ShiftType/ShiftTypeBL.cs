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
    public class ShiftTypeBL
    {
        HRMSDBInterface dbInterface;
        public ShiftTypeBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditShiftType(HR_ShiftTypeViewModel shiftTypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_ShiftType hR_ShiftType = new HR_ShiftType
                {
                    ShiftTypeId = shiftTypeViewModel.ShiftTypeId,
                    ShiftTypeName = shiftTypeViewModel.ShiftTypeName,
                    Status = shiftTypeViewModel.ShiftType_Status
                };
                responseOut = dbInterface.AddEditShfitType(hR_ShiftType);
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

        public List<HR_ShiftTypeViewModel> GetShiftTypeList(string shiftTypeName = "", string Status = "")
        {
            List<HR_ShiftTypeViewModel> shiftTypeViewModel = new List<HR_ShiftTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetShiftTypeList(shiftTypeName, Status);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        shiftTypeViewModel.Add(new HR_ShiftTypeViewModel
                        {
                            ShiftTypeId = Convert.ToInt32(dr["ShiftTypeId"]),
                            ShiftTypeName = Convert.ToString(dr["ShiftTypeName"]),
                            ShiftType_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return shiftTypeViewModel;
        }

        public HR_ShiftTypeViewModel GetShiftTypeDetail(int shiftTypeId = 0)
        {
            HR_ShiftTypeViewModel shiftTypeViewModel = new HR_ShiftTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetShiftTypeDetail(shiftTypeId);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        shiftTypeViewModel = new HR_ShiftTypeViewModel
                        {
                            ShiftTypeId = Convert.ToInt32(dr["ShiftTypeId"]),
                            ShiftTypeName = Convert.ToString(dr["ShiftTypeName"]),
                            ShiftType_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return shiftTypeViewModel;
        }

        public List<HR_ShiftTypeViewModel> GetHRShiftTypeList()
        {
            List<HR_ShiftTypeViewModel> shiftTypeViewModel = new List<HR_ShiftTypeViewModel>();
            try
            {
                List<HR_ShiftType> shiftTypeList = dbInterface.GetHRShiftTypeList();
                if (shiftTypeList != null && shiftTypeList.Count > 0)
                {
                    foreach (HR_ShiftType advance in shiftTypeList)
                    {
                        shiftTypeViewModel.Add(new HR_ShiftTypeViewModel { ShiftTypeId = advance.ShiftTypeId, ShiftTypeName = advance.ShiftTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return shiftTypeViewModel;
        }



    }
}
