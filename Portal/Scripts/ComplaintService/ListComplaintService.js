
$(document).ready(function () {
    BindServiceEngineerList();
    BindSDealerList();
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
    BindCompanyBranchList();

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

function ClearFields() {
    //$("#txtComplaintNo").val("");
    //$("#ddlComplaintType").val("0");
    //$("#ddlComplaintMode").val("0");
    //$("#txtCustomerMobile").val("");
    //$("#txtCustomerName").val(" ");
    //$("#txtFromDate").val("");
    //$("#ddlStatus").val("0");
    window.location.href = "../ComplaintService/ListComplaintService";

}

function SearchComplaintservice() {
    var txtComplaintNo = $("#txtComplaintNo");
    var ddlComplaintType = $("#ddlComplaintType");
    var ddlComplaintMode = $("#ddlComplaintMode");
    var txtCustomerMobile = $("#txtCustomerMobile");
    var txtCustomerName = $("#txtCustomerName");
    var txtComplaintSubject = $("#txtComplaintSubject");
    var ddlStatus = $("#ddlStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlServiceEngineer = $("#ddlServiceEngineer");
    var ddlDealer = $("#ddlDealer");

    var requestData = {
        complaintNo: txtComplaintNo.val().trim(),
        enquiryType: ddlComplaintType.val(),
        complaintMode: ddlComplaintMode.val().trim(),
        customerMobile: txtCustomerMobile.val(),
        customerName: txtCustomerName.val(),
        approvalStatus: ddlStatus.val(),
        companyBranchId: ddlCompanyBranch.val(),
        serviceEngineerId: ddlServiceEngineer.val(),
        dealerId: ddlDealer.val()
    };
    $.ajax({
        url: "../ComplaintService/GetComplaintServiceList",
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

function SearchAPCSComplaintservice() {
    debugger
    var txtComplaintNo = $("#txtComplaintNo");
    var ddlComplaintType = $("#ddlComplaintType");
    var ddlComplaintMode = $("#ddlComplaintMode");
    var txtCustomerMobile = $("#txtCustomerMobile");
    var txtCustomerName = $("#txtCustomerName");
    var txtComplaintSubject = $("#txtComplaintSubject");
    var ddlStatus = $("#ddlStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlServiceEngineer = $("#ddlServiceEngineer");
    var ddlDealer = $("#ddlDealer");

    var requestData = {
        complaintNo: txtComplaintNo.val().trim(),
        enquiryType: ddlComplaintType.val(),
        complaintMode: ddlComplaintMode.val().trim(),
        customerMobile: txtCustomerMobile.val(),
        customerName: txtCustomerName.val(),
        approvalStatus: ddlStatus.val(),
        companyBranchId: ddlCompanyBranch.val(),
        serviceEngineerId: ddlServiceEngineer.val(),
        dealerId: ddlDealer.val()
    };
    $.ajax({
        url: "../ComplaintService/GetAPCSComplaintServiceList",
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

function BindServiceEngineerList() {
    $.ajax({
        type: "GET",
        url: "../Employee/GetDesignationByDepartmentID",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlServiceEngineer").append($("<option></option>").val(0).html("Select Service Engineer"));
            $.each(data, function (i, item) {
                $("#ddlServiceEngineer").append($("<option></option>").val(item.EmployeeId).html(item.EmployeeName));
            });
        },
        error: function (Result) {
            $("#ddlServiceEngineer").append($("<option></option>").val(0).html("Select Service Engineer"));
        }
    });
}

function BindSDealerList() {
    $.ajax({
        type: "GET",
        url: "../ComplaintService/GetCustomerTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlDealer").append($("<option></option>").val(0).html("Select Dealer"));
            $.each(data, function (i, item) {
                $("#ddlDealer").append($("<option></option>").val(item.ValueInt).html(item.Text));
            });
        },
        error: function (Result) {
            $("#ddlDealer").append($("<option></option>").val(0).html("Select Service Engineer"));
        }
    });
}

