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
using System.Transactions;
using Portal.DAL.Infrastructure;
namespace Portal.Core
{
    public class ApplicantBL
    {
        HRMSDBInterface dbInterface;
        public ApplicantBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        #region Job Applicant
        public ResponseOut AddEditApplicant(ApplicantViewModel applicantViewModel, List<ApplicantEducationViewModel> educations, List<ApplicantPrevEmployerViewModel> employers, List<ApplicantProjectViewModel> projects,ApplicantExtraActivityViewModel extraActivities)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_Applicant applicant = new HR_Applicant
                {
                    ApplicantId=Convert.ToInt32(applicantViewModel.ApplicantId),
                    ApplicationDate =Convert.ToDateTime(applicantViewModel.ApplicationDate),
                    JobOpeningId = applicantViewModel.JobOpeningId,
                    CompanyId = applicantViewModel.CompanyId,
                    CompanyBranchId = applicantViewModel.CompanyBranchId,
                    ProjectNo = applicantViewModel.ProjectNo,
                    FirstName = applicantViewModel.FirstName,
                    LastName = applicantViewModel.LastName,
                    Gender = applicantViewModel.Gender,
                    FatherSpouseName = applicantViewModel.FatherSpouseName,
                    DOB = Convert.ToDateTime(applicantViewModel.DOB),
                    BloodGroup = applicantViewModel.BloodGroup,
                    MaritalStatus = applicantViewModel.MaritalStatus,
                    ApplicantAddress = applicantViewModel.ApplicantAddress,
                    City = applicantViewModel.City,
                    StateId = applicantViewModel.StateId,
                    CountryId = applicantViewModel.CountryId,
                    PinCode = applicantViewModel.PinCode,
                    ContactNo = applicantViewModel.ContactNo,
                    MobileNo = applicantViewModel.MobileNo,
                    Email = applicantViewModel.Email,
                    ResumeSourceId = applicantViewModel.ResumeSourceId,
                    PositionAppliedId = applicantViewModel.PositionAppliedId,
                    TotalExperience = applicantViewModel.TotalExperience,
                    ReleventExperience = applicantViewModel.ReleventExperience,
                    NoticePeriod = applicantViewModel.NoticePeriod,
                    CurrentCTC = applicantViewModel.CurrentCTC,
                    ExpectedCTC = applicantViewModel.ExpectedCTC,
                    PreferredLocation = applicantViewModel.PreferredLocation,
                    ResumeText = applicantViewModel.ResumeText,
                    CreatedBy = applicantViewModel.CreatedBy,
                    ApplicantStatus=applicantViewModel.ApplicantStatus
                };


                List<HR_ApplicantEducation> educationList = new List<HR_ApplicantEducation>();
                if (educations != null && educations.Count > 0)
                {
                    foreach (ApplicantEducationViewModel item in educations)
                    {
                        educationList.Add(new HR_ApplicantEducation
                        {
                            EducationId = item.EducationId,
                            RegularDistant = item.RegularDistant,
                            BoardUniversityName = item.BoardUniversityName,
                            PercentageObtained = item.PercentageObtained
                        });
                    }
                }

                List<HR_ApplicantPrevEmployer> employerList = new List<HR_ApplicantPrevEmployer>();
                if (employers != null && employers.Count > 0)
                {
                    foreach (ApplicantPrevEmployerViewModel item in employers)
                    {
                        employerList.Add(new HR_ApplicantPrevEmployer
                        {
                            CurrentEmployer = item.CurrentEmployer,
                            EmployerName = item.EmployerName,
                            StartDate = Convert.ToDateTime(item.StartDate),
                            EndDate = Convert.ToDateTime(item.EndDate),
                            LastCTC = item.LastCTC,
                            ReasonOfLeaving = item.ReasonOfLeaving,
                            LastDesignationId = item.LastDesignationId,
                            EmploymentStatusId = item.EmploymentStatusId
                        });
                    }
                }

