$(document).ready(function () {
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnScheduleId = $("#hdnScheduleId");
    if (hdnScheduleId.val() != "" && hdnScheduleId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetScheduleDetail(hdnScheduleId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $('#chkstatus').attr('disabled', true);
        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }
    $("#txtScheduleName").focus();
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
 

function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function GetScheduleDetail(scheduleId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Schedule/GetScheduleDetail",
        data: { scheduleId: scheduleId },
        dataType: "json",
        success: function (data) {
            $("#txtScheduleName").val(data.ScheduleName);
            $("#txtScheduleNo").val(data.ScheduleNo);
            if (data.Schedule_Status == true) {
                $("#chkstatus").attr("checked", true);
            }
            else {
                $("#chkstatus").attr("checked", false);
            }
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function SaveData() {
    var txtScheduleName = $("#txtScheduleName");
    var hdnScheduleId = $("#hdnScheduleId");
    var txtScheduleNo = $("#txtScheduleNo");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false; 
    if (txtScheduleName.val().trim() == "") {
        ShowModel("Alert", "Please enter Schedule Name")
        txtScheduleName.focus();
        return false;
    }
    if (txtScheduleNo.val() == "") {
        ShowModel("Alert", "Please enter Schedule No")
        txtScheduleNo.focus();
        return false;
    }
    var ScheduleViewModel = { 
        ScheduleId: hdnScheduleId.val(),
        ScheduleNo: txtScheduleNo.val(),
        ScheduleName: txtScheduleName.val().trim(), 
        Schedule_Status: chkstatus
    };
    var requestData = { ScheduleViewModel: ScheduleViewModel };
    var accessMode = 1;//Add Mode
    if (hdnScheduleId.val() != null && hdnScheduleId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    $.ajax({
        url: "../Schedule/AddEditSchedule?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                if (accessMode == 2)
                {
                    window.location.href = "../Schedule/ListSchedule";
                }
                $("#btnSave").show();
                $("#btnUpdate").hide();
            }
            else {
                ShowModel("Error", data.message)
            }
        },
        error: function (err) {
            ShowModel("Error", err)
        }
    });
}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);
}
function ClearFields() {
    $("#hdnScheduleId").val("0");
    $("#txtScheduleName").val("");
    $("#txtScheduleNo").val("");
   // $("#ddlGLType").val("0");
    $("#chkstatus").prop("checked", true);

}
