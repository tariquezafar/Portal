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
    public class ResourceRequisitionBL
    {
        HRMSDBInterface dbInterface;
        public ResourceRequisitionBL()
        {
            dbInterface = new HRMSDBInterface();
        }
        public ResponseOut AddEditResourceRequisition(ResourceRequisitionViewModel resourceRequisitionViewModel, List<ResourceRequisitionSkillViewModel> resourceRequisitionSkills, List<HR_ResourceRequisitionInterviewStageViewModel> resourceRequisitionInterviewStages)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                HR_ResourceRequisition resourceRequisition = new HR_ResourceRequisition
                {
                    RequisitionId = resourceRequisitionViewModel.RequisitionId,
                    NumberOfResources = resourceRequisitionViewModel.NumberOfResources,
                    PositionLevelId = resourceRequisitionViewModel.PositionLevelId, 
                    PriorityLevel = resourceRequisitionViewModel.PriorityLevel,
                    PositionTypeId = resourceRequisitionViewModel.PositionTypeId,
                    ContractPeriod = resourceRequisitionViewModel.ContractPeriod,
                    DepartmentId = resourceRequisitionViewModel.DepartmentId,
                    DesignationId = resourceRequisitionViewModel.DesignationId,
                    EducationId = resourceRequisitionViewModel.EducationId,
                    JobDescription = resourceRequisitionViewModel.JobDescription,
                    OtherQualification = resourceRequisitionViewModel.OtherQualification,
                    MinExp = resourceRequisitionViewModel.MinExp,
                    MaxExp = resourceRequisitionViewModel.MaxExp,
                    MinSalary = resourceRequisitionViewModel.MinSalary,
                    MaxSalary = resourceRequisitionViewModel.MaxSalary,
                    CurrencyCode = resourceRequisitionViewModel.CurrencyCode,
                    Remarks = resourceRequisitionViewModel.Remarks,
                    JustificationNotes = resourceRequisitionViewModel.JustificationNotes,
                    InterviewStartDate =Convert.ToDateTime(resourceRequisitionViewModel.InterviewStartDate),
                    HireByDate = Convert.ToDateTime(resourceRequisitionViewModel.HireByDate),
                    RequisitionStatus = resourceRequisitionViewModel.RequisitionStatus, 
                    CompanyId = resourceRequisitionViewModel.CompanyId,
                    CreatedBy = resourceRequisitionViewModel.CreatedBy,
                    CompanyBranchId=resourceRequisitionViewModel.CompanyBranchId
                };


                List<HR_ResourceRequisitionSkill> requisitionSkillList = new List<HR_ResourceRequisitionSkill>();
                if (resourceRequisitionSkills != null && resourceRequisitionSkills.Count > 0)
                {
                    foreach (ResourceRequisitionSkillViewModel item in resourceRequisitionSkills)
                    {
                        requisitionSkillList.Add(new HR_ResourceRequisitionSkill
                        {
                            SkillId = item.SkillId
                        });
                    }
                }

                List<HR_ResourceRequisitionInterviewStage> requisitionInterviewStageList = new List<HR_ResourceRequisitionInterviewStage>();
                if (resourceRequisitionInterviewStages != null && resourceRequisitionInterviewStages.Count > 0)
                {
                    foreach (HR_ResourceRequisitionInterviewStageViewModel item in resourceRequisitionInterviewStages)
                    {
                        requisitionInterviewStageList.Add(new HR_ResourceRequisitionInterviewStage
                        {
                            InterviewTypeId = item.InterviewTypeId,
                            InterviewDescription=item.InterviewDescription,
                            InterviewAssignToUserId=item.InterviewAssignToUserId
                        });
                    }
                }

                responseOut = sqlDbInterface.AddEditResourceRequisition(resourceRequisition, requisitionSkillList,requisitionInterviewStageList); 
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


        public List<PositionLevelViewModel> GetPositionLevelList()
        {
            List<PositionLevelViewModel> positionLevels = new List<PositionLevelViewModel>();
            try
            {
                List<HR_PositionLevel> positionLevelList = dbInterface.GetPositionLevelList();
                if (positionLevelList != null && positionLevelList.Count > 0)
                {
                    foreach (HR_PositionLevel positionLevel in positionLevelList)
                    {
                        positionLevels.Add(new PositionLevelViewModel { PositionLevelId = positionLevel.PositionLevelId, PositionLevelName = positionLevel.PositionLevelName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return positionLevels;
        }

        public List<HR_EducationViewModel> GetEducationList()
        {
            List<HR_EducationViewModel> educations = new List<HR_EducationViewModel>();
            try
            {
                List<HR_Education> educationList = dbInterface.GetEducationList();
                if (educationList != null && educationList.Count > 0)
                {
                    foreach (HR_Education education in educationList)
                    {
                        educations.Add(new HR_EducationViewModel { EducationId = education.EducationId, EducationName = education.EducationName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return educations;
        }

        public List<HR_InterviewTypeViewModel> GetInterviewTypeList()
        {
            List<HR_InterviewTypeViewModel> interviewtypes = new List<HR_InterviewTypeViewModel>();
            try
            {
                List<HR_InterviewType> interviewtypeList = dbInterface.GetInterviewTypeList();
                if (interviewtypeList != null && interviewtypeList.Count > 0)
                {
                    foreach (HR_InterviewType interviewtype in interviewtypeList)
                    {
                        interviewtypes.Add(new HR_InterviewTypeViewModel { InterviewTypeId = interviewtype.InterviewTypeId, InterviewTypeName = interviewtype.InterviewTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return interviewtypes;
        }


        public List<SkillViewModel> GetSkillAutoCompleteList(string searchTerm)
        {
            List<SkillViewModel> skills = new List<SkillViewModel>();
            try
            {
                List<HR_Skills> skillList = dbInterface.GetSkillAutoCompleteList(searchTerm);
                if (skillList != null && skillList.Count > 0)
                {
                    foreach (HR_Skills skill in skillList)
                    {
                        skills.Add(new SkillViewModel { SkillId = skill.SkillId, SkillName = skill.SkillName, SkillCode = skill.SkillCode });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return skills;
        }

        public List<PositionTypeViewModel> GetPositionTypeList()
        {
            List<PositionTypeViewModel> positionTypes = new List<PositionTypeViewModel>();
            try
            {
                List<HR_PositionType> positionTypeList = dbInterface.GetPositionTypeList();
                if (positionTypeList != null && positionTypeList.Count > 0)
                {
                    foreach (HR_PositionType positionType in positionTypeList)
                    {
                        positionTypes.Add(new PositionTypeViewModel { PositionTypeId = positionType.PositionTypeId, PositionTypeName = positionType.PositionTypeName });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return positionTypes;
        }


        public List<ResourceRequisitionViewModel> GetResourceRequisitionList(string requisitionNo, int positionLevelId, string priorityLevel, int positionTypeId, int departmentId, string approvalStatus, string fromDate, string toDate, int companyId,int companyBranchId)
        {
            List<ResourceRequisitionViewModel> requisitions = new List<ResourceRequisitionViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitions = sqlDbInterface.GetResourceRequisitionList(requisitionNo, positionLevelId, priorityLevel,  positionTypeId, departmentId, approvalStatus, Convert.ToDateTime(fromDate),Convert.ToDateTime(toDate), companyId, companyBranchId);
                if (dtRequisitions != null && dtRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitions.Rows)
                    {
                        requisitions.Add(new ResourceRequisitionViewModel
                        {
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            NumberOfResources = Convert.ToInt16(dr["NumberOfResources"]),
                            PositionLevelName = Convert.ToString(dr["PositionLevelName"]),
                            PriorityLevel = Convert.ToString(dr["PriorityLevel"]),
                            PositionTypeName = Convert.ToString(dr["PositionTypeName"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
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
            return requisitions;
        }



        public ResourceRequisitionViewModel GetResourceRequisitionDetail(long requisitionId = 0)
        {
            ResourceRequisitionViewModel requisition = new ResourceRequisitionViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisition = sqlDbInterface.GetResourceRequisitionDetail(requisitionId);
                if (dtRequisition != null && dtRequisition.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisition.Rows)
                    {
                        requisition = new ResourceRequisitionViewModel
                        {
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            NumberOfResources = Convert.ToInt16(dr["NumberOfResources"]),
                            PositionLevelId = Convert.ToInt32(dr["PositionLevelId"]),
                            PriorityLevel = Convert.ToString(dr["PriorityLevel"]),
                            PositionTypeId = Convert.ToInt32(dr["PositionTypeId"]),
                            DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            EducationId = Convert.ToInt32(dr["EducationId"]),
                            JobDescription = Convert.ToString(dr["JobDescription"]),
                            OtherQualification = Convert.ToString(dr["OtherQualification"]),
                            MinExp = Convert.ToInt16(dr["MinExp"]),
                            MaxExp = Convert.ToInt16(dr["MaxExp"]),
                            MinSalary = Convert.ToDecimal(dr["MinSalary"]),
                            MaxSalary = Convert.ToDecimal(dr["MaxSalary"]),
                            CurrencyCode = Convert.ToString(dr["CurrencyCode"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            JustificationNotes = Convert.ToString(dr["JustificationNotes"]),
                            InterviewStartDate = Convert.ToString(dr["InterviewStartDate"]),
                            HireByDate = Convert.ToString(dr["HireByDate"]),

                            ContractPeriod = Convert.ToInt16(dr["ContractPeriod"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            RequisitionStatus = Convert.ToString(dr["RequisitionStatus"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            ApprovedDate= Convert.ToString(dr["ApprovedDate"]),
                            RejectionStatus = Convert.ToString(dr["RejectionStatus"]),
                            RejectedDate = Convert.ToString(dr["RejectedDate"]),
                            RejectedReason = Convert.ToString(dr["RejectedReason"]),
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
            return requisition;
        }
        

        public List<ResourceRequisitionSkillViewModel> GetResourceRequisitionSkillList(long resourceRequisitionId)
        {
            List<ResourceRequisitionSkillViewModel> skills = new List<ResourceRequisitionSkillViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetResourceRequisitionSkillList(resourceRequisitionId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        skills.Add(new ResourceRequisitionSkillViewModel
                        { 
                            SkillSequenceNo = Convert.ToInt32(dr["SkillSequenceNo"]),
                            RequisitionSkillId = Convert.ToInt32(dr["RequisitionSkillId"]),
                            SkillId = Convert.ToInt32(dr["SkillId"]),
                            SkillName = Convert.ToString(dr["SkillName"]),
                            SkillCode = Convert.ToString(dr["SkillCode"])
                      
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return skills;
        }

        public List<HR_ResourceRequisitionInterviewStageViewModel> GetResourceRequisitionInterviewStageList(long resourceRequisitionId)
        {
            List<HR_ResourceRequisitionInterviewStageViewModel> interviews = new List<HR_ResourceRequisitionInterviewStageViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtCustomers = sqlDbInterface.GetResourceRequisitionInterviewStageList(resourceRequisitionId);
                if (dtCustomers != null && dtCustomers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCustomers.Rows)
                    {
                        interviews.Add(new HR_ResourceRequisitionInterviewStageViewModel
                        {
                            InterviewSequenceNo = Convert.ToInt32(dr["InterviewSequenceNo"]),
                            RequisitionInterviewStagesId = Convert.ToInt32(dr["RequisitionInterviewStagesId"]),
                            InterviewTypeId = Convert.ToInt32(dr["InterviewTypeId"]), 
                            InterviewTypeName = Convert.ToString(dr["InterviewTypeName"]),
                            InterviewDescription = Convert.ToString(dr["InterviewDescription"]),
                            InterviewAssignToUserId = Convert.ToInt32(dr["InterviewAssignToUserId"]),
                            InterviewAssignToUserName = Convert.ToString(dr["InterviewAssignToUserName"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return interviews;
        }

        public List<ResourceRequisitionViewModel> GetResourceRequisitionApprovalList(string requisitionNo, int positionLevelId, string priorityLevel, int positionTypeId, int departmentId, string approvalStatus, string fromDate, string toDate, int companyId)
        {
            List<ResourceRequisitionViewModel> requisitions = new List<ResourceRequisitionViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtRequisitions = sqlDbInterface.GetResourceRequisitionApprovalList(requisitionNo, positionLevelId, priorityLevel, positionTypeId, departmentId, approvalStatus, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyId);
                if (dtRequisitions != null && dtRequisitions.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtRequisitions.Rows)
                    {
                        requisitions.Add(new ResourceRequisitionViewModel
                        {
                            RequisitionId = Convert.ToInt32(dr["RequisitionId"]),
                            RequisitionNo = Convert.ToString(dr["RequisitionNo"]),
                            NumberOfResources = Convert.ToInt16(dr["NumberOfResources"]),
                            PositionLevelName = Convert.ToString(dr["PositionLevelName"]),
                            PriorityLevel = Convert.ToString(dr["PriorityLevel"]),
                            PositionTypeName = Convert.ToString(dr["PositionTypeName"]),
                            DepartmentName = Convert.ToString(dr["DepartmentName"]),
                            DesignationName = Convert.ToString(dr["DesignationName"]),
                            ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ApprovedByUserName = Convert.ToString(dr["ApprovedByName"]),
                            ApprovedDate = Convert.ToString(dr["ApprovedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return requisitions;
        }


        public ResponseOut ApproveRejectResourceRequisition(ResourceRequisitionViewModel resourceRequisitionViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                HR_ResourceRequisition resourceRequisition = new HR_ResourceRequisition
                {
                    RequisitionId = resourceRequisitionViewModel.RequisitionId,
                    CreatedBy = resourceRequisitionViewModel.CreatedBy,
                    RejectedReason= resourceRequisitionViewModel.RejectedReason,
                    ApprovalStatus= resourceRequisitionViewModel.ApprovalStatus
                };
    
                responseOut = dbInterface.ApproveRejectResourceRequisition(resourceRequisition);
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



    

    }
}
