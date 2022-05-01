$(document).ready(function () {
    //var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindPayrollMonthList();
    GetEmployeeInOutDetails();
    GetEssEmployeeDetail();
    GetUpComingHolidayDetail();
    GetUpComingActivityDetail();
    GetESSEmployeeRoasterList();
    GetESSEmployeeLeaveApplicationList();
    GetESSEmployeeAdvanceApplicationList();
    GetESSEmployeeAssetApplicationList();
    GetESSEmployeeClaimApplicationList();
    GetESSEmployeeLoanApplicationList();
    GetESSEmployeeTravelApplicationList();
    BindPayrollYearList();
});

function GetEssEmployeeDetail() {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Dashboard/GetESSEmployeeDetail",
        data: {},
        dataType: "json",
        success: function (data) {
            if (data.FirstName != null) {
                $("#hdnEmployeeCode").val(data.EmployeeCode);
                $("#hdnCompanyBranchId").val(data.CompanyBranchId);
                $("#hdnDepartmentId").val(data.DepartmentId);
                $("#hdnDesignationId").val(data.DesignationId);
                $("#hdnEmployeeType").val(data.EmploymentType);
                
                //DesignationId

                $("#profilename").text(data.FirstName + " " + data.LastName);
                $("#userName").text(data.FirstName + " " + data.LastName);
                $("#headername").text(data.FirstName + " " + data.LastName);
                $("#designation").text(data.DesignationName);
                $("#email").text(data.Email);
                $("#alternatemail").text(data.AlternateEmail);
                $("#contactno").text("+91 " + data.AlternateContactno);
                $("#mobile").text("+91 " + data.MobileNo);
                $("#profileimage").attr('src', "/Images/EmployeeImages/" + data.EmployeePicName);
                $("#userimage").attr('src', "/Images/EmployeeImages/" + data.EmployeePicName);
                $("#headerimage").attr('src', "/Images/EmployeeImages/" + data.EmployeePicName);

                $("#department").text(data.DepartmentName);
                $("#employmenttype").text(data.EmploymentType);
                $("#dob").text(data.DateOfBirth);
                $("#bloodgroup").text(data.BloodGroup);
                $("#panno").text(data.PANNo);
                $("#adharno").text(data.AadharNo);
                $("#address").text(data.CAddress);
                $("#cityname").text(data.CCity);
                $("#statename").text(data.CStateName);
                $("#pinno").text(data.CPinCode);
                var gender = (data.Gender == "M") ? "Male" : "Female";
                $("#gender").text(gender);
                $("#maritalstatus").text(data.MaritalStatus);
            }
            else
            {
                ShowModel("Alert", "Please Contact to Administrator");
                setTimeout(
                  function () {
                      window.location.href = "../Dashboard/ModuleDashboard";
                  }, 3000);
            }

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function GetUpComingHolidayDetail()
{
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Dashboard/GetUpComingHolidayDetail",
        data: {},
        dataType: "json",
        success:function(data)
        {
            $("#holidaydate").text(data.HolidayDate);
            $("#holidayDescription").text(data.HolidayDescription);

        },
        error:function(Result)
        {
            ShowModel("Alert", "Problem in Request");
        }

    });
}

function GetUpComingActivityDetail() {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Dashboard/GetUpComingActivityDetail",
        data: {},
        dataType: "json",
        success: function (data) {
            $("#activityDate").text(data.ActivityDate);
            $("#activityDescription").text(data.ActivityDescription);
            $("#calenderName").text(data.CalenderName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }

    });
}

function GetESSEmployeeRoasterList() {
    var hdnEmployeeId = $("#hdnEmployeeId")

    var requestData = {
        employeeId: hdnEmployeeId.val().trim()
    };
    $.ajax({
        url: "../Dashboard/GetESSEmployeeRoasterList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            //$("#carousel-example-generic").html("");
          //  $("#carousel-example-generic").html(err);
        },
        success: function (data) {
          
            $("#carousel-example-generic").html(data);
        }
    });
}

function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}

function GetESSEmployeeAdvanceApplicationList() {
    var requestData = { userId: 0, selfOrTeam: "SELF"};
    $.ajax({
        url: "../Dashboard/GetESSEmployeeAdvanceApplicationList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divAdvanceApplicationList").html("");
            $("#divAdvanceApplicationList").html(err);

        },
        success: function (data) {
            $("#divAdvanceApplicationList").html("");
            $("#divAdvanceApplicationList").html(data);
        }
    });
}

function GetESSEmployeeLeaveApplicationList() {
    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetESSEmployeeLeaveApplicationList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divleaveApplicationList").html("");
            $("#divleaveApplicationList").html(err);

        },
        success: function (data) {
            $("#divleaveApplicationList").html("");
            $("#divleaveApplicationList").html(data);
        }
    });
}

function GetESSEmployeeAssetApplicationList() {
    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetESSEmployeeAssetApplicationList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divAssetApplicationList").html("");
            $("#divAssetApplicationList").html(err);

        },
        success: function (data) {
            $("#divAssetApplicationList").html("");
            $("#divAssetApplicationList").html(data);
        }
    });
}

function GetESSEmployeeClaimApplicationList() {
    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetESSEmployeeClaimApplicationList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divClaimApplicationList").html("");
            $("#divClaimApplicationList").html(err);

        },
        success: function (data) {
            $("#divClaimApplicationList").html("");
            $("#divClaimApplicationList").html(data);
        }
    });
}

