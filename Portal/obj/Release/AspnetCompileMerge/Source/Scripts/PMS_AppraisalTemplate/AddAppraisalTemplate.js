$(document).ready(function () { 
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) { 
        }
    });
    BindCompanyBranchList();
    BindPMSGoalCategoryList();
    BindPMSSectionList();
    BindPerformanceCycleList();
    BindDepartmentList();
    $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnTemplateId = $("#hdnTemplateId");
    if (hdnTemplateId.val() != "" && hdnTemplateId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetAppraisalTemplateDetail(hdnTemplateId.val());
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

    var templateGoals = [];
    GetAppraisalTemplateGoalList(templateGoals);

  
 
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


function GetAppraisalTemplateGoalList(templateGoals) {
    var hdnTemplateId = $("#hdnTemplateId");
    var requestData = { templateGoals: templateGoals, templateId: hdnTemplateId.val() };
    $.ajax({
        url: "../PMS_AppraisalTemplate/GetAppraisalTemplateGoalList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divGoalList").html("");
            $("#divGoalList").html(err);
        },
        success: function (data) {
            $("#divGoalList").html("");
            $("#divGoalList").html(data);
            ShowHideAppraisalTemplateGoalPanel(2);
        }
    });
}


function GetAppraisalTemplateDetail(templateId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PMS_AppraisalTemplate/GetAppraisalTemplateDetail",
        data: { templateId: templateId },
        dataType: "json",
        success: function (data) {
            $("#txtTemplateName").val(data.TemplateName);
            $("#txtQuotationDate").val(data.QuotationDate);
            $("#hdnTemplateId").val(data.TemplateId);
            $("#ddlDepartment").val(data.DepartmentId);
            BindDesignationList(data.DesignationId);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            if (data.AppraisalTemplate_Status == true) {
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
            $("#btnPrint").show();
            $("#btnEmail").show(); 
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}

function AddAppraisalTemplateGoal(action) {
    var taxEntrySequence = 0;
    var flag = true;
    var hdnTaxSequenceNo = $("#hdnTaxSequenceNo"); 
    var hdnTemplateGoalId = $("#hdnTemplateGoalId");
    var hdnGoalId = $("#hdnGoalId"); 
    var txtGoalName = $("#txtGoalName");
    var chkstatusGoal = $("#chkstatusGoal").is(':checked') ? true : false; 
   
    if (txtGoalName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Goal Name")
        txtGoalName.focus();
        return false;
    }
    
    var tamplateGoalList = [];
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        taxEntrySequence = 1;
    }
    $('#tblGoalList tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var templateGoalId = $row.find("#hdnTemplateGoalId").val();
        var goalId = $row.find("#hdnGoalId").val();
        var goalName = $row.find("#hdnGoalName").val();
        var goalStatus = $row.find("#hdnStatus").val()=="true"?true:false;
        
        if (goalId != undefined) {
            if (action == 1 || (hdnTaxSequenceNo.val() != taxSequenceNo)) { 
                if (goalName == txtGoalName.val()) {
                    ShowModel("Alert", "Goal Name already added!!!")
                    txtTaxName.focus();
                    flag = false;
                    return false;
                } 
                var tamplateGoal = {
                    TemplateGoalId: templateGoalId,
                    TaxSequenceNo:taxSequenceNo,
                    GoalId: goalId,
                    GoalName: goalName,
                    Goal_Status: goalStatus
                };
                tamplateGoalList.push(tamplateGoal);
                taxEntrySequence = parseInt(taxEntrySequence) + 1;

            }
        
            else if (hdnTemplateGoalId.val() == templateGoalId && hdnTaxSequenceNo.val() == taxSequenceNo)
        {
                var tamplateGoal = {
                TemplateGoalId: hdnTemplateGoalId.val(),
                TaxSequenceNo: hdnTaxSequenceNo.val(),
                GoalId: hdnGoalId.val(),
                GoalName: txtGoalName.val().trim(),
                Goal_Status: chkstatusGoal
             
            };
                tamplateGoalList.push(tamplateGoal);
            hdnTaxSequenceNo.val("0");
        }
    }   
    });
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        hdnTaxSequenceNo.val(taxEntrySequence);
    }
    if (action == 1)
        {
        var tamplateGoalAddEdit = {
           TemplateGoalId: hdnTemplateGoalId.val(),
           TaxSequenceNo: hdnTaxSequenceNo.val(),
           GoalId: hdnGoalId.val(),
           GoalName: txtGoalName.val().trim(),
           Goal_Status: chkstatusGoal
        };
        tamplateGoalList.push(tamplateGoalAddEdit);
       hdnTaxSequenceNo.val("0");
    }
    if (flag == true) {
        GetAppraisalTemplateGoalList(tamplateGoalList);
    }
}


function EditAppraisalTemplateGoalRow(obj) {
    var row = $(obj).closest("tr");
    var templateGoalId = $(row).find("#hdnTemplateGoalId").val();
    var taxSequenceNo = $(row).find("#hdnTaxSequenceNo").val();
    var goalId = $(row).find("#hdnGoalId").val();
    var goalName = $(row).find("#hdnGoalName").val();
    var goalStatus = $(row).find("#hdnStatus").val() == "true" ? true : false;
    $("#txtGoalName").val(goalName);
    $("#hdnTemplateGoalId").val(templateGoalId);
    $("#hdnTaxSequenceNo").val(taxSequenceNo);
    $("#hdnGoalId").val(goalId);
    $("#hdnStatus").val(goalStatus);
    $("#btnAddAppraisalTemplateGoal").hide();
    $("#btnUpdateAppraisalTemplateGoal").show();
    ShowHideAppraisalTemplateGoalPanel(1);  
   
} 
function RemoveAppraisalTemplateGoalRow(obj) {
    if (confirm("Do you want to remove selected Goal?")) {
        var row = $(obj).closest("tr");
        var templateGoalId = $(row).find("#hdnTemplateGoalId").val();
        ShowModel("Alert", "Goal Removed from List.");
        row.remove();  
    }
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
            $("#ddlPerformanceName").append($("<option></option>").val(0).html("Select Performance Cycle"));
            $.each(data, function (i, item) {
                $("#ddlPerformanceName").append($("<option></option>").val(item.PerformanceCycleId).html(item.PerformanceCycleName));
            });
        },
        error: function (Result) {
            $("#ddlPerformanceName").append($("<option></option>").val(0).html("Select Performance Cycle"));
        }
    });
}


