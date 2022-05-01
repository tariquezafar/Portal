using Portal.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Common;
namespace Portal.Core.ViewModel
{
    public class HR_RoasterViewModel : IModel
    {
        public int RoasterId { get; set; }
        public string RoasterName { get; set; }
        public string RoasterDesc { get; set; }
        public string RoasterStartDate { get; set; }
        public string RoasterEndDate { get; set; }
        public string RoasterType { get; set; }
        public string Remarks { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int NoOfWeeks { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public String CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public String ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public bool RoasterStatus { get; set; }
        public string message { get; set; }
        public string status { get; set; }

        public string RoasterDay { get; set; }

        public string ShiftName { get; set; }

        public string ShiftDescription { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public decimal FullDayWorkHour { get; set; }
        public decimal HalfDayWorkHour { get; set; }
        public string ShiftTypeName { get; set; }
        public int CompanyBranchId { get; set; }
        public string CompanyBranchName { get; set; }



    }
    public class HR_RoasterWeekViewModel : IModel
    {
        public long RoasterWeekId { get; set; }
        public int WeekSequenceNo { get; set; }
        public int RoasterId { get; set; }
        public int WeekNo { get; set; }
        public int Mon_ShiftId { get; set; }
        public string Mon_ShiftName { get; set; }
        public int Tue_ShiftId { get; set; }
        public string Tue_ShiftName { get; set; }
        public int Wed_ShiftId { get; set; }
        public string Wed_ShiftName { get; set; }
        public int Thu_ShiftId { get; set; }
        public string Thu_ShiftName { get; set; }
        public int Fri_ShiftId { get; set; }
        public string Fri_ShiftName { get; set; }
        public int Sat_ShiftId { get; set; }
        public string Sat_ShiftName { get; set; }
        public int Sun_ShiftId { get; set; }
        public string Sun_ShiftName { get; set; }
    }
    public class HR_EmployeeRosterViewModel : IModel
    {
        public long EmployeeRoasterId { get; set; }
        public int EmployeeId { get; set; }
        public int RoasterId { get; set; }
        public int DepartmentId { get; set; }
        public int WeekNo { get; set; }
        public string RosterDate { get; set; }
        public int ShiftId { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedDate { get; set; }
        public bool Roster_Status { get; set; }
    }

}
