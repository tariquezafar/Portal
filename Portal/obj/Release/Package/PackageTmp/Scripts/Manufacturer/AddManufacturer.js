
$(document).ready(function () {

    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnmanufacturerId = $("#hdnmanufacturerId");
    if (hdnmanufacturerId.val() != "" && hdnmanufacturerId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetManufacturerDetail(hdnmanufacturerId.val());
        
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
    $("#txtManufacturerCode").focus();
        


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


function GetManufacturerDetail(manufacturerId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../Manufacturer/GetManufacturereDetail",
        data: { manufacturerId: manufacturerId },
        dataType: "json",
        success: function (data) {
            $("#txtManufacturerName").val(data.ManufacturerName);
            $("#txtManufacturerCode").val(data.ManufacturerCode);
            if (data.Manufacturer_Status == true) {
                $("#chkstatus").attr("checked", true);
            }
            else {
                $("#chkstatus").attr("checked", false);
            }

            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    
}

function SaveData()
{
    var txtManufacturerCode = $("#txtManufacturerCode");
    var hdnmanufacturerId = $("#hdnmanufacturerId");
    var txtManufacturerName = $("#txtManufacturerName");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtManufacturerName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Manufacturer Name")
        txtManufacturerName.focus();
        return false;
    }
    if (txtManufacturerCode.val().trim() == "") {
        ShowModel("Alert", "Please Enter Manufacturer Code")
        txtManufacturerCode.focus();
        return false;
    } 
    
    var manufacturerViewModel = {
        ManufacturerId: hdnmanufacturerId.val(),
        ManufacturerName: txtManufacturerName.val().trim(),
        ManufacturerCode: txtManufacturerCode.val().trim(),
        Manufacturer_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnmanufacturerId.val() != null && hdnmanufacturerId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { manufacturerViewModel: manufacturerViewModel };
    $.ajax({
        url: "../Manufacturer/AddEditManufacturer?AccessMode=" + accessMode + "",
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
                setTimeout(
               function () {
                   window.location.href = "../Manufacturer/ListManufacturer";
               }, 2000);
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
    $("#hdnmanufacturerId").val("0");
    $("#txtManufacturerCode").val("");
    $("#txtManufacturerName").val("");
    $("#chkstatus").prop("checked", true);
    
}
