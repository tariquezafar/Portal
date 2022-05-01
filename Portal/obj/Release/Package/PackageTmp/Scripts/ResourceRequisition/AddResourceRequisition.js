$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtResourceRequisitionNo").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);

    $("#txtSkillCode").attr('readOnly', true);
    

    $("#txtInterviewStartDate").attr('readOnly', true);
    $("#txtHireByDate").attr('readOnly', true);

    $("#txtInterviewStartDate,#txtHireByDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    BindCompanyBranchList();
    BindEductaionList();
    BindInterviewTypeList();
    BindPositionLevelList();
    BindPositionTypeList();
    BindDepartmentList();
    $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
    BindCurrencyList();

    $("#txtSkillName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../ResourceRequisition/GetSkillAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.SkillName, value: item.SkillId,code: item.SkillCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtSkillName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtSkillName").val(ui.item.label);
            $("#hdnSkillId").val(ui.item.value);
            $("#txtSkillCode").val(ui.item.code);
      
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtSkillName").val("");
                $("#hdnSkillId").val("0");
                $("#txtSkillCode").val("");
                ShowModel("Alert", "Please select Skill from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b></div>")
      .appendTo(ul);
};

    $("#txtInterviewAssignToUserName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Lead/GetUserAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, companyBranchID: 0 },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FullName, value: item.UserName, UserId: item.UserId, MobileNo: item.MobileNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtInterviewAssignToUserName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtInterviewAssignToUserName").val(ui.item.label);
            $("#hdnInterviewAssignToUserId").val(ui.item.UserId);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtInterviewAssignToUserName").val("");
                $("#hdnInterviewAssignToUserId").val("0");
                ShowModel("Alert", "Please select User from List")
            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.value + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnResourceRequisitionId = $("#hdnResourceRequisitionId");
    if (hdnResourceRequisitionId.val() != "" && hdnResourceRequisitionId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetResourceRequisitionDetail(hdnResourceRequisitionId.val());
       }, 2000);

        

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $("#ddlCompanyBranch").attr('readOnly', true);

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
    var skills = [];
    GetSkillList(skills);
    var interviews = [];
    GetInterviewList(interviews);
    

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
 

function BindInterviewTypeList() {
    $("#ddlInterviewType").val(0);
    $("#ddlInterviewType").html("");
    $.ajax({
        type: "GET",
        url: "../ResourceRequisition/GetInterviewTypeList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlInterviewType").append($("<option></option>").val(0).html("-Select Interview Type-"));
            $.each(data, function (i, item) {
                $("#ddlInterviewType").append($("<option></option>").val(item.InterviewTypeId).html(item.InterviewTypeName));
            });
        },
        error: function (Result) {
            $("#ddlInterviewType").append($("<option></option>").val(0).html("-Select Interview Type-"));
        }
    });
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

function GetSkillList(skills) {
    var hdnResourceRequisitionId = $("#hdnResourceRequisitionId");
    var requestData = { skills: skills, resourcerequisitionId: hdnResourceRequisitionId.val() };
    $.ajax({
        url: "../ResourceRequisition/GetSkillList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divSkillList").html("");
            $("#divSkillList").html(err);
        },
        success: function (data) {
            $("#divSkillList").html("");
            $("#divSkillList").html(data);
          
            ShowHideSkillPanel(2);
        }
    });
}
  
function GetInterviewList(interviews) {
    var hdnResourceRequisitionId = $("#hdnResourceRequisitionId");
    var requestData = { interviews: interviews, resourcerequisitionId: hdnResourceRequisitionId.val() };
    $.ajax({
        url: "../ResourceRequisition/GetInterviewList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divInterviewDetailList").html("");
            $("#divInterviewDetailList").html(err);
        },
        success: function (data) {
            $("#divInterviewDetailList").html("");
            $("#divInterviewDetailList").html(data);

            ShowHideInterviewPanel(2);
        }
    });
}

