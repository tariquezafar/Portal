$(document).ready(function () {

    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtSONo").attr('readOnly', true);
    $("#txtSODate").attr('readOnly', true);
    $("#txtQuotationNo").attr('readOnly', true);
    $("#txtQuotationDate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtCustomerCode").attr('readOnly', true);
    $("#txtConsigneeCode").attr('readOnly', true);

    $("#txtCGSTPerc").attr('readOnly', true);
    $("#txtSGSTPerc").attr('readOnly', true);
    $("#txtIGSTPerc").attr('readOnly', true);
    $("#txtCGSTPercAmount").attr('readOnly', true);
    $("#txtSGSTPercAmount").attr('readOnly', true);
    $("#txtIGSTPercAmount").attr('readOnly', true);
    $("#txtHSN_Code").attr('readOnly', true);



    $("#txtRefDate").attr('readOnly', true);
    $("#txtPayToBookBranch").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtDiscountAmount").attr('readOnly', true);
    $("#txtTaxAmount").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtAvailableStock").attr('readOnly', true);
    $("#txtTotalPrice").attr('readOnly', true);
    $("#txtBasicValue").attr('readOnly', true);

    $("#txtTaxPercentage").attr('readOnly', true);
    $("#txtTaxAmount").attr('readOnly', true);
    $("#txtTotalValue").attr('readOnly', true);

    $("#txtFreightCGST_Amt").attr('readOnly', true);
    $("#txtFreightSGST_Amt").attr('readOnly', true);
    $("#txtFreightIGST_Amt").attr('readOnly', true);

    $("#txtInsuranceCGST_Amt").attr('readOnly', true);
    $("#txtInsuranceSGST_Amt").attr('readOnly', true);
    $("#txtInsuranceIGST_Amt").attr('readOnly', true);

    $("#txtLoadingCGST_Amt").attr('readOnly', true);
    $("#txtLoadingSGST_Amt").attr('readOnly', true);
    $("#txtLoadingIGST_Amt").attr('readOnly', true);




  
    $("#txtSODate,#txtRefDate").datepicker({
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

    BindCompanyBranchList();
    BindCurrencyList();
    BindTermTemplateList();
    BindDocumentTypeList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnSOId = $("#hdnSOId");
    if (hdnSOId.val() != "" && hdnSOId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetSODetail(hdnSOId.val());

       }, 2000);
        var customerId = $("#hdnCustomerId").val();
        BindCustomerBranchList(customerId);
        BindConsigneeBranchList(customerId);       
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $(".editonly").hide();
            $("#btnReCalculate").hide();
            $("#chkReverseCharge").attr('disabled', true);
            if ($(".editonly").hide()) {
                $('#lblQuotationDate').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
            }
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


        //if ($("#hdnCustomerId").val() != "0") {
        //    GetCustomerDetail($("#hdnCustomerId").val());

        //}
    }
    $("#txtCancelReason").attr('readOnly', false);
    var soProducts = [];
    GetSOProductList(soProducts);
    var soTaxes = [];
    GetSOTaxList(soTaxes);
    var soTerms = [];
    GetSOTermList(soTerms);
    var soDocuments = [];
    GetSODocumentList(soDocuments);

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
function BindCurrencyList() {
    $("#ddlCurrency").val(0);
    $("#ddlCurrency").html("");
    $.ajax({
        type: "GET",
        url: "../Quotation/GetCurrencyList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCurrency").append($("<option></option>").val(0).html("-Select Currency-"));
            $.each(data, function (i, item) {
                $("#ddlCurrency").append($("<option></option>").val(item.CurrencyCode).html(item.CurrencyName));
            });
            $("#ddlCurrency").val("INR");
        },
        error: function (Result) {
            $("#ddlCurrency").append($("<option></option>").val(0).html("-Select Currency-"));
        }
    });
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}


function GetSODocumentList(soDocuments) {
    var hdnSOId = $("#hdnSOId");
    var requestData = { soDocuments: soDocuments, soId: hdnSOId.val() };
    $.ajax({
        url: "../SO/GetSOSupportingDocumentList",
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
    if ($("#ddlDocumentType").val() == "0") {
        ShowModel("Alert", "Please Select document type")
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

            ShowModel("Alert", "Please Select File")
            return false;

        }


    } else {

        ShowModel("Alert", "FormData is not supported.")
        return "";
    }

    $.ajax({
        url: "../SO/SaveSupportingDocument",
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
                var hdnSODocId = $("#hdnSODocId");
                var FileUpload1 = $("#FileUpload1");

                if (ddlDocumentType.val() == "" || ddlDocumentType.val() == "0") {
                    ShowModel("Alert", "Please select Document Type")
                    ddlDocumentType.focus();
                    return false;
                }

                if (FileUpload1.val() == undefined || FileUpload1.val() == "") {
                    ShowModel("Alert", "Please select File To Upload")
                    return false;
                }
                var soDocumentList = [];
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    docEntrySequence = 1;
                }
                $('#tblDocumentList tr').each(function (i, row) {
                    var $row = $(row);
                    var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
                    var hdnSODocId = $row.find("#hdnSODocId").val();
                    var documentTypeId = $row.find("#hdnDocumentTypeId").val();
                    var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
                    var documentName = $row.find("#hdnDocumentName").val();
                    var documentPath = $row.find("#hdnDocumentPath").val();

                    if (hdnSODocId != undefined) {
                        if ((hdnDocumentSequence.val() != documentSequenceNo)) {

                            var soDocument = {
                                SODocId: hdnSODocId,
                                DocumentSequenceNo: documentSequenceNo,
                                DocumentTypeId: documentTypeId,
                                DocumentTypeDesc: documentTypeDesc,
                                DocumentName: documentName,
                                DocumentPath: documentPath
                            };
                            soDocumentList.push(soDocument);
                            docEntrySequence = parseInt(docEntrySequence) + 1;
                        }
                        else if (hdnSODocId.val() == sODocId && hdnDocumentSequence.val() == documentSequenceNo) {
                            var soDocument = {
                                SODocId: hdnSODocId.val(),
                                DocumentSequenceNo: hdnDocumentSequence.val(),
                                DocumentTypeId: ddlDocumentType.val(),
                                DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                                DocumentName: newFileName,
                                DocumentPath: newFileName
                            };
                            soDocumentList.push(soDocument);
                        }
                    }
                });
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    hdnDocumentSequence.val(docEntrySequence);
                }

                var soDocumentAddEdit = {
                    SODocId: hdnSODocId.val(),
                    DocumentSequenceNo: hdnDocumentSequence.val(),
                    DocumentTypeId: ddlDocumentType.val(),
                    DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                    DocumentName: newFileName,
                    DocumentPath: newFileName
                };
                soDocumentList.push(soDocumentAddEdit);
                hdnDocumentSequence.val("0");

                GetSODocumentList(soDocumentList);



            }
            else {
                ShowModel("Alert", result.message);
            }
        }
    });
}
function ShowHideDocumentPanel(action) {
    if (action == 1) {
        $(".documentsection").show();
    }
    else {
        $(".documentsection").hide();
        $("#ddlDocumentType").val("0");
        $("#hdnSODocId").val("0");
        $("#FileUpload1").val("");

        $("#btnAddDocument").show();
        $("#btnUpdateDocument").hide();
    }
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
        var poDocId = $(row).find("#hdnPODocId").val();
        ShowModel("Alert", "Document Removed from List.");
        row.remove();
    }
}

