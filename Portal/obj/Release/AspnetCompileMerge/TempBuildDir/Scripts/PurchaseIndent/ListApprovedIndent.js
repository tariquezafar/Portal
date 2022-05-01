
$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            GenerateReportParameters();
        }
    });

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnProductId = $("#hdnProductId");
   
    var url = "../SaleInvoice/ChassisNoSoldReport?customerName=" + $("#txtCustomerName").val() + "&productId=" + $("#hdnProductId").val() + "&productSubGroupId=" + $("#hdnProductSubGroupId").val() + "&invoiceNo=" + $("#txtInvoiceNo").val() + "&chassisNo=" + $("#txtChassisNo").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
    $('#lnkExport').attr('href', url);

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



$("body").on('blur', '#txtChassisNo', function () {
    GenerateReportParameters();
});

$("body").on('blur', '#txtFromDate', function () {
    GenerateReportParameters();
});

$("body").on('blur', '#txtToDate', function () {
    GenerateReportParameters();
});


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
function ClearFields() {
    $("#txtIndentNo").val("");
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
    $("#txtSearchPONo").val("");
    $("#txtVendorName").val(""); 
    $("#ddlCompanyBranch").val(0);
}


function GenerateReportParameters() {
    var url = "../PurchaseIndent/ApprovedIndentReport?indentNo=" + $("#txtIndentNo").val() + "&poNo=" + $("#txtSearchPONo").val() + "&vendorName=" + $("#txtVendorName").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&companyBranch=" + $("#ddlCompanyBranch").val() + "&companyBranchName=" + $("#ddlCompanyBranch  option:selected").text() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    var url = "../PurchaseIndent/ApprovedIndentReport?indentNo=" + $("#txtIndentNo").val() + "&poNo=" + $("#txtSearchPONo").val() + "&vendorName=" + $("#txtVendorName").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&companyBranch=" + $("#ddlCompanyBranch").val() + "&companyBranchName=" + $("#ddlCompanyBranch  option:selected").text() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

}