function AddSkill(action) {
    var skillEntrySequence = 0;
    var hdnSkillSequenceNo = $("#hdnSkillSequenceNo");
    var hdnRequisitionSkillId = $("#hdnRequisitionSkillId");
    var txtSkillName = $("#txtSkillName");
    var txtSkillCode = $("#txtSkillCode"); 
    var hdnSkillId = $("#hdnSkillId");

    if (txtSkillName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Skill Name")
        txtSkillName.focus();
        return false;
    } 

    var skillList = [];
    if (action == 1 && (hdnSkillSequenceNo.val() == "" || hdnSkillSequenceNo.val() == "0")) {
        skillEntrySequence = 1;
    }
    $('#tblSkillList tr').each(function (i, row) {
        var $row = $(row);
        var skillSequenceNo = $row.find("#hdnSkillSequenceNo").val();
        var requisitionSkillId = $row.find("#hdnRequisitionSkillId").val();
        var skillId = $row.find("#hdnSkillId").val();
        var skillName = $row.find("#hdnSkillName").val();
        var skillCode = $row.find("#hdnSkillCode").val();  
        if (skillId != undefined) {
            if (action == 1 || (hdnSkillSequenceNo.val() != skillSequenceNo)) {

                if (skillId == hdnSkillId.val()) {
                    ShowModel("Alert", "Skill already added!!!")
                    return false;
                }
                var skill = {
                    RequisitionSkillId: requisitionSkillId,
                    SkillSequenceNo: skillSequenceNo,
                    SkillId: skillId,
                    SkillName: skillName,
                    SkillCode: skillCode
                };
                skillList.push(skill);
                skillEntrySequence = parseInt(skillEntrySequence) + 1;
            }
            else if (hdnSkillSequenceNo.val() == skillSequenceNo)
            {
                var skillAddEdit = {
                    SkillSequenceNo: hdnSkillSequenceNo.val(),
                    RequisitionSkillId:hdnRequisitionSkillId.val(),
                    SkillId: hdnSkillId.val(),
                    SkillName: txtSkillName.val().trim(),
                    SkillCode: txtSkillCode.val().trim()
                };
                skillList.push(skillAddEdit);
                hdnSkillSequenceNo.val("0");
            }
        } 
    });
    if (action == 1 && (hdnSkillSequenceNo.val() == "" || hdnSkillSequenceNo.val() == "0")) {
        hdnSkillSequenceNo.val(skillEntrySequence);
    }
    if (action == 1) {
        var skillAddEdit = {
            SkillSequenceNo: hdnSkillSequenceNo.val(),
            RequisitionSkillId: hdnRequisitionSkillId.val(),
            SkillId: hdnSkillId.val(), 
            SkillName: txtSkillName.val().trim(),
            SkillCode: txtSkillCode.val().trim()
        };
        skillList.push(skillAddEdit);
        hdnSkillSequenceNo.val("0");
    }
    GetSkillList(skillList);
}
function EditSkillRow(obj) { 
    var row = $(obj).closest("tr"); 
    var requisitionSkillId = $(row).find("#hdnRequisitionSkillId").val();
    var skillId = $(row).find("#hdnSkillId").val();
    var skillName = $(row).find("#hdnSkillName").val();
    var skillCode = $(row).find("#hdnSkillCode").val(); 
    var skillSequenceNo = $(row).find("#hdnSkillSequenceNo").val();
    

    $("#hdnRequisitionSkillId").val(requisitionSkillId);
    $("#hdnSkillSequenceNo").val(skillSequenceNo);
    $("#txtSkillName").val(skillName);
    $("#hdnSkillId").val(skillId);
    $("#txtSkillCode").val(skillCode); 
     
    $("#btnAddSkill").hide();
    $("#btnUpdateSkill").show();
    ShowHideSkillPanel(1);
}

