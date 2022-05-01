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
    public class HRShiftBL
    {
        HRMSDBInterface dbInterface;
        public HRShiftBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        #region  HRShift

        public ResponseOut AddEditShift(ShiftViewModel shiftViewModel)
        {

            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDBInterface = new HRSQLDBInterface();              
            try
            {
                HR_Shift shift = new HR_Shift
                {
                    ShiftId = shiftViewModel.ShiftId,
                    ShiftName = shiftViewModel.ShiftName, 
                    ShiftDescription = shiftViewModel.ShiftDescription,
                    CompanyId = shiftViewModel.CompanyId,
                    ShiftTypeId = shiftViewModel.ShiftTypeId,
                    NormalPaidHours = shiftViewModel.NormalPaidHours,
                    Allowance = shiftViewModel.Allowance,
                    ShiftStartTime = TimeSpan.Parse(shiftViewModel.ShiftStartTime),
                    ShiftEndTime = TimeSpan.Parse(shiftViewModel.ShiftEndTime),
                    LateArrivalLimit = TimeSpan.Parse(shiftViewModel.LateArrivalLimit),
                    EarlyGoLimit = TimeSpan.Parse(shiftViewModel.EarlyGoLimit),
                    OvertimeStartTime = TimeSpan.Parse(shiftViewModel.OvertimeStartTime),
                    ValidTill = Convert.ToDateTime(shiftViewModel.ValidTill),
                    FullDayWorkHour = shiftViewModel.FullDayWorkHour,
                    HalfDayWorkHour = shiftViewModel.HalfDayWorkHour,
                    CreatedBy = shiftViewModel.CreatedBy,
                    Status = shiftViewModel.Shift_Status,
                    CompanyBranchId=shiftViewModel.CompanyBranchId

                };
            responseOut = sqlDBInterface.AddEditShift(shift);

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
        public List<ShiftViewModel> GetShiftList(string shiftName ="",int shiftTypeId = 0, string Status = "",int companyId=0,int companyBranchId=0)
        {
            List<ShiftViewModel> shiftViewModel = new List<ShiftViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetShiftList(shiftName,shiftTypeId, Status, companyId, companyBranchId);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        shiftViewModel.Add(new ShiftViewModel
                        { 
                            ShiftId = Convert.ToInt32(dr["ShiftId"]),
                            ShiftName = Convert.ToString(dr["ShiftName"]),
                            ShiftTypeName = Convert.ToString(dr["ShiftTypeName"]),
                            ShiftTypeId = Convert.ToInt32(dr["ShiftTypeId"]),
                            ShiftDescription = Convert.ToString(dr["ShiftDescription"]),
                            NormalPaidHours = Convert.ToInt32(dr["NormalPaidHours"]),
                            ShiftStartTime = Convert.ToString(dr["ShiftStartTime"]),
                            ShiftEndTime = Convert.ToString(dr["ShiftEndTime"]),
                            LateArrivalLimit = Convert.ToString(dr["LateArrivalLimit"]),
                            EarlyGoLimit = Convert.ToString(dr["EarlyGoLimit"]),
                            OvertimeStartTime = Convert.ToString(dr["OvertimeStartTime"]),
                            ValidTill = Convert.ToString(dr["ValidTill"]),
                            FullDayWorkHour = Convert.ToDecimal(dr["FullDayWorkHour"]),
                            HalfDayWorkHour = Convert.ToDecimal(dr["HalfDayWorkHour"]),
                            Allowance = Convert.ToInt32(dr["Allowance"]),
                            Shift_Status = Convert.ToBoolean(dr["Status"]),
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
            return shiftViewModel;
        }
        public ShiftViewModel GetShiftDetail(int shiftId = 0)
        {
            ShiftViewModel shiftViewModel = new ShiftViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dteducation = sqlDbInterface.GetShiftDetail(shiftId);
                if (dteducation != null && dteducation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dteducation.Rows)
                    {
                        shiftViewModel = new ShiftViewModel
                        {
                            ShiftId = Convert.ToInt32(dr["ShiftId"]),
                            ShiftName = Convert.ToString(dr["ShiftName"]),
                            ShiftTypeId = Convert.ToInt32(dr["ShiftTypeId"]),
                            ShiftDescription = Convert.ToString(dr["ShiftDescription"]),
                            NormalPaidHours = Convert.ToInt32(dr["NormalPaidHours"]),
                            ShiftStartTime = Convert.ToString(dr["ShiftStartTime"]),
                            ShiftEndTime = Convert.ToString(dr["ShiftEndTime"]),
                            LateArrivalLimit = Convert.ToString(dr["LateArrivalLimit"]),
                            EarlyGoLimit = Convert.ToString(dr["EarlyGoLimit"]),
                            OvertimeStartTime = Convert.ToString(dr["OvertimeStartTime"]),
                            ValidTill = Convert.ToString(dr["ValidTill"]),
                            Allowance = Convert.ToInt32(dr["Allowance"]),
                            FullDayWorkHour = Convert.ToInt32(dr["FullDayWorkHour"]),
                            HalfDayWorkHour = Convert.ToInt32(dr["HalfDayWorkHour"]),
                            Shift_Status = Convert.ToBoolean(dr["Status"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchId= Convert.ToInt32(dr["CompanyBranchId"]),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return shiftViewModel;
        }
        #endregion
    }
}
