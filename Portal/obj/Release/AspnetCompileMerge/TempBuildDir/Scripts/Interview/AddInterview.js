$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    BindCompanyBranchList();
    BindResumeSourceList();
    BindPositionList();
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
    $("#txtResourceRequisitionNo").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnInterviewId = $("#hdnInterviewId");
    if (hdnInterviewId.val() != "" && hdnInterviewId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetInterviewDetail(hdnInterviewId.val());
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
function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
 
function SaveData() {
    var hdnApplicantId = $("#hdnApplicantId");
    var hdnInterviewId = $("#hdnInterviewId");
    var txtApplicantNo = $("#txtApplicantNo");
    var ddlAptitudeTestStatus = $("#ddlAptitudeTestStatus");
    var txtAptitudeTestRemarks = $("#txtAptitudeTestRemarks");
    var txtAptitudeTestTotalMarks = $("#txtAptitudeTestTotalMarks");
    var txtAptitudeTestMarkObtained = $("#txtAptitudeTestMarkObtained");
    var ddlTechnicalRound1_Status = $("#ddlTechnicalRound1_Status");
    var txtTechnicalRound1_Remarks = $("#txtTechnicalRound1_Remarks");
    var txtTechnicalRound1_TotalMarks = $("#txtTechnicalRound1_TotalMarks");
    var txtTechnicalRound1_MarkObtained = $("#txtTechnicalRound1_MarkObtained");
    var ddlTechnicalRound2_Status = $("#ddlTechnicalRound2_Status");
    var txtTechnicalRound2_Remarks = $("#txtTechnicalRound2_Remarks");
    var txtTechnicalRound2_TotalMarks = $("#txtTechnicalRound2_TotalMarks");
    var txtTechnicalRound2_MarkObtained = $("#txtTechnicalRound2_MarkObtained");
    var ddlTechnicalRound3_Status = $("#ddlTechnicalRound3_Status");
    var txtTechnicalRound3_Remarks = $("#txtTechnicalRound3_Remarks");
    var txtTechnicalRound3_TotalMarks = $("#txtTechnicalRound3_TotalMarks");
    var txtTechnicalRound3_MarkObtained = $("#txtTechnicalRound3_MarkObtained");
    var ddlMachineRoundStatus = $("#ddlMachineRoundStatus");
    var txtMachineRoundRemarks = $("#txtMachineRoundRemarks");
    var txtMachineRound_TotalMarks = $("#txtMachineRound_TotalMarks");
    var txtMachineRound_MarkObtained = $("#txtMachineRound_MarkObtained");
    var ddlHRRound_Status = $("#ddlHRRound_Status");
    var txtHRRound_Remarks = $("#txtHRRound_Remarks");
    var txtHRRound_TotalMarks = $("#txtHRRound_TotalMarks");
    var txtHRRound_MarkObtained = $("#txtHRRound_MarkObtained");
    var ddlInterviewFinalStatus = $("#ddlInterviewFinalStatus");
    var txtFinalRemarks = $("#txtFinalRemarks");
    var ddlCompanyBranch=$("#ddlCompanyBranch");
    if (txtApplicantNo.val().trim() == "" || txtApplicantNo.val() == 0)
    {
        ShowModel("Alert", "Please Enter Applicant No")
        return false;
    }
    if (ddlAptitudeTestStatus.val() != "0" && $('#ddlAptitudeTestStatus option:selected').text() != "Pending")
    {
        if(txtAptitudeTestRemarks.val().trim()=="")
        {
            ShowModel("Alert", "Please Aptitude Test Remarks");
            txtAptitudeTestRemarks.focus();
            return false;
        }
        if (txtAptitudeTestTotalMarks.val().trim() == "" || txtAptitudeTestTotalMarks.val()=="0") {
            ShowModel("Alert", "Please Aptitude Test Total marks");
            txtAptitudeTestTotalMarks.focus();
            return false;
        }
        if (txtAptitudeTestMarkObtained.val().trim() == "" || txtAptitudeTestMarkObtained.val() == "0") {
            ShowModel("Alert", "Please Aptitude Test Mark Obtained");
            txtAptitudeTestMarkObtained.focus();
            return false;
        }
            
    }

    if (ddlTechnicalRound1_Status.val() != "0" && $('#ddlTechnicalRound1_Status option:selected').text() != "Pending") {
        if (txtTechnicalRound1_Remarks.val().trim() == "") {
            ShowModel("Alert", "Please Technical Round-1 Remarks");
            txtTechnicalRound1_Remarks.focus();
            return false;
        }
        if (txtTechnicalRound1_TotalMarks.val().trim() == "" || txtTechnicalRound1_TotalMarks.val() == "0") {
            ShowModel("Alert", "Please Technical Round-1 Total Marks");
            txtTechnicalRound1_TotalMarks.focus();
            return false;
        }
        if (txtAptitudeTestMarkObtained.val().trim() == "" || txtAptitudeTestMarkObtained.val() == "0") {
            ShowModel("Alert", "Please Aptitude Test Mark Obtained");
            txtAptitudeTestMarkObtained.focus();
            return false;
        }

        if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
            ShowModel("Alert", "Please select Company Branch")
            return false;
        }

    }
    var interview = {
                
        InterviewId: hdnInterviewId.val(),
        ApplicantId: hdnApplicantId.val(),
        AptitudeTestStatus: ddlAptitudeTestStatus.val(),
        AptitudeTestRemarks: txtAptitudeTestRemarks.val().trim(),
        AptitudeTestTotalMarks: txtAptitudeTestTotalMarks.val().trim(),
        AptitudeTestMarkObtained: txtAptitudeTestMarkObtained.val().trim(),
       
        TechnicalRound1_Status: ddlTechnicalRound1_Status.val().trim(),
        TechnicalRound1_Remarks: txtTechnicalRound1_Remarks.val().trim(),
        TechnicalRound1_TotalMarks: txtTechnicalRound1_TotalMarks.val(),
        TechnicalRound1_MarkObtained: txtTechnicalRound1_MarkObtained.val(),

        TechnicalRound2_Status: ddlTechnicalRound2_Status.val().trim(),
        TechnicalRound2_Remarks: txtTechnicalRound2_Remarks.val().trim(),
        TechnicalRound2_TotalMarks: txtTechnicalRound2_TotalMarks.val(),
        TechnicalRound2_MarkObtained: txtTechnicalRound2_MarkObtained.val(),

        TechnicalRound3_Status: ddlTechnicalRound3_Status.val().trim(),
        TechnicalRound3_Remarks: txtTechnicalRound3_Remarks.val().trim(),
        TechnicalRound3_TotalMarks: txtTechnicalRound3_TotalMarks.val(),
        TechnicalRound3_MarkObtained: txtTechnicalRound3_MarkObtained.val(),

        MachineRound_Status: ddlMachineRoundStatus.val().trim(),
        MachineRound_Remarks: txtMachineRoundRemarks.val().trim(),
        MachineRound_TotalMarks: txtMachineRound_TotalMarks.val().trim(),
        MachineRound_MarkObtained: txtMachineRound_MarkObtained.val().trim(),

        HRRound_Status: ddlHRRound_Status.val(),
        HRRound_Remarks: txtHRRound_Remarks.val().trim(),
        HRRound_TotalMarks: txtHRRound_TotalMarks.val().trim(),
        HRRound_MarkObtained: txtHRRound_MarkObtained.val(),
        FinalRemarks:txtFinalRemarks.val().trim(),
        InterviewFinalStatus: ddlInterviewFinalStatus.val(),
        CompanyBranchId: ddlCompanyBranch.val()

        
    };
    
    var accessMode = 1;//Add Mode
    if (hdnInterviewId.val() != null && hdnInterviewId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { interviewViewModel:interview };
    $.ajax({
        url: "../Interview/AddEditInterview?accessMode=" + accessMode + "",
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
                      window.location.href = "../Interview/ListInterview";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        //window.location.href = "../Interview/AddEditInterview";
                        window.location.href = "../Interview/AddEditInterview?interviewId=" + data.trnId + "&AccessMode=3";
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

    $("#txtJobOpeningNo").val("");
    $("#txtResourceRequisitionNo").val("");
    $("#ddlEducation").val("0");
    $("#txtJobPortalRefNo").val("");
    $("#txtJobTitle").val("");
    $("#txtJobDescription").val("");
    $("#txtKeySkills").val("");
    $("#ddlMinExp").val("0");
    $("#ddlMaxExp").val("0");
    $("#txtMinSalary").val("");
    $("#txtMaxSalary").val("");
    $("#txtJobStartDate").val("");
    $("#txtJobExpiryDate").val("");
    $("#ddlCurrency").val("0");
    $("#txtOtherQualification").val("");
    $("#txtMaxSalary").val("");
    $("#ddlCurrency").val("0");
    $("#txtRemark").val("");
    $("#txtNoOfOpening").val("");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
function GetInterviewDetail(interviewId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Interview/GetInterviewDetail",
        data: { interviewId: interviewId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicantNo").val(data.ApplicantNo);
            $("#hdnApplicantId").val(data.ApplicantId);
            $("#hdnInterviewId").val(data.InterviewId);
            $("#ddlAptitudeTestStatus").val(data.AptitudeTestStatus);
            $("#txtAptitudeTestRemarks").val(data.AptitudeTestRemarks);
            $("#txtAptitudeTestTotalMarks").val(data.AptitudeTestTotalMarks);
            $("#txtAptitudeTestMarkObtained").val(data.AptitudeTestMarkObtained);

            $("#ddlTechnicalRound1_Status").val(data.TechnicalRound1_Status);
            $("#txtTechnicalRound1_Remarks").val(data.TechnicalRound1_Remarks);
            $("#txtTechnicalRound1_TotalMarks").val(data.TechnicalRound1_TotalMarks);
            $("#txtTechnicalRound1_MarkObtained").val(data.TechnicalRound1_MarkObtained);

            $("#ddlTechnicalRound2_Status").val(data.TechnicalRound2_Status);
            $("#txtTechnicalRound2_Remarks").val(data.TechnicalRound2_Remarks);
            $("#txtTechnicalRound2_TotalMarks").val(data.TechnicalRound2_TotalMarks);
            $("#txtTechnicalRound2_MarkObtained").val(data.TechnicalRound2_MarkObtained);

            $("#ddlTechnicalRound3_Status").val(data.TechnicalRound3_Status);
            $("#txtTechnicalRound3_Remarks").val(data.TechnicalRound3_Remarks);
            $("#txtTechnicalRound3_TotalMarks").val(data.TechnicalRound3_TotalMarks);
            $("#txtTechnicalRound3_MarkObtained").val(data.TechnicalRound3_MarkObtained);

            $("#ddlMachineRoundStatus").val(data.MachineRound_Status);
            $("#txtMachineRoundRemarks").val(data.MachineRound_Remarks);
            $("#txtMachineRound_TotalMarks").val(data.MachineRound_TotalMarks);
            $("#txtMachineRound_MarkObtained").val(data.MachineRound_MarkObtained);


            $("#ddlHRRound_Status").val(data.HRRound_Status);
            $("#txtHRRound_Remarks").val(data.HRRound_Remarks);
            $("#txtHRRound_TotalMarks").val(data.HRRound_TotalMarks);
            $("#txtHRRound_MarkObtained").val(data.HRRound_MarkObtained);
            $("#ddlInterviewFinalStatus").val(data.InterviewFinalStatus);
            $("#txtFinalRemarks").val(data.FinalRemarks);

            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            

            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            $("#btnAddNew").show();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function OpenInterviewSearchPopup() {
    $("#SearchApplicantModel").modal();
    BindResumeSourceList();
    BindPositionList();


}
function SearchApplicant() {
    var txtApplicantNo = $("#txtApplicantNo");
    var txtProjectNo = $("#txtProjectNo");
    var txtFirstName = $("#txtFirstName");
    var txtLastName = $("#txtLastName");
    var ddlResumeSource = $("#ddlResumeSource");
    var ddlDesignation = $("#ddlDesignation");
    var ddlApplicantStatus = $("#ddlApplicantStatus");

    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var requestData = {
        applicantNo: txtApplicantNo.val().trim(), projectNo: txtProjectNo.val().trim(),
        firstName: txtFirstName.val().trim(), lastName: txtLastName.val(), resumeSource: ddlResumeSource.val(),
        designation: ddlDesignation.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(),
        applicantStatus: ddlApplicantStatus.val()
    };
    $.ajax({
        url: "../Interview/GetApplicantList",
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
function SelectApplicant(applicantNo, applicantId) {
    $("#txtApplicantNo").val(applicantNo);
    $("#hdnApplicantId").val(applicantId);

    $("#SearchApplicantModel").modal('hide');
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

    $("#ddlDesignation").val(0);
    $("#ddlDesignation").html("");

    $.ajax({
        type: "GET",
        url: "../Applicant/GetAllDesignationList",
        data: {},
        asnc: false,
        dataType: "json",
        success: function (data) {
            $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
            $.each(data, function (i, item) {
                $("#ddlDesignation").append($("<option></option>").val(item.DesignationId).html(item.DesignationName));
            });
        },
        error: function (Result) {
            $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
        }
    });


}
