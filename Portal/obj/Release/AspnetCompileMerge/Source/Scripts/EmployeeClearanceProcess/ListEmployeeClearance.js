
$(document).ready(function () {
    
    BindClearanceTemplateList();
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
    //SearchEmployeeClearanceProcess();
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


function ClearFields() {
    $("#txtEmployeeClearanceNo").val("");
    $("#txtEmployee").val("");
    $("#hdnEmployeeId").val("0");
    $("#ddlClearanceTemplate").val("0");
 
    
}
function SearchEmployeeClearanceProcess() {
    var txtEmployeeClearanceNo = $("#txtEmployeeClearanceNo");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var ddlClearanceTemplate = $("#ddlClearanceTemplate");
  
    

    var requestData = {
        employeeClearanceNo: txtEmployeeClearanceNo.val().trim(),
        employeeId: hdnEmployeeId.val(),
        clearanceTemplateId: ddlClearanceTemplate.val()
    };
    $.ajax({
        url: "../EmployeeClearanceProcess/GetEmployeeClearanceList",
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
function Export() {
    var divList = $("#divList");
    ExporttoExcel(divList);
}
