using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Common
{
    public class ActionMessage
    {
        #region Exception
        public const String ApplicationException = "Error occured in application. Please contact administrator";
        public const String SessionException = "Session Expired!!!";
        public const String AuthenticationException = "User not authenticated";
        public const String AccessDenied = "Access Denied";
        public const String ProbleminData = "Problem in Data";

        #endregion
        #region Login
        public const String LoginSuccess = "Login Success";
        public const String UserNotActive = "User is not active.";
        public const String UserLoginExpired = "User login expired.";
        public const String InvalidCredential = "Invalid User Name/Password";
        #endregion
        #region Company
        public const String CompanyCreatedSuccess = "Company Created Successfully";
        public const String CompanyUpdatedSuccess = "Company Updated Successfully";
        public const String DuplicateCompanyCode = "Company Code already opt out";
        public const String DuplicateCompanyName= "Company already exist with Same Name & Email";
        #endregion
        #region CompanyBranch
        public const String CompanyBranchCreatedSuccess = "Company Branch Created Successfully";
        public const String CompanyBranchUpdatedSuccess = "Company Branch Updated Successfully";
        public const String CompanyBranchDuplicateGSTNo = "GST No. Already Exist";
        public const String CompanyBranchDuplicateBranch = "Branch Already Exist";

        #endregion


        #region CompanyForm
        public const String CompanyFormCreatedSuccess = "Company Form Created Successfully";
        public const String CompanyFormUpdatedSuccess = "Company Form Updated Successfully";

        #endregion

        #region User
        public const String UserCreatedSuccess = "User Created Successfully";
        public const String UserUpdatedSuccess = "User Updated Successfully";
        public const String DuplicateUsername = "Login Id already opt out";
        public const String PasswordDoesNotMatch = "Password does not match ";
       // public const String UserImageRemoveSuccess = "User Image Removed Successfully";
        public const String PasswordChangeSuccessfully = "Password Change Successfully ";
        
        #endregion
        #region FinYear
        public const String FinYearCreatedSuccess = "Financial Year Created Successfully";
        public const String FinYearUpdatedSuccess = "Financial Year Updated Successfully";
        public const String DuplicateFinYear = "Financial Year already exists";
        #endregion


        #region Country
        public const String CountryCreatedSuccess = "Country Created Successfully";
        public const String CountryUpdatedSuccess = "Country Updated Successfully";
        public const String DuplicateCountryCode = "Country Code already opt out";
        public const String DuplicateCountryName = "Country already exist with Same Name";
        #endregion

        #region State
        public const String StateCreatedSuccess = "State Created Successfully";
        public const String StateUpdatedSuccess = "State Updated Successfully";
        public const String DuplicateStateCode = "State Code already opt out";
        public const String DuplicateStateName = "State already exist with Same Name";
        #endregion

        #region City
        public const String CityCreatedSuccess = "City Created Successfully";
        public const String CityUpdatedSuccess = "City Updated Successfully";
        public const String DuplicateCityName = "City already exist with same name";
        #endregion

        #region Role
        public const String RoleCreatedSuccess = "Role Created Successfully";
        public const String RoleUpdatedSuccess = "Role Updated Successfully";
        public const String DuplicateRoleDesc = "Role Code already opt out";
        public const String DuplicateRoleName = "Role already exist with Same Name";
        #endregion

        #region LeadStatus
        public const String LeadStatusCreatedSuccess = "Lead Status Created Successfully";
        public const String LeadStatusUpdatedSuccess = "Lead Status Updated Successfully"; 
        public const String DuplicateLeadStatusName = "Lead Status Name already exist with Same Name";
        #endregion

        #region LeadSource
        public const String LeadSourceCreatedSuccess = "Lead Source Created Successfully";
        public const String LeadSourceUpdatedSuccess = "Lead Source Updated Successfully";
        public const String DuplicateLeadSourceName = "Lead Source Name already exist with Same Name";
        #endregion

        #region Role UI Mapping
        public const String RoleUIMappingSuccessful = "Role UI Mapping Updated Successfully";
        #endregion

        #region ProductType
        public const String ProductTypeCreatedSuccess = "Product Type Created Successfully";
        public const String ProductTypeUpdatedSuccess = "Product Type Updated Successfully";
        public const String DuplicateProductTypeName = "Product Type Name already exist with Same Name";
        public const String DuplicateProductTypeCode = "Product Type Code already exist with Same Name";
        #endregion
        #region PositionType
        public const String PositionTypeCreatedSuccess = "Position Type Created Successfully";
        public const String PositionTypeUpdatedSuccess = "Position Type Updated Successfully";
        public const String DuplicatePositionTypeName = "Position Type Name already exist";
        public const String DuplicatePositionTypeCode = "Position Type Code already exist";
        #endregion
        #region InterviewType
        public const String InterviewTypeCreatedSuccess = "Interview Type Created Successfully";
        public const String InterviewTypeUpdatedSuccess = "Interview Type Updated Successfully";
        public const String DuplicateInterviewTypeName = "Interview Type Name already exist";
        #endregion
        #region PMS_Section
        public const String PMS_SectionCreatedSuccess = "PMS Section Created Successfully";
        public const String PMS_SectionUpdatedSuccess = "PMS Section Updated Successfully";
        public const String DuplicatePMS_SectionName = "PMS Section Name already exist";
        #endregion

        #region PMS_PerformanceCycle
        public const String PMS_PerformanceCycleCreatedSuccess = "PMS Performance Cycle Created Successfully";
        public const String PMS_PerformanceCycleUpdatedSuccess = "PMS Performance Cycle Updated Successfully";
        public const String DuplicatePMS_PerformanceCycleName = "PMS Performance Cycle Name already exist";
        #endregion

        #region Resume Source
        public const String ResumeSourceCreatedSuccess = "Resume Source Created Successfully";
        public const String ResumeSourceUpdatedSuccess = "Resume Source Updated Successfully";
        public const String DuplicateResumeSourceName = "Resume Source Name already exist";
        #endregion
        #region AssetType
        public const String AssetTypeCreatedSuccess = "Asset Type Created Successfully";
        public const String AssetTypeUpdatedSuccess = "Asset Type Updated Successfully";
        public const String DuplicateAssetTypeName = "Asset Type Name already exist";
        #endregion
        #region Roaster
        public const String RoasterCreatedSuccess = "Roaster Created Successfully";
        public const String RoasterUpdatedSuccess = "Roaster Updated Successfully";
        public const String DuplicateRoasterName = "Roaster Name already exist";
        #endregion

        #region VerificationAgency
        public const String VerificationAgencyCreatedSuccess = "Verification Agency Created Successfully";
        public const String VerificationAgencyUpdatedSuccess = "Verification Agency Updated Successfully";
        public const String DuplicateVerificationAgencyName = "Verification Agency Name already exist";
        #endregion

        #region LoanType
        public const String LoanTypeCreatedSuccess = "Loan Type Created Successfully";
        public const String LoanTypeUpdatedSuccess = "Loan Type Updated Successfully";
        public const String DuplicateLoanTypeName = "Loan Type Name already exist";
        #endregion


        #region TravelType
        public const String TravelTypeCreatedSuccess = "Travel Type Created Successfully";
        public const String TravelTypeUpdatedSuccess = "Travel Type Updated Successfully";
        public const String DuplicateTravelTypeName = "Travel Type Name already exist";
        #endregion
        #region AdvanceType
        public const String AdvanceTypeCreatedSuccess = "Advance Type Created Successfully";
        public const String AdvanceTypeUpdatedSuccess = "Advance Type Updated Successfully";
        public const String DuplicateAdvanceTypeName = "Advance Type Name already exist";
        #endregion
        #region SeparationClearList
        public const String SeparationClearListCreatedSuccess = "Separation Clear List Created Successfully";
        public const String SeparationClearListUpdatedSuccess = "Separation Clear List Updated Successfully";
        public const String DuplicateSeparationClearListName = " Separation Clear List Name already exist";
        #endregion
        #region SeparationCategory
        public const String SeparationCategoryCreatedSuccess = "Separation Category Created Successfully";
        public const String SeparationCategoryUpdatedSuccess = "Separation Category Updated Successfully";
        public const String DuplicateSeparationCategoryName = " Separation Category Name already exist";
        #endregion

        #region HolidayType
        public const String HolidayTypeCreatedSuccess = "Holiday Type Created Successfully";
        public const String HolidayTypeUpdatedSuccess = "Holiday Type Updated Successfully";
        public const String DuplicateHolidayTypeName = "Holiday Type Name already exist"; 
        #endregion

        #region ProductLevel
        public const String PositionLevelCreatedSuccess = "Position Level Created Successfully";
        public const String PositionLevelUpdatedSuccess = "Position Level Updated Successfully";
        public const String DuplicatePositionLevelName = "Position Level Name already exist";
        public const String DuplicatePositionLevelCode = "Position Level Code already exist";
        #endregion

        #region Calender
        public const String CalenderCreatedSuccess = "Calender Created Successfully";
        public const String CalenderUpdatedSuccess = "Calender Updated Successfully";
        public const String DuplicateCalenderName = "Calender Name already exist";
        public const String DuplicateCalenderYear = "Calender Year already exist";
        #endregion
        #region ClaimType
        public const String ClaimTypeCreatedSuccess = "Claim Type Created Successfully";
        public const String ClaimTypeUpdatedSuccess = "Claim Type Updated Successfully";
        public const String DuplicateClaimTypeName = "Claim Type Name already exist with Same Name";
        public const String DuplicateClaimNature = "Claim Nature already exist with Same Name";
        #endregion

        #region LeaveType
        public const String LeaveTypeCreatedSuccess = "Leave Type Created Successfully";
        public const String LeaveTypeUpdatedSuccess = "Leave Type Updated Successfully";
        public const String DuplicateLeaveTypeName = "Leave Type Name already exist with Same Name";
        public const String DuplicateLeaveCode = "Leave Code already exist with Same Name";
        #endregion

        #region CTC
        public const String CTCCreatedSuccess = "CTC Created Successfully";
        public const String CTCUpdatedSuccess = "CTC Updated Successfully";
        public const String DuplicateDesignationId = "Designation Id already Exist";
        #endregion
        #region Education
        public const String EducationCreatedSuccess = "Education Created Successfully";
        public const String EducationUpdatedSuccess = "Education Updated Successfully";
        public const String DuplicateEducation = "Education already exist with Same Name";

        #endregion
        #region Religion
        public const String ReligionCreatedSuccess = "Religion Created Successfully";
        public const String ReligionUpdatedSuccess = "Religion Updated Successfully";
        public const String DuplicateReligion = "Religion already exist with Same Name";

        #endregion

        #region Language
        public const String LanguageCreatedSuccess = "Language Created Successfully";
        public const String LanguageUpdatedSuccess = "Language Updated Successfully";
        public const String DuplicateLanguage = "Language already exist with Same Name";

        #endregion
        #region SHiftType
        public const String ShiftTypeCreatedSuccess = "Shift Type Created Successfully";
        public const String ShiftTypeUpdatedSuccess = "Shift Type Updated Successfully";
        public const String DuplicateShiftType = "Shift Type already exist with Same Name";

        #endregion



        #region PaymentTerm
        public const String PaymentTermCreatedSuccess = "Payment Term Created Successfully";
        public const String PaymentTermUpdatedSuccess = "Payment Term Updated Successfully";
        public const String DuplicatePaymentTermDesc = "Payment Term Name already exist with Same Name";
        #endregion

        #region Department
        public const String DepartmentCreatedSuccess = "Department Created Successfully";
        public const String DepartmentUpdatedSuccess = "Department Updated Successfully";
        public const String DuplicateDepartmentName= "Department Name already exist with Same Name";
        public const String DuplicateDepartmentCode = "Department Code already exist with Same Name";
        #endregion

        #region Product
        public const String ProductCreatedSuccess = "Product Created Successfully";
        public const String ProductUpdatedSuccess = "Product Updated Successfully";
        public const String ProductImageRemoveSuccess = "Product Image Removed Successfully";
        public const String DuplicateProductCode = "Product Code already exist";
        public const String DuplicateProductName = "Product Name already exist";
        #endregion


        #region ProductMainGroup
        public const String ProductMainGroupCreatedSuccess = "Product Main Group Created Successfully";
        public const String ProductMainGroupUpdatedSuccess = "Product Main Group Updated Successfully";
        public const String DuplicateProductMainGroupName = "Product Main Group Name already exist";
        public const String DuplicateProductMainGroupCode = "Product Main Group Code already exist";
        #endregion 

        #region ProductSubGroup
        public const String ProductSubGroupCreatedSuccess = "Product Sub Group Created Successfully";
        public const String ProductSubGroupUpdatedSuccess = "Product Sub Group Updated Successfully";
        public const String DuplicateProductSubGroupName = "Products Sub Group Name already exist";
        public const String DuplicateProductSubGroupCode = "Product Sub Group Code already exist";
        #endregion

        #region UOM
        public const String UOMCreatedSuccess = "UOM Created Successfully";
        public const String UOMUpdatedSuccess = "UOM Updated Successfully";
        public const String DuplicateUOMName = "UOM Name already exist";
        public const String DuplicateUOMDesc = "UOM Desc already exist";
        #endregion

        #region Designation
        public const String DesignationCreatedSuccess = "Designation Created Successfully";
        public const String DesignationUpdatedSuccess = "Designation Updated Successfully";
        public const String DuplicateDesignationName = "Designation Name already exist with Same Name";
        public const String DuplicateDesignationCode = "Designation Code already exist with Same Name";
        #endregion
        #region HR_ActivityCalender
        public const String ActivityCalenderCreatedSuccess = "Activity Calender Created Successfully";
        public const String ActivityCalenderUpdatedSuccess = "Activity Calender Updated Successfully"; 
        #endregion 

        #region HR_HolidayCalender
        public const String HolidayCalenderCreatedSuccess = "Holiday Calender Created Successfully";
        public const String HolidayCalenderUpdatedSuccess = "Holiday Calender Updated Successfully"; 
        #endregion


        #region Lead
        public const String LeadCreatedSuccess = "Lead Created Successfully";
        public const String LeadUpdatedSuccess = "Lead Updated Successfully";
        public const String DuplicateLeadCode = "Lead Code already opt out";
        public const String DuplicateLead = "Company Name already exist"; 
        #endregion
        #region Product Opening Stock
        public const String ProductOpeningCreatedSuccess = "Product Opening Stock Created Successfully";
        public const String ProductOpeningUpdatedSuccess = "Product Opening Stock Updated Successfully";
        public const String DuplicateProductOpening = "Product Opening Stock already exist for selected Financial year";
        #endregion

        #region Product BOM
        public const String ProductBOMCreatedSuccess = "Assembly BOM Created Successfully";
        public const String ProductBOMUpdatedSuccess = "Assembly BOM Updated Successfully";
        public const String DuplicateBOM = "Assembly BOM Already exist with this Assembly and Component";
        public const String ProductBOMCopySuccess = "Assembly BOM Copied Successfully";
        public const String ProductBOMCopyFail = "Assembly BOM Copied Fail";
        #endregion
        #region PaymentMode
        public const String PaymentModeCreatedSuccess = "Payment Mode Created Successfully";
        public const String PaymentModeUpdatedSuccess = "Payment Mode Updated Successfully";
        public const String DuplicatePaymentModeName = "Payment Mode Name already exist with Same Name";
        #endregion

        #region SLType
        public const String SLTypeCreatedSuccess = "SL Type Created Successfully";
        public const String SLTypeUpdatedSuccess = "SL Type Updated Successfully";
        public const String DuplicateSLTypeName = "SL Type Name already exist with Same Name";
        #endregion

        #region Services
        public const String ServicesCreatedSuccess = "Services Created Successfully";
        public const String ServicesUpdatedSuccess = "Services Updated Successfully";
        public const String DuplicateServicesName = "Services Name already exist with Same Name";
        #endregion

        #region Employee State Mapping
        public const String EmployeeStateMappingSuccessful = "Employee State Mapping Updated Successfully";
        #endregion

        #region Employee
        public const String EmployeeCreatedSuccess = "Employee Created Successfully";
        public const String EmployeeUpdatedSuccess = "Employee Updated Successfully";
        public const String DuplicateEmployeeCode = "Employee Code already exist";
        public const String EmployeeImageRemoveSuccess = "Employee Image Removed Successfully";
        public const String DuplicateEmployeeDetail = "Employee with Same Name, Mobile Number already exist";
        public const String DuplicateUANNoDetail = "UAN No cannot be Dublicate";
        #endregion

        #region Tax
        public const String TaxCreatedSuccess = "Tax Created Successfully";
        public const String TaxUpdatedSuccess = "Tax Updated Successfully";
        public const String DuplicateTaxName = "Tax Name already exist";
        #endregion

        #region Book
        public const String BookCreatedSuccess = "Book Created Successfully";
        public const String BookUpdatedSuccess = "Book Updated Successfully";
        public const String DuplicateBookName = "Book Name already exist";
        public const String DuplicateBookCode = "Book Code already exist";
        public const String DuplicateBookGLCode = "GL Code already used for other Book";
        #endregion

        #region Customer
        public const String CustomerCreatedSuccess = "Customer Created Successfully";
        public const String CustomerUpdatedSuccess = "Customer Updated Successfully";
        public const String DuplicateCustomerCode = "Mobile No. already exist";
        public const String DuplicateCustomerDetail = "Customer with Same Name, Mobile Number and Type already exist";
        public const String CustomerBranchRemovedSuccess = "Customer Branch removed";
        public const String CustomerProductRemovedSuccess = "Customer Product removed";
        public const String CustomerFollowUpsDateCheck = "Due Date Should be Greater then Reminder Date";
        #endregion

        #region Location
        public const String LocationCreatedSuccess = "Location Created Successfully";
        public const String LocationUpdatedSuccess = "Location Updated Successfully";
        public const String DuplicateIsStoreLocation = "Store Location Already Exist for Current Branch";
        public const String DuplicateLocation = "Location Already Exist for Current Branch";
        #endregion





        #region CustomerType
        public const String CustomerTypeCreatedSuccess = "Customer Type Created Successfully";
        public const String CustomerTypeUpdatedSuccess = "Customer Type Updated Successfully";
        public const String DuplicateCustomerTypeDesc = "Customer Type Desc already exist";
        #endregion

        #region Vendor
        public const String VendorCreatedSuccess = "Vendor Created Successfully";
        public const String VendorUpdatedSuccess = "Vendor Updated Successfully";
        public const String DuplicateVendorCode = "Vendor Code already exist";
        public const String DuplicateVendorDetail = "Vendor with Same Name and Mobile Number already exist";
        public const String VendorProductRemovedSuccess = "Vendor Product removed";
        #endregion


        #region GLMainGroup
        public const String GLMainGroupCreatedSuccess = "GL Main Group Created Successfully";
        public const String GLMainGroupUpdatedSuccess = "GL Main Group Updated Successfully";
        public const String DuplicateGLMainGroupName = "GL Main Group Name already exist";
        public const String DuplicateGLType = "GL Type with Same GLMainGroupName already exist";
        #endregion

        #region Schedule
        public const String ScheduleCreatedSuccess = "Schedule Created Successfully";
        public const String ScheduleUpdatedSuccess = "Schedule Updated Successfully";

        public const String DuplicateScheduleName = "Schedule Name already exist";
        public const String DuplicateScheduleNo = "Schedule No already exist";

        #endregion
        #region Schedule
        public const String FormTypeCreatedSuccess = "Form Type Created Successfully";
        public const String FormTypeUpdatedSuccess = "Form Type Updated Successfully";
        public const String DuplicateFormType = "Form Type Desc already exist";
        public const String DuplicateFormTypeName = "Form Type Name already exist";

        #endregion
        #region GLSubGroup
        public const String GLSubGroupCreatedSuccess = "GL Sub Group Created Successfully";
        public const String GLSubGroupUpdatedSuccess = "GL Sub Group Updated Successfully";
        public const String DuplicateGLSubGroupName = "GL Sub Group Name already exist";
        public const String DuplicateGLMainGroupId = "GL Main Group with Same GL Sub Group Name already exist";
        #endregion


        #region QuotationSetting
        public const String QuotationSettingCreatedSuccess = "Quotation Setting Created Successfully";
        public const String QuotationSettingUpdatedSuccess = "Quotation Setting Updated Successfully";
        public const String QuotationSettingExist = "Active Quotation Setting already exist";
        #endregion

        #region TermTemplate
        public const String TermTemplateCreatedSuccess = "Term Template Created Successfully";
        public const String TermTemplateUpdatedSuccess = "Term Template Updated Successfully";
        public const String TermTemplateRemovedSuccess = "Term Template Created Successfully";
        public const String DuplicateTermTempalteName = "Term Template Name Already Exit";
        #endregion


        
        #region Quotation
        public const String QuotationCreatedSuccess = "Quotation Created Successfully";
        public const String QuotationUpdatedSuccess = "Quotation Updated Successfully";
        public const String RevisedQuotationCreatedSuccess = "Revised Quotation Created Successfully";
        #endregion
        #region AppraisalTemplate
        public const String AppraisalTemplateCreatedSuccess = "Appraisal Template Created Successfully";
        public const String AppraisalTemplateUpdatedSuccess = "Appraisal Template Updated Successfully";
        #endregion
        #region ClearanceTemplate
        public const String ClearanceTemplateCreatedSuccess = "Clearance Template Created Successfully";
        public const String ClearanceTemplateUpdatedSuccess = "Clearance Template Updated Successfully";
        #endregion
        #region PO
        public const String POCreatedSuccess = "Purchase Order Created Successfully";
        public const String POUpdatedSuccess = "Purchase Order Updated Successfully";
        
        public const String RevisedPOCreatedSuccess = "Revised Purchase Order Created Successfully";
        public const String PORejectionCreatedSuccess = "PO Rejection Updated Successfully";
        public const String POApproveUpdatedSuccess = "PO Approvel Updated Successfully";
        public const String PORecommendedSuccess = "Purchase Order Recommended Successfully";
        public const String POCancelSuccess = "Purchase Order Cancel Successfully";
        #endregion

        #region PI
        public const String PICreatedSuccess = "Purchase Invoice Created Successfully";
        public const String PIUpdatedSuccess = "Purchase Invoice Updated Successfully";
        public const String PurchaseInvoiceCancelSuccess = "Purchase Invoice Cancel Successfully";
        #endregion

        #region DocumentType
        public const String DocumentTypeCreatedSuccess = "Document Type Created Successfully";
        public const String DocumentTypeUpdatedSuccess = "Document Type Updated Successfully";
        public const String DuplicateDocumentTypeDesc = "Document Type Desc already exist";
        #endregion

        #region SO
        public const String SOCreatedSuccess = "SO Created Successfully";
        public const String SOUpdatedSuccess = "SO Updated Successfully";
        public const String SOCancelSuccess = "SO Cancel Successfully";
        #endregion

        #region SaleInvoice
        public const String SaleInvoiceCreatedSuccess = "Invoice Created Successfully";
        public const String SaleInvoiceUpdatedSuccess = "Invoice Updated Successfully";
        public const String SaleInvoiceCancelSuccess = "Invoice Cancel Successfully";
        public const String SaleInvoiceProductQuantity = " Product Quantity does not Match..";
        #endregion

        #region ResourceRequisition
        public const String ResourceRequisitionCreatedSuccess = "Resource Requisition Created Successfully";
        public const String ResourceRequisitionUpdatedSuccess = "Resource Requisition Updated Successfully";
        public const String ResourceRequisitionRejectSuccess = "Resource Requisition Rejected Successfully";
        public const String ResourceRequisitionApproveSuccess = "Resource Requisition Approved Successfully";
        #endregion
         
        #region EmployeeAdvanceApplication
        public const String EmployeeAdvanceApplicationCreatedSuccess = "Employee Advance Application Created Successfully";
        public const String EmployeeAdvanceApplicationUpdatedSuccess = "Employee Advance Application Updated Successfully";
        public const String EmployeeAdvanceApplicationRejectSuccess = "Employee Advance Application Rejected Successfully";
        public const String EmployeeAdvanceApplicationApproveSuccess = "Employee Advance Application Approved Successfully";
        #endregion
        #region SeparationApplication
        public const String SeparationApplicationCreatedSuccess = "Separation Application Created Successfully";
        public const String SeparationApplicationUpdatedSuccess = "Separation Application Updated Successfully";
        public const String SeparationApplicationRejectSuccess = "Separation Application Rejected Successfully";
        public const String SeparationApplicationApproveSuccess = "Separation Application Approved Successfully";
        #endregion
        #region ExitInterview
        public const String ExitInterviewCreatedSuccess = "Exit Interview Created Successfully";
        public const String ExitInterviewUpdatedSuccess = "Exit Interview Updated Successfully";
        #endregion

        #region Separation Order
        public const String SeparationOrderCreatedSuccess = "Separation Order Created Successfully";
        public const String SeparationOrderUpdatedSuccess = " Separation Order Updated Successfully";
        #endregion

        #region EmployeeLeaveApplication
        public const String EmployeeLeaveApplicationCreatedSuccess = "Employee Leave Application Created Successfully";
        public const String EmployeeLeaveApplicationUpdatedSuccess = "Employee Leave Application Updated Successfully";
        public const String EmployeeLeaveApplicationRejectSuccess = "Employee Leave Application Rejected Successfully";
        public const String EmployeeLeaveApplicationApproveSuccess = "Employee Leave Application Approved Successfully";

        #endregion

        #region PayrollTds
        public const String PayrollTdsCreatedSuccess = "Payroll TDS Created Successfully";
        public const String PayrollTdsUpdatedSuccess = "Payroll TDS Updated Successfully";

        #endregion

        #region LeadTypeMaster
        public const String LeadTypeMasterCreatedSuccess = "Lead Type Master Created Successfully";
        public const String LeadTypeMasterUpdatedSuccess = "Lead Type Master Updated Successfully";

        #endregion

        #region EmployeeTravelApplication
        public const String EmployeeTravelApplicationCreatedSuccess = "Employee Travel Application Created Successfully";
        public const String EmployeeTravelApplicationUpdatedSuccess = "Employee Travel Application Updated Successfully";
        public const String EmployeeTravelApplicationRejectSuccess = "Employee Travel Application Rejected Successfully";
        public const String EmployeeTravelApplicationApproveSuccess = "Employee Travel Application Approved Successfully";
        #endregion

        #region SoSetting
        public const String SOSettingCreatedSuccess = "Sale Order Setting Created Successfully";
        public const String SOSettingUpdatedSuccess = "Sale Order Setting Updated Successfully";
        public const String SOSettingExist = "Active Sale Order Setting already exist";
        #endregion

        #region Product State Tax Mapping
        public const String ProductStateTaxMappingCreatedSuccessful = "Product State Tax Mapping Created Successfully";
        public const String ProductStateTaxMappingUpdateSuccessful = "Product State Tax Mapping Updated Successfully";
        public const String DuplicateProductStateTaxMapping = "Product State Tax Mapping already exist with same Product";
        #endregion

        #region Product GL Mapping
        public const String ProductGLMappingCreatedSuccessful = "Product GL Mapping Created Successfully";
        public const String ProductGLMappingUpdateSuccessful = "Product GL Mapping Updated Successfully";
        public const String DuplicateProductSubGroup = "Product Type and GL Mapping already exist with same Product Type";
        #endregion

        #region Customer Payment
        public const String CustomerPaymentCreatedSuccess = "Customer Payment Created Successfully";
        public const String CustomerPaymentUpdatedSuccess = "Customer Payment Updated Successfully"; 
        public const String DuplicateCustomerPaymentDetail = "Customer with Same Name, Invoice Id and Ref No already exist";
        #endregion
        #region Vendor Payment
        public const String VendorPaymentCreatedSuccess = "Vendor Payment Created Successfully";
        public const String VendorPaymentUpdatedSuccess = "Vendor Payment Updated Successfully";
        public const String DuplicateVendorPaymentDetail = "Vendor with Same Name, Invoice Id and Ref No already exist";
        #endregion

        #region Delivery Challan
        public const String ChallanCreatedSuccess = "Challan Created Successfully";
        public const String ChallanUpdatedSuccess = "Challan Updated Successfully";
        #endregion

        #region Sale Return
        public const String SaleReturnCreatedSuccess = "Sale Return Created Successfully";
        public const String SaleReturnUpdatedSuccess = "Sale Return Updated Successfully";

        public const String SaleReturnCancelSuccess = "Sale Return Cancel Successfully";
        #endregion

        #region Purchase Return
        public const String PurchaseReturnCreatedSuccess = "Purchase Return Created Successfully";
        public const String PurchaseReturnUpdatedSuccess = "Purchase Return Updated Successfully";
        #endregion

        #region EmployeeLoanApplication
        public const String EmployeeLoanApplicationCreatedSuccess = "Employee Loan Application Created Successfully";
        public const String EmployeeLoanApplicationUpdatedSuccess = "Employee Loan Application Updated Successfully";
        public const String EmployeeLoanApplicationRejectionCreatedSuccess = "Loan Status Rejection Updated Successfully";
        public const String EmployeeLoanApplicationApproveUpdatedSuccess = "Loan Status Approve Updated Successfully";
        #endregion

        #region MRN
        public const String MRNCreatedSuccess = "MRN Created Successfully";
        public const String MRNUpdatedSuccess = "MRN Updated Successfully";
        public const String MRNCancelSuccess = "MRN Canceled Successfully";
        #endregion

        #region STN
        public const String STNCreatedSuccess = "STN Created Successfully";
        public const String STNUpdatedSuccess = "STN Updated Successfully";
        #endregion
        #region SIN
        public const String SINCreatedSuccess = "SIN Created Successfully";
        public const String SINUpdatedSuccess = "SIN Updated Successfully";
        public const String SINCancelSuccess = "SIN Cancel Successfully";
        #endregion

        #region STR
        public const String STRCreatedSuccess = "STR Created Successfully";
        public const String STRUpdatedSuccess = "STR Updated Successfully";
        #endregion

        #region FollowUp Activity Type
        public const String FollowUpActivityTypeCreatedSuccess = "Follow Up Activity Type Created Successfully";
        public const String FollowUpActivityTypeUpdatedSuccess = "Follow Up Activity Type Updated Successfully";
        public const String DuplicateFollowUpActivityTypeName = "Follow Up Activity Type already exist";
        #endregion

        #region UserEmailSettinh
        public const String UserEmailSettingCreatedSuccess = "User Email Setting Created Successfully";
        public const String UserEmailSettingUpdatedSuccess = "User Email Setting Updated Successfully";
        public const String DuplicateUserEmailSetting = "Email Setting already Created..";
        #endregion

        #region VendorForm
        public const String VendorFormCreatedSuccess = "Vendor Form Created Successfully";
        public const String VendorFormUpdatedSuccess = "Vendor Form Updated Successfully";
        #endregion

        #region CustomerForm
        public const String CustomerFormCreatedSuccess = "Customer Form Created Successfully";
        public const String CustomerFormUpdatedSuccess = "Customer Form Updated Successfully";

        #endregion

        #region GL
        public const String GLCreatedSuccess = "GL Created Successfully";
        public const String GLUpdatedSuccess = "GL Updated Successfully";
        public const String GLDuplicateCode = "GL Code already exist";
        #endregion

        #region CostCenter
        public const String CostCenterCreatedSuccess = "Cost Center Created Successfully";
        public const String CostCenterUpdatedSuccess = "Cost Center Updated Successfully";
        public const String DuplicateCostCenterName = "Cost Center Name already exist";
        public const String DuplicateCostCenter = "Cost Center with Same CostCenterName already exist";
        #endregion
        
        #region Bank Voucher
        public const String BankVoucherCreatedSuccess = "Bank Voucher Created Successfully";
        public const String BankVoucherUpdatedSuccess = "Bank Voucher Updated Successfully";
        public const String BankVoucherCancelSuccess = "Bank Voucher Cancel Successfully";
        public const String BankVoucherApprovedSuccess = "Bank Voucher Approved Successfully";
        #endregion
        #region Cash Voucher
        public const String CashVoucherCreatedSuccess = "Cash Voucher Created Successfully";
        public const String CashVoucherUpdatedSuccess = "Cash Voucher Updated Successfully";
        public const String CashVoucherCancelSuccess = "Cash Voucher Cancel Successfully";
        public const String CashVoucherApprovedSuccess = "Cash Voucher Approved Successfully";
        #endregion

        #region Journal  Voucher
        public const String JournalVoucherCreatedSuccess = "Journal Voucher Created Successfully";
        public const String JournalVoucherUpdatedSuccess = "Journal Voucher Updated Successfully";
        public const String JournalVoucherCancelSuccess = "Journal Voucher Cancel Successfully";
        public const String JournalVoucherApprovedSuccess = "Journal Voucher Approved Successfully";
        #endregion

        #region Debit Note Voucher
        public const String DebitNoteVoucherCreatedSuccess = "Debit Note Voucher Created Successfully";
        public const String DebitNoteVoucherUpdatedSuccess = "Debit Note Voucher Updated Successfully";
        public const String DebitNoteVoucherCancelSuccess = "Debit Note Voucher Cancel Successfully";
        public const String DebitNoteVoucherApprovedSuccess = "Debit Note Voucher Approved Successfully";
        #endregion

        #region Credit Note Voucher
        public const String CreditNoteVoucherCreatedSuccess = "Credit Note Voucher Created Successfully";
        public const String CreditNoteVoucherUpdatedSuccess = "Credit Note Voucher Updated Successfully";
        public const String CreditNoteVoucherCancelSuccess = "Credit Note Voucher Cancel Successfully";
        public const String CreditNoteVoucherApprovedSuccess = "Credit Note Voucher Approved Successfully";
        #endregion

        #region SL
        public const String SLCreatedSuccess = "SL Created Successfully";
        public const String SLUpdatedSuccess = "SL Updated Successfully";
        public const String DuplicateSLName = "SL Name already exist";
        public const String DuplicateSLCode = "SL Code already exist";
        #endregion

        #region SL Detail
        public const String SLDetailSuccessful = "SL Detail Updated Successfully";
        #endregion

        #region ChasisSerialMapping
        public const String ChasisSerialMappingCreatedSuccess = "Chasis Serial Mapping Created Successfully";
        public const String ChasisSerialMappingUpdatedSuccess = "Chasis Serial Mapping Updated Successfully";
        
        public const String DuplicateChasisSerialNo = "Chasis Serial No. already exist";
        public const String DuplicateMotorNo = "Motor No already exist";
        public const String DuplicateControllerNo = "Controller No already exist";
        #endregion


        #region JobOpening
        public const String JobOpeningCreatedSuccess = "Job Opening Created Successfully";
        public const String JobOpeningUpdatedSuccess = "Job Opening Updated Successfully";
        public const String JobOpeningCancelSuccess = "Job Opening Cancel Successfully";
        #endregion

        #region Interview
        public const String InterviewCreatedSuccess = "Interview Created Successfully";
        public const String InterviewUpdatedSuccess = "Interview Updated Successfully";

        #endregion
        #region Shift
        public const String ShiftCreatedSuccess = "Shift Created Successfully";
        public const String ShiftUpdatedSuccess = "Shift Updated Successfully";

        #endregion

        #region Appointment
        public const String AppointmentCreatedSuccess = "Appointment Created Successfully";
        public const String AppointmentUpdatedSuccess = "Appointment Updated Successfully";
        #endregion

        #region Applicant
        public const String ApplicantCreatedSuccess = "Applicant information Inserted Successfully";
        public const String ApplicantUpdatedSuccess = "Applicant Information Updated Successfully";

        #endregion

        #region EmployeeAssetApplication
        public const String EmployeeAssetApplicationCreatedSuccess = "Employee Asset Application Created Successfully";
        public const String EmployeeAssetApplicationUpdatedSuccess = "Employee Asset Application Updated Successfully";
        public const String EmployeeAssetApplicationRejectionCreatedSuccess = "Asset Status Rejection Updated Successfully";
        public const String EmployeeAssetApplicationApproveUpdatedSuccess = "Asset Status Approve Updated Successfully";
        #endregion

        #region Employee Claim Application
        public const String EmployeeClaimApplicationCreatedSuccess = "Employee Claim Application Created Successfully";
        public const String EmployeeClaimApplicationUpdatedSuccess = "Employee Claim Application Updated Successfully";
        public const String EmployeeClaimApplicationRejectionCreatedSuccess = "Claim Status Rejection Updated Successfully";
        public const String EmployeeClaimApplicationApproveUpdatedSuccess = "Claim Status Approve Updated Successfully";
        public const String EmployeeClaimApplicationStatusUpdatedSuccess = "Claim Status  Updated Successfully";
        #endregion

        #region PMSGoalCategory
        public const String GoalCategoryCreatedSuccess = "Goal Category Created Successfully";
        public const String GoalCategoryUpdatedSuccess = "Goal Category Updated Successfully";
        public const String DuplicateGoalCategory = "Goal Category already exist with Same Name";

        #endregion
        #region PMS_Goal 
        public const String GoalCreatedSuccess = "Goal Created Successfully";
        public const String GoalUpdatedSuccess = "Goal Updated Successfully";
        public const String DuplicateGoal = "Goal exist with Same Name";

        public const String GoalCategoryWeight = "Please Enter the Correct Weightage.";
        #endregion

        #region EmployeeLeaveDetail
        public const String EmployeeLeaveDetailCreatedSuccess = "Employee Leave Detail Created Successfully";
        public const String EmployeeLeaveDetailUpdatedSuccess = "Employee Leave Detail Updated Successfully";

        #endregion

        #region Employee Appraisal Template Mapping
        public const String EmployeeAppraisalTemplateMappingCreatedSuccess = "Employee Appraisal Template Mapping Created Successfully";
        public const String EmployeeAppraisalTemplateMappingUpdatedSuccess = "Employee Appraisal Template Mapping Updated Successfully";
        public const String EmployeeAssessmentUpdatedSuccess = "Employee Assessment Updated Successfully";
        #endregion
        #region Employee Appraisal Review
        public const String EmployeeAppraisalReviewCreatedSuccess = "Employee Appraisal Review Updated Successfully";
        #endregion

        #region Thought
        public const String ThoughtCreatedSuccess = "Thought Created Successfully";
        public const String ThoughtUpdatedSuccess = "Thought Updated Successfully";
        public const String DuplicateThoughtName= "Thought already opt out";
        #endregion


        #region EmailTemplate
        public const String EmailTemplateCreatedSuccess = "Email Template Created Successfully";
        public const String EmailTemplateUpdatedSuccess = "Email Template Updated Successfully";
        public const String EmailTemplateRemovedSuccess = "Email Template Created Successfully";
        public const String DuplicateEmailTempalteName = "Email Template Name Already Exit";
        #endregion

        #region StickyNotes
        public const String StickyNotesCreatedSuccess = "Sticky Notes Created Successfully";
        public const String StickyNotesUpdatedSuccess = "Sticky Notes Updated Successfully";
        public const String DuplicateStickyNotesName = "Sticky Notes already opt out";
        #endregion
        #region Employee Clearance Process Mapping
        public const String EmployeeClearanceProcessMappingCreatedSuccess = "Employee Clearance Process Mapping Created Successfully";
        public const String EmployeeClearanceProcessMappingUpdatedSuccess = "Employee Clearance Process Mapping Updated Successfully";
        public const String EmployeeClearanceUpdatedSuccess = "Employee Clearance Updated Successfully";
        #endregion

        #region Manufacturer
        public const String ManufacturerCreatedSuccess = "Manufacturer Created Successfully";
        public const String ManufacturerUpdatedSuccess = "Manufacturer Updated Successfully";
        public const String DuplicateManufacturer = "Manufacturer Name already exist with Same Name";
        public const String DuplicateManufacturercode = "Manufacturer Code already exist with Same Name";
        #endregion
        #region Size
        public const String SizeCreatedSuccess = "Size Created Successfully";
        public const String SizeUpdatedSuccess = "Size Updated Successfully";
        public const String DuplicateSizeCode = "Size Code already exist";
        public const String DuplicateSizeDesc = "Size Desc already exist";
        #endregion

        #region Product Serial
        public const String ProductSerialCreatedSuccess = "Product Serial Created Successfully";
        public const String ProductSerialUpdatedSuccess = "Product Serial Updated Successfully";
        public const String DuplicateProductSerialName = "Product Serial Name already exist";
        public const String DuplicateProductSerialCode = "Product Serial Code already exist";
        #endregion

        #region Employee Attendance
        public const String EmployeeAttendanceCreatedSuccess = "Employee Attendance Created Successfully";
        public const String EmployeeAttendanceUpdatedSuccess = "Employee Attendance Updated Successfully";
        public const String EmployeeAttendanceRejectSuccess = "Employee Attendance Rejected Successfully";
        public const String EmployeeAttendanceApproveSuccess = "Employee Attendance Approved Successfully";
        #endregion

        #region Work Order
        public const String WorkOrderCreatedSuccess = "Work Order Created Successfully";
        public const String WorkOrderUpdatedSuccess = "Work Order Updated Successfully"; 
         public const String WorkOrderCancelSuccess = "Work Order Cancel Successfully"; 

        #endregion

        #region StoreRequisition
        public const String StoreRequisitionCreatedSuccess = "Store Requisition Created Successfully";
        public const String StoreRequisitionUpdatedSuccess = "Store Requisition Updated Successfully";
        public const String StoreRequisitionRejectionCreatedSuccess = "Store Requisition Rejection Updated Successfully";
        public const String StoreRequisitionApproveUpdatedSuccess = "Store Requisition Approve Updated Successfully";
        #endregion

        #region Purchase Quotation
        public const String PurchaseQuotationCreatedSuccess = "Purchase Quotation Created Successfully";
        public const String PurchaseQuotationUpdatedSuccess = "Purchase Quotation Updated Successfully";
        public const String PurchaseRevisedQuotationCreatedSuccess = "Revised Purchase Quotation Created Successfully";
        #endregion

        #region PurchaseIndent
        public const String PurchaseIndentCreatedSuccess = "Purchase Indent Created Successfully";
        public const String PurchaseIndentUpdatedSuccess = "Purchase Indent Updated Successfully";
        public const String PurchaseIndentRejectionCreatedSuccess = "Purchase Indent Rejection Updated Successfully";
        public const String PurchaseIndentApproveUpdatedSuccess = "Purchase Indent Approve Updated Successfully";
        #endregion

        #region Fabrication
        public const String FabricationCreatedSuccess = "Fabrication Created Successfully";
        public const String FabricationUpdatedSuccess = "Fabrication Updated Successfully";
        public const String FabricationCancelSuccess = "Fabrication Cancelled Successfully";

        #endregion

        #region InternalRequisition
        public const String InternalRequisitionCreatedSuccess = "Internal Requisition Created Successfully";
        public const String InternalRequisitionUpdatedSuccess = "Internal Requisition Updated Successfully";
        public const String InternalRequisitionRejectionCreatedSuccess = "Internal Requisition Rejection Updated Successfully";
        public const String InternalRequisitionApproveUpdatedSuccess = "Internal Requisition Approve Updated Successfully";
        #endregion

        #region ApprovelStoreRequisition
        public const String ApprovelStoreRequisitionUpdatedSuccess = "Store Requisition Approvel Updated Successfully";
        #endregion


        #region Project
        public const String ProjectCreatedSuccess = "Project Created Successfully";
        public const String ProjectUpdatedSuccess = "Project Updated Successfully";
        public const String DuplicateProjectCode = "Project already exist with same name";
        #endregion
        #region Payroll Processing
        public const String PayrollProcessSuccess = "Payroll Processed Done!!!";
        public const String PayrollProcessFail = "Payroll Processing Fail";
        public const String PayrollProcessLockUnlock = "Payroll Lock Status Updated Successfully ";
        public const String PayrollSalaryJVSuccess = "Salary JV Generated Successfully ";

        #endregion

        #region PaintProcess
        public const String PaintProcessCreatedSuccess = "Paint Process Created Successfully";
        public const String PaintProcessUpdatedSuccess = "Paint Process Updated Successfully";
        public const String PaintProcessCancelSuccess = "PaintProcess Cancel Successfully";

        #endregion

        #region AssemblingProcess
        public const String AssemblingProcessCreatedSuccess = "Assembling Process Created Successfully";
        public const String AssemblingProcessUpdatedSuccess = "Assembling Process Updated Successfully";
        public const String AssemblingProcessCancelSuccess = "Assembling Process Cancelled Successfully";
        #endregion

        #region PayrollOtherEarningDeduction
        public const String PayrollOtherEarningDeductionCreatedSuccess = "Payroll Other Earning Deduction Created Successfully";
        public const String PayrollOtherEarningDeductionUpdatedSuccess = "Payroll Other Earning Deduction Updated Successfully";
        #endregion
        #region PayrollMonthlyAdjustment
        public const String PayrollMonthlyAdjustmentCreatedSuccess = "Payroll Monthly Adjustment Created Successfully";
        public const String PayrollMonthlyAdjustmentUpdatedSuccess = "Payroll Monthly Adjustment Updated Successfully";
        #endregion
        #region PackingListType
        public const String PackingListCreatedSuccess = "Packing List Created Successfully";
        public const String PackingListUpdatedSuccess = "Packing List Updated Successfully";

        #endregion

        #region FinishedGoodProcess
        public const String FinishedGoodProcessCreatedSuccess = "Finished Good Process Created Successfully";
        public const String FinishedGoodProcessUpdatedSuccess = "Finished Good Process Updated Successfully";

        public const String FinishedGoodProcessCancelSuccess = "Finished Good Process Cancel Successfully";
        #endregion

        #region PayHeadGLMapping
        public const String PayHeadGLMappingCreatedSuccess = "PayHead GL Mapping Created Successfully";
        public const String PayHeadGLMappingUpdatedSuccess = "PayHead GL Mapping Updated Successfully";
        public const String DuplicatePayHeadGLMappingName = "PayHead GL Mapping Name already exist";
        #endregion

        #region Invoice Packing List
        public const String InvoicePackingListCreatedSuccess = "Invoice Packing List Created Successfully";
        public const String InvoicePackingListUpdatedSuccess = "Invoice Packing List Updated Successfully";
        #endregion

        #region Packing Material BOM
        public const String PackingMaterialBOMCreatedSuccess = "Packing Material BOM Created Successfully";
        public const String PackingMaterialBOMUpdatedSuccess = "Packing Material BOM Updated Successfully";

        #endregion

        #region Fabrication
        public const String ChasisSerialPlanCreatedSuccess = "Chasis Serial Plan Created Successfully";
        public const String ChasisSerialPlanUpdatedSuccess = "Chasis Serial Plan Updated Successfully";

        #endregion

        #region ChasisModel
        public const String ChasisModelCreatedSuccess = "Chasis Model Created Successfully";
        public const String ChasisModelUpdatedSuccess = "Chasis Model Updated Successfully";
        public const String DuplicateChasisModelSubGroupName = "Chasis Model Name already exist";
        #endregion
        #region Fabrication
        public const String PrintChasisCreatedSuccess = "Print Chasis Created Successfully";
        public const String PrintChasisUpdatedSuccess = "Print Chasis Updated Successfully";
        #endregion
        #region CarryForwardChasis
        public const String CarryForwardChasisCreatedSuccess = "Carry Forward Chasis Created Successfully";
        public const String CarryForwardChasisUpdatedSuccess = "Carry Forward Chasis Updated Successfully";

        #endregion

        #region Job Work 
        public const String JobWorkCreatedSuccess = "Job Work Created Successfully";
        public const String JobWorkUpdatedSuccess = "Job Work Updated Successfully";

        #endregion

        #region Bank Statement
        public const String BankStatementCreatedSuccess = "Bank Statement Created Successfully";
        public const String BankStatementUpdatedSuccess = "Bank Statement Updated Successfully";

        #endregion

        #region Bank Statement
        public const String BankReconcilationCreatedSuccess = "Bank Reconcilation Created Successfully";
        public const String BankReconcilationUpdatedSuccess = "Bank Reconcilation Updated Successfully";

        #endregion


        #region Fabrication
        public const String JobWorkMRNCreatedSuccess = "Job Work MRN Created Successfully";
        public const String JobWorkMRNUpdatedSuccess = "Job Work MRN Updated Successfully";

        #endregion

        #region Bank Statement
        public const String PStockCreatedSuccess = "Physical Stock Created Successfully";
        public const String PStockUpdatedSuccess = "Physical Stock Updated Successfully";

        #endregion
        #region MRN
        public const String MRNPOCreatedSuccess = "MRN PO Created Successfully";
        public const String MRNPOUpdatedSuccess = "MRN PO Updated Successfully";
        public const String MRNPOCancelSuccess = "MRN PO Canceled Successfully";
        #endregion


        #region GateIn
        public const String GateInCreatedSuccess = "GateIn Created Successfully";
        public const String GateInUpdatedSuccess = "GateIn Updated Successfully";
        public const String GateInCancelSuccess = "GateIn Canceled Successfully";
        #endregion
        #region QualityCheck
        public const String QualityCheckCreatedSuccess = "Quality Check Created Successfully";
        public const String QualityCheckUpdatedSuccess = "Quality Check Updated Successfully";
        public const String QualityCheckCancelSuccess = "Quality Check Canceled Successfully";
        #endregion


        #region MaterialRejectNote
        public const String MaterialRejectNoteCreatedSuccess = "Material RejectNote Created Successfully";
        public const String MaterialRejectNoteUpdatedSuccess = "MaterialRejectNote Updated Successfully";
        
        #endregion


        #region MRN QC
        public const String MRNQCCreatedSuccess = "MRN QC Created Successfully";
        public const String MRNQCUpdatedSuccess = "MRN QC Updated Successfully";
        public const String MRNQCCancelSuccess = "MRN QC Canceled Successfully";
        #endregion

        #region Target Type
        public const String TargetTypeCreatedSuccess = "Target Type Created Successfully";
        public const String TargetTypeUpdatedSuccess = "Target Type Updated Successfully";
        public const String DuplicateTargetType = "Target Type Description already opt out";
        public const String DuplicateTargetTypeName = "Target Type already exist with Same Name";
        #endregion

        #region TDS Section
        public const String TDSSectionCreatedSuccess = "TDS Section Created Successfully";
        public const String TDSSectionUpdatedSuccess = "TDS Section Updated Successfully";
        public const String DuplicateTDSSection = "TDS Section Description already opt out";
        public const String DuplicateTDSSectionName = "TDS Section already exist with Same Name";
        #endregion

        #region ChasisSerialMapping
        public const String EmployeePayInfoCreatedSuccess = "Chasis Serial Mapping Created Successfully";
        public const String EmployeePayInfoUpdatedSuccess = "Chasis Serial Mapping Updated Successfully";
        public const String DuplicateEmployeePayInfo = "Employee Id already exist";
        #endregion

        #region Return 
        public const String ReturnCreatedSuccess = "Replacement Created Successfully";
        public const String ReturnUpdatedSuccess = "Replacement Updated Successfully";

        #endregion

        #region Target
        public const String TargetCreatedSuccess = "Target Created Successfully";
        public const String TargetUpdatedSuccess = "Target Updated Successfully";
        public const String TargetCancelSuccess = "Target Cancel Successfully";

        #endregion

        #region Complaint Service
        public const String ComplaintServiceCreatedSuccess = "Complaint Service Created Successfully";
        public const String ComplaintServiceUpdatedSuccess = "Complaint Service Updated Successfully";
        public const String ComplaintServiceCancelSuccess = "Complaint Service Cancel Successfully";

        #endregion
        #region Balance Transfer
        public const String BalanceTransferCreatedSuccess = "Product Closing Balance Transfer Successfully";
        public const String ReveseTransferCreatedSuccess = "Balance Transfer Reversed Created Successfully";

        #endregion

        #region GLBalance Transfer
        public const String GLBalanceTransferCreatedSuccess = "GL Closing Balance Transfer Successfully";
        public const String GLReveseTransferCreatedSuccess = "Balance Transfer Reversed Created Successfully";

        #endregion

        #region SLBalance Transfer
        public const String SLBalanceTransferCreatedSuccess = "SL Closing Balance Transfer Successfully";

        #endregion


        #region PurchaseInvoiceImport
        public const String PurchaseInvoiceImportCreatedSuccess = "Purchase Invoice Import Created Successfully";
        public const String PurchaseInvoiceImportUpdatedSuccess = "Purchase Invoice Import Updated Successfully";
        public const String PurchaseInvoiceImportCancelSuccess = "Purchase Invoice Import Cancel Successfully";
        #endregion


        #region Dashboard Interface
        public const String DICreatedSuccess = "Dashboard Interface Created Successfully";
        public const String DIUpdatedSuccess = "Dashboard Interface Updated Successfully";

        #endregion

        #region Dashboard Container 
        public const String DuplicateDashboardContainerName = "Duplicate Dashboard Container Name.";        
        public const String DuplicateDashboardContainerDisplayName = "Duplicate Dashboard Container Display Name.";
        public const String DuplicateDashboardContainerNo = "Duplicate Dashboard Container No.";
        public const String DashboardContainerCreatedSuccess = "Dashboard Container Created Successfully.";
        public const String RoleDashboardMappingSuccessful = "Role Dashboard Mapping Updated Successfully";
        #endregion
        #region Service
        public const String ServicCreatedSuccess = "Service Created Successfully";
        public const String ServicUpdatedSuccess = "Service Updated Successfully";

        #endregion

        #region JobCard
        public const String JobCardCreatedSuccess = "JobCard Created Successfully";
        public const String JobCardUpdatedSuccess = "JobCard Updated Successfully";

        #endregion

        #region HSN
        public const String HSNCreatedSuccess = "HSN Created Successfully.";
        public const String HSNUpdatedSuccess = "HSN Updated Successfully.";
        public const String HSNDublicate = "HSN Code already Added.";
       
        #endregion
    }
}
