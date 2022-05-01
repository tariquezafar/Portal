$(document).ready(function () {
    BindCompanyBranchList();
    BindPayrollMonthList();
 
    setTimeout(
    function () {
        //SearchPayrollProcessPeriod();
    }, 500);
  
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
        url: "../PayrollProcessPeriod/GetPayrollMonthList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Select Month-"));
            $.each(data, function (i, item) {
                $("#ddlMonth").append($("<option></option>").val(item.MonthId).html(item.MonthShortName));
            });
        },
        error: function (Result) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Select Month-"));
        }
    });
}


function ClearFields() {
    //$("#ddlMonth").val("0");
    //$("#ddlPayrollProcessStatus").val("");
    //$("#ddlPayrollLocked").val("");
    window.location.href = "../PayrollProcessPeriod/ListPayrollProcessPeriod";

   
}
function SearchPayrollProcessPeriod() {
    var ddlMonth = $("#ddlMonth");
    var ddlPayrollProcessStatus = $("#ddlPayrollProcessStatus");
    var ddlPayrollLocked = $("#ddlPayrollLocked");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
  
    var requestData = {
        monthId: ddlMonth.val(), payrollProcessStatus: ddlPayrollProcessStatus.val(),
        payrollLocked: ddlPayrollLocked.val(),companyBranch: ddlCompanyBranch.val()};
    $.ajax({
        url: "../PayrollProcessPeriod/GetPayrollProcessPeriodList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divList").html("");
            $("#divList").html(err);
        },
        success: function (data) {
            $("#divList").html("");
            $("#divList").html(data);
        }
    });
}