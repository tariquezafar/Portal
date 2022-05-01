
$(document).ready(function () {
    BindSaparationApplicationList();
    //SearchExitInterview();
    $("#txtExitInterviewDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
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

    $("#txtInterviewByUser").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Lead/GetUserAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FullName, value: item.UserName, UserId: item.UserId, MobileNo: item.MobileNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtInterviewByUser").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtInterviewByUser").val(ui.item.label);
            $("#hdnInterviewByUserId").val(ui.item.UserId);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtInterviewByUser").val("");
                $("#hdnInterviewByUserId").val("");
                ShowModel("Alert", "Please select User from List")
            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.value + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};
  
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
 

function BindSaparationApplicationList() {
    $.ajax({
        type: "GET",
        url: "../ExitInterview/GetSeparationApplicationForExitInterviewList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSeparationApplication").append($("<option></option>").val(0).html("-Select Separation Application-"));
            $.each(data, function (i, item) {

                $("#ddlSeparationApplication").append($("<option></option>").val(item.ApplicationId).html(item.ApplicationNo));
            });
        },
        error: function (Result) {
            $("#ddlSeparationApplication").append($("<option></option>").val(0).html("-Select Separation Application-"));
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



function SearchExitInterview() {  
    var txtExitInterviewNo = $("#txtExitInterviewNo");
    var hdnExitInterviewId = $("#hdnExitInterviewId");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var txtEmployee = $("#txtEmployee"); 
    var hdnInterviewByUserId = $("#hdnInterviewByUserId");
    var txtInterviewByUser = $("#txtInterviewByUser");
    var ddlSeparationApplication = $("#ddlSeparationApplication");
    var ddlInterviewStatus = $("#ddlInterviewStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var requestData = {
        exitinterviewNo: txtExitInterviewNo.val().trim(), employeeId: hdnEmployeeId.val(), applicationId: ddlSeparationApplication.val(),
        interviewStatus: ddlInterviewStatus.val(),interviewbyuserId:hdnInterviewByUserId.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val()
    };
    $.ajax({
        url: "../ExitInterview/GetExitInterviewList",
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
function ClearFields() {
    $("#txtExitInterviewNo").val("");
    $("#hdnExitInterviewId").val("0");
    $("#txtApplicationDate").val("");
    $("#ddlSeparationApplication").val("0");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployee").val("");
    $("#hdnInterviewByUserId").val("0");
    $("#txtInterviewByUser").val("");  
    $("#ddlInterviewStatus").val("");
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
}
function Export() {
    var divList = $("#divList");
    ExporttoExcel(divList);
}