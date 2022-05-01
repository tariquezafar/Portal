
$(document).ready(function () {
    $("#txtPurchaseIndentNo").attr('readOnly', true);
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


function OpenPurchaseIndentSearchPopup() {
    $("#SearchPurchaseIndentModel").modal();
    BindSearchCompanyBranchList();
}

function BindSearchCompanyBranchList() {
    $("#ddlSearchCompanyBranch").val(0);
    $("#ddlSearchCompanyBranch").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSearchCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlSearchCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");
            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlSearchCompanyBranch").val(hdnSessionCompanyBranchId.val());
                $("#ddlSearchCompanyBranch").attr('disabled', true);
            }
        },
        error: function (Result) {
            $("#ddlSearchCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}

function SelectPurchaseIndent(purchaseIndentId, purchaseIndentNo, purchaseIndentDate) {
    $("#txtPurchaseIndentNo").val(purchaseIndentNo);
    $("#hdnPurchaseIndentId").val(purchaseIndentId);
    $("#txtPurchaseIndentDate").val(purchaseIndentDate);

    $("#SearchPurchaseIndentModel").modal('hide');
}

function SearchPurchaseIndent() {
    var txtIndentNo = $("#txtIndentNo");
    var ddlIndentType = $("#ddlIndentType");
    var txtCustomerName = $("#txtCustomerName");
    var ddlSearchCompanyBranch = $("#ddlSearchCompanyBranch");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var requestData = { indentNo: txtIndentNo.val().trim(), indentType: ddlIndentType.val(), customerName: txtCustomerName.val().trim(), companyBranchId: ddlSearchCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../PurchaseQuotation/GetPurchaseIndentList",
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

function GenerateReportParameters() {
    var hdnPurchaseIndentId = $("#hdnPurchaseIndentId");
   var url = "../PurchaseQuotation/Report?indentId=" + hdnPurchaseIndentId.val() + "&reportType=PDF";
   $('#btnPdf').attr('href', url);
   var urlSummary = "../PurchaseQuotation/Report?indentId=" + hdnPurchaseIndentId.val() + "&reportType=Excel";
   $('#btnExcel').attr('href', urlSummary);
}
function OpenPrintPopup() {
    var hdnPurchaseIndentId = $("#hdnPurchaseIndentId");
    if (hdnPurchaseIndentId.val() == "0") {
        ShowModel("Alert", "Select Purchase Indent");
        $("#txtPurchaseIndentNo").focus();
        return false;
    }
  
    $("#printModelReport").modal();
    GenerateReportParameters();
}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

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
    $("#txtPurchaseIndentNo").val("");
    $("#hdnPurchaseIndentId").val("0");
}