$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnCalenderID = $("#hdnCalenderID");
    if (hdnCalenderID.val() != "" && hdnCalenderID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetCalenderDetail(hdnCalenderID.val());
        
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


function GetCalenderDetail(calenderId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../Calender/GetCalenderDetail",
        data: { calenderId: calenderId },
        dataType: "json",
        success: function (data) {
            $("#txtCalenderName").val(data.CalenderName);
            $("#ddlCalenderYear").val(data.CalenderYear);
            if (data.Calender_Status == true) {
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
    var txtCalenderName = $("#txtCalenderName");
    var hdnCalenderID = $("#hdnCalenderID");
    var ddlCalenderYear = $("#ddlCalenderYear");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtCalenderName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Calender Name")
        txtCalenderName.focus();
        return false;
    }
    if (ddlCalenderYear.val() == "" || ddlCalenderYear.val() == "0") {
        ShowModel("Alert", "Please select Calender Year")
        ddlCalenderYear.focus();
        return false;
    }
   
    
    var calenderViewModel = {
        CalenderId: hdnCalenderID.val(),
        CalenderName: txtCalenderName.val().trim(),
        CalenderYear: ddlCalenderYear.val().trim(),
        Calender_Status: chkstatus
        
    };
    var requestData = { calenderViewModel: calenderViewModel };
    var accessMode = 1;//Add Mode
    if (hdnCalenderID.val() != null && hdnCalenderID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    $.ajax({
        url: "../Calender/AddEditCalender?accessMode=" + accessMode + "",
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
                      window.location.href = "../Calender/ListCalender";
                  }, 2000);
                }
                else {
             setTimeout(
                        function () {
                            window.location.href = "../Calender/AddEditCalender?accessMode=1";
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
    $("#txtCalenderName").val("");
    $("#ddlCalenderYear").val("0");
    $("#chkstatus").prop("checked", true);
    
}
