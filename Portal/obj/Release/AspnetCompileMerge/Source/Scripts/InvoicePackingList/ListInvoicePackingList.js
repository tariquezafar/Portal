$(document).ready(function () {
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    BindPackingListType();
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
           }
    });
    BindCompanyBranchList();
    var hdnTotalPackingList = $("#hdnTotalPackingList").val();
    if(hdnTotalPackingList=="true")
    {
        SearchInvoicePackingList()
    }
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
function BindPackingListType() {

    $("#ddlPackingListType").val(0);
    $("#ddlPackingListType").html("");
    $.ajax({
        type: "GET",
        url: "../PackingList/GetAllPackingListType",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPackingListType").append($("<option></option>").val(0).html("-Select Packing Type-"));
            $.each(data, function (i, item) {
                $("#ddlPackingListType").append($("<option></option>").val(item.PackingListTypeID).html(item.PackingListTypeName));
            });
        },
        error: function (Result) {
            $("#ddlPackingListType").append($("<option></option>").val(0).html("-Select Packing Type-"));
        }
    });
}
function ClearFields() {
    //$("#txtInvoicePackingListNo").val("");
    //$("#txtInvoiceNo").val("");
    //$("#ddlPackingListType").val("0");
    //$("#ddlApprovalStatus").val("0");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    window.location.href = "../InvoicePackingList/ListInvoicePackingList";
    
}

function SearchInvoicePackingList() {
    var txtInvoicePackingListNo = $("#txtInvoicePackingListNo");
    var txtInvoiceNo = $("#txtInvoiceNo");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var ddlPackingListType = $("#ddlPackingListType");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var txtSearchCreatedBy = $("#txtSearchCreatedBy");
    var ddlCompanyBranch = $("#ddlCompanyBranch")
    var requestData = {
        invoicePackingListNo: txtInvoicePackingListNo.val().trim(), invoiceNo: txtInvoiceNo.val().trim(),
        packingListType:ddlPackingListType.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(),
        approvalStatus:ddlApprovalStatus.val(), CreatedByUserName: txtSearchCreatedBy.val(),
        companyBranchId: ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../InvoicePackingList/GetInvoicePackingList",
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
        }
    });
}
