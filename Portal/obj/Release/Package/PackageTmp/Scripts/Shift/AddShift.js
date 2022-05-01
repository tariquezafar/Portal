$(document).ready(function () {  
   
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtValidTill").attr('readOnly', true);
 
    BindCompanyBranchList();
    BindShiftTypeList();

    $("#txtValidTill").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    }); 

    $('#txtShiftStartTime').timepicker({ 'step': 15, 'timeFormat': 'H:i:s' });
    $('#txtShiftEndTime').timepicker({ 'step': 15, 'timeFormat': 'H:i:s' });
    $('#txtOvertimeStartTime').timepicker({ 'step': 15, 'timeFormat': 'H:i:s' });
    $('#txtLateArrivalLimit').timepicker({ 'step': 15, 'timeFormat': 'H:i:s' });
    $('#txtEarlyGoLimit').timepicker({ 'step': 15, 'timeFormat': 'H:i:s' }); 
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnShiftId = $("#hdnShiftId");
     
    if (hdnShiftId.val() != "" && hdnShiftId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetShiftDetail(hdnShiftId.val());
       }, 2000);

        

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
        

        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    } 

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
 
function BindShiftTypeList() {
    $.ajax({
        type: "GET",
        url: "../Shift/GetShiftTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlShiftType").append($("<option></option>").val(0).html("Select Shift Type"));
            $.each(data, function (i, item) {
                $("#ddlShiftType").append($("<option></option>").val(item.ShiftTypeId).html(item.ShiftTypeName));
            });
        },
        error: function (Result) {
            $("#ddlShiftType").append($("<option></option>").val(0).html("Select Shift Type"));
        }
    });
}

function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    } 
}
 
