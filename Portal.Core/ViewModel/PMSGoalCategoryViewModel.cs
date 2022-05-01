using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public partial class PMSGoalCategoryViewModel
    {
        public int GoalCategoryId { get; set; }
        public string GoalCategoryName { get; set; }
        public bool GoalCategory_Status { get; set; }

        public int Weight { get; set; }
        public string message { get; set; }
        public string status { get; set; } 
 

}
    
}
