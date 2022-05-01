$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtPINo").attr('readOnly', true);
    $("#txtPIDate").attr('readOnly', true);
    $("#txtVendorCode").attr('readOnly', true);
    $("#txtRefDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtTotalPrice").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
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
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode };
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
            GetProductDetail(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductName").val("");
                $("#hdnProductId").val("0");
                $("#txtProductShortDesc").val("");
                $("#txtProductCode").val("");
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
                url: "../PO/GetTaxAutoCompleteList",
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
                $("#txtTaxAmount").val(Math.round(taxAmount).toFixed(2));
            }
            else
            {
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
      .append("<div><b>" + item.label + " || " + item.code + "</b></div>")
      .appendTo(ul);
};

    $("#txtPIDate,#txtRefDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });


    BindCountryList();
    BindTermTemplateList();
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPIId = $("#hdnPIId");
    if (hdnPIId.val() != "" && hdnPIId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPIDetail(hdnPIId.val());
       }, 2000);

        if (hdnAccessMode.val() == "4")
        {
            $("#txtPONo").attr('readOnly', true);
            $("#txtPINo").attr('readOnly', true);
            $("#txtPIDate").attr('readOnly', true);
            $("#txtPONo").attr('readOnly', true);
            $("#txtPODate").attr('readOnly', true);
            $("#hdnVendorId").attr('readOnly', true);
            $("#txtVendorCode").attr('readOnly', true);
            $("#txtVendorName").attr('readOnly', true);
            $("#txtAddress").attr('readOnly', true);
            $("#txtCity").attr('readOnly', true);
            $("#ddlCountry").attr('readOnly', true);
           
            $("#ddlState").attr('readOnly', true);
            $("#txtPinCode").attr('readOnly', true);
            $("#txtCSTNo").attr('readOnly', true);
            $("#txtTINNo").attr('readOnly', true);
            $("#txtPANNo").attr('readOnly', true);
            $("#txtRefNo").attr('readOnly', true);
            $("#txtRefDate").attr('readOnly', true);
        
            $("#ddlCountry").prop('disabled', true);
            $("#ddlState").prop('disabled', true);
          //  $("#id_messageKindId option").prop('disabled', true);

            $("#txtCreatedBy").attr('readOnly', true);
            $("#txtCreatedDate").attr('readOnly', true);
            $("#txtRemarksValue").attr('readOnly', true);
            $(".editonly").hide();
            if ($(".editonly").hide()) {
                $('#lblPONo').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
            }

            $("#txtPinCode").attr('readOnly', true);    
            $("#txtPinCode").attr('readOnly', true);
        }

        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $(".editonly").hide();
            if ($(".editonly").hide()) {
                $('#lblPONo').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
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
    }
    $("#txtCancelReason").attr('readOnly', false);
    var piProducts = [];
    GetPIProductList(piProducts);
    var piTaxes = [];
    GetPITaxList(piTaxes);
    var piTerms = [];
    GetPITermList(piTerms);
    


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


function BindCountryList() {
    $.ajax({
        type: "GET",
        url: "../Company/GetCountryList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCountry").append($("<option></option>").val(0).html("-Select Country-"));
            $.each(data, function (i, item) {

                $("#ddlCountry").append($("<option></option>").val(item.CountryId).html(item.CountryName));
            });
        },
        error: function (Result) {
            $("#ddlCountry").append($("<option></option>").val(0).html("-Select Country-"));
        }
    });
}

