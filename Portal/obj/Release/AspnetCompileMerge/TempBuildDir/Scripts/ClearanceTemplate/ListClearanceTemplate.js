
$(document).ready(function () { 
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
    BindSeparationCategoryList();
    BindDepartmentList();
    $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-")); 
    //SearchClearanceTemplate();
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

function BindSeparationCategoryList() {
    $.ajax({
        type: "GET",
        url: "../SeparationApplication/GetSeparationCategoryForSeparationApplicationList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSeparationCategory").append($("<option></option>").val(0).html("Select Separation Category"));
            $.each(data, function (i, item) {
                $("#ddlSeparationCategory").append($("<option></option>").val(item.SeparationCategoryId).html(item.SeparationCategoryName));
            });
        },
        error: function (Result) {
            $("#ddlSeparationCategory").append($("<option></option>").val(0).html("Select Separation Category"));
        }
    });
}
function ClearFields() {
    $("#txtClearanceTemplateName").val("");
    $("#ddlSeparationCategory").val("0");
    $("#ddlDepartment").val("0");
    $("#ddlDesignation").val("0");
    $("#ddlStatus").val("");
 
    
}
function SearchClearanceTemplate() {
    var txtClearanceTemplateName = $("#txtClearanceTemplateName");
    var ddlSeparationCategory = $("#ddlSeparationCategory");
    var ddlDepartment = $("#ddlDepartment");
    var ddlDesignation = $("#ddlDesignation"); 
    var ddlStatus = $("#ddlStatus");
    

    var requestData = {
        clearancetemplateName: txtClearanceTemplateName.val().trim(),
        separationCategory: ddlSeparationCategory.val(),
        department: ddlDepartment.val(),
        designation: ddlDesignation.val(),
        clearancetemplateStatus: ddlStatus.val()
    };
    $.ajax({
        url: "../ClearanceTemplate/GetClearanceTemplateLists",
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
