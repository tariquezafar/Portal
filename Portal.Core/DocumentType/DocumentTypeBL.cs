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
    public class DocumentTypeBL
    {
        DBInterface dbInterface;
        public DocumentTypeBL()
        {
            dbInterface = new DBInterface();
        }

        public List<DocumentTypeViewModel> GetDocumentTypeList(int companyId)
        {
            List<DocumentTypeViewModel> documentTypes = new List<DocumentTypeViewModel>();
            try
            {
                List<DocumentType> documentTypeList = dbInterface.GetDocumentTypeList(companyId);
                if (documentTypeList != null && documentTypeList.Count > 0)
                {
                    foreach (DocumentType documentType in documentTypeList)
                    {
                        documentTypes.Add(new DocumentTypeViewModel { DocumentTypeId = documentType.DocumentTypeId, DocumentTypeDesc = documentType.DocumentTypeDesc });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return documentTypes;
        }

        public ResponseOut AddEditDocumentType(DocumentTypeViewModel documenttypeViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                DocumentType documenttype = new DocumentType
                {
                    DocumentTypeId = documenttypeViewModel.DocumentTypeId,
                    DocumentTypeDesc = documenttypeViewModel.DocumentTypeDesc,
                    CompanyId = documenttypeViewModel.CompanyId,
                    Status = documenttypeViewModel.DocumentType_Status,
                    CompanyBranchId=documenttypeViewModel.CompanyBranchId

                };
                responseOut = dbInterface.AddEditDocumentType(documenttype);
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


        public List<DocumentTypeViewModel> GetDocumentTypeList(string documenttypeDesc = "", int companyId = 0, string Status = "",int companyBranchId=0)
        {
            List<DocumentTypeViewModel> documenttypes = new List<DocumentTypeViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocumentTypes = sqlDbInterface.GetDocumentTypeList(documenttypeDesc, companyId, Status, companyBranchId);
                if (dtDocumentTypes != null && dtDocumentTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocumentTypes.Rows)
                    {
                        documenttypes.Add(new DocumentTypeViewModel
                        {
                            DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]),
                            CompanyId = Convert.ToInt32(dr["CompanyId"]),
                            DocumentTypeDesc = Convert.ToString(dr["DocumentTypeDesc"]),
                            DocumentType_Status = Convert.ToBoolean(dr["Status"]),
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
            return documenttypes;
        }

        public DocumentTypeViewModel GetDocumentTypeDetail(int documenttypeId = 0)
        {
            DocumentTypeViewModel documenttype = new DocumentTypeViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocumentTypes = sqlDbInterface.GetDocumentTypeDetail(documenttypeId);
                if (dtDocumentTypes != null && dtDocumentTypes.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocumentTypes.Rows)
                    {
                        documenttype = new DocumentTypeViewModel
                        {
                            DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]),
                            DocumentTypeDesc = Convert.ToString(dr["DocumentTypeDesc"]),
                            DocumentType_Status = Convert.ToBoolean(dr["Status"]),
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
            return documenttype;
        }





    }
}
