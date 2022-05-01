
$(document).ready(function () {
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
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnProductId = $("#hdnProductId");
    
    $("#txtProductSubGroupName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Product/GetProductSubGroupAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ProductSubGroupName, value: item.ProductSubGroupId, code: item.ProductSubGroupCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtProductSubGroupName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtProductSubGroupName").val(ui.item.label);
            $("#hdnProductSubGroupId").val(ui.item.value);
            GenerateReportParameters();
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductSubGroupName").val("");
                $("#hdnProductSubGroupId").val("0");
                GenerateReportParameters();
                ShowModel("Alert", "Please select Product from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + "</b></div>")
      .appendTo(ul);
};

    var url = "../SaleInvoice/SubGroupWiseChassisNoSoldReport?productSubGroupId=" + $("#hdnProductSubGroupId").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#lnkExport').attr('href', url);

});

function ClearFields() {

    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
    $("#txtCustomerName").val("");
    $("#hdnCustomerId").val("0");
    $("#txtProductName").val("");
    $("#hdnProductId").val("0");
    $("#txtProductSubGroupName").val("");
    $("#hdnProductSubGroupId").val("0");
    $("#txtChassisNo").val("");

    $("#divList").html("");
    var url = "../SaleInvoice/SubGroupWiseChassisNoSoldReport?productSubGroupId=" + $("#hdnProductSubGroupId").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#lnkExport').attr('href', url);

  
    
}
function GenerateReportParameters() {
    var url = "../SaleInvoice/SubGroupWiseChassisNoSoldReport?productSubGroupId=" + $("#hdnProductSubGroupId").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#lnkExport').attr('href', url);   
     
  
}

function OpenPrintPopup() {
    $("#printModelStockSummary").modal(); 
    GenerateStockSummaryReports();
}

function OpenPrintPopupLedger() {
    $("#printModelStockLedger").modal(); 
    GenerateStockLedgerParameters();
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

//$("body").on('blur', '#txtChassisNo', function () {
//    GenerateReportParameters();
//});

//$("body").on('blur', '#txtFromDate', function () {
//    GenerateReportParameters();
//});

//$("body").on('blur', '#txtToDate', function () {
//    GenerateReportParameters();
//});

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