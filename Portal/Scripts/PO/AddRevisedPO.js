$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    GetOtherChargesGSTPercentage();
    $("#txtDeliveryDate").attr('readOnly', true);
    $("#txtQuotationDate").attr('disabled', true);
    $("#txtIndentDate").attr('disabled', true);
    $("#txtIndentNo").attr('readOnly', true);
    $("#txtQuotationNo").attr('readOnly', true);
    $("#txtPONo").attr('readOnly', true);
    $("#txtPODate").attr('readOnly', true);
    
    $("#txtVendorName").attr('readOnly', true);
    $("#txtVendorCode").attr('readOnly', true);
    $("#txtConsigneeCode").attr('readOnly', true);
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
    $("#txtRefDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtTotalPrice").attr('readOnly', true);
    $("#txtBasicValue").attr('readOnly', true);

    $("#txtTaxPercentage").attr('readOnly', true);
    $("#txtTaxAmount").attr('readOnly', true);
    $("#txtTotalValue").attr('readOnly', true);

    $("#txtVendorName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../PO/GetVendorAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.VendorName, value: item.VendorId, Address: item.Address, code: item.VendorCode };
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
            $("#txtVendorCode").val(ui.item.code);
            GetVendorDetail(ui.item.value);
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
        .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.Address + "</div>")
        .appendTo(ul);
  };

    $("#txtConsigneeName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SaleInvoice/GetCustomerAutoCompleteList",
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
            var hdnBillingStateId = $("#ddlBState");
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
                url: "../Quotation/GetTaxAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.TaxName, value: item.TaxId, percentage: item.TaxPercentage, SurchargeName_1: item.SurchargeName_1, SurchargePercentage_1: item.SurchargePercentage_1, SurchargeName_2: item.SurchargeName_2, SurchargePercentage_2: item.SurchargePercentage_2, SurchargeName_3: item.SurchargeName_3, SurchargePercentage_3: item.SurchargePercentage_3 };
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

            $("#hdnSurchargeName_1").val(ui.item.SurchargeName_1);
            $("#hdnSurchargePercentage_1").val(ui.item.SurchargePercentage_1);
            $("#hdnSurchargeName_2").val(ui.item.SurchargeName_2);
            $("#hdnSurchargePercentage_2").val(ui.item.SurchargePercentage_2);
            $("#hdnSurchargeName_3").val(ui.item.SurchargeName_3);
            $("#hdnSurchargePercentage_3").val(ui.item.SurchargePercentage_3);

            if (parseFloat($("#txtBasicValue").val()) > 0) {
                var taxAmount = (parseFloat($("#txtBasicValue").val()) * (parseFloat($("#txtTaxPercentage").val()) / 100));
                $("#txtTaxAmount").val(taxAmount.toFixed(2));
                var surchargeAmount_1 = (parseFloat(taxAmount) * (parseFloat($("#hdnSurchargePercentage_1").val()) / 100));
                $("#txtSurchargeAmount_1").val(surchargeAmount_1.toFixed(2));
                var surchargeAmount_2 = (parseFloat(taxAmount) * (parseFloat($("#hdnSurchargePercentage_2").val()) / 100));
                $("#txtSurchargeAmount_2").val(surchargeAmount_2.toFixed(2));
                var surchargeAmount_3 = (parseFloat(taxAmount) * (parseFloat($("#hdnSurchargePercentage_3").val()) / 100));
                $("#txtSurchargeAmount_3").val(surchargeAmount_3.toFixed(2));
                var totalTaxAmount = parseFloat(taxAmount) + parseFloat(surchargeAmount_1) + +parseFloat(surchargeAmount_2) + +parseFloat(surchargeAmount_3);
                $("#txtTotalTaxAmount").val(totalTaxAmount.toFixed(2));


            }
            else {
                $("#txtTaxAmount").val("0");
                $("#txtSurchargeAmount_1").val("0");
                $("#txtSurchargeAmount_2").val("0");
                $("#txtSurchargeAmount_3").val("0");
                $("#txtTotalTaxAmount").val("0");
            }
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtTaxName").val("");
                $("#hdnTaxId").val("0");
                $("#txtTaxPercentage").val("0");
                $("#txtTaxAmount").val("0");

                $("#hdnSurchargeName_1").val("");
                $("#hdnSurchargePercentage_1").val("0");
                $("#hdnSurchargeName_2").val("");
                $("#hdnSurchargePercentage_2").val("0");
                $("#hdnSurchargeName_3").val("");
                $("#hdnSurchargePercentage_3").val("0");

                $("#txtSurchargeAmount_1").val("0");
                $("#txtSurchargeAmount_2").val("0");
                $("#txtSurchargeAmount_3").val("0");

                $("#txtTotalTaxAmount").val("0");
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
                url: "../SaleInvoice/GetTaxAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.TaxName, value: item.TaxId, percentage: item.TaxPercentage, SurchargeName_1: item.SurchargeName_1, SurchargePercentage_1: item.SurchargePercentage_1, SurchargeName_2: item.SurchargeName_2, SurchargePercentage_2: item.SurchargePercentage_2, SurchargeName_3: item.SurchargeName_3, SurchargePercentage_3: item.SurchargePercentage_3 };
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

            $("#hdnProductSurchargeName_1").val(ui.item.SurchargeName_1);
            $("#hdnProductSurchargePercentage_1").val(ui.item.SurchargePercentage_1);
            $("#hdnProductSurchargeName_2").val(ui.item.SurchargeName_2);
            $("#hdnProductSurchargePercentage_2").val(ui.item.SurchargePercentage_2);
            $("#hdnProductSurchargeName_3").val(ui.item.SurchargeName_3);
            $("#hdnProductSurchargePercentage_3").val(ui.item.SurchargePercentage_3);


            CalculateTotalCharges();
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductTaxName").val("");
                $("#hdnProductTaxId").val("0");
                $("#hdnProductTaxPerc").val("0");
                $("#txtProductTaxAmount").val("0");

                $("#hdnProductSurchargeName_1").val("");
                $("#hdnProductSurchargePercentage_1").val("0");
                $("#hdnProductSurchargeName_2").val("");
                $("#hdnProductSurchargePercentage_2").val("0");
                $("#hdnProductSurchargeName_3").val("");
                $("#hdnProductSurchargePercentage_3").val("0");

                $("#txtProductSurchargeAmount_1").val("0");
                $("#txtProductSurchargeAmount_2").val("0");
                $("#txtProductSurchargeAmount_3").val("0");

                $("#txtProductTotalTaxAmount").val("0");


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


    $("#txtRefDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    $("#txtPODate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {
            $("#txtDeliveryDate").datepicker("option", "minDate", selected);

        }
    });


    $("#txtDeliveryDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    BindDocumentTypeList();
    BindCurrencyList();
    BindCompanyBranchList();
    BindTermTemplateList();
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPOId = $("#hdnPOId");
    if (hdnPOId.val() != "" && hdnPOId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPODetail(hdnPOId.val());
       }, 3000);



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
            $("#btnSave").show(); 
            $("#btnReset").show();
        }
    }
    else {
        $("#btnSave").show(); 
        $("#btnReset").show();
    }

    var poProducts = [];
    GetPOProductList(poProducts);
    var poTaxes = [];
    GetPOTaxList(poTaxes);
    var poTerms = [];
    GetPOTermList(poTerms);
    var poDocuments = [];
    GetPODocumentList(poDocuments);

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
        },
        error: function (Result) {
            $("#ddlCurrency").append($("<option></option>").val(0).html("-Select Currency-"));
        }
    });
}
function GetOtherChargesGSTPercentage() {
            $("#hdnLoadingCGST_Perc").val(9);
            $("#hdnLoadingSGST_Perc").val(9);
            $("#hdnLoadingIGST_Perc").val(18);
            $("#hdnFreightCGST_Perc").val(9);
            $("#hdnFreightSGST_Perc").val(9);
            $("#hdnFreightIGST_Perc").val(18);
            $("#hdnInsuranceCGST_Perc").val(9);
            $("#hdnInsuranceSGST_Perc").val(9);
            $("#hdnInsuranceIGST_Perc").val(18);
}
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
        var siProductDetailId = $row.find("#hdnSIProductDetailId").val();
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


    $("#txtBasicValue").val(basicValue);
    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(taxValue) + parseFloat(freightValue) + parseFloat(freightCGST_Amt) + parseFloat(freightSGST_Amt) + parseFloat(freightIGST_Amt) + parseFloat(loadingValue) + parseFloat(loadingCGST_Amt) + parseFloat(loadingSGST_Amt) + parseFloat(loadingIGST_Amt) + parseFloat(insuranceValue) + parseFloat(insuranceCGST_Amt) + parseFloat(insuranceSGST_Amt) + parseFloat(insuranceIGST_Amt)).toFixed(0));
}
function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtProductName = $("#txtProductName");
    var hdnPOProductDetailId = $("#hdnPOProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtPrice = $("#txtPrice");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var txtTotalPrice = $("#txtTotalPrice");
    var txtDiscountPerc = $("#txtDiscountPerc");
    var txtDiscountAmount = $("#txtDiscountAmount");
    var txtProductTaxName = $("#txtProductTaxName");
    var hdnProductTaxId = $("#hdnProductTaxId");
    var hdnProductTaxPerc = $("#hdnProductTaxPerc");
    var txtProductTaxAmount = $("#txtProductTaxAmount");

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
    if (txtTotalPrice.val().trim() == "" || txtTotalPrice.val().trim() == "0" || parseFloat(txtTotalPrice.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter correct Price and Quantity")
        txtQuantity.focus();
        return false;
    }
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    var poProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var poProductDetailId = $row.find("#hdnPOProductDetailId").val();
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

                if (productId == hdnProductId.val() && parseFloat(price) == parseFloat(txtPrice.val())) {
                    ShowModel("Alert", "Product already added with same Price!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }
                if (productId == hdnProductId.val() && parseFloat(price) != parseFloat(txtPrice.val())) {
                    if (confirm("You have allready Added this Product with Different Price. Do you want to add it?")) {
                        var poProduct = {
                            POProductDetailId: poProductDetailId,
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
                            HSN_Code: hsn_Code
                        };
                    }
                    else {
                        flag = false;
                        return false;
                    }
                }
                var poProduct = {
                    POProductDetailId: poProductDetailId,
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
                    HSN_Code: hsn_Code
                };
                poProductList.push(poProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnProductId.val() == productId && hdnSequenceNo.val() == sequenceNo) {

                var poProduct = {
                    POProductDetailId: hdnPOProductDetailId.val(),
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
                    HSN_Code: txtHSN_Code.val()
                };
                poProductList.push(poProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var poProductAddEdit = {
            POProductDetailId: hdnPOProductDetailId.val(),
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
            HSN_Code: txtHSN_Code.val()
        };
        poProductList.push(poProductAddEdit);
        hdnSequenceNo.val("0");
    }

    if (flag == true) {
        GetPOProductList(poProductList);
    }

}
function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var poProductDetailId = $(row).find("#hdnPOProductDetailId").val();
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
    $("#hdnPOProductDetailId").val(poProductDetailId);
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
    ShowHideProductPanel(1);
}
function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var poProductDetailId = $(row).find("#hdnPOProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
    }
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
        }
    });
}
function GetPODocumentList(poDocuments) {
    var hdnPOId = $("#hdnPOId");
    var requestData = { poDocuments: poDocuments, poId: hdnPOId.val() };
    $.ajax({
        url: "../PO/GetPOSupportingDocumentList",
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


    } else {

        ShowModel("Alert", "FormData is not supported.")
        return "";
    }

    $.ajax({
        url: "../PO/SaveRevisedSupportingDocument",
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
                var hdnPODocId = $("#hdnPODocId");
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



                var poDocumentList = [];
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    docEntrySequence = 1;
                }
                $('#tblDocumentList tr').each(function (i, row) {
                    var $row = $(row);
                    var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
                    var poDocId = $row.find("#hdnPODocId").val();
                    var documentTypeId = $row.find("#hdnDocumentTypeId").val();
                    var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
                    var documentName = $row.find("#hdnDocumentName").val();
                    var documentPath = $row.find("#hdnDocumentPath").val();

                    if (poDocId != undefined) {
                        if ((hdnDocumentSequence.val() != documentSequenceNo)) {

                            var poDocument = {
                                PODocId: poDocId,
                                DocumentSequenceNo: documentSequenceNo,
                                DocumentTypeId: documentTypeId,
                                DocumentTypeDesc: documentTypeDesc,
                                DocumentName: documentName,
                                DocumentPath: documentPath
                            };
                            poDocumentList.push(poDocument);
                            docEntrySequence = parseInt(docEntrySequence) + 1;
                        }
                        else if (hdnPODocId.val() == poDocId && hdnDocumentSequence.val() == documentSequenceNo) {
                            var poDocument = {
                                PODocId: hdnPODocId.val(),
                                DocumentSequenceNo: hdnDocumentSequence.val(),
                                DocumentTypeId: ddlDocumentType.val(),
                                DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                                DocumentName: newFileName,
                                DocumentPath: newFileName
                            };
                            poDocumentList.push(poDocument);
                        }
                    }
                });
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    hdnDocumentSequence.val(docEntrySequence);
                }

                var poDocumentAddEdit = {
                    PODocId: hdnPODocId.val(),
                    DocumentSequenceNo: hdnDocumentSequence.val(),
                    DocumentTypeId: ddlDocumentType.val(),
                    DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                    DocumentName: newFileName,
                    DocumentPath: newFileName
                };
                poDocumentList.push(poDocumentAddEdit);
                hdnDocumentSequence.val("0");

                GetPODocumentList(poDocumentList);



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
        var poDocId = $(row).find("#hdnPODocId").val();
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
        $("#hdnPODocId").val("0");
        $("#FileUpload1").val("");

        $("#btnAddDocument").show();
        $("#btnUpdateDocument").hide();
    }
}

