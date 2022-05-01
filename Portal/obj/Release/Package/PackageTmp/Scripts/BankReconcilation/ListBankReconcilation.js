
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
    //setTimeout(
    // function () {
    //     SearchBankRecoStatement();
    // }, 1000);
   
    $("#txtBankBookName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SO/GetBookAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, bookType: "B" },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.BookName, value: item.BookId, branch: item.BankBranch, ifsc: item.IFSC };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtBankBookName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtBankBookName").val(ui.item.label);
            $("#hdnBankBookId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtBankBookName").val("");
                $("#hdnBankBookId").val(0);
                ShowModel("Alert", "Please select Bank from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.branch + "</b><br>" + item.ifsc + "</div>")
      .appendTo(ul);
};
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


function ClearFields() {
    //$("#txtBankReconcilationNo").val("");
    //$("#ddlBankReconcilationStatus").val("0");
    //$("#txtBankStatementNo").val("");
    //$("#txtBankBookName").val("");
    //$("#hdnBankBookId").val("0");
    //$("#ddlCompanyBranch").val("0");
    //$("#ddlBankStatementStatus").val("0");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    window.location.href = "../BankReconcilation/ListBankReconcilation";


}
function SearchBankRecoStatement() {
    var txtBankReconcilationNo = $("#txtBankReconcilationNo");
    var txtBankBookName = $("#txtBankBookName");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var hdnBankBookId = $("#hdnBankBookId");
    var ddlBankReconcilationStatus = $("#ddlBankReconcilationStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");

    var requestData = { bankRecoNo: txtBankReconcilationNo.val(), bankBookId: hdnBankBookId.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), bankRecoStatus: ddlBankReconcilationStatus.val() };
    $.ajax({
        url: "../BankReconcilation/GetBankReconcilationList",
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
