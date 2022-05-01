$(document).ready(function () {
    //$('#tblCompanyList').paging({ limit: 2 });
    //SearchGLMainGroup();
    //SearchSchedule();
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
    //$("#txtScheduleName").val("");
    //$("#txtScheduleNo").val("");
    //$("#ddlStatus").val("");
    window.location.href = "../Schedule/ListSchedule";



}

function SearchSchedule() {
    var txtScheduleName = $("#txtScheduleName");
    var txtScheduleNo = $("#txtScheduleNo");
    var hdnScheduleId = $("#hdnScheduleId");
    var ddlStatus = $("#ddlStatus");
    var requestData = {
        ScheduleName: txtScheduleName.val().trim(),
        ScheduleNo: txtScheduleNo.val(),
        Status: ddlStatus.val()
    };

    $.ajax({
        url: "../Schedule/GetScheduleList",
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