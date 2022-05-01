using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using Portal.DAL.Infrastructure;
namespace Portal.DAL
{
    public class HRSQLDBInterface : IDisposable
    {
        private readonly string connectionString = "";
        public HRSQLDBInterface()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SQLConnStr"].ConnectionString;
        }
        #region Dispose Methods
        public void Dispose()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
        }

        #endregion

        #region Employee Advance Application
        public ResponseOut AddEditEmployeeAdvanceApp(HR_EmployeeAdvanceApplication employeeAdvanceApplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeAdvanceApplication", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", employeeAdvanceApplication.ApplicationId);
                        sqlCommand.Parameters.AddWithValue("@ApplicationDate", employeeAdvanceApplication.ApplicationDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeAdvanceApplication.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@AdvanceTypeId", employeeAdvanceApplication.AdvanceTypeId);
                        sqlCommand.Parameters.AddWithValue("@AdvanceAmount", employeeAdvanceApplication.AdvanceAmount);
                        sqlCommand.Parameters.AddWithValue("@AdvanceInstallmentAmount", employeeAdvanceApplication.AdvanceInstallmentAmount);
                        sqlCommand.Parameters.AddWithValue("@AdvanceReason", employeeAdvanceApplication.AdvanceReason);
                        sqlCommand.Parameters.AddWithValue("@AdvanceStartDate", employeeAdvanceApplication.AdvanceStartDate);
                        sqlCommand.Parameters.AddWithValue("@AdvanceEndDate", employeeAdvanceApplication.AdvanceEndDate);
                        sqlCommand.Parameters.AddWithValue("@AdvanceStatus", employeeAdvanceApplication.AdvanceStatus);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeAdvanceApplication.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@companyBranch", employeeAdvanceApplication.CompanyBranchId);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetApplicationId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (employeeAdvanceApplication.ApplicationId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeAdvanceApplicationCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeAdvanceApplicationUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetApplicationId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetApplicationId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }




        public DataTable GetEmployeeAdvanceAppDetail(long applicationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAdvanceAppDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationId", applicationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetEmployeeAdvanceAppList(string applicationNo, int employeeId, string advanceTypeId, string advanceStatus, DateTime fromDate, DateTime toDate, int companyId,string companyBranch)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAdvanceApps", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@AdvanceTypeId", advanceTypeId);
                    da.SelectCommand.Parameters.AddWithValue("@AdvanceStatus", advanceStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId); 
                    da.SelectCommand.Parameters.AddWithValue("@companyBranch", companyBranch);

                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion


        #region Employee Advance Application Approval
        public DataTable GetEmployeeAdvanceAppApprovalList(string applicationNo, int employeeId, string advanceTypeId, string advanceStatus, DateTime fromDate, DateTime toDate, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAdvanceAppApprovalList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@AdvanceTypeId", advanceTypeId);
                    da.SelectCommand.Parameters.AddWithValue("@AdvanceStatus", advanceStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion



        #region Applicant

        public ResponseOut AddEditApplicant(HR_Applicant applicant, List<HR_ApplicantEducation> educationList, HR_ApplicantExtraActivity extraActivityList, List<HR_ApplicantPrevEmployer> employerList, List<HR_ApplicantProject> projectList)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {
                DataTable dtApplicantEducation = new DataTable();
                dtApplicantEducation.Columns.Add("EducationId", typeof(int));
                dtApplicantEducation.Columns.Add("RegularDistant", typeof(string));
                dtApplicantEducation.Columns.Add("BoardUniversityName", typeof(string));
                dtApplicantEducation.Columns.Add("PercentageObtained", typeof(decimal));

                if (educationList != null && educationList.Count > 0)
                {
                    foreach (HR_ApplicantEducation item in educationList)
                    {
                        DataRow dtrow = dtApplicantEducation.NewRow();
                        dtrow["EducationId"] = item.EducationId;
                        dtrow["RegularDistant"] = item.RegularDistant;
                        dtrow["BoardUniversityName"] = item.BoardUniversityName;
                        dtrow["PercentageObtained"] = item.PercentageObtained;
                        dtApplicantEducation.Rows.Add(dtrow);
                    }
                    dtApplicantEducation.AcceptChanges();
                }


                DataTable dtApplicantEmployer = new DataTable();
                dtApplicantEmployer.Columns.Add("CurrentEmployer", typeof(bool));
                dtApplicantEmployer.Columns.Add("EmployerName", typeof(string));
                dtApplicantEmployer.Columns.Add("StartDate", typeof(DateTime));
                dtApplicantEmployer.Columns.Add("EndDate", typeof(DateTime));
                dtApplicantEmployer.Columns.Add("LastCTC", typeof(decimal));
                dtApplicantEmployer.Columns.Add("ReasonOfLeaving", typeof(string));
                dtApplicantEmployer.Columns.Add("LastDesignationId", typeof(int));
                dtApplicantEmployer.Columns.Add("EmploymentStatusId", typeof(int));

                if (employerList != null && employerList.Count > 0)
                {
                    foreach (HR_ApplicantPrevEmployer item in employerList)
                    {
                        DataRow dtrow = dtApplicantEmployer.NewRow();
                        dtrow["CurrentEmployer"] = item.CurrentEmployer;
                        dtrow["EmployerName"] = item.EmployerName;
                        dtrow["StartDate"] = item.StartDate;
                        dtrow["EndDate"] = item.EndDate;
                        dtrow["LastCTC"] = item.LastCTC;
                        dtrow["ReasonOfLeaving"] = item.ReasonOfLeaving;
                        dtrow["LastDesignationId"] = item.LastDesignationId;
                        dtrow["EmploymentStatusId"] = item.EmploymentStatusId;
                        dtApplicantEmployer.Rows.Add(dtrow);
                    }
                    dtApplicantEmployer.AcceptChanges();
                }

                DataTable dtApplicantProject = new DataTable();
                dtApplicantProject.Columns.Add("ProjectName", typeof(string));
                dtApplicantProject.Columns.Add("ClientName", typeof(string));
                dtApplicantProject.Columns.Add("RoleDesc", typeof(string));
                dtApplicantProject.Columns.Add("TeamSize", typeof(int));
                dtApplicantProject.Columns.Add("ProjectDesc", typeof(string));
                dtApplicantProject.Columns.Add("TechnologiesUsed", typeof(string));

                if (projectList != null && projectList.Count > 0)
                {
                    foreach (HR_ApplicantProject item in projectList)
                    {
                        DataRow dtrow = dtApplicantProject.NewRow();
                        dtrow["ProjectName"] = item.ProjectName;
                        dtrow["ClientName"] = item.ClientName;
                        dtrow["RoleDesc"] = item.RoleDesc;
                        dtrow["TeamSize"] = item.TeamSize;
                        dtrow["ProjectDesc"] = item.ProjectDesc;
                        dtrow["TechnologiesUsed"] = item.TechnologiesUsed;
                        dtApplicantProject.Rows.Add(dtrow);
                    }
                    dtApplicantProject.AcceptChanges();
                }




                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditApplicant", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicantId", applicant.ApplicantId);
                        sqlCommand.Parameters.AddWithValue("@ApplicationDate", applicant.ApplicationDate);
                        sqlCommand.Parameters.AddWithValue("@JobOpeningId", applicant.JobOpeningId);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", applicant.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", applicant.CompanyBranchId);
                        sqlCommand.Parameters.AddWithValue("@ProjectNo", applicant.ProjectNo);
                        sqlCommand.Parameters.AddWithValue("@FirstName", applicant.FirstName);
                        sqlCommand.Parameters.AddWithValue("@LastName", applicant.LastName);
                        sqlCommand.Parameters.AddWithValue("@Gender", applicant.Gender);

                        sqlCommand.Parameters.AddWithValue("@FatherSpouseName", applicant.FatherSpouseName);
                        sqlCommand.Parameters.AddWithValue("@DOB", applicant.DOB);
                        sqlCommand.Parameters.AddWithValue("@BloodGroup", applicant.BloodGroup);
                        sqlCommand.Parameters.AddWithValue("@MaritalStatus", applicant.MaritalStatus);
                        sqlCommand.Parameters.AddWithValue("@ApplicantAddress", applicant.ApplicantAddress);
                        sqlCommand.Parameters.AddWithValue("@City", applicant.City);
                        sqlCommand.Parameters.AddWithValue("@StateId", applicant.StateId);
                        sqlCommand.Parameters.AddWithValue("@CountryId", applicant.CountryId);
                        sqlCommand.Parameters.AddWithValue("@PinCode", applicant.PinCode);
                        sqlCommand.Parameters.AddWithValue("@ContactNo", applicant.ContactNo);
                        sqlCommand.Parameters.AddWithValue("@MobileNo", applicant.MobileNo);
                        sqlCommand.Parameters.AddWithValue("@Email", applicant.Email);
                        sqlCommand.Parameters.AddWithValue("@ResumeSourceId", applicant.ResumeSourceId);
                        sqlCommand.Parameters.AddWithValue("@PositionAppliedId", applicant.PositionAppliedId);
                        sqlCommand.Parameters.AddWithValue("@TotalExperience", applicant.TotalExperience);
                        sqlCommand.Parameters.AddWithValue("@ReleventExperience", applicant.ReleventExperience);
                        sqlCommand.Parameters.AddWithValue("@NoticePeriod", applicant.NoticePeriod);
                        sqlCommand.Parameters.AddWithValue("@CurrentCTC", applicant.CurrentCTC);
                        sqlCommand.Parameters.AddWithValue("@ExpectedCTC", applicant.ExpectedCTC);
                        sqlCommand.Parameters.AddWithValue("@PreferredLocation", applicant.PreferredLocation);
                        sqlCommand.Parameters.AddWithValue("@ResumeText", applicant.ResumeText);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", applicant.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@ApplicantStatus", applicant.ApplicantStatus);
                        sqlCommand.Parameters.AddWithValue("@Strength1", extraActivityList.Strength1);
                        sqlCommand.Parameters.AddWithValue("@Strength2", extraActivityList.Strength2);
                        sqlCommand.Parameters.AddWithValue("@Strength3", extraActivityList.Strength3);
                        sqlCommand.Parameters.AddWithValue("@Weakness1", extraActivityList.Weakness1);
                        sqlCommand.Parameters.AddWithValue("@Weakness2", extraActivityList.Weakness2);
                        sqlCommand.Parameters.AddWithValue("@Weakness3", extraActivityList.Weakness3);
                        sqlCommand.Parameters.AddWithValue("@Hobbies", extraActivityList.Hobbies);

                        sqlCommand.Parameters.AddWithValue("@ApplicantEducationDetail", dtApplicantEducation);
                        sqlCommand.Parameters.AddWithValue("@ApplicantPrevEmployerDetail", dtApplicantEmployer);
                        sqlCommand.Parameters.AddWithValue("@ApplicantProjectDetail", dtApplicantProject);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetApplicantId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (applicant.ApplicantId == 0)
                            {
                                responseOut.message = ActionMessage.ApplicantCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.ApplicantUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetApplicantId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetApplicantId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        public DataTable GetApplicantEducationList(long applicantId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetApplicantEducations", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantId", applicantId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetApplicantPrevEmployerList(long applicantId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetApplicantPrevEmployers", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantId", applicantId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetApplicantProjectList(long applicantId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetApplicantProjects", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantId", applicantId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetApplicantList(string applicantNo, string projectNo, string firstName, string lastName, Int32 resumeSource, Int32 designation, DateTime fromDate, DateTime toDate, int companyId, string applicantStatus = "Final")
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetApplicants", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantNo", applicantNo);
                    da.SelectCommand.Parameters.AddWithValue("@ProjectNo", projectNo);
                    da.SelectCommand.Parameters.AddWithValue("@FirstName", firstName);
                    da.SelectCommand.Parameters.AddWithValue("@LastName", lastName);
                    da.SelectCommand.Parameters.AddWithValue("@ResumeSource", resumeSource);
                    da.SelectCommand.Parameters.AddWithValue("@Designation", designation);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantStatus", applicantStatus);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetApplicantDetail(long applicantId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetApplicantDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantId", applicantId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetApplicantExtraActivityDetail(long applicantId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetApplicantExtraActivityDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantId", applicantId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion


        #region Employee Travel Application
        public ResponseOut AddEditEmployeeTravelApp(HR_EmployeeTravelApplication employeeTravelApplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeTravelApplication", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", employeeTravelApplication.ApplicationId);
                        sqlCommand.Parameters.AddWithValue("@ApplicationDate", employeeTravelApplication.ApplicationDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeTravelApplication.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@TravelTypeId", employeeTravelApplication.TravelTypeId);
                        sqlCommand.Parameters.AddWithValue("@TravelReason", employeeTravelApplication.TravelReason);
                        sqlCommand.Parameters.AddWithValue("@TravelDestination", employeeTravelApplication.TravelDestination);
                        sqlCommand.Parameters.AddWithValue("@TravelStartDate", employeeTravelApplication.TravelStartDate);
                        sqlCommand.Parameters.AddWithValue("@TravelEndDate", employeeTravelApplication.TravelEndDate);
                        sqlCommand.Parameters.AddWithValue("@TravelStatus", employeeTravelApplication.TravelStatus);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeTravelApplication.CompanyId);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetApplicationId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (employeeTravelApplication.ApplicationId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeTravelApplicationCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeTravelApplicationUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetApplicationId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetApplicationId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }




        public DataTable GetEmployeeTravelAppDetail(long applicationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeTravelAppDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationId", applicationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetEmployeeTravelAppList(string applicationNo, int employeeId, string travelTypeId, string travelStatus, string travelDestination, DateTime fromDate, DateTime toDate, int companyId,string companyBranch)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeTravelApps", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@TravelTypeId", travelTypeId);
                    da.SelectCommand.Parameters.AddWithValue("@TravelStatus", travelStatus);
                    da.SelectCommand.Parameters.AddWithValue("@TravelDestination", travelDestination);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);


                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion

        #region EmployeeLoanApplication

        public ResponseOut AddEditEmployeeLoanApplication(HR_EmployeeLoanApplication employeeLoanApplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeLoanApplication", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", employeeLoanApplication.ApplicationId);
                        // sqlCommand.Parameters.AddWithValue("@ApplicationNo", employeeLoanApplication.ApplicationNo);
                        sqlCommand.Parameters.AddWithValue("@ApplicationDate", employeeLoanApplication.ApplicationDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeLoanApplication.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@LoanTypeId", employeeLoanApplication.LoanTypeId);
                        sqlCommand.Parameters.AddWithValue("@LoanInterestRate", employeeLoanApplication.LoanInterestRate);
                        sqlCommand.Parameters.AddWithValue("@InterestCalcOn", employeeLoanApplication.InterestCalcOn);
                        sqlCommand.Parameters.AddWithValue("@LoanAmount", employeeLoanApplication.LoanAmount);
                        sqlCommand.Parameters.AddWithValue("@LoanStartDate", employeeLoanApplication.LoanStartDate);
                        sqlCommand.Parameters.AddWithValue("@LoanEndDate", employeeLoanApplication.LoanEndDate);
                        sqlCommand.Parameters.AddWithValue("@LoanInstallmentAmount", employeeLoanApplication.LoanInstallmentAmount);
                        sqlCommand.Parameters.AddWithValue("@LoanReason", employeeLoanApplication.LoanReason);
                        sqlCommand.Parameters.AddWithValue("@LoanStatus", employeeLoanApplication.LoanStatus);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeLoanApplication.CompanyId);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetApplicationId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (employeeLoanApplication.ApplicationId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeLoanApplicationCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeLoanApplicationUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetApplicationId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetApplicationId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        public DataTable GetEmployeeLoanApplicationList(string applicationNo, int employeeId, string loanTypeName, string loanStatus, DateTime fromDate, DateTime toDate, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeLoanApplicationList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@loanTypeId", loanTypeName);
                    da.SelectCommand.Parameters.AddWithValue("@loanStatus", loanStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetEmployeeLoanApplicationDetail(long applicationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeLoanApplicationDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationId", applicationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region EmployeeLoanApplicationApproval

        public DataTable GetEmployeeLoanApplicationApprovalList(string applicationNo, int employeeId, string loanTypeName, string loanStatus, DateTime fromDate, DateTime toDate, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeLoanApplicationApprovalList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@loanTypeId", loanTypeName);
                    da.SelectCommand.Parameters.AddWithValue("@loanStatus", loanStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetEmployeeLoanApplicationApprovalDetail(long applicationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeLoanApplicationApprovalDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationId", applicationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region Employee Asset Application Approval

        public DataTable GetEmployeeAssetApplicationApprovalList(string applicationNo, int employeeId, string loanTypeName, string loanStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAssetApplicationApprovalList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@AssetTypeId", loanTypeName);
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationStatus", loanStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId); 
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion

        #region Interview
        public ResponseOut AddEditInterview(HR_Interview interview)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditInterview", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@InterviewId", interview.InterviewId);
                        sqlCommand.Parameters.AddWithValue("@ApplicantId", interview.ApplicantId);

                        sqlCommand.Parameters.AddWithValue("@AptitudeTestStatus", interview.AptitudeTestStatus);
                        sqlCommand.Parameters.AddWithValue("@AptitudeTestRemarks ", interview.AptitudeTestRemarks);
                        sqlCommand.Parameters.AddWithValue("@AptitudeTestTotalMarks", interview.AptitudeTestTotalMarks);
                        sqlCommand.Parameters.AddWithValue("@AptitudeTestMarkObtained", interview.AptitudeTestMarkObtained);

                        sqlCommand.Parameters.AddWithValue("@TechnicalRound1_Status", interview.TechnicalRound1_Status);
                        sqlCommand.Parameters.AddWithValue("@TechnicalRound1_Remarks  ", interview.TechnicalRound1_Remarks);
                        sqlCommand.Parameters.AddWithValue("@TechnicalRound1_TotalMarks", interview.TechnicalRound1_TotalMarks);
                        sqlCommand.Parameters.AddWithValue("@TechnicalRound1_MarkObtained", interview.TechnicalRound1_MarkObtained);

                        sqlCommand.Parameters.AddWithValue("@TechnicalRound2_Status", interview.TechnicalRound2_Status);
                        sqlCommand.Parameters.AddWithValue("@TechnicalRound2_Remarks  ", interview.TechnicalRound2_Remarks);
                        sqlCommand.Parameters.AddWithValue("@TechnicalRound2_TotalMarks", interview.TechnicalRound2_TotalMarks);
                        sqlCommand.Parameters.AddWithValue("@TechnicalRound2_MarkObtained", interview.TechnicalRound2_MarkObtained);


                        sqlCommand.Parameters.AddWithValue("@TechnicalRound3_Status", interview.TechnicalRound3_Status);
                        sqlCommand.Parameters.AddWithValue("@TechnicalRound3_Remarks  ", interview.TechnicalRound3_Remarks);
                        sqlCommand.Parameters.AddWithValue("@TechnicalRound3_TotalMarks", interview.TechnicalRound3_TotalMarks);
                        sqlCommand.Parameters.AddWithValue("@TechnicalRound3_MarkObtained", interview.TechnicalRound3_MarkObtained);


                        sqlCommand.Parameters.AddWithValue("@MachineRound_Status", interview.MachineRound_Status);
                        sqlCommand.Parameters.AddWithValue("@MachineRound_Remarks", interview.MachineRound_Remarks);
                        sqlCommand.Parameters.AddWithValue("@MachineRound_TotalMarks", interview.MachineRound_TotalMarks);
                        sqlCommand.Parameters.AddWithValue("@MachineRound_MarkObtained", interview.MachineRound_MarkObtained);

                        sqlCommand.Parameters.AddWithValue("@HRRound_Status", interview.HRRound_Status);
                        sqlCommand.Parameters.AddWithValue("@HRRound_Remarks", interview.HRRound_Remarks);
                        sqlCommand.Parameters.AddWithValue("@HRRound_TotalMarks", interview.HRRound_TotalMarks);
                        sqlCommand.Parameters.AddWithValue("@HRRound_MarkObtained", interview.HRRound_MarkObtained);

                        sqlCommand.Parameters.AddWithValue("@FinalRemarks", interview.FinalRemarks);
                        sqlCommand.Parameters.AddWithValue("@InterviewFinalStatus", interview.InterviewFinalStatus);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", interview.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", interview.CompanyId); 
                        sqlCommand.Parameters.AddWithValue("@companyBranch", interview.CompanyBranchId);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetInterviewId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (interview.InterviewId == 0)
                            {
                                responseOut.message = ActionMessage.InterviewCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.InterviewUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetInterviewId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetInterviewId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        public DataTable GetInterviewDetail(long interviewId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetInterviewDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@InterviewId", interviewId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetInterviewList(string interviewNo, string applicantNo, string interviewFinalStatus, int companyId, DateTime fromDate, DateTime toDate,string companyBranch)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetInterviews", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@InterviewNo", interviewNo);
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantNo", applicantNo);
                    da.SelectCommand.Parameters.AddWithValue("@InterviewFinalStatus", interviewFinalStatus);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate); 
                    da.SelectCommand.Parameters.AddWithValue("@companyBranch", companyBranch);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion

        #region Appointment
        public ResponseOut AddEditAppointment(HR_Appointment appointment, HR_AppointmentCTC appointmentCTC)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditAppointment", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@AppointLetterId", appointment.AppointLetterId);
                        sqlCommand.Parameters.AddWithValue("@AppointDate", appointment.AppointDate);
                        sqlCommand.Parameters.AddWithValue("@InterviewId", appointment.InterviewId);
                        sqlCommand.Parameters.AddWithValue("@JoiningDate", appointment.JoiningDate);
                        sqlCommand.Parameters.AddWithValue("@AppointmentLetterDesc", appointment.AppointmentLetterDesc);
                        sqlCommand.Parameters.AddWithValue("@AppointStatus", appointment.AppointStatus);
                        sqlCommand.Parameters.AddWithValue("@Basic", appointmentCTC.Basic);
                        sqlCommand.Parameters.AddWithValue("@HRAAmount", appointmentCTC.HRAAmount);
                        sqlCommand.Parameters.AddWithValue("@Conveyance", appointmentCTC.Conveyance);
                        sqlCommand.Parameters.AddWithValue("@Medical ", appointmentCTC.Medical);
                        sqlCommand.Parameters.AddWithValue("@ChildEduAllow", appointmentCTC.ChildEduAllow);
                        sqlCommand.Parameters.AddWithValue("@LTA", appointmentCTC.LTA);
                        sqlCommand.Parameters.AddWithValue("@SpecialAllow", appointmentCTC.SpecialAllow);
                        sqlCommand.Parameters.AddWithValue("@OtherAllow", appointmentCTC.OtherAllow);
                        sqlCommand.Parameters.AddWithValue("@GrossSalary", appointmentCTC.GrossSalary);
                        sqlCommand.Parameters.AddWithValue("@EmployeePF", appointmentCTC.EmployeePF);
                        sqlCommand.Parameters.AddWithValue("@EmployeeESI", appointmentCTC.EmployerESI);
                        sqlCommand.Parameters.AddWithValue("@ProfessionalTax", appointmentCTC.ProfessionalTax);
                        sqlCommand.Parameters.AddWithValue("@NetSalary", appointmentCTC.NetSalary);
                        sqlCommand.Parameters.AddWithValue("@EmployerPF", appointmentCTC.EmployerPF);
                        sqlCommand.Parameters.AddWithValue("@EmployerESI", appointmentCTC.EmployerESI);
                        sqlCommand.Parameters.AddWithValue("@MonthlyCTC", appointmentCTC.MonthlyCTC);
                        sqlCommand.Parameters.AddWithValue("@YearlyCTC", appointmentCTC.YearlyCTC);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", appointment.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", appointment.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", appointment.CompanyBranchId);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetAppointLetterId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (appointment.AppointLetterId == 0)
                            {
                                responseOut.message = ActionMessage.AppointmentCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.AppointmentUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetAppointLetterId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetAppointLetterId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        public DataTable GetAppointmentDetail(long appointLetterId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetAppointmentDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@AppointLetterId", appointLetterId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetAppointmentCTCDetail(long appointLetterId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetAppointmentCTCDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@AppointLetterId", appointLetterId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetApplicantDetailForAppoint(long interviewId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetApplicantDetailForAppoint", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@InterviewId", interviewId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetApplicantCTCDetailForAppoint(long interviewId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetApplicantCTCDetailForAppoint", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@InterviewId", interviewId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetAppointmentList(string appointLetterNo, string interviewNo, string applicantName, string appointStatus, int companyId, DateTime fromDate, DateTime toDate,string companyBranch)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetAppointments", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@AppointLetterNo", appointLetterNo);
                    da.SelectCommand.Parameters.AddWithValue("@InterviewNo", interviewNo);
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantName", applicantName);
                    da.SelectCommand.Parameters.AddWithValue("@AppointStatus", appointStatus);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate); 
                    da.SelectCommand.Parameters.AddWithValue("@companyBranch", companyBranch);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetApplicantCTCDetail(long applicantId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetApplicantCTCDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantId", applicantId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region EmployeeAssetApplication
        public ResponseOut AddEditEmployeeAssetApplication(HR_EmployeeAssetApplication employeeAssetApplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeAssetApplication", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", employeeAssetApplication.ApplicationId);
                        sqlCommand.Parameters.AddWithValue("@ApplicationDate", employeeAssetApplication.ApplicationDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeAssetApplication.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@AssetTypeId", employeeAssetApplication.AssetTypeId);

                        sqlCommand.Parameters.AddWithValue("@AssetReason", employeeAssetApplication.AssetReason);
                        sqlCommand.Parameters.AddWithValue("@AssetStatus", true);
                        sqlCommand.Parameters.AddWithValue("@ApplicationStatus",employeeAssetApplication.ApplicationStatus);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeAssetApplication.CompanyId);

                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", employeeAssetApplication.CompanyBranchId);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetApplicationId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (employeeAssetApplication.ApplicationId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeAssetApplicationCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeAssetApplicationUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetApplicationId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetApplicationId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        public DataTable GetEmployeeAssetApplicationList(string applicationNo, int employeeId, string assetTypeName, string assetStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAssetApplicationList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@AssetTypeId", assetTypeName);
                    da.SelectCommand.Parameters.AddWithValue("@assetStatus", assetStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId); 
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetEmployeeAssetApplicationDetail(long applicationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAssetApplicationDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationId", applicationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region Employee Travel Application Approval
        public DataTable GetEmployeeTravelAppApprovalList(string applicationNo, int employeeId, string travelTypeId, string travelStatus, string travelDestination, DateTime fromDate, DateTime toDate, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeTravelAppApprovalList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@TravelTypeId", travelTypeId);
                    da.SelectCommand.Parameters.AddWithValue("@TravelStatus", travelStatus);
                    da.SelectCommand.Parameters.AddWithValue("@TravelDestination", travelDestination);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);


                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }
        #endregion

        #region Employee Claim Application
        public ResponseOut AddEditEmployeeClaimApplication(HR_EmployeeClaimApplication employeeClaimApplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeClaimApplication", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", employeeClaimApplication.ApplicationId);
                        sqlCommand.Parameters.AddWithValue("@ApplicationDate", employeeClaimApplication.ApplicationDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeClaimApplication.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeClaimApplication.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@ClaimTypeId", employeeClaimApplication.ClaimTypeId);
                        sqlCommand.Parameters.AddWithValue("@ClaimAmount", employeeClaimApplication.ClaimAmount);
                        sqlCommand.Parameters.AddWithValue("@ClaimReason", employeeClaimApplication.ClaimReason);
                        sqlCommand.Parameters.AddWithValue("@ClaimStatus", employeeClaimApplication.ClaimStatus);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", employeeClaimApplication.CompanyBranchId);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetApplicationId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (employeeClaimApplication.ApplicationId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeClaimApplicationCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeClaimApplicationUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetApplicationId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetApplicationId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        public DataTable GetEmployeeClaimApplicationList(string applicationNo, int employeeId, int claimTypeId, string claimStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeClaimApplicationList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@ClaimTypeId", claimTypeId);
                    da.SelectCommand.Parameters.AddWithValue("@ClaimStatus", claimStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId); 
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetEmployeeClaimApplicationDetail(long applicationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("[proc_GetEmployeeClaimApplicationDetails]", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationId", applicationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region Shortlist Applicant
        public DataTable GetShortlistApplicantList(string applicantNo, string projectNo, string firstName, string lastName, Int32 resumeSource, Int32 designation, DateTime fromDate, DateTime toDate, int companyId, string applicantShortlistStatus = "Shortlist")
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetShortlistApplicants", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantNo", applicantNo);
                    da.SelectCommand.Parameters.AddWithValue("@ProjectNo", projectNo);
                    da.SelectCommand.Parameters.AddWithValue("@FirstName", firstName);
                    da.SelectCommand.Parameters.AddWithValue("@LastName", lastName);
                    da.SelectCommand.Parameters.AddWithValue("@ResumeSource", resumeSource);
                    da.SelectCommand.Parameters.AddWithValue("@Designation", designation);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantStortlistStatus", applicantShortlistStatus);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public ResponseOut ShortlistApplicant(HR_Applicant applicant, HR_ApplicantVerification verificationList)
        {

            ResponseOut responseOut = new ResponseOut();
            try
            {

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_ShortlistApplicant", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicantId", applicant.ApplicantId);

                        sqlCommand.Parameters.AddWithValue("@CreatedBy", applicant.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@ApplicantStortlistStatus", applicant.ApplicantStortlistStatus);
                        sqlCommand.Parameters.AddWithValue("@VerificationAgencyId", verificationList.VerificationAgencyId);
                        sqlCommand.Parameters.AddWithValue("@VerificationDate", verificationList.VerificationDate);
                        sqlCommand.Parameters.AddWithValue("@VerificationCharges", verificationList.VerificationCharges);
                        sqlCommand.Parameters.AddWithValue("@VerificationStatus", verificationList.VerificationStatus);
                        sqlCommand.Parameters.AddWithValue("@Remarks", verificationList.Remarks);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetApplicantId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            responseOut.message = ActionMessage.ApplicantUpdatedSuccess;
                            if (sqlCommand.Parameters["@RetApplicantId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetApplicantId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        public DataTable GetApplicantVerificationDetail(long applicantId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetApplicantVerificationDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicantId", applicantId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region Employee Leave Application
        public ResponseOut AddEditEmployeeLeaveApplication(HR_EmployeeLeaveApplication employeeleaveApplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeLeaveApplication", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", employeeleaveApplication.ApplicationId);
                        sqlCommand.Parameters.AddWithValue("@ApplicationDate", employeeleaveApplication.ApplicationDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeleaveApplication.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@LeaveTypeId", employeeleaveApplication.LeaveTypeId);
                        sqlCommand.Parameters.AddWithValue("@LeaveReason", employeeleaveApplication.LeaveReason);
                        sqlCommand.Parameters.AddWithValue("@NoofDays", employeeleaveApplication.NoofDays);
                        sqlCommand.Parameters.AddWithValue("@FromDate", employeeleaveApplication.FromDate);
                        sqlCommand.Parameters.AddWithValue("@ToDate", employeeleaveApplication.ToDate);
                        sqlCommand.Parameters.AddWithValue("@LeaveStatus", employeeleaveApplication.LeaveStatus);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeleaveApplication.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", employeeleaveApplication.CompanyBranchId);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetApplicationId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (employeeleaveApplication.ApplicationId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeLeaveApplicationCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeLeaveApplicationUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetApplicationId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetApplicationId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        #region Lead Type Master
        public ResponseOut AddEditLeadTypeMaster(LeadMaster LeadMaster)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("LeadMasterAddEdit", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@LeadId", LeadMaster.LeadTypeId);
                        sqlCommand.Parameters.AddWithValue("@Companyid", LeadMaster.Companyid);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchid", LeadMaster.CompanyBranchid);
                        sqlCommand.Parameters.AddWithValue("@LeadName", LeadMaster.LeadTypeName);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", LeadMaster.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@Istrue", LeadMaster.status);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@LeadIdout", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (LeadMaster.LeadTypeId == 0)
                            {
                                responseOut.message = ActionMessage.LeadTypeMasterCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.LeadTypeMasterUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@LeadIdout"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@LeadIdout"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        #endregion

        #region Payroll Tds
        public ResponseOut AddEditPayrollTds(PR_PayrollTdsSlab PayrollTds)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_PayrollTdsSlab", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@TdsSlabid", PayrollTds.TdsSlaBid);
                        sqlCommand.Parameters.AddWithValue("@Companyid", PayrollTds.Companyid);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchid", PayrollTds.CompanyBranchid);
                        sqlCommand.Parameters.AddWithValue("@FromDate", PayrollTds.FromDate);
                        sqlCommand.Parameters.AddWithValue("@ToDate", PayrollTds.ToDate);
                        sqlCommand.Parameters.AddWithValue("@FromAmount", PayrollTds.FromAmount);
                        sqlCommand.Parameters.AddWithValue("@ToAmount", PayrollTds.ToAmount);
                        sqlCommand.Parameters.AddWithValue("@Category", PayrollTds.Category);
                        sqlCommand.Parameters.AddWithValue("@TDSPerc", PayrollTds.TDSPerc);
                        sqlCommand.Parameters.AddWithValue("@CessPerc", PayrollTds.CessPerc);
                        sqlCommand.Parameters.AddWithValue("@SurcharegePerc", PayrollTds.SurcharegePerc);
                        sqlCommand.Parameters.AddWithValue("@SurchargePerc2", PayrollTds.SurchargePerc2);
                        sqlCommand.Parameters.AddWithValue("@SurchargePerc3", PayrollTds.SurchargePerc3);
                        sqlCommand.Parameters.AddWithValue("@YearlyDiscount", PayrollTds.YearlyDiscount);
                        sqlCommand.Parameters.AddWithValue("@MonthlyDiscount", PayrollTds.MonthlyDiscount);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", PayrollTds.CreatedBy);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@TdsSlabidout", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (PayrollTds.TdsSlaBid == 0)
                            {
                                responseOut.message = ActionMessage.PayrollTdsCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.PayrollTdsUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@TdsSlabidout"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@TdsSlabidout"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        #endregion
        public DataTable GetEmployeeLeaveApplicationDetail(long applicationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeLeaveApplicationDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationId", applicationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetEmployeeLeaveApplicationList(string applicationNo, int employeeId, string leaveTypeId, string leaveStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeLeaveApps", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@LeaveTypeId", leaveTypeId);
                    da.SelectCommand.Parameters.AddWithValue("@LeaveStatus", leaveStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId); 
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);

                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion

        #region Employee Leave Application Approval
        public DataTable GetEmployeeLeaveApplicationApprovalList(string applicationNo, int employeeId, string leaveTypeId, string leaveStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeLeaveApplicationApprovalList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@LeaveTypeId", leaveTypeId);
                    da.SelectCommand.Parameters.AddWithValue("@LeaveStatus", leaveStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);


                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }

        public DataTable GetEmployeeLeaveApplicationDetails(long applicationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("[proc_GetEmployeeLeaveApplicationDetails]", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationId", applicationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region Employee Claim Application Approval

        public ResponseOut ApproveRejectEmployeeClaimApplication(HR_EmployeeClaimApplication employeeClaimApplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeClaimApplicationApproval", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", employeeClaimApplication.ApplicationId);

                        sqlCommand.Parameters.AddWithValue("@ClaimStatus", employeeClaimApplication.ClaimStatus);
                        sqlCommand.Parameters.AddWithValue("@RejectBy", employeeClaimApplication.ApproveBy);
                        sqlCommand.Parameters.AddWithValue("@RejectDate", DateTime.Now);
                        sqlCommand.Parameters.AddWithValue("@RejectReason", employeeClaimApplication.RejectReason);
                        sqlCommand.Parameters.AddWithValue("@ApproveBy", employeeClaimApplication.ApproveBy);
                        sqlCommand.Parameters.AddWithValue("@ApproveDate", DateTime.Now);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        responseOut.message = ActionMessage.EmployeeClaimApplicationStatusUpdatedSuccess;




                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        public DataTable GetEmployeeClaimApplicationApprovalList(string applicationNo, int employeeId, int claimTypeId, string claimStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeClaimApplicationApprovalList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@ClaimTypeId", claimTypeId);
                    da.SelectCommand.Parameters.AddWithValue("@ClaimStatus", claimStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId); 
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion

        #region Shift
        public ResponseOut AddEditShift(HR_Shift shift)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditShift", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ShiftId", shift.ShiftId);
                        sqlCommand.Parameters.AddWithValue("@ShiftName", shift.ShiftName);

                        sqlCommand.Parameters.AddWithValue("@ShiftDescription", shift.ShiftDescription);
                        sqlCommand.Parameters.AddWithValue("@ShiftTypeId ", shift.ShiftTypeId);
                        sqlCommand.Parameters.AddWithValue("@NormalPaidHours", shift.NormalPaidHours);
                        sqlCommand.Parameters.AddWithValue("@Allowance", shift.Allowance);

                        sqlCommand.Parameters.AddWithValue("@ShiftStartTime", new DateTime(1900, 1, 1, shift.ShiftStartTime.Value.Hours, shift.ShiftStartTime.Value.Minutes, 0));
                        sqlCommand.Parameters.AddWithValue("@ShiftEndTime  ", new DateTime(1900, 1, 1, shift.ShiftEndTime.Value.Hours, shift.ShiftEndTime.Value.Minutes, 0));
                        sqlCommand.Parameters.AddWithValue("@LateArrivalLimit", new DateTime(1900, 1, 1, shift.LateArrivalLimit.Value.Hours, shift.LateArrivalLimit.Value.Minutes, 0));
                        sqlCommand.Parameters.AddWithValue("@EarlyGoLimit", new DateTime(1900, 1, 1, shift.EarlyGoLimit.Value.Hours, shift.EarlyGoLimit.Value.Minutes, 0));

                        sqlCommand.Parameters.AddWithValue("@OvertimeStartTime", new DateTime(1900, 1, 1, shift.OvertimeStartTime.Value.Hours, shift.OvertimeStartTime.Value.Minutes, 0));
                        sqlCommand.Parameters.AddWithValue("@ValidTill  ", shift.ValidTill);
                        sqlCommand.Parameters.AddWithValue("@FullDayWorkHour", shift.FullDayWorkHour);
                        sqlCommand.Parameters.AddWithValue("@HalfDayWorkHour", shift.HalfDayWorkHour);

                        sqlCommand.Parameters.AddWithValue("@ShiftStatus", shift.Status);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", shift.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", shift.CompanyId);

                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", shift.CompanyBranchId);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetShiftId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (shift.ShiftId == 0)
                            {
                                responseOut.message = ActionMessage.ShiftCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.ShiftUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetShiftId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetShiftId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        #endregion

        #region Update Employee Roster
        public DataTable GetRoasterLinkedEmployeeDetail(int roasterId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetRoasterLinkedEmployeeDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@RoasterId", roasterId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public ResponseOut UpdateEmployeeRosterShift(int rosterId, int shiftId, DateTime startDate, DateTime endDate, int createdBy,int companyBranchId, List<HR_EmployeeRoster> employeeRosterList)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                DataTable dtEmployeeList = new DataTable();
                dtEmployeeList.Columns.Add("EmployeeId", typeof(int));
                if (employeeRosterList != null && employeeRosterList.Count > 0)
                {
                    foreach (HR_EmployeeRoster item in employeeRosterList)
                    {
                        DataRow dtrow = dtEmployeeList.NewRow();
                        dtrow["EmployeeId"] = item.EmployeeId;

                        dtEmployeeList.Rows.Add(dtrow);
                    }
                    dtEmployeeList.AcceptChanges();
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_UpdateEmployeeRoster", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@RoasterId", rosterId);
                        sqlCommand.Parameters.AddWithValue("@ShiftId", shiftId);
                        sqlCommand.Parameters.AddWithValue("@StartDate", startDate);
                        sqlCommand.Parameters.AddWithValue("@EndDate", endDate);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", createdBy);
                        sqlCommand.Parameters.AddWithValue("@EmployeeList", dtEmployeeList); 
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            responseOut.message = ActionMessage.RoasterUpdatedSuccess;

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        #endregion

        #region PMS_Goal
        public DataTable GetGoalDetail(long goalId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetGoalDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@GoalId", goalId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetGoalList(string goalName, int sectionId, int goalCategoryId, int performanceCycleId, string goalStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetGoalApps", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@GoalName", goalName);
                    da.SelectCommand.Parameters.AddWithValue("@SectionId", sectionId);
                    da.SelectCommand.Parameters.AddWithValue("@GoalCategoryId", goalCategoryId);
                    da.SelectCommand.Parameters.AddWithValue("@PerformanceCycleId", performanceCycleId);
                    da.SelectCommand.Parameters.AddWithValue("@Status", goalStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId); 
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);

                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion


        #region Appraisal Template
        public DataTable GetTemplateGoalList(string goalName, int sectionId, int goalCategoryId, int performanceCycleId, DateTime fromDate, DateTime toDate, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetGoals", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@GoalName", goalName);
                    da.SelectCommand.Parameters.AddWithValue("@SectionId", sectionId);
                    da.SelectCommand.Parameters.AddWithValue("@GoalCategoryId", goalCategoryId);
                    da.SelectCommand.Parameters.AddWithValue("@PerformanceCycleId", performanceCycleId);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);

                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }


        public ResponseOut AddEditAppraisalTemplate(HR_PMS_AppraisalTemplate appraisaltemplate, List<HR_PMS_AppraisalTemplateGoal> templateGoalsList)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                DataTable dtTemplateGoal = new DataTable();
                dtTemplateGoal.Columns.Add("GoalId", typeof(int));
                dtTemplateGoal.Columns.Add("Goal_Status", typeof(bool));
                if (templateGoalsList != null && templateGoalsList.Count > 0)
                {
                    foreach (HR_PMS_AppraisalTemplateGoal item in templateGoalsList)
                    {
                        DataRow dtrow = dtTemplateGoal.NewRow();
                        dtrow["GoalId"] = item.GoalId;
                        dtrow["Goal_Status"] = item.Status;
                        dtTemplateGoal.Rows.Add(dtrow);
                    }
                    dtTemplateGoal.AcceptChanges();
                }


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditAppraisalTemplate", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@TemplateId", appraisaltemplate.TemplateId);
                        sqlCommand.Parameters.AddWithValue("@TemplateName", appraisaltemplate.TemplateName);
                        sqlCommand.Parameters.AddWithValue("@DepartmentId", appraisaltemplate.DepartmentId);
                        sqlCommand.Parameters.AddWithValue("@DesignationId", appraisaltemplate.DesignationId);

                        sqlCommand.Parameters.AddWithValue("@CompanyId", appraisaltemplate.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", appraisaltemplate.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@AppraisalTemplate_Status", appraisaltemplate.Status);

                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", appraisaltemplate.CompanyBranchId);

                        sqlCommand.Parameters.AddWithValue("@HR_PMS_AppraisalTemplateGoal", dtTemplateGoal);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetTemplateId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (appraisaltemplate.TemplateId == 0)
                            {
                                responseOut.message = ActionMessage.AppraisalTemplateCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.AppraisalTemplateUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetTemplateId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetTemplateId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }


        public DataTable GetAppraisalTemplateGoalList(long templateId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetAppraisalTemplateGoals", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@TemplateId", templateId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetAppraisalTemplateDetail(long templateId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetAppraisalTemplateDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@TemplateId", templateId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }


        public DataTable GetAppraisalTemplateLists(string templateName, int department, int designation, int companyId, string appraisaltemplateStatus,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetAppraisalTemplates", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@TemplateName", templateName);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", department);
                    da.SelectCommand.Parameters.AddWithValue("@DesignationId", designation);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.SelectCommand.Parameters.AddWithValue("@Status", appraisaltemplateStatus); 
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }


        public DataTable GetAppraisalTemplateDetailList(string templateName, int department, int designation, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetAppraisalTemplateDetailList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@TemplateName", templateName);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", department);
                    da.SelectCommand.Parameters.AddWithValue("@DesignationId", designation);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetAppriasalTemplateGoalDetailList(long templateId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetAppraisalTemplateGoalDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@TemplateId", templateId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region EmployeeLeaveDetail
        public ResponseOut AddEditEmployeeLeaveDetail(HR_EmployeeLeaveDetail employeeLeaveDetail)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeLeaveDetail", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@EmployeeLeaveId", employeeLeaveDetail.EmployeeLeaveId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeLeaveDetail.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@LeaveTypeId", employeeLeaveDetail.LeaveTypeId);
                        sqlCommand.Parameters.AddWithValue("@LeaveCount", employeeLeaveDetail.LeaveCount);
                        sqlCommand.Parameters.AddWithValue("@LeaveDate", employeeLeaveDetail.LeaveDate);
                        sqlCommand.Parameters.AddWithValue("@FinYearId", employeeLeaveDetail.FinYearId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeLeaveDetailStatus", employeeLeaveDetail.Status);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeLeaveDetail.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", employeeLeaveDetail.CreatedBy);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetEmployeeLeaveId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (employeeLeaveDetail.EmployeeLeaveId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeLeaveDetailCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeLeaveDetailUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetEmployeeLeaveId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetEmployeeLeaveId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        public DataTable GetEmployeeLeaveDetail(int employeeLeaveId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeLeaveDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeLeaveId", employeeLeaveId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }

        public DataTable GetEmployeeLeaveDetailList(int employeeId, int leaveTypeId, DateTime fromDate, DateTime toDate, int companyId, string status)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeLeaveDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);

                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@LeaveTypeId", leaveTypeId);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);


                    da.SelectCommand.Parameters.AddWithValue("@Status", status);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetEmployeeLeaveBalanceDetailsList(int employeeId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetLeaveBalanceDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }
        #endregion
        #region Employee Appraisal Template Mapping
        public DataTable GetEmployeeGoalList(long empAppraisalTemplateMappingId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeGoals", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmpAppraisalTemplateMappingId", empAppraisalTemplateMappingId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }

        public ResponseOut AddEditEmployeeAppraisalTemplateMapping(HR_PMS_EmployeeAppraisalTemplateMapping empAppraisalTemplateMapping, List<HR_PMS_EmployeeGoals> employeeGoalList)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                DataTable dtEmployeeGoals = new DataTable();
                dtEmployeeGoals.Columns.Add("GoalId", typeof(int));
                dtEmployeeGoals.Columns.Add("GoalName", typeof(string));
                dtEmployeeGoals.Columns.Add("GoalDescription", typeof(string));
                dtEmployeeGoals.Columns.Add("SectionId", typeof(int));
                dtEmployeeGoals.Columns.Add("GoalCategoryId", typeof(int));
                dtEmployeeGoals.Columns.Add("EvalutionCriteria", typeof(string));
                dtEmployeeGoals.Columns.Add("StartDate", typeof(DateTime));
                dtEmployeeGoals.Columns.Add("DueDate", typeof(DateTime));
                dtEmployeeGoals.Columns.Add("Weight", typeof(decimal));
                dtEmployeeGoals.Columns.Add("SelfScore", typeof(decimal));
                dtEmployeeGoals.Columns.Add("SelfRemarks", typeof(string));
                dtEmployeeGoals.Columns.Add("AppraiserScore", typeof(decimal));
                dtEmployeeGoals.Columns.Add("AppraiserRemarks", typeof(string));
                dtEmployeeGoals.Columns.Add("ReviewScore", typeof(decimal));
                dtEmployeeGoals.Columns.Add("ReviewRemarks", typeof(string));
                dtEmployeeGoals.Columns.Add("Status", typeof(bool));
                dtEmployeeGoals.Columns.Add("FixedDyanmic", typeof(string));


                if (employeeGoalList != null && employeeGoalList.Count > 0)
                {
                    foreach (HR_PMS_EmployeeGoals item in employeeGoalList)
                    {
                        DataRow dtrow = dtEmployeeGoals.NewRow();
                        dtrow["GoalId"] = item.GoalId;
                        dtrow["GoalName"] = item.GoalName;
                        dtrow["GoalDescription"] = item.GoalDescription;
                        dtrow["SectionId"] = item.SectionId;
                        dtrow["GoalCategoryId"] = item.GoalCategoryId;
                        dtrow["EvalutionCriteria"] = item.EvalutionCriteria;
                        dtrow["StartDate"] = item.StartDate;
                        dtrow["DueDate"] = item.DueDate;
                        dtrow["Weight"] = item.Weight;
                        dtrow["SelfScore"] = 0;
                        dtrow["SelfRemarks"] = "";
                        dtrow["AppraiserScore"] = 0;
                        dtrow["AppraiserRemarks"] = "";
                        dtrow["ReviewScore"] = 0;
                        dtrow["ReviewRemarks"] = "";
                        dtrow["Status"] = item.Status;
                        dtrow["FixedDyanmic"] = item.FixedDyanmic;
                        dtEmployeeGoals.Rows.Add(dtrow);
                    }
                    dtEmployeeGoals.AcceptChanges();
                }


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeAppraisalTemplateMapping", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@EmpAppraisalTemplateMappingId", empAppraisalTemplateMapping.EmpAppraisalTemplateMappingId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", empAppraisalTemplateMapping.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@TemplateId", empAppraisalTemplateMapping.TemplateId);
                        sqlCommand.Parameters.AddWithValue("@PerformanceCycleId", empAppraisalTemplateMapping.PerformanceCycleId);
                        sqlCommand.Parameters.AddWithValue("@FinYearId", empAppraisalTemplateMapping.FinYearId);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", empAppraisalTemplateMapping.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", empAppraisalTemplateMapping.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@EmployeeMapping_Status", empAppraisalTemplateMapping.Status);
                        sqlCommand.Parameters.AddWithValue("@HR_PMS_EmployeeGoalDetail", dtEmployeeGoals);
                        sqlCommand.Parameters.AddWithValue("@companyBranchId", empAppraisalTemplateMapping.CompanyBranchId);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetEmpAppraisalTemplateMappingId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (empAppraisalTemplateMapping.EmpAppraisalTemplateMappingId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeAppraisalTemplateMappingCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeAppraisalTemplateMappingUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetEmpAppraisalTemplateMappingId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetEmpAppraisalTemplateMappingId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        public DataTable GetEmployeeAppraisalTemplateMappingDetail(long empAppraisalTemplateMappingId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAppraisalTemplateMappingDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmpAppraisalTemplateMappingId", empAppraisalTemplateMappingId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }

        public DataTable GetEmployeeAppraisalTemplateMappingLists(string templateName, string employeeName, int companyId, string employeeAppraisalTemplateStatus,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAppraisalTemplateMappings", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@TemplateName", templateName);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeName", employeeName);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.SelectCommand.Parameters.AddWithValue("@Status", employeeAppraisalTemplateStatus); 
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion
        #region Separation Application
        public ResponseOut AddEditSeparationApplication(HR_SeparationApplication separationApplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditSeparationApplication", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", separationApplication.ApplicationId);
                        sqlCommand.Parameters.AddWithValue("@ApplicationDate", separationApplication.ApplicationDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", separationApplication.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@SeparationCategoryId", separationApplication.SeparationCategoryId);
                        sqlCommand.Parameters.AddWithValue("@Reason", separationApplication.Reason);
                        sqlCommand.Parameters.AddWithValue("@Remarks", separationApplication.Remarks);
                        sqlCommand.Parameters.AddWithValue("@ApplicationStatus", separationApplication.ApplicationStatus);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", separationApplication.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", separationApplication.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", separationApplication.CompanyBranchId);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetApplicationId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (separationApplication.ApplicationId == 0)
                            {
                                responseOut.message = ActionMessage.SeparationApplicationCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.SeparationApplicationUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetApplicationId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetApplicationId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }




        public DataTable GetSeparationApplicationDetail(long applicationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetSeparationApplicationDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationId", applicationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetSeparationApplicationList(string applicationNo, int employeeId, string separationcategoryId, string applicationStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetSeparationApplications", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@SeparationCategoryId", separationcategoryId);
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationStatus", applicationStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetSeparationApplicationApprovalList(string applicationNo, int employeeId, string separationcategoryId, string applicationStatus, DateTime fromDate, DateTime toDate, int companyId,int companyBranchId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetSeparationApplicationApprovalList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationNo", applicationNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@SeparationCategoryId", separationcategoryId);
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationStatus", applicationStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId); 
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);

                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }



        #endregion
        #region Exit Interview
        public ResponseOut AddEditExitInterview(HR_ExitInterview exitInterview)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditExitInterview", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ExitInterviewId", exitInterview.ExitInterviewId);
                        sqlCommand.Parameters.AddWithValue("@ExitInterviewDate", exitInterview.ExitInterviewDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", exitInterview.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", exitInterview.ApplicationId);
                        sqlCommand.Parameters.AddWithValue("@InterviewByUserId", exitInterview.InterviewByUserId);
                        sqlCommand.Parameters.AddWithValue("@InterviewDescription", exitInterview.InterviewDescription);
                        sqlCommand.Parameters.AddWithValue("@InterviewRemarks", exitInterview.InterviewRemarks);
                        sqlCommand.Parameters.AddWithValue("@InterviewStatus", exitInterview.InterviewStatus);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", exitInterview.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", exitInterview.CreatedBy);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetExitInterviewId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (exitInterview.ExitInterviewId == 0)
                            {
                                responseOut.message = ActionMessage.ExitInterviewCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.ExitInterviewUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetExitInterviewId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetExitInterviewId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }




        public DataTable GetExitInterviewDetail(long exitinterviewId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetExitInterviewDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ExitInterviewId", exitinterviewId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetExitInterviewList(string exitinterviewNo, int employeeId, int applicationId, string interviewStatus, int interviewbyuserId, DateTime fromDate, DateTime toDate, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetExitInterviews", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ExitInterviewNo", exitinterviewNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@ApplicationId", applicationId);
                    da.SelectCommand.Parameters.AddWithValue("@InterviewStatus", interviewStatus);
                    da.SelectCommand.Parameters.AddWithValue("@InterviewByUserId", interviewbyuserId);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);


                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }






        #endregion



        #region Clearance Template

        public ResponseOut AddEditClearanceTemplate(HR_ClearanceTemplate clearancetemplate, List<HR_ClearanceTemplateDetail> templateDetailsList)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                DataTable dtTemplateDetail = new DataTable();
                dtTemplateDetail.Columns.Add("SeparationClearListId", typeof(int));
                dtTemplateDetail.Columns.Add("ClearanceByUserId", typeof(int));
                if (templateDetailsList != null && templateDetailsList.Count > 0)
                {
                    foreach (HR_ClearanceTemplateDetail item in templateDetailsList)
                    {
                        DataRow dtrow = dtTemplateDetail.NewRow();
                        dtrow["SeparationClearListId"] = item.SeparationClearListId;
                        dtrow["ClearanceByUserId"] = item.ClearanceByUserId;
                        dtTemplateDetail.Rows.Add(dtrow);
                    }
                    dtTemplateDetail.AcceptChanges();
                }


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditClearanceTemplate", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ClearanceTemplateId", clearancetemplate.ClearanceTemplateId);
                        sqlCommand.Parameters.AddWithValue("@ClearanceTemplateName", clearancetemplate.ClearanceTemplateName);
                        sqlCommand.Parameters.AddWithValue("@DepartmentId", clearancetemplate.DepartmentId);
                        sqlCommand.Parameters.AddWithValue("@DesignationId", clearancetemplate.Designationid);
                        sqlCommand.Parameters.AddWithValue("@SeparationCategoryId", clearancetemplate.SeparationCategoryId);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", clearancetemplate.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", clearancetemplate.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@ClearanceTemplate_Status", clearancetemplate.Status);

                        sqlCommand.Parameters.AddWithValue("@HR_ClearanceTemplateDetail", dtTemplateDetail);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetClearanceTemplateId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (clearancetemplate.ClearanceTemplateId == 0)
                            {
                                responseOut.message = ActionMessage.ClearanceTemplateCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.ClearanceTemplateUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetClearanceTemplateId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetClearanceTemplateId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }


        public DataTable GetClearanceTemplateLists(string clearancetemplateName, int department, int designation, int separationCategory, int companyId, string clearancetemplateStatus)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetClearanceTemplates", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ClearanceTemplateName", clearancetemplateName);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", department);
                    da.SelectCommand.Parameters.AddWithValue("@DesignationId", designation);
                    da.SelectCommand.Parameters.AddWithValue("@SeparationCategoryId", separationCategory);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.SelectCommand.Parameters.AddWithValue("@Status", clearancetemplateStatus);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }




        public DataTable GetClearanceTemplateDetail(long clearancetemplateId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetClearanceTemplateDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ClearanceTemplateId", clearancetemplateId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }

        public DataTable GetClearanceTemplateDetailList(long clearancetemplateId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetClearanceTemplateDetailList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ClearanceTemplateId", clearancetemplateId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetClearanceProcessList(int clearancetemplateId, int separationclearlistId, int clearancebyuserId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetClearanceProcessList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@ClearanceTemplateId", clearancetemplateId);
                    da.SelectCommand.Parameters.AddWithValue("@SeparationClearListId", separationclearlistId);
                    da.SelectCommand.Parameters.AddWithValue("@ClearanceByUserId", clearancebyuserId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion

        #region Separation Order
        public ResponseOut AddEditSeparationOrder(HR_SeparationOrder separationOrder)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditSeparationOrder", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@SeparationOrderId", separationOrder.SeparationOrderId);
                        sqlCommand.Parameters.AddWithValue("@SeparationOrderDate", separationOrder.SeparationOrderDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", separationOrder.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeClearanceId", separationOrder.EmployeeClearanceId);
                        sqlCommand.Parameters.AddWithValue("@ExitInterviewId", separationOrder.ExitInterviewId);
                        sqlCommand.Parameters.AddWithValue("@ExperienceLetter", separationOrder.ExperienceLetter);
                        sqlCommand.Parameters.AddWithValue("@SepartionRemarks", separationOrder.SepartionRemarks);
                        sqlCommand.Parameters.AddWithValue("@SeparationStatus", separationOrder.SeparationStatus);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", separationOrder.CreatedBy);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetSeparationOrderId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (separationOrder.SeparationOrderId == 0)
                            {
                                responseOut.message = ActionMessage.SeparationOrderCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.SeparationOrderUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetSeparationOrderId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetSeparationOrderId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }




        public DataTable GetSeparationOrderDetail(long separationorderId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetSeparationOrderDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@SeparationOrderId", separationorderId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetSeparationOrderList(string separationorderNo, int employeeId, string employeeClearanceNo, string exitInterviewNo, string separationStatus, DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetSeparationOrders", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@SeparationOrderNo", separationorderNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeClearanceNo", employeeClearanceNo);
                    da.SelectCommand.Parameters.AddWithValue("@ExitInterviewNo", exitInterviewNo);
                    da.SelectCommand.Parameters.AddWithValue("@SeparationStatus", separationStatus);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);


                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetEmployeeInterviewClearanceDetail(long employeeId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeInterviewClearanceDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);

                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }





        #endregion



        #region EmployeeLeave Application Approve
        public ResponseOut ApproveEmployeeLeaveDetail(HR_EmployeeLeaveApplication employeeleaveapplication)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_ApprovalEmployeeLeaveApplication", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@ApplicationId", employeeleaveapplication.ApplicationId);
                        sqlCommand.Parameters.AddWithValue("@LeaveStatus", employeeleaveapplication.LeaveStatus);
                        sqlCommand.Parameters.AddWithValue("@RejectReason", employeeleaveapplication.RejectReason);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", employeeleaveapplication.ApproveBy);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            responseOut.message = ActionMessage.EmployeeLeaveApplicationApproveSuccess;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        #endregion

        #region PMS Employee Assessment
        public ResponseOut AddEditEmployeeAssessment(HR_PMS_EmployeeAppraisalTemplateMapping empAppraisalTemplateMapping, List<HR_PMS_EmployeeGoals> employeeGoalList)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                DataTable dtEmployeeGoals = new DataTable();
                dtEmployeeGoals.Columns.Add("GoalId", typeof(int));
                dtEmployeeGoals.Columns.Add("GoalName", typeof(string));
                dtEmployeeGoals.Columns.Add("GoalDescription", typeof(string));
                dtEmployeeGoals.Columns.Add("SectionId", typeof(int));
                dtEmployeeGoals.Columns.Add("GoalCategoryId", typeof(int));
                dtEmployeeGoals.Columns.Add("EvalutionCriteria", typeof(string));
                dtEmployeeGoals.Columns.Add("StartDate", typeof(DateTime));
                dtEmployeeGoals.Columns.Add("DueDate", typeof(DateTime));
                dtEmployeeGoals.Columns.Add("Weight", typeof(decimal));
                dtEmployeeGoals.Columns.Add("SelfScore", typeof(decimal));
                dtEmployeeGoals.Columns.Add("SelfRemarks", typeof(string));
                dtEmployeeGoals.Columns.Add("AppraiserScore", typeof(decimal));
                dtEmployeeGoals.Columns.Add("AppraiserRemarks", typeof(string));
                dtEmployeeGoals.Columns.Add("ReviewScore", typeof(decimal));
                dtEmployeeGoals.Columns.Add("ReviewRemarks", typeof(string));
                dtEmployeeGoals.Columns.Add("Status", typeof(bool));
                dtEmployeeGoals.Columns.Add("FixedDyanmic", typeof(string));


                if (employeeGoalList != null && employeeGoalList.Count > 0)
                {
                    foreach (HR_PMS_EmployeeGoals item in employeeGoalList)
                    {
                        DataRow dtrow = dtEmployeeGoals.NewRow();
                        dtrow["GoalId"] = item.GoalId;
                        dtrow["GoalName"] = item.GoalName;
                        dtrow["GoalDescription"] = item.GoalDescription;
                        dtrow["SectionId"] = item.SectionId;
                        dtrow["GoalCategoryId"] = item.GoalCategoryId;
                        dtrow["EvalutionCriteria"] = item.EvalutionCriteria;
                        dtrow["StartDate"] = item.StartDate;
                        dtrow["DueDate"] = item.DueDate;
                        dtrow["Weight"] = item.Weight;
                        dtrow["SelfScore"] = item.SelfScore;
                        dtrow["SelfRemarks"] = item.SelfRemarks;
                        dtrow["AppraiserScore"] = item.AppraiserScore;
                        dtrow["AppraiserRemarks"] = item.AppraiserRemarks;
                        dtrow["ReviewScore"] = item.ReviewScore;
                        dtrow["ReviewRemarks"] = item.ReviewRemarks;
                        dtrow["Status"] = item.Status;
                        dtrow["FixedDyanmic"] = item.FixedDyanmic;
                        dtEmployeeGoals.Rows.Add(dtrow);
                    }
                    dtEmployeeGoals.AcceptChanges();
                }


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeAssessment", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@EmpAppraisalTemplateMappingId", empAppraisalTemplateMapping.EmpAppraisalTemplateMappingId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", empAppraisalTemplateMapping.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@TemplateId", empAppraisalTemplateMapping.TemplateId);
                        sqlCommand.Parameters.AddWithValue("@PerformanceCycleId", empAppraisalTemplateMapping.PerformanceCycleId);
                        sqlCommand.Parameters.AddWithValue("@FinYearId", empAppraisalTemplateMapping.FinYearId);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", empAppraisalTemplateMapping.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", empAppraisalTemplateMapping.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@EmployeeMapping_Status", empAppraisalTemplateMapping.Status);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", empAppraisalTemplateMapping.CompanyBranchId);
                        sqlCommand.Parameters.AddWithValue("@HR_PMS_EmployeeGoalDetail", dtEmployeeGoals);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetEmpAppraisalTemplateMappingId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (empAppraisalTemplateMapping.EmpAppraisalTemplateMappingId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeAppraisalTemplateMappingCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeAssessmentUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetEmpAppraisalTemplateMappingId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetEmpAppraisalTemplateMappingId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        #endregion

        #region Employee Appraisal Review 
        public ResponseOut AddEditEmployeeAppraisalReview(HR_PMS_EmployeeAppraisalReview empAppraisalReview)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeAppraisalReview", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@PMSReviewId", empAppraisalReview.PMSReviewId);
                        sqlCommand.Parameters.AddWithValue("@PerformanceCycleId", empAppraisalReview.PerformanceCycleId);
                        sqlCommand.Parameters.AddWithValue("@FinYearId", empAppraisalReview.FinYearId);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", empAppraisalReview.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", empAppraisalReview.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@EmpAppraisalTemplateMappingId", empAppraisalReview.EmpAppraisalTemplateMappingId);
                        sqlCommand.Parameters.AddWithValue("@PMSFormStatus", empAppraisalReview.PMSFormStatus);
                        sqlCommand.Parameters.AddWithValue("@PMSFormSubmitDate", empAppraisalReview.PMSFormSubmitDate);
                        sqlCommand.Parameters.AddWithValue("@PMSReviewRemarks", empAppraisalReview.PMSReviewRemarks);
                        sqlCommand.Parameters.AddWithValue("@PMSFinalStatus", empAppraisalReview.PMSFinalStatus);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", empAppraisalReview.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", empAppraisalReview.CompanyBranchId);


                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetPMSReviewId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (empAppraisalReview.PMSReviewId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeAppraisalReviewCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeAppraisalReviewCreatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetPMSReviewId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetPMSReviewId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        public DataTable GetEmployeeAppraisalReviewList(string employeeName, string pmsFinalStatus,int companyBranchId, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAppraisalReviews", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeName", employeeName);
                    da.SelectCommand.Parameters.AddWithValue("@PMSFinalStatus", pmsFinalStatus);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetEmployeeAppraisalReviewDetail(long pmsReviewId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAppraisalReviewDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@PMSReviewId", pmsReviewId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetPMS_EmployeeDetail(long empAppraisalTemplateMappingId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_PMS_GetPMS_EmployeeDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmpAppraisalTemplateMappingId", empAppraisalTemplateMappingId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetPMS_EmployeeAssessmentDetail(long empAppraisalTemplateMappingId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_PMS_GetPMS_EmployeeAssessmentDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmpAppraisalTemplateMappingId", empAppraisalTemplateMappingId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetPMS_EmployeeAssessmentFooterDetail(long empAppraisalTemplateMappingId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_PMS_GetPMS_EmployeeAssessmentFooterDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmpAppraisalTemplateMappingId", empAppraisalTemplateMappingId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        #endregion

        #region Employee Clearance Process Mapping
        public ResponseOut AddEditEmployeeClearanceProcessMapping(HR_EmployeeClearanceProcess employeeClearanceProcessMapping, List<HR_EmployeeClearanceProcessDetail> employeeClearanceProcessList)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                DataTable dtClearanceProcessList = new DataTable();
                dtClearanceProcessList.Columns.Add("SeparationClearListId", typeof(int));
                dtClearanceProcessList.Columns.Add("ClearanceByUserId", typeof(int));
                dtClearanceProcessList.Columns.Add("ClearanceStatus", typeof(string));
                dtClearanceProcessList.Columns.Add("ClearanceRemarks", typeof(string));
                if (employeeClearanceProcessList != null && employeeClearanceProcessList.Count > 0)
                {
                    foreach (HR_EmployeeClearanceProcessDetail item in employeeClearanceProcessList)
                    {
                        DataRow dtrow = dtClearanceProcessList.NewRow();
                        dtrow["SeparationClearListId"] = item.SeparationClearListId;
                        dtrow["ClearanceByUserId"] = item.ClearanceByUserId;
                        dtrow["ClearanceStatus"] = item.ClearanceStatus;
                        dtrow["ClearanceRemarks"] = item.ClearanceRemarks;
                        dtClearanceProcessList.Rows.Add(dtrow);
                    }
                    dtClearanceProcessList.AcceptChanges();
                }


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeClearanceProcessMapping", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@EmployeeClearanceId", employeeClearanceProcessMapping.EmployeeClearanceId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeClearanceProcessMapping.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@ClearanceTemplateId", employeeClearanceProcessMapping.ClearanceTemplateId);
                        sqlCommand.Parameters.AddWithValue("@ClearanceFinalStatus", employeeClearanceProcessMapping.ClearanceFinalStatus);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeClearanceProcessMapping.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", employeeClearanceProcessMapping.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@EmployeeClearanceProcessDetail", dtClearanceProcessList);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetEmployeeClearanceId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (employeeClearanceProcessMapping.EmployeeClearanceId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeClearanceProcessMappingCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeClearanceProcessMappingUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetEmployeeClearanceId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetEmployeeClearanceId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        public DataTable GetEmployeeClearanceProcessMappingList(string employeeClearanceNo = "", long employeeId = 0, int clearanceTemplateId = 0, int companyId = 0)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeClearanceProcessMappingList", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeClearanceNo", employeeClearanceNo);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@ClearanceTemplateId", clearanceTemplateId);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetEmployeeClearanceProcessDetail(long employeeClearanceId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeClearanceProcessMappingDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeClearanceId", employeeClearanceId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }
        public DataTable GetEmployeeClearanceProcesses(long employeeClearanceId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeClearanceProcesses", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeClearanceId", employeeClearanceId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }

        public ResponseOut AddEditEmployeeClearance(HR_EmployeeClearanceProcess employeeClearanceProcessMapping, List<HR_EmployeeClearanceProcessDetail> employeeClearanceProcessList)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                DataTable dtClearanceProcessList = new DataTable();
                dtClearanceProcessList.Columns.Add("SeparationClearListId", typeof(int));
                dtClearanceProcessList.Columns.Add("ClearanceByUserId", typeof(int));
                dtClearanceProcessList.Columns.Add("ClearanceStatus", typeof(string));
                dtClearanceProcessList.Columns.Add("ClearanceRemarks", typeof(string));
                if (employeeClearanceProcessList != null && employeeClearanceProcessList.Count > 0)
                {
                    foreach (HR_EmployeeClearanceProcessDetail item in employeeClearanceProcessList)
                    {
                        DataRow dtrow = dtClearanceProcessList.NewRow();
                        dtrow["SeparationClearListId"] = item.SeparationClearListId;
                        dtrow["ClearanceByUserId"] = item.ClearanceByUserId;
                        dtrow["ClearanceStatus"] = item.ClearanceStatus;
                        dtrow["ClearanceRemarks"] = item.ClearanceRemarks;
                        dtClearanceProcessList.Rows.Add(dtrow);
                    }
                    dtClearanceProcessList.AcceptChanges();
                }


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeClearance", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@EmployeeClearanceId", employeeClearanceProcessMapping.EmployeeClearanceId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeClearanceProcessMapping.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@ClearanceTemplateId", employeeClearanceProcessMapping.ClearanceTemplateId);
                        sqlCommand.Parameters.AddWithValue("@ClearanceFinalStatus", employeeClearanceProcessMapping.ClearanceFinalStatus);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeClearanceProcessMapping.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", employeeClearanceProcessMapping.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@EmployeeClearanceProcessDetail", dtClearanceProcessList);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetEmployeeClearanceId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (employeeClearanceProcessMapping.EmployeeClearanceId == 0)
                            {
                                responseOut.message = ActionMessage.EmployeeClearanceUpdatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.EmployeeClearanceUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetEmployeeClearanceId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetEmployeeClearanceId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        #endregion

        #region Employee Mark Attendance
        public ResponseOut AddEditEmployeeMarkAttendance(HR_EmployeeAttendance employeeAttendance)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeMarkAttendance", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@EmployeeAttendanceId", employeeAttendance.EmployeeAttendanceId);
                        sqlCommand.Parameters.AddWithValue("@AttendanceDate", employeeAttendance.AttendanceDate);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeAttendance.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@PresentAbsent", employeeAttendance.PresentAbsent);
                        sqlCommand.Parameters.AddWithValue("@AttendanceStatus", employeeAttendance.AttendanceStatus);
                        sqlCommand.Parameters.AddWithValue("@InOut", employeeAttendance.InOut);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeAttendance.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", employeeAttendance.CreatedBy);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetApplicationId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (employeeAttendance.EmployeeAttendanceId == 0)
                            {
                                responseOut.message = "Successfully " + Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                            else
                            {
                                responseOut.message = "Successfully " + Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                            if (sqlCommand.Parameters["@RetApplicationId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetApplicationId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        public DataTable GetEmployeeInOutDetails(string attendanceDate, int employeeId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeInOutDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@AttendanceDate", attendanceDate);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    //da.SelectCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }

        public DataTable GetEmployeeMarkAttendanceList(int employeeId, DateTime fromDate, DateTime toDate, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeMarkAttendance", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region Employee Attendance

        public ResponseOut AddEditEmployeeAttendance(int companyId, int employeeId, string attendanceDate, string presentAbsent, string inTime, string outTime, string attendanceStatus, int userId,int companyBranch)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeAttendance", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@CompanyId", companyId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                        sqlCommand.Parameters.AddWithValue("@AttendanceDate", attendanceDate);
                        sqlCommand.Parameters.AddWithValue("@PresentAbsent", presentAbsent);
                        sqlCommand.Parameters.AddWithValue("@InTime", inTime);
                        sqlCommand.Parameters.AddWithValue("@OutTime", outTime);
                        sqlCommand.Parameters.AddWithValue("@AttendanceStatus", attendanceStatus);
                        sqlCommand.Parameters.AddWithValue("@UserId", userId);
                        sqlCommand.Parameters.AddWithValue("@companyBranch", companyBranch);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            responseOut.message = "Successfully " + Convert.ToString(sqlCommand.Parameters["@message"].Value);

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        public DataTable GetEmployeeAttendanceList(int employeeId, string attendanceDate, int departmentId, int designationId,string companyBranch)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAttendanceDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@AttendanceDate", attendanceDate);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    da.SelectCommand.Parameters.AddWithValue("@DesignationId", designationId); 
                    da.SelectCommand.Parameters.AddWithValue("@companyBranch", companyBranch);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public ResponseOut UpdateEmployeeAttendanceByEmployer(HR_EmployeeAttendance employeeAttendance)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_UpdateEmployeeAttendanceByEmployer", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeAttendance.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@AttendanceDate", employeeAttendance.AttendanceDate);
                        sqlCommand.Parameters.AddWithValue("@AttendanceStatus", employeeAttendance.AttendanceStatus);
                        sqlCommand.Parameters.AddWithValue("@ModifiedBy", employeeAttendance.ModifiedBy);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeAttendance.CompanyId);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        /*------*/

        public DataTable GetEmployeeAttendanceFormList(int employeeId, string attendanceDate, string employeeType, int departmentId, int designationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAttendanceFormDetailsNew", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeType", employeeType);
                    da.SelectCommand.Parameters.AddWithValue("@AttendanceDate", attendanceDate);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    da.SelectCommand.Parameters.AddWithValue("@DesignationId", designationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public DataTable GetTempEmployeeAttendanceFormList(int employeeId, string attendanceDate, string employeeType, int departmentId, int designationId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetTempEmployeeAttendanceFormDetailsNew", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeType", employeeType);
                    da.SelectCommand.Parameters.AddWithValue("@AttendanceDate", attendanceDate);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    da.SelectCommand.Parameters.AddWithValue("@DesignationId", designationId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public ResponseOut AddEditEmployeeAttendanceFormByEmployer(HR_EmployeeAttendance employeeAttendance, string inTime, string outTime)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditEmployeeAttendanceFormByEmployer", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeAttendance.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@AttendanceDate", employeeAttendance.AttendanceDate);
                        sqlCommand.Parameters.AddWithValue("@AttendanceStatus", employeeAttendance.AttendanceStatus);
                        sqlCommand.Parameters.AddWithValue("@InTime", inTime);
                        sqlCommand.Parameters.AddWithValue("@OutTime", outTime);
                        sqlCommand.Parameters.AddWithValue("@AbsentPresent", employeeAttendance.PresentAbsent);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", employeeAttendance.ModifiedBy);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", employeeAttendance.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", employeeAttendance.CompanyBranchId);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        public DataTable GetEmployeeAttendanceReport(int month, int year)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEmployeeAttendanceRecords", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@month", month);
                    da.SelectCommand.Parameters.AddWithValue("@year", year);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }


        #endregion

        #region Payroll Processing
        public ResponseOut AddEditPayrollProcessing(PR_PayrollProcessPeriod payrollProcessPeriod)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_PayrollProcessing", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollProcessPeriod.PayrollProcessingPeriodId);
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingStartDate", payrollProcessPeriod.PayrollProcessingStartDate);
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingEndDate", payrollProcessPeriod.PayrollProcessingEndDate);
                        sqlCommand.Parameters.AddWithValue("@MonthId", payrollProcessPeriod.MonthId);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", payrollProcessPeriod.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@FinYearId", payrollProcessPeriod.FinYearId);
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessStatus", payrollProcessPeriod.PayrollProcessStatus);
                        sqlCommand.Parameters.AddWithValue("@PayrollLocked", payrollProcessPeriod.PayrollLocked);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", payrollProcessPeriod.CreatedBy);
                        sqlCommand.Parameters.AddWithValue("@companyBranch", payrollProcessPeriod.CompanyBranchId);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetPayrollProcessingPeriodId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (payrollProcessPeriod.PayrollProcessingPeriodId == 0)
                            {
                                responseOut.message = ActionMessage.PayrollProcessSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.PayrollProcessSuccess;
                            }
                            if (sqlCommand.Parameters["@RetPayrollProcessingPeriodId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetPayrollProcessingPeriodId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        public DataTable GetPayrollProcessPeriodList(int monthId, string payrollProcessStatus, string payrollLocked, int companyId,string companyBranch)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetPayrollProcessPeriods", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@MonthId", monthId);
                    da.SelectCommand.Parameters.AddWithValue("@PayrollProcessStatus", payrollProcessStatus);
                    da.SelectCommand.Parameters.AddWithValue("@PayrollLocked", payrollLocked);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId); 
                    da.SelectCommand.Parameters.AddWithValue("@companyBranch", companyBranch);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetPayrollProcessPeriodDetail(long payrollProcessingPeriodId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetPayrollProcessPeriodDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollProcessingPeriodId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public ResponseOut LockUnlockPayrollProcessing(PR_PayrollProcessPeriod payrollProcessPeriod)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_PayrollUpdateProcessing", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollProcessPeriod.PayrollProcessingPeriodId);
                        sqlCommand.Parameters.AddWithValue("@PayrollLocked", payrollProcessPeriod.PayrollLocked);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", payrollProcessPeriod.CreatedBy);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetPayrollProcessingPeriodId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }
                        }
                        else
                        {
                            if (payrollProcessPeriod.PayrollProcessingPeriodId == 0)
                            {
                                responseOut.message = ActionMessage.PayrollProcessSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.PayrollProcessLockUnlock;
                            }
                            if (sqlCommand.Parameters["@RetPayrollProcessingPeriodId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetPayrollProcessingPeriodId"].Value);
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        public DataTable GetEssPayrollProcessPeriodDetail(int monthId,int yearId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetESSPayrollProcessPeriodDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@monthId", monthId);
                    da.SelectCommand.Parameters.AddWithValue("@YearId", yearId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region Salary Summary Report
        public DataTable GetSalarySummaryReport(long payrollProcessingPeriodId, int companyBranchId, int departmentId, int designationId, string employeeType, string employeeCodes)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                DataTable dtEmployee = new DataTable();
                dtEmployee.Columns.Add("EmployeeCode", typeof(string));
                if (employeeCodes != null && employeeCodes.Length > 0)
                {
                    string[] empCode = employeeCodes.Split(',');

                    foreach (string item in empCode)
                    {
                        if (item.Trim() == "") break;
                        DataRow dtrow = dtEmployee.NewRow();
                        dtrow["EmployeeCode"] = item;
                        dtEmployee.Rows.Add(dtrow);
                    }
                    dtEmployee.AcceptChanges();
                }


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetSalarySummaryReport", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollProcessingPeriodId);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    da.SelectCommand.Parameters.AddWithValue("@DesignationId", designationId);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeType", employeeType);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeCodes", dtEmployee);


                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }


        public DataTable GetEssSalarySummaryReport(long payrollProcessingPeriodId, int companyBranchId, int departmentId, int designationId, string employeeType, string employeeCodes)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                //DataTable dtEmployee = new DataTable();
                //dtEmployee.Columns.Add("EmployeeCode", typeof(string));
                //if (employeeCodes != null && employeeCodes.Length > 0)
                //{
                //    string[] empCode = employeeCodes.Split(',');

                //    foreach (string item in empCode)
                //    {
                //        if (item.Trim() == "") break;
                //        DataRow dtrow = dtEmployee.NewRow();
                //        dtrow["EmployeeCode"] = item;
                //        dtEmployee.Rows.Add(dtrow);
                //    }
                //    dtEmployee.AcceptChanges();
                //}


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetEssSalarySummaryReport", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollProcessingPeriodId);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    da.SelectCommand.Parameters.AddWithValue("@DesignationId", designationId);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeType", employeeType);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeCodes", employeeCodes);


                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region Salary JV
        public DataTable GetSalaryJVReport(long payrollProcessingPeriodId, int companyBranchId, int departmentId, int designationId, string employeeType, string employeeCodes)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                DataTable dtEmployee = new DataTable();
                dtEmployee.Columns.Add("EmployeeCode", typeof(string));
                if (employeeCodes != null && employeeCodes.Length > 0)
                {
                    string[] empCode = employeeCodes.Split(',');

                    foreach (string item in empCode)
                    {
                        if (item.Trim() == "") break;
                        DataRow dtrow = dtEmployee.NewRow();
                        dtrow["EmployeeCode"] = item;
                        dtEmployee.Rows.Add(dtrow);
                    }
                    dtEmployee.AcceptChanges();
                }


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetSalaryJVReport", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollProcessingPeriodId);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    da.SelectCommand.Parameters.AddWithValue("@DesignationId", designationId);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeType", employeeType);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeCodes", dtEmployee);


                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }

        public ResponseOut GenerateSalaryJV(long payrollProcessingPeriodId, int companyBranchId, int departmentId, int designationId, string employeeType, int createdBy, string employeeCodes)
        {
            ResponseOut responseOut = new ResponseOut();

            try
            {
                DataTable dtEmployee = new DataTable();
                dtEmployee.Columns.Add("EmployeeCode", typeof(string));
                if (employeeCodes != null && employeeCodes.Length > 0)
                {
                    string[] empCode = employeeCodes.Split(',');

                    foreach (string item in empCode)
                    {
                        if (item.Trim() == "") break;
                        DataRow dtrow = dtEmployee.NewRow();
                        dtrow["EmployeeCode"] = item;
                        dtEmployee.Rows.Add(dtrow);
                    }
                    dtEmployee.AcceptChanges();
                }


                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_GenerateSalaryJV", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;


                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollProcessingPeriodId);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchId);
                        sqlCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                        sqlCommand.Parameters.AddWithValue("@DesignationId", designationId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeType", employeeType);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", createdBy);
                        sqlCommand.Parameters.AddWithValue("@EmployeeCodes", dtEmployee);
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetSalaryJVId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {

                            responseOut.message = ActionMessage.PayrollSalaryJVSuccess;

                            if (sqlCommand.Parameters["@RetSalaryJVId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetSalaryJVId"].Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        #endregion

        #region PayrollOtherEarningDeduction
        public ResponseOut AddEditPayrollOtherEarningDeduction(PR_PayrollOtherEarningDeduction payrollOtherEarningDeduction)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditPayrollOtherEarningDeduction", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@MonthlyInputId", payrollOtherEarningDeduction.MonthlyInputId);                      
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollOtherEarningDeduction.PayrollProcessingPeriodId);
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingStartDate", payrollOtherEarningDeduction.PayrollProcessingStartDate);
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingEndDate", payrollOtherEarningDeduction.PayrollProcessingEndDate);
                        sqlCommand.Parameters.AddWithValue("@MonthId", payrollOtherEarningDeduction.MonthId);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", payrollOtherEarningDeduction.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", payrollOtherEarningDeduction.CompanyBranchId);
                        sqlCommand.Parameters.AddWithValue("@DepartmentId", payrollOtherEarningDeduction.DepartmentId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", payrollOtherEarningDeduction.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@TDSApplicable", payrollOtherEarningDeduction.TDSApplicable);
                        sqlCommand.Parameters.AddWithValue("@IncomeTax", payrollOtherEarningDeduction.IncomeTax);
                        sqlCommand.Parameters.AddWithValue("@Exgretia", payrollOtherEarningDeduction.Exgretia);
                        sqlCommand.Parameters.AddWithValue("@Incentive", payrollOtherEarningDeduction.Incentive);
                        sqlCommand.Parameters.AddWithValue("@LeaveEncashment", payrollOtherEarningDeduction.LeaveEncashment);
                        sqlCommand.Parameters.AddWithValue("@NoticePayPayable", payrollOtherEarningDeduction.NoticePayPayable);
                        sqlCommand.Parameters.AddWithValue("@OverTimeAllow", payrollOtherEarningDeduction.OverTimeAllow);
                        sqlCommand.Parameters.AddWithValue("@VariablePay", payrollOtherEarningDeduction.VariablePay);
                        sqlCommand.Parameters.AddWithValue("@OtherDeduction", payrollOtherEarningDeduction.OtherDeduction);
                        sqlCommand.Parameters.AddWithValue("@OtherAllowance", payrollOtherEarningDeduction.OtherAllowance);
                        sqlCommand.Parameters.AddWithValue("@LoanPayable", payrollOtherEarningDeduction.LoanPayable);
                        sqlCommand.Parameters.AddWithValue("@LoanRecv", payrollOtherEarningDeduction.LoanRecv);
                        sqlCommand.Parameters.AddWithValue("@AdvancePayable", payrollOtherEarningDeduction.AdvancePayable);
                        sqlCommand.Parameters.AddWithValue("@AdvanceRecv", payrollOtherEarningDeduction.AdvanceRecv);
                        sqlCommand.Parameters.AddWithValue("@AnnualBonus", payrollOtherEarningDeduction.AnnualBonus);
                        
                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetMonthlyInputIdId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (payrollOtherEarningDeduction.MonthlyInputId == 0)
                            {
                                responseOut.message = ActionMessage.PayrollOtherEarningDeductionCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.PayrollOtherEarningDeductionUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetMonthlyInputIdId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetMonthlyInputIdId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }

        public DataTable GetPayrollOtherEarningDeductionList(int payrollProcessingPeriodId , int employeeId, int departmentID , int companyBranchID, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    da = new SqlDataAdapter("proc_GetPayrollOtherEarningDeduction", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollProcessingPeriodId);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", departmentID);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchID);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetPayrollOtherEarningDeductionDetail(long monthlyInputId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetPayrollOtherEarningDeductionDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@MonthlyInputId", monthlyInputId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        #region PayrollMonthlyAdjustment
        public ResponseOut AddEditPayrollMonthlyAdjustment(PR_PayrollMonthlyAdjustment payrollMonthlyAdjustment)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("proc_AddEditPayrollMonthlyAdjustment", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@PayrollAdjustmentId", payrollMonthlyAdjustment.PayrollAdjustmentId);
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollMonthlyAdjustment.PayrollProcessingPeriodId);
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingStartDate", payrollMonthlyAdjustment.PayrollProcessingStartDate);
                        sqlCommand.Parameters.AddWithValue("@PayrollProcessingEndDate", payrollMonthlyAdjustment.PayrollProcessingEndDate);
                        sqlCommand.Parameters.AddWithValue("@MonthId", payrollMonthlyAdjustment.MonthId);
                        sqlCommand.Parameters.AddWithValue("@CompanyId", payrollMonthlyAdjustment.CompanyId);
                        sqlCommand.Parameters.AddWithValue("@CompanyBranchId", payrollMonthlyAdjustment.CompanyBranchId);
                        sqlCommand.Parameters.AddWithValue("@DepartmentId", payrollMonthlyAdjustment.DepartmentId);
                        sqlCommand.Parameters.AddWithValue("@EmployeeId", payrollMonthlyAdjustment.EmployeeId);
                        sqlCommand.Parameters.AddWithValue("@BasicPay", payrollMonthlyAdjustment.BasicPay);
                        sqlCommand.Parameters.AddWithValue("@ConveyanceAllow", payrollMonthlyAdjustment.ConveyanceAllow);
                        sqlCommand.Parameters.AddWithValue("@SpecialAllow", payrollMonthlyAdjustment.SpecialAllow);
                        sqlCommand.Parameters.AddWithValue("@OtherAllow", payrollMonthlyAdjustment.OtherAllow);
                        sqlCommand.Parameters.AddWithValue("@MedicalAllow", payrollMonthlyAdjustment.MedicalAllow);
                        sqlCommand.Parameters.AddWithValue("@ChildEduAllow", payrollMonthlyAdjustment.ChildEduAllow);
                        sqlCommand.Parameters.AddWithValue("@LTA", payrollMonthlyAdjustment.LTA);
                        sqlCommand.Parameters.AddWithValue("@EmployeePF", payrollMonthlyAdjustment.EmployeePF);
                        sqlCommand.Parameters.AddWithValue("@EmployeeESI", payrollMonthlyAdjustment.EmployeeESI);
                        sqlCommand.Parameters.AddWithValue("@OtherDeduction", payrollMonthlyAdjustment.OtherDeduction);
                        sqlCommand.Parameters.AddWithValue("@ProfessionalTax", payrollMonthlyAdjustment.ProfessionalTax);
                        sqlCommand.Parameters.AddWithValue("@AdhocAllowance", payrollMonthlyAdjustment.AdhocAllowance);
                        sqlCommand.Parameters.AddWithValue("@AnnualBonus", payrollMonthlyAdjustment.AnnualBonus);
                        sqlCommand.Parameters.AddWithValue("@Exgratia", payrollMonthlyAdjustment.Exgratia);
                        sqlCommand.Parameters.AddWithValue("@LeaveEncashment", payrollMonthlyAdjustment.LeaveEncashment);
                        sqlCommand.Parameters.AddWithValue("@SalaryAdvancePayable", payrollMonthlyAdjustment.SalaryAdvancePayable);
                        sqlCommand.Parameters.AddWithValue("@NoticePayPayable", payrollMonthlyAdjustment.NoticePayPayable);
                        sqlCommand.Parameters.AddWithValue("@SalaryAdvanceRecv", payrollMonthlyAdjustment.SalaryAdvanceRecv);
                        sqlCommand.Parameters.AddWithValue("@NoticePayRecv", payrollMonthlyAdjustment.NoticePayRecv);
                        sqlCommand.Parameters.AddWithValue("@LoanPayable", payrollMonthlyAdjustment.LoanPayable);
                        sqlCommand.Parameters.AddWithValue("@LoanRecv", payrollMonthlyAdjustment.LoanRecv);
                        sqlCommand.Parameters.AddWithValue("@IncomeTax", payrollMonthlyAdjustment.IncomeTax);
                        sqlCommand.Parameters.AddWithValue("@CreatedBy", payrollMonthlyAdjustment.CreatedBy);

                        sqlCommand.Parameters.Add("@status", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add("@RetPayrollAdjustmentId", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        if (sqlCommand.Parameters["@status"].Value != null)
                        {
                            responseOut.status = Convert.ToString(sqlCommand.Parameters["@status"].Value);
                        }
                        if (responseOut.status != ActionStatus.Success)
                        {
                            if (sqlCommand.Parameters["@message"].Value != null)
                            {
                                responseOut.message = Convert.ToString(sqlCommand.Parameters["@message"].Value);
                            }

                        }
                        else
                        {
                            if (payrollMonthlyAdjustment.PayrollAdjustmentId == 0)
                            {
                                responseOut.message = ActionMessage.PayrollMonthlyAdjustmentCreatedSuccess;
                            }
                            else
                            {
                                responseOut.message = ActionMessage.PayrollMonthlyAdjustmentUpdatedSuccess;
                            }
                            if (sqlCommand.Parameters["@RetPayrollAdjustmentId"].Value != null)
                            {
                                responseOut.trnId = Convert.ToInt64(sqlCommand.Parameters["@RetPayrollAdjustmentId"].Value);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return responseOut;

        }
        public DataTable GetPayrollMonthlyAdjustmentList(int payrollProcessingPeriodId, int employeeId, int departmentID, int companyBranchID, int companyId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetPayrollMonthlyAdjustments", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@PayrollProcessingPeriodId", payrollProcessingPeriodId);
                    da.SelectCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
                    da.SelectCommand.Parameters.AddWithValue("@DepartmentId", departmentID);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyBranchId", companyBranchID);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetPayrollMonthlyAdjustmentDetail(long payrollAdjustmentId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetPayrollMonthlyAdjustmentDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@PayrollAdjustmentId", payrollAdjustmentId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        #endregion

        # region PayHeadGLMapping
        public DataTable GetPayHeadGLMappingList(string payHeadName = "", int companyId = 0, string status = "",string companyBranch="")
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetPayHeadGLMappings", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@PayHeadName", payHeadName);
                    da.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                    da.SelectCommand.Parameters.AddWithValue("@Status", status);
                    da.SelectCommand.Parameters.AddWithValue("@companyBranch", companyBranch);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;

        }
        public DataTable GetPayHeadGLMappingDetail(int payHeadMappingId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    da = new SqlDataAdapter("proc_GetPayHeadGLMappingDetail", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@PayHeadMappingId", payHeadMappingId);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return dt;
        }
        #endregion 
    }
}
