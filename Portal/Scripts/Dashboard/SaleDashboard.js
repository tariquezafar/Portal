﻿$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
   

    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindFinYearList(hdnCurrentFinyearId.val());
    $("#ddlFinYear").val(hdnCurrentFinyearId);
    SearchTodayNewCustomer();
    GetSOForSIPendingList();
    GetQautationCountList();
    GetSOCountList();
    GetSICountList();
    GetSISumByUserId();
   
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

function GetQautationCountList() {

    var requestData = { userId: 0, selfOrTeam: "SELF" };
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

            var ctx1 = document.getElementById("barcanvasQuatation").getContext("2d");
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

function GetSOCountList() {

    var requestData = { userId: 0, selfOrTeam: "SELF" };
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

            var ctx1 = document.getElementById("barcanvasSaleOrder").getContext("2d");
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


function GetSICountList() {

    var requestData = { userId: 0, selfOrTeam: "SELF" };
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

            var ctx1 = document.getElementById("barcanvasSI").getContext("2d");
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

function OpenTodayNewCustomersPopup() {
    $("#TodayCustomersModel").modal();

}

function OpenTotalCustomersPopup() {
    $("#TotalCustomersModel").modal();

}

function SearchTodayNewCustomer() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetTodayNewCustomerList",
        data: requestData,
        dataType: "html",
        type: "POST",
        error: function (err) {
            $("#divTodayNewCustomersList").html("");
            $("#divTodayNewCustomersList").html(err);
        },
        success: function (data) {
            $("#divTodayNewCustomersList").html("");
            $("#divTodayNewCustomersList").html(data);
        }
    });
}


function GetSOForSIPendingList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetSOForSIPendingList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivSOPending").html("");
            $("#DivSOPending").html(err);

        },
        success: function (data) {
            $("#DivSOPending").html("");
            $("#DivSOPending").html(data);

        }
    });
}

   


