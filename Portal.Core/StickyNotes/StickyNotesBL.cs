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
    public class StickyNotesBL
    {
        DBInterface dbInterface;
        public StickyNotesBL()
        {
            dbInterface = new DBInterface();
        }

        public ResponseOut AddEditStickyNotes(StickyNotesViewModel stickyNotesViewModel)
        {
            ResponseOut responseOut = new ResponseOut();
            try
            {
                StickyNote stickyNote = new StickyNote
                {
                    UserId = stickyNotesViewModel.UserId,
                    StickyNoteMessage = stickyNotesViewModel.StickyNoteMessage,
                    StickyNoteId = stickyNotesViewModel.StickyNoteId,
                    LastModifyDateTime = DateTime.Now.ToString()
                };
                responseOut = dbInterface.AddEditStickyNotes(stickyNote);
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
        public StickyNotesViewModel GetStickyNotesDetail(int UserId = 0)
        {
            StickyNotesViewModel StickyNotes = new StickyNotesViewModel();
            SQLDbInterface sqlDbInterface = new SQLDbInterface();
            try
            {
                DataTable dtThought = sqlDbInterface.GetStickyNotestDetail(UserId);
                if (dtThought != null && dtThought.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtThought.Rows)
                    {
                        StickyNotes = new StickyNotesViewModel
                        {
                            StickyNoteId = Convert.ToInt32(dr["StickyNoteId"]),
                            StickyNoteMessage = Convert.ToString(dr["StickyNoteMessage"]),
                            UserId = Convert.ToInt32(dr["UserId"]),
                            LastModifyDateTime = Convert.ToDateTime(dr["LastModifyDateTime"]).ToString("dd-MMM-yyyy")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.SaveErrorLog(this.ToString(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }
            return StickyNotes;
        }

       

    }
}
