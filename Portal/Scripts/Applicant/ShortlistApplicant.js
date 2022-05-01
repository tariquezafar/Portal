$(document).ready(function () {
    $("#txtApplicantNo").attr('readonly', true);
    $("#txtApplicationDate").attr('readonly', true);
    $("#txtJobOpeningNo").attr('readonly', true);
    $("#ddlCompanyBranch").attr('disabled', true);
    $("#txtProjectNo").attr('readonly', true);
    $("#ddlResumeSource").attr('disabled', true);
    $("#ddlDesignation").attr('disabled', true);

    $("#txtFirstName").attr('readonly', true);
    $("#txtLastName").attr('readonly', true);

    $("#rdoGenderM").attr('disabled', true);
    $("#rdoGenderF").attr('disabled', true);

    $("#txtFatherSpouseName").attr('readonly', true);
    $("#txtDateOfBirth").attr('readonly', true);
    $("#ddlBloodGroup").attr('disabled', true);
    $("#ddlMaritalStatus").attr('disabled', true);
    $("#txtApplicantAddress").attr('readonly', true);
    $("#txtCity").attr('readonly', true);
    $("#ddlCountry").attr('disabled', true);

    $("#ddlState").attr('disabled', true);
    $("#txtPinCode").attr('readonly', true);
    $("#txtContactNo").attr('readonly', true);
    $("#txtMobileNo").attr('readonly', true);
    $("#txtEmail").attr('readonly', true);

    $("#txtNoticePeriod").attr('readonly', true);
    $("#txtTotalExperience").attr('readonly', true);
    $("#txtReleventExperience").attr('readonly', true);
    $("#txtCurrentCTC").attr('readonly', true);
    $("#txtExpectedCTC").attr('readonly', true);
    $("#txtPreferredLocation").attr('readonly', true);
    $("#txtResumeText").attr('readonly', true);

    $("#txtStrength1").attr('readonly', true);
    $("#txtWeakness1").attr('readonly', true);
    $("#txtStrength2").attr('readonly', true);
    $("#txtWeakness2").attr('readonly', true);
    $("#txtStrength3").attr('readonly', true);
    $("#txtWeakness3").attr('readonly', true);
    $("#txtHobbies").attr('readonly', true);

    $("#txtCreatedBy").attr('readonly', true);
    $("#txtCreatedDate").attr('readonly', true);
    $("#txtModifiedBy").attr('readonly', true);
    $("#txtModifiedDate").attr('readonly', true);
    
    $("#txtVerificationDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        onSelect: function (selected) {
        }
    });
   
  
    $("#tabs").tabs({
        collapsible: true
    });

    BindCompanyBranchList();
    BindResumeSourceList();
    BindPositionList();
    BindCountryList();
    BindVerificationAgencyList();
    
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicantId = $("#hdnApplicantId");
    if (hdnApplicantId.val() != "" && hdnApplicantId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetApplicantDetail(hdnApplicantId.val());
           GetApplicantExtraActivityDetail(hdnApplicantId.val());
           GetApplicantVerificationDetail(hdnApplicantId.val());
       }, 3000);



        if (hdnAccessMode.val() == "3") {
            $("#btnUpdate").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            
        }
        else {
            $("#btnUpdate").show();
        }
    }
    else {
        $("#btnUpdate").hide();
    }
    

    var educations = [];
    GetApplicantEducationList(educations);
    var employers = [];
    GetApplicantPrevEmployerList(employers);
    var projects = [];
    GetApplicantProjectList(projects);
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
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}
function BindResumeSourceList() {
    $("#ddlResumeSource").val(0);
    $("#ddlResumeSource").html("");
    $.ajax({
        type: "GET",
        url: "../Applicant/GetResumeSourceList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlResumeSource").append($("<option></option>").val(0).html("-Select Resume Source-"));
            $.each(data, function (i, item) {
                $("#ddlResumeSource").append($("<option></option>").val(item.ResumeSourceId).html(item.ResumeSourceName));
            });
        },
        error: function (Result) {
            $("#ddlResumeSource").append($("<option></option>").val(0).html("-Select Resume Source-"));
        }
    });
}
function BindPositionList() {
    
    $("#ddlDesignation,#ddlLastDesignation").val(0);
    $("#ddlDesignation,#ddlLastDesignation").html("");
   
        $.ajax({
            type: "GET",
            url: "../Applicant/GetAllDesignationList",
            data: {},
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlDesignation,#ddlLastDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
                $.each(data, function (i, item) {
                    $("#ddlDesignation,#ddlLastDesignation").append($("<option></option>").val(item.DesignationId).html(item.DesignationName));
                });
            },
            error: function (Result) {
                $("#ddlDesignation,#ddlLastDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
            }
        });
   

}
function BindCountryList() {
    $.ajax({
        type: "GET",
        url: "../Company/GetCountryList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCountry").append($("<option></option>").val(0).html("-Select Country-"));
            $.each(data, function (i, item) {

                $("#ddlCountry").append($("<option></option>").val(item.CountryId).html(item.CountryName));
            });
        },
        error: function (Result) {
            $("#ddlCountry").append($("<option></option>").val(0).html("-Select Country-"));
        }
    });
}
function BindStateList(stateId) {
    var countryId = $("#ddlCountry option:selected").val();
    $("#ddlState").val(0);
    $("#ddlState").html("");
    if (countryId != undefined && countryId != "" && countryId != "0") {
        var data = { countryId: countryId };
        $.ajax({
            type: "GET",
            url: "../Company/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                $("#ddlState").val(stateId);
            },
            error: function (Result) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
    }
    else {

        $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    }

}
function BindVerificationAgencyList() {
    $.ajax({
        type: "GET",
        url: "../Applicant/GetVerificationAgencyList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlVerificationAgency").append($("<option></option>").val(0).html("-Select Agency-"));
            $.each(data, function (i, item) {
                $("#ddlVerificationAgency").append($("<option></option>").val(item.VerificationAgencyId).html(item.VerificationAgencyName));
            });
        },
        error: function (Result) {
            $("#ddlVerificationAgency").append($("<option></option>").val(0).html("-Select Agency-"));
        }
    });
}
function GetApplicantEducationList(educations) {
    var hdnApplicantId = $("#hdnApplicantId");
    var requestData = { educations: educations, applicantId: hdnApplicantId.val() };
    $.ajax({
        url: "../Applicant/GetApplicantEducationList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divEducationList").html("");
            $("#divEducationList").html(err);
        },
        success: function (data) {
            $("#divEducationList").html("");
            $("#divEducationList").html(data);
        }
    });
}

