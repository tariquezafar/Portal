$(document).ready(function () {
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindFinYearList(hdnCurrentFinyearId.val());
    $("#ddlFinYear").val(hdnCurrentFinyearId);
    GetSOPendingList();
    GetProdctionSummaryReportList();
    GetWOPendingList();
});

function BindFinYearList(selectedFinYear) {
    $.ajax({
        type: "GET",
        url: "../Product/GetFinYearList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $.each(data, function (i, item) {
                $("#ddlFinYear").append($("<option></option>").val(item.FinYearId).html(item.FinYearDesc));
            });
            $("#ddlFinYear").val(selectedFinYear);
        },
        error: function (Result) {
            
        }
    });
}

function SetFinancialYearSession()
{
    var finYearId = $("#ddlFinYear option:selected").val();
    var data = { finYearId: finYearId };
    $.ajax({
        type: "POST",
        url: "../Dashboard/SetFinancialYear",
        data: data,
        asnc: false,
        success: function (data) {
            GetQautationCountList();
            GetSOCountList();
            GetSICountList();
        
        },
        error: function (Result) {
        
        }
    });
}

function GetSOPendingList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetSOPendingCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivPendingSOCount").html("");
            $("#DivPendingSOCount").html(err);

        },
        success: function (data) {
            $("#DivPendingSOCount").html("");
            $("#DivPendingSOCount").html(data);
        }
    });
}
function GetProdctionSummaryReportList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetProdctionSummaryReportList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivProductionReport").html("");
            $("#DivProductionReport").html(err);

        },
        success: function (data) {
            $("#DivProductionReport").html("");
            $("#DivProductionReport").html(data);
        }
    });
}
function GetWOPendingList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetWOPendingCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivPendingWOCount").html("");
            $("#DivPendingWOCount").html(err);

        },
        success: function (data) {
            $("#DivPendingWOCount").html("");
            $("#DivPendingWOCount").html(data);
        }
    });
}