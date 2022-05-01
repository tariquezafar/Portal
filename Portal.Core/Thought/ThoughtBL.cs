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
    public class ThoughtBL
    {
        DBInterface dbInterface;
        public ThoughtBL()
        {
            dbInterface = new DBInterface();
        }
        public ResponseOut AddEditThought(ThoughtViewModel thoughtViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                Thought thought = new Thought
                {
                    ThoughtId = thoughtViewModel.ThoughtId,
                    ThoughtMessage = thoughtViewModel.ThoughtMessage,
                    ThoughtType = thoughtViewModel.ThoughtType,
                    ExpiryDate = Convert.ToDateTime(thoughtViewModel.ExpiryDate),
                    Status = thoughtViewModel.Thought_Status
                };
                responseOut = dbInterface.AddEditThought(thought);
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

        public List<ThoughtViewModel> GetThoughtList(string thoughtName = "", string thoughtType = "")
        {
            List<ThoughtViewModel> thoughts = new List<ThoughtViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtThoughts = sqlDbInterface.GetThoughtList(thoughtName,thoughtType);
                if (dtThoughts != null && dtThoughts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtThoughts.Rows)
                    {
                        thoughts.Add(new ThoughtViewModel
                        {
                            ThoughtId  = Convert.ToInt32(dr["ThoughtId"]),
                            ThoughtMessage = Convert.ToString(dr["ThoughtMessage"]),
                            ThoughtType = Convert.ToString(dr["ThoughtType"]),
                            ExpiryDate = Convert.ToDateTime(dr["ExpiryDate"]).ToString("dd-MMM-yyyy"),
                            Thought_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return thoughts;
        }

        public ThoughtViewModel GetThoughtDetail(int thoughtId = 0)
        {
            ThoughtViewModel thought = new ThoughtViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtThought = sqlDbInterface.GetThoughtDetail(thoughtId);
                if (dtThought != null && dtThought.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtThought.Rows)
                    {
                        thought = new ThoughtViewModel
                        {
                            ThoughtId = Convert.ToInt32(dr["ThoughtId"]),
                            ThoughtMessage = Convert.ToString(dr["ThoughtMessage"]),
                            ThoughtType = Convert.ToString(dr["ThoughtType"]),
                            ExpiryDate = Convert.ToDateTime(dr["ExpiryDate"]).ToString("dd-MMM-yyyy"),
                            Thought_Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return thought;
        }

        public List<ThoughtViewModel> GetDashboardThoughtDetail()
        {
            List<ThoughtViewModel> thoughts = new List<ThoughtViewModel>();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtThoughts = sqlDbInterface.GetDashboardThoughtDetail();
                if (dtThoughts != null && dtThoughts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtThoughts.Rows)
                    {
                        thoughts.Add(new ThoughtViewModel
                        {
                            ThoughtId = Convert.ToInt32(dr["ThoughtId"]),
                            ThoughtMessage = Convert.ToString(dr["ThoughtMessage"]),
                            ThoughtType = Convert.ToString(dr["ThoughtType"]),
                            ExpiryDate =Convert.ToDateTime(dr["ExpiryDate"]).ToString("dd-MMM-yyyy"),
                            Thought_Status = Convert.ToBoolean(dr["Status"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return thoughts;
        }
    }
}