function RemoveSkillRow(obj) {
    if (confirm("Do you want to remove selected Skill?")) {
        var row = $(obj).closest("tr");
        var SkillId = $(row).find("#hdnSkillId").val();
        ShowModel("Alert", "Skill Removed from List.");
        row.remove();
         
    }
}
 

function AddInterview(action) {
    var interviewEntrySequence = 0;
    var hdnRequisitionInterviewStagesId = $("#hdnRequisitionInterviewStagesId");
    var hdnInterviewSequenceNo = $("#hdnInterviewSequenceNo");
    var ddlInterviewType = $("#ddlInterviewType");  
    var txtInterviewDescription = $("#txtInterviewDescription");
    var txtInterviewAssignToUserName = $("#txtInterviewAssignToUserName");
    var hdnInterviewAssignToUserId = $("#hdnInterviewAssignToUserId");
    

    if (ddlInterviewType.val() == "" || ddlInterviewType.val() == "0") {
        ShowModel("Alert", "Please select Interview Type")
        return false; 
    } 
    if (txtInterviewDescription.val().trim() == "") {
        ShowModel("Alert", "Please Enter Interview Description")
        return false;
    }
    if (txtInterviewAssignToUserName.val() == "" || txtInterviewAssignToUserName.val() == "0") {
        ShowModel("Alert", "Please Select User Name for interview")
        return false;
    }
    var interviewList = [];
    if (action == 1 && (hdnInterviewSequenceNo.val() == "" || hdnInterviewSequenceNo.val() == "0")) {
        interviewEntrySequence = 1;
    }
    $('#tblInterviewList tr').each(function (i, row) {
        var $row = $(row);
        var interviewSequenceNo = $row.find("#hdnInterviewSequenceNo").val();
        var requisitionInterviewStagesId = $row.find("#hdnRequisitionInterviewStagesId").val();
        var interviewTypeId = $row.find("#hdnInterviewTypeId").val();
        var interviewTypeName = $row.find("#hdnInterviewTypeName").val(); 
        var interviewDescription = $row.find("#hdnInterviewDescription").val();
        var interviewassigntoUserId = $row.find("#hdnInterviewAssignToUserId").val();
        var interviewassigntoUserName = $row.find("#hdnInterviewAssignToUserName").val();
        if (requisitionInterviewStagesId != undefined) {
            if (action == 1 || (hdnInterviewSequenceNo.val() != interviewSequenceNo)) {
               
                var interview = {
                    RequisitionInterviewStagesId: requisitionInterviewStagesId,
                    InterviewSequenceNo: interviewSequenceNo,
                    InterviewTypeId: interviewTypeId,
                    InterviewTypeName:interviewTypeName,
                    InterviewDescription: interviewDescription,
                    InterviewAssignToUserId: interviewassigntoUserId,
                   InterviewassigntoUserName: interviewassigntoUserName
                };
                interviewList.push(interview);
                interviewEntrySequence = parseInt(interviewEntrySequence) + 1;
            }
            else if (hdnInterviewSequenceNo.val() == interviewSequenceNo) {
                var interviewAddEdit = {
                    RequisitionInterviewStagesId: hdnRequisitionInterviewStagesId.val(),
                    InterviewSequenceNo: hdnInterviewSequenceNo.val(),
                    InterviewTypeId: ddlInterviewType.val(),
                    InterviewTypeName: $("#ddlInterviewType option:selected").text(),
                    InterviewDescription: txtInterviewDescription.val().trim(),
                    InterviewAssignToUserId: hdnInterviewAssignToUserId.val(),
                    InterviewAssignToUserName: txtInterviewAssignToUserName.val()
                };
                interviewList.push(interviewAddEdit);
                hdnInterviewSequenceNo.val("0");
            }
        }
    });
    if (action == 1 && (hdnInterviewSequenceNo.val() == "" || hdnInterviewSequenceNo.val() == "0")) {
        hdnInterviewSequenceNo.val(interviewEntrySequence);
    }
    if (action == 1) {
        var interviewAddEdit = {
            RequisitionInterviewStagesId: hdnRequisitionInterviewStagesId.val(),
            InterviewSequenceNo: hdnInterviewSequenceNo.val(),
            InterviewTypeId: ddlInterviewType.val(),
            InterviewTypeName: $("#ddlInterviewType option:selected").text(),
            InterviewDescription: txtInterviewDescription.val().trim(),
            InterviewAssignToUserId: hdnInterviewAssignToUserId.val(),
            InterviewAssignToUserName: txtInterviewAssignToUserName.val()
        };
        interviewList.push(interviewAddEdit);
        hdnInterviewSequenceNo.val("0");
    }
    GetInterviewList(interviewList);
}
function EditInterviewRow(obj) {
    var row = $(obj).closest("tr");
    var requisitionInterviewStagesId = $row.find("#hdnRequisitionInterviewStagesId").val();
    var interviewSequenceNo = $(row).find("#hdnInterviewSequenceNo").val();
    var interviewTypeId = $row.find("#hdnInterviewTypeId").val();
    var interviewDescription = $row.find("#hdnInterviewDescription").val();
    var interviewassigntoUserName = $row.find("#hdnInterviewAssignToUserName").val();
    var interviewassigntoUserId = $row.find("#hdnInterviewAssignToUserId").val();
    
    $("#hdnRequisitionInterviewStagesId").val(requisitionInterviewStagesId);
    $("#hdnInterviewSequenceNo").val(interviewSequenceNo);
    $("#ddlInterviewType").val(interviewTypeId);
    $("#txtInterviewDescription").val(interviewDescription);
    $("#txtInterviewAssignToUserName").val(interviewassigntoUserName);
    $("#hdnInterviewAssignToUserId").val(interviewassigntoUserId);

    $("#btnAddInterview").hide();
    $("#btnUpdateInterview").show();
    ShowHideInterviewPanel(1);
}

