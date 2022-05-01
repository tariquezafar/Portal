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
    public class CTCBL
    {
        HRMSDBInterface dbInterface;
        public CTCBL()
        {
            dbInterface = new HRMSDBInterface();
        }
       
        public ResponseOut AddEditCTC(HR_CTCViewModel cTCViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                HR_CTC hR_CTC = new HR_CTC
                {
                    CTCId = cTCViewModel.CTCId,
                    DesignationId = cTCViewModel.DesignationId,
                    Basic = cTCViewModel.Basic,
                    HRAPerc = cTCViewModel.HRAPerc,
                    HRAAmount = cTCViewModel.HRAAmount,
                    Conveyance = cTCViewModel.Conveyance,

                    Medical = cTCViewModel.Medical,
                    ChildEduAllow = cTCViewModel.ChildEduAllow,
                    LTA = cTCViewModel.LTA,
                    SpecialAllow = cTCViewModel.SpecialAllow,


                    OtherAllow = cTCViewModel.OtherAllow,
                    GrossSalary = cTCViewModel.GrossSalary,
                    EmployeePF = cTCViewModel.EmployeePF,
                    EmployeeESI = cTCViewModel.EmployeeESI,

                    ProfessionalTax = cTCViewModel.ProfessionalTax,
                    NetSalary = cTCViewModel.NetSalary,
                    EmployerPF = cTCViewModel.EmployerPF,
                    EmployerESI = cTCViewModel.EmployerESI,
                 
                    MonthlyCTC = cTCViewModel.MonthlyCTC,
                    YearlyCTC = cTCViewModel.YearlyCTC,
                    CreatedBy=cTCViewModel.CreatedBy,
                    CompanyId = cTCViewModel.CompanyId, 
                Status = cTCViewModel.CTC_Status
                };
                responseOut = dbInterface.AddEditCTC(hR_CTC);
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

        public List<HR_CTCViewModel> GetCTCList(string designationId = "", int companyId = 0 ,string ctcStatus = "")
        {
            List<HR_CTCViewModel> ctcViewModel = new List<HR_CTCViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCTCs = sqlDbInterface.GetCTCList(designationId, companyId,ctcStatus);
                if (dtCTCs != null && dtCTCs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCTCs.Rows)
                    {
                        ctcViewModel.Add(new HR_CTCViewModel
                        {
                            CTCId = Convert.ToInt32(dr["CTCId"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]), 
                            Basic = Convert.ToDecimal(dr["Basic"]),
                            HRAPerc = Convert.ToDecimal(dr["HRAPerc"]),
                            HRAAmount = Convert.ToDecimal(dr["HRAAmount"]),
                            Conveyance = Convert.ToDecimal(dr["Conveyance"]),
                            Medical = Convert.ToDecimal(dr["Medical"]),
                            ChildEduAllow = Convert.ToDecimal(dr["ChildEduAllow"]),
                            LTA = Convert.ToDecimal(dr["LTA"]), 
                            SpecialAllow = Convert.ToDecimal(dr["SpecialAllow"]), 
                            OtherAllow = Convert.ToDecimal(dr["OtherAllow"]),
                            ProfessionalTax = Convert.ToDecimal(dr["ProfessionalTax"]),
                            NetSalary = Convert.ToDecimal(dr["NetSalary"]),
                            EmployeePF = Convert.ToDecimal(dr["EmployeePF"]),
                            EmployeeESI = Convert.ToDecimal(dr["EmployeeESI"]),
                            EmployerPF = Convert.ToDecimal(dr["EmployeePF"]),
                            EmployerESI = Convert.ToDecimal(dr["EmployerESI"]),
                            MonthlyCTC = Convert.ToDecimal(dr["MonthlyCTC"]),
                            YearlyCTC = Convert.ToDecimal(dr["YearlyCTC"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            CreatedByName = Convert.ToString(dr["CreatedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ModifiedByName = Convert.ToString(dr["ModifiedByName"]),
                            CTC_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return ctcViewModel;
        }

        public HR_CTCViewModel GetCTCDetail(int ctcId = 0)
        {
            HR_CTCViewModel ctcViewModel = new HR_CTCViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCTCs = sqlDbInterface.GetCTCDetail(ctcId);
                if (dtCTCs != null && dtCTCs.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCTCs.Rows)
                    {
                        ctcViewModel = new HR_CTCViewModel
                        {
                            CTCId = Convert.ToInt32(dr["CTCId"]),
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),
                            Basic = Convert.ToDecimal(dr["Basic"]),
                            HRAPerc = Convert.ToDecimal(dr["HRAPerc"]),
                            HRAAmount = Convert.ToDecimal(dr["HRAAmount"]),
                            Conveyance = Convert.ToDecimal(dr["Conveyance"]),
                            GrossSalary = Convert.ToDecimal(dr["GrossSalary"]), 
                            Medical = Convert.ToDecimal(dr["Medical"]),
                            ChildEduAllow = Convert.ToDecimal(dr["ChildEduAllow"]),
                            LTA = Convert.ToDecimal(dr["LTA"]),
                            SpecialAllow = Convert.ToDecimal(dr["SpecialAllow"]),
                            OtherAllow = Convert.ToDecimal(dr["OtherAllow"]),
                            ProfessionalTax = Convert.ToDecimal(dr["ProfessionalTax"]),
                            NetSalary = Convert.ToDecimal(dr["NetSalary"]),
                            EmployeePF = Convert.ToDecimal(dr["EmployeePF"]),
                            EmployeeESI = Convert.ToDecimal(dr["EmployeeESI"]),
                            EmployerPF = Convert.ToDecimal(dr["EmployeePF"]),
                            EmployerESI = Convert.ToDecimal(dr["EmployerESI"]),
                            MonthlyCTC = Convert.ToDecimal(dr["MonthlyCTC"]),
                            YearlyCTC = Convert.ToDecimal(dr["YearlyCTC"]),
                            CTC_Status = Convert.ToBoolean(dr["Status"])

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return ctcViewModel;
        }

       

    }
}
