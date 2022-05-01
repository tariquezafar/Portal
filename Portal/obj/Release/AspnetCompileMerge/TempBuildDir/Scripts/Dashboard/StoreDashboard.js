$(document).ready(function () {
    GetContainer5List();
    GetContainer6List();
   BindCompanyBranchListForComman();
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindFinYearList(hdnCurrentFinyearId.val());
    $("#ddlFinYear").val(hdnCurrentFinyearId);

   

   // GetQautationCountList();
    //GetSOCountList();
    //GetSICountList();
    ///////////
    //GetSOPendingList();
    //GetProdctionSummaryReportList();
    //GetWOPendingList();
    
});


function BindCompanyBranchListForComman() {
    $("#ddlCompanyBranchForComman").val(0);
    $("#ddlCompanyBranchForComman").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
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
                $(":input#ddlCompanyBranchForComman").trigger('change');
            }
            $(":input#ddlCompanyBranchForComman").trigger('change');
        },
        error: function (Result) {
            $("#ddlCompanyBranchForComman").append($("<option></option>").val(0).html("-Select Company Branch-"));
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
            GetQautationCountList();
            GetSOCountList();
            GetSICountList();
        
        },
        error: function (Result) {
        
        }
    });
}

function GetQautationCountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetQuatationCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivQuatationCount").html("");
            $("#DivQuatationCount").html(err);

        },
        success: function (data) {
            $("#DivQuatationCount").html("");
            $("#DivQuatationCount").html(data);

            /*var ctx1 = document.getElementById("barcanvasQuatation").getContext("2d");
            window.myBar = new Chart(ctx1,
                {
                    type: 'bar',
                    data: barChartData,
                    options:
                        {
                            title:
                            {
                                display: true,
                                text: ""
                            },
                            responsive: true,
                            maintainAspectRatio: true
                        }
                });*/


        }
    });
}

function GetSOCountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetSOCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivSOCount").html("");
            $("#DivSOCount").html(err);

        },
        success: function (data) {
            $("#DivSOCount").html("");
            $("#DivSOCount").html(data);

            /*var ctx1 = document.getElementById("barcanvasSaleOrder").getContext("2d");
            window.myBar = new Chart(ctx1,
                {
                    type: 'bar',
                    data: barChartData,
                    options:
                        {
                            title:
                            {
                                display: true,
                                text: ""
                            },
                            responsive: true,
                            maintainAspectRatio: true
                        }
                });*/


        }
    });
}


function GetSICountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetSICountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivSICount").html("");
            $("#DivSICount").html(err);

        },
        success: function (data) {
            $("#DivSICount").html("");
            $("#DivSICount").html(data);

          /*  var ctx1 = document.getElementById("barcanvasSI").getContext("2d");
            window.myBar = new Chart(ctx1,
                {
                    type: 'bar',
                    data: barChartData,
                    options:
                        {
                            title:
                            {
                                display: true,
                                text: ""
                            },
                            responsive: true,
                            maintainAspectRatio: true
                        }
                });*/


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

function CommanMethod() {
    GetProductionCountDashboard();
}

function GetProductionCountDashboard() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetProductionCountDashboard",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#todayProductBOMCount").text("0.00");
            $("#totalProductBOMCount").text("0.00");
            $("#todayPendingWorkOrderCount").text("0.00");
            $("#totalPendingWorkOrderCount").text("0.00");
            $("#todayFinishedGoodCount").text("0.00");
            $("#totalFinishedGoodCount").text("0.00");
            $("#todayFabricationCount").text("0.00");
            $("#totalFabricationCount").text("0.00");

            $("#total_no_of_so_rec_sdept").text("0.00");
            $("#total_no_of_work_order_gen_so").text("0.00");
            $("#total_no_of_work_order_gen_with_so").text("0.00");
            $("#total_no_of_incom_vej_online").text("0.00");

            $("#total_no_of_Work_Order_Pending_ForSo").text("0.00");
            $("#total_no_of_Work_Order_Pending").text("0.00");
            $("#total_no_of_veh_in_fin_good").text("0.00");
        },
        success: function (data) {
            $("#targetAmount").text(data.TargetAmount);
            $("#totalInvoiceAmount").text(data.TotalInvoiceAmount);

            $("#todayProductBOMCount").text(data.TodayProductBOMCount);
            $("#totalProductBOMCount").text(data.TotalProductBOMCount);
            $("#todayPendingWorkOrderCount").text(data.TodayPendingWorkOrderCount);
            $("#totalPendingWorkOrderCount").text(data.TotalPendingWorkOrderCount);
            $("#todayFinishedGoodCount").text(data.TodayFinishedGoodCount);
            $("#totalFinishedGoodCount").text(data.TotalFinishedGoodCount);
            $("#todayFabricationCount").text(data.TodayFabricationCount);
            $("#totalFabricationCount").text(data.TotalFabricationCount);

            $("#total_no_of_so_rec_sdept").text(data.Total_no_of_so_rec_sdept);
            $("#total_no_of_work_order_gen_so").text(data.Total_no_of_work_order_gen_so);
            $("#total_no_of_work_order_gen_with_so").text(data.Total_no_of_work_order_gen_with_so);
            $("#total_no_of_incom_vej_online").text(data.Total_no_of_incom_vej_online);
            $("#total_no_of_Work_Order_Pending_ForSo").text(data.Total_no_of_Work_Order_Pending_ForSo);
            $("#total_no_of_Work_Order_Pending").text(data.Total_no_of_Work_Order_Pending);
            $("#total_no_of_veh_in_fin_good").text(data.Total_no_of_veh_in_fin_good);
        }
    });
}

function GetContainer5List() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetProductionList",
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

function GetContainer6List() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetProductionWOList",
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