
$(document).ready(function () {
    BindCustomerTypeList();
    var hdnListStatus = $("#hdnListStatus").val();
    if(hdnListStatus=="true")
    {
     SearchCustomer();
    }
    if (hdnListStatus == "today")
    {
        SearchTodayCustomer();
    }
    GenerateReportParameters();
    BindCompanyBranchList();
});
function BindCustomerTypeList() {
    $.ajax({
        type: "GET",
        url: "../Customer/GetCustomerTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCustomerType").append($("<option></option>").val(0).html("-Select Customer Type-"));
            $.each(data, function (i, item) {

                $("#ddlCustomerType").append($("<option></option>").val(item.CustomerTypeId).html(item.CustomerTypeDesc));
            });
        },
        error: function (Result) {
            $("#ddlCustomerType").append($("<option></option>").val(0).html("-Select Customer Type-"));
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

function ClearFields() {
    //$("#txtCustomerName").val("");
    //$("#txtCustomerCode").val("");
    //$("#txtCity").val("");
    //$("#txtState").val("");
    //$("#txtMobileNo").val("");
    //$("#ddlCustomerType").val("0");
    //$("#ddlStatus").val("");
    window.location.href = "../Customer/ListCustomer";
}
function SearchCustomer() {
    var txtCustomerName = $("#txtCustomerName");
    var txtCustomerCode = $("#txtCustomerCode");

    var txtMobileNo = $("#txtMobileNo");
    var ddlCustomerType = $("#ddlCustomerType");
    var txtCity = $("#txtCity");
    var txtState = $("#txtState");
    var ddlStatus = $("#ddlStatus");
    var ddlCompanyBranch =$("#ddlCompanyBranch");
    var requestData = {
        customerName: txtCustomerName.val().trim(), customerCode: txtCustomerCode.val().trim(),
        mobileNo: txtMobileNo.val().trim(), customerTypeid: ddlCustomerType.val(),
        city: txtCity.val().trim(), state: txtState.val().trim(),
        customerStatus: ddlStatus.val(),
        companyBranchId:ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../Customer/GetCustomerList",
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


function OpenPrintPopup() {
    $("#printModel").modal();
    GenerateReportParameters();
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
}
function GenerateReportParameters() {
    var url = "../Customer/CustomerExport?customerName=" + $("#txtCustomerName").val() + "&customerCode=" + $("#txtCustomerCode").val() + "&mobileNo=" + $("#txtMobileNo").val() + "&customerTypeid=" + $("#ddlCustomerType").val() + "&city=" + $("#txtCity").val() + "&state=" + $("#txtState").val() + "&customerStatus=" + $("#ddlStatus").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    url = "../Customer/CustomerExport?customerName=" + $("#txtCustomerName").val() + "&customerCode=" + $("#txtCustomerCode").val() + "&mobileNo=" + $("#txtMobileNo").val() + "&customerTypeid=" + $("#ddlCustomerType").val() + "&city=" + $("#txtCity").val() + "&state=" + $("#txtState").val() + "&customerStatus=" + $("#ddlStatus").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

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

function SearchTodayCustomer()
{
    var requestData = { };
    $.ajax({
        url: "../Customer/GetTodayCustomerList",
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

 