/// <reference path="AddPurchaseQuotation.js" />
$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtQuotationNo").attr('readOnly', true);
    $("#txtQuotationDate").attr('readOnly', true); 
    $("#txtCGSTPerc").attr('readOnly', true);
    $("#txtSGSTPerc").attr('readOnly', true);
    $("#txtIGSTPerc").attr('readOnly', true);
    $("#txtCGSTAmount").attr('readOnly', true);
    $("#txtSGSTAmount").attr('readOnly', true);
    $("#txtIGSTAmount").attr('readOnly', true);
    $("#txtHSN_Code").attr('readOnly', true);
    $("#txtVendorCode").attr('readOnly', true);
    $("#txtRefDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtAvailableStock").attr('readOnly', true);       
    $("#txtTotalPrice").attr('readOnly', true);
    $("#txtBasicValue").attr('readOnly', true);
    $("#txtTaxPercentage").attr('readOnly', true);
    $("#txtTaxAmount").attr('readOnly', true);
    $("#txtTotalValue").attr('readOnly', true);
    $("#txtDiscountAmount").attr('readOnly', true);
    $("#txtPurchaseIndentDate").attr('readOnly', true);
    $("#txtPurchaseIndentNo").attr('readOnly', true);
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
  
    BindDocumentTypeList();
    
   
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
            var hdnCustomerStateId = $("#ddlState");
            var hdnBillingStateId = $("#hdnBillingStateId");
            if (hdnCustomerStateId.val() == hdnBillingStateId.val()) {
                $("#txtCGSTPerc").val(ui.item.CGST_Perc);
                $("#txtSGSTPerc").val(ui.item.SGST_Perc);
                $("#txtIGSTPerc").val(0);
                $("#txtCGSTAmount").val(0);
                $("#txtSGSTAmount").val(0);
                $("#txtIGSTAmount").val(0);

            }
            else {
                $("#txtCGSTPerc").val(0);
                $("#txtSGSTPerc").val(0);
                $("#txtIGSTPerc").val(ui.item.IGST_Perc);
                $("#txtCGSTAmount").val(0);
                $("#txtSGSTAmount").val(0);
                $("#txtIGSTAmount").val(0);
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
                $("#txtCGSTAmount").val(0);
                $("#txtSGSTAmount").val(0);
                $("#txtIGSTAmount").val(0);
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

    $("#txtQuotationDate,#txtRefDate,#txtRequisitionDate,#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    BindCompanyBranchList();
    BindCurrencyList();
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnQuotationId = $("#hdnQuotationId");
    if (hdnQuotationId.val() != "" && hdnQuotationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetQuotationDetail(hdnQuotationId.val());
           $("#ddlCompanyBranch").attr('disabled', true);
       }, 2000);



        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $(".editonly").hide();
            if ($(".editonly").hide()) {
                $('#lblPurchaseIndentDate').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
            }
        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
            $(".editonly").show();
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
        $(".editonly").show();

        if ($("#hdnVendorId").val() != "0") {
           // GetCustomerDetail($("#hdnCustomerId").val());

        }
    }

    var quotationProducts = [];
    GetQuotationProductList(quotationProducts);
  
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

function GetQuotationProductList(quotationProducts) {
    var hdnQuotationId = $("#hdnQuotationId");
    var requestData = { quotationProducts: quotationProducts, quotationId: hdnQuotationId.val() };
    $.ajax({
        url: "../PurchaseQuotation/GetPurchaseQuotationProductList",
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
    $("#txtCGSTAmount").val(CGSTAmount.toFixed(2));

    if (parseFloat(SGSTPerc) > 0) {

        SGSTAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(SGSTPerc)) / 100;
    }
    $("#txtSGSTAmount").val(SGSTAmount.toFixed(2));

    if (parseFloat(IGSTPerc) > 0) {

        IGSTAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(IGSTPerc)) / 100;
    }
    $("#txtIGSTAmount").val(IGSTAmount.toFixed(2));


   

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

    $("#txtTotalPrice").val((totalPrice - discountAmount).toFixed(2));


}


