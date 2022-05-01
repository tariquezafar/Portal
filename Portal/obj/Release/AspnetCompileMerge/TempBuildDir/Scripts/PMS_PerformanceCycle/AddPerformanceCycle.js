$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnPerformanceCycleID = $("#hdnPerformanceCycleID");
    if (hdnPerformanceCycleID.val() != "" && hdnPerformanceCycleID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetPerformanceCycleDetail(hdnPerformanceCycleID.val());
        
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


function GetPerformanceCycleDetail(performancecycleId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../PMS_PerformanceCycle/GetPerformanceCycleDetail",
        data: { performancecycleId: performancecycleId },
        dataType: "json",
        success: function (data) {
            $("#txtPerformanceCycleName").val(data.PerformanceCycleName);
            if (data.PerformanceCycle_Status == true) {
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
    var txtPerformanceCycleName = $("#txtPerformanceCycleName");
    var hdnPerformanceCycleID = $("#hdnPerformanceCycleID");
  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtPerformanceCycleName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Performance Cycle Name")
        txtPerformanceCycleName.focus();
        return false;
    } 
    var pmsperformancecycleNameViewModel = {
        PerformanceCycleId: hdnPerformanceCycleID.val(),
        PerformanceCycleName: txtPerformanceCycleName.val().trim(),
        PerformanceCycle_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnPerformanceCycleID.val() != null && hdnPerformanceCycleID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { pmsperformancecycleViewModel: pmsperformancecycleNameViewModel };
    $.ajax({
        url: "../PMS_PerformanceCycle/AddEditPerformanceCycle?accessMode=" + accessMode + "",
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
                      window.location.href = "../PMS_PerformanceCycle/ListPerformanceCycle";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PMS_PerformanceCycle/AddEditPerformanceCycle";
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
    $("#txtPerformanceCycleName").val("");
    $("#chkstatus").prop("checked", true);
    
}
