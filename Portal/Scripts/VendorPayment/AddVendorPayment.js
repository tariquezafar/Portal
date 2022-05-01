$(document).ready(function () { 
    $("#txtPaymentNo").attr('readOnly', true);
    $("#txtPaymentDate").attr('readOnly', true); 
    $("#txtRefDate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtInvoiceNo").attr('readOnly', true);
    $("#txtInvoiceDate").attr('readOnly', true);
    $("#txtValueDate").attr('readOnly', true);
    $("#txtVendorCode").attr('readOnly', true); 

    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);
     
    $("#txtVendorName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../VendorPayment/GetVendorAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.VendorName, value: item.VendorId, Address: item.Address,   Code: item.VendorCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtVendorName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtVendorName").val(ui.item.label);
            $("#hdnVendorId").val(ui.item.value);
            $("#txtVendorCode").val(ui.item.Code);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtVendorName").val("");
                $("#hdnVendorId").val("0");
                $("#txtVendorCode").val("");
                ShowModel("Alert", "Please select Vendor from List") 
            }
            return false;
        }

    })
  .autocomplete("instance")._renderItem = function (ul, item) {
      return $("<li>")
        .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.primaryAddress + "</div>")
        .appendTo(ul);
  }; 



 




    $("#txtPaymentDate,#txtRefDate,#txtValueDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) { 
        }
    });

    $("#txtSearchFromDate,#txtSearchToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) { 
        }
    });

   
    BindBookTypeList();
    BindPaymentModeList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPaymentTrnId = $("#hdnPaymentTrnId");
    if (hdnPaymentTrnId.val() != "" && hdnPaymentTrnId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetVendorPaymentDetail(hdnPaymentTrnId.val());
       }, 2000);
         
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }

  

    //$("#txtCustomerName").focus();

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
function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}

function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function OpenInvoiceSearchPopup() {
    $("#SearchInvoiceModel").modal();

}
function SearchInvoice() {
    var txtInvoiceNo = $("#txtSearchInvoiceNo");
    var txtVendorName = $("#txtSearchVendorName"); 
    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { piNo: txtInvoiceNo.val().trim(), vendorName: txtVendorName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../VendorPayment/GetPIList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divInvoiceList").html("");
            $("#divInvoiceList").html(err);
        },
        success: function (data) {
            $("#divInvoiceList").html("");
            $("#divInvoiceList").html(data);
        }
    });
}
function SelectInvoice(invoiceId, piNo, invoiceDate, vendorId, vendorCode, vendorName) {
    $("#txtInvoiceNo").val(piNo);
    $("#hdnInvoiceId").val(invoiceId);
    $("#txtInvoiceDate").val(invoiceDate);
    $("#hdnVendorId").val(vendorId);
    $("#txtVendorCode").val(vendorCode);
    $("#txtVendorName").val(vendorName);
    $("#SearchInvoiceModel").modal('hide'); 
   
}

function BindBookTypeList() {
    $.ajax({
        type: "GET",
        url: "../VendorPayment/GetBookList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPaymentType").append($("<option></option>").val(0).html("-Select Book Type-"));
            $.each(data, function (i, item) {

                $("#ddlPaymentType").append($("<option></option>").val(item.BookId).html(item.BookName + ": " + item.BankAccountNo + ":" + item.BankBranch));
            });
        },
        error: function (Result) {
            $("#ddlPaymentType").append($("<option></option>").val(0).html("-Select Book Type-"));
        }
    });
} 



function BindPaymentModeList() {
    $.ajax({
        type: "GET",
        url: "../VendorPayment/GetPaymentModeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPaymentModeName").append($("<option></option>").val(0).html("-Select Payment Mode-"));
            $.each(data, function (i, item) {

                $("#ddlPaymentModeName").append($("<option></option>").val(item.PaymentModeId).html(item.PaymentModeName));
            });
        },
        error: function (Result) {
            $("#ddlPaymentModeName").append($("<option></option>").val(0).html("-Select Payment Mode-"));
        }
    });
}
$(function () { 
    $('#ddlPaymentModeName').change(function () {
        var x = $('#ddlPaymentModeName option:selected').text(); 
        if (x == 'Cash') {
            $('#refhide').hide();
            $('#refdatehide').hide();
            $('#valudatehide').hide(); 
            $("#txtRefNo").val("");
            $("#txtRefDate").val("");
            $("#txtValueDate").val("");
        } else {
            $('#refhide').show();
            $('#refdatehide').show();
            $('#valudatehide').show();
        }
    });
});
 
