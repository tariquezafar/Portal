$(document).ready(function () {
    //$('#tblCompanyList').paging({ limit: 2 });
   // SearchGLSubGroup();
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
    //$("#txtGLSubGroupName").val("");
    //$("#txtSequenceNo").val("");
    //$("#ddlGLMainGroupId").val("0");
    //$("#ddlScheduleName").val("0");
    //$("#ddlStatus").val("");
    window.location.href = "../GLSubGroup/ListGLSubGroup";

}


BindGLMainGroupList();
BindGLScheduleList();
function BindGLMainGroupList() {
    $.ajax({
        type: "GET",
        url: "../GLSubGroup/GetGLMainGroupList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlGLMainGroupId").append($("<option></option>").val(0).html("-Select GL Main Group-"));
            $.each(data, function (i, item) {

                $("#ddlGLMainGroupId").append($("<option></option>").val(item.GLMainGroupId).html(item.GLMainGroupName));
            });
        },
        error: function (Result) {
            $("#ddlGLMainGroupId").append($("<option></option>").val(0).html("-Select GL Main Group-"));
        }
    });
}
function BindGLScheduleList() {
    $.ajax({
        type: "GET",
        url: "../GLSubGroup/GetGLScheduleList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlScheduleName").append($("<option></option>").val(0).html("-Select Schedule-"));
            $.each(data, function (i, item) {

                $("#ddlScheduleName").append($("<option></option>").val(item.ScheduleId).html(item.ScheduleName));
            });
        },
        error: function (Result) {
            $("#ddlScheduleName").append($("<option></option>").val(0).html("-Select Schedule-"));
        }
    });
}


function SearchGLSubGroup() {
    var txtGLSubGroupName = $("#txtGLSubGroupName");
    var txtSequenceNo = $("#txtSequenceNo");
    var ddlGLMainGroupId = $("#ddlGLMainGroupId");
    var ddlScheduleName = $("#ddlScheduleName");
    var hdnGLSubGroupId = $("#hdnGLSubGroupId");
    var ddlStatus = $("#ddlStatus");
    var requestData = {
        GLSubGroupName: txtGLSubGroupName.val().trim(),
        SequenceNo: txtSequenceNo.val().trim(),
        GLMainGroupId: ddlGLMainGroupId.val(),
        ScheduleId: ddlScheduleName.val(),
        Status: ddlStatus.val()
    };

    $.ajax({
        url: "../GLSubGroup/GetGLSubGroupList",
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