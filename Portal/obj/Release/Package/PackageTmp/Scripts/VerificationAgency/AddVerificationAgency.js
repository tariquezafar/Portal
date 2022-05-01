$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnVerificationAgencyID = $("#hdnVerificationAgencyID");
    if (hdnVerificationAgencyID.val() != "" && hdnVerificationAgencyID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetVerificationAgencyDetail(hdnVerificationAgencyID.val());
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true); 
            $("#chkstatus").attr('disabled', true);
        }
        else
        {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else
    {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }
    $("#txtVerificationAgencyName").focus();
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


function GetVerificationAgencyDetail(verificationagencyId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../VerificationAgency/GetVerificationAgencyDetail",
        data: { verificationagencyId: verificationagencyId },
        dataType: "json",
        success: function (data) {
            $("#txtVerificationAgencyName").val(data.VerificationAgencyName);
            if (data.VerificationAgency_Status == true) {
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

function SaveData()
{
    var txtVerificationAgencyName = $("#txtVerificationAgencyName");
    var hdnVerificationAgencyID = $("#hdnVerificationAgencyID");
  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtVerificationAgencyName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Verification Agency Name")
        txtVerificationAgencyName.focus();
        return false;
    } 
    
    var verificationagencyViewModel = {
        VerificationAgencyId: hdnVerificationAgencyID.val(),
        VerificationAgencyName: txtVerificationAgencyName.val().trim(),
        VerificationAgency_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnVerificationAgencyID.val() != null && hdnVerificationAgencyID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { verificationagencyViewModel: verificationagencyViewModel };
    $.ajax({
        url: "../VerificationAgency/AddEditVerificationAgency?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify( requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status=="SUCCESS")
            {
                ShowModel("Alert", data.message);
                ClearFields();
                $("#btnSave").show();
                $("#btnUpdate").hide();
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../VerificationAgency/ListVerificationAgency";
                  }, 2000);

                }
                else {
                    setTimeout(
                   function () {
                       window.location.href = "../VerificationAgency/AddEditVerificationAgency?accessMode=1";
                   }, 2000);
                }
            }
            else
            {
                ShowModel("Error", data.message)
            }
        },
        error: function (err) {
            ShowModel("Error", err)
        }
    });

}
function ShowModel(headerText,bodyText)
{
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields()
{ 
    $("#txtVerificationAgencyName").val("");
    $("#chkstatus").prop("checked", true);
    
}
