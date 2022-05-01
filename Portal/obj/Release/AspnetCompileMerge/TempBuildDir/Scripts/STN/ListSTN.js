
$(document).ready(function () {
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
    //SearchSTN();
    BindFromWareBranchList(0);
    BindToWareBranchList(0);
    var hdnStatus = $("#hdnStatus");
    if (hdnStatus.val() == "Final") {
        $("#ddlApprovalStatus").val("Final");
        SearchSTN();
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
function BindFromWareBranchList(companyId) {
    $("#ddlFromWarehouse").val(0);
    $("#ddlFromWarehouse").html("");
    $.ajax({
        type: "GET",
        url: "../STN/GetCompanyBranchList",
        data: { companyId: companyId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlFromWarehouse").append($("<option></option>").val(0).html("-Select From Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlFromWarehouse").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");
            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlFromWarehouse").val(hdnSessionCompanyBranchId.val());
                $("#ddlFromWarehouse").attr('disabled', true);
            }
        },
        error: function (Result) {
            $("#ddlFromWarehouse").append($("<option></option>").val(0).html("-Select From Company Branch-"));
        }
    });
}
function BindToWareBranchList(companyId) {
    $("#ddlToWarehouse").val(0);
    $("#ddlToWarehouse").html("");
    $.ajax({
        type: "GET",
        url: "../STN/GetCompanyBranchList",
        data: { companyId: companyId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlToWarehouse").append($("<option></option>").val(0).html("-Select To Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlToWarehouse").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            
        },
        error: function (Result) {
            $("#ddlToWarehouse").append($("<option></option>").val(0).html("-Select To Company Branch-"));
        }
    });
}
function ClearFields() {
    window.location.href = "../STN/ListSTN";
    //var id = $("#hdnSessionUserID").val();
    //$("#txtSTNNo").val("");
    //$("#txtGRNo").val("");
    //$("#ddlApprovalStatus").val(0);
   
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    //if (id == 2) {
    //    $("#ddlFromWarehouse").val("0");
    //    $("#ddlToWarehouse").val("0");
    //}
    
}
function SearchSTN() {
    var txtSTNNo = $("#txtSTNNo");
    var txtGRNo = $("#txtGRNo");
    var ddlFromWarehouse = $("#ddlFromWarehouse");
    var ddlToWarehouse = $("#ddlToWarehouse");
   
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var requestData = { stnNo: txtSTNNo.val().trim(), glNo: txtGRNo.val().trim(), fromLocation: ddlFromWarehouse.val(), toLocation: ddlToWarehouse.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: ddlApprovalStatus.val() };
    $.ajax({
        url: "../STN/GetSTNList",
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
