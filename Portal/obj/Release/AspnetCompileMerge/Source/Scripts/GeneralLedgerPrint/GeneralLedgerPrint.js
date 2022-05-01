
$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            GenerateReportParameters();
        }
    });
    $("#txtGLHead").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../GeneralLedgerPrint/GetGLAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.GLHead, value: item.GLId, code: item.GLCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtGLHead").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtGLHead").val(ui.item.label);
            $("#hdnGLId").val(ui.item.value);
            GenerateReportParameters();
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtGLHead").val("");
                $("#hdnGLId").val("0");
                ShowModel("Alert", "Please select GL from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b></div>")
      .appendTo(ul);
};



    var url = "../GeneralLedgerPrint/GeneralLedgerWoFyReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../GeneralLedgerPrint/GeneralLedgerWoFyReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
    $('#btnPrintExcel').attr('href', url);
});


function ClearFields() {
    $("#txtGLHead").val("ALL");
    $("#hdnGLId").val("0");
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
    var url = "../GeneralLedgerPrint/GeneralLedgerWoFyReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../GeneralLedgerPrint/GeneralLedgerWoFyReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=Excel";
    $('#btnPrintExcel').attr('href', url);

}
function SearchBankVoucher() {
    var hdnGLId = $("#hdnGLId");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (hdnGLId.val() == undefined || hdnGLId.val() == "") {
        ShowModel("Alert", "Please select correct GL")
        return false;
    }

    var requestData = { glId: hdnGLId.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(),companyBranchId: ddlCompanyBranch.val() };
    $.ajax({
        url: "../GeneralLedgerPrint/GeneralLedgerGenerate",
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
    var url = "../GeneralLedgerPrint/GeneralLedgerWoFyReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../GeneralLedgerPrint/GeneralLedgerWoFyReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
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