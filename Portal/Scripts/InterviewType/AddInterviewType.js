$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnInterviewTypeID = $("#hdnInterviewTypeID");
    if (hdnInterviewTypeID.val() != "" && hdnInterviewTypeID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetInterviewTypeDetail(hdnInterviewTypeID.val());
        
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true); 
            $("#chkStatus").attr('disabled', true);
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
    $("#txtInterviewTypeName").focus();
        


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


function GetInterviewTypeDetail(interviewtypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../InterviewType/GetInterviewTypeDetail",
        data: { interviewtypeId: interviewtypeId },
        dataType: "json",
        success: function (data) {
            $("#txtInterviewTypeName").val(data.InterviewTypeName); 
            if (data.InterviewType_Status == true) {
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
    var txtInterviewTypeName = $("#txtInterviewTypeName");
    var hdnInterviewTypeID = $("#hdnInterviewTypeID");
  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtInterviewTypeName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Interview Type Name")
        txtInterviewTypeName.focus();
        return false;
    }
  
    
    var interviewtypeViewModel = { 
        InterviewTypeId: hdnInterviewTypeID.val(),
        InterviewTypeName: txtInterviewTypeName.val().trim(),
        InterviewType_Status: chkstatus
        
    };
    var requestData = { interviewtypeViewModel: interviewtypeViewModel };
    var accessMode = 1;//Add Mode
    if (hdnInterviewTypeID.val() != null && hdnInterviewTypeID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    $.ajax({
        url: "../InterviewType/AddEditInterviewType?AccessMode=" + accessMode + "",
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
                      window.location.href = "../InterviewType/ListInterviewType";
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
    $("#txtInterviewTypeName").val(""); 
    $("#chkstatus").prop("checked", true);
    
}
