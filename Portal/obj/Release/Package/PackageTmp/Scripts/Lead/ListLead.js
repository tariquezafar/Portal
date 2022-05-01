$(document).ready(function () {
    BindTeamUserList();
    BindStateList();
  //  SearchLead();
    BindLeadSourceList();
    BindLeadStatusList();
    GenerateReportParameters();
    BindLeadTypeList();
    BindCompanyBranchList();
   
});




function BindCompanyBranchList() {
    $("#ddlCompanyBranch").val(0);
    $("#ddlCompanyBranch").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");

            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
                $("#ddlCompanyBranch").attr('disabled', true);
            }
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}

$(".alpha-only").on("input", function () {
    var regexp = /[^a-zA-Z]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".alpha-space-only").on("input", function () {
    var regexp = /[^a-zA-Z\s]+$/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".numeric-only").on("input", function () {
    var regexp = /\D/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".alpha-numeric-only").on("input", function () {
    var regexp = /[^a-zA-Z0-9]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});

function ClearFields() {
    //$("#txtLeadCode").val("");
    //$("#txtCompanyName").val("");
    //$("#txtContactPersoName").val("");
    //$("#txtEmail").val("");
    
    $("#txtContactNo").val("");
    $("#ddlCompanyBranch").val("0");
    
    //$("#txtAddress").val("");
    //$("#txtCompanyCity").val("");
    //$("#ddlState").val("0");
    //$("#ddlLeadSource").val("0");
    //$("#ddlLeadStatus").val("0");
    //$("#ddlStatus").val("");
    //$("#ddlleadtype").val("1");
    //$("#ddlTeamUser").val("0");
    //$("#divTeamMember").hide();
    //$("#ddlleadtypeName").val("0"); 
    //$("#ddlleadtype").val("0");
    window.location.href = "../Lead/ListLead";
    
}
function HideShowTeamMember()
{
    var leadType = $("#ddlleadtype").val();
    if (leadType=="2")
    {
        $("#divTeamMember").show();
    }
    else
    {
        $("#ddlTeamUser").val("0");
        $("#divTeamMember").hide();
    }
}

function BindLeadTypeList() {
   
    $.ajax({
        type: "GET",
        url: "../Lead/GetAllLeadType",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlleadtypeName").append($("<option></option>").val(0).html("-Select Lead Type Name-"));
            $.each(data, function (i, item) {

                $("#ddlleadtypeName").append($("<option></option>").val(item.LeadTypeId).html(item.LeadTypeName));
            });
           
        },
        error: function (Result) {
            $("#ddlleadtypeName").append($("<option></option>").val(0).html("-Select Lead Type Name-"));
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
function BindStateList() {
    var data = { countryId: 1 };
   $.ajax({
            type: "GET",
            url: "../Lead/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                
            },
            error: function (Result) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
}
 
function BindLeadSourceList() {
    $.ajax({
        type: "GET",
        url: "../Lead/GetLeadSourceList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlLeadSource").append($("<option></option>").val(0).html("-Select Lead Source-"));
            $.each(data, function (i, item) {

                $("#ddlLeadSource").append($("<option></option>").val(item.LeadSourceId).html(item.LeadSourceName));
            });
        },
        error: function (Result) {
            $("#ddlLeadSource").append($("<option></option>").val(0).html("-Lead Source-"));
        }
    });
}


function BindLeadStatusList() {
    $.ajax({
        type: "GET",
        url: "../Lead/GetLeadStatusList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlLeadStatus").append($("<option></option>").val(0).html("-Select Lead Status-"));
            $.each(data, function (i, item) {

                $("#ddlLeadStatus").append($("<option></option>").val(item.LeadStatusId).html(item.LeadStatusName));
            });
        },
        error: function (Result) {
            $("#ddlLeadStatus").append($("<option></option>").val(0).html("-Lead Status-"));
        }
    });

}
 
function SearchLead() { 
    var txtLeadCode = $("#txtLeadCode");
    var txtCompanyName = $("#txtCompanyName");
    var txtContactPersoName = $("#txtContactPersoName");
    var txtEmail = $("#txtEmail");
    var txtContactNo = $("#txtContactNo"); 
    var txtAddress = $("#txtAddress");
    var txtCompanyCity = $("#txtCompanyCity");
    var ddlState = $("#ddlState");
    var ddlLeadSource = $("#ddlLeadSource");  
    var ddlLeadStatus = $("#ddlLeadStatus");
    var ddlStatus = $("#ddlStatus");
    var ddlTeamUser = $("#ddlTeamUser");
    var ddlleadtype = $("#ddlleadtype");
    var ddlleadtypeName = $("#ddlleadtypeName");
    var ddlCompanyBranch=$("#ddlCompanyBranch");

    var requestData = { 
        leadCode: txtLeadCode.val().trim(),
        companyName: txtCompanyName.val().trim(),
        contactPersonName: txtContactPersoName.val().trim(),
        email: txtEmail.val().trim(),
        contactNo: txtContactNo.val().trim(),
        companyAddress: txtAddress.val(),
        companyCity: txtCompanyCity.val().trim(),
        companyStateId: ddlState.val(),
        leadSourceId: ddlLeadSource.val(),
        leadStatusId: ddlLeadStatus.val(),
        leadSourceType: ddlleadtype.val(),
        userId: ddlTeamUser.val(),
        status: ddlStatus.val(),
        LeadTypeId: ddlleadtypeName.val(),
        companyBranch: ddlCompanyBranch.val()
    }; 
    $.ajax({
        url: "../Lead/GetLeadList",
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


function OpenPrintPopup() {
    $("#printModel").modal();
    GenerateReportParameters();
   
}
function ShowHidePrintOption() {
    var reportOption = $("#ddlPrintOption").val();
    if (reportOption == "PDF") {
        $("#btnPdf").show();
        $("#btnExcel").hide();
    }
    else if (reportOption == "Excel") {
        $("#btnExcel").show();
        $("#btnPdf").hide();
    }
}
function GenerateReportParameters() {

    var url = "../Lead/GenerateLeadReport?leadCode=" + $("#txtLeadCode").val() + "&companyName=" + $("#txtCompanyName").val() + "&contactPersonName=" + $("#txtContactPersoName").val() + "&email=" + $("#txtEmail").val() + "&contactNo=" + $("#txtContactNo").val() + "&companyCity=" + $("#txtAddress").val() + "&companyStateId=" + $("#ddlState").val() + "&leadSourceId=" + $("#ddlLeadSource").val() + "&leadStatusId=" + $("#ddlLeadStatus").val() + "&leadSourceType=" + $("#ddlleadtype").val() + "&userId=" + $("#ddlTeamUser").val() + "&status=" + $("#ddlStatus").val() + "&leadTypeId=" + $("#ddlleadtypeName").val() + "&companyBranch=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    var url = "../Lead/GenerateLeadReport?leadCode=" + $("#txtLeadCode").val() + "&companyName=" + $("#txtCompanyName").val() + "&contactPersonName=" + $("#txtContactPersoName").val() + "&email=" + $("#txtEmail").val() + "&contactNo=" + $("#txtContactNo").val() + "&companyCity=" + $("#txtAddress").val() + "&companyStateId=" + $("#ddlState").val() + "&leadSourceId=" + $("#ddlLeadSource").val() + "&leadStatusId=" + $("#ddlLeadStatus").val() + "&leadSourceType=" + $("#ddlleadtype").val() + "&userId=" + $("#ddlTeamUser").val() + "&status=" + $("#ddlStatus").val() + "&leadTypeId=" + $("#ddlleadtypeName").val() + "&companyBranch=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

}
