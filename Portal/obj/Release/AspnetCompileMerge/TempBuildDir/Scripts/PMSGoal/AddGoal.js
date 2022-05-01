$(document).ready(function () {
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtGoalStartDate").attr('readOnly', true);
    $("#txtGoalDueDate").attr('readOnly', true);
    $("#txtGoalStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',      
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
    BindPMSGoalCategoryList();
    BindPMSSectionList();
    BindPerformanceCycleList();
    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdngoalId = $("#hdngoalId");
    if (hdngoalId.val() != "" && hdngoalId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetGoalDetail(hdngoalId.val());
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
            $("#ddlSectionName,#ddlNewSectionName").append($("<option></option>").val(0).html("Select Section"));
            $.each(data, function (i, item) {
                $("#ddlSectionName,#ddlNewSectionName").append($("<option></option>").val(item.SectionId).html(item.SectionName));
            });
        },
        error: function (Result) {
            $("#ddlSectionName,#ddlNewSectionName").append($("<option></option>").val(0).html("Select Section"));
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
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            if (data.Goal_Status == true) {
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


             
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

function SaveData() {
    var txtGoalName = $("#txtGoalName");
    var hdngoalId = $("#hdngoalId");
    var txtGoalDescription = $("#txtGoalDescription");
    var ddlGoalCategoryName = $("#ddlGoalCategoryName");
    var ddlPerformanceName = $("#ddlPerformanceName");
    var ddlSectionName = $("#ddlSectionName");
    var txtWeight = $("#txtWeight");
    var txtGoalStartDate = $("#txtGoalStartDate");
    var txtGoalDueDate = $("#txtGoalDueDate");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    
    if (txtGoalName.val().trim() == "" ) {
        ShowModel("Alert", "Enter Goal Name")
        return false;

    }
    if (txtGoalDescription.val().trim() == "") {
        ShowModel("Alert", "Enter Goal Description Name")
        return false;

    }
    if (ddlGoalCategoryName.val() == "" || ddlGoalCategoryName.val() == "0") {
        ShowModel("Alert", "Select Goal Category Name")
        return false;
    }
 
    if (ddlPerformanceName.val() == "" || ddlPerformanceName.val() == "0") {
        ShowModel("Alert", "Select Performance Name")
        return false;
    }
    if (ddlSectionName.val() == "" || ddlSectionName.val() == "0") {
        ShowModel("Alert", "Select Section Name")
        return false;
    }
    if (txtWeight.val() == "" ) {
        ShowModel("Alert", "Enter Weight Name")
        return false;
    }
    if ((txtWeight.val() <= 0) || (txtWeight.val() > 100)) {
        ShowModel("Alert", "Please Enter the correct weight")
        txtWeight.focus();
        return false;
    }
    if (txtGoalStartDate.val() == "") {
        ShowModel("Alert", "Enter Start Date Name")
        return false;
    }
    if (txtGoalDueDate.val() == "") {
        ShowModel("Alert", "Enter Due Date Name")
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    var eDate = new Date(txtGoalDueDate.val());
    var sDate = new Date(txtGoalStartDate.val());
    if (sDate > eDate)
    {
        ShowModel("Alert", "Goal Start Date Should be Less than Goal Due Date.")
        return false;
    }

    var goalViewModel = {
        GoalId: hdngoalId.val(),
        GoalName: txtGoalName.val().trim(),
        GoalDescription: txtGoalDescription.val().trim(),
        SectionId: ddlSectionName.val(),
        GoalCategoryId: ddlGoalCategoryName.val(),
        PerformanceCycleId: ddlPerformanceName.val(), 
        StartDate: txtGoalStartDate.val(),
        DueDate: txtGoalDueDate.val(),
        Weight: txtWeight.val(),
        GoalStatus: chkstatus,
        CompanyBranchId: ddlCompanyBranch.val(),
    };
    var accessMode = 1;//Add Mode
    if (hdngoalId.val() != null && hdngoalId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    

    var requestData = { pmsgoalViewModel: goalViewModel };
    $.ajax({
        url: "../PMSGoal/AddEditGoal?accessMode=" + accessMode + "",
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
                      window.location.href = "../PMSGoal/ListGoal";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PMSGoal/AddEditGoal";
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
    $("#txtGoalName").val("");
    $("#txtGoalDescription").val("");
    $("#txtWeight").val("");
    $("#txtGoalStartDate").val("");
    $("#txtGoalDueDate").val("");
    $("#ddlSectionName").val("0");
    $("#ddlGoalCategoryName").val("0");
    $("#ddlPerformanceName").val("0");
    $("#btnSave").show();
    $("#btnUpdate").hide();
    $("#ddlCompanyBranch").val("0");
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
 
 
 