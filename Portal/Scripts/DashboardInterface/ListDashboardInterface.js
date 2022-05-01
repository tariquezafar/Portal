
$(document).ready(function () { 
 
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
            //var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            //var hdnSessionUserID = $("#hdnSessionUserID");
            //if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
            //    $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
            //    $("#ddlCompanyBranch").attr('disabled', true);
            //}
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}




 
 
function ClearFields()
{

    
    window.location.href = "../DashboardInterface/ListDashboardInterface";
 
    
}
function SearchDashboardInterface() {
    var txtItemName = $("#txtItemName");
    var txtContainerName = $("#txtContainerName");
    var ddlModuleName = $("#ddlModuleName");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlStatus = $("#ddlStatus");

    var requestData = { 
        itemName: txtItemName.val().trim(),
        containerName: txtContainerName.val().trim(),
        moduleName: ddlModuleName.val(),
        status: ddlStatus.val(),
        companyBranchId: ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../DashboardInterface/GetDashboardInterfaceList",
        data: requestData,
        dataType: "html",
        asnc: true,
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
