
$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdngoalCategoryId = $("#hdngoalCategoryId");
    if (hdngoalCategoryId.val() != "" && hdngoalCategoryId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetGoalCategoryDetail(hdngoalCategoryId.val());
        
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
    $("#txtGoalCategoryName").focus();
        


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
function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}

function GetGoalCategoryDetail(goalCategoryId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../PMSGoalCategory/GetGoalCategoryDetail",
        data: { goalCategoryId: goalCategoryId },
        dataType: "json",
        success: function (data) {
            $("#txtGoalCategoryName").val(data.GoalCategoryName);
            $("#txtWeight").val(data.Weight);

            if (data.GoalCategory_Status == true) {
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
    var txtGoalCategoryName = $("#txtGoalCategoryName");
    var hdngoalCategoryId = $("#hdngoalCategoryId");
    var txtWeight = $("#txtWeight");



    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtGoalCategoryName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Goal Category Name")
        txtGoalCategoryName.focus();
        return false;
    }
    if (txtWeight.val().trim() == "" ) {
        ShowModel("Alert", "Please Enter Weight")
        txtWeight.focus();
        return false;
    }
    if ((txtWeight.val() <= 0) || (txtWeight.val() > 100)) {
        ShowModel("Alert", "Please Enter the correct Weightage(%)")
        txtWeight.focus();
        return false;
    }
    
    var pMSGoalCategoryViewModel = {
        GoalCategoryId: hdngoalCategoryId.val(),
        GoalCategoryName: txtGoalCategoryName.val().trim(),
        Weight: txtWeight.val().trim(),
        GoalCategory_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdngoalCategoryId.val() != null && hdngoalCategoryId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { pMSGoalCategoryViewModel: pMSGoalCategoryViewModel };
    $.ajax({
        url: "../PMSGoalCategory/AddEditGoalCategory?accessMode=" + accessMode + "",
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
                      window.location.href = "../PMSGoalCategory/ListGoalCategoryType";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PMSGoalCategory/AddEditGoalCategory";
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
 
    $("#txtGoalCategoryName").val("");
    $("#txtWeight").val("");
    $("#chkstatus").prop("checked", true);
    
}
