
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
    //BindBookTypeList();
    var url = "../BookPrint/DebitNoteBookReport?bookName=" + $("#ddlBook  option:selected").text() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() +"&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../BookPrint/DebitNoteBookReport?bookName=" + $("#ddlBook  option:selected").text() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
    $('#btnPrintExcel').attr('href', url);
});

function BindBookTypeList() {
    var companyBranchId = $("#ddlCompanyBranch").val();
    $("#ddlBook").val(0);
    $("#ddlBook").html("");
    var requestData = { bookType: "DN", companyBranchId: companyBranchId };
    $.ajax({
        type: "GET",
        url: "../Voucher/GetBookList",
        data: requestData,
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlBook").append($("<option></option>").val(0).html("-Select Book-"));
            $.each(data, function (i, item) {
                $("#ddlBook").append($("<option></option>").val(item.BookId).html(item.BookName));
            });
        },
        error: function (Result) {
            $("#ddlBook").append($("<option></option>").val(0).html("-Select Book-"));
        }
    });
}
function ClearFields() {
    $("#ddlBook").val("0");
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
    var url = "../BookPrint/DebitNoteBookReport?bookName=" + $("#ddlBook  option:selected").text() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../BookPrint/DebitNoteBookReport?bookName=" + $("#ddlBook  option:selected").text() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=Excel";
    $('#btnPrintExcel').attr('href', url);

}
function SearchCashVoucher() {
    var ddlBook = $("#ddlBook");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (ddlBook.val() == "" || ddlBook.val() == "0") {
        ShowModel("Alert", "Please select Credit Note Book")
        ddlBook.focus();
        return false;
    }

    var requestData = { bookId: ddlBook.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), companyBranchId: ddlCompanyBranch.val() };
    $.ajax({
        url: "../BookPrint/DebitNoteBookGenerate",
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
    var url = "../BookPrint/DebitNoteBookReport?bookName=" + $("#ddlBook  option:selected").text() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../BookPrint/DebitNoteBookReport?bookName=" + $("#ddlBook  option:selected").text() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
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
                $(":input#ddlCompanyBranch").trigger('change');
            }
            if (hdnSessionUserID.val() == "2") {
                $(":input#ddlCompanyBranch").trigger('change');
            }
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("All Company Branch"));
        }
    });
}
//End Code