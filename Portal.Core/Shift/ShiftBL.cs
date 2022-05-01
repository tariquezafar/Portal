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
    public class ShiftBL
    {
        HRMSDBInterface dbInterface;
        public ShiftBL()
        {
            dbInterface = new HRMSDBInterface();
        }


        public List<ShiftViewModel> GetShiftList(int companyId)
        {
            List<ShiftViewModel> shifts = new List<ShiftViewModel>();
            try
            {
                List<HR_Shift> shiftList = dbInterface.GetShiftList(companyId);
                if (shiftList != null && shiftList.Count > 0)
                {
                    foreach (HR_Shift shift in shiftList)
                    {
                        shifts.Add(new ShiftViewModel { ShiftId = shift.ShiftId, ShiftName = shift.ShiftName,ShiftDescription= shift.ShiftDescription });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return shifts;
        }


    }
}