function CalculateGrossandNetValues() {
    var basicValue = 0;
  
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var totalPrice = $row.find("#hdnTotalPrice").val();
        if (totalPrice != undefined) {
            basicValue += parseFloat(totalPrice);
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
    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(freightValue) + parseFloat(freightCGST_Amt) + parseFloat(freightSGST_Amt) + parseFloat(freightIGST_Amt) + parseFloat(loadingValue) + parseFloat(loadingCGST_Amt) + parseFloat(loadingSGST_Amt) + parseFloat(loadingIGST_Amt) + parseFloat(insuranceValue) + parseFloat(insuranceCGST_Amt) + parseFloat(insuranceSGST_Amt) + parseFloat(insuranceIGST_Amt)).toFixed(0));
}


function CalculateLoadingTotalCharges() {
    var hdnBillingStateId = $("#hdnBillingStateId").val();
    var ddlState = $("#ddlState").val();

    if (ddlState == "0" || ddlState == "") {
        ShowModel("Alert", "Please Select Party")
        $("#txtLoadingValue").val(0);
        return false;
    }
    else if (hdnBillingStateId == "" || hdnBillingStateId == "0") {
        ShowModel("Alert", "Please Select Billing Location");
        $("#txtLoadingValue").val(0);
        return false;
    }

    var loadingValue = $("#txtLoadingValue").val();
    var loadingCGST_Perc = $("#hdnLoadingCGST_Perc").val();
    var loadingSGST_Perc = $("#hdnLoadingSGST_Perc").val();
    var loadingIGST_Perc = $("#hdnLoadingIGST_Perc").val();

    loadingValue = loadingValue == "" ? 0 : loadingValue;
    if (hdnBillingStateId == ddlState) {
        loadingCGST_Perc = loadingCGST_Perc == "" ? 0 : loadingCGST_Perc;
        loadingSGST_Perc = loadingSGST_Perc == "" ? 0 : loadingSGST_Perc;
        loadingIGST_Perc = 0;
    }
    else {
        loadingCGST_Perc = 0;
        loadingSGST_Perc = 0;
        loadingIGST_Perc = loadingIGST_Perc == "" ? 0 : loadingIGST_Perc;
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
    var ddlState = $("#ddlState").val();

    if (ddlState == "0" || ddlState == "") {
        ShowModel("Alert", "Please Select Party ")
        $("#txtFreightValue").val(0);
        return false;
    }
    else if (hdnBillingStateId == "" || hdnBillingStateId == "0") {
        ShowModel("Alert", "Please Select Billing Location");
        $("#txtFreightValue").val(0);
        return false;
    }

    var freightValue = $("#txtFreightValue").val();
    var freightCGST_Perc = $("#hdnFreightCGST_Perc").val();
    var freightSGST_Perc = $("#hdnFreightSGST_Perc").val();
    var freightIGST_Perc = $("#hdnFreightIGST_Perc").val();

    freightValue = freightValue == "" ? 0 : freightValue;
    if (hdnBillingStateId == ddlState) {
        freightCGST_Perc = freightCGST_Perc == "" ? 0 : freightCGST_Perc;
        freightSGST_Perc = freightSGST_Perc == "" ? 0 : freightSGST_Perc;
        freightIGST_Perc = 0;
    }
    else {
        freightCGST_Perc = 0;
        freightSGST_Perc = 0;
        freightIGST_Perc = freightIGST_Perc == "" ? 0 : freightIGST_Perc;
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
    var ddlState = $("#ddlState").val();

    if (ddlState == "0" || ddlState == "") {
        ShowModel("Alert", "Please Select Party")
        $("#txtInsuranceValue").val(0);
        return false;
    }
    else if (hdnBillingStateId == "" || hdnBillingStateId == "0") {
        ShowModel("Alert", "Please Select Billing Location");
        $("#txtInsuranceValue").val(0);
        return false;
    }

    var insuranceValue = $("#txtInsuranceValue").val();
    var insuranceCGST_Perc = $("#hdnInsuranceCGST_Perc").val();
    var insuranceSGST_Perc = $("#hdnInsuranceSGST_Perc").val();
    var insuranceIGST_Perc = $("#hdnInsuranceIGST_Perc").val();

    insuranceValue = insuranceValue == "" ? 0 : insuranceValue;
    if (hdnBillingStateId == ddlState) {
        insuranceCGST_Perc = insuranceCGST_Perc == "" ? 0 : insuranceCGST_Perc;
        insuranceSGST_Perc = insuranceSGST_Perc == "" ? 0 : insuranceSGST_Perc;
        insuranceIGST_Perc = 0;
    }
    else {
        insuranceCGST_Perc = 0;
        insuranceSGST_Perc = 0;
        insuranceIGST_Perc = insuranceIGST_Perc == "" ? 0 : insuranceIGST_Perc;
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


function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtProductName = $("#txtProductName");
    var hdnQuotationProductDetailId = $("#hdnQuotationProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtPrice = $("#txtPrice");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var txtDiscountPerc = $("#txtDiscountPerc");
    var txtDiscountAmount = $("#txtDiscountAmount");
    var txtTotalPrice = $("#txtTotalPrice");
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtCGSTPerc = $("#txtCGSTPerc");
    var txtCGSTAmount = $("#txtCGSTAmount");
    var txtSGSTPerc = $("#txtSGSTPerc");
    var txtSGSTAmount = $("#txtSGSTAmount");
    var txtIGSTPerc = $("#txtIGSTPerc");
    var txtIGSTAmount = $("#txtIGSTAmount");
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

    var quotationProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var quotationProductDetailId = $row.find("#hdnQuotationProductDetailId").val();
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
        var totalPrice = $row.find("#hdnTotalPrice").val();

        var cGSTPerc = $row.find("#hdnCGSTPerc").val();
        var cGSTAmount = $row.find("#hdnCGSTAmount").val();
        var sGSTPerc = $row.find("#hdnSGSTPerc").val();
        var sGSTAmount = $row.find("#hdnSGSTAmount").val();
        var iGSTPerc = $row.find("#hdnIGSTPerc").val();
        var iGSTAmount = $row.find("#hdnIGSTAmount").val();
        var hsn_Code = $row.find("#hdnHSN_Code").val();
        
        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var quotationProduct = {
                    QuotationProductDetailId: quotationProductDetailId,
                    SequenceNo:sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Price: price,
                    Quantity: quantity,
                    DiscountPercentage: discountPerc,
                    DiscountAmount: discountAmount,
                    TotalPrice: totalPrice,
                    CGST_Perc: cGSTPerc,
                    CGST_Amount: cGSTAmount,
                    SGST_Perc: sGSTPerc,
                    SGST_Amount: sGSTAmount,
                    IGST_Perc: iGSTPerc,
                    IGST_Amount: iGSTAmount,
                    HSN_Code: hsn_Code
                  
                };
                quotationProductList.push(quotationProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
                
            }
            else if (hdnQuotationProductDetailId.val() == quotationProductDetailId && hdnSequenceNo.val()==sequenceNo)
            {
                var quotationProduct = {
                    QuotationProductDetailId: hdnQuotationProductDetailId.val(),
                    SequenceNo:hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Price: txtPrice.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    DiscountPercentage: txtDiscountPerc.val().trim(),
                    DiscountAmount: txtDiscountAmount.val().trim(),
                    TotalPrice: txtTotalPrice.val().trim(),
                    CGST_Perc: txtCGSTPerc.val(),
                    CGST_Amount: txtCGSTAmount.val(),
                    SGST_Perc: txtSGSTPerc.val(),
                    SGST_Amount: txtSGSTAmount.val(),
                    IGST_Perc: txtIGSTPerc.val(),
                    IGST_Amount: txtIGSTAmount.val(),
                    HSN_Code: txtHSN_Code.val()
                   
                };
                quotationProductList.push(quotationProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var quotationProductAddEdit = {
            QuotationProductDetailId: hdnQuotationProductDetailId.val(),
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
            TotalPrice: txtTotalPrice.val().trim(),
            CGST_Perc: txtCGSTPerc.val(),
            CGST_Amount: txtCGSTAmount.val(),
            SGST_Perc: txtSGSTPerc.val(),
            SGST_Amount: txtSGSTAmount.val(),
            IGST_Perc: txtIGSTPerc.val(),
            IGST_Amount: txtIGSTAmount.val(),
            HSN_Code: txtHSN_Code.val()
          };
        quotationProductList.push(quotationProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetQuotationProductList(quotationProductList);
    }

}

function EditProductRow(obj) {

    var hdnAccessMode = $("#hdnAccessMode");
    if (hdnAccessMode.val() == "3") {
        ShowModel("Alert", "You can't modify in view mode.")
        return false;
    }
    else {
        var row = $(obj).closest("tr");
        var quotationProductDetailId = $(row).find("#hdnQuotationProductDetailId").val();
        var sequenceNo = $(row).find("#hdnSequenceNo").val();
        var productId = $(row).find("#hdnProductId").val();
        var productName = $(row).find("#hdnProductName").val();
        var productCode = $(row).find("#hdnProductCode").val();
        var productShortDesc = $(row).find("#hdnProductShortDesc").val();
        var uomName = $(row).find("#hdnUOMName").val();
        var price = $(row).find("#hdnPrice").val();
        var quantity = $(row).find("#hdnQuantity").val();
        var totalPrice = $(row).find("#hdnTotalPrice").val();
        var discountPerc = $(row).find("#hdnDiscountPerc").val();
        var discountAmount = $(row).find("#hdnDiscountAmount").val();

        var cGSTPerc = $(row).find("#hdnCGSTPerc").val();
        var cGSTAmount = $(row).find("#hdnCGSTAmount").val();
        var sGSTPerc = $(row).find("#hdnSGSTPerc").val();
        var sGSTAmount = $(row).find("#hdnSGSTAmount").val();
        var iGSTPerc = $(row).find("#hdnIGSTPerc").val();
        var iGSTAmount = $(row).find("#hdnIGSTAmount").val();
        var hsn_Code = $(row).find("#hdnHSN_Code").val();


        $("#hdnQuotationProductDetailId").val(quotationProductDetailId);
        $("#hdnSequenceNo").val(sequenceNo);
        $("#hdnProductId").val(productId);
        $("#txtProductCode").val(productCode);
        $("#txtProductName").val(productName);

        $("#txtProductShortDesc").val(productShortDesc);
        $("#txtUOMName").val(uomName);
        $("#txtPrice").val(price);
        $("#txtQuantity").val(quantity);
        $("#txtTotalPrice").val(totalPrice);
        $("#txtDiscountPerc").val(discountPerc);
        $("#txtDiscountAmount").val(discountAmount);

        $("#txtTotalPrice").val(totalPrice);
        $("#txtCGSTPerc").val(cGSTPerc);
        $("#txtCGSTAmount").val(cGSTAmount);
        $("#txtSGSTPerc").val(sGSTPerc);
        $("#txtSGSTAmount").val(sGSTAmount);
        $("#txtIGSTPerc").val(iGSTPerc);
        $("#txtIGSTAmount").val(iGSTAmount);
        $("#txtHSN_Code").val(hsn_Code);
        $("#btnAddProduct").hide();
        $("#btnUpdateProduct").show();
        ShowHideProductPanel(1);
    }

}
function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var quotationProductDetailId = $(row).find("#hdnQuotationProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
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
function BindStateList(stateId) {
    var countryId = $("#ddlCountry option:selected").val();
    $("#ddlState").val(0);
    $("#ddlState").html("");
    if (countryId != undefined && countryId != "" && countryId != "0") {
        var data = { countryId: countryId };
        $.ajax({
            type: "GET",
            url: "../Company/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                $("#ddlState").val(stateId);
            },
            error: function (Result) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
    }
    else {

        $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    }

}

function GetQuotationDetail(quotationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PurchaseQuotation/GetPurchaseQuotationDetail",
        data: { quotationId: quotationId },
        dataType: "json",
        success: function (data) {
            $("#txtQuotationNo").val(data.QuotationNo);
            $("#txtQuotationDate").val(data.QuotationDate);
            $("#hdnPurchaseIndentId").val(data.RequisitionId);
            $("#txtPurchaseIndentNo").val(data.RequisitionNo);
            $("#txtPurchaseIndentDate").val(data.RequisitionDate);
            $("#hdnVendorId").val(data.VendorId);
            $("#txtVendorCode").val(data.VendorCode);
            $("#txtVendorName").val(data.VendorName);
            $("#txtDeliveryDays").val(data.DeliveryDays);
            $("#txtDeliveryAt").val(data.DeliveryAt);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#ddlCurrency").val(data.CurrencyCode);
            $("#ddlApprovalStatus").val(data.ApprovalStatus);
            $("#txtRefNo").val(data.RefNo);
            $("#txtRefDate").val(data.RefDate);
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUser);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUser != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUser);
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
            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);

            $("#btnAddNew").show();
            $("#btnPrint").show();
            //$("#btnEmail").show();
            GetBranchStateId();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
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

function ExecuteSave()
{
    //SetGSTPercentageInProduct();
    setTimeout(
function () {
    SaveData();
    }, 100);
  
}

function SaveData() {
    var txtQuotationNo = $("#txtQuotationNo");
    var hdnQuotationId = $("#hdnQuotationId");
    var txtQuotationDate = $("#txtQuotationDate");
    var hdnPurchaseIndentId = $("#hdnPurchaseIndentId");
    var txtPurchaseIndentNo = $("#txtPurchaseIndentNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlCurrency = $("#ddlCurrency");
    var hdnVendorId = $("#hdnVendorId");
    var txtVendorName = $("#txtVendorName");
    var txtDeliveryDays = $("#txtDeliveryDays")
    var txtDeliveryAt = $("#txtDeliveryAt");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtRefNo = $("#txtRefNo");
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
    
    var txtRemarks1 = $("#txtRemarks1");
    var txtRemarks2 = $("#txtRemarks2");

    if (txtVendorName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Vendor Name")
        txtVendorName.focus();
        return false;
    }
    if (hdnVendorId.val() == "" || hdnVendorId.val() == "0") {
        ShowModel("Alert", "Please select Vendor from list")
        txtVendorName.focus();
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Company Branch");
        ddlCompanyBranch.focus();
        return false;
    }
    if (ddlCurrency.val() == "" || ddlCurrency.val() == "0")
    {
        ShowModel("Alert", "Please Select Currency");
        ddlCurrency.focus();
        return false;
    }
    if (txtDeliveryDays.val().trim() == "" || txtDeliveryDays.val()=="0")
    {
        ShowModel("Alert", "Please Enter Delivery Days")
        txtDeliveryDays.focus();
        return false;
    }

    if (txtDeliveryAt.val().trim() == "") {
        ShowModel("Alert", "Please Enter Delivery At")
        txtDeliveryAt.focus();
        return false;
    }
  
  if (txtBasicValue.val() == "" || parseFloat(txtBasicValue.val()) <=0) {
        ShowModel("Alert", "Please select at least one Product")
        return false;
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


    var quotationViewModel = {
        QuotationId: hdnQuotationId.val(),
        QuotationNo: txtQuotationNo.val().trim(),
        QuotationDate: txtQuotationDate.val().trim(),
        RequisitionId: hdnPurchaseIndentId.val(),
        RequisitionNo: txtPurchaseIndentNo.val().trim(),
        CompanyBranchId: ddlCompanyBranch.val().trim(),
        CurrencyCode: ddlCurrency.val().trim(),
        VendorId: hdnVendorId.val().trim(),
        VendorName: txtVendorName.val().trim(),
        DeliveryDays:txtDeliveryDays.val(),
        DeliveryAt: txtDeliveryAt.val().trim(),
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
        Remarks1: txtRemarks1.val(),
        Remarks2: txtRemarks2.val(),
        ApprovalStatus: ddlApprovalStatus.val(),
    };

    var quotationProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var quotationProductDetailId = $row.find("#hdnQuotationProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var totalPrice = $row.find("#hdnTotalPrice").val();
        var discountPerc = $row.find("#hdnDiscountPerc").val();
        var discountAmount = $row.find("#hdnDiscountAmount").val();
        var cGSTPerc = $row.find("#hdnCGSTPerc").val();
        var cGSTAmount = $row.find("#hdnCGSTAmount").val();

        var sGSTPerc = $row.find("#hdnSGSTPerc").val();
        var sGSTAmount = $row.find("#hdnSGSTAmount").val();

        var iGSTPerc = $row.find("#hdnIGSTPerc").val();
        var iGSTAmount = $row.find("#hdnIGSTAmount").val();
        var hsn_Code = $row.find("#hdnHSN_Code").val();
        if (quotationProductDetailId != undefined) {

            var quotationProduct = {
                QuotationProductDetailId: quotationProductDetailId,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc,
                UOMName: uomName,
                Price: price,
                Quantity: quantity,
                DiscountPercentage: discountPerc,
                DiscountAmount: discountAmount,
                TotalPrice: totalPrice,
                CGST_Perc: cGSTPerc,
                CGST_Amount: cGSTAmount,
                SGST_Perc: sGSTPerc,
                SGST_Amount: sGSTAmount,
                IGST_Perc: iGSTPerc,
                IGST_Amount: iGSTAmount,
                HSN_Code: hsn_Code
            };
            quotationProductList.push(quotationProduct);
        }
    });

    var accessMode = 1;//Add Mode
    if (hdnQuotationId.val() != null && hdnQuotationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }


    var requestData = { quotationViewModel: quotationViewModel, quotationProducts: quotationProductList };
    $.ajax({
        url: "../PurchaseQuotation/AddEditPurchaseQuotation?accessMode=" + accessMode + "",
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
                       window.location.href = "../PurchaseQuotation/AddEditPurchaseQuotation?QuotationId=" + data.trnId + "&AccessMode=3";
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

    $("#txtQuotationNo").val("");
    $("#hdnQuotationId").val("0");
    $("#txtQuotationDate").val($("#hdnCurrentDate").val());
    $("#hdnVendorId").val("0");
    $("#txtVendorName").val("");
    $("#txtVendorCode").val("");
    $("#txtDeliveryDays").val("");
    $("#txtDeliveryAt").val("");
    $("#ddlApprovalStatus").val("Draft");
    $("#ddlCompanyBranch").val("0");
    $("#btnSave").show();
    $("#btnUpdate").hide();
}

function GetProductDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productid: productId },
        dataType: "json",
        success: function (data) {
            $("#txtPrice").val(data.PurchasePrice);
            $("#txtUOMName").val(data.PurchaseUOMName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}


function ShowHideProductPanel(action) {
    if (action == 1) {
        $(".productsection").show();
        if ($("#hdnProductId").val() != "0") {
            $("#txtProductName").attr('readOnly', true);
        }
        else {
            $("#txtProductName").attr('readOnly', false);
        }
        $(".productsection").show();
    }
    else {
        $(".productsection").hide();
        $("#txtProductName").val("");
        $("#hdnProductId").val("0");
        $("#hdnQuotationProductDetailId").val("0");
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtPrice").val("");
        $("#txtUOMName").val("");
        $("#txtQuantity").val("");
        $("#txtTotalPrice").val("");
        $("#txtDiscountPerc").val("");
        $("#txtDiscountAmount").val("");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
        $("#txtCGSTPerc").val("");
        $("#txtCGSTAmount").val("");
        $("#txtSGSTPerc").val("");
        $("#txtSGSTAmount").val("");
        $("#txtIGSTPerc").val("");
        $("#txtIGSTAmount").val("");
        $("#txtHSN_Code").val("");



    }
}

function validateStateSelection(action) {
    var hdnCustomerStateId = $("#ddlState");
    var hdnBillingStateId = $("#hdnBillingStateId");

    if (hdnCustomerStateId.val() == "0") {
        ShowModel("Alert", "Please Select Party State")
        return false;
    }
    //else if (hdnBillingStateId.val() == "0") {
    //    ShowModel("Alert", "Please Select Billing Location")
    //    return false;
    //}
    ShowHideProductPanel(action);
}

function SendMail()
{
    var hdnQuotationId = $("#hdnQuotationId");
    var requestData = { quotationId: hdnQuotationId.val(), reportType: "PDF" };
    $.ajax({
        url: "../Quotation/QuotationMail",
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

function OpenPurchaseIndentSearchPopup() {
    if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
        ShowModel("Alert", "Please Select Company Branch");
        return false;
    }
    $("#divList").html("");
    $("#SearchPurchaseIndentModel").modal();
    BindSearchCompanyBranchList1();
}

function BindSearchCompanyBranchList1() {
    $("#ddlSearchCompanyBranch1").val(0);
    $("#ddlSearchCompanyBranch1").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSearchCompanyBranch1").append($("<option></option>").val(0).html("-Select Company Branch1-"));
            $.each(data, function (i, item) {
                $("#ddlSearchCompanyBranch1").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId1 = $("#hdnSessionCompanyBranchId1");
            var hdnSessionUserID1 = $("#hdnSessionUserID1");
            if (hdnSessionCompanyBranchId1.val() != "0" && hdnSessionUserID1.val() != "2") {
                $("#ddlSearchCompanyBranch1").val(hdnSessionCompanyBranchId1.val());
                $("#ddlSearchCompanyBranch1").attr('disabled', true);
            }
        },
        error: function (Result) {
            $("#ddlSearchCompanyBranch1").append($("<option></option>").val(0).html("-Select Company Branch1-"));
        }
    });
}

function SelectPurchaseIndent(purchaseIndentId, purchaseIndentNo, purchaseIndentDate) {
    $("#txtPurchaseIndentNo").val(purchaseIndentNo);
    $("#hdnPurchaseIndentId").val(purchaseIndentId);
    $("#txtPurchaseIndentDate").val(purchaseIndentDate);
    $("#ddlCompanyBranch").attr('disabled', true);
    var purchaseIndentProducts = [];
    GetPurchaseIndentProductList(purchaseIndentProducts, purchaseIndentId);
    $("#SearchPurchaseIndentModel").modal('hide');
}
function GetPurchaseIndentProductList(purchaseIndentProducts, purchaseIndentId) {
    var requestData = { purchaseIndentProducts: purchaseIndentProducts, purchaseIndentId: purchaseIndentId };
    $.ajax({
        url: "../PurchaseQuotation/GetPurchaseIndentProductList",
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
            $("#divProductList").html("");
            $("#divProductList").html(data);
            CalculateGrossandNetValues();
            ShowHideProductPanel(2);
        }
    });
}
function SearchPurchaseIndent() {
    var txtIndentNo = $("#txtIndentNo");
    var ddlIndentType = $("#ddlIndentType");
    var txtCustomerName = $("#txtCustomerName");
    var ddlSearchCompanyBranch = $("#ddlCompanyBranch");//$("#ddlSearchCompanyBranch");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var requestData = { indentNo: txtIndentNo.val().trim(), indentType: ddlIndentType.val(), customerName: txtCustomerName.val().trim(), companyBranchId: ddlSearchCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../PurchaseQuotation/GetPurchaseIndentList",
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

function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../PurchaseQuotation/AddEditPurchaseQuotation";
    }
}