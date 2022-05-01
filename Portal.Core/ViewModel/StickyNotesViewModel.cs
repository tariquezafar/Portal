using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{

    public class StickyNotesViewModel : IModel
    {
        public int StickyNoteId { get; set; }
        public int UserId { get; set; }
        public string StickyNoteMessage { get; set; }    
        public string LastModifyDateTime { get; set; } 
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
