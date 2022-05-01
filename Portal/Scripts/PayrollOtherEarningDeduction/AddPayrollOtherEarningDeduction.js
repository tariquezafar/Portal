$(document).ready(function () {
 
    $("#txtPayrollProcessDate").attr('readOnly', true);
    $("#txtPayrollProcessingStartDate").attr('readOnly', true);
    $("#txtPayrollProcessingEndDate").attr('readOnly', true); 
    $("#txtEmployeeCode").attr('readOnly', true);
    $("#ddlPayrollProcessStatus").attr('disabled', true);
    $("#ddlPayrollLocked").attr('disabled', true);
    $("#txtPayrollLockDate").attr('readOnly', true);
    $("#ddlDepartment").attr('disabled', true);
   // $("#ddlCompanyBranch").attr('disabled', true);
    $("#ddlDesignation").attr('disabled', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    BindPayrollMonthList();
    BindDepartmentList();   
    BindCompanyBranchList();
   
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnMonthlyInputId = $("#hdnMonthlyInputId");
    if (hdnMonthlyInputId.val() != "" && hdnMonthlyInputId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPayrollOtherEarningDeductionDetail(hdnMonthlyInputId.val());
       }, 1000);

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
            //$("#txtEmployee").attr('readOnly', true);
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
                    companyBranchId: 0,
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.FirstName, value: item.EmployeeId, EmployeeCode: item.EmployeeCode, MobileNo: item.MobileNo, DepartmentId: item.DepartmentId, CompanyBranchId: item.CompanyBranchId, DesignationId: item.DesignationId
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
            $(":input#ddlDepartment").trigger('change');
            BindDesignationList(ui.item.DesignationId);
            $("#ddlDesignation").val(ui.item.DesignationId);
            //$("#ddlCompanyBranch").val(ui.item.CompanyBranchId);
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

function GetPayrollOtherEarningDeductionDetail(monthlyInputId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PayrollOtherEarningDeduction/GetPayrollOtherEarningDeductionDetail",
        data: { monthlyInputId: monthlyInputId },
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

            if (data.TDSApplicable =="True")
            {
                $("#ddlTDSApplicable").val("1");
            }
            else {
                $("#ddlTDSApplicable").val("0");
            }           
            $("#txtIncomeTax").val(data.IncomeTax);
            $("#txtAnnualBonus").val(data.AnnualBonus);
            $("#txtExgretia").val(data.Exgretia);
            $("#txtIncentive").val(data.Incentive);
            $("#txtLeaveEncashment").val(data.LeaveEncashment);
            $("#txtNoticePayPayable").val(data.NoticePayPayable);
            $("#txtOverTimeAllow").val(data.OverTimeAllow);
            $("#txtVariablePay").val(data.VariablePay);
            $("#txtOtherDeduction").val(data.OtherDeduction);
            $("#txtOtherAllowance").val(data.OtherAllowance);
            $("#txtLoanPayable").val(data.LoanPayable);
            $("#txtLoanRecv").val(data.LoanRecv);
            $("#txtAdvancePayable").val(data.AdvancePayable);
            $("#txtAdvanceRecv").val(data.AdvanceRecv);           

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}

function SaveData() {
    var hdnMonthlyInputId = $("#hdnMonthlyInputId");
    var ddlMonth = $("#ddlMonth");
    var txtPayrollProcessingStartDate = $("#txtPayrollProcessingStartDate");
    var txtPayrollProcessingEndDate = $("#txtPayrollProcessingEndDate");
    var ddlPayrollLocked = $("#ddlPayrollLocked");
    var hdnMonthID = $("#hdnMonthID"); 
    var hdnEmployeeId = $("#hdnEmployeeId"); 
    var ddlDepartment = $("#ddlDepartment");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlTDSApplicable = $("#ddlTDSApplicable");
    var txtIncomeTax = $("#txtIncomeTax");
    var txtAnnualBonus = $("#txtAnnualBonus");
    var txtExgretia = $("#txtExgretia");
    var txtIncentive = $("#txtIncentive");
    var txtLeaveEncashment = $("#txtLeaveEncashment");
    var txtNoticePayPayable = $("#txtNoticePayPayable");
    var txtOverTimeAllow = $("#txtOverTimeAllow");
    var txtVariablePay = $("#txtVariablePay");
    var txtOtherDeduction = $("#txtOtherDeduction");
    var txtOtherAllowance = $("#txtOtherAllowance");
    var txtLoanPayable = $("#txtLoanPayable");
    var txtLoanRecv = $("#txtLoanRecv");
    var txtAdvancePayable = $("#txtAdvancePayable"); 
    var txtAdvanceRecv = $("#txtAdvanceRecv");
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
    var payrollOtherEarningDeductionViewModel = {
        MonthlyInputId: hdnMonthlyInputId.val(),
        PayrollProcessingPeriodId: ddlMonth.val(),
        PayrollProcessingStartDate: txtPayrollProcessingStartDate.val().trim(),
        PayrollProcessingEndDate: txtPayrollProcessingEndDate.val().trim(),
        MonthId: hdnMonthID.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
        DepartmentId: ddlDepartment.val(),
        EmployeeId: hdnEmployeeId.val(),
        TDSApplicable: ddlTDSApplicable.val(),
        IncomeTax: txtIncomeTax.val().trim(),
        AnnualBonus: txtAnnualBonus.val().trim(),
        Exgretia: txtExgretia.val().trim(),
        Incentive: txtIncentive.val().trim(),
        LeaveEncashment: txtLeaveEncashment.val().trim(),
        NoticePayPayable: txtNoticePayPayable.val().trim(),
        OverTimeAllow: txtOverTimeAllow.val().trim(),
        VariablePay: txtVariablePay.val().trim(),
        OtherDeduction: txtOtherDeduction.val().trim(),
        OtherAllowance: txtOtherAllowance.val().trim(),
        LoanPayable: txtLoanPayable.val().trim(),
        LoanRecv: txtLoanRecv.val().trim(),
        AdvancePayable: txtAdvancePayable.val().trim(),
        AdvanceRecv: txtAdvanceRecv.val().trim(),
    };

    var accessMode = 1;//Add Mode
    if (hdnMonthlyInputId.val() != null && hdnMonthlyInputId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = {
        payrollOtherEarningDeductionViewModel: payrollOtherEarningDeductionViewModel
    };
    $.ajax({
        url: "../PayrollOtherEarningDeduction/AddEditPayrollOtherEarningDeduction?accessMode=" + accessMode + "",
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
                      window.location.href = "../PayrollOtherEarningDeduction/ListPayrollOtherEarningDeduction";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PayrollOtherEarningDeduction/AddEditPayrollOtherEarningDeduction";
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
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");
            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
                $("#ddlCompanyBranch").attr('disabled', true);
            }

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
    $("#txtIncomeTax").val("");
    $("#txtAnnualBonus").val("");
    $("#txtExgretia").val("");
    $("#txtIncentive").val("");
    $("#txtLeaveEncashment").val("");
    $("#txtNoticePayPayable").val("");
    $("#txtOverTimeAllow").val("");

    $("#txtVariablePay").val("");
    $("#txtOtherDeduction").val("");
    $("#txtOtherAllowance").val("");
    $("#txtLoanPayable").val("");

    $("#txtLoanRecv").val("");
    $("#txtAdvancePayable").val("");
    $("#txtAdvanceRecv").val("");   
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

function ShowHideIncomeTax() {
    if($("#ddlTDSApplicable").val()=="1")
    {
        $("#txtIncomeTax").attr('readOnly', false);

    }
    else {
        $("#txtIncomeTax").attr('readOnly', true);
        $("#txtIncomeTax").val("");
    }
}