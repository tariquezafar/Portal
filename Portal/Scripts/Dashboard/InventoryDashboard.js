$(document).ready(function () {
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    GetSRPendingCountList();
    GetMRNPendingCountList();
    GetJobMRNPendingCountList();
    BindFinYearList(hdnCurrentFinyearId.val());
    $("#ddlFinYear").val(hdnCurrentFinyearId);
    GetReorderPointProductCountList();
    GetInOutProductQuantityCountList();
    GetSINProductQuantityCountList();
   
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
            GetReorderPointProductCountList();
        },
        error: function (Result) {
        
        }
    });
}

function GetReorderPointProductCountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetReorderPointProductCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivReorderProductCount").html("");
            $("#DivReorderProductCount").html(err);

        },
        success: function (data) {
            $("#DivReorderProductCount").html("");
            $("#DivReorderProductCount").html(data);

        }
    });
}
function GetInOutProductQuantityCountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetInOutProductQuantityCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivInOutProductQuantityCount").html("");
            $("#DivInOutProductQuantityCount").html(err);

        },
        success: function (data) {
            $("#DivInOutProductQuantityCount").html("");
            $("#DivInOutProductQuantityCount").html(data);

        }
    });
}

function GetSINProductQuantityCountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetSINProductQuantityCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivSINQuantityCount").html("");
            $("#DivSINQuantityCount").html(err);

        },
        success: function (data) {
            $("#DivSINQuantityCount").html("");
            $("#DivSINQuantityCount").html(data);

        }
    });
}

function OpenWorkOrderPopup() {
    $("#WorkOrderModel").modal();
    SearchWorkOrders();
}
function SearchWorkOrders() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetWorkOrderList",
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

function GetSRPendingCountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetSRPendingCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivSRPending").html("");
            $("#DivSRPending").html(err);

        },
        success: function (data) {
            $("#DivSRPending").html("");
            $("#DivSRPending").html(data);

        }
    });
}
function GetMRNPendingCountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetMRNPendingCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivMRNPending").html("");
            $("#DivMRNPending").html(err);

        },
        success: function (data) {
            $("#DivMRNPending").html("");
            $("#DivMRNPending").html(data);

        }
    });
}

function GetJobMRNPendingCountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetJobMRNPendingCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivJobMRNPending").html("");
            $("#DivJobMRNPending").html(err);
        },
        success: function (data) {
            $("#DivJobMRNPending").html("");
            $("#DivJobMRNPending").html(data);
        }
    });
}