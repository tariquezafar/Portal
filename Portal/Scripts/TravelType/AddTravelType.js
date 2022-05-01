$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnTravelTypeID = $("#hdnTravelTypeID");
    if (hdnTravelTypeID.val() != "" && hdnTravelTypeID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetTravelTypeDetail(hdnTravelTypeID.val());
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
    $("#txtTravelTypeName").focus(); 
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


function GetTravelTypeDetail(traveltypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../TravelType/GetTravelTypeDetail",
        data: { traveltypeId: traveltypeId },
        dataType: "json",
        success: function (data) {
            $("#txtTravelTypeName").val(data.TravelTypeName); 
            if (data.TravelType_Status == true) {
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
    var txtTravelTypeName = $("#txtTravelTypeName");
    var hdnTravelTypeID = $("#hdnTravelTypeID");
  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtTravelTypeName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Travel Type Name")
        txtTravelTypeName.focus();
        return false;
    } 
    
    var traveltypeViewModel = { 
        TravelTypeId: hdnTravelTypeID.val(),
        TravelTypeName: txtTravelTypeName.val().trim(),
        TravelType_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnTravelTypeID.val() != null && hdnTravelTypeID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { traveltypeViewModel: traveltypeViewModel };
    $.ajax({
        url: "../TravelType/AddEditTravelType?accessMode=" + accessMode + "",
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
                      window.location.href = "../TravelType/ListTravelType";
                  }, 2000);
                }
                else {

                    setTimeout(
                 function () {
                     window.location.href = "../TravelType/AddEditTravelType?accessMode=3";
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
    $("#txtTravelTypeName").val("");
    $("#chkstatus").prop("checked", true);
    
}