function GetVendorPaymentDetail(paymenttrnId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../VendorPayment/GetVendorPaymentDetail",
        data: { paymenttrnId: paymenttrnId },
        dataType: "json",
        success: function (data) { 
            $("#txtPaymentNo").val(data.PaymentNo);
            $("#txtPaymentDate").val(data.PaymentDate);
            $("#txtInvoiceNo").val(data.InvoiceNo);
            $("#hdnInvoiceId").val(data.InvoiceId);
            $("#txtInvoiceDate").val(data.InvoiceDate);
            $("#hdnVendorId").val(data.VendorId);
            $("#txtVendorCode").val(data.VendorCode);
            $("#txtVendorName").val(data.VendorName);
            $("#ddlPaymentType").val(data.BookId);
            $("#ddlPaymentModeName").val(data.PaymentModeId);
            $("#txtRefNo").val(data.RefNo);
            $("#txtRefDate").val(data.RefDate);
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            } 
            $("#txtAmount").val(data.Amount);
            $("#txtValueDate").val(data.ValueDate);
            $("#txtRemarks").val(data.Remarks); 
            $("#btnAddNew").show();
            $("#btnPrint").show();
            if (data.VendorPayment_Status == true) {
                $("#chkstatus").attr("checked", true);
            }
            else {
                $("#chkstatus").attr("checked", false);
            }


        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
$(function () {
    $("#dialogForQuotationNo").dialog({
        autoOpen: false,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
    });

    $("#opener").on("click", function () {
        $("#dialogForQuotationNo").dialog("open");
    });
});
function SaveData() {
    var txtPaymentNo = $("#txtPaymentNo");
    var hdnPaymentTrnId = $("#hdnPaymentTrnId");
    var txtPaymentDate = $("#txtPaymentDate");
    var txtInvoiceNo = $("#txtInvoiceNo");
    var hdnInvoiceId = $("#hdnInvoiceId"); 
    var hdnVendorId = $("#hdnVendorId");
    var txtVendorName = $("#txtVendorName");
    var txtRefNo = $("#txtRefNo");
    var txtRefDate = $("#txtRefDate");
    var ddlPaymentType = $("#ddlPaymentType");
    var ddlPaymentModeName = $("#ddlPaymentModeName");
    var txtAmount = $("#txtAmount");
    var txtValueDate = $("#txtValueDate");
    var txtRemarks = $("#txtRemarks");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtVendorName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Party Name")
        txtVendorName.focus();
        return false;
    }
    if (hdnVendorId.val() == "" || hdnVendorId.val() == "0") {
        ShowModel("Alert", "Please select Party from list")
        txtVendorName.focus();
        return false;
    }
   
    if (txtInvoiceNo.val().trim() == "") {
        ShowModel("Alert", "Please select Invoice No from list")
        txtInvoiceNo.focus();
        return false;
    }

    if (ddlPaymentModeName.val() == "" || ddlPaymentModeName.val() == "0") {
        ShowModel("Alert", "Please select Payment Mode")
        ddlPaymentModeName.focus();
        return false;
    }

    var vendorpaymentViewModel = {
        PaymentTrnId: hdnPaymentTrnId.val(),
        PaymentNo: txtPaymentNo.val().trim(),
        PaymentDate: txtPaymentDate.val().trim(),
        InvoiceId: hdnInvoiceId.val().trim(),
        InvoiceNo: txtInvoiceNo.val().trim(),
        VendorId: hdnVendorId.val().trim(),
        BookId: ddlPaymentType.val(),
        VendorName: txtVendorName.val().trim(), 
        RefNo: txtRefNo.val().trim(),
        RefDate: txtRefDate.val(),
        PaymentModeId: ddlPaymentModeName.val(),
        Amount: txtAmount.val(),
        ValueDate: txtValueDate.val(),
        Remarks: txtRemarks.val(),
        VendorPayment_Status: chkstatus
    };
     

    var accessMode = 1;//Add Mode
    if (hdnPaymentTrnId.val() != null && hdnPaymentTrnId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { vendorpaymentViewModel: vendorpaymentViewModel };
    $.ajax({
        url: "../VendorPayment/AddEditVendorPayment?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
                   function () {
                       window.location.href = "../VendorPayment/AddEditVendorPayment?PaymentTrnId=" + data.trnId + "&AccessMode=3";
                   }, 2000);

                $("#btnSave").show();
                $("#btnUpdate").hide();
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
function ClearFields() { 
    $("#txtPaymentNo").val("");
    $("#hdnPaymentTrnId").val("0");
    $("#txtPaymentDate").val($("#hdnCurrentDate").val());
    $("#txtValueDate").val("");
    $("#hdnInvoiceId").val("0");
    $("#txtVendorName").val("");
    $("#txtInvoiceNo").val("");
    $("#txtVendorCode").val("");  
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
    $("#txtRefNo").val("");
    $("#txtRefDate").val("");
    $("#ddlPaymentType").val("0");
    $("#ddlPaymentModeName").val("0");
    $("#txtAmount").val("");
    $("#txtRemarks").val("");
    $('#chkstatus').attr('checked', false);
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
 
 
 