function GetShiftDetail(shiftId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Shift/GetShiftDetail",
        data: { shiftId: shiftId },
        dataType: "json",
        success: function (data) {
            
            $("#txtShiftName").val(data.ShiftName);
            $("#hdnShiftId").val(data.ShiftId);
            $("#txtShiftDescription").val(data.ShiftDescription);
            $("#ddlShiftType").val(data.ShiftTypeId);
            $("#txtNormalPaidHours").val(data.NormalPaidHours);
            $("#txtShiftStartTime").val(data.ShiftStartTime);
            $("#txtShiftEndTime").val(data.ShiftEndTime);
            $("#txtLateArrivalLimit").val(data.LateArrivalLimit);
            $("#txtEarlyGoLimit").val(data.EarlyGoLimit);

            $("#txtOvertimeStartTime").val(data.OvertimeStartTime);
            $("#txtValidTill").val(data.ValidTill);
            $("#txtFullDayWorkHour").val(data.FullDayWorkHour);
            $("#txtHalfDayWorkHour").val(data.HalfDayWorkHour);
            $("#txtAllowance").val(data.Allowance);

            $("#ddlCompanyBranch").val(data.CompanyBranchId);

            if (data.Shift_Status == true) {
                $("#chkstatus").attr("checked", true);
            }
            else {
                $("#chkstatus").attr("checked", false);
            }
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
           
            $("#btnAddNew").show();
             
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() {
    var txtShiftName = $("#txtShiftName");
    var hdnShiftId = $("#hdnShiftId");
    var txtShiftDescription = $("#txtShiftDescription");
    var ddlShiftType = $("#ddlShiftType");
    var txtNormalPaidHours = $("#txtNormalPaidHours");
    var txtShiftStartTime = $("#txtShiftStartTime");
    var txtShiftEndTime = $("#txtShiftEndTime");
    var txtLateArrivalLimit = $("#txtLateArrivalLimit");
    var txtEarlyGoLimit = $("#txtEarlyGoLimit");
    var txtOvertimeStartTime = $("#txtOvertimeStartTime");
    var txtValidTill = $("#txtValidTill");
    var txtFullDayWorkHour = $("#txtFullDayWorkHour");
    var txtHalfDayWorkHour = $("#txtHalfDayWorkHour");
    var txtAllowance = $("#txtAllowance");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var chkstatus = $("#chkstatus").is(':checked') ? true : false;;

    if (txtShiftName.val() == "") {
        ShowModel("Alert", "Please select Shift Name")
        return false;

    } 
    if (ddlShiftType.val() == "" || ddlShiftType.val() == "0") {
        ShowModel("Alert", "Please select Shift Type")
        return false;

    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    if (txtNormalPaidHours.val() == "") {
        ShowModel("Alert", "Please select Normal Paid Hours.")
        return false;

    } 
    if (txtShiftStartTime.val() == "" || txtShiftStartTime.val() == "0") {
        ShowModel("Alert", "Please select Shift Start Time")
        return false;
    }
 
    if (txtShiftEndTime.val() == "" || txtShiftEndTime.val() == "0") {
        ShowModel("Alert", "Please Enter Shift End Time.")
        return false;

    }
    if (txtLateArrivalLimit.val() == "" || txtLateArrivalLimit.val() == "0") {
        ShowModel("Alert", "Please Enter Late Arrival Limit")
        return false;

    }
    if (txtEarlyGoLimit.val() == "" || txtEarlyGoLimit.val() == "0") {
        ShowModel("Alert", "Please Enter Early Go Limit")
        return false;

    }
    
    if (txtOvertimeStartTime.val() == "" || txtOvertimeStartTime.val() == "0") {
        ShowModel("Alert", "Please Select Overtime Start Time")
        return false;

    }
    if (txtValidTill.val() == "" || txtValidTill.val() == "0") {
        ShowModel("Alert", "Please Enter Vaid Till")
        return false;

    }
    if (txtFullDayWorkHour.val() == "" || txtFullDayWorkHour.val() == "0") {
        ShowModel("Alert", "Please Enter Full Day Work Hour")
        return false; 
    }
    if (txtHalfDayWorkHour.val() == "" || txtHalfDayWorkHour.val() == "0") {
        ShowModel("Alert", "Please Enter Half Day Work Hour")
        return false;
    }
    if (txtAllowance.val()== "") {
        ShowModel("Alert", "Please Enter Allowance") 
        return false;
    }
    
    var shiftViewModel = {
        ShiftName: txtShiftName.val(),
        ShiftId: hdnShiftId.val(),
        ShiftDescription: txtShiftDescription.val(),
        ShiftTypeId: ddlShiftType.val(),
        NormalPaidHours: txtNormalPaidHours.val(),
        ShiftStartTime: txtShiftStartTime.val(),
        ShiftEndTime: txtShiftEndTime.val(),
        LateArrivalLimit: txtLateArrivalLimit.val(),
        EarlyGoLimit: txtEarlyGoLimit.val(),
        OvertimeStartTime: txtOvertimeStartTime.val(),
        ValidTill: txtValidTill.val(),
        FullDayWorkHour: txtFullDayWorkHour.val(),
        HalfDayWorkHour: txtHalfDayWorkHour.val(),
        Allowance: txtAllowance.val(),
        Shift_Status: chkstatus,
        CompanyBranchId: ddlCompanyBranch.val(),
    };
     
    var accessMode = 1;//Add Mode
    if (hdnShiftId.val() != null && hdnShiftId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { shiftViewModel: shiftViewModel };
    $.ajax({
        url: "../Shift/AddEditShift?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();

                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../Shift/ListShift";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../Shift/AddEditShift";
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
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields() {  

    $("#txtShiftName").val("");
    $("#txtNormalPaidHours").val("");
    $("#hdnShiftId").val("0");
    $("#txtShiftDescription").val("");
    $("#ddlShiftType").val("0");
    $("#txtShiftStartTime").val("");
    $("#txtShiftEndTime").val("");
    $("#txtLateArrivalLimit").val("");
    $("#txtEarlyGoLimit").val("");
    $("#txtOvertimeStartTime").val("");
    $("#txtLoanStartDate").val("");
    $("#txtLoanEndDate").val("");
    $("#txtLoanInstallmentAmount").val("");
    $("#txtValidTill").val("");
    $("#txtFullDayWorkHour").val("");
    $("#txtHalfDayWorkHour").val("");
    $("#txtAllowance").val("");
    $("#btnSave").show();
    $("#btnUpdate").hide();
    $("#ddlCompanyBranch").val("0");

}
 
 
 
function BindCompanyBranchList() {
    $("#ddlCompanyBranch").val(0);
    $("#ddlCompanyBranch").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");
            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
                $("#ddlCompanyBranch").attr('disabled', true);
            }
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}
 