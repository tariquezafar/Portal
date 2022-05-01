$(document).ready(function () {
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    BindCompanyBranchList();
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) { 
        }
    });
    $("#ddlFromLocation").append($("<option></option>").val(0).html("-Select From Department-"));
    $("#ddlToLocation").append($("<option></option>").val(0).html("-Select To Department-"));
    //SearchSINRegister();
   
    GenerateReportParameters();

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
function BindFromLocationList(fromLocationId) {
    var companyBranchID = $("#ddlCompanyBranch option:selected").val();
    $("#ddlFromLocation").val(0);
    $("#ddlFromLocation").html("");
    $.ajax({
        type: "GET",
        url: "../SIN/GetFromLocationList",
        data: { companyBranchID: companyBranchID },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlFromLocation").append($("<option></option>").val(0).html("-Select From Department-"));
            $.each(data, function (i, item) {
                $("#ddlFromLocation").append($("<option></option>").val(item.LocationId).html(item.LocationName));
            });

            $("#ddlFromLocation").val(fromLocationId);
        },
        error: function (Result) {
            $("#ddlFromLocation").append($("<option></option>").val(0).html("-Select From Department-"));
        }
    });
}

function BindToLocationList(toLocationId) {
    var companyBranchID = $("#ddlCompanyBranch option:selected").val();
    $("#ddlToLocation").val(0);
    $("#ddlToLocation").html("");
    $.ajax({
        type: "GET",
        url: "../SIN/GetFromLocationList",
        data: { companyBranchID: companyBranchID },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlToLocation").append($("<option></option>").val(0).html("-Select To Department-"));
            $.each(data, function (i, item) {
                $("#ddlToLocation").append($("<option></option>").val(item.LocationId).html(item.LocationName));
                $("#ddlToLocation").val(toLocationId);
            });
        },
        error: function (Result) {
            $("#ddlToLocation").append($("<option></option>").val(0).html("-Select To Department-"));
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
    //$("#txtSINNo").val("");
    //$("#txtRequisitionNo").val("");
    //$("#txtJobNo").val("");
    //$("#ddlCompanyBranch").val("0");
    //$("#ddlFromLocation").val("0");
    //$("#ddlToLocation").val("0");
    //$("#txtCreatedBy").val("");
    //$("#ddlSortBy").val("E.SINNo");
    //$("#ddlSortOrder").val("ASC");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());

    window.location.href = "../SINRegister/ListSINRegister";
  
    
}
function SearchSINRegister() { 
    var txtSINNo = $("#txtSINNo");
    var txtRequisitionNo = $("#txtRequisitionNo");
    var txtJobNo = $("#txtJobNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlFromLocation = $("#ddlFromLocation");
    var ddlToLocation = $("#ddlToLocation");
    var ddlSortBy = $("#ddlSortBy");
    var ddlSortOrder = $("#ddlSortOrder");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var fromLocation = 0;
    var toLocation=0
    if (ddlFromLocation.val() == null || ddlFromLocation.val()==undefined)
    {
        fromLocation = 0;
    }
    else
    {
        fromLocation = ddlFromLocation.val();
    }
    if (ddlToLocation.val() == null || ddlToLocation.val() == undefined) {
        toLocation = 0;
    }
    else {
        toLocation = ddlToLocation.val();
    }

    var requestData = { sinNo: txtSINNo.val().trim(),requisitionNo:txtRequisitionNo.val().trim(), jobNo: txtJobNo.val().trim(),companyBranchId:ddlCompanyBranch.val(), fromLocation: fromLocation, toLocation: toLocation, fromDate: txtFromDate.val(), toDate: txtToDate.val(), employee: "", sortBy: ddlSortBy.val(), SortOrder: ddlSortOrder.val() };
    $.ajax({
        url: "../SINRegister/GetSINRegisterList",
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
function Export()
{
    var divList = $("#divList");
    ExporttoExcel(divList);

}
function OpenPrintPopup() {
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

    var url = "../SINRegister/GetSINRegisterReport?sinNo=" + $("#txtSINNo").val() + "&requisitionNo=" + $("#txtRequisitionNo").val() + "&jobNo=" + $("#txtJobNo").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&fromLocation=" + $("#ddlFromLocation").val() + "&toLocation=" + $("#ddlToLocation").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&sortBy=" + $("#ddlSortBy").val() + "&SortOrder=" + $("#ddlSortOrder").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    var url = "../SINRegister/GetSINRegisterReport?sinNo=" + $("#txtSINNo").val() + "&requisitionNo=" + $("#txtRequisitionNo").val() + "&jobNo=" + $("#txtJobNo").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&fromLocation=" + $("#ddlFromLocation").val() + "&toLocation=" + $("#ddlToLocation").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&sortBy=" + $("#ddlSortBy").val() + "&SortOrder=" + $("#ddlSortOrder").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

}