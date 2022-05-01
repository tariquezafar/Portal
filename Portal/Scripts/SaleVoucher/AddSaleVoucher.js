$(document).ready(function () {
    $(".sltype").hide();
    $("#txtVoucherNo").attr('readOnly', true);
    $("#txtVoucherDate").attr('readOnly', true);

    $("#txtBookBalance").attr('readOnly', true);

    $("#txtTotalDebit").attr('readOnly', true);
    $("#txtTotalCredit").attr('readOnly', true);


    $("#txtGLCode").attr('readOnly', true);
    $("#txtGLBalance").attr('readOnly', true);

    $("#txtSLCode").attr('readOnly', true);
    $("#txtSLBalance").attr('readOnly', true);

    $("#txtChequeRefDate").attr('readOnly', true);
    $("#txtValueDate").attr('readOnly', true);

   
    $("#txtBillDate").attr('readOnly', true);

    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);

    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);

    $("#txtCancelReason").attr('readOnly', true);

    $("#txtCancelledDate").attr('readOnly', true);
    $("#txtPISearchFromDate").attr('readOnly', true);
    $("#txtPISearchToDate").attr('readOnly', true);
    $("#txtSISearchFromDate").attr('readOnly', true);
    $("#txtSISearchToDate").attr('readOnly', true);

    $("#txtVoucherDate,#txtChequeRefDate,#txtValueDate,#txtBillDate,#txtPISearchFromDate,#txtPISearchToDate,#txtSISearchFromDate,#txtSISearchToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0d',
        onSelect: function (selected) {
        }
    });

    $("#txtVoucherAmount").attr('readOnly', true);
    $("#ddlPayeeSLType").attr('readOnly', true);




    BindBookTypeList();
    BindSLTypeList();
    BindPaymentModeList();
    BindCostCenterList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnVoucherId = $("#hdnVoucherId");
    if (hdnVoucherId.val() != "" && hdnVoucherId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetJournalVoucherDetail(hdnVoucherId.val());
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

    var voucherEntryList = [];
    GetBankVoucherEntryList(voucherEntryList);



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

function GetGLBalance(glId) {
    $("#txtGLBalance").val(0);
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Voucher/GetGLBalance",
        data: { glId: glId },
        dataType: "json",
        success: function (data) {
            $("#txtGLBalance").val(data);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
            $("#txtGLBalance").val(0);
        }
    });
}
function GetSLBalance(slId) {
    var glId = $("#hdnGLId").val();
    $("#txtSLBalance").val(0);
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Voucher/GetSLBalance",
        data: { glId: glId, slId: slId },
        dataType: "json",
        success: function (data) {
            $("#txtSLBalance").val(data);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
            $("#txtSLBalance").val(0);
        }
    });
}
function OpenInvoiceSearchPopup() {
    if ($("#hdnSLTypeId").val() == "1") {
        $("#SearchPurchaseInvoiceModel").modal();
    }
    else if ($("#hdnSLTypeId").val() == "2") {
        $("#SearchSaleInvoiceModel").modal();
    }
    else {
        ShowModel("Alert", "Bill Not exist!!!")
    }
}
function SearchPI() {
    var txtInvoiceNo = $("#txtPISearchInvoiceNo");
    var txtRefNo = $("#txtPISearchRefNo");
    var txtFromDate = $("#txtPISearchFromDate");
    var txtToDate = $("#txtPISearchToDate");
    var vendorCode = $("#txtSLCode").val();
    var requestData = { piNo: txtInvoiceNo.val().trim(), vendorName: "", refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final", vendorCode: vendorCode };
    $.ajax({
        url: "../Voucher/GetPIList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divPIList").html("");
            $("#divPIList").html(err);
        },
        success: function (data) {
            $("#divPIList").html("");
            $("#divPIList").html(data);
        }
    });
}
function SearchSI() {
    var txtInvoiceNo = $("#txtSISearchInvoiceNo");
    var txtRefNo = $("#txtSISearchRefNo");
    var txtFromDate = $("#txtSISearchFromDate");
    var txtToDate = $("#txtSISearchToDate");
    var customerCode = $("#txtSLCode").val();
    var requestData = { saleinvoiceNo: txtInvoiceNo.val().trim(), customerName: "", refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final", customerCode: customerCode };
    $.ajax({
        url: "../Voucher/GetSaleInvoiceList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divSIList").html("");
            $("#divSIList").html(err);
        },
        success: function (data) {
            $("#divSIList").html("");
            $("#divSIList").html(data);
        }
    });
}
function SelectPurchaseInvoice(piNo, invoiceDate, poNo) {
    $("#txtBillNo").val(piNo);
    $("#txtBillDate").val(invoiceDate);
    $("#txtPO_SONo").val(poNo);
    $("#SearchPurchaseInvoiceModel").modal('hide');
}
function SelectSaleInvoice(siNo, invoiceDate, soNo) {
    $("#txtBillNo").val(siNo);
    $("#txtBillDate").val(invoiceDate);
    $("#txtPO_SONo").val(soNo);
    $("#SearchSaleInvoiceModel").modal('hide');
}
function BindBookTypeList() {
    var requestData = { bookType: "S" };
    $.ajax({
        type: "GET",
        url: "../JournalVoucher/GetBookList",
        data: requestData,
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlBook").append($("<option></option>").val(0).html("-Select Sale Voucher-"));
            $.each(data, function (i, item) {
                $("#ddlBook").append($("<option></option>").val(item.BookId).html(item.BookName));
            });
        },
        error: function (Result) {
            $("#ddlBook").append($("<option></option>").val(0).html("-Select Journal Voucher-"));
        }
    });
}
function BindSLTypeList() {
    $.ajax({
        type: "GET",
        url: "../Voucher/GetSLTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $.each(data, function (i, item) {
                $("#ddlPayeeSLType").append($("<option></option>").val(item.SLTypeId).html(item.SLTypeName));
            });
        },
        error: function (Result) {
            $("#ddlPayeeSLType").append($("<option></option>").val(0).html("NA"));
        }
    });
}
function BindPaymentModeList() {
    var requestData = {};
    $.ajax({
        type: "GET",
        url: "../Voucher/GetPaymentModeList",
        data: requestData,
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPaymentMode").append($("<option></option>").val(0).html("-Payment Mode-"));
            $.each(data, function (i, item) {
                $("#ddlPaymentMode").append($("<option></option>").val(item.PaymentModeId).html(item.PaymentModeName));
            });
        },
        error: function (Result) {
            $("#ddlPaymentMode").append($("<option></option>").val(0).html("-Payment Mode-"));
        }
    });
}
function BindCostCenterList() {
    var requestData = {};
    $.ajax({
        type: "GET",
        url: "../Voucher/GetCostCenterList",
        data: requestData,
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCostCenter").append($("<option></option>").val(0).html("-Cost Center-"));
            $.each(data, function (i, item) {
                $("#ddlCostCenter").append($("<option></option>").val(item.CostCenterId).html(item.CostCenterName));
            });
        },
        error: function (Result) {
            $("#ddlCostCenter").append($("<option></option>").val(0).html("-Cost Center-"));
        }
    });
}

