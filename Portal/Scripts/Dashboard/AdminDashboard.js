$(document).ready(function () {
    BindCompanyBranchListForComman();
    $("#tabs").tabs({
        collapsible: true
    });

    //GetAdminDashboardUsersList();
   // GetAdminDashboardRolesList();
});

function BindUser() {

    var txtUserName = $("#txtUserName");
    var txtPhoneNo = $("#txtPhoneNo");
    var txtFullName = $("#txtFullName");
    var txtEmail = $("#txtEmail");
    var ddlRole = $("#ddlRole");

    var requestData = { };
    $.ajax({
        url: "../User/GetDashboardUser",
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

//function drawCharts() {

//function GetAdminDashboardUsersList() {

//    var requestData = {};
//    $.ajax({
//        url: "../Dashboard/GetAdminDashboardUsersList",
//        data: requestData,
//        dataType: "html",
//        type: "POST",
//        asnc: false,
//        error: function (err) {
//            $("#DivUsersList").html("");
//            $("#DivUsersList").html(err);

//        },
//        success: function (data) {
//            $("#DivUsersList").html("");
//            $("#DivUsersList").html(data);

//        }
//    });
//}

//function GetAdminDashboardRolesList() {

//    var requestData = {};
//    $.ajax({
//        url: "../Dashboard/GetAdminDashboardRolesList",
//        data: requestData,
//        dataType: "html",
//        type: "POST",
//        asnc: false,
//        error: function (err) {
//            $("#DivRolesList").html("");
//            $("#DivRolesList").html(err);

//        },
//        success: function (data) {
//            $("#DivRolesList").html("");
//            $("#DivRolesList").html(data);
//        }
//    });
//}

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
            $("#ddlCompanyBranchForComman").append($("<option></option>").val(0).html("Select Company Branch"));
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
            $("#ddlCompanyBranchForComman").append($("<option></option>").val(0).html("Select Company Branch"));
        }
    });
}
function commanfunction() {
    GetAdminDashboardUsersList();
    GetAdminDashboardRolesList();
    
}
//company Branch 

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
            $("#ddlCompanyBranchForComman").append($("<option></option>").val(0).html("Select Company Branch"));
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
            $("#ddlCompanyBranchForComman").append($("<option></option>").val(0).html("Select Company Branch"));
        }
    });
}
//
function GetAdminDashboardUsersList() {
    var ddlCompanyBranchForComman = $("#ddlCompanyBranchForComman");
    var requestData = { companyBranchId: ddlCompanyBranchForComman.val() };

    $.ajax({
        url: "../Dashboard/GetAdminDashboardUsersList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivUsersList").html("");
            $("#DivUsersList").html(err);

        },
        success: function (data) {
            $("#DivUsersList").html("");
            $("#DivUsersList").html(data);

        }
    });
}

function GetAdminDashboardRolesList() {

    var ddlCompanyBranchForComman = $("#ddlCompanyBranchForComman");
    var requestData = { companyBranchId: ddlCompanyBranchForComman.val() };
    $.ajax({
        url: "../Dashboard/GetAdminDashboardRolesList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivRolesList").html("");
            $("#DivRolesList").html(err);

        },
        success: function (data) {
            $("#DivRolesList").html("");
            $("#DivRolesList").html(data);

        }
    });
}

function CommanMethodForCount() {
    var ddlCompanyBranchForComman = $("#ddlCompanyBranchForComman");
    var requestData = { companyBranchId: ddlCompanyBranchForComman.val() };

    $.ajax({
        url: "../Dashboard/CommanMethodForCount",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,

    });
}