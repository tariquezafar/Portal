$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindFinYearList(hdnCurrentFinyearId.val());
    $("#ddlFinYear").val(hdnCurrentFinyearId);
    BindCompanyBranchListForComman();
    /*
    GetTodayPOSumAmount();
    GetTodayPISumAmount();
    GetPOCountList();
    GetPICountList();
    GetPendingPOList();
    GetPendingIndentList();
    BindCompanyBranchList();
    GetPendingPOCountList();
    GetPendingPQCountList();*/
    GetPurchaseDashboardPOList();
    GetPurchaseDashboardPIList();
    GetPurchaseDashboardPendingPOList();
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
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { userId: 0, companyBranchId: companyBranchId, selfOrTeam: "SELF" };
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
                    data: barPOChartData,
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
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { userId: 0, companyBranchId: companyBranchId, selfOrTeam: "SELF" };
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
                    data: barPIChartData,
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
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { userId: 0,companyBranchId:companyBranchId,selfOrTeam: "SELF" };
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
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { userId: 0, companyBranchId: companyBranchId, selfOrTeam: "SELF" };
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
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
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

function GetPendingPOCountList() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { userId: 0, companyBranchId: companyBranchId, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetPendingPOCountList",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#lblTodayPOCountApproval").html("");
            $("#lblTodayPOCountApproval").html(err);

        },
        success: function (data) {
            $("#lblTodayPOCountApproval").html("");
            $("#lblTodayPOCountApproval").html(data);
        }
    });
}

function GetPendingPQCountList() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = {companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetPendingPQCountList",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#lblTodayPurchaseQuotation").html("");
            $("#lblTodayPurchaseQuotation").html(err);

        },
        success: function (data) {
            $("#lblTodayPurchaseQuotation").html("");
            $("#lblTodayPurchaseQuotation").html(data);
        }
    });
}

function CommanMethod() {
    GetPendingPOCountList();
    GetPendingPQCountList();
    GetTodayPISumAmount();
    GetTodayPOSumAmount();
    GetPOCountList();
    GetPICountList();
    GetPendingPOList();
}

function GetPurchaseDashboardPOList() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetPurchaseDashboardPOs",
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

function GetPurchaseDashboardPIList() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetPurchaseDashboardPIs",
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

function GetPurchaseDashboardPendingPOList() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetPurchaseDashboardPendingPOs",
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
            // $(":input#ddlCompanyBranchForComman").trigger('change');
        },
        error: function (Result) {
            $("#ddlCompanyBranchForComman").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}