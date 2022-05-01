$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    BindCompanyBranchList();
    $("#txtResourceRequisitionNo").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtJobOpeningDate").attr('readOnly', true);
    $("#txtJobStartDate").attr('readOnly', true);
    $("#txtJobExpiryDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate").attr('readOnly', true);

    $("#txtJobStartDate,#txtJobExpiryDate,#txtJobOpeningDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    BindPositionLevelList();
    BindPositionTypeList();
    BindDepartmentList();

    SearchResourceRequisition();

    BindEductaionList();
    
    BindCurrencyList();

    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnJobopeningId = $("#hdnJobopeningId");
    if (hdnJobopeningId.val() != "" && hdnJobopeningId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetJobOpeningDetail(hdnJobopeningId.val());
       }, 2000);

         if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#btnSearchSO").hide();
         
            //$("#chkstatus").attr('disabled', true);
           // $('#chkstatus').prop('disabled');
          //  $('#chkstatus').style('display', 'none')
           

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
 
function BindEductaionList() {
    $("#ddlEducation").val(0);
    $("#ddlEducation").html("");
    $.ajax({
        type: "GET",
        url: "../ResourceRequisition/GetEducationList",
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
 
function BindCurrencyList() {
    $("#ddlCurrency").val(0);
    $("#ddlCurrency").html("");
    $.ajax({
        type: "GET",
        url: "../Quotation/GetCurrencyList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCurrency").append($("<option></option>").val(0).html("-Select Currency-"));
            $.each(data, function (i, item) {
                $("#ddlCurrency").append($("<option></option>").val(item.CurrencyCode).html(item.CurrencyName));
            });
        },
        error: function (Result) {
            $("#ddlCurrency").append($("<option></option>").val(0).html("-Select Currency-"));
        }
    });
}
 
function SaveData() {
    var hdnResourceRequisitionId = $("#hdnResourceRequisitionId");
    var txtJobOpeningNo = $("#txtJobOpeningNo");
    var hdnJobopeningId = $("#hdnJobopeningId");
    var ddlEducation = $("#ddlEducation");
    var txtJobPortalRefNo = $("#txtJobPortalRefNo");
    var txtJobTitle = $("#txtJobTitle");
    var txtJobOpeningDate = $("#txtJobOpeningDate");
    var txtJobDescription = $("#txtJobDescription");
    var txtKeySkills = $("#txtKeySkills");
    var ddlMinExp = $("#ddlMinExp");
    var ddlMaxExp = $("#ddlMaxExp");
    var txtMinSalary = $("#txtMinSalary");
    var txtMaxSalary = $("#txtMaxSalary");
    var txtJobStartDate = $("#txtJobStartDate");
    var txtJobExpiryDate = $("#txtJobExpiryDate");
    var ddlCurrency = $("#ddlCurrency");
    var txtOtherQualification = $("#txtOtherQualification");
    var txtNoOfOpening = $("#txtNoOfOpening");
    var txtRemark = $("#txtRemark");
    var chkStatus = $("#chkStatus");
    var jobOpeningStatus = true;
    var ddlCompanyBranch = $("#ddlCompanyBranch");


    if (chkStatus.prop("checked") == true)
    { jobOpeningStatus = true; }
    else
    {
        jobOpeningStatus = false;
    }
    if (txtJobTitle.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Job Title")
        return false;
    }
    if (txtNoOfOpening.val().trim() == "" || parseInt(txtNoOfOpening.val().trim()) <= 0) {
        ShowModel("Alert", "Please Enter valid Number Of Opening")
        txtNumberofResource.focus();
        return false;
    }
   
   
    if (ddlEducation.val() == "" || ddlEducation.val() == "0") {
        ShowModel("Alert", "Please select Education Level")
        return false;
    }
    if (txtKeySkills.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Desire Skills")
        return false;
    }
    
    if (ddlMinExp.val() == "" || ddlMinExp.val() == "0") {
        ShowModel("Alert", "Please select Minimum Experience")
        return false;
    }
    if (ddlMaxExp.val() == "" || ddlMaxExp.val() == "0") {
        ShowModel("Alert", "Please select Maximum Experience")
        return false;
    }
    if (txtMinSalary.val() == "0" || txtMinSalary.val() == "")
    {
        ShowModel("Alert", "Enter Minimum Salary")
        return false;
    }
    if (txtMaxSalary.val() == "0" || txtMaxSalary.val() == "") {
        ShowModel("Alert", "Enter Maximum Salary")
        return false;
    }
    if (txtJobStartDate.val().trim() == "") {
        ShowModel("Alert", "Please select Job Start Date")
        return false;
    }
    if (txtJobExpiryDate.val().trim() == "") {
        ShowModel("Alert", "Please select Job Expiry by Date")
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    var jobOpeningViewModel = {
        JobOpeningId: hdnJobopeningId.val(),
        JobOpeningNo:txtJobOpeningNo.val(),
        JobTitle:txtJobTitle.val(),
        JobOpeningDate: txtJobOpeningDate.val().trim(),
        JobPortalRefNo: txtJobPortalRefNo.val().trim(),
        NoOfOpening: txtNoOfOpening.val().trim(),
        MinExp: ddlMinExp.val(),
        MaxExp: ddlMaxExp.val(),
        MinSalary: txtMinSalary.val().trim() == "" ? "0" : txtMinSalary.val().trim(),
        MaxSalary: txtMaxSalary.val().trim() == "" ? "0" : txtMaxSalary.val().trim(),
        KeySkills: txtKeySkills.val().trim(),
        JobDescription: txtJobDescription.val().trim(),
        JobStartDate: txtJobStartDate.val().trim(),
        JobExpiryDate: txtJobExpiryDate.val().trim(),
        JobStatus: jobOpeningStatus,
        RequisitionId: hdnResourceRequisitionId.val(),
        EducationId: ddlEducation.val().trim(),
        OtherQualification: txtOtherQualification.val().trim(),
        CurrencyCode: ddlCurrency.val(),
        Remarks: txtRemark.val(),
        CompanyBranchId: ddlCompanyBranch.val()

    };
    var accessMode = 1;//Add Mode
    if (hdnJobopeningId.val() != null && hdnJobopeningId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = {  jobOpeningViewModel:jobOpeningViewModel};
    $.ajax({
        url: "../JobOpening/AddEditJobOpening?accessMode=" + accessMode + "",
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
                      window.location.href = "../JobOpening/ListJobOpening";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                       // window.location.href = "../JobOpening/AddEditJobOpening";
                        window.location.href = "../JobOpening/AddEditJobOpening?jobopeningId=" + data.trnId + "&AccessMode=3";
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
function ShowHideSkillPanel(action) {
    if (action == 1) {
        $(".skillsection").show();
    }
    else {
        $(".skillsection").hide();
        $("#hdnRequisitionSkillId").val("0");
        $("#hdnSkillSequenceNo").val("0");
        $("#txtSkillName").val("");
        $("#hdnSkillId").val("0");
        $("#txtSkillCode").val("");
    
        $("#btnAddSkill").show();
        $("#btnUpdateSkill").hide();
    }
}
function ShowHideInterviewPanel(action) {
    if (action == 1) {
        $(".interviewsection").show();
    }
    else {
        $(".interviewsection").hide();
        $("#ddlInterviewType").val("0");
        $("#hdnRequisitionInterviewStagesId").val("0");
        $("#hdnInterviewSequenceNo").val("0");
        $("#txtInterviewDescription").val("");
        $("#txtInterviewAssignToUserName").val("");
        $("#hdnInterviewAssignToUserId").val("0");

        $("#btnAddInterview").show();
        $("#btnUpdateInterview").hide();
    }
}
function BindPositionLevelList() {
    $("#ddlPositionLevel").val(0);
    $("#ddlPositionLevel").html("");
    $.ajax({
        type: "GET",
        url: "../ResourceRequisition/GetPositionLevelList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPositionLevel").append($("<option></option>").val(0).html("-Select Position Level-"));
            $.each(data, function (i, item) {
                $("#ddlPositionLevel").append($("<option></option>").val(item.PositionLevelId).html(item.PositionLevelName));
            });
        },
        error: function (Result) {
            $("#ddlPositionLevel").append($("<option></option>").val(0).html("-Select Position Level-"));
        }
    });
}
function BindPositionTypeList() {
    $("#ddlPositionType").val(0);
    $("#ddlPositionType").html("");
    $.ajax({
        type: "GET",
        url: "../ResourceRequisition/GetPositionTypeList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPositionType").append($("<option></option>").val(0).html("-Select Position Type-"));
            $.each(data, function (i, item) {
                $("#ddlPositionType").append($("<option></option>").val(item.PositionTypeId).html(item.PositionTypeName));
            });
        },
        error: function (Result) {
            $("#ddlPositionType").append($("<option></option>").val(0).html("-Select Position Type-"));
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


function OpenRequisitionSearchPopup() {
    if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
        ShowModel("Alert", "Please Select Company Branch");
        return false;
    }
    $("#SearchRequisitionModel").modal();

}

function SearchResourceRequisition() {
    var txtResourceRequisitionNo = $("#txtSearchRequisition");
    var ddlPositionLevel = $("#ddlPositionLevel");
    var ddlPriorityLevel = $("#ddlPriorityLevel");
    var ddlPositionType = $("#ddlPositionType");
    var ddlDepartment = $("#ddlDepartment");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");

    var requestData = {
        requisitionNo: txtResourceRequisitionNo.val().trim(), positionLevelId: ddlPositionLevel.val(), priorityLevel: ddlPriorityLevel.val(),
        positionTypeId: ddlPositionType.val(), departmentId: ddlDepartment.val(), approvalStatus: ddlApprovalStatus.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val()
    };
    $.ajax({
        url: "../JobOpening/GetResourceRequisitionList",
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
function SelectRequisition(requisitionNo, requisitionId) {
    $("#txtResourceRequisitionNo").val(requisitionNo);
    $("#hdnResourceRequisitionId").val(requisitionId);
    $("#SearchRequisitionModel").modal('hide');
}

function GetJobOpeningDetail(jobOpeningId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../JobOpening/GetJobOpeningDetail",
        data: { jobOpeningId: jobOpeningId },
        dataType: "json",
        success: function (data) {
            $("#txtJobOpeningNo").val(data.JobOpeningNo);
            
            $("#txtNoOfOpening").val(data.NoOfOpening);
            $("#hdnResourceRequisitionId").val(data.RequisitionId);
            $("#txtResourceRequisitionNo").val(data.RequisitionNo);
            $("#txtJobOpeningDate").val(data.JobOpeningDate);
            $("#txtJobTitle").val(data.JobTitle);
            $("#txtJobPortalRefNo").val(data.JobPortalRefNo);
            $("#ddlEducation").val(data.EducationId);
            $("#txtKeySkills").val(data.KeySkills);
            $("#txtJobDescription").val(data.JobDescription);
            $("#ddlMinExp").val(data.MinExp);
            $("#ddlMaxExp").val(data.MaxExp);
            $("#txtMinSalary").val(data.MinSalary);
            $("#txtMaxSalary").val(data.MaxSalary);
            $("#txtJobStartDate").val(data.JobStartDate);
            $("#txtJobExpiryDate").val(data.JobExpiryDate);
            $("#txtJobDescription").val(data.JobDescription);
            $("#txtOtherQualification").val(data.OtherQualification);
            $("#ddlCurrency").val(data.CurrencyCode);
            $("#txtRemark").val(data.Remarks);
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);

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