$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnPositionTypeID = $("#hdnPositionTypeID");
    if (hdnPositionTypeID.val() != "" && hdnPositionTypeID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetPositionTypeDetail(hdnPositionTypeID.val());
        
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
    $("#txtPositionTypeName").focus(); 

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


function GetPositionTypeDetail(positiontypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../PositionType/GetPositionTypeDetail",
        data: { positiontypeId: positiontypeId },
        dataType: "json",
        success: function (data) {
            $("#txtPositionTypeName").val(data.PositionTypeName);
            $("#txtPositionTypeCode").val(data.PositionTypeCode);
            if (data.PositionType_Status == true) {
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
    var txtPositionTypeName = $("#txtPositionTypeName");
    var hdnPositionTypeID = $("#hdnPositionTypeID");
    var txtPositionTypeCode = $("#txtPositionTypeCode");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtPositionTypeName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Position Type Name")
        txtPositionTypeName.focus();
        return false;
    }
    if (txtPositionTypeCode.val().trim() == "") {
        ShowModel("Alert", "Please Enter Position Type Code")
        txtPositionTypeCode.focus();
        return false;
    }  
    var positiontypeViewModel = {
        PositionTypeId: hdnPositionTypeID.val(),
        PositionTypeName: txtPositionTypeName.val().trim(),
        PositionTypeCode: txtPositionTypeCode.val().trim(),
        PositionType_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnPositionTypeID.val() != null && hdnPositionTypeID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { positiontypeViewModel: positiontypeViewModel };
    $.ajax({
        url: "../PositionType/AddEditPositionType?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status=="SUCCESS")
            {
                ShowModel("Alert", data.message);
                ClearFields();
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../PositionType/ListPositionType";
                  }, 2000);
                }
                else {
                    setTimeout(
                   function () {
                       window.location.href = "../PositionType/AddEditPositionType?accessMode=1";
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
    $("#txtPositionTypeName").val("");
    $("#txtPositionTypeCode").val("");
    $("#chkstatus").prop("checked", true);
    
}
function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
}
document.onkeypress = stopRKey;