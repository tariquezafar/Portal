$(document).ready(function () {
    BindTeamUserList();
    RefreshTeamData();
});
function RefreshTeamData()
{
    var teamUserId = $("#ddlTeamUser").val();
    GetLeadStatusCountList(teamUserId);
    GetLeadSourceCountList(teamUserId);
    GetLeadFollowUpCountList(teamUserId);
    GetDateWiseLeadConversionCountList(teamUserId);
    GetLeadFollowUpReminderCountList(teamUserId);
}


function GetLeadStatusCountList(teamUserId) {
    
    var requestData = { userId: teamUserId, selfOrTeam: "TEAM" };
    $.ajax({
        url: "../Dashboard/GetLeadStatusCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc:false,
        error: function (err) {
            $("#DivLeadStatusCount").html("");
            $("#DivLeadStatusCount").html(err);

        },
        success: function (data) {
            $("#DivLeadStatusCount").html("");
            $("#DivLeadStatusCount").html(data);
           
        }
    });
}
function GetLeadSourceCountList(teamUserId) {

    var requestData = { userId: teamUserId, selfOrTeam: "TEAM" };
    $.ajax({
        url: "../Dashboard/GetLeadSourceCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivLeadSourceCount").html("");
            $("#DivLeadSourceCount").html(err);

        },
        success: function (data) {
            $("#DivLeadSourceCount").html("");
            $("#DivLeadSourceCount").html(data);

        }
    });
}
function GetLeadFollowUpCountList(teamUserId) {

    var requestData = { userId: teamUserId, selfOrTeam: "TEAM" };
    $.ajax({
        url: "../Dashboard/GetLeadFollowUpCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivLeadFollowUpCount").html("");
            $("#DivLeadFollowUpCount").html(err);

        },
        success: function (data) {
            $("#DivLeadFollowUpCount").html("");
            $("#DivLeadFollowUpCount").html(data);

        }
    });
}
function GetDateWiseLeadConversionCountList(teamUserId) {

    var requestData = { userId: teamUserId, selfOrTeam: "TEAM" };
    $.ajax({
        url: "../Dashboard/GetDateWiseLeadConversionCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivDateWiseLeadConversionCount").html("");
            $("#DivDateWiseLeadConversionCount").html(err);

        },
        success: function (data) {
            $("#DivDateWiseLeadConversionCount").html("");
            $("#DivDateWiseLeadConversionCount").html(data);
            var ctx1 = document.getElementById("DateWiseLeadConversionCountbarcanvas").getContext("2d");
            window.myBar = new Chart(ctx1,
                {
                    type: 'bar',
                    data: dateWiseLeadConversionChartData,
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
function GetLeadFollowUpReminderCountList(teamUserId) {

    var requestData = { userId: teamUserId, selfOrTeam: "TEAM" };
    $.ajax({
        url: "../Dashboard/GetLeadFollowUpReminderDueDateCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivLeadFollowUpReminderCount").html("");
            $("#DivLeadFollowUpReminderCount").html(err);

        },
        success: function (data) {
            $("#DivLeadFollowUpReminderCount").html("");
            $("#DivLeadFollowUpReminderCount").html(data);

        }
    });
}

function OpenLeadFollowUpDetailsPopUp() {
    $("#LeadFollowUpDetails").modal();
}

function OpenLeadFollowUpDetailsPopUp(FollowUpActivityTypeId) {
    GetLeadFollowUpReminderDueDateList(FollowUpActivityTypeId);
    $("#LeadFollowUpDetails").modal();
}

function GetLeadFollowUpReminderDueDateList(FollowUpActivityTypeId) {
    var teamUserId = $("#ddlTeamUser").val();
    var requestData = { userId: teamUserId, selfOrTeam: "TEAM", FollowUpActivityTypeId: FollowUpActivityTypeId };
    $.ajax({
        url: "../Dashboard/GetLeadFollowUpReminderDueDateList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#divLeadFollowUpList").html("");
            $("#divLeadFollowUpList").html(err);

        },
        success: function (data) {
            $("#divLeadFollowUpList").html("");
            $("#divLeadFollowUpList").html(data);

        }
    });
}