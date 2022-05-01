$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnResumeSourceID = $("#hdnResumeSourceID");
    if (hdnResumeSourceID.val() != "" && hdnResumeSourceID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetResumeSourceDetail(hdnResumeSourceID.val());
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
    $("#txtResumeSourceName").focus();
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


function GetResumeSourceDetail(resumesourceId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../ResumeSource/GetResumeSourceDetail",
        data: { resumesourceId: resumesourceId },
        dataType: "json",
        success: function (data) {
            $("#txtResumeSourceName").val(data.ResumeSourceName);
            if (data.ResumeSource_Status == true) {
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
    var txtResumeSourceName = $("#txtResumeSourceName");
    var hdnResumeSourceID = $("#hdnResumeSourceID");
  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtResumeSourceName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Resume Source Name")
        txtResumeSourceName.focus();
        return false;
    } 
    
    var resumesourceViewModel = {
        ResumeSourceId: hdnResumeSourceID.val(),
        ResumeSourceName: txtResumeSourceName.val().trim(),
        ResumeSource_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnResumeSourceID.val() != null && hdnResumeSourceID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { resumesourceViewModel: resumesourceViewModel };
    $.ajax({
        url: "../ResumeSource/AddEditResumeSource?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify( requestData),
        contentType: 'application/json',
        success: function (data)
        {
            if (data.status=="SUCCESS")
            {
                ShowModel("Alert", data.message);
                ClearFields();
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../ResumeSource/ListResumeSource";
                  }, 2000);
                }
                else {
                    setTimeout(
                        function () {
                            window.location.href = "../ResumeSource/AddEditResumeSource?accessMode=1";
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
    $("#txtResumeSourceName").val("");
    $("#chkstatus").prop("checked", true);
    
}
