$(document).ready(function () {

    BindCompanyBranchList();
   // SearchTargetType();
});

function ClearFields()
{
    //$("#txtTargetName").val("");
    //$("#txtTargetDescription").val("");
    //$("#ddlStatus").val("");
    //$("#ddlCompanyBranch").val(0);
    window.location.href = "../TargetType/ListTargetType";

}

function SearchTargetType() {

    var txtTargetName = $("#txtTargetName");
    var txtTargetDescription = $("#txtTargetDescription");
    var ddlStatus = $("#ddlStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestDataSearch = {
        targettypeName: txtTargetName.val().trim(),
        targettypeDesc: txtTargetDescription.val().trim(),
        Status: ddlStatus.val(),
        companyBranchId: ddlCompanyBranch.val()
    };

    $.ajax({
        url: "../TargetType/GetTargetTypeList",
        data: requestDataSearch,
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