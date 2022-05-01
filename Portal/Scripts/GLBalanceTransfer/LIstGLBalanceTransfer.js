$(document).ready(function () { 
    //SearchBook();
    //$('#tblCompanyList').paging({ limit: 2 });  
    BindCompanyBranchList();
});


$(".numeric-only").on("input", function () {
    var regexp = /\D/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});

function ClearFields() {
    $("#txtFromFinYearID").val("");
    $("#txtToFinYearID").val("");
    $("#txtCreatedByUserName").val("");
    $("#ddlCompanyBranch").val("0");
   
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
function SearchGLTransfer() {
    var txtFromFinYearID = $("#txtFromFinYearID");
    var txtToFinYearID = $("#txtToFinYearID");
    var txtCreatedByUserName = $("#txtCreatedByUserName");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { 
        companyBranchId: ddlCompanyBranch.val(),
        fromFinYearID: txtFromFinYearID.val().trim(),
        toFinYearID: txtToFinYearID.val().trim(),
        createdBy: txtCreatedByUserName.val().trim()
    }; 
    $.ajax({
        url: "../GLBalanceTransfer/GetGLBalanceTransferList",
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
