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
    public class UploadUtilityBL
    {
        DBInterface dbInterface;
        public UploadUtilityBL()
        {
            dbInterface = new DBInterface();
        }
        #region Import Lead


        #endregion

        #region Common Method
        public int GetIdByStateName(string stateName)
        {
            int stateId = 0;
            try
            {

                stateId = dbInterface.GetIdByStateName(stateName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return stateId;
        }
        public int GetIdByCountryName(string countryName)
        {
            int countryId = 0;
            try
            {
                countryId = dbInterface.GetIdByCountryName(countryName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return countryId;
        }
        public int GetIdByLeadSourceName(string leadSourceName)
        {
            int leadSourceId = 0;
            try
            {
                leadSourceId = dbInterface.GetIdByLeadSourceName(leadSourceName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadSourceId;
        }
        public int GetIdByLeadStatusName(string leadStatusName)
        {
            int leadStatusId = 0;
            try
            {
                leadStatusId = dbInterface.GetIdByLeadStatusName(leadStatusName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadStatusId;
        }
        public int GetIdByFollowUpActivityName(string followUpActivityTypeName)
        {
            int followUpActivityTypeId = 0;
            try
            {
                followUpActivityTypeId = dbInterface.GetIdByFollowUpActivityTypeName(followUpActivityTypeName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return followUpActivityTypeId;
        }
        public int GetIdByPriorityName(string priorityName)
        {
            int priorityId = 0;
            try
            {
                switch (priorityName.Trim().ToUpper())
                {
                    case "URGENT":
                        priorityId = 1;
                        break;
                    case "HIGH":
                        priorityId = 2;
                        break;
                    case "MEDIUM":
                        priorityId = 3;
                        break;
                    case "LOW":
                        priorityId = 4;
                        break;
                    default:
                        priorityId = 4;
                        break; 
                }

                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return priorityId;
        }
        public int GetIdByGlMainGroupName(string glMainGroupName)
        {
            int glMainGroupId = 0;
            try
            {

                glMainGroupId = dbInterface.GetIdByGLMainGroupName(glMainGroupName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return glMainGroupId;
        }
        public int GetIdByScheduleName(string scheduleName)
        {
            int scheduleID = 0;
            try
            {

                scheduleID = dbInterface.GetIdByScheduleName(scheduleName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return scheduleID;
        }

        public int GetIdBySLTypeName(string sLTypeName)
        {
            int sLTypeId = 0;
            try
            {

                sLTypeId = dbInterface.GetIdBySLTypeName(sLTypeName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sLTypeId;
        }


        public int GetIdByGLSubGroupName(string gLSubGroupName)
        {
            int gLSubGroupId = 0;
            try
            {

                gLSubGroupId = dbInterface.GetIdByGLSubGroupName(gLSubGroupName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return gLSubGroupId;
        }
        public int GetIdByCostCenterName(string costCenterName)
        {
            int costCenterId = 0;
            try
            {

                costCenterId = dbInterface.GetIdByCostCenterName(costCenterName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return costCenterId;
        }
        public int GetIdBySubCostCenterName(string subCostCenterName)
        {
            int subCostCenterId = 0;
            try
            {

                subCostCenterId = dbInterface.GetIdBySubCostCenterName(subCostCenterName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return subCostCenterId;
        }
        public int GetIdByGLHead(string gLHead)
        {
            int postingGLId = 0;
            try
            {

                postingGLId = dbInterface.GetIdByGLHead(gLHead);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return postingGLId;
        }

        public int GetIdByEmployeeName(string employeeName)
        {
            int employeeId = 0;
            try
            {

                employeeId = dbInterface.GetIdByEmployeeName(employeeName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeId;
        }
        public int GetIdByCustomerTypeDesc(string customerTypeDesc)
        {
            int customerTypeId = 0;
            try
            {

                customerTypeId = dbInterface.GetIdByCustomerTypeDesc(customerTypeDesc);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerTypeId;
        }
        public int GetIdByDepartmentName(string departmentName)
        {
            int departmentId = 0;
            try
            {

                departmentId = dbInterface.GetIdByDepartmentName(departmentName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return departmentId;
        }
        public int GetIdByDesignationName(string designationName)
        {
            int designationId = 0;
            try
            {

                designationId = dbInterface.GetIdByDesignationName(designationName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return designationId;
        }

        public int GetIdByProductMainGroupName(string mainGroupName)
        {
            int productMainGroupID = 0;
            try
            {

                productMainGroupID = dbInterface.GetIdByProductMainGroupName(mainGroupName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productMainGroupID;
        }
        public int GetIdByProductSubGroupName(string subGroupName)
        {
            int productSubGroupID = 0;
            try
            {

                productSubGroupID = dbInterface.GetIdByProductSubGroupName(subGroupName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productSubGroupID;
        }
        public int GetIdByProductTypeName(string productTypeName)
        {
            int productTypeId = 0;
            try
            {

                productTypeId = dbInterface.GetIdByProductTypeName(productTypeName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productTypeId;
        }
        public int GetIdByUOMName(string uOMName)
        {
            int UOMId = 0;
            try
            {

                UOMId = dbInterface.GetIdByUOMName(uOMName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return UOMId;
        }

      
        

        public int GetIdByCustomerName(string customerName)
        {
            int customerId = 0;
            try
            {

                customerId = dbInterface.GetIdByCustomerName(customerName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return customerId;
        }

        public string GetAssemblyCodeByAssemblyName(string assemblyName)
        {
            
            string assemblyCode = "";
            try
            {
                switch (assemblyName.Trim().ToUpper())
                {
                    case "MainAssembly":
                        assemblyCode = "MA";
                        break;
                    case "SubAssembly":
                        assemblyCode = "SA";
                        break;
                    default:
                        assemblyCode = "SA";
                        break;
                }


            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return assemblyCode;
        }

        public long GetIdByAssemblyName(string assemblyName)
        {
            long assemblyId = 0;
            
            try
            {
                assemblyId = dbInterface.GetIdByAssemblyName(assemblyName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return assemblyId;
        }
        public long GetIdByProductName(string productName)
        {
            long productId = 0;
            try
            {

                productId = dbInterface.GetIdByProductName(productName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }
        public long GetIdByAllProductName(string productName)
        {
            long productId = 0;
            try
            {

                productId = dbInterface.GetIdByAllProductName(productName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }
        public long GetIdByProductNameForChessis(string productName)
        {
            long productId = 0;
            try
            {

                productId = dbInterface.GetIdByProductNameForChessis(productName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }

       


        public long GetProductIdByProductName(string productName)
        {
            long productId = 0;
            try
            {
                productId = dbInterface.GetProductIdByProductName(productName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }
        public long GetIdByCompanyBranchName(string assemblyName)
        {
            long companyBranchId = 0;

            try
            {
                companyBranchId = dbInterface.GetIdByCompanyBranchName(assemblyName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return companyBranchId;
        }


      


        public long GetIdByProductNameForOpening(string productName)
        {
            long productId = 0;
            try
            {

                productId = dbInterface.GetIdByProductNameForOpening(productName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }

        public int GetIdByGLName(string gLHead)
        {
            int GLId = 0;
            try
            {

                GLId = dbInterface.GetIdByGLName(gLHead);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return GLId;
        }
        public long GetIdBySLHead(string sLHead)
        {
            long sLId = 0;
            try
            {

                sLId = dbInterface.GetIdBySLHead(sLHead);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sLId;
        }

        public int GetIdBySizeDesc(string sizeDesc)
        {
            int sizeId = 0;
            try
            {

                sizeId = dbInterface.GetIdBySizeDesc(sizeDesc);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return sizeId;
        }
        public int GetIdByManufacturerName(string manufacturerName)
        {
            int manfId = 0;
            try
            {

                manfId = dbInterface.GetIdByManufacturerName(manufacturerName);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return manfId;
        }

        public long GetIdByChasisSerialNo(string chasisSerialNo)
        {
            long productId = 0;
            try
            {

                productId = dbInterface.GetIdByChasisSerialNo(chasisSerialNo);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return productId;
        }

        #endregion




    }
}
