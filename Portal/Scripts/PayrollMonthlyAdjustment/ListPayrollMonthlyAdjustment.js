$(document).ready(function () {
    BindPayrollMonthList();
    BindCompanyBranchList();
    BindDepartmentList();
 
    setTimeout(
    function () {
        //SearchPayrollMonthlyAdjustment();
    }, 500);

    $("#txtEmployee").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term,
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.FirstName, value: item.EmployeeId, EmployeeCode: item.EmployeeCode, MobileNo: item.MobileNo, DepartmentId: item.DepartmentId, CompanyBranchId: item.CompanyBranchId
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtEmployee").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtEmployee").val(ui.item.label);
            $("#hdnEmployeeId").val(ui.item.value);

            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtEmployee").val("");
                $("#hdnEmployeeId").val("0");
                ShowModel("Alert", "Please select Employee from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.EmployeeCode + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};
  
});


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


function ClearFields() {
    //$("#ddlMonth").val("0");
    //$("#ddlDepartment").val("0");
    //$("#ddlCompanyBranch").val("0");
    //$("#txtEmployee").val("");
    //$("#hdnEmployeeId").val("0");
    window.location.href = "../PayrollMonthlyAdjustment/ListPayrollMonthlyAdjustment";

   
}
function SearchPayrollMonthlyAdjustment() {
    var ddlMonth = $("#ddlMonth");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var ddlDepartment = $("#ddlDepartment"); 
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var requestData = {
        payrollProcessingPeriodId: ddlMonth.val(), employeeId: hdnEmployeeId.val(),
        departmentID: ddlDepartment.val(), companyBranchID: ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../PayrollMonthlyAdjustment/GetPayrollMonthlyAdjustmentList",
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
function BindCompanyBranchList() {
    $("#ddlCompanyBranch,#ddlSearchCompanyBranch").val(0);
    $("#ddlCompanyBranch,#ddlSearchCompanyBranch").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");
            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
                $("#ddlCompanyBranch").attr('disabled', true);
            }
        },
        error: function (Result) {
            $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
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

