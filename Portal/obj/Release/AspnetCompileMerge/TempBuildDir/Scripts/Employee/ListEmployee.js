
$(document).ready(function () {
    BindDepartmentList();
    BindCompanyBranchList();
   // SearchEmployee(); 
});
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
$(".alpha-only").on("input", function () {
    var regexp = /[^a-zA-Z]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".alpha-space-only").on("input", function () {
    var regexp = /[^a-zA-Z\s]+$/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".numeric-only").on("input", function () {
    var regexp = /\D/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".alpha-numeric-only").on("input", function () {
    var regexp = /[^a-zA-Z0-9]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});



function ClearFields()
{
    $("#txtEmployeeName").val("");
    $("#txtEmployeeCode").val("");

    $("#txtMobileNo").val("");
    $("#txtEmail").val("");
    $("#txtPANNo").val("");
    $("#ddlDepartment").val("0");
    $("#ddlEmploymentType").val("0");
    $("#ddlCurrentStatus").val("0");
    $("#ddlStatus").val("");
}
function SearchEmployee() { 
    var txtEmployeeName = $("#txtEmployeeName");
    var txtEmployeeCode = $("#txtEmployeeCode");

    var txtMobileNo = $("#txtMobileNo");
    var txtEmail = $("#txtEmail");
    var txtPANNo = $("#txtPANNo");
    var ddlDepartment = $("#ddlDepartment");
    var ddlEmploymentType = $("#ddlEmploymentType");
    var ddlCurrentStatus = $("#ddlCurrentStatus");
    var ddlStatus = $("#ddlStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    
    var requestData = { employeeName: txtEmployeeName.val().trim(), employeeCode: txtEmployeeCode.val().trim(), mobileNo: txtMobileNo.val().trim(), email: txtEmail.val().trim(), panNo: txtPANNo.val().trim(), departmentId: ddlDepartment.val(), employeeType: ddlEmploymentType.val(), currentStatus: ddlCurrentStatus.val(), employeeStatus: ddlStatus.val(), companyBranchId: ddlCompanyBranch.val() };
    $.ajax({
        url: "../Employee/GetEmployeeList",
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