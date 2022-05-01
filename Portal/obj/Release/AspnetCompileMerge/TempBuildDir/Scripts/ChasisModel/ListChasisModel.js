
$(document).ready(function () { 
    BindProductSubGroupList();
    BindCompanyBranchList();
});
 
function BindProductSubGroupList() {
    $.ajax({
        type: "GET",
        url: "../ChasisModel/GetChasisModelSubGroupList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
            $.each(data, function (i, item) {
                $("#ddlProductSubGroup").append($("<option></option>").val(item.ProductSubGroupId).html(item.ProductSubGroupName));
            });
        },
        error: function (Result) {
            $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
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
    //$("#txtChasisModelName").val("");
    //$("#txtChasisModelCode").val("");
    //$("#txtMotorModelCode").val("");
    //$("#ddlProductSubGroup").val("0");
    //$("#ddlStatus").val("");
    window.location.href = "../ChasisModel/ListChasisModel";

 
    
}
function SearchChasisModel() {
    var txtChasisModelName = $("#txtChasisModelName");
    var txtChasisModelCode = $("#txtChasisModelCode");
    var txtMotorModelCode = $("#txtMotorModelCode");
    var ddlProductSubGroup = $("#ddlProductSubGroup");
    var ddlStatus = $("#ddlStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var requestData = { 
        chasisModelName: txtChasisModelName.val().trim(),
        chasisModelCode: txtChasisModelCode.val().trim(),
        motorModelCode: txtMotorModelCode.val().trim(),
        productSubGroupId: ddlProductSubGroup.val(),
        ChasisModelStatus: ddlStatus.val(),
        companyBranchId:0
    };
    $.ajax({
        url: "../ChasisModel/GetChasisModelList",
        data: requestData,
        dataType: "html",
        asnc: true,
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

    var url = "../ChasisModel/ChasisModelPrint?chasisModelName=" + $("#txtChasisModelName").val() + "&chasisModelCode=" + $("#txtChasisModelCode").val() + "&motorModelCode=" + $("#txtMotorModelCode").val() + "&productSubGroupId=" + $("#ddlProductSubGroup").val() + "&ChasisModelStatus=" + $("#ddlStatus").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    url = "../ChasisModel/ChasisModelPrint?chasisModelName=" + $("#txtChasisModelName").val() + "&chasisModelCode=" + $("#txtChasisModelCode").val() + "&motorModelCode=" + $("#txtMotorModelCode").val() + "&productSubGroupId=" + $("#ddlProductSubGroup").val() + "&ChasisModelStatus=" + $("#ddlStatus").val() + "&reportType=Excel";
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}
