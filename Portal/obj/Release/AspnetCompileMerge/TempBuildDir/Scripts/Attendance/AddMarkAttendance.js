$(document).ready(function () {

    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnAttendanceId = $("#hdnAttendanceId");
    var hdnAttendanceDate = $("#hdnAttendanceDate");
    $("#txtAttendanceDate").attr('readOnly', true);
    if (hdnEssEmployeeId.val() != "0") {
        $("#txtEmployee").attr('readOnly', true);
        $("#txtEmployee").val(hdnEssEmployeeName.val());
        $("#hdnEmployeeId").val(hdnEssEmployeeId.val());
    }

    $("#txtAttendanceDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        minDate: 0,
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
    var hdnAttendanceId = $("#hdnAttendanceId");
    if (hdnAttendanceId.val() != "" && hdnAttendanceId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {

        if (hdnAccessMode.val() == "3") {
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
        }
        else {
            $("#btnIn").hide();
            $("#btnOut").show();
        }
    }
    else {
        $("#btnIn").show();
        $("#btnOut").show();
    }
    GetEmployeeInOutDetails();

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


function SaveData(InOut) {
    var InOutValue;
    if (InOut == 1) {
        InOutValue = "IN";
    }
    else {
        InOutValue = "OUT";
    }
    var hdnAttendanceId = $("#hdnAttendanceId");
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var txtAttendanceDate = $("#txtAttendanceDate");
    var txtEmployee = $("#txtEmployee");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var EmployeeId;
    if (hdnEssEmployeeId.val() != "0") {
        EmployeeId = hdnEssEmployeeId.val();
    }
    else {
        EmployeeId = hdnEmployeeId.val();
    }
    if (txtAttendanceDate.val() == "") {
        ShowModel("Alert", "Please select Attendance Date")
        return false;
    }
    if (txtEmployee.val() == "" || txtEmployee.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false;
    }

    if (txtEmployee.val().trim() == "") {
        ShowModel("Alert", "Please Enter Employee Name")
        txtEmployee.focus();
        return false;
    }

    var employeeMarkAttendanceViewModel = {
        EmployeeAttendanceId: hdnAttendanceId.val(),
        AttendanceDate: txtAttendanceDate.val(),
        EmployeeId: EmployeeId,
        PresentAbsent: "P",
        InOut: InOutValue,
        AttendanceStatus : "Pending"
    };


    var requestData = { employeeMarkAttendanceViewModel: employeeMarkAttendanceViewModel };
    $.ajax({
        url: "../Attendance/AddEditEmployeeMarkAttendance",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
                function () {
                    window.location.href = "../Attendance/MarkAttendance?employeeAttendanceId=" + data.trnId + "&essEmployeeId=" + hdnEssEmployeeId.val() + "&essEmployeeName=" + hdnEssEmployeeName.val() + "&AccessMode=2";
                }, 2000);
                
                GetEmployeeInOutDetails();
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

function GetEmployeeInOutDetails() {
    var attendanceDate = $("#hdnAttendanceDate").val();
    var employeeId = $("#hdnEssEmployeeId").val();
    var requestData = { attendanceDate: attendanceDate, employeeId: employeeId };
    $.ajax({
        url: "../Attendance/GetEmployeeInOutDetails",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data != null) {
                if (data.length == 0) {
                    $("#btnIn").show();
                    $("#btnOut").hide();
                    $("#txtInTime").val("");
                    $("#txtOutTime").val("");
                }
                if (data.length > 0) {
                    $.each(data, function (i, item) {
                        if (item.InOut == "IN") {
                            $("#btnIn").hide();
                            $("#btnOut").show();
                            $("#txtInTime").val(item.TrnDateTime);
                        }
                        if (item.InOut == "OUT") {
                            $("#btnIn").hide();
                            $("#btnOut").hide();
                            $("#txtOutTime").val(item.TrnDateTime);
                        }
                    });
                }
            }
            else {
                ShowModel("No Data", "No Data Found")
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
    $("#txtEmployee").val("");
    $("#hdnAttendanceId").val("0");
    $("#ddlAdvanceType").val("0");
    $("#txtAdvanceAmount").val("");
    $("#txtAdvanceInstallmentAmount").val("");
    $("#txtAdvanceReason").val("");
    $("#ddlAdvanceStatus").val("Final");
    $("#btnIn").hide();
    $("#btnOut").hide();
}




