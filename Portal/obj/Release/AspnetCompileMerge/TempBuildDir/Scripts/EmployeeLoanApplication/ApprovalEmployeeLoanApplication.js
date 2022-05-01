$(document).ready(function () {  
    $("#txtApplicationNo").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);   
    $("#txtApplicationDate").attr('readOnly', true);
    $("#txtLoanStartDate").attr('readOnly', true);
    $("#txtLoanEndDate").attr('readOnly', true);
    $("#txtRejectedReason").attr("disabled", true);
    $("#txtApplicationDate,#txtLoanStartDate,#txtLoanEndDate").datepicker({
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
 
    BindEmployeeLoanApplicationTypeList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnApplicationId.val() != "" && hdnApplicationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeLoanApplicationApprovalDetail(hdnApplicationId.val());
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
 
function BindEmployeeLoanApplicationTypeList() {
    $.ajax({
        type: "GET",
        url: "../EmployeeLoanApplication/GetEmployeeLoanApplicationTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlLoanType").append($("<option></option>").val(0).html("Select Loan Type"));
            $.each(data, function (i, item) {
            $("#ddlLoanType").append($("<option></option>").val(item.LoanTypeId).html(item.LoanTypeName));
            });
        },
        error: function (Result) {
            $("#ddlLoanType").append($("<option></option>").val(0).html("Select Loan Type"));
        }
    });
}
   
function GetEmployeeLoanApplicationApprovalDetail(applicationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeLoanApplication/GetEmployeeLoanApplicationApprovalDetail",
        data: { applicationId: applicationId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicationNo").val(data.ApplicationNo);
            $("#hdnApplicationId").val(data.ApplicationId);
            $("#txtApplicationDate").val(data.ApplicationDate);
            $("#ddlLoanType").val(data.LoanTypeId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#hdnEmployeeId").val(data.EmployeeId);

            $("#txtLoanInterestRate").val(data.LoanInterestRate);
            $("#txtInterestCalcOn").val(data.InterestCalcOn);

            $("#txtLoanAmount").val(data.LoanAmount);
            $("#txtLoanStartDate").val(data.LoanStartDate);
            $("#txtLoanEndDate").val(data.LoanEndDate);
            $("#txtLoanInstallmentAmount").val(data.LoanInstallmentAmount);
            $("#txtLoanReason").val(data.LoanReason);
            $("#ddlLoanStatus").val(data.LoanStatus);
            $("#txtRejectedReason").val(data.RejectReason);
            //if (data.RejectedReason != "") { 
            //    $("#divReject").show();
            //    $("#txtRejectReason").val(data.RejectedReason);
            //    $("#txtRejectedDate").val(data.RejectedDate);
            //}


            $("#btnAddNew").show();
             
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() {
   
    var hdnApplicationId = $("#hdnApplicationId");     
    var ddlLoanStatus = $("#ddlLoanStatus");
    var txtRejectedReason = $("#txtRejectedReason");          
    if (ddlLoanStatus.val() == "0" || ddlLoanStatus.val() == undefined) {
        ShowModel("Alert", "Please Enter Loan Status")
        return false;

    }     
    if (ddlLoanStatus.val() == "Rejected")
    {   
        if (txtRejectedReason.val() == "") {
        ShowModel("Alert", "Please Enter Loan Reason")
        return false;

    }
    }
    var employeeLoanApplicationViewModel = {
        ApplicationId: hdnApplicationId.val(),     
        LoanStatus: ddlLoanStatus.val(),
        RejectReason: txtRejectedReason.val(),
    };
    var accessMode = 1;//Add Mode
    if (hdnApplicationId.val() != null && hdnApplicationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { employeeLoanApplicationViewModel: employeeLoanApplicationViewModel };
    $.ajax({
        url: "../EmployeeLoanApplication/ApproveRejectEmployeeLoanApplication?accessMode=" + accessMode + "",
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
                      window.location.href = "../EmployeeLoanApplication/ListEmployeeLoanApplicationApproval";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../EmployeeLoanApplication/ApprovalEmployeeLoanApplication";
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
    $("#ddlLoanType").val("0");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployee").val("");
    $("#txtLoanInterestRate").val("");
    $("#txtInterestCalcOn").val("");
    $("#txtLoanAmount").val(""); 
    $("#txtLoanStartDate").val("");
    $("#txtLoanEndDate").val("");
    $("#txtLoanInstallmentAmount").val("");
    $("#txtLoanReason").val("");
    $("#ddlLoanStatus").val("Final");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
 
function EnableDisableRejectReason() {
    var approvalStatus = $("#ddlLoanStatus option:selected").text();
    if (approvalStatus == "Rejected") {
        $("#txtRejectedReason").attr("disabled", false);
    }
    else {
        $("#txtRejectedReason").attr("disabled", true);
        $("#txtRejectedReason").val("");
    }
}
 
 
 