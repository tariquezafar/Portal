
$(document).ready(function () {
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);

 BindCompanyBranchList();

    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    var hdnTotalSaleTargetList = $("#hdnTotalSaleTargetList").val();
    if (hdnTotalSaleTargetList == "true") {
        setTimeout(
        function () {
            SearchSaleTarget();
         }, 1000);
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}
function ClearFields() {
    //$("#txtWorkOrderNo").val("");
    //$("#ddlCompanyBranch").val("0");
    //$("#ddlApprovalStatus").val("0");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    window.location.href = "../SaleTarget/ListSaleTarget";
    
}
function SearchSaleTarget() {
    var txtTargetNo = $("#txtTargetNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");

    var requestData = { targetNo: txtTargetNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: ddlApprovalStatus.val() };
    $.ajax({
        url: "../SaleTarget/GetSaleTargetList",
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
function OpenWorkOrderSearchPopup() {
    $("#SearchWordOrderModel").modal();

}



function SearchCostWorkOrder() {
    var txtWorkOrderNo = $("#txtSearchWorkOrderNo");
    var ddlCompanyBranch = $("#ddlSearchCompanyBranch");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var requestData = { workOrderNo: txtWorkOrderNo.val(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../WorkOrder/GetWorkOrderCostList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divSearchList").html("");
            $("#divSearchList").html(err);
        },
        success: function (data) {
            $("#divSearchList").html("");
            $("#divSearchList").html(data);
        }
    });
}

function SelectWorkOrder(workOrderId, workOrderNo, workOrderDate, quantity, companyBranchId) {
    $("#txtWorkOrderNo").val(workOrderNo);
    $("#hdnWorkOrderId").val(workOrderId);
    $("#txtWorkOrderDate").val(workOrderDate);
    $("#ddlCompanyBranch").val(companyBranchId);
    $("#SearchWordOrderModel").modal('hide');
}

function OpenPrintPopup() {
    $("#printModel").modal();
    GenerateReportParameters();

}
function ShowHidePrintOption() {
    var reportOption = $("#ddlPrintOption2").val();
    if (reportOption == "PDF") {
        $("#btnPdf").show();
        $("#btnExcel").hide();
    }
    else if (reportOption == "Excel") {
        $("#btnExcel").show();
        $("#btnPdf").hide();
    }
}
function GenerateReportParameters() {
    var url = "../WorkOrder/WoCostReport?workOrderId=" + $("#hdnWorkOrderId").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    url = "../WorkOrder/WoCostReport?workOrderId=" + $("#hdnWorkOrderId").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);


}