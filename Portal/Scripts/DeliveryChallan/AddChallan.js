$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtChallanNo").attr('readOnly', true);
    $("#txtChallanDate").attr('readOnly', true);
    $("#txtInvoiceNo").attr('readOnly', true);
    $("#txtInvoiceDate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtDispatchRefDate").attr('readOnly', true);
    $("#txtLRDate").attr('readOnly', true);

    $("#txtCGSTPerc").attr('readOnly', true);
    $("#txtSGSTPerc").attr('readOnly', true);
    $("#txtIGSTPerc").attr('readOnly', true);
    $("#txtCGSTPercAmount").attr('readOnly', true);
    $("#txtSGSTPercAmount").attr('readOnly', true);
    $("#txtIGSTPercAmount").attr('readOnly', true);
    $("#txtHSN_Code").attr('readOnly', true);

    $("#txtLoadingCGST_Amt").attr('readOnly', true);
    $("#txtLoadingSGST_Amt").attr('readOnly', true);
    $("#txtLoadingIGST_Amt").attr('readOnly', true);

    $("#txtFreightCGST_Amt").attr('readOnly', true);
    $("#txtFreightSGST_Amt").attr('readOnly', true);
    $("#txtFreightIGST_Amt").attr('readOnly', true);

    $("#txtInsuranceCGST_Amt").attr('readOnly', true);
    $("#txtInsuranceSGST_Amt").attr('readOnly', true);
    $("#txtInsuranceIGST_Amt").attr('readOnly', true); 

    $("#txtCustomerCode").attr('readOnly', true);
    $("#txtConsigneeCode").attr('readOnly', true);

    $("#txtRefDate").attr('readOnly', true);
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
    BindCountryList();
    BindCompanyBranchList();
    $("#ddlSState").append($("<option></option>").val(0).html("-Select State-"));

    $("#txtCustomerName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../DeliveryChallan/GetCustomerAutoCompleteList",
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
            //GetCustomerDetail(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtCustomerName").val("");
                $("#hdnCustomerId").val("0");
                $("#txtCustomerCode").val("");
              //  ShowModel("Alert", "Please select Customer from List")

            }
            return false;
        }

    })
  .autocomplete("instance")._renderItem = function (ul, item) {
      return $("<li>")
        .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.primaryAddress + "</div>")
        .appendTo(ul);
  };

    $("#txtConsigneeName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../DeliveryChallan/GetCustomerAutoCompleteList",
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
            $("#txtConsigneeName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtConsigneeName").val(ui.item.label);
            $("#hdnConsigneeId").val(ui.item.value);
            $("#txtConsigneeCode").val(ui.item.code);
            GetConsigneeDetail(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtConsigneeName").val("");
                $("#hdnConsigneeId").val("0");
                $("#txtConsigneeCode").val("");
                ShowModel("Alert", "Please select Consignee from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.primaryAddress + "</div>")
      .appendTo(ul);
};

    $("#txtProductName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Product/GetProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode, CGST_Perc: item.CGST_Perc, SGST_Perc: item.SGST_Perc, IGST_Perc: item.IGST_Perc, HSN_Code: item.HSN_Code };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            $("#hdnProductId").val(ui.item.value);
            $("#txtProductShortDesc").val(ui.item.desc);
            $("#txtProductCode").val(ui.item.code);
            $("#txtHSN_Code").val(ui.item.HSN_Code);
            var ddlSState = $("#ddlSState");
            var hdnBillingStateId = $("#hdnBillingStateId");
            if (ddlSState.val() == hdnBillingStateId.val()) {
                $("#txtCGSTPerc").val(ui.item.CGST_Perc);
                $("#txtSGSTPerc").val(ui.item.SGST_Perc);
                $("#txtIGSTPerc").val(0);
                $("#txtCGSTPercAmount").val(0);
                $("#txtSGSTPercAmount").val(0);
                $("#txtIGSTPercAmount").val(0);
            }
            else {
                $("#txtCGSTPerc").val(0);
                $("#txtSGSTPerc").val(0);
                $("#txtIGSTPerc").val(ui.item.IGST_Perc);
                $("#txtCGSTPercAmount").val(0);
                $("#txtSGSTPercAmount").val(0);
                $("#txtIGSTPercAmount").val(0);
            }
            GetProductDetail(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductName").val("");
                $("#hdnProductId").val("0");
                $("#txtProductShortDesc").val("");
                $("#txtProductCode").val("");
                $("#txtHSN_Code").val("");
                $("#txtCGSTPerc").val(0);
                $("#txtSGSTPerc").val(0);
                $("#txtIGSTPerc").val(0);
                $("#txtCGSTPercAmount").val(0);
                $("#txtSGSTPercAmount").val(0);
                $("#txtIGSTPercAmount").val(0);
                ShowModel("Alert", "Please select Product from List")

            }
            return false;
        }

    })
 .autocomplete("instance")._renderItem = function (ul, item) {
     return $("<li>")
       .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.desc + "</div>")
       .appendTo(ul);
 };

 

    $("#txtTaxName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../DeliveryChallan/GetTaxAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.TaxName, value: item.TaxId, percentage: item.TaxPercentage };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtTaxName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtTaxName").val(ui.item.label);
            $("#hdnTaxId").val(ui.item.value);
            $("#txtTaxPercentage").val(ui.item.percentage);

            if (parseFloat($("#txtBasicValue").val()) > 0) {
                var taxAmount = (parseFloat($("#txtBasicValue").val()) * (parseFloat($("#txtTaxPercentage").val()) / 100));
                $("#txtTaxAmount").val(taxAmount.toFixed(2));
            }
            else {
                $("#txtTaxAmount").val("0");
            }
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtTaxName").val("");
                $("#hdnTaxId").val("0");
                $("#txtTaxPercentage").val("0");
                $("#txtTaxAmount").val("0");
                ShowModel("Alert", "Please select Tax from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + "</b></div>")
      .appendTo(ul);
};

    $("#txtProductTaxName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../DeliveryChallan/GetTaxAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.TaxName, value: item.TaxId, percentage: item.TaxPercentage };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtProductTaxName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtProductTaxName").val(ui.item.label);
            $("#hdnProductTaxId").val(ui.item.value);
            $("#hdnProductTaxPerc").val(ui.item.percentage);
            CalculateTotalCharges();
            //if (parseFloat($("#txtBasicValue").val()) > 0) {
            //    var taxAmount = (parseFloat($("#txtBasicValue").val()) * (parseFloat($("#txtTaxPercentage").val()) / 100));
            //    $("#txtProductTaxAmount").val(taxAmount.toFixed(2));
            //}
            //else {
            //    $("#txtProductTaxAmount").val("0");
            //}
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductTaxName").val("");
                $("#hdnProductTaxId").val("0");
                $("#hdnProductTaxPerc").val("0");
                $("#txtProductTaxAmount").val("0");
                ShowModel("Alert", "Please select Tax from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + "</b></div>")
      .appendTo(ul);
};

    $("#txtChallanDate,#txtLRDate,#txtDispatchRefDate").datepicker({
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

    BindDocumentTypeList();
    BindTermTemplateList();
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnChallanId = $("#hdnChallanId");
    if (hdnChallanId.val() != "" && hdnChallanId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetChallanDetail(hdnChallanId.val());
           $("#ddlCompanyBranch").attr('disabled', true);
       }, 3000);
        $(function () {
            $('input#txtCustomerName').blur();
        });
        var consigneeId = $("#hdnConsigneeId").val();
        BindConsigneeBranchList(consigneeId);
        $(function () {
            $('input#txtCustomerName').blur();
        });
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

    var challanProducts = [];
    GetChallanProductList(challanProducts);
    var challanTaxes = [];
    GetChallanTaxList(challanTaxes);
    var challanTerms = [];
    GetChallanTermList(challanTerms);
    var deliveryChallanDocuments = [];
    GetDeliveryChallanDocumentList(deliveryChallanDocuments);

});

$(  ".alpha-only").on("input", function () {
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

function BindCountryList() {
    $.ajax({
        type: "GET",
        url: "../Company/GetCountryList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSCountry").append($("<option></option>").val(0).html("-Select Country-"));
            $.each(data, function (i, item) {
                $("#ddlSCountry").append($("<option></option>").val(item.CountryId).html(item.CountryName));
            });
        },
        error: function (Result) {
            $("#ddlSCountry").append($("<option></option>").val(0).html("-Select Country-"));
        }
    });
}
function BindStateList(stateId) {
    var countryId = $("#ddlSCountry option:selected").val();
    $("#ddlSState").val(0);
    $("#ddlSState").html("");
    if (countryId != undefined && countryId != "" && countryId != "0") {
        var data = { countryId: countryId };
        $.ajax({
            type: "GET",
            url: "../Company/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlSState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlSState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                $("#ddlSState").val(stateId);
            },
            error: function (Result) {
                $("#ddlSState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
    }
    else {

        $("#ddlSState").append($("<option></option>").val(0).html("-Select State-"));
    }

}
function BindCompanyBranchList() {
    $("#ddlCompanyBranch").val(0);
    $("#ddlCompanyBranch").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: { },
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
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}

function GetBranchStateId() {
    var companyBranchID = $("#ddlCompanyBranch option:selected").val();
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchByStateIdList",
        data: { companyBranchID: companyBranchID },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#hdnBillingStateId").val(data.StateId);
            $("#hdnLoadingCGST_Perc").val(data.LoadingCGST_Perc);
            $("#hdnLoadingSGST_Perc").val(data.LoadingSGST_Perc);
            $("#hdnLoadingIGST_Perc").val(data.LoadingIGST_Perc);

            $("#hdnFreightCGST_Perc").val(data.FreightCGST_Perc);
            $("#hdnFreightSGST_Perc").val(data.FreightSGST_Perc);
            $("#hdnFreightIGST_Perc").val(data.FreightIGST_Perc);

            $("#hdnInsuranceCGST_Perc").val(data.InsuranceCGST_Perc);
            $("#hdnInsuranceSGST_Perc").val(data.InsuranceSGST_Perc);
            $("#hdnInsuranceIGST_Perc").val(data.InsuranceIGST_Perc);


        },
        error: function (Result) {
            $("#hdnBillingStateId").val(0);
            $("#hdnLoadingCGST_Perc").val(0);
            $("#hdnLoadingSGST_Perc").val(0);
            $("#hdnLoadingIGST_Perc").val(0);

            $("#hdnFreightCGST_Perc").val(0);
            $("#hdnFreightSGST_Perc").val(0);
            $("#hdnFreightIGST_Perc").val(0);

            $("#hdnInsuranceCGST_Perc").val(0);
            $("#hdnInsuranceSGST_Perc").val(0);
            $("#hdnInsuranceIGST_Perc").val(0);
        }

    });
}



