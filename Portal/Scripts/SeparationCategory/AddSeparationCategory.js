$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnSeparationCategoryID = $("#hdnSeparationCategoryID");
    if (hdnSeparationCategoryID.val() != "" && hdnSeparationCategoryID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetSeparationCategoryDetail(hdnSeparationCategoryID.val());
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
    $("#hdnSeparationCategoryID").focus();
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


function GetSeparationCategoryDetail(separationcategoryId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../SeparationCategory/GetSeparationCategoryDetail",
        data: { separationcategoryId: separationcategoryId },
        dataType: "json",
        success: function (data) {
            $("#txtSeparationCategoryName").val(data.SeparationCategoryName);
            if (data.SeparationCategory_Status == true) {
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
    var txtSeparationCategoryName = $("#txtSeparationCategoryName");
    var hdnSeparationCategoryID = $("#hdnSeparationCategoryID");
  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtSeparationCategoryName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Separation Category Name")
        txtSeparationCategoryName.focus();
        return false;
    }
    
    
    var separationcategoryViewModel = {
        SeparationCategoryId: hdnSeparationCategoryID.val(),
        SeparationCategoryName: txtSeparationCategoryName.val().trim(),
        SeparationCategory_Status: chkstatus
        
    };

    var accessMode = 1;//Add Mode
    if (hdnSeparationCategoryID.val() != null && hdnSeparationCategoryID.val() != 0) {
        accessMode = 2;//Edit Mode
    }


    var requestData = { separationcategoryViewModel: separationcategoryViewModel };
    $.ajax({
        url: "../SeparationCategory/AddEditSeparationCategory?accessMode=" + accessMode + "",
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
                      window.location.href = "../SeparationCategory/ListSeparationCategory";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../SeparationCategory/AddEditSeparationCategory";
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
    $("#txtSeparationCategoryName").val(""); 
    $("#chkstatus").prop("checked", true);
    
}
