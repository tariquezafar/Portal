    $(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
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
            $("#hdnPayeeId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtPayee").val("");
                $("#hdnPayeeId").val("0");
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
            GetSLBalance(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtSLHead").val("");
                $("#hdnSLId").val("0");
                $("#txtSLCode").val("");
                $("#txtSLBalance").val("0");
                ShowModel("Alert", "Please select Sub Ledger from List")

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
                url: "../Voucher/GetGLAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, bookId: $("#ddlBook").val() },
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
            GetGLBalance(ui.item.value);

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
                $("#txtGLBalance").val("0");
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
   // BindBookTypeList("");
    BindSLTypeList();
    BindPaymentModeList();
    BindCostCenterList();
    BindDocumentTypeList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnVoucherId = $("#hdnVoucherId");
    if (hdnVoucherId.val() != "" && hdnVoucherId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetBankVoucherDetail(hdnVoucherId.val());
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
    var voucherDocuments = [];
    GetVoucherDocumentList(voucherDocuments);


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


function GetVoucherDocumentList(voucherDocuments) {
    var hdnVoucherId = $("#hdnVoucherId");
    var requestData = { voucherDocuments: voucherDocuments, voucherId: hdnVoucherId.val() };
    $.ajax({
        url: "../Voucher/GetVoucherSupportingDocumentList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divDocumentList").html("");
            $("#divDocumentList").html(err);
        },
        success: function (data) {
            $("#divDocumentList").html("");
            $("#divDocumentList").html(data);
            ShowHideDocumentPanel(2);
        }
    });
}
function SaveDocument() {
    if ($("#ddlDocumentType").val() == "0")
    {
        ShowModel("Alert", "Please select document type.");
        $("#ddlDocumentType").focus();
        return false;
    }
    if (window.FormData !== undefined) {
        var uploadfile = document.getElementById('FileUpload1');
        var fileData = new FormData();
        if (uploadfile.value != '') {
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;

            if (uploadfile.files[0].size > 50000000) {
                uploadfile.files[0].name.length = 0;
                ShowModel("Alert", "File is too big")
                uploadfile.value = "";
                return "";
            }

            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
        }
        else {

            ShowModel("Alert", "Please select document.")
            return false;
        }


    } else {

        ShowModel("Alert", "FormData is not supported.")
        return "";
    }

    $.ajax({
        url: "../Voucher/SaveSupportingDocument",
        type: "POST",
        asnc: false,
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        error: function () {
            ShowModel("Alert", "An error occured")
            return "";
        },
        success: function (result) {
            if (result.status == "SUCCESS") {






                var newFileName = result.message;

                var docEntrySequence = 0;
                var hdnDocumentSequence = $("#hdnDocumentSequence");
                var ddlDocumentType = $("#ddlDocumentType");
                var hdnVoucherDocId = $("#hdnVoucherDocId");
                var FileUpload1 = $("#FileUpload1");

                if (ddlDocumentType.val().trim() == "" || ddlDocumentType.val().trim() == "0") {
                    ShowModel("Alert", "Please select Document Type")
                    ddlDocumentType.focus();
                    return false;
                }

                if (FileUpload1.val() == undefined || FileUpload1.val() == "") {
                    ShowModel("Alert", "Please select File To Upload")
                    return false;
                }



                var voucherDocumentList = [];
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    docEntrySequence = 1;
                }
                $('#tblDocumentList tr').each(function (i, row) {
                    var $row = $(row);
                    var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
                    var voucherDocId = $row.find("#hdnVoucherDocId").val();
                    var documentTypeId = $row.find("#hdnDocumentTypeId").val();
                    var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
                    var documentName = $row.find("#hdnDocumentName").val();
                    var documentPath = $row.find("#hdnDocumentPath").val();

                    if (voucherDocId != undefined) {
                        if ((hdnDocumentSequence.val() != documentSequenceNo)) {

                            var voucherDocument = {
                                VoucherDocId: voucherDocId,
                                DocumentSequenceNo: documentSequenceNo,
                                DocumentTypeId: documentTypeId,
                                DocumentTypeDesc: documentTypeDesc,
                                DocumentName: documentName,
                                DocumentPath: documentPath
                            };
                            voucherDocumentList.push(voucherDocument);
                            docEntrySequence = parseInt(docEntrySequence) + 1;
                        }
                        else if (hdnVoucherDocId.val() == voucherDocId && hdnDocumentSequence.val() == documentSequenceNo) {
                            var voucherDocument = {
                                VoucherDocId: hdnVoucherDocId.val(),
                                DocumentSequenceNo: hdnDocumentSequence.val(),
                                DocumentTypeId: ddlDocumentType.val(),
                                DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                                DocumentName: newFileName,
                                DocumentPath: newFileName
                            };
                            voucherDocumentList.push(voucherDocument);
                        }
                    }
                });
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    hdnDocumentSequence.val(docEntrySequence);
                }

                var voucherDocumentAddEdit = {
                    VoucherDocId: hdnVoucherDocId.val(),
                    DocumentSequenceNo: hdnDocumentSequence.val(),
                    DocumentTypeId: ddlDocumentType.val(),
                    DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                    DocumentName: newFileName,
                    DocumentPath: newFileName
                };
                voucherDocumentList.push(voucherDocumentAddEdit);
                hdnDocumentSequence.val("0");

                GetVoucherDocumentList(voucherDocumentList);



            }
            else {
                ShowModel("Alert", result.message);
            }
        }
    });
}
function BindDocumentTypeList() {
    $.ajax({
        type: "GET",
        url: "../PO/GetDocumentTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlDocumentType").append($("<option></option>").val(0).html("-Select Document Type-"));
            $.each(data, function (i, item) {

                $("#ddlDocumentType").append($("<option></option>").val(item.DocumentTypeId).html(item.DocumentTypeDesc));
            });
        },
        error: function (Result) {
            $("#ddlDocumentType").append($("<option></option>").val(0).html("-Select Document Type-"));
        }
    });
}
function RemoveDocumentRow(obj) {
    if (confirm("Do you want to remove selected Document?")) {
        var row = $(obj).closest("tr");
        var hdnVoucherDocId = $(row).find("#hdnVoucherDocId").val();
        ShowModel("Alert", "Document Removed from List.");
        row.remove();
    }
}
function ShowHideDocumentPanel(action) {
    if (action == 1) {
        $(".documentsection").show();
    }
    else {
        $(".documentsection").hide();
        $("#ddlDocumentType").val("0");
        $("#hdnVoucherDocId").val("0");
        $("#FileUpload1").val("");

        $("#btnAddDocument").show();
        $("#btnUpdateDocument").hide();
    }
}



