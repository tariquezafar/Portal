
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

        }
    });
    //BindBookTypeList();
    GenerateReportParameters();
    

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
function BindBookTypeList() {
    var companyBranchId = $("#ddlCompanyBranch").val();
    $("#ddlBook").val(0);
    $("#ddlBook").html("");
    var requestData = { bookType: "B", companyBranchId: 0 };
    $.ajax({
        type: "GET",
        url: "../Voucher/GetBookList",
        data: requestData,
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlBook").append($("<option></option>").val(0).html("-Select Book-"));
            $.each(data, function (i, item) {              
                $("#ddlBook").append($("<option></option>").val(item.BookId).html(item.BookName + ": " + item.BankAccountNo + ":" + item.BankBranch));
            });
        },
        error: function (Result) {
            $("#ddlBook").append($("<option></option>").val(0).html("-Select Book-"));
        }
    });
}
function ClearFields() {
    //$("#ddlBook").val("0");
    //$("#ddlVoucherMode").val("");
    //$("#txtVoucherNo").val("");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    //$("#ddlVoucherStatus").val("0");
    //$("#ddlCompanyBranch").val("0");
    window.location.href = "../Voucher/ListBankVoucher";


    
}


function checkdropDown() {
    var ddlBook = $("#ddlBook");
    var ddlVoucherMode = $("#ddlVoucherMode"); 
    var ddlVoucherStatus = $("#ddlVoucherStatus");
    if (ddlBook.val()!= "" && ddlBook.val() != "0") {
        $("#hdnDdlBook").val(ddlBook.val());
        var bookId = '@ViewData["bookId"]';
        bookId = ddlBook.val();
       
    }   
    if (ddlVoucherMode.val() != "" && ddlVoucherMode.val() != "0") {
        $("#hdnddlVouchermode").val(ddlVoucherMode.val());
    }

    if (ddlVoucherStatus.val() != "" && ddlVoucherStatus.val() != "0") {
        $("#hdnddlVoucherStatus").val(ddlVoucherStatus.val());
    }
    

}

function SearchBankVoucher() {
    var ddlBook = $("#ddlBook");
    var ddlVoucherMode = $("#ddlVoucherMode");
    var txtVoucherNo = $("#txtVoucherNo");
    var ddlVoucherStatus = $("#ddlVoucherStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (ddlBook.val() == "" || ddlBook.val() == "0") {
        alert("Please select Bank Book")
        ddlBook.focus();
        return false;
    }

    var requestData = {
        bookId: ddlBook.val(), voucherMode: ddlVoucherMode.val(),
        voucherNo: txtVoucherNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(),
        voucherStatus: ddlVoucherStatus.val(), companyBranchId: ddlCompanyBranch.val(),
    };
    $.ajax({
        url: "../Voucher/GetBankVoucherList",
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

function OpenPrintPopup() {
    var ddlBook = $("#ddlBook");
    if (ddlBook.val() == "" || ddlBook.val() == "0") {
        alert("Please Select Bank Note Book")
        return false;
    }
    else {
        $("#printModel").modal();
        GenerateReportParameters();
    }
}
function ShowHidePrintOption() {
    var reportOption = $("#ddlPrintOption").val();
    if (reportOption == "PDF") {
        $("#btnPdf").show();
        $("#btnExcel").hide();
    }
    else if (reportOption == "Excel") {
        $("#btnExcel").show();
        $("#btnPdf").hide();
    }   
}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}

function GenerateReportParameters() {
   
    var url = "../Voucher/GenerateBankVoucherReport?bookId=" + $("#ddlBook").val() + "&voucherMode=" + $("#ddlVoucherMode").val() + "&voucherNo=" + $("#txtVoucherNo").val() + "&voucherStatus=" + $("#ddlVoucherStatus").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    url = "../Voucher/GenerateBankVoucherReport?bookId=" + $("#ddlBook").val() + "&voucherMode=" + $("#ddlVoucherMode").val() + "&voucherNo=" + $("#txtVoucherNo").val() + "&voucherStatus=" + $("#ddlVoucherStatus").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}
//End Code