$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtApplicationNo").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);

    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    BindLeaveTypeList();
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $("#txtEmployee").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, departmentId: $("#txtEmployee").val(), designationId: $("#txtEmployee").val()
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
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnApplicationId.val() != "" && hdnApplicationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeLeaveAppDetail(hdnApplicationId.val());
       }, 2000); 
        CalculateDate();
        if (hdnAccessMode.val() == "3") { 
            $("#btnUpdate").hide(); 
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true); 
        }
        else {
            
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);

            $("#ddlLeaveStatus").attr('disabled', false);
            $("#txtRejectedReason").attr('disabled', true);
            $("#txtRejectedReason").attr('readOnly', false);

            $("#btnUpdate").show();
        }
    }
    else {
         
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
    var hdnApplicationId = $("#hdnApplicationId");
    var ddlLeaveStatus = $("#ddlLeaveStatus");
    var txtRejectedReason = $("#txtRejectedReason");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var ddlLeaveType = $("#ddlLeaveType");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var txtNoOfDays = $("#txtNoOfDays");
    var status = true;
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (ddlLeaveStatus.val() == "0" || ddlLeaveStatus.val() == "") {
        ShowModel("Alert", "Please select Approved/ Rejdcted")
        ddlLeaveStatus.focus();
        return false;
    } 
    if (txtRejectedReason.val() == "Rejected" && txtRejectedReason.val() == "") {
        ShowModel("Alert", "Please Enter Rejection Reason")
        txtRejectedReason.focus();
        return false;
    }
    
    var employeeleaveapplicationViewModel = {
        ApplicationId: hdnApplicationId.val(),
        LeaveStatus: ddlLeaveStatus.val(),
        RejectReason: txtRejectedReason.val().trim()
    };
    
    var accessMode = 1;//Add Mode
    if (hdnApplicationId.val() != null && hdnApplicationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { employeeleaveApplicationViewModel: employeeleaveapplicationViewModel};
    $.ajax({
        url: "../EmployeeLeaveApplication/ApproveEmployeeLeaveApplication?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../EmployeeLeaveApplication/ListEmployeeLeaveApplicationApproval";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../EmployeeLeaveApplication/ApproveEmployeeLeaveApplication";
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
    $("#txtApplicationDate").val("");
    $("#ddlLeaveType").val("0");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployee").val(""); 
    $("#txtFromDate").val("");
    $("#txtToDate").val("");
    $("#txtLeaveReason").val("");
    $("#ddlLeaveStatus").val("");
    $("#btnSave").show();
    $("#btnUpdate").hide();
    $("#ddlCompanyBranch").val("0");
    
}


function EnableDisableRejectReason() {
    var approvalStatus = $("#ddlLeaveStatus option:selected").text();
    if (approvalStatus == "Rejected") {
        $("#txtRejectedReason").attr("disabled", false);
    }
    else {
        $("#txtRejectedReason").attr("disabled", true);
        $("#txtRejectedReason").val("");
    }
}
function CalculateDate() {
    var d1 = $("#txtFromDate").val();
    var d2 = $("#txtToDate").val();

    var date1 = new Date(d1);
    var date2 = new Date(d2);

    var date1_ms = date1.getTime();
    var date2_ms = date2.getTime();
    var diff = date2_ms - date1_ms;
    // get days
    var days = diff / 1000 / 60 / 60 / 24;
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