function AddVoucherEntry(action) {
    var voucherEntrySequence = 0;
    var hdnVoucherDetailId = $("#hdnVoucherDetailId");
    var ddlEntryMode = $("#ddlEntryMode");
    var hdnSequenceNo = $("#hdnSequenceNo");

    var txtPayee = $("#txtPayee");
    var hdnPayeeId = $("#hdnPayeeId");

    var txtGLHead = $("#txtGLHead");
    var hdnGLId = $("#hdnGLId");
    var hdnSLTypeId = $("#hdnSLTypeId");
    var txtGLCode = $("#txtGLCode");

    var txtSLHead = $("#txtSLHead");
    var hdnSLId = $("#hdnSLId");
    var txtSLCode = $("#txtSLCode");

    var txtNarration = $("#txtNarration");
    var ddlPaymentMode = $("#ddlPaymentMode");
    var txtAmount = $("#txtAmount");
    var txtChequeRefNo = $("#txtChequeRefNo");
    var txtChequeRefDate = $("#txtChequeRefDate");
    var txtValueDate = $("#txtValueDate");

    var txtDrawnOnBank = $("#txtDrawnOnBank");
    var ddlCostCenter = $("#ddlCostCenter");
    var txtPO_SONo = $("#txtPO_SONo");

    var txtBillNo = $("#txtBillNo");
    var txtBillDate = $("#txtBillDate");



    if (txtGLHead.val().trim() == "") {
        ShowModel("Alert", "Please Enter General Ledger")
        txtGLHead.focus(); 8
        return false;
    }
    if (hdnGLId.val().trim() == "" || hdnGLId.val().trim() == "0") {
        ShowModel("Alert", "Please select General Ledger from list")
        hdnGLId.focus();
        return false;
    }

    if (parseInt(hdnSLTypeId.val()) > 0) {
        if (txtSLHead.val().trim() == "") {
            ShowModel("Alert", "Please Enter Sub Ledger")
            txtSLHead.focus();
            return false;
        }
        if (hdnSLId.val().trim() == "" || hdnSLId.val().trim() == "0") {
            ShowModel("Alert", "Please select Sub Ledger from list")
            hdnSLId.focus();
            return false;
        }
    }
    if (txtNarration.val().trim() == "") {
        ShowModel("Alert", "Please Enter Narration")
        txtNarration.focus();
        return false;
    }
    if (ddlPaymentMode.val() == "" || ddlPaymentMode.val() == "0") {
        ShowModel("Alert", "Please select Payment mode")
        ddlPaymentMode.focus();
        return false;
    }
    if (txtAmount.val() == "" || txtAmount.val() == "0" || parseFloat(txtAmount.val()) <= 0) {
        ShowModel("Alert", "Please enter amount")
        txtAmount.focus();
        return false;
    }
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        voucherEntrySequence = 1;
    }
    var voucherEntryList = [];
    $('#tblVoucherEntryList tr').each(function (i, row) {
        var $row = $(row);
        var voucherDetailId = $row.find("#hdnVoucherDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var entryMode = $row.find("#hdnEntryMode").val();
        var glId = $row.find("#hdnGLId").val();
        var glCode = $row.find("#hdnGLCode").val();
        var glHead = $row.find("#hdnGLHead").val();
        var slTypeId = $row.find("#hdnSLTypeId").val();
        var slId = $row.find("#hdnSLId").val();

        var slCode = $row.find("#hdnSLCode").val();
        var slHead = $row.find("#hdnSLHead").val();
        var amount = $row.find("#hdnAmount").val();
        var narration = $row.find("#hdnNarration").val();
        var paymentModeId = $row.find("#hdnPaymentModeId").val();
        var chequeRefNo = $row.find("#hdnChequeRefNo").val();
        var chequeRefDate = $row.find("#hdnChequeRefDate").val();
        var costCenterId = $row.find("#hdnCostCenterId").val();
        var valueDate = $row.find("#hdnValueDate").val();
        var drawnOnBank = $row.find("#hdnDrawnOnBank").val();
        var po_SONo = $row.find("#hdnPO_SONo").val();
        var billNo = $row.find("#hdnBillNo").val();
        var billDate = $row.find("#hdnBillDate").val();
        var payeeId = $row.find("#hdnPayeeId").val();
        var payeeName = $row.find("#hdnPayeeName").val();
        var autoEntry = $row.find("#hdnAutoEntry").val();
        if (voucherDetailId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {
                var voucherEntry = {
                    VoucherDetailId: voucherDetailId,
                    SequenceNo: sequenceNo,
                    EntryMode: entryMode,
                    GLId: glId,
                    GLCode: glCode,
                    GLHead: glHead,
                    SLTypeId: slTypeId,
                    SLId: slId,
                    SLCode: slCode,
                    SLHead: slHead,
                    Amount: amount,
                    Narration: narration,
                    PaymentModeId: paymentModeId,
                    ChequeRefNo: chequeRefNo,
                    ChequeRefDate: chequeRefDate,
                    CostCenterId: costCenterId,
                    ValueDate: valueDate,
                    DrawnOnBank: drawnOnBank,
                    PO_SONo: po_SONo,
                    BillNo: billNo,
                    BillDate: billDate,
                    PayeeId: payeeId,
                    PayeeName: payeeName,
                    AutoEntry: autoEntry
                };
                voucherEntryList.push(voucherEntry);
                voucherEntrySequence = parseInt(voucherEntrySequence) + 1;
            }
            else if (hdnVoucherDetailId.val() == voucherDetailId && hdnSequenceNo.val() == sequenceNo) {
                var voucherEntry = {
                    VoucherDetailId: hdnVoucherDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    EntryMode: ddlEntryMode.val(),
                    GLId: hdnGLId.val(),
                    GLCode: txtGLCode.val().trim(),
                    GLHead: txtGLHead.val().trim(),
                    SLTypeId: hdnSLTypeId.val().trim(),
                    SLId: hdnSLId.val(),
                    SLCode: txtSLCode.val().trim(),
                    SLHead: txtSLHead.val().trim(),
                    Amount: txtAmount.val().trim(),
                    Narration: txtNarration.val().trim(),
                    PaymentModeId: ddlPaymentMode.val(),
                    ChequeRefNo: txtChequeRefNo.val().trim(),
                    ChequeRefDate: txtChequeRefDate.val().trim(),
                    CostCenterId: ddlCostCenter.val(),
                    ValueDate: txtValueDate.val().trim(),
                    DrawnOnBank: txtDrawnOnBank.val().trim(),
                    PO_SONo: txtPO_SONo.val().trim(),
                    BillNo: txtBillNo.val().trim(),
                    BillDate: txtBillDate.val().trim(),
                    PayeeId: hdnPayeeId.val(),
                    PayeeName: txtPayee.val().trim(),
                    AutoEntry: false
                };
                voucherEntryList.push(voucherEntry);

            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(voucherEntrySequence);
    }

    if (action == 1) {

        var voucherEntryAddEdit = {
            VoucherDetailId: hdnVoucherDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            EntryMode: ddlEntryMode.val(),
            GLId: hdnGLId.val(),
            GLCode: txtGLCode.val().trim(),
            GLHead: txtGLHead.val().trim(),
            SLTypeId: hdnSLTypeId.val().trim(),
            SLId: hdnSLId.val(),
            SLCode: txtSLCode.val().trim(),
            SLHead: txtSLHead.val().trim(),
            Amount: txtAmount.val().trim(),
            Narration: txtNarration.val().trim(),
            PaymentModeId: ddlPaymentMode.val(),
            ChequeRefNo: "",
            ChequeRefDate: "",
            CostCenterId: ddlCostCenter.val(),
            ValueDate: "",
            DrawnOnBank: "",
            PO_SONo: txtPO_SONo.val().trim(),
            BillNo: txtBillNo.val().trim(),
            BillDate: txtBillDate.val().trim(),
            PayeeId: hdnPayeeId.val(),
            PayeeName: txtPayee.val().trim(),
            AutoEntry: false
        };
        voucherEntryList.push(voucherEntryAddEdit);
    }

    GetBankVoucherEntryList(voucherEntryList);

}
function EditVoucherEntryRow(obj) {

    var row = $(obj).closest("tr");
    var voucherDetailId = $(row).find("#hdnVoucherDetailId").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var entryMode = $(row).find("#hdnEntryMode").val();
    var glId = $(row).find("#hdnGLId").val();
    var glCode = $(row).find("#hdnGLCode").val();
    var glHead = $(row).find("#hdnGLHead").val();
    GetGLBalance(glId);
    var slTypeId = $(row).find("#hdnSLTypeId").val();
    
    var slId = $(row).find("#hdnSLId").val();

    var slCode = $(row).find("#hdnSLCode").val();
    var slHead = $(row).find("#hdnSLHead").val();
    setTimeout(
       function () {
           GetSLBalance(slId);
       }, 1000);
   
    var amount = $(row).find("#hdnAmount").val();
    var narration = $(row).find("#hdnNarration").val();
    var paymentModeId = $(row).find("#hdnPaymentModeId").val();
    var chequeRefNo = $(row).find("#hdnChequeRefNo").val();
    var chequeRefDate = $(row).find("#hdnChequeRefDate").val();

    var costCenterId = $(row).find("#hdnCostCenterId").val();
    var valueDate = $(row).find("#hdnValueDate").val();
    var drawnOnBank = $(row).find("#hdnDrawnOnBank").val();
    var po_SONo = $(row).find("#hdnPO_SONo").val();
    var billNo = $(row).find("#hdnBillNo").val();
    var billDate = $(row).find("#hdnBillDate").val();
    var payeeId = $(row).find("#hdnPayeeId").val();
    var payeeName = $(row).find("#hdnPayeeName").val();
    var autoEntry = $(row).find("#hdnAutoEntry").val();


    $("#hdnVoucherDetailId").val(voucherDetailId);
    $("#ddlEntryMode").val(entryMode);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#txtPayee").val(payeeName);
    $("#hdnPayeeId").val(payeeId);
    $("#txtGLHead").val(glHead);
    $("#hdnGLId").val(glId);
    $("#hdnSLTypeId").val(slTypeId);

    $("#txtGLCode").val(glCode);
    $("#txtSLHead").val(slHead);
    $("#hdnSLId").val(slId);
    $("#txtSLCode").val(slCode);
    if (slTypeId != "" && slTypeId != "0") {
        $(".sltype").show();
    }
    else {
        $(".sltype").hide();
        $("#txtSLHead").val("");
        $("#hdnSLId").val("0");
        $("#txtSLCode").val("");
        $("#txtSLBalance").val("");
    }


    $("#txtNarration").val(narration);
    $("#ddlPaymentMode").val(paymentModeId);
    $("#txtAmount").val(amount);

    $("#txtChequeRefNo").val(chequeRefNo);
    $("#txtChequeRefDate").val(chequeRefDate);
    $("#txtValueDate").val(valueDate);
    $("#txtDrawnOnBank").val(drawnOnBank);
    $("#ddlCostCenter").val(costCenterId);
    $("#txtPO_SONo").val(po_SONo);
    $("#txtBillNo").val(billNo);
    $("#txtBillDate").val(billDate);

    $("#btnAddEntry").hide();
    $("#btnUpdateEntry").show();
    ShowHideVoucherEntryPanel(1);
}
function RemoveVoucherEntryRow(obj) {
    if (confirm("Do you want to remove selected Entry?")) {
        var row = $(obj).closest("tr");
        var voucherDetailId = $(row).find("#hdnVoucherDetailId").val();
        var sequenceNo = $(row).find("#hdnSequenceNo").val();
        ShowModel("Alert", "Entry Removed from List.");
        row.remove();

        var voucherEntrySequence = 1;
        var voucherEntryList = [];
        $('#tblVoucherEntryList tr').each(function (i, row) {
            var $row = $(row);
            var voucherDetailId = $row.find("#hdnVoucherDetailId").val();
            var entryMode = $row.find("#hdnEntryMode").val();
            var glId = $row.find("#hdnGLId").val();
            var glCode = $row.find("#hdnGLCode").val();
            var glHead = $row.find("#hdnGLHead").val();
            var slTypeId = $row.find("#hdnSLTypeId").val();
            var slId = $row.find("#hdnSLId").val();
            var slCode = $row.find("#hdnSLCode").val();
            var slHead = $row.find("#hdnSLHead").val();
            var amount = $row.find("#hdnAmount").val();
            var narration = $row.find("#hdnNarration").val();
            var paymentModeId = $row.find("#hdnPaymentModeId").val();
            var chequeRefNo = $row.find("#hdnChequeRefNo").val();
            var chequeRefDate = $row.find("#hdnChequeRefDate").val();
            var costCenterId = $row.find("#hdnCostCenterId").val();
            var valueDate = $row.find("#hdnValueDate").val();
            var drawnOnBank = $row.find("#hdnDrawnOnBank").val();
            var po_SONo = $row.find("#hdnPO_SONo").val();
            var billNo = $row.find("#hdnBillNo").val();
            var billDate = $row.find("#hdnBillDate").val();
            var payeeId = $row.find("#hdnPayeeId").val();
            var payeeName = $row.find("#hdnPayeeName").val();
            var autoEntry = $row.find("#hdnAutoEntry").val();
            if (voucherDetailId != undefined) {
                var voucherEntry = {
                    VoucherDetailId: voucherDetailId,
                    SequenceNo: voucherEntrySequence,
                    EntryMode: entryMode,
                    GLId: glId,
                    GLCode: glCode,
                    GLHead: glHead,
                    SLTypeId: slTypeId,
                    SLId: slId,
                    SLCode: slCode,
                    SLHead: slHead,
                    Amount: amount,
                    Narration: narration,
                    PaymentModeId: paymentModeId,
                    ChequeRefNo: chequeRefNo,
                    ChequeRefDate: chequeRefDate,
                    CostCenterId: costCenterId,
                    ValueDate: valueDate,
                    DrawnOnBank: drawnOnBank,
                    PO_SONo: po_SONo,
                    BillNo: billNo,
                    BillDate: billDate,
                    PayeeId: payeeId,
                    PayeeName: payeeName,
                    AutoEntry: autoEntry
                };
                voucherEntryList.push(voucherEntry);
                voucherEntrySequence = parseInt(voucherEntrySequence) + 1;


            }
        });
        GetBankVoucherEntryList(voucherEntryList);

    }
}
function GetBankVoucherEntryList(voucherEntryList) {
    var hdnVoucherId = $("#hdnVoucherId");
    var requestData = { voucherEntryList: voucherEntryList, voucherId: hdnVoucherId.val() };
    $.ajax({
        url: "../SaleVoucher/GetSaleVoucherEntryList",
        cache: false,
        data: requestData,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "GET",
        error: function (err) {
            $("#divVoucherEntryList").html("");
            $("#divVoucherEntryList").html(err);
        },
        success: function (data) {
            $("#divVoucherEntryList").html("");
            $("#divVoucherEntryList").html(data);
           CalculateVoucherDebitCreditAmount();
           // ShowHideVoucherEntryPanel(2);
        }
    });
}
function GetJournalVoucherDetail(voucherId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../JournalVoucher/GetJournalVoucherDetail",
        data: { voucherId: voucherId },
        dataType: "json",
        success: function (data) {
            $("#txtVoucherNo").val(data.VoucherNo);
            $("#txtVoucherDate").val(data.VoucherDate);
            $("#ddlBook").val(data.BookId);
            $("#ddlBook").attr('disabled', true);
            $("#ddlVoucherMode").val(data.VoucherMode);
            $("#txtVoucherAmount").val(data.VoucherAmount);

            $("#ddlPayeeSLType").val(data.PayeeSLTypeId);

            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedName);
            $("#txtCreatedDate").val(data.CreatedDate);

            if (data.CancelReason != "") {
                $("#divCancelled").show();
                $("#txtCancelReason").val(data.CancelReason);
                $("#txtCancelledDate").val(data.CancelledDate);
            }

            if (data.ModifiedName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            if (data.VoucherStatus == "Cancel" || data.VoucherStatus == "Approved") {
                $("#btnUpdate").hide();
                $("#btnSave").hide();
            }

            $("#btnAddNew").show();
            //$("#btnPrint").show();



        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });


}
function CalculateVoucherDebitCreditAmount() {
    var debitAmount = 0;
    var creditAmount = 0;
    $('#tblVoucherEntryList tr').each(function (i, row) {
        var $row = $(row);
        var voucherDetailId = $row.find("#hdnVoucherDetailId").val();
        var entryMode = $row.find("#hdnEntryMode").val();
        var amount = $row.find("#hdnAmount").val();
        var autoEntry = $row.find("#hdnAutoEntry").val();

        if (voucherDetailId != undefined && autoEntry == false) {
            if (entryMode == "P") {
                debitAmount += parseFloat(amount);
            }
            else {
                creditAmount += parseFloat(amount);
            }
        }

    });

    $("#txtTotalDebit").val(debitAmount.toFixed(2));
    $("#txtTotalCredit").val(creditAmount.toFixed(2));
}