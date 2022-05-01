$(document).ready(function () {
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindFinYearList(hdnCurrentFinyearId.val());
    BindCompanyBranchListForComman();
    GetLeadStatusCountList();
    GetLeadSourceCountList();
    GetLeadFollowUpCountList();
    GetDateWiseLeadConversionCountList();
    GetLeadFollowUpReminderCountList();
});


function GetLeadStatusCountList() {
    
    var requestData = { userId :0, selfOrTeam:"SELF"};
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
function GetLeadSourceCountList() {

    var requestData = {};
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
function GetLeadFollowUpCountList() {

    var requestData = {};
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
function GetDateWiseLeadConversionCountList() {

    var requestData = {};
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
function GetLeadFollowUpReminderCountList() {

    var requestData = {};
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

function OpenLeadFollowUpDetailsPopUp(FollowUpActivityTypeId)
{
    GetLeadFollowUpReminderDueDateList(FollowUpActivityTypeId);
    $("#LeadFollowUpDetails").modal();
}

function GetLeadFollowUpReminderDueDateList(FollowUpActivityTypeId)
{
    var requestData = { userId: 0, selfOrTeam: "SELF", FollowUpActivityTypeId: FollowUpActivityTypeId};
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