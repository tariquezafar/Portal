$(document).ready(function () {
   
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnEssEmployeeId.val() != "0") {
        $("#txtEmployee").attr('readOnly', true);
        $("#txtEmployee").val(hdnEssEmployeeName.val());
        $("#hdnEmployeeId").val(hdnEssEmployeeId.val());
    }
    $("#txtApplicationNo").attr('readOnly', true);
    $("#txtApplicationDate").attr('readOnly', true);
    if (hdnApplicationId.val() != "0") {

        $("#txtCreatedBy").attr('readOnly', true);
        $("#txtCreatedDate").attr('readOnly', true);
        $("#txtModifiedBy").attr('readOnly', true);
        $("#txtModifiedDate").attr('readOnly', true);
        $("#txtRejectedDate").attr('readOnly', true);
        $("#txtFromDate").attr('readOnly', true);
        $("#txtToDate").attr('readOnly', true);
        $("#txtNoOfDays").attr('readOnly', true);
    }
   

    $("#txtFromDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        
        onSelect: function (date) {
            CalculateDate();
            var fromDate = $('#txtFromDate').datepicker('getDate');
            fromDate.setDate(fromDate.getDate() + 0);
            //$('#txtToDate').datepicker('setDate', fromDate);
            //sets minDate to dt1 date + 1
            $('#txtToDate').datepicker('option', 'minDate', fromDate);
        }
    });

    $("#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
       
        onSelect: function (date) {
            //CalculateDate();
        }
    });

    $("#ddlLeaveType").change(function () {
        CalculateDate();
    });

    var ddlCompanyBranch = $("#ddlCompanyBranch");
    $("#txtEmployee").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, departmentId: $("#txtEmployee").val(), companyBranchId: ddlCompanyBranch.val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.FirstName, value: item.EmployeeId, EmployeeCode: item.EmployeeCode, MobileNo: item.MobileNo
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtEmployee").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtEmployee").val(ui.item.label);
            $("#hdnEmployeeId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtEmployee").val("");
                $("#hdnEmployeeId").val("0");
                ShowModel("Alert", "Please select Employee from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.EmployeeCode + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};
    BindLeaveTypeList();
    BindCompanyBranchList();
    GetEmployeeLeaveBalanceList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnApplicationId.val() != "" && hdnApplicationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeLeaveAppDetail(hdnApplicationId.val());
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
 
function BindLeaveTypeList() {
    $.ajax({
        type: "GET",
        url: "../EmployeeLeaveApplication/GetLeaveTypeForEmpolyeeLeaveAppList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlLeaveType").append($("<option></option>").val(0).html("-Select Leave Type-"));
            $.each(data, function (i, item) {

                $("#ddlLeaveType").append($("<option></option>").val(item.LeaveTypeId).html(item.LeaveTypeName));
            });
        },
        error: function (Result) {
            $("#ddlLeaveType").append($("<option></option>").val(0).html("-Select Leave Type-"));
        }
    });
}
   
