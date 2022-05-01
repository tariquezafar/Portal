
$(document).ready(function () { 
    $("#txtApplicationDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    var ddlCompanyBranch = $("#ddlCompanyBranch");
    $("#txtEmployee").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, departmentId: $("#txtEmployee").val(), companyBranchId: ddlCompanyBranch.val()
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
    BindSaparationCategoryList();
    BindCompanyBranchList();
   // SearchSeparationApplication();
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
function BindSaparationCategoryList() {
    $.ajax({
        type: "GET",
        url: "../SeparationApplication/GetSeparationCategoryForSeparationApplicationList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSeparationCategory").append($("<option></option>").val(0).html("-Select Separation Category-"));
            $.each(data, function (i, item) {

                $("#ddlSeparationCategory").append($("<option></option>").val(item.SeparationCategoryId).html(item.SeparationCategoryName));
            });
        },
        error: function (Result) {
            $("#ddlSeparationCategory").append($("<option></option>").val(0).html("-Select Separation Category-"));
        }
    });
}
function ClearFields() {
    $("#txtApplicationNo").val("");
    $("#hdnApplicationId").val("0");
    $("#txtApplicationDate").val("");
    $("#ddlSeparationCategory").val("0");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployee").val("");  
    $("#ddlApplicationStatus").val("0"); 
    $("#ddlApprovalStatus").val("0");
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
    $("#ddlCompanyBranch").val("0");
}
function SearchSeparationApplication() {
    var txtApplicationNo = $("#txtApplicationNo");
    var hdnApplicationId = $("#hdnApplicationId");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var txtEmployee = $("#txtEmployee"); 
    var ddlSeparationCategory = $("#ddlSeparationCategory");
    var ddlApplicationStatus = $("#ddlApplicationStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = {
        applicationNo: txtApplicationNo.val().trim(), employeeId: hdnEmployeeId.val(), separationcategoryId: ddlSeparationCategory.val(),
        applicationStatus: ddlApplicationStatus.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), companyBranchId: ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../SeparationApplication/GetSeparationApplicationList",
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