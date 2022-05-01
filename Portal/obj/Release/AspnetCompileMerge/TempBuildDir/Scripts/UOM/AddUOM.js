$(document).ready(function () {
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnUOMId = $("#hdnUOMId");
    if (hdnUOMId.val() != "" && hdnUOMId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetUOMDetail(hdnUOMId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true);
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
    $("#txtDepartmentName").focus();



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


function GetUOMDetail(uomId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../UOM/GetUOMDetail",
        data: { uomId: uomId },
        dataType: "json",
        success: function (data) {
            $("#txtUOMName").val(data.UOMName);
            $("#txtUOMDesc").val(data.UOMDesc);
            if (data.UOM_Status == true) {
                $("#chkStatus").attr("checked", true);
            }
            else {
                $("#chkStatus").attr("checked", false);
            }
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function SaveData() {
    var txtUOMName = $("#txtUOMName");
    var txtUOMDesc = $("#txtUOMDesc");
    var hdnUOMId = $("#hdnUOMId");
    var chkStatus = $("#chkStatus").is(':checked') ? true : false;

    if (txtUOMName.val().trim() == "") {
        ShowModel("Alert", "Please enter UOM Name")
        txtUOMName.focus();
        return false;
    }
    if (txtUOMDesc.val().trim() == "") {
        ShowModel("Alert", "Please enter UOM Desc")
        txtUOMDesc.focus();
        return false;
    }
    var uomViewModel = {
        UOMId: hdnUOMId.val(),
        UOMName: txtUOMName.val().trim(),
        UOMDesc: txtUOMDesc.val().trim(),
        UOM_Status: chkStatus
    };
    var requestData = { uomViewModel: uomViewModel };
    $.ajax({
        url: "../UOM/AddEditUOM",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
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
    $("#hdnUOMId").val("0");
    $("#txtUOMName").val("");
    $("#txtUOMDesc").val("");
    $("#chkStatus").prop("checked", true);

}
