$(document).ready(function () {
    //$('#tblCompanyList').paging({ limit: 2 });
    // SearchLeadSource();
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
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
    //$("#txtLeadSourceName").val("");
    //$("#ddlStatus").val("");
    //$("#ddlCompanyBranch").val("");
    window.location.href = "../LeadSource/ListLeadSource";
}

function SearchLeadSource() {
    var txtLeadSourceName = $("#txtLeadSourceName");
    var ddlStatus = $("#ddlStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var requestData = {
        LeadSourceName: txtLeadSourceName.val().trim(),
        Status: ddlStatus.val(),
        CompanyBranch: ddlCompanyBranch.val()
    };

    $.ajax({
        url: "../LeadSource/GetLeadSourceList",
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
