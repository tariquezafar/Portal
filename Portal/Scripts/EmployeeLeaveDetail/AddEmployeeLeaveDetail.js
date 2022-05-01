$(document).ready(function () {
    BindLeaveTypeList();
    $("#txtLeaveDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);

    $("#txtLeaveDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        maxDate: '0D',
        onSelect: function (selected) {
        }
    });
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnEmployeeLeaveId = $("#hdnEmployeeLeaveId");
    if (hdnEmployeeLeaveId.val() != "" && hdnEmployeeLeaveId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetEmployeeLeaveDetail(hdnEmployeeLeaveId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
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
   

    $("#txtEmployeeName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, departmentId: $("#txtEmployeeName").val(), designationId: $("#txtEmployeeName").val()
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
            $("#txtEmployeeName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtEmployeeName").val(ui.item.label);
            $("#hdnEmployeeId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtEmployeeName").val("");
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
function BindLeaveTypeList() {
    $("#ddlLeaveType").val(0);
    $("#ddlLeaveType").html("");
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

function GetEmployeeLeaveDetail(employeeLeaveId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeLeaveDetail/GetEmployeeLeaveDetail",
        data: { employeeLeaveId: employeeLeaveId },
        dataType: "json",
        success: function (data) {
            
            $("#hdnEmployeeLeaveId").val(data.EmployeeLeaveId);
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#txtEmployeeName").val(data.EmployeeName);
           
            $("#ddlLeaveType").val(data.LeaveTypeId);
            $("#txtLeaveDate").val(data.LeaveDate);
            $("#txtLeaveCount").val(data.LeaveCount);
            if (data.EmployeeLeaveDetailStatus == true) {
                $("#chkStatus").attr("checked", true);
            }
            else {
                $("#chkStatus").attr("checked", false);
            }
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function SaveData() {
    var hdnEmployeeLeaveId = $("#hdnEmployeeLeaveId")
    var hdnEmployeeId = $("#hdnEmployeeId");
    var txtEmployee = $("#txtEmployee");
    var ddlLeaveType = $("#ddlLeaveType");
    var txtLeaveDate = $("#txtLeaveDate");
    var txtLeaveCount = $("#txtLeaveCount");
    var chkStatus = $("#chkStatus").is(':checked') ? true : false;
  
    if (txtEmployee.val() == "" || txtEmployee.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false;
    }
    if (ddlLeaveType.val() == "" || ddlLeaveType.val() == "0") {
        ShowModel("Alert", "Please select Leave Type")
        return false;
    }
    if (txtLeaveDate.val().trim() == "") {
        ShowModel("Alert", "Please Select Leave Date")
        txtLeaveDate.focus();
        return false;
    }
      if (txtLeaveCount.val().trim() == "") {
        ShowModel("Alert", "Please Enter No Of Leaves")
        txtLeaveCount.focus();
        return false;
    }
    var employeeLeaveDetailViewmodel = {
        EmployeeLeaveId: hdnEmployeeLeaveId.val(),
        LeaveTypeId: ddlLeaveType.val(),
        EmployeeId: hdnEmployeeId.val(),
        LeaveDate: txtLeaveDate.val(),
        LeaveCount: txtLeaveCount.val(),
        EmployeeLeaveDetailStatus: chkStatus
    };

    var accessMode = 1;//Add Mode
    if (hdnEmployeeLeaveId.val() != null && hdnEmployeeLeaveId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { employeeLeaveDetailViewmodel: employeeLeaveDetailViewmodel };
    $.ajax({
        url: "../EmployeeLeaveDetail/AddEditEmployeeLeaveDetail",
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
                     window.location.href = "../EmployeeLeaveDetail/ListEmployeeLeaveDetail";
                 }, 2000);
               }
               else {
                   setTimeout(
                   function () {
                       window.location.href = "../EmployeeLeaveDetail/AddEditEmployeeLeaveDetail";
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
    $("#hdnEmployeeLeaveId").val("0");
    $("#ddlLeaveType").val("0");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployee").val("");
    $("#txtLeaveDate").val("");
    $("#txtLeaveCount").val("");
    $("#chkStatus").prop("checked", true);
    $("#btnSave").show();
    $("#btnUpdate").hide();


}