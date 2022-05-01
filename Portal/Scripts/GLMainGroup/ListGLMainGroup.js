$(document).ready(function () {
    //$('#tblCompanyList').paging({ limit: 2 });
   // SearchGLMainGroup();
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
    //$("#txtGLMainGroupName").val("");
    //$("#txtSequenceNo").val("");
    //$("#ddlGLType").val("0");
    //$("#ddlStatus").val("");
    window.location.href = "../GLMainGroup/ListGLMainGroup";

}

function SearchGLMainGroup() {
    var txtGLMainGroupName = $("#txtGLMainGroupName");
    var txtSequenceNo = $("#txtSequenceNo");
    var ddlGLType = $("#ddlGLType");
    var hdnGLMainGroupId = $("#hdnGLMainGroupId");
    var ddlStatus = $("#ddlStatus");
    var requestData = {
        GLMainGroupName: txtGLMainGroupName.val().trim(),
        SequenceNo: txtSequenceNo.val().trim(),
        GLType: ddlGLType.val(),
        Status: ddlStatus.val()
    };

    $.ajax({
        url: "../GLMainGroup/GetGLMainGroupList",
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
function Export() {
    var divList = $("#divList");
    ExporttoExcel(divList);

}