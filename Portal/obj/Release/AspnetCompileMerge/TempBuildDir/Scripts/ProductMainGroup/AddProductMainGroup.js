$(document).ready(function () {
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnProductMainGroupId = $("#hdnProductMainGroupId");
    if (hdnProductMainGroupId.val() != "" && hdnProductMainGroupId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetProductMainGroupDetail(hdnProductMainGroupId.val());
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
    $("#txtProductMainGroupName").focus();
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


function GetProductMainGroupDetail(productmaingroupId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ProductMainGroup/GetProductMainGroupDetail",
        data: { productmaingroupId: productmaingroupId },
        dataType: "json",
        success: function (data) {
            $("#txtProductMainGroupName").val(data.ProductMainGroupName);
            $("#txtProductMainGroupCode").val(data.ProductMainGroupCode);
            if (data.ProductMainGroup_Status == true) {
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
    var txtProductMainGroupName = $("#txtProductMainGroupName");
    var txtProductMainGroupCode = $("#txtProductMainGroupCode");
    var hdnProductMainGroupId = $("#hdnProductMainGroupId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtProductMainGroupName.val().trim() == "") {
        ShowModel("Alert", "Please enter Product Main Group Name")
        txtProductMainGroupName.focus();
        return false;
    }
    if (txtProductMainGroupCode.val().trim() == "") {
        ShowModel("Alert", "Please enter Product Main Group Code")
        txtProductMainGroupCode.focus();
        return false;
    }

    var productmaingroupViewModel = {
        ProductMainGroupId: hdnProductMainGroupId.val(),
        productMainGroupName: txtProductMainGroupName.val().trim(),
        productMainGroupCode: txtProductMainGroupCode.val().trim(),
        ProductMainGroup_Status: chkstatus
    };

    var accessMode = 1;//Add Mode
    if (hdnProductMainGroupId.val() != null && hdnProductMainGroupId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { productmaingroupViewModel: productmaingroupViewModel };
    $.ajax({
        url: "../ProductMainGroup/AddEditProductMainGroup?AccessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
              function () {
                  window.location.href = "../ProductMainGroup/ListProductMainGroup";
              }, 2000);
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
    $("#hdnProductMainGroupId").val("0");
    $("#txtProductMainGroupName").val("");
    $("#txtProductMainGroupCode").val("");
    $("#chkstatus").prop("checked", true);

}
