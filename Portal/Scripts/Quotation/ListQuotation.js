
$(document).ready(function () {
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
    BindCompanyBranchList();
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

function ClearFields() {
    //$("#txtQuotationNo").val("");
    //$("#txtCustomerName").val("");
    //$("#ddlApprovalStatus").val("0");
    //$("#txtRefNo").val("");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    window.location.href = "../Quotation/ListQuotation";
    
}
function SearchQuotation() {
    var txtQuotationNo = $("#txtQuotationNo");
    var txtCustomerName = $("#txtCustomerName");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtRefNo = $("#txtRefNo");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var requestData = { quotationNo: txtQuotationNo.val().trim(), customerName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: ddlApprovalStatus.val(), companyBranchId: ddlCompanyBranch.val(), LocationId: $("#ddlLocation").val() };
    $.ajax({
        url: "../Quotation/GetQuotationList",
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
function BindBranchLocation() {

    if ($("#ddlCompanyBranch").val() != "0" && $("#ddlCompanyBranch").val() != "") {
        BranchId = $("#ddlCompanyBranch").val();
        $.ajax({
            type: "GET",
            url: "../Fabrication/GetBranchLocationList",
            data: { companyBranchID: BranchId },
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlLocation").append($("<option></option>").val(0).html("-Select Branch Location-"));
                $.each(data, function (i, item) {
                    $("#ddlLocation").append($("<option></option>").val(item.LocationId).html(item.LocationName));
                });
                if ($("#hdnLocationId").val() != "0") {
                    $("#ddlLocation").val($("#hdnLocationId").val());
                }

            },
            error: function (Result) {
                $("#ddlLocation").append($("<option></option>").val(0).html("-Select Branch Location-"));
            }
        });
    }
    else {
        $("#ddlLocation").html('');
        $("#ddlLocation").append($("<option></option>").val(0).html("-Select Branch Location-"));
    }

}