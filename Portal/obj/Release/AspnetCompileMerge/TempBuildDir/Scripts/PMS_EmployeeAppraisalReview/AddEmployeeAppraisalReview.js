$(document).ready(function () { 
    $("#hdnEmpAppraisalTemplateMappingId").attr("disabled", true);
    $("#ddlPerformanceCycle").attr("disabled", true);
    $("#txtEmployee").attr("disabled", true);
    $("#ddlFinYear").attr("disabled", true);

    $("#txtPMSFormSubmitDate").attr('readOnly', true);
    $("#txtPMSFormSubmitDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        minDate: '0',
        onSelect: function (selected) {

        }
    });

    BindCompanyBranchList();
    BindFinYearList();
    BindPerformanceCycleList();
    BindCompanyBranchList();
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPMSReviewId = $("#hdnPMSReviewId");
     if (hdnPMSReviewId.val() != "" && hdnPMSReviewId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeAppraisalReviewDetail(hdnPMSReviewId.val());
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

 
     var url = "../PMS_EmployeeAppraisalReview/PMSReport?empAppraisalTemplateMappingId=" + $("#hdnEmpAppraisalTemplateMappingId").val() + "&reportType=PDF";
     $('#btnPrint').attr('href', url);

  
 
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

function BindFinYearList() {
    $.ajax({
        type: "GET",
        url: "../PMS_EmployeeAppraisalTemplateMapping/GetFinancialYearForEmployeeAppraisalTemplateList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlFinYear").append($("<option></option>").val(0).html("Select Financial Year"));
            $.each(data, function (i, item) {
                $("#ddlFinYear").append($("<option></option>").val(item.FinYearId).html(item.FinYearDesc));
            });
        },
        error: function (Result) {
            $("#ddlFinYear").append($("<option></option>").val(0).html("Select Financial Year"));
        }
    });
}


function BindPerformanceCycleList() {
    $.ajax({
        type: "GET",
        url: "../PMSGoal/GetPMSPerformanceCycleList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPerformanceCycle").append($("<option></option>").val(0).html("Select Performance Cycle"));
            $.each(data, function (i, item) {
                $("#ddlPerformanceCycle").append($("<option></option>").val(item.PerformanceCycleId).html(item.PerformanceCycleName));
            });
        },
        error: function (Result) {
            $("#ddlPerformanceCycle").append($("<option></option>").val(0).html("Select Performance Cycle"));
        }
    });
}

 

function OpenApSearchPopup() {
    $("#SearchAppraisalTemplateModel").modal();

}
function SearchTemplate() {
    var txtTemplateName = $("#txtTemplateNameSearch");
    var txtEmployeeName = $("#txtEmployeeNameSearch");

    var requestData = {
        templateName: txtTemplateName.val().trim(),
        employeeName: txtEmployeeName.val().trim(),
        employeeMapping_Status: "1"
    };
    $.ajax({
        url: "../PMS_EmployeeAppraisalReview/GetFinalAssessmentList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divTemplateList").html("");
            $("#divTemplateList").html(err);
        },
        success: function (data) {
            $("#divTemplateList").html("");
            $("#divTemplateList").html(data);
        }
    });
}

function SelectTemplate(empAppraisalTemplateMappingId, performanceCycleId, employeeId, employeeName, finYearId) {
    $("#hdnEmpAppraisalTemplateMappingId").val(empAppraisalTemplateMappingId);
    $("#ddlPerformanceCycle").val(performanceCycleId);
    $("#txtEmployee").val(employeeName);
    $("#hdnEmployeeId").val(employeeId);

    $("#ddlFinYear").val(finYearId);
    
    var url = "../PMS_EmployeeAppraisalReview/PMSReport?empAppraisalTemplateMappingId=" + empAppraisalTemplateMappingId + "&reportType=PDF";
    $('#btnPrint').attr('href', url);
    $('#btnPrint').show();

    $("#SearchAppraisalTemplateModel").modal('hide');
}

