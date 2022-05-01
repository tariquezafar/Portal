using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class SeparationCategoryViewModel : IModel
    {
        public int SeparationCategoryId { get; set; }
        public string SeparationCategoryName { get; set; }
        public bool SeparationCategory_Status { get; set; }
        public string message { get; set; }
        public string status { get; set; }

    }
    
}
