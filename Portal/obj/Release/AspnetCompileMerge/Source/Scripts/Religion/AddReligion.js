
$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnReligionId = $("#hdnReligionId");
    if (hdnReligionId.val() != "" && hdnReligionId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetReligionDetail(hdnReligionId.val());
        
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
    $("#txtReligionName").focus();
        


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


function GetReligionDetail(religionId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../Religion/GetReligionDetail",
        data: { religionId: religionId },
        dataType: "json",
        success: function (data) {
            $("#txtReligionName").val(data.ReligionName);
            if (data.Religion_Status == true) {
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
    var txtReligionName = $("#txtReligionName");
    var hdnReligionId = $("#hdnReligionId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtReligionName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Religion Name")
        txtReligionName.focus();
        return false;
    }
   
    
    var religionViewModel = {
        ReligionId: hdnReligionId.val(),
        ReligionName: txtReligionName.val().trim(),
        Religion_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnReligionId.val() != null && hdnReligionId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { religionViewModel: religionViewModel };
    $.ajax({
        url: "../Religion/AddEditReligion?accessMode=" + accessMode + "",
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
                      window.location.href = "../Religion/ListReligion";
                  }, 2000);
                }
                else
                {
                    setTimeout(
                  function () {
                      window.location.href = "../Religion/AddEditReligion?accessMode=1";
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
 
    $("#txtReligionName").val("");
    $("#chkstatus").prop("checked", true);
    
}
