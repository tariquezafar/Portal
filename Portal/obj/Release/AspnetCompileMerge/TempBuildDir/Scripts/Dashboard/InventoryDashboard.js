$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
   // BindCompanyBranchListForComman();
    GetInventoryDashboardMRNList();
    GetInventoryDashboardSINList();
    GetInventoryDashboardSTNList();
    GetInventoryDashboardCOSTList();
    GetInventoryDashboardConsumeList();
    GetInventoryDashboardConsumeList();
    GetInventoryDashboardPendingMRNList();
    GetInventoryDashboardPendingOtherList();
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindFinYearList(hdnCurrentFinyearId.val());
    $("#ddlFinYear").val(hdnCurrentFinyearId);
    GetReorderPointProductCountList();
    GetInOutProductQuantityCountList();
    GetSINProductQuantityCountList();
    GetSRPendingCountList();
    GetMRNPendingCountList();
    GetJobMRNPendingCountList();
    GetSISumByUserId();

    //Sale Dashboard
    GetSISumByUserId();

    //Purchase Dashboard
    GetTodayPOSumAmount();
    GetTodayPISumAmount();
    BindCompanyBranchList();

});


function GetTodayPOSumAmount() {
    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetTodayPOSumAmount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#lblTodayPOSum").html("");
            $("#lblTodayPOSum").html(err);
        },
        success: function (data) {
            $("#lblTodayPOSum").html("");
            $("#lblTodayPOSum").html(data.TodayPOSumAmount);
        }
    });
}

function GetTodayPISumAmount() {
    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetTodayPISumAmount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#lblTodayPISum").html("");
            $("#lblTodayPISum").html(err);

        },
        success: function (data) {
            $("#lblTodayPISum").html("");
            $("#lblTodayPISum").html(data.TodayPISumAmount);
        }
    });
}

function GetSISumByUserId() {

    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetSISumByUser",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#lblSiSumAmount").html("");
            //$("#lblSiSumAmount").html(err);

        },
        success: function (data) {
            // $("#lblSiSumAmount").html("");
            $("#lblSiSumAmount").html(data.SITotalAmountSum);


        }
    });
}

function GetSISumByUserId() {

    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetSISumByUser",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#lblSiSumAmount").html("");
            //$("#lblSiSumAmount").html(err);

        },
        success: function (data) {
            // $("#lblSiSumAmount").html("");
            $("#lblSiSumAmount").html(data.SITotalAmountSum);


        }
    });
}

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
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBrachId :companyBranchId};
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
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
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

function BindCompanyBranchList() {
    $("#ddlCompanyBranchForComman").val(0);
    $("#ddlCompanyBranchForComman").html("");
    $.ajax({
        type: "GET",
        url: "../Dashboard/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCompanyBranchForComman").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranchForComman").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");
            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlCompanyBranchForComman").val(hdnSessionCompanyBranchId.val());
                $("#ddlCompanyBranchForComman").attr('disabled', true);
            
            }
        },
        error: function (Result) {
            $("#ddlCompanyBranchForComm an").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}
function CommanMethod() {
    GetMRNInventoryDashboardCount();
    GetSINInventoryDashboardCount();
    GetSTNInventoryDashboardCount();
    GetConsumeProductInventoryDashboardCount();
    GetPendingMRNInventoryDashboardCount();
    GetInventoryDashboardProductPrice();
    GetSINProductQuantityCountList();
    GetInOutProductQuantityCountList();
    GetTotalStockReceiveCount();
}
function GetMRNInventoryDashboardCount() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val() == "0" ? $("#hdnSessionCompanyBranchId").val() : $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetMRNInventoryDashboardCount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#todaymrnCount").html("");
            $("#mtdmrnCount").html("");
            $("#ytdmrnCount").html("");

        },
        success: function (data) {
            $("#todaymrnCount").html(data.MRNTodayCount);
            $("#mtdmrnCount").html(data.MRNMtdCount);
            $("#ytdmrnCount").html(data.MRNYtdCount);
        }
    });
}

function GetSINInventoryDashboardCount() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val() == "0" ? $("#hdnSessionCompanyBranchId").val() : $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetSINInventoryDashboardCount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#todaysinCount").html("");
            $("#mtdsinCount").html("");
            $("#ytdsinCount").html("");

        },
        success: function (data) {
            $("#todaysinCount").html(data.SINTodayCount);
            $("#mtdsinCount").html(data.SINMtdCount);
            $("#ytdsinCount").html(data.SINYtdCount);
        }
    });
}

