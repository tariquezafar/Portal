using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Portal.DAL.Infrastructure;
using System.Data.Entity.SqlServer;

namespace Portal.DAL
{
    /// <summary>
    /// Class to Provide Services of DB
    /// </summary>
    public partial class HRMSDBInterface : IDisposable
    {
        private readonly HRMSEntities entities = new HRMSEntities();

        public HRMSDBInterface()
        {

        }
        #region Dispose Methods
        public void Dispose()
        {
            try
            {
                entities.Dispose();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region ClaimType
        public ResponseOut AddEditClaimType(HR_ClaimType claimType)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_ClaimType.Any(x => x.ClaimTypeName == claimType.ClaimTypeName && x.ClaimTypeId != claimType.ClaimTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateClaimTypeName;
                }
                else if (entities.HR_ClaimType.Any(x => x.ClaimNature == claimType.ClaimNature && x.ClaimTypeId != claimType.ClaimTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateClaimNature;
                }
                else
                {
                    if (claimType.ClaimTypeId == 0)
                    {
                        entities.HR_ClaimType.Add(claimType);
                        responseOut.message = ActionMessage.ClaimTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_ClaimType.Where(a => a.ClaimTypeId == claimType.ClaimTypeId).ToList().ForEach(a =>
                        {
                            a.ClaimTypeId = claimType.ClaimTypeId;
                            a.ClaimTypeName = claimType.ClaimTypeName;
                            a.ClaimNature = claimType.ClaimNature;
                            a.Status = claimType.Status;
                        });
                        responseOut.message = ActionMessage.ClaimTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<HR_ClaimType> GetClaimTypeList()
        {
            List<HR_ClaimType> claimTypeList = new List<HR_ClaimType>();
            try
            {
                var claimTypes = entities.HR_ClaimType.Where(x => x.Status == true).Select(s => new
                {
                    ClaimTypeId = s.ClaimTypeId,
                    ClaimTypeName = s.ClaimTypeName
                }).ToList();
                if (claimTypes != null && claimTypes.Count > 0)
                {
                    foreach (var item in claimTypes)
                    {
                        claimTypeList.Add(new HR_ClaimType { ClaimTypeId = item.ClaimTypeId, ClaimTypeName = item.ClaimTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return claimTypeList;
        }

        #endregion

        #region LeaveType
        public ResponseOut AddEditLeaveType(HR_LeaveType hR_LeaveType)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_LeaveType.Any(x => x.LeaveTypeName == hR_LeaveType.LeaveTypeName && x.LeaveTypeId != hR_LeaveType.LeaveTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateLeaveTypeName;
                }
                else if (entities.HR_LeaveType.Any(x => x.LeaveTypeCode == hR_LeaveType.LeaveTypeCode && x.LeaveTypeId != hR_LeaveType.LeaveTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateLeaveCode;
                }
                else
                {
                    if (hR_LeaveType.LeaveTypeId == 0)
                    {
                        entities.HR_LeaveType.Add(hR_LeaveType);
                        responseOut.message = ActionMessage.LeaveTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_LeaveType.Where(a => a.LeaveTypeId == hR_LeaveType.LeaveTypeId).ToList().ForEach(a =>
                        {
                            a.LeaveTypeId = hR_LeaveType.LeaveTypeId;
                            a.LeaveTypeName = hR_LeaveType.LeaveTypeName;
                            a.LeaveTypeCode = hR_LeaveType.LeaveTypeCode;
                            a.LeavePeriod = hR_LeaveType.LeavePeriod;
                            a.PayPeriod = hR_LeaveType.PayPeriod;
                            a.WorkPeriod = hR_LeaveType.WorkPeriod;
                            a.Status = hR_LeaveType.Status;
                        });
                        responseOut.message = ActionMessage.LeaveTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        #endregion
        #region CTC
        public ResponseOut AddEditCTC(HR_CTC hR_CTC)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                //if (entities.HR_LeaveType.Any(x => x.LeaveTypeName == hR_LeaveType.LeaveTypeName && x.LeaveTypeId != hR_LeaveType.LeaveTypeId))
                //{
                //    responseOut.status = ActionStatus.Fail;
                //    responseOut.message = ActionMessage.DuplicateLeaveTypeName;
                //}
                //else if (entities.HR_LeaveType.Any(x => x.LeaveTypeCode == hR_LeaveType.LeaveTypeCode && x.LeaveTypeId != hR_LeaveType.LeaveTypeId))
                //{
                //    responseOut.status = ActionStatus.Fail;
                //    responseOut.message = ActionMessage.DuplicateLeaveCode;
                //}
                //else
                //{
                    if (hR_CTC.CTCId == 0)
                    {
                        hR_CTC.CreatedDate = DateTime.Now;
                        entities.HR_CTC.Add(hR_CTC);
                        responseOut.message = ActionMessage.CTCCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_CTC.Where(a => a.CTCId == hR_CTC.CTCId).ToList().ForEach(a =>
                        {
                            a.CTCId = hR_CTC.CTCId;
                            a.DesignationId = hR_CTC.DesignationId;
                            a.Basic = hR_CTC.Basic;
                            a.HRAPerc = hR_CTC.HRAPerc;
                            a.HRAAmount = hR_CTC.HRAAmount;
                            a.Conveyance = hR_CTC.Conveyance;

                            a.Medical = hR_CTC.Medical;
                            a.ChildEduAllow = hR_CTC.ChildEduAllow;
                            a.LTA = hR_CTC.LTA;
                            a.SpecialAllow = hR_CTC.SpecialAllow;

                            a.OtherAllow = hR_CTC.OtherAllow;
                            a.GrossSalary = hR_CTC.GrossSalary;
                            a.EmployeePF = hR_CTC.EmployeePF;
                            a.EmployeeESI = hR_CTC.EmployeeESI;
                            a.ProfessionalTax = hR_CTC.ProfessionalTax;
                            a.NetSalary = hR_CTC.NetSalary;
                            a.EmployerPF = hR_CTC.EmployerPF;
                            a.EmployerESI = hR_CTC.EmployerESI;

                            a.MonthlyCTC = hR_CTC.MonthlyCTC;
                            a.YearlyCTC = hR_CTC.YearlyCTC;

                            a.ModifiedBy = hR_CTC.CreatedBy;
                            a.ModifiedDate = DateTime.Now;

                            a.Status = hR_CTC.Status;
                        });
                        responseOut.message = ActionMessage.CTCUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                //}
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
       
        #endregion
        #region Education
        public ResponseOut AddEditEducation(HR_Education education)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_Education.Any(x => x.EducationName == education.EducationName && x.EducationId != education.EducationId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateEducation;
                }               
                else
                {
                    if (education.EducationId == 0)
                    {
                        entities.HR_Education.Add(education);
                        responseOut.message = ActionMessage.EducationCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_Education.Where(a => a.EducationId == education.EducationId).ToList().ForEach(a =>
                        {
                            a.EducationId = education.EducationId;
                            a.EducationName = education.EducationName;                         
                            a.Status = education.Status;
                        });
                        responseOut.message = ActionMessage.EducationUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<HR_Education> GetEducationList()
        {
            List<HR_Education> educationList = new List<HR_Education>();
            try
            {
                var educations = entities.HR_Education.Where(x => x.Status == true).Select(s => new
                {
                    EducationId = s.EducationId,
                    EducationName = s.EducationName
                }).ToList();
                if (educations != null && educations.Count > 0)
                {
                    foreach (var item in educations)
                    {
                        educationList.Add(new HR_Education { EducationId = item.EducationId, EducationName = item.EducationName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return educationList;
        }
        #endregion
        #region Religion
        public ResponseOut AddEditReligion(HR_Religion religion)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_Religion.Any(x => x.ReligionName == religion.ReligionName && x.ReligionId != religion.ReligionId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateReligion;
                }
                else
                {
                    if (religion.ReligionId == 0)
                    {
                        entities.HR_Religion.Add(religion);
                        responseOut.message = ActionMessage.ReligionCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_Religion.Where(a => a.ReligionId == religion.ReligionId).ToList().ForEach(a =>
                        {
                            a.ReligionId = religion.ReligionId;
                            a.ReligionName = religion.ReligionName;
                            a.Status = religion.Status;
                        });
                        responseOut.message = ActionMessage.ReligionUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        
        #endregion
        #region Language
        public ResponseOut AddEditLanguage(HR_Language language)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_Language.Any(x => x.LanguageName == language.LanguageName && x.LanguageId != language.LanguageId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateLanguage;
                }
                else
                {
                    if (language.LanguageId == 0)
                    {
                        entities.HR_Language.Add(language);
                        responseOut.message = ActionMessage.LanguageCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_Language.Where(a => a.LanguageId == language.LanguageId).ToList().ForEach(a =>
                        {
                            a.LanguageId = language.LanguageId;
                            a.LanguageName = language.LanguageName;
                            a.Status = language.Status;
                        });
                        responseOut.message = ActionMessage.LanguageUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
    
        #endregion
        #region ShiftType
        public ResponseOut AddEditShfitType(HR_ShiftType shiftType)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_ShiftType.Any(x => x.ShiftTypeName == shiftType.ShiftTypeName && x.ShiftTypeId != shiftType.ShiftTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateShiftType;
                }
                else
                {
                    if (shiftType.ShiftTypeId == 0)
                    {
                        entities.HR_ShiftType.Add(shiftType);
                        responseOut.message = ActionMessage.ShiftTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_ShiftType.Where(a => a.ShiftTypeId == shiftType.ShiftTypeId).ToList().ForEach(a =>
                        {
                            a.ShiftTypeId = shiftType.ShiftTypeId;
                            a.ShiftTypeName = shiftType.ShiftTypeName;
                            a.Status = shiftType.Status;
                        });
                        responseOut.message = ActionMessage.ShiftTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        public List<HR_ShiftType> GetHRShiftTypeList()
        {
            List<HR_ShiftType> shiftTypeList = new List<HR_ShiftType>();
            try
            {
                var hRshiftTypeList = entities.HR_ShiftType.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.ShiftTypeName)).ThenBy(x => x.ShiftTypeName).Select(s => new
                {
                    ShiftTypeId = s.ShiftTypeId,
                    ShiftTypeName = s.ShiftTypeName

                }).ToList();
                if (hRshiftTypeList != null && hRshiftTypeList.Count > 0)
                {
                    foreach (var item in hRshiftTypeList)
                    {
                        shiftTypeList.Add(new HR_ShiftType { ShiftTypeId = item.ShiftTypeId, ShiftTypeName = item.ShiftTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return shiftTypeList;
        }

        #endregion
        #region Resource Requisition
        public List<HR_PositionLevel> GetPositionLevelList()
        {
            List<HR_PositionLevel> positionLevelList = new List<HR_PositionLevel>();
            try
            {
                var positionlevels = entities.HR_PositionLevel.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.PositionLevelName)).ThenBy(x => x.PositionLevelName).Select(s => new
                {
                    PositionLevelId = s.PositionLevelId,
                    PositionLevelName = s.PositionLevelName
                }).ToList();
                if (positionlevels != null && positionlevels.Count > 0)
                {
                    foreach (var item in positionlevels)
                    {
                        positionLevelList.Add(new HR_PositionLevel { PositionLevelId = item.PositionLevelId, PositionLevelName = item.PositionLevelName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return positionLevelList;
        }

        public List<HR_InterviewType> GetInterviewTypeList()
        {
            List<HR_InterviewType> interviewtypeList = new List<HR_InterviewType>();
            try
            {
                var interviewtypes = entities.HR_InterviewType.Where(x => x.Status == true).Select(s => new
                {
                    InterviewTypeId = s.InterviewTypeId,
                    InterviewTypeName = s.InterviewTypeName
                }).ToList();
                if (interviewtypes != null && interviewtypes.Count > 0)
                {
                    foreach (var item in interviewtypes)
                    {
                        interviewtypeList.Add(new HR_InterviewType { InterviewTypeId = item.InterviewTypeId, InterviewTypeName = item.InterviewTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return interviewtypeList;
        }




        public List<HR_PositionType> GetPositionTypeList()
        {
            List<HR_PositionType> positionTypeList = new List<HR_PositionType>();
            try
            {
                var positiontypes = entities.HR_PositionType.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.PositionTypeName)).ThenBy(x => x.PositionTypeName).Select(s => new
                {
                    PositionTypeId = s.PositionTypeId,
                    PositionTypeName = s.PositionTypeName
                }).ToList();
                if (positiontypes != null && positiontypes.Count > 0)
                {
                    foreach (var item in positiontypes)
                    {
                        positionTypeList.Add(new HR_PositionType { PositionTypeId = item.PositionTypeId, PositionTypeName = item.PositionTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return positionTypeList;
        }
        public List<HR_Skills> GetSkillAutoCompleteList(string searchTerm)
        {
            List<HR_Skills> skillList = new List<HR_Skills>();
            try
            {
                var skills = (from p in entities.HR_Skills 
                          
                              where (p.SkillName.ToLower().Contains(searchTerm.ToLower()) || p.SkillCode.ToLower().Contains(searchTerm.ToLower())) && p.Status == true
                                select new
                                {
                                    SkillId = p.SkillId,
                                    SkillName = p.SkillName,
                                    SkillCode = p.SkillCode,
                                   

                                }).ToList();


                if (skills != null && skills.Count > 0)
                {
                    foreach (var item in skills)
                    {
                        skillList.Add(new HR_Skills { SkillId = item.SkillId, SkillName = item.SkillName, SkillCode = item.SkillCode});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return skillList;


             
        }


        public ResponseOut ApproveRejectResourceRequisition(HR_ResourceRequisition requisition)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (requisition.ApprovalStatus == "Rejected")
                {
                    entities.HR_ResourceRequisition.Where(a => a.RequisitionId== requisition.RequisitionId).ToList().ForEach(a =>
                    {
                        a.RequisitionStatus = requisition.ApprovalStatus;
                        a.RejectedBy = requisition.CreatedBy;
                        a.RejectedDate = DateTime.Now;
                        a.RejectedReason = requisition.RejectedReason;
                        a.ApprovalStatus= requisition.ApprovalStatus;
                    });
                    responseOut.message = ActionMessage.ResourceRequisitionRejectSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
                else if (requisition.ApprovalStatus == "Approved")
                {
                    entities.HR_ResourceRequisition.Where(a => a.RequisitionId == requisition.RequisitionId).ToList().ForEach(a =>
                    {
                        a.RequisitionStatus = requisition.ApprovalStatus;
                        a.ApprovedBy = requisition.CreatedBy;
                        a.ApprovedDate = DateTime.Now;
                        a.ApprovalStatus = requisition.ApprovalStatus;
                    });
                    responseOut.message = ActionMessage.ResourceRequisitionApproveSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }

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
        #endregion

        #region PositionType
        public ResponseOut AddEditPositionType(HR_PositionType positiontype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_PositionType.Any(x => x.PositionTypeName == positiontype.PositionTypeName && x.PositionTypeId != positiontype.PositionTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicatePositionTypeName;
                }
                else if (entities.HR_PositionType.Any(x => x.PositionTypeCode == positiontype.PositionTypeCode && x.PositionTypeId != positiontype.PositionTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicatePositionTypeCode;
                }
                else
                {
                    if (positiontype.PositionTypeId == 0)
                    {
                        entities.HR_PositionType.Add(positiontype);
                        responseOut.message = ActionMessage.PositionTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_PositionType.Where(a => a.PositionTypeId == positiontype.PositionTypeId).ToList().ForEach(a =>
                        {
                            a.PositionTypeId = positiontype.PositionTypeId;
                            a.PositionTypeName = positiontype.PositionTypeName;
                            a.PositionTypeCode = positiontype.PositionTypeCode;
                            a.Status = positiontype.Status;
                        });
                        responseOut.message = ActionMessage.PositionTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        
        #endregion 

        #region PositionLevel
        public ResponseOut AddEditPositionLevel(HR_PositionLevel positionlevel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_PositionLevel.Any(x => x.PositionLevelName == positionlevel.PositionLevelName && x.PositionLevelId != positionlevel.PositionLevelId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicatePositionLevelName;
                }
                else if (entities.HR_PositionLevel.Any(x => x.PositionLevelCode == positionlevel.PositionLevelCode && x.PositionLevelId != positionlevel.PositionLevelId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicatePositionLevelCode;
                }
                else
                {
                    if (positionlevel.PositionLevelId == 0)
                    {
                        entities.HR_PositionLevel.Add(positionlevel);
                        responseOut.message = ActionMessage.PositionLevelCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_PositionLevel.Where(a => a.PositionLevelId == positionlevel.PositionLevelId).ToList().ForEach(a =>
                        {
                            a.PositionLevelId = positionlevel.PositionLevelId;
                            a.PositionLevelName = positionlevel.PositionLevelName;
                            a.PositionLevelCode = positionlevel.PositionLevelCode;
                            a.Status = positionlevel.Status;
                        });
                        responseOut.message = ActionMessage.PositionLevelUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #endregion

        #region InterviewType
        public ResponseOut AddEditInterviewType(HR_InterviewType interviewtype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_InterviewType.Any(x => x.InterviewTypeName == interviewtype.InterviewTypeName && x.InterviewTypeId != interviewtype.InterviewTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateInterviewTypeName;
                } 
                else
                {
                    if (interviewtype.InterviewTypeId == 0)
                    {
                        entities.HR_InterviewType.Add(interviewtype);
                        responseOut.message = ActionMessage.InterviewTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_InterviewType.Where(a => a.InterviewTypeId == interviewtype.InterviewTypeId).ToList().ForEach(a =>
                        {
                            a.InterviewTypeId = interviewtype.InterviewTypeId;
                            a.InterviewTypeName = interviewtype.InterviewTypeName; 
                            a.Status = interviewtype.Status;
                        });
                        responseOut.message = ActionMessage.InterviewTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #endregion

        #region Calender
        public ResponseOut AddEditCalender(HR_Calender calender)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_Calender.Any(x => x.CalenderName == calender.CalenderName && x.CalenderId != calender.CalenderId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCalenderName;
                }
                else if (entities.HR_Calender.Any(x => x.CalenderYear == calender.CalenderYear && x.CalenderId != calender.CalenderId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateCalenderYear;
                }
                else
                {
                    if (calender.CalenderId == 0)
                    {
                        calender.CreatedDate = DateTime.Now;
                        entities.HR_Calender.Add(calender);
                        responseOut.message = ActionMessage.CalenderCreatedSuccess;
                    }
                    else
                    {
                        calender.ModifiedBy = calender.CreatedBy;
                        calender.ModifiedDate = DateTime.Now;
                        entities.HR_Calender.Where(a => a.CalenderId == calender.CalenderId).ToList().ForEach(a =>
                        {
                            a.CalenderId = calender.CalenderId;
                            a.CalenderName = calender.CalenderName;
                            a.CalenderYear = calender.CalenderYear;
                            a.CreatedBy = calender.CreatedBy;
                            a.ModifiedBy = calender.ModifiedBy;
                            a.ModifiedDate = calender.ModifiedDate;
                            a.Status = calender.Status;
                        });
                        responseOut.message = ActionMessage.CalenderUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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



        public List<HR_Calender> GetCalenderList()
        {
            List<HR_Calender> calenderList = new List<HR_Calender>();
            try
            {
                var calenders = entities.HR_Calender.Where(x => x.Status == true).Select(s => new
                {
                     CalenderId = s.CalenderId,
                    CalenderName = s.CalenderName,
                    CalenderYear = s.CalenderYear
                }).ToList();
                if (calenders != null && calenders.Count > 0)
                {
                    foreach (var item in calenders)
                    {
                        calenderList.Add(new HR_Calender { CalenderId = item.CalenderId, CalenderName = item.CalenderName, CalenderYear = item.CalenderYear });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return calenderList;
        }





        #endregion

        #region Activity Calender
        public ResponseOut AddEditActivityCalender(HR_ActivityCalender activitycalender)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            { 
                    if (activitycalender.ActivityCalenderId == 0)
                    {
                        entities.HR_ActivityCalender.Add(activitycalender);
                        responseOut.message = ActionMessage.ActivityCalenderCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_ActivityCalender.Where(a => a.ActivityCalenderId == activitycalender.ActivityCalenderId).ToList().ForEach(a =>
                        {
                            a.ActivityCalenderId = activitycalender.ActivityCalenderId;
                            a.ActivityDate = activitycalender.ActivityDate;
                            a.ActivityDescription = activitycalender.ActivityDescription;
                            a.CalenderId = activitycalender.CalenderId;
                            a.Status = activitycalender.Status;
                            a.CompanyBranchId = activitycalender.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.ActivityCalenderUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                 
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
        #endregion

        #region HolidayType
        public ResponseOut AddEditHolidayType(HR_HolidayType holidaytype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_HolidayType.Any(x => x.HolidayTypeName == holidaytype.HolidayTypeName && x.HolidayTypeId != holidaytype.HolidayTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateHolidayTypeName;
                }
                else
                {
                    if (holidaytype.HolidayTypeId == 0)
                    {
                        entities.HR_HolidayType.Add(holidaytype);
                        responseOut.message = ActionMessage.HolidayTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_HolidayType.Where(a => a.HolidayTypeId == holidaytype.HolidayTypeId).ToList().ForEach(a =>
                        {
                            a.HolidayTypeId = holidaytype.HolidayTypeId;
                            a.HolidayTypeName = holidaytype.HolidayTypeName;
                            a.Status = holidaytype.Status;
                            a.CompanyBranchId = holidaytype.CompanyBranchId;
                        });
                        responseOut.message = ActionMessage.HolidayTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #endregion
        #region Holiday Calender
        public List<HR_HolidayType> GetHolidayTypeIdList(int companyBranchId)
        {
            List<HR_HolidayType> holidaytypeList = new List<HR_HolidayType>();
            try
            {
                var holidayTypes = entities.HR_HolidayType.Where(x => x.Status == true && x.CompanyBranchId==companyBranchId).Select(s => new
                {
                    HolidayTypeId = s.HolidayTypeId,
                    HolidayTypeName = s.HolidayTypeName,
                    Status = s.Status
                }).ToList();
                if (holidayTypes != null && holidayTypes.Count > 0)
                {
                    foreach (var item in holidayTypes)
                    {
                        holidaytypeList.Add(new HR_HolidayType { HolidayTypeId = item.HolidayTypeId, HolidayTypeName = item.HolidayTypeName, Status = item.Status});
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return holidaytypeList;
        }

        public ResponseOut AddEditHolidayCalender(HR_HolidayCalender holidaycalender)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (holidaycalender.HolidayCalenderId == 0)
                {
                    entities.HR_HolidayCalender.Add(holidaycalender);
                    responseOut.message = ActionMessage.HolidayCalenderCreatedSuccess;
                }
                else
                {
                    entities.HR_HolidayCalender.Where(a => a.HolidayCalenderId == holidaycalender.HolidayCalenderId).ToList().ForEach(a =>
                    {
                        a.HolidayCalenderId = holidaycalender.HolidayCalenderId;
                        a.HolidayDate = holidaycalender.HolidayDate;
                        a.HolidayDescription = holidaycalender.HolidayDescription;
                        a.CalenderId = holidaycalender.CalenderId;
                        a.HolidayTypeId = holidaycalender.HolidayTypeId;
                        a.Status = holidaycalender.Status;
                        a.CompanyBranchId = holidaycalender.CompanyBranchId;
                    });
                    responseOut.message = ActionMessage.HolidayCalenderUpdatedSuccess;
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;

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

        #endregion
        #region AssetType
        public ResponseOut AddEditAssetType(HR_AssetType assettype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_AssetType.Any(x => x.AssetTypeName == assettype.AssetTypeName && x.AssetTypeId != assettype.AssetTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateAssetTypeName;
                }
                else
                {
                    if (assettype.AssetTypeId == 0)
                    {
                        entities.HR_AssetType.Add(assettype);
                        responseOut.message = ActionMessage.AssetTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_AssetType.Where(a => a.AssetTypeId == assettype.AssetTypeId).ToList().ForEach(a =>
                        {
                            a.AssetTypeId = assettype.AssetTypeId;
                            a.AssetTypeName = assettype.AssetTypeName;
                            a.Status = assettype.Status;
                        });
                        responseOut.message = ActionMessage.AssetTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<HR_AssetType> GetEmployeeAssetApplicationTypeList()
        {
            List<HR_AssetType> employeeAssettypeList = new List<HR_AssetType>();
            try
            {
                var employeeAssetApplicationtypeList = entities.HR_AssetType.Where(x => x.Status == true).Select(s => new
                {
                    AssetTypeId = s.AssetTypeId,
                    AssetTypeName = s.AssetTypeName

                }).ToList();
                if (employeeAssetApplicationtypeList != null && employeeAssetApplicationtypeList.Count > 0)
                {
                    foreach (var item in employeeAssetApplicationtypeList)
                    {
                        employeeAssettypeList.Add(new HR_AssetType { AssetTypeId = item.AssetTypeId, AssetTypeName = item.AssetTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeAssettypeList;
        }
        #endregion

        #region TravelType
        public ResponseOut AddEditTravelType(HR_TravelType traveltype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_TravelType.Any(x => x.TravelTypeName == traveltype.TravelTypeName && x.TravelTypeId != traveltype.TravelTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateAssetTypeName;
                }
                else
                {
                    if (traveltype.TravelTypeId == 0)
                    {
                        entities.HR_TravelType.Add(traveltype);
                        responseOut.message = ActionMessage.TravelTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_TravelType.Where(a => a.TravelTypeId == traveltype.TravelTypeId).ToList().ForEach(a =>
                        {
                            a.TravelTypeId = traveltype.TravelTypeId;
                            a.TravelTypeName = traveltype.TravelTypeName;
                            a.Status = traveltype.Status;
                        });
                        responseOut.message = ActionMessage.TravelTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #endregion

        #region AdvanceType
        public ResponseOut AddEditAdvanceType(HR_AdvanceType advancetype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_AdvanceType.Any(x => x.AdvanceTypeName == advancetype.AdvanceTypeName && x.AdvanceTypeId != advancetype.AdvanceTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateAssetTypeName;
                }
                else
                {
                    if (advancetype.AdvanceTypeId == 0)
                    {
                        entities.HR_AdvanceType.Add(advancetype);
                        responseOut.message = ActionMessage.AdvanceTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_AdvanceType.Where(a => a.AdvanceTypeId == advancetype.AdvanceTypeId).ToList().ForEach(a =>
                        {
                            a.AdvanceTypeId = advancetype.AdvanceTypeId;
                            a.AdvanceTypeName = advancetype.AdvanceTypeName;
                            a.Status = advancetype.Status;
                        });
                        responseOut.message = ActionMessage.AdvanceTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        #endregion
    
        #region LoanType
        public ResponseOut AddEditLoanType(HR_LoanType loantype)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_LoanType.Any(x => x.LoanTypeName == loantype.LoanTypeName && x.LoanTypeId != loantype.LoanTypeId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateAssetTypeName;
                }
                else
                {
                    if (loantype.LoanTypeId == 0)
                    {
                        entities.HR_LoanType.Add(loantype);
                        responseOut.message = ActionMessage.LoanTypeCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_LoanType.Where(a => a.LoanTypeId == loantype.LoanTypeId).ToList().ForEach(a =>
                        {
                            a.LoanTypeId = loantype.LoanTypeId;
                            a.LoanTypeName = loantype.LoanTypeName;
                            a.LoanInterestRate = loantype.LoanInterestRate;
                            a.InterestCalcOn = loantype.InterestCalcOn;
                            a.Status = loantype.Status;
                        });
                        responseOut.message = ActionMessage.LoanTypeUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<HR_LoanType> GetEmployeeLoanApplicationTypeList()
        {
            List<HR_LoanType> employeeLoantypeList = new List<HR_LoanType>();
            try
            {
                var employeeLoanApplicationtypeList = entities.HR_LoanType.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.LoanTypeName)).ThenBy(x => x.LoanTypeName).Select(s => new
                {
                    LoanTypeId = s.LoanTypeId,
                    LoanTypeName = s.LoanTypeName

                }).ToList();
                if (employeeLoanApplicationtypeList != null && employeeLoanApplicationtypeList.Count > 0)
                {
                    foreach (var item in employeeLoanApplicationtypeList)
                    {
                        employeeLoantypeList.Add(new HR_LoanType { LoanTypeId = item.LoanTypeId, LoanTypeName = item.LoanTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return employeeLoantypeList;
        }
        #endregion

        #region Verification Agency
        public ResponseOut AddEditVerificationAgency(HR_VerificationAgency verificationagency)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_VerificationAgency.Any(x => x.VerificationAgencyName == verificationagency.VerificationAgencyName && x.VerificationAgencyId != verificationagency.VerificationAgencyId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateVerificationAgencyName;
                }
                else
                {
                    if (verificationagency.VerificationAgencyId == 0)
                    {
                        entities.HR_VerificationAgency.Add(verificationagency);
                        responseOut.message = ActionMessage.VerificationAgencyCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_VerificationAgency.Where(a => a.VerificationAgencyId == verificationagency.VerificationAgencyId).ToList().ForEach(a =>
                        {
                            a.VerificationAgencyId = verificationagency.VerificationAgencyId;
                            a.VerificationAgencyName = verificationagency.VerificationAgencyName;
                            a.Status = verificationagency.Status;
                        });
                        responseOut.message = ActionMessage.VerificationAgencyUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<HR_VerificationAgency> GetVerificationAgencyList()
        {
            List<HR_VerificationAgency> agencyList = new List<HR_VerificationAgency>();
            try
            {
                var agencies= entities.HR_VerificationAgency.Where(x => x.Status == true).Select(s => new
                {
                    VerificationAgencyId = s.VerificationAgencyId,
                    VerificationAgencyName = s.VerificationAgencyName

                }).ToList();
                if (agencies != null && agencies.Count > 0)
                {
                    foreach (var item in agencies)
                    {
                        agencyList.Add(new HR_VerificationAgency { VerificationAgencyId = item.VerificationAgencyId, VerificationAgencyName = item.VerificationAgencyName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return agencyList;
        }
        #endregion
        #region ResumeSource
        public ResponseOut AddEditResumeSource(HR_ResumeSource resumesource)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_ResumeSource.Any(x => x.ResumeSourceName == resumesource.ResumeSourceName && x.ResumeSourceId != resumesource.ResumeSourceId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateResumeSourceName;
                }
                else
                {
                    if (resumesource.ResumeSourceId == 0)
                    {
                        entities.HR_ResumeSource.Add(resumesource);
                        responseOut.message = ActionMessage.ResumeSourceCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_ResumeSource.Where(a => a.ResumeSourceId == resumesource.ResumeSourceId).ToList().ForEach(a =>
                        {
                            a.ResumeSourceId = resumesource.ResumeSourceId;
                            a.ResumeSourceName = resumesource.ResumeSourceName;
                            a.Status = resumesource.Status;
                        });
                        responseOut.message = ActionMessage.ResumeSourceUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<HR_ResumeSource> GetResumeSourceList()
        {
            List<HR_ResumeSource> resumeSourceList = new List<HR_ResumeSource>();
            try
            {
                var resumeSources = entities.HR_ResumeSource.Where(x => x.Status == true).Select(s => new
                {
                    ResumeSourceId = s.ResumeSourceId,
                    ResumeSourceName = s.ResumeSourceName
                }).ToList();
                if (resumeSources != null && resumeSources.Count > 0)
                {
                    foreach (var item in resumeSources)
                    {
                        resumeSourceList.Add(new HR_ResumeSource { ResumeSourceId = item.ResumeSourceId, ResumeSourceName = item.ResumeSourceName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return resumeSourceList;
        }

        #endregion

        #region Roaster
        public ResponseOut AddEditRoaster(HR_Roaster roaster)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_Roaster.Any(x => x.RoasterName == roaster.RoasterName && x.RoasterId != roaster.RoasterId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateRoasterName;
                }
                else
                {
                    if (roaster.RoasterId == 0)
                    {
                        roaster.CreatedDate = DateTime.Now;
                        entities.HR_Roaster.Add(roaster);
                        responseOut.message = ActionMessage.RoasterCreatedSuccess;
                    }
                    else
                    {
                       
                        entities.HR_Roaster.Where(a => a.RoasterId == roaster.RoasterId).ToList().ForEach(a =>
                        {
                            a.RoasterId = roaster.RoasterId;
                            a.RoasterName = roaster.RoasterName;
                            a.RoasterDesc = roaster.RoasterDesc;
                            a.RoasterStartDate = roaster.RoasterStartDate;
                            a.RoasterType = roaster.RoasterType;
                            a.DepartmentId = roaster.DepartmentId;
                            a.Remarks = roaster.Remarks;
                            a.NoOfWeeks = roaster.NoOfWeeks;
                            a.CompanyId = roaster.CompanyId;
                            a.ModifiedBy = roaster.CreatedBy;
                            a.ModifiedDate = DateTime.Now;
                            a.Status = roaster.Status;
                        });
                        responseOut.message = ActionMessage.RoasterUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<HR_Roaster> GetRosterList(int companyId)
        {
            List<HR_Roaster> rosters = new List<HR_Roaster>();
            try
            {
                var rosterList = entities.HR_Roaster.Where(x => x.CompanyId==companyId && x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.RoasterName)).ThenBy(x => x.RoasterName).Select(s => new
                {
                    RoasterId = s.RoasterId,
                    RoasterName = s.RoasterName
                }).ToList();
                if (rosterList != null && rosterList.Count > 0)
                {
                    foreach (var item in rosterList)
                    {
                        rosters.Add(new HR_Roaster { RoasterId= item.RoasterId , RoasterName= item.RoasterName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return rosters;
        }
        #endregion

        #region Approval Employee Asset Application
        public ResponseOut ApproveRejectEmployeeAssetApplication(HR_EmployeeAssetApplication employeeAssetApplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (employeeAssetApplication.ApplicationStatus == "Rejected")
                {
                    entities.HR_EmployeeAssetApplication.Where(a => a.ApplicationId == employeeAssetApplication.ApplicationId).ToList().ForEach(a =>
                    {
                        a.ApplicationStatus = employeeAssetApplication.ApplicationStatus;
                        a.RejectBy = employeeAssetApplication.ApproveBy;
                        a.RejectDate = DateTime.Now;
                        a.RejectReason = employeeAssetApplication.RejectReason;
                        a.CompanyBranchId = employeeAssetApplication.CompanyBranchId;
                    });
                    responseOut.message = ActionMessage.EmployeeAssetApplicationRejectionCreatedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
                else if (employeeAssetApplication.ApplicationStatus == "Approved")
                {
                    entities.HR_EmployeeAssetApplication.Where(a => a.ApplicationId == employeeAssetApplication.ApplicationId).ToList().ForEach(a =>
                    {
                        a.ApplicationStatus = employeeAssetApplication.ApplicationStatus;
                        a.ApproveBy = employeeAssetApplication.ApproveBy;
                        a.ApproveDate = DateTime.Now;
                        a.CompanyBranchId = employeeAssetApplication.CompanyBranchId;
                    });
                    responseOut.message = ActionMessage.EmployeeAssetApplicationApproveUpdatedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }

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
        #endregion

        #region Approval EmployeeLoanApplication
        public ResponseOut ApproveRejectEmployeeLoanApplication(HR_EmployeeLoanApplication employeeLoanApplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (employeeLoanApplication.LoanStatus == "Rejected")
                {
                    entities.HR_EmployeeLoanApplication.Where(a => a.ApplicationId == employeeLoanApplication.ApplicationId).ToList().ForEach(a =>
                    {
                        a.LoanStatus = employeeLoanApplication.LoanStatus;
                        a.RejectBy = employeeLoanApplication.ApproveBy;
                        a.RejectDate = DateTime.Now;
                        a.RejectReason = employeeLoanApplication.RejectReason;
                    });
                    responseOut.message = ActionMessage.EmployeeLoanApplicationRejectionCreatedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
                else if (employeeLoanApplication.LoanStatus == "Approved")
                {
                    entities.HR_EmployeeLoanApplication.Where(a => a.ApplicationId == employeeLoanApplication.ApplicationId).ToList().ForEach(a =>
                    {
                        a.LoanStatus = employeeLoanApplication.LoanStatus;
                        a.ApproveBy = employeeLoanApplication.ApproveBy;
                        a.ApproveDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.EmployeeLoanApplicationApproveUpdatedSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }

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
        #endregion

        #region Employee Advance Application
        public List<HR_AdvanceType> GetAdvanceTypeForEmpolyeeAdvanceAppList()
        {
            List<HR_AdvanceType> advancetypeList = new List<HR_AdvanceType>();
            try
            {
                var advancetypes = entities.HR_AdvanceType.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.AdvanceTypeName)).ThenBy(x => x.AdvanceTypeName).Select(s => new
                {
                    AdvanceTypeId = s.AdvanceTypeId,
                    AdvanceTypeName = s.AdvanceTypeName

                }).ToList();
                if (advancetypes != null && advancetypes.Count > 0)
                {
                    foreach (var item in advancetypes)
                    {
                        advancetypeList.Add(new HR_AdvanceType { AdvanceTypeId = item.AdvanceTypeId, AdvanceTypeName = item.AdvanceTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return advancetypeList;
        }

        public ResponseOut ApproveRejectEmployeeAdvanceApp(HR_EmployeeAdvanceApplication employeeAdvanceApp)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (employeeAdvanceApp.AdvanceStatus == "Rejected")
                {
                    entities.HR_EmployeeAdvanceApplication.Where(a => a.ApplicationId == employeeAdvanceApp.ApplicationId).ToList().ForEach(a =>
                    {
                        a.AdvanceStatus = employeeAdvanceApp.AdvanceStatus;
                        a.RejectBy = employeeAdvanceApp.ApproveBy;
                        a.RejectDate = DateTime.Now;
                        a.RejectReason = employeeAdvanceApp.RejectReason;
                    });
                    responseOut.message = ActionMessage.EmployeeAdvanceApplicationRejectSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
                else if (employeeAdvanceApp.AdvanceStatus == "Approved")
                {
                    entities.HR_EmployeeAdvanceApplication.Where(a => a.ApplicationId == employeeAdvanceApp.ApplicationId).ToList().ForEach(a =>
                    {
                        a.AdvanceStatus = employeeAdvanceApp.AdvanceStatus;
                        a.ApproveBy = employeeAdvanceApp.ApproveBy;
                        a.ApproveDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.EmployeeAdvanceApplicationApproveSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }

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


        #endregion

        #region Employee Travel Application
        public List<HR_TravelType> GetTravelTypeForEmpolyeeTravelAppList()
        {
            List<HR_TravelType> traveltypeList = new List<HR_TravelType>();
            try
            {
                var traveltypes = entities.HR_TravelType.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.TravelTypeName)).ThenBy(x => x.TravelTypeName).Select(s => new
                {
                    TravelTypeId = s.TravelTypeId,
                    TravelTypeName = s.TravelTypeName

                }).ToList();
                if (traveltypes != null && traveltypes.Count > 0)
                {
                    foreach (var item in traveltypes)
                    {
                        traveltypeList.Add(new HR_TravelType { TravelTypeId = item.TravelTypeId, TravelTypeName = item.TravelTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return traveltypeList;
        }

        public ResponseOut ApproveRejectEmployeeTravelApp(HR_EmployeeTravelApplication employeeTravelApp)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (employeeTravelApp.TravelStatus == "Rejected")
                {
                    entities.HR_EmployeeTravelApplication.Where(a => a.ApplicationId == employeeTravelApp.ApplicationId).ToList().ForEach(a =>
                    {
                        a.TravelStatus = employeeTravelApp.TravelStatus;
                        a.RejectBy = employeeTravelApp.ApproveBy;
                        a.RejectDate = DateTime.Now;
                        a.RejectReason = employeeTravelApp.RejectReason;
                    });
                    responseOut.message = ActionMessage.EmployeeTravelApplicationRejectSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
                else if (employeeTravelApp.TravelStatus == "Approved")
                {
                    entities.HR_EmployeeTravelApplication.Where(a => a.ApplicationId == employeeTravelApp.ApplicationId).ToList().ForEach(a =>
                    {
                        a.TravelStatus = employeeTravelApp.TravelStatus;
                        a.ApproveBy = employeeTravelApp.ApproveBy;
                        a.ApproveDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.EmployeeTravelApplicationApproveSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }

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


        #endregion

        #region Separation Clear List
        public ResponseOut AddEditSeparationClearList(HR_SeparationClearList separationclearlist)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_SeparationClearList.Any(x => x.SeparationClearListName == separationclearlist.SeparationClearListName && x.SeparationClearListId != separationclearlist.SeparationClearListId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSeparationClearListName;
                }
                else
                {
                    if (separationclearlist.SeparationClearListId == 0)
                    {
                        entities.HR_SeparationClearList.Add(separationclearlist);
                        responseOut.message = ActionMessage.SeparationClearListCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_SeparationClearList.Where(a => a.SeparationClearListId == separationclearlist.SeparationClearListId).ToList().ForEach(a =>
                        {
                            a.SeparationClearListId = separationclearlist.SeparationClearListId;
                            a.SeparationClearListName = separationclearlist.SeparationClearListName;
                            a.SeparationClearListDesc = separationclearlist.SeparationClearListDesc;
                            a.Status = separationclearlist.Status;
                        });
                        responseOut.message = ActionMessage.SeparationClearListUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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


        public List<HR_SeparationClearList> GetSeparationClearListForClearanceTemplate()
        {
            List<HR_SeparationClearList> separationClearList = new List<HR_SeparationClearList>();
            try
            {
                var separationClears = entities.HR_SeparationClearList.Where(x => x.Status == true).Select(s => new
                {
                    SeparationClearListId = s.SeparationClearListId,
                    SeparationClearListName = s.SeparationClearListName

                }).ToList();
                if (separationClears != null && separationClears.Count > 0)
                {
                    foreach (var item in separationClears)
                    {
                        separationClearList.Add(new HR_SeparationClearList { SeparationClearListId = item.SeparationClearListId, SeparationClearListName = item.SeparationClearListName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationClearList;
        }


        #endregion

        #region Separation Category
        public ResponseOut AddEditSeparationCategory(HR_SeparationCategory separationcategory)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_SeparationCategory.Any(x => x.SeparationCategoryName == separationcategory.SeparationCategoryName && x.SeparationCategoryId != separationcategory.SeparationCategoryId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateSeparationCategoryName;
                }
                else
                {
                    if (separationcategory.SeparationCategoryId == 0)
                    {
                        entities.HR_SeparationCategory.Add(separationcategory);
                        responseOut.message = ActionMessage.SeparationCategoryCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_SeparationCategory.Where(a => a.SeparationCategoryId == separationcategory.SeparationCategoryId).ToList().ForEach(a =>
                        {
                            a.SeparationCategoryId = separationcategory.SeparationCategoryId;
                            a.SeparationCategoryName = separationcategory.SeparationCategoryName;
                            a.status = separationcategory.status;
                        });
                        responseOut.message = ActionMessage.SeparationCategoryUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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


        public List<HR_SeparationCategory> GetSeparationCategoryForSeparationApplicationList()
        {
            List<HR_SeparationCategory> separationCategoryList = new List<HR_SeparationCategory>();
            try
            {
                var separationcategorys = entities.HR_SeparationCategory.Where(x => x.status == true).OrderBy(x => SqlFunctions.IsNumeric(x.SeparationCategoryName)).ThenBy(x => x.SeparationCategoryName).Select(s => new
                {
                    SeparationCategoryId = s.SeparationCategoryId,
                    SeparationCategoryName = s.SeparationCategoryName

                }).ToList();
                if (separationcategorys != null && separationcategorys.Count > 0)
                {
                    foreach (var item in separationcategorys)
                    {
                        separationCategoryList.Add(new HR_SeparationCategory { SeparationCategoryId = item.SeparationCategoryId, SeparationCategoryName = item.SeparationCategoryName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationCategoryList;
        }

        #endregion

        #region Employee Leave Application
        public List<HR_LeaveType> GetLeaveTypeForEmpolyeeLeaveAppList()
        {
            List<HR_LeaveType> leavetypeList = new List<HR_LeaveType>();
            try
            {
                var leavetypes = entities.HR_LeaveType.Where(x => x.Status == true).Select(s => new
                {
                    LeaveTypeId = s.LeaveTypeId,
                    LeaveTypeName = s.LeaveTypeName

                }).ToList();
                if (leavetypes != null && leavetypes.Count > 0)
                {
                    foreach (var item in leavetypes)
                    {
                        leavetypeList.Add(new HR_LeaveType { LeaveTypeId = item.LeaveTypeId, LeaveTypeName = item.LeaveTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leavetypeList;
        }




        public ResponseOut ApproveRejectEmployeeLeaveApplication(HR_EmployeeLeaveApplication employeeLeaveApp)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (employeeLeaveApp.LeaveStatus == "Rejected")
                {
                    entities.HR_EmployeeLeaveApplication.Where(a => a.ApplicationId == employeeLeaveApp.ApplicationId).ToList().ForEach(a =>
                    {
                        a.LeaveStatus = employeeLeaveApp.LeaveStatus;
                        a.RejectBy = employeeLeaveApp.ApproveBy;
                        a.RejectDate = DateTime.Now;
                        a.RejectReason = employeeLeaveApp.RejectReason;
                    });
                    responseOut.message = ActionMessage.EmployeeLeaveApplicationRejectSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
                else if (employeeLeaveApp.LeaveStatus == "Approved")
                {
                    entities.HR_EmployeeLeaveApplication.Where(a => a.ApplicationId == employeeLeaveApp.ApplicationId).ToList().ForEach(a =>
                    {
                        a.LeaveStatus = employeeLeaveApp.LeaveStatus;
                        a.ApproveBy = employeeLeaveApp.ApproveBy;
                        a.ApproveDate = DateTime.Now;
                    });
                    responseOut.message = ActionMessage.EmployeeLeaveApplicationApproveSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }

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









        #endregion

        #region PMSGoalCategory
        public ResponseOut AddEditGoalCategory(HR_PMS_GoalCategory pMSGoalCategory)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_PMS_GoalCategory.Any(x => x.GoalCategoryName == pMSGoalCategory.GoalCategoryName && x.GoalCategoryId != pMSGoalCategory.GoalCategoryId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateGoalCategory;
                }
                else
                {
                    if (pMSGoalCategory.GoalCategoryId == 0)
                    {
                        entities.HR_PMS_GoalCategory.Add(pMSGoalCategory);
                        responseOut.message = ActionMessage.GoalCategoryCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_PMS_GoalCategory.Where(a => a.GoalCategoryId == pMSGoalCategory.GoalCategoryId).ToList().ForEach(a =>
                        {
                            a.GoalCategoryId = pMSGoalCategory.GoalCategoryId;
                            a.GoalCategoryName = pMSGoalCategory.GoalCategoryName;
                            a.Weight = pMSGoalCategory.Weight;
                            a.Status = pMSGoalCategory.Status;
                        });
                        responseOut.message = ActionMessage.GoalCategoryUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        public List<HR_PMS_GoalCategory> GetPMSGoalCategoryList()
        {
            List<HR_PMS_GoalCategory> pMSGoalCategory = new List<HR_PMS_GoalCategory>();
            try
            {
                var pMSGoalCategoryList = entities.HR_PMS_GoalCategory.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.GoalCategoryName)).ThenBy(x => x.GoalCategoryName).Select(s => new
                {
                    GoalCategoryId = s.GoalCategoryId,
                    GoalCategoryName = s.GoalCategoryName

                }).ToList();
                if (pMSGoalCategoryList != null && pMSGoalCategoryList.Count > 0)
                {
                    foreach (var item in pMSGoalCategoryList)
                    {
                        pMSGoalCategory.Add(new HR_PMS_GoalCategory { GoalCategoryId = item.GoalCategoryId, GoalCategoryName = item.GoalCategoryName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pMSGoalCategory;
        }
        #endregion


        #region PMS_Section
        public ResponseOut AddEditSection(HR_PMS_Section pmssection)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_PMS_Section.Any(x => x.SectionName == pmssection.SectionName && x.SectionId != pmssection.SectionId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.PMS_SectionCreatedSuccess;
                }
                else
                {
                    if (pmssection.SectionId == 0)
                    {
                        entities.HR_PMS_Section.Add(pmssection);
                        responseOut.message = ActionMessage.PMS_SectionCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_PMS_Section.Where(a => a.SectionId == pmssection.SectionId).ToList().ForEach(a =>
                        {
                            a.SectionId = pmssection.SectionId;
                            a.SectionName = pmssection.SectionName;
                            a.Status = pmssection.Status;
                        });
                        responseOut.message = ActionMessage.PMS_SectionUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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
        public List<HR_PMS_Section> GetPMSSectionList()
        {
            List<HR_PMS_Section> pMSSection = new List<HR_PMS_Section>();
            try
            {
                var pMSSectionList = entities.HR_PMS_Section.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.SectionName)).ThenBy(x => x.SectionName).Select(s => new
                {
                    SectionId = s.SectionId,
                    SectionName = s.SectionName

                }).ToList();
                if (pMSSectionList != null && pMSSectionList.Count > 0)
                {
                    foreach (var item in pMSSectionList)
                    {
                        pMSSection.Add(new HR_PMS_Section { SectionId = item.SectionId, SectionName = item.SectionName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pMSSection;
        }
        #endregion

        #region PMS_PerformanceCycle
        public ResponseOut AddEditPerformanceCycle(HR_PMS_PerformanceCycle pmsperformancecycle)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_PMS_PerformanceCycle.Any(x => x.PerformanceCycleName == pmsperformancecycle.PerformanceCycleName && x.PerformanceCycleId != pmsperformancecycle.PerformanceCycleId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.PMS_PerformanceCycleCreatedSuccess;
                }
                else
                {
                    if (pmsperformancecycle.PerformanceCycleId == 0)
                    {
                        entities.HR_PMS_PerformanceCycle.Add(pmsperformancecycle);
                        responseOut.message = ActionMessage.PMS_PerformanceCycleCreatedSuccess;
                    }
                    else
                    {
                        entities.HR_PMS_PerformanceCycle.Where(a => a.PerformanceCycleId == pmsperformancecycle.PerformanceCycleId).ToList().ForEach(a =>
                        {
                            a.PerformanceCycleId = pmsperformancecycle.PerformanceCycleId;
                            a.PerformanceCycleName = pmsperformancecycle.PerformanceCycleName;
                            a.Status = pmsperformancecycle.Status;
                        });
                        responseOut.message = ActionMessage.PMS_PerformanceCycleUpdatedSuccess;
                    }
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }
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

        public List<HR_PMS_PerformanceCycle> GetPMSPerformanceCycleList()
        {
            List<HR_PMS_PerformanceCycle> pMSPerformanceCycle = new List<HR_PMS_PerformanceCycle>();
            try
            {
                var pMSPerformanceCycleList = entities.HR_PMS_PerformanceCycle.Where(x => x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.PerformanceCycleName)).ThenBy(x => x.PerformanceCycleName).Select(s => new
                {
                    PerformanceCycleId = s.PerformanceCycleId,
                    PerformanceCycleName = s.PerformanceCycleName

                }).ToList();
                if (pMSPerformanceCycleList != null && pMSPerformanceCycleList.Count > 0)
                {
                    foreach (var item in pMSPerformanceCycleList)
                    {
                        pMSPerformanceCycle.Add(new HR_PMS_PerformanceCycle { PerformanceCycleId = item.PerformanceCycleId, PerformanceCycleName = item.PerformanceCycleName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return pMSPerformanceCycle;
        }
        #endregion


        #region PMS_Goal
        public ResponseOut AddEditGoal(HR_PMS_Goal pMSGoal)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (entities.HR_PMS_Goal.Any(x => x.GoalName == pMSGoal.GoalName && x.GoalId != pMSGoal.GoalId))
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.DuplicateGoal;
                }
                else
                {
                    //var categoryWeight = entities.HR_PMS_Goal.Where(a => a.GoalCategoryId == pMSGoal.GoalCategoryId && a.GoalId != pMSGoal.GoalId && a.Status==true) .Sum(x => x.Weight);

                    //if (categoryWeight + pMSGoal.Weight > 100)
                    //{
                    //    responseOut.message = ActionMessage.GoalCategoryWeight;
                    //    responseOut.status = ActionStatus.Fail;
                    //}
                    //else
                    //{

                        if (pMSGoal.GoalId == 0)
                        {
                            pMSGoal.CreatedDate = DateTime.Now;
                            entities.HR_PMS_Goal.Add(pMSGoal);
                            responseOut.message = ActionMessage.GoalCreatedSuccess;
                        }
                        else
                        {
                            entities.HR_PMS_Goal.Where(a => a.GoalId == pMSGoal.GoalId && a.GoalCategoryId == pMSGoal.GoalCategoryId).ToList().ForEach(a =>
                              {
                                  a.GoalId = pMSGoal.GoalId;
                                  a.GoalName = pMSGoal.GoalName;
                                  a.GoalDescription = pMSGoal.GoalDescription;
                                  a.SectionId = pMSGoal.SectionId;
                                  a.GoalCategoryId = pMSGoal.GoalCategoryId;
                                  a.PerformanceCycleId = pMSGoal.PerformanceCycleId;
                                  a.CompanyId = pMSGoal.CompanyId;
                                  a.StartDate = pMSGoal.StartDate;
                                  a.DueDate = pMSGoal.DueDate;
                                  a.Weight = pMSGoal.Weight;
                                  a.CreatedBy = pMSGoal.CreatedBy;
                                  a.ModifiedBy = pMSGoal.CreatedBy;
                                  a.ModifiedDate = DateTime.Now;
                                  a.Status = pMSGoal.Status;
                                  a.CompanyBranchId = pMSGoal.CompanyBranchId;
                              });
                            responseOut.message = ActionMessage.GoalUpdatedSuccess;
                        }
                        entities.SaveChanges();
                        responseOut.trnId = pMSGoal.GoalId;
                        responseOut.status = ActionStatus.Success;
                   // }
                  
                }
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

        public List<HR_PMS_Goal> GetGoalAutoCompleteList(string searchTerm, int companyId)
        {
            List<HR_PMS_Goal> hR_PMS_GoalList = new List<HR_PMS_Goal>();
            try
            {
                var pMSGoal = (from goal in entities.HR_PMS_Goal
                                 where ((goal.GoalName.ToLower().Contains(searchTerm.ToLower())) && goal.CompanyId == companyId && goal.Status == true)
                                 select new
                                 {
                                     GoalId = goal.GoalId,
                                     GoalName = goal.GoalName,
                                     GoalDescription = goal.GoalDescription,
                                     SectionId = goal.SectionId,
                                     GoalCategoryId = goal.GoalCategoryId,
                                     PerformanceCycleId = goal.PerformanceCycleId,
                                     FinYearId = goal.FinYearId,
                                     StartDate =goal.StartDate,
                                     DueDate = goal.DueDate,
                                     Weight = goal.Weight,
                                     Status = goal.Status,                                    

                                 }).ToList();
                if (pMSGoal != null && pMSGoal.Count > 0)
                {
                    foreach (var item in pMSGoal)
                    {

                        hR_PMS_GoalList.Add(new HR_PMS_Goal
                        {
                            GoalId = item.GoalId,
                            GoalName = item.GoalName,
                            GoalDescription = item.GoalDescription,
                            SectionId = item.SectionId,
                            GoalCategoryId = item.GoalCategoryId,
                            PerformanceCycleId = item.PerformanceCycleId,
                            FinYearId = item.FinYearId,
                            StartDate = item.StartDate,
                            DueDate = item.DueDate,
                            Weight = item.Weight,
                            Status = item.Status,
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return hR_PMS_GoalList;
        }
        #endregion





        #region Shift Master
        public List<HR_Shift> GetShiftList(int companyId)
        {
            List<HR_Shift> shiftList = new List<HR_Shift>();
            try
            {
                var shifts = entities.HR_Shift.Where(x => x.CompanyId==companyId && x.Status == true).OrderBy(x => SqlFunctions.IsNumeric(x.ShiftName)).ThenBy(x => x.ShiftName).Select(s => new
                {
                    ShiftId = s.ShiftId,
                    ShiftName = s.ShiftName,
                    ShiftDescription = s.ShiftDescription

                }).ToList();
                if (shifts != null && shifts.Count > 0)
                {
                    foreach (var item in shifts)
                    {
                        shiftList.Add(new HR_Shift { ShiftId = item.ShiftId, ShiftName = item.ShiftName, ShiftDescription = item.ShiftDescription });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return shiftList;
        }
        #endregion


        #region Separation Application
        public ResponseOut ApproveRejectSeparationApplication(HR_SeparationApplication separation)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                if (separation.ApplicationStatus == "Rejected")
                {
                    entities.HR_SeparationApplication.Where(a => a.ApplicationId == separation.ApplicationId).ToList().ForEach(a =>
                    {
                        a.ApplicationStatus = separation.ApplicationStatus;
                        a.RejectBy = separation.CreatedBy;
                        a.RejectDate = DateTime.Now;
                        a.RejectReason = separation.RejectReason;
                        a.ApplicationStatus = separation.ApplicationStatus;
                    });
                    responseOut.message = ActionMessage.SeparationApplicationRejectSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;

                }
                else if (separation.ApplicationStatus == "Approved")
                {
                    entities.HR_SeparationApplication.Where(a => a.ApplicationId == separation.ApplicationId).ToList().ForEach(a =>
                    {
                        a.ApplicationStatus = separation.ApplicationStatus;
                        a.ApproveBy = separation.CreatedBy;
                        a.ApproveDate = DateTime.Now;
                        a.ApplicationStatus = separation.ApplicationStatus;
                       
                    });
                    responseOut.message = ActionMessage.SeparationApplicationApproveSuccess;
                    entities.SaveChanges();
                    responseOut.status = ActionStatus.Success;
                }

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
       
        public List<HR_SeparationApplication> GetSeparationApplicationForExitInterviewList()
        {
            List<HR_SeparationApplication> separationapplicationList = new List<HR_SeparationApplication>();
            try
            {
                var separationapplications = entities.HR_SeparationApplication.Where(x => x.ApplicationStatus == "Approved").Select(s => new
                {
                    ApplicationId = s.ApplicationId,
                    ApplicationNo = s.ApplicationNo

                }).ToList();
                if (separationapplications != null && separationapplications.Count > 0)
                {
                    foreach (var item in separationapplications)
                    {
                        separationapplicationList.Add(new HR_SeparationApplication { ApplicationId = item.ApplicationId, ApplicationNo = item.ApplicationNo });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return separationapplicationList;
        }
        #endregion


        #region Separation Order
        public List<HR_ExitInterview> GetExitInterviewForSeparationOrderList()
        {
            List<HR_ExitInterview> exitinterviewList = new List<HR_ExitInterview>();
            try
            {
                var exitinterviews = entities.HR_ExitInterview.Where(x => x.InterviewStatus == "True").Select(s => new
                {
                    ExitInterviewId = s.ExitInterviewId,
                    ExitInterviewNo = s.ExitInterviewNo 

                }).ToList();
                if (exitinterviews != null && exitinterviews.Count > 0)
                {
                    foreach (var item in exitinterviews)
                    {
                        exitinterviewList.Add(new HR_ExitInterview { ExitInterviewId = item.ExitInterviewId, ExitInterviewNo = item.ExitInterviewNo });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return exitinterviewList;
        }


        public List<HR_ClearanceTemplate> GetClearanceTemplateList(int companyId=0)
        {
            List<HR_ClearanceTemplate> clearancetemplateList = new List<HR_ClearanceTemplate>();
            try
            {
                var clearancetemplates = entities.HR_ClearanceTemplate.Where(x => x.CompanyId == companyId &&  x.Status == true).Select(s => new
                {
                    ClearanceTemplateId = s.ClearanceTemplateId,
                    ClearanceTemplateName = s.ClearanceTemplateName

                }).ToList();
                if (clearancetemplates != null && clearancetemplates.Count > 0)
                {
                    foreach (var item in clearancetemplates)
                    {
                        clearancetemplateList.Add(new HR_ClearanceTemplate { ClearanceTemplateId = item.ClearanceTemplateId, ClearanceTemplateName = item.ClearanceTemplateName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return clearancetemplateList;
        }
        #endregion

        #region Payroll Month
        public List<PR_PayrollMonth> GetPayrollMonth()
        {
            List<PR_PayrollMonth> monthList = new List<PR_PayrollMonth>();
            try
            {
                 monthList = entities.PR_PayrollMonth.ToList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return monthList;
        }

      
        public PR_PayrollMonth GetPayrollMonthDetail(int monthId)
        {
            PR_PayrollMonth month = new PR_PayrollMonth();
            try
            {
                month = entities.PR_PayrollMonth.Where(x => x.MonthId == monthId).FirstOrDefault();
                
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return month;
        }

        #endregion

        #region Payroll Salary Summary Report
        public List<PR_PayrollProcessPeriod> GetPayrollProcessedMonth(int companyId,int finYearId)
        {
            List<PR_PayrollProcessPeriod> monthList = new List<PR_PayrollProcessPeriod>();
            try
            {
                monthList = entities.PR_PayrollProcessPeriod.Where(x=>x.CompanyId==companyId && x.FinYearId==finYearId).OrderBy(y=>y.MonthId).ToList();
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return monthList;
        }
        #endregion

        #region PayHeadGLMapping
        public ResponseOut AddEditPayHeadGLMapping(PR_PayHeadGLMapping payHeadGLMapping)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                //if (entities.PR_PayHeadGLMapping.Any(x => x.PayHeadName == payHeadGLMapping.PayHeadName && x.CompanyId == payHeadGLMapping.CompanyId && x.PayHeadMappingId != payHeadGLMapping.PayHeadMappingId))
                //{
                //    responseOut.status = ActionStatus.Fail;
                //    responseOut.message = ActionMessage.DuplicatePayHeadGLMappingName;
                //}


                if (payHeadGLMapping.PayHeadMappingId == 0)
                {

                    entities.PR_PayHeadGLMapping.Add(payHeadGLMapping);
                    responseOut.message = ActionMessage.PayHeadGLMappingCreatedSuccess;
                }
                else
                {

                    entities.PR_PayHeadGLMapping.Where(a => a.PayHeadMappingId == payHeadGLMapping.PayHeadMappingId).ToList().ForEach(a =>
                    {
                        a.PayHeadMappingId = payHeadGLMapping.PayHeadMappingId;
                        a.CompanyId = payHeadGLMapping.CompanyId;
                        a.PayHeadName = payHeadGLMapping.PayHeadName;
                        a.GLId = payHeadGLMapping.GLId;
                        a.SLId = payHeadGLMapping.SLId;
                        a.GLCode = payHeadGLMapping.GLCode;
                        a.SLCode = payHeadGLMapping.SLCode;
                        a.CompanyBranchId = payHeadGLMapping.CompanyBranchId;
                        a.Status = payHeadGLMapping.Status;
                    });
                    responseOut.message = ActionMessage.PayHeadGLMappingUpdatedSuccess;
                }
                entities.SaveChanges();
                responseOut.status = ActionStatus.Success;


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
        #endregion
    }
}