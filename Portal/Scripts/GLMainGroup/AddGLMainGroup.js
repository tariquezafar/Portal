$(document).ready(function () {
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnGLMainGroupId = $("#hdnGLMainGroupId");
    if (hdnGLMainGroupId.val() != "" && hdnGLMainGroupId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetGLMainGroupDetail(hdnGLMainGroupId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
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
    $("#txtGLMainGroupName").focus();
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


function GetGLMainGroupDetail(glmaingroupId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../GLMainGroup/GetGLMainGroupDetail",
        data: { glmaingroupId: glmaingroupId },
        dataType: "json",
        success: function (data) {
            $("#txtGLMainGroupName").val(data.GLMainGroupName);
            $("#ddlGLType").val(data.GLType);
            $("#txtSequenceNo").val(data.SequenceNo);
            if (data.GLMainGroup_Status == true) {
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
    var txtGLMainGroupName = $("#txtGLMainGroupName");
    var ddlGLType = $("#ddlGLType");
    var txtSequenceNo = $("#txtSequenceNo");
    var hdnGLMainGroupId = $("#hdnGLMainGroupId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtGLMainGroupName.val().trim() == "") {
        ShowModel("Alert", "Please enter GL Main Group Name")
        txtGLMainGroupName.focus();
        return false;
    }
    if (ddlGLType.val().trim() == "0") {
        ShowModel("Alert", "Please Select GL Type")
        ddlGLType.focus();
        return false;
    }

    var glmaingroupViewModel = {
        GLMainGroupId: hdnGLMainGroupId.val(),
        GLMainGroupName: txtGLMainGroupName.val().trim(),
        GLType: ddlGLType.val().trim(),
        SequenceNo: txtSequenceNo.val().trim(),
        GLMainGroup_Status: chkstatus
    };
    var accessMode = 1;//Add Mode
    if (hdnGLMainGroupId.val() != null && hdnGLMainGroupId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { glmaingroupViewModel: glmaingroupViewModel };
    $.ajax({
        url: "../GLMainGroup/AddEditGLMainGroup?accessMode=" + accessMode + "",
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
                           window.location.href = "../GLMainGroup/ListGLMainGroup";
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
    $("#hdnGLMainGroupId").val("0");
    $("#txtGLMainGroupName").val("");
    $("#txtSequenceNo").val("");
    $("#ddlGLType").val("0");
    $("#chkstatus").prop("checked", true);

}