function GetConsigneeDetail(customerId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Customer/GetCustomerDetail",
        data: { customerId: customerId },
        dataType: "json",
        success: function (data) {
            $("#txtSContactPerson").val(data.ContactPersonName);
            $("#txtSAddress").val(data.PrimaryAddress);
            $("#txtSCity").val(data.City);
            $("#ddlSCountry").val(data.CountryId);
            $("#ddlSState").val(data.StateId);
            $("#txtSPinCode").val(data.PinCode);
            $("#txtSTINNo").val(data.TINNo);
            $("#txtSEmail").val(data.Email);
            $("#txtSMobileNo").val(data.MobileNo);
            $("#txtSContactNo").val(data.ContactNo);
            $("#txtSFax").val(data.Fax);

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    BindConsigneeBranchList(customerId)
}

function BindCustomerBranchList(customerId) {
    $("#ddlBCustomerBranch").val(0);
    $("#ddlBCustomerBranch").html("");
    $.ajax({
        type: "GET",
        url: "../SO/GetCustomerBranchList",
        data: { customerId: customerId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlBCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
            $.each(data, function (i, item) {
                $("#ddlBCustomerBranch").append($("<option></option>").val(item.CustomerBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlBCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
        }
    });
}

function BindConsigneeBranchList(customerId) {
    $("#ddlSCustomerBranch").val(0);
    $("#ddlSCustomerBranch").html("");
    $.ajax({
        type: "GET",
        url: "../SO/GetCustomerBranchList",
        data: { customerId: customerId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
            $.each(data, function (i, item) {
                $("#ddlSCustomerBranch").append($("<option></option>").val(item.CustomerBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlSCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
        }
    });
}

function BindTermTemplateList() {
    $.ajax({
        type: "GET",
        url: "../SO/GetTermTemplateList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlTermTemplate").append($("<option></option>").val(0).html("-Select Terms Template-"));
            $.each(data, function (i, item) {

                $("#ddlTermTemplate").append($("<option></option>").val(item.TermTemplateId).html(item.TermTempalteName));
            });
        },
        error: function (Result) {
            $("#ddlTermTemplate").append($("<option></option>").val(0).html("-Select Terms Template-"));
        }
    });
}

function BindTermsDescription() {
    var termTemplateId = $("#ddlTermTemplate option:selected").val();
    var soTermList = [];
    if (termTemplateId != undefined && termTemplateId != "" && termTemplateId != "0") {
        var data = { termTemplateId: termTemplateId };

        $.ajax({
            type: "GET",
            url: "../SO/GetTermTemplateDetailList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                var termCounter = 1;
                $.each(data, function (i, item) {
                    var soTerm = {
                        SOTermDetailId: item.TrnId,
                        TermDesc: item.TermsDesc,
                        TermSequence: termCounter
                    };
                    soTermList.push(soTerm);
                    termCounter += 1;
                });
                GetSOTermList(soTermList);
            },
            error: function (Result) {
                GetSOTermList(soTermList);
            }
        });
    }
    else {
        GetSOTermList(soTermList);
    }
}
function GetSOTermList(soTerms) {
    var hdnSOId = $("#hdnSOId");
    var requestData = { soTerms: soTerms, soId: hdnSOId.val() };
    $.ajax({
        url: "../SO/GetSOTermList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divTermList").html("");
            $("#divTermList").html(err);
        },
        success: function (data) {
            $("#divTermList").html("");
            $("#divTermList").html(data);
            ShowHideTermPanel(2);
        }
    });
}
function AddTerm(action) {
    var txtTermDesc = $("#txtTermDesc");
    var hdnSOTermDetailId = $("#hdnSOTermDetailId");
    var hdnTermSequence = $("#hdnTermSequence");

    if (txtTermDesc.val().trim() == "") {
        ShowModel("Alert", "Please Enter Terms")
        txtTermDesc.focus();
        return false;
    }

    var soTermList = [];
    var termCounter = 1;
    $('#tblTermList tr').each(function (i, row) {
        var $row = $(row);
        var soTermDetailId = $row.find("#hdnSOTermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (termDesc != undefined) {
            if (action == 1 || hdnSOTermDetailId.val() != soTermDetailId) {

                if (termSequence == 0)
                { termSequence = termCounter; }

                var soTerm = {
                    SOTermDetailId: soTermDetailId,
                    TermDesc: termDesc,
                    TermSequence: termSequence
                };
                soTermList.push(soTerm);
                termCounter += 1;
            }
        }

    });

    if (hdnTermSequence.val() == "" || hdnTermSequence.val() == "0") {
        hdnTermSequence.val(termCounter);
    }
    var soTermAddEdit = {
        SOTermDetailId: hdnSOTermDetailId.val(),
        TermDesc: txtTermDesc.val().trim(),
        TermSequence: hdnTermSequence.val()
    };

    soTermList.push(soTermAddEdit);
    GetSOTermList(soTermList);

}
function EditTermRow(obj) {

    var row = $(obj).closest("tr");
    var soTermDetailId = $(row).find("#hdnSOTermDetailId").val();
    var termDesc = $(row).find("#hdnTermDesc").val();
    var termSequence = $(row).find("#hdnTermSequence").val();


    $("#txtTermDesc").val(termDesc);
    $("#hdnSOTermDetailId").val(soTermDetailId);
    $("#hdnTermSequence").val(termSequence);

    $("#btnAddTerm").hide();
    $("#btnUpdateTerm").show();
    ShowHideTermPanel(1);
}

function RemoveTermRow(obj) {
    if (confirm("Do you want to remove selected Term?")) {
        var row = $(obj).closest("tr");
        var soTermDetailId = $(row).find("#hdnSOTermDetailId").val();
        ShowModel("Alert", "Term Removed from List.");
        row.remove();
    }
}

function GetSOTaxList(soTaxes) {
    var hdnSOId = $("#hdnSOId");
    var requestData = { soTaxes: soTaxes, soId: hdnSOId.val() };
    $.ajax({
        url: "../SO/GetSOTaxList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divTaxList").html("");
            $("#divTaxList").html(err);
        },
        success: function (data) {
            $("#divTaxList").html("");
            $("#divTaxList").html(data);
            CalculateGrossandNetValues();
            ShowHideTaxPanel(2);
        }
    });
}
function AddTax(action) {
    var flag = true;
    var taxEntrySequence = 0;
    var hdnTaxSequenceNo = $("#hdnTaxSequenceNo");
    var txtBasicValue = $("#txtBasicValue");
    var txtTaxName = $("#txtTaxName");
    var hdnSOTaxDetailId = $("#hdnSOTaxDetailId");
    var hdnTaxId = $("#hdnTaxId");
    var txtTaxPercentage = $("#txtTaxPercentage");
    var txtTaxAmount = $("#txtTaxAmount");

    var txtTotalTaxAmount = $("#txtTotalTaxAmount");
    var txtSurchargeAmount_1 = $("#txtSurchargeAmount_1");
    var txtSurchargeAmount_2 = $("#txtSurchargeAmount_2");
    var txtSurchargeAmount_3 = $("#txtSurchargeAmount_3");
    var hdnSurchargeName_1 = $("#hdnSurchargeName_1");
    var hdnSurchargePercentage_1 = $("#hdnSurchargePercentage_1");
    var hdnSurchargeName_2 = $("#hdnSurchargeName_2");
    var hdnSurchargePercentage_2 = $("#hdnSurchargePercentage_2");
    var hdnSurchargeName_3 = $("#hdnSurchargeName_3");
    var hdnSurchargePercentage_3 = $("#hdnSurchargePercentage_3");

    if (txtBasicValue.val().trim() == "" || txtBasicValue.val().trim() == "0" || parseFloat(txtBasicValue.val().trim()) <= 0) {
        ShowModel("Alert", "Please select at least single product")
        txtTaxName.focus();
        return false;
    }

    if (txtTaxName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Tax Name")
        txtTaxName.focus();
        return false;
    }
    if (hdnTaxId.val().trim() == "" || hdnTaxId.val().trim() == "0") {
        ShowModel("Alert", "Please select Tax from list")
        txtTaxName.focus();
        return false;
    }

    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        taxEntrySequence = 1;
    }
    var soTaxList = [];
    $('#tblTaxList tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var soTaxDetailId = $row.find("#hdnSOTaxDetailId").val();
        var taxId = $row.find("#hdnTaxId").val();
        var taxName = $row.find("#hdnTaxName").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();
        var taxAmount = $row.find("#hdnTaxAmount").val();

        var surchargeName_1 = $row.find("#hdnSurchargeName_1").val();
        var surchargePercentage_1 = $row.find("#hdnSurchargePercentage_1").val();
        var surchargeAmount_1 = $row.find("#hdnSurchargeAmount_1").val();

        var surchargeName_2 = $row.find("#hdnSurchargeName_2").val();
        var surchargePercentage_2 = $row.find("#hdnSurchargePercentage_2").val();
        var surchargeAmount_2 = $row.find("#hdnSurchargeAmount_2").val();

        var surchargeName_3 = $row.find("#hdnSurchargeName_3").val();
        var surchargePercentage_3 = $row.find("#hdnSurchargePercentage_3").val();
        var surchargeAmount_3 = $row.find("#hdnSurchargeAmount_3").val();


        if (taxId != undefined) {
            if (action == 1 || (hdnTaxSequenceNo.val() != taxSequenceNo)) {

                if (taxId == hdnTaxId.val()) {
                    ShowModel("Alert", "Tax already added!!!")
                    txtTaxName.focus();
                    var flag = false;
                    return false;
                }
                var soTaxAddEdit = {
                    SOTaxDetailId: soTaxDetailId,
                    TaxSequenceNo: taxSequenceNo,
                    TaxId: taxId,
                    TaxName: taxName,
                    TaxPercentage: taxPercentage,
                    TaxAmount: taxAmount,
                    SurchargeName_1: surchargeName_1,
                    SurchargePercentage_1: surchargePercentage_1,
                    SurchargeAmount_1: surchargeAmount_1,
                    SurchargeName_2: surchargeName_2,
                    SurchargePercentage_2: surchargePercentage_2,
                    SurchargeAmount_2: surchargeAmount_2,
                    SurchargeName_3: surchargeName_3,
                    SurchargePercentage_3: surchargePercentage_3,
                    SurchargeAmount_3: surchargeAmount_3
                };
                soTaxList.push(soTaxAddEdit);
                taxEntrySequence = parseInt(taxEntrySequence) + 1;
            }
            else if (hdnSOTaxDetailId.val() == soTaxDetailId && hdnTaxSequenceNo.val() == taxSequenceNo) {
                var soTaxAddEdit = {
                    SOTaxDetailId: hdnSOTaxDetailId.val(),
                    TaxSequenceNo: hdnTaxSequenceNo.val(),
                    TaxId: hdnTaxId.val(),
                    TaxName: txtTaxName.val().trim(),
                    TaxPercentage: txtTaxPercentage.val().trim(),
                    TaxAmount: txtTaxAmount.val().trim(),
                    SurchargeName_1: hdnSurchargeName_1.val(),
                    SurchargePercentage_1: hdnSurchargePercentage_1.val(),
                    SurchargeAmount_1: txtSurchargeAmount_1.val(),
                    SurchargeName_2: hdnSurchargeName_2.val(),
                    SurchargePercentage_2: hdnSurchargePercentage_2.val(),
                    SurchargeAmount_2: txtSurchargeAmount_2.val(),
                    SurchargeName_3: hdnSurchargeName_3.val(),
                    SurchargePercentage_3: hdnSurchargePercentage_3.val(),
                    SurchargeAmount_3: txtSurchargeAmount_3.val()
                };
                soTaxList.push(soTaxAddEdit);
                hdnTaxSequenceNo.val("0");
            }
        }
    });

    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        hdnTaxSequenceNo.val(taxEntrySequence);
    }
    if (action == 1) {
        var soTaxAddEdit = {
            SOTaxDetailId: hdnSOTaxDetailId.val(),
            TaxSequenceNo: hdnTaxSequenceNo.val(),
            TaxId: hdnTaxId.val(),
            TaxName: txtTaxName.val().trim(),
            TaxPercentage: txtTaxPercentage.val().trim(),
            TaxAmount: txtTaxAmount.val().trim(),
            SurchargeName_1: hdnSurchargeName_1.val(),
            SurchargePercentage_1: hdnSurchargePercentage_1.val(),
            SurchargeAmount_1: txtSurchargeAmount_1.val(),
            SurchargeName_2: hdnSurchargeName_2.val(),
            SurchargePercentage_2: hdnSurchargePercentage_2.val(),
            SurchargeAmount_2: txtSurchargeAmount_2.val(),
            SurchargeName_3: hdnSurchargeName_3.val(),
            SurchargePercentage_3: hdnSurchargePercentage_3.val(),
            SurchargeAmount_3: txtSurchargeAmount_3.val()
        };
        soTaxList.push(soTaxAddEdit);
        hdnTaxSequenceNo.val("0");
    }
    if (flag == true) {
        GetSOTaxList(soTaxList);
    }
}
function EditTaxRow(obj) {

    var row = $(obj).closest("tr");
    var soTaxDetailId = $(row).find("#hdnSOTaxDetailId").val();
    var taxSequenceNo = $(row).find("#hdnTaxSequenceNo").val();
    var taxId = $(row).find("#hdnTaxId").val();
    var taxName = $(row).find("#hdnTaxName").val();
    var taxPercentage = $(row).find("#hdnTaxPercentage").val();
    var taxAmount = $(row).find("#hdnTaxAmount").val();

    var surchargeName_1 = $(row).find("#hdnSurchargeName_1").val();
    var surchargePercentage_1 = $(row).find("#hdnSurchargePercentage_1").val();
    var surchargeAmount_1 = $(row).find("#hdnSurchargeAmount_1").val();

    var surchargeName_2 = $(row).find("#hdnSurchargeName_2").val();
    var surchargePercentage_2 = $(row).find("#hdnSurchargePercentage_2").val();
    var surchargeAmount_2 = $(row).find("#hdnSurchargeAmount_2").val();

    var surchargeName_3 = $(row).find("#hdnSurchargeName_3").val();
    var surchargePercentage_3 = $(row).find("#hdnSurchargePercentage_3").val();
    var surchargeAmount_3 = $(row).find("#hdnSurchargeAmount_3").val();


    $("#txtTaxName").val(taxName);
    $("#hdnSOTaxDetailId").val(soTaxDetailId);
    $("#hdnTaxSequenceNo").val(taxSequenceNo);
    $("#hdnTaxId").val(taxId);
    $("#txtTaxPercentage").val(taxPercentage);
    $("#txtTaxAmount").val(taxAmount);

    $("#hdnSurchargeName_1").val(surchargeName_1);
    $("#hdnSurchargePercentage_1").val(surchargePercentage_1);
    $("#txtSurchargeAmount_1").val(surchargeAmount_1);

    $("#hdnSurchargeName_2").val(surchargeName_2);
    $("#hdnSurchargePercentage_2").val(surchargePercentage_2);
    $("#txtSurchargeAmount_2").val(surchargeAmount_2);

    $("#hdnSurchargeName_3").val(surchargeName_3);
    $("#hdnSurchargePercentage_3").val(surchargePercentage_3);
    $("#txtSurchargeAmount_3").val(surchargeAmount_3);
    $("#txtTotalTaxAmount").val(parseFloat(taxAmount) + parseFloat(surchargeAmount_1) + parseFloat(surchargeAmount_2) + parseFloat(surchargeAmount_3));


    $("#btnAddTax").hide();
    $("#btnUpdateTax").show();
    ShowHideTaxPanel(1);
}

