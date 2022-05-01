$(document).ready(function () {
    BindCompanyBranchList();
    var hdnTodayList = $("#hdnTodayList");
    if (hdnTodayList.val() == "true") {
        DashboardSearchSaleInvoice();
    }
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
    //$("#txtSINo").val("");
    //$("#txtCustomerName").val("");
    //$("#txtRefNo").val(""); 
    //$("#ddlApprovalStatus").val("0");
    //$("#ddlCompanyBranch").val("0");
    //$("#ddlInvoiceType").val("0"); 
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    //$("#ddlSaleType").val("0");
    window.location.href = "../SaleInvoice/ListSaleInvoice";
    
}
function SearchSaleInvoice() {
    var txtSINo = $("#txtSINo");
    var txtCustomerName = $("#txtCustomerName");
    var txtRefNo = $("#txtRefNo");
    var ddlInvoiceType = $("#ddlInvoiceType");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlSaleType = $("#ddlSaleType");
    var txtSearchCreatedBy = $("#txtSearchCreatedBy");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { saleinvoiceNo: txtSINo.val().trim(), customerName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), invoiceType: ddlInvoiceType.val(), approvalStatus: ddlApprovalStatus.val(), saleType: ddlSaleType.val(), CreatedByUserName: txtSearchCreatedBy.val() };
    $.ajax({
        url: "../SaleInvoice/GetSaleInvoiceList",
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

function DashboardSearchSaleInvoice() {
    var txtSINo = $("#txtSINo");
    var txtCustomerName = $("#txtCustomerName");
    var txtRefNo = $("#txtRefNo");
    var ddlInvoiceType = $("#ddlInvoiceType");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlSaleType = $("#ddlSaleType");
    var txtSearchCreatedBy = $("#txtSearchCreatedBy");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var hdnCurrentDate = $("#hdnCurrentDate");
    var requestData = { saleinvoiceNo: txtSINo.val().trim(), customerName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: hdnCurrentDate.val(), toDate: hdnCurrentDate.val(), invoiceType: ddlInvoiceType.val(), approvalStatus: ddlApprovalStatus.val(), saleType: ddlSaleType.val(), CreatedByUserName: txtSearchCreatedBy.val() };
    $.ajax({
        url: "../SaleInvoice/GetSaleInvoiceList",
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