function GetEmployeeLeaveAppDetail(applicationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeLeaveApplication/GetEmployeeLeaveApplicationDetail",
        data: { applicationId: applicationId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicationNo").val(data.ApplicationNo);
            $("#hdnApplicationId").val(data.ApplicationId);
            $("#txtApplicationDate").val(data.ApplicationDate);
            $("#ddlLeaveType").val(data.LeaveTypeId);
            $("#hdnEmployeeId").val(data.EmployeeId);
            //$("#hdnEssEmployeeId").val(data.EmployeeId); 
            $("#txtEmployee").val(data.EmployeeName); 
            $("#txtFromDate").val(data.FromDate);
            $("#txtToDate").val(data.ToDate);
            $("#txtNoOfDays").val(data.NoofDays);
            $("#txtLeaveReason").val(data.LeaveReason);
            $("#ddlLeaveStatus").val(data.LeaveStatus);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
 
            if (data.RejectReason != "") { 
                $("#divReject").show();
                $("#txtRejectReason").val(data.RejectReason);
                $("#txtRejectedDate").val(data.RejectDate);
            }


            $("#btnAddNew").show();
             
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() {
    var txtApplicationNo = $("#txtApplicationNo");
    var hdnApplicationId = $("#hdnApplicationId");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var txtApplicationDate = $("#txtApplicationDate");
    var txtEmployee = $("#txtEmployee"); 
    var ddlLeaveType = $("#ddlLeaveType");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var txtNoOfDays = $("#txtNoOfDays");
    var txtLeaveReason = $("#txtLeaveReason");
    var ddlLeaveStatus = $("#ddlLeaveStatus");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var EmployeeId;
    if (hdnEssEmployeeId.val() != "0") {
        EmployeeId = hdnEssEmployeeId.val();
    }
    else {
        EmployeeId = hdnEmployeeId.val();
    }
   
    if (txtApplicationDate.val() == "") {
        ShowModel("Alert", "Please select Application Date")
        return false;  
    } 
    if (txtEmployee.val() == "" || txtEmployee.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false; 
    } 
    if (ddlLeaveType.val() == "" || ddlLeaveType.val() == "0") {
        ShowModel("Alert", "Please select Leave Type")
        return false;
    } 
    if (txtFromDate.val().trim() == "") {
        ShowModel("Alert", "Please Enter From Date")
        txtFromDate.focus();
        return false;
    }

    if (txtToDate.val().trim() == "") {
        ShowModel("Alert", "Please Enter To Date")
        txtToDate.focus();
        return false;
    }
    if (txtNoOfDays.val().trim() == "") {
        ShowModel("Alert", "Please Enter No Of Days")
        txtNoOfDays.focus();
        return false;
    }
    if (txtLeaveReason.val().trim() == "") {
        ShowModel("Alert", "Please Enter Leave Reason")
        txtLeaveReason.focus();
        return false;
    }
   
    if (ddlLeaveStatus.val().trim() == "") {
        ShowModel("Alert", "Please Enter Leave Reason")
        ddlLeaveStatus.focus();
        return false;
    }    

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }


    var employeeleaveapplicationViewModel = {
        ApplicationId: hdnApplicationId.val(),
        ApplicationNo: txtApplicationNo.val().trim(),
        ApplicationDate: txtApplicationDate.val(),
        LeaveTypeId: ddlLeaveType.val(),
        EmployeeId: EmployeeId,
        FromDate: txtFromDate.val(),
        ToDate: txtToDate.val(),
        LeaveReason: txtLeaveReason.val(),
        NoOfDays: txtNoOfDays.val(),
        LeaveStatus: ddlLeaveStatus.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
    };
    var accessMode = 1;//Add Mode
    if (hdnApplicationId.val() != null && hdnApplicationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

   var requestData = { employeeleaveApplicationViewModel: employeeleaveapplicationViewModel };
    $.ajax({
        url: "../EmployeeLeaveApplication/AddEditEmployeeLeaveApplication?accessMode=" + accessMode + "",
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
                      window.location.href = "../EmployeeLeaveApplication/ListEmployeeLeaveApplication";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../EmployeeLeaveApplication/AddEditEmployeeLeaveApplication";
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
    $("#txtApplicationNo").val("");
    $("#hdnApplicationId").val("0");   
    $("#ddlLeaveType").val("0");       
    $("#txtNoOfDays").val("");
    $("#txtLeaveReason").val("");
    $("#ddlLeaveStatus").val("Draft");
    $("#btnSave").show();
    $("#btnUpdate").hide();
    $("#ddlCompanyBranch").val("0");

}
function CalculateDate() {
    var fromDate = $("#txtFromDate").val();
    var toDate =  $("#txtToDate").val();
    var leaveType = $("#ddlLeaveType").val();

    if (fromDate != "" && toDate != "" && leaveType != "0") {
        var dateFrom = new Date(fromDate);
        var dateTo = new Date(toDate);

        var dateFrom_ms = dateFrom.getTime();
        var dateTo_ms = dateTo.getTime();
        var dateDifference = dateTo_ms - dateFrom_ms;
        // get days
        var leaveDays = dateDifference / 1000 / 60 / 60 / 24;

        if (leaveDays == 0) {
            if ($("#ddlLeaveType option:selected").text() == "HPL") {
                $('#txtNoOfDays').val(0.5);
            }
            else {
                $('#txtNoOfDays').val(1);
            }
        }
        else {
            if ($("#ddlLeaveType option:selected").text() == "HPL") {
                ShowModel("alert", "From date and To date should be same in HPL type leave");
                $('#txtNoOfDays').val("");
                $("#txtToDate").focus();
                return false;
            }
            else {
                $('#txtNoOfDays').val(leaveDays+1);
            }
        }
    }

}

function GetEmployeeLeaveBalanceList() {
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var requestData = {
        employeeId: hdnEssEmployeeId.val()
    };
    $.ajax({
        url: "../EmployeeLeaveApplication/GetEmployeeLeaveBalanceList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divEmployeeBalanceLeave").html("");
            $("#divEmployeeBalanceLeave").html(err);
        },
        success: function (data) {
            $("#divEmployeeBalanceLeave").html("");
            $("#divEmployeeBalanceLeave").html(data);
        }
    });
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