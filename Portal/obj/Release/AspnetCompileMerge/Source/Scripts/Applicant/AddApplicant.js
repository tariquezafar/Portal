$(document).ready(function () {
    $("#txtApplicantNo").attr('readonly', true);
    $("#txtApplicationDate").attr('readonly', true);
    $("#txtJobOpeningNo").attr('readonly', true);

    $("#txtCreatedBy").attr('readonly', true);
    $("#txtCreatedDate").attr('readonly', true);
    $("#txtModifiedBy").attr('readonly', true);
    $("#txtModifiedDate").attr('readonly', true);
    $("#txtApplicantStortlistStatus").attr('readonly', true);

    $("#txtDateOfBirth").attr('readonly', true);
    $("#txtJobStartDate").attr('readonly', true);
    $("#txtJobEndDate").attr('readonly', true);
    $("#txtJobFromDate").attr('readonly', true);
    $("#txtJobToDate").attr('readonly', true);
    
    $("#txtApplicationDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        maxDate: '0D',
        onSelect: function (selected) {
        }
    });
    $("#txtDateOfBirth").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        maxDate: '0D',
        onSelect: function (selected) {
        }
    });

    $("#txtJobStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        maxDate: '0D',
        onSelect: function (selected) {
        }
    });
    $("#txtJobEndDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        maxDate: '0D',
        onSelect: function (selected) {
        }
    });
    $("#txtJobFromDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',       
        onSelect: function (selected) {
        }
    });
    $("#txtJobToDate").datepicker({
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
    BindEducationList();
    
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicantId = $("#hdnApplicantId");
    if (hdnApplicantId.val() != "" && hdnApplicantId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetApplicantDetail(hdnApplicantId.val());
           GetApplicantExtraActivityDetail(hdnApplicantId.val());
       }, 3000);



        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            
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
function BindEducationList() {
    $("#ddlEducation").val(0);
    $("#ddlEducation").html("");
    $.ajax({
        type: "GET",
        url: "../Applicant/GetEducationList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlEducation").append($("<option></option>").val(0).html("-Select Education-"));
            $.each(data, function (i, item) {
                $("#ddlEducation").append($("<option></option>").val(item.EducationId).html(item.EducationName));
            });
        },
        error: function (Result) {
            $("#ddlEducation").append($("<option></option>").val(0).html("-Select Education-"));
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

            ShowHideEducationPanel(2);
        }
    });
}
function AddEducation(action) {
    var educationEntrySequence = 0;
    var hdnEducationSequenceNo = $("#hdnEducationSequenceNo");
    var hdnApplicantEducationId = $("#hdnApplicantEducationId");
    var ddlEducation = $("#ddlEducation");
    var ddlRegularDistant = $("#ddlRegularDistant");
    var txtBoardName = $("#txtBoardName");
    var txtPercentageObtained = $("#txtPercentageObtained");

    if (ddlEducation.val() == "" || ddlEducation.val()=="0") {
        ShowModel("Alert", "Please select Education")
        return false;
    }
    if (ddlRegularDistant.val() == "" || ddlRegularDistant.val() == "0") {
        ShowModel("Alert", "Please select Regular/Distant")
        return false;
    }
    if (txtBoardName.val().trim() == "") {
        ShowModel("Alert", "Please enter Board/ University Name")
        return false;
    }
    if (txtPercentageObtained.val().trim() == "" || parseFloat(txtPercentageObtained.val().trim())<=0) {
        ShowModel("Alert", "Please enter correct percentage")
        return false;
    }
    var educationList = [];
    if (action == 1 && (hdnEducationSequenceNo.val() == "" || hdnEducationSequenceNo.val() == "0")) {
        educationEntrySequence = 1;
    }
    $('#tblEducationList tr').each(function (i, row) {
        var $row = $(row);
        var educationSequenceNo = $row.find("#hdnEducationSequenceNo").val();
        var applicantEducationId = $row.find("#hdnApplicantEducationId").val();
        var educationId = $row.find("#hdnEducationId").val();
        var educationName = $row.find("#hdnEducationName").val();
        var regularDistant = $row.find("#hdnRegularDistant").val();
        var boardUniversityName = $row.find("#hdnBoardUniversityName").val();
        var percentageObtained = $row.find("#hdnPercentageObtained").val();

        if (educationId != undefined) {
            if (action == 1 || (hdnEducationSequenceNo.val() != educationSequenceNo)) {

                if (educationId == ddlEducation.val()) {
                    ShowModel("Alert", "Education already added!!!")
                    return false;
                }
                var education = {
                    ApplicantEducationId: applicantEducationId,
                    EducationSequenceNo: educationSequenceNo,
                    EducationId: educationId,
                    EducationName: educationName,
                    RegularDistant: regularDistant,
                    BoardUniversityName: boardUniversityName,
                    PercentageObtained: percentageObtained
                };
                educationList.push(education);
                educationEntrySequence = parseInt(educationEntrySequence) + 1;
            }
            else if (hdnEducationSequenceNo.val() == educationSequenceNo) {
                var educationAddEdit = {
                    ApplicantEducationId: hdnApplicantEducationId.val(),
                    EducationSequenceNo: hdnEducationSequenceNo.val(),
                    EducationId: ddlEducation.val(),
                    EducationName: $("#ddlEducation option:selected").text(),
                    RegularDistant: ddlRegularDistant.val(),
                    BoardUniversityName: txtBoardName.val(),
                    PercentageObtained: txtPercentageObtained.val()
                };
                educationList.push(educationAddEdit);
                hdnEducationSequenceNo.val("0");
            }
        }
    });
    if (action == 1 && (hdnEducationSequenceNo.val() == "" || hdnEducationSequenceNo.val() == "0")) {
        hdnEducationSequenceNo.val(educationEntrySequence);
    }
    if (action == 1) {
        var educationAddEdit = {
            ApplicantEducationId: hdnApplicantEducationId.val(),
            EducationSequenceNo: hdnEducationSequenceNo.val(),
            EducationId: ddlEducation.val(),
            EducationName: $("#ddlEducation option:selected").text(),
            RegularDistant: ddlRegularDistant.val(),
            BoardUniversityName: txtBoardName.val(),
            PercentageObtained: txtPercentageObtained.val()
        };
        educationList.push(educationAddEdit);
        hdnEducationSequenceNo.val("0");
    }
    GetApplicantEducationList(educationList);
}
function EditEducationRow(obj) {
    var row = $(obj).closest("tr");
    var educationSequenceNo = $(row).find("#hdnEducationSequenceNo").val();
    var applicantEducationId = $(row).find("#hdnApplicantEducationId").val();
    var educationId = $(row).find("#hdnEducationId").val();
    var educationName = $(row).find("#hdnEducationName").val();
    var regularDistant = $(row).find("#hdnRegularDistant").val();
    var boardUniversityName = $(row).find("#hdnBoardUniversityName").val();
    var percentageObtained = $(row).find("#hdnPercentageObtained").val();


    $("#hdnEducationSequenceNo").val(educationSequenceNo);
    $("#hdnApplicantEducationId").val(applicantEducationId);
    $("#ddlEducation").val(educationId);
    $("#ddlRegularDistant").val(regularDistant);
    $("#txtBoardName").val(boardUniversityName);
    $("#txtPercentageObtained").val(percentageObtained);

    $("#btnAddEducation").hide();
    $("#btnUpdateEducation").show();
    ShowHideEducationPanel(1);
}
function RemoveEducationRow(obj) {
    if (confirm("Do you want to remove selected Education?")) {
        var row = $(obj).closest("tr");
        var education = $(row).find("#hdnEducationId").val();
        ShowModel("Alert", "Education Removed from List.");
        row.remove();

    }
}
function ShowHideEducationPanel(action) {
    if (action == 1) {
        $(".educationsection").show();
    }
    else {
        $(".educationsection").hide();
        $("#hdnEducationSequenceNo").val("0");
        $("#hdnApplicantEducationId").val("0");
        $("#ddlEducation").val("0");
        $("#ddlRegularDistant").val("0");
        $("#txtBoardName").val("");
        $("#txtPercentageObtained").val("0");

        $("#btnAddEducation").show();
        $("#btnUpdateEducation").hide();
    }
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
            ShowHideEmployerPanel(2);
        }
    });
}
function AddEmployer(action) {
    var employerEntrySequence = 0;
    var hdnEmployerSequenceNo = $("#hdnEmployerSequenceNo");
    var hdnApplicantPrevEmployerId = $("#hdnApplicantPrevEmployerId");
    var chkCurrentEmployer = $("#chkCurrentEmployer").is(':checked')?true:false;
    var txtEmployerName = $("#txtEmployerName");
    var txtJobStartDate = $("#txtJobStartDate");
    var txtJobEndDate = $("#txtJobEndDate");
    var txtLastCTC = $("#txtLastCTC");
    var txtReasonOfLeaving = $("#txtReasonOfLeaving");
    var ddlLastDesignation = $("#ddlLastDesignation");
    var ddlEmploymentStatus = $("#ddlEmploymentStatus");

    if (txtEmployerName.val().trim() == "") {
        ShowModel("Alert", "Please enter Employer Name")
        return false;
    }
    if (txtJobStartDate.val().trim() == "" ) {
        ShowModel("Alert", "Please select Job Start date")
        return false;
    }
    if (txtJobEndDate.val().trim() == "") {
        ShowModel("Alert", "Please select Job End Date")
        return false;
    }
    if (txtLastCTC.val().trim() == "" || parseFloat(txtLastCTC.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter correct Last CTC")
        return false;
    }
    if (txtReasonOfLeaving.val().trim() == "") {
        ShowModel("Alert", "Please enter reason of leaving")
        return false;
    }
    if (ddlLastDesignation.val() == "" || ddlLastDesignation.val() == "0") {
        ShowModel("Alert", "Please select last designation")
        return false;
    }
    if (ddlEmploymentStatus.val() == "" || ddlEmploymentStatus.val() == "0") {
        ShowModel("Alert", "Please select employment type")
        return false;
    }

    var employerList = [];
    if (action == 1 && (hdnEmployerSequenceNo.val() == "" || hdnEmployerSequenceNo.val() == "0")) {
        employerEntrySequence = 1;
    }
    $('#tblEmployerList tr').each(function (i, row) {
        var $row = $(row);
        var employerSequenceNo = $row.find("#hdnEmployerSequenceNo").val();
        var applicantPrevEmployerId = $row.find("#hdnApplicantPrevEmployerId").val();
        var currentEmployer = $row.find("#hdnCurrentEmployer").val()=="Yes"?true:false;
        var employerName = $row.find("#hdnEmployerName").val();
        var startDate = $row.find("#hdnStartDate").val();
        var endDate = $row.find("#hdnEndDate").val();
        var lastCTC = $row.find("#hdnLastCTC").val();
        var reasonOfLeaving = $row.find("#hdnReasonOfLeaving").val();
        var lastDesignationName = $row.find("#hdnLastDesignationName").val();
        var lastDesignationId = $row.find("#hdnLastDesignationId").val();
        var employmentStatusName = $row.find("#hdnEmploymentStatusName").val();
        var employmentStatusId = $row.find("#hdnEmploymentStatusId").val();
        if (employerName != undefined) {
            if (action == 1 || (hdnEmployerSequenceNo.val() != employerSequenceNo)) {

                if (employerName == txtEmployerName.val()) {
                    ShowModel("Alert", "Employer already added!!!")
                    return false;
                }
                var employer = {
                    ApplicantPrevEmployerId: applicantPrevEmployerId,
                    EmployerSequenceNo: employerSequenceNo,
                    CurrentEmployer: currentEmployer,
                    EmployerName: employerName,
                    StartDate: startDate,
                    EndDate: endDate,
                    LastCTC: lastCTC,
                    ReasonOfLeaving: reasonOfLeaving,
                    LastDesignationId: lastDesignationId,
                    LastDesignationName: lastDesignationName,
                    EmploymentStatusId: employmentStatusId,
                    EmploymentStatusName: employmentStatusName
                };
                employerList.push(employer);
                employerEntrySequence = parseInt(employerEntrySequence) + 1;
            }
            else if (hdnEmployerSequenceNo.val() == employerSequenceNo) {
                var employerAddEdit = {
                    ApplicantPrevEmployerId: hdnApplicantPrevEmployerId.val(),
                    EmployerSequenceNo: hdnEmployerSequenceNo.val(),
                    CurrentEmployer: chkCurrentEmployer,
                    EmployerName: txtEmployerName.val(),
                    StartDate: txtJobStartDate.val(),
                    EndDate: txtJobEndDate.val(),
                    LastCTC: txtLastCTC.val(),
                    ReasonOfLeaving: txtReasonOfLeaving.val(),
                    LastDesignationId: ddlLastDesignation.val(),
                    LastDesignationName: $("#ddlLastDesignation option:selected").text(),
                    EmploymentStatusId: ddlEmploymentStatus.val(),
                    EmploymentStatusName: $("#ddlEmploymentStatus option:selected").text()
                };
                employerList.push(employerAddEdit);
                hdnEmployerSequenceNo.val("0");
            }
        }
    });
    if (action == 1 && (hdnEmployerSequenceNo.val() == "" || hdnEmployerSequenceNo.val() == "0")) {
        hdnEmployerSequenceNo.val(employerEntrySequence);
    }
    if (action == 1) {
        var employerAddEdit = {
            ApplicantPrevEmployerId: hdnApplicantPrevEmployerId.val(),
            EmployerSequenceNo: hdnEmployerSequenceNo.val(),
            CurrentEmployer: chkCurrentEmployer,
            EmployerName: txtEmployerName.val(),
            StartDate: txtJobStartDate.val(),
            EndDate: txtJobEndDate.val(),
            LastCTC: txtLastCTC.val(),
            ReasonOfLeaving: txtReasonOfLeaving.val(),
            LastDesignationId: ddlLastDesignation.val(),
            LastDesignationName: $("#ddlLastDesignation option:selected").text(),
            EmploymentStatusId: ddlEmploymentStatus.val(),
            EmploymentStatusName: $("#ddlEmploymentStatus option:selected").text()
        };
        employerList.push(employerAddEdit);
        hdnEmployerSequenceNo.val("0");
    }
    GetApplicantPrevEmployerList(employerList);
}
function EditEmployerRow(obj) {
    var row = $(obj).closest("tr");
    var employerSequenceNo = $(row).find("#hdnEmployerSequenceNo").val();
    var applicantPrevEmployerId = $(row).find("#hdnApplicantPrevEmployerId").val();
    var currentEmployer = $(row).find("#hdnCurrentEmployer").val() == "Yes" ? true : false;
    var employerName = $(row).find("#hdnEmployerName").val();
    var startDate = $(row).find("#hdnStartDate").val();
    var endDate = $(row).find("#hdnEndDate").val();
    var lastCTC = $(row).find("#hdnLastCTC").val();
    var reasonOfLeaving = $(row).find("#hdnReasonOfLeaving").val();
    var lastDesignationId = $(row).find("#hdnLastDesignationId").val();
    var employmentStatusId = $(row).find("#hdnEmploymentStatusId").val();
    $("#hdnEmployerSequenceNo").val(employerSequenceNo);
    $("#hdnApplicantPrevEmployerId").val(applicantPrevEmployerId);
    if (currentEmployer == true)
    {$('#chkCurrentEmployer').prop('checked', true);}
    else { $('#chkCurrentEmployer').prop('checked', false);}
    $("#txtEmployerName").val(employerName);
    $("#txtJobStartDate").val(startDate);
    $("#txtJobEndDate").val(endDate);
    $("#txtLastCTC").val(lastCTC);

    $("#txtReasonOfLeaving").val(reasonOfLeaving);
    $("#ddlLastDesignation").val(lastDesignationId);
    $("#ddlEmploymentStatus").val(employmentStatusId);

    $("#btnAddEmployer").hide();
    $("#btnUpdateEmployer").show();
    ShowHideEmployerPanel(1);
}
function RemoveEmployerRow(obj) {
    if (confirm("Do you want to remove selected Employer?")) {
        var row = $(obj).closest("tr");
        var employerName = $(row).find("#txtEmployerName").val();
        ShowModel("Alert", "Employer Removed from List.");
        row.remove();

    }
}
function ShowHideEmployerPanel(action) {
    if (action == 1) {
        $(".employersection").show();
    }
    else {
        $(".employersection").hide();
        $("#hdnEmployerSequenceNo").val("0");
        $("#hdnApplicantPrevEmployerId").val("0");
        $('#chkCurrentEmployer').prop('checked', false);
        $("#txtEmployerName").val("");
        $("#txtJobStartDate").val("");
        $("#txtJobEndDate").val("");
        $("#txtLastCTC").val("");
        $("#txtReasonOfLeaving").val("");
        $("#ddlLastDesignation").val("0");
        $("#ddlEmploymentStatus").val("0");

        $("#btnAddEmployer").show();
        $("#btnUpdateEmployer").hide();
    }
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
            ShowHideProjectPanel(2);
        }
    });
}
function AddProject(action) {
    var projectEntrySequence = 0;
    var hdnProjectSequenceNo = $("#hdnProjectSequenceNo");
    var hdnApplicantProjectId = $("#hdnApplicantProjectId");
    var txtProjectName = $("#txtProjectName");
    var txtClientName = $("#txtClientName");
    var txtRoleDesc = $("#txtRoleDesc");
    var txtTeamSize = $("#txtTeamSize");
    var txtProjectDesc = $("#txtProjectDesc");
    var txtTechnologyUsed = $("#txtTechnologyUsed");
    if (txtProjectName.val().trim() == "") {
        ShowModel("Alert", "Please enter Project Name")
        return false;
    }
    if (txtTeamSize.val().trim() == "" || parseFloat(txtTeamSize.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter correct Team Size")
        return false;
    }
    var projectList = [];
    if (action == 1 && (hdnProjectSequenceNo.val() == "" || hdnProjectSequenceNo.val() == "0")) {
        projectEntrySequence = 1;
    }
    $('#tblProjectList tr').each(function (i, row) {
        var $row = $(row);
        var projectSequenceNo = $row.find("#hdnProjectSequenceNo").val();
        var applicantProjectId = $row.find("#hdnApplicantProjectId").val();
        var projectName = $row.find("#hdnProjectName").val();
        var clientName = $row.find("#hdnClientName").val();
        var roleDesc = $row.find("#hdnRoleDesc").val();
        var teamSize = $row.find("#hdnTeamSize").val();
        var projectDesc = $row.find("#hdnProjectDesc").val();
        var technologiesUsed = $row.find("#hdnTechnologiesUsed").val();
        if (projectName != undefined) {
            if (action == 1 || (hdnProjectSequenceNo.val() != projectSequenceNo)) {

                if (projectName == txtProjectName.val()) {
                    ShowModel("Alert", "Project already added!!!")
                    return false;
                }
                var project = {
                    ApplicantProjectId: applicantProjectId,
                    ProjectSequenceNo: projectSequenceNo,
                    ProjectName: projectName,
                    ClientName: clientName,
                    RoleDesc: roleDesc,
                    TeamSize: teamSize,
                    ProjectDesc: projectDesc,
                    TechnologiesUsed: technologiesUsed
                };
                projectList.push(project);
                projectEntrySequence = parseInt(projectEntrySequence) + 1;
            }
            else if (hdnProjectSequenceNo.val() == projectSequenceNo) {
                var projectAddEdit = {
                    ApplicantProjectId: hdnApplicantProjectId.val(),
                    ProjectSequenceNo: hdnProjectSequenceNo.val(),
                    ProjectName: txtProjectName.val().trim(),
                    ClientName: txtClientName.val().trim(),
                    RoleDesc: txtRoleDesc.val().trim(),
                    TeamSize: txtTeamSize.val().trim(),
                    ProjectDesc: txtProjectDesc.val().trim(),
                    TechnologiesUsed: txtTechnologyUsed.val().trim()
                };
                projectList.push(projectAddEdit);
                hdnProjectSequenceNo.val("0");
            }
        }
    });
    if (action == 1 && (hdnProjectSequenceNo.val() == "" || hdnProjectSequenceNo.val() == "0")) {
        hdnProjectSequenceNo.val(projectEntrySequence);
    }
    if (action == 1) {
        var projectAddEdit = {
            ApplicantProjectId: hdnApplicantProjectId.val(),
            ProjectSequenceNo: hdnProjectSequenceNo.val(),
            ProjectName: txtProjectName.val().trim(),
            ClientName: txtClientName.val().trim(),
            RoleDesc: txtRoleDesc.val().trim(),
            TeamSize: txtTeamSize.val().trim(),
            ProjectDesc: txtProjectDesc.val().trim(),
            TechnologiesUsed: txtTechnologyUsed.val().trim()
        };
        projectList.push(projectAddEdit);
        hdnProjectSequenceNo.val("0");
    }
    GetApplicantProjectList(projectList);
}
function EditProjectRow(obj) {
    var row = $(obj).closest("tr");
    var projectSequenceNo = $(row).find("#hdnProjectSequenceNo").val();
    var applicantProjectId = $(row).find("#hdnApplicantProjectId").val();
    var projectName = $(row).find("#hdnProjectName").val();
    var clientName = $(row).find("#hdnClientName").val();
    var roleDesc = $(row).find("#hdnRoleDesc").val();
    var teamSize = $(row).find("#hdnTeamSize").val();
    var projectDesc = $(row).find("#hdnProjectDesc").val();
    var technologiesUsed = $(row).find("#hdnTechnologiesUsed").val();

    $("#hdnProjectSequenceNo").val(projectSequenceNo);
    $("#hdnApplicantProjectId").val(applicantProjectId);
    $("#txtProjectName").val(projectName);
    $("#txtClientName").val(clientName);
    $("#txtRoleDesc").val(roleDesc);
    $("#txtTeamSize").val(teamSize);
    $("#txtProjectDesc").val(projectDesc);
    $("#txtTechnologyUsed").val(technologiesUsed);
    

    $("#btnAddProject").hide();
    $("#btnUpdateProject").show();
    ShowHideProjectPanel(1);
}
function RemoveProjectRow(obj) {
    if (confirm("Do you want to remove selected Project?")) {
        var row = $(obj).closest("tr");
        var projectName = $(row).find("#txtProjectName").val();
        ShowModel("Alert", "Project Removed from List.");
        row.remove();

    }
}
function ShowHideProjectPanel(action) {
    if (action == 1) {
        $(".projectsection").show();
    }
    else {
        $(".projectsection").hide();
        $("#hdnProjectSequenceNo").val("0");
        $("#hdnApplicantProjectId").val("0");
        $("#txtProjectName").val("");
        $("#txtClientName").val("");
        $("#txtRoleDesc").val("");
        $("#txtTeamSize").val("");
        $("#txtProjectDesc").val("");
        $("#txtTechnologyUsed").val("0");
        
        $("#btnAddProject").show();
        $("#btnUpdateProject").hide();
    }
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
            $("#ddlApplicantStatus").val(data.ApplicantStatus);
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);

            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            if (data.ApplicantStortlistStatus != "" && data.ApplicantStortlistStatus != "0") {
                $("#divShortlistStatus").show();
                $("#txtApplicantStortlistStatus").val(data.ApplicantStortlistStatus);
                
            }


            $("#btnAddNew").show();

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
function SaveData() {
    var txtApplicantNo = $("#txtApplicantNo");
    var hdnApplicantId = $("#hdnApplicantId");
    var txtApplicationDate = $("#txtApplicationDate");

    var txtJobOpeningNo = $("#txtJobOpeningNo");
    var hdnJobOpeningId = $("#hdnJobOpeningId");

    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtProjectNo = $("#txtProjectNo");

    var ddlResumeSource = $("#ddlResumeSource");

    var ddlDesignation = $("#ddlDesignation");
    var txtFirstName = $("#txtFirstName");
    var txtLastName = $("#txtLastName");
    var rdoGenderM = $("#rdoGenderM");
    var txtFatherSpouseName = $("#txtFatherSpouseName");
    var txtDateOfBirth = $("#txtDateOfBirth");

    var ddlBloodGroup = $("#ddlBloodGroup");
    var ddlMaritalStatus = $("#ddlMaritalStatus");
    var txtApplicantAddress = $("#txtApplicantAddress");
    var txtCity  = $("#txtCity");
    var ddlCountry = $("#ddlCountry");
    var ddlState = $("#ddlState");
    var txtPinCode = $("#txtPinCode");
    var txtContactNo = $("#txtContactNo");
    var txtMobileNo = $("#txtMobileNo");
    var txtEmail = $("#txtEmail");
    var txtNoticePeriod = $("#txtNoticePeriod");
    var txtTotalExperience = $("#txtTotalExperience");
    var txtReleventExperience = $("#txtReleventExperience");
    var txtCurrentCTC = $("#txtCurrentCTC");
    var txtExpectedCTC = $("#txtExpectedCTC");
    var txtPreferredLocation = $("#txtPreferredLocation");
    var txtResumeText = $("#txtResumeText");

    var txtStrength1 = $("#txtStrength1");
    var txtWeakness1 = $("#txtWeakness1");
    var txtStrength2 = $("#txtStrength2");
    var txtWeakness2 = $("#txtWeakness2");
    var txtStrength3 = $("#txtStrength3");
    var txtWeakness3 = $("#txtWeakness3");
    var txtHobbies = $("#txtHobbies");
    var ddlApplicantStatus = $("#ddlApplicantStatus");

    if (txtJobOpeningNo.val().trim() == "") {
        ShowModel("Alert", "Please select Job Opening No.")
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Location")
        return false;
    }
    if (ddlResumeSource.val() == "" || ddlResumeSource.val() == "0") {
        ShowModel("Alert", "Please select Resume Source")
        return false;
    }
    if (ddlDesignation.val() == "" || ddlDesignation.val() == "0") {
        ShowModel("Alert", "Please select Position Applied")
        return false;
    }
    if (txtFirstName.val() == "") {
        ShowModel("Alert", "Please enter First name")
        return false;
    }
    if (txtFatherSpouseName.val() == "") {
        ShowModel("Alert", "Please enter Father/ Spouse name")
        return false;
    }
    if (txtDateOfBirth.val() == "") {
        ShowModel("Alert", "Please select Date of Birth")
        return false;
    }
    if (ddlMaritalStatus.val() == "" || ddlMaritalStatus.val() == "0") {
        ShowModel("Alert", "Please select Matital Status")
        return false;
    }

    if (txtApplicantAddress.val().trim() == "" ) {
        ShowModel("Alert", "Please enter Address")
        return false;
    }
    if (txtCity.val().trim() == "") {
        ShowModel("Alert", "Please enter City")
        return false;
    }
    if (ddlCountry.val().trim() == "" || ddlCountry.val().trim() == "0") {
        ShowModel("Alert", "Please select Country")
        return false;
    }
    if (ddlState.val().trim() == "" || ddlState.val().trim() == "0") {
        ShowModel("Alert", "Please select State")
        return false;
    }
    if (txtMobileNo.val().trim() == "") {
        ShowModel("Alert", "Please enter Mobile No.")
        return false;
    }
    if (txtMobileNo.val().trim().length < 10) {
        ShowModel("Alert", "Please enter valid Mobile No.")
        return false;
    }
    if (txtEmail.val().trim() == "") {
        ShowModel("Alert", "Please enter Email ID")
        return false;
    }
    if (!ValidEmailCheck(txtEmail.val().trim())) {
        ShowModel("Alert", "Please enter Valid Email Id")
        return false;
    }
    if (txtNoticePeriod.val().trim() == "" || parseInt(txtNoticePeriod.val().trim())<=0 ) {
        ShowModel("Alert", "Please enter correct Notice Period")
        return false;
    }
    if (txtTotalExperience.val().trim() == "" || parseInt(txtTotalExperience.val().trim()) < 0) {
        ShowModel("Alert", "Please enter correct Total Experience")
        return false;
    }
    if (txtReleventExperience.val().trim() == "" || parseInt(txtReleventExperience.val().trim()) < 0) {
        ShowModel("Alert", "Please enter correct Relevant Experience")
        return false;
    }
    if (txtCurrentCTC.val().trim() == "" || parseInt(txtCurrentCTC.val().trim()) < 0) {
        ShowModel("Alert", "Please enter correct Current CTC")
        return false;
    }
    if (txtExpectedCTC.val().trim() == "" || parseInt(txtExpectedCTC.val().trim()) < 0) {
        ShowModel("Alert", "Please enter correct Expected CTC")
        return false;
    }

    var applicantViewModel = {
        ApplicantId: hdnApplicantId.val(),
        ApplicantNo: txtApplicantNo.val().trim(),
        ApplicationDate: txtApplicationDate.val().trim(),
        JobOpeningId: hdnJobOpeningId.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
        ProjectNo: txtProjectNo.val(),
        ResumeSourceId: ddlResumeSource.val(),
        PositionAppliedId:ddlDesignation.val(),
        FirstName: txtFirstName.val(),
        LastName: txtLastName.val(),
        Gender: rdoGenderM.is(':checked')?"M":"F",
        FatherSpouseName: txtFatherSpouseName.val(),
        DOB: txtDateOfBirth.val().trim(),
        BloodGroup: ddlBloodGroup.val().trim(),
        MaritalStatus: ddlMaritalStatus.val(),
        ApplicantAddress: txtApplicantAddress.val(),
        City: txtCity.val().trim(),
        StateId: ddlState.val(),
        CountryId: ddlCountry.val(),
        PinCode: txtPinCode.val().trim(),
        ContactNo: txtContactNo.val().trim(),
        MobileNo: txtMobileNo.val().trim(),
        Email: txtEmail.val().trim(),
        NoticePeriod:txtNoticePeriod.val().trim(),
        TotalExperience: txtTotalExperience.val().trim(),
        ReleventExperience: txtReleventExperience.val().trim(),
        CurrentCTC: txtCurrentCTC.val().trim(),
        ExpectedCTC: txtExpectedCTC.val().trim(),
        PreferredLocation: txtPreferredLocation.val().trim(),
        ResumeText:txtResumeText.val().trim(),
        ApplicantStatus: ddlApplicantStatus.val()
    };
    var extraActivityViewModel = {
        ApplicantExtraId: 0,
        Strength1: txtStrength1.val().trim(),
        Strength2: txtStrength2.val().trim(),
        Strength3: txtStrength3.val().trim(),
        Weakness1: txtWeakness1.val(),
        Weakness2: txtWeakness2.val(),
        Weakness3: txtWeakness3.val(),
        Hobbies: txtHobbies.val()
    };

    var educationList = [];
    $('#tblEducationList tr').each(function (i, row) {
        var $row = $(row);
        var educationId = $row.find("#hdnEducationId").val();
        var regularDistant = $row.find("#hdnRegularDistant").val();
        var boardUniversityName = $row.find("#hdnBoardUniversityName").val();
        var percentageObtained = $row.find("#hdnPercentageObtained").val();

        if (educationId != undefined) {
            var education = {
                    EducationId: educationId,
                    RegularDistant: regularDistant,
                    BoardUniversityName: boardUniversityName,
                    PercentageObtained: percentageObtained
                };
                educationList.push(education);
            }
    });
    var employerList = [];
    $('#tblEmployerList tr').each(function (i, row) {
        var $row = $(row);
        var currentEmployer = $row.find("#hdnCurrentEmployer").val() == "Yes" ? true : false;
        var employerName = $row.find("#hdnEmployerName").val();
        var startDate = $row.find("#hdnStartDate").val();
        var endDate = $row.find("#hdnEndDate").val();
        var lastCTC = $row.find("#hdnLastCTC").val();
        var reasonOfLeaving = $row.find("#hdnReasonOfLeaving").val();
        var lastDesignationId = $row.find("#hdnLastDesignationId").val();
        var employmentStatusId = $row.find("#hdnEmploymentStatusId").val();
        if (employerName != undefined) {
                var employer = {
                    CurrentEmployer: currentEmployer,
                    EmployerName: employerName,
                    StartDate: startDate,
                    EndDate: endDate,
                    LastCTC: lastCTC,
                    ReasonOfLeaving: reasonOfLeaving,
                    LastDesignationId: lastDesignationId,
                    EmploymentStatusId: employmentStatusId
                    };
                employerList.push(employer);
        }
    });

    var projectList = [];
    $('#tblProjectList tr').each(function (i, row) {
        var $row = $(row);
        var projectName = $row.find("#hdnProjectName").val();
        var clientName = $row.find("#hdnClientName").val();
        var roleDesc = $row.find("#hdnRoleDesc").val();
        var teamSize = $row.find("#hdnTeamSize").val();
        var projectDesc = $row.find("#hdnProjectDesc").val();
        var technologiesUsed = $row.find("#hdnTechnologiesUsed").val();
        if (projectName != undefined) {
                var project = {
                    ProjectName: projectName,
                    ClientName: clientName,
                    RoleDesc: roleDesc,
                    TeamSize: teamSize,
                    ProjectDesc: projectDesc,
                    TechnologiesUsed: technologiesUsed
                };
                projectList.push(project);
            }
    });

    var requestData = {
        applicant: applicantViewModel,
        educations: educationList,
        employers: employerList,
        projects: projectList,
        extraActivity:extraActivityViewModel
    };

    var accessMode = 1;//Add Mode
    if (hdnApplicantId.val() != null && hdnApplicantId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    $.ajax({
        url: "../Applicant/AddEditApplicant?accessMode=" + accessMode + "",
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
                      window.location.href = "../Applicant/ListApplicant";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../Applicant/AddEditApplicant";
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

function OpenJobOpeningSearchPopup() {
    $("#SearchJobOpeningModel").modal();
}
function SearchJobOpenings() {
    var txtResourceRequisitionNo = $("#txtResourceRequisitionNo");
    var txtJobOpeningNo = $("#txtSearchJobOpeningNo");
    var txtJobPortalRefNo = $("#txtJobPortalRefNo");
    var txtJobTitle = $("#txtJobTitle");

    var txtFromDate = $("#txtJobFromDate");
    var txtToDate = $("#txtJobToDate");
    var requestData = {
        jobOpeningNo: txtJobOpeningNo.val().trim(), requisitionNo: txtResourceRequisitionNo.val().trim(), jobPortalRefNo: txtJobPortalRefNo.val().trim(),
        jobTitle: txtJobTitle.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), jobStatus: "Final" };
    $.ajax({
        url: "../Applicant/GetJobOpeningList",
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
function SelectJobOpening(JobOpeningId, JobOpeningNo) {
    $("#hdnJobOpeningId").val(JobOpeningId);
    $("#txtJobOpeningNo").val(JobOpeningNo);
    $("#SearchJobOpeningModel").modal('hide');
}