$(document).ready(function () {
   
    $("#txtEmployeeClearanceNo").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    
    BindClearanceTemplateList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnEmployeeClearanceId = $("#hdnEmployeeClearanceId");
    if (hdnEmployeeClearanceId.val() != "" && hdnEmployeeClearanceId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeClearanceProcessDetail(hdnEmployeeClearanceId.val());
       }, 2000);
         

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
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
    var employeeClearanceProcesses = [];
    GetEmployeeClearanceProcesses(employeeClearanceProcesses);
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
function GetClearanceProcessDetailList() {
    var ddlClearanceTemplate = $("#ddlClearanceTemplate");
    var requestData = { clearancetemplateId: ddlClearanceTemplate.val()};
    $.ajax({
        url: "../EmployeeClearanceProcess/GetClearanceProcessList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divClearanceProcessList").html("");
            $("#divClearanceProcessList").html(err);
        },
        success: function (data) {
            $("#divClearanceProcessList").html("");
            $("#divClearanceProcessList").html(data);
        }
    });
}

function GetEmployeeClearanceProcesses(employeeClearanceProcesses) {
    var hdnEmployeeClearanceId = $("#hdnEmployeeClearanceId");
    var requestData = { employeeClearanceProcesses: employeeClearanceProcesses, employeeClearanceId: hdnEmployeeClearanceId.val() };
    $.ajax({
        url: "../EmployeeClearanceProcess/GetEmployeeClearanceProcesses",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divClearanceProcessList").html("");
            $("#divClearanceProcessList").html(err);
        },
        success: function (data) {
            $("#divClearanceProcessList").html("");
            $("#divClearanceProcessList").html(data);
        }
    });
}




 
function BindClearanceTemplateList() {
    $.ajax({
        type: "GET",
        url: "../EmployeeClearanceProcess/GetClearanceTemplateList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlClearanceTemplate").append($("<option></option>").val(0).html("Select Clearance Template"));
            $.each(data, function (i, item) {
                $("#ddlClearanceTemplate").append($("<option></option>").val(item.ClearanceTemplateId).html(item.ClearanceTemplateName));
            });
        },
        error: function (Result) {
            $("#ddlClearanceTemplate").append($("<option></option>").val(0).html("Select Clearance Template"));
        }
    });
}

function GetEmployeeClearanceProcessDetail(employeeClearanceId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeClearanceProcess/GetEmployeeClearanceProcessDetail",
        data: { employeeClearanceId: employeeClearanceId },
        dataType: "json",
        success: function (data) {


            $("#txtEmployeeClearanceNo").val(data.EmployeeClaaranceNo);
            $("#hdnEmployeeClearanceId").val(data.EmployeeClearanceId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#ddlClearanceTemplate").val(data.ClearanceTemplateId);

            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }

            $("#btnAddNew").show();
            
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

    
}
function SaveData() {
    
    var hdnEmployeeClearanceId = $("#hdnEmployeeClearanceId");
    var txtEmployee = $("#txtEmployee");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var ddlClearanceTemplate = $("#ddlClearanceTemplate");
    
    if (txtEmployee.val().trim() == "") {
        ShowModel("Alert", "Please Enter Employee Name")
        return false;
    }
    if (hdnEmployeeId.val() == "" || hdnEmployeeId.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false;
    }
    if (ddlClearanceTemplate.val() == "" || ddlClearanceTemplate.val() == "0") {
        ShowModel("Alert", "Please select Clearance Template")
        return false;
    } 
    var employeeClearanceProcessViewModel = {
        EmployeeClearanceId: hdnEmployeeClearanceId.val(),
        EmployeeId: hdnEmployeeId.val(),
        ClearanceTemplateId: ddlClearanceTemplate.val(),
        ClearanceFinalStatus: "Pending"
       
    }; 
     

    var employeeClearanceProcessDetailList = [];
    $('#tblClearanceTemplate tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var employeeClearanceDetailId = $row.find("#hdnEmployeeClearanceDetailId").val();
        var separationClearListId = $row.find("#hdnSeparationClearListId").val();
        var clearanceByUserId = $row.find("#hdnClearanceByUserId").val();

        
        if (employeeClearanceDetailId != undefined) {
            var employeeClearanceProcessDetail= {
                EmployeeClearanceDetailId: employeeClearanceDetailId,
                SeparationClearListId: separationClearListId,
                ClearanceByUserId: clearanceByUserId
            };
            employeeClearanceProcessDetailList.push(employeeClearanceProcessDetail);
        } 
    });

    var accessMode = 1;//Add Mode
    if (hdnEmployeeClearanceId.val() != null && hdnEmployeeClearanceId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { employeeClearanceProcess: employeeClearanceProcessViewModel, employeeClearanceProcessDetails: employeeClearanceProcessDetailList };
    $.ajax({
        url: "../EmployeeClearanceProcess/AddEditEmployeeClearanceProcess?accessMode=" + accessMode + "",
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
                      window.location.href = "../EmployeeClearanceProcess/ListEmployeeClearanceProcess";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../EmployeeClearanceProcess/AddEditEmployeeClearanceProcess";
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
    $("#txtEmployeeClearanceNo").val("");
    $("#hdnEmployeeClearanceId").val("0");
    $("#txtEmployee").val("");
    $("#hdnEmployeeId").val("0");
    $("#ddlClearanceTemplate").val("0");
    $("#btnSave").show();
    $("#btnUpdate").hide(); 
}
 

 