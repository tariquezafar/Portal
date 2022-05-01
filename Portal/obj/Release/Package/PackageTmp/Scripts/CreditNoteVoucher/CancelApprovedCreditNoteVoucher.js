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

    $("#txtPO_SONo").attr('readOnly', true);
    $("#txtBillDate").attr('readOnly', true);

    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);

    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);

    $("#txtCancelReason").attr('readOnly', true);

    $("#txtCancelledDate").attr('readOnly', true);

    $("#txtVoucherDate,#txtChequeRefDate,#txtValueDate,#txtBillDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0d',
        onSelect: function (selected) {
        }
    });

    $("#txtPayee").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Voucher/GetSLAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, slTypeId: $("#ddlPayeeSLType").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.SLHead, value: item.SLId, code: item.SLCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtPayee").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtPayee").val(ui.item.label);
            $("#hdnPayeeSLId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtPayee").val("");
                $("#hdnPayeeSLId").val("0");
                ShowModel("Alert", "Please select Payee from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b></div>")
      .appendTo(ul);
};
    $("#txtSLHead").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Voucher/GetSLAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, slTypeId: $("#hdnSLTypeId").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.SLHead, value: item.SLId, code: item.SLCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtSLHead").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtSLHead").val(ui.item.label);
            $("#hdnSLId").val(ui.item.value);
            $("#txtSLCode").val(ui.item.code);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtSLHead").val("");
                $("#hdnSLId").val("0");
                $("#txtSLCode").val("");
                ShowModel("Alert", "Please select Payee from List")

            }
            return false;
        }

    })
 .autocomplete("instance")._renderItem = function (ul, item) {
     return $("<li>")
       .append("<div><b>" + item.label + " || " + item.code + "</b></div>")
       .appendTo(ul);
 };

    $("#txtGLHead").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../JournalVoucher/GetJVGLAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.GLHead, value: item.GLId, SLTypeId: item.SLTypeId, code: item.GLCode };
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
            $("#txtGLCode").val(ui.item.code);
            $("#hdnSLTypeId").val(ui.item.SLTypeId);

            if (ui.item.SLTypeId != undefined && ui.item.SLTypeId != "" && ui.item.SLTypeId != "0") {
                $(".sltype").show();
            }
            else {
                $(".sltype").hide();
                $("#txtSLHead").val("");
                $("#hdnSLId").val("0");
                $("#txtSLCode").val("");
                $("#txtSLBalance").val("");
            }

            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtGLHead").val("");
                $("#hdnGLId").val("0");
                $("#txtGLCode").val("");
                $("#hdnSLTypeId").val("0");
                $(".sltype").hide();

                $("#txtSLHead").val("");
                $("#hdnSLId").val("0");
                $("#txtSLCode").val("");
                $("#txtSLBalance").val("");

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
    BindCompanyBranchList();
    BindBookTypeList(); 
    BindPaymentModeList();
    BindCostCenterList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnVoucherId = $("#hdnVoucherId");
    if (hdnVoucherId.val() != "" && hdnVoucherId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetCreditNoteVoucherDetail(hdnVoucherId.val());
       }, 2000);



        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("#btnCancel").hide();
            $(".approvalStatus").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $("#ddlApprovalStatus").attr('disabled', false);
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
    GetCreditNoteVoucherEntryList(voucherEntryList);



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


