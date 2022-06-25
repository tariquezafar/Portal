$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindFinYearList(hdnCurrentFinyearId.val());
    $("#ddlFinYear").val(hdnCurrentFinyearId);
    GetTodayPOSumAmount();
    GetTodayPISumAmount();
    GetPOCountList();
    GetPICountList();
    GetPendingPOList();
    GetPendingIndentList();

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
            GetPOCountList();
            GetPICountList();
        },
        error: function (Result) {
        
        }
    });
}

function GetPOCountList() {

    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetPOCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivPOCount").html("");
            $("#DivPOCount").html(err);

        },
        success: function (data) {
            $("#DivPOCount").html("");
            $("#DivPOCount").html(data);

            var ctx1 = document.getElementById("barcanvasPO").getContext("2d");
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
                });


        }
    });
}
function GetPICountList() {

    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetPICountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivPIICount").html("");
            $("#DivPIICount").html(err);

        },
        success: function (data) {
            $("#DivPIICount").html("");
            $("#DivPIICount").html(data);

            var ctx1 = document.getElementById("barcanvasPII").getContext("2d");
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
                });


        }
    });
}

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

function GetPendingPOList() {

    var requestData = { };
    $.ajax({
        url: "../Dashboard/GetPendingPOList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivPOPending").html("");
            $("#DivPOPending").html(err);

        },
        success: function (data) {
            $("#DivPOPending").html("");
            $("#DivPOPending").html(data);

        }
    });
}
function GetPendingIndentList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetPendingIndentList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivIndentCount").html("");
            $("#DivIndentCount").html(err);

        },
        success: function (data) {
            $("#DivIndentCount").html("");
            $("#DivIndentCount").html(data);

        }
    });
}