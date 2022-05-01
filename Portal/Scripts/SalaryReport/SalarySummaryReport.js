$(document).ready(function () {
    BindCompanyBranchList();
    
    $("#ddlPayrollProcessStatus").attr('disabled', true);
    $("#ddlPayrollLocked").attr('disabled', true);
 
    BindPayrollMonthList();
   
    BindDepartmentList();
    $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));

    GenerateReportParameters();
    GenerateSlipReportParameters();
 
 
});


function BindCompanyBranchList() {
    $("#ddlCompanyBranch").val(0);
    $("#ddlCompanyBranch").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");

            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
                $("#ddlCompanyBranch").attr('disabled', true);
            }
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}




function BindPayrollMonthList() {
    $("#ddlMonth").val(0);
    $("#ddlMonth").html("");
    $.ajax({
        type: "GET",
        url: "../SalaryReport/GetProcessedMonthList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Select Period-"));
            $.each(data, function (i, item) {
                $("#ddlMonth").append($("<option></option>").val(item.PayrollProcessingPeriodId).html(item.PayrollProcessingStartDate + ' - ' + item.PayrollProcessingEndDate));
            });
        },
        error: function (Result) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Select Period-"));
        }
    });
}

function BindDepartmentList() {
    $.ajax({
        type: "GET",
        url: "../Employee/GetDepartmentList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlDepartment").append($("<option></option>").val(0).html("-Select Department-"));
            $.each(data, function (i, item) {

                $("#ddlDepartment").append($("<option></option>").val(item.DepartmentId).html(item.DepartmentName));
            });
        },
        error: function (Result) {
            $("#ddlDepartment").append($("<option></option>").val(0).html("-Select Department-"));
        }
    });
}

function BindDesignationList(designationId) {
    var departmentId = $("#ddlDepartment option:selected").val();
    $("#ddlDesignation").val(0);
    $("#ddlDesignation").html("");
    if (departmentId != undefined && departmentId != "" && departmentId != "0") {
        var data = { departmentId: departmentId };
        $.ajax({
            type: "GET",
            url: "../Employee/GetDesignationList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
                $.each(data, function (i, item) {
                    $("#ddlDesignation").append($("<option></option>").val(item.DesignationId).html(item.DesignationName));
                });
                $("#ddlDesignation").val(designationId);
            },
            error: function (Result) {
                $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
            }
        });
    }
    else {

        $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
    }
}
function GetProcessStartEndDate() {
    var monthId = $("#ddlMonth option:selected").val();
    $("#txtPayrollProcessingStartDate").val("");
    $("#txtPayrollProcessingEndDate").val("");
    $.ajax({
        type: "GET",
        url: "../PayrollProcessPeriod/GetPayrollMonthStartAndEndDate",
        data: { monthId: monthId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#txtPayrollProcessingStartDate").val(data.PayrollProcessingStartDate);
            $("#txtPayrollProcessingEndDate").val(data.PayrollProcessingEndDate);
            
            
        },
        error: function (Result) {
            $("#txtPayrollProcessingStartDate").val("");
            $("#txtPayrollProcessingEndDate").val("");
        }
    });
}
function GetPayrollProcessPeriodDetail(obj) {
    var payrollProcessingPeriodId = $(obj).val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PayrollProcessPeriod/GetPayrollProcessPeriodDetail",
        data: { payrollProcessingPeriodId: payrollProcessingPeriodId },
        dataType: "json",
        success: function (data) {
            
            $("#hdnpayrollProcessingPeriodId").val(data.PayrollProcessingPeriodId);
            $("#ddlPayrollProcessStatus").val(data.PayrollProcessStatus);
            $("#ddlPayrollLocked").val(data.PayrollLocked);
            

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}

function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function OpenPrintPopup() {
    var ddlMonth = $("#ddlMonth");
    if (ddlMonth.val() == "" || ddlMonth.val() == "0") {
        ShowModel("Alert", "Please select Salary Period!!!")
        return false;
    }
    GenerateReportParameters();
    $("#printModel").modal();

    
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
function ShowHidePrintOption() {
    var reportOption = $("#ddlPrintOption").val();
    if (reportOption == "PDF") {
        $("#btnPdf").show();
        $("#btnExcel").hide();
    }
    else if (reportOption == "Excel") {
        $("#btnExcel").show();
        $("#btnPdf").hide();
    }
    else
    {
        $("#btnExcel").hide();
        $("#btnPdf").hide();
    }
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
function GenerateReportParameters() {

    var url = "../SalaryReport/GenerateSalarySummaryReport?payrollProcessingPeriodId=" + $("#ddlMonth").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&departmentId=" + $("#ddlDepartment").val() + "&designationId=" + $("#ddlDesignation").val() + "&employeeType=" + $("#ddlEmploymentType").val() + "&employeeCodes=" + $("#txtEmployeeCodes").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    url = "../SalaryReport/GenerateSalarySummaryReport?payrollProcessingPeriodId=" + $("#ddlMonth").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&departmentId=" + $("#ddlDepartment").val() + "&designationId=" + $("#ddlDesignation").val() + "&employeeType=" + $("#ddlEmploymentType").val() + "&employeeCodes=" + $("#txtEmployeeCodes").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

}
function GenerateSlipReportParameters() {

    var url = "../SalaryReport/GenerateSalarySlipReport?payrollProcessingPeriodId=" + $("#ddlMonth").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&departmentId=" + $("#ddlDepartment").val() + "&designationId=" + $("#ddlDesignation").val() + "&employeeType=" + $("#ddlEmploymentType").val() + "&employeeCodes=" + $("#txtEmployeeCodes").val() + "&reportType=PDF";
    $('#btnSlipPdf').attr('href', url);
    url = "../SalaryReport/GenerateSalarySlipReport?payrollProcessingPeriodId=" + $("#ddlMonth").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&departmentId=" + $("#ddlDepartment").val() + "&designationId=" + $("#ddlDesignation").val() + "&employeeType=" + $("#ddlEmploymentType").val() + "&employeeCodes=" + $("#txtEmployeeCodes").val() + "&reportType=Excel";
    $('#btnSlipExcel').attr('href', url);

}