
$(document).ready(function () {
    BindPositionLevelList();
    BindPositionTypeList();
    BindDepartmentList();
    BindCompanyBranchList();
    //SearchResourceRequisition();
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    }); 
});
 
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

function BindPositionLevelList() {
    $("#ddlPositionLevel").val(0);
    $("#ddlPositionLevel").html("");
    $.ajax({
        type: "GET",
        url: "../ResourceRequisition/GetPositionLevelList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPositionLevel").append($("<option></option>").val(0).html("-Select Position Level-"));
            $.each(data, function (i, item) {
                $("#ddlPositionLevel").append($("<option></option>").val(item.PositionLevelId).html(item.PositionLevelName));
            });
        },
        error: function (Result) {
            $("#ddlPositionLevel").append($("<option></option>").val(0).html("-Select Position Level-"));
        }
    });
}

function BindPositionTypeList() {
    $("#ddlPositionType").val(0);
    $("#ddlPositionType").html("");
    $.ajax({
        type: "GET",
        url: "../ResourceRequisition/GetPositionTypeList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPositionType").append($("<option></option>").val(0).html("-Select Position Type-"));
            $.each(data, function (i, item) {
                $("#ddlPositionType").append($("<option></option>").val(item.PositionTypeId).html(item.PositionTypeName));
            });
        },
        error: function (Result) {
            $("#ddlPositionType").append($("<option></option>").val(0).html("-Select Position Type-"));
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


function ClearFields() {
    $("#txtResourceRequisitionNo").val("");
    $("#ddlPositionLevel").val("0");
    $("#ddlPriorityLevel").val("");
    $("#ddlPositionType").val("0");
    $("#ddlDepartment").val("0");
    $("#ddlApprovalStatus").val("0");
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
}
function SearchResourceRequisition() {
    var txtResourceRequisitionNo = $("#txtResourceRequisitionNo");
    var ddlPositionLevel = $("#ddlPositionLevel");
    var ddlPriorityLevel = $("#ddlPriorityLevel");
    var ddlPositionType = $("#ddlPositionType");
    var ddlDepartment = $("#ddlDepartment");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var requestData = {
        requisitionNo: txtResourceRequisitionNo.val().trim(), positionLevelId: ddlPositionLevel.val(), priorityLevel: ddlPriorityLevel.val(),
        positionTypeId: ddlPositionType.val(), departmentId: ddlDepartment.val(), approvalStatus: ddlApprovalStatus.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(),
        companyBranchId: ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../ResourceRequisition/GetResourceRequisitionList",
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
function Export() {
    var divList = $("#divList");
    ExporttoExcel(divList);
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