function GetPOProductList(poProducts) {
    var hdnPOId = $("#hdnPOId");
    var requestData = { poProducts: poProducts, poId: hdnPOId.val() };
    $.ajax({
        url: "../PO/GetPOProductList",
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


    $("#txtBasicValue").val(basicValue);
    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(taxValue) + parseFloat(freightValue) + parseFloat(freightCGST_Amt) + parseFloat(freightSGST_Amt) + parseFloat(freightIGST_Amt) + parseFloat(loadingValue) + parseFloat(loadingCGST_Amt) + parseFloat(loadingSGST_Amt) + parseFloat(loadingIGST_Amt) + parseFloat(insuranceValue) + parseFloat(insuranceCGST_Amt) + parseFloat(insuranceSGST_Amt) + parseFloat(insuranceIGST_Amt)).toFixed(0));
}
function GetPOTermList(poTerms) {
    var hdnPOId = $("#hdnPOId");
    var requestData = { poTerms: poTerms, poId: hdnPOId.val() };
    $.ajax({
        url: "../PO/GetPOTermList",
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
function BindTermTemplateList() {
    $.ajax({
        type: "GET",
        url: "../PO/GetTermTemplateList",
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
    var poTermList = [];
    if (termTemplateId != undefined && termTemplateId != "" && termTemplateId != "0") {
        var data = { termTemplateId: termTemplateId };

        $.ajax({
            type: "GET",
            url: "../PO/GetTermTemplateDetailList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                var termCounter = 1;

                $.each(data, function (i, item) {
                    var poTerm = {
                        POTermDetailId: item.TrnId,
                        TermDesc: item.TermsDesc,
                        TermSequence: termCounter
                    };
                   poTermList.push(poTerm);
                    termCounter += 1;
                });
                GetPOTermList(poTermList);
            },
            error: function (Result) {
                GetPOTermList(poTermList);
            }
        });
    }
    else {
        GetPOTermList(poTermList);
    }
}
function GetPOTermList(poTerms) {
    var hdnPOId = $("#hdnPOId");
    var requestData = { poTerms: poTerms, poId: hdnPOId.val() };
    $.ajax({
        url: "../PO/GetPOTermList",
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
    var hdnPOTermDetailId = $("#hdnPOTermDetailId");
    var hdnTermSequence = $("#hdnTermSequence");

    if (txtTermDesc.val().trim() == "") {
        ShowModel("Alert", "Please Enter Terms")
        txtTermDesc.focus();
        return false;
    }

    var poTermList = [];
    var termCounter = 1;
    $('#tblTermList tr').each(function (i, row) {
        var $row = $(row);
        var poTermDetailId = $row.find("#hdnPOTermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (termDesc != undefined) {
            if (action == 1 || hdnPOTermDetailId.val() != poTermDetailId) {

                if (termSequence == 0)
                { termSequence = termCounter; }

                var poTerm = {
                    POTermDetailId:poTermDetailId,
                    TermDesc: termDesc,
                    TermSequence: termSequence
                };
                poTermList.push(poTerm);
                termCounter += 1;
            }
        }

    });

    if (hdnTermSequence.val() == "" || hdnTermSequence.val() == "0") {
        hdnTermSequence.val(termCounter);
    }
    var poTermAddEdit = {
        POTermDetailId: hdnPOTermDetailId.val(),
        TermDesc: txtTermDesc.val().trim(),
        TermSequence: hdnTermSequence.val()
    };

    poTermList.push(poTermAddEdit);
    GetPOTermList(poTermList);

}
function EditTermRow(obj) {
    var row = $(obj).closest("tr");
    var poTermDetailId = $(row).find("#hdnPOTermDetailId").val();
    var termDesc = $(row).find("#hdnTermDesc").val();
    var termSequence = $(row).find("#hdnTermSequence").val();
    $("#txtTermDesc").val(termDesc);
    $("#hdnPOTermDetailId").val(poTermDetailId);
    $("#hdnTermSequence").val(termSequence);

    $("#btnAddTerm").hide();
    $("#btnUpdateTerm").show();
    ShowHideTermPanel(1);
}
function RemoveTermRow(obj) {
    if (confirm("Do you want to remove selected Term?")) {
        var row = $(obj).closest("tr");
        var poTermDetailId = $(row).find("#hdnPOTermDetailId").val();
        ShowModel("Alert", "Term Removed from List.");
        row.remove();
    }
}
function GetPOTaxList(poTaxes) {
    var hdnPOId = $("#hdnPOId");
    var requestData = { poTaxes: poTaxes, poId: hdnPOId.val() };
    $.ajax({
        url: "../PO/GetPOTaxList",
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
    var hdnPOTaxDetailId = $("#hdnPOTaxDetailId");
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

    var poTaxList = [];
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        taxEntrySequence = 1;
    }

    $('#tblTaxList tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var poTaxDetailId = $row.find("#hdnPOTaxDetailId").val();
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
            if (action == 1 || hdnPOTaxDetailId.val() != poTaxDetailId) {

                if (taxId == hdnTaxId.val()) {
                    ShowModel("Alert", "Tax already added!!!")
                    txtTaxName.focus();
                    flag = false;
                    return false;
                }

                var poTax = {
                    POTaxDetailId: poTaxDetailId,
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
                poTaxList.push(poTax);
                taxEntrySequence = parseInt(taxEntrySequence) + 1;
            }
            else if (hdnPOTaxDetailId.val() == poTaxDetailId && hdnTaxSequenceNo.val() == taxSequenceNo) {
                var poTax = {
                    POTaxDetailId: hdnPOTaxDetailId.val(),
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

                poTaxList.push(poTax);
                hdnTaxSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        hdnTaxSequenceNo.val(taxEntrySequence);
    }
    if (action == 1) {
        var poTaxAddEdit = {
            POTaxDetailId: hdnPOTaxDetailId.val(),
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

        poTaxList.push(poTaxAddEdit);
        hdnTaxSequenceNo.val("0");
    }
    if (flag == true) {
        GetPOTaxList(poTaxList);
    }

}
function EditTaxRow(obj) {

    var row = $(obj).closest("tr");
    var poTaxDetailId = $(row).find("#hdnPOTaxDetailId").val();
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
    $("#hdnPOTaxDetailId").val(poTaxDetailId);
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
        var poTaxDetailId = $(row).find("#hdnPOTaxDetailId").val();
        ShowModel("Alert", "Tax Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
    }
}

function GetPODetail(poId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PO/GetPODetail",
        data: { poId: poId },
        dataType: "json",
        success: function (data) {
            $("#txtPONo").val(data.PONo);
            $("#txtPODate").val(data.PODate);

            $("#hdnQuotationId").val(data.QuotationId);
            $("#txtQuotationNo").val(data.QuotationNo);
            $("#txtQuotationDate").val(data.QuotationDate);

            $("#txtIndentNo").val(data.IndentNo);
            $("#hdnIndentId").val(data.IndentId);
            $("#txtIndentDate").val(data.IndentDate);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtDeliveryDate").val(data.ExpectedDeliveryDate);

            $("#ddlCurrency").val(data.CurrencyCode);
            $("#hdnVendorId").val(data.VendorId);
            $("#txtVendorCode").val(data.VendorCode);
            $("#txtVendorName").val(data.VendorName);
            $("#hdnConsigneeId").val(data.ConsigneeId);
            $("#txtConsigneeCode").val(data.ConsigneeCode);
            $("#txtConsigneeName").val(data.ConsigneeName);
            $("#txtBAddress").val(data.BillingAddress);
            $("#txtShippingAddress").val(data.ShippingAddress);
            $("#txtBCity").val(data.City);
            $("#ddlBCountry").val(data.CountryId);
            
            $("#ddlBState").val(data.StateId);
            $("#ddlRevisedApprovalStatus").val(data.POStatus)
            $("#txtBPinCode").val(data.PinCode);

            $("#txtBTINNo").val(data.TINNo);

            $("#txtBGSTNo").val(data.GSTNo);

            $("#txtSCity").val(data.ShippingCity);
            $("#ddlSCountry").val(data.ShippingCountryId);
            $("#ddlSState").val(data.ShippingStateId);
            $("#txtSPinCode").val(data.ShippingPinCode);
            $("#txtSGSTNo").val(data.ConsigneeGSTNo);

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

            $("#ddlCompanyBranch").val(data.CompanyBranchId);

            if (data.ReverseChargeApplicable == true) {
                $("#chkReverseChargeApplicable").prop("checked", true);
                changeReverseChargeStatus();
            }
            $("#txtReverseChargeAmount").val(data.ReverseChargeAmount);

            $("#txtRemarks1").val(data.Remarks1)
            $("#txtRemarks2").val(data.Remarks2)
            $("#btnAddNew").show();
            $("#btnPrint").show();
            BindConsigneeBranchList(data.ConsigneeId);
            GetOtherChargesGSTPercentage();
          

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}


function SaveData() {
   
    var txtPONo = $("#txtPONo");
    var hdnPOId = $("#hdnPOId");
    var ddlCurrency = $("#ddlCurrency");
    var txtPODate = $("#txtPODate");
    var hdnVendorId = $("#hdnVendorId");
    var txtVendorName = $("#txtVendorName");
    var hdnConsigneeId = $("#hdnConsigneeId");
    var txtConsigneeName = $("#txtConsigneeName");
    var txtAddress = $("#txtBAddress");
    var txtShippingAddress = $("#txtShippingAddress");

    var txtCity = $("#txtBCity");
    var ddlCountry = $("#ddlBCountry");
    var ddlState = $("#ddlBState");
    var txtPinCode = $("#txtBPinCode");
    var txtTINNo = $("#txtBTINNo");
    var txtGSTNo = $("#txtBGSTNo");


    var txtSCity = $("#txtSCity");
    var ddlSCountry = $("#ddlSCountry");
    var ddlSState = $("#ddlSState");
    var txtSPinCode = $("#txtSPinCode");
    var txtSGSTNo = $("#txtSGSTNo");

    var txtRefNo = $("#txtRefNo");


    var ddlRevisedApprovalStatus = $("#ddlRevisedApprovalStatus");

    var txtRefDate = $("#txtRefDate");
    var txtBasicValue = $("#txtBasicValue");
    var txtTotalValue = $("#txtTotalValue");
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
    var chkReverseChargeApplicable = $("#chkReverseChargeApplicable");
    var txtReverseChargeAmount = $("#txtReverseChargeAmount");
    var txtRemarks1 = $("#txtRemarks1");
    var txtRemarks2 = $("#txtRemarks2");

    var txtDeliveryDate = $("#txtDeliveryDate");
    var hdnQuotationId = $("#hdnQuotationId");
    var txtQuotationNo = $("#txtQuotationNo");
    var hdnIndentId = $("#hdnIndentId");
    var txtIndentNo = $("#txtIndentNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (txtVendorName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Vendor Name")
        txtVendorName.focus();
        return false;
    }
    if (hdnVendorId.val() == "" || hdnVendorId.val() == "0") 
    {
        ShowModel("Alert", "Please select vendor from list")
        txtVendorName.focus();
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

    if (txtAddress.val().trim() == "") {
        ShowModel("Alert", "Please Enter Vendor Billing Address")
        txtAddress.focus();
        return false;
    }
    if (txtShippingAddress.val().trim() == "") {
        ShowModel("Alert", "Please Enter Vendor Shipping Address")
        txtShippingAddress.focus();
        return false;
    }
    if (txtCity.val().trim() == "") {
        ShowModel("Alert", "Please enter billing city")
        txtCity.focus();
        return false;
    }
    if (ddlCountry.val() == "" || ddlCountry.val() == "0") {
        ShowModel("Alert", "Please select billing country")
        ddlCountry.focus();
        return false;
    }
    if (ddlState.val() == "" || ddlState.val() == "0") {
        ShowModel("Alert", "Please select billing State")
        ddlState.focus();
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


    var poViewModel = {
        POId: hdnPOId.val(),
        PONo: txtPONo.val().trim(),
        PODate: txtPODate.val().trim(),
        IndentId: hdnIndentId.val(),
        IndentNo: txtIndentNo.val().trim(),
        QuotationId: hdnQuotationId.val(),
        QuotationNo: txtQuotationNo.val().trim(),
        CurrencyCode: ddlCurrency.val().trim(),
        VendorId: hdnVendorId.val().trim(),
        VendorName: txtVendorName.val().trim(),
        ConsigneeId: hdnConsigneeId.val().trim(),
        ConsigneeName: txtConsigneeName.val().trim(),
        BillingAddress: txtAddress.val().trim(),
        ShippingAddress: txtShippingAddress.val().trim(),
        City: txtCity.val().trim(),
        StateId: ddlState.val(),
        CountryId: ddlCountry.val(),
        ApprovalStatus:ddlRevisedApprovalStatus.val(),
        PinCode: txtPinCode.val().trim(),
        TINNo: txtTINNo.val().trim(),
        GSTNo: txtGSTNo.val().trim(),
        ShippingCity: txtSCity.val().trim(),
        ShippingStateId: ddlSState.val(),
        ShippingCountryId: ddlSCountry.val(),
        ShippingPinCode: txtSPinCode.val().trim(),
        ConsigneeGSTNo: txtSGSTNo.val().trim(),
        RefNo: txtRefNo.val().trim(),
        RefDate: txtRefDate.val(),
        BasicValue: txtBasicValue.val(),
        TotalValue: txtTotalValue.val(),
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
        Remarks1:txtRemarks1.val(),
        Remarks2: txtRemarks2.val(),
        ExpectedDeliveryDate: txtDeliveryDate.val(),
        CompanyBranchID: ddlCompanyBranch.val()
        
    };

    var poProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var poProductDetailId = $row.find("#hdnPOProductDetailId").val();
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


        if (productName != undefined) {

            var poProduct = {
                POProductDetailId: poProductDetailId,
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
                HSN_Code: hsn_Code
            };
            poProductList.push(poProduct);
        }
    });

    var poTaxList = [];
    $('#tblTaxList tr').each(function (i, row) {
        var $row = $(row);
        var poTaxDetailId = $row.find("#hdnPOTaxDetailId").val();
        var taxId = $row.find("#hdnTaxId").val();
        var taxName = $row.find("#hdnTaxName").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();
        var taxAmount = $row.find("#hdnTaxAmount").val();

        var surchargeName_1 = $row.find("#hdnSurchargeName_2").val();
        var surchargePercentage_1 = $row.find("#hdnSurchargePercentage_1").val();
        var surchargeAmount_1 = $row.find("#hdnSurchargeAmount_1").val();

        var surchargeName_2 = $row.find("#hdnSurchargeName_2").val();
        var surchargePercentage_2 = $row.find("#hdnSurchargePercentage_2").val();
        var surchargeAmount_2 = $row.find("#hdnSurchargeAmount_2").val();

        var surchargeName_3 = $row.find("#hdnSurchargeName_3").val();
        var surchargePercentage_3 = $row.find("#hdnSurchargePercentage_3").val();
        var surchargeAmount_3 = $row.find("#hdnSurchargeAmount_3").val();

        if (taxName != undefined) {
            var poTax = {
                poTaxDetailId: poTaxDetailId,
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
            poTaxList.push(poTax);
        }

    });

    var poTermList = [];
    $('#tblTermList tr').each(function (i, row) {
        var $row = $(row);
        var poTermDetailId = $row.find("#hdnPOTermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (poTermDetailId != undefined) {
            var poTerm = {
                POTermDetailId: poTermDetailId,
                TermDesc: termDesc,
                TermSequence: termSequence
            };
            poTermList.push(poTerm);
        }

    });

    var RevisedPODocumentList = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var poDocId = $row.find("#hdnPODocId").val();
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        if (poDocId != undefined) {
            var revisedPODocument = {
                PODocId: poDocId,
                DocumentSequenceNo: documentSequenceNo,
                DocumentTypeId: documentTypeId,
                DocumentTypeDesc: documentTypeDesc,
                DocumentName: documentName,
                DocumentPath: documentPath
            };
            RevisedPODocumentList.push(revisedPODocument);
        }

    });
    var accessMode = 1//Add Mode
    if (hdnPOId.val() != null && hdnPOId.val() != 0) {
        accessMode = 5;//Edit Mode
    }

    var requestData = { poViewModel: poViewModel, poProducts: poProductList, poTaxes: poTaxList, poTerms: poTermList, revisedPODocuments: RevisedPODocumentList };
    $.ajax({
        url: "../PO/AddRevisedPO?accessMode=" + accessMode + "",
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
                      window.location.href = "../PO/AddRevisedPO?POId=" + data.trnId + "&AccessMode=2";
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

    $("#txtPONo").val("");
    $("#hdnPOId").val("0");
    $("#txtPODate").val($("#hdnCurrentDate").val());
    $("#hdnVendorId").val("0");
    $("#txtVendorName").val("");
    $("#txtVendorCode").val("");
    $("#txtBAddress").val("");
    $("#txtBCity").val("");
    $("#ddlBCountry").val("0");
    $("#ddlBState").val("0");
    $("#txtBPinCode").val("");
    $("#ddlRevisedApprovalStatus").val("Draft");

    
    $("#txtBTINNo").val("");
    $("#txtBGSTNo").val("");
    $("#txtRefNo").val("");
    $("#txtRefDate").val("");

    $("#btnSave").show();
    


}
function GetVendorDetail(vendorId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Vendor/GetVendorDetail",
        data: { vendorId: vendorId },
        dataType: "json",
        success: function (data) {
            $("#txtBAddress").val(data.Address);
            $("#txtBCity").val(data.City);
            $("#ddlBCountry").val(data.CountryId);
            
            $("#ddlBState").val(data.StateId);
            $("#txtBPinCode").val(data.PinCode);

            $("#txtBTINNo").val(data.TINNo);

            $("#txtBGSTNo").val(data.GSTNo);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function GetConsigneeDetail(consigneeId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Customer/GetCustomerDetail",
        data: { customerId: consigneeId },
        dataType: "json",
        success: function (data) {
            $("#txtShippingAddress").val(data.PrimaryAddress);
            $("#txtSCity").val(data.City);
            $("#ddlSCountry").val(data.CountryId);
            $("#ddlSState").val(data.StateId);
            $("#txtSPinCode").val(data.PinCode);
            $("#txtSGSTNo").val(data.GSTNo);

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    BindConsigneeBranchList(consigneeId)
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
        $("#hdnPOProductDetailId").val("0");
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtPrice").val("");
        $("#txtUOMName").val("");
        $("#txtQuantity").val("");
        $("#txtTotalPrice").val("");
        $("#txtHSN_Code").val("");
        $("#txtCGSTPerc").val("");
        $("#txtCGSTPercAmount").val("");
        $("#txtSGSTPerc").val("");
        $("#txtSGSTPercAmount").val("");
        $("#txtIGSTPerc").val("");
        $("#txtIGSTPercAmount").val("");


        $("#txtDiscountPerc").val("");
        $("#txtDiscountAmount").val("");
        $("#txtProductTaxName").val("");
        $("#hdnProductTaxId").val("");
        $("#hdnProductTaxPerc").val("");
        $("#txtProductTaxAmount").val("");

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
        $("#hdnPOTaxDetailId").val("0");
        $("#txtTaxPercentage").val("");
        $("#txtTaxAmount").val("");
        $("#btnAddTax").show();
        $("#btnUpdateTax").hide();
    }
}
function validateStateSelection(action) {
    var hdnCustomerStateId = $("#ddlSState");
    var hdnBillingStateId = $("#ddlBState");

    if (hdnCustomerStateId.val() == "0") {
        ShowModel("Alert", "Please Select Consignee")
        return false;
    }
    else if (hdnBillingStateId.val() == "0") {
        ShowModel("Alert", "Please Select Vendor")
        return false;
    }
    ShowHideProductPanel(action);
}
function ShowHideTermPanel(action) {
    if (action == 1) {
        $(".termsection").show();
    }
    else {
        $(".termsection").hide();
        $("#txtTermDesc").val("");
        $("#hdnPOTermDetailId").val("0");
        $("#hdnTermSequence").val("0");
        $("#btnAddTerm").show();
        $("#btnUpdateTerm").hide();
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


            $("#txtShippingAddress").val(data.PrimaryAddress);
            $("#txtSCity").val(data.City);
            $("#ddlSCountry").val(data.CountryId);
            $("#ddlSState").val(data.StateId);
            $("#txtSPinCode").val(data.PinCode);
            $("#txtSGSTNo").val(data.GSTNo);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
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
    var hdnBillingStateId = $("#ddlBState");

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
    var hdnBillingStateId = $("#ddlBState");

    //if (hdnCustomerStateId.val() == "0") {
    //    ShowModel("Alert", "Please Select Consignee")
    //    return false;
    //}
    //else if (hdnBillingStateId.val() == "0") {
    //    ShowModel("Alert", "Please Select Vendor")
    //    return false;
    //}


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
    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(freightValue) + parseFloat(freightCGST_Amount) + parseFloat(freightSGST_Amount) + parseFloat(freightIGST_Amount) + parseFloat(loadingValue) + parseFloat(loadingCGST_Amount) + parseFloat(loadingSGST_Amount) + parseFloat(loadingIGST_Amount) + parseFloat(insuranceValue) + parseFloat(insuranceCGST_Amount) + parseFloat(insuranceSGST_Amount) + parseFloat(insuranceIGST_Amount)).toFixed(0));
}

function ExecuteSave() {
    SetGSTPercentageInProduct();
    setTimeout(
function () {
    SaveData();
}, 2000);

}
function CalculateLoadingTotalCharges() {
    var hdnBillingStateId = $("#ddlBState").val();
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
    var hdnBillingStateId = $("#ddlBState").val();
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

function CalculateInsuranceTotalCharges() {
    var hdnBillingStateId = $("#ddlBState").val();
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