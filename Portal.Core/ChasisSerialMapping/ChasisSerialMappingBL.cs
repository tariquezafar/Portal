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
    public class ChasisSerialMappingBL
    {
        DBInterface dbInterface;
        public ChasisSerialMappingBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditChasisSerialMapping(ChasisSerialMappingViewModel chasisSerialMappingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ChasisSerialMapping chasisSerialMapping = new ChasisSerialMapping {
                    MappingId = chasisSerialMappingViewModel.MappingId,
                    ProductId = chasisSerialMappingViewModel.ProductId,
                    ChasisSerialNo = chasisSerialMappingViewModel.ChasisSerialNo,
                    MotorNo = chasisSerialMappingViewModel.MotorNo,
                    ControllerNo = chasisSerialMappingViewModel.ControllerNo,
                    Color = chasisSerialMappingViewModel.Color,
                    BatteryPower = chasisSerialMappingViewModel.BatteryPower,
                    BatterySerialNo1 = chasisSerialMappingViewModel.BatterySerialNo1,
                    BatterySerialNo2 = chasisSerialMappingViewModel.BatterySerialNo2,
                    BatterySerialNo3 = chasisSerialMappingViewModel.BatterySerialNo3,
                    BatterySerialNo4 = chasisSerialMappingViewModel.BatterySerialNo4,
                    Tier = chasisSerialMappingViewModel.Tier,
                    FrontGlassAvailable = chasisSerialMappingViewModel.FrontGlassAvailable,
                    ViperAvailable = chasisSerialMappingViewModel.ViperAvailable,
                    RearShockerAvailable = chasisSerialMappingViewModel.RearShockerAvailable,
                    ChargerAvailable = chasisSerialMappingViewModel.ChargerAvailable,
                    FM = chasisSerialMappingViewModel.FM,
                    CreatedBy = chasisSerialMappingViewModel.CreatedBy,
                    Status = chasisSerialMappingViewModel.ChasisSerialMapping_Status,
                    CompanyBranchId= chasisSerialMappingViewModel.CompanyBranchId

                };
             responseOut = dbInterface.AddEditChasisSerialMapping(chasisSerialMapping);
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

        public List<ChasisSerialMappingViewModel> GetChasisSerialList(int productId=0, string ChasisSerialNo="", string MotorNo="", string ControllerNo="", int companyBranchId=0,string serialStatus="")
        {
            List<ChasisSerialMappingViewModel> chasisSerialMappings = new List<ChasisSerialMappingViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtChasisSerials = sqlDbInterface.GetChasisSerialList(productId, ChasisSerialNo, MotorNo, ControllerNo, companyBranchId, serialStatus);
                if (dtChasisSerials != null && dtChasisSerials.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtChasisSerials.Rows)
                    {
                        chasisSerialMappings.Add(new ChasisSerialMappingViewModel
                        {
                            MappingId=Convert.ToInt64(dr["MappingId"]),
                            ProductId = Convert.ToInt64(dr["Productid"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]),
                            ControllerNo = Convert.ToString(dr["ControllerNo"]),
                            Color = Convert.ToString(dr["Color"]),
                            BatteryPower = Convert.ToString(dr["BatteryPower"]),
                            BatterySerialNo1 = Convert.ToString(dr["BatterySerialNo1"]),
                            BatterySerialNo2 = Convert.ToString(dr["BatterySerialNo2"]),
                            BatterySerialNo3 = Convert.ToString(dr["BatterySerialNo3"]),
                            BatterySerialNo4 = Convert.ToString(dr["BatterySerialNo4"]),
                            Tier = Convert.ToString(dr["Tier"]),
                            FrontGlassAvailable = Convert.ToBoolean(dr["FrontGlassAvailable"]),
                            ViperAvailable = Convert.ToBoolean(dr["ViperAvailable"]),
                            RearShockerAvailable = Convert.ToBoolean(dr["RearShockerAvailable"]),
                            ChargerAvailable = Convert.ToBoolean(dr["ChargerAvailable"]),
                            FM = Convert.ToBoolean(dr["FM"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            ChasisSerialMapping_Status= Convert.ToBoolean(dr["Status"]),
                            ComapnyBranchName= Convert.ToString(dr["ComapnyBranchName"]),
                            ProductSerialStatus= Convert.ToString(dr["SerialStatus"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return chasisSerialMappings;
        }
      
        public ChasisSerialMappingViewModel GetChasisSerialMappingDetail(long mappingId = 0)
        {
            ChasisSerialMappingViewModel chasisSerial = new ChasisSerialMappingViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtProducts = sqlDbInterface.GetChasisSerialDetail(mappingId);
                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        chasisSerial= new ChasisSerialMappingViewModel
                        {
                           MappingId=Convert.ToInt64(dr["MappingId"]),
                            ProductId = Convert.ToInt64(dr["Productid"]),
                            ProductName = Convert.ToString(dr["ProductName"]),
                            ChasisSerialNo = Convert.ToString(dr["ChasisSerialNo"]),
                            MotorNo = Convert.ToString(dr["MotorNo"]),
                            ControllerNo = Convert.ToString(dr["ControllerNo"]),
                            Color = Convert.ToString(dr["Color"]),
                            BatteryPower = Convert.ToString(dr["BatteryPower"]),
                            BatterySerialNo1 = Convert.ToString(dr["BatterySerialNo1"]),
                            BatterySerialNo2 = Convert.ToString(dr["BatterySerialNo2"]),
                            BatterySerialNo3 = Convert.ToString(dr["BatterySerialNo3"]),
                            BatterySerialNo4 = Convert.ToString(dr["BatterySerialNo4"]),
                            Tier = Convert.ToString(dr["Tier"]),
                            FrontGlassAvailable = Convert.ToBoolean(dr["FrontGlassAvailable"]),
                            ViperAvailable = Convert.ToBoolean(dr["ViperAvailable"]),
                            RearShockerAvailable = Convert.ToBoolean(dr["RearShockerAvailable"]),
                            ChargerAvailable = Convert.ToBoolean(dr["ChargerAvailable"]),
                            FM=Convert.ToBoolean(dr["FM"]),
                            ChasisSerialMapping_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchId=Convert.ToInt32(dr["CompanyBranchId"]),
                            ComapnyBranchName=Convert.ToString(dr["ComapnyBranchName"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return chasisSerial;
        }

        
        public ResponseOut ImportChessisSerialMapping(ChasisSerialMappingViewModel chasisSerialMappingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ChasisSerialMapping chasisSerialMapping = new ChasisSerialMapping
                {
                    MappingId = chasisSerialMappingViewModel.MappingId,
                    ProductId = chasisSerialMappingViewModel.ProductId,
                    ChasisSerialNo = chasisSerialMappingViewModel.ChasisSerialNo,
                    MotorNo = chasisSerialMappingViewModel.MotorNo,
                    ControllerNo = chasisSerialMappingViewModel.ControllerNo,
                    Color = chasisSerialMappingViewModel.Color,
                    BatteryPower = chasisSerialMappingViewModel.BatteryPower,
                    BatterySerialNo1 = chasisSerialMappingViewModel.BatterySerialNo1,
                    BatterySerialNo2 = chasisSerialMappingViewModel.BatterySerialNo2,
                    BatterySerialNo3 = chasisSerialMappingViewModel.BatterySerialNo3,
                    BatterySerialNo4 = chasisSerialMappingViewModel.BatterySerialNo4,
                    Tier = chasisSerialMappingViewModel.Tier,
                    FrontGlassAvailable = chasisSerialMappingViewModel.FrontGlassAvailable,
                    ViperAvailable = chasisSerialMappingViewModel.ViperAvailable,
                    RearShockerAvailable = chasisSerialMappingViewModel.RearShockerAvailable,
                    ChargerAvailable = chasisSerialMappingViewModel.ChargerAvailable,
                    CreatedBy = chasisSerialMappingViewModel.CreatedBy,
                    Status = chasisSerialMappingViewModel.ChasisSerialMapping_Status

                };
                responseOut = dbInterface.AddEditChasisSerialMapping(chasisSerialMapping);
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

        public ResponseOut ImportChessisSerialMappingForUploadUtility(ChasisSerialMappingViewModel chasisSerialMappingViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ChasisSerialMapping chasisSerialMapping = new ChasisSerialMapping
                {
                    MappingId = chasisSerialMappingViewModel.MappingId,
                    ProductId = chasisSerialMappingViewModel.ProductId,
                    ChasisSerialNo = chasisSerialMappingViewModel.ChasisSerialNo,
                    MotorNo = chasisSerialMappingViewModel.MotorNo,
                    ControllerNo = chasisSerialMappingViewModel.ControllerNo,
                    Color = chasisSerialMappingViewModel.Color,
                    BatteryPower = chasisSerialMappingViewModel.BatteryPower,
                    BatterySerialNo1 = chasisSerialMappingViewModel.BatterySerialNo1,
                    BatterySerialNo2 = chasisSerialMappingViewModel.BatterySerialNo2,
                    BatterySerialNo3 = chasisSerialMappingViewModel.BatterySerialNo3,
                    BatterySerialNo4 = chasisSerialMappingViewModel.BatterySerialNo4,
                    Tier = chasisSerialMappingViewModel.Tier,
                    FrontGlassAvailable = chasisSerialMappingViewModel.FrontGlassAvailable,
                    ViperAvailable = chasisSerialMappingViewModel.ViperAvailable,
                    RearShockerAvailable = chasisSerialMappingViewModel.RearShockerAvailable,
                    ChargerAvailable = chasisSerialMappingViewModel.ChargerAvailable,
                    CreatedBy = chasisSerialMappingViewModel.CreatedBy,
                    Status = chasisSerialMappingViewModel.ChasisSerialMapping_Status,
                    CompanyBranchId=chasisSerialMappingViewModel.CompanyBranchId

                };
                responseOut = dbInterface.AddEditChasisSerialMappingForUploadUtility(chasisSerialMapping);
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
    }
}
