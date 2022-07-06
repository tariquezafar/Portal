
$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtSearchFromDate,#txtSearchToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
});

function ClearFields() {
    window.location.href = "../DispatchPlan/ListDispatchPlan";

}

function SearchDispatchPlan() {
    var txtDispatchPlanNo = $("#txtDispatchPlanNo");
    var txtCustomerName = $("#txtCustomerName");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtSearchFromDate = $("#txtSearchFromDate");
    var txtSearchToDate = $("#txtSearchToDate");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
   
    var requestData = {
        dispatchPlanNo: txtDispatchPlanNo.val().trim(),
        customerName: txtCustomerName.val().trim(),
        companyBranchId: ddlCompanyBranch.val(),
        fromDate: txtSearchFromDate.val(),
        toDate: txtSearchToDate.val(),
        approvalStatus: ddlApprovalStatus.val()
    };
    $.ajax({
        url: "../DispatchPlan/GetDispatchPlanList",
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


    function SearchApproveDispatchPlan() {
        var txtDispatchPlanNo = $("#txtDispatchPlanNo");
        var txtCustomerName = $("#txtCustomerName");
        var ddlCompanyBranch = $("#ddlCompanyBranch");
        var txtSearchFromDate = $("#txtSearchFromDate");
        var txtSearchToDate = $("#txtSearchToDate");
        var ddlApprovalStatus = $("#ddlApprovalStatus");

        var requestData = {
            dispatchPlanNo: txtDispatchPlanNo.val().trim(),
            customerName: txtCustomerName.val().trim(),
            companyBranchId: ddlCompanyBranch.val(),
            fromDate: txtSearchFromDate.val(),
            toDate: txtSearchToDate.val(),
            approvalStatus: ddlApprovalStatus.val()
        };
        $.ajax({
            url: "../DispatchPlan/GetApproveDispatchPlanList",
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

