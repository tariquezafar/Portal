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
    public class ProductSerialBL
    {
        DBInterface dbInterface;
        public ProductSerialBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut ImportProductSerial(ProductSerialViewModel productSerialViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                ProductSerialDetail productSerialDetail = new ProductSerialDetail
                {
                    ProductSerialId = productSerialViewModel.ProductSerialId,
                    ProductId = productSerialViewModel.ProductId,
                    ProductSerialNo = productSerialViewModel.ProductSerialNo,
                    Serial1 = productSerialViewModel.Serial1,
                    Serial2 = productSerialViewModel.Serial2,
                    Serial3 = productSerialViewModel.Serial3,
                    ProductSerialStatus = productSerialViewModel.ProductSerialStatus,
                };
                responseOut = dbInterface.AddEditProductSerial(productSerialDetail);
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
