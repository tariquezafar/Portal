$(document).ready(function () {  
    $("#txtSeparationOrderNo").attr('readOnly', true);

    $("#txtExitInterviewNo").attr('readOnly', true);
    $("#txtEmployeeClearanceNo").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);

    $("#txtSeparationOrderDate").attr('readOnly', true);
    $("#txtSeparationOrderDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        minDate:0,
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
            GetEmployeeInterviewClearanceDetail(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtEmployee").val("");
                $("#hdnEmployeeId").val("0");
                $("#hdnExitInterviewId").val("0");
                $("#txtExitInterviewNo").val("");
                $("#txtEmployeeClearanceNo").val("");
                $("#hdnEmployeeClearanceId").val("0");


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
    var hdnSeparationOrderId = $("#hdnSeparationOrderId");
    if (hdnSeparationOrderId.val() != "" && hdnSeparationOrderId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetSeparationOrderDetail(hdnSeparationOrderId.val());
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


function GetSeparationOrderDetail(separationorderId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SeparationOrder/GetSeparationOrderDetail",
        data: { separationorderId: separationorderId },
        dataType: "json",
        success: function (data) {
            $("#txtSeparationOrderNo").val(data.SeparationOrderNo);
            $("#hdnSeparationOrderId").val(data.SeparationOrderId);
            $("#txtSeparationOrderDate").val(data.SeparationOrderDate);
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#txtEmployee").val(data.EmployeeName);

            $("#hdnExitInterviewId").val(data.ExitInterviewId);
            $("#txtExitInterviewNo").val(data.ExitInterviewNo);

            $("#txtEmployeeClearanceNo").val(data.EmployeeClearanceNo);
            $("#hdnEmployeeClearanceId").val(data.EmployeeClearanceId);
            $("#txtExperienceLetter").val(data.ExperienceLetter);
            $("#txtSeparationRemarks").val(data.SepartionRemarks);
            $("#ddlSeparationStatus").val(data.SeparationStatus);
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
function GetEmployeeInterviewClearanceDetail(employeeId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SeparationOrder/GetEmployeeInterviewClearanceDetail",
        data: { employeeId: employeeId },
        dataType: "json",
        success: function (data) {
            $("#hdnExitInterviewId").val(data.ExitInterviewId);
            $("#txtExitInterviewNo").val(data.ExitInterviewNo);

            $("#txtEmployeeClearanceNo").val(data.EmployeeClearanceNo);
            $("#hdnEmployeeClearanceId").val(data.EmployeeClearanceId);
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() {
    var txtSeparationOrderNo = $("#txtSeparationOrderNo");
    var hdnSeparationOrderId = $("#hdnSeparationOrderId");
    var txtSeparationOrderDate = $("#txtSeparationOrderDate");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var txtEmployee = $("#txtEmployee");
    var hdnExitInterviewId = $("#hdnExitInterviewId");
    var txtExitInterviewNo = $("#txtExitInterviewNo");
    var txtEmployeeClearanceNo = $("#txtEmployeeClearanceNo");
    var hdnEmployeeClearanceId = $("#hdnEmployeeClearanceId");

    var txtExperienceLetter = $("#txtExperienceLetter");
    var txtSeparationRemarks = $("#txtSeparationRemarks");

    var ddlSeparationStatus = $("#ddlSeparationStatus");
    
    if (txtSeparationOrderDate.val() == "") {
        ShowModel("Alert", "Please select Separation Order Date")
        return false;  
    } 
    if (txtEmployee.val() == "" || txtEmployee.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false; 
    } 

    if (hdnEmployeeId.val() == "" || hdnEmployeeId.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false;
    }

    if (hdnExitInterviewId.val() == "" || hdnExitInterviewId.val() == "0") {
        ShowModel("Alert", "Exit Interview of Employee is pending")
        return false;
    }
    if (hdnEmployeeClearanceId.val() == "" || hdnEmployeeClearanceId.val() == "0") {
        ShowModel("Alert", "Employee Clearance is pending")
        return false;
    } 
    if (txtSeparationRemarks.val().trim() == "") {
        ShowModel("Alert", "Please Enter Separation Remarks")
        return false;
    }
    if (ddlSeparationStatus.val() == "" || ddlSeparationStatus.val() == "0") {
        ShowModel("Alert", "Please select Separation Status")
        return false;
    }


    var separationorderViewModel = {
        SeparationOrderId: hdnSeparationOrderId.val(),
        SeparationOrderNo: txtSeparationOrderNo.val().trim(),
        SeparationOrderDate: txtSeparationOrderDate.val(),
        EmployeeId: hdnEmployeeId.val(),
        ExitInterviewId: hdnExitInterviewId.val(),
        EmployeeClearanceId: hdnEmployeeClearanceId.val(),
        ExperienceLetter: txtExperienceLetter.val(),
        SepartionRemarks: txtSeparationRemarks.val(), 
        SeparationStatus: ddlSeparationStatus.val()
    }; 
    var accessMode = 1;//Add Mode
    if (hdnSeparationOrderId.val() != null && hdnSeparationOrderId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { separationorderViewModel: separationorderViewModel };
    $.ajax({
        url: "../SeparationOrder/AddEditSeparationOrder?accessMode=" + accessMode + "",
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
                      window.location.href = "../SeparationOrder/ListSeparationOrder";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../SeparationOrder/AddEditSeparationOrder";
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
    $("#txtSeparationOrderNo").val("");
    $("#hdnSeparationOrderId").val("0");
    $("#txtSeparationOrderDate").val("");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployee").val("");

    $("#hdnExitInterviewId").val("0");
    $("#txtExitInterviewNo").val("");
    $("#txtEmployeeClearanceNo").val("");
    $("#hdnEmployeeClearanceId").val("0");

    $("#txtExperienceLetter").val("");
    $("#txtSeparationRemarks").val(""); 
    $("#ddlSeparationStatus").val("Draft");

    $("#btnSave").show();
    $("#btnUpdate").hide();


}
 
 
 
 
 