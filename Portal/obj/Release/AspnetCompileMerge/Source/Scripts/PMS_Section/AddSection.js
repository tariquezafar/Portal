$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnSectionID = $("#hdnSectionID");
    if (hdnSectionID.val() != "" && hdnSectionID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetSectionDetail(hdnSectionID.val());
        
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


function GetSectionDetail(sectionId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../PMS_Section/GetSectionDetail",
        data: { sectionId: sectionId },
        dataType: "json",
        success: function (data) {
            $("#txtSectionName").val(data.SectionName);
            if (data.Section_Status == true) {
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
    var txtSectionName = $("#txtSectionName");
    var hdnSectionID = $("#hdnSectionID");
  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtSectionName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Section Name")
        txtSectionName.focus();
        return false;
    } 
    var sectionViewModel = {
        SectionId: hdnSectionID.val(),
        SectionName: txtSectionName.val().trim(),
        Section_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnSectionID.val() != null && hdnSectionID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { pmssectionViewModel: sectionViewModel };
    $.ajax({
        url: "../PMS_Section/AddEditSection?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify( requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                if(accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../PMS_Section/ListSection";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PMS_Section/AddEditSection";
                    }, 2000);
                }
            }
            else {
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
    $("#txtSectionName").val("");
    $("#chkstatus").prop("checked", true);
    
}