function RemoveInterviewRow(obj) {
    if (confirm("Do you want to remove selected Interview Detail?")) {
        var row = $(obj).closest("tr");
        var requisitionInterviewStagesId = $(row).find("#hdnRequisitionInterviewStagesId").val();
        ShowModel("Alert", "Interview Detail Removed from List.");
        row.remove();
      
    }
}
 
function GetResourceRequisitionDetail(resourceRequisitionId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ResourceRequisition/GetResourceRequisitionDetail",
        data: { requisitionId: resourceRequisitionId },
        dataType: "json",
        success: function (data) {
            $("#txtResourceRequisitionNo").val(data.RequisitionNo);
            $("#hdnResourceRequisitionId").val(data.RequisitionId);
            $("#txtNumberofResource").val(data.NumberOfResources);
            $("#ddlPositionLevel").val(data.PositionLevelId);
            $("#ddlPriorityLevel").val(data.PriorityLevel);
            $("#ddlDepartment").val(data.DepartmentId);
            BindDesignationList(data.DesignationId);
            $("#ddlDesignation").val(data.DesignationId);
            $("#ddlPositionType").val(data.PositionTypeId);
            EnableDisableContractPeriod();
            $("#ddlContractPeriod").val(data.ContractPeriod);
            $("#ddlEducation").val(data.EducationId);
            $("#txtJobDescription").val(data.JobDescription);
            $("#txtOtherQualification").val(data.OtherQualification);
            $("#ddlMinExp").val(data.MinExp);
            $("#ddlMaxExp").val(data.MaxExp);
            $("#txtMinSalary").val(data.MinSalary);
            $("#txtMaxSalary").val(data.MaxSalary);
            $("#ddlCurrency").val(data.CurrencyCode);
            $("#txtRemark").val(data.Remarks);
            $("#txtJustificationNotes").val(data.JustificationNotes);
            $("#txtInterviewStartDate").val(data.InterviewStartDate);
            $("#txtHireByDate").val(data.HireByDate);

            $("#ddlApprovalStatus").val(data.RequisitionStatus);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);



            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);

            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            if (data.RejectedReason != "") {
                $("#divReject").show();
                $("#txtRejectReason").val(data.RejectedReason);
                $("#txtRejectedDate").val(data.RejectedDate);
            }


            $("#btnAddNew").show();
             
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() {
    var txtResourceRequisitionNo = $("#txtResourceRequisitionNo");
    var hdnResourceRequisitionId = $("#hdnResourceRequisitionId");
    var txtNumberofResource = $("#txtNumberofResource");
    var ddlPositionLevel = $("#ddlPositionLevel");
    var ddlPriorityLevel = $("#ddlPriorityLevel");
    var ddlDepartment = $("#ddlDepartment");
    
    var ddlDesignation = $("#ddlDesignation");

    var ddlPositionType = $("#ddlPositionType");
    var ddlContractPeriod = $("#ddlContractPeriod");

    var ddlEducation = $("#ddlEducation");
    var txtJobDescription = $("#txtJobDescription");
    var txtOtherQualification = $("#txtOtherQualification");

    var ddlMinExp = $("#ddlMinExp");
    var ddlMaxExp = $("#ddlMaxExp");
    var txtMinSalary = $("#txtMinSalary");
    var txtMaxSalary = $("#txtMaxSalary");
    var ddlCurrency = $("#ddlCurrency");
    var txtRemark = $("#txtRemark");
    var txtJustificationNotes = $("#txtJustificationNotes");
    var txtInterviewStartDate = $("#txtInterviewStartDate");
    var txtHireByDate = $("#txtHireByDate");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
   
    
    if (txtNumberofResource.val().trim() == "" || parseInt(txtNumberofResource.val().trim())<=0) {
        ShowModel("Alert", "Please Enter valid Number Of Resources")
        txtNumberofResource.focus();
        return false;
    }
    if (ddlPositionLevel.val() == "" || ddlPositionLevel.val() == "0") {
        ShowModel("Alert", "Please select Position Level")
        return false;

    } 
    if (ddlPriorityLevel.val() == "" || ddlPriorityLevel.val() == "0") {
        ShowModel("Alert", "Please select Priority Level")
        return false;
    }
    if (ddlDepartment.val() == "" || ddlDepartment.val() == "0") {
        ShowModel("Alert", "Please select Department")
        return false;
    }
    if (ddlDesignation.val() == "" || ddlDesignation.val() == "0") {
        ShowModel("Alert", "Please select Designation")
        return false;
    }
   
    if (ddlPositionType.val() == "" || ddlPositionType.val() == "0") {
        ShowModel("Alert", "Please select Position Type")
        return false;
    }

    if (ddlEducation.val() == "" || ddlEducation.val() == "0") {
        ShowModel("Alert", "Please select Education Level")
        return false;
    }
    if (txtJobDescription.val().trim() == "" ) {
        ShowModel("Alert", "Please enter job description")
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
    if (txtJustificationNotes.val().trim() == "") {
        ShowModel("Alert", "Please enter Justification note")
        return false;
    }

    if (txtInterviewStartDate.val().trim() == "") {
        ShowModel("Alert", "Please select Interview Start Date")
        return false;
    }
    if (txtHireByDate.val().trim() == "") {
        ShowModel("Alert", "Please select Hire by Date")
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Location")
        return false;
    }

    var rrViewModel = {
        RequisitionId: hdnResourceRequisitionId.val(),
        RequisitionNo: txtResourceRequisitionNo.val().trim(),
        NumberOfResources: txtNumberofResource.val().trim(),
        PositionLevelId: ddlPositionLevel.val(),
        PriorityLevel: ddlPriorityLevel.val(),
        DepartmentId: ddlDepartment.val(),
        DesignationId: ddlDesignation.val(),
        PositionTypeId: ddlPositionType.val(), 
        ContractPeriod: ddlContractPeriod.val(),
        EducationId: ddlEducation.val(),
        JobDescription: txtJobDescription.val().trim(),
        OtherQualification: txtOtherQualification.val().trim(),
        MinExp: ddlMinExp.val(),
        MaxExp: ddlMaxExp.val(),
            MinSalary: txtMinSalary.val().trim() ==""?"0": txtMinSalary.val().trim(),
            MaxSalary: txtMaxSalary.val().trim() ==""?"0": txtMaxSalary.val().trim(),
        CurrencyCode: ddlCurrency.val(),
        Remark: txtRemark.val().trim(),
        JustificationNotes: txtJustificationNotes.val().trim(),
        InterviewStartDate:txtInterviewStartDate.val().trim(),
        HireByDate: txtHireByDate.val().trim(),
        RequisitionStatus: ddlApprovalStatus.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
    };
     
    var skillList = [];
    $('#tblSkillList tr').each(function (i, row) {
        var $row = $(row);
        var skillId = $row.find("#hdnSkillId").val();
        if (skillId != undefined) {
            var skill = {
                SkillId: skillId
            };
            skillList.push(skill);
        }

    });

    var interviewList = [];
    $('#tblInterviewList tr').each(function (i, row) {
        var $row = $(row);
        var interviewTypeId = $row.find("#hdnInterviewTypeId").val();
        var interviewDescription = $row.find("#hdnInterviewDescription").val();
        var interviewassigntoUserId = $row.find("#hdnInterviewAssignToUserId").val();

        if (interviewTypeId != undefined) {
            var interview = {
                InterviewTypeId: interviewTypeId,
                InterviewDescription: interviewDescription,
                InterviewAssignToUserId: interviewassigntoUserId
            };
            interviewList.push(interview);
        }

    });

    var requestData = { resourceRequisitionViewModel: rrViewModel, skills: skillList, interviews: interviewList };
    var accessMode = 1;//Add Mode
    if (hdnResourceRequisitionId.val() != null && hdnResourceRequisitionId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    $.ajax({
        url: "../ResourceRequisition/AddEditResourceRequisition?accessMode=" + accessMode + "",
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
                      window.location.href = "../ResourceRequisition/ListResourceRequisition";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../ResourceRequisition/AddEditResourceRequisition";
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

    $("#txtResourceRequisitionNo").val("");
    $("#hdnResourceRequisitionId").val("0");
    $("#txtNumberofResource").val("0");
    $("#ddlPositionLevel").val("0");
    $("#ddlPriorityLevel").val("0");
    $("#ddlDepartment").val("0");
    $("#ddlDesignation").val("0");
    $("#ddlPositionType").val("0");
    $("#ddlContractPeriod").val("0");
    $("#ddlEducation").val("0");
    $("#txtJobDescription").val("");
    $("#txtOtherQualification").val("");
    $("#ddlMinExp").val("0");
    $("#ddlMaxExp").val("0");
    $("#txtMinSalary").val("");
    $("#txtMaxSalary").val("");
    $("#ddlCurrency").val("0");
    $("#txtRemark").val("");
    $("#ddlApprovalStatus").val("Draft");
    
    $("#txtJustificationNotes").val("");
    $("#txtInterviewStartDate").val("");
    $("#txtHireByDate").val("");
      
    $("#divInterviewDetailList").html("");
    $("#divSkillList").html("");
    $("#btnSave").show();
    $("#btnUpdate").hide();
    $("##ddlCompanyBranch").val("0");

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
 
 
function EnableDisableContractPeriod()
{
    var positionTpe = $("#ddlPositionType option:selected").text();
    if (positionTpe=="Contract")
    {
        $("#ddlContractPeriod").attr("disabled", false);
    }
    else
    {
        $("#ddlContractPeriod").attr("disabled", true);
        $("#ddlContractPeriod").val("0");
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