                List<HR_ApplicantProject> projectList = new List<HR_ApplicantProject>();
                if (projects != null && projects.Count > 0)
                {
                    foreach (ApplicantProjectViewModel item in projects)
                    {
                        projectList.Add(new HR_ApplicantProject
                        {
                            ProjectName = item.ProjectName,
                            ClientName = item.ClientName,
                            RoleDesc = item.RoleDesc,
                            TeamSize = item.TeamSize,
                            ProjectDesc = item.ProjectDesc,
                            TechnologiesUsed = item.TechnologiesUsed
                        });
                    }
                }

                HR_ApplicantExtraActivity extraActivityList = new HR_ApplicantExtraActivity
                {
                    Strength1 = extraActivities.Strength1,
                    Strength2 = extraActivities.Strength2,
                    Strength3 = extraActivities.Strength3,
                    Weakness1 = extraActivities.Weakness1,
                    Weakness2 = extraActivities.Weakness2,
                    Weakness3 = extraActivities.Weakness3,
                    Hobbies = extraActivities.Hobbies
                };

                responseOut = sqlDbInterface.AddEditApplicant(applicant,educationList,extraActivityList,employerList,projectList);
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
        public List<ApplicantEducationViewModel> GetApplicantEducationList(long applicantId)
        {
            List<ApplicantEducationViewModel> applicantEducations = new List<ApplicantEducationViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEducations = sqlDbInterface.GetApplicantEducationList(applicantId);
                if (dtEducations != null && dtEducations.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEducations.Rows)
                    {
                        applicantEducations.Add(new ApplicantEducationViewModel
                        {
                            EducationSequenceNo = Convert.ToInt32(dr["EducationSequenceNo"]),
                            ApplicantEducationId = Convert.ToInt32(dr["ApplicantEducationId"]),
                            EducationId = Convert.ToInt32(dr["EducationId"]),
                            EducationName = Convert.ToString(dr["EducationName"]),
                            RegularDistant = Convert.ToString(dr["RegularDistant"]),
                            BoardUniversityName = Convert.ToString(dr["BoardUniversityName"]),
                            PercentageObtained = Convert.ToDecimal(dr["PercentageObtained"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return applicantEducations;
        }
        public List<ApplicantPrevEmployerViewModel> GetApplicantPrevEmployerList(long applicantId)
        {
            List<ApplicantPrevEmployerViewModel> applicantEmployers = new List<ApplicantPrevEmployerViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployers = sqlDbInterface.GetApplicantPrevEmployerList(applicantId);
                if (dtEmployers != null && dtEmployers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployers.Rows)
                    {
                        applicantEmployers.Add(new ApplicantPrevEmployerViewModel
                        {
                            EmployerSequenceNo = Convert.ToInt32(dr["EmployerSequenceNo"]),
                            ApplicantPrevEmployerId = Convert.ToInt32(dr["ApplicantPrevEmployerId"]),
                            CurrentEmployer = Convert.ToBoolean(dr["CurrentEmployer"]),
                            EmployerName = Convert.ToString(dr["EmployerName"]),
                            StartDate = Convert.ToString(dr["StartDate"]),
                            EndDate = Convert.ToString(dr["EndDate"]),
                            LastCTC = Convert.ToDecimal(dr["LastCTC"]),
                            ReasonOfLeaving = Convert.ToString(dr["ReasonOfLeaving"]),
                            LastDesignationId = Convert.ToInt32(dr["LastDesignationId"]),
                            LastDesignationName = Convert.ToString(dr["DesignationName"]),
                            EmploymentStatusId = Convert.ToInt32(dr["EmploymentStatusId"]),
                            EmploymentStatusName = Convert.ToString(dr["EmploymentStatusName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return applicantEmployers;
        }
        public List<ApplicantProjectViewModel> GetApplicantProjectList(long applicantId)
        {
            List<ApplicantProjectViewModel> applicantProjects = new List<ApplicantProjectViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtEmployers = sqlDbInterface.GetApplicantProjectList(applicantId);
                if (dtEmployers != null && dtEmployers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEmployers.Rows)
                    {
                        applicantProjects.Add(new ApplicantProjectViewModel
                        {
                            ProjectSequenceNo = Convert.ToInt32(dr["ProjectSequenceNo"]),
                            ApplicantProjectId = Convert.ToInt32(dr["ApplicantProjectId"]),
                            ProjectName = Convert.ToString(dr["ProjectName"]),
                            ClientName = Convert.ToString(dr["ClientName"]),
                            RoleDesc = Convert.ToString(dr["RoleDesc"]),
                            TeamSize = Convert.ToInt32(dr["TeamSize"]),
                            ProjectDesc = Convert.ToString(dr["ProjectDesc"]),
                            TechnologiesUsed = Convert.ToString(dr["TechnologiesUsed"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return applicantProjects;
        }
        public List<ApplicantViewModel> GetApplicantList(string applicantNo, string projectNo, string firstName, string lastName, Int32 resumeSource, Int32 designation, string fromDate, string toDate, int companyId, string applicantStatus = "Final")
        {
            List<ApplicantViewModel> applicants = new List<ApplicantViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtApplicants = sqlDbInterface.GetApplicantList(applicantNo, projectNo, firstName, lastName, resumeSource,  designation, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate),  companyId, applicantStatus);
                if (dtApplicants != null && dtApplicants.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtApplicants.Rows)
                    {
                        applicants.Add(new ApplicantViewModel
                        {
                            ApplicantId = Convert.ToInt32(dr["ApplicantId"]),
                            ApplicantNo = Convert.ToString(dr["ApplicantNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            JobOpeningNo = Convert.ToString(dr["JobOpeningNo"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            ProjectNo = Convert.ToString(dr["ProjectNo"]),
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            FatherSpouseName = Convert.ToString(dr["FatherSpouseName"]),
                            ApplicantAddress = Convert.ToString(dr["ApplicantAddress"]),
                            ResumeSourceName = Convert.ToString(dr["ResumeSourceName"]),
                            PositionAppliedName = Convert.ToString(dr["DesignationName"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ApplicantStatus = Convert.ToString(dr["ApplicantStatus"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return applicants;
        }
        public ApplicantViewModel GetApplicantDetail(long applicantId = 0)
        {
            ApplicantViewModel applicant = new ApplicantViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtApplicant = sqlDbInterface.GetApplicantDetail(applicantId);
                if (dtApplicant != null && dtApplicant.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtApplicant.Rows)
                    {
                        applicant = new ApplicantViewModel
                        {
                            ApplicantId = Convert.ToInt32(dr["ApplicantId"]),
                            ApplicantNo = Convert.ToString(dr["ApplicantNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            JobOpeningId = Convert.ToInt32(dr["JobOpeningId"]),
                            JobOpeningNo = Convert.ToString(dr["JobOpeningNo"]),
                            CompanyBranchId = Convert.ToInt32(dr["CompanyBranchId"]),
                            ProjectNo = Convert.ToString(dr["ProjectNo"]),
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            FatherSpouseName = Convert.ToString(dr["FatherSpouseName"]),
                            DOB = Convert.ToString(dr["DOB"]),
                            BloodGroup = Convert.ToString(dr["BloodGroup"]),
                            MaritalStatus = Convert.ToString(dr["MaritalStatus"]),
                            ApplicantAddress = Convert.ToString(dr["ApplicantAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Email = Convert.ToString(dr["Email"]),
                            ResumeSourceId = Convert.ToInt32(dr["ResumeSourceId"]),
                            PositionAppliedId = Convert.ToInt32(dr["PositionAppliedId"]),
                            TotalExperience = Convert.ToDecimal(dr["TotalExperience"]),
                            ReleventExperience = Convert.ToDecimal(dr["ReleventExperience"]),
                            NoticePeriod = Convert.ToInt16(dr["NoticePeriod"]),
                            CurrentCTC = Convert.ToInt16(dr["CurrentCTC"]),
                            ExpectedCTC = Convert.ToInt16(dr["ExpectedCTC"]),
                            PreferredLocation = Convert.ToString(dr["PreferredLocation"]),
                            ResumeText = Convert.ToString(dr["ResumeText"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ApplicantStatus = Convert.ToString(dr["ApplicantStatus"]),
                            ApplicantStortlistStatus = Convert.ToString(dr["ApplicantStortlistStatus"])
                            
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return applicant;
        }
        public ApplicantExtraActivityViewModel GetApplicantExtraActivityDetail(long applicantId = 0)
        {
            ApplicantExtraActivityViewModel applicantActivity = new ApplicantExtraActivityViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtApplicant = sqlDbInterface.GetApplicantExtraActivityDetail(applicantId);
                if (dtApplicant != null && dtApplicant.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtApplicant.Rows)
                    {
                        applicantActivity = new ApplicantExtraActivityViewModel
                        {
                            Strength1 = Convert.ToString(dr["Strength1"]),
                            Strength2 = Convert.ToString(dr["Strength2"]),
                            Strength3 = Convert.ToString(dr["Strength3"]),
                            Weakness1 = Convert.ToString(dr["Weakness1"]),
                            Weakness2 = Convert.ToString(dr["Weakness2"]),
                            Weakness3 = Convert.ToString(dr["Weakness3"]),
                            Hobbies = Convert.ToString(dr["Hobbies"])

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return applicantActivity;
        }
        #endregion
        #region Job Applicant Shortlist
        public List<ApplicantViewModel> GetShortlistApplicantList(string applicantNo, string projectNo, string firstName, string lastName, Int32 resumeSource, Int32 designation, string fromDate, string toDate, int companyId, string applicantShortlistStatus = "Shortlist")
        {
            List<ApplicantViewModel> applicants = new List<ApplicantViewModel>();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtApplicants = sqlDbInterface.GetShortlistApplicantList(applicantNo, projectNo, firstName, lastName, resumeSource, designation, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId, applicantShortlistStatus);
                if (dtApplicants != null && dtApplicants.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtApplicants.Rows)
                    {
                        applicants.Add(new ApplicantViewModel
                        {
                            ApplicantId = Convert.ToInt32(dr["ApplicantId"]),
                            ApplicantNo = Convert.ToString(dr["ApplicantNo"]),
                            ApplicationDate = Convert.ToString(dr["ApplicationDate"]),
                            JobOpeningNo = Convert.ToString(dr["JobOpeningNo"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),
                            ProjectNo = Convert.ToString(dr["ProjectNo"]),
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            FatherSpouseName = Convert.ToString(dr["FatherSpouseName"]),
                            ApplicantAddress = Convert.ToString(dr["ApplicantAddress"]),
                            ResumeSourceName = Convert.ToString(dr["ResumeSourceName"]),
                            PositionAppliedName = Convert.ToString(dr["DesignationName"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            ApplicantStortlistStatus = Convert.ToString(dr["ApplicantStortlistStatus"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return applicants;
        }
        public ResponseOut ShortlistApplicant(ApplicantViewModel applicantViewModel, ApplicantVerificationViewModel verificationViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                HR_Applicant applicant = new HR_Applicant
                {
                    ApplicantId = Convert.ToInt32(applicantViewModel.ApplicantId),
                    CreatedBy = applicantViewModel.CreatedBy,
                    ApplicantStortlistStatus = applicantViewModel.ApplicantStortlistStatus
                };

                HR_ApplicantVerification verification = new HR_ApplicantVerification
                {
                    VerificationAgencyId = verificationViewModel.VerificationAgencyId,
                    VerificationDate = Convert.ToDateTime(verificationViewModel.VerificationDate),
                    VerificationCharges = verificationViewModel.VerificationCharges,
                    VerificationStatus = verificationViewModel.VerificationStatus,
                    Remarks = verificationViewModel.Remarks
                };

                responseOut = sqlDbInterface.ShortlistApplicant(applicant, verification);
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
        public ApplicantVerificationViewModel GetApplicantVerificationDetail(long applicantId = 0)
        {
            ApplicantVerificationViewModel applicantVerification= new ApplicantVerificationViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtApplicant = sqlDbInterface.GetApplicantVerificationDetail(applicantId);
                if (dtApplicant != null && dtApplicant.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtApplicant.Rows)
                    {
                        applicantVerification = new ApplicantVerificationViewModel
                        {
                            VerificationAgencyId = Convert.ToInt32(dr["VerificationAgencyId"]),
                            VerificationDate = Convert.ToString(dr["VerificationDate"]),
                            VerificationCharges = Convert.ToDecimal(dr["VerificationCharges"]),
                            VerificationStatus = Convert.ToString(dr["VerificationStatus"]),
                            Remarks = Convert.ToString(dr["Remarks"])

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return applicantVerification;
        }
        #endregion
    }
}
