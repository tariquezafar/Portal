$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindFinYearList(hdnCurrentFinyearId.val());
    $("#ddlFinYear").val(hdnCurrentFinyearId);
    BindTeamUserList();
    RefreshTeamData();
});

function RefreshTeamData() {
    var teamUserId = $("#ddlTeamUser").val();
    GetPOCountList(teamUserId);
    GetPICountList(teamUserId);
    GetTodayPOSumAmount(teamUserId);
    GetTodayPISumAmount(teamUserId);
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

function SetFinancialYearSession() {
    var finYearId = $("#ddlFinYear option:selected").val();
    var data = { finYearId: finYearId };
    $.ajax({
        type: "POST",
        url: "../Dashboard/SetFinancialYear",
        data: data,
        asnc: false,
        success: function (data) {
            GetPOCountList(teamUserId);
            GetPICountList(teamUserId);
        },
        error: function (Result) {

        }
    });
}

function GetPOCountList(teamUserId) {

    var requestData = { userId: teamUserId, selfOrTeam: "TEAM" };
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
function GetPICountList(teamUserId) {

    var requestData = { userId: teamUserId, selfOrTeam: "TEAM" };
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

function BindTeamUserList() {
    $("#ddlTeamUser").val(0);
    $("#ddlTeamUser").html("");
    $.ajax({
        type: "GET",
        url: "../Dashboard/GetTeamDetailList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlTeamUser").append($("<option></option>").val(0).html("-All Team(s)-"));
            $.each(data, function (i, item) {
                $("#ddlTeamUser").append($("<option></option>").val(item.UserId).html(item.FullName));
            });
        },
        error: function (Result) {
            $("#ddlTeamUser").append($("<option></option>").val(0).html("-All Team(s)-"));
        }
    });
}

function GetTodayPOSumAmount(teamUserId) {
    var requestData = { userId: teamUserId, selfOrTeam: "TEAM" };
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

function GetTodayPISumAmount(teamUserId) {
    var requestData = { userId: teamUserId, selfOrTeam: "TEAM" };
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