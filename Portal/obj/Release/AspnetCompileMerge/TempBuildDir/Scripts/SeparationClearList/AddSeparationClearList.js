$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnSeparationClearListID = $("#hdnSeparationClearListID");
    if (hdnSeparationClearListID.val() != "" && hdnSeparationClearListID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetSeparationClearListDetail(hdnSeparationClearListID.val());
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
    $("#hdnSeparationClearListID").focus();
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


function GetSeparationClearListDetail(separationclearlistId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../SeparationClearList/GetSeparationClearListDetail",
        data: { separationclearlistId: separationclearlistId },
        dataType: "json",
        success: function (data) {
            $("#txtSeparationClearListName").val(data.SeparationClearListName);
            $("#txtSeparationClearListDesc").val(data.SeparationClearListDesc);
            if (data.SeparationClearList_Status == true) {
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
    var txtSeparationClearListName = $("#txtSeparationClearListName");
    var txtSeparationClearListDesc = $("#txtSeparationClearListDesc");
    var hdnSeparationClearListID = $("#hdnSeparationClearListID");
  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtSeparationClearListName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Separation Clear List Name")
        txtSeparationClearListName.focus();
        return false;
    }
    if (txtSeparationClearListDesc.val().trim() == "") {
        ShowModel("Alert", "Please Enter Separation Clear List Desc")
        txtSeparationClearListDesc.focus();
        return false;
    }
    
    var separationclearlistViewModel = {
        SeparationClearListId: hdnSeparationClearListID.val(),
        SeparationClearListName: txtSeparationClearListName.val().trim(),
        SeparationClearListDesc: txtSeparationClearListDesc.val().trim(),
        SeparationClearList_Status: chkstatus
        
    };

    var accessMode = 1;//Add Mode
    if (hdnSeparationClearListID.val() != null && hdnSeparationClearListID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { separationclearlistViewModel: separationclearlistViewModel };
    $.ajax({
        url: "../SeparationClearList/AddEditSeparationClearList?accessMode=" + accessMode + "",
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
                      window.location.href = "../SeparationClearList/ListSeparationClearList";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../SeparationClearList/AddEditSeparationClearList";
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
    $("#txtSeparationClearListName").val("");
    $("#txtSeparationClearListDesc").val("");
    $("#chkstatus").prop("checked", true);
    
}
