using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{

    public partial class HR_ResourceRequisitionInterviewStageViewModel
    {
        public long RequisitionInterviewStagesId { get; set; }
        public int RequisitionId { get; set; }
        public int InterviewTypeId { get; set; }
        public string InterviewTypeName { get; set; }
        public int InterviewSequenceNo { get; set; }
        public string InterviewDescription { get; set; }
        public int InterviewAssignToUserId { get; set; }
        public string InterviewAssignToUserName { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
    
}
