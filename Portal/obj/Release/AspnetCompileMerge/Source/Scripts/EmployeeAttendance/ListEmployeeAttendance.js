
$(document).ready(function () {
    BindCompanyBranchList();
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEmployeeName = $("#hdnEmployeeName");

    //SearchEmployeeMarkAttendance();
    $("#txtAttendanceDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtAttendanceDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    BindDepartmentList();
    $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
    //SearchEmployeeAttendance();
    $(document).on('change','#ddlAllApproveReject',function(){
        $('#ddlApproveReject').val(0);
        if ($('#ddlAllApproveReject').val() == "Approved") {
            $('.dropApproveReject').val("Approved");
        }
        else if ($('#ddlAllApproveReject').val() == "Rejected") {
            $('.dropApproveReject').val("Rejected");
        }
        else {
            $('.dropApproveReject').val(0);
        }
    });
});



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
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    window.location.href = "../EmployeeAttendance/ListEmployeeAttendance";
}
function SearchEmployeeAttendance() {
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEmployeeName = $("#hdnEmployeeName");
    var txtAttendanceDate = $("#txtAttendanceDate");
    var ddlDepartment = $("#ddlDepartment");
    var ddlDesignation = $("#ddlDesignation");
    var ddlCompanyBranch=$("#ddlCompanyBranch");
    var requestData = {
        employeeId: hdnEmployeeId.val(), attendanceDate: txtAttendanceDate.val(),
        departmentId: ddlDepartment.val(), designationId: ddlDesignation.val(),companyBranch: ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../EmployeeAttendance/GetEmployeeAttendanceList",
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
function BindDepartmentList() {
    $.ajax({
        type: "GET",
        url: "../Employee/GetDepartmentList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlDepartment").append($("<option></option>").val(0).html("-Select Department-"));
            $.each(data, function (i, item) {

                $("#ddlDepartment").append($("<option></option>").val(item.DepartmentId).html(item.DepartmentName));
            });
        },
        error: function (Result) {
            $("#ddlDepartment").append($("<option></option>").val(0).html("-Select Department-"));
        }
    });
}
function BindDesignationList(designationId) {
    var departmentId = $("#ddlDepartment option:selected").val();
    $("#ddlDesignation").val(0);
    $("#ddlDesignation").html("");
    if (departmentId != undefined && departmentId != "" && departmentId != "0") {
        var data = { departmentId: departmentId };
        $.ajax({
            type: "GET",
            url: "../Employee/GetDesignationList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
                $.each(data, function (i, item) {
                    $("#ddlDesignation").append($("<option></option>").val(item.DesignationId).html(item.DesignationName));
                });
                $("#ddlDesignation").val(designationId);
            },
            error: function (Result) {
                $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
            }
        });
    }
    else {

        $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
    }
}

function SaveData() {
    var approveRejectList = [];
    $('.table-hover tr').not(':first').each(function (i, row) {
        var $row = $(row);
        var AttendanceStatus = "";
        var employeeId = $row.find("#hdnEmployeeId");
        var attendanceDate = $row.find("#hdnAttendanceDate");
        var ddlApproveReject = $row.find("#ddlApproveReject");

        if (employeeId != undefined) {
            if (ddlApproveReject.val() == "Approved") {
                AttendanceStatus = "Approved";
            }
            else if (ddlApproveReject.val() == "Rejected") {
                AttendanceStatus = "Rejected";
            }
            else {
                AttendanceStatus = "Pending";
            }
            var approveReject = {
                EmployeeId: employeeId.val(),
                AttendanceDate: attendanceDate.val(),
                AttendanceStatus: AttendanceStatus
            };
            approveRejectList.push(approveReject);
        }
    });
    var requestData = { employeeAttendanceList: approveRejectList};
    $.ajax({
        url: "../EmployeeAttendance/UpdateEmployeeAttendanceByEmployer",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", "Successfully " +data.message);
                // ClearFields();
                SearchEmployeeAttendance();
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

