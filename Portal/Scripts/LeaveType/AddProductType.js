
$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnProductTypeID = $("#hdnProductTypeID");
    if (hdnProductTypeID.val() != "" && hdnProductTypeID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetProductTypeDetail(hdnProductTypeID.val());
        
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
    $("#txtProductTypeName").focus();
        


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


function GetProductTypeDetail(producttypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../ProductType/GetProductTypeDetail",
        data: { producttypeId: producttypeId },
        dataType: "json",
        success: function (data) {
            $("#txtProductTypeName").val(data.ProductTypeName);
            $("#txtProductTypeCode").val(data.ProductTypeCode);
            if (data.ProductType_Status == true) {
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
    var txtProductTypeName = $("#txtProductTypeName");
    var hdnProductTypeID = $("#hdnProductTypeID");
    var txtProductTypeCode = $("#txtProductTypeCode");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtProductTypeName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Product Type Name")
        txtProductTypeName.focus();
        return false;
    }
    if (txtProductTypeCode.val().trim() == "") {
        ShowModel("Alert", "Please Enter Product Type Code")
        txtProductTypeCode.focus();
        return false;
    } 
    
    var producttypeViewModel = {
        ProductTypeId: hdnProductTypeID.val(),
        ProductTypeName: txtProductTypeName.val().trim(),
        ProductTypeCode: txtProductTypeCode.val().trim(),
        ProductType_Status: chkstatus
        
    };
    var requestData = { producttypeViewModel: producttypeViewModel };
    $.ajax({
        url: "../ProductType/AddEditProductType",
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
 
    $("#txtProductTypeName").val("");
    $("#txtProductTypeCode").val("");
    $("#chkstatus").prop("checked", true);
    
}
