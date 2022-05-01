$(document).ready(function () {
    debugger;
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnServicesId = $("#hdnServicesId");
    if (hdnServicesId.val() != "" && hdnServicesId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetServicesDetail(hdnServicesId.val());
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
    $("#txtServicesName").focus();



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


function GetServicesDetail(servicesId) {
    debugger;
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Services/GetServicesDetail",
        data: { servicesId: servicesId },
        dataType: "json",
        success: function (data) {
            $("#txtServicesName").val(data.ServicesName);
            if (data.Services_Status == true) {
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
    debugger;
    var txtServicesName = $("#txtServicesName");
    var hdnServicesId = $("#hdnServicesId");
    var chkStatus = $("#chkStatus").is(':checked') ? true : false;

    if (txtServicesName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Services Name")
        txtServicesName.focus();
        return false;
    }

    var servicesViewModel = {
        ServicesId: hdnServicesId.val(),
        ServicesName: txtServicesName.val().trim(),
        Services_Status: chkStatus
    };
    var requestData = { servicesViewModel: servicesViewModel };
    $.ajax({
        url: "../Services/AddEditServices",
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
    $("#hdnServicesId").val("0");
    $("#txtServicesName").val("");
    $("#chkStatus").prop("checked", true);

}
