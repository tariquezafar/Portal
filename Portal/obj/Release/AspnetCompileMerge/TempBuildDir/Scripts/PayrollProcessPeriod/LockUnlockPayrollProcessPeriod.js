$(document).ready(function () {
 

    $("#txtPayrollProcessDate").attr('readOnly', true);
    $("#txtPayrollProcessingStartDate").attr('readOnly', true);
    $("#txtPayrollProcessingEndDate").attr('readOnly', true);

    $("#ddlPayrollProcessStatus").attr('disabled', true);
    $("#ddlPayrollLocked").attr('disabled', true);
    $("#txtPayrollLockDate").attr('readOnly', true);

    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    BindPayrollMonthList();
    ShowHideParollProcessDate();
 
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPayrollProcessingPeriodId = $("#hdnPayrollProcessingPeriodId");
    if (hdnPayrollProcessingPeriodId.val() != "" && hdnPayrollProcessingPeriodId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPayrollProcessPeriodDetail(hdnPayrollProcessingPeriodId.val());
       }, 1000);

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $(".editonly").hide();           
        }
        else {
            $("#ddlMonth").attr('disabled', true);
            $("#ddlPayrollLocked").attr('disabled', false);           
            $("#btnUpdate").hide();
            $(".editonly").show();
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $(".editonly").show();
        $("#ddlMonth").attr('disabled', true);
        $("#ddlMonth").attr('disabled', true);
        $("#ddlPayrollLocked").attr('disabled', false);
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

function SaveData() {
    var hdnpayrollProcessingPeriodId = $("#hdnPayrollProcessingPeriodId");
    var ddlPayrollLocked = $("#ddlPayrollLocked");  
    var payrollProcessPeriodViewModel = {
        PayrollProcessingPeriodId: hdnpayrollProcessingPeriodId.val(),
        PayrollLocked: ddlPayrollLocked.val(),
       
       
    }; 
    var requestData = { payrollProcessPeriodViewModel: payrollProcessPeriodViewModel };
    $.ajax({
        url: "../PayrollProcessPeriod/LockUnlockPayrollProcessPeriod",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
            
                setTimeout(
                   function () {
                       window.location.href = "../PayrollProcessPeriod/LockUnlockPayrollProcessPeriod?payrollProcessingPeriodId=" + data.trnId + "&AccessMode=2";
                   }, 2000);

              
                $("#btnUpdate").show();
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
function BindPayrollMonthList() {
    $("#ddlMonth").val(0);
    $("#ddlMonth").html("");
    $.ajax({
        type: "GET",
        url: "../SalaryReport/GetProcessedMonthList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Select Period-"));
            $.each(data, function (i, item) {
                $("#ddlMonth").append($("<option></option>").val(item.PayrollProcessingPeriodId).html(item.PayrollProcessingStartDate + ' - ' + item.PayrollProcessingEndDate));
            });
        },
        error: function (Result) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Select Period-"));
        }
    });
}

function GetProcessStartEndDate() {
    var monthId = $("#ddlMonth option:selected").val();
    $("#txtPayrollProcessingStartDate").val("");
    $("#txtPayrollProcessingEndDate").val("");
    $.ajax({
        type: "GET",
        url: "../PayrollProcessPeriod/GetPayrollMonthStartAndEndDate",
        data: { monthId: monthId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#txtPayrollProcessingStartDate").val(data.PayrollProcessingStartDate);
            $("#txtPayrollProcessingEndDate").val(data.PayrollProcessingEndDate);
            
            
        },
        error: function (Result) {
            $("#txtPayrollProcessingStartDate").val("");
            $("#txtPayrollProcessingEndDate").val("");
        }
    });
}
function GetPayrollProcessPeriodDetail(obj) {
    var payrollProcessingPeriodId = $(obj).val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PayrollProcessPeriod/GetPayrollProcessPeriodDetail",
        data: { payrollProcessingPeriodId: payrollProcessingPeriodId },
        dataType: "json",
        success: function (data) {
            $("#hdnpayrollProcessingPeriodId").val(data.PayrollProcessingPeriodId);
            $("#ddlPayrollProcessStatus").val(data.PayrollProcessStatus);
            $("#ddlPayrollLocked").val(data.PayrollLocked);


        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}
function GetPayrollProcessPeriodDetail(payrollProcessingPeriodId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PayrollProcessPeriod/GetPayrollProcessPeriodDetail",
        data: { payrollProcessingPeriodId: payrollProcessingPeriodId },
        dataType: "json",
        success: function (data) {
            $("#ddlMonth").val(data.PayrollProcessingPeriodId);
            $("#hdnPayrollProcessingPeriodId").val(data.PayrollProcessingPeriodId);
            $("#ddlPayrollProcessStatus").val(data.PayrollProcessStatus);          
            $("#ddlPayrollLocked").val(data.PayrollLocked);
            $("#hdnPayrollLocked").val(data.PayrollLocked);
            $("#txtPayrollLockDate").val(data.PayrollLockDate);           
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}
function ShowHideParollProcessDate()
{     
    if ($("#ddlPayrollLocked").val() == $("#hdnPayrollLocked").val())
    {
        $("#btnUpdate").hide();
    }
    else
    {
        $("#btnUpdate").show();
    }
}