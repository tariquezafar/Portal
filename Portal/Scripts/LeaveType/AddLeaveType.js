
$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnLeaveTypeId = $("#hdnLeaveTypeId");
    if (hdnLeaveTypeId.val() != "" && hdnLeaveTypeId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetLeaveTypeDetail(hdnLeaveTypeId.val());
        
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
    $("#txtLeaveTypeName").focus();
        


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

function GetLeaveTypeDetail(leaveTypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../LeaveType/GetLeaveTypeDetail",
        data: { leaveTypeId: leaveTypeId },
        dataType: "json",
        success: function (data) {
            $("#txtLeaveTypeName").val(data.LeaveTypeName);
            $("#txtLeaveTypeCode").val(data.LeaveTypeCode);
            $("#txtLeavePeriod").val(data.LeavePeriod);
            $("#txtPayPeriod").val(data.PayPeriod);
            $("#txtWorkPeriod").val(data.WorkPeriod);
            if (data.LeaveType_Status == true) {
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
    var txtLeaveTypeName = $("#txtLeaveTypeName");
    var hdnLeaveTypeId = $("#hdnLeaveTypeId");
    var txtLeaveTypeCode = $("#txtLeaveTypeCode");
    var txtLeavePeriod = $("#txtLeavePeriod");
    var txtPayPeriod = $("#txtPayPeriod");
    var txtWorkPeriod = $("#txtWorkPeriod");

    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtLeaveTypeName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Leave Type Name")
        txtLeaveTypeName.focus();
        return false;
    }
    if (txtLeaveTypeCode.val().trim() == "") {
        ShowModel("Alert", "Please Enter Leave Type Code")
        txtLeaveTypeCode.focus();
        return false;
    }
    //if (txtLeavePeriod.val().trim() == "") {
    //    ShowModel("Alert", "Please Enter Leave Period")
    //    txtLeavePeriod.focus();
    //    return false;
    //}
    //if (txtPayPeriod.val().trim() == "") {
    //    ShowModel("Alert", "Please Enter Pay Period")
    //    txtPayPeriod.focus();
    //    return false;
    //}
    //if (txtWorkPeriod.val().trim() == "") {
    //    ShowModel("Alert", "Please Enter Work Period")
    //    txtWorkPeriod.focus();
    //    return false;
    //}
   
    
    var leavetypeViewModel = {
        LeaveTypeId: hdnLeaveTypeId.val(),
        LeaveTypeName: txtLeaveTypeName.val().trim(),
        LeaveTypeCode: txtLeaveTypeCode.val().trim(),
        LeavePeriod: txtLeavePeriod.val().trim(),
        PayPeriod: txtPayPeriod.val().trim(),
        WorkPeriod: txtWorkPeriod.val().trim(),
        LeaveType_Status: chkstatus
      };
    var requestData = { leavetypeViewModel: leavetypeViewModel };
    var accessMode = 1;//Add Mode
    if (hdnLeaveTypeId.val() != null && hdnLeaveTypeId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    $.ajax({
        url: "../LeaveType/AddEditLeaveType?accessMode=" + accessMode + "",
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
                      window.location.href = "../LeaveType/ListLeaveType";
                  }, 2000);
                }
                else {
                    setTimeout(
                        function () {
                            window.location.href = "../LeaveType/AddEditLeaveType?accessMode=1";
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
 
    $("#txtLeaveTypeName").val("");
    $("#txtLeaveTypeCode").val("");
    $("#txtLeavePeriod").val("");
    $("#txtPayPeriod").val("");
    $("#txtWorkPeriod").val("");
    $("#chkstatus").prop("checked", true);
    
}
