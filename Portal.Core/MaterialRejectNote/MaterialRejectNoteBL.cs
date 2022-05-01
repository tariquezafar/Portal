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
   public class MaterialRejectNoteBL
    {
        DBInterface dbInterface;
        public MaterialRejectNoteBL()
        {
            dbInterface = new DBInterface();
        }


        public ResponseOut AddEditMaterialRejectNote(MaterialRejectNoteViewModel materialRejectNoteViewModel, List<MaterialRejectNoteProductDetailViewModel> mrnProducts)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                MaterialRejectNote materialRejectNote = new MaterialRejectNote
                {
                    MaterialReceiveNoteId= materialRejectNoteViewModel.MaterialReceiveNoteId,
                    MaterialReceiveNoteNo=materialRejectNoteViewModel.MaterialReceiveNoteNo,
                    
                    MaterialReceiveNoteDate = string.IsNullOrEmpty(materialRejectNoteViewModel.MaterialReceiveNoteDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(materialRejectNoteViewModel.MaterialReceiveNoteDate),
                    QualityCheckNo = materialRejectNoteViewModel.QualityCheckNo,
                    QualityCheckId = materialRejectNoteViewModel.QualityCheckId,
                    GateInId = materialRejectNoteViewModel.GateInId,
                    GateInNo = materialRejectNoteViewModel.GateInNo,
                    POID = materialRejectNoteViewModel.POID,
                    PONO= materialRejectNoteViewModel.PONo,
                    VendorId= materialRejectNoteViewModel.VendorId,
                    VendorName= materialRejectNoteViewModel.VendorName,
                    Remarks = materialRejectNoteViewModel.Remarks,
                    RejectRemarks = materialRejectNoteViewModel.RejectRemarks,
                    CompanyBranchId = materialRejectNoteViewModel.CompanyBranchId,
                    FinYearId = materialRejectNoteViewModel.FinYearId,
                    CompanyId = materialRejectNoteViewModel.CompanyId,
                    CreatedBy = materialRejectNoteViewModel.CreatedBy,
                    ApprovalStatus = materialRejectNoteViewModel.ApprovalStatus


                };
                List<MaterialRejectNoteProductDetail> mrnProductsList = new List<MaterialRejectNoteProductDetail>();
                if (mrnProducts != null && mrnProducts.Count > 0)
                {
                    foreach (MaterialRejectNoteProductDetailViewModel item in mrnProducts)
                    {
                        mrnProductsList.Add(new MaterialRejectNoteProductDetail
                        {
                            MaterialReceiveNoteId= materialRejectNoteViewModel.MaterialReceiveNoteId,
                            Price= item.Price,
                            Quantity= item.ReceivedQuantity,
                            ProductId = item.ProductId,
                            ReceivedQuantity = item.ReceivedQuantity,
                            AcceptQuantity = item.AcceptQuantity,
                            RejectQuantity = item.RejectQuantity,
                            RejectMarks = item.RejectMarks,

                        });
                    }
                }

               

                responseOut = sqlDbInterface.AddEditMaterialRejectNote(materialRejectNote, mrnProductsList);

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



        public List<MaterialRejectNoteViewModel> GetMaterialRejectNoteList(string materialReceiveNoteNo, string qualityCheckNo, string gateInNo, string poNo, string fromDate, string toDate, int companyId, string approvalStatus, string companyBranch)
        {
            List<MaterialRejectNoteViewModel> MaterialRejectNote = new List<MaterialRejectNoteViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtMaterialRejectNote = sqlDbInterface.GetMaterialRejectNoteList(materialReceiveNoteNo,qualityCheckNo,gateInNo, poNo, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), approvalStatus, companyId, companyBranch);
                if (dtMaterialRejectNote != null && dtMaterialRejectNote.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMaterialRejectNote.Rows)
                    {
                        MaterialRejectNote.Add(new MaterialRejectNoteViewModel
                        {

                            MaterialReceiveNoteId = Convert.ToInt32(dr["MaterialReceiveNoteId"]),
                            MaterialReceiveNoteNo = Convert.ToString(dr["MaterialReceiveNoteNo"]),
                            MaterialReceiveNoteDate = Convert.ToString(dr["MaterialReceiveNoteDate"]),
                            QualityCheckNo = Convert.ToString(dr["QualityCheckNo"]),
                            QualityCheckId = Convert.ToInt64(dr["QualityCheckId"]),
                            QualityCheckDate= Convert.ToString(dr["QualityCheckDate"]),


                            GateInId = Convert.ToInt32(dr["GateInId"]),
                            GateInNo = Convert.ToString(dr["GateInNo"]),
                            GateInDate = Convert.ToString(dr["GateInDate"]),
                            POID = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"])
                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return MaterialRejectNote;
        }

        public MaterialRejectNoteViewModel GetMaterialRejectNoteDetail(long materialReceiveNoteId = 0)
        {
            MaterialRejectNoteViewModel qc = new MaterialRejectNoteViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtmaterialRejectNote = sqlDbInterface.GetMaterialRejectNoteDetail(materialReceiveNoteId);
                if (dtmaterialRejectNote != null && dtmaterialRejectNote.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtmaterialRejectNote.Rows)
                    {
                        qc = new MaterialRejectNoteViewModel
                        {

                            MaterialReceiveNoteId = Convert.ToInt64(dr["MaterialReceiveNoteId"]),
                            MaterialReceiveNoteNo = Convert.ToString(dr["MaterialReceiveNoteNo"]),
                            MaterialReceiveNoteDate= Convert.ToString(dr["MaterialReceiveNoteDate"]),
                            QualityCheckId = Convert.ToInt32(dr["QualityCheckId"]),
                            QualityCheckNo = Convert.ToString(dr["QualityCheckNo"]),
                            QualityCheckDate = Convert.ToString(dr["QualityCheckDate"]),
                            GateInId = Convert.ToInt32(dr["GateInId"]),
                            GateInNo = Convert.ToString(dr["GateInNo"]),
                            GateInDate = Convert.ToString(dr["GateInDate"]),
                            POID = Convert.ToInt32(dr["POId"]),
                            PONo = Convert.ToString(dr["PONo"]),
                            PoDate = Convert.ToString(dr["PODate"]),
                            VendorId = Convert.ToInt32(dr["VendorId"]),
                            VendorName = Convert.ToString(dr["VendorName"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            RejectRemarks = Convert.ToString(dr["RejectRemarks"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return qc;
        }


        public DataTable GetMaterialRejectNoteDetailReport(long materialReceiveNoteId = 0)
        {
            DataTable dtMaterialRejectNoteDetail;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtMaterialRejectNoteDetail = sqlDbInterface.GetMaterialRejectNoteDetail(materialReceiveNoteId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }


            
                return dtMaterialRejectNoteDetail;
        }


        public DataTable GetMaterialRejectNoteProductDetailReport(long materialReceiveNoteId = 0)
        {
            DataTable dtMaterialRejectNoteProductDetail;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                dtMaterialRejectNoteProductDetail = sqlDbInterface.GetQualityCheckRejectProductList(materialReceiveNoteId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

            return dtMaterialRejectNoteProductDetail;
        }


        
    }
    }

