$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnPositionLevelID = $("#hdnPositionLevelID");
    if (hdnPositionLevelID.val() != "" && hdnPositionLevelID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetPositionLevelDetail(hdnPositionLevelID.val());
        
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
    $("#txtPositionLevelName").focus();
        


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


function GetPositionLevelDetail(positionlevelId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../PositionLevel/GetPositionLevelDetail",
        data: { positionlevelId: positionlevelId },
        dataType: "json",
        success: function (data) {
            $("#txtPositionLevelName").val(data.PositionLevelName);
            $("#txtPositionLevelCode").val(data.PositionLevelCode);
            if (data.PositionLevel_Status == true) {
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
    var txtPositionLevelName = $("#txtPositionLevelName");
    var hdnPositionLevelID = $("#hdnPositionLevelID");
    var txtPositionLevelCode = $("#txtPositionLevelCode");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtPositionLevelName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Position Level Name")
        txtPositionLevelName.focus();
        return false;
    }
    if (txtPositionLevelCode.val().trim() == "") {
        ShowModel("Alert", "Please Enter Position Level Code")
        txtPositionLevelCode.focus();
        return false;
    } 
    
    var positionlevelViewModel = {
        PositionLevelId: hdnPositionLevelID.val(),
        PositionLevelName: txtPositionLevelName.val().trim(),
        PositionLevelCode: txtPositionLevelCode.val().trim(),
        PositionLevel_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnPositionLevelID.val() != null && hdnPositionLevelID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { positionlevelViewModel: positionlevelViewModel };
    $.ajax({
        url: "../PositionLevel/AddEditPositionLevel?accessMode=" + accessMode + "",
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
                      window.location.href = "../PositionLevel/ListPositionLevel";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PositionLevel/AddEditPositionLevel?accessMode=1";
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
    $("#txtPositionLevelName").val("");
    $("#txtPositionLevelCode").val("");
    $("#chkstatus").prop("checked", true);
    
}
