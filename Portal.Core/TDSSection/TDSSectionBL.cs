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
    public class TDSSectionBL
    {
        DBInterface dbInterface;
        public TDSSectionBL()
        {
            dbInterface = new DBInterface();
        }
     
        public ResponseOut AddEditTDSSection(TDSSetionViewModel tdsSetionViewModel, List<TDSSectionDocumentDetailViewModel> tdsSectionDocumentDetails)
        {
            SQLDbInterface sQLDbInterface = new SQLDbInterface();
            ResponseOut responseOut = new ResponseOut();
            try
            {


                TDSSection tdsSection = new TDSSection
                {
                    
                    SectionId = tdsSetionViewModel.SectionId,
                    SectionName = tdsSetionViewModel.SectionName,
                    SectionDesc = tdsSetionViewModel.SectionDesc,
                    SectionMaxValue=tdsSetionViewModel.SectionMaxValue,
                    Status = tdsSetionViewModel.TDSSetion_Status,
                    CompanyBranchId= tdsSetionViewModel.CompanyBranch

                };

                List<TDSSectionDocumentDetail> tDSSectionDocumentDetailList = new List<TDSSectionDocumentDetail>();
                if (tdsSectionDocumentDetails != null && tdsSectionDocumentDetails.Count > 0)
                {
                    foreach (TDSSectionDocumentDetailViewModel item in tdsSectionDocumentDetails)
                    {
                        tDSSectionDocumentDetailList.Add(new TDSSectionDocumentDetail {
                            SectionId = item.SectionId,
                            SectionDetailID=item.SectionDetailID,
                            DocumentName=item.DocumentName

                        });
 
                    }
                }
                responseOut = sQLDbInterface.AddEditTdsSection(tdsSection, tDSSectionDocumentDetailList);
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

        public List<TDSSetionViewModel> GetTDSSectionList(string sectionName = "", string sectionDesc = "", decimal sectionMAXValue = 0, string Status = "",string companyBranch="")
        {
            List<TDSSetionViewModel> tdsSection = new List<TDSSetionViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtTargetTypes = sqlDbInterface.GetTDSSectionList(sectionName, sectionDesc, sectionMAXValue, Status, companyBranch);
                if (dtTargetTypes != null && dtTargetTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTargetTypes.Rows)
                    {
                        tdsSection.Add(new TDSSetionViewModel
                        {
                            SectionId = Convert.ToInt32(dr["SectionId"]),
                            SectionName = Convert.ToString(dr["SectionName"]),
                            SectionDesc = Convert.ToString(dr["SectionDesc"]),
                            SectionMaxValue = Convert.ToDecimal(dr["SectionMaxValue"]),
                            TDSSetion_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranchName= Convert.ToString(dr["BranchName"])
                         });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return tdsSection;
        }

        public TDSSetionViewModel GetTDSSectionDetail(int tdsSectionId = 0)
        {
            TDSSetionViewModel tdsSection = new TDSSetionViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dttdsSection = sqlDbInterface.GetTDSSectionDetail(tdsSectionId);
                if (dttdsSection != null && dttdsSection.Rows.Count > 0)
                {
                    foreach (DataRow dr in dttdsSection.Rows)
                    {
                        tdsSection = new TDSSetionViewModel
                        {
                            SectionId = Convert.ToInt32(dr["SectionId"]),
                            SectionName = Convert.ToString(dr["SectionName"]),
                            SectionDesc = Convert.ToString(dr["SectionDesc"]),
                            SectionMaxValue = Convert.ToDecimal(dr["SectionMaxValue"]),
                            TDSSetion_Status = Convert.ToBoolean(dr["Status"]),
                            CompanyBranch=Convert.ToInt32(dr["CompanyBranchId"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return tdsSection;
        }

    }
}
