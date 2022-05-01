using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class SeparationClearListViewModel : IModel
    {
        public int SeparationClearListId { get; set; }
        public string SeparationClearListName { get; set; }
        public string SeparationClearListDesc { get; set; }
        public bool SeparationClearList_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
