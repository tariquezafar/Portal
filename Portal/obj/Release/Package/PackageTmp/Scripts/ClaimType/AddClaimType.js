
$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnClaimTypeId = $("#hdnClaimTypeId");
    if (hdnClaimTypeId.val() != "" && hdnClaimTypeId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetClaimTypeDetail(hdnClaimTypeId.val());
        
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
    $("#txtClaimTypeName").focus();
        


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


function GetClaimTypeDetail(claimTypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../ClaimType/GetClaimTypeDetail",
        data: { claimTypeId: claimTypeId },
        dataType: "json",
        success: function (data) {
            $("#txtClaimTypeName").val(data.ClaimTypeName);
            $("#txtClaimnature").val(data.ClaimNature);
            if (data.ClaimType_Status == true) {
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
    var txtClaimTypeName = $("#txtClaimTypeName");
    var hdnClaimTypeId = $("#hdnClaimTypeId");
    var txtClaimnature = $("#txtClaimnature");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtClaimTypeName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Claim Type Name")
        txtClaimTypeName.focus();
        return false;
    }
    if (txtClaimnature.val().trim() == "") {
        ShowModel("Alert", "Please Enter Claim Type Code")
        txtClaimnature.focus();
        return false;
    } 
    
    var claimtypeViewModel = {
        ClaimTypeId: hdnClaimTypeId.val(),
        ClaimTypeName: txtClaimTypeName.val().trim(),
        ClaimNature: txtClaimnature.val().trim(),
        ClaimType_Status: chkstatus
    };
   
    var accessMode = 1;//Add Mode
    if (hdnClaimTypeId.val() != null && hdnClaimTypeId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { claimtypeViewModel: claimtypeViewModel };
    $.ajax({
        url: "../ClaimType/AddEditClaimType?accessMode=" + accessMode + "",
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
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../ClaimType/ListClaimType";
                  }, 2000);
                }
                else {
                    setTimeout(
                        function () {
                            window.location.href = "../ClaimType/AddEditClaimType?accessMode=1";
                        }, 2000);
                }
                $("#btnSave").show();
                $("#btnUpdate").hide();
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
 
    $("#txtClaimTypeName").val("");
    $("#txtClaimnature").val("");
    $("#chkstatus").prop("checked", true);
    
}
