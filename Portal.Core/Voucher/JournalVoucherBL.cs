﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Core.ViewModel;
using Portal.DAL;
using Portal.Common;
using System.Reflection;
using System.Data;
using System.Transactions;

namespace Portal.Core
{
  public class JournalVoucherBL
    {
        DBInterface dbInterface;
        public JournalVoucherBL()
        {
            dbInterface = new DBInterface();
        }


        public List<VoucherDetailViewModel> GetJournalVoucherEntryList(long voucherId)
        {
            List<VoucherDetailViewModel> voucherEntries = new List<VoucherDetailViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVoucherEntry = sqlDbInterface.GetJournalVoucherEntryDetail(voucherId);
                if (dtVoucherEntry != null && dtVoucherEntry.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVoucherEntry.Rows)
                    {

                        voucherEntries.Add(new VoucherDetailViewModel
                        {
                            VoucherDetailId = Convert.ToInt32(dr["VoucherDetailId"]),
                            VoucherId = Convert.ToInt32(dr["VoucherId"]),
                            SequenceNo = Convert.ToInt16(dr["SequenceNo"]),
                            EntryMode = Convert.ToString(dr["EntryMode"]),
                            GLId = Convert.ToInt32(dr["GLId"]),
                            GLCode = Convert.ToString(dr["GLCode"]),
                            GLHead = Convert.ToString(dr["GLHead"]),
                            SLTypeId = Convert.ToInt16(dr["SLTypeId"]),
                            SLTypeName = Convert.ToString(dr["SLTypeName"]),
                            SLId = Convert.ToInt32(dr["SLId"]),
                            SLCode = Convert.ToString(dr["SLCode"]),
                            SLHead = Convert.ToString(dr["SLHead"]),
                            Narration = Convert.ToString(dr["Narration"]),
                            PaymentModeId = Convert.ToInt32(dr["PaymentModeId"]),
                            PaymentModeName = Convert.ToString(dr["PaymentModeName"]),
                            ChequeRefNo = Convert.ToString(dr["ChequeRefNo"]),
                            ChequeRefDate = Convert.ToString(dr["ChequeRefDate"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),
                            CostCenterId = Convert.ToInt32(dr["CostCenterId"]),
                            CostCenterName = Convert.ToString(dr["CostCenterName"]),
                            ValueDate = Convert.ToString(dr["ValueDate"]),
                            DrawnOnBank = Convert.ToString(dr["DrawnOnBank"]),
                            PO_SONo = Convert.ToString(dr["PO_SONo"]),
                            BillNo = Convert.ToString(dr["BillNo"]),
                            BillDate = Convert.ToString(dr["BillDate"]),
                            PayeeId = Convert.ToInt32(dr["PayeeId"]),
                            PayeeName = Convert.ToString(dr["PayeeName"]),
                            AutoEntry = Convert.ToBoolean(dr["AutoEntry"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return voucherEntries;
        }
        public ResponseOut AddEditJournalVoucher(VoucherViewModel voucherViewModel, List<VoucherDetailViewModel> voucherEntryList,List<VoucherSupportingDocumentViewModel> journalvoucherDocuments)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                Voucher voucher = new Voucher
                {
                    VoucherId = voucherViewModel.VoucherId,
                    VoucherDate = Convert.ToDateTime(voucherViewModel.VoucherDate),
                    VoucherMode = voucherViewModel.VoucherMode,
                    VoucherAmount = voucherViewModel.VoucherAmount,
                    PayeeSLTypeId = voucherViewModel.PayeeSLTypeId,
                    BookId = voucherViewModel.BookId,
                    CompanyId = voucherViewModel.CompanyId,
                    FinYearId = voucherViewModel.FinYearId,
                    VoucherStatus = voucherViewModel.VoucherStatus,                   
                    CreatedBy = voucherViewModel.CreatedBy,
                    CompanyBranchId = voucherViewModel.CompanyBranchId,
                };
                List<VoucherDetail> voucherDetail = new List<VoucherDetail>();
                if (voucherEntryList != null && voucherEntryList.Count > 0)
                {
                    foreach (VoucherDetailViewModel item in voucherEntryList)
                    {
                        voucherDetail.Add(new VoucherDetail
                        {
                            SequenceNo = item.SequenceNo,
                            EntryMode = item.EntryMode,
                            GLId = item.GLId,
                            GLCode = item.GLCode,
                            SLId = item.SLId,
                            SLCode = item.SLCode,
                            Narration = item.Narration,
                            PaymentModeId = item.PaymentModeId,
                            ChequeRefNo = "NA",
                            ChequeRefDate = string.IsNullOrEmpty(item.ChequeRefDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime("01-01-1900"),
                            Amount = item.Amount,
                            CostCenterId = item.CostCenterId,
                            SubCostCenterId = 0,
                            ValueDate = string.IsNullOrEmpty(item.ValueDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime("01-01-1900"),
                            DrawnOnBank = "",
                            PO_SONo = item.PO_SONo,
                            BillNo = item.BillNo,
                            BillDate = string.IsNullOrEmpty(item.BillDate) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(item.BillDate),
                            PayeeId = item.PayeeId,
                            PayeeName = item.PayeeName
                        });
                    }
                }

                List<VoucherSupportingDocument> journalvoucherDocumentList = new List<VoucherSupportingDocument>();
                if (journalvoucherDocuments != null && journalvoucherDocuments.Count > 0)
                {
                    foreach (VoucherSupportingDocumentViewModel item in journalvoucherDocuments)
                    {
                        journalvoucherDocumentList.Add(new VoucherSupportingDocument
                        {
                            DocumentTypeId = item.DocumentTypeId,
                            DocumentName = item.DocumentName,
                            DocumentPath = item.DocumentPath
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditJournalVoucher(voucher, voucherDetail, journalvoucherDocumentList);

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

        public List<VoucherViewModel> GetJournalVoucherList(Int32 bookId, string voucherMode, string voucherNo, string voucherStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            List<VoucherViewModel> vouchers = new List<VoucherViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVouchers = sqlDbInterface.GetJournalVoucherList( bookId, voucherMode, voucherNo,  voucherStatus,  fromDate, toDate, companyId, companyBranchId);


                if (dtVouchers != null && dtVouchers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVouchers.Rows)
                    {

                        vouchers.Add(new VoucherViewModel
                        {
                            VoucherId = Convert.ToInt64(dr["VoucherId"]),
                            VoucherNo = Convert.ToString(dr["VoucherNo"]),
                            VoucherDate = Convert.ToString(dr["VoucherDate"]),
                            VoucherMode = Convert.ToString(dr["VoucherMode"]),
                            VoucherAmount = Convert.ToDecimal(dr["VoucherAmount"]),
                            ContraVoucherNo = Convert.ToString(dr["ContraVoucherNo"]),
                            VoucherStatus = Convert.ToString(dr["VoucherStatus"]),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return vouchers;
        }


        public DataTable GenerateJournalVoucherReports(Int32 bookId, string voucherMode, string voucherNo, string voucherStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtVoucher = new DataTable();
            try
            {
                dtVoucher = sqlDbInterface.GenerateJournalVoucherReports(bookId, voucherMode, voucherNo, voucherStatus, fromDate, toDate, companyId, companyBranchId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtVoucher;
        }
        public List<VoucherViewModel> GetApprovedJournalVoucherList(Int32 bookId, string voucherMode, string voucherNo, string voucherStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            List<VoucherViewModel> vouchers = new List<VoucherViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVouchers = sqlDbInterface.GetApprovedJournalVoucherList(bookId, voucherMode, voucherNo, voucherStatus, fromDate, toDate, companyId, companyBranchId);


                if (dtVouchers != null && dtVouchers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVouchers.Rows)
                    {

                        vouchers.Add(new VoucherViewModel
                        {
                            VoucherId = Convert.ToInt64(dr["VoucherId"]),
                            VoucherNo = Convert.ToString(dr["VoucherNo"]),
                            VoucherDate = Convert.ToString(dr["VoucherDate"]),
                            VoucherMode = Convert.ToString(dr["VoucherMode"]),
                            VoucherAmount = Convert.ToDecimal(dr["VoucherAmount"]),
                            ContraVoucherNo = Convert.ToString(dr["ContraVoucherNo"]),
                            VoucherStatus = Convert.ToString(dr["VoucherStatus"]),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return vouchers;
        }

         



        public VoucherViewModel GetJournalVoucherDetail(long voucherId = 0)
        {
            VoucherViewModel voucher = new VoucherViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtVoucher = sqlDbInterface.GetJournalVoucherDetail(voucherId);
                if (dtVoucher != null && dtVoucher.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtVoucher.Rows)
                    {
                        voucher = new VoucherViewModel
                        {
                            VoucherId = Convert.ToInt64(dr["VoucherId"]),

                            CompanyBranchId = Convert.ToInt64(dr["CompanyBranchId"]),
                            CompanyBranchName = Convert.ToString(dr["CompanyBranchName"]),

                            VoucherNo = Convert.ToString(dr["VoucherNo"]),
                            VoucherDate = Convert.ToString(dr["VoucherDate"]),
                            VoucherType = Convert.ToString(dr["VoucherType"]),
                            VoucherMode = Convert.ToString(dr["VoucherMode"]),
                            VoucherAmount = Convert.ToDecimal(dr["VoucherAmount"]),
                            PayeeSLTypeId = Convert.ToInt16(dr["PayeeSLTypeId"]),
                            BookId = Convert.ToInt32(dr["BookId"]),
                            ContraVoucherId = Convert.ToInt64(dr["ContraVoucherId"]),
                            ContraVoucherNo = Convert.ToString(dr["ContraVoucherNo"]),
                            VoucherStatus = Convert.ToString(dr["VoucherStatus"]),
                            ContraBookId = Convert.ToInt32(dr["ContraBookId"]),
                            ContraCompanyId = Convert.ToInt32(dr["ContraCompanyId"]),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            CancelledDate = Convert.ToString(dr["CancelledDate"]),
                            CancelReason = Convert.ToString(dr["CancelReason"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return voucher;
        }

       
        public DataTable GetJournalVoucherDetailDataTable(long voucherId = 0)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtVoucher = new DataTable();
            try
            {
                dtVoucher = sqlDbInterface.GetBankVoucherDetail(voucherId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtVoucher;
        }

        public DataTable GetJournalVoucherEntryListDataTable(long voucherId)
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtVoucherEntryList = new DataTable();
            try
            {
                dtVoucherEntryList = sqlDbInterface.GetJournalVoucherEntryDetail(voucherId);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtVoucherEntryList;
        }

        public ResponseOut CancelApprovedJournalVoucher(VoucherViewModel voucherViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Voucher voucher = new Voucher
                {
                    VoucherId = voucherViewModel.VoucherId,
                    VoucherStatus = voucherViewModel.VoucherStatus,
                    CreatedBy = voucherViewModel.CreatedBy,
                    CancelReason = voucherViewModel.CancelReason
                };
                responseOut = dbInterface.CancelApprovedJournalVoucher(voucher);
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


        public List<VoucherSupportingDocumentViewModel> GetJournalVoucherSupportingDocumentList(long voucherId)
        {
            List<VoucherSupportingDocumentViewModel> voucherDocuments = new List<VoucherSupportingDocumentViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtDocument = sqlDbInterface.GetJournalVoucherSupportingDocumentList(voucherId);
                if (dtDocument != null && dtDocument.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDocument.Rows)
                    {
                        voucherDocuments.Add(new VoucherSupportingDocumentViewModel
                        {
                            VoucherDocId = Convert.ToInt32(dr["VoucherDocId"]),
                            DocumentSequenceNo = Convert.ToInt32(dr["SNo"]),
                            DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]),
                            DocumentTypeDesc = Convert.ToString(dr["DocumentTypeDesc"]),
                            DocumentName = Convert.ToString(dr["DocumentName"]),
                            DocumentPath = Convert.ToString(dr["DocumentPath"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return voucherDocuments;
        }
    }
}