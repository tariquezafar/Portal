$(document).ready(function () {
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnPaymentTermId = $("#hdnPaymentTermId");
    if (hdnPaymentTermId.val() != "" && hdnPaymentTermId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetPaymentTermDetail(hdnPaymentTermId.val());
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
    $("#txtPaymentTermDesc").focus();



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


function GetPaymentTermDetail(paymenttermId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PaymentTerm/GetPaymentTermDetail",
        data: { paymenttermId: paymenttermId },
        dataType: "json",
        success: function (data) {
            $("#txtPaymentTermDesc").val(data.PaymentTermDesc);
            if (data.PaymentTerm_Status == true) {
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
    var txtPaymentTermDesc = $("#txtPaymentTermDesc");
    var hdnPaymentTermId = $("#hdnPaymentTermId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtPaymentTermDesc.val().trim() == "") {
        ShowModel("Alert", "Please enter Payment Term Desc")
        txtPaymentTermDesc.focus();
        return false;
    }

    var paymenttermViewModel = {
        PaymentTermId: hdnPaymentTermId.val(),
        PaymentTermDesc: txtPaymentTermDesc.val().trim(),
        PaymentTerm_Status: chkstatus
    };
    var accessMode = 1;//Add Mode
    if (hdnPaymentTermId.val() != null && hdnPaymentTermId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { paymenttermViewModel: paymenttermViewModel };
    $.ajax({
        url: "../PaymentTerm/AddEditPaymentTerm?accessMode=" + accessMode + "",
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
                      window.location.href = "../PaymentTerm/ListPaymentTerm";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PaymentTerm/AddEditPaymentTerm";
                    }, 2000);
                }
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
    $("#hdnPaymentTermId").val("0");
    $("#txtPaymentTermDesc").val("");
    $("#chkstatus").prop("checked", true);

}
function Export() {
    var divList = $("#divList");
    ExporttoExcel(divList);

}