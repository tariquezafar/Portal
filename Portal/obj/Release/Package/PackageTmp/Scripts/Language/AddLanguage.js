
$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnlanguageId = $("#hdnlanguageId");
    if (hdnlanguageId.val() != "" && hdnlanguageId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetLanguageDetail(hdnlanguageId.val());
        
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


function GetLanguageDetail(languageId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../Language/GetLanguageDetail",
        data: { languageId: languageId },
        dataType: "json",
        success: function (data) {
            $("#txtLanguageName").val(data.LanguageName);
            if (data.Language_Status == true) {
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
    var txtLanguageName = $("#txtLanguageName");
    var hdnlanguageId = $("#hdnlanguageId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtLanguageName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Language Name")
        txtLanguageName.focus();
        return false;
    }
   
    var languageViewModel = {
        LanguageId: hdnlanguageId.val(),
        LanguageName: txtLanguageName.val().trim(),
        Language_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnlanguageId.val() != null && hdnlanguageId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { languageViewModel: languageViewModel };
    $.ajax({
        url: "../Language/AddEditLanguage?accessMode=" + accessMode + "",
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
                      window.location.href = "../Language/ListLanguage";
                  }, 2000);

                }
                else {
                    setTimeout(
                function () {
                    window.location.href = "../Language/AddEditLanguage?accessMode=1";
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
 
    $("#txtLanguageName").val("");
    $("#chkstatus").prop("checked", true);
    
}
