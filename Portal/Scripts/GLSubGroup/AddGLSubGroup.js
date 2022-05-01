$(document).ready(function () {
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnGLSubGroupId = $("#hdnGLSubGroupId");
    if (hdnGLSubGroupId.val() != "" && hdnGLSubGroupId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
        function () {
            GetGLSubGroupDetail(hdnGLSubGroupId.val());
        }, 1000);
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
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
    $("#txtGLSubGroupName").focus();
});
//$(".alpha-only").on("keydown", function (event) {
//    // Allow controls such as backspace
//    var arr = [8, 16, 17, 20, 35, 36, 37, 38, 39, 40, 45, 46];

//    // Allow letters
//    for (var i = 65; i <= 90; i++) {
//        arr.push(i);
//    }

//    // Prevent default if not in array
//    if (jQuery.inArray(event.which, arr) === -1) {
//        event.preventDefault();
//    }
//});

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

function  BindGLScheduleList() {
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


function GetGLSubGroupDetail(glsubgroupId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../GLSubGroup/GetGLSubGroupDetail",
        data: { glsubgroupId: glsubgroupId },
        dataType: "json",
        success: function (data) {
            $("#txtGLSubGroupName").val(data.GLSubGroupName);
            $("#ddlGLMainGroupId").val(data.GLMainGroupId);
            $("#ddlScheduleName").val(data.ScheduleId);
            $("#txtSequenceNo").val(data.SequenceNo);
            if (data.GLSubGroup_Status == true) {
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
    var txtGLSubGroupName = $("#txtGLSubGroupName");
    var ddlGLMainGroupId = $("#ddlGLMainGroupId");
    var txtSequenceNo = $("#txtSequenceNo");
    var ddlScheduleName = $("#ddlScheduleName"); 
    var hdnGLSubGroupId = $("#hdnGLSubGroupId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtGLSubGroupName.val().trim() == "") {
        ShowModel("Alert", "Please enter GL Sub Group")
        txtGLSubGroupName.focus();
        return false;
    }
    if (ddlGLMainGroupId.val().trim() == "0") {
        ShowModel("Alert", "Please Select GL Main Group")
        ddlGLMainGroupId.focus();
        return false;
    }
    if (ddlScheduleName.val().trim() == "0") {
        ShowModel("Alert", "Please Select Schedule")
        ddlScheduleName.focus();
        return false;
    }

    var glsubgroupViewModel = {
        GLSubGroupId: hdnGLSubGroupId.val(),
        GLSubGroupName: txtGLSubGroupName.val().trim(),
        GLMainGroupId: ddlGLMainGroupId.val().trim(),
        ScheduleId: ddlScheduleName.val().trim(),
        SequenceNo: txtSequenceNo.val().trim(),
        GLSubGroup_Status: chkstatus
    };
    var accessMode = 1;//Add Mode
    if (hdnGLSubGroupId.val() != null && hdnGLSubGroupId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { glsubgroupViewModel: glsubgroupViewModel };
    $.ajax({
        url: "../GLSubGroup/AddEditGLSubGroup?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();

                      
                
                
                if (accessMode == 2) {
                    setTimeout(
               function () {
                   window.location.href = "../GLSubGroup/ListGLSubGroup";
               }, 1000);
                  
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
    $("#hdnGLSubGroupId").val("0");
    $("#txtGLSubGroupName").val("");
    $("#txtSequenceNo").val("");
    $("#ddlScheduleName").val("0");
    $("#ddlGLMainGroupId").val("0");
    $("#chkstatus").prop("checked", true); 
}