//function BindCustomerBranchList(customerId) {
//    $("#ddlSCustomerBranch").val(0);
//    $("#ddlSCustomerBranch").html("");
//    $.ajax({
//        type: "GET",
//        url: "../DeliveryChallan/GetCustomerBranchList",
//        data: { customerId: customerId },
//        dataType: "json",
//        asnc: false,
//        success: function (data) {
//            $("#ddlSCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
//            $.each(data, function (i, item) {
//                $("#ddlSCustomerBranch").append($("<option></option>").val(item.CustomerBranchId).html(item.BranchName));
//            });
//        },
//        error: function (Result) {
//            $("#ddlSCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
//        }
//    });
//}
function BindConsigneeBranchList(consigneeId) {
    $.ajax({
        type: "GET",
        url: "../SO/GetCustomerBranchList",
        data: { customerId: consigneeId },
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
        url: "../DeliveryChallan/GetTermTemplateList",
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

function GetDeliveryChallanDocumentList(deliveryChallanDocuments) {
    var hdnChallanId = $("#hdnChallanId");
    var requestData = { deliveryChallanDocuments: deliveryChallanDocuments, challanId: hdnChallanId.val() };
    $.ajax({
        url: "../DeliveryChallan/GetDeliveryChallanSupportingDocumentList",
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
        url: "../DeliveryChallan/SaveSupportingDocument",
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
                var hdnDeliveryChallanDocId = $("#hdnDeliveryChallanDocId");
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
                 
                var deliveryChallanDocuments = [];
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    docEntrySequence = 1;
                }
                $('#tblDocumentList tr').each(function (i, row) {
                    var $row = $(row);
                    var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
                    var hdnDeliveryChallanDocId = $row.find("#hdnDeliveryChallanDocId").val();
                    var documentTypeId = $row.find("#hdnDocumentTypeId").val();
                    var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
                    var documentName = $row.find("#hdnDocumentName").val();
                    var documentPath = $row.find("#hdnDocumentPath").val();

                    if (hdnDeliveryChallanDocId != undefined) {
                        if ((hdnDocumentSequence.val() != documentSequenceNo)) {

                            var deliveryChallanDocument = {
                                DeliveryChallanDocId: hdnDeliveryChallanDocId,
                                DocumentSequenceNo: documentSequenceNo,
                                DocumentTypeId: documentTypeId,
                                DocumentTypeDesc: documentTypeDesc,
                                DocumentName: documentName,
                                DocumentPath: documentPath
                            };
                            deliveryChallanDocuments.push(deliveryChallanDocument);
                            docEntrySequence = parseInt(docEntrySequence) + 1;
                        }
                        else if (hdnDeliveryChallanDocId.val() == deliveryChallanDocId && hdnDocumentSequence.val() == documentSequenceNo) {
                            var deliveryChallanDocument = {
                                DeliveryChallanDocId: hdnDeliveryChallanDocId.val(),
                                DocumentSequenceNo: hdnDocumentSequence.val(),
                                DocumentTypeId: ddlDocumentType.val(),
                                DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                                DocumentName: newFileName,
                                DocumentPath: newFileName
                            };
                            deliveryChallanDocuments.push(deliveryChallanDocument);
                        }
                    }
                });
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    hdnDocumentSequence.val(docEntrySequence);
                }

                var deliveryChallanDocumentAddEdit = {
                    DeliveryChallanDocId: hdnDeliveryChallanDocId.val(),
                    DocumentSequenceNo: hdnDocumentSequence.val(),
                    DocumentTypeId: ddlDocumentType.val(),
                    DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                    DocumentName: newFileName,
                    DocumentPath: newFileName
                };
                deliveryChallanDocuments.push(deliveryChallanDocumentAddEdit);
                hdnDocumentSequence.val("0");

                GetDeliveryChallanDocumentList(deliveryChallanDocuments);



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
        $("#hdnDeliveryChallanDocId").val("0");
        $("#FileUpload1").val("");

        $("#btnAddDocument").show();
        $("#btnUpdateDocument").hide();
    }
}

function BindDocumentTypeList() {
    $.ajax({
        type: "GET",
        url: "../PO/GetModuleDocumentTypeList",
        data: { employeeDoc: "Sales" },
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
        var hdnDeliveryChallanDocId = $(row).find("#hdnDeliveryChallanDocId").val();
        ShowModel("Alert", "Document Removed from List.");
        row.remove();
    }
}



function BindTermsDescription() {
    var termTemplateId = $("#ddlTermTemplate option:selected").val();
    var challanTermList = [];
    if (termTemplateId != undefined && termTemplateId != "" && termTemplateId != "0") {
        var data = { termTemplateId: termTemplateId };

        $.ajax({
            type: "GET",
            url: "../DeliveryChallan/GetTermTemplateDetailList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                var termCounter = 1;

                $.each(data, function (i, item) {
                    var challanTerm = {
                        ChallanTermDetailId: item.TrnId,
                        TermDesc: item.TermsDesc,
                        TermSequence: termCounter
                    };
                    challanTermList.push(challanTerm);
                    termCounter += 1;
                });
                GetChallanTermList(challanTermList);
            },
            error: function (Result) {
                GetChallanTermList(challanTermList);
            }
        });
    }
    else {
        GetChallanTermList(challanTermList);
    }
}
function GetChallanTermList(challanTerms) {
    var hdnChallanId = $("#hdnChallanId");
    var requestData = { challanTerms: challanTerms, challanId: hdnChallanId.val() };
    $.ajax({
        url: "../DeliveryChallan/GetChallanTermList",
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
    var hdnChallanTermDetailId = $("#hdnChallanTermDetailId");
    var hdnTermSequence = $("#hdnTermSequence");

    if (txtTermDesc.val().trim() == "") {
        ShowModel("Alert", "Please Enter Terms")
        txtTermDesc.focus();
        return false;
    }

    var challanTermList = [];
    var termCounter = 1;
    $('#tblTermList tr').each(function (i, row) {
        var $row = $(row);
        var challanTermDetailId = $row.find("#hdnChallanTermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (termDesc != undefined) {
            if (action == 1 || hdnChallanTermDetailId.val() != challanTermDetailId) {

                if (termSequence == 0)
                { termSequence = termCounter; }

                var challanTerm = {
                    ChallanTermDetailId: challanTermDetailId,
                    TermDesc: termDesc,
                    TermSequence: termSequence
                };
                challanTermList.push(challanTerm);
                termCounter += 1;
            }
        }

    });

    if (hdnTermSequence.val() == "" || hdnTermSequence.val() == "0") {
        hdnTermSequence.val(termCounter);
    }
    var challanTermAddEdit = {
        ChallanTermDetailId: hdnChallanTermDetailId.val(),
        TermDesc: txtTermDesc.val().trim(),
        TermSequence: hdnTermSequence.val()
    };

    challanTermList.push(challanTermAddEdit);
    GetChallanTermList(challanTermList);

}
function EditTermRow(obj) {

    var row = $(obj).closest("tr");
    var challanTermDetailId = $(row).find("#hdnChallanTermDetailId").val();
    var termDesc = $(row).find("#hdnTermDesc").val();
    var termSequence = $(row).find("#hdnTermSequence").val();


    $("#txtTermDesc").val(termDesc);
    $("#hdnChallanTermDetailId").val(challanTermDetailId);
    $("#hdnTermSequence").val(termSequence);

    $("#btnAddTerm").hide();
    $("#btnUpdateTerm").show();
    ShowHideTermPanel(1);
}
function RemoveTermRow(obj) {
    if (confirm("Do you want to remove selected Term?")) {
        var row = $(obj).closest("tr");
        var challanTermDetailId = $(row).find("#hdnChallanTermDetailId").val();
        ShowModel("Alert", "Term Removed from List.");
        row.remove();
    }
}
function GetChallanTaxList(challanTaxes) {
    var hdnChallanId = $("#hdnChallanId");
    var requestData = { challanTaxes: challanTaxes, challanId: hdnChallanId.val() };
    $.ajax({
        url: "../DeliveryChallan/GetChallanTaxList",
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
    var taxEntrySequence = 0;
    var hdnTaxSequenceNo = $("#hdnTaxSequenceNo");
    var txtBasicValue = $("#txtBasicValue");
    var txtTaxName = $("#txtTaxName");
    var hdnChallanTaxDetailId = $("#hdnChallanTaxDetailId");
    var hdnTaxId = $("#hdnTaxId");
    var txtTaxPercentage = $("#txtTaxPercentage");
    var txtTaxAmount = $("#txtTaxAmount");

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

    var challanTaxList = [];
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        taxEntrySequence = 1;
    }
    $('#tblTaxList tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var challanTaxDetailId = $row.find("#hdnChallanTaxDetailId").val();
        var taxId = $row.find("#hdnTaxId").val();
        var taxName = $row.find("#hdnTaxName").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();
        var taxAmount = $row.find("#hdnTaxAmount").val();

        if (taxId != undefined) {
            if (action == 1 || (hdnTaxSequenceNo.val() != taxSequenceNo)) {

                if (taxId == hdnTaxId.val()) {
                    ShowModel("Alert", "Tax already added!!!")
                    txtTaxName.focus();
                    return false;
                }

                var challanTax = {
                    ChallanTaxDetailId: challanTaxDetailId,
                    TaxSequenceNo: taxSequenceNo,
                    TaxId: taxId,
                    TaxName: taxName,
                    TaxPercentage: taxPercentage,
                    TaxAmount: taxAmount
                };
                challanTaxList.push(challanTax);
                taxEntrySequence = parseInt(taxEntrySequence) + 1;
            }
            else if (hdnChallanTaxDetailId.val() == challanTaxDetailId && hdnTaxSequenceNo.val() == taxSequenceNo)
            {
                var challanTaxAddEdit = {
                    ChallanTaxDetailId: hdnChallanTaxDetailId.val(),
                    TaxSequenceNo: hdnTaxSequenceNo.val(),
                    TaxId: hdnTaxId.val(),
                    TaxName: txtTaxName.val().trim(),
                    TaxPercentage: txtTaxPercentage.val().trim(),
                    TaxAmount: txtTaxAmount.val().trim()
                };
                challanTaxList.push(challanTaxAddEdit);
            }
        }
    });
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        hdnTaxSequenceNo.val(taxEntrySequence);
    }
    if (action == 1) {
        var challanTaxAddEdit = {
            ChallanTaxDetailId: hdnChallanTaxDetailId.val(),
            TaxSequenceNo: hdnTaxSequenceNo.val(),
            TaxId: hdnTaxId.val(),
            TaxName: txtTaxName.val().trim(),
            TaxPercentage: txtTaxPercentage.val().trim(),
            TaxAmount: txtTaxAmount.val().trim()
        };
        challanTaxList.push(challanTaxAddEdit);
        hdnTaxSequenceNo.val("0");
    }
   GetChallanTaxList(challanTaxList);
}
function EditTaxRow(obj) {

    var row = $(obj).closest("tr");
    var challanTaxDetailId = $(row).find("#hdnChallanTaxDetailId").val();
    var taxSequenceNo = $(row).find("#hdnTaxSequenceNo").val();
    var taxId = $(row).find("#hdnTaxId").val();
    var taxName = $(row).find("#hdnTaxName").val();
    var taxPercentage = $(row).find("#hdnTaxPercentage").val();
    var taxAmount = $(row).find("#hdnTaxAmount").val();

    $("#txtTaxName").val(taxName);
    $("#hdnChallanTaxDetailId").val(challanTaxDetailId);
    $("#hdnTaxSequenceNo").val(taxSequenceNo);
    $("#hdnTaxId").val(taxId);
    $("#txtTaxPercentage").val(taxPercentage);
    $("#txtTaxAmount").val(taxAmount);

    $("#btnAddTax").hide();
    $("#btnUpdateTax").show();
    ShowHideTaxPanel(1);
}
function RemoveTaxRow(obj) {
    if (confirm("Do you want to remove selected Tax?")) {
        var row = $(obj).closest("tr");
        var challanTaxDetailId = $(row).find("#hdnChallanTaxDetailId").val();
        ShowModel("Alert", "Tax Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
    }
}


function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");

    var txtProductName = $("#txtProductName");
    var hdnChallanProductDetailId = $("#hdnChallanProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtPrice = $("#txtPrice");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var txtAvailableStock = $("#txtAvailableStock");
    var txtDiscountPerc = $("#txtDiscountPerc");
    var txtDiscountAmount = $("#txtDiscountAmount");
    var txtProductTaxName = $("#txtProductTaxName");
    var hdnProductTaxId = $("#hdnProductTaxId");
    var hdnProductTaxPerc = $("#hdnProductTaxPerc");
    var txtProductTaxAmount = $("#txtProductTaxAmount");
    var txtTotalPrice = $("#txtTotalPrice");

    var txtCGSTPerc = $("#txtCGSTPerc");
    var txtCGSTPercAmount = $("#txtCGSTPercAmount");
    var txtSGSTPerc = $("#txtSGSTPerc");
    var txtSGSTPercAmount = $("#txtSGSTPercAmount");
    var txtIGSTPerc = $("#txtIGSTPerc");
    var txtIGSTPercAmount = $("#txtIGSTPercAmount");
    var txtHSN_Code = $("#txtHSN_Code");

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
    if (parseFloat(txtQuantity.val().trim()) > parseFloat(txtAvailableStock.val().trim())) {
        ShowModel("Alert", "Challan Quantity cannot be greater than Available Stock Quantity")
        txtQuantity.focus();
        return false;
    }
    if (txtTotalPrice.val().trim() == "" || txtTotalPrice.val().trim() == "0" || parseFloat(txtTotalPrice.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter correct Price and Quantity")
        txtQuantity.focus();
        return false;
    }
    var challanProductList = [];
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var challanProductDetailId = $row.find("#hdnChallanProductDetailId").val();
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

                var challanProduct = {
                    ChallanProductDetailId: challanProductDetailId,
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
                    TotalPrice: totalPrice,
                    CGST_Perc: cGSTPerc,
                    CGST_Amount: cGSTPercAmount,
                    SGST_Perc: sGSTPerc,
                    SGST_Amount: sGSTPercAmount,
                    IGST_Perc: iGSTPerc,
                    IGST_Amount: iGSTPercAmount,
                    HSN_Code: hsn_Code
                };
                challanProductList.push(challanProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnProductId.val() == productId && hdnSequenceNo.val() == sequenceNo) {
                var challanProduct = {
                    ChallanProductDetailId: hdnChallanProductDetailId.val(),
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
                    TotalPrice: txtTotalPrice.val().trim(),
                    CGST_Perc: txtCGSTPerc.val(),
                    CGST_Amount: txtCGSTPercAmount.val(),
                    SGST_Perc: txtSGSTPerc.val(),
                    SGST_Amount: txtSGSTPercAmount.val(),
                    IGST_Perc: txtIGSTPerc.val(),
                    IGST_Amount: txtIGSTPercAmount.val(),
                    HSN_Code: txtHSN_Code.val()
                };
                challanProductList.push(challanProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var challanProductAddEdit = {
            ChallanProductDetailId: hdnChallanProductDetailId.val(),
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
            TotalPrice: txtTotalPrice.val().trim(),
            CGST_Perc: txtCGSTPerc.val(),
            CGST_Amount: txtCGSTPercAmount.val(),
            SGST_Perc: txtSGSTPerc.val(),
            SGST_Amount: txtSGSTPercAmount.val(),
            IGST_Perc: txtIGSTPerc.val(),
            IGST_Amount: txtIGSTPercAmount.val(),
            HSN_Code: txtHSN_Code.val()
        };

        challanProductList.push(challanProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetChallanProductList(challanProductList);
    }

}
function GetChallanProductList(challanProducts) {
    var hdnChallanId = $("#hdnChallanId");
    var requestData = { challanProducts: challanProducts, challanId: hdnChallanId.val() };
    $.ajax({
        url: "../DeliveryChallan/GetChallanProductList",
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
    var CGSTPerc = $("#txtCGSTPerc").val();
    var SGSTPerc = $("#txtSGSTPerc").val();
    var IGSTPerc = $("#txtIGSTPerc").val();

    var discountAmount = 0;
    var taxAmount = 0;
    var CGSTAmount = 0;
    var SGSTAmount = 0;
    var IGSTAmount = 0;

    price = price == "" ? 0 : price;
    quantity = quantity == "" ? 0 : quantity;
    var totalPrice = parseFloat(price) * parseFloat(quantity);
    if (parseFloat(discountPerc) > 0)
    {
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

    $("#txtTotalPrice").val((totalPrice - discountAmount + taxAmount + CGSTAmount + SGSTAmount + IGSTAmount).toFixed(2));


}
function CalculateGrossandNetValues() {
    var basicValue = 0;
    var taxValue = 0;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var challanProductDetailId = $row.find("#hdnChallanProductDetailId").val();
        var totalPrice = $row.find("#hdnTotalPrice").val();
        if (totalPrice != undefined) {
            basicValue += parseFloat(totalPrice);
        }

    });
    $('#tblTaxList tr').each(function (i, row) {
        var $row = $(row);
        var challanTaxDetailId = $row.find("#hdnChallanTaxDetailId").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();
        //var taxAmount = $row.find("#hdnTaxAmount").val();
        var taxAmount = 0;
        if (taxPercentage != undefined) {
            if (parseFloat(basicValue) > 0) {
                taxAmount = (parseFloat(basicValue) * (parseFloat(taxPercentage) / 100));
                $row.find("#hdnTaxAmount").val(taxAmount.toFixed(2));
                $row.find("#tdTaxAmount").html(taxAmount.toFixed(2));
            }
            else {
                taxAmount = 0;
                $row.find("#hdnTaxAmount").val("0");
                $row.find("#tdTaxAmount").html("0");
            }
            taxValue += parseFloat(taxAmount);
        }

    });

    var freightValue = $("#txtFreightValue").val() == "" ? "0" : $("#txtFreightValue").val();
    var freightCGST_Amt = $("#txtFreightCGST_Amt").val() == "" ? "0" : $("#txtFreightCGST_Amt").val();
    var freightSGST_Amt = $("#txtFreightSGST_Amt").val() == "" ? "0" : $("#txtFreightSGST_Amt").val();
    var freightIGST_Amt = $("#txtFreightIGST_Amt").val() == "" ? "0" : $("#txtFreightIGST_Amt").val();

    var loadingValue = $("#txtLoadingValue").val() == "" ? "0" : $("#txtLoadingValue").val();
    var loadingCGST_Amt = $("#txtLoadingCGST_Amt").val() == "" ? "0" : $("#txtLoadingCGST_Amt").val();
    var loadingSGST_Amt = $("#txtLoadingSGST_Amt").val() == "" ? "0" : $("#txtLoadingSGST_Amt").val();
    var loadingIGST_Amt = $("#txtLoadingIGST_Amt").val() == "" ? "0" : $("#txtLoadingIGST_Amt").val();

    var insuranceValue = $("#txtInsuranceValue").val() == "" ? "0" : $("#txtInsuranceValue").val();
    var insuranceCGST_Amt = $("#txtInsuranceCGST_Amt").val() == "" ? "0" : $("#txtInsuranceCGST_Amt").val();
    var insuranceSGST_Amt = $("#txtInsuranceSGST_Amt").val() == "" ? "0" : $("#txtInsuranceSGST_Amt").val();
    var insuranceIGST_Amt = $("#txtInsuranceIGST_Amt").val() == "" ? "0" : $("#txtInsuranceIGST_Amt").val();


    if (parseFloat(freightValue) <= 0) {
        freightValue = 0;
    }
    if (parseFloat(freightCGST_Amt) <= 0) {
        freightCGST_Amt = 0;
    }
    if (parseFloat(freightSGST_Amt) <= 0) {
        freightSGST_Amt = 0;
    }
    if (parseFloat(freightIGST_Amt) <= 0) {
        freightIGST_Amt = 0;
    }


    if (parseFloat(loadingValue) <= 0) {
        loadingValue = 0;
    }
    if (parseFloat(loadingCGST_Amt) <= 0) {
        loadingCGST_Amt = 0;
    }
    if (parseFloat(loadingSGST_Amt) <= 0) {
        loadingSGST_Amt = 0;
    }
    if (parseFloat(loadingIGST_Amt) <= 0) {
        loadingIGST_Amt = 0;
    }

    if (parseFloat(insuranceValue) <= 0) {
        insuranceValue = 0;
    }
    if (parseFloat(insuranceCGST_Amt) <= 0) {
        insuranceCGST_Amt = 0;
    }
    if (parseFloat(insuranceSGST_Amt) <= 0) {
        insuranceSGST_Amt = 0;
    }
    if (parseFloat(insuranceIGST_Amt) <= 0) {
        insuranceIGST_Amt = 0;
    }


    $("#txtBasicValue").val(basicValue.toFixed(2));

    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(taxValue) + parseFloat(freightValue) + parseFloat(freightCGST_Amt) + parseFloat(freightSGST_Amt) + parseFloat(freightIGST_Amt) + parseFloat(loadingValue) + parseFloat(loadingCGST_Amt) + parseFloat(loadingSGST_Amt) + parseFloat(loadingIGST_Amt) + parseFloat(insuranceValue) + parseFloat(insuranceCGST_Amt) + parseFloat(insuranceSGST_Amt) + parseFloat(insuranceIGST_Amt)).toFixed(2));
}


function CalculateLoadingTotalCharges() {
    var hdnBillingStateId = $("#hdnBillingStateId").val();
    var ddlSState = $("#ddlSState").val();

    if (ddlSState == "0" || ddlSState == "") {
        ShowModel("Alert", "Please Select Consignee")
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

function CalculateInsuranceTotalCharges() {
    var hdnBillingStateId = $("#hdnBillingStateId").val();
    var ddlSState = $("#ddlSState").val();

    if (ddlSState == "0" || ddlSState == "") {
        ShowModel("Alert", "Please Select Consignee")
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



//function CalculateLoadingTotalCharges() {
//    var hdnBillingStateId = $("#hdnBillingStateId").val();
//    var ddlSState = $("#ddlSState").val();

//    if (ddlSState == "0" || ddlSState == "") {
//        ShowModel("Alert", "Please Select Consignee")
//        $("#txtLoadingValue").val(0);
//        return false;
//    }
//    else if (hdnBillingStateId == "" || hdnBillingStateId == "0") {
//        ShowModel("Alert", "Please Select Billing Location");
//        $("#txtLoadingValue").val(0);
//        return false;
//    }

//    var loadingValue = $("#txtLoadingValue").val();
//    var loadingCGST_Perc = $("#hdnLoadingCGST_Perc").val();
//    var loadingSGST_Perc = $("#hdnLoadingSGST_Perc").val();
//    var loadingIGST_Perc = $("#hdnLoadingIGST_Perc").val();

//    loadingValue = loadingValue == "" ? 0 : loadingValue;
//    if (hdnBillingStateId == ddlSState) {
//        loadingCGST_Perc = loadingCGST_Perc == "" ? 0 : loadingCGST_Perc;
//        loadingSGST_Perc = loadingSGST_Perc == "" ? 0 : loadingSGST_Perc;
//        loadingIGST_Perc = 0;
//    }
//    else {
//        loadingCGST_Perc = 0;
//        loadingSGST_Perc = 0;
//        loadingIGST_Perc = loadingIGST_Perc == "" ? 0 : loadingIGST_Perc;
//    }

//    var loadingCGST_Amount = 0;
//    var loadingSGST_Amount = 0;
//    var loadingIGST_Amount = 0;


//    if (parseFloat(loadingCGST_Perc) > 0) {
//        loadingCGST_Amount = (parseFloat(loadingValue) * parseFloat(loadingCGST_Perc)) / 100;
//    }
//    if (parseFloat(loadingSGST_Perc) > 0) {
//        loadingSGST_Amount = (parseFloat(loadingValue) * parseFloat(loadingSGST_Perc)) / 100;
//    }
//    if (parseFloat(loadingIGST_Perc) > 0) {
//        loadingIGST_Amount = (parseFloat(loadingValue) * parseFloat(loadingIGST_Perc)) / 100;
//    }

//    $("#txtLoadingCGST_Amt").val(loadingCGST_Amount.toFixed(2));
//    $("#txtLoadingSGST_Amt").val(loadingSGST_Amount.toFixed(2));
//    $("#txtLoadingIGST_Amt").val(loadingIGST_Amount.toFixed(2));




//}

//function CalculateFreightTotalCharges() {
//    var hdnBillingStateId = $("#hdnBillingStateId").val();
//    var ddlSState = $("#ddlSState").val();

//    if (ddlSState == "0" || ddlSState == "") {
//        ShowModel("Alert", "Please Select Consignee ")
//        $("#txtFreightValue").val(0);
//        return false;
//    }
//    else if (hdnBillingStateId == "" || hdnBillingStateId == "0") {
//        ShowModel("Alert", "Please Select Billing Location");
//        $("#txtFreightValue").val(0);
//        return false;
//    }

//    var freightValue = $("#txtFreightValue").val();
//    var freightCGST_Perc = $("#hdnFreightCGST_Perc").val();
//    var freightSGST_Perc = $("#hdnFreightSGST_Perc").val();
//    var freightIGST_Perc = $("#hdnFreightIGST_Perc").val();

//    freightValue = freightValue == "" ? 0 : freightValue;
//    if (hdnBillingStateId == ddlSState) {
//        freightCGST_Perc = freightCGST_Perc == "" ? 0 : freightCGST_Perc;
//        freightSGST_Perc = freightSGST_Perc == "" ? 0 : freightSGST_Perc;
//        freightIGST_Perc = 0;
//    }
//    else {
//        freightCGST_Perc = 0;
//        freightSGST_Perc = 0;
//        freightIGST_Perc = freightIGST_Perc == "" ? 0 : freightIGST_Perc;
//    }

//    var freightCGST_Amount = 0;
//    var freightSGST_Amount = 0;
//    var freightIGST_Amount = 0;


//    if (parseFloat(freightCGST_Perc) > 0) {
//        freightCGST_Amount = (parseFloat(freightValue) * parseFloat(freightCGST_Perc)) / 100;
//    }
//    if (parseFloat(freightSGST_Perc) > 0) {
//        freightSGST_Amount = (parseFloat(freightValue) * parseFloat(freightSGST_Perc)) / 100;
//    }
//    if (parseFloat(freightIGST_Perc) > 0) {
//        freightIGST_Amount = (parseFloat(freightValue) * parseFloat(freightIGST_Perc)) / 100;
//    }

//    $("#txtFreightCGST_Amt").val(freightCGST_Amount.toFixed(2));
//    $("#txtFreightSGST_Amt").val(freightSGST_Amount.toFixed(2));
//    $("#txtFreightIGST_Amt").val(freightIGST_Amount.toFixed(2));
//}

//function CalculateInsuranceTotalCharges() {
//    var hdnBillingStateId = $("#hdnBillingStateId").val();
//    var ddlSState = $("#ddlSState").val();

//    if (ddlSState == "0" || ddlSState == "") {
//        ShowModel("Alert", "Please Select Consignee")
//        $("#txtInsuranceValue").val(0);
//        return false;
//    }
//    else if (hdnBillingStateId == "" || hdnBillingStateId == "0") {
//        ShowModel("Alert", "Please Select Billing Location");
//        $("#txtInsuranceValue").val(0);
//        return false;
//    }

//    var insuranceValue = $("#txtInsuranceValue").val();
//    var insuranceCGST_Perc = $("#hdnInsuranceCGST_Perc").val();
//    var insuranceSGST_Perc = $("#hdnInsuranceSGST_Perc").val();
//    var insuranceIGST_Perc = $("#hdnInsuranceIGST_Perc").val();

//    insuranceValue = insuranceValue == "" ? 0 : insuranceValue;
//    if (hdnBillingStateId == ddlSState) {
//        insuranceCGST_Perc = insuranceCGST_Perc == "" ? 0 : insuranceCGST_Perc;
//        insuranceSGST_Perc = insuranceSGST_Perc == "" ? 0 : insuranceSGST_Perc;
//        insuranceIGST_Perc = 0;
//    }
//    else {
//        insuranceCGST_Perc = 0;
//        insuranceSGST_Perc = 0;
//        insuranceIGST_Perc = insuranceIGST_Perc == "" ? 0 : insuranceIGST_Perc;
//    }

//    var insuranceCGST_Amount = 0;
//    var insuranceSGST_Amount = 0;
//    var insuranceIGST_Amount = 0;


//    if (parseFloat(insuranceCGST_Perc) > 0) {
//        insuranceCGST_Amount = (parseFloat(insuranceValue) * parseFloat(insuranceCGST_Perc)) / 100;
//    }
//    if (parseFloat(insuranceSGST_Perc) > 0) {
//        insuranceSGST_Amount = (parseFloat(insuranceValue) * parseFloat(insuranceSGST_Perc)) / 100;
//    }
//    if (parseFloat(insuranceIGST_Perc) > 0) {
//        insuranceIGST_Amount = (parseFloat(insuranceValue) * parseFloat(insuranceIGST_Perc)) / 100;
//    }

//    $("#txtInsuranceCGST_Amt").val(insuranceCGST_Amount.toFixed(2));
//    $("#txtInsuranceSGST_Amt").val(insuranceSGST_Amount.toFixed(2));
//    $("#txtInsuranceIGST_Amt").val(insuranceIGST_Amount.toFixed(2));
//}

function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var challanProductDetailId = $(row).find("#hdnChallanProductDetailId").val();
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

    var cGSTPerc = $(row).find("#hdnCGSTPerc").val();
    var cGSTAmount = $(row).find("#hdnCGSTAmount").val();
    var sGSTPerc = $(row).find("#hdnSGSTPerc").val();
    var sGSTAmount = $(row).find("#hdnSGSTAmount").val();
    var iGSTPerc = $(row).find("#hdnIGSTPerc").val();
    var iGSTAmount = $(row).find("#hdnIGSTAmount").val();
    var hsn_Code = $(row).find("#hdnHSN_Code").val();


    $("#txtProductName").val(productName);
    $("#hdnChallanProductDetailId").val(challanProductDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
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
        var challanProductDetailId = $(row).find("#hdnChallanProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
    }
}

function GetChallanDetail(challanId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../DeliveryChallan/GetChallanDetail",
        data: { challanId: challanId },
        dataType: "json",
        success: function (data) {
            $("#txtChallanNo").val(data.ChallanNo);
            $("#txtChallanDate").val(data.ChallanDate);
            $("#txtInvoiceNo").val(data.InvoiceNo);
            $("#hdnInvoiceId").val(data.InvoiceId);
            $("#txtInvoiceDate").val(data.InvoiceDate);
            if (data.InvoiceId != 0) {
                $("#txtCustomerName").attr('disabled', true);
                $("#txtConsigneeName").attr('disabled', true);
            }
            else
            {
                $("#txtCustomerName").attr('disabled', false);
                $("#txtConsigneeName").attr('disabled', false);
            }

            $("#hdnCustomerId").val(data.CustomerId);
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtCustomerName").val(data.CustomerName);

            $("#hdnConsigneeId").val(data.ConsigneeId);
            $("#txtConsigneeCode").val(data.ConsigneeCode);
            $("#txtConsigneeName").val(data.ConsigneeName);


            $("#ddlApprovalStatus").val(data.ApprovalStatus)
    
            $("#txtSContactPerson").val(data.ShippingContactPerson);
            $("#txtSAddress").val(data.ShippingBillingAddress);
            $("#txtSCity").val(data.ShippingCity);
            $("#ddlSCountry").val(data.ShippingCountryId); 
            BindStateList(data.ShippingStateId);
            $("#ddlSState").val(data.ShippingStateId);
            $("#txtSPinCode").val(data.ShippingPinCode);
            $("#txtSTINNo").val(data.ShippingTINNo);
            $("#txtSGSTNo").val(data.ShippingTINNo);

            $("#txtSEmail").val(data.ShippingEmail);
            $("#txtSMobileNo").val(data.ShippingMobileNo);
            $("#txtSContactNo").val(data.ShippingContactNo);
            $("#txtSFax").val(data.ShippingFax);

            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtDispatchRefNo").val(data.DispatchRefNo);
            $("#txtDispatchRefDate").val(data.DispatchRefDate);
            $("#txtLRNo").val(data.LRNo);
            $("#txtLRDate").val(data.LRDate);
            $("#txtTransportVia").val(data.TransportVia);
            $("#txtNoOfPackets").val(data.NoOfPackets);
            
             
            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);


            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }

            $("#txtBasicValue").val(data.BasicValue);
            $("#txtTotalValue").val(data.TotalValue);
            $("#txtFreightValue").val(data.FreightValue);
            $("#hdnFreightCGST_Perc").val(data.FreightCGST_Perc);
            $("#txtFreightCGST_Amt").val(data.FreightCGST_Amt);
            $("#hdnFreightSGST_Perc").val(data.FreightSGST_Perc);
            $("#txtFreightSGST_Amt").val(data.FreightSGST_Amt);
            $("#hdnFreightIGST_Perc").val(data.FreightIGST_Perc);
            $("#txtFreightIGST_Amt").val(data.FreightIGST_Amt);

            $("#txtLoadingValue").val(data.LoadingValue);
            $("#hdnLoadingCGST_Perc").val(data.LoadingCGST_Perc);
            $("#txtLoadingCGST_Amt").val(data.LoadingCGST_Amt);
            $("#hdnLoadingSGST_Perc").val(data.LoadingSGST_Perc);
            $("#txtLoadingSGST_Amt").val(data.LoadingSGST_Amt);
            $("#hdnLoadingIGST_Perc").val(data.LoadingIGST_Perc);
            $("#txtLoadingIGST_Amt").val(data.LoadingIGST_Amt);

            $("#txtInsuranceValue").val(data.InsuranceValue);
            $("#hdnInsuranceCGST_Perc").val(data.InsuranceCGST_Perc);
            $("#txtInsuranceCGST_Amt").val(data.InsuranceCGST_Amt);
            $("#hdnInsuranceSGST_Perc").val(data.InsuranceSGST_Perc);
            $("#txtInsuranceSGST_Amt").val(data.InsuranceSGST_Amt);
            $("#hdnInsuranceIGST_Perc").val(data.InsuranceIGST_Perc);
            $("#txtInsuranceIGST_Amt").val(data.InsuranceIGST_Amt);
            if (data.ReverseChargeApplicable == true) {
                $("#chkReverseChargeApplicable").prop("checked", true);
                changeReverseChargeStatus();
            }
            $("#txtReverseChargeAmount").val(data.ReverseChargeAmount);

            GetBranchStateId();

            $("#btnAddNew").show();
            $("#btnPrint").show();
            $("#btnEmail").show();


        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
           
    
}
function SaveData() {
    var txtChallanNo = $("#txtChallanNo");
    var hdnChallanId = $("#hdnChallanId");
    var txtChallanDate = $("#txtChallanDate");
    var hdnInvoiceId = $("#hdnInvoiceId");
    var txtInvoiceNo = $("#txtInvoiceNo");
    var hdnCustomerId = $("#hdnCustomerId");
    var txtCustomerName = $("#txtCustomerName");
    var hdnConsigneeId = $("#hdnConsigneeId");
    var txtConsigneeName = $("#txtConsigneeName");
    var ddlSCustomerBranch = $("#ddlSCustomerBranch"); 
    var txtSAddress = $("#txtSAddress");
    var txtSCity = $("#txtSCity");
    var ddlSCountry = $("#ddlSCountry");
    var ddlSState = $("#ddlSState");
    var txtSPinCode = $("#txtSPinCode"); 
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtSTINNo = $("#txtSTINNo");
    var txtSGSTNo = $("#txtSGSTNo");
    var txtSEmail = $("#txtSEmail");
    var txtSMobileNo = $("#txtSMobileNo");
    var txtSContactNo = $("#txtSContactNo");
    var txtSFax = $("#txtSFax");
    var txtSContactPerson = $("#txtSContactPerson");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var txtDispatchRefNo = $("#txtDispatchRefNo");
    var txtDispatchRefDate = $("#txtDispatchRefDate"); 
    var txtLRNo = $("#txtLRNo");
    var txtLRDate = $("#txtLRDate");
    var txtTransportVia = $("#txtTransportVia");
    var txtNoOfPackets = $("#txtNoOfPackets");
    var txtBasicValue = $("#txtBasicValue");
    var txtFreightValue = $("#txtFreightValue");
    var hdnFreightCGST_Perc = $("#hdnFreightCGST_Perc");
    var txtFreightCGST_Amt = $("#txtFreightCGST_Amt");
    var hdnFreightSGST_Perc = $("#hdnFreightSGST_Perc");
    var txtFreightSGST_Amt = $("#txtFreightSGST_Amt");
    var hdnFreightIGST_Perc = $("#hdnFreightIGST_Perc");
    var txtFreightIGST_Amt = $("#txtFreightIGST_Amt");

    var txtLoadingValue = $("#txtLoadingValue");
    var hdnLoadingCGST_Perc = $("#hdnLoadingCGST_Perc");
    var txtLoadingCGST_Amt = $("#txtLoadingCGST_Amt");
    var hdnLoadingSGST_Perc = $("#hdnLoadingSGST_Perc");
    var txtLoadingSGST_Amt = $("#txtLoadingSGST_Amt");
    var hdnLoadingIGST_Perc = $("#hdnLoadingIGST_Perc");
    var txtLoadingIGST_Amt = $("#txtLoadingIGST_Amt");

    var txtInsuranceValue = $("#txtInsuranceValue");
    var hdnInsuranceCGST_Perc = $("#hdnInsuranceCGST_Perc");
    var txtInsuranceCGST_Amt = $("#txtInsuranceCGST_Amt");
    var hdnInsuranceSGST_Perc = $("#hdnInsuranceSGST_Perc");
    var txtInsuranceSGST_Amt = $("#txtInsuranceSGST_Amt");
    var hdnInsuranceIGST_Perc = $("#hdnInsuranceIGST_Perc");
    var txtInsuranceIGST_Amt = $("#txtInsuranceIGST_Amt");

    var txtTotalValue = $("#txtTotalValue");
    var chkReverseChargeApplicable = $("#chkReverseChargeApplicable");
    var txtReverseChargeAmount = $("#txtReverseChargeAmount");
    var txtRemarks1 = $("#txtRemarks1");
    var txtRemarks2 = $("#txtRemarks2");

    if (txtInvoiceNo.val().trim() == "") {
        ShowModel("Alert", "Please select Invoice No")
        txtInvoiceNo.focus();
        return false;
    }
    
   if (hdnInvoiceId.val() == "" || hdnInvoiceId.val() == "0") {
           ShowModel("Alert", "Please select invoice No")
           txtInvoiceNo.focus();
           return false;
     }

    if (txtCustomerName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Party Name")
       // txtCustomerName.focus();
        return false;
    }
    if (hdnCustomerId.val() == "" || hdnCustomerId.val() == "0") {
        ShowModel("Alert", "Please select Party from list")
       // txtCustomerName.focus();
        return false;
    }
    if (txtConsigneeName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Consignee Name")
        return false;
    }
    if (hdnConsigneeId.val() == "" || hdnConsigneeId.val() == "0") {
        ShowModel("Alert", "Please select Consignee from list")
        return false;
    }


    if (txtSAddress.val().trim() == "") {
        ShowModel("Alert", "Please Enter Consignee shipping Address")
        txtSAddress.focus();
        return false;
    }
    if (txtSCity.val().trim() == "") {
        ShowModel("Alert", "Please enter shipping city")
        txtSCity.focus();
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Dispatch From Location")
        ddlCompanyBranch.focus();
        return false;
    }
    if (txtBasicValue.val() == "" || parseFloat(txtBasicValue.val()) <= 0) {
        ShowModel("Alert", "Please select at least one Product")
        return false;
    }

    var reverseChargeApplicableStatus = $("#chkReverseChargeApplicable").is(':checked') ? true : false;
    if (reverseChargeApplicableStatus == true) {
        if (txtReverseChargeAmount.val().trim() == "") {
            ShowModel("Alert", "Please enter Reverse Charge Amount")
            return false;
        }
    }

    var loadingCGSTPerc = 0;
    if (txtLoadingCGST_Amt.val() == "" || parseFloat(txtLoadingCGST_Amt.val()) == 0) {
        loadingCGSTPerc = 0;
    }
    else {
        loadingCGSTPerc = hdnLoadingCGST_Perc.val();
    }

    var loadingSGSTPerc = 0;
    if (txtLoadingSGST_Amt.val() == "" || parseFloat(txtLoadingSGST_Amt.val()) == 0) {
        loadingSGSTPerc = 0;
    }
    else {
        loadingSGSTPerc = hdnLoadingSGST_Perc.val();
    }

    var loadingIGSTPerc = 0;
    if (txtLoadingIGST_Amt.val() == "" || parseFloat(txtLoadingIGST_Amt.val()) == 0) {
        loadingIGSTPerc = 0;
    }
    else {
        loadingIGSTPerc = hdnLoadingIGST_Perc.val();
    }

    var freightCGSTPerc = 0;
    if (txtFreightCGST_Amt.val() == "" || parseFloat(txtFreightCGST_Amt.val()) == 0) {
        freightCGSTPerc = 0;
    }
    else {
        freightCGSTPerc = hdnFreightCGST_Perc.val();
    }

    var freightSGSTPerc = 0;
    if (txtFreightSGST_Amt.val() == "" || parseFloat(txtFreightSGST_Amt.val()) == 0) {
        freightSGSTPerc = 0;
    }
    else {
        freightSGSTPerc = hdnFreightSGST_Perc.val();
    }

    var freightIGSTPerc = 0;
    if (txtFreightIGST_Amt.val() == "" || parseFloat(txtFreightIGST_Amt.val()) == 0) {
        freightIGSTPerc = 0;
    }
    else {
        freightIGSTPerc = hdnFreightIGST_Perc.val();
    }

    var insuranceCGSTPerc = 0;
    if (txtInsuranceCGST_Amt.val() == "" || parseFloat(txtInsuranceCGST_Amt.val()) == 0) {
        insuranceCGSTPerc = 0;
    }
    else {
        insuranceCGSTPerc = hdnInsuranceCGST_Perc.val();
    }

    var insuranceSGSTPerc = 0;
    if (txtInsuranceSGST_Amt.val() == "" || parseFloat(txtInsuranceSGST_Amt.val()) == 0) {
        insuranceSGSTPerc = 0;
    }
    else {
        insuranceSGSTPerc = hdnInsuranceSGST_Perc.val();
    }

    var insuranceIGSTPerc = 0;
    if (txtInsuranceIGST_Amt.val() == "" || parseFloat(txtInsuranceIGST_Amt.val()) == 0) {
        insuranceIGSTPerc = 0;
    }
    else {
        insuranceIGSTPerc = hdnInsuranceIGST_Perc.val();
    }



    var challanViewModel = {
        ChallanId: hdnChallanId.val(),
        ChallanNo: txtChallanNo.val().trim(),
        ChallanDate: txtChallanDate.val().trim(),
        InvoiceId: hdnInvoiceId.val().trim(),
        InvoiceNo: txtInvoiceNo.val().trim(),
        CustomerId: hdnCustomerId.val().trim(),
        CustomerName: txtCustomerName.val().trim(),
        ConsigneeId: hdnConsigneeId.val().trim(),
        ConsigneeName: txtConsigneeName.val().trim(),
        ContactPerson: txtSContactPerson.val().trim(), 
        ShippingContactPerson: txtSContactPerson.val().trim(),
        ShippingBillingAddress: txtSAddress.val().trim(),
        ShippingCity: txtSCity.val().trim(),
        ShippingStateId: ddlSState.val(),
        ShippingCountryId: ddlSCountry.val(),
        ShippingPinCode: txtSPinCode.val().trim(),
        ShippingTINNo: txtSGSTNo.val().trim(),
        ShippingEmail: txtSEmail.val().trim(),
        ShippingMobileNo: txtSMobileNo.val().trim(),
        ShippingContactNo: txtSContactNo.val().trim(),
        ShippingFax: txtSFax.val().trim(),
        CompanyBranchId: ddlCompanyBranch.val(),
        DispatchRefNo: txtDispatchRefNo.val().trim(),
        DispatchRefDate: txtDispatchRefDate.val(),
        LRNo: txtLRNo.val().trim(),
        LRDate: txtLRDate.val(),

        TransportVia: txtTransportVia.val().trim(),
        NoOfPackets: txtNoOfPackets.val(),
        ApprovalStatus:ddlApprovalStatus.val(),
        BasicValue: txtBasicValue.val(),
        FreightValue: txtFreightValue.val(),
        FreightCGST_Perc: freightCGSTPerc,
        FreightCGST_Amt: txtFreightCGST_Amt.val(),
        FreightSGST_Perc: freightSGSTPerc,
        FreightSGST_Amt: txtFreightSGST_Amt.val(),
        FreightIGST_Perc: freightIGSTPerc,
        FreightIGST_Amt: txtFreightIGST_Amt.val(),
        LoadingValue: txtLoadingValue.val(),
        LoadingCGST_Perc: loadingCGSTPerc,
        LoadingCGST_Amt: txtLoadingCGST_Amt.val(),
        LoadingSGST_Perc: loadingSGSTPerc,
        LoadingSGST_Amt: txtLoadingSGST_Amt.val(),
        LoadingIGST_Perc: loadingIGSTPerc,
        LoadingIGST_Amt: txtLoadingIGST_Amt.val(),
        InsuranceValue: txtInsuranceValue.val(),
        InsuranceCGST_Perc: insuranceCGSTPerc,
        InsuranceCGST_Amt: txtInsuranceCGST_Amt.val(),
        InsuranceSGST_Perc: insuranceSGSTPerc,
        InsuranceSGST_Amt: txtInsuranceSGST_Amt.val(),
        InsuranceIGST_Perc: insuranceIGSTPerc,
        InsuranceIGST_Amt: txtInsuranceIGST_Amt.val(),
        ReverseChargeApplicable: reverseChargeApplicableStatus,
        ReverseChargeAmount: txtReverseChargeAmount.val(),
        TotalValue: txtTotalValue.val(), 
        Remarks1: txtRemarks1.val(),
        Remarks2: txtRemarks2.val()
    };

    var challanProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var challanProductDetailId = $row.find("#hdnChallanProductDetailId").val();
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
        var cGSTPerc = $row.find("#hdnCGSTPerc").val();
        var cGSTPercAmount = $row.find("#hdnCGSTAmount").val();
        var sGSTPerc = $row.find("#hdnSGSTPerc").val();
        var sGSTPercAmount = $row.find("#hdnSGSTAmount").val();
        var iGSTPerc = $row.find("#hdnIGSTPerc").val();
        var iGSTPercAmount = $row.find("#hdnIGSTAmount").val();
        var hsn_Code = $row.find("#hdnHSN_Code").val();

        if (productId != undefined) {

            var challanProduct = {
                challanProductDetailId: challanProductDetailId,
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
                TotalPrice: totalPrice,
                CGST_Perc: cGSTPerc,
                CGST_Amount: cGSTPercAmount,
                SGST_Perc: sGSTPerc,
                SGST_Amount: sGSTPercAmount,
                IGST_Perc: iGSTPerc,
                IGST_Amount: iGSTPercAmount,
                HSN_Code: hsn_Code
            };
            challanProductList.push(challanProduct);
        }
    });


    var challanTaxList = [];
    $('#tblTaxList tr').each(function (i, row) {
        var $row = $(row);
        var challanTaxDetailId = $row.find("#hdnChallanTaxDetailId").val();
        var taxId = $row.find("#hdnTaxId").val();
        var taxName = $row.find("#hdnTaxName").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();
        var taxAmount = $row.find("#hdnTaxAmount").val();

        if (taxId != undefined) {
            var challanTax = {
                ChallanTaxDetailId: challanTaxDetailId,
                TaxId: taxId,
                TaxName: taxName,
                TaxPercentage: taxPercentage,
                TaxAmount: taxAmount
            };
            challanTaxList.push(challanTax);
        }

    });

    var challanTermList = [];
    $('#tblTermList tr').each(function (i, row) {
        var $row = $(row);
        var challanTermDetailId = $row.find("#hdnChallanTermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (termDesc != undefined) {
            var challanTerm = {
                ChallanTermDetailId: challanTermDetailId,
                TermDesc: termDesc,
                TermSequence: termSequence
            };
            challanTermList.push(challanTerm);
        }

    });


    var deliveryChallanDocumentList = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var deliveryChallanDocId = $row.find("#hdnDeliveryChallanDocId").val();
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        if (deliveryChallanDocId != undefined) {
            var deliveryChallanDocument = {
                DeliveryChallanDocId: deliveryChallanDocId,
                DocumentSequenceNo: documentSequenceNo,
                DocumentTypeId: documentTypeId,
                DocumentTypeDesc: documentTypeDesc,
                DocumentName: documentName,
                DocumentPath: documentPath
            };
            deliveryChallanDocumentList.push(deliveryChallanDocument);
        }

    });

    var accessMode = 1;//Add Mode
    if (hdnChallanId.val() != null && hdnChallanId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { challanViewModel: challanViewModel, challanProducts: challanProductList, challanTaxes: challanTaxList, challanTerms: challanTermList, deliveryChallanDocuments: deliveryChallanDocumentList };
    $.ajax({
        url: "../DeliveryChallan/AddEditDeliveryChallan?AccessMode=" + accessMode + "",
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
                       window.location.href = "../DeliveryChallan/AddEditDeliveryChallan?ChallanId=" + data.trnId + "&AccessMode=3";
                   }, 3000);

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

    $("#txtChallanNo").val("");
    $("#hdnChallanId").val("0");
    $("#txtChallanDate").val($("#hdnCurrentDate").val());
    $("#hdnCustomerId").val("0");
    $("#txtCustomerName").val("");
    $("#txtCustomerCode").val("");
    $("#hdnConsigneeId").val("0");
    $("#txtConsigneeName").val("");
    $("#txtConsigneeCode").val("");
    $("#txtInvoiceNo").val("");
    $("#txtInvoiceDate").val("");
    $("#txtInvoiceId").val("0"); 
    $("#ddlSCustomerBranch").val("0");
    $("#ddlCompanyBranch").val("0");
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
    $("#ddlApprovalStatus").val("Draft");
    $("#txtDispatchRefNo").val("");
    $("#txtDispatchRefDate").val(""); 
    $("#txtLRNo").val("");
    $("#txtLRDate").val("");
    $("#txtTransportVia").val("");
    $("#txtNoOfPackets").val("");
 
    $("#txtBasicValue").val("");
    $("#txtFreightValue").val("");
    $("#txtLoadingValue").val("");
    $("#txtTotalValue").val("");
    $("#txtRemarks1").val("");
    $("#txtRemarks2").val("");

    $("#btnSave").show();
    $("#btnUpdate").hide();


}
function GetConsigneeDetail(consigneeId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Customer/GetCustomerDetail",
        data: { customerId: consigneeId },
        dataType: "json",
        success: function (data) {
            $("#txtSContactPerson").val(data.ContactPersonName);
            $("#txtSAddress").val(data.PrimaryAddress);
            $("#txtSCity").val(data.City);
            $("#ddlSCountry").val(data.CountryId);
            BindConsigneeBranchList(consigneeId)
            
            $("#ddlSCustomerBranch").val(data.customerBranchId)
            BindStateList(data.StateId);
            $("#ddlSState").val(data.StateId);
            $("#txtSPinCode").val(data.PinCode);
            $("#txtSTINNo").val(data.TINNo);
            $("#txtSEmail").val(data.Email);
            $("#txtSMobileNo").val(data.MobileNo);
            $("#txtSContactNo").val(data.ContactNo);
            $("#txtSFax").val(data.Fax);
            $("#txtSGSTNo").val(data.GSTNo);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
   
}
function GetProductDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productid: productId },
        dataType: "json",
        success: function (data) {
            $("#txtPrice").val(data.SalePrice);
            $("#txtUOMName").val(data.UOMName);
            $("#txtQuantity").val("");
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
        data: { productid: productId, companyBranchId: $("#ddlCompanyBranch").val() , trnId: $("#hdnChallanId").val() , trnType: "DC" },
        dataType: "json",
        success: function (data) {
            $("#txtAvailableStock").val(data);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}
function ShowHideProductPanel(action) {
    if (action == 1) {
        $(".productsection").show();
    }
    else {
        $(".productsection").hide();
        $("#txtProductName").val("");
        $("#hdnProductId").val("0");
        $("#hdnChallanProductDetailId").val("0");
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
        $("#txtHSN_Code").val("");
        $("#txtCGSTPerc").val("");
        $("#txtCGSTPercAmount").val("");
        $("#txtSGSTPerc").val("");
        $("#txtSGSTPercAmount").val("");
        $("#txtIGSTPerc").val("");
        $("#txtIGSTPercAmount").val("");

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
        $("#hdnChallanTaxDetailId").val("0");
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
        $("#hdnChallanTermDetailId").val("0");
        $("#hdnTermSequence").val("0");
        $("#btnAddTerm").show();
        $("#btnUpdateTerm").hide();
    }
}
function OpenInvoiceSearchPopup() {
    if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
        ShowModel("Alert", "Please Select Company Branch");
        return false;
    }
    $("#divInvoiceList").html("");
    $("#SearchInvoiceModel").modal();

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
function SearchInvoice() {
    var txtInvoiceNo = $("#txtSearchInvoiceNo");
    var txtCustomerName = $("#txtSearchCustomerName"); 
    var txtRefNo = $("#txtSearchRefNo");
    var ddlSearchInvoiceType = $("#ddlSearchInvoiceType");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { saleinvoiceNo: txtInvoiceNo.val().trim(), customerName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), invoiceType: ddlSearchInvoiceType.val(), displayType: "Popup", approvalStatus: "Final", companyBranchId: ddlCompanyBranch.val() };
    $.ajax({
        url: "../DeliveryChallan/GetSaleInvoiceList",
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
function SelectInvoice(invoiceId, saleinvoiceNo, invoiceDate, customerId, customerCode, customerName, consigneeId, consigneeCode, consigneeName) {
    $("#txtInvoiceNo").val(saleinvoiceNo);
    $("#hdnInvoiceId").val(invoiceId);
    $("#txtInvoiceDate").val(invoiceDate);
    $("#hdnCustomerId").val(customerId);
    $("#txtCustomerCode").val(customerCode);
    $("#txtCustomerName").val(customerName);
    $("#hdnConsigneeId").val(consigneeId);
    $("#txtConsigneeCode").val(consigneeCode);
    $("#txtConsigneeName").val(consigneeName);

   
    $("#ddlCompanyBranch").attr('disabled', true);
    $("#txtLoadingValue").attr('disabled', true);
    $("#txtFreightValue").attr('disabled', true);
    $("#txtInsuranceValue").attr('disabled', true);
   
    GetConsigneeDetail(consigneeId);
    GetSaleInvoiceDetail(invoiceId);
    var saleinvoiceProducts = [];
    GetSaleInvoiceProductList(saleinvoiceProducts, invoiceId);
    var saleinvoiceTaxes = [];
    GetSaleInvoiceTaxList(saleinvoiceTaxes, invoiceId);
    var saleinvoiceTerms = [];
    GetSaleInvoiceTermList(saleinvoiceTerms, invoiceId);
    $("#txtCustomerName").attr('disabled', true);
    $("#txtConsigneeName").attr('disabled', true);
    $("#SearchInvoiceModel").modal('hide');
}
 
function GetSaleInvoiceProductList(saleinvoiceProducts, invoiceId) {
   
    var requestData = { saleinvoiceProducts: saleinvoiceProducts, saleinvoiceId: invoiceId };
    $.ajax({
        url: "../DeliveryChallan/GetChallanSaleInvoiceProductList",
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
function GetSaleInvoiceTaxList(saleinvoiceTaxes, invoiceId) {
    var requestData = { saleinvoiceTaxes: saleinvoiceTaxes, saleinvoiceId: invoiceId };
    $.ajax({
        url: "../SaleInvoice/GetSaleInvoiceTaxList",
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
function GetSaleInvoiceTermList(saleinvoiceTerms, invoiceId) {
   var requestData = { saleinvoiceTerms: saleinvoiceTerms, saleinvoiceId: invoiceId };
    $.ajax({
        url: "../SaleInvoice/GetSaleInvoiceTermList",
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
function GetSaleInvoiceDetail(saleinvoiceId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SaleInvoice/GetSaleInvoiceDetail",
        data: { saleinvoiceId: saleinvoiceId },
        dataType: "json",
        success: function (data) {
            //BindCompanyBranchList();
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtSAddress").val(data.ShippingBillingAddress);
            $("#txtSCity").val(data.ShippingCity);
            $("#ddlSCountry").val(data.ShippingCountryId);
            BindStateList(data.ShippingStateId);
            $("#ddlSState").val(data.ShippingStateId);
            $("#txtSPinCode").val(data.ShippingPinCode);
            $("#txtSTINNo").val(data.ShippingTINNo);
            $("#txtSGSTNo").val(data.ShippingTINNo);
            $("#txtRemarks1").val(data.Remarks);
            $("#txtBasicValue").val(data.BasicValue);
            $("#txtTotalValue").val(data.TotalValue);
            $("#txtFreightValue").val(data.FreightValue);
            $("#hdnFreightCGST_Perc").val(data.FreightCGST_Perc);
            $("#txtFreightCGST_Amt").val(data.FreightCGST_Amt);
            $("#hdnFreightSGST_Perc").val(data.FreightSGST_Perc);
            $("#txtFreightSGST_Amt").val(data.FreightSGST_Amt);
            $("#hdnFreightIGST_Perc").val(data.FreightIGST_Perc);
            $("#txtFreightIGST_Amt").val(data.FreightIGST_Amt);

            $("#txtLoadingValue").val(data.LoadingValue);
            $("#hdnLoadingCGST_Perc").val(data.LoadingCGST_Perc);
            $("#txtLoadingCGST_Amt").val(data.LoadingCGST_Amt);
            $("#hdnLoadingSGST_Perc").val(data.LoadingSGST_Perc);
            $("#txtLoadingSGST_Amt").val(data.LoadingSGST_Amt);
            $("#hdnLoadingIGST_Perc").val(data.LoadingIGST_Perc);
            $("#txtLoadingIGST_Amt").val(data.LoadingIGST_Amt);

            $("#txtInsuranceValue").val(data.InsuranceValue);
            $("#hdnInsuranceCGST_Perc").val(data.InsuranceCGST_Perc);
            $("#txtInsuranceCGST_Amt").val(data.InsuranceCGST_Amt);
            $("#hdnInsuranceSGST_Perc").val(data.InsuranceSGST_Perc);
            $("#txtInsuranceSGST_Amt").val(data.InsuranceSGST_Amt);
            $("#hdnInsuranceIGST_Perc").val(data.InsuranceIGST_Perc);
            $("#txtInsuranceIGST_Amt").val(data.InsuranceIGST_Amt);
            if (data.ReverseChargeApplicable == true) {
                $("#chkReverseChargeApplicable").prop("checked", true);
                changeReverseChargeStatus();
            }
            $("#txtReverseChargeAmount").val(data.ReverseChargeAmount);
            GetBranchStateId();

          },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}


function FillShippingAddress() {

    var customerBranchId = $("#ddlSCustomerBranch option:selected").val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../DeliveryChallan/GetCustomerBranchDetail",
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
            $("#txtSGSTNo").val(data.GSTNo);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}
 
function SendMail() {
    var hdnChallanId = $("#hdnChallanId");
    var requestData = { challanId: hdnChallanId.val(), reportType: "PDF" };
    $.ajax({
        url: "../DeliveryChallan/ChallanMail",
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

function changeReverseChargeStatus() {
    if ($("#chkReverseChargeApplicable").is(':checked')) {
        $("#txtReverseChargeAmount").attr("disabled", false);
    }
    else {
        $("#txtReverseChargeAmount").attr("disabled", true);
        $("#txtReverseChargeAmount").val("");
    }
}

function SetGSTPercentageInProduct() {
    var hdnCustomerStateId = $("#ddlSState");
    var hdnBillingStateId = $("#hdnBillingStateId");

    if (hdnCustomerStateId.val() == "0") {
        ShowModel("Alert", "Please Select Consignee")
        return false;
    }
    else if (hdnBillingStateId.val() == "0") {
        ShowModel("Alert", "Please Billing Location")
        return false;
    }


    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var hdnProductId = $row.find("#hdnProductId").val();
        if (hdnProductId != undefined) {
            $.ajax({
                type: "GET",
                url: "../Quotation/GetProductTaxPercentage",
                data: { productId: hdnProductId },
                dataType: "json",
                asnc: false,
                success: function (data) {
                    if (hdnCustomerStateId.val() == hdnBillingStateId.val()) {
                        $row.find("#hdnCGSTPerc").val(data.CGST_Perc);
                        $row.find("#hdnSGSTPerc").val(data.SGST_Perc);
                        $row.find("#hdnIGSTPerc").val(0);
                    }
                    else {
                        $row.find("#hdnCGSTPerc").val(0);
                        $row.find("#hdnSGSTPerc").val(0);
                        $row.find("#hdnIGSTPerc").val(data.IGST_Perc);
                    }


                },
                error: function (Result) {
                    $row.find("#hdnCGSTPerc").val(0);
                    $row.find("#hdnSGSTPerc").val(0);
                    $row.find("#hdnIGSTPerc").val(0);
                }

            });
        }
    });

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

    if (hdnCustomerStateId.val() == "0") {
        ShowModel("Alert", "Please Select Consignee")
        return false;
    }
    else if (hdnBillingStateId.val() == "0") {
        ShowModel("Alert", "Please Billing Location")
        return false;
    }


    $('#tblProductList tr').each(function (i, row) {


        var $row = $(row);
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


    //var loadingCGST_Perc = $("#hdnLoadingCGST_Perc").val();
    //var loadingSGST_Perc = $("#hdnLoadingSGST_Perc").val();
    //var loadingIGST_Perc = $("#hdnLoadingIGST_Perc").val();

    //var freightCGST_Perc = $("#hdnFreightCGST_Perc").val();
    //var freightSGST_Perc = $("#hdnFreightSGST_Perc").val();
    //var freightIGST_Perc = $("#hdnFreightIGST_Perc").val();

    //var insuranceCGST_Perc = $("#hdnInsuranceCGST_Perc").val();
    //var insuranceSGST_Perc = $("#hdnInsuranceSGST_Perc").val();
    //var insuranceIGST_Perc = $("#hdnInsuranceIGST_Perc").val();

    if (hdnCustomerStateId.val() == hdnBillingStateId.val()) {
        //loadingCGST_Perc = loadingCGST_Perc == "" ? 0 : loadingCGST_Perc;
        //loadingSGST_Perc = loadingSGST_Perc == "" ? 0 : loadingSGST_Perc;
        //loadingIGST_Perc = 0;

        //freightCGST_Perc = freightCGST_Perc == "" ? 0 : freightCGST_Perc;
        //freightSGST_Perc = freightSGST_Perc == "" ? 0 : freightSGST_Perc;
        //freightIGST_Perc = 0;

        //insuranceCGST_Perc = insuranceCGST_Perc == "" ? 0 : insuranceCGST_Perc;
        //insuranceSGST_Perc = insuranceSGST_Perc == "" ? 0 : insuranceSGST_Perc;
        //insuranceIGST_Perc = 0;

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
        //loadingCGST_Perc = 0;
        //loadingSGST_Perc = 0;
        //loadingIGST_Perc = loadingIGST_Perc == "" ? 0 : loadingIGST_Perc;

        //freightCGST_Perc = 0;
        //freightSGST_Perc = 0;
        //freightIGST_Perc = freightIGST_Perc == "" ? 0 : freightIGST_Perc;

        //insuranceCGST_Perc = 0;
        //insuranceSGST_Perc = 0;
        //insuranceIGST_Perc = insuranceIGST_Perc == "" ? 0 : insuranceIGST_Perc;
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
    SetGSTPercentageInProduct();
    setTimeout(
function () {
    SaveData();
}, 2000);

}

function Reset() {
    if (confirm("Do you want to clear Delivery Challan?")) {
        window.location.href = "../MRN/ListMRN";
    }
}