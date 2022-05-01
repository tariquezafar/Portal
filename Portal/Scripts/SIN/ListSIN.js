
$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtFromDate").css('cursor', 'pointer');
    $("#txtToDate").css('cursor', 'pointer');
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

   // SearchSIN();
    

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
function BindCompanyBranchList() {
    $("#ddlCompanyBranch").val(0);
    $("#ddlCompanyBranch").html("");
    $.ajax({
        type: "GET",
        url: "../SIN/GetCompanyBranchList",
        data: { },
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

    window.location.href = "../SIN/ListSIN";
    //$("#txtSINNo").val("");
    //$("#txtRequisitionNo").val("");
    //$("#txtJobNo").val("");
    //$("#ddlCompanyBranch").val("0");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    //$("#ddlSINStatus").val("0");
    
}
function SearchSIN() {
    var txtSINNo = $("#txtSINNo");
    var txtRequisitionNo = $("#txtRequisitionNo");
    var txtJobNo = $("#txtJobNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlSINStatus = $("#ddlSINStatus");
    var requestData = { sinNo: txtSINNo.val().trim(),requisitionNo:txtRequisitionNo.val().trim(), jobno: txtJobNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), sINStatus: ddlSINStatus.val() };
    $.ajax({
        url: "../SIN/GetSINList",
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
