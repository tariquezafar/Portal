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
using System.Globalization;

namespace Portal.Core
{
    public class LeadBL
    {
        DBInterface dbInterface;
        public LeadBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditLead(LeadViewModel leadViewModel , List<LeadFollowUpViewModel> leadFollowUps)
        {
            ResponseOut responseOut = new ResponseOut();
            ResponseOut responseOutFollowUp = new ResponseOut();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    Lead lead = new Lead
                    {
                        LeadId = leadViewModel.LeadId,
                        LeadCode = leadViewModel.LeadCode,
                        CompanyId = leadViewModel.CompanyId,
                        CompanyName = leadViewModel.CompanyName,
                        Designation = leadViewModel.Designation,
                        City = leadViewModel.City,
                        ContactPersonName = leadViewModel.ContactPersonName,
                        Email = leadViewModel.Email,
                        AlternateEmail = leadViewModel.AlternateEmail,
                        ContactNo = leadViewModel.ContactNo,
                        AlternateContactNo = leadViewModel.AlternateContactNo,
                        Fax = leadViewModel.Fax,
                        CompanyAddress = leadViewModel.CompanyAddress,
                        BranchAddress = leadViewModel.BranchAddress,
                        StateId = leadViewModel.StateId,
                        CountryId = leadViewModel.CountryId,
                        PinCode = leadViewModel.PinCode,
                        CompanyCity = leadViewModel.CompanyCity,
                        CompanyStateId = leadViewModel.CompanyStateId,
                        CompanyCountryId = leadViewModel.CompanyCountryId,
                        CompanyPinCode = leadViewModel.CompanyPinCode,
                        LeadStatusId = leadViewModel.LeadStatusId,
                        LeadSourceId = leadViewModel.LeadSourceId,
                        OtherLeadSourceDescription = leadViewModel.OtherLeadSourceDescription,
                        CreatedBy = leadViewModel.CreatedBy,
                        Status = leadViewModel.Lead_Status,
                        LeadTypeId= leadViewModel.LeadTypeId,
                        CompanyBranchId= leadViewModel.CompanyBranch
                    };
                    int leadId = 0;
                    int leadStatusId = 0;
                    int leadFollowUpByUserId = 0;
                    responseOut = dbInterface.AddEditLead(lead,out leadId);
                    if (responseOut.status == ActionStatus.Success)
                    {
                        
                        if (leadFollowUps != null && leadFollowUps.Count > 0)
                        {
                          
                            foreach (LeadFollowUpViewModel leadFollowUpViewModel in leadFollowUps)
                            {
                                    LeadFollowUp leadFollowUp = new LeadFollowUp {
                                    LeadFollowUpId = leadFollowUpViewModel.LeadFollowUpId,
                                    LeadId = leadId,
                                    FollowUpActivityTypeId = leadFollowUpViewModel.FollowUpActivityTypeId,
                                    FollowUpDueDateTime =Convert.ToDateTime(leadFollowUpViewModel.FollowUpDueDateTime),
                                    FollowUpReminderDateTime =Convert.ToDateTime( leadFollowUpViewModel.FollowUpReminderDateTime),
                                    FollowUpRemarks=leadFollowUpViewModel.FollowUpRemarks,
                                    Priority =Convert.ToByte(leadFollowUpViewModel.Priority),
                                    LeadStatusId = leadFollowUpViewModel.LeadStatusId,
                                    LeadStatusReason = leadFollowUpViewModel.LeadStatusReason,
                                    FollowUpByUserId = leadFollowUpViewModel.FollowUpByUserId==0? leadFollowUpViewModel.CreatedBy: leadFollowUpViewModel.FollowUpByUserId,
                                    CreatedBy = leadFollowUpViewModel.CreatedBy,
                                    
                                    };
                                leadStatusId =leadFollowUpViewModel.LeadStatusId;
                                leadFollowUpByUserId = leadFollowUp.FollowUpByUserId==null?leadViewModel.CreatedBy:Convert.ToInt32(leadFollowUp.FollowUpByUserId);
                                responseOutFollowUp = dbInterface.AddEditLeadFollowUp(leadFollowUp);

                            }
                        }

                        dbInterface.UpdateLastLeadStatus(leadId, leadStatusId, leadFollowUpByUserId);
                    }
                    transactionscope.Complete();
                }
                catch(TransactionException ex1)
                {
                    transactionscope.Dispose();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex1);
                    throw ex1;
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                    throw ex;
                }
                return responseOut;
            }
        }

        public List<LeadTypeMasterViewModel> GetAllLeadType()
        {
            List<LeadTypeMasterViewModel> followUpActivityTypeList = new List<LeadTypeMasterViewModel>();

            try
            {
                List<Portal.DAL.LeadMaster> followUpActivityType = dbInterface.GetAllLeadTypeList();
                if (followUpActivityType != null && followUpActivityType.Count > 0)
                {
                    foreach (Portal.DAL.LeadMaster followuptype in followUpActivityType)
                    {
                        followUpActivityTypeList.Add(new LeadTypeMasterViewModel { LeadTypeId = followuptype.LeadTypeId, LeadTypeName = followuptype.LeadTypeName });
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return followUpActivityTypeList;
        }

        public ResponseOut ImportLead(LeadViewModel leadViewModel, LeadFollowUpViewModel leadFollowUpViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            ResponseOut responseOutFollowUp = new ResponseOut();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    Lead lead = new Lead
                    {
                        LeadId = leadViewModel.LeadId,
                        LeadCode = leadViewModel.LeadCode,
                        CompanyId = leadViewModel.CompanyId,
                        CompanyName = leadViewModel.CompanyName,
                        Designation = leadViewModel.Designation,
                        City = leadViewModel.City,
                        ContactPersonName = leadViewModel.ContactPersonName,
                        Email = leadViewModel.Email,
                        AlternateEmail = leadViewModel.AlternateEmail,
                        ContactNo = leadViewModel.ContactNo,
                        AlternateContactNo = leadViewModel.AlternateContactNo,
                        Fax = leadViewModel.Fax,
                        CompanyAddress = leadViewModel.CompanyAddress,
                        BranchAddress = leadViewModel.BranchAddress,
                        StateId = leadViewModel.StateId,
                        CountryId = leadViewModel.CountryId,
                        PinCode = leadViewModel.PinCode,
                        CompanyCity = leadViewModel.CompanyCity,
                        CompanyStateId = leadViewModel.CompanyStateId,
                        CompanyCountryId = leadViewModel.CompanyCountryId,
                        CompanyPinCode = leadViewModel.CompanyPinCode,
                        LeadStatusId = leadViewModel.LeadStatusId,
                        LeadSourceId = leadViewModel.LeadSourceId,
                        OtherLeadSourceDescription = leadViewModel.OtherLeadSourceDescription,
                        CreatedBy = leadViewModel.CreatedBy,
                        Status = leadViewModel.Lead_Status
                    };
                    int leadId = 0;
                    int leadStatusId = 0;
                    responseOut = dbInterface.AddEditLead(lead, out leadId);
                    if (responseOut.status == ActionStatus.Success)
                    {

                        if (leadFollowUpViewModel != null )
                        {
                                LeadFollowUp leadFollowUp = new LeadFollowUp
                                {
                                    LeadFollowUpId = leadFollowUpViewModel.LeadFollowUpId,
                                    LeadId = leadId,
                                    FollowUpActivityTypeId = leadFollowUpViewModel.FollowUpActivityTypeId,
                                    FollowUpDueDateTime = DateTime.Now.AddDays(1),
                                    FollowUpReminderDateTime = DateTime.Now.AddDays(1),
                                    FollowUpRemarks = leadFollowUpViewModel.FollowUpRemarks,
                                    Priority = Convert.ToByte(leadFollowUpViewModel.Priority),
                                    LeadStatusId = leadFollowUpViewModel.LeadStatusId,
                                    LeadStatusReason = leadFollowUpViewModel.LeadStatusReason,
                                    FollowUpByUserId = leadFollowUpViewModel.FollowUpByUserId == 0 ? leadFollowUpViewModel.CreatedBy : leadFollowUpViewModel.FollowUpByUserId,
                                    CreatedBy = leadFollowUpViewModel.CreatedBy,

                                };
                            leadStatusId = Convert.ToInt32(leadFollowUpViewModel.LeadStatusId);
                            responseOutFollowUp = dbInterface.AddEditLeadFollowUp(leadFollowUp);
                        }
                        dbInterface.UpdateLastLeadStatus(leadId, leadStatusId);
                    }
                transactionscope.Complete();
                }
                catch (TransactionException ex1)
                {
                    transactionscope.Dispose();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex1);
                    throw ex1;
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.ApplicationException;
                    Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                    throw ex;
                }
                return responseOut;
            }
        }

        public string GetUserIdByEmail(string userEmail)
        {
            string userId = string.Empty;
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtUserId = sqlDbInterface.GetEmployeeId(userEmail);
                if(dtUserId.Rows.Count>0)
                {
                    foreach(DataRow dr in dtUserId.Rows)
                    {
                        userId = dr["Employeeid"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return userId;
        }
        public List<UserViewModel> GetUserAutoCompleteList(string searchTerm,long companyBranchID)
        {

            List<UserViewModel> users = new List<UserViewModel>();
            try
            {
                List<User> userList = dbInterface.GetUserAutoCompleteList(searchTerm, companyBranchID);

                if (userList != null && userList.Count > 0)
                {
                    foreach (User user in userList)
                    {
                        users.Add(new UserViewModel {
                            
                            FullName =user.FullName,
                            UserName=user.UserName,
                            UserId = user.UserId,
                            MobileNo =user.MobileNo
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return users;
        }
        public List<LeadViewModel> GetLeadList(string leadCode = "", string companyName = "", string contactPersonName = "",  string email = "", string contactNo = "", string companyAddress = "", string companyCity = "", int companyStateId = 0,  int leadStatusId = 0, int leadSourceId = 0,int userId=0 ,string status = "", int reportingUserId=0, int reportingRoleId=0,int LeadTypeId=0,string companyBranch="")
        {
            List<LeadViewModel> leads = new List<LeadViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtLeads = sqlDbInterface.GetLeadList(leadCode, companyName, contactPersonName, email,  contactNo, companyAddress, companyCity, companyStateId, leadStatusId, leadSourceId, userId, status,reportingUserId,reportingRoleId, LeadTypeId, companyBranch);
                if (dtLeads != null && dtLeads.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLeads.Rows)
                    {
                        leads.Add(new LeadViewModel
                        {
                            LeadId = Convert.ToInt32(dr["LeadId"]),
                            LeadCode = Convert.ToString(dr["LeadCode"]),
                            CompanyName = Convert.ToString(dr["CompanyName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            AlternateContactNo = Convert.ToString(dr["AlternateContactNo"]),
                            Email = Convert.ToString(dr["Email"]),
                            AlternateEmail = Convert.ToString(dr["AlternateEmail"]),
                            Fax = Convert.ToString(dr["Fax"]),
                            CompanyAddress = Convert.ToString(dr["CompanyAddress"]),
                            CompanyCity = Convert.ToString(dr["CompanyCity"]), 
                            CompanyStateId = Convert.ToInt32(dr["CompanyStateId"]),
                            StateName = Convert.ToString(dr["StateName"]),
                            CompanyCountryId = Convert.ToInt32(dr["CompanyCountryId"]),
                            CompanyPinCode = Convert.ToString(dr["CompanyPinCode"]),
                            BranchAddress = Convert.ToString(dr["BranchAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            LeadSourceId = Convert.ToInt32(dr["LeadSourceId"]),
                            LeadSourceName = Convert.ToString(dr["LeadSourceName"]),
                            LeadStatusId = Convert.ToInt32(dr["LeadStatusId"]),
                            LeadStatusName = Convert.ToString(dr["LeadStatusName"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"].ToString() == "" ? "0" : dr["CreatedBy"].ToString()),
                            CreatedName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedName = Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            Lead_Status = Convert.ToBoolean(dr["Status"]),
                            LeadTypeName = Convert.ToString(dr["LeadTypeName"]),
                            CompanyBranchName = Convert.ToString(dr["BranchName"]),

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leads;
        }

        public LeadViewModel GetLeadDetail(int leadId = 0)
        {
            LeadViewModel lead = new LeadViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtLeads = sqlDbInterface.GetLeadDetail(leadId);
                if (dtLeads != null && dtLeads.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLeads.Rows)
                    {
                        lead = new LeadViewModel
                        {
                            LeadId = Convert.ToInt32(dr["LeadId"]),
                            LeadCode = Convert.ToString(dr["LeadCode"]),
                            CompanyName = Convert.ToString(dr["CompanyName"]),
                            ContactPersonName = Convert.ToString(dr["ContactPersonName"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            AlternateContactNo = Convert.ToString(dr["AlternateContactNo"]),
                            Email = Convert.ToString(dr["Email"]),
                            AlternateEmail = Convert.ToString(dr["AlternateEmail"]),
                            Fax = Convert.ToString(dr["Fax"]),
                            CompanyAddress = Convert.ToString(dr["CompanyAddress"]),
                            CompanyCity = Convert.ToString(dr["CompanyCity"]),
                            CompanyStateId = Convert.ToInt32(dr["CompanyStateId"]),
                            CompanyCountryId = Convert.ToInt32(dr["CompanyCountryId"]),
                            CompanyPinCode = Convert.ToString(dr["CompanyPinCode"]),
                            BranchAddress = Convert.ToString(dr["BranchAddress"]),
                            City = Convert.ToString(dr["City"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            PinCode = Convert.ToString(dr["PinCode"]),
                            OtherLeadSourceDescription = Convert.ToString(dr["OtherLeadSourceDescription"]),
                            LeadSourceId = Convert.ToInt32(dr["LeadSourceId"]),
                            LeadStatusId = Convert.ToInt32(dr["LeadStatusId"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString() == "" ? "0" : dr["ModifiedBy"].ToString()),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            Lead_Status = Convert.ToBoolean(dr["Status"]),
                            LeadTypeId= Convert.ToInt32(dr["LeadTypeId"]),
                            CompanyBranch= Convert.ToInt32(dr["CompanyBranchId"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return lead;
        }
        public List<StateViewModel> GetStateList(string EmployeeUserName,int countryId = 0)
        {
            List<StateViewModel> stateList = new List<StateViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable states = sqlDbInterface.GetLeadStateMap(EmployeeUserName, countryId);
                if (states != null && states.Rows.Count > 0)
                {
                    foreach (DataRow dr in states.Rows)
                    {
                        stateList.Add(new StateViewModel {
                        StateId = Convert.ToInt16(dr["Stateid"].ToString()),
                        StateCode =dr["StateCode"].ToString(),
                        StateName =dr["StateName"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return stateList;
        }
        public List<FollowUpActivityTypeViewModel> GetFollowUpActivityTypeList()
        {
            List<FollowUpActivityTypeViewModel> followUpActivityTypeList = new List<FollowUpActivityTypeViewModel>();

            try
            {
                List<Portal.DAL.FollowUpActivityType> followUpActivityType = dbInterface.GetFollowUpActivityTypeList();
                if (followUpActivityType != null && followUpActivityType.Count > 0)
                {
                    foreach (Portal.DAL.FollowUpActivityType followuptype in followUpActivityType)
                    {
                        followUpActivityTypeList.Add(new FollowUpActivityTypeViewModel { FollowUpActivityTypeId = followuptype.FollowUpActivityTypeId, FollowUpActivityTypeName = followuptype.FollowUpActivityTypeName });
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return followUpActivityTypeList;
        }
        public ResponseOut LeadFollowUpValidation(LeadFollowUpViewModel leadFollowUpViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            if (leadFollowUpViewModel != null)
            {
                DateTime dueDate;
                DateTime RemDate;
                DateTime.TryParse(leadFollowUpViewModel.FollowUpDueDateTime, out dueDate);
                DateTime.TryParse(leadFollowUpViewModel.FollowUpReminderDateTime, out RemDate);
                if (RemDate > dueDate)
                {
                    responseOut.status = ActionStatus.Fail;
                    responseOut.message = ActionMessage.CustomerFollowUpsDateCheck;
                }
                else
                {
                    responseOut.status = ActionStatus.Success;
                }
            }
            return responseOut;
        }
        public List<LeadFollowUpViewModel> GetLeadFollowUpList(int leadId)
        {
            List<LeadFollowUpViewModel> leadFollowUps = new List<LeadFollowUpViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            
            try
            {
                DataTable dtleads = sqlDbInterface.GetLeadFollowUpList(leadId);
                if (dtleads != null && dtleads.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtleads.Rows)
                    {
                        leadFollowUps.Add(new LeadFollowUpViewModel {
                            SequenceNo = Convert.ToInt32(dr["SNo"]),
                            LeadFollowUpId = Convert.ToInt32(dr["LeadFollowUpId"]), 
                            FollowUpActivityTypeId = Convert.ToInt32(dr["FollowUpActivityTypeId"]),
                            FollowUpActivityTypeName = dr["FollowUpActivityTypeName"].ToString(),
                            FollowUpDueDateTime = Convert.ToString(dr["FollowUpDueDateTime"]),
                            FollowUpReminderDateTime = Convert.ToString(dr["FollowUpReminderDateTime"]),
                            FollowUpRemarks = dr["FollowUpRemarks"].ToString(),
                            Priority = Convert.ToInt16(dr["Priority"]),
                            PriorityName = ((Convert.ToInt16(dr["Priority"].ToString()) == 1) ? "Urgent" : (Convert.ToInt16(dr["Priority"].ToString()) == 2) ? "High" : (Convert.ToInt16(dr["Priority"].ToString()) == 3) ? "Medium" : "Low"),
                            LeadStatusId=Convert.ToInt32(dr["LeadStatusId"]),
                            LeadStatusName = dr["LeadStatusName"].ToString(),
                            LeadStatusReason = dr["LeadStatusReason"].ToString(),
                            FollowUpByUserName = dr["FollowUpByUserName"].ToString(),
                            CreatedByName = dr["FullName"].ToString(),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"])==0?0: Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = dr["CreatedDate"].ToString()==""?"": dr["CreatedDate"].ToString(),
                            ModifiedBy = (Convert.ToInt16(dr["ModifiedBy"]) == 0) ? 0 : Convert.ToInt16(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]) == "" ? "" : Convert.ToString(dr["ModifiedDate"]),

                        });
                
                }
                       
               }
            }
            
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return leadFollowUps;
        }
        public List<UserViewModel> GetTeamDetailList(int reportingUserId)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();

            try
            {
                DataTable dtUsers = sqlDbInterface.GetTeamDetailList(reportingUserId);
                if (dtUsers != null && dtUsers.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtUsers.Rows)
                    {
                        users.Add(new UserViewModel
                        {
                            FullName = Convert.ToString(dr["FullName"]),
                            UserId = Convert.ToInt32(dr["UserId"])

                        });

                    }

                }
            }

            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return users;
        }


        public DataTable GenerateLeadReport(string leadCode = "", string companyName = "", string contactPersonName = "", string email = "", string contactNo = "", string companyAddress = "", string companyCity = "", int companyStateId = 0, int leadStatusId = 0, int leadSourceId = 0, int userId = 0, string status = "", int reportingUserId = 0, int reportingRoleId = 0,int leadTypeId=0,string companyBranch="")
        {
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            DataTable dtLead = new DataTable();
            try
            {
                dtLead = sqlDbInterface.GenerateLeadReport(leadCode, companyName, contactPersonName, email, contactNo, companyAddress, companyCity, companyStateId, leadStatusId, leadSourceId, userId, status, reportingUserId, reportingRoleId, leadTypeId, companyBranch);
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dtLead;
        }
    }
}








