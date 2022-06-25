
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

        }
    });
    var hdnlistStatus = $("#hdnlistStatus").val();
    if (hdnlistStatus=="true")
    {
     SearchPI();
    }
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("--Select Company Branch--"));
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
    //$("#txtPONo").val("");
    //$("#txtPINo").val(""); 
    //$("#txtVendorName").val("");
    //$("#txtRefNo").val("");
    //$("#ddlApprovalStatus").val("0"); 
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    //$("#ddlPurchaseType").val("0");
    //$("#txtSearchCreatedBy").val("");
    //$("#txtSearchPONO").val("");
    window.location.href = "../PurchaseInvoice/ListPI";
    
}
function SearchPI() {
    var txtPINo = $("#txtPINo");
    var txtVendorName = $("#txtVendorName");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtRefNo = $("#txtRefNo");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate"); 
    var ddlPurchaseType = $("#ddlPurchaseType");
    var txtSearchCreatedBy = $("#txtSearchCreatedBy");
    var txtSearchPONO = $("#txtSearchPONO");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var MRNNO = $("#txtSearchMRNNo");
    var requestData = {
        piNo: txtPINo.val().trim(), vendorName: txtVendorName.val().trim(),
        refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(),
        approvalStatus: ddlApprovalStatus.val(), purchaseType: ddlPurchaseType.val(),
        CreatedByUserName: txtSearchCreatedBy.val(), poNo: txtSearchPONO.val(), companyBranch: ddlCompanyBranch.val(),
        MRNNo: MRNNO.val()
    };
    $.ajax({
        url: "../PurchaseInvoice/GetPIList",
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
