
$(document).ready(function () {
   
   // SearchSaleInvoice();
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
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
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
}
function SearchSaleInvoice() {
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var requestData = { fromDate: txtFromDate.val(), toDate: txtToDate.val() };
    $.ajax({
        url: "../SaleInvoice/GetGSTR1List",
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
    var reportOption = $("#ddlPrintOption").val();
    ddlGSTR1 = $("#ddlGSTR1").val();
    if (ddlGSTR1 == '0') {
        ShowModel("Alert", "Select GSTR1 Type!!");
        return false;
    }
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
    ddlGSTR1 = $("#ddlGSTR1").val();
    if (ddlGSTR1 == "B2B") {
        var url = "../SaleInvoice/ReportGSTR1B2B?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
        $('#btnPdf').attr('href', url);
        url = "../SaleInvoice/ReportGSTR1B2B?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=Excel";
        $('#btnExcel').attr('href', url);
    }
    if (ddlGSTR1 == "B2CL") {
        var url = "../SaleInvoice/ReportGSTR1B2CL?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
        $('#btnPdf').attr('href', url);
        url = "../SaleInvoice/ReportGSTR1B2CL?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=Excel";
        $('#btnExcel').attr('href', url);
    }
    if (ddlGSTR1 == "B2CS") {
        var url = "../SaleInvoice/ReportGSTR1B2CS?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
        $('#btnPdf').attr('href', url);
        url = "../SaleInvoice/ReportGSTR1B2CS?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=Excel";
        $('#btnExcel').attr('href', url);
    }
    if (ddlGSTR1 == "CDNR") {
        var url = "../SaleInvoice/ReportGSTR1CDNR?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
        $('#btnPdf').attr('href', url);
        url = "../SaleInvoice/ReportGSTR1CDNR?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=Excel";
        $('#btnExcel').attr('href', url);
    }
    if (ddlGSTR1 == "CDNUR") {
        var url = "../SaleInvoice/ReportGSTR1CDNUR?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
        $('#btnPdf').attr('href', url);
        url = "../SaleInvoice/ReportGSTR1CDNUR?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=Excel";
        $('#btnExcel').attr('href', url);
    }
 
    if (ddlGSTR1=="GSTR3B") {
        var url = "../SaleInvoice/ReportGSTR3B?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
        $('#btnPdf').attr('href', url);
        url = "../SaleInvoice/ReportGSTR3B?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=Excel";
        $('#btnExcel').attr('href', url);
    }
    
}


function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);
}