
$(document).ready(function () {   
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEmployeeName = $("#hdnEmployeeName");
    $("#txtCustomerName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Quotation/GetCustomerAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.CustomerName, value: item.CustomerId, primaryAddress: item.PrimaryAddress, code: item.CustomerCode, gSTNo: item.GSTNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtCustomerName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtCustomerName").val(ui.item.label);
            $("#hdnCustomerId").val(ui.item.value);
            // $("#txtCustomerCode").val(ui.item.code);
            BindCustomerBranchList(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtCustomerName").val("");
                $("#hdnCustomerId").val("0");
                //  $("#txtCustomerCode").val("");
                ShowModel("Alert", "Please select Customer from List");
            }
            return false;
        }

    })
    $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Customer Site-"));
    //SearchEmployeeMarkAttendance();
    $("#txtAttendanceDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtAttendanceDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: 0,
        onSelect: function (selected) {

        }
    });
    //dateFormat: '',
    //timeFormat: 'hh:mm tt'

    BindDepartmentList();
    $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
   // SearchEmployeeAttendance();
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

    //Get Logged In Employee Role and populate Employment type dropdown based on it
    var loggedInUserRoleId = checkLoggedInUserRole();
    if (loggedInUserRoleId != null && loggedInUserRoleId == 74) {
        $("#ddlEmploymentType").find('option:contains("Regular")').hide();
        //$("#ddlEmploymentType").find('option:contains("Workers")').hide();
        //$("#ddlEmploymentType").find('option:contains("Consultant")').hide();
        $("#ddlEmploymentType").val("Temporary");
    }
    else {
        $("#ddlEmploymentType").find('option:contains("Temporary")').hide();
        $("#ddlEmploymentType").find('option:contains("Local")').hide();
        $("#ddlEmploymentType").find('option:contains("Workers")').hide();
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

function ClearFields() {
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
}
function SearchEmployeeAttendance() {
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEmployeeName = $("#hdnEmployeeName");
    var ddlEmploymentType = $("#ddlEmploymentType");
    var txtAttendanceDate = $("#txtAttendanceDate");
    var ddlDepartment = $("#ddlDepartment");
    var ddlDesignation = $("#ddlDesignation");
    if (ddlEmploymentType.val() == "Temporary" || ddlEmploymentType.val() == "Workers" || ddlEmploymentType.val() == "Local")
    {
        if ($("#txtCustomerName").val() == "")
        {
            ShowModel('Alert', 'Please enter customer name');
            $("#txtCustomerName").focus();
            return false;
        }
        if ($("#ddlCustomerBranch").val() == "0") {
            ShowModel('Alert', 'Please select customer site');
            $("#ddlCustomerBranch").focus();
            return false;
        }
    }
    var requestData = {
        employeeId: hdnEmployeeId.val(),
        attendanceDate: txtAttendanceDate.val(),
        employeeType: ddlEmploymentType.val(),
        departmentId: ddlDepartment.val(),
        designationId: ddlDesignation.val()
    };
    $.ajax({
        url: "../EmployeeAttendance/GetEmployeeAttendanceFormList",
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

function SearchTempEmployeeAttendance() {
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEmployeeName = $("#hdnEmployeeName");
    var ddlEmploymentType = $("#ddlEmploymentType");
    var txtAttendanceDate = $("#txtAttendanceDate");
    var ddlDepartment = $("#ddlDepartment");
    var ddlDesignation = $("#ddlDesignation");
    if (ddlEmploymentType.val() == "Temporary" || ddlEmploymentType.val() == "Workers" || ddlEmploymentType.val() == "Local") {
        if ($("#txtCustomerName").val() == "") {
            ShowModel('Alert', 'Please enter customer name');
            $("#txtCustomerName").focus();
            return false;
        }
        if ($("#ddlCustomerBranch").val() == "0") {
            ShowModel('Alert', 'Please select customer site');
            $("#ddlCustomerBranch").focus();
            return false;
        }
    }
    var requestData = {
        employeeId: hdnEmployeeId.val(), attendanceDate: txtAttendanceDate.val(), employeeType: ddlEmploymentType.val(),
        departmentId: ddlDepartment.val(), designationId: ddlDesignation.val()
    };
    $.ajax({
        url: "../EmployeeAttendance/GetTempEmployeeAttendanceFormList",
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
    var employeeAttendanceList = [];
    var hdnCustomerBranchId = $("#hdnCustomerBranchId");
    $('.table-hover tr').not(':first').each(function (i, row) {
        var $row = $(row);
        var AttendanceStatus = "";
        var employeeId = $row.find("#hdnEmployeeId");
        var attendanceDate = $row.find("#hdnAttendanceDate");
        var ddlApproveReject = $row.find("#ddlApproveReject");
        var txtInTime = $row.find("#txtInTime");
        var hdnInTime = $row.find("#hdnInTime");
        var txtOutTime = $row.find("#txtOutTime");
        var hdnOutTime = $row.find("#hdnOutTime");
        var ddlAbsentPresent = $row.find("#ddlAbsentPresent");

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
            var employeeAttendance = {
                EmployeeId: employeeId.val(),
                AttendanceDate: attendanceDate.val(),
                AttendanceStatus: AttendanceStatus,
                InTime: txtInTime.val(),
                OutTime: txtOutTime.val(),
                PresentAbsent: ddlAbsentPresent.val()
            };
            employeeAttendanceList.push(employeeAttendance);
        }
    });
    var requestData = { employeeAttendanceList: employeeAttendanceList };
    $.ajax({
        url: "../EmployeeAttendance/AddEditEmployeeAttendanceFormByEmployer",
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

function BindCustomerBranchList(customerBranchId) {
    var customerId = $("#hdnCustomerId").val();
    $("#ddlCustomerBranch").val(0);
    $("#ddlCustomerBranch").html("");
    if (customerId != undefined && customerId != "" && customerId != "0") {
        var data = { customerId: customerId };
        $.ajax({
            type: "GET",
            url: "../SO/GetCustomerBranchList",
            data: data,
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Customer Site-"));
                $.each(data, function (i, item) {
                    $("#ddlCustomerBranch").append($("<option></option>").val(item.CustomerBranchId).html(item.BranchName));
                });
                // $("#ddlCustomerBranch option:selected").val(customerBranchId);
            },
            error: function (Result) {
                $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
            }
        });
    }
    else {
        $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
    }
}
function GetCustomerBranchListByID(customerId, customerBranchId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Project/GetCustomerBranchListByID",
        data: { customerId: customerId },
        dataType: "json",
        success: function (data) {

            $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Customer Site-"));
            $.each(data, function (i, item) {

                $("#ddlCustomerBranch").append($("<option></option>").val(item.CustomerBranchId).html(item.BranchName));
            });
            if (customerBranchId != null && customerBranchId != "") {
                $("#ddlCustomerBranch").val(customerBranchId);
            }
        },
        error: function (Result) {
            $("#ddlCustomerType").append($("<option></option>").val(0).html("-Select Customer Branch-"));
        }
    });


}

$('body').on('change', '#ddlCustomerBranch', function () {
    $('#hdnCustomerBranchId').val($('#ddlCustomerBranch').val());
});

$('body').on('change', '#ddlEmploymentType', function () {
    if ($("#ddlEmploymentType").val() == "Temporary" || $("#ddlEmploymentType").val() == "Workers" || $("#ddlEmploymentType").val() == "Local") {
        $("#spnCustomerName").css('display', '');
        $("#spnCustomerSite").css('display', '');
    }
    else {
        $("#spnCustomerName").css('display', 'none');
        $("#spnCustomerSite").css('display', 'none');
    }
});

$(document).ready(function () {
    if ($("#ddlEmploymentType").val() == "Temporary" || $("#ddlEmploymentType").val() == "Workers" || $("#ddlEmploymentType").val() == "Local") {
        $("#spnCustomerName").css('display', '');
        $("#spnCustomerSite").css('display', '');
    }
    else {
        $("#spnCustomerName").css('display', 'none');
        $("#spnCustomerSite").css('display', 'none');
    }
});

function checkLoggedInUserRole()
{
    var loggedInUserRoleId = 0;
    if ($("#hdnLoggedInUserRoleId").val() != 0) {
        loggedInUserRoleId = $("#hdnLoggedInUserRoleId").val();
    }
    else {
        loggedInUserRoleId = 0;
    }
    return loggedInUserRoleId;
}

function RoleBasedAttendanceMark()
{
    var loggedInUserEmployeeRoleId = 0;
    if ($("#hdnLoggedInUserRoleId").val() != 0) {
        loggedInUserEmployeeRoleId = $("#hdnLoggedInUserRoleId").val();
    }
    else {
        loggedInUserEmployeeRoleId = 0;
    }

    if (loggedInUserEmployeeRoleId != null && loggedInUserEmployeeRoleId == 74) {
        SearchTempEmployeeAttendance()
    }
    else {
        SearchEmployeeAttendance();
    }
}