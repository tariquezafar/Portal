$(document).ready(function () {
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
    $("#txtCustomerName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Quotation/GetCustomerAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.CustomerName, value: item.CustomerId, primaryAddress: item.PrimaryAddress, code: item.CustomerCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtCustomerName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtCustomerName").val(ui.item.label);
            $("#hdnCustomerId").val(ui.item.value);
            $("#txtCustomerCode").val(ui.item.code);
           // GetCustomerDetail(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtCustomerName").val("");
                $("#hdnCustomerId").val("0");
                $("#txtCustomerCode").val("");
                ShowModel("Alert", "Please select Customer from List") 
            }
            return false;
        } 
    })
 .autocomplete("instance")._renderItem = function (ul, item) {
     return $("<li>")
       .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.primaryAddress + "</div>")
       .appendTo(ul);
 };


    $("#txtCreatedBy").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Lead/GetUserAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FullName, value: item.UserName, UserId: item.UserId, MobileNo: item.MobileNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtCreatedBy").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtCreatedBy").val(ui.item.label);
            $("#hdnCreatedId").val(ui.item.UserId); 
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtCreatedBy").val("");
                $("#hdnCreatedId").val("");
                ShowModel("Alert", "Please select User from List")
            }
            return false;
        }

    })
    .autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
          .append("<div><b>" + item.label + " || " + item.value + "</b><br>" + item.MobileNo + "</div>")
          .appendTo(ul);
    };
     

    //BindStateList(); 
    // SearchCustomerRegister();
    BindCompanyBranchList();
    GenerateReportParameters();

});
 
function BindStateList() { 
    $("#ddlState,#ddlShippingState").val(0);
    $("#ddlState,#ddlShippingState").html("");
        var data = { countryId: 1 };
        $.ajax({
            type: "GET",
            url: "../Company/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlState,#ddlShippingState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlState,#ddlShippingState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                
            },
            error: function (Result) {
                $("#ddlState,#ddlShippingState").append($("<option></option>").val(0).html("-Select State-"));
            }
        }); 
} 

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
    $("#ddlFormStatus").val("");
    $("#txtCustomerName").val("");
    $("#hdnCustomerId").val("0");
    $("#ddlSortBy").val("C.CustomerName");
    $("#ddlSortOrder").val("ASC");
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
    $("#txtInvoiceNo").val("");
    $("#hdnCreatedId").val("0");
    
}
function SearchCustomerRegister() {
    var ddlFormStatus = $("#ddlFormStatus");
    var txtCustomerName = $("#txtCustomerName");
    var hdnCustomerId = $("#hdnCustomerId"); 
    var txtInvoiceNo = $("#txtInvoiceNo");
    var txtRefNo = $("#txtRefNo");
    var hdnCreatedId = $("#hdnCreatedId");
    var ddlSortBy = $("#ddlSortBy");
    var ddlSortOrder = $("#ddlSortOrder");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    //var ddlCompanyBranch = $("#ddlCompanyBranch");
    var requestData = { formStatus: ddlFormStatus.val() == 0 ? '' : ddlFormStatus.val(), customerId: hdnCustomerId.val(), invoiceNo: txtInvoiceNo.val(), refNo: txtRefNo.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), createdBy: hdnCreatedId.val(), sortBy: ddlSortBy.val(), SortOrder: ddlSortOrder.val() };
    $.ajax({
        url: "../CustomerFormRegister/GetCustomerFormRegisterList",
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
function Export()
{
    var divList = $("#divList");
    ExporttoExcel(divList);

}

function OpenPrintPopup() {
    $("#printModel").modal();
    GenerateReportParameters();
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
function GenerateReportParameters() {

    var url = "../CustomerFormRegister/GenerateCustomerFormRegisterReports?formStatus=" + $("#ddlFormStatus").val() + "&customerId=" + $("#hdnCustomerId").val() + "&invoiceNo=" + $("#txtInvoiceNo").val() + "&refNo=" + $("#txtRefNo").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&createdBy=" + $("#hdnCreatedId").val() + "&sortBy=" + $("#ddlSortBy").val() + "&sortOrder=" + $("#ddlSortOrder").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    url = "../CustomerFormRegister/GenerateCustomerFormRegisterReports?formStatus=" + $("#ddlFormStatus").val() + "&customerId=" + $("#hdnCustomerId").val() + "&invoiceNo=" + $("#txtInvoiceNo").val() + "&refNo=" + $("#txtRefNo").val() + "&fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&createdBy=" + $("#hdnCreatedId").val() + "&sortBy=" + $("#ddlSortBy").val() + "&sortOrder=" + $("#ddlSortOrder").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

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