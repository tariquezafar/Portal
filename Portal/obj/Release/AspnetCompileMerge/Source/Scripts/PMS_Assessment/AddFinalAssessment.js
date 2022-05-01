$(document).ready(function () {
    $("#txtTemplateName").attr('readOnly', true);
    $("#txtDepartment").attr('readOnly', true);
    $("#txtDesignation").attr('readOnly', true);
    $("#txtEmployee").attr('readOnly', true);

    $("#txtEmployee").attr('disabled', true);
    $("#txtCreatedBy").attr('disabled', true);
    $("#txtModifiedBy").attr('disabled', true);
    $("#txtCreatedDate").attr('disabled', true);
    $("#txtModifiedDate").attr('disabled', true);

    $("#ddlFinYear").attr('disabled', true);
    $("#ddlPerformanceCycle").attr('disabled', true);

    $("#txtGoalStartDate").attr('readOnly', true);
    $("#txtGoalDueDate").attr('readOnly', true);

    $("#txtGoalStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        minDate: '0',
        onSelect: function (selected) {

        }
    });
    $("#txtGoalDueDate").datepicker({
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
    BindPMSGoalCategoryList();
    BindPMSSectionList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnEmpAppraisalTemplateMappingId = $("#hdnEmpAppraisalTemplateMappingId");
    if (hdnEmpAppraisalTemplateMappingId.val() != "" && hdnEmpAppraisalTemplateMappingId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeAppraisalTemplateMappingDetail(hdnEmpAppraisalTemplateMappingId.val());
       }, 2000);
         

        if (hdnAccessMode.val() == "3") {

            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
           $("#chkstatus").attr('disabled', true);
        }
        else {

            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else {

        $("#btnUpdate").hide();
        $("#btnReset").show();
    }


    var goalList = [];
    GetEmployeeGoalList(goalList);
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
function BindPMSGoalCategoryList() {
    $.ajax({
        type: "GET",
        url: "../PMSGoal/GetPMSGoalCategoryList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlGoalCategoryName").append($("<option></option>").val(0).html("Select Goal Category"));
            $.each(data, function (i, item) {
                $("#ddlGoalCategoryName").append($("<option></option>").val(item.GoalCategoryId).html(item.GoalCategoryName));
            });
        },
        error: function (Result) {
            $("#ddlGoalCategoryName").append($("<option></option>").val(0).html("Select Goal Category"));
        }
    });
}
function BindPMSSectionList() {
    $.ajax({
        type: "GET",
        url: "../PMSGoal/GetPMSSectionList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSectionName").append($("<option></option>").val(0).html("Select Section"));
            $.each(data, function (i, item) {
                $("#ddlSectionName").append($("<option></option>").val(item.SectionId).html(item.SectionName));
            });
        },
        error: function (Result) {
            $("#ddlSectionName").append($("<option></option>").val(0).html("Select Section"));
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

function SaveData() {
    var hdnEmpAppraisalTemplateMappingId = $("#hdnEmpAppraisalTemplateMappingId");
    var txtTemplateName = $("#txtTemplateName");
    var hdnTemplateId = $("#hdnTemplateId");
    var txtDepartment = $("#txtDepartment");
    var hdnDepartmentId = $("#hdnDepartmentId");
    var txtDesignation = $("#txtDesignation");
    var hdnDesignationId = $("#hdnDesignationId");
    var txtEmployee = $("#txtEmployee");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var ddlFinYear = $("#ddlFinYear");
    var ddlPerformanceCycle = $("#ddlPerformanceCycle");
    var chkstatus = true;
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    
    if (txtTemplateName.val().trim() == "" || hdnTemplateId.val() == "" || hdnTemplateId.val() == "0") {
        ShowModel("Alert", "Please Select Template")
        txtTemplateName.focus();
        return false;
    }

    if (txtEmployee.val().trim() == "" || hdnEmployeeId.val() == "" || hdnEmployeeId.val() == "0") {
        ShowModel("Alert", "Please Select Employee")
        txtEmployee.focus();
        return false;
    }
    if (ddlFinYear.val() == "" || ddlFinYear.val() == "0") {
        ShowModel("Alert", "Please Select Financial Year")
        return false;
    }
    if (ddlPerformanceCycle.val() == "" || ddlPerformanceCycle.val() == "0") {
        ShowModel("Alert", "Please Select Performance Cycle")
        return false;
        }
    
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    var empTemplateViewModel = {
        EmpAppraisalTemplateMappingId: hdnEmpAppraisalTemplateMappingId.val(),
        EmployeeId: hdnEmployeeId.val(),
        TemplateId: hdnTemplateId.val(),
        PerformanceCycleId: ddlPerformanceCycle.val(),
        FinYearId: ddlFinYear.val(),
        EmpAppraisalTemplateMapping_Status: chkstatus,
        CompanyBranchId: ddlCompanyBranch.val(),
    }; 
     

    var goalList = [];
    $('#tblEmployeeGoal tr').each(function (i, row) {
        var $row = $(row);
        var goalName = $row.find("#hdnGoalName").val();
        var goalId = $row.find("#hdnGoalId").val();
        var goalDescription = $row.find("#hdnGoalDescription").val();
        var sectionId = $row.find("#hdnSectionId").val();
        var goalCategoryId = $row.find("#hdnGoalCategoryId").val();
        var evalutionCriteria = $row.find("#hdnEvalutionCriteria").val();
        var startDate = $row.find("#hdnStartDate").val();
        var dueDate = $row.find("#hdnDueDate").val();
        var weight = $row.find("#hdnWeight").val();
        var selfScore = $row.find("#hdnSelfScore").val();
        var selfRemarks = $row.find("#hdnSelfRemarks").val();
        var appraiserScore = $row.find("#hdnAppraiserScore").val();
        var appraiserRemarks = $row.find("#hdnAppraiserRemarks").val();
        var reviewScore = $row.find("#hdnReviewScore").val();
        var reviewRemarks = $row.find("#hdnReviewRemarks").val();

        var fixedDynamic = $row.find("#hdnFixedDynamic").val();
        var employeeGoal_Status = $row.find("#hdnEmployeeGoal_Status").val() == "Active" ? true: false;
        if (goalId != undefined) {
            var goal = {
                        GoalId: goalId,
                        GoalName: goalName,
                        GoalDescription: goalDescription,
                        SectionId : sectionId,
                        GoalCategoryId: goalCategoryId,
                        EvalutionCriteria: evalutionCriteria,
                        StartDate: startDate,
                        DueDate: dueDate,
                        Weight: weight,
                        FixedDyanmic: fixedDynamic,
                        SelfScore: selfScore,
                        SelfRemarks: selfRemarks,
                        AppraiserScore: appraiserScore,
                        AppraiserRemarks: appraiserRemarks,
                        ReviewScore: reviewScore,
                        ReviewRemarks: reviewRemarks,
                        EmployeeGoal_Status: employeeGoal_Status
                        };
            goalList.push(goal);
        } 
    });

    var accessMode = 1;//Add Mode
    if (hdnEmpAppraisalTemplateMappingId.val() != null && hdnEmpAppraisalTemplateMappingId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = {
        employeeAppraisalTemplateMappingViewModel: empTemplateViewModel, employeeGoals: goalList
};
    $.ajax({
        url: "../PMS_Assessment/AddEditFinalAssessment?accessMode=" + accessMode + "",
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
                      window.location.href = "../PMS_Assessment/ListFinalAssessment";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PMS_Assessment/AddEditFinalAssessment";
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
    $("#txtTemplateName").val("");
    $("#hdnTemplateId").val("0");
    $("#hdnEmpAppraisalTemplateMappingId").val("0");
    $("#txtDepartment").val("");
    $("#hdnDepartmentId").val("0");
    $("#txtDesignation").val("");
    $("#hdnDesignationId").val("0");
    $("#txtEmployee").val("");
    $("#hdnEmployeeId").val("0");
    $("#ddlFinYear").val("0");
    $("#ddlPerformanceCycle").val("0");
    $("#chkstatus").prop("checked", true);
    $("#btnSave").show();
    $("#btnUpdate").hide();
    $("#divTemplateGoalList").html("");
    $("#ddlCompanyBranch").val("0");
    
}
function GetEmployeeGoalList(goalList) {
    var hdnEmpAppraisalTemplateMappingId = $("#hdnEmpAppraisalTemplateMappingId");
    var requestData = { goals: goalList, empAppraisalTemplateMappingId: hdnEmpAppraisalTemplateMappingId.val() };
    $.ajax({
        url: "../PMS_Assessment/GetFinalAssessmentGoalList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divTemplateGoalList").html("");
            $("#divTemplateGoalList").html(err);
        },
        success: function (data) {
            $("#divTemplateGoalList").html("");
            $("#divTemplateGoalList").html(data);
            ShowHideGoalPanel(2);
        }
    });
}
function AddGoal(action) {
    var goalEntrySequence = 0;
    var flag = true;
    var hdnEmployeeGoalSequenceNo = $("#hdnEmployeeGoalSequenceNo");
    var hdnEmployeeGoalId = $("#hdnEmployeeGoalId");
    var hdnGoalId = $("#hdnGoalId");
    var txtGoalName = $("#txtGoalName");
    var txtGoalDescription = $("#txtGoalDescription");
    var ddlSectionName = $("#ddlSectionName");
    var ddlGoalCategoryName = $("#ddlGoalCategoryName");
    var txtEvalutionCriteria = $("#txtEvalutionCriteria");
    var txtWeight = $("#txtWeight");
    var txtGoalStartDate = $("#txtGoalStartDate");
    var txtGoalDueDate = $("#txtGoalDueDate");
    var txtSelfScore = $("#txtSelfScore");
    var txtSelfRemarks = $("#txtSelfRemarks");
    var txtAppraiserScore = $("#txtAppraiserScore");
    var txtAppraiserRemarks = $("#txtAppraiserRemarks");
    var txtReviewScore = $("#txtReviewScore");
    var txtReviewRemarks = $("#txtReviewRemarks");
    var hdnFixedDynamic = $("#hdnFixedDynamic");
    var chkGoalStatus = $("#chkGoalStatus").is(':checked') ? true : false;

    if (txtGoalName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Goal Name")
        txtGoalName.focus();
        return false;
    }
    if (txtGoalDescription.val().trim() == "") {
        ShowModel("Alert", "Please Enter Goal Description")
        txtGoalDescription.focus();
        return false;
    }
    if (ddlSectionName.val() == "" || ddlSectionName.val() == "0") {
        ShowModel("Alert", "Please select Section")
        return false;
    }
    if (ddlGoalCategoryName.val() == "" || ddlGoalCategoryName.val() == "0") {
        ShowModel("Alert", "Please select Goal Category")
        return false;
    }
    if (txtEvalutionCriteria.val().trim() == "") {
        ShowModel("Alert", "Please Enter Goal Evalution Criteria")
        txtEvalutionCriteria.focus();
        return false;
    }
    if (txtWeight.val().trim() == "" || parseInt(txtWeight.val().trim())<= 0) {
        ShowModel("Alert", "Please enter Weightage(%)")
        return false;
    }
    if (txtSelfScore.val().trim() == "" || parseInt(txtSelfScore.val().trim()) < 0) {
        ShowModel("Alert", "Please enter Self Score")
        return false;
    }
    if (txtAppraiserScore.val().trim() == "" || parseInt(txtAppraiserScore.val().trim()) < 0) {
        ShowModel("Alert", "Please enter Appraiser Score")
        return false;
    }
    if (txtReviewScore.val().trim() == "" || parseInt(txtReviewScore.val().trim()) < 0) {
        ShowModel("Alert", "Please enter Review Score")
        return false;
    }
    if (txtGoalStartDate.val().trim() == "" || txtGoalStartDate.val().trim() == "0") {
        ShowModel("Alert", "Please select Goal Start Date")
        return false;
    }
    if (txtGoalDueDate.val().trim() == "" || txtGoalDueDate.val().trim() == "0") {
        ShowModel("Alert", "Please select Goal Start Date")
        return false;
    }

    var goalList = [];
    if (action == 1 && (hdnEmployeeGoalSequenceNo.val() == "" || hdnEmployeeGoalSequenceNo.val() == "0")) {
        goalEntrySequence = 1;
    }
    $('#tblEmployeeGoal tr').each(function (i, row) {
        var $row = $(row);
        var employeeGoalSequenceNo = $row.find("#hdnEmployeeGoalSequenceNo").val();
        var employeeGoalId = $row.find("#hdnEmployeeGoalId").val();
        var goalName = $row.find("#hdnGoalName").val();
        var goalId = $row.find("#hdnGoalId").val();
        var goalDescription = $row.find("#hdnGoalDescription").val();
        var sectionName = $row.find("#hdnSectionName").val();
        var sectionId = $row.find("#hdnSectionId").val();
        var goalCategoryId = $row.find("#hdnGoalCategoryId").val();
        var goalCategoryName = $row.find("#hdnGoalCategoryName").val();
        var evalutionCriteria = $row.find("#hdnEvalutionCriteria").val();
        var startDate = $row.find("#hdnStartDate").val();
        var dueDate = $row.find("#hdnDueDate").val();
        var weight = $row.find("#hdnWeight").val();
        var selfScore = $row.find("#hdnSelfScore").val();
        var selfRemarks = $row.find("#hdnSelfRemarks").val();
        var appraiserScore = $row.find("#hdnAppraiserScore").val();
        var appraiserRemarks = $row.find("#hdnAppraiserRemarks").val();
        var reviewScore = $row.find("#hdnReviewScore").val();
        var reviewRemarks = $row.find("#hdnReviewRemarks").val();
        var fixedDynamic = $row.find("#hdnFixedDynamic").val();
        var employeeGoal_Status = $row.find("#hdnEmployeeGoal_Status").val() == "Active" ? true : false;

        if (goalId != undefined) {
            if (action == 1 || (hdnEmployeeGoalSequenceNo.val() != employeeGoalSequenceNo)) {
                if (goalName == txtGoalName.val()) {
                    ShowModel("Alert", "Goal Name already added!!!")
                    return false;
                }
                var goal = {
                    EmployeeGoal_SequenceNo: employeeGoalSequenceNo,
                    EmployeeGoalId: employeeGoalId,
                    GoalId: goalId,
                    GoalName: goalName,
                    GoalDescription: goalDescription,
                    SectionId: sectionId,
                    SectionName: sectionName,
                    GoalCategoryId: goalCategoryId,
                    GoalCategoryName: goalCategoryName,
                    EvalutionCriteria: evalutionCriteria,
                    StartDate: startDate,
                    DueDate: dueDate,
                    Weight: weight,
                    SelfScore: selfScore,
                    SelfRemarks: selfRemarks,
                    AppraiserScore: appraiserScore,
                    AppraiserRemarks: appraiserRemarks,
                    ReviewScore: reviewScore,
                    ReviewRemarks: reviewRemarks,
                    FixedDyanmic: fixedDynamic,
                    EmployeeGoal_Status: employeeGoal_Status
                };
                goalList.push(goal);
                goalEntrySequence = parseInt(goalEntrySequence) + 1;

            }
            else if (hdnEmployeeGoalId.val() == employeeGoalId && hdnEmployeeGoalSequenceNo.val() == employeeGoalSequenceNo) {
                var goal = {
                    EmployeeGoal_SequenceNo: hdnEmployeeGoalSequenceNo.val(),
                    EmployeeGoalId: hdnEmployeeGoalId.val(),
                    GoalId: hdnGoalId.val(),
                    GoalName: txtGoalName.val(),
                    GoalDescription: txtGoalDescription.val(),
                    SectionId: ddlSectionName.val(),
                    SectionName: $("#ddlSectionName option:Selected").text(),
                    GoalCategoryId: ddlGoalCategoryName.val(),
                    GoalCategoryName:  $("#ddlGoalCategoryName option:Selected").text(),
                    EvalutionCriteria: txtEvalutionCriteria.val(),
                    StartDate: txtGoalStartDate.val(),
                    DueDate: txtGoalDueDate.val(),
                    Weight: txtWeight.val(),
                    SelfScore: txtSelfScore.val(),
                    SelfRemarks: txtSelfRemarks.val(),
                    AppraiserScore: txtAppraiserScore.val(),
                    AppraiserRemarks: txtAppraiserRemarks.val(),
                    ReviewScore: txtReviewScore.val(),
                    ReviewRemarks: txtReviewRemarks.val(),
                    FixedDyanmic: hdnFixedDynamic.val(),
                    EmployeeGoal_Status: chkGoalStatus
                };
                goalList.push(goal);
                hdnEmployeeGoalSequenceNo.val("0");
            }
        }
    });
    if (action == 1 && (hdnEmployeeGoalSequenceNo.val() == "" || hdnEmployeeGoalSequenceNo.val() == "0")) {
        hdnEmployeeGoalSequenceNo.val(goalEntrySequence);
    }
    if (action == 1) {
        var goalAddEdit = {
            EmployeeGoal_SequenceNo: hdnEmployeeGoalSequenceNo.val(),
            EmployeeGoalId: hdnEmployeeGoalId.val(),
            GoalId: hdnGoalId.val(),
            GoalName: txtGoalName.val(),
            GoalDescription: txtGoalDescription.val(),
            SectionId: ddlSectionName.val(),
            SectionName: $("#ddlSectionName option:Selected").text(),
            GoalCategoryId: ddlGoalCategoryName.val(),
            GoalCategoryName: $("#ddlGoalCategoryName option:Selected").text(),
            EvalutionCriteria: txtEvalutionCriteria.val(),
            StartDate: txtGoalStartDate.val(),
            DueDate: txtGoalDueDate.val(),
            Weight: txtWeight.val(),
            SelfScore: txtSelfScore.val(),
            SelfRemarks: txtSelfRemarks.val(),
            AppraiserScore: txtAppraiserScore.val(),
            AppraiserRemarks: txtAppraiserRemarks.val(),
            ReviewScore: txtReviewScore.val(),
            ReviewRemarks: txtReviewRemarks.val(),
            FixedDyanmic: hdnFixedDynamic.val(),
            EmployeeGoal_Status: chkGoalStatus
        };
        goalList.push(goalAddEdit);
        hdnEmployeeGoalSequenceNo.val("0");
    }
    GetEmployeeGoalList(goalList);
    
}
function EditGoalRow(obj) {
    var row = $(obj).closest("tr");
    var employeeGoalSequenceNo = $(row).find("#hdnEmployeeGoalSequenceNo").val();
    var employeeGoalId = $(row).find("#hdnEmployeeGoalId").val();
    var goalName = $(row).find("#hdnGoalName").val();
    var goalId = $(row).find("#hdnGoalId").val();
    var goalDescription = $(row).find("#hdnGoalDescription").val();
    var sectionId = $(row).find("#hdnSectionId").val();
    var goalCategoryId = $(row).find("#hdnGoalCategoryId").val();
    var evalutionCriteria = $(row).find("#hdnEvalutionCriteria").val();
    var startDate = $(row).find("#hdnStartDate").val();
    var dueDate = $(row).find("#hdnDueDate").val();
    var weight = $(row).find("#hdnWeight").val();
    var selfScore = $(row).find("#hdnSelfScore").val();
    var selfRemarks = $(row).find("#hdnSelfRemarks").val();
    var appraiserScore = $(row).find("#hdnAppraiserScore").val();
    var appraiserRemarks = $(row).find("#hdnAppraiserRemarks").val();
    var reviewScore = $(row).find("#hdnReviewScore").val();
    var reviewRemarks = $(row).find("#hdnReviewRemarks").val();

    var fixedDynamic = $(row).find("#hdnFixedDynamic").val();
    var employeeGoal_Status = $(row).find("#hdnEmployeeGoal_Status").val() == "Active" ? true : false;

    $("#txtGoalName").val(goalName);
    $("#hdnGoalId").val(goalId);
    $("#hdnEmployeeGoalId").val(employeeGoalId);
    $("#hdnEmployeeGoalSequenceNo").val(employeeGoalSequenceNo);
    $("#txtGoalDescription").val(goalDescription);
    $("#ddlSectionName").val(sectionId);
    $("#ddlGoalCategoryName").val(goalCategoryId);
    $("#txtWeight").val(weight);
    $("#txtSelfScore").val(selfScore);
    $("#txtSelfRemarks").val(selfRemarks);
    $("#txtAppraiserScore").val(appraiserScore);
    $("#txtAppraiserRemarks").val(appraiserRemarks);
    $("#txtReviewScore").val(reviewScore);
    $("#txtReviewRemarks").val(reviewRemarks);

    $("#txtEvalutionCriteria").val(evalutionCriteria);
    $("#txtGoalStartDate").val(startDate);
    $("#txtGoalDueDate").val(dueDate);
    if (employeeGoal_Status)
    {
        $("#chkGoalStatus").prop("checked", true);
    }
    else
    {
        $("#chkGoalStatus").prop("checked", false);
    }
    $("#hdnFixedDynamic").val(fixedDynamic);
    if (fixedDynamic == "F")
    {
        $("#txtGoalDescription").attr("disabled", true);
        $("#txtGoalName").attr("disabled",true);
        $("#ddlSectionName").attr("disabled",true)
        $("#ddlGoalCategoryName").attr("disabled", true)
        $("#txtWeight").attr("disabled", true)
        $("#txtGoalStartDate").attr("disabled", true)
        $("#txtGoalDueDate").attr("disabled", true)
        $("#txtSelfScore").attr("disabled", true)
        $("#txtSelfRemarks").attr("disabled", true)
        $("#txtAppraiserScore").attr("disabled", true)
        $("#txtAppraiserRemarks").attr("disabled", true)
    }
    else
    {
        $("#txtGoalDescription").attr("disabled", true);
        $("#txtGoalName").attr("disabled", true);
        $("#ddlSectionName").attr("disabled", true)
        $("#ddlGoalCategoryName").attr("disabled", true)
        $("#txtWeight").attr("disabled", true)
        $("#txtGoalStartDate").attr("disabled", true)
        $("#txtGoalDueDate").attr("disabled", true)
        $("#txtSelfScore").attr("disabled", true)
        $("#txtSelfRemarks").attr("disabled", true)
        $("#txtAppraiserScore").attr("disabled", true)
        $("#txtAppraiserRemarks").attr("disabled", true)
    }


    $("#btnUpdateGoal").show();
    ShowHideGoalPanel(1);
}
function ShowHideGoalPanel(action) {
    if (action == 1) {
        $(".goalsection").show();
    }
    else {
        $(".goalsection").hide();

        $("#txtGoalName").val("");
        $("#hdnGoalId").val("0");
        $("#hdnEmployeeGoalId").val("0");
        $("#txtGoalDescription").val("");
        $("#ddlSectionName").val("0");
        $("#ddlGoalCategoryName").val("0");
        $("#txtWeight").val("");
        $("#txtEvalutionCriteria").val("");
        $("#txtGoalStartDate").val("");
        $("#txtGoalDueDate").val("");
        $("#txtSelfScore").val("");
        $("#txtSelfRemarks").val("");
        $("#txtAppraiserScore").val("");
        $("#txtAppraiserRemarks").val("");
        $("#txtAppraiserScore").val("0");
        $("#txtAppraiserRemarks").val("");
        $("#chkGoalStatus").prop("checked", true);
        $("#hdnFixedDynamic").val("F");
        $("#txtGoalDescription").attr("disabled", true);
        $("#txtGoalName").attr("disabled", true);
        $("#ddlSectionName").attr("disabled", true)
        $("#ddlGoalCategoryName").attr("disabled", true)
        $("#txtWeight").attr("disabled", true)
        $("#txtGoalStartDate").attr("disabled", true)
        $("#txtGoalDueDate").attr("disabled", true)
        $("#txtSelfScore").attr("disabled", true)
        $("#txtSelfRemarks").attr("disabled", true)
        $("#txtAppraiserScore").attr("disabled", true)
        $("#txtAppraiserRemarks").attr("disabled", true)
        $("#btnUpdateGoal").hide();
    }
}

function GetEmployeeAppraisalTemplateMappingDetail(empAppraisalTemplateMappingId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PMS_EmployeeAppraisalTemplateMapping/GetEmployeeAppraisalTemplateMappingDetail",
        data: { empAppraisalTemplateMappingId: empAppraisalTemplateMappingId },
        dataType: "json",
        success: function (data) {
            $("#txtTemplateName").val(data.TemplateName);
            $("#hdnTemplateId").val(data.TemplateId);
            $("#txtDepartment").val(data.DepartmentName);
            $("#hdnDepartmentId").val(data.DepartmentId);
            $("#txtDesignation").val(data.DesignationName);
            $("#hdnDesignationId").val(data.DesignationId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#ddlFinYear").val(data.FinYearId);
            $("#ddlPerformanceCycle").val(data.PerformanceCycleId);
           
            
            if (data.EmpAppraisalTemplateMapping_Status == true) {
                $("#chkstatus").attr("checked", true);
            }
            else {
                $("#chkstatus").attr("checked", false);
            }
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