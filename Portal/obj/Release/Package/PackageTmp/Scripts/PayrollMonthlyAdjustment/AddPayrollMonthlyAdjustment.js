$(document).ready(function () {
 
    $("#txtPayrollProcessDate").attr('readOnly', true);
    $("#txtPayrollProcessingStartDate").attr('readOnly', true);
    $("#txtPayrollProcessingEndDate").attr('readOnly', true); 
    $("#txtEmployeeCode").attr('readOnly', true);
    $("#ddlPayrollProcessStatus").attr('disabled', true);
    $("#ddlPayrollLocked").attr('disabled', true);
    $("#txtPayrollLockDate").attr('readOnly', true);
    $("#ddlDepartment").attr('disabled', true);
    $("#ddlCompanyBranch").attr('disabled', true);
    $("#ddlDesignation").attr('disabled', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    BindPayrollMonthList();
    BindDepartmentList();   
    BindCompanyBranchList();
   
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPayrollAdjustmentId = $("#hdnPayrollAdjustmentId");
    if (hdnPayrollAdjustmentId.val() != "" && hdnPayrollAdjustmentId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPayrollMonthlyAdjustmentDetail(hdnPayrollAdjustmentId.val());
       }, 2000);

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $(".editonly").hide();
            

        }
        else {
            $("#ddlMonth").attr('disabled', true);
            $("#txtEmployee").attr('readOnly', true);
            $("#btnUpdate").show();
            $("#btnSave").hide();
            $(".editonly").show();
        }
    }
    else {
        $("#btnSave").show();
        $(".editonly").show();
    }

 

    $("#txtEmployee").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, 
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.FirstName, value: item.EmployeeId, EmployeeCode: item.EmployeeCode, MobileNo: item.MobileNo, DepartmentId: item.DepartmentId, CompanyBranchId: item.CompanyBranchId, DesignationId:item.DesignationId
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
            $("#hdnEmployeeName").val(ui.item.label);            
            $("#txtEmployeeCode").val(ui.item.EmployeeCode);
            $("#ddlDepartment").val(ui.item.DepartmentId);
            BindDesignationList(ui.item.DesignationId);
            $("#ddlDesignation").val(ui.item.DesignationId);
            $("#ddlCompanyBranch").val(ui.item.CompanyBranchId);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtEmployee").val("");
                $("#hdnEmployeeId").val("0");
                $("#txtEmployeeCode").val("");
                $("#ddlDepartment").val("0");
                $("#ddlCompanyBranch").val("0");
                $("#ddlDepartment").val("0");
                ShowModel("Alert", "Please select Employee from List");

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.EmployeeCode + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};





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
function BindPayrollMonthList() {
    $("#ddlMonth").val(0);
    $("#ddlMonth").html("");
    $.ajax({
        type: "GET",
        url: "../SalaryReport/GetProcessedMonthList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Select Period-"));
            $.each(data, function (i, item) {
                $("#ddlMonth").append($("<option></option>").val(item.PayrollProcessingPeriodId).html(item.PayrollProcessingStartDate + ' - ' + item.PayrollProcessingEndDate));
            });
        },
        error: function (Result) {
            $("#ddlMonth").append($("<option></option>").val(0).html("-Select Period-"));
        }
    });
}

function GetProcessStartEndDate() {
    var payrollProcessingPeriodId = $("#ddlMonth option:selected").val();
    $("#txtPayrollProcessingStartDate").val("");
    $("#txtPayrollProcessingEndDate").val("");
    $.ajax({
        type: "GET",
        url: "../PayrollProcessPeriod/GetPayrollProcessPeriodDetail",
        data: { payrollProcessingPeriodId: payrollProcessingPeriodId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlMonth").val(data.PayrollProcessingPeriodId);
            $("#hdnpayrollProcessingPeriodId").val(data.PayrollProcessingPeriodId);
            $("#txtPayrollProcessDate").val(data.PayrollProcessDate);
            $("#txtPayrollProcessingStartDate").val(data.PayrollProcessingStartDate);
            $("#txtPayrollProcessingEndDate").val(data.PayrollProcessingEndDate);
            $("#ddlPayrollProcessStatus").val(data.PayrollProcessStatus);
            $("#ddlPayrollLocked").val(data.PayrollLocked);
            $("#txtPayrollLockDate").val(data.PayrollLockDate);
            
            
        },
        error: function (Result) {
            $("#txtPayrollProcessingStartDate").val("");
            $("#txtPayrollProcessingEndDate").val("");
        }
    });
}

