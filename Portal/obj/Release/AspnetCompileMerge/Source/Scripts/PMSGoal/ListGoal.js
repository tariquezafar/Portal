
$(document).ready(function () { 
    $("#txtGoalStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    BindPMSGoalCategoryList();
    BindPMSSectionList();
    BindPerformanceCycleList();
    BindCompanyBranchList();
   // SearchGoal();
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

function ClearFields() {
    $("#txtGoalName").val("");
    $("#ddlSectionName").val("0");
    $("#ddlGoalCategoryName").val("0");
    $("#ddlPerformanceName").val("0");
    $("#ddlStatus").val(""); 
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
}

function SearchGoal() {
    var txtGoalName = $("#txtGoalName");
    var ddlSectionName = $("#ddlSectionName");
    var ddlGoalCategoryName = $("#ddlGoalCategoryName");
    var ddlPerformanceName = $("#ddlPerformanceName"); 
    var ddlStatus = $("#ddlStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var requestData = {
        goalName: txtGoalName.val().trim(), sectionId: ddlSectionName.val(), goalCategoryId: ddlGoalCategoryName.val(),
        performanceCycleId: ddlPerformanceName.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), goalStatus: ddlStatus.val(),
        companyBranchId: ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../PMSGoal/GetGoalList",
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
function Export() {
    var divList = $("#divList");
    ExporttoExcel(divList);
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