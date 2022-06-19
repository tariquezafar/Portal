$(document).ready(function () { 
    //SearchBook();
    //$('#tblCompanyList').paging({ limit: 2 }); 
    BindCompanyBranchList();
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

function ClearFields() {
    //$("#hdnBookId").val("0");
    //$("#txtBookName").val("");
    //$("#txtBookCode").val("");
    //$("#ddlBookType").val("0");
    //$("#ddlStatus").val("");
    window.location.href = "../Book/ListBook";

}
 
 
function SearchDashboardContainer() {
    var txtDashBoardContainerName = $("#txtDashBoardContainerName");
    var txtDashBoardContainerDisplayName = $("#txtDashBoardContainerDisplayName");
    var txtContainerNo = $("#txtContainerNo");
    var txtTotalItem = $("#txtTotalItem");
    var ddlModule = $("#ddlModule");
     
    //var ddlStatus = $("#ddlStatus");
    //var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { 
        dashboardContainerName: txtDashBoardContainerName.val().trim(),
        dashboardContainerdisplayName: txtDashBoardContainerDisplayName.val().trim(),
        dashboardcontainerNo: txtContainerNo.val().trim(),
        totalItem: txtTotalItem.val().trim(),
        module: ddlModule.val().trim(),
        
       // status: ddlStatus.val(),
        //companyBranchId: ddlCompanyBranch.val(),

    }; 
    $.ajax({
        url: "../DashBoardContainer/GetDashboardContainerList",
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

//Add Company Branch In User InterFAce So Binding Process By Dheeraj
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
        }
    });
}
//End Code