function BindBookTypeList() {
    var requestData = { bookType: "CN" };
    $.ajax({
        type: "GET",
        url: "../CreditNoteVoucher/GetBookList",
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
function GetCreditNoteVoucherDetail(voucherId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../CreditNoteVoucher/GetCreditNoteVoucherDetail",
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
            if (data.ModifiedName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }

            $("#btnAddNew").show();
            $("#btnPrint").show();



        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
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

    GetCreditNoteVoucherEntryList(voucherEntryList);

}
function EditVoucherEntryRow(obj) {

    var row = $(obj).closest("tr");
    var voucherDetailId = $(row).find("#hdnVoucherDetailId").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var entryMode = $(row).find("#hdnEntryMode").val();
    var glId = $(row).find("#hdnGLId").val();
    var glCode = $(row).find("#hdnGLCode").val();
    var glHead = $(row).find("#hdnGLHead").val();
    var slTypeId = $(row).find("#hdnSLTypeId").val();
    
    var slId = $(row).find("#hdnSLId").val();

    var slCode = $(row).find("#hdnSLCode").val();
    var slHead = $(row).find("#hdnSLHead").val();
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
    if (slTypeId != undefined && slTypeId != "" && slTypeId != "0") {
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
        GetCreditNoteVoucherEntryList(voucherEntryList);

    }
}
function GetCreditNoteVoucherEntryList(voucherEntryList) {
    var hdnVoucherId = $("#hdnVoucherId");
    var requestData = { voucherEntryList: voucherEntryList, voucherId: hdnVoucherId.val() };
    $.ajax({
        url: "../CreditNoteVoucher/GetCreditNoteVoucherEntryList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divVoucherEntryList").html("");
            $("#divVoucherEntryList").html(err);
        },
        success: function (data) {
            $("#divVoucherEntryList").html("");
            $("#divVoucherEntryList").html(data);
            CalculateVoucherDebitCreditAmount();
            ShowHideVoucherEntryPanel(2);
        }
    });
}
function ShowHideVoucherEntryPanel(action) {
    if (action == 1) {
        var ddlBook = $("#ddlBook");
        if (ddlBook.val() == "" || ddlBook.val() == "0") {
        ShowModel("Alert", "Please select Bank Book")
        ddlBook.focus();
        return false;
    }

        $(".voucherentrysection").show();

    }
    else {
        $(".voucherentrysection").hide();
        $("#ddlEntryMode").val("P");
        $("#hdnVoucherDetailId").val("0");
        $("#hdnSequenceNo").val("0");

        $("#txtGLHead").val("");
        $("#hdnGLId").val("0");
        $("#hdnSLTypeId").val("0");
        $("#txtGLCode").val("");
        $("#txtGLBalance").val("");

        $("#txtSLHead").val("");
        $("#hdnSLId").val("0");
        $("#txtSLCode").val("");
        $("#txtSLBalance").val("");

        $("#txtNarration").val("");
        $("#ddlPaymentMode").val("0");
        $("#txtAmount").val("");
        $("#txtChequeRefNo").val("");

        $("#txtChequeRefDate").val("");
        $("#txtValueDate").val("");
        $("#txtDrawnOnBank").val("");
        $("#ddlCostCenter").val("0");
        $("#txtPO_SONo").val("");
        $("#txtBillNo").val("");
        $("#txtBillDate").val("");
        $("#btnAddEntry").show();
        $("#btnUpdateEntry").hide();


    }
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
function SaveData() {
    var txtVoucherNo = $("#txtVoucherNo");
    var hdnVoucherId = $("#hdnVoucherId");
    var txtVoucherDate = $("#txtVoucherDate");
    var ddlBook = $("#ddlBook");
    var txtBookBalance = $("#txtBookBalance");

    var ddlVoucherMode = $("#ddlVoucherMode");
    var txtVoucherAmount = $("#txtVoucherAmount");
    var ddlPayeeSLType = $("#ddlPayeeSLType");

    var txtTotalDebit = $("#txtTotalDebit");
    var txtTotalCredit = $("#txtTotalCredit");

    if (ddlBook.val() == "" || ddlBook.val() == "0") {
        ShowModel("Alert", "Please select Bank Book")
        ddlBook.focus();
        return false;
    }


    if (txtVoucherAmount.val().trim() == "" || parseFloat(txtVoucherAmount.val().trim()) <= 0) {
        ShowModel("Alert", "Please Enter Voucher Amount")
        txtVoucherAmount.focus();
        return false;
    }
    if (ddlPayeeSLType.val() == "") {
        ShowModel("Alert", "Please select Payee SL Type")
        ddlPayeeSLType.focus();
        return false;
    }
    if ((txtTotalDebit.val().trim() == "" || txtTotalDebit.val().trim() == "0") && (txtTotalCredit.val().trim() == "" || txtTotalCredit.val().trim() == "0")) {
        ShowModel("Alert", "Please enter atleast single Voucher Entry")
        return false;
    }

    if (parseFloat(txtTotalDebit.val().trim() == "" ? "0" : txtTotalDebit.val().trim()) !=  parseFloat(txtTotalCredit.val().trim() == "" ? "0" : txtTotalCredit.val().trim())) {
        ShowModel("Alert", "Voucher Credit and Voucher Debit Entry Total not matching!!!")
        return false;
    }
    if ((parseFloat(txtVoucherAmount.val().trim()) != parseFloat(txtTotalCredit.val().trim()))||(parseFloat(txtVoucherAmount.val().trim()) != parseFloat(txtTotalDebit.val().trim()))) {
      //  ShowModel("Alert", parseFloat(txtVoucherAmount.val().trim()));
       // ShowModel("Alert", parseFloat(txtTotalCredit.val().trim()));
       // ShowModel("Alert", parseFloat(txtTotalDebit.val().trim()));
        ShowModel("Alert", "Voucher Amount and Voucher Entry Total not matching!!!")
        return false;
    }
    


    var voucherViewModel = {
        VoucherId: hdnVoucherId.val(),
        VoucherNo: txtVoucherNo.val().trim(),
        VoucherDate: txtVoucherDate.val().trim(),
        VoucherMode: "JV",//ddlVoucherMode.val(),
        VoucherAmount: txtVoucherAmount.val().trim(),
        PayeeSLTypeId: ddlPayeeSLType.val(),
        BookId: ddlBook.val()
    };



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
        var chequeRefNo = ""; //$row.find("#hdnChequeRefNo").val();
        var chequeRefDate = ""; //$row.find("#hdnChequeRefDate").val();
        var costCenterId = $row.find("#hdnCostCenterId").val();
        var valueDate = "";//$row.find("#hdnValueDate").val();
        var drawnOnBank = "";//$row.find("#hdnDrawnOnBank").val();
        var po_SONo = $row.find("#hdnPO_SONo").val();
        var billNo = $row.find("#hdnBillNo").val();
        var billDate = $row.find("#hdnBillDate").val();
        var payeeId = $row.find("#hdnPayeeId").val();
        var payeeName = $row.find("#hdnPayeeName").val();
        var autoEntry = $row.find("#hdnAutoEntry").val();
        if (voucherDetailId != undefined) {
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
        }
    });




    var requestData = { voucherViewModel: voucherViewModel, voucherEntryList: voucherEntryList };
    $.ajax({
        url: "../CreditNoteVoucher/AddEditCreditNoteVoucher",
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
                       //window.location.href = "../CreditNoteVoucher/AddEditCreditNoteVoucher?VoucherId=" + data.trnId + "&AccessMode=2";
                       window.location.href = "../CreditNoteVoucher/ListApprovedCreditNoteVoucher";
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
function CancelCreditNoteVoucher() {
    var hdnVoucherId = $("#hdnVoucherId");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtCancelReasons = $("#txtCancelReasons");
    var requestData = {
        voucherId: hdnVoucherId.val(),
        cancelReason: txtCancelReasons.val().trim(),
        voucherStatus: ddlApprovalStatus.val()
    };
    $.ajax({
        //  url: "../CashVoucher/CancelApprovedCashVoucher",
        url: "../CreditNoteVoucher/CancelApprovedCreditNoteVoucher",
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
                       window.location.href = "../CreditNoteVoucher/ListApprovedCreditNoteVoucher";
                   }, 2000);

                $("#btnCancel").hide();
                $("#btnList").show();
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

function ClearFields() {

    $("#txtVoucherNo").val("");
    $("#hdnVoucherId").val("0");
    $("#txtVoucherDate").val($("#hdnCurrentDate").val());

    $("#ddlBook").val("0");
    $("#txtBookBalance").val("");
    $("#ddlVoucherMode").val("P");

    $("#txtVoucherAmount").val("");

    $("#divCreated").hide();
    $("#divModified").hide();
    $("#divCancelled").hide();
    $("#btnSave").show();
    $("#btnUpdate").hide();



}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function changeSaveMode() {
    var ddlApprovalStatus = $("#ddlApprovalStatus").val();
    if (ddlApprovalStatus != undefined) {
        if (ddlApprovalStatus == "Cancel") {
            $("#txtCancelReasonhide").show();
            $("#txtCancelReasons").attr('readOnly', false);
        }
        else {
            $("#txtCancelReasonhide").hide();
        }

    }
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
        }
    });
}
//End Code