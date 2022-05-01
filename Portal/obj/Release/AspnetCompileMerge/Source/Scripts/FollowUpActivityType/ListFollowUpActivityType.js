$(document).ready(function () {
    //$('#tblCompanyList').paging({ limit: 2 });
    SearchFollowUpActivityType();
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
    $("#txtFollowUpActivityTypeName").val("");
    $("#ddlStatus").val("");
}

function SearchFollowUpActivityType() {
    var txtFollowUpActivityTypeName = $("#txtFollowUpActivityTypeName");
    var ddlStatus = $("#ddlStatus");
    var requestData = {
        FollowUpActivityTypeName: txtFollowUpActivityTypeName.val().trim(),
        Status: ddlStatus.val()
    };
    $.ajax({
        url: "../FollowUpActivityType/GetFollowUpActivityTypeList",
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
