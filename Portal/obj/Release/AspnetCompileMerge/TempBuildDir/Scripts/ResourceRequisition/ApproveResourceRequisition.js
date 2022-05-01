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
    $("#ddlContractPeriod").prop("disabled", true);
    
    

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

    
    BindEductaionList();
    BindPositionLevelList();
    BindPositionTypeList();
    BindDepartmentList();
    $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
    BindCurrencyList();

    
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnResourceRequisitionId = $("#hdnResourceRequisitionId");
    if (hdnResourceRequisitionId.val() != "" && hdnResourceRequisitionId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetResourceRequisitionDetail(hdnResourceRequisitionId.val());
       }, 2000);

        

        if (hdnAccessMode.val() == "3") {
            $("#btnUpdate").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            //$("#ddlContractPeriod").prop('disabled', true);
            $("#ddlContractPeriod").prop("disabled", true);
    

        }
        else {
            $("#btnUpdate").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            
            $("#ddlApprovalStatus").attr('disabled', false);
            $("#txtRejectedReason").attr('disabled', true);
            $("#txtRejectedReason").attr('readOnly', false);

            $("#btnUpdate").show();
        
        }
    }
    else {
        
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

        }
    });
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

            $("#ddlApprovalStatus").val(data.ApprovalStatus);




            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);

            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }

            $("#txtRejectedReason").val(data.RejectedReason);
            $("#btnAddNew").show();
             
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() {
    
    var hdnResourceRequisitionId = $("#hdnResourceRequisitionId");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtRejectedReason = $("#txtRejectedReason");
    if (ddlApprovalStatus.val().trim() == "0" || ddlApprovalStatus.val().trim() == "")
    {
        ShowModel("Alert", "Please select Approved/ Rejdcted")
        ddlApprovalStatus.focus();
        return false;
    }
   
    if (ddlApprovalStatus.val().trim() == "Rejected" && txtRejectedReason.val().trim()=="") {
        ShowModel("Alert", "Please Enter Rejection Reason")
        txtRejectedReason.focus();
        return false;
        }
       var rrViewModel = {
            RequisitionId: hdnResourceRequisitionId.val(),
            ApprovalStatus: ddlApprovalStatus.val(),
            RejectedReason:txtRejectedReason.val().trim()
        };
       var accessMode = 1;//Add Mode
       if (hdnResourceRequisitionId.val() != null && hdnResourceRequisitionId.val() != 0) {
           accessMode = 2;//Edit Mode
       }
    
    var requestData = { resourceRequisitionViewModel: rrViewModel };
    $.ajax({
        url: "../ResourceRequisition/ApproveResourceRequisition?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../ResourceRequisition/ListResourceRequisitionApproval";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../ResourceRequisition/ApproveResourceRequisition";
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
    $("#txtJustificationNotes").val("");
    $("#txtInterviewStartDate").val("");
    $("#txtHireByDate").val("");
    $("#ddlApprovalStatus").val("");    
    $("#divInterviewDetailList").html("");
    $("#divSkillList").html("");
    $("#btnSave").show();
    $("#btnUpdate").hide();


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
function EnableDisableRejectReason()
{
    var approvalStatus = $("#ddlApprovalStatus option:selected").text();
    if (approvalStatus == "Rejected") {
        $("#txtRejectedReason").attr("disabled", false);
    }
    else {
        $("#txtRejectedReason").attr("disabled", true);
        $("#txtRejectedReason").val("");
    }
}