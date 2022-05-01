
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
    setTimeout(
     function () {
        // SearchCarryForwardChasis();
     }, 1000);
    BindCompanyBranchList();


    $("#txtFromDate").val("01-Jan-2018");
    $("#txtToDate").val("31-Dec-2019");

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
    //$("#txtCarryForwardNo").val("");
    //$("#ddlCompanyBranch").val("0");
    //$("#ddlApprovalStatus").val("0");
    //$("#PrevddlYear").val("0");
    //$("#PrevddlMonth").val("0");
    //$("#CarryddlYear").val("0");
    //$("#CarryddlMonth").val("0");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    window.location.href = "../CarryForwardChasis/ListCarryForwardChasis";

    
}
function SearchCarryForwardChasis() {
    var txtCarryForwardNo = $("#txtCarryForwardNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var PrevddlYear = $("#PrevddlYear");
    var PrevddlMonth = $("#PrevddlMonth");  
    var CarryddlYear = $("#CarryddlYear");
    var CarryddlMonth = $("#CarryddlMonth");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
  
    var requestData = {
        carryForwardNo: txtCarryForwardNo.val(), companyBranchId: ddlCompanyBranch.val(), prevddlYear: PrevddlYear.val(),
        prevddlMonth: PrevddlMonth.val(),carryddlYear:CarryddlYear.val(),carryddlMonth:CarryddlMonth.val(),
        fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: ddlApprovalStatus.val()
    };
    $.ajax({
        url: "../CarryForwardChasis/GetCarryForwardChasisList",
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