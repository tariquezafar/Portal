$(document).ready(function () {
    //$('#tblCompanyList').paging({ limit: 2 });
    //SearchProductMainGroup();
});

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
    //$("#txtProductMainGroupName").val("");
    //$("#txtProductMainGroupCode").val("");
    //$("#ddlStatus").val("");
    window.location.href = "../ProductMainGroup/ListProductMainGroup";
}

function SearchProductMainGroup() {
    var txtProductMainGroupName = $("#txtProductMainGroupName");
    var txtProductMainGroupCode = $("#txtProductMainGroupCode");
    var hdnProductMainGroupId = $("#hdnProductMainGroupId");
    var ddlStatus = $("#ddlStatus");
    var requestData = {
        ProductMainGroupName: txtProductMainGroupName.val().trim(),
        ProductMainGroupCode: txtProductMainGroupCode.val().trim(),
        Status: ddlStatus.val()
    };

    $.ajax({
        url: "../ProductMainGroup/GetProductMainGroupList",
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
