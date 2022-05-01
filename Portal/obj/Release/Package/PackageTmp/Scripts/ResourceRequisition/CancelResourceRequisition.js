$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtSINo").attr('readOnly', true); 
    $("#txtSIDate").attr('readOnly', true);
    $("#txtSONo").attr('readOnly', true);
    $("#txtSODate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);


    $("#txtCustomerCode").attr('readOnly', true);
    $("#txtRefDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtPayToBookBranch").attr('readOnly', true);

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
   
    $("#txtCustomerName").autocomplete({
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
            $("#txtCustomerName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtCustomerName").val(ui.item.label);
            $("#hdnCustomerId").val(ui.item.value);
            $("#txtCustomerCode").val(ui.item.code);
            GetCustomerDetail(ui.item.value);
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

    $("#txtPayToBookName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SO/GetBookAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, bookType: "B" },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.BookName, value: item.BookId, branch: item.BankBranch, ifsc: item.IFSC };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtPayToBookName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtPayToBookName").val(ui.item.label);
            $("#hdnPayToBookId").val(ui.item.value);
            $("#txtPayToBookBranch").val(ui.item.branch);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtPayToBookName").val("");
                $("#hdnPayToBookId").val("0");
                $("#txtPayToBookBranch").val("0");
                ShowModel("Alert", "Please select Bank from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.branch + "</b><br>" + item.ifsc + "</div>")
      .appendTo(ul);
};



    $("#txtTaxName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SaleInvoice/GetTaxAutoCompleteList",
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
                url: "../SO/GetTaxAutoCompleteList",
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



    $("#txtSIDate,#txtRefDate").datepicker({
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

    BindTermTemplateList();
    
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnSIId = $("#hdnSIId");
    if (hdnSIId.val() != "" && hdnSIId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetSaleInvoiceDetail(hdnSIId.val());
       }, 2000);

        var customerId = $("#hdnCustomerId").val();
        BindCustomerBranchList(customerId);

        
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
    $("#txtCancelReason").attr('readOnly', false);
    var saleinvoiceProducts = [];
    GetSaleInvoiceProductList(saleinvoiceProducts);
    var saleinvoiceTaxes = [];
    GetSaleInvoiceTaxList(saleinvoiceTaxes);
    var saleinvoiceTerms = [];
    GetSaleInvoiceTermList(saleinvoiceTerms);

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


function BindCustomerBranchList(customerId) {
    $.ajax({
        type: "GET",
        url: "../SO/GetCustomerBranchList",
        data: { customerId: customerId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlBCustomerBranch,#ddlSCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
            $.each(data, function (i, item) {
                $("#ddlBCustomerBranch,#ddlSCustomerBranch").append($("<option></option>").val(item.CustomerBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlBCustomerBranch,#ddlSCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
        }
    });
}
function BindTermTemplateList() {
    $.ajax({
        type: "GET",
        url: "../SaleInvoice/GetTermTemplateList",
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
    var saleinvoiceTermList = [];
    if (termTemplateId != undefined && termTemplateId != "" && termTemplateId != "0") {
        var data = { termTemplateId: termTemplateId };

        $.ajax({
            type: "GET",
            url: "../SaleInvoice/GetTermTemplateDetailList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                var termCounter = 1;

                $.each(data, function (i, item) {
                    var saleinvoiceTerm = {
                        InvoiceTermDetailId: item.TrnId,
                        TermDesc: item.TermsDesc,
                        TermSequence: termCounter
                    };
                    saleinvoiceTermList.push(saleinvoiceTerm);
                    termCounter += 1;
                });
                GetSaleInvoiceTermList(saleinvoiceTermList);
            },
            error: function (Result) {
                GetSaleInvoiceTermList(saleinvoiceTermList);
            }
        });
    }
    else {
        GetSaleInvoiceTermList(saleinvoiceTermList);
    }
}
function GetSaleInvoiceTermList(saleinvoiceTerms) {
    var hdnSIId = $("#hdnSIId");
    var requestData = { saleinvoiceTerms: saleinvoiceTerms, saleinvoiceId: hdnSIId.val() };
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
function AddTerm(action) {
    var txtTermDesc = $("#txtTermDesc");
    var hdnSITermDetailId = $("#hdnSITermDetailId");
    var hdnTermSequence = $("#hdnTermSequence");

    if (txtTermDesc.val().trim() == "") {
        ShowModel("Alert", "Please Enter Terms")
        txtTermDesc.focus();
        return false;
    }

    var saleInvoiceTermList = [];
    var termCounter = 1;
    $('#tblSaleInvoiceTermList tr').each(function (i, row) {
        var $row = $(row);
        var invoiceTermDetailId = $row.find("#hdnSITermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (invoiceTermDetailId != undefined) {
            if (action == 1 || hdnSITermDetailId.val() != invoiceTermDetailId) {

                if (termSequence == 0)
                { termSequence = termCounter; }

                var saleinvoiceTerm = {
                    InvoiceTermDetailId: invoiceTermDetailId,
                    TermDesc: termDesc,
                    TermSequence: termSequence
                };
                saleInvoiceTermList.push(saleinvoiceTerm);
                termCounter += 1;
            }
        }

    });

    if (hdnTermSequence.val() == "" || hdnTermSequence.val() == "0") {
        hdnTermSequence.val(termCounter);
    }
    var saleinvoiceTermAddEdit = {
        InvoiceTermDetailId: hdnSITermDetailId.val(),
        TermDesc: txtTermDesc.val().trim(),
        TermSequence: hdnTermSequence.val()
    };

    saleInvoiceTermList.push(saleinvoiceTermAddEdit);
    GetSaleInvoiceTermList(saleInvoiceTermList);

}
function EditTermRow(obj) {

    var row = $(obj).closest("tr");
    var invoiceTermDetailId = $(row).find("#hdnSITermDetailId").val();
    var termDesc = $(row).find("#hdnTermDesc").val();
    var termSequence = $(row).find("#hdnTermSequence").val();


    $("#txtTermDesc").val(termDesc);
    $("#hdnSITermDetailId").val(invoiceTermDetailId);
    $("#hdnTermSequence").val(termSequence);

    $("#btnAddTerm").hide();
    $("#btnUpdateTerm").show();
    ShowHideTermPanel(1);
}

function RemoveTermRow(obj) {
    if (confirm("Do you want to remove selected Term?")) {
        var row = $(obj).closest("tr");
        var saleinvoiceTermDetailId = $(row).find("#hdnSITermDetailId").val();
        ShowModel("Alert", "Term Removed from List.");
        row.remove();
    }
}

function GetSaleInvoiceTaxList(saleinvoiceTaxes) {
    var hdnSIId = $("#hdnSIId");
    var requestData = { saleinvoiceTaxes: saleinvoiceTaxes, saleinvoiceId: hdnSIId.val() };
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
function AddTax(action) {

    var txtBasicValue = $("#txtBasicValue");
    var txtTaxName = $("#txtTaxName");
    var hdnSITaxDetailId = $("#hdnSITaxDetailId");
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

    var saleinvoiceTaxList = [];
    $('#tblSaleInvoiceTaxList tr').each(function (i, row) {
        var $row = $(row);
        var invoiceTaxDetailId = $row.find("#hdnSITaxDetailId").val();
        var taxId = $row.find("#hdnTaxId").val();
        var taxName = $row.find("#hdnTaxName").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();
        var taxAmount = $row.find("#hdnTaxAmount").val();

        if (invoiceTaxDetailId != undefined) {
            if (action == 1 || hdnSITaxDetailId.val() != invoiceTaxDetailId) {

                if (taxId == hdnTaxId.val()) {
                    ShowModel("Alert", "Tax already added!!!")
                    txtTaxName.focus();
                    return false;
                }

                var saleinvoiceTax = {
                    InvoiceTaxDetailId: invoiceTaxDetailId,
                    TaxId: taxId,
                    TaxName: taxName,
                    TaxPercentage: taxPercentage,
                    TaxAmount: taxAmount
                };
                saleinvoiceTaxList.push(saleinvoiceTax);
            }
        }

    });

    var saleinvoiceTaxAddEdit = {
        SITaxDetailId: hdnSITaxDetailId.val(),
        TaxId: hdnTaxId.val(),
        TaxName: txtTaxName.val().trim(),
        TaxPercentage: txtTaxPercentage.val().trim(),
        TaxAmount: txtTaxAmount.val().trim()
    };

    saleinvoiceTaxList.push(saleinvoiceTaxAddEdit);
    GetSaleInvoiceTaxList(saleinvoiceTaxList);

}
function EditTaxRow(obj) {

    var row = $(obj).closest("tr");
    var saleinvoiceTaxDetailId = $(row).find("#hdnSITaxDetailId").val();
    var taxId = $(row).find("#hdnTaxId").val();
    var taxName = $(row).find("#hdnTaxName").val();
    var taxPercentage = $(row).find("#hdnTaxPercentage").val();
    var taxAmount = $(row).find("#hdnTaxAmount").val();

    $("#txtTaxName").val(taxName);
    $("#hdnSITaxDetailId").val(saleinvoiceTaxDetailId);
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
        var saleinvoiceTaxDetailId = $(row).find("#hdnSITaxDetailId").val();
        ShowModel("Alert", "Tax Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
    }
}


function AddProduct(action) {
    var txtProductName = $("#txtProductName");
    var hdnSIProductDetailId = $("#hdnSIProductDetailId");
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
        ShowModel("Alert", "Invoice Quantity cannot be greater than Available Stock Quantity")
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

    var saleinvoiceProductList = [];
    $('#tblSaleInvoiceProductList tr').each(function (i, row) {
        var $row = $(row);
        var invoiceProductDetailId = $row.find("#hdnSIProductDetailId").val();
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

        if (invoiceProductDetailId != undefined) {
            if (action == 1 || hdnSIProductDetailId.val() != invoiceProductDetailId) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    return false;
                }

                var saleinvoiceProduct = {
                    InvoiceProductDetailId: invoiceProductDetailId,
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
                    TotalPrice: totalPrice
                };
                saleinvoiceProductList.push(saleinvoiceProduct);
            }
        }

    });

    var saleinvoiceProductAddEdit = {
        InvoiceProductDetailId: hdnSIProductDetailId.val(),
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
        TotalPrice: txtTotalPrice.val().trim()
    };

    saleinvoiceProductList.push(saleinvoiceProductAddEdit);
    GetSaleInvoiceProductList(saleinvoiceProductList);

}
function GetSaleInvoiceProductList(saleinvoiceProducts) {
    var hdnSIId = $("#hdnSIId");
    var requestData = { saleinvoiceProducts: saleinvoiceProducts, saleinvoiceId: hdnSIId.val() };
    $.ajax({
        url: "../SaleInvoice/GetSaleInvoiceProductList",
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
    var discountAmount = 0;
    var taxAmount = 0;
    price = price == "" ? 0 : price;
    quantity = quantity == "" ? 0 : quantity;
    var totalPrice = parseFloat(price) * parseFloat(quantity);
    if (parseFloat(discountPerc) > 0) {
        discountAmount = (parseFloat(totalPrice) * parseFloat(discountPerc)) / 100

    }
    $("#txtDiscountAmount").val(discountAmount.toFixed(2));

    if (parseFloat(productTaxPerc) > 0) {
        taxAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(productTaxPerc)) / 100;
    }
    $("#txtProductTaxAmount").val(taxAmount.toFixed(2));

    $("#txtTotalPrice").val((totalPrice - discountAmount + taxAmount).toFixed(2));


}
function CalculateGrossandNetValues() {
    var basicValue = 0;
    var taxValue = 0;
    $('#tblSaleInvoiceProductList tr').each(function (i, row) {
        var $row = $(row);
        var siProductDetailId = $row.find("#hdnSIProductDetailId").val();
        var totalPrice = $row.find("#hdnTotalPrice").val(); 
        if (siProductDetailId != undefined) {
            basicValue += parseFloat(totalPrice);
        }

    });
    $('#tblSaleInvoiceTaxList tr').each(function (i, row) {
        var $row = $(row);
        var saleinvoiceTaxDetailId = $row.find("#hdnSITaxDetailId").val();
        var taxPercentage = $row.find("#hdnTaxPercentage").val();
        var taxAmount = 0;
        if (saleinvoiceTaxDetailId != undefined) {

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
    var loadingValue = $("#txtLoadingValue").val() == "" ? "0" : $("#txtLoadingValue").val();
    if (parseFloat(freightValue) <= 0) {
        freightValue = 0;
    }
    if (parseFloat(loadingValue) <= 0) {
        loadingValue = 0;
    }

    $("#txtBasicValue").val(basicValue);
    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(taxValue) + parseFloat(freightValue) + parseFloat(loadingValue)).toFixed(2));
}
 

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var saleinvoiceProductDetailId = $(row).find("#hdnSIProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
        CalculateGrossandNetValues();
    }
}


function GetSaleInvoiceDetail(saleinvoiceId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SaleInvoice/GetSaleInvoiceDetail",
        data: { saleinvoiceId: saleinvoiceId },
        dataType: "json",
        success: function (data) {
            $("#txtSINo").val(data.InvoiceNo);
            $("#txtSIDate").val(data.InvoiceDate);
            $("#txtSONo").val(data.SONo);
            $("#hdnSOId").val(data.SOId);
            $("#txtSODate").val(data.SODate);

            $("#hdnCustomerId").val(data.CustomerId);
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtCustomerName").val(data.CustomerName);

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
            $("#ddlApprovalStatus").val(data.ApprovalStatus);

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

            $("#txtRefNo").val(data.RefNo);
            $("#txtRefDate").val(data.RefDate);

            $("#txtPayToBookName").val(data.PayToBookName);
            $("#hdnPayToBookId").val(data.PayToBookId);
            $("#txtPayToBookBranch").val(data.PayToBookBranch);

            $("#txtRemarks").val(data.Remarks);

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
            $("#txtLoadingValue").val(data.LoadingValue);

            $("#btnAddNew").show();
            $("#btnPrint").show();
            $("#btnEmail").show();


        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

function CancelInvoice() { 
    var hdnSIId = $("#hdnSIId"); 
    var txtCancelReason = $("#txtCancelReason"); 
    var requestData = {
        invoiceId: hdnSIId.val(),
        cancelReason: txtCancelReason.val().trim()
    };
    $.ajax({
        url: "../SaleInvoice/CancelSaleInvoice",
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
                       window.location.href = "../SaleInvoice/ListSaleInvoice";
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
    $("#hdnSIId").val("0"); 
    $("#txtCancelReason").val(""); 

    $("#btnCancel").show();
    $("#btnList").show();
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
        data: { productid: productId, companyBranchId: 0, trnId: 0, trnType: "Invoice" },
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
        $("#hdnSIProductDetailId").val("0");
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
        $("#hdnSITaxDetailId").val("0");
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
        $("#hdnSITermDetailId").val("0");
        $("#hdnTermSequence").val("0");
        $("#btnAddTerm").show();
        $("#btnUpdateTerm").hide();
    }
}
function OpenSOSearchPopup() {
    $("#SearchQuotationModel").modal();

}
function SearchSaleOrder() {
    var txtSearchSONo = $("#txtSearchSONo");
    var txtCustomerName = $("#txtSearchCustomerName");

    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { soNo: txtSearchSONo.val().trim(), customerName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: "Final", displayType: "Popup" };
    $.ajax({
        url: "../SaleInvoice/GetSaleOrderList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divSOList").html("");
            $("#divSOList").html(err);
        },
        success: function (data) {
            $("#divSOList").html("");
            $("#divSOList").html(data);
        }
    });
}
function SelectSO(soId, soNo, soDate, customerId, customerCode, customerName) {
    $("#txtSONo").val(soNo);
    $("#hdnSOId").val(soId);
    $("#txtSODate").val(soDate);
    $("#hdnCustomerId").val(customerId);
    $("#txtCustomerCode").val(customerCode);
    $("#txtCustomerName").val(customerName);
    $("#SearchQuotationModel").modal('hide');
    GetCustomerDetail(customerId);
    GetSODetail(soId);
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
 
 
function GetSODetail(soId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SO/GetSODetail",
        data: { soId: soId },
        dataType: "json",
        success: function (data) {  
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

            $("#txtPayToBookName").val(data.PayToBookName);
            $("#hdnPayToBookId").val(data.PayToBookId);
            $("#txtPayToBookBranch").val(data.PayToBookBranch);

            
            $("#txtBasicValue").val(data.BasicValue);
            $("#txtTotalValue").val(data.TotalValue);
            $("#txtFreightValue").val(data.FreightValue);
            $("#txtLoadingValue").val(data.LoadingValue);

            

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

    var soProducts = [];
    GetSOProductList(soProducts,soId);
    var soTaxes = [];
    GetSOTaxList(soTaxes,soId);

}
function GetSOProductList(soProducts,soId) { 
    var requestData = { soProducts: soProducts, soId: soId };
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
        }
    });
}
function GetSOTaxList(soTaxes, soId) {  
    var requestData = { soTaxes: soTaxes, soId: soId };
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
 
        }
    });
}