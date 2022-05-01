using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Common
{
    public class UserInterfaceHelper
    {
        public const int Add_Edit_Company_CP = 1;
        public const int Add_Edit_User_CP = 2;
        public const int Add_Edit_User_ADMIN = 3;
        public const int Add_Edit_Country_CP = 4;
        public const int Add_Edit_FinYear_CP = 5;
        public const int Add_Edit_State_CP = 6;
        public const int Add_Edit_Role_CP = 7;
        public const int Add_Edit_LeadStatus_CP = 8;
        public const int Add_Edit_LeadSource_CP = 10;
        public const int Add_Edit_ProductType_ADMIN = 12;
        public const int Add_Edit_Product = 13;

        public const int Add_Edit_BalanceTransfer = 12263;
        public const int Add_Edit_BalanceTransferGL = 12265;
        public const int Add_Edit_ProductReorder = 11125;
        public const int Add_Edit_BalanceTransferSL = 12266;
        public const int Add_Edit_ProductReport = 12225;

        public const int Add_Edit_PaymentTerm_ACCOUNT = 14;
        public const int Add_Edit_Department_HR = 15;
        public const int Add_Edit_ProductMainGroup_INVENTORY = 16;
        public const int Add_Edit_ProductSubGroup_INVENTORY = 18;
        public const int Add_Edit_UOM_CP = 20;
        public const int Add_Edit_Designation_HR = 21;
        public const int Add_Edit_Lead_CRM = 23;
        public const int Add_Edit_Employee_State_Mapping_CRM = 29;
        public const int Add_Edit_City_CP =27;
        public const int Add_Edit_Tax_ACCOUNT = 30; 
        public const int Add_Edit_Book_ACCOUNT = 31;
        public const int Add_Edit_CustomerType_CP = 34;
        public const int Add_Edit_Vendor_SALE = 35;
        public const int Add_Edit_GLMainGroup_ACCOUNT = 36;
        public const int Add_Edit_GLSubGroup_ACCOUNT = 37;
        public const int Add_Edit_QuotationSetting_SALE = 41;
        public const int Add_Edit_TermTemplate_Admin = 42;
        public const int Add_Edit_DocumentType_Admin = 44;
        public const int Add_Edit_SO = 46;
        public const int Add_Edit_SaleInvoice = 48;
        public const int Add_Edit_CustomerPayment= 53;
        public const int Add_Edit_VendorPayment = 54;
        public const int Add_Edit_SOSetting_SALE = 51;
        public const int Add_Edit_DeliveryChallan = 1054;
        public const int Add_RevisedQuotation = 1057;
        public const int Add_RevisedPO = 1061;
        public const int Add_RevisedIdPO = 43;
        public const int Add_RevisedIdQuotation = 39;

        public const int Add_QuotationRegister = 1062;
        public const int Add_PORegister = 1065;
        public const int Add_SORegister = 1066;
        public const int Add_SaleReturnRegister = 12197;
        public const int Add_SaleInvoiceRegister = 2062;
        public const int Add_MRNRegister = 2065;
        public const int Add_STNRegister = 2067;
        public const int Add_STRRegister = 2068;
        public const int Add_SINRegister = 12195;
        public const int AddVendor_Payment_Register = 2070;
        public const int Add_SaleInvoiceSummary = 2071;
        public const int Add_PurchaseSummary = 2076;
        public const int Add_Edit_DebitNoteVoucher = 3080;
        public const int Add_Edit_CreditNoteVoucher = 3081;
        public const int Add_Edit_ApprovedBankVoucher = 7096;
        public const int Add_Edit_ApprovedCashVoucher = 7097;
        public const int Add_Edit_ApprovedJournalVoucher = 7098;
        public const int Add_Edit_ApprovedDebitNoteVoucher = 7099;
        public const int Add_Edit_ApprovedCreditNoteVoucher = 7100;
        public const int Add_Edit_ResourceRequisition = 10113;
        public const int Add_Edit_EmployeeAdvanceApplication = 11140;
        public const int ApproveEmployeeAdvanceApplication = 11141;
        public const int Add_Edit_EmployeeTravelApplication = 12126;
        public const int ApproveEmployeeTravelApplication = 12127;
        public const int Add_Edit_SeparationClearList = 12131;
        public const int Add_Edit_SeparationCategory = 12132;
        public const int Add_Edit_EmployeeLeaveApplication = 12135;
        public const int ApproveEmployeeLeaveApplication = 12136;
        public const int Add_Edit_PMS_Section = 12157;
        public const int Add_Edit_PMS_PerformanceCycle = 12160;
        public const int Add_Edit_PMS_AppraisalTemplate = 12161;
        public const int Add_Edit_PMS_EmployeeAppraisalTemplateMapping = 12164;
        public const int Add_Edit_SeparationApplication = 12168;
        public const int ApproveSeparationApplication = 12169;
       

        public const int Add_DeliveryChallanRegister = 2064;
        public const int Add_PurchaseInvoiceRegister = 2063;
        public const int Add_Edit_Role_UI_Mapping_Admin = 9;
        public const int Add_Edit_Product_Opening_Stock = 17;
        public const int Add_Edit_ProductBOM = 19;
        public const int Add_Edit_Employee = 28;
        public const int Add_Edit_PaymentMode_CP =22;
        public const int Add_Edit_SLType_CP = 24;
        public const int Add_Edit_Services_CP = 25;
        public const int Add_Edit_Schedule_ACCOUNT = 1055;
        public const int Add_Edit_CompanyBranch_ADMIN = 1059;
        public const int Add_Edit_FormType_ADMIN = 1063;
        public const int Add_Edit_CustomerForm = 1067;
        public const int Add_Edit_CostCenter_ACCOUNT = 2079;
        public const int Add_Edit_CashVoucher= 2080;
        public const int Add_Edit_ProductGLMapping = 3090;
        public const int Add_Edit_AdditionalTax_ACCOUNT = 9105;
        public const int Add_Edit_PositionType = 10115;
        public const int Add_Edit_PositionLevel = 10116;
        public const int Add_Edit_InterviewType = 10117;
        public const int Add_Edit_Calender = 10118;
        public const int Add_Edit_ActivityCalender = 11118; 
        public const int Add_Edit_HolidayType = 11119;
        public const int Add_Edit_HolidayCalender = 11121;
        public const int Add_Edit_AssetType = 11122;
        public const int Add_Edit_TravelType = 11123;
        public const int Add_Edit_AdvanceType = 11124;
        public const int Add_Edit_LoanType = 11129;
        public const int Add_Edit_VerificationAgency = 11132;
        public const int Add_Edit_ResumeSource = 11134;
        public const int Add_Edit_Roaster = 11136;
        public const int Add_Edit_ExitInterview = 12171;
        public const int Add_Edit_ClearanceTemplate = 12173;
        public const int Add_Edit_SeparationOrder = 12175;
        public const int Add_Edit_EmployeeClearanceProces = 12176;
        public const int ListEmployeeClearance = 12188;
        public const int Add_Edit_Size = 12198;
        public const int Add_Manufacturer = 12199;
        public const int Add_ProductSerial = 12200;
        public const int ImportManufacturer = 12204;
        public const int ImportSize = 12201;
        public const int Add_Edit_LeadMaster = 12259;



        public const int Add_Edit_EmployeeProfile = 12177; 
        

        public const int Add_Edit_Customer = 32;
        public const int Add_Edit_Quotation = 39;
        public const int Add_Edit_PO = 43;
        public const int Add_Edit_PI = 49;
        public const int Add_Edit_ProductTaxMapping = 50;
        public const int Add_Edit_ApprovePo = 12219;



        public const int Add_Edit_MRN = 1056;
        public const int Add_Edit_MRNPOLIST = 12245;
        public const int Add_Edit_MRNQC = 12251;
        public const int Add_Edit_MRNQCLIST = 12251;
        public const int Add_Edit_STN = 1064;

        public const int Add_Edit_STR = 1068;

        public const int Add_Edit_FollowUpActivityType_CP = 33;

        public const int Print_Stock_Ledger = 1069;
        public const int Stock_Ledger_DrilDown = 12210;
        public const int AddCustomer_Form_Register = 2066;

        public const int AddCustomer_Payment_Register = 2069;

        public const int AddEdit_UserEmailSetting = 2072;

        public const int AddEdit_GL = 2073;

        public const int Add_Edit_VendorForm = 2075;
        public const int Add_Edit_BankVoucher = 2074;

        public const int Add_Edit_SL = 2077;
        public const int Add_Edit_SLDetail = 2078;

        public const int BankBookPrint = 2081;
        public const int CashBookPrint = 3083;
        public const int Add_Edit_JournalVoucher = 2082;
        public const int JournalBookPrint = 3082;
        public const int SaleBookPrint = 3085;
        public const int PurchaseBookPrint = 3086;
        public const int GeneralLedgerPrint = 3088;
        public const int CreditNoteBookPrint = 3087;
        public const int DebitNoteBookPrint = 3089;

        public const int SubLedgerPrint = 4090;
        public const int GLTrialBalancePrint = 5090;
        public const int SLTrialBalancePrint = 5091;
        public const int PLStatementPrint = 6090;
        public const int BalanceSheetPrint = 6091;
        public const int TBDrillDown = 7090;

        public const int PLDrillDown = 12280;

        public const int BalanceSheetDrillDown = 12276;
        
        public const int ImportLead = 8096;
        public const int ImportGLMainGroup = 8097;
        public const int ImportGLSubGroup = 8098;
        public const int ImportSL = 8099;
        public const int ImportCustomer = 9097;
        public const int ImportEmployee = 9099;
        public const int ImportDepartment = 9101;
        public const int ImportDesignation = 9102;
        public const int ImportProductSubGroup = 9103;
        public const int ImportProduct = 9104;
        public const int ImportVendor = 9098;
        public const int ImportCustomerBranch = 9100;
        public const int ImportGL = 9106;
        public const int ImportProductBOM = 10106;
        public const int ImportProductOpeningStock = 10107;
        public const int ImportProductMainGroup = 10108;
        public const int ImportGLDetail = 10109;
        public const int ImportSLDetail = 10110;       
        public const int Add_Edit_SaleVoucher = 10111;
        public const int Add_Edit_PurchaseVoucher = 10112;
        public const int Add_Edit_ChasisSerialMapping = 10114; 
        public const int ApproveResourceRequisition = 11120;
        public const int Add_Edit_Claim_HR = 11126;
        public const int Add_Edit_Education_HR = 11127;
        public const int Add_Edit_Shift_HR = 12165;
        public const int Add_Edit_Location = 12187;

        public const int Add_Edit_CTC_HR = 11137;
        public const int Add_Edit_JobOpening = 11128;

        public const int Add_Edit_Interview = 11139;
       // public const int Add_Edit_Interview = 12156;


        public const int Add_Edit_Appointment = 12130;
        public const int Add_Edit_EmployeeApprovalLoanApplication = 12128;
        public const int Add_Edit_EmployeeLoanApplication = 11142;
        public const int Add_Edit_EmployeeAssetApplication = 12129;
        public const int Add_Edit_ApprovalEmployeeAssetApplication = 12133;
        public const int Add_Edit_Applicant = 11138;
        public const int ShortlistApplicant = 12134;
        public const int Add_Edit_Goal = 12159;

        public const int UpdateDateWiseShift = 12163;
        public const int Add_Edit_PMS_EmployeeAppraisalReview = 12170;
        public const int Add_Edit_EmployeeLeaveDetail = 12172;
        public const int Add_Edit_PayrollProcessPeriod = 12174;



        public const int Add_Edit_PayrollProcessReport = 12283;
        
        public const int Add_Edit_Thought = 12181;
        public const int Add_Edit_PayrollTds = 12257;


        public const int Add_Edit_PMS_EmployeeSelfAssessment= 12178;
        public const int Add_Edit_PMS_AppraiserAssessment = 12179;
        public const int Add_Edit_PMS_FinalAssessment = 12180;
        public const int Add_Edit_SIN = 12186;

        public const int Add_Edit_Email_Template = 12185;

        public const int CustomMail_SendCustomMail = 12189;
        public const int Add_Edit_SaleReturn = 12194;

        public const int MarkAttendance = 12191;
        public const int ApproveAttendance = 12192;

        public const int Add_GETGSTR1 = 12193;
        public const int Add_Edit_PurchaseReturn = 12196;
        public const int Add_Edit_EmployeeAttandance = 12208;      
        public const int Add_Edit_WorkOrder = 12206;
        public const int Add_Edit_StoreRequisition = 12207;
        public const int Add_Edit_ModifyStoreRequisition = 12218;
        public const int ProductBOM_Report = 12212;
        public const int Add_Edit_PurchaseQuotation = 12211;
        public const int Add_Edit_PurchaseIndent = 12213;
        public const int Add_Edit_Fabrication = 12214;
        public const int Add_Edit_InternalRequisition = 12217;
        public const int List_Product_Purchase = 12220;
        public const int Add_Edit_Project = 12221;
        public const int Add_Edit_PaintProcess = 12222; 
        public const int Add_Edit_AssemblingProcess= 12223;
        public const int Add_Edit_FinishedGoodProcess = 12224;

        public const int SalarySummaryReport = 12226;
        public const int SalaryJV = 12228;
        public const int AddEditInvoicePackingList = 12230;
        public const int Add_Edit_PayrollOtherEarningDeduction = 12227;
        public const int Add_Edit_PayrollMonthlyAdjustment = 12231; 
        public const int Add_Edit_PackingList = 12229;
        public const int List_PayHeadGlMapping = 12232;
        public const int ImportChessisSerialMapping = 12233;
        public const int Add_Edit_PackingMaterialBOM = 12235;
        public const int Add_Edit_ChasisSerialPlan = 12166;
        public const int Add_Edit_ChasisModel = 12236;
        public const int Add_Edit_PrintChasis = 12237;
        public const int Add_Edit_CarryForwardChasis = 12238;
        public const int Add_Edit_ChassisNoSoldDetails = 12239;
        public const int Add_Edit_JobWorkMRN = 12242;

        public const int Add_Edit_JobWork = 12241;

        public const int Add_Edit_BankStatement = 12243;

        public const int Add_Edit_BankReconcilation = 12244;
        public const int Add_Edit_MRNPO = 12245;
        public const int Add_Edit_PhysicalStock = 12246;
        public const int WIP_Report = 12248;

        public const int Add_Edit_GateIn = 12249;
        public const int Add_Edit_QualityCheck = 12250;
        public const int Add_Edit_Return = 12253;

        public const int Add_Edit_TargetType = 12255;
        public const int Add_Edit_TDSSection = 12256;
        public const int ImportEmployeePayInfo = 12258;
        public const int Add_Edit_SaleTarget = 12254;
        public const int SalesTargetType_Report = 12279;

        public const int Add_Edit_ComplaintService = 12260;

        public const int Add_Edit_Language = 11131;

        public const int Add_Edit_Religion_HR = 11133;

        public const int Add_Edit_LeaveType = 11135;

        public const int Add_Edit_GoalCategory_HR = 12158;

        public const int Add_Edit_QuotationComparison = 12216;
        public const int Add_Edit_ApprovePurchaseIndent = 12215;

        public const int Add_Edit_CancelSaleInvoice = 48;

        public const int Add_Edit_ShiftType_HR = 11130;

        public const int Add_Edit_SubGroupWiseChassisNoSoldDetails = 12240;

        public const int Add_Edit_EmployeeClaimApplication = 12137;
        public const int Add_Edit_ApprovalEmployeeClaimApplication = 12141;

        public const int Add_Edit_ApprovePIList = 12267;
        public const int Add_Edit_PurchaseInvoiceImport = 12268;
        public const int LegerReport = 12271;
      //  public const int Add_Edit_PurchaseInvoiceImport = 12268; 
        public const int Add_Edit_MaterialRejectNote = 12270;
        public const int Add_Edit_DailyProductionReport = 12273;
        public const int Add_SaleInvoiceCancel = 12274;
        public const int Add_Edit_DashboardInterface_ADMIN = 12278;
        public const int Add_Edit_DashboardContainer_ADMIN = 12281;

        public const int AddEditDashDashboardItemMapping_Admin = 12282;

        public const int Add_Edit_Service = 12286;
        public const int Add_Edit_JobCard = 12287;
        public const int Add_Edit_HSN = 12288;

        public const int Add_Edit_ServiceInvoice = 12289;
        public const int Add_Edit_CancelServiceInvoice = 12289;

    }
}

