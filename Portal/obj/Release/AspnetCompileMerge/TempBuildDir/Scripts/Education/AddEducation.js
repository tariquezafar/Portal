
$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnEducationId = $("#hdnEducationId");
    if (hdnEducationId.val() != "" && hdnEducationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetEducationDetail(hdnEducationId.val());
        
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
    $("#txtEducationName").focus();
        


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


function GetEducationDetail(educationId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../Education/GetEducationDetail",
        data: { educationId: educationId },
        dataType: "json",
        success: function (data) {
            $("#txtEducationName").val(data.EducationName);
            if (data.Education_Status == true) {
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
    var txtEducationName = $("#txtEducationName");
    var hdnEducationId = $("#hdnEducationId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtEducationName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Education Name")
        txtEducationName.focus();
        return false;
    } 
    var eductaionViewModel = {
        EducationId: hdnEducationId.val(),
        EducationName: txtEducationName.val().trim(),
        Education_Status: chkstatus 
    };
    var accessMode = 1;//Add Mode
    if (hdnEducationId.val() != null && hdnEducationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { eductaionViewModel: eductaionViewModel };
    $.ajax({
        url: "../Education/AddEditEducation?AccessMode=" + accessMode + "",
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
                      window.location.href = "../Education/ListEducationType";
                  }, 2000);

                }
                else
                {
                    setTimeout(
                 function () {
                     window.location.href = "../Education/AddEditEducation";
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
 
    $("#txtEducationName").val("");   
    $("#chkstatus").prop("checked", true);
    
}
