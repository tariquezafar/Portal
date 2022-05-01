$(document).ready(function () {
    debugger;
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnPaymentModeId = $("#hdnPaymentModeId");
    if (hdnPaymentModeId.val() != "" && hdnPaymentModeId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetPaymentModeDetail(hdnPaymentModeId.val());
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
    $("#txtPaymentModeName").focus();



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


function GetPaymentModeDetail(paymentModeId) {
    debugger;
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PaymentMode/GetPaymentModeDetail",
        data: { paymentModeId: paymentModeId },
        dataType: "json",
        success: function (data) {
            $("#txtPaymentModeName").val(data.PaymentModeName);
            if (data.PaymentMode_Status == true) {
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
    var txtPaymentModeName = $("#txtPaymentModeName");
    var hdnPaymentModeId = $("#hdnPaymentModeId");
    var chkStatus = $("#chkStatus").is(':checked') ? true : false;

    if (txtPaymentModeName.val().trim() == "") {
        ShowModel("Alert", "Please enter Payment Mode Name")
        txtPaymentModeName.focus();
        return false;
    }
   
    var paymentModeViewModel = {
        PaymentModeId: hdnPaymentModeId.val(),
        PaymentModeName: txtPaymentModeName.val().trim(),
        PaymentMode_Status: chkStatus
    };
    var requestData = { paymentModeViewModel: paymentModeViewModel };
    $.ajax({
        url: "../PaymentMode/AddEditPaymentMode",
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
    $("#hdnPaymentModeId").val("0");
    $("#txtPaymentModeName").val("");
    $("#chkStatus").prop("checked", true);

}
