
$(document).ready(function () {
    BindCompanyBranchList();

    setTimeout(function () {
        if ($("#hdnSessionCompanyBranchId").val() != "0") {
            $("#ddlCompanyBranch").val($("#hdnSessionCompanyBranchId").val());
            $("#ddlCompanyBranch").attr('disabled', true);
        }
    }, 500);

    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
   

    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
         //   GenerateReportParameters();
        }
    });
});


function ClearFields() {
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
}
function GenerateBSDrillDown() {
    var txtFromDate = $("#hdnFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { fromDate: txtFromDate.val(), toDate: txtToDate.val(), companyBranchId: ddlCompanyBranch.val() };
    $.ajax({
        url: "../BalanceSheetDrillDown/BalanceSheetDrillDownGenerate",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {

                //GenerateReportParameters();
                if ($("input[name='ReportLevel']:checked").val() == "MainGroup") {
                    window.location.href = "../BalanceSheetDrillDown/MainGroupWiseBSDrillDown?ReportLevel=" + $("input[name='ReportLevel']:checked").val() + "&FromDate=" + txtFromDate.val() + "&ToDate=" + txtToDate.val() + "&companyBranchId=" + ddlCompanyBranch.val() + "&AccessMode=3";
                }
                else if ($("input[name='ReportLevel']:checked").val() == "SubGroup") {
                    window.location.href = "../BalanceSheetDrillDown/SubGroupWiseBSDrillDown?ReportLevel=" + $("input[name='ReportLevel']:checked").val() + "&FromDate=" + txtFromDate.val() + "&ToDate=" + txtToDate.val() + "&companyBranchId=" + ddlCompanyBranch.val() + "&AccessMode=3";
                }
                else if ($("input[name='ReportLevel']:checked").val() == "GLWise") {
                    window.location.href = "../BalanceSheetDrillDown/GLWiseBSDrillDown?ReportLevel=" + $("input[name='ReportLevel']:checked").val() + "&FromDate=" + txtFromDate.val() + "&ToDate=" + txtToDate.val() + "&companyBranchId=" + ddlCompanyBranch.val() + "&AccessMode=3";
                }
                else if ($("input[name='ReportLevel']:checked").val() == "SLWise") {
                    window.location.href = "../BalanceSheetDrillDown/SLWiseBSDrillDown?ReportLevel=" + $("input[name='ReportLevel']:checked").val() + "&FromDate=" + txtFromDate.val() + "&ToDate=" + txtToDate.val() + "&companyBranchId=" + ddlCompanyBranch.val() + "&AccessMode=3";
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
function ShowPrintModel() {
    $("#printModel").modal();
    
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("All Company Branch"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
          

        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("All Company Branch"));
        }
    });
}
//End Code