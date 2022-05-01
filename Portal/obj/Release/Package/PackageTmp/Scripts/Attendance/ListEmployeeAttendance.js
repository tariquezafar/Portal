
$(document).ready(function () {
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");

    SearchEmployeeMarkAttendance();
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    }); 
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
}
function SearchEmployeeMarkAttendance() {
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var requestData = {
        employeeId: hdnEssEmployeeId.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(),
        essEmployeeId: hdnEssEmployeeId.val(), essEmployeeName: hdnEssEmployeeName.val()
    };
    $.ajax({
        url: "../Attendance/GetEmployeeMarkAttendanceList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divEmployeeAttendanceList").html("");
            $("#divEmployeeAttendanceList").html(err);
        },
        success: function (data) {
            $("#divEmployeeAttendanceList").html("");
            $("#divEmployeeAttendanceList").html(data);
        }
    });
}
function Export() {
    var divList = $("#divEmployeeAttendanceList");
    ExporttoExcel(divList);
}