function GetPIProductList(piProducts) {
    var hdnPIId = $("#hdnPIId");
    var requestData = { piProducts: piProducts, invoiceId: hdnPIId.val() };
    $.ajax({
        url: "../PurchaseInvoice/GetPIProductList",
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
    price = price == "" ? 0 : price;
    quantity = quantity == "" ? 0 : quantity;

    var totalPrice = parseFloat(price) * parseFloat(quantity);
    $("#txtTotalPrice").val(totalPrice);


}
function CalculateGrossandNetValues() {
    var basicValue = 0;
    var taxValue = 0;
    $('#tblPIProductList tr').each(function (i, row) {
        var $row = $(row);
        var poProductDetailId = $row.find("#hdnPIProductDetailId").val();
        var totalPrice = $row.find("#hdnTotalPrice").val();
        if (poProductDetailId != undefined) {
            basicValue += parseFloat(totalPrice);
        }

    });
    $('#tblPITaxList tr').each(function (i, row) {
        var $row = $(row);
        var piTaxDetailId = $row.find("#hdnPITaxDetailId").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();

        var taxAmount = 0;
        if (piTaxDetailId != undefined) {
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

    $("#txtBasicValue").val(basicValue.toFixed(2));

    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(taxValue)));
}
function AddProduct(action) {
    var txtProductName = $("#txtProductName");
    var hdnPIProductDetailId = $("#hdnPIProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtPrice = $("#txtPrice");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
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
    if (txtTotalPrice.val().trim() == "" || txtTotalPrice.val().trim() == "0" || parseFloat(txtTotalPrice.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter correct Price and Quantity")
        txtQuantity.focus();
        return false;
    }

    var piProductList = [];
    $('#tblPIProductList tr').each(function (i, row) {
        var $row = $(row);
        var piProductDetailId = $row.find("#hdnPIProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var totalPrice = $row.find("#hdnTotalPrice").val();

        if (piProductDetailId != undefined) {
            if (action == 1 || hdnPIProductDetailId.val() != piProductDetailId) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    return false;
                }

                var piProduct = {
                    InvoiceProductDetailId: piProductDetailId,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Price: price,
                    Quantity: quantity,
                    TotalPrice: totalPrice
                };
                piProductList.push(piProduct);
            }
        }

    });

    var piProductAddEdit = {
        InvoiceProductDetailId: hdnPIProductDetailId.val(),
        ProductId: hdnProductId.val(),
        ProductName: txtProductName.val().trim(),
        ProductCode: txtProductCode.val().trim(),
        ProductShortDesc: txtProductShortDesc.val().trim(),
        UOMName: txtUOMName.val().trim(),
        Price: txtPrice.val().trim(),
        Quantity: txtQuantity.val().trim(),
        TotalPrice: txtTotalPrice.val().trim()
    };

    piProductList.push(piProductAddEdit);
    GetPIProductList(piProductList);

}
function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var piProductDetailId = $(row).find("#hdnPIProductDetailId").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var price = $(row).find("#hdnPrice").val();
    var quantity = $(row).find("#hdnQuantity").val();
    var totalPrice = $(row).find("#hdnTotalPrice").val();

    $("#txtProductName").val(productName);
    $("#hdnPIProductDetailId").val(piProductDetailId);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);
    $("#txtPrice").val(price);
    $("#txtQuantity").val(quantity);
    $("#txtTotalPrice").val(totalPrice);

    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    ShowHideProductPanel(1);
}
function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var piProductDetailId = $(row).find("#hdnPIProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
    }
}

function GetPITermList(piTerms) {
    var hdnPIId = $("#hdnPIId");
    var requestData = { piTerms: piTerms, invoiceId: hdnPIId.val() };
    $.ajax({
        url: "../PurchaseInvoice/GetPITermList",
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
        url: "../PurchaseInvoice/GetTermTemplateList",
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
    var piTermList = [];
    if (termTemplateId != undefined && termTemplateId != "" && termTemplateId != "0") {
        var data = { termTemplateId: termTemplateId };

        $.ajax({
            type: "GET",
            url: "../PurchaseInvoice/GetTermTemplateDetailList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                var termCounter = 1;

                $.each(data, function (i, item) {
                    var piTerm = {
                        InvoiceTermDetailId: item.TrnId,
                        TermDesc: item.TermsDesc,
                        TermSequence: termCounter
                    };
                   piTermList.push(piTerm);
                    termCounter += 1;
                });
                GetPITermList(piTermList);
            },
            error: function (Result) {
                GetPITermList(piTermList);
            }
        });
    }
    else {
        GetPITermList(piTermList);
    }
}

function AddTerm(action) {

    var txtTermDesc = $("#txtTermDesc");
    var hdnPITermDetailId = $("#hdnPITermDetailId");
    var hdnTermSequence = $("#hdnTermSequence");


    if (txtTermDesc.val().trim() == "") {
        ShowModel("Alert", "Please Enter Terms")
        txtTermDesc.focus();
        return false;
    }

    var piTermList = [];
    var termCounter = 1;
    $('#tblPITermList tr').each(function (i, row) {
        var $row = $(row);
        var piTermDetailId = $row.find("#hdnPITermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (piTermDetailId != undefined) {
            if (action == 1 || hdnPITermDetailId.val() != piTermDetailId) {

                if (termSequence == 0)
                { termSequence = termCounter; }

                var piTerm = {
                    InvoiceTermDetailId: piTermDetailId,
                    TermDesc: termDesc,
                    TermSequence: termSequence
                };
                piTermList.push(piTerm);
                termCounter += 1;
            }
        }

    });

    if (hdnTermSequence.val() == "" || hdnTermSequence.val() == "0") {
        hdnTermSequence.val(termCounter);
    }
    var piTermAddEdit = {
        InvoiceTermDetailId: hdnPITermDetailId.val(),
        TermDesc: txtTermDesc.val().trim(),
        TermSequence: hdnTermSequence.val()
    };

    piTermList.push(piTermAddEdit);
    GetPITermList(piTermList);

}
function EditTermRow(obj) {
    var row = $(obj).closest("tr");
    var piTermDetailId = $(row).find("#hdnPITermDetailId").val();
    var termDesc = $(row).find("#hdnTermDesc").val();
    var termSequence = $(row).find("#hdnTermSequence").val();
    $("#txtTermDesc").val(termDesc);
    $("#hdnPITermDetailId").val(piTermDetailId);
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

function GetPITaxList(piTaxes) {
    var hdnPIId = $("#hdnPIId");
    var requestData = { piTaxes: piTaxes, invoiceId: hdnPIId.val() };
    $.ajax({
        url: "../PurchaseInvoice/GetPITaxList",
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

    var txtBasicValue = $("#txtBasicValue");
    var txtTaxName = $("#txtTaxName");
    var hdnPITaxDetailId = $("#hdnPITaxDetailId");
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

    var piTaxList = [];
    $('#tblPITaxList tr').each(function (i, row) {
        var $row = $(row);
        var piTaxDetailId = $row.find("#hdnPITaxDetailId").val();
        var taxId = $row.find("#hdnTaxId").val();
        var taxName = $row.find("#hdnTaxName").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();
        var taxAmount = $row.find("#hdnTaxAmount").val();

        if (piTaxDetailId != undefined) {
            if (action == 1 || hdnPITaxDetailId.val() != piTaxDetailId) {

                if (taxId == hdnTaxId.val()) {
                    ShowModel("Alert", "Tax already added!!!")
                    txtTaxName.focus();
                    return false;
                }

                var piTax = {
                    InvoiceTaxDetailId: piTaxDetailId,
                    TaxId: taxId,
                    TaxName: taxName,
                    TaxPercentage: taxPercentage,
                    TaxAmount: taxAmount
                };
                piTaxList.push(piTax);            }
        }

    });

    var piTaxAddEdit = {
        InvoiceTaxDetailId: hdnPITaxDetailId.val(),
        TaxId: hdnTaxId.val(),
        TaxName: txtTaxName.val().trim(),
        TaxPercentage: txtTaxPercentage.val().trim(),
        TaxAmount: txtTaxAmount.val().trim()
    };

    piTaxList.push(piTaxAddEdit);
    GetPITaxList(piTaxList);

}
function EditTaxRow(obj) {

    var row = $(obj).closest("tr");
    var piTaxDetailId = $(row).find("#hdnPITaxDetailId").val();
    var taxId = $(row).find("#hdnTaxId").val();
    var taxName = $(row).find("#hdnTaxName").val();
    var taxPercentage = $(row).find("#hdnTaxPercentage").val();
    var taxAmount = $(row).find("#hdnTaxAmount").val();

    $("#txtTaxName").val(taxName);
    $("#hdnPITaxDetailId").val(piTaxDetailId);
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
        var piTaxDetailId = $(row).find("#hdnPITaxDetailId").val();
        ShowModel("Alert", "Tax Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
    }
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
function GetPIDetail(invoiceId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PurchaseInvoice/GetPIDetail",
        data: { invoiceId: invoiceId },
        dataType: "json",
        success: function (data) {
            $("#txtPINo").val(data.InvoiceNo);
            $("#txtPIDate").val(data.InvoiceDate);
            $("#txtPONo").val(data.PONo),
            $("#txtPODate").val(data.PODate),
            $("#hdnVendorId").val(data.VendorId);
            $("#txtVendorCode").val(data.VendorCode);
            $("#txtVendorName").val(data.VendorName);
            $("#txtAddress").val(data.BillingAddress);
            $("#txtCity").val(data.City);
            $("#ddlCountry").val(data.CountryId);
            BindStateList(data.StateId);
            $("#ddlState").val(data.StateId);
            $("#txtPinCode").val(data.PinCode);
            $("#txtCSTNo").val(data.CSTNo);
            $("#txtTINNo").val(data.TINNo);
            $("#txtPANNo").val(data.PANNo);
            $("#txtRefNo").val(data.RefNo);
            $("#txtRefDate").val(data.RefDate);
            $("#ddlApprovalStatus").val(data.ApprovalStatus)
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
          //  $("#btnAddNew").show();
          //  $("#btnPrint").show();
           // $("#btnEmail").show();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function CancelPurchaseInvoice() {
    var hdnPIId = $("#hdnPIId");
    var txtPINo = $("#txtPINo");
    var txtCancelReason = $("#txtCancelReason");
    var requestData = {
        invoiceId: hdnPIId.val(),
        invoiceNo:txtPINo.val().trim(),
        cancelReason: txtCancelReason.val().trim()
    };
    $.ajax({
        url: "../PurchaseInvoice/CancelPI",
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
                       window.location.href = "../PurchaseInvoice/ListPI";
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


 
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields() {

    $("#txtPINo").val("");
    $("#hdnPIId").val("0");
    $("#txtPIDate").val($("#hdnCurrentDate").val());
    $("#hdnVendorId").val("0");
    $("#txtVendorName").val("");
    $("#txtVendorCode").val("");
    $("#txtAddress").val("");
    $("#txtCity").val("");
    $("#ddlCountry").val("0");
    $("#ddlState").val("0");
    $("#txtPinCode").val("");
    $("#txtCSTNo").val("");
    $("#txtTINNo").val("");
    $("#txtPANNo").val("");
    $("#txtRefNo").val("");
    $("#txtRefDate").val("");
    $("#ddlApprovalStatus").val("Final");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
function GetVendorDetail(vendorId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Vendor/GetVendorDetail",
        data: { vendorId: vendorId },
        dataType: "json",
        success: function (data) {
            $("#txtAddress").val(data.Address);
            $("#txtCity").val(data.City);
            $("#ddlCountry").val(data.CountryId);
            BindStateList(data.StateId);
            $("#ddlState").val(data.StateId);
            $("#txtPinCode").val(data.PinCode);
            $("#txtCSTNo").val(data.CSTNo);
            $("#txtTINNo").val(data.TINNo);
            $("#txtPANNo").val(data.PANNo);
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
        $("#hdnPIProductDetailId").val("0");
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtPrice").val("");
        $("#txtUOMName").val("");
        $("#txtQuantity").val("");
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
        $("#hdnPITaxDetailId").val("0");
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
        $("#hdnPITermDetailId").val("0");
        $("#hdnTermSequence").val("0");
        $("#btnAddTerm").show();
        $("#btnUpdateTerm").hide();
    }
}

function OpenPOSearchPopup() {
    $("#SearchPOModel").modal();

}

function SelectPO(poId, poNo, poDate, vendorId, vendorCode, vendorName) {
    $("#txtPONo").val(poNo);
    $("#hdnPOId").val(poId);
    $("#txtPODate").val(poDate);
    $("#hdnVendorId").val(vendorId);
    $("#txtVendorCode").val(vendorCode);
    $("#txtVendorName").val(vendorName);
    $("#SearchPOModel").modal('hide');
    GetVendorDetail(vendorId);
}
function SearchPO() {
    var txtSearchPONo = $("#txtSearchPONo");
    var txtVendorName = $("#txtSearchVendorName");

    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { poNo: txtSearchPONo.val().trim(), vendorName: txtVendorName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: "Final", displayType: "Popup" };
    $.ajax({
        url: "../PurchaseInvoice/GetPurchaseOrderList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divPOList").html("");
            $("#divPOList").html(err);
        },
        success: function (data) {
            $("#divPOList").html("");
            $("#divPOList").html(data);
        }
    });
}
function ClearFields() {
    $("#hdnSIId").val("0");
    $("#txtCancelReason").val(""); 
    $("#btnCancel").show();
    $("#btnList").show();
}

function SendMail() {
    var hdnPIId = $("#hdnPIId");
    var requestData = { piId: hdnPIId.val(), reportType: "PDF" };
    $.ajax({
        url: "../PurchaseInvoice/PurchaseInvoiceMail",
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