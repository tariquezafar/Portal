
$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnShiftTypeId = $("#hdnShiftTypeId");
    if (hdnShiftTypeId.val() != "" && hdnShiftTypeId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetShiftTypeDetail(hdnShiftTypeId.val());
        
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
    $("#txtShiftTypeName").focus();
        


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


function GetShiftTypeDetail(shiftTypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../ShiftType/GetShiftTypeDetail",
        data: { shiftTypeId: shiftTypeId },
        dataType: "json",
        success: function (data) {
            $("#txtShiftTypeName").val(data.ShiftTypeName);
            if (data.ShiftType_Status == true) {
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
    var txtShiftTypeName = $("#txtShiftTypeName");
    var hdnShiftTypeId = $("#hdnShiftTypeId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtShiftTypeName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Shift Type Name")
        txtShiftTypeName.focus();
        return false;
    }
   
    
    var shiftTypeViewModel = {
        ShiftTypeId: hdnShiftTypeId.val(),
        ShiftTypeName: txtShiftTypeName.val().trim(),
        ShiftType_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnShiftTypeId.val() != null && hdnShiftTypeId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { shiftTypeViewModel: shiftTypeViewModel };
    $.ajax({
        url: "../ShiftType/AddEditShiftType?accessMode=" + accessMode + "",
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
                      window.location.href = "../ShiftType/ListShiftType";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../ShiftType/AddEditShiftType";
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
 
    $("#txtShiftTypeName").val("");
    $("#chkstatus").prop("checked", true);
    
}
