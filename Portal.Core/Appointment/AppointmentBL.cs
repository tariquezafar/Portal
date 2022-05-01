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

    public class AppointmentBL
    {
        HRMSDBInterface dbInterface;
        public AppointmentBL()
        {
            dbInterface = new HRMSDBInterface();
        }

        public ResponseOut AddEditAppointment(AppointmentViewModel appointmentViewModel, AppointmentCTCViewModel appointmentCTCViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
             
            try
            {
                HR_Appointment appointment = new HR_Appointment
                {
                    AppointLetterId=appointmentViewModel.AppointLetterId,
                    AppointDate=Convert.ToDateTime(appointmentViewModel.AppointDate),
                    InterviewId = appointmentViewModel.InterviewId,
                    JoiningDate = Convert.ToDateTime(appointmentViewModel.JoiningDate),
                    AppointmentLetterDesc = appointmentViewModel.AppointmentLetterDesc,
                    AppointStatus=appointmentViewModel.AppointStatus,
                    CompanyId = appointmentViewModel.CompanyId,
                    CreatedBy = appointmentViewModel.CreatedBy,
                    CompanyBranchId= appointmentViewModel.companyBranch
                };
                HR_AppointmentCTC appointmentCTC = new HR_AppointmentCTC {
                   AppointLetterId= appointmentCTCViewModel.AppointCTCId,
                   AppointCTCId= appointmentCTCViewModel.AppointLetterId,
                   Basic = appointmentCTCViewModel.Basic,
                   HRAAmount = appointmentCTCViewModel.HRAAmount,
                   Conveyance = appointmentCTCViewModel.Conveyance,
                   Medical = appointmentCTCViewModel.Medical,
                   ChildEduAllow = appointmentCTCViewModel.ChildEduAllow,
                   LTA = appointmentCTCViewModel.LTA,
                   SpecialAllow= appointmentCTCViewModel.SpecialAllow,
                   OtherAllow = appointmentCTCViewModel.OtherAllow,
                   GrossSalary = appointmentCTCViewModel.GrossSalary,
                   EmployeePF= appointmentCTCViewModel.EmployeePF,
                   EmployeeESI = appointmentCTCViewModel.EmployerESI,
                   ProfessionalTax = appointmentCTCViewModel.ProfessionalTax,
                   NetSalary = appointmentCTCViewModel.NetSalary,
                   EmployerESI= appointmentCTCViewModel.EmployerESI,
                   MonthlyCTC= appointmentCTCViewModel.MonthlyCTC,
                   YearlyCTC = appointmentCTCViewModel.YearlyCTC
               };

         responseOut = sqlDbInterface.AddEditAppointment(appointment, appointmentCTC);
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

        public AppointmentViewModel GetAppointmentDetail(long appointLetterId=0)
        {
            AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtAppointment = sqlDbInterface.GetAppointmentDetail(appointLetterId);
                if (dtAppointment != null && dtAppointment.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAppointment.Rows)
                    {
                        appointmentViewModel = new AppointmentViewModel
                        {
                            InterviewNo= Convert.ToString(dr["InterviewNo"]),
                            ApplicantNo = Convert.ToString(dr["ApplicantNo"]),
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Email = Convert.ToString(dr["Email"]),
                            AppointLetterId = Convert.ToInt32(dr["AppointLetterId"]),
                            AppointLetterNo = Convert.ToString(dr["AppointLetterNo"]),
                            AppointDate = Convert.ToString(dr["AppointDate"]),
                            InterviewId = Convert.ToInt64(dr["InterviewId"]),
                            JoiningDate = Convert.ToString(dr["JoiningDate"]),
                            AppointmentLetterDesc = Convert.ToString(dr["AppointmentLetterDesc"]),
                            AppointStatus=Convert.ToString(dr["AppointStatus"]),
                            CreatedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["CreatedByName"])) ? "" : Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = string.IsNullOrEmpty(Convert.ToString(dr["CreatedDate"])) ? "" : Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByName"])) ? "" : Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"])) ? "" : Convert.ToString(dr["ModifiedDate"])
                        };
              
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return appointmentViewModel;
        }

        public AppointmentCTCViewModel GetAppointmentCTCDetail(long appointLetterId = 0)
        {
           
            AppointmentCTCViewModel appointmentCTCViewModel = new AppointmentCTCViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtAppointment = sqlDbInterface.GetAppointmentCTCDetail(appointLetterId);
                if (dtAppointment != null && dtAppointment.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAppointment.Rows)
                    {
                        appointmentCTCViewModel = new AppointmentCTCViewModel
                        {
                                Basic = Convert.ToDecimal(dr["Basic"]),
                                HRAAmount = Convert.ToDecimal(dr["HRAAmount"]),
                                Conveyance = Convert.ToDecimal(dr["Conveyance"]),
                                Medical = Convert.ToDecimal(dr["Medical"]),
                                ChildEduAllow = Convert.ToDecimal(dr["ChildEduAllow"]),
                                LTA = Convert.ToDecimal(dr["LTA"]),
                                SpecialAllow = Convert.ToDecimal(dr["SpecialAllow"]),
                                OtherAllow = Convert.ToDecimal(dr["OtherAllow"]),
                                GrossSalary = Convert.ToDecimal(dr["GrossSalary"]),
                                EmployeePF = Convert.ToDecimal(dr["EmployeePF"]),
                                EmployeeESI = Convert.ToDecimal(dr["EmployeeESI"]),
                                ProfessionalTax = Convert.ToDecimal(dr["ProfessionalTax"]),
                                NetSalary = Convert.ToDecimal(dr["NetSalary"]),
                                EmployerPF = Convert.ToDecimal(dr["EmployerPF"]),
                                EmployerESI = Convert.ToDecimal(dr["EmployerESI"]),
                                MonthlyCTC = Convert.ToDecimal(dr["MonthlyCTC"]),
                                YearlyCTC = Convert.ToDecimal(dr["YearlyCTC"]),
                            };

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return appointmentCTCViewModel;
        }


        public AppointmentViewModel GetApplicantDetailForAppoint(long interviewId)
        {
            AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtAppointment = sqlDbInterface.GetApplicantDetailForAppoint(interviewId);
                if (dtAppointment != null && dtAppointment.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAppointment.Rows)
                    {
                        appointmentViewModel = new AppointmentViewModel
                        {
                            ApplicantNo = Convert.ToString(dr["ApplicantNo"]),
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Email = Convert.ToString(dr["Email"])
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return appointmentViewModel;
        }

        public AppointmentCTCViewModel GetApplicantCTCDetailForAppoint(long interviewId)
        {

            AppointmentCTCViewModel appointmentCTCViewModel = new AppointmentCTCViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtAppointment = sqlDbInterface.GetApplicantCTCDetailForAppoint(interviewId);
                if (dtAppointment != null && dtAppointment.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAppointment.Rows)
                    {
                        appointmentCTCViewModel = new AppointmentCTCViewModel
                        {
                            Basic = Convert.ToDecimal(dr["Basic"]),
                            HRAAmount = Convert.ToDecimal(dr["HRAAmount"]),
                            Conveyance = Convert.ToDecimal(dr["Conveyance"]),
                            Medical = Convert.ToDecimal(dr["Medical"]),
                            ChildEduAllow = Convert.ToDecimal(dr["ChildEduAllow"]),
                            LTA = Convert.ToDecimal(dr["LTA"]),
                            SpecialAllow = Convert.ToDecimal(dr["SpecialAllow"]),
                            OtherAllow = Convert.ToDecimal(dr["OtherAllow"]),
                            GrossSalary = Convert.ToDecimal(dr["GrossSalary"]),
                            EmployeePF = Convert.ToDecimal(dr["EmployeePF"]),
                            EmployeeESI = Convert.ToDecimal(dr["EmployeeESI"]),
                            ProfessionalTax = Convert.ToDecimal(dr["ProfessionalTax"]),
                            NetSalary = Convert.ToDecimal(dr["NetSalary"]),
                            EmployerPF = Convert.ToDecimal(dr["EmployerPF"]),
                            EmployerESI = Convert.ToDecimal(dr["EmployerESI"]),
                            MonthlyCTC = Convert.ToDecimal(dr["MonthlyCTC"]),
                            YearlyCTC = Convert.ToDecimal(dr["YearlyCTC"]),
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return appointmentCTCViewModel;
        }

        public List<AppointmentViewModel> GetAppointmentList(string appointLetterNo, string interviewNo, string applicantName, string appointStatus, int companyId, string fromDate, string toDate,string companyBranch)
        {
            List<AppointmentViewModel> appointments = new List<AppointmentViewModel>();
            HRSQLDBInterface hrsqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtAppointments = hrsqlDbInterface.GetAppointmentList(appointLetterNo, interviewNo, applicantName, appointStatus, companyId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), companyBranch);
                if (dtAppointments != null && dtAppointments.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAppointments.Rows)
                    {
                        appointments.Add(new AppointmentViewModel
                        {
                            InterviewId = Convert.ToInt32(dr["InterviewId"]),
                            InterviewNo = Convert.ToString(dr["InterviewNo"]),
                            ApplicantNo = Convert.ToString(dr["ApplicantNo"]),
                            AppointLetterId= Convert.ToInt32(dr["AppointLetterId"]),
                            FirstName= Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            MobileNo= Convert.ToString(dr["MobileNo"]),
                            Email=Convert.ToString(dr["Email"]),
                            AppointDate=Convert.ToString(dr["AppointDate"]),
                            JoiningDate=Convert.ToString(dr["JoiningDate"]),
                            AppointLetterNo = Convert.ToString(dr["AppointLetterNo"]),
                            AppointStatus = Convert.ToString(dr["AppointStatus"]),
                            CreatedByUserName = Convert.ToString(dr["CreatedByName"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedByUserName =string.IsNullOrEmpty(Convert.ToString(dr["ModifiedByName"]))?"": Convert.ToString(dr["ModifiedByName"]),
                            ModifiedDate = string.IsNullOrEmpty(Convert.ToString(dr["ModifiedDate"]))?"":Convert.ToString(dr["ModifiedDate"]),
                            companyBranchName = Convert.ToString(dr["BranchName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return appointments;
        }

        public AppointmentCTCViewModel GetApplicantCTCDetail(long applicantId)
        {

            AppointmentCTCViewModel appointmentCTCViewModel = new AppointmentCTCViewModel();
            HRSQLDBInterface sqlDbInterface = new HRSQLDBInterface();
            try
            {
                DataTable dtAppointment = sqlDbInterface.GetApplicantCTCDetail(applicantId);
                if (dtAppointment != null && dtAppointment.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAppointment.Rows)
                    {
                        appointmentCTCViewModel = new AppointmentCTCViewModel
                        {
                            Basic = Convert.ToDecimal(dr["Basic"]),
                            HRAAmount = Convert.ToDecimal(dr["HRAAmount"]),
                            Conveyance = Convert.ToDecimal(dr["Conveyance"]),
                            Medical = Convert.ToDecimal(dr["Medical"]),
                            ChildEduAllow = Convert.ToDecimal(dr["ChildEduAllow"]),
                            LTA = Convert.ToDecimal(dr["LTA"]),
                            SpecialAllow = Convert.ToDecimal(dr["SpecialAllow"]),
                            OtherAllow = Convert.ToDecimal(dr["OtherAllow"]),
                            GrossSalary = Convert.ToDecimal(dr["GrossSalary"]),
                            EmployeePF = Convert.ToDecimal(dr["EmployeePF"]),
                            EmployeeESI = Convert.ToDecimal(dr["EmployeeESI"]),
                            ProfessionalTax = Convert.ToDecimal(dr["ProfessionalTax"]),
                            NetSalary = Convert.ToDecimal(dr["NetSalary"]),
                            EmployerPF = Convert.ToDecimal(dr["EmployerPF"]),
                            EmployerESI = Convert.ToDecimal(dr["EmployerESI"]),
                            MonthlyCTC = Convert.ToDecimal(dr["MonthlyCTC"])
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return appointmentCTCViewModel;
        }
    }
}
