using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class FinYearViewModel
    {
        public int FinYearId { get; set; }
        public string FinYearDesc { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string FinYearCode { get; set; }
        public bool FinYearStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public int PreviousFinYearId { get; set; }
        public int CurrentFinYearID { get; set; }
        public string PreviousFinYearCode { get; set; }
        public string CurrentFinYearCode { get; set; }

    }
}