function GetSTNInventoryDashboardCount() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val() == "0" ? $("#hdnSessionCompanyBranchId").val() : $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetSTNInventoryDashboardCount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#todaystnCount").html("");
            $("#mtdstnCount").html("");
            $("#ytdstnCount").html("");

        },
        success: function (data) {
            $("#todaystnCount").html(data.STNTodayCount);
            $("#mtdstnCount").html(data.STNMtdCount);
            $("#ytdstnCount").html(data.STNYtdCount);
        }
    });
}

function GetConsumeProductInventoryDashboardCount() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val() == "0" ? $("#hdnSessionCompanyBranchId").val() : $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetConsumeProductInventoryDashboardCount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#mostConsumeProductTODAY").html("");
            $("#mostConsumeProductMTD").html("");
            $("#mostConsumeProductYTD").html("");

        },
        success: function (data) {
            $("#mostConsumeProductTODAY").html(data.ProductConsumeTodayCount);
            $("#mostConsumeProductMTD").html(data.ProductConsumeMtdCount);
            $("#mostConsumeProductYTD").html(data.ProductConsumeYtdCount);
        }
    });
}

function GetPendingMRNInventoryDashboardCount() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val() == "0" ? $("#hdnSessionCompanyBranchId").val() : $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetPendingMRNInventoryDashboardCount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#jobWorkCount").html("");
            $("#pendingJobWorkMRN").html("");
            $("#qCPendingForMRN").html("");

        },
        success: function (data) {
            $("#jobWorkCount").html(data.JobWorkCount);
            $("#pendingJobWorkMRN").html(data.PendingJobWorkMRN);
            $("#qCPendingForMRN").html(data.QCPendingForMRN);
        }
    });
}


function GetInventoryDashboardProductPrice() {
    var companyBranchID = $("#ddlCompanyBranchForComman").val();

    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Dashboard/GetInventoryDashboardProductPrice",
        data: { companyBranchId: companyBranchID },
        dataType: "json",
        success: function (data) {
            $("#rawMaterialTotalPrice").html(parseFloat(data.RawMaterialTotalPrice).toFixed(2));
            $("#consumableTotalPrice").html(parseFloat(data.ConsumableTotalPrice).toFixed(2));
            $("#finishedGoodTotalPrice").html(parseFloat(data.FinishedGoodTotalPrice).toFixed(2));
        },
        error: function (Result) {
            $("#rawMaterialTotalPrice").html("0.00");
            $("#consumableTotalPrice").html("0.00");
            $("#finishedGoodTotalPrice").html("0.00");
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function GetTotalStockReceiveCount() {
    var companyBranchID = $("#ddlCompanyBranchForComman").val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Dashboard/GetTotalStockReceiveCount",
        data: { companyBranchId: companyBranchID },
        dataType: "json",
        success: function (data) {
            $("#stockReceiveCount").text(data);
        },
        error: function (Result) {
            $("#stockReceiveCount").html("0.00");
            ShowModel("Alert", "Problem in Request");
        }
    });
}



function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}

function GetInventoryDashboardMRNList() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetInventoryDashboardMRNs",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divContainer5").html("");
            $("#divContainer5").html(err);

        },
        success: function (data) {
            $("#divContainer5").html("");
            $("#divContainer5").html(data);
        }
    });
}

function GetInventoryDashboardSINList() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetInventoryDashboardSINs",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divContainer6").html("");
            $("#divContainer6").html(err);

        },
        success: function (data) {
            $("#divContainer6").html("");
            $("#divContainer6").html(data);
        }
    });
}

function GetInventoryDashboardSTNList() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetInventoryDashboardSTNs",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divContainer7").html("");
            $("#divContainer7").html(err);

        },
        success: function (data) {
            $("#divContainer7").html("");
            $("#divContainer7").html(data);
        }
    });
}
function GetInventoryDashboardCOSTList() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetInventoryDashboardCosts",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divContainer8").html("");
            $("#divContainer8").html(err);

        },
        success: function (data) {
            $("#divContainer8").html("");
            $("#divContainer8").html(data);
        }
    });
}
function GetInventoryDashboardConsumeList() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetInventoryDashboardConsumes",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divContainer9").html("");
            $("#divContainer9").html(err);

        },
        success: function (data) {
            $("#divContainer9").html("");
            $("#divContainer9").html(data);
        }
    });
}

function GetInventoryDashboardPendingMRNList() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetInventoryDashboardPendingMRNs",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divContainer10").html("");
            $("#divContainer10").html(err);

        },
        success: function (data) {
            $("#divContainer10").html("");
            $("#divContainer10").html(data);
        }
    });
}

function GetInventoryDashboardPendingOtherList() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetInventoryDashboardPendingOthers",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divContainer11").html("");
            $("#divContainer11").html(err);

        },
        success: function (data) {
            $("#divContainer11").html("");
            $("#divContainer11").html(data);
        }
    });
}

