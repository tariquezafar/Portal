
$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtAsOnDate").attr('readOnly', true);
    
    $("#txtAsOnDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            GenerateReportParameters();
        }
    });
    
    
  


    var url = "../GLTrialBalancePrint/GLTrialBalance2ColumnReport?asOnDate=" + $("#txtAsOnDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../GLTrialBalancePrint/GLTrialBalance2ColumnReport?asOnDate=" + $("#txtAsOnDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
    $('#btnPrintExcel').attr('href', url);
});


function ClearFields() {
    $("#txtAsOnDate").val($("#hdnFromDate").val());
    var url = "../GLTrialBalancePrint/GLTrialBalance2ColumnReport?asOnDate=" + $("#txtAsOnDate").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../GLTrialBalancePrint/GLTrialBalance2ColumnReport?asOnDate=" + $("#txtAsOnDate").val() + "&reportType=Excel";
    $('#btnPrintExcel').attr('href', url);

}

function SearchBankVoucher() {
    
    var txtFromDate = $("#txtAsOnDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    
    
    var requestData = { asOnDate: txtFromDate.val(), companyBranchId: ddlCompanyBranch.val() };
    $.ajax({
        url: "../GLTrialBalancePrint/GLTrialBalance2ColumnGenerate",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                GenerateReportParameters();
                ShowPrintModel();
                
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
function GenerateReportParameters() {
    
    var url = "../GLTrialBalancePrint/GLTrialBalance2ColumnReport?asOnDate=" + $("#txtAsOnDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
        $('#btnPrintPDF').attr('href', url);
        url = "../GLTrialBalancePrint/GLTrialBalance2ColumnReport?asOnDate=" + $("#txtAsOnDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
        $('#btnPrintExcel').attr('href', url);
    
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("All Company Branch"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("All Company Branch"));
        }
    });
}
//End Code