function BindBookTypeList(trnValue) {
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
            if (trnValue != null && trnValue != "")
            {
                $("#ddlBook").val(trnValue);
            }
        },
        error: function (Result) {
            $("#ddlBook").append($("<option></option>").val(0).html("-Select Book-"));
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
function GetBankVoucherDetail(voucherId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Voucher/GetBankVoucherDetail",
        data: { voucherId: voucherId },
        dataType: "json",
        success: function (data) {
            $("#txtVoucherNo").val(data.VoucherNo);
            $("#txtVoucherDate").val(data.VoucherDate);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);

          
          
            $("#ddlVoucherMode").val(data.VoucherMode);
            $("#txtVoucherAmount").val(data.VoucherAmount);
            BindBookTypeList();
            setTimeout(
      function () {
          $("#ddlBook").val(data.BookId);
          $("#ddlBook").attr('disabled', true);
      }, 500);
            
           

            $("#ddlPayeeSLType").val(data.PayeeSLTypeId);


            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            if (data.CancelReason != "") {
                $("#divCancelled").show();
                $("#txtCancelReason").val(data.CancelReason);
                $("#txtCancelledDate").val(data.CancelledDate);
            }
            if (data.ContraVoucherNo != "") {
                $("#divContraVoucherNo").show();
                $("#txtContraNo").val(data.ContraVoucherNo);
                $("#btnUpdate").hide();
                $("#btnSave").hide(); 
            }
            if (data.VoucherStatus =="Cancel" || data.VoucherStatus == "Approved")  {
                $("#btnUpdate").hide();
                $("#btnSave").hide();
            }
            $("#btnAddNew").show();
            $("#btnPrint").show();
            
            GetBankBalance();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });


}
function GetBankBalance() {
    $("#txtBookBalance").val(0);
    var voucherId = $("#hdnVoucherId").val();
    var bookId = $("#ddlBook").val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Voucher/GetBookBalance",
        data: { voucherId: voucherId, bookId: bookId },
        dataType: "json",
        success: function (data) {
            $("#txtBookBalance").val(data);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
            $("#txtBookBalance").val(0);
        }
    });


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
        voucherEntryList.push(voucherEntryAddEdit);
    }

    GetBankVoucherEntryList(voucherEntryList);

}
function EditVoucherEntryRow(obj) {
    $(".sltype").hide();
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
    GetSLBalance(slId);
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
        url: "../Voucher/GetBankVoucherEntryList",
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

    var ddlCompanyBranch = $("#ddlCompanyBranch");

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

    if (ddlCompanyBranch.val() == "0" || ddlCompanyBranch.val() == "") {
        ShowModel("Alert", "Please Select Company Branch")
        ddlCompanyBranch.focus();
        return false;
    }
    if (ddlVoucherMode.val() == "P" && parseFloat(txtBookBalance.val().trim()) < parseFloat(txtVoucherAmount.val().trim())) {
        ShowModel("Alert", "Insufficient Book Balance for Payment Voucher")
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

    if (ddlVoucherMode.val() == "P") {
        if (parseFloat(txtVoucherAmount.val().trim()) != parseFloat(txtTotalDebit.val().trim() == "" ? "0" : txtTotalDebit.val().trim()) - parseFloat(txtTotalCredit.val().trim() == "" ? "0" : txtTotalCredit.val().trim())) {
            ShowModel("Alert", "Voucher Amount and Voucher Entry Total not matching!!!")
            return false;
        }
    }
    else {
        if (parseFloat(txtVoucherAmount.val().trim()) != parseFloat(txtTotalCredit.val().trim() == "" ? "0" : txtTotalCredit.val().trim()) - parseFloat(txtTotalDebit.val().trim() == "" ? "0" : txtTotalDebit.val().trim())) {
            ShowModel("Alert", "Voucher Amount and Voucher Entry Total not matching!!!")
            return false;
        }

    }



    var voucherViewModel = {
        VoucherId: hdnVoucherId.val(),
        VoucherNo: txtVoucherNo.val().trim(),
        VoucherDate: txtVoucherDate.val().trim(),
        VoucherMode: ddlVoucherMode.val(),
        VoucherAmount: txtVoucherAmount.val().trim(),
        PayeeSLTypeId: ddlPayeeSLType.val(),
        BookId: ddlBook.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
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


    var voucherDocumentList = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var voucherDocId = $row.find("#hdnVoucherDocId").val();
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        if (voucherDocId != undefined) {
            var voucherDocument = {
                VoucherDocId: voucherDocId,
                DocumentSequenceNo: documentSequenceNo,
                DocumentTypeId: documentTypeId,
                DocumentTypeDesc: documentTypeDesc,
                DocumentName: documentName,
                DocumentPath: documentPath
            };
            voucherDocumentList.push(voucherDocument);
        }

    });

    var accessMode = 1;//Add Mode
    if (hdnVoucherId.val() != null && hdnVoucherId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { voucherViewModel: voucherViewModel, voucherEntryList: voucherEntryList, voucherDocuments: voucherDocumentList };
    $.ajax({
        url: "../Voucher/AddEditBankVoucher?accessMode=" + accessMode + "",
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
                       if ($("#hdnStartInterface").val() == "")
                           {
                           window.location.href = "../Voucher/AddEditBankVoucher?VoucherId=" + data.trnId + "&AccessMode=3";
                       }
                       else
                       {
                           window.location.href = "../Voucher/AddEditBankVoucher?ReportLevel=" + $("#hdnReportLevel").val() + "&FromDate=" + $("#hdnDrillFromDate").val() + "&ToDate=" + $("#hdnDrillToDate").val() + "&StartInterface=" + $("#hdnStartInterface").val() + "&GLMainGroupId=" + $("#hdnGLMainGroupId").val() + "&GLSubGroupId=" + $("#hdnGLSubGroupId").val() + "&GLId=" + $("#hdnDrillGlId").val() + "&SLId=" + $("#hdnDrillSlId").val() + "&VoucherId=" + data.trnId + "&AccessMode=2";
                       }
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
    var requestData = { piNo: txtInvoiceNo.val().trim(), vendorName: "", refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "", approvalStatus: "Final",vendorCode:vendorCode };
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

function OpenNewBankBookPopup(bookType) {
    if (bookType != null && bookType != "") {
        $("#ddlBookTypePopUp").val(bookType);
        $("#ddlBookTypePopUp").attr("disabled", true);
        changeBookTypePopUp();
        $("#AddBankBook").modal();
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
                $(":input#ddlCompanyBranch").trigger('change');
            }
            if (hdnSessionUserID.val() == "2")
            {
                $(":input#ddlCompanyBranch").trigger('change');
            }
            
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
        }
    });
}
//End Code