function RemoveTaxRow(obj) {
    if (confirm("Do you want to remove selected Tax?")) {
        var row = $(obj).closest("tr");
        var soTaxDetailId = $(row).find("#hdnSOTaxDetailId").val();
        ShowModel("Alert", "Tax Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
    }
}


function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var txtProductName = $("#txtProductName");
    var hdnSOProductDetailId = $("#hdnSOProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtPrice = $("#txtPrice");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var txtDiscountPerc = $("#txtDiscountPerc");
    var txtDiscountAmount = $("#txtDiscountAmount");
    var txtProductTaxName = $("#txtProductTaxName");
    var hdnProductTaxId = $("#hdnProductTaxId");
    var hdnProductTaxPerc = $("#hdnProductTaxPerc");
    var txtProductTaxAmount = $("#txtProductTaxAmount");
    var hdnSequenceNo = $("#hdnSequenceNo");

    var txtProductSurchargeAmount_1 = $("#txtProductSurchargeAmount_1");
    var txtProductSurchargeAmount_2 = $("#txtProductSurchargeAmount_2");
    var txtProductSurchargeAmount_3 = $("#txtProductSurchargeAmount_3");
    var hdnProductSurchargeName_1 = $("#hdnProductSurchargeName_1");
    var hdnProductSurchargePercentage_1 = $("#hdnProductSurchargePercentage_1");
    var hdnProductSurchargeName_2 = $("#hdnProductSurchargeName_2");
    var hdnProductSurchargePercentage_2 = $("#hdnProductSurchargePercentage_2");
    var hdnProductSurchargeName_3 = $("#hdnProductSurchargeName_3");
    var hdnProductSurchargePercentage_3 = $("#hdnProductSurchargePercentage_3");
    var txtProductTotalTaxAmount = $("#txtProductTotalTaxAmount");

    var txtCGSTPerc = $("#txtCGSTPerc");
    var txtCGSTPercAmount = $("#txtCGSTPercAmount");
    var txtSGSTPerc = $("#txtSGSTPerc");
    var txtSGSTPercAmount = $("#txtSGSTPercAmount");
    var txtIGSTPerc = $("#txtIGSTPerc");
    var txtIGSTPercAmount = $("#txtIGSTPercAmount");
    var txtHSN_Code = $("#txtHSN_Code");

    var txtTotalPrice = $("#txtTotalPrice");

    if (txtProductName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Product Name")
        txtProductName.focus();
        return false;
    }
    if (hdnProductId.val().trim() == "" || hdnProductId.val().trim() == "0") {
        ShowModel("Alert", "Please select Product from list")
        hdnProductId.focus();
        return false;
    }
    if (txtPrice.val().trim() == "" || txtPrice.val().trim() == "0" || parseFloat(txtPrice.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter Price")
        txtPrice.focus();
        return false;
    }

    if (txtQuantity.val().trim() == "" || txtQuantity.val().trim() == "0" || parseFloat(txtQuantity.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter Quantity")
        txtQuantity.focus();
        return false;
    }

    if (txtDiscountPerc.val().trim() != "" && (parseFloat(txtDiscountPerc.val().trim()) < 0 || parseFloat(txtDiscountPerc.val().trim()) > 100)) {
        ShowModel("Alert", "Please enter correct discount %")
        txtDiscountPerc.focus();
        return false;
    }


    if (txtTotalPrice.val().trim() == "" || txtTotalPrice.val().trim() == "0" || parseFloat(txtTotalPrice.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter correct Price and Quantity")
        txtQuantity.focus();
        return false;
    }

    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }
    var soProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var soProductDetailId = $row.find("#hdnSOProductDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var discountPerc = $row.find("#hdnDiscountPerc").val();
        var discountAmount = $row.find("#hdnDiscountAmount").val();
        var productTaxId = $row.find("#hdnProductTaxId").val();
        var productTaxPerc = $row.find("#hdnProductTaxPerc").val();
        var productTaxAmount = $row.find("#hdnProductTaxAmount").val();
        var productTaxName = $row.find("#hdnProductTaxName").val();
        var totalPrice = $row.find("#hdnTotalPrice").val();

        var productSurchargeName_1 = $row.find("#hdnProductSurchargeName_1").val();
        var productSurchargePercentage_1 = $row.find("#hdnProductSurchargePercentage_1").val();
        var productSurchargeAmount_1 = $row.find("#hdnProductSurchargeAmount_1").val();

        var productSurchargeName_2 = $row.find("#hdnProductSurchargeName_2").val();
        var productSurchargePercentage_2 = $row.find("#hdnProductSurchargePercentage_2").val();
        var productSurchargeAmount_2 = $row.find("#hdnProductSurchargeAmount_2").val();

        var productSurchargeName_3 = $row.find("#hdnProductSurchargeName_3").val();
        var productSurchargePercentage_3 = $row.find("#hdnProductSurchargePercentage_3").val();
        var productSurchargeAmount_3 = $row.find("#hdnProductSurchargeAmount_3").val();

        var cGSTPerc = $row.find("#hdnCGSTPerc").val();
        var cGSTPercAmount = $row.find("#hdnCGSTAmount").val();
        var sGSTPerc = $row.find("#hdnSGSTPerc").val();
        var sGSTPercAmount = $row.find("#hdnSGSTAmount").val();
        var iGSTPerc = $row.find("#hdnIGSTPerc").val();
        var iGSTPercAmount = $row.find("#hdnIGSTAmount").val();
        var hsn_Code = $row.find("#hdnHSN_Code").val();


        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var soProduct = {
                    SOProductDetailId: soProductDetailId,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Price: price,
                    Quantity: quantity,
                    DiscountPercentage: discountPerc,
                    DiscountAmount: discountAmount,
                    TaxId: productTaxId,
                    TaxName: productTaxName,
                    TaxPercentage: productTaxPerc,
                    TaxAmount: productTaxAmount,
                    SurchargeName_1: productSurchargeName_1,
                    SurchargePercentage_1: productSurchargePercentage_1,
                    SurchargeAmount_1: productSurchargeAmount_1,
                    SurchargeName_2: productSurchargeName_2,
                    SurchargePercentage_2: productSurchargePercentage_2,
                    SurchargeAmount_2: productSurchargeAmount_2,
                    SurchargeName_3: productSurchargeName_3,
                    SurchargePercentage_3: productSurchargePercentage_3,
                    SurchargeAmount_3: productSurchargeAmount_3,
                    CGST_Perc: cGSTPerc,
                    CGST_Amount: cGSTPercAmount,
                    SGST_Perc: sGSTPerc,
                    SGST_Amount: sGSTPercAmount,
                    IGST_Perc: iGSTPerc,
                    IGST_Amount: iGSTPercAmount,
                    HSN_Code: hsn_Code,
                    TotalPrice: totalPrice
                };
                soProductList.push(soProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnSOProductDetailId.val() == soProductDetailId && hdnSequenceNo.val() == sequenceNo) {
                var soProduct = {
                    SOProductDetailId: hdnSOProductDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Price: txtPrice.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    DiscountPercentage: txtDiscountPerc.val().trim(),
                    DiscountAmount: txtDiscountAmount.val().trim(),
                    TaxId: hdnProductTaxId.val().trim(),
                    TaxName: txtProductTaxName.val().trim(),
                    TaxPercentage: hdnProductTaxPerc.val().trim(),
                    TaxAmount: txtProductTaxAmount.val().trim(),

                    SurchargeName_1: hdnProductSurchargeName_1.val(),
                    SurchargePercentage_1: hdnProductSurchargePercentage_1.val(),
                    SurchargeAmount_1: txtProductSurchargeAmount_1.val(),
                    SurchargeName_2: hdnProductSurchargeName_2.val(),
                    SurchargePercentage_2: hdnProductSurchargePercentage_2.val(),
                    SurchargeAmount_2: txtProductSurchargeAmount_2.val(),
                    SurchargeName_3: hdnProductSurchargeName_3.val(),
                    SurchargePercentage_3: hdnProductSurchargePercentage_3.val(),
                    SurchargeAmount_3: txtProductSurchargeAmount_3.val(),
                    CGST_Perc: txtCGSTPerc.val(),
                    CGST_Amount: txtCGSTPercAmount.val(),
                    SGST_Perc: txtSGSTPerc.val(),
                    SGST_Amount: txtSGSTPercAmount.val(),
                    IGST_Perc: txtIGSTPerc.val(),
                    IGST_Amount: txtIGSTPercAmount.val(),
                    HSN_Code: txtHSN_Code.val(),
                    TotalPrice: txtTotalPrice.val().trim()
                };
                soProductList.push(soProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var soProductAddEdit = {
            SOProductDetailId: hdnSOProductDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Price: txtPrice.val().trim(),
            Quantity: txtQuantity.val().trim(),
            DiscountPercentage: txtDiscountPerc.val().trim(),
            DiscountAmount: txtDiscountAmount.val().trim(),
            TaxId: hdnProductTaxId.val().trim(),
            TaxName: txtProductTaxName.val().trim(),
            TaxPercentage: hdnProductTaxPerc.val().trim(),
            TaxAmount: txtProductTaxAmount.val().trim(),
            SurchargeName_1: hdnProductSurchargeName_1.val(),
            SurchargePercentage_1: hdnProductSurchargePercentage_1.val(),
            SurchargeAmount_1: txtProductSurchargeAmount_1.val(),
            SurchargeName_2: hdnProductSurchargeName_2.val(),
            SurchargePercentage_2: hdnProductSurchargePercentage_2.val(),
            SurchargeAmount_2: txtProductSurchargeAmount_2.val(),
            SurchargeName_3: hdnProductSurchargeName_3.val(),
            SurchargePercentage_3: hdnProductSurchargePercentage_3.val(),
            SurchargeAmount_3: txtProductSurchargeAmount_3.val(),
            CGST_Perc: txtCGSTPerc.val(),
            CGST_Amount: txtCGSTPercAmount.val(),
            SGST_Perc: txtSGSTPerc.val(),
            SGST_Amount: txtSGSTPercAmount.val(),
            IGST_Perc: txtIGSTPerc.val(),
            IGST_Amount: txtIGSTPercAmount.val(),
            HSN_Code: txtHSN_Code.val(),
            TotalPrice: txtTotalPrice.val().trim(),
        };
        soProductList.push(soProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetSOProductList(soProductList);
    }
}
function GetSOProductList(soProducts) {
    var hdnSOId = $("#hdnSOId");
    var requestData = { soProducts: soProducts, soId: hdnSOId.val() };
    $.ajax({
        url: "../SO/GetSOProductList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divProductList").html("");
            $("#divProductList").html(err);
        },
        success: function (data) {
            $("#divProductList").html("");
            $("#divProductList").html(data);
            CalculateGrossandNetValues();
            ShowHideProductPanel(2);
        }
    });
}

function CalculateTotalCharges() {
    var price = $("#txtPrice").val();
    var quantity = $("#txtQuantity").val();
    var discountPerc = $("#txtDiscountPerc").val();
    var productTaxPerc = $("#hdnProductTaxPerc").val();
    var productSurchargePerc_1 = $("#hdnProductSurchargePercentage_1").val();
    var productSurchargePerc_2 = $("#hdnProductSurchargePercentage_2").val();
    var productSurchargePerc_3 = $("#hdnProductSurchargePercentage_3").val();
    var CGSTPerc = $("#txtCGSTPerc").val();
    var SGSTPerc = $("#txtSGSTPerc").val();
    var IGSTPerc = $("#txtIGSTPerc").val();
    var discountAmount = 0;
    var taxAmount = 0;
    var CGSTAmount = 0;
    var SGSTAmount = 0;
    var IGSTAmount = 0;
    var productSurchargeAmount_1 = 0;
    var productSurchargeAmount_2 = 0;
    var productSurchargeAmount_3 = 0;
    var totalTaxAmount = 0;
    price = price == "" ? 0 : price;
    quantity = quantity == "" ? 0 : quantity;
    var totalPrice = parseFloat(price) * parseFloat(quantity);
    if (parseFloat(discountPerc) > 0) {
        discountAmount = (parseFloat(totalPrice) * parseFloat(discountPerc)) / 100
    }
    $("#txtDiscountAmount").val(discountAmount.toFixed(2));


    if (parseFloat(CGSTPerc) > 0) {

        CGSTAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(CGSTPerc)) / 100;
    }
    $("#txtCGSTPercAmount").val(CGSTAmount.toFixed(2));

    if (parseFloat(SGSTPerc) > 0) {

        SGSTAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(SGSTPerc)) / 100;
    }
    $("#txtSGSTPercAmount").val(SGSTAmount.toFixed(2));

    if (parseFloat(IGSTPerc) > 0) {

        IGSTAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(IGSTPerc)) / 100;
    }
    $("#txtIGSTPercAmount").val(IGSTAmount.toFixed(2));


    if (parseFloat(productTaxPerc) > 0) {
        taxAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(productTaxPerc)) / 100;
    }
    $("#txtProductTaxAmount").val(taxAmount.toFixed(2));

    if (parseFloat(taxAmount) > 0 && parseFloat(productSurchargePerc_1) > 0) {
        productSurchargeAmount_1 = (parseFloat(taxAmount) * parseFloat(productSurchargePerc_1)) / 100;
    }
    $("#txtProductSurchargeAmount_1").val(productSurchargeAmount_1.toFixed(2));

    if (parseFloat(taxAmount) > 0 && parseFloat(productSurchargePerc_2) > 0) {
        productSurchargeAmount_2 = (parseFloat(taxAmount) * parseFloat(productSurchargePerc_2)) / 100;
    }
    $("#txtProductSurchargeAmount_2").val(productSurchargeAmount_2.toFixed(2));

    if (parseFloat(taxAmount) > 0 && parseFloat(productSurchargePerc_3) > 0) {
        productSurchargeAmount_3 = (parseFloat(taxAmount) * parseFloat(productSurchargePerc_3)) / 100;
    }
    $("#txtProductSurchargeAmount_3").val(productSurchargeAmount_3.toFixed(2));
    totalTaxAmount = parseFloat(taxAmount) + parseFloat(productSurchargeAmount_1) + parseFloat(productSurchargeAmount_2) + parseFloat(productSurchargeAmount_3);
    $("#txtProductTotalTaxAmount").val(totalTaxAmount.toFixed(2));

    $("#txtTotalPrice").val((totalPrice - discountAmount + taxAmount + CGSTAmount + SGSTAmount + IGSTAmount + productSurchargeAmount_1 + productSurchargeAmount_2 + productSurchargeAmount_3).toFixed(2));


}
function CalculateGrossandNetValues() {
    var basicValue = 0;
    var taxValue = 0;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var siProductDetailId = $row.find("#hdnSOProductDetailId").val();
        var totalPrice = $row.find("#hdnTotalPrice").val();
        if (totalPrice != undefined) {
            basicValue += parseFloat(totalPrice);
        }

    });
    $('#tblTaxList tr').each(function (i, row) {
        var $row = $(row);
        var saleinvoiceTaxDetailId = $row.find("#hdnSITaxDetailId").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();
        var surchargePercentage_1 = $row.find("#hdnSurchargePercentage_1").val();
        var surchargePercentage_2 = $row.find("#hdnSurchargePercentage_2").val();
        var surchargePercentage_3 = $row.find("#hdnSurchargePercentage_3").val();

        var taxAmount = 0;
        var surchargeAmount_1 = 0;
        var surchargeAmount_2 = 0;
        var surchargeAmount_3 = 0;
        var totalTaxAmount = 0;

        if (taxPercentage != undefined) {

            if (parseFloat(basicValue) > 0) {
                taxAmount = (parseFloat(basicValue) * (parseFloat(taxPercentage) / 100));
                if (parseFloat(taxAmount) > 0 && parseFloat(surchargePercentage_1) > 0) {
                    surchargeAmount_1 = (parseFloat(taxAmount) * (parseFloat(surchargePercentage_1) / 100));
                }
                if (parseFloat(taxAmount) > 0 && parseFloat(surchargePercentage_2) > 0) {
                    surchargeAmount_2 = (parseFloat(taxAmount) * (parseFloat(surchargePercentage_2) / 100));
                }
                if (parseFloat(taxAmount) > 0 && parseFloat(surchargePercentage_3) > 0) {
                    surchargeAmount_3 = (parseFloat(taxAmount) * (parseFloat(surchargePercentage_3) / 100));
                }
                totalTaxAmount = parseFloat(taxAmount) + parseFloat(surchargeAmount_1) + parseFloat(surchargeAmount_2) + parseFloat(surchargeAmount_3);
                $row.find("#hdnTaxAmount").val(taxAmount.toFixed(2));
                $row.find("#hdnSurchargeAmount_1").val(surchargeAmount_1.toFixed(2));
                $row.find("#hdnSurchargeAmount_2").val(surchargeAmount_2.toFixed(2));
                $row.find("#hdnSurchargeAmount_3").val(surchargeAmount_3.toFixed(2));

                $row.find("#tdTaxAmount").html(totalTaxAmount.toFixed(2));
            }
            else {
                taxAmount = 0;
                $row.find("#hdnTaxAmount").val("0");
                $row.find("#hdnSurchargeAmount_1").val("0");
                $row.find("#hdnSurchargeAmount_2").val("0");
                $row.find("#hdnSurchargeAmount_3").val("0");

                $row.find("#tdTaxAmount").html("0");
            }
            taxValue += parseFloat(totalTaxAmount);
        }

    });
    var freightValue = $("#txtFreightValue").val() == "" ? "0" : $("#txtFreightValue").val();

    var freightCGST = $("#txtFreightCGST_Amt").val() == "" ? "0" : $("#txtFreightCGST_Amt").val();
    var freightSGST = $("#txtFreightSGST_Amt").val() == "" ? "0" : $("#txtFreightSGST_Amt").val();
    var freightIGST = $("#txtFreightIGST_Amt").val() == "" ? "0" : $("#txtFreightIGST_Amt").val();

    var loadingValue = $("#txtLoadingValue").val() == "" ? "0" : $("#txtLoadingValue").val();

    var loadingCGST = $("#txtLoadingCGST_Amt").val() == "" ? "0" : $("#txtLoadingCGST_Amt").val();
    var loadingSGST = $("#txtLoadingSGST_Amt").val() == "" ? "0" : $("#txtLoadingSGST_Amt").val();
    var loadingIGST = $("#txtLoadingIGST_Amt").val() == "" ? "0" : $("#txtLoadingIGST_Amt").val();

    var insuranceValue = $("#txtInsuranceValue").val() == "" ? "0" : $("#txtInsuranceValue").val();
    var insuranceCGST = $("#txtInsuranceCGST_Amt").val() == "" ? "0" : $("#txtInsuranceCGST_Amt").val();
    var insuranceSGST = $("#txtInsuranceSGST_Amt").val() == "" ? "0" : $("#txtInsuranceSGST_Amt").val();
    var insuranceIGST = $("#txtInsuranceIGST_Amt").val() == "" ? "0" : $("#txtInsuranceIGST_Amt").val();

    if (parseFloat(freightValue) <= 0) {
        freightValue = 0;
    }
    if (parseFloat(loadingValue) <= 0) {
        loadingValue = 0;
    }
    if (parseFloat(insuranceValue) <= 0) {
        insuranceValue = 0;
    }
    if (parseFloat(freightCGST) <= 0) {
        freightCGST = 0;
    }
    if (parseFloat(freightSGST) <= 0) {
        freightSGST = 0;
    }
    if (parseFloat(freightIGST) <= 0) {
        freightIGST = 0;
    }

    if (parseFloat(loadingCGST) <= 0) {
        loadingCGST = 0;
    }
    if (parseFloat(loadingSGST) <= 0) {
        loadingSGST = 0;
    }
    if (parseFloat(loadingIGST) <= 0) {
        loadingIGST = 0;
    }

    if (parseFloat(insuranceCGST) <= 0) {
        insuranceCGST = 0;
    }
    if (parseFloat(insuranceSGST) <= 0) {
        insuranceSGST = 0;
    }
    if (parseFloat(insuranceIGST) <= 0) {
        insuranceIGST = 0;
    }
    $("#txtBasicValue").val(basicValue);
    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(taxValue) + parseFloat(freightValue) + parseFloat(loadingValue) + parseFloat(insuranceValue) + parseFloat(freightCGST) + parseFloat(freightSGST) + parseFloat(freightIGST) + parseFloat(loadingCGST) + parseFloat(loadingSGST) + parseFloat(loadingIGST) + parseFloat(insuranceCGST) + parseFloat(insuranceSGST) + parseFloat(insuranceIGST)).toFixed(0));
}
function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var soProductDetailId = $(row).find("#hdnSOProductDetailId").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var price = $(row).find("#hdnPrice").val();
    var quantity = $(row).find("#hdnQuantity").val();
    var discountPerc = $(row).find("#hdnDiscountPerc").val();
    var discountAmount = $(row).find("#hdnDiscountAmount").val();
    var productTaxId = $(row).find("#hdnProductTaxId").val();
    var productTaxPerc = $(row).find("#hdnProductTaxPerc").val();
    var productTaxAmount = $(row).find("#hdnProductTaxAmount").val();
    var productTaxName = $(row).find("#hdnProductTaxName").val();
    var totalPrice = $(row).find("#hdnTotalPrice").val();


    var productSurchargeName_1 = $(row).find("#hdnProductSurchargeName_1").val();
    var productSurchargePercentage_1 = $(row).find("#hdnProductSurchargePercentage_1").val();
    var productSurchargeAmount_1 = $(row).find("#hdnProductSurchargeAmount_1").val();
    var productSurchargeName_2 = $(row).find("#hdnProductSurchargeName_2").val();
    var productSurchargePercentage_2 = $(row).find("#hdnProductSurchargePercentage_2").val();
    var productSurchargeAmount_2 = $(row).find("#hdnProductSurchargeAmount_2").val();
    var productSurchargeName_3 = $(row).find("#hdnProductSurchargeName_3").val();
    var productSurchargePercentage_3 = $(row).find("#hdnProductSurchargePercentage_3").val();
    var productSurchargeAmount_3 = $(row).find("#hdnProductSurchargeAmount_3").val();

    var cGSTPerc = $(row).find("#hdnCGSTPerc").val();
    var cGSTAmount = $(row).find("#hdnCGSTAmount").val();
    var sGSTPerc = $(row).find("#hdnSGSTPerc").val();
    var sGSTAmount = $(row).find("#hdnSGSTAmount").val();
    var iGSTPerc = $(row).find("#hdnIGSTPerc").val();
    var iGSTAmount = $(row).find("#hdnIGSTAmount").val();
    var hsn_Code = $(row).find("#hdnHSN_Code").val();


    $("#txtProductName").val(productName);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnSOProductDetailId").val(soProductDetailId);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);
    $("#txtPrice").val(price);
    $("#txtQuantity").val(quantity);
    $("#txtDiscountPerc").val(discountPerc);
    $("#txtDiscountAmount").val(discountAmount);
    $("#txtProductTaxName").val(productTaxName);
    $("#hdnProductTaxId").val(productTaxId);
    $("#hdnProductTaxPerc").val(productTaxPerc);
    $("#txtProductTaxAmount").val(productTaxAmount);
    $("#txtTotalPrice").val(totalPrice);

    $("#hdnProductSurchargeName_1").val(productSurchargeName_1);
    $("#hdnProductSurchargePercentage_1").val(productSurchargePercentage_1);
    $("#txtProductSurchargeAmount_1").val(productSurchargeAmount_1);

    $("#hdnProductSurchargeName_2").val(productSurchargeName_2);
    $("#hdnProductSurchargePercentage_2").val(productSurchargePercentage_2);
    $("#txtProductSurchargeAmount_2").val(productSurchargeAmount_2);

    $("#hdnProductSurchargeName_3").val(productSurchargeName_3);
    $("#hdnProductSurchargePercentage_3").val(productSurchargePercentage_3);
    $("#txtProductSurchargeAmount_3").val(productSurchargeAmount_3);

    $("#txtProductTotalTaxAmount").val(parseFloat(productTaxAmount) + parseFloat(productSurchargeAmount_1) + parseFloat(productSurchargeAmount_2) + parseFloat(productSurchargeAmount_3));

    $("#txtCGSTPerc").val(cGSTPerc);
    $("#txtCGSTPercAmount").val(cGSTAmount);
    $("#txtSGSTPerc").val(sGSTPerc);
    $("#txtSGSTPercAmount").val(sGSTAmount);
    $("#txtIGSTPerc").val(iGSTPerc);
    $("#txtIGSTPercAmount").val(iGSTAmount);
    $("#txtHSN_Code").val(hsn_Code);


    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    GetProductAvailableStock(productId);
    ShowHideProductPanel(1);
}
function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var soProductDetailId = $(row).find("#hdnSOProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
    }
}
function GetSODetail(soId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SO/GetSODetail",
        data: { soId: soId },
        dataType: "json",
        success: function (data) {
            $("#txtSONo").val(data.SONo);
            $("#txtSODate").val(data.SODate);
            $("#txtQuotationNo").val(data.QuotationNo);
            $("#hdnQuotationId").val(data.QuotationId);
            $("#txtQuotationDate").val(data.QuotationDate);

            $("#ddlCompanyBranch").val(data.CompanyBranchId);

            $("#ddlCurrency").val(data.CurrencyCode);

            $("#hdnCustomerId").val(data.CustomerId);
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtCustomerName").val(data.CustomerName);

            $("#hdnConsigneeId").val(data.ConsigneeId);
            $("#txtConsigneeCode").val(data.ConsigneeCode);
            $("#txtConsigneeName").val(data.ConsigneeName);

            $("#ddlApprovalStatus").val(data.ApprovalStatus);
            if (data.ApprovalStatus == "Final") {
                $("#btnUpdate").hide();
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);
                $("#chkstatus").attr('disabled', true);
                $(".editonly").hide();
                $("#btnReCalculate").hide();
                $("#chkReverseCharge").attr('disabled', true);
                if ($(".editonly").hide()) {
                    $('#lblQuotationDate').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
                }
                $("#txtCancelReason").attr('readOnly', false);
            }
            if (data.ReverseChargeApplicable == true) {
                $("#chkReverseCharge").prop("checked", true);
                $(".revserseSection").show();
                $("#txtRevserseAmount").val(data.ReverseChargeAmount);
            }

            $("#txtBContactPerson").val(data.ContactPerson);
            $("#txtBAddress").val(data.BillingAddress);
            $("#txtBCity").val(data.City);
            $("#ddlBCountry").val(data.CountryId);
            $("#ddlBState").val(data.StateId);
            $("#txtBPinCode").val(data.PinCode);
            $("#txtBTINNo").val(data.TINNo);

            $("#txtBEmail").val(data.Email);
            $("#txtBMobileNo").val(data.MobileNo);
            $("#txtBContactNo").val(data.ContactNo);
            $("#txtBFax").val(data.Fax);

            $("#txtSContactPerson").val(data.ShippingContactPerson);
            $("#txtSAddress").val(data.ShippingBillingAddress);
            $("#txtSCity").val(data.ShippingCity);
            $("#ddlSCountry").val(data.ShippingCountryId);
            $("#ddlSState").val(data.ShippingStateId);
            $("#txtSPinCode").val(data.ShippingPinCode);
            $("#txtSTINNo").val(data.ShippingTINNo);

            $("#txtSEmail").val(data.ShippingEmail);
            $("#txtSMobileNo").val(data.ShippingMobileNo);
            $("#txtSContactNo").val(data.ShippingContactNo);
            $("#txtSFax").val(data.ShippingFax);

            $("#txtRevserseAmount").val(data.RevserseChargeAmount);
            $("#txtRefNo").val(data.RefNo);
            $("#txtRefDate").val(data.RefDate);

            $("#txtPayToBookName").val(data.PayToBookName);
            $("#hdnPayToBookId").val(data.PayToBookId);
            $("#txtPayToBookBranch").val(data.PayToBookBranch);

            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);


            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);


            $("#txtBasicValue").val(data.BasicValue);
            $("#txtTotalValue").val(data.TotalValue);
            $("#txtFreightValue").val(data.FreightValue);
            $("#txtFreightCGST_Amt").val(data.FreightCGST_Amt);
            $("#txtFreightSGST_Amt").val(data.FreightSGST_Amt);
            $("#txtFreightIGST_Amt").val(data.FreightIGST_Amt);
            $("#hdnFreightCGST_Perc").val(data.FreightCGST_Perc)
            $("#hdnFreightSGST_Perc").val(data.FreightSGST_Perc)
            $("#hdnFreightIGST_Perc").val(data.FreightIGST_Perc)

            $("#txtLoadingValue").val(data.LoadingValue);
            $("#txtLoadingCGST_Amt").val(data.LoadingCGST_Amt);
            $("#txtLoadingSGST_Amt").val(data.LoadingSGST_Amt);
            $("#txtLoadingIGST_Amt").val(data.LoadingIGST_Amt);

            $("#hdnLoadingCGST_Perc").val(data.LoadingCGST_Perc);
            $("#hdnLoadingSGST_Perc").val(data.LoadingSGST_Perc);
            $("#hdnLoadingIGST_Perc").val(data.LoadingIGST_Perc);

            $("#txtInsuranceValue").val(data.InsuranceValue);
            $("#txtInsuranceCGST_Amt").val(data.InsuranceCGST_Amt)
            $("#txtInsuranceSGST_Amt").val(data.InsuranceSGST_Amt)
            $("#txtInsuranceIGST_Amt").val(data.InsuranceIGST_Amt)

            $("#hdnInsuranceCGST_Perc").val(data.InsuranceCGST_Perc)
            $("#hdnInsuranceSGST_Perc").val(data.InsuranceSGST_Perc)
            $("#hdnInsuranceIGST_Perc").val(data.InsuranceIGST_Perc)
            GetBranchStateId();

            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }


            $("#btnAddNew").show();
            $("#btnPrint").show();
            $("#btnEmail").show();
            $("#btnReset").hide();


        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function CancelSO() {
    var hdnSOId = $("#hdnSOId");
    var txtSONo = $("#txtSONo");
    var txtCancelReason = $("#txtCancelReason");
    var requestData = {
        soId: hdnSOId.val(),
        sONo: txtSONo.val().trim(),
        cancelReason: txtCancelReason.val().trim()
    };
    $.ajax({
        url: "../SO/CancelSO",
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
                       window.location.href = "../SO/ListSO";
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
function ShowHideRerevserseSection() {
    if ($("#chkReverseCharge").is(':checked')) {
        $(".revserseSection").show();
    }
    else {
        $(".revserseSection").hide();
    }
}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields() {

    $("#txtSONo").val("");
    $("#hdnSOId").val("0");
    $("#txtSODate").val($("#hdnCurrentDate").val());
    $("#hdnCustomerId").val("0");
    $("#txtCustomerName").val("");
    $("#txtCustomerCode").val("");
    $("#txtQuotationNo").val("");
    $("#txtQuotationDate").val("");
    $("#txtQuotationId").val("0");

    $("#ddlBCustomerBranch").val("0");
    $("#txtBContactPerson").val("");
    $("#txtBAddress").val("");
    $("#txtBCity").val("");
    $("#ddlBCountry").val("0");
    $("#ddlBState").val("0");
    $("#txtBPinCode").val("");
    $("#txtBTINNo").val("");
    $("#txtBEmail").val("");
    $("#txtBMobileNo").val("");
    $("#txtBEmail").val("");
    $("#txtBFax").val("");
    $("#ddlApprovalStatus").val("Draft");
    $("#ddlSCustomerBranch").val("0");
    $("#txtSContactPerson").val("");
    $("#txtSAddress").val("");
    $("#txtSCity").val("");
    $("#ddlSCountry").val("0");
    $("#ddlSState").val("0");
    $("#txtSPinCode").val("");
    $("#txtSTINNo").val("");
    $("#txtSEmail").val("");
    $("#txtSMobileNo").val("");
    $("#txtSEmail").val("");
    $("#txtSFax").val("");


    $("#txtRefNo").val("");
    $("#txtRefDate").val("");
    $("#hdnPayToBookId").val("0");
    $("#txtPayToBookName").val("");
    $("#txtPayToBookBranch").val("");
    $("#txtBasicValue").val("");
    $("#txtFreightValue").val("");
    $("#txtLoadingValue").val("");
    $("#txtTotalValue").val("");
    $("#txtRemarks1").val("");
    $("#txtRemarks2").val("");

    $("#btnSave").show();
    $("#btnUpdate").hide();


}
function GetCustomerDetail(customerId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Customer/GetCustomerDetail",
        data: { customerId: customerId },
        dataType: "json",
        success: function (data) {
            $("#txtBContactPerson").val(data.ContactPersonName);
            $("#txtBAddress").val(data.PrimaryAddress);
            $("#txtBCity").val(data.City);
            $("#ddlBCountry").val(data.CountryId);
            $("#ddlBState").val(data.StateId);
            $("#txtBPinCode").val(data.PinCode);
            $("#txtBTINNo").val(data.TINNo);
            $("#txtBEmail").val(data.Email);
            $("#txtBMobileNo").val(data.MobileNo);
            $("#txtBContactNo").val(data.ContactNo);
            $("#txtBFax").val(data.Fax);

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    BindCustomerBranchList(customerId)
}
function GetProductDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productid: productId },
        dataType: "json",
        success: function (data) {
            $("#txtQuantity").val("0");
            $("#txtPrice").val(data.SalePrice);
            $("#txtUOMName").val(data.UOMName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    GetProductAvailableStock(productId);


}
function GetProductAvailableStock(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductAvailableStock",
        data: { productid: productId, companyBranchId: 0, trnId: 0, trnType: "SO" },
        dataType: "json",
        success: function (data) {
            $("#txtAvailableStock").val(data);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}
function validateStateSelection(action) {
    var hdnCustomerStateId = $("#ddlSState");
    var hdnBillingStateId = $("#hdnBillingStateId");

    if (hdnCustomerStateId.val() == "0") {
        ShowModel("Alert", "Please Select Consignee")
        return false;
    }
    else if (hdnBillingStateId.val() == "0") {
        ShowModel("Alert", "Please Select Billing Location")
        return false;
    }
    ShowHideProductPanel(action);
}
function ShowHideProductPanel(action) {
    if (action == 1) {
        $(".productsection").show();
    }
    else {
        $(".productsection").hide();
        $("#txtProductName").val("");
        $("#hdnProductId").val("0");
        $("#hdnSOProductDetailId").val("0");
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtPrice").val("");
        $("#txtAvailableStock").val("");
        $("#txtUOMName").val("");
        $("#txtQuantity").val("");
        $("#txtDiscountPerc").val("");
        $("#txtDiscountAmount").val("");
        $("#txtProductTaxName").val("");
        $("#hdnProductTaxId").val("");
        $("#hdnProductTaxPerc").val("");
        $("#txtProductTaxAmount").val("");
        $("#txtTotalPrice").val("");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();


    }
}
function ShowHideTaxPanel(action) {
    if (action == 1) {
        $(".taxsection").show();
    }
    else {
        $(".taxsection").hide();
        $("#txtTaxName").val("");
        $("#hdnTaxId").val("0");
        $("#hdnSOTaxDetailId").val("0");
        $("#txtTaxPercentage").val("");
        $("#txtTaxAmount").val("");
        $("#btnAddTax").show();
        $("#btnUpdateTax").hide();
    }
}
function ShowHideTermPanel(action) {
    if (action == 1) {
        $(".termsection").show();
    }
    else {
        $(".termsection").hide();
        $("#txtTermDesc").val("");
        $("#hdnSOTermDetailId").val("0");
        $("#hdnTermSequence").val("0");
        $("#btnAddTerm").show();
        $("#btnUpdateTerm").hide();
    }
}
function OpenQuotationSearchPopup() {
    $("#SearchQuotationModel").modal();

}
function SearchQuotation() {
    var txtQuotationNo = $("#txtSearchQuotationNo");
    var txtCustomerName = $("#txtSearchCustomerName");

    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { quotationNo: txtQuotationNo.val().trim(), customerName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../SO/GetQuotationList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divQuotationList").html("");
            $("#divQuotationList").html(err);
        },
        success: function (data) {
            $("#divQuotationList").html("");
            $("#divQuotationList").html(data);
        }
    });
}
function SelectQuotation(quotationId, quotationNo, quotationDate, customerId, customerCode, customerName) {
    $("#txtQuotationNo").val(quotationNo);
    $("#hdnQuotationId").val(quotationId);
    $("#txtQuotationDate").val(quotationDate);
    $("#hdnCustomerId").val(customerId);
    $("#txtCustomerCode").val(customerCode);
    $("#txtCustomerName").val(customerName);

    $("#hdnConsigneeId").val(customerId);
    $("#txtConsigneeName").val(customerName);
    $("#txtConsigneeCode").val(customerCode);
    $("#SearchQuotationModel").modal('hide');
    GetCustomerDetail(customerId);
    GetConsigneeDetail(customerId);
    GetQuotationDetail(quotationId);

}

function GetQuotationDetail(quotationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Quotation/GetQuotationDetail",
        data: { quotationId: quotationId },
        dataType: "json",
        success: function (data) {
            //$("#txtQuotationNo").val(data.QuotationNo);
            //$("#txtQuotationDate").val(data.QuotationDate);
            //$("#hdnCustomerId").val(data.CustomerId);
            //$("#txtCustomerCode").val(data.CustomerCode);
            //$("#txtCustomerName").val(data.CustomerName);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            if (data.CurrencyCode != null) {
                $("#ddlCurrency").val(data.CurrencyCode);
            }
            else {
                $("#ddlCurrency").val("INR");
            }
            $("#txtAddress").val(data.BillingAddress);
            // $("#ddlApprovalStatus").val(data.ApprovalStatus);
            $("#txtPinCode").val(data.PinCode);
            $("#txtCSTNo").val(data.CSTNo);
            $("#txtTINNo").val(data.TINNo);
            $("#txtPANNo").val(data.PANNo);
            $("#txtRefNo").val(data.RefNo);
            $("#txtRefDate").val(data.RefDate);
            $("#txtBasicValue").val(data.BasicValue);
            $("#txtTotalValue").val(data.TotalValue);

            $("#txtTotalValue").val(data.TotalValue);
            $("#txtFreightValue").val(data.FreightValue);
            $("#txtFreightCGST_Amt").val(data.FreightCGST_Amt);
            $("#txtFreightSGST_Amt").val(data.FreightSGST_Amt);
            $("#txtFreightIGST_Amt").val(data.FreightIGST_Amt);
            $("#hdnFreightCGST_Perc").val(data.FreightCGST_Perc)
            $("#hdnFreightSGST_Perc").val(data.FreightSGST_Perc)
            $("#hdnFreightIGST_Perc").val(data.FreightIGST_Perc)

            $("#txtLoadingValue").val(data.LoadingValue);
            $("#txtLoadingCGST_Amt").val(data.LoadingCGST_Amt);
            $("#txtLoadingSGST_Amt").val(data.LoadingSGST_Amt);
            $("#txtLoadingIGST_Amt").val(data.LoadingIGST_Amt);

            $("#hdnLoadingCGST_Perc").val(data.LoadingCGST_Perc);
            $("#hdnLoadingSGST_Perc").val(data.LoadingSGST_Perc);
            $("#hdnLoadingIGST_Perc").val(data.LoadingIGST_Perc);

            $("#txtInsuranceValue").val(data.InsuranceValue);
            $("#txtInsuranceCGST_Amt").val(data.InsuranceCGST_Amt)
            $("#txtInsuranceSGST_Amt").val(data.InsuranceSGST_Amt)
            $("#txtInsuranceIGST_Amt").val(data.InsuranceIGST_Amt)

            $("#hdnInsuranceCGST_Perc").val(data.InsuranceCGST_Perc)
            $("#hdnInsuranceSGST_Perc").val(data.InsuranceSGST_Perc)
            $("#hdnInsuranceIGST_Perc").val(data.InsuranceIGST_Perc)

            if (data.ReverseChargeApplicable == true) {
                $("#chkReverseChargeApplicable").prop("checked", true);
                ShowHideRerevserseSection();
            }
            $("#txtReverseChargeAmount").val(data.ReverseChargeAmount);

            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);

            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            GetBranchStateId();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

    var quotationProducts = [];
    GetQuotationProductList(quotationProducts, quotationId);
    var quotationTaxes = [];
    GetQuotationTaxList(quotationTaxes, quotationId);
}
function GetQuotationProductList(quotationProducts) {
    var hdnQuotationId = $("#hdnQuotationId");
    var requestData = { quotationProducts: quotationProducts, quotationId: hdnQuotationId.val() };
    $.ajax({
        url: "../SO/GetSOQuotationProductList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divProductList").html("");
            $("#divProductList").html(err);
        },
        success: function (data) {
            $("#divProductList").html("");
            $("#divProductList").html(data);
            CalculateGrossandNetValues();
            ShowHideProductPanel(2);
        }
    });
}
function GetQuotationTaxList(quotationTaxes, quotationId) {

    var requestData = { quotationTaxes: quotationTaxes, quotationId: quotationId };
    $.ajax({
        url: "../Quotation/GetQuotationTaxList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divTaxList").html("");
            $("#divTaxList").html(err);
        },
        success: function (data) {
            $("#divTaxList").html("");
            $("#divTaxList").html(data);
            CalculateGrossandNetValues();
            ShowHideTaxPanel(2);
        }
    });
}
function GetQuotationTermList(quotationTerms, quotationId) {

    var requestData = { quotationTerms: quotationTerms, quotationId: quotationId };
    $.ajax({
        url: "../Quotation/GetQuotationTermList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divTermList").html("");
            $("#divTermList").html(err);
        },
        success: function (data) {
            $("#divTermList").html("");
            $("#divTermList").html(data);
            ShowHideTermPanel(2);
        }
    });
}
function CopyCurrentAddress() {
    if ($("#chkSamePermanentAddress").is(':checked')) {
        if ($("#txtBContactPerson").val().trim() != "") {
            $("#txtSContactPerson").val($("#txtBContactPerson").val().trim());
        }
        if ($("#txtBAddress").val().trim() != "") {
            $("#txtSAddress").val($("#txtBAddress").val().trim());
        }
        if ($("#txtBCity").val().trim() != "") {
            $("#txtSCity").val($("#txtBCity").val().trim());
        }
        if ($("#ddlBCountry").val() != "" && $("#ddlBCountry").val() != "0") {
            $("#ddlSCountry").val($("#ddlBCountry").val());
        }
        if ($("#ddlBState").val() != "" && $("#ddlBState").val() != "0") {
            $("#ddlSState").val($("#ddlBState").val());
        }
        if ($("#txtBPinCode").val().trim() != "") {
            $("#txtSPinCode").val($("#txtBPinCode").val().trim());
        }
        if ($("#txtBTINNo").val().trim() != "") {
            $("#txtSTINNo").val($("#txtBTINNo").val().trim());
        }
        if ($("#txtBEmail").val().trim() != "") {
            $("#txtSEmail").val($("#txtSEmail").val().trim());
        }
        if ($("#txtBMobileNo").val().trim() != "") {
            $("#txtSMobileNo").val($("#txtBMobileNo").val().trim());
        }
        if ($("#txtBContactNo").val().trim() != "") {
            $("#txtSContactNo").val($("#txtBContactNo").val().trim());
        }
        if ($("#txtBFax").val().trim() != "") {
            $("#txtSFax").val($("#txtBFax").val().trim());
        }
    }
    else {
        $("#txtSContactPerson").val("");
        $("#txtSAddress").val("");
        $("#txtSCity").val("");
        $("#ddlSCountry").val("0");
        $("#ddlSState").val("0");
        $("#txtSPinCode").val("");
        $("#txtSTINNo").val("");
        $("#txtSEmail").val("");
        $("#txtSMobileNo").val("");
        $("#txtSContactNo").val("");
        $("#txtSFax").val("");
    }

}
function FillShippingAddress() {

    var customerBranchId = $("#ddlSCustomerBranch option:selected").val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SO/GetCustomerBranchDetail",
        data: {
            customerBranchId: customerBranchId
        },
        dataType: "json",
        success: function (data) {

            $("#txtSContactPerson").val(data.ContactPersonName);
            $("#txtSAddress").val(data.PrimaryAddress);
            $("#txtSCity").val(data.City);
            $("#ddlSCountry").val(data.CountryId);
            $("#ddlSState").val(data.StateId);
            $("#txtSPinCode").val(data.PinCode);
            $("#txtSTINNo").val(data.TINNo);
            $("#txtSEmail").val(data.Email);
            $("#txtSMobileNo").val(data.MobileNo);
            $("#txtSContactNo").val(data.ContactNo);
            $("#txtSFax").val(data.Fax);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}
function FillBillingAddress() {

    var customerBranchId = $("#ddlBCustomerBranch option:selected").val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SO/GetCustomerBranchDetail",
        data: {
            customerBranchId: customerBranchId
        },
        dataType: "json",
        success: function (data) {

            $("#txtBContactPerson").val(data.ContactPersonName);
            $("#txtBAddress").val(data.PrimaryAddress);
            $("#txtBCity").val(data.City);
            $("#ddlBCountry").val(data.CountryId);
            $("#ddlBState").val(data.StateId);
            $("#txtBPinCode").val(data.PinCode);
            $("#txtBTINNo").val(data.TINNo);
            $("#txtBEmail").val(data.Email);
            $("#txtBMobileNo").val(data.MobileNo);
            $("#txtBContactNo").val(data.ContactNo);
            $("#txtBFax").val(data.Fax);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}
function SendMail() {
    var hdnSOId = $("#hdnSOId");
    var requestData = { soId: hdnSOId.val(), reportType: "PDF" };
    $.ajax({
        url: "../SO/SOMail",
        cache: false,
        type: "GET",
        dataType: "json",
        data: requestData,
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
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
function CalculateFreightTotalCharges() {
    var hdnBillingStateId = $("#hdnBillingStateId").val();
    var ddlSState = $("#ddlSState").val();

    if (ddlSState == "0" || ddlSState == "") {
        ShowModel("Alert", "Please Select Consignee ")
        $("#txtFreightValue").val(0);
        return false;
    }
    else if (hdnBillingStateId == "" || hdnBillingStateId == "0") {
        ShowModel("Alert", "Please Select Billing Location");
        $("#txtFreightValue").val(0);
        return false;
    }


    var maxIGSTPerc = 0;
    var maxCGSTPerc = 0;
    var maxSGSTPerc = 0;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var cGSTPerc = $(row).find("#hdnCGSTPerc").val();
        var sGSTPerc = $(row).find("#hdnSGSTPerc").val();
        var iGSTPerc = $(row).find("#hdnIGSTPerc").val();
        if (cGSTPerc != undefined) {
            if (parseFloat(cGSTPerc) > parseFloat(maxCGSTPerc)) {
                maxCGSTPerc = parseFloat(cGSTPerc);
            }
        }
        if (sGSTPerc != undefined) {
            if (parseFloat(sGSTPerc) > parseFloat(maxSGSTPerc)) {
                maxSGSTPerc = parseFloat(sGSTPerc);
            }
        }
        if (iGSTPerc != undefined) {
            if (parseFloat(iGSTPerc) > parseFloat(maxIGSTPerc)) {
                maxIGSTPerc = parseFloat(iGSTPerc);
            }
        }

    });

    //$("#hdnFreightCGST_Perc").val(data.FreightCGST_Perc)
    //$("#hdnFreightSGST_Perc").val(data.FreightSGST_Perc)
    //$("#hdnFreightIGST_Perc").val(data.FreightIGST_Perc)

    //var freightValue = $("#txtFreightValue").val();
    //var freightCGST_Perc = $("#hdnFreightCGST_Perc").val();
    //var freightSGST_Perc = $("#hdnFreightSGST_Perc").val();
    //var freightIGST_Perc = $("#hdnFreightIGST_Perc").val();

    var freightValue = $("#txtFreightValue").val();
    var freightCGST_Perc = maxCGSTPerc;
    var freightSGST_Perc = maxSGSTPerc;
    var freightIGST_Perc = maxIGSTPerc;



    freightValue = freightValue == "" ? 0 : freightValue;
    if (hdnBillingStateId == ddlSState) {
        freightCGST_Perc = freightCGST_Perc == "" ? 0 : freightCGST_Perc;
        freightSGST_Perc = freightSGST_Perc == "" ? 0 : freightSGST_Perc;
        freightIGST_Perc = 0;
        $("#hdnFreightCGST_Perc").val(freightCGST_Perc);
        $("#hdnFreightSGST_Perc").val(freightSGST_Perc);
        $("#hdnFreightIGST_Perc").val(freightIGST_Perc);

    }
    else {
        freightCGST_Perc = 0;
        freightSGST_Perc = 0;
        freightIGST_Perc = freightIGST_Perc == "" ? 0 : freightIGST_Perc;
        $("#hdnFreightCGST_Perc").val(freightCGST_Perc);
        $("#hdnFreightSGST_Perc").val(freightSGST_Perc);
        $("#hdnFreightIGST_Perc").val(freightIGST_Perc);
    }

    var freightCGST_Amount = 0;
    var freightSGST_Amount = 0;
    var freightIGST_Amount = 0;


    if (parseFloat(freightCGST_Perc) > 0) {
        freightCGST_Amount = (parseFloat(freightValue) * parseFloat(freightCGST_Perc)) / 100;
    }
    if (parseFloat(freightSGST_Perc) > 0) {
        freightSGST_Amount = (parseFloat(freightValue) * parseFloat(freightSGST_Perc)) / 100;
    }
    if (parseFloat(freightIGST_Perc) > 0) {
        freightIGST_Amount = (parseFloat(freightValue) * parseFloat(freightIGST_Perc)) / 100;
    }

    $("#txtFreightCGST_Amt").val(freightCGST_Amount.toFixed(2));
    $("#txtFreightSGST_Amt").val(freightSGST_Amount.toFixed(2));
    $("#txtFreightIGST_Amt").val(freightIGST_Amount.toFixed(2));
}
function CalculateLoadingTotalCharges() {
    var hdnBillingStateId = $("#hdnBillingStateId").val();
    var ddlSState = $("#ddlSState").val();

    if (ddlSState == "0" || ddlSState == "") {
        ShowModel("Alert", "Please Select Consignee ")
        $("#txtLoadingValue").val(0);
        return false;
    }
    else if (hdnBillingStateId == "" || hdnBillingStateId == "0") {
        ShowModel("Alert", "Please Select Billing Location");
        $("#txtLoadingValue").val(0);
        return false;
    }



    var maxIGSTPerc = 0;
    var maxCGSTPerc = 0;
    var maxSGSTPerc = 0;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var cGSTPerc = $(row).find("#hdnCGSTPerc").val();
        var sGSTPerc = $(row).find("#hdnSGSTPerc").val();
        var iGSTPerc = $(row).find("#hdnIGSTPerc").val();
        if (cGSTPerc != undefined) {
            if (parseFloat(cGSTPerc) > parseFloat(maxCGSTPerc)) {
                maxCGSTPerc = parseFloat(cGSTPerc);
            }
        }
        if (sGSTPerc != undefined) {
            if (parseFloat(sGSTPerc) > parseFloat(maxSGSTPerc)) {
                maxSGSTPerc = parseFloat(sGSTPerc);
            }
        }
        if (iGSTPerc != undefined) {
            if (parseFloat(iGSTPerc) > parseFloat(maxIGSTPerc)) {
                maxIGSTPerc = parseFloat(iGSTPerc);
            }
        }

    });
    //var loadingValue = $("#txtLoadingValue").val();
    //var loadingCGST_Perc = $("#hdnLoadingCGST_Perc").val();
    //var loadingSGST_Perc = $("#hdnLoadingSGST_Perc").val();
    //var loadingIGST_Perc = $("#hdnLoadingIGST_Perc").val();

    //$("#hdnLoadingCGST_Perc").val(data.LoadingCGST_Perc)
    //$("#hdnLoadingSGST_Perc").val(data.LoadingSGST_Perc)
    //$("#hdnLoadingIGST_Perc").val(data.LoadingIGST_Perc)

    var loadingValue = $("#txtLoadingValue").val();
    var loadingCGST_Perc = maxCGSTPerc;
    var loadingSGST_Perc = maxSGSTPerc;
    var loadingIGST_Perc = maxIGSTPerc;


    loadingValue = loadingValue == "" ? 0 : loadingValue;
    if (hdnBillingStateId == ddlSState) {
        loadingCGST_Perc = loadingCGST_Perc == "" ? 0 : loadingCGST_Perc;
        loadingSGST_Perc = loadingSGST_Perc == "" ? 0 : loadingSGST_Perc;
        loadingIGST_Perc = 0;
        $("#hdnLoadingCGST_Perc").val(loadingCGST_Perc);
        $("#hdnLoadingSGST_Perc").val(loadingSGST_Perc);
        $("#hdnLoadingIGST_Perc").val(loadingIGST_Perc);
    }
    else {
        loadingCGST_Perc = 0;
        loadingSGST_Perc = 0;
        loadingIGST_Perc = loadingIGST_Perc == "" ? 0 : loadingIGST_Perc;
        $("#hdnLoadingCGST_Perc").val(loadingCGST_Perc);
        $("#hdnLoadingSGST_Perc").val(loadingSGST_Perc);
        $("#hdnLoadingIGST_Perc").val(loadingIGST_Perc);
    }

    var loadingCGST_Amount = 0;
    var loadingSGST_Amount = 0;
    var loadingIGST_Amount = 0;


    if (parseFloat(loadingCGST_Perc) > 0) {
        loadingCGST_Amount = (parseFloat(loadingValue) * parseFloat(loadingCGST_Perc)) / 100;
    }
    if (parseFloat(loadingSGST_Perc) > 0) {
        loadingSGST_Amount = (parseFloat(loadingValue) * parseFloat(loadingSGST_Perc)) / 100;
    }
    if (parseFloat(loadingIGST_Perc) > 0) {
        loadingIGST_Amount = (parseFloat(loadingValue) * parseFloat(loadingIGST_Perc)) / 100;
    }

    $("#txtLoadingCGST_Amt").val(loadingCGST_Amount.toFixed(2));
    $("#txtLoadingSGST_Amt").val(loadingSGST_Amount.toFixed(2));
    $("#txtLoadingIGST_Amt").val(loadingIGST_Amount.toFixed(2));
}
function CalculateInsuranceTotalCharges() {
    var hdnBillingStateId = $("#hdnBillingStateId").val();
    var ddlSState = $("#ddlSState").val();

    if (ddlSState == "0" || ddlSState == "") {
        ShowModel("Alert", "Please Select Consignee ")
        $("#txtInsuranceValue").val(0);
        return false;
    }
    else if (hdnBillingStateId == "" || hdnBillingStateId == "0") {
        ShowModel("Alert", "Please Select Billing Location");
        $("#txtInsuranceValue").val(0);
        return false;
    }

    var maxIGSTPerc = 0;
    var maxCGSTPerc = 0;
    var maxSGSTPerc = 0;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var cGSTPerc = $(row).find("#hdnCGSTPerc").val();
        var sGSTPerc = $(row).find("#hdnSGSTPerc").val();
        var iGSTPerc = $(row).find("#hdnIGSTPerc").val();
        if (cGSTPerc != undefined) {
            if (parseFloat(cGSTPerc) > parseFloat(maxCGSTPerc)) {
                maxCGSTPerc = parseFloat(cGSTPerc);
            }
        }
        if (sGSTPerc != undefined) {
            if (parseFloat(sGSTPerc) > parseFloat(maxSGSTPerc)) {
                maxSGSTPerc = parseFloat(sGSTPerc);
            }
        }
        if (iGSTPerc != undefined) {
            if (parseFloat(iGSTPerc) > parseFloat(maxIGSTPerc)) {
                maxIGSTPerc = parseFloat(iGSTPerc);
            }
        }

    });


    //var insuranceValue = $("#txtInsuranceValue").val();
    //var insuranceCGST_Perc = $("#hdnInsuranceCGST_Perc").val();
    //var insuranceSGST_Perc = $("#hdnInsuranceSGST_Perc").val();
    //var insuranceIGST_Perc = $("#hdnInsuranceIGST_Perc").val();



    var insuranceValue = $("#txtInsuranceValue").val();
    var insuranceCGST_Perc = maxCGSTPerc;
    var insuranceSGST_Perc = maxSGSTPerc;
    var insuranceIGST_Perc = maxIGSTPerc;


    insuranceValue = insuranceValue == "" ? 0 : insuranceValue;
    if (hdnBillingStateId == ddlSState) {
        insuranceCGST_Perc = insuranceCGST_Perc == "" ? 0 : insuranceCGST_Perc;
        insuranceSGST_Perc = insuranceSGST_Perc == "" ? 0 : insuranceSGST_Perc;
        insuranceIGST_Perc = 0;
        $("#hdnInsuranceCGST_Perc").val(insuranceCGST_Perc);
        $("#hdnInsuranceSGST_Perc").val(insuranceSGST_Perc);
        $("#hdnInsuranceIGST_Perc").val(insuranceIGST_Perc);
    }
    else {
        insuranceCGST_Perc = 0;
        insuranceSGST_Perc = 0;
        insuranceIGST_Perc = insuranceIGST_Perc == "" ? 0 : insuranceIGST_Perc;
        $("#hdnInsuranceCGST_Perc").val(insuranceCGST_Perc);
        $("#hdnInsuranceSGST_Perc").val(insuranceSGST_Perc);
        $("#hdnInsuranceIGST_Perc").val(insuranceIGST_Perc);
    }

    var insuranceCGST_Amount = 0;
    var insuranceSGST_Amount = 0;
    var insuranceIGST_Amount = 0;


    if (parseFloat(insuranceCGST_Perc) > 0) {
        insuranceCGST_Amount = (parseFloat(insuranceValue) * parseFloat(insuranceCGST_Perc)) / 100;
    }
    if (parseFloat(insuranceSGST_Perc) > 0) {
        insuranceSGST_Amount = (parseFloat(insuranceValue) * parseFloat(insuranceSGST_Perc)) / 100;
    }
    if (parseFloat(insuranceIGST_Perc) > 0) {
        insuranceIGST_Amount = (parseFloat(insuranceValue) * parseFloat(insuranceIGST_Perc)) / 100;
    }

    $("#txtInsuranceCGST_Amt").val(insuranceCGST_Amount.toFixed(2));
    $("#txtInsuranceSGST_Amt").val(insuranceSGST_Amount.toFixed(2));
    $("#txtInsuranceIGST_Amt").val(insuranceIGST_Amount.toFixed(2));
}
function GetBranchStateId() {
    var companyBranchID = $("#ddlCompanyBranch option:selected").val();
    $.ajax({
        type: "GET",
        url: "../SO/GetCompanyBranchDetails",
        data: { companyBranchID: companyBranchID },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#hdnBillingStateId").val(data.StateId);
            //$("#hdnFreightCGST_Perc").val(data.FreightCGST_Perc)
            //$("#hdnFreightSGST_Perc").val(data.FreightSGST_Perc)
            //$("#hdnFreightIGST_Perc").val(data.FreightIGST_Perc)

            //$("#hdnLoadingCGST_Perc").val(data.LoadingCGST_Perc)
            //$("#hdnLoadingSGST_Perc").val(data.LoadingSGST_Perc)
            //$("#hdnLoadingIGST_Perc").val(data.LoadingIGST_Perc)

            //$("#hdnInsuranceCGST_Perc").val(data.InsuranceCGST_Perc)
            //$("#hdnInsuranceSGST_Perc").val(data.InsuranceSGST_Perc)
            //$("#hdnInsuranceIGST_Perc").val(data.InsuranceIGST_Perc)

        },
        error: function (Result) {
            $("#hdnBillingStateId").val(0);
        }

    });
}
function ShowHideProductModel() {
    $("#AddProductModel").modal();
}
function BindProductDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productid: productId },
        dataType: "json",
        success: function (data) {
            $("#txtProductName").val(data.ProductName);
            $("#hdnProductId").val(data.Productid);
            $("#txtProductShortDesc").val(data.ProductShortDesc);
            $("#txtProductCode").val(data.ProductCode);
            $("#txtHSN_Code").val(data.HSN_Code);
            var ddlSState = $("#ddlSState");
            var hdnBillingStateId = $("#ddlBState");
            if (ddlSState.val() == hdnBillingStateId.val()) {
                $("#txtCGSTPerc").val(data.CGST_Perc);
                $("#txtSGSTPerc").val(data.SGST_Perc);
                $("#txtIGSTPerc").val(0);
                $("#txtCGSTPercAmount").val(0);
                $("#txtSGSTPercAmount").val(0);
                $("#txtIGSTPercAmount").val(0);
            }
            else {
                $("#txtCGSTPerc").val(0);
                $("#txtSGSTPerc").val(0);
                $("#txtIGSTPerc").val(data.IGST_Perc);
                $("#txtCGSTPercAmount").val(0);
                $("#txtSGSTPercAmount").val(0);
                $("#txtIGSTPercAmount").val(0);
            }

            $("#txtPrice").val(data.PurchasePrice);
            $("#txtQuantity").val("");
            $("#txtUOMName").val(data.UOMName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

function SetGSTPercentageInProduct() {
    var hdnCustomerStateId = $("#ddlSState");
    var hdnBillingStateId = $("#hdnBillingStateId");

    //if (hdnCustomerStateId.val() == "0") {
    //    ShowModel("Alert", "Please Select Consignee")
    //    return false;
    //}
    //else if (hdnBillingStateId.val() == "0") {
    //    ShowModel("Alert", "Please Billing Location")
    //    return false;
    //}


    //$('#tblProductList tr').each(function (i, row) {
    //    var $row = $(row);
    //    var hdnProductId = $row.find("#hdnProductId").val();
    //    if (hdnProductId != undefined) {
    //        $.ajax({
    //            type: "GET",
    //            url: "../Quotation/GetProductTaxPercentage",
    //            data: { productId: hdnProductId },
    //            dataType: "json",
    //            asnc: false,
    //            success: function (data) {
    //                if (hdnCustomerStateId.val() == hdnBillingStateId.val()) {
    //                    $row.find("#hdnCGSTPerc").val(data.CGST_Perc);
    //                    $row.find("#hdnSGSTPerc").val(data.SGST_Perc);
    //                    $row.find("#hdnIGSTPerc").val(0);
    //                }
    //                else {
    //                    $row.find("#hdnCGSTPerc").val(0);
    //                    $row.find("#hdnSGSTPerc").val(0);
    //                    $row.find("#hdnIGSTPerc").val(data.IGST_Perc);
    //                }


    //            },
    //            error: function (Result) {
    //                $row.find("#hdnCGSTPerc").val(0);
    //                $row.find("#hdnSGSTPerc").val(0);
    //                $row.find("#hdnIGSTPerc").val(0);
    //            }

    //        });
    //    }
    //});
    setTimeout(
                     function () {
                         ReCalculateNetValues();
                     }, 1000);


}

function ReCalculateNetValues() {
    var basicValue = 0;
    var taxValue = 0;

    var hdnCustomerStateId = $("#ddlSState");
    var hdnBillingStateId = $("#hdnBillingStateId");

    //if (hdnCustomerStateId.val() == "0") {
    //    ShowModel("Alert", "Please Select Consignee")
    //    return false;
    //}
    //else if (hdnBillingStateId.val() == "0") {
    //    ShowModel("Alert", "Please Billing Location")
    //    return false;
    //}


    $('#tblProductList tr').each(function (i, row) {


        var $row = $(row);
        var siProductDetailId = $row.find("#hdnSIProductDetailId").val();
        var hdnProductId = $row.find("#hdnProductId").val();
        if (hdnProductId != undefined) {

            var quantity = $row.find("#hdnQuantity").val();
            var price = $row.find("#hdnPrice").val();
            var CGSTPerc = $row.find("#hdnCGSTPerc").val();
            var SGSTPerc = $row.find("#hdnSGSTPerc").val();
            var IGSTPerc = $row.find("#hdnIGSTPerc").val();
            var discountAmount = $row.find("#hdnDiscountAmount").val();

            var CGSTAmount = 0;
            var SGSTAmount = 0;
            var IGSTAmount = 0;

            price = price == "" ? 0 : price;
            quantity = quantity == "" ? 0 : quantity;
            var totalPrice = parseFloat(price) * parseFloat(quantity);


            if (hdnCustomerStateId.val() == hdnBillingStateId.val()) {

                if (parseFloat(CGSTPerc) > 0) {

                    CGSTAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(CGSTPerc)) / 100;
                }
                $row.find("#tdCGSTAmount").html(CGSTAmount.toFixed(2));
                $row.find("#tdCGST_Perc").html(CGSTPerc);
                $row.find("#hdnCGSTAmount").val(CGSTAmount.toFixed(2));
                $row.find("#hdnCGSTPerc").val(CGSTPerc);

                if (parseFloat(SGSTPerc) > 0) {
                    SGSTAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(SGSTPerc)) / 100;
                }

                $row.find("#tdSGSTAmount").html(SGSTAmount.toFixed(2));
                $row.find("#tdSGST_Perc").html(SGSTPerc);
                $row.find("#hdnSGSTAmount").val(SGSTAmount.toFixed(2));
                $row.find("#hdnSGSTPerc").val(SGSTPerc);
                $row.find("#tdIGSTAmount").html(0);
                $row.find("#tdIGST_Perc").html(0);
                $row.find("#hdnIGSTAmount").val(0);
                $row.find("#hdnIGSTPerc").val(0);

            }
            else {

                CGSTAmount = 0;
                $row.find("#tdCGSTAmount").html(0);
                $row.find("#tdCGST_Perc").html(0);
                $row.find("#hdnCGSTAmount").val(0);
                $row.find("#hdnCGSTPerc").val(0);
                SGSTAmount = 0;
                $row.find("#tdSGSTAmount").html(0);
                $row.find("#tdSGST_Perc").html(0);
                $row.find("#hdnSGSTAmount").val(0);
                $row.find("#hdnSGSTPerc").val(0);

                if (parseFloat(IGSTPerc) > 0) {
                    IGSTAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(IGSTPerc)) / 100;
                }

                $row.find("#tdIGSTAmount").html(IGSTAmount.toFixed(2));
                $row.find("#tdIGST_Perc").html(IGSTPerc);
                $row.find("#hdnIGSTAmount").val(IGSTAmount.toFixed(2));
                $row.find("#hdnIGSTPerc").val(IGSTPerc);



            }

            if (totalPrice != undefined) {
                var itemTotal = parseFloat(totalPrice - discountAmount + CGSTAmount + SGSTAmount + IGSTAmount);
                basicValue += itemTotal;
                $row.find("#tdTotalPrice").val(itemTotal.toFixed(2));
                $row.find("#hdnTotalPrice").val(itemTotal.toFixed(2));

            }
        }
    });

    $('#tblTaxList tr').each(function (i, row) {
        var $row = $(row);
        var saleinvoiceTaxDetailId = $row.find("#hdnSITaxDetailId").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();
        var surchargePercentage_1 = $row.find("#hdnSurchargePercentage_1").val();
        var surchargePercentage_2 = $row.find("#hdnSurchargePercentage_2").val();
        var surchargePercentage_3 = $row.find("#hdnSurchargePercentage_3").val();

        var taxAmount = 0;
        var surchargeAmount_1 = 0;
        var surchargeAmount_2 = 0;
        var surchargeAmount_3 = 0;
        var totalTaxAmount = 0;

        if (taxPercentage != undefined) {

            if (parseFloat(basicValue) > 0) {
                taxAmount = (parseFloat(basicValue) * (parseFloat(taxPercentage) / 100));
                if (parseFloat(taxAmount) > 0 && parseFloat(surchargePercentage_1) > 0) {
                    surchargeAmount_1 = (parseFloat(taxAmount) * (parseFloat(surchargePercentage_1) / 100));
                }
                if (parseFloat(taxAmount) > 0 && parseFloat(surchargePercentage_2) > 0) {
                    surchargeAmount_2 = (parseFloat(taxAmount) * (parseFloat(surchargePercentage_2) / 100));
                }
                if (parseFloat(taxAmount) > 0 && parseFloat(surchargePercentage_3) > 0) {
                    surchargeAmount_3 = (parseFloat(taxAmount) * (parseFloat(surchargePercentage_3) / 100));
                }
                totalTaxAmount = parseFloat(taxAmount) + parseFloat(surchargeAmount_1) + parseFloat(surchargeAmount_2) + parseFloat(surchargeAmount_3);
                $row.find("#hdnTaxAmount").val(taxAmount.toFixed(2));
                $row.find("#hdnSurchargeAmount_1").val(surchargeAmount_1.toFixed(2));
                $row.find("#hdnSurchargeAmount_2").val(surchargeAmount_2.toFixed(2));
                $row.find("#hdnSurchargeAmount_3").val(surchargeAmount_3.toFixed(2));

                $row.find("#tdTaxAmount").html(totalTaxAmount.toFixed(2));
            }
            else {
                taxAmount = 0;
                $row.find("#hdnTaxAmount").val("0");
                $row.find("#hdnSurchargeAmount_1").val("0");
                $row.find("#hdnSurchargeAmount_2").val("0");
                $row.find("#hdnSurchargeAmount_3").val("0");

                $row.find("#tdTaxAmount").html("0");
            }
            taxValue += parseFloat(totalTaxAmount);
        }

    });


    var freightValue = $("#txtFreightValue").val() == "" ? "0" : $("#txtFreightValue").val();

    var loadingValue = $("#txtLoadingValue").val() == "" ? "0" : $("#txtLoadingValue").val();

    var insuranceValue = $("#txtInsuranceValue").val() == "" ? "0" : $("#txtInsuranceValue").val();

    if (parseFloat(freightValue) <= 0) {
        freightValue = 0;
    }
    if (parseFloat(loadingValue) <= 0) {
        loadingValue = 0;
    }
    if (parseFloat(insuranceValue) <= 0) {
        insuranceValue = 0;
    }


    // Select MAX CGST,SGST,IGST AMount In Freight,Loading and Insurance IN Product Grid By Dheeraj

    var maxIGSTPerc = 0;
    var maxCGSTPerc = 0;
    var maxSGSTPerc = 0;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var cGSTPerc = $(row).find("#hdnCGSTPerc").val();
        var sGSTPerc = $(row).find("#hdnSGSTPerc").val();
        var iGSTPerc = $(row).find("#hdnIGSTPerc").val();
        if (cGSTPerc != undefined) {
            if (parseFloat(cGSTPerc) > parseFloat(maxCGSTPerc)) {
                maxCGSTPerc = parseFloat(cGSTPerc);
            }
        }
        if (sGSTPerc != undefined) {
            if (parseFloat(sGSTPerc) > parseFloat(maxSGSTPerc)) {
                maxSGSTPerc = parseFloat(sGSTPerc);
            }
        }
        if (iGSTPerc != undefined) {
            if (parseFloat(iGSTPerc) > parseFloat(maxIGSTPerc)) {
                maxIGSTPerc = parseFloat(iGSTPerc);
            }
        }

    });

    var loadingCGST_Perc = maxCGSTPerc;
    var loadingSGST_Perc = maxSGSTPerc;
    var loadingIGST_Perc = maxIGSTPerc;

    var freightCGST_Perc = maxCGSTPerc;
    var freightSGST_Perc = maxSGSTPerc;
    var freightIGST_Perc = maxIGSTPerc;

    var insuranceCGST_Perc = maxCGSTPerc;
    var insuranceSGST_Perc = maxSGSTPerc;
    var insuranceIGST_Perc = maxIGSTPerc;

    // End Code

    //var loadingValue = $("#txtLoadingValue").val();
    //var loadingCGST_Perc = $("#hdnLoadingCGST_Perc").val();
    //var loadingSGST_Perc = $("#hdnLoadingSGST_Perc").val();
    //var loadingIGST_Perc = $("#hdnLoadingIGST_Perc").val();
    //var freightValue = $("#txtFreightValue").val();
    //var freightCGST_Perc = $("#hdnFreightCGST_Perc").val();
    //var freightSGST_Perc = $("#hdnFreightSGST_Perc").val();
    //var freightIGST_Perc = $("#hdnFreightIGST_Perc").val();   

    //var insuranceValue = $("#txtInsuranceValue").val();
    //var insuranceCGST_Perc = $("#hdnInsuranceCGST_Perc").val();
    //var insuranceSGST_Perc = $("#hdnInsuranceSGST_Perc").val();
    //var insuranceIGST_Perc = $("#hdnInsuranceIGST_Perc").val();




    if (hdnCustomerStateId.val() == hdnBillingStateId.val()) {
        loadingCGST_Perc = loadingCGST_Perc == "" ? 0 : loadingCGST_Perc;
        loadingSGST_Perc = loadingSGST_Perc == "" ? 0 : loadingSGST_Perc;
        loadingIGST_Perc = 0;
        $("#hdnLoadingCGST_Perc").val(loadingCGST_Perc);
        $("#hdnLoadingSGST_Perc").val(loadingSGST_Perc);
        $("#hdnLoadingIGST_Perc").val(loadingIGST_Perc);

        freightCGST_Perc = freightCGST_Perc == "" ? 0 : freightCGST_Perc;
        freightSGST_Perc = freightSGST_Perc == "" ? 0 : freightSGST_Perc;
        freightIGST_Perc = 0;
        $("#hdnFreightCGST_Perc").val(freightCGST_Perc);
        $("#hdnFreightSGST_Perc").val(freightSGST_Perc);
        $("#hdnFreightIGST_Perc").val(freightIGST_Perc);

        insuranceCGST_Perc = insuranceCGST_Perc == "" ? 0 : insuranceCGST_Perc;
        insuranceSGST_Perc = insuranceSGST_Perc == "" ? 0 : insuranceSGST_Perc;
        insuranceIGST_Perc = 0;
        $("#hdnInsuranceCGST_Perc").val(insuranceCGST_Perc);
        $("#hdnInsuranceSGST_Perc").val(insuranceSGST_Perc);
        $("#hdnInsuranceIGST_Perc").val(insuranceIGST_Perc);


    }
    else {
        loadingCGST_Perc = 0;
        loadingSGST_Perc = 0;
        loadingIGST_Perc = loadingIGST_Perc == "" ? 0 : loadingIGST_Perc;
        $("#hdnLoadingCGST_Perc").val(loadingCGST_Perc);
        $("#hdnLoadingSGST_Perc").val(loadingSGST_Perc);
        $("#hdnLoadingIGST_Perc").val(loadingIGST_Perc);

        freightCGST_Perc = 0;
        freightSGST_Perc = 0;
        freightIGST_Perc = freightIGST_Perc == "" ? 0 : freightIGST_Perc;
        $("#hdnFreightCGST_Perc").val(freightCGST_Perc);
        $("#hdnFreightSGST_Perc").val(freightSGST_Perc);
        $("#hdnFreightIGST_Perc").val(freightIGST_Perc);

        insuranceCGST_Perc = 0;
        insuranceSGST_Perc = 0;
        insuranceIGST_Perc = insuranceIGST_Perc == "" ? 0 : insuranceIGST_Perc;
        $("#hdnInsuranceCGST_Perc").val(insuranceCGST_Perc);
        $("#hdnInsuranceSGST_Perc").val(insuranceSGST_Perc);
        $("#hdnInsuranceIGST_Perc").val(insuranceIGST_Perc);
    }
    var loadingCGST_Amount = 0;
    var loadingSGST_Amount = 0;
    var loadingIGST_Amount = 0;

    var freightCGST_Amount = 0;
    var freightSGST_Amount = 0;
    var freightIGST_Amount = 0;

    var insuranceCGST_Amount = 0;
    var insuranceSGST_Amount = 0;
    var insuranceIGST_Amount = 0;

    if (parseFloat(loadingCGST_Perc) > 0) {
        loadingCGST_Amount = (parseFloat(loadingValue) * parseFloat(loadingCGST_Perc)) / 100;
    }
    if (parseFloat(loadingSGST_Perc) > 0) {
        loadingSGST_Amount = (parseFloat(loadingValue) * parseFloat(loadingSGST_Perc)) / 100;
    }
    if (parseFloat(loadingIGST_Perc) > 0) {
        loadingIGST_Amount = (parseFloat(loadingValue) * parseFloat(loadingIGST_Perc)) / 100;
    }

    if (parseFloat(freightCGST_Perc) > 0) {
        freightCGST_Amount = (parseFloat(freightValue) * parseFloat(freightCGST_Perc)) / 100;
    }
    if (parseFloat(freightSGST_Perc) > 0) {
        freightSGST_Amount = (parseFloat(freightValue) * parseFloat(freightSGST_Perc)) / 100;
    }
    if (parseFloat(freightIGST_Perc) > 0) {
        freightIGST_Amount = (parseFloat(freightValue) * parseFloat(freightIGST_Perc)) / 100;
    }

    if (parseFloat(insuranceCGST_Perc) > 0) {
        insuranceCGST_Amount = (parseFloat(insuranceValue) * parseFloat(insuranceCGST_Perc)) / 100;
    }
    if (parseFloat(insuranceSGST_Perc) > 0) {
        insuranceSGST_Amount = (parseFloat(insuranceValue) * parseFloat(insuranceSGST_Perc)) / 100;
    }
    if (parseFloat(insuranceIGST_Perc) > 0) {
        insuranceIGST_Amount = (parseFloat(insuranceValue) * parseFloat(insuranceIGST_Perc)) / 100;
    }

    $("#txtLoadingCGST_Amt").val(loadingCGST_Amount.toFixed(2));
    $("#txtLoadingSGST_Amt").val(loadingSGST_Amount.toFixed(2));
    $("#txtLoadingIGST_Amt").val(loadingIGST_Amount.toFixed(2));

    $("#txtFreightCGST_Amt").val(freightCGST_Amount.toFixed(2));
    $("#txtFreightSGST_Amt").val(freightSGST_Amount.toFixed(2));
    $("#txtFreightIGST_Amt").val(freightIGST_Amount.toFixed(2));

    $("#txtInsuranceCGST_Amt").val(insuranceCGST_Amount.toFixed(2));
    $("#txtInsuranceSGST_Amt").val(insuranceSGST_Amount.toFixed(2));
    $("#txtInsuranceIGST_Amt").val(insuranceIGST_Amount.toFixed(2));



    $("#txtBasicValue").val(basicValue);
    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(taxValue) + parseFloat(freightValue) + parseFloat(freightCGST_Amount) + parseFloat(freightSGST_Amount) + parseFloat(freightIGST_Amount) + parseFloat(loadingValue) + parseFloat(loadingCGST_Amount) + parseFloat(loadingSGST_Amount) + parseFloat(loadingIGST_Amount) + parseFloat(insuranceValue) + parseFloat(insuranceCGST_Amount) + parseFloat(insuranceSGST_Amount) + parseFloat(insuranceIGST_Amount)).toFixed(0));
}

