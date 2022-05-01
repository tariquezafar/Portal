$(document).ready(function () {
    $("#txtApplicationNo").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);

    $("#txtApplicationDate").attr('readOnly', true);
    $("#txtHireByDate").attr('readOnly', true);

    $("#txtApplicationDate,#txtAdvanceStartDate,#txtAdvanceEndDate").datepicker({
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
    BindAdvanceTypeList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnApplicationId.val() != "" && hdnApplicationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeAdvanceAppDetail(hdnApplicationId.val());
       }, 2000);



        if (hdnAccessMode.val() == "3") { 
            $("#btnUpdate").hide(); 
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);


        }
        else {
            
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);

            $("#ddlAdvanceStatus").attr('disabled', false);
            $("#txtRejectedReason").attr('disabled', true);
            $("#txtRejectedReason").attr('readOnly', false);

            $("#btnUpdate").show();
        }
    }
    else {
         
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

function BindAdvanceTypeList() {
    $.ajax({
        type: "GET",
        url: "../EmployeeAdvanceApp/GetAdvanceTypeForEmpolyeeAdvanceAppList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlAdvanceType").append($("<option></option>").val(0).html("-Select Advance Type-"));
            $.each(data, function (i, item) {

                $("#ddlAdvanceType").append($("<option></option>").val(item.AdvanceTypeId).html(item.AdvanceTypeName));
            });
        },
        error: function (Result) {
            $("#ddlAdvanceType").append($("<option></option>").val(0).html("-Select Advance Type-"));
        }
    });
}

function GetEmployeeAdvanceAppDetail(applicationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeAdvanceApp/GetEmployeeAdvanceAppDetail",
        data: { applicationId: applicationId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicationNo").val(data.ApplicationNo);
            $("#hdnApplicationId").val(data.ApplicationId);
            $("#txtApplicationDate").val(data.ApplicationDate);
            $("#ddlAdvanceType").val(data.AdvanceTypeId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#txtAdvanceAmount").val(data.AdvanceAmount);
            $("#txtAdvanceStartDate").val(data.AdvanceStartDate);
            $("#txtAdvanceEndDate").val(data.AdvanceEndDate);
            $("#txtAdvanceInstallmentAmount").val(data.AdvanceInstallmentAmount);
            $("#txtAdvanceReason").val(data.AdvanceReason);
            $("#ddlAdvanceStatus").val(data.AdvanceStatus);
            if (data.RejectReason != "") {
                $("#divReject").show();
                $("#txtRejectedReason").val(data.RejectReason);
                $("#txtRejectedDate").val(data.RejectDate);
            }
            $("#btnAddNew").show();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() { 
    var hdnApplicationId = $("#hdnApplicationId");
    var ddlAdvanceStatus = $("#ddlAdvanceStatus");
    var txtRejectedReason = $("#txtRejectedReason");
    if (ddlAdvanceStatus.val() == "0" || ddlAdvanceStatus.val() == "") {
        ShowModel("Alert", "Please select Approved/ Rejdcted")
        ddlAdvanceStatus.focus();
        return false;
    } 

    if (txtRejectedReason.val() == "Rejected" && txtRejectedReason.val() == "") {
        ShowModel("Alert", "Please Enter Rejection Reason")
        txtRejectedReason.focus();
        return false;
    }
    var employeeadvanceapplicationViewModel = {
        ApplicationId: hdnApplicationId.val(),
        AdvanceStatus: ddlAdvanceStatus.val(),
        RejectReason: txtRejectedReason.val().trim()
    };
    var accessMode = 1;//Add Mode
    if (hdnApplicationId.val() != null && hdnApplicationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { employeeadvanceApplicationViewModel: employeeadvanceapplicationViewModel };
    $.ajax({
        url: "../EmployeeAdvanceApp/ApproveEmployeeAdvanceApp?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../EmployeeAdvanceApp/ListEmployeeAdvanceAppApproval";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../EmployeeAdvanceApp/ApproveEmployeeAdvanceApp";
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
    $("#txtApplicationNo").val("");
    $("#hdnApplicationId").val("0");
    $("#txtApplicationDate").val("");
    $("#ddlAdvanceType").val("0");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployee").val("");
    $("#txtAdvanceAmount").val("");
    $("#txtAdvanceStatDate").val("");
    $("#txtAdvanceEndDate").val("");
    $("#txtAdvanceInstallmentAmount").val("");
    $("#txtAdvanceReason").val("");
    $("#ddlAdvanceStatus").val("");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}


function EnableDisableRejectReason() {
    var approvalStatus = $("#ddlAdvanceStatus option:selected").text();
    if (approvalStatus == "Rejected") {
        $("#txtRejectedReason").attr("disabled", false);
    }
    else {
        $("#txtRejectedReason").attr("disabled", true);
        $("#txtRejectedReason").val("");
    }
}