function GetApplicantPrevEmployerList(employers) {
    var hdnApplicantId = $("#hdnApplicantId");
    var requestData = { employers: employers, applicantId: hdnApplicantId.val() };
    $.ajax({
        url: "../Applicant/GetApplicantPrevEmployerList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divEmployerList").html("");
            $("#divEmployerList").html(err);
        },
        success: function (data) {
            $("#divEmployerList").html("");
            $("#divEmployerList").html(data);
        }
    });
}

function GetApplicantProjectList(projects) {
    var hdnApplicantId = $("#hdnApplicantId");
    var requestData = { projects: projects, applicantId: hdnApplicantId.val() };
    $.ajax({
        url: "../Applicant/GetApplicantProjectList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divProjectList").html("");
            $("#divProjectList").html(err);
        },
        success: function (data) {
            $("#divProjectList").html("");
            $("#divProjectList").html(data);
            
        }
    });
}


function ClearFields() {

    $("#txtApplicantNo").val("");
    $("#hdnApplicantId").val("0");
    $("#txtApplicationDate").val($("#hdnApplicationDate").val());

    $("#txtJobOpeningNo").val("");
    $("#hdnJobOpeningId").val("0");
    $("#ddlCompanyBranch").val("0");
    $("#txtProjectNo").val("");
    $("#ddlResumeSource").val("0");
    $("#ddlDesignation").val("0");
    $("#divCreated").hide();
    $("#txtCreatedBy").val("");
    $("#txtCreatedDate").val("");

    $("#divModified").hide();
    $("#txtModifiedBy").val("");
    $("#txtModifiedDate").val("");

    $("#divShortlistStatus").hide();
    $("#txtApplicantStortlistStatus").val("");
    

    $("#txtFirstName").val("");
    $("#txtLastName").val("");
    $("#rdoGenderM").prop("checked", true);
    $("#rdoGenderF").prop("checked", false);
    $("#txtFatherSpouseName").val("");
    $("#txtDateOfBirth").val("");
    $("#ddlBloodGroup").val("0");
    $("#ddlMaritalStatus").val("0");
    $("#txtApplicantAddress").val("");
    $("#txtCity").val("");
    $("#ddlCountry").val("0");
    $("#ddlState").val("0");
    $("#txtPinCode").val("");
    $("#txtContactNo").val("");
    $("#txtMobileNo").val("");
    $("#txtEmail").val("");
    $("#txtNoticePeriod").val("");
    $("#txtTotalExperience").val("");
    $("#txtReleventExperience").val("");
    $("#txtCurrentCTC").val("");
    $("#txtExpectedCTC").val("");
    $("#txtPreferredLocation").val("");
    $("#txtResumeText").val("");

    $("#txtStrength1").val("");
    $("#txtWeakness1").val("");
    $("#txtStrength2").val("");
    $("#txtWeakness2").val("");
    $("#txtStrength3").val("");
    $("#txtWeakness3").val("");
    $("#txtHobbies").val("");

    $("#ddlVerificationAgency").val("0");
    $("#txtVerificationDate").val("");
    $("#txtVerificationCharges").val("");
    $("#ddlVerificationStatus").val("0");
    $("#txtRemarks").val("");

    $("#ddlApplicantStatus").val("Final");

    $("#divEducationList").html("");
    $("#divEmployerList").html("");
    $("#divProjectList").html("");

    $("#btnSave").show();
    $("#btnUpdate").hide();


}
function GetApplicantDetail(applicantId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Applicant/GetApplicantDetail",
        data: { applicantId: applicantId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicantNo").val(data.ApplicantNo);
            $("#hdnApplicantId").val(data.ApplicantId);
            $("#txtApplicationDate").val(data.ApplicationDate);
            $("#txtJobOpeningNo").val(data.JobOpeningNo);
            $("#hdnJobOpeningId").val(data.JobOpeningId);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
           
            $("#txtProjectNo").val(data.ProjectNo);
            $("#ddlResumeSource").val(data.ResumeSourceId);
            $("#ddlDesignation").val(data.PositionAppliedId);
            $("#txtFirstName").val(data.FirstName);
            $("#txtLastName").val(data.LastName);
            if (data.Gender == "M")
            {
                $("#rdoGenderM").prop("checked", true);
                $("#rdoGenderF").prop("checked", false);
            }
            else
            {
                $("#rdoGenderM").prop("checked", false);
                $("#rdoGenderF").prop("checked", true);
            }
            $("#txtFatherSpouseName").val(data.FatherSpouseName);
            $("#txtDateOfBirth").val(data.DOB);
            $("#ddlBloodGroup").val(data.BloodGroup);
            $("#ddlMaritalStatus").val(data.MaritalStatus);
            $("#txtApplicantAddress").val(data.ApplicantAddress);
            $("#txtCity").val(data.City);
            $("#ddlCountry").val(data.CountryId);
            BindStateList(data.StateId);
            $("#ddlState").val(data.StateId);
            $("#txtPinCode").val(data.PinCode);
            $("#txtContactNo").val(data.ContactNo);
            $("#txtMobileNo").val(data.MobileNo);
            $("#txtEmail").val(data.Email);
            $("#txtNoticePeriod").val(data.NoticePeriod);
            $("#txtTotalExperience").val(data.TotalExperience);
            $("#txtReleventExperience").val(data.ReleventExperience);
            $("#txtCurrentCTC").val(data.CurrentCTC);
            $("#txtExpectedCTC").val(data.ExpectedCTC);
            $("#txtPreferredLocation").val(data.PreferredLocation);
            $("#txtResumeText").val(data.ResumeText);

            $("#ddlApplicantStatus").val(data.ApplicantStortlistStatus);

            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);

            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function GetApplicantExtraActivityDetail(applicantId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Applicant/GetApplicantExtraActivityDetail",
        data: { applicantId: applicantId },
        dataType: "json",
        success: function (data) {
            $("#txtStrength1").val(data.Strength1);
            $("#txtStrength2").val(data.Strength2);
            $("#txtStrength3").val(data.Strength3);
            $("#txtWeakness1").val(data.Weakness1);
            $("#txtWeakness2").val(data.Weakness2);
            $("#txtWeakness3").val(data.Weakness3);
            $("#txtHobbies").val(data.Hobbies);
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function GetApplicantVerificationDetail(applicantId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Applicant/GetApplicantVerificationDetail",
        data: { applicantId: applicantId },
        dataType: "json",
        success: function (data) {
            $("#ddlVerificationAgency").val(data.VerificationAgencyId);
            $("#txtVerificationDate").val(data.VerificationDate);
            $("#txtVerificationCharges").val(data.VerificationCharges);
            $("#ddlVerificationStatus").val(data.VerificationStatus);
            $("#txtRemarks").val(data.Remarks);
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() {
    var hdnApplicantId = $("#hdnApplicantId");
    var ddlVerificationAgency = $("#ddlVerificationAgency");
    var txtVerificationDate = $("#txtVerificationDate");
    var txtVerificationCharges = $("#txtVerificationCharges");
    var ddlVerificationStatus = $("#ddlVerificationStatus");
    var txtRemarks = $("#txtRemarks");
    var ddlApplicantStatus = $("#ddlApplicantStatus");

    if (ddlVerificationAgency.val() == "" || ddlVerificationAgency.val() == "0") {
        ShowModel("Alert", "Please select Verification Agency")
        return false;
    }
    if (txtVerificationDate.val().trim() == "") {
        ShowModel("Alert", "Please select Verification Date")
        return false;
    }
    if (ddlVerificationStatus.val() == "" || ddlVerificationStatus.val() == "0") {
        ShowModel("Alert", "Please select Verification Status")
        return false;
    }
    if (txtRemarks.val() == "") {
        ShowModel("Alert", "Please enter Verification and Shortlist Remarks")
        return false;
    }
    if (ddlApplicantStatus.val() == "" || ddlApplicantStatus.val() == "0") {
        ShowModel("Alert", "Please select Shortlist Status")
        return false;
    }
    var applicantViewModel = {
        ApplicantId: hdnApplicantId.val(),
        ApplicantStortlistStatus: ddlApplicantStatus.val()
    };
    var verificationViewModel = {
        ApplicantVerificationId: 0,
        VerificationAgencyId: ddlVerificationAgency.val(),
        VerificationDate: txtVerificationDate.val().trim(),
        VerificationCharges: txtVerificationCharges.val().trim(),
        VerificationStatus: ddlVerificationStatus.val(),
        Remarks: txtRemarks.val()
    };

    var requestData = {
        applicant: applicantViewModel,
        verification: verificationViewModel
    };
    $.ajax({
        url: "../Applicant/ShortlistApplicant",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
                   function () {
                       window.location.href = "../Applicant/ListShortlistApplicant";
                   }, 2000);

                $("#btnUpdate").hide();
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
