$(document).ready(function () {
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");
    BindFinYearList(hdnCurrentFinyearId.val());

    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        onSelect: function (selected) {
        }
    });

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

    BindLeaveTypeList();
 
    
  
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

function ClearFields() {
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
    $("#txtEmployeeName").val("");
    $("#ddlLeaveType").val("0");
    $("#ddlStatus").val("");
}
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
function BindFinYearList(selectedFinYear) {
    $.ajax({
        type: "GET",
        url: "../Product/GetFinYearList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $.each(data, function (i, item) {
                $("#ddlFinYear").append($("<option></option>").val(item.FinYearId).html(item.FinYearDesc));
            });
            $("#ddlFinYear").val(selectedFinYear);
        },
        error: function (Result) {

        }
    });
}

function SearchEmployeeLeaveDetail() {
    var hdnEmployeeId = $("#hdnEmployeeId");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate"); 
    var ddlLeaveType = $("#ddlLeaveType");
    var ddlStatus = $("#ddlStatus");
    var requestData = {  
        employeeId: hdnEmployeeId.val(),
        leaveTypeId: ddlLeaveType.val(),
        fromDate: txtFromDate.val(),
        toDate: txtToDate.val(),
        status: ddlStatus.val()
    }; 
    $.ajax({
        url: "../EmployeeLeaveDetail/GetEmployeeLeaveDetailList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divList").html("");
            $("#divList").html(err);
        },
        success: function (data) {
            $("#divList").html("");
            $("#divList").html(data); 
        }
    });
}
