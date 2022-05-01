
$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtApplicationDate,#txtAdvanceStartDate,#txtAdvanceEndDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var hdnEmployeeId = $("#hdnEmployeeId");
    if (hdnEssEmployeeId.val() != "0") {
        $("#txtEmployee").attr('readOnly', true);
        $("#txtEmployee").val(hdnEssEmployeeName.val());
        $("#hdnEmployeeId").val(hdnEssEmployeeId.val());
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
            $("#hdnEssEmployeeId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtEmployee").val("");
                $("#hdnEmployeeId").val("0");
                $("#hdnEssEmployeeId").val("0");
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
    BindTravelTypeList();
    //SearchEmployeeTravelApp();
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
function BindTravelTypeList() {
    $.ajax({
        type: "GET",
        url: "../EmployeeTravelApp/GetTravelTypeForEmpolyeeTravelAppList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlTravelType").append($("<option></option>").val(0).html("-Select Travel Type-"));
            $.each(data, function (i, item) {

                $("#ddlTravelType").append($("<option></option>").val(item.TravelTypeId).html(item.TravelTypeName));
            });
        },
        error: function (Result) {
            $("#ddlTravelType").append($("<option></option>").val(0).html("-Select Travel Type-"));
        }
    });
}

function ClearFields() {
    //$("#txtApplicationNo").val("");
    //$("#hdnApplicationId").val("0");
    //$("#txtApplicationDate").val("");
    //$("#ddlTravelType").val("0"); 
    //$("#txtTravelDestination").val("");
    //$("#ddlTravelStatus").val("0"); 
    //$("#ddlApprovalStatus").val("0");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    window.location.href = "../EmployeeTravelApp/ListEmployeeTravelApp";
}
function SearchEmployeeTravelApp() {
    var txtApplicationNo = $("#txtApplicationNo");
    var hdnApplicationId = $("#hdnApplicationId");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var txtEmployee = $("#txtEmployee");
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var txtTravelDestination = $("#txtTravelDestination");
    var ddlTravelType = $("#ddlTravelType");
    var ddlTravelStatus = $("#ddlTravelStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch=$("#ddlCompanyBranch");
    var requestData = {
        applicationNo: txtApplicationNo.val().trim(), employeeId: hdnEmployeeId.val(), travelDestination:txtTravelDestination.val(), travelTypeId: ddlTravelType.val(),
        travelStatus: ddlTravelStatus.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(),
        essEmployeeId: hdnEssEmployeeId.val(), essEmployeeName: hdnEssEmployeeName.val(),companyBranch: ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../EmployeeTravelApp/GetEmployeeTravelAppList",
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