function SaveData() {
    var hdnEmpAppraisalTemplateMappingId = $("#hdnEmpAppraisalTemplateMappingId");
    var ddlPerformanceCycle = $("#ddlPerformanceCycle");
    var hdnPMSReviewId = $("#hdnPMSReviewId");
    var txtEmployee = $("#txtEmployee");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var ddlFinYear = $("#ddlFinYear");
    var ddlPMSFormStatus = $("#ddlPMSFormStatus");
    var txtPMSFormSubmitDate = $("#txtPMSFormSubmitDate");
    var txtPMSReviewRemarks = $("#txtPMSReviewRemarks");
    var ddlPMSFinalStatus = $("#ddlPMSFinalStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (hdnEmpAppraisalTemplateMappingId.val() == "" || hdnEmpAppraisalTemplateMappingId.val() == "0") {
        ShowModel("Alert", "Please select Employee Assessment Id")
        return false;
    }

    if (hdnEmployeeId.val() == "" || hdnEmployeeId.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false;
    }
    if (ddlPMSFormStatus.val() == "" || ddlPMSFormStatus.val() == "0") {
        ShowModel("Alert", "Please select Employee Asseessment Form Submit Status ")
        return false;
    }
    if (ddlPMSFormStatus.val() == "Submitted" && txtPMSFormSubmitDate.val() == "" ) {
        ShowModel("Alert", "Please select Employee Asseessment Form Submit Date")
        return false;
    }
    if (txtPMSReviewRemarks.val() == "" ) {
        ShowModel("Alert", "Please enter Employee Asseessment review remarks")
        return false;
    }
    if (ddlPMSFinalStatus.val() == "" || ddlPMSFinalStatus.val() == "0") {
        ShowModel("Alert", "Please select Employee Asseessment Final Status ")
        return false;
    }
    
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    var empReviewViewModel = {
        PMSReviewId: hdnPMSReviewId.val(),
        PerformanceCycleId: ddlPerformanceCycle.val(),
        FinYearId: ddlFinYear.val(),
        EmployeeId: hdnEmployeeId.val(),
        EmpAppraisalTemplateMappingId: hdnEmpAppraisalTemplateMappingId.val(),
        PMSFormStatus: ddlPMSFormStatus.val(),
        PMSFormSubmitDate: txtPMSFormSubmitDate.val(),
        PMSReviewRemarks: txtPMSReviewRemarks.val().trim(),
        PMSFinalStatus: ddlPMSFinalStatus.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
    }; 
     
    var accessMode = 1;//Add Mode
    if (hdnEmpAppraisalTemplateMappingId.val() != null && hdnEmpAppraisalTemplateMappingId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    
    var requestData = { employeeAppraisalReviewViewModel: empReviewViewModel};
    $.ajax({
        url: "../PMS_EmployeeAppraisalReview/AddEditEmployeeAppraisalReview?accessMode=" + accessMode + "",
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
                      window.location.href = "../PMS_EmployeeAppraisalReview/ListEmployeeAppraisalReview";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PMS_EmployeeAppraisalReview/AddEditEmployeeAppraisalReview";
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
    $("#ddlCompanyBranch").val(0);
    $("#btnSave").show();
    $("#btnUpdate").hide(); 
}
 
 
function GetEmployeeAppraisalReviewDetail(pmsReviewId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PMS_EmployeeAppraisalReview/GetEmployeeAppraisalReviewDetail",
        data: { pmsReviewId: pmsReviewId },
        dataType: "json",
        success: function (data) {
            $("#hdnEmpAppraisalTemplateMappingId").val(data.EmpAppraisalTemplateMappingId);
            $("#ddlPerformanceCycle").val(data.PerformanceCycleId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#ddlFinYear").val(data.FinYearId);

            $("#ddlPMSFormStatus").val(data.PMSFormStatus);
            if (data.PMSFormStatus == "Submitted")
            {
                $("#txtPMSFormSubmitDate").val(data.PMSFormSubmitDate);
            }
            else
            {
                $("#txtPMSFormSubmitDate").val("");
}
            $("#txtPMSReviewRemarks").val(data.PMSReviewRemarks);
            $("#ddlPMSFinalStatus").val(data.PMSFinalStatus);
            
            
            


            
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            var url = "../PMS_EmployeeAppraisalReview/PMSReport?empAppraisalTemplateMappingId=" + data.EmpAppraisalTemplateMappingId + "&reportType=PDF";
            $('#btnPrint').attr('href', url);
            $('#btnPrint').show();
            $("#btnAddNew").show();
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

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