$(document).ready(function () {
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnAdvanceTypeID = $("#hdnAdvanceTypeID");
    if (hdnAdvanceTypeID.val() != "" && hdnAdvanceTypeID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetAdvanceTypeDetail(hdnAdvanceTypeID.val());
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
    $("#hdnAdvanceTypeID").focus();
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


function GetAdvanceTypeDetail(advancetypeId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../AdvanceType/GetAdvanceTypeDetail",
        data: { advancetypeId: advancetypeId },
        dataType: "json",
        success: function (data) {
            $("#txtAdvanceTypeName").val(data.AdvanceTypeName);
            if (data.AdvanceType_Status == true) {
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
    var txtAdvanceTypeName = $("#txtAdvanceTypeName");
    var hdnAdvanceTypeID = $("#hdnAdvanceTypeID");

    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtAdvanceTypeName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Advance Type Name")
        txtAdvanceTypeName.focus();
        return false;
    }

    var advancetypeViewModel = {
        AdvanceTypeId: hdnAdvanceTypeID.val(),
        AdvanceTypeName: txtAdvanceTypeName.val().trim(),
        AdvanceType_Status: chkstatus
    };
    var accessMode = 1;//Add Mode
    if (hdnAdvanceTypeID.val() != null && hdnAdvanceTypeID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { advancetypeViewModel: advancetypeViewModel };
    $.ajax({
        url: "../AdvanceType/AddEditAdvanceType?accessMode=" + accessMode + "",
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
                      window.location.href = "../AdvanceType/ListAdvanceType";
                  }, 2000);
                }
                else {
             setTimeout(
                   function () {
                       window.location.href = "../AdvanceType/AddEditAdvanceType?accessMode=1";
                   }, 2000);
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
    $("#txtAdvanceTypeName").val("");
    $("#chkstatus").prop("checked", true);

}
