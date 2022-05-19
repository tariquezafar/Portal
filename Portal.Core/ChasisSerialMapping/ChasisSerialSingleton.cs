using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Portal.Core
{
   public class ChasisSerialSingleton
    {
        ChasisSerialPlanBL chasisSerialPlanBL = new ChasisSerialPlanBL();
        ProductBL productBL = new ProductBL();
        private static ChasisSerialSingleton instance;
        public static List<ChasisSerialMappingViewModel> chasisSerialList = null;

        private static readonly object syncRoot = new object();
        public ChasisSerialSingleton()
        {

            if (chasisSerialList == null)
            {
                chasisSerialList = chasisSerialPlanBL.GetChasisSerialNoList();

            }
        }
        public static ChasisSerialSingleton Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new ChasisSerialSingleton();
                    }
                }
            }
            return instance;
        }
        public static void ResetChasisSerialSingleton()
        {
            lock (syncRoot)
            {
                instance = null;
                chasisSerialList = null;
            }
        }
        public static List<ChasisSerialMappingViewModel> GetChasisSerials(string term,int companyBranchId)
        {
            List<ChasisSerialMappingViewModel> chasisSerials = new List<ChasisSerialMappingViewModel>();
            try
            {
                if (chasisSerialList != null && chasisSerialList.Count > 0)
                {
                    foreach (ChasisSerialMappingViewModel chasis in chasisSerialList)
                    {
                        if ((chasis.ChasisSerialNo.Trim().ToLower().Contains(term.ToLower()) && chasis.CompanyBranchId == companyBranchId))
                        {
                            chasisSerials.Add(
                                new ChasisSerialMappingViewModel
                                {
                                    ProductId = Convert.ToInt64(chasis.ProductId),
                                    ChasisSerialNo = chasis.ChasisSerialNo,
                                    MotorNo = chasis.MotorNo,
                                    ControllerNo = chasis.ControllerNo,
                                    BatterySerialNo1=chasis.BatterySerialNo1,
                                    BatterySerialNo2=chasis.BatterySerialNo2
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                //throw ex;
            }
            return chasisSerials;
        }


    }
}
