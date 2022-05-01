$(document).ready(function () {
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnFormTypeId = $("#hdnFormTypeId");
    if (hdnFormTypeId.val() != "" && hdnFormTypeId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetFormTypeDetail(hdnFormTypeId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").prop('disabled', true);
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
    $("#txtFormTypeDesc").focus();
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
function GetFormTypeDetail(formTypeId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../FormType/GetFormTypeDetail",
        data: { formTypeId: formTypeId },
        dataType: "json",
        success: function (data) {
            $("#txtFormTypeDesc").val(data.FormTypeDesc);
            if (data.FormType_Status == true) {
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
    var txtFormTypeDesc = $("#txtFormTypeDesc");
    var hdnFormTypeId = $("#hdnFormTypeId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtFormTypeDesc.val().trim() == "") {
        ShowModel("Alert", "Please enter Form Type Desc")
        txtFormTypeDesc.focus();
        return false;
    }

    
    var accessMode = 1;//Add Mode
    if (hdnFormTypeId.val() != null && hdnFormTypeId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var FormTypeViewModel = {
        
        FormTypeId: hdnFormTypeId.val(),
        FormTypeDesc: txtFormTypeDesc.val().trim(),       
        FormType_Status: chkstatus
    };
    var requestData = { FormTypeViewModel: FormTypeViewModel };
    $.ajax({
        url: "../FormType/AddEditFormType?accessMode=" + accessMode + "",
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
                   window.location.href = "../FormType/ListFormType";
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
    $("#hdnFormTypeId").val("0");
    $("#txtFormTypeDesc").val("");
    $("#chkstatus").prop("checked", true);

}
function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
}
document.onkeypress = stopRKey;