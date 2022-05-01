$(document).ready(function () {
    
    BindPayrollMonthList();
    //$("#btnESIExport").hide();
    //$("#btnESITXTExport").hide();
    $("#btnSave").hide();
    
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


function BindPayrollMonthList() {
    $("#ddlMonth").val(0);
    $("#ddlMonth").html("");
    $.ajax({
        type: "GET",
        url: "../PayrollProcessPeriod/GetPayrollMonthList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Select Month-"));
            $.each(data, function (i, item) {
                $("#ddlMonth").append($("<option></option>").val(item.MonthId).html(item.MonthShortName));
            });
        },
        error: function (Result) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Select Month-"));
        }
    });
}


function GeneratePayrollProcessReport() {
    var ddlMonth = $("#ddlMonth");
    var ddlPayrollProcessYear = $("#ddlPayrollProcessYear");
  
    var requestData = {
        monthId: ddlMonth.val(), year: ddlPayrollProcessYear.val()
    };
    $.ajax({
        url: "../PayRollProcessReport/GetPayrollProcessReportList",
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

function GeneratePayrollProcessReportESI() {
    var ddlMonth = $("#ddlMonth");
    var ddlPayrollProcessYear = $("#ddlPayrollProcessYear");

    var requestData = {
        monthId: ddlMonth.val(), year: ddlPayrollProcessYear.val()
    };
    $.ajax({
        url: "../PayRollProcessReport/GetPayrollProcessReportESIList",
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
            $("#btnSave").show();
            $("#btnSearch").hide();
        }
    });
}

function GeneratePayrollProcessReportESIREPORT() {
    var ddlMonth = $("#ddlMonth");
    var ddlPayrollProcessYear = $("#ddlPayrollProcessYear");

    var requestData = {
        monthId: ddlMonth.val(), year: ddlPayrollProcessYear.val()
    };
    $.ajax({
        url: "../PayRollProcessReport/GetPayrollProcessReportESIListREPORT",
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
            $("#btnSave").show();
            $("#btnSearch").hide();
        }
    });
}


function OpenPrintPopup() {
    $("#printModel").modal();
    GenerateReportParameters();
}

function GenerateReportParameters() {

    var url = "../PayRollProcessReport/PayRollProcessReports?monthId=" + $("#ddlMonth").val() + "&year=" + $("#ddlPayrollProcessYear").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    url = "../PayRollProcessReport/PayRollProcessReports?monthId=" + $("#ddlMonth").val() + "&year=" + $("#ddlPayrollProcessYear").val() + "&reportType=EXCEL";
    $('#btnExcel').attr('href', url);

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

function GenerateECRTXT() {
   
    var ddlMonth = $("#ddlMonth");
    var ddlPayrollProcessYear = $("#ddlPayrollProcessYear");  
    window.location.href = "../PayRollProcessReport/CreateFile?monthId=" + ddlMonth.val() + "&year=" + ddlPayrollProcessYear.val() + "";
  
}

function GenerateESIECRTXT() {

    var ddlMonth = $("#ddlMonth");
    var ddlPayrollProcessYear = $("#ddlPayrollProcessYear");
    window.location.href = "../PayRollProcessReport/GenerateESITXT?monthId=" + ddlMonth.val() + "&year=" + ddlPayrollProcessYear.val() + "";

}

function OpenPrintPopupESI() {
    $("#printModel").modal();
    GenerateReportParametersSI();
}

function GenerateReportParametersSI() {

    var url = "../PayRollProcessReport/PayRollProcessESIReports?monthId=" + $("#ddlMonth").val() + "&year=" + $("#ddlPayrollProcessYear").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    url = "../PayRollProcessReport/PayRollProcessESIReports?monthId=" + $("#ddlMonth").val() + "&year=" + $("#ddlPayrollProcessYear").val() + "&reportType=EXCEL";
    $('#btnExcel').attr('href', url);

}
function ShowHidePrintOptionESI() {
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
function SaveData() {
 
    var eSIViewModel = [];
    $('#tblESI tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var payRollProcessPeriodId = $row.find("#hdnPayRollProcessPeriodId").val();
        var companyBranchId = $row.find("#hdnCompanyBranchId").val();
        var employeeId = $row.find("#hdnEmployeeId").val();
        var monthId = $row.find("#hdnMonthId").val();
        var reasonCode = $row.find(".ddlReasonCode").val();

        if (payRollProcessPeriodId != undefined) {
                var eSIproduct = {
                    PayrollProcessingPeriodId: payRollProcessPeriodId,
                    MonthId: monthId,
                    CompanyBranchId:companyBranchId,
                    EmployeeId: employeeId,
                    ReasonCode: reasonCode,

                };
                eSIViewModel.push(eSIproduct);
          
        }
    });

   
  

    var requestData = { eSIViewModel: eSIViewModel };
    $.ajax({
        url: "../PayRollProcessReport/AddEditESI",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                GeneratePayrollProcessReportESIREPORT();
            }
            else {
                ShowModel("Error", data.message)
            }
        },
        error: function (err) {
            ShowModel("Error", err)
        }
    });

}