function GetESSEmployeeLoanApplicationList() {
    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetESSEmployeeLoanApplicationList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divLoanApplicationList").html("");
            $("#divLoanApplicationList").html(err);

        },
        success: function (data) {
            $("#divLoanApplicationList").html("");
            $("#divLoanApplicationList").html(data);
        }
    });
}

function GetESSEmployeeTravelApplicationList() {
    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetESSEmployeeTravelApplicationList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divTravelApplicationList").html("");
            $("#divTravelApplicationList").html(err);

        },
        success: function (data) {
            $("#divTravelApplicationList").html("");
            $("#divTravelApplicationList").html(data);
        }
    });
}

function GetEmployeeInOutDetails() {
    var attendanceDate = $("#hdnAttendanceDate").val();
    var employeeId = $("#hdnEssEmployeeId").val();
    var requestData = { attendanceDate: attendanceDate, employeeId: employeeId };
    $.ajax({
        url: "../Attendance/GetEmployeeInOutDetails",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data != null) {
                if (data.length == 0) {
                    $("#lblInTime").html("");
                    $("#lblOutTime").html("");
                }
                if (data.length > 0) {
                    $.each(data, function (i, item) {
                        if (item.InOut == "IN") {
                            $("#lblInTime").html(item.TrnDateTime);
                        }
                        if (item.InOut == "OUT") {
                            $("#lblOutTime").html(item.TrnDateTime);
                        }
                    });
                }
            }
            else {
                ShowModel("No Data", "No Data Found")
            }
        },
        error: function (err) {
            ShowModel("Error", err)
        }
    });
}

function BindPayrollMonthList() {
    $("#ddlMonth").val(0);
    $("#ddlMonth").html("");
    $.ajax({
        type: "GET",
        url: "../PayrollProcessPeriod/GetESSPayrollMonthList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Month-"));
            $.each(data, function (i, item) {
                $("#ddlMonth").append($("<option></option>").val(item.MonthId).html(item.MonthShortName));
            });
        },
        error: function (Result) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Month-"));
        }
    });
}

function BindPayrollYearList() {
    $("#ddlYear").val(0);
    $("#ddlYear").html("");
    $.ajax({
        type : "GET",
        url: "../FinYear/GetEssFinYearList",
        data: { },
    dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlYear").append($("<option></option>").val(0).html("-Year-"));
            $.each(data, function (i, item) {
                $("#ddlYear").append($("<option></option>").val(item.FinYearId).html(item.FinYearId));
            });
            },
                error: function (Result) {
                    $("#ddlYear").append($("<option></option>").val(0).html("-Year-"));
            }
            });
            }


function OpenSlipPrintPopup() {
    var ddlMonth = $("#ddlMonth");
    if (ddlMonth.val() == "" || ddlMonth.val() == "0") {
        ShowModel("Alert", "Please select Salary Period!!!")
        return false;
    }
    GenerateSlipReportParameters();
    $("#printModelSalarySlip").modal();
}

function GenerateSlipReportParameters()
{

    var url = "../Dashboard/GenerateESSSalarySlipReport?payrollProcessingPeriodId=" + $("#hdnPayrollProcessingPeriodId").val() + "&companyBranchId=" + $("#hdnCompanyBranchId").val() + "&departmentId=" + $("#hdnDepartmentId").val() + "&designationId=" + $("#hdnDesignationId").val() + "&employeeType=" + $("#hdnEmployeeType").val() + "&employeeCodes=" + $("#hdnEmployeeCode").val() + "&reportType=PDF";
    $('#btnSlipPdf').attr('href', url);
    url = "../Dashboard/GenerateESSSalarySlipReport?payrollProcessingPeriodId=" + $("#hdnPayrollProcessingPeriodId").val() + "&companyBranchId=" + $("#hdnCompanyBranchId").val() + "&departmentId=" + $("#hdnDepartmentId").val() + "&designationId=" + $("#hdnDesignationId").val() + "&employeeType=" + $("#hdnEmployeeType").val() + "&employeeCodes=" + $("#hdnEmployeeCode").val() + "&reportType=Excel";
    $('#btnSlipExcel').attr('href', url);
}

function ShowHideSlipPrintOption() {
    var reportOption = $("#ddlSlipPrintOption").val();
    if (reportOption == "PDF") {
        $("#btnSlipPdf").show();
        $("#btnSlipExcel").hide();
    }
    else if (reportOption == "Excel") {
        $("#btnSlipExcel").show();
        $("#btnSlipPdf").hide();
    }
    else {
        $("#btnSlipExcel").hide();
        $("#btnSlipPdf").hide();
    }
}

function GetPayrollProcessPeriodDetail(obj) {
    var yearId = $("#ddlYear").val();
    var monthId = $(obj).val();
    if (monthId==0)
    {
        ShowModel("Alert", "Please Select Month");
        return false;
    }
   
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Dashboard/GetPayrollProcessPeriodDetail",
        data: { monthId: monthId, yearId: yearId },
        dataType: "json",
        success: function (data) {
            
            $("#hdnPayrollProcessingPeriodId").val(data.PayrollProcessingPeriodId);
            //$("#ddlPayrollProcessStatus").val(data.PayrollProcessStatus);
            //$("#ddlPayrollLocked").val(data.PayrollLocked);
            

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}