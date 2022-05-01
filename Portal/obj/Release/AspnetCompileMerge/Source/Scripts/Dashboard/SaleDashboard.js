$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });

   
   
    GetContainer9List();
    GetContainer10List();
    GetContainer11List();
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindFinYearList(hdnCurrentFinyearId.val());
    $("#ddlFinYear").val(hdnCurrentFinyearId);
    BindCompanyBranchListForComman();
    

   // GetSOCountList();
    GetSICountList();
    GetSISumByUserId();
    GetQautationCountList();
    //SearchTodayNewCustomer();

   // CommanMethod();

 
    
   
});

//Start

function GetContainer9List()
{
   var requestData = { };
    $.ajax({
        url: "../Dashboard/GetContainer9List",
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

function GetContainer10List() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetContainer10List",
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

function GetContainer11List() {
    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetContainer11List",
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

    var ddlCompanyBranchForComman = $("#ddlCompanyBranchForComman");
    var combraid = 0;
    if (ddlCompanyBranchForComman.val()!= null)
    {
        combraid = ddlCompanyBranchForComman.val();
    }

    var requestData = { userId: 0, selfOrTeam: "SELF", companyBranchId: combraid };
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
            $("#lblSiSumAmount").html(err);

        },
        success: function (data) {
             $("#lblSiSumAmount").html("");
            $("#lblSiSumAmount").html(data.SITotalAmountSum);


        }
    });
}


function GetSICountList() {
    var ddlCompanyBranchForComman = $("#ddlCompanyBranchForComman");
    var combraid = 0;
    if (ddlCompanyBranchForComman.val() != null) {
        combraid = ddlCompanyBranchForComman.val();
    }

    var requestData = { userId: 0, selfOrTeam: "SELF", companyBranchId: combraid };
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

function CommanMethod() {
    GetSaleCountList();
    GetSaleDashboardQutationCount();
    GetSaleDashboardSOCount();
    GetSaleDashboardCount();
    GetSalePendingPaymentCount();
    GetSalePendingTargetCountDashboard();
}

function GetSaleCountList() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetSaleDashboardSaleCount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#todaySaleCount").text("0");
            $("#todaysaleamount").text("0.00");
            $("#mtdsalecount").text("0");
            $("#mtdsaleamount").text("0.00");
            $("#ytdsalecount").text("0");
            $("#ytdsaleamount").text("0.00");
        },
        success: function (data) {
            $("#todaySaleCount").text(data.TodaySaleCount);
            $("#todaysaleamount").text(data.TodaySaleAmount);
            $("#mtdsalecount").text(data.MTDSaleCount);
            $("#mtdsaleamount").text(data.MTASaleAmount);
            $("#ytdsalecount").text(data.YTDSaleCount);
            $("#ytdsaleamount").text(data.YTDSaleAmount);
        }
    });
}


function GetSaleDashboardQutationCount() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetSaleDashboardQutationCount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#todayQuotationCount").text("0");
            $("#todayQuotationamount").text("0.00");
            $("#mtdQuotationCount").text("0");
            $("#mtdQuotationamount").text("0.00");
            $("#ytdQuotationCount").text("0");
            $("#ytdQuotationamount").text("0.00");
        },
        success: function (data) {
            $("#todayQuotationCount").text(data.TodaySaleQutationCount);
            $("#todayQuotationamount").text(data.TodaySaleQutationAmount);
            $("#mtdQuotationCount").text(data.MTDSaleQutationCount);
            $("#mtdQuotationamount").text(data.MTASaleQutationAmount);
            $("#ytdQuotationCount").text(data.YTDSaleQutationCount);
            $("#ytdQuotationamount").text(data.YTDSaleQutationAmount);
        }
    });
}


function GetSaleDashboardSOCount() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetSaleDashboardSOCount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#todaySOCount").text("0");
            $("#todaySOAmount").text("0.00");
            $("#mTDSOCount").text("0");
            $("#mTDSOAmount").text("0.00");
            $("#yTDSOCount").text("0");
            $("#yTDSOCount").text("0.00");
        },
        success: function (data) {
            $("#todaySOCount").text(data.TodaySaleOrderCount);
            $("#todaySOAmount").text(data.TodaySaleOrderAmount);
            $("#mTDSOCount").text(data.MTDSaleOrderCount);
            $("#mTDSOAmount").text(data.MTDSaleOrderAmount);
            $("#yTDSOCount").text(data.YTDSaleOrderCount);
            $("#yTDSOAmount").text(data.YTDSaleOrderAmount);
        }
    });
}


function GetSaleDashboardCount() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetSaleDashboardCount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#todaySOCount").text("0");
            $("#todaySOAmount").text("0.00");
            $("#mTDSOCount").text("0");
            $("#mTDSOAmount").text("0.00");
            $("#yTDSOCount").text("0");
            $("#yTDSOCount").text("0.00");
        },
        success: function (data) {
            $("#totalInvoicePackingCount").text(data.TotalInvoicePackingCount);
            $("#totalSaleReturn").text(data.TotalSaleReturn);
            $("#totalSaleTarget").text(data.TotalSaleTarget);
            $("#todaysaleamt").text(data.TotalSaleAmount);
           
        }
    });
}
   

function GetSalePendingPaymentCount() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetSalePendingPaymentCount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#pendingInvoiceCount").text("0");
            $("#salePendingInvoiceAmount").text("0.00");
           
        },
        success: function (data) {
            $("#pendingInvoiceCount").text(data.salePendingInvoiceCount);
            $("#salePendingInvoiceAmount").text(data.salePendingInvoiceAmount);
        }
    });
}


function GetSalePendingTargetCountDashboard() {
    var companyBranchId = $("#ddlCompanyBranchForComman").val();
    var requestData = { companyBranchId: companyBranchId };
    $.ajax({
        url: "../Dashboard/GetSalePendingTargetCountDashboard",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#targetAmount").text("0.00");
            $("#totalInvoiceAmount").text("0.00");

        },
        success: function (data) {
            $("#targetAmount").text(data.TargetAmount);
            $("#totalInvoiceAmount").text(data.TotalInvoiceAmount);
        }
    });
}