$(document).ready(function () {
    $("#txtDateOfBirth").attr('readonly', true);
    $("#txtDOJ").attr('readonly', true);
    $("#txtStatusStartDate").attr('readonly', true);
    $("#txtDOL").attr('readonly', true);
    $("#txtPayableSalary").attr('readonly', true);


    $("#txtDateOfBirth").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        maxDate: '0D',
        onSelect: function (selected) {
        }
    });
    $("#txtDOJ").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        maxDate: '0D',
        onSelect: function (selected) {
        }
    });

    $("#txtDOL").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        minDate: '0D',
        onSelect: function (selected) {
        }
    });
    $("#txtStatusStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        minDate: '0D',
        onSelect: function (selected) {
        }
    });


    $("#tabs").tabs({
        collapsible: true
    });

    BindCountryList();
    BindDepartmentList();
    BindReportingDepartmentList();
    $("#ddlCState,#ddlPState").append($("<option></option>").val(0).html("-Select State-"));
    $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
    $("#ddlReportingDesignation").append($("<option></option>").val(0).html("-Select Designation-"));

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnEmployeeID = $("#hdnEmployeeID");
    if (hdnEmployeeID.val() != "" && hdnEmployeeID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeDetail(hdnEmployeeID.val());
       }, 3000);
        $("#txtEmployeeFirstName").attr('readonly', true);
        $("#txtEmployeeLastName").attr('readonly', true);
        $("#txtEmployeeCode").attr('readonly', true);
        $("#txtEmail").attr('readonly', true);
        $("#ddlDivision").prop("disabled", true);
        $("#ddlEmploymentType").prop("disabled", true);
        $("#ddlDepartment").prop("disabled", true);
        $("#ddlDesignation").prop("disabled", true);
        $("#ddlReporingDepartment").prop("disabled", true);
        $("#ddlReportingDesignation").prop("disabled", true);
        $("#txtReportingEmployeeName").prop("disabled", true);
        $("#ddlCurrentStatus").prop("disabled", true);
        $("#ddlStatusPeriod").prop("disabled", true);
        $("#txtStatusStartDate").prop("disabled", true);
        $("#txtDOJ").prop("disabled", true);
        
        $("#txtBasicPay").attr("disabled", true);
        $("#txtOTRate").attr("disabled", true);
        $("#txtHRA").attr("disabled", true);
        $("#txtConveyanceAllow").attr("disabled", true);

        $("#txtSpecialAllow").attr("disabled", true);
        $("#txtMedicalAllow").attr("disabled", true);
        $("#txtChildEduAllow").attr("disabled", true);
        $("#txtLTA").attr("disabled", true);

        $("#txtOtherAllow").attr("disabled", true);
        $("#txtEmployerPF").attr("disabled", true);
        $("#txtEmployerESI").attr("disabled", true);
        $("#txtNetCTC").attr("disabled", true);

        $("#txtEmployeePF").attr("disabled", true);
        $("#txtEmployeeESI").attr("disabled", true);
        $("#txtProfessionalTax").attr("disabled", true);
        $("#txtOtherDeduction").attr("disabled", true);

        
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
    $("#txtEmployeeName").focus();


    $("#txtReportingEmployeeName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, departmentId: $("#ddlReporingDepartment").val(), designationId: $("#ddlReportingDesignation").val()
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
            $("#txtReportingEmployeeName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtReportingEmployeeName").val(ui.item.label);
            $("#hdnReportingEmployeeId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtReportingEmployeeName").val("");
                $("#hdnReportingEmployeeId").val("0");
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

function checkPhone(el) {
    var ex = /^[0-9]+\-?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function changePFNoStatus() {
    if ($("#chkPFApplicable").is(':checked')) {
        $("#txtPFNo").attr("disabled", false);
    }
    else {
        $("#txtPFNo").attr("disabled", true);
        $("#txtPFNo").val("");
    }
}
function changeESINoStatus() {
    if ($("#chkESIApplicable").is(':checked')) {
        $("#txtESINo").attr("disabled", false);
    }
    else {
        $("#txtESINo").attr("disabled", true);
        $("#txtESINo").val("");
    }
}
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
function BindCountryList() {
    $.ajax({
        type: "GET",
        url: "../Company/GetCountryList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCCountry,#ddlPCountry").append($("<option></option>").val(0).html("-Select Country-"));
            $.each(data, function (i, item) {

                $("#ddlCCountry,#ddlPCountry").append($("<option></option>").val(item.CountryId).html(item.CountryName));
            });
        },
        error: function (Result) {
            $("#ddlCCountry,#ddlPCountry").append($("<option></option>").val(0).html("-Select Country-"));
        }
    });
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
function BindReportingDepartmentList() {
    $.ajax({
        type: "GET",
        url: "../Employee/GetDepartmentList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlReporingDepartment").append($("<option></option>").val(0).html("-Select Department-"));
            $.each(data, function (i, item) {

                $("#ddlReporingDepartment").append($("<option></option>").val(item.DepartmentId).html(item.DepartmentName));
            });
        },
        error: function (Result) {
            $("#ddlReporingDepartment").append($("<option></option>").val(0).html("-Select Department-"));
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
function BindReportingDesignationList(designationId) {
    var departmentId = $("#ddlReporingDepartment option:selected").val();
    $("#ddlReportingDesignation").val(0);
    $("#ddlReportingDesignation").html("");
    if (departmentId != undefined && departmentId != "" && departmentId != "0") {
        var data = { departmentId: departmentId };
        $.ajax({
            type: "GET",
            url: "../Employee/GetDesignationList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlReportingDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
                $.each(data, function (i, item) {
                    $("#ddlReportingDesignation").append($("<option></option>").val(item.DesignationId).html(item.DesignationName));
                });
                $("#ddlReportingDesignation").val(designationId);
            },
            error: function (Result) {
                $("#ddlReportingDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
            }
        });
    }
    else {

        $("#ddlReportingDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
    }

}
function BindCurrentStateList(stateId) {
    var countryId = $("#ddlCCountry option:selected").val();
    $("#ddlCState").val(0);
    $("#ddlCState").html("");
    if (countryId != undefined && countryId != "" && countryId != "0") {
        var data = { countryId: countryId };
        $.ajax({
            type: "GET",
            url: "../Company/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlCState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlCState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                $("#ddlCState").val(stateId);
            },
            error: function (Result) {
                $("#ddlCState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
    }
    else {

        $("#ddlCState").append($("<option></option>").val(0).html("-Select State-"));
    }

}
function BindPermanentStateList(stateId) {
    var countryId = $("#ddlPCountry option:selected").val();
    $("#ddlPState").val(0);
    $("#ddlPState").html("");
    if (countryId != undefined && countryId != "" && countryId != "0") {
        var data = { countryId: countryId };
        $.ajax({
            type: "GET",
            url: "../Company/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlPState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlPState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                $("#ddlPState").val(stateId);
            },
            error: function (Result) {
                $("#ddlPState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
    }
    else {

        $("#ddlPState").append($("<option></option>").val(0).html("-Select State-"));
    }

}
function GetEmployeeDetail(employeeId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Employee/GetEmployeeDetail",
        data: { employeeId: employeeId },
        dataType: "json",
        success: function (data) {
            $("#txtEmployeeCode").val(data.EmployeeCode);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtEmployeeFirstName").val(data.FirstName);
             $("#txtEmployeeLastName").val(data.LastName);
            $("#txtFatherSpouseName").val(data.FatherSpouseName);
            if (data.Gender == "M") {
                $("#rdoGenderM").prop("checked", true);
            }
            else {
                $("#rdoGenderF").prop("checked", true);
            }
            $("#txtDateOfBirth").val(data.DateOfBirth);
            $("#ddlMaritalStatus").val(data.MaritalStatus);
            $("#ddlBloodGroup").val(data.BloodGroup);
            $("#txtMobileNo").val(data.MobileNo);
            $("#txtEmail").val(data.Email);
            $("#txtAlternateEmail").val(data.AlternateEmail);
            $("#txtContactNo").val(data.ContactNo);
            $("#txtAlternateContactNo").val(data.AlternateContactno);
            $("#txtCAddress").val(data.CAddress);
            $("#txtCCity").val(data.CCity);
            $("#ddlCCountry").val(data.CCountryId);
            BindCurrentStateList(data.CStateId);
            $("#ddlCState").val(data.CStateId);
            $("#txtCPinCode").val(data.CPinCode);
            $("#txtPAddress").val(data.PAddress);
            $("#txtPCity").val(data.PCity);
            $("#ddlPCountry").val(data.PCountryId);
            BindPermanentStateList(data.PStateId);
            $("#ddlPState").val(data.PStateId);
            $("#txtPPinCode").val(data.PPinCode);
            $("#txtDOJ").val(data.DateOfJoin);
            $("#ddlCurrentStatus").val(data.EmployeeCurrentStatus);
            if (data.EmployeeStatusStartDate != "01-Jan-1900") {
                $("#txtStatusStartDate").val(data.EmployeeStatusStartDate);
            }
            $("#ddlStatusPeriod").val(data.EmployeeStatusPeriod);
            $("#txtPANNo").val(data.PANNo);
            $("#txtAadharNo").val(data.AadharNo);
            $("#txtBankDetail").val(data.BankDetail);
            $("#txtBankAccountNo").val(data.BankAccountNo);
            if (data.PFApplicable == true) {
                $("#chkPFApplicable").prop("checked", true);
                changePFNoStatus();
            }
            $("#txtPFNo").val(data.PFNo);
            if (data.ESIApplicable == true) {
                $("#chkESIApplicable").prop("checked", true);
                changeESINoStatus();
            }
            $("#txtESINo").val(data.ESINo);
            $("#ddlDivision").val(data.Division);
            $("#ddlEmploymentType").val(data.EmploymentType);
            $("#ddlDepartment").val(data.DepartmentId);
            BindDesignationList(data.DesignationId);
            $("#ddlDesignation").val(data.DesignationId);
            if (data.DateOfLeave != "01-Jan-1900") {
                $("#txtDOL").val(data.DateOfLeave);
            }
            $("#ddlReporingDepartment").val(data.ReportingDepartmentId);
            BindReportingDesignationList(data.ReportingDesignationId);
            $("#ddlReportingDesignation").val(data.ReportingDesignationId);
            $("#txtReportingEmployeeName").val(data.ReportingEmployeeName);
            $("#hdnReportingEmployeeId").val(data.ReportingEmployeeId);

            $("#txtBasicPay").val(data.BasicPay);
            $("#txtOTRate").val(data.OTRate);
            $("#txtHRA").val(data.HRA);
            $("#txtConveyanceAllow").val(data.ConveyanceAllow);
            $("#txtSpecialAllow").val(data.SpecialAllow);
            $("#txtOtherAllow").val(data.OtherAllow);
            $("#txtOtherDeduction").val(data.OtherDeduction);
            $("#txtMedicalAllow").val(data.MedicalAllow);
            $("#txtChildEduAllow").val(data.ChildEduAllow);
            $("#txtLTA").val(data.LTA);
            $("#txtEmployeePF").val(data.EmployeePF);
            $("#txtEmployeeESI").val(data.EmployeeESI);
            $("#txtProfessionalTax").val(data.ProfessinalTax);
            $("#txtEmployerPF").val(data.EmployerPF);
            $("#txtEmployerESI").val(data.EmployerESI);

            CalculateSalary();
            $("#imgUserPic").attr("src", "../Images/EmployeeImages/" + data.EmployeePicName);
            if (data.Emp_Status == true) {
                $("#chkStatus").attr("checked", true);
            }
            else {
                $("#chkStatus").attr("checked", false);
            }
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function CalculateSalary() {
    var basicSalary = $("#txtBasicPay").val();
    var hra = $("#txtHRA").val();
    var conveyanceAllow = $("#txtConveyanceAllow").val();
    var specialAllow = $("#txtSpecialAllow").val();
    var otherAllow = $("#txtOtherAllow").val();
    var otherDeduction = $("#txtOtherDeduction").val();
    basicSalary = basicSalary == "" ? 0 : basicSalary;
    hra = hra == "" ? 0 : hra;
    conveyanceAllow = conveyanceAllow == "" ? 0 : conveyanceAllow;
    specialAllow = specialAllow == "" ? 0 : specialAllow;
    otherAllow = otherAllow == "" ? 0 : otherAllow;
    otherDeduction = otherDeduction == "" ? 0 : otherDeduction;

    var totalPayableSalary = parseFloat(basicSalary) + parseFloat(hra) + parseFloat(conveyanceAllow) + parseFloat(specialAllow) + parseFloat(otherAllow) - parseFloat(otherDeduction);
    $("#txtPayableSalary").val(totalPayableSalary);


}
function CopyCurrentAddress() {
    if ($("#chkSamePermanentAddress").is(':checked')) {
        if ($("#txtCAddress").val().trim() != "") {
            $("#txtPAddress").val($("#txtCAddress").val().trim());
        }
        if ($("#txtCCity").val().trim() != "") {
            $("#txtPCity").val($("#txtCCity").val().trim());
        }
        if ($("#ddlCCountry").val() != "" && $("#ddlCCountry").val() != "0") {
            $("#ddlPCountry").val($("#ddlCCountry").val());
        }


        if ($("#ddlCState").val() != "" && $("#ddlCState").val() != "0") {
            BindPermanentStateList($("#ddlCState").val());
            $("#ddlPState").val($("#ddlCState").val());
        }

        if ($("#txtCPinCode").val().trim() != "") {
            $("#txtPPinCode").val($("#txtCPinCode").val().trim());
        }
    }
    else {
        $("#txtPAddress").val("");
        $("#txtPCity").val("");
        $("#ddlPCountry").val("0");
        $("#ddlPState").val("0");

        $("#txtPPinCode").val("");
    }

}
function SaveData() {
    var txtEmployeeFirstName = $("#txtEmployeeFirstName");
    var hdnEmployeeID = $("#hdnEmployeeID");

    var hdnApplicantId = $("#hdnApplicantId");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var txtEmployeeLastName = $("#txtEmployeeLastName");
    var txtEmployeeCode = $("#txtEmployeeCode");
    var gender = $("#rdoGenderM").is(':checked') ? "M" : "F";
    var txtFatherSpouseName = $("#txtFatherSpouseName");
    var txtDateOfBirth = $("#txtDateOfBirth");
    var ddlMaritalStatus = $("#ddlMaritalStatus");
    var ddlBloodGroup = $("#ddlBloodGroup");
    var txtMobileNo = $("#txtMobileNo");
    var txtEmail = $("#txtEmail");
    var txtAlternateEmail = $("#txtAlternateEmail");
    var txtContactNo = $("#txtContactNo");
    var txtAlternateContactNo = $("#txtAlternateContactNo");
    var txtCAddress = $("#txtCAddress");
    var txtCCity = $("#txtCCity");
    var ddlCCountry = $("#ddlCCountry");
    var ddlCState = $("#ddlCState");
    var txtCPinCode = $("#txtCPinCode");
    var txtPAddress = $("#txtPAddress");
    var txtPCity = $("#txtPCity");
    var ddlPCountry = $("#ddlPCountry");
    var ddlPState = $("#ddlPState");
    var txtPPinCode = $("#txtPPinCode");
    var txtDOJ = $("#txtDOJ");
    var ddlCurrentStatus = $("#ddlCurrentStatus");
    var txtStatusStartDate = $("#txtStatusStartDate");
    var ddlStatusPeriod = $("#ddlStatusPeriod");
    var txtPANNo = $("#txtPANNo");
    var txtAadharNo = $("#txtAadharNo");
    var txtBankDetail = $("#txtBankDetail");
    var txtBankAccountNo = $("#txtBankAccountNo");
    var chkPFApplicable = $("#chkPFApplicable");
    var txtPFNo = $("#txtPFNo");
    var chkESIApplicable = $("#chkESIApplicable");
    var txtESINo = $("#txtESINo");
    var ddlDivision = $("#ddlDivision");
    var ddlEmploymentType = $("#ddlEmploymentType");
    var ddlDepartment = $("#ddlDepartment");
    var ddlDesignation = $("#ddlDesignation");
    var txtDOL = $("#txtDOL");

    var ddlReporingDepartment = $("#ddlReporingDepartment");
    var ddlReportingDesignation = $("#ddlReportingDesignation");
    var txtReportingEmployeeName = $("#txtReportingEmployeeName");
    var hdnReportingEmployeeId = $("#hdnReportingEmployeeId");

    var txtBasicPay = $("#txtBasicPay");
    var txtOTRate = $("#txtOTRate");
    var txtHRA = $("#txtHRA");
    var txtConveyanceAllow = $("#txtConveyanceAllow");
    var txtSpecialAllow = $("#txtSpecialAllow");
    var txtOtherAllow = $("#txtOtherAllow");
    var txtOtherDeduction = $("#txtOtherDeduction");

    var txtMedicalAllow = $("#txtMedicalAllow");
    var txtChildEduAllow = $("#txtChildEduAllow");
    var txtLTA = $("#txtLTA");
    var txtEmployeePF = $("#txtEmployeePF");
    var txtEmployeeESI = $("#txtEmployeeESI");
    var txtProfessionalTax = $("#txtProfessionalTax");
    var txtEmployerPF = $("#txtEmployerPF");
    var txtEmployerESI = $("#txtEmployerESI");

    var txtPayableSalary = $("#txtPayableSalary");
    var txtNetCTC = $("#txtNetCTC");
    var chkStatus = $("#chkStatus").is(':checked') ? true : false;


    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Location")
        return false;
    }
    if (txtEmployeeFirstName.val().trim() == "") {
        ShowModel("Alert", "Please Enter First Name")
        txtEmployeeFirstName.focus();
        return false;
    }
    if (txtEmployeeCode.val().trim() == "") {
        ShowModel("Alert", "Please Enter Employee Code")
        txtEmployeeCode.focus();
        return false;
    }
    if (txtFatherSpouseName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Father/ Spouse name")
        txtFatherSpouseName.focus();
        return false;
    }

    if (txtDateOfBirth.val().trim() == "") {
        ShowModel("Alert", "Please Enter Date Of Birth")
        txtDateOfBirth.focus();
        return false;
    }
    if (ddlMaritalStatus.val() == "" || ddlMaritalStatus.val() == "0") {
        ShowModel("Alert", "Please select Marital Status")
        ddlMaritalStatus.focus();
        return false;
    }
    if (txtMobileNo.val().trim() == "") {
        ShowModel("Alert", "Please enter Mobile No.")
        txtMobileNo.focus();
        return false;
    }
    if (txtMobileNo.val().trim().length < 10) {
        ShowModel("Alert", "Please enter valid Mobile No.")
        txtMobileNo.focus();
        return false;
    }
    if (txtEmail.val().trim() == "") {
        ShowModel("Alert", "Please enter Email ID")
        txtEmail.focus();
        return false;
    }
    if (!ValidEmailCheck(txtEmail.val().trim())) {
        ShowModel("Alert", "Please enter Valid Email Id")
        txtEmail.focus();
        return false;
    }
    if (txtAlternateEmail.val().trim() != "" && !ValidEmailCheck(txtAlternateEmail.val().trim())) {
        ShowModel("Alert", "Please enter Valid Alternate Email Id")
        txtAlternateEmail.focus();
        return false;
    }

    if (txtCAddress.val().trim() == "") {
        ShowModel("Alert", "Please enter Current Address")
        txtCAddress.focus();
        return false;
    }
    if (txtCCity.val().trim() == "") {
        ShowModel("Alert", "Please enter Current City")
        txtCCity.focus();
        return false;
    }
    if (ddlCCountry.val() == "" || ddlCCountry.val() == "0") {
        ShowModel("Alert", "Please select Current Country")
        ddlCCountry.focus();
        return false;
    }
    if (ddlCState.val() == "" || ddlCState.val() == "0") {
        ShowModel("Alert", "Please select Current State")
        ddlCState.focus();
        return false;
    }
    if (txtPAddress.val().trim() == "") {
        ShowModel("Alert", "Please enter Permanent Address")
        txtPAddress.focus();
        return false;
    }
    if (txtPCity.val().trim() == "") {
        ShowModel("Alert", "Please enter Permanent City")
        txtPCity.focus();
        return false;
    }
    if (ddlPCountry.val() == "" || ddlPCountry.val() == "0") {
        ShowModel("Alert", "Please select Permanent Country")
        ddlPCountry.focus();
        return false;
    }
    if (ddlPState.val() == "" || ddlPState.val() == "0") {
        ShowModel("Alert", "Please select Permanent State")
        ddlPState.focus();
        return false;
    }
    if (txtDOJ.val().trim() == "") {
        ShowModel("Alert", "Please enter Date Of Joining")
        txtDOJ.focus();
        return false;
    }

    var pfApplicableStatus = $("#chkPFApplicable").is(':checked') ? true : false;
    var esiApplicableStatus = $("#chkESIApplicable").is(':checked') ? true : false;

    if (pfApplicableStatus == true) {
        if (txtPFNo.val().trim() == "") {
            ShowModel("Alert", "Please enter PF no.")
            txtPFNo.focus();
            return false;
        }
    }
    if (esiApplicableStatus == true) {
        if (txtESINo.val().trim() == "") {
            ShowModel("Alert", "Please enter ESI no.")
            txtESINo.focus();
            return false;
        }
    }

    if (ddlCurrentStatus.val() == "" || ddlCurrentStatus.val() == "0") {
        ShowModel("Alert", "Please select Current Employee Status")
        ddlCurrentStatus.focus();
        return false;
    }
    if (ddlDivision.val() == "" || ddlDivision.val() == "0") {
        ShowModel("Alert", "Please select Employee Division")
        ddlDivision.focus();
        return false;
    }
    if (ddlEmploymentType.val() == "" || ddlEmploymentType.val() == "0") {
        ShowModel("Alert", "Please select Employee Type")
        ddlEmploymentType.focus();
        return false;
    }
    if (ddlDepartment.val() == "" || ddlDepartment.val() == "0") {
        ShowModel("Alert", "Please select Employee Department")
        ddlDepartment.focus();
        return false;
    }
    if (ddlDesignation.val() == "" || ddlDesignation.val() == "0") {
        ShowModel("Alert", "Please select Employee Designation")
        ddlDesignation.focus();
        return false;
    }

    if (txtBasicPay.val().trim() == "") {
        ShowModel("Alert", "Please enter Basic Pay")
        txtBasicPay.focus();
        return false;
    }



    var employeeViewModel = {
        EmployeeId: hdnEmployeeID.val(),
        ApplicantId: hdnApplicantId.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
        EmployeeCode: txtEmployeeCode.val().trim(),
        FirstName: txtEmployeeFirstName.val().trim(),
        LastName: txtEmployeeLastName.val().trim(),
        FatherSpouseName: txtFatherSpouseName.val().trim(),
        Gender: gender,
        DateOfBirth: txtDateOfBirth.val().trim(),
        MaritalStatus: ddlMaritalStatus.val(),
        BloodGroup: ddlBloodGroup.val(),
        Email: txtEmail.val().trim(),
        AlternateEmail: txtAlternateEmail.val().trim(),
        ContactNo: txtContactNo.val().trim(),
        AlternateContactno: txtAlternateContactNo.val().trim(),
        MobileNo: txtMobileNo.val().trim(),
        CAddress: txtCAddress.val().trim(),
        CCity: txtCCity.val().trim(),
        CStateId: ddlCState.val(),
        CCountryId: ddlCCountry.val(),
        CPinCode: txtCPinCode.val().trim(),
        PAddress: txtPAddress.val().trim(),
        PCity: txtPCity.val().trim(),
        PStateId: ddlPState.val(),
        PCountryId: ddlPCountry.val(),
        PPinCode: txtPPinCode.val().trim(),
        DateOfJoin: txtDOJ.val().trim(),
        DateOfLeave: txtDOL.val().trim(),
        PANNo: txtPANNo.val().trim(),
        AadharNo: txtAadharNo.val().trim(),
        BankDetail: txtBankDetail.val().trim(),
        BankAccountNo: txtBankAccountNo.val().trim(),
        PFApplicable: pfApplicableStatus,
        PFNo: txtPFNo.val().trim(),
        ESIApplicable: esiApplicableStatus,
        ESINo: txtESINo.val().trim(),
        Division: ddlDivision.val(),
        DepartmentId: ddlDepartment.val(),
        DesignationId: ddlDesignation.val(),
        EmploymentType: ddlEmploymentType.val(),
        EmployeeCurrentStatus: ddlCurrentStatus.val(),
        EmployeeStatusPeriod: ddlStatusPeriod.val(),
        EmployeeStatusStartDate: txtStatusStartDate.val().trim(),
        OTRate: txtOTRate.val().trim(),
        BasicPay: txtBasicPay.val().trim(),
        HRA: txtHRA.val().trim(),
        ConveyanceAllow: txtConveyanceAllow.val().trim(),
        SpecialAllow: txtSpecialAllow.val().trim(),
        OtherAllow: txtOtherAllow.val().trim(),
        OtherDeduction: txtOtherDeduction.val().trim(),
        MedicalAllow: txtMedicalAllow.val().trim(),
        ChildEduAllow: txtChildEduAllow.val().trim(),
        LTA: txtLTA.val().trim(),
        EmployeePF: txtEmployeePF.val().trim(),
        EmployeeESI: txtEmployeeESI.val().trim(),
        EmployerPF: txtEmployerPF.val().trim(),
        EmployerESI: txtEmployerESI.val().trim(),
        ProfessinalTax: txtProfessionalTax.val().trim(),
        ReportingEmployeeId: hdnReportingEmployeeId.val(),
        Emp_Status: chkStatus

    };
    var requestData = { employeeViewModel: employeeViewModel };
    $.ajax({
        url: "../Employee/AddEditEmployee",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                SaveEmployeeImage(data.trnId);
                ShowModel("Alert", data.message);
                $("#btnUpdate").show();
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

    $("#txtEmployeeFirstName").val("");
    $("#hdnEmployeeID").val("0");
    $("#txtEmployeeLastName").val("");
    $("#txtEmployeeCode").val("");

    $("#rdoGenderM").prop("checked", true);
    $("#rdoGenderF").prop("checked", false);
    //$("#rdoGender").val("M");
    $("#txtFatherSpouseName").val("");
    $("#txtDateOfBirth").val("");
    $("#ddlMaritalStatus").val("0");
    $("#ddlBloodGroup").val("0");
    $("#txtMobileNo").val("");
    $("#txtEmail").val("");
    $("#txtAlternateEmail").val("");
    $("#txtContactNo").val("");
    $("#txtAlternateContactNo").val("");
    $("#txtCAddress").val("");
    $("#txtCCity").val("");
    $("#ddlCCountry").val("0");
    $("#ddlCState").val("0");
    $("#txtCPinCode").val("");
    $("#txtPAddress").val("");
    $("#txtPCity").val("");
    $("#ddlPCountry").val("0");
    $("#ddlPState").val("0");
    $("#txtPPinCode").val("");
    $("#txtDOJ").val("");
    $("#ddlCurrentStatus").val("0");
    $("#txtStatusStartDate").val("");
    $("#ddlStatusPeriod").val("0");
    $("#txtPANNo").val("");
    $("#txtAadharNo").val("");
    $("#txtBankDetail").val("");
    $("#txtBankAccountNo").val("");
    $("#chkPFApplicable").prop("checked", false);
    $("#txtPFNo").val("");
    $("#txtPFNo").attr("disabled", true);
    $("#chkESIApplicable").prop("checked", false);
    $("#txtESINo").val("");
    $("#txtESINo").attr("disabled", true);
    $("#ddlDivision").val("0");
    $("#ddlEmploymentType").val("0");
    $("#ddlDepartment").val("0");
    $("#ddlDesignation").val("0");
    $("#txtDOL").val("");

    $("#ddlReporingDepartment").val("0");
    $("#ddlReportingDesignation").val("0");
    $("#txtReportingEmployeeName").val("");
    $("#hdnReportingEmployeeId").val("0");

    $("#txtBasicPay").val("");
    $("#txtOTRate").val("");
    $("#txtHRA").val("");
    $("#txtConveyanceAllow").val("");
    $("#txtSpecialAllow").val("");
    $("#txtOtherAllow").val("");
    $("#txtOtherDeduction").val("");
    $("#txtPayableSalary").val("");
    document.getElementById('FileUpload1').value = "";
    document.getElementById('imgUserPic').src = "";
    $("#chkStatus").prop("checked", true);
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
function ShowImagePreview(input) {
    var fname = input.value;
    var ext = fname.split(".");
    var x = ext.length;
    var extstr = ext[x - 1].toLowerCase();
    if (extstr == 'jpg' || extstr == 'jpeg' || extstr == 'png' || extstr == 'gif') {
    }
    else {
        alert("File doesnt match png, jpg or gif");
        input.focus();
        input.value = "";
        return false;
    }
    if (typeof (FileReader) != "undefined") {
        if (input.files && input.files[0]) {
            if (input.files[0].name.length < 1) {
            }
            else if (input.files[0].size > 5000000) {
                input.files[0].name.length = 0;
                alert("File is too big");
                input.value = "";
                return false;
            }
            else if (input.files[0].type != 'image/png' && input.files[0].type != 'image/jpg' && !input.files[0].type != 'image/gif' && input.files[0].type != 'image/jpeg') {
                input.files[0].name.length = 0;
                alert("File doesnt match png, jpg or gif");
                input.value = "";
                return false;
            }
            var reader = new FileReader();
            reader.onload = function (e) {
                $("#imgUserPic").prop('src', e.target.result)
                    .width(150)
                    .height(150);
            };
            reader.readAsDataURL(input.files[0]);
        }
    }
    else {
        alert("This browser does not support FileReader.");
        input.value = "";
        //return false;
    }

}
function SaveEmployeeImage(employeeId) {
    if (parseInt(employeeId) <= 0) {
        ShowModel("Alert", "Employee ID not available")
        return false;
    }

    if (window.FormData !== undefined) {
        var uploadfile = document.getElementById('FileUpload1');
        var fileData = new FormData();
        if (uploadfile.value != '') {
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;

            if (uploadfile.files[0].size > 5000000) {
                uploadfile.files[0].name.length = 0;
                ShowModel("Alert", "File is too big")
                uploadfile.value = "";
                return false;
            }

            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
        }
        fileData.append("employeeId", employeeId);

    } else {

        ShowModel("Alert", "FormData is not supported.")
    }

    $.ajax({
        url: "../Employee/UpdateEmployeePic",
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        error: function () {
            ShowModel("Alert", "An error occured")
            return;
        },
        success: function (result) {
            if (result.status == "SUCCESS") {
                document.getElementById('FileUpload1').value = "";
                document.getElementById('imgUserPic').src = "";
            }
            else {
                ShowModel("Alert", result.message);
            }
        }
    });
}

