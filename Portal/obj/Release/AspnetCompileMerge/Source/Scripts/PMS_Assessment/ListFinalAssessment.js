
$(document).ready(function () {
    //SearchEmployeeAppraisalTemplateMapping();
    BindCompanyBranchList();
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

function ClearFields() {
    $("#txtTemplateName").val("");
    $("#txtEmployeeName").val("");

 
    
}
function SearchEmployeeAppraisalTemplateMapping() {
    var txtTemplateName = $("#txtTemplateName");
    var txtEmployeeName = $("#txtEmployeeName");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var id = $("#hdnSessionUserID");

    //if (id.val() == "2")
    //{
    //    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
    //        ShowModel("Alert", "Please select Company Branch")
    //        return false;
    //    }

    //}

    var requestData = {
        templateName: txtTemplateName.val().trim(),
        employeeName: txtEmployeeName.val().trim(),
        employeeMapping_Status: "1",
        companyBranchId: ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../PMS_Assessment/GetFinalAssessmentList",
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
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
