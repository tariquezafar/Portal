$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtInvoiceFromDate").attr('readOnly', true);
    $("#txtInvoiceToDate").attr('readOnly', true);
    $("#txtInvoiceFromDate,#txtInvoiceToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });


    var hdnAccessMode = $("#hdnAccessMode");
    var hdnProductId = $("#hdnProductId");
    $("#txtProductName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Product/GetProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            $("#hdnProductId").val(ui.item.value);
            // GenerateReportParameters();
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductName").val("");
                $("#hdnProductId").val("0");
                // GenerateReportParameters();
                ShowModel("Alert", "Please select Product from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.desc + "</div>")
      .appendTo(ul);
};
    //var url = "../StockLedger/Report?productTypeId=0&assemblyType=0&productMainGroupId=0&productSubGroupId=0&productId=0&customerBranchId=0&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
    //$('#lnkExport,#lnkExportSummary').attr('href', url);

    //var urlSummary = "../StockLedger/SummaryReport?productTypeId=0&assemblyType=0&productMainGroupId=0&productSubGroupId=0&productId=0&customerBranchId=0&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
    //$('#lnkExportSummary').attr('href', urlSummary);
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




function ClearFields() {
    $("#txtProductName").val("");
    $("#hdnProductId").val("0");
    $("#divList").html("");
    var url = "../StockLedger/Report?productTypeId=0&assemblyType=0&productMainGroupId=0&productSubGroupId=0&productId=0&customerBranchId=0&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
    $('#lnkExport,#lnkExportSummary').attr('href', url);
    var urlSummary = "../StockLedger/SummaryReport?productTypeId=0&assemblyType=0&productMainGroupId=0&productSubGroupId=0&productId=0&customerBranchId=0&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
    $('#lnkExportSummary').attr('href', urlSummary);

}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function GetProductPurchaseList() {

    var hdnProductId = $("#hdnProductId");
    var txtVendorName = $("#txtVendorName");
    var txtInvoiceFromDate = $("#txtInvoiceFromDate");
    var txtInvoiceToDate = $("#txtInvoiceToDate");
    var ddlCompanyBranch= $("#ddlCompanyBranch");


    if (hdnProductId.val() == "0") {
        ShowModel("Alert", "Please Select Product from List")
        return false;
    }
    var requestData = { productId: hdnProductId.val(), VendorName: txtVendorName.val(), InvoiceFromDate: txtInvoiceFromDate.val(), InvoiceToDate: txtInvoiceToDate.val(),companyBranch: ddlCompanyBranch.val() };
    $.ajax({
        url: "../ProductPurchase/GetProductPurchaseList",
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

function OpenPrintPopup() {
    var hdnProductId = $("#hdnProductId");
    if (hdnProductId.val() == "0") {
        ShowModel("Alert", "Please Select Product from List")
        return false;
    }
    $("#printModel").modal();
    GenerateReportParameters();
}

function GenerateReportParameters() {
    var url = "../ProductPurchase/Report?productId=" + $("#hdnProductId").val() + "&companyBranch=" + $("#ddlCompanyBranch").val() + "&companyBranchName=" + $("#ddlCompanyBranch  option:selected").text() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    var urlSummary = "../ProductPurchase/Report?productId=" + $("#hdnProductId").val() + "&companyBranch=" + $("#ddlCompanyBranch").val() + "&companyBranchName=" + $("#ddlCompanyBranch  option:selected").text() + "&reportType=EXCEL";
    $('#btnExcel').attr('href', urlSummary);
}