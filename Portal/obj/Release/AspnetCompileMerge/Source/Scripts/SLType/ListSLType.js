$(document).ready(function () {
    //$('#tblCompanyList').paging({ limit: 2 });
    SearchSLType();
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
    $("#txtSLTypeName").val("");
    $("#ddlStatus").val("");
}

function SearchSLType() {
    var txtSLTypeName = $("#txtSLTypeName");
    var ddlStatus = $("#ddlStatus");
    var requestData = {
        SLTypeName: txtSLTypeName.val().trim(),
        Status: ddlStatus.val()
    }; 
    $.ajax({
        url: "../SLType/GetSLTypeList",
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