function GetPayrollMonthlyAdjustmentDetail(payrollAdjustmentId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PayrollMonthlyAdjustment/GetPayrollMonthlyAdjustmentDetail",
        data: {  payrollAdjustmentId: payrollAdjustmentId },
        dataType: "json",
        success: function (data) {
            $("#ddlMonth").val(data.PayrollProcessingPeriodId);           
            $("#txtPayrollProcessDate").val(data.PayrollProcessDate);
            $("#txtPayrollProcessingStartDate").val(data.PayrollProcessingStartDate);
            $("#txtPayrollProcessingEndDate").val(data.PayrollProcessingEndDate);            
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#txtEmployeeCode").val(data.EmployeeCode); 
            $("#ddlDepartment").val(data.DepartmentId);
            $("#hdnDepartment").val(data.DepartmentId);
            BindDesignationList(data.DesignationId);
            $("#ddlDesignation").val(data.DesignationId);
          
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#hdnCompanyBranch").val(data.CompanyBranchId);

            if (parseFloat(data.BasicPay)>=0)
            {
                $("#ddlBasicPay").val("+");
                $("#txtBasicPay").val(data.BasicPay);
            }
          else
            {
                $("#ddlBasicPay").val("-");
                $("#txtBasicPay").val(Math.abs(data.BasicPay));
            }

            if (parseFloat(data.ConveyanceAllow) >= 0) {
                $("#ddlConveyanceAllow").val("+");
                $("#txtConveyanceAllow").val(data.ConveyanceAllow);
            }
            else {
                $("#ddlConveyanceAllow").val("-");
                $("#txtConveyanceAllow").val(Math.abs(data.ConveyanceAllow));
            }
            if (parseFloat(data.SpecialAllow) >= 0) {
                $("#ddlSpecialAllow").val("+");
                $("#txtSpecialAllow").val(data.SpecialAllow);
            }
            else {
                $("#ddlSpecialAllow").val("-");
                $("#txtSpecialAllow").val(Math.abs(data.SpecialAllow));
            }
            if (parseFloat(data.OtherAllow) >= 0) {
                $("#ddlOtherAllow").val("+");
                $("#txtOtherAllow").val(data.OtherAllow);
            }
            else {
                $("#ddlOtherAllow").val("-");
                $("#txtOtherAllow").val(Math.abs(data.OtherAllow));
            }
            if (parseFloat(data.MedicalAllow) >= 0) {
                $("#ddlMedicalAllow").val("+");
                $("#txtMedicalAllow").val(data.MedicalAllow);
            }
            else {
                $("#ddlMedicalAllow").val("-");
                $("#txtMedicalAllow").val(Math.abs(data.MedicalAllow));
            }
            if (parseFloat(data.ChildEduAllow) >= 0) {
                $("#ddlChildEduAllow").val("+");
                $("#txtChildEduAllow").val(data.ChildEduAllow);
            }
            else {
                $("#ddlChildEduAllow").val("-");
                $("#txtChildEduAllow").val(Math.abs(data.ChildEduAllow));
            }
            if (parseFloat(data.LTA) >= 0) {
                $("#ddlLTA").val("+");
                $("#txtLTA").val(data.LTA);
            }
            else {
                $("#ddlLTA").val("-");
                $("#txtLTA").val(Math.abs(data.LTA));
            }
            if (parseFloat(data.EmployeePF) >= 0) {
                $("#ddlEmployeePF").val("+");
                $("#txtEmployeePF").val(data.EmployeePF);
            }
            else {
                $("#ddlEmployeePF").val("-");
                $("#txtEmployeePF").val(Math.abs(data.EmployeePF));
            }
            if (parseFloat(data.EmployeeESI) >= 0) {
                $("#ddlEmployeeESI").val("+");
                $("#txtEmployeeESI").val(data.EmployeeESI);
            }
            else {
                $("#ddlEmployeeESI").val("-");
                $("#txtEmployeeESI").val(Math.abs(data.EmployeeESI));
            }
            if (parseFloat(data.OtherDeduction) >= 0) {
                $("#ddlOtherDeduction").val("+");
                $("#txtOtherDeduction").val(data.OtherDeduction);
            }
            else {
                $("#ddlOtherDeduction").val("-");
                $("#txtOtherDeduction").val(Math.abs(data.OtherDeduction));
            }
            if (parseFloat(data.ProfessionalTax) >= 0) {
                $("#ddlProfessionalTax").val("+");
                $("#txtProfessionalTax").val(data.ProfessionalTax);
            }
            else {
                $("#ddlProfessionalTax").val("-");
                $("#txtProfessionalTax").val(Math.abs(data.ProfessionalTax));
            }
            if (parseFloat(data.AdhocAllowance) >= 0) {
                $("#ddlAdhocAllowance").val("+");
                $("#txtAdhocAllowance").val(data.AdhocAllowance);
            }
            else {
                $("#ddlAdhocAllowance").val("-");
                $("#txtAdhocAllowance").val(Math.abs(data.AdhocAllowance));
            }
            if (parseFloat(data.AnnualBonus) >= 0) {
                $("#ddlAnnualBonus").val("+");
                $("#txtAnnualBonus").val(data.AnnualBonus);
            }
            else {
                $("#ddlAnnualBonus").val("-");
                $("#txtAnnualBonus").val(Math.abs(data.AnnualBonus));
            }
            if (parseFloat(data.Exgratia) >= 0) {
                $("#ddlExgratia").val("+");
                $("#txtExgratia").val(data.Exgratia);
            }
            else {
                $("#ddlExgratia").val("-");
                $("#txtExgratia").val(Math.abs(data.Exgratia));
            }
            if (parseFloat(data.LeaveEncashment) >= 0) {
                $("#ddlLeaveEncashment").val("+");
                $("#txtLeaveEncashment").val(data.LeaveEncashment);
            }
            else {
                $("#ddlLeaveEncashment").val("-");
                $("#txtLeaveEncashment").val(Math.abs(data.LeaveEncashment));
            }
            if (parseFloat(data.SalaryAdvancePayable) >= 0) {
                $("#ddlSalaryAdvancePayable").val("+");
                $("#txtSalaryAdvancePayable").val(data.SalaryAdvancePayable);
            }
            else {
                $("#ddlSalaryAdvancePayable").val("-");
                $("#txtSalaryAdvancePayable").val(Math.abs(data.SalaryAdvancePayable));
            }
            if (parseFloat(data.NoticePayPayable) >= 0) {
                $("#ddlNoticePayPayable").val("+");
                $("#txtNoticePayPayable").val(data.NoticePayPayable);
            }
            else {
                $("#ddlNoticePayPayable").val("-");
                $("#txtNoticePayPayable").val(Math.abs(data.NoticePayPayable));
            }
            if (parseFloat(data.SalaryAdvanceRecv) >= 0) {
                $("#ddlSalaryAdvanceRecv").val("+");
                $("#txtSalaryAdvanceRecv").val(data.SalaryAdvanceRecv);
            }
            else {
                $("#ddlSalaryAdvanceRecv").val("-");
                $("#txtSalaryAdvanceRecv").val(Math.abs(data.SalaryAdvanceRecv));
            }
            if (parseFloat(data.NoticePayRecv) >= 0) {
                $("#ddlNoticePayRecv").val("+");
                $("#txtNoticePayRecv").val(data.NoticePayRecv);
            }
            else {
                $("#ddlNoticePayRecv").val("-");
                $("#txtNoticePayRecv").val(Math.abs(data.NoticePayRecv));
            }
            if (parseFloat(data.LoanPayable) >= 0) {
                $("#ddlLoanPayable").val("+");
                $("#txtLoanPayable").val(data.LoanPayable);
            }
            else {
                $("#ddlLoanPayable").val("-");
                $("#txtLoanPayable").val(Math.abs(data.LoanPayable));
            }
            if (parseFloat(data.LoanRecv) >= 0) {
                $("#ddlLoanRecv").val("+");
                $("#txtLoanRecv").val(data.LoanRecv);
            }
            else {
                $("#ddlLoanRecv").val("-");
                $("#txtLoanRecv").val(Math.abs(data.LoanRecv));
            }
            if (parseFloat(data.IncomeTax) >= 0) {
                $("#ddlIncomeTax").val("+");
                $("#txtIncomeTax").val(data.IncomeTax);
            }
            else {
                $("#ddlIncomeTax").val("-");
                $("#txtIncomeTax").val(Math.abs(data.IncomeTax));
            }
    
                  

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}

function SaveData() {
    var hdnPayrollAdjustmentId = $("#hdnPayrollAdjustmentId");
    var ddlMonth = $("#ddlMonth");
    var txtPayrollProcessingStartDate = $("#txtPayrollProcessingStartDate");
    var txtPayrollProcessingEndDate = $("#txtPayrollProcessingEndDate");
    var ddlPayrollLocked = $("#ddlPayrollLocked");
    var hdnMonthID = $("#hdnMonthID"); 
    var hdnEmployeeId = $("#hdnEmployeeId"); 
    var ddlDepartment = $("#ddlDepartment");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var basicPay = $("#txtBasicPay").val();
    if ($("#ddlBasicPay").val() == "-" && parseFloat(basicPay)>0)
    {
        basicPay = -1 * parseFloat(basicPay);
    }
    else {
        basicPay = $("#txtBasicPay").val();
    }
    var conveyanceAllow = $("#txtConveyanceAllow").val();
    if ($("#ddlConveyanceAllow").val() == "-" && parseFloat(conveyanceAllow) > 0) {
        conveyanceAllow = -1 * parseFloat(conveyanceAllow);
    }
    else {
        conveyanceAllow = $("#txtConveyanceAllow").val();
    }

    var specialAllow = $("#txtSpecialAllow").val();  
    if ($("#ddlSpecialAllow").val() == "-" && parseFloat(specialAllow) > 0) {
        specialAllow = -1 * parseFloat(specialAllow);
    }
    else {
        specialAllow = $("#txtSpecialAllow").val();
    }
    var otherAllow = $("#txtOtherAllow").val();
    if ($("#ddlOtherAllow").val() == "-" && parseFloat(otherAllow) > 0) {
        otherAllow = -1 * parseFloat(otherAllow);
    }
    else {
        otherAllow = $("#txtOtherAllow").val();
    }
    var medicalAllow = $("#txtMedicalAllow").val();
    if ($("#ddlMedicalAllow").val() == "-" && parseFloat(medicalAllow) > 0) {
        medicalAllow = -1 * parseFloat(medicalAllow);
    }
    else {
        medicalAllow = $("#txtMedicalAllow").val();
    }

    var childEduAllow = $("#txtChildEduAllow").val();
    if ($("#ddlChildEduAllow").val() == "-" && parseFloat(childEduAllow) > 0) {
        childEduAllow = -1 * parseFloat(childEduAllow);
    }
    else {
        childEduAllow = $("#txtChildEduAllow").val();
    }
    var lTA = $("#txtLTA").val();
    if ($("#ddlLTA").val() == "-" && parseFloat(lTA) > 0) {
        lTA = -1 * parseFloat(lTA);
    }
    else {
        lTA = $("#txtLTA").val();
    }

    var employeePF = $("#txtEmployeePF").val();
    if ($("#ddlEmployeePF").val() == "-" && parseFloat(employeePF) > 0) {
        employeePF = -1 * parseFloat(employeePF);
    }
    else {
        employeePF = $("#txtEmployeePF").val();
    }

    var employeeESI = $("#txtEmployeeESI").val();
    if ($("#ddlEmployeeESI").val() == "-" && parseFloat(employeeESI) > 0) {
        employeeESI = -1 * parseFloat(employeeESI);
    }
    else {
        employeeESI = $("#txtEmployeeESI").val();
    }
    var otherDeduction = $("#txtOtherDeduction").val();
    if ($("#ddlOtherDeduction").val() == "-" && parseFloat(otherDeduction) > 0) {
        otherDeduction = -1 * parseFloat(otherDeduction);
    }
    else {
        otherDeduction = $("#txtOtherDeduction").val();
    }
    var professionalTax = $("#txtProfessionalTax").val();
    if ($("#ddlProfessionalTax").val() == "-" && parseFloat(professionalTax) > 0) {
        professionalTax = -1 * parseFloat(professionalTax);
    }
    else {
        professionalTax = $("#txtProfessionalTax").val();
    }
    var adhocAllowance = $("#txtAdhocAllowance").val();
    if ($("#ddlAdhocAllowance").val() == "-" && parseFloat(adhocAllowance) > 0) {
        adhocAllowance = -1 * parseFloat(adhocAllowance);
    }
    else {
        adhocAllowance = $("#txtAdhocAllowance").val();
    }
    var annualBonus = $("#txtAnnualBonus").val();
    if ($("#ddlAnnualBonus").val() == "-" && parseFloat(annualBonus) > 0) {
        annualBonus = -1 * parseFloat(annualBonus);
    }
    else {
        annualBonus = $("#txtAnnualBonus").val();
    }
    var exgratia = $("#txtExgratia").val();
    if ($("#ddlExgratia").val() == "-" && parseFloat(exgratia) > 0) {
        exgratia = -1 * parseFloat(exgratia);
    }
    else {
        exgratia = $("#txtExgratia").val();
    }
    var leaveEncashment = $("#txtLeaveEncashment").val();
    if ($("#ddlLeaveEncashment").val() == "-" && parseFloat(leaveEncashment) > 0) {
        leaveEncashment = -1 * parseFloat(leaveEncashment);
    }
    else {
        leaveEncashment = $("#txtLeaveEncashment").val();
    }
    var salaryAdvancePayable = $("#txtSalaryAdvancePayable").val();
    if ($("#ddlSalaryAdvancePayable").val() == "-" && parseFloat(salaryAdvancePayable) > 0) {
        salaryAdvancePayable = -1 * parseFloat(salaryAdvancePayable);
    }
    else {
        salaryAdvancePayable = $("#txtSalaryAdvancePayable").val();
    }
    var noticePayPayable = $("#txtNoticePayPayable").val();
    if ($("#ddlNoticePayPayable").val() == "-" && parseFloat(noticePayPayable) > 0) {
        noticePayPayable = -1 * parseFloat(noticePayPayable);
    }
    else {
        noticePayPayable = $("#txtNoticePayPayable").val();
    }
    var salaryAdvanceRecv = $("#txtSalaryAdvanceRecv").val();
    if ($("#ddlSalaryAdvanceRecv").val() == "-" && parseFloat(salaryAdvanceRecv) > 0) {
        salaryAdvanceRecv = -1 * parseFloat(salaryAdvanceRecv);
    }
    else {
        salaryAdvanceRecv = $("#txtSalaryAdvanceRecv").val();
    }
    var noticePayRecv = $("#txtNoticePayRecv").val();
    if ($("#ddlNoticePayRecv").val() == "-" && parseFloat(noticePayRecv) > 0) {
        noticePayRecv = -1 * parseFloat(noticePayRecv);
    }
    else {
        noticePayRecv = $("#txtNoticePayRecv").val();
    }
    var loanPayable = $("#txtLoanPayable").val();
    if ($("#ddlLoanPayable").val() == "-" && parseFloat(loanPayable) > 0) {
        loanPayable = -1 * parseFloat(loanPayable);
    }
    else {
        loanPayable = $("#txtLoanPayable").val();
    }
    var loanRecv = $("#txtLoanRecv").val();
    if ($("#ddlLoanRecv").val() == "-" && parseFloat(loanRecv) > 0) {
        loanRecv = -1 * parseFloat(loanRecv);
    }
    else {
        loanRecv = $("#txtLoanRecv").val();
    }
    var incomeTax = $("#txtIncomeTax").val();
    if ($("#ddlIncomeTax").val() == "-" && parseFloat(incomeTax) > 0) {
        incomeTax = -1 * parseFloat(incomeTax);
    }
    else {
        incomeTax = $("#txtIncomeTax").val();
    }

    
    if (ddlMonth.val() == "" || ddlMonth.val() == "0") {
        ShowModel("Alert", "Please select Payroll Processing Month")
        return false;
    }
    if (ddlPayrollLocked.val() == "1" ) {
        ShowModel("Alert", "Please Unlock Payroll Process Period")
        return false;
    }
    if (hdnEmployeeId.val() == "" || hdnEmployeeId.val() == "0") {
        ShowModel("Alert", "Please Select Employee")
        return false;
    }
    var payrollMonthlyAdjustmentViewModel = {
        PayrollAdjustmentId: hdnPayrollAdjustmentId.val(),
        PayrollProcessingPeriodId: ddlMonth.val(),
        PayrollProcessingStartDate: txtPayrollProcessingStartDate.val().trim(),
        PayrollProcessingEndDate: txtPayrollProcessingEndDate.val().trim(),
        MonthId: hdnMonthID.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
        DepartmentId: ddlDepartment.val(),
        EmployeeId: hdnEmployeeId.val(),
        BasicPay: basicPay,
        ConveyanceAllow: conveyanceAllow,
        SpecialAllow: specialAllow,
        OtherAllow: otherAllow,
        MedicalAllow: medicalAllow,
        ChildEduAllow: childEduAllow,
        LTA: lTA,
        EmployeePF: employeePF,
        EmployeeESI: employeeESI,
        OtherDeduction: otherDeduction,
        ProfessionalTax: professionalTax,
        AdhocAllowance: adhocAllowance,
        AnnualBonus: annualBonus,
        Exgratia: exgratia,
        LeaveEncashment: leaveEncashment,
        SalaryAdvancePayable: salaryAdvancePayable,
        NoticePayPayable: noticePayPayable,
        SalaryAdvanceRecv: salaryAdvanceRecv,
        NoticePayRecv: noticePayRecv,
        LoanPayable: loanPayable,
        LoanRecv: loanRecv,
        IncomeTax: incomeTax
    };

    var accessMode = 1;//Add Mode
    if (hdnPayrollAdjustmentId.val() != null && hdnPayrollAdjustmentId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = {
        payrollMonthlyAdjustmentViewModel: payrollMonthlyAdjustmentViewModel
    };
    $.ajax({
        url: "../PayrollMonthlyAdjustment/AddEditPayrollMonthlyAdjustment?accessMode=" + accessMode + "",
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
                      window.location.href = "../PayrollMonthlyAdjustment/ListPayrollMonthlyAdjustment";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PayrollMonthlyAdjustment/AddEditPayrollMonthlyAdjustment";
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

function BindCompanyBranchList() {
    $("#ddlCompanyBranch,#ddlSearchCompanyBranch").val(0);
    $("#ddlCompanyBranch,#ddlSearchCompanyBranch").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}
function ClearFields() {
    $("#ddlMonth").val("0");
    $("#ddlDepartment").val("0");
    $("#ddlCompanyBranch").val("0");
    $("#txtEmployee").val("");
    $("#hdnEmployeeId").val("0");

    $("#txtEmployeeCode").val("");

    $("#ddlBasicPay").val("+");
    $("#ddlConveyanceAllow").val("+");
    $("#ddlSpecialAllow").val("+");
    $("#ddlOtherAllow").val("+");
    $("#ddlMedicalAllow").val("+");
    $("#ddlChildEduAllow").val("+");
    $("#ddlLTA").val("+");
    $("#ddlEmployeePF").val("+");
    $("#ddlEmployeeESI").val("+");
    $("#ddlOtherDeduction").val("+");
    $("#ddlProfessionalTax").val("+");
    $("#ddlAdhocAllowance").val("+");
    $("#ddlAnnualBonus").val("+");
    $("#ddlExgratia").val("+");
    $("#ddlLeaveEncashment").val("+");
    $("#ddlSalaryAdvancePayable").val("+");
    $("#ddlNoticePayPayable").val("+");
    $("#ddlSalaryAdvanceRecv").val("+");
    $("#ddlNoticePayRecv").val("+");
    $("#ddlLoanPayable").val("+");
    $("#ddlLoanRecv").val("+");
    $("#ddlIncomeTax").val("+");


    $("#txtBasicPay").val("");
    $("#txtConveyanceAllow").val("");
    $("#txtSpecialAllow").val("");
    $("#txtOtherAllow").val("");
    $("#txtMedicalAllow").val("");
    $("#txtChildEduAllow").val("");
    $("#txtLTA").val("");
    $("#txtEmployeeESI").val("");
    $("#txtOtherDeduction").val("");
    $("#txtProfessionalTax").val("");
    $("#txtAdhocAllowance").val("");
    $("#txtAnnualBonus").val("");
    $("#txtExgratia").val("");
    $("#txtLeaveEncashment").val("");
    $("#txtNoticePayPayable").val("");
    $("#txtSalaryAdvanceRecv").val("");
    $("#txtNoticePayRecv").val("");
    $("#txtLoanPayable").val("");
    $("#txtIncomeTax").val("");
   

   
}

function BindDesignationList(DesignationId) {
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
                $("#ddlDesignation").val(DesignationId);
               
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