function OpenSOSearchPopup() {
    $("#SearchGoalModel").modal();

}
function SearchGoal() {
    var txtGoalName = $("#txtGoalNameSearch");
    var ddlSectionName = $("#ddlSectionName");
    var ddlGoalCategoryName = $("#ddlGoalCategoryName");
    var ddlPerformanceName = $("#ddlPerformanceName"); 
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var requestData = {
        goalName: txtGoalName.val().trim(), sectionId: ddlSectionName.val(), goalCategoryId: ddlGoalCategoryName.val(),
        performanceCycleId: ddlPerformanceName.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val()
    }; 
    $.ajax({
        url: "../PMS_AppraisalTemplate/GetTemplateGoalList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divTemplateGoalList").html("");
            $("#divTemplateGoalList").html(err);
        },
        success: function (data) {
            $("#divTemplateGoalList").html("");
            $("#divTemplateGoalList").html(data);
        }
    });
}
function SelectGoal(goalId, goalName) {
    $("#hdnGoalId").val(goalId);
    $("#txtGoalName").val(goalName);
    $("#SearchGoalModel").modal('hide'); 
    GetGoalDetail(goalId);
}
function GetGoalDetail(goalId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PMSGoal/GetGoalDetail",
        data: { goalId: goalId },
        dataType: "json",
        success: function (data) { 
            $("#txtGoalName").val(data.GoalName);
            $("#hdngoalId").val(data.GoalId);
            $("#txtGoalDescription").val(data.GoalDescription);
            $("#ddlGoalCategoryName").val(data.GoalCategoryId);
            $("#ddlPerformanceName").val(data.PerformanceCycleId);
            $("#ddlSectionName").val(data.SectionId);
            $("#txtWeight").val(data.Weight);
            $("#txtGoalStartDate").val(data.StartDate);
            $("#txtGoalDueDate").val(data.DueDate);
            $("#chkstatus").val(data.GoalStatus);
       
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

    
}
function SaveData() {
    var txtTemplateName = $("#txtTemplateName");
    var hdnTemplateId = $("#hdnTemplateId"); 
    var ddlDepartment = $("#ddlDepartment");
    var ddlDesignation = $("#ddlDesignation");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;  
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (txtTemplateName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Template Name")
        txtTemplateName.focus();
        return false;
    }
    if (ddlDepartment.val() == "" || ddlDepartment.val() == "0") {
        ShowModel("Alert", "Please select Department")
        ddlDepartment.focus();
        return false;
    }
    if (ddlDesignation.val() == "" || ddlDesignation.val() == "0") {
        ShowModel("Alert", "Please select Designation")
        ddlDesignation.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    var templateViewModel = {
        TemplateId: hdnTemplateId.val(),
        TemplateName: txtTemplateName.val().trim(), 
        DepartmentId: ddlDepartment.val(),
        DesignationId: ddlDesignation.val(), 
        AppraisalTemplate_Status: chkstatus,
        CompanyBranchId: ddlCompanyBranch.val(),
    }; 
     

    var tamplateGoalList = [];
    $('#tblGoalList tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var templateGoalId = $row.find("#hdnTemplateGoalId").val();
        var goalId = $row.find("#hdnGoalId").val();
        var goalName = $row.find("#hdnGoalName").val();
        var goalStatus = $row.find("#hdnStatus").val() == "true" ? true : false;
        if (templateGoalId != undefined) {
            var tamplateGoal = {
                TemplateGoalId: templateGoalId,
                TaxSequenceNo: taxSequenceNo,
                GoalId: goalId,
                Goal_Status: goalStatus
            };
            tamplateGoalList.push(tamplateGoal);
        } 
    });

    var accessMode = 1;//Add Mode
    if (hdnTemplateId.val() != null && hdnTemplateId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { templateViewModel: templateViewModel, templateGoals: tamplateGoalList };
    $.ajax({
        url: "../PMS_AppraisalTemplate/AddEditAppraisalTemplate?accessMode=" + accessMode + "",
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
                      window.location.href = "../PMS_AppraisalTemplate/ListAppraisalTemplate";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PMS_AppraisalTemplate/AddEditAppraisalTemplate";
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
    $("#ddlDepartment").val("0");
    $("#ddlDesignation").val("0");  
    $("#chkstatus").val(""); 
    $("#btnSave").show();
    $("#btnUpdate").hide();
    $("#ddlCompanyBranch").val("0");
}
 
function ShowHideAppraisalTemplateGoalPanel(action) {
    if (action == 1) {
        $(".appraisaltemplategoalsection").show();
    }
    else {
        $(".appraisaltemplategoalsection").hide();
        $("#txtGoalName").val("");
        $("#hdnGoalId").val("0");
        $("#hdnTemplateGoalId").val("0");
        $("#btnAddAppraisalTemplateGoal").show();
        $("#btnUpdateAppraisalTemplateGoal").hide();
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