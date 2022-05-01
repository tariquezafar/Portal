$(document).ready(function () {
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnEssEmployeeId.val() != "0") {
        $("#txtEmployee").attr('readOnly', true);
        $("#txtEmployee").val(hdnEssEmployeeName.val());
        $("#hdnEmployeeId").val(hdnEssEmployeeId.val());
    }
    $("#txtApplicationNo").attr('readOnly', true);
    if (hdnApplicationId.val() != "0") {
        $("#txtCreatedBy").attr('readOnly', true);
        $("#txtCreatedDate").attr('readOnly', true);
        $("#txtModifiedBy").attr('readOnly', true);
        $("#txtModifiedDate").attr('readOnly', true);
        $("#txtRejectedDate").attr('readOnly', true);
        $("#txtApplicationDate").attr('readOnly', true);
        $("#txtLoanStartDate").attr('readOnly', true);
        $("#txtLoanEndDate").attr('readOnly', true);
        $("#txtHireByDate").attr('readOnly', true);
    }
    $("#txtApplicationDate,#txtLoanEndDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    $("#txtLoanStartDate").datepicker({
        minDate : 0,
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
           GetEmployeeLoanApplicationDetail(hdnApplicationId.val());
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
   
function GetEmployeeLoanApplicationDetail(applicationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeLoanApplication/GetEmployeeLoanApplicationDetail",
        data: { applicationId: applicationId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicationNo").val(data.ApplicationNo);
            $("#hdnApplicationId").val(data.ApplicationId);
            $("#txtApplicationDate").val(data.ApplicationDate);
            $("#ddlLoanType").val(data.LoanTypeId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#hdnEmployeeId").val(data.EmployeeId);
            //$("#hdnEssEmployeeId").val(data.EmployeeId);

            $("#txtLoanInterestRate").val(data.LoanInterestRate);
            $("#txtInterestCalcOn").val(data.InterestCalcOn);

            $("#txtLoanAmount").val(data.LoanAmount);
            $("#txtLoanStartDate").val(data.LoanStartDate);
            $("#txtLoanEndDate").val(data.LoanEndDate);
            $("#txtLoanInstallmentAmount").val(data.LoanInstallmentAmount);
            $("#txtLoanReason").val(data.LoanReason);
            $("#ddlLoanStatus").val(data.LoanStatus);
 
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
    var txtApplicationNo = $("#txtApplicationNo");
    var hdnApplicationId = $("#hdnApplicationId");
    var txtApplicationDate = $("#txtApplicationDate");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var txtEmployee = $("#txtEmployee");
    var ddlLoanType = $("#ddlLoanType"); 
    var txtLoanInterestRate = $("#txtLoanInterestRate");
    var txtInterestCalcOn = $("#txtInterestCalcOn"); 
    var txtLoanAmount = $("#txtLoanAmount");
    var txtLoanStartDate = $("#txtLoanStartDate");
    var txtLoanEndDate = $("#txtLoanEndDate");
    var txtLoanInstallmentAmount = $("#txtLoanInstallmentAmount");
    var txtLoanReason = $("#txtLoanReason");
    var ddlLoanStatus = $("#ddlLoanStatus");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var EmployeeId;
    if (hdnEssEmployeeId.val() != "0") {
        EmployeeId = hdnEssEmployeeId.val();
    }
    else {
        EmployeeId = hdnEmployeeId.val();
    }
   
    if (txtApplicationDate.val() == "" || txtApplicationDate.val() == "0") {
        ShowModel("Alert", "Please select Application Date")
        return false;

    }
    if (txtEmployee.val() == "" || txtEmployee.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false;

    } 
    if (ddlLoanType.val() == "" || ddlLoanType.val() == "0") {
        ShowModel("Alert", "Please select Loan Type")
        return false;
    }
 
    if (txtLoanInterestRate.val() == "" || txtLoanInterestRate.val() == "0") {
        ShowModel("Alert", "Please Enter Loan Interest rate")
        return false;

    }
    if (txtInterestCalcOn.val() == "" || txtInterestCalcOn.val() == "0") {
        ShowModel("Alert", "Please Enter Interest CalcOn")
        return false;

    }
    if (txtLoanAmount.val() == "" || txtLoanAmount.val() == "0") {
        ShowModel("Alert", "Please Enter Loan Amount")
        return false;

    }
    if (txtLoanStartDate.val() == "" || txtLoanStartDate.val() == "0") {
        ShowModel("Alert", "Please Select Loan Start Date")
        return false;

    }
    if (txtLoanEndDate.val() == "" || txtLoanEndDate.val() == "0") {
        ShowModel("Alert", "Please Select Loan End Date")
        return false;

    }
    if (txtLoanInstallmentAmount.val() == "" || txtLoanInstallmentAmount.val() == "0") {
        ShowModel("Alert", "Please Enter Loan  Installment Amount")
        return false;

    }
    if (txtLoanReason.val() == "" || txtLoanReason.val() == "0") {
        ShowModel("Alert", "Please Enter Loan Reason")
        return false;

    }

    var employeeLoanApplicationViewModel = {
        ApplicationId: hdnApplicationId.val(),
        ApplicationNo: txtApplicationNo.val().trim(),
        ApplicationDate: txtApplicationDate.val(),
        LoanTypeId: ddlLoanType.val(),
        EmployeeId: EmployeeId,
        LoanInterestRate: txtLoanInterestRate.val(),
        InterestCalcOn:txtInterestCalcOn.val(),
        LoanAmount: txtLoanAmount.val(),       
        LoanStartDate: txtLoanStartDate.val(),
        LoanEndDate:txtLoanEndDate.val(),
        LoanInstallmentAmount: txtLoanInstallmentAmount.val(),
        LoanReason: txtLoanReason.val(),
        LoanStatus: ddlLoanStatus.val()
    };
    var accessMode = 1;//Add Mode
    if (hdnApplicationId.val() != null && hdnApplicationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    

    var requestData = { employeeLoanApplicationViewModel: employeeLoanApplicationViewModel };
    $.ajax({
        url: "../EmployeeLoanApplication/AddEditEmployeeLoanApplication?accessMode=" + accessMode + "",
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
                      window.location.href = "../EmployeeLoanApplication/ListEmployeeLoanApplication";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../EmployeeLoanApplication/AddEditEmployeeLoanApplication";
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
    $("#ddlLoanType").val("0");     
    $("#txtLoanInterestRate").val("");
    $("#txtInterestCalcOn").val("");
    $("#txtLoanAmount").val("");  
    $("#txtLoanInstallmentAmount").val("");
    $("#txtLoanReason").val("");
    $("#ddlLoanStatus").val("Draft");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
 
 
 
 
 