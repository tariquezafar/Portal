$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnAssetTypeID = $("#hdnAssetTypeID");
    if (hdnAssetTypeID.val() != "" && hdnAssetTypeID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetAssetTypeDetail(hdnAssetTypeID.val()); 
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
    $("#txtAssetTypeName").focus(); 
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


function GetAssetTypeDetail(assettypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../AssetType/GetAssetTypeDetail",
        data: { assettypeId: assettypeId },
        dataType: "json",
        success: function (data) {
            $("#txtAssetTypeName").val(data.AssetTypeName); 
            if (data.AssetType_Status == true) {
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
    var txtAssetTypeName = $("#txtAssetTypeName");
    var hdnAssetTypeID = $("#hdnAssetTypeID");
  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtAssetTypeName.val().trim() == ""){
        ShowModel("Alert", "Please Enter Asset Type Name")
        txtAssetTypeName.focus();
        return false;
    } 
    
    var assettypeViewModel = { 
        AssetTypeId: hdnAssetTypeID.val(),
        AssetTypeName: txtAssetTypeName.val().trim(),
        AssetType_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnAssetTypeID.val() != null && hdnAssetTypeID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { assettypeViewModel: assettypeViewModel };
    $.ajax({
        url: "../AssetType/AddEditAssetType?accessMode=" + accessMode + "",
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
                      window.location.href = "../AssetType/ListAssetType";
                  }, 2000);
                }
                else {
                    setTimeout(
                   function () {
                       window.location.href = "../AssetType/AddEditAssetType?accessMode=1";
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
    $("#txtAssetTypeName").val("");
    $("#chkstatus").prop("checked", true);
    
}