function ExecuteSave() {

    setTimeout(
function () {
    SaveData();
}, 2000);
    SetGSTPercentageInProduct();
}

///////*************For Only Allow Decimal Values For Two Digits Values******************///////

////////**************Created By Dheeraj**********////////////////////

$('.Quantity').keypress(function (event) {
    var $this = $(this);
    if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
       ((event.which < 48 || event.which > 57) &&
       (event.which != 0 && event.which != 8))) {
        event.preventDefault();
    }

    var text = $(this).val();
    if ((event.which == 46) && (text.indexOf('.') == -1)) {
        setTimeout(function () {
            if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
            }
        }, 1);
    }

    if ((text.indexOf('.') != -1) &&
        (text.substring(text.indexOf('.')).length > 2) &&
        (event.which != 0 && event.which != 8) &&
        ($(this)[0].selectionStart >= text.length - 2)) {
        event.preventDefault();
    }
});

$('.Quantity').bind("paste", function (e) {
    var text = e.originalEvent.clipboardData.getData('Text');
    if ($.isNumeric(text)) {
        if ((text.substring(text.indexOf('.')).length > 3) && (text.indexOf('.') > -1)) {
            e.preventDefault();
            $(this).val(text.substring(0, text.indexOf('.') + 3));
        }
    }
    else {
        e.preventDefault();
    }
});

function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../SO/AddEditSO";
    }
}

//////*************************End Code****************************/////////////////////////