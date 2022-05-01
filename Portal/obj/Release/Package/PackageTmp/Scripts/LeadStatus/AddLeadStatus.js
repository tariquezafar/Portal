$(document).ready(function () {
    var hdnAccessMode = $("#hdnAccessMode");
    
    var hdnLeadStatusId = $("#hdnLeadStatusId");
    if (hdnLeadStatusId.val() != "" && hdnLeadStatusId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetLeadStatusDetail(hdnLeadStatusId.val());
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
    $("#txtLeadStatusName").focus();



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


function GetLeadStatusDetail(leadstatusId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../LeadStatus/GetLeadStatusDetail",
        data: { leadstatusId: leadstatusId },
        dataType: "json",
        success: function (data) {
            $("#txtLeadStatusName").val(data.LeadStatusName);
            if (data.Lead_Status == true) {
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
    var txtLeadStatusName = $("#txtLeadStatusName");
    var hdnLeadStatusId = $("#hdnLeadStatusId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtLeadStatusName.val().trim() == "") {
        ShowModel("Alert", "Please enter Lead Status Name")
        txtLeadStatusName.focus();
        return false;
    }

    var leadstatusViewModel = {
        LeadStatusId: hdnLeadStatusId.val(),
        LeadStatusName: txtLeadStatusName.val().trim(),
        Lead_Status: chkstatus
    };
    var requestData = { leadstatusViewModel: leadstatusViewModel };
    $.ajax({
        url: "../LeadStatus/AddEditLeadStatus",
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
    $("#hdnLeadStatusId").val("0");
    $("#txtLeadStatusName").val("");
    $("#chkstatus").prop("checked", true);

}
