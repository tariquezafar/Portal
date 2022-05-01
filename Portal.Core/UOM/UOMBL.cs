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
    public class UOMBL
    {
        DBInterface dbInterface;
        public UOMBL()
        {
            dbInterface = new DBInterface();
        }


        public ResponseOut AddEditUOM(UOMViewModel uomViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                UOM uom = new UOM
                {
                    UOMId = uomViewModel.UOMId,
                    UOMName = uomViewModel.UOMName,
                    UOMDesc = uomViewModel.UOMDesc,  
                    Status = uomViewModel.UOM_Status
                };
                responseOut = dbInterface.AddEditUOM(uom);
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

    public List<UOMViewModel> GetUOMList(string uomName = "", string uomDesc = "", string Status = "")
        {
            List<UOMViewModel> uoms = new List<UOMViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtuoms = sqlDbInterface.GetUOMList(uomName, uomDesc, Status);
                if (dtuoms != null && dtuoms.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtuoms.Rows)
                    {
                        uoms.Add(new UOMViewModel
                        {
                            UOMId = Convert.ToInt32(dr["UOMId"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            UOMDesc = Convert.ToString(dr["UOMDesc"]), 
                            UOM_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return uoms;
        }
        public UOMViewModel GetUOMDetail(int uomId = 0)
        {
            UOMViewModel uom = new UOMViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dUOMs = sqlDbInterface.GetUOMDetail(uomId);
                if (dUOMs != null && dUOMs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dUOMs.Rows)
                    {
                        uom = new UOMViewModel
                        {
                            UOMId = Convert.ToInt32(dr["UOMId"]),
                            UOMName = Convert.ToString(dr["UOMName"]),
                            UOMDesc = Convert.ToString(dr["UOMDesc"]),
                            UOM_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return uom;
        }

         



        public List<UOMViewModel> GetUOMList()
        {
            List<UOMViewModel> uoms= new List<UOMViewModel>();
            try
            {
                List<UOM> uomList = dbInterface.GetUOMList();
                if (uomList != null && uomList.Count > 0)
                {
                    foreach (UOM uom in uomList)
                    {
                        uoms.Add(new UOMViewModel { UOMId = uom.UOMId, UOMName = uom.UOMName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return uoms;
        }


    }
}
