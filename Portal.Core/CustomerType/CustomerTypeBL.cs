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
    public class CustomerTypeBL
    {
        DBInterface dbInterface;
        public CustomerTypeBL()
        {
            dbInterface = new DBInterface();
        }
  
        public List<CustomerTypeViewModel> GetCustomerTypeList()
        {
            List<CustomerTypeViewModel> customerTypes = new List<CustomerTypeViewModel>();
            try
            {
                List<CustomerType> customerTypeList = dbInterface.GetCustomerTypeList();
                if (customerTypeList != null && customerTypeList.Count > 0)
                {
                    foreach (CustomerType customerType in customerTypeList)
                    {
                        customerTypes.Add(new CustomerTypeViewModel { CustomerTypeId = customerType.CustomerTypeId, CustomerTypeDesc = customerType.CustomerTypeDesc,CustomerTypeCode=customerType.CustomerTypeCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerTypes;
        }

        public ResponseOut AddEditCustomerType(CustomerTypeViewModel customertypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                CustomerType customertype = new CustomerType
                {
                   CustomerTypeId = customertypeViewModel.CustomerTypeId,
                    CustomerTypeDesc = customertypeViewModel.CustomerTypeDesc,
                    Status = customertypeViewModel.CustomerType_Status,
                    CompanyBranchId=customertypeViewModel.CompanyBranchId,

                };
                responseOut = dbInterface.AddEditCustomerType(customertype);
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



        public List<CustomerTypeViewModel> GetCustomerTypeList(string customertypeDesc = "", string Status = "",int companyBranchId=0)
        {
            List<CustomerTypeViewModel> customertypes = new List<CustomerTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomerTypes = sqlDbInterface.GetCustomerTypeList(customertypeDesc, Status, companyBranchId);
                if (dtCustomerTypes != null && dtCustomerTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomerTypes.Rows)
                    {
                        customertypes.Add(new CustomerTypeViewModel
                        {
                            CustomerTypeId = Convert.ToInt32(dr["CustomerTypeId"]),
                            CustomerTypeDesc = Convert.ToString(dr["CustomerTypeDesc"]),
                            CustomerType_Status = Convert.ToBoolean(dr["Status"]),
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
            return customertypes;
        }

        public CustomerTypeViewModel GetCustomerTypeDetail(int customertypeId = 0)
        {
            CustomerTypeViewModel customertype = new CustomerTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomerTypes = sqlDbInterface.GetCustomerTypeDetail(customertypeId);
                if (dtCustomerTypes != null && dtCustomerTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomerTypes.Rows)
                    {
                        customertype = new CustomerTypeViewModel
                        {
                            CustomerTypeId = Convert.ToInt32(dr["CustomerTypeId"]),
                            CustomerTypeDesc = Convert.ToString(dr["CustomerTypeDesc"]),
                            CustomerType_Status = Convert.ToBoolean(dr["Status"]),
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
            return customertype;
        }





    }
}
