$(document).ready(function () {

    $("#txtExpiryDate").attr('readOnly', true);
   // GetThoughtDetails();

    $("#txtExpiryDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        minDate: 0,
        onSelect: function (selected) {

        }
    });

    var hdnThoughtId = $("#hdnThoughtId");
    var hdnAccessMode = $("#hdnAccessMode");

    if (hdnThoughtId.val() != "" && hdnThoughtId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
       {
           GetThoughtDetails(hdnThoughtId.val());
       }

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readonly', true);
            $("textarea").attr('readonly', true);
            $("#chkstatus").attr('disabled', true);
            $("#txtExpiryDate").prop('disabled', true);
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

function GetThoughtDetails(thoughtId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Thought/GetThoughtDetail",
        data: { thoughtId: thoughtId },
        dataType: "json",
        success: function (data) {
            $("#txtThoughtMsg").val(data.ThoughtMessage);
            $("#txtThoughtType").val(data.ThoughtType);
            $("#txtExpiryDate").val(data.ExpiryDate);
            if (data.Thought_Status == true) {
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
    var txtThoughtMsg = $("#txtThoughtMsg");
    var hdnThoughtId = $("#hdnThoughtId");
    var txtThoughtType = $("#txtThoughtType");
    var txtExpiryDate = $("#txtExpiryDate");

    var chkstatus = $("#chkstatus").is(':checked') ? true : false;;

    if (txtThoughtMsg.val() == "") {
        ShowModel("Alert", "Please Enter Thought Message")
        return false;

    }
    if (txtExpiryDate.val() == "") {
        ShowModel("Alert", "Please Select Expiry Date.")
        return false;

    }

    if (txtThoughtType.val() == "") {
        ShowModel("Alert", "Please Enter Thought Type.")
        return false;

    }

    var accessMode = 1;//Add Mode
    if (hdnThoughtId.val() != null && hdnThoughtId.val() != 0) {
        accessMode = 2;//Edit Mode
    }


    var thoughtViewModel = {
        ThoughtMessage: txtThoughtMsg.val(),
        ThoughtId: hdnThoughtId.val(),
        ThoughtType: txtThoughtType.val(),
        ExpiryDate: txtExpiryDate.val(),
        Thought_Status: chkstatus
    };
    var requestData = { thoughtViewModel: thoughtViewModel };
    $.ajax({
        url: "../Thought/AddEditThought?accessMode=" + accessMode + "",
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
                       window.location.href = "../Thought/ListThought";
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
    $("#txtThoughtMsg").val("");
    $("#txtThoughtType").val("");
    $("#txtExpiryDate").val("");
    $("#btnSave").show();
    $("#btnUpdate").hide();
}




