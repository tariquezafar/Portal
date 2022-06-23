$(document).ready(function () {txtVendorCode
    $("#tabs").tabs({
        collapsible: true
    });

    
    $("#txtCustomerInvoiceDate").attr('readOnly', true);
    $("#txtPONo").attr('readOnly', true);
    $("#txtPODate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtInvoiceNo").attr('readOnly', true);
    $("#txtVendorCode").attr('readOnly', true);
    $("#txtConsigneeCode").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtIGSTPercAmount").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtIGSTPerc").attr('readOnly', true);
    $("#txtCustomDutyRate,#txtExciseDutyRate,#txtGSTCessAmt,#txtBCDAmtRs,#txtCVDAmtRs").attr('readOnly', true);
    $("#txtInvoiceDate,#txtBEDate,#txtLocalIGMDate,#txtGatewayIGMDate,#txtBLDate,#txtSupplierInvoiceDate").attr('readOnly', true);
    $("#txtTotalValue").attr('readOnly', true);
    $("#txtTotalPrice").attr('readOnly', true);
    $("#txtDiscountAmount").attr('readOnly', true);
    $("#txtEducationalCessOnCVDAmt,#txtSecHigherEduCessOnCVDAmt,#txtCustomSecHigherEduCessAmt,#txtCustomEducationalCessAmt").attr('readOnly', true);
    
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
            
            $("#txtIGSTPerc").val(ui.item.IGST_Perc);
            
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

    $("#txtSearchFromDate,#txtSearchToDate,#txtSupplierInvoiceDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $("#txtBEDate,#txtLocalIGMDate,#txtGatewayIGMDate,#txtBLDate,#txtRefDate,#txtCustomerInvoiceDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    $("#txtInvoiceDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {

        }
    });
    BindCountryList();
    BindPurchaseUOMList();
    BindTermTemplateList();
    BindCompanyBranchList();
    BindCurrencyList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnInvoiceId = $("#hdnInvoiceId");
    if (hdnInvoiceId.val() != "" && hdnInvoiceId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPurchaseInvoiceImportDetail(hdnInvoiceId.val());
           $("#ddlCompanyBranch").attr('disabled', true);
       }, 4000);
       
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $("#txtMark").attr('disabled', true);
            $(".editonly").hide();
            if ($(".editonly").hide())
            {
                $('#lblPONo,#lblVendorCode').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
            }
           

        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReCalculate").show();
            $("#btnReset").show();
            $(".editonly").show();
        }
    }
    else {
        $("#btnSave").show();
        $("#btnReCalculate").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
        $(".editonly").show();
    }

    var piProducts = [];
    GetPIProductList(piProducts);
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

function BindCurrencyList() {
    $("#ddlInvoiceValueCurrency,#ddlProductCurrency,#ddlFreightCurrency,#ddlInsurenceCurrency").val(0);
    $("#ddlInvoiceValueCurrency,#ddlProductCurrency,#ddlFreightCurrency,#ddlInsurenceCurrency").html("");
    $.ajax({
        type: "GET",
        url: "../Quotation/GetCurrencyList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlInvoiceValueCurrency,#ddlProductCurrency,#ddlFreightCurrency,#ddlInsurenceCurrency").append($("<option></option>").val(0).html("-Select Currency-"));
            $.each(data, function (i, item) {
                if (item.CurrencyCode == "INR") {
                    $("#ddlInvoiceValueCurrency,#ddlProductCurrency,#ddlFreightCurrency,#ddlInsurenceCurrency").append($("<option selected></option>").val(item.CurrencyCode).html(item.CurrencyName));
                }
                else {
                    $("#ddlInvoiceValueCurrency,#ddlProductCurrency,#ddlFreightCurrency,#ddlInsurenceCurrency").append($("<option></option>").val(item.CurrencyCode).html(item.CurrencyName));
                }
            });
        },
        error: function (Result) {
            $("#ddlInvoiceValueCurrency,#ddlProductCurrency").append($("<option></option>").val(0).html("-Select Currency-"));
        }
    });
}

function BindConsigneeBranchList(consigneeId) {
    $("#ddlSCustomerBranch").val(0);
    $("#ddlSCustomerBranch").html("");
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


function GetPIProductList(piProducts) {
    var hdnInvoiceId = $("#hdnInvoiceId");
    var requestData = { piProducts: piProducts, invoiceId: hdnInvoiceId.val() };
    $.ajax({
        url: "../PurchaseInvoiceImport/GetPurchaseInvoiceImportProductList",
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
    var IGSTPerc = $("#txtIGSTPerc").val();
    var exchangeRate = $("#txtExchangeRate").val();
    var customDutyPerc = $("#txtCustomDutyPerc").val();
    var exciseDutyPerc = $("#txtExciseDutyPerc").val();
    var gSTCessPerc = $("#txtGSTCessPerc").val();

    var educationalCessOnCVDPerc = $("#txtEducationalCessOnCVDPerc").val();
    var secHigherEduCessOnCVDPerc = $("#txtSecHigherEduCessOnCVDPerc").val();
    var customEducationalCessPerc = $("#txtCustomEducationalCessPerc").val();
    var customSecHigherEduCessPerc = $("#txtCustomSecHigherEduCessPerc").val();

    var discountAmount = 0;
    var IGSTAmount = 0;
    var assValue = 0;
    var gSTCessAmt = 0;
    price = price == "" ? 0 : price;
    quantity = quantity == "" ? 0 : quantity;
    customDutyPerc = customDutyPerc == "" ? 0 : customDutyPerc;
    customDutyAmt = 0;

    exciseDutyPerc = exciseDutyPerc == "" ? 0 : exciseDutyPerc;
    exciseDutyAmt = 0;

    educationalCessOnCVDPerc = educationalCessOnCVDPerc == "" ? 0 : educationalCessOnCVDPerc;
    educationalCessOnCVDAmt = 0;
    secHigherEduCessOnCVDPerc = secHigherEduCessOnCVDPerc == "" ? 0 : secHigherEduCessOnCVDPerc;
    secHigherEduCessOnCVDAmt = 0;
    customEducationalCessPerc = customEducationalCessPerc == "" ? 0 : customEducationalCessPerc;
    customEducationalCessAmt = 0;
    customSecHigherEduCessPerc = customSecHigherEduCessPerc == "" ? 0 : customSecHigherEduCessPerc;
    customSecHigherEduCessAmt = 0;
  

    assValue = parseFloat(parseFloat(quantity)*parseFloat(price)*parseFloat(exchangeRate)).toFixed(2);
    var totalPrice = parseFloat(price) * parseFloat(quantity);
    if (parseFloat(discountPerc) > 0) {

        discountAmount = ((parseFloat(assValue) * parseFloat(discountPerc)) / 100).toFixed(2);
    }

    $("#txtDiscountAmount").val(discountAmount);

   
    if (parseFloat(customDutyPerc) > 0)
    {
        customDutyAmt = ((parseFloat(assValue) * parseFloat(customDutyPerc)) / 100).toFixed(2);
    }

    if (parseFloat(exciseDutyPerc) > 0) {
        exciseDutyAmt = ((parseFloat(assValue) * parseFloat(exciseDutyPerc)) / 100).toFixed(2);
    }
   
    //-------------------------Code For Dheeraj Date 20-Apr-2018

    educationalCessOnCVDPerc = educationalCessOnCVDPerc == "" ? 0 : educationalCessOnCVDPerc;
    educationalCessOnCVDAmt = 0;
    secHigherEduCessOnCVDPerc = secHigherEduCessOnCVDPerc == "" ? 0 : secHigherEduCessOnCVDPerc;
    secHigherEduCessOnCVDAmt = 0;
    customEducationalCessPerc = customEducationalCessPerc == "" ? 0 : customEducationalCessPerc;
    customEducationalCessAmt = 0;
    customSecHigherEduCessPerc = customSecHigherEduCessPerc == "" ? 0 : customSecHigherEduCessPerc;
    customSecHigherEduCessAmt = 0;

    if (parseFloat(educationalCessOnCVDPerc) > 0) {
        educationalCessOnCVDAmt = ((parseFloat(exciseDutyAmt) * parseFloat(educationalCessOnCVDPerc)) / 100).toFixed(2);
    }
    if (parseFloat(secHigherEduCessOnCVDPerc) > 0) {
        secHigherEduCessOnCVDAmt = ((parseFloat(exciseDutyAmt) * parseFloat(secHigherEduCessOnCVDPerc)) / 100).toFixed(2);
    }

    if (parseFloat(customEducationalCessPerc) > 0) {
        customEducationalCessAmt = ((parseFloat(customDutyAmt) * parseFloat(customEducationalCessPerc)) / 100).toFixed(2);
    }
    if (parseFloat(customSecHigherEduCessPerc) > 0) {
        customSecHigherEduCessAmt = ((parseFloat(customDutyAmt) * parseFloat(customSecHigherEduCessPerc)) / 100).toFixed(2);
    }

    if (parseFloat(IGSTPerc) > 0) {

        IGSTAmount = (((parseFloat(assValue)-parseFloat(discountAmount) + parseFloat(educationalCessOnCVDAmt) + parseFloat(secHigherEduCessOnCVDAmt)
            + parseFloat(customEducationalCessAmt) + parseFloat(customSecHigherEduCessAmt) + parseFloat(customDutyAmt) + parseFloat(exciseDutyAmt)) * parseFloat(IGSTPerc)) / 100).toFixed(2);
    }

    if (parseFloat(gSTCessPerc) > 0) {
        gSTCessAmt = (((parseFloat(assValue) - parseFloat(discountAmount) + parseFloat(educationalCessOnCVDAmt) + parseFloat(secHigherEduCessOnCVDAmt)
            + parseFloat(customEducationalCessAmt) + parseFloat(customSecHigherEduCessAmt) + parseFloat(customDutyAmt) + parseFloat(exciseDutyAmt)) * parseFloat(gSTCessPerc)) / 100).toFixed(2);
    }

    if (parseFloat(IGSTPerc) > 0) {

        IGSTAmount = (((parseFloat(assValue) - parseFloat(discountAmount) + parseFloat(educationalCessOnCVDAmt) + parseFloat(secHigherEduCessOnCVDAmt)
            + parseFloat(customEducationalCessAmt) + parseFloat(customSecHigherEduCessAmt) + parseFloat(customDutyAmt) + parseFloat(exciseDutyAmt)) * parseFloat(IGSTPerc)) / 100).toFixed(2);
    }

    $("#txtIGSTPercAmount").val(IGSTAmount);
   
    $("#txtAssValue").val(assValue);
    $("#txtCustomDutyRate").val(parseFloat(customDutyAmt));
    $("#txtExciseDutyRate").val(parseFloat(exciseDutyAmt));

    $("#txtBCDAmtRs").val(parseFloat(customDutyAmt).toFixed(2));
    $("#txtCVDAmtRs").val(parseFloat(exciseDutyAmt).toFixed(2));
    $("#txtGSTCessAmt").val(parseFloat(gSTCessAmt).toFixed(2));
    
    $("#txtEducationalCessOnCVDAmt").val(parseFloat(educationalCessOnCVDAmt).toFixed(2));
    $("#txtSecHigherEduCessOnCVDAmt").val(parseFloat(secHigherEduCessOnCVDAmt).toFixed(2));
    $("#txtCustomEducationalCessAmt").val(parseFloat(customEducationalCessAmt).toFixed(2));
    $("#txtCustomSecHigherEduCessAmt").val(parseFloat(customSecHigherEduCessAmt).toFixed(2));

    //$("#txtTotalPrice").val((parseFloat(totalPrice) -parseFloat(discountAmount) + parseFloat(IGSTAmount)));

    $("#txtTotalPrice").val(
        (parseFloat(assValue) - parseFloat(discountAmount) +
        parseFloat(IGSTAmount)+
         parseFloat(gSTCessAmt) +
        parseFloat(educationalCessOnCVDAmt) +
        parseFloat(secHigherEduCessOnCVDAmt) +
        parseFloat(customEducationalCessAmt) +
        parseFloat(customSecHigherEduCessAmt) +
        parseFloat(customDutyAmt) +
        parseFloat(exciseDutyAmt))).toFixed(2);
}
function CalculateEducationalCessOnCVD() {
    var eduCessOnCVDAmt = 0;
    var secHigherCessOnCVDAmt = 0;
    var customSecHigherEducationCessAmt = 0;
    var customEducationalCessAmt = 0;
    var totalAssValue = 0;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var assValue = $row.find("#hdnAssValue").val();
        if (assValue != undefined) {
            totalAssValue += parseFloat(assValue);
        }
    });
    
    var educationalCessOnCVDPerc = $("#txtEducationalCessOnCVDPerc").val() == "" ? 0 : $("#txtEducationalCessOnCVDPerc").val();
    var secHigherEduCessOnCVDPerc = $("#txtSecHigherEduCessOnCVDPerc").val() == "" ? 0 : $("#txtSecHigherEduCessOnCVDPerc").val();
    var customSecHigherEduCessPerc = $("#txtCustomSecHigherEduCessPerc").val() =="" ? 0 : $("#txtCustomSecHigherEduCessPerc").val();
    var customEducationalCessPerc = $("#txtCustomEducationalCessPerc").val() == "" ? 0 : $("#txtCustomEducationalCessPerc").val();
    if(educationalCessOnCVDPerc > 0)
    {
        eduCessOnCVDAmt = (parseFloat(totalAssValue) * parseFloat(educationalCessOnCVDPerc)) / 100;
    }
    if(secHigherEduCessOnCVDPerc > 0)
     {
        secHigherCessOnCVDAmt = (parseFloat(totalAssValue) * parseFloat(secHigherEduCessOnCVDPerc)) / 100;
     }
    if (customSecHigherEduCessPerc > 0) {
        customSecHigherEducationCessAmt = (parseFloat(totalAssValue) * parseFloat(customSecHigherEduCessPerc)) / 100;
    }

    if (customEducationalCessPerc > 0) {
        customEducationalCessAmt = (parseFloat(totalAssValue) * parseFloat(customEducationalCessPerc)) / 100;
    }
    $("#txtEducationalCessOnCVDAmt").val(parseFloat(eduCessOnCVDAmt).toFixed(2));
    $("#txtSecHigherEduCessOnCVDAmt").val(parseFloat(secHigherCessOnCVDAmt).toFixed(2));
    $("#txtCustomSecHigherEduCessAmt").val(parseFloat(customSecHigherEducationCessAmt).toFixed(2));
    $("#txtCustomEducationalCessAmt").val(parseFloat(customEducationalCessAmt).toFixed(2));
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

    $("#txtBasicValue").val(basicValue);
    $("#txtGrossValue").val(parseFloat(basicValue).toFixed(2));

    var eduCessOnCVDAmt = $("#txtEducationalCessOnCVDAmt").val() == "" ? 0 : parseFloat($("#txtEducationalCessOnCVDAmt").val());
    var secHigherCessOnCVDAmt = $("#txtSecHigherEduCessOnCVDAmt").val() == "" ? 0 : parseFloat($("#txtSecHigherEduCessOnCVDAmt").val());
    var customSecHigherEducationCessAmt = $("#txtCustomSecHigherEduCessAmt").val() == "" ? 0 : parseFloat($("#txtCustomSecHigherEduCessAmt").val());
    var customEducationalCessAmt = $("#txtCustomEducationalCessAmt").val() == "" ? 0 :parseFloat($("#txtCustomEducationalCessAmt").val());

        //var grossValue = $("#txtGrossValue").val();
        //var str = grossValue.split('.');
        //var roundVal = parseFloat("0." + str[1]);
        //var round_Amt;
        //if (roundVal == '0.00') {
        //    round_Amt = roundVal;
        //}
        //else if (roundVal < 0.51) {
        //    round_Amt = "-" + roundVal;
        //}
        //else {
        //    round_Amt = "0." + parseInt(100 - str[1]);
        //}
        //$("#txtRoundOfValue").val(round_Amt);

    var grossValue = $("#txtGrossValue").val();
    var str = grossValue.split('.');
    var roundVal = parseFloat("0." + str[1]);

    var round_Amt;
    if (roundVal == '0.00') {
        round_Amt = roundVal;
    }
    else if (roundVal < 0.50) {

        round_Amt = "-" +roundVal;
    }
    else {
        var roundAmt = parseInt(100 - str[1]);
        if (roundAmt > 0 && roundAmt < 10) {
            round_Amt = "0.0" +roundAmt;
        }
        else {
            round_Amt = "0." +roundAmt;
    }

    }


    $("#txtRoundOfValue").val(round_Amt);

        //$("#txtTotalValue").val(parseFloat(basicValue).toFixed(0) + parseFloat(eduCessOnCVDAmt).toFixed(0) + parseFloat(secHigherCessOnCVDAmt).toFixed(0) + parseFloat(customSecHigherEducationCessAmt).toFixed(0) + parseFloat(customEducationalCessAmt).toFixed(0));
        $("#txtTotalValue").val(parseFloat(basicValue).toFixed(2));
}

    function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtProductName = $("#txtProductName");
    var hdnPIProductDetailId = $("#hdnPIProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtPrice = $("#txtPrice");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var txtTotalPrice = $("#txtTotalPrice");
    var txtDiscountPerc = $("#txtDiscountPerc");
    var txtDiscountAmount = $("#txtDiscountAmount");
    var txtIGSTPerc = $("#txtIGSTPerc");
    var txtIGSTPercAmount = $("#txtIGSTPercAmount");
    var txtHSN_Code = $("#txtHSN_Code");
        /*==============================================================*/
    var ddlProductCurrency = $("#ddlProductCurrency");
    var txtRITC = $("#txtRITC");
    var txtAssValue = $("#txtAssValue");
    var txtCTH = $("#txtCTH");
    var txtCETH = $("#txtCETH");
    var txtCNotn = $("#txtCNotn");
    var txtENotn = $("#txtENotn");
    var txtRSP = $("#txtRSP");
    var txtExciseDutyPerc = $("#txtExciseDutyPerc");
    var txtExciseDutyRate = $("#txtExciseDutyRate");
    var txtCustomDutyPerc = $("#txtCustomDutyPerc");
    var txtCustomDutyRate = $("#txtCustomDutyRate");
    var txtBCDAmtRs = $("#txtBCDAmtRs");
    var txtCVDAmtRs = $("#txtCVDAmtRs");

    var txtGSTCessPerc = $("#txtGSTCessPerc");
    var txtGSTCessAmt = $("#txtGSTCessAmt");
    var txtLoadProv = $("#txtLoadProv");
    var txtCNSNO = $("#txtCNSNO");
    var txtENSNO = $("#txtENSNO");

    var txtEducationalCessOnCVDPerc = $("#txtEducationalCessOnCVDPerc");
    var txtEducationalCessOnCVDAmt = $("#txtEducationalCessOnCVDAmt");
    var txtSecHigherEduCessOnCVDPerc = $("#txtSecHigherEduCessOnCVDPerc");
    var txtSecHigherEduCessOnCVDAmt = $("#txtSecHigherEduCessOnCVDAmt");
    var txtCustomEducationalCessPerc = $("#txtCustomEducationalCessPerc");
    var txtCustomEducationalCessAmt = $("#txtCustomEducationalCessAmt");
    var txtCustomSecHigherEduCessPerc = $("#txtCustomSecHigherEduCessPerc");
    var txtCustomSecHigherEduCessAmt = $("#txtCustomSecHigherEduCessAmt");
        /*==============================================================*/


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
        //if (txtTotalPrice.val().trim() == "" || txtTotalPrice.val().trim() == "0" || parseFloat(txtTotalPrice.val().trim()) <= 0) {
        //    ShowModel("Alert", "Please enter correct Price and Quantity")
        //    txtQuantity.focus();
        //    return false;
        //}
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }
    var piProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var piProductDetailId = $row.find("#hdnPIProductDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var discountPerc = $row.find("#hdnDiscountPerc").val();
        var discountAmount = $row.find("#hdnDiscountAmount").val();
        var quantity = $row.find("#hdnQuantity").val();
        var totalPrice = $row.find("#hdnTotalPrice").val();
        var iGSTPerc = $row.find("#hdnIGSTPerc").val();
        var iGSTPercAmount = $row.find("#hdnIGSTAmount").val();
        var hsn_Code = $row.find("#hdnHSN_Code").val();
        var currency = $row.find("#hdnCurrency").val();
        var rITC = $row.find("#hdnRITC").val();
        var assValue = $row.find("#hdnAssValue").val();
        var cTH = $row.find("#hdnCTH").val();
        var cETH = $row.find("#hdnCETH").val();
        var cNotn = $row.find("#hdnCNotn").val();
        var eNotn = $row.find("#hdnENotn").val();
        var eNsno = $row.find("#hdnENSNO").val();
        var cNsno = $row.find("#hdnCNSNO").val();
        var rSP = $row.find("#hdnRSP").val();
        var loadProv = $row.find("#hdnLoadProv").val();

        var bCDAmtRs = $row.find("#hdnBCDAmtRs").val();
        var cVDAmtRs = $row.find("#hdnCVDAmtRs").val();
        var customDutyPerc = $row.find("#hdnCustomDutyPerc").val()
        var customDutyRate = $row.find("#hdnCustomDutyRate").val();
        var exciseDutyPerc = $row.find("#hdnExciseDutyPerc").val();
        var exciseDutyRate = $row.find("#hdnExciseDutyRate").val();
        var gSTCessPerc = $row.find("#hdnGSTCessPerc").val();
        var gSTCessAmt = $row.find("#hdnGSTCessAmt").val();


        var educationalCessOnCVDPerc = $row.find("#hdnEducationalCessOnCVDPerc").val();
        var educationalCessOnCVDAmt = $row.find("#hdnEducationalCessOnCVDAmt").val();
        var secHigherEduCessOnCVDPerc = $row.find("#hdnSecHigherEduCessOnCVDPerc").val()
        var secHigherEduCessOnCVDAmt = $row.find("#hdnSecHigherEduCessOnCVDAmt").val();
        var customSecHigherEduCessPerc = $row.find("#hdnCustomSecHigherEduCessPerc").val();
        var customSecHigherEduCessAmt = $row.find("#hdnCustomSecHigherEduCessAmt").val();
        var customEducationalCessPerc = $row.find("#hdnCustomEducationalCessPerc").val();
        var customEducationalCessAmt = $row.find("#hdnCustomEducationalCessAmt").val();


        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
            }

                var piProduct = {
                        InvoiceProductDetailId: piProductDetailId,
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
                        IGST_Perc: iGSTPerc,
                        IGST_Amount: iGSTPercAmount,
                        HSN_Code: hsn_Code,
                        Total: totalPrice,
                        Currency: currency,
                        RITC: rITC,
                        AssValue: assValue,
                        CTH: cTH,
                        CETH: cETH,
                        CNotn: cNotn,
                        ENotn: eNotn,
                        Cnsno: eNsno,
                        Ensno: cNsno,
                        RSP: rSP,
                        LoadProv: loadProv,
                        BCDAmtRs: bCDAmtRs,
                        CVDAmtRs: cVDAmtRs,
                        CustomDutyPerc: customDutyPerc,
                        CustomDutyRate: customDutyRate,
                        ExciseDutyPerc: exciseDutyPerc,
                        ExciseDutyRate: exciseDutyRate,
                        GSTCessPerc: gSTCessPerc,
                        GSTCessAmt: gSTCessAmt,

                        EducationalCessOnCVDPerc: educationalCessOnCVDPerc,
                        EducationalCessOnCVDAmt: educationalCessOnCVDAmt,
                        SecHigherEduCessOnCVDPerc: secHigherEduCessOnCVDPerc,
                        SecHigherEduCessOnCVDAmt: secHigherEduCessOnCVDAmt,
                        CustomSecHigherEduCessPerc: customSecHigherEduCessPerc,
                        CustomSecHigherEduCessAmt: customSecHigherEduCessAmt,
                        CustomEducationalCessPerc: customEducationalCessPerc,
                        CustomEducationalCessAmt: customEducationalCessAmt
            };
                piProductList.push(piProduct);
                productEntrySequence = parseInt(productEntrySequence) +1;
            }
            else if (hdnProductId.val() == productId && hdnSequenceNo.val() == sequenceNo) {
                var piProductAddEdit = {
                        InvoiceProductDetailId: hdnPIProductDetailId.val(),
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
                        IGST_Perc: txtIGSTPerc.val(),
                        IGST_Amount: txtIGSTPercAmount.val(),
                        HSN_Code: txtHSN_Code.val(),
                        Total: txtTotalPrice.val().trim(),
                        Currency: ddlProductCurrency.val(),
                        RITC: txtRITC.val(),
                        AssValue: txtAssValue.val(),
                        CTH: txtCTH.val(),
                        CETH: txtCETH.val(),
                        CNotn: txtCNotn.val(),
                        ENotn: txtENotn.val(),
                        Cnsno: txtCNSNO.val(),
                        Ensno: txtENSNO.val(),
                        RSP: txtRSP.val(),
                        LoadProv: txtLoadProv.val(),
                        BCDAmtRs: txtBCDAmtRs.val(),
                        CVDAmtRs: txtCVDAmtRs.val(),
                        CustomDutyPerc: txtCustomDutyPerc.val(),
                        CustomDutyRate: txtCustomDutyRate.val(),
                        ExciseDutyPerc: txtExciseDutyPerc.val(),
                        ExciseDutyRate: txtExciseDutyRate.val(),
                        GSTCessPerc: txtGSTCessPerc.val(),
                        GSTCessAmt: txtGSTCessAmt.val(),

                        EducationalCessOnCVDPerc: txtEducationalCessOnCVDPerc.val(),
                        EducationalCessOnCVDAmt: txtEducationalCessOnCVDAmt.val(),
                        SecHigherEduCessOnCVDPerc: txtSecHigherEduCessOnCVDPerc.val(),
                        SecHigherEduCessOnCVDAmt: txtSecHigherEduCessOnCVDAmt.val(),
                        CustomSecHigherEduCessPerc: txtCustomEducationalCessPerc.val(),
                        CustomSecHigherEduCessAmt: txtCustomEducationalCessAmt.val(),
                        CustomEducationalCessPerc: txtCustomSecHigherEduCessPerc.val(),
                        CustomEducationalCessAmt: txtCustomSecHigherEduCessAmt.val()

            };
                piProductList.push(piProductAddEdit);
                hdnSequenceNo.val("0");
        }
    }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }

    
    if (action == 1) {
        var piProductAddEdit = {
                InvoiceProductDetailId: hdnPIProductDetailId.val(),
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
                IGST_Perc: txtIGSTPerc.val(),
                IGST_Amount: txtIGSTPercAmount.val(),
                HSN_Code: txtHSN_Code.val(),
                Total: txtTotalPrice.val().trim(),
                Currency: ddlProductCurrency.val(),
                RITC: txtRITC.val(),
                AssValue: txtAssValue.val(),
                CTH: txtCTH.val(),
                CETH: txtCETH.val(),
                CNotn: txtCNotn.val(),
                ENotn: txtENotn.val(),
                Cnsno: txtCNSNO.val(),
                Ensno: txtENSNO.val(),
                RSP: txtRSP.val(),
                LoadProv: txtLoadProv.val(),
                BCDAmtRs: txtBCDAmtRs.val(),
                CVDAmtRs: txtCVDAmtRs.val(),
                CustomDutyPerc: txtCustomDutyPerc.val(),
                CustomDutyRate: txtCustomDutyRate.val(),
                ExciseDutyPerc: txtExciseDutyPerc.val(),
                ExciseDutyRate: txtExciseDutyRate.val(),
                GSTCessPerc: txtGSTCessPerc.val(),
                GSTCessAmt: txtGSTCessAmt.val(),

                EducationalCessOnCVDPerc: txtEducationalCessOnCVDPerc.val(),
                EducationalCessOnCVDAmt: txtEducationalCessOnCVDAmt.val(),
                SecHigherEduCessOnCVDPerc: txtSecHigherEduCessOnCVDPerc.val(),
                SecHigherEduCessOnCVDAmt: txtSecHigherEduCessOnCVDAmt.val(),
                CustomSecHigherEduCessPerc: txtCustomEducationalCessPerc.val(),
                CustomSecHigherEduCessAmt: txtCustomEducationalCessAmt.val(),
                CustomEducationalCessPerc: txtCustomSecHigherEduCessPerc.val(),
                CustomEducationalCessAmt: txtCustomSecHigherEduCessAmt.val()
    };
        piProductList.push(piProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetPIProductList(piProductList);
    }

}
    function EditProductRow(obj) {
    var row = $(obj).closest("tr");
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var piProductDetailId = $(row).find("#hdnPIProductDetailId").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var price = $(row).find("#hdnPrice").val();
    var quantity = $(row).find("#hdnQuantity").val();
    var discountPerc = $(row).find("#hdnDiscountPerc").val();
    var discountAmount = $(row).find("#hdnDiscountAmount").val();
    var totalPrice = $(row).find("#hdnTotalPrice").val();
    var iGSTPerc = $(row).find("#hdnIGSTPerc").val();
    var iGSTAmount = $(row).find("#hdnIGSTAmount").val();
    var hsn_Code = $(row).find("#hdnHSN_Code").val();

    var rITC = $(row).find("#hdnRITC").val();
    var assValue = $(row).find("#hdnAssValue").val();
    var cTH = $(row).find("#hdnCTH").val();
    var cETH = $(row).find("#hdnCETH").val();
    var cNotn = $(row).find("#hdnCNotn").val();
    var eNotn = $(row).find("#hdnENotn").val();
    var cnsno = $(row).find("#hdnCNSNO").val();
    var ensno = $(row).find("#hdnENSNO").val();

    var rSP = $(row).find("#hdnRSP").val();
    var loadProv = $(row).find("#hdnLoadProv").val();
    var bCDAmtRs = $(row).find("#hdnBCDAmtRs").val();
    var cVDAmtRs = $(row).find("#hdnCVDAmtRs").val();
    var customDutyPerc = $(row).find("#hdnCustomDutyPerc").val();
    var customDutyRate = $(row).find("#hdnCustomDutyRate").val();
    var exciseDutyPerc = $(row).find("#hdnExciseDutyPerc").val();
    var exciseDutyRate = $(row).find("#hdnExciseDutyRate").val();
    var gSTCessPerc = $(row).find("#hdnGSTCessPerc").val();
    var gSTCessAmt = $(row).find("#hdnGSTCessAmt").val();
    var currency = $(row).find("#hdnCurrency").val();

    var educationalCessOnCVDPerc = $(row).find("#hdnEducationalCessOnCVDPerc").val();
    var educationalCessOnCVDAmt = $(row).find("#hdnEducationalCessOnCVDAmt").val();
    var secHigherEduCessOnCVDPerc = $(row).find("#hdnSecHigherEduCessOnCVDPerc").val()
    var secHigherEduCessOnCVDAmt = $(row).find("#hdnSecHigherEduCessOnCVDAmt").val();
    var customSecHigherEduCessPerc = $(row).find("#hdnCustomSecHigherEduCessPerc").val();
    var customSecHigherEduCessAmt = $(row).find("#hdnCustomSecHigherEduCessAmt").val();
    var customEducationalCessPerc = $(row).find("#hdnCustomEducationalCessPerc").val();
    var customEducationalCessAmt = $(row).find("#hdnCustomEducationalCessAmt").val();

    $("#txtProductName").val(productName);
    $("#hdnPIProductDetailId").val(piProductDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);
    $("#txtPrice").val(price);
    $("#txtQuantity").val(quantity);
    $("#txtDiscountPerc").val(discountPerc);
    $("#txtDiscountAmount").val(discountAmount);
    $("#txtTotalPrice").val(totalPrice);
    $("#txtIGSTPerc").val(iGSTPerc);
    $("#txtIGSTPercAmount").val(iGSTAmount);
    $("#txtHSN_Code").val(hsn_Code);
    $("#ddlProductCurrency").val(currency);
    $("#txtRITC").val(rITC);
    $("#txtAssValue").val(assValue);
    $("#txtCTH").val(cTH);
    $("#txtCETH").val(cETH);
    $("#txtCNotn").val(cNotn);
    $("#txtENotn").val(eNotn);
    $("#txtCNSNO").val(cnsno);
    $("#txtENSNO").val(ensno);

    $("#txtRSP").val(rSP);
    $("#txtExciseDutyPerc").val(exciseDutyPerc);
    $("#txtExciseDutyRate").val(exciseDutyRate);
    $("#txtCustomDutyPerc").val(customDutyPerc);
    $("#txtCustomDutyRate").val(customDutyRate);
    $("#txtBCDAmtRs").val(bCDAmtRs);
    $("#txtCVDAmtRs").val(cVDAmtRs);
    $("#txtGSTCessPerc").val(gSTCessPerc);
    $("#txtGSTCessAmt").val(gSTCessAmt);
    $("#txtLoadProv").val(loadProv);

    $("#txtEducationalCessOnCVDPerc").val(educationalCessOnCVDPerc);
    $("#txtEducationalCessOnCVDAmt").val(educationalCessOnCVDAmt);
    $("#txtSecHigherEduCessOnCVDPerc").val(secHigherEduCessOnCVDPerc);
    $("#txtSecHigherEduCessOnCVDAmt").val(secHigherEduCessOnCVDAmt);
    $("#txtCustomEducationalCessPerc").val(customSecHigherEduCessPerc);
    $("#txtCustomEducationalCessAmt").val(customSecHigherEduCessAmt);
    $("#txtCustomSecHigherEduCessPerc").val(customEducationalCessPerc);
    $("#txtCustomSecHigherEduCessAmt").val(customEducationalCessAmt);

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
    var hdnPIId = $("#hdnInvoiceId");
    var requestData = { piTerms: piTerms, invoiceId: hdnPIId.val()
    };
    $.ajax({
            url: "../PurchaseInvoiceImport/GetPurchaseInvoiceImportTermList",
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
                if (item.TermTemplateId == 1011) {
                    $("#ddlTermTemplate option:selected").append($("<option></option>").val(item.TermTemplateId).html(item.TermTempalteName));
                    BindTermsDescriptions();
                }
                else {
                    $("#ddlTermTemplate").append($("<option></option>").val(item.TermTemplateId).html(item.TermTempalteName));
            }
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
        var data = { termTemplateId: termTemplateId
    };

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
    $('#tblTermList tr').each(function (i, row) {
        var $row = $(row);
        var piTermDetailId = $row.find("#hdnPITermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (termDesc != undefined) {
            if (action == 1 || hdnPITermDetailId.val() != piTermDetailId) {

                if (termSequence == 0)
                { termSequence = termCounter;
            }

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

    function GetPurchaseInvoiceImportDetail(invoiceId) {
    $.ajax({
            type: "GET",
            asnc: false,
            url: "../PurchaseInvoiceImport/GetPurchaseInvoiceImportDetail",
            data: { invoiceId: invoiceId},
            dataType: "json",
            success: function (data) {
                $("#txtInvoiceNo").val(data.InvoiceNo);
                $("#txtInvoiceDate").val(data.InvoiceDate);
                $("#txtCustomStn").val(data.CustomStn);
                $("#txtCHA").val(data.CHA);
                $("#txtBENo").val(data.BENo);
                $("#txtBEDate").val(data.BEDate);
                $("#txtType").val(data.Type);
                $("#hdnVendorId").val(data.VendorId);
                $("#txtVendorCode").val(data.VendorCode);
                $("#txtVendorName").val(data.VendorName);
                $("#txtBAddress").val(data.BillingAddress);
                $("#ddlBCountry").val(data.CountryId);
                $("#txtBPinCode").val(data.PinCode);
                $("#txtBGSTNo").val(data.GSTNo);
                $("#hdnConsigneeId").val(data.ConsigneeId);
                $("#txtConsigneeCode").val(data.ConsigneeCode);
                $("#txtConsigneeName").val(data.ConsigneeName);
                $("#txtShippingAddress").val(data.ShippingAddress);
                $("#txtSCity").val(data.ShippingCity);
                $("#ddlSCountry").val(data.ShippingCountryId);
                $("#ddlSState").val(data.ShippingStateId);
                $("#txtSPinCode").val(data.ShippingPinCode);
                $("#txtSGSTNo").val(data.ConsigneeGSTNo);
                $("#txtLocalIGMNo").val(data.LocalIGMNo);
                $("#txtLocalIGMDate").val(data.LocalIGMDate);
                $("#txtGatewayIGMNo").val(data.LocalIGMNo);
                $("#txtGatewayIGMDate").val(data.LocalIGMDate);
                $("#ddlCountryOfOrigin").val(data.CountryOfOrigin);
                $("#txtPortOfReporing").val(data.txtPortOfReporing);
                $("#txtAdCode").val(data.AdCode);
                $("#txtForeigncurrency").val(data.ForeigncurrencyValue);
                $("#ddlForeigncurrency").val(data.ForeigncurrencyName);
                $("#ddlExchangecurrency").val(data.ExchangeRatecurrencyName);
                $("#txtExchangeRate").val(data.ExchangeRate);
                $("#ddlForeigncurrency").val(data.InvoiceValueCurrency);
               

                
                $("#txtPONo").val(data.PONo);
                $("#hdnPOId").val(data.POId);
                $("#txtCustomerInvoiceNo").val(data.CustomerInvoiceNo);
                $("#txtCustomerInvoiceDate").val(data.CustomerInvoiceDate);
                
            

                $("#ddlInvoiceStatus").val(data.InvoiceStatus);
              //  BindCompanyBranchList();
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            if (data.InvoiceStatus == "Final") {
                $("#btnUpdate").hide();
                $("#btnPrint").show();
                $("#btnPrintForm").show();
                $(".editonly").hide();
                $("#btnReset").hide();
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);
                if ($(".editonly").hide()) {
                    $('#txtPONo,#txtVendorCode').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
            }
                $("#btnReCalculate").hide();
                

            }
            $("#txtVendorAddress").val(data.BillingAddress);
            $("#ddlBCountry").val(data.CountryId);
            $("#txtBPinCode").val(data.PinCode);
            $("#txtBGSTNo").val(data.GSTNo);
            $("#txtBExciseNo").val(data.ExciseNo);
            $("#txtTOI").val(data.TOI);
            $("#txtSupplierInvoiceNo").val(data.SupplierInvoiceNo);
            $("#txtSupplierInvoiceDate").val(data.SupplierInvoiceDate);
            $("#txtPortOfLoading").val(data.PortOfLoading);
            $("#txtLocalIGMNo").val(data.LocalIGMNo);
            $("#txtLocalIGMDate").val(data.LocalIGMDate);
            $("#txtPortOfReporing").val(data.PortOfReporing);
            $("#txtGatewayIGMNo").val(data.GatewayIGMNo);
            $("#txtGatewayIGMDate").val(data.GatewayIGMDate);
            $("#ddlCountryOfConsignee").val(data.CountryOfConsignee);
            $("#ddlCountryOfOrigin").val(data.CountryOfOrigin);
            $("#txtBLNo").val(data.BLNo);
            $("#txtBLDate").val(data.BLDate);
            $("#txtHBLNo").val(data.HBLNo);
            $("#txtNoOfPkgs").val(data.NoOfPkgs);
            BindPurchaseUOMList();
            $("#ddlNoOfPkgsUnit").val(data.NoOfPkgsUnit);
            $("#txtGrossWt").val(data.GrossWt);
            BindPurchaseUOMList();
            $("#ddlGrossWtUnit").val(data.GrossWtUnit);
            $("#txtInvoiceValue").val(data.InvoiceValue);
            $("#ddlInvoiceValueCurrency").val(data.InvoiceValueCurrency);
            $("#txtMark").val(data.Mark);
            $("#txtCC").val(data.CC);
            $("#txtSVBLoadingASS").val(data.SVBLoadingASS);
            $("#txtSVBLoadingDty").val(data.SVBLoadingDty);
            $("#txtCustHouseNo").val(data.CustHouseNo);
            $("#txtHSSLoadRate").val(data.HSSLoadRate);
            $("#txtHSSAmount").val(data.HSSAmount);
            $("#txtMiscCharges").val(data.MiscCharges);
            $("#txtEDD").val(data.EDD);
            $("#txtThirdParty").val(data.ThirdParty);
            $("#txtXBEDutyFGInt").val(data.XBEDutyFGInt);
            $("#txtBuyerSellarRelted").val(data.BuyerSellarRelted);
            $("#ddlPaymentMethod").val(data.PaymentMethod);
           
            $("#txtRefNo").val(data.RefNo);
            $("#txtRefDate").val(data.RefDate);
            $("#txtRemarks").val(data.Remarks);
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
            $("#txtGrossValue").val(data.GrossValue);
            $("#txtRoundOfValue").val(data.RoundOfValue);
            $("#txtFreight").val(data.Freight);
            $("#ddlFreightCurrency").val(data.FreightCurrency);
            $("#txtInsurence").val(data.Insurence);
            $("#ddlInsurenceCurrency").val(data.InsurenceCurrency);

            $("#txtEducationalCessOnCVDPerc").val(data.EducationalCessOnCVDPerc);
            $("#txtEducationalCessOnCVDAmt").val(data.EducationalCessOnCVDAmt);
            $("#txtSecHigherEduCessOnCVDPerc").val(data.SecHigherEduCessOnCVDPerc);
            $("#txtSecHigherEduCessOnCVDAmt").val(data.SecHigherEduCessOnCVDAmt);
            $("#txtCustomSecHigherEduCessPerc").val(data.CustomSecHigherEduCessPerc);
            $("#txtCustomSecHigherEduCessAmt").val(data.CustomSecHigherEduCessAmt);
            $("#txtCustomEducationalCessPerc").val(data.CustomEducationalCessPerc);
            $("#txtCustomEducationalCessAmt").val(data.CustomEducationalCessAmt);
            $("#btnAddNew").show();
            $("#btnPrint").show();
            $("#btnEmail").show();
            BindConsigneeBranchList(data.ConsigneeId);
                //GetOtherChargesGSTPercentage();

    },
            error: function (Result) {
            ShowModel("Alert", "Problem in Request");
    }
    });

}

    function SaveData() {
    var txtInvoiceNo = $("#txtInvoiceNo");
    var hdnInvoiceId = $("#hdnInvoiceId");
    var txtInvoiceDate = $("#txtInvoiceDate");
    var txtCustomStn = $("#txtCustomStn");

    var txtPONo = $("#txtPONo");
    var hdnPOId = $("#hdnPOId");
    var txtCustomerInvoiceNo = $("#txtCustomerInvoiceNo");
    var txtCustomerInvoiceDate = $("#txtCustomerInvoiceDate");

    var txtCHA = $("#txtCHA");
    var txtBENo = $("#txtBENo");
    var txtBEDate = $("#txtBEDate");
    var hdnVendorId = $("#hdnVendorId");
    var txtVendorName = $("#txtVendorName");
    var txtVendorAddress = $("#txtVendorAddress");
    var txtSupplierInvoiceNo = $("#txtSupplierInvoiceNo");
    var txtSupplierInvoiceDate = $("#txtSupplierInvoiceDate");
    var txtCity = $("#txtBCity");
    var ddlCountry = $("#ddlBCountry");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var ddlState = $("#ddlBState");
    var txtPinCode = $("#txtBPinCode");
    var txtGSTNo = $("#txtBGSTNo");
    var txtBExciseNo = $("#txtBExciseNo");
    var hdnConsigneeId = $("#hdnConsigneeId");
    var txtConsigneeName = $("#txtConsigneeName");
    var txtShippingAddress = $("#txtShippingAddress");
    var txtAddress = $("#txtBAddress");
    var txtSCity = $("#txtSCity");
    var ddlSCountry = $("#ddlSCountry");
    var ddlSState = $("#ddlSState");
    var txtSPinCode = $("#txtSPinCode");
    var txtSGSTNo = $("#txtSGSTNo");
    var txtAdCode = $("#txtAdCode");

   

        /*---------------------------------*/
    var txtPortOfLoading = $("#txtPortOfLoading");
    var txtLocalIGMNo = $("#txtLocalIGMNo");
    var txtLocalIGMDate = $("#txtLocalIGMDate");
    var txtPortOfReporing = $("#txtPortOfReporing");
    var txtGatewayIGMNo = $("#txtGatewayIGMNo");
    var txtGatewayIGMDate = $("#txtGatewayIGMDate");
    var ddlCountryOfOrigin = $("#ddlCountryOfOrigin");
    var ddlCountryOfConsignee = $("#ddlCountryOfConsignee");
    var txtBLNo = $("#txtBLNo");
    var txtBLDate = $("#txtBLDate");
    var txtHBLNo = $("#txtHBLNo");
    var txtHBLDate = $("#txtHBLDate");
    var txtNoOfPkgs = $("#txtNoOfPkgs");
    var ddlNoOfPkgsUnit = $("#ddlNoOfPkgsUnit");
    var txtGrossWt = $("#txtGrossWt");
    var ddlGrossWtUnit = $("#ddlGrossWtUnit");
    var txtInvoiceValue = $("#txtInvoiceValue");
    var ddlInvoiceValueCurrency = $("#ddlInvoiceValueCurrency");
    var txtTOI = $("#txtTOI");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtMark = $("#txtMark");
    var txtFreight = $("#txtFreight");
    var ddlFreightCurrency = $("#ddlFreightCurrency");
    var txtInsurence = $("#txtInsurence");
    var ddlInsurenceCurrency = $("#ddlInsurenceCurrency");
    var txtSVBLoadingASS = $("#txtSVBLoadingASS");
    var txtSVBLoadingDty = $("#txtSVBLoadingDty");
    var txtCustHouseNo = $("#txtCustHouseNo");
    var txtHSSLoadRate = $("#txtHSSLoadRate");
    var txtHSSAmount = $("#txtHSSAmount");
    var txtMiscCharges = $("#txtMiscCharges");
    var txtEDD = $("#txtEDD");
    var txtThirdParty = $("#txtThirdParty");
    var txtXBEDutyFGInt = $("#txtXBEDutyFGInt");
    var txtBuyerSellarRelted = $("#txtBuyerSellarRelted");
    var ddlPaymentMethod = $("#ddlPaymentMethod");
    
    var txtRemarks = $("#txtRemarks");
    var txtBasicValue = $("#txtBasicValue");
    var txtGrossValue = $("#txtGrossValue");
    var txtRoundOfValue = $("#txtRoundOfValue");
    var txtTotalValue = $("#txtTotalValue");
    var txtType = $("#txtType");
    var txtCC = $("#txtCC");
    var txtRefNo = $("#txtRefNo");
    var txtRefDate = $("#txtRefDate");
    var ddlInvoiceStatus = $("#ddlInvoiceStatus");
    var txtEducationalCessOnCVDPerc = 0;
    var txtEducationalCessOnCVDAmt = 0;
    var txtSecHigherEduCessOnCVDPerc = 0;
    var txtSecHigherEduCessOnCVDAmt = 0;
    var txtCustomSecHigherEduCessPerc =0;
    var txtCustomSecHigherEduCessAmt =0;
    var txtCustomEducationalCessPerc = 0;
    var txtCustomEducationalCessAmt = 0;

    var txtForeigncurrency = $("#txtForeigncurrency");
    var ddlForeigncurrency = $("#ddlForeigncurrency");
    var ddlExchangecurrency = $("#ddlExchangecurrency");
    var txtExchangeRate = $("#txtExchangeRate");

    if (ddlCompanyBranch.val() == "0" || ddlCompanyBranch.val() == "") {
        ShowModel("Alert", "Please Select Company Branch.")
        ddlCompanyBranch.focus();
        return false;
    }
    if (ddlForeigncurrency.val() == "0" || ddlForeigncurrency.val() == "") {
        ShowModel("Alert", "Please Select Foreign currency")
        ddlForeigncurrency.focus();
        return false;
    }

    if (txtForeigncurrency.val().trim() == "") {
        ShowModel("Alert", "Please Enter Foreign currency Value")
        txtForeigncurrency.focus();
        return false;
    }
    if (ddlExchangecurrency.val() == "0" || ddlExchangecurrency.val() == "") {
        ShowModel("Alert", "Please Select Exchange currency")
        ddlExchangecurrency.focus();
        return false;
    }

    if (txtExchangeRate.val().trim() == "") {
        ShowModel("Alert", "Please Enter Exchange currency Value")
        txtExchangeRate.focus();
        return false;
    }
  
    var piViewModel = {
            InvoiceId: hdnInvoiceId.val(),
            InvoiceNo: txtInvoiceNo.val().trim(),
            InvoiceDate: txtInvoiceDate.val().trim(),
            CustomStn: txtCustomStn.val().trim(),
            CHA: txtCHA.val().trim(),
            BENo: txtBENo.val().trim(),
            BEDate: txtBEDate.val().trim(),
            CC: txtCC.val().trim(),
            Type: txtType.val().trim(),
            POId: 0,
            PONo: "",
            ConsigneeId: hdnConsigneeId.val(),
            ConsigneeName: txtConsigneeName.val(),
            ShippingAddress: txtShippingAddress.val().trim(),
            ShippingCity: txtSCity.val().trim(),
            ShippingStateId: ddlSState.val(),
            ShippingCountryId: ddlSCountry.val(),
            ShippingPinCode: txtSPinCode.val().trim(),
            ConsigneeGSTNo: txtSGSTNo.val().trim(),
            VendorId: hdnVendorId.val().trim(),
            VendorName: txtVendorName.val().trim(),
            BillingAddress: txtVendorAddress.val().trim(),
            CountryId: ddlCountry.val(),
            PinCode: txtPinCode.val().trim(),
            GSTNo: "",
            ExciseNo: txtBExciseNo.val().trim(),
            LocalIGMNo: txtLocalIGMNo.val().trim(),
            LocalIGMDate: txtLocalIGMDate.val().trim(),
            GatewayIGMNo: txtGatewayIGMNo.val().trim(),
            GatewayIGMDate: txtGatewayIGMDate.val().trim(),
            PortOfLoading: txtPortOfLoading.val(),
            PortOfReporing: txtPortOfReporing.val(),
            CountryOfOrigin: ddlCountryOfOrigin.val(),
            CountryOfConsignee: ddlCountryOfConsignee.val(),
            BLNo: txtBLNo.val(),
            BLDate: txtBLDate.val(),
            HBLNo: txtHBLNo.val(),
            HBLDate: txtHBLDate.val(),
            NoOfPkgs: txtNoOfPkgs.val(),
            NoOfPkgsUnit: txtNoOfPkgs.val(),
            GrossWt: txtGrossWt.val(),
            GrossWtUnit: ddlGrossWtUnit.val(),
            Mark: txtMark.val().trim(),
            SupplierInvoiceNo: txtSupplierInvoiceNo.val().trim(),
            SupplierInvoiceDate: txtSupplierInvoiceDate.val().trim(),
            InvoiceValue: txtInvoiceValue.val(),
            InvoiceValueCurrency: ddlInvoiceValueCurrency.val(),
            TOI: txtTOI.val(),
            Freight: txtFreight.val(),
            FreightCurrency: ddlFreightCurrency.val(),
            Insurence: txtInsurence.val(),
            InsurenceCurrency: ddlInsurenceCurrency.val(),
            SVBLoadingASS: txtSVBLoadingASS.val(),
            SVBLoadingDty: txtSVBLoadingDty.val(),
            CustHouseNo: txtCustHouseNo.val(),
            HSSLoadRate: txtHSSLoadRate.val(),
            HSSAmount: txtHSSAmount.val(),
            MiscCharges: txtMiscCharges.val(),
            EDD: txtEDD.val(),
            ThirdParty: txtThirdParty.val(),
            XBEDutyFGInt: txtXBEDutyFGInt.val(),
            BuyerSellarRelted: txtBuyerSellarRelted.val(),
            PaymentMethod: ddlPaymentMethod.val(),
            RefNo: txtRefNo.val(),
            RefDate: txtRefDate.val(),
            CompanyBranchId: ddlCompanyBranch.val(),
            Remarks: txtRemarks.val(),
            InvoiceStatus: ddlInvoiceStatus.val(),
            EducationalCessOnCVDPerc: txtEducationalCessOnCVDPerc,
            EducationalCessOnCVDAmt: txtEducationalCessOnCVDAmt,
            SecHigherEduCessOnCVDPerc: txtSecHigherEduCessOnCVDPerc,
            SecHigherEduCessOnCVDAmt: txtSecHigherEduCessOnCVDAmt,
            CustomSecHigherEduCessPerc: txtCustomSecHigherEduCessPerc,
            CustomSecHigherEduCessAmt: txtCustomSecHigherEduCessAmt,
            CustomEducationalCessPerc: txtCustomEducationalCessPerc,
            CustomEducationalCessAmt: txtCustomEducationalCessAmt,
            BasicValue: txtBasicValue.val(),
            TotalValue: txtTotalValue.val(),
            RoundOfValue: txtRoundOfValue.val(),
            GrossValue: txtGrossValue.val(),
            AdCode: txtAdCode.val(),
            ForeigncurrencyValue: txtForeigncurrency.val(),
            ForeigncurrencyName: ddlForeigncurrency.val(),
            ExchangeRatecurrencyName: ddlExchangecurrency.val(),
            ExchangeRate: txtExchangeRate.val(),
            PONo: txtPONo.val(),
            POId: hdnPOId.val(),
            CustomerInvoiceNo: txtCustomerInvoiceNo.val(),
            CustomerInvoiceDate: txtCustomerInvoiceDate.val()

    };

    var piProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var piProductDetailId = $row.find("#hdnPIProductDetailId").val();
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
        var iGSTPerc = $row.find("#hdnIGSTPerc").val();
        var iGSTPercAmount = $row.find("#hdnIGSTAmount").val();
        var hsn_Code = $row.find("#hdnHSN_Code").val();

        var currency = $row.find("#hdnCurrency").val();
        var ritc = $row.find("#hdnRITC").val();
        var assValue = $row.find("#hdnAssValue").val();

        var cth = $row.find("#hdnCTH").val();
        var ceth = $row.find("#hdnCETH").val();
        var cnotn = $row.find("#hdnCNotn").val();
        var enotn = $row.find("#hdnENotn").val();
        var rsp = $row.find("#hdnRSP").val();
        var loadprov = $row.find("#hdnLoadProv").val();
        var bcdamtrs = $row.find("#hdnBCDAmtRs").val();
        var cvdamtrs = $row.find("#hdnCVDAmtRs").val();
        var customDutyRate = $row.find("#hdnCustomDutyRate").val();
        var exciseDutyRate = $row.find("#hdnExciseDutyRate").val();
        var gSTCessPerc = $row.find("#hdnGSTCessPerc").val();
        var gSTCessAmt = $row.find("#hdnGSTCessAmt").val();

        var customDutyPerc = $row.find("#hdnCustomDutyPerc").val();
        var customDutyRate = $row.find("#hdnCustomDutyRate").val();
        var exciseDutyPerc = $row.find("#hdnExciseDutyPerc").val();
        var exciseDutyRate = $row.find("#hdnExciseDutyRate").val();


       
        var cnsno = $row.find("#hdnCNSNO").val();
        var ensno = $row.find("#hdnENSNO").val();


        var educationalCessOnCVDPerc = $row.find("#hdnEducationalCessOnCVDPerc").val();
        var educationalCessOnCVDAmt = $row.find("#hdnEducationalCessOnCVDAmt").val();
        var secHigherEduCessOnCVDPerc = $row.find("#hdnSecHigherEduCessOnCVDPerc").val();
        var secHigherEduCessOnCVDAmt = $row.find("#hdnSecHigherEduCessOnCVDAmt").val();
        var customSecHigherEduCessPerc = $row.find("#hdnCustomSecHigherEduCessPerc").val();
        var customSecHigherEduCessAmt = $row.find("#hdnCustomSecHigherEduCessAmt").val();
        var customEducationalCessPerc = $row.find("#hdnCustomEducationalCessPerc").val();
        var customEducationalCessAmt = $row.find("#hdnCustomEducationalCessAmt").val();


        if (productName != undefined) {

            var piProduct = {
                    ProductId: productId,
                    ProductShortDesc: productShortDesc,
                    Price: price,
                    Quantity: quantity,
                    Currency: currency,
                    DiscountPercentage: discountPerc,
                    DiscountAmount: discountAmount,
                    IGST_Perc: iGSTPerc,
                    IGST_Amount: iGSTPercAmount,
                    HSN_Code: iGSTPercAmount,
                    Total: totalPrice,
                    RITC: ritc,
                    AssValue: assValue,
                    CTH: cth,
                    CETH: ceth,
                    CNotn: cnotn,
                    ENotn: enotn,
                    RSP: rsp,
                    LoadProv: loadprov,
                    BCDAmtRs: bcdamtrs,
                    CVDAmtRs: cvdamtrs,
                    CustomDutyPerc: customDutyPerc,
                    CustomDutyRate: customDutyRate,
                    ExciseDutyPerc: exciseDutyPerc,
                    ExciseDutyRate: exciseDutyRate,
                    GSTCessPerc: gSTCessPerc,
                    GSTCessAmt: gSTCessAmt,
                    Cnsno: cnsno,
                    Ensno: cnsno,

                    EducationalCessOnCVDPerc: educationalCessOnCVDPerc,
                    EducationalCessOnCVDAmt: educationalCessOnCVDAmt,
                    SecHigherEduCessOnCVDPerc: secHigherEduCessOnCVDPerc,
                    SecHigherEduCessOnCVDAmt: secHigherEduCessOnCVDAmt,
                    CustomSecHigherEduCessPerc: customSecHigherEduCessPerc,
                    CustomSecHigherEduCessAmt: customSecHigherEduCessAmt,
                    CustomEducationalCessPerc: customEducationalCessPerc,
                    CustomEducationalCessAmt: customEducationalCessAmt,
        };
            piProductList.push(piProduct);
    }
    });

    var piTermList = [];
    $('#tblTermList tr').each(function (i, row) {
        var $row = $(row);
        var piTermDetailId = $row.find("#hdnPITermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (termDesc != undefined) {
            var piTerm = {
                    PITermDetailId: piTermDetailId,
                    TermDesc: termDesc,
                    TermSequence: termSequence
        };
            piTermList.push(piTerm);
    }

    });


    var accessMode = 1;//Add Mode
    if (hdnInvoiceId.val() != null && hdnInvoiceId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { piViewModel: piViewModel, piProducts: piProductList, piTerms: piTermList
    };
    $.ajax({
            url: "../PurchaseInvoiceImport/AddEditPurchaseInvoiceImport?accessMode=" + accessMode + "",
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
                      window.location.href = "../PurchaseInvoiceImport/AddEditPurchaseInvoiceImport?invoiceId=" + data.trnId + "&AccessMode=3";
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

        $("#txtInvoiceNo").val("");
        $("#hdnInvoiceId").val("0");
        $("#txtInvoiceDate").val($("#hdnCurrentDate").val());
        $("#txtCustomStn").val("");
        $("#hdnVendorId").val("0");
        $("#txtVendorName").val("");
        $("#txtVendorCode").val("");
        $("#txtBAddress").val("");
        $("#ddlBCountry").val("0");
        $("#txtBPinCode").val("");
        $("#txtRefNo").val("");
        $("#txtRefDate").val("");
        $("#ddlInvoiceStatus").val("Draft");
        $("#btnSave").show();
        $("#btnUpdate").hide();


}
    function GetVendorDetail(vendorId) {
    $.ajax({
            type: "GET",
            asnc: false,
            url: "../Vendor/GetVendorDetail",
            data: { vendorId: vendorId
    },
            dataType: "json",
            success: function (data) {
                $("#txtVendorAddress").val(data.Address);
            $("#txtBCity").val(data.City);
            $("#ddlBCountry").val(data.CountryId);
            $("#ddlBState").val(data.StateId);
            $("#txtBPinCode").val(data.PinCode);
            $("#txtBTINNo").val(data.TINNo);
            $("#txtBGSTNo").val(data.GSTNo);
            if (data.GST_Exempt =='1') {
                $("#divGSTExempt").show();
                $("#lblGSTExemptStatus").attr('readOnly', true);
                $("#lblGSTExemptStatus").val("Unregistered Vendor");
            }
            else {
                $("#divGSTExempt").hide();
                $("#lblGSTExemptStatus").attr('readOnly', true);
                $("#lblGSTExemptStatus").val("");
            }

            

         


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
            data: { customerId: consigneeId
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
    BindConsigneeBranchList(consigneeId)
}

    function GetProductDetail(productId) {
    $.ajax({
            type: "GET",
            asnc: false,
            url: "../Product/GetProductDetail",
            data: { productid: productId
    },
            dataType: "json",
            success: function (data) {
            $("#txtPrice").val(data.PurchasePrice);
            $("#txtQuantity").val("");
            $("#txtUOMName").val(data.UOMName);
    },
            error: function (Result) {
            ShowModel("Alert", "Problem in Request");
    }
    });

}
    function ShowHideProductPanel(action) {
    if (action == 1) {
        var txtExchangeRate = $("#txtExchangeRate");
        if (txtExchangeRate.val() == "" || txtExchangeRate.val() == "0" || txtExchangeRate.val() < "0") {
            ShowModel("Alert", "Please Enter Exchange Rate");
            return false;
    }
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
        $("#txtDiscountPerc").val("");
        $("#txtDiscountAmount").val("");
        $("#txtHSN_Code").val("");
        $("#txtIGSTPerc").val("");
        $("#txtIGSTPercAmount").val("");
        $("#txtRITC").val("");
        $("#txtAssValue").val("");
        $("#txtCTH").val("");
        $("#txtCETH").val("");
        $("#txtCNotn").val("");
        $("#txtENotn").val("");
        $("#txtRSP").val("");
        $("#txtCustomDutyPerc").val("");
        $("#txtCustomDutyRate").val("");
        $("#txtExciseDutyPerc").val("");
        $("#txtExciseDutyRate").val("");
        $("#txtBCDAmtRs").val("");
        $("#txtCVDAmtRs").val("");
        $("#txtGSTCessPerc").val("");
        $("#txtGSTCessAmt").val("");
        $("#txtLoadProv").val("");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
        $("#txtGSTCessAmt").val("");
        $("#txtCNSNO").val("");
        $("#txtENSNO").val("");
        $("#txtEducationalCessOnCVDPerc").val("");
        $("#txtEducationalCessOnCVDAmt").val("");
        $("#txtSecHigherEduCessOnCVDPerc").val("");
        $("#txtSecHigherEduCessOnCVDAmt").val("");
        $("#txtCustomEducationalCessPerc").val("");
        $("#txtCustomEducationalCessAmt").val("");
        $("#txtCustomSecHigherEduCessPerc").val("");
        $("#txtCustomSecHigherEduCessAmt").val("");
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

    function SendMail() {
    var hdnPIId = $("#hdnPIId");
    var requestData = { piId: hdnPIId.val(), reportType: "PDF"
    };
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
    function changeReverseChargeStatus() {
    if ($("#chkReverseChargeApplicable").is(':checked')) {
        var CGSTAmount = 0;
        var SGSTAmount = 0;
        var IGSTAmount = 0;
        var TotalGST = 0;
        var ddlSState = $("#ddlSState");
        var hdnBillingStateId = $("#ddlBState");
        $('#tblProductList tr').each(function (i, row) {

            var $row = $(row);

            var hdnProductId = $row.find("#hdnProductId").val();
            if (hdnProductId != undefined) {
                if (ddlSState.val() == hdnBillingStateId.val()) {
                    CGSTAmount = parseFloat(CGSTAmount) + parseFloat($row.find("#hdnCGSTAmount").val());
                    SGSTAmount = parseFloat(SGSTAmount) + parseFloat($row.find("#hdnSGSTAmount").val());
                    IGSTAmount = 0;
                }
                else {
                    CGSTAmount = 0;
                    SGSTAmount = 0;
                    IGSTAmount = parseFloat(IGSTAmount) + parseFloat($row.find("#hdnIGSTAmount").val());
            }
        }
            if (CGSTAmount > 0 && SGSTAmount > 0) {
                TotalGST = CGSTAmount +SGSTAmount;
            }
            else if (IGSTAmount > 0) {
                TotalGST = IGSTAmount;
        }
        });
        $("#txtReverseChargeAmount").val(parseFloat(TotalGST).toFixed(2));


        $("#txtReverseChargeAmount").attr("disabled", true);
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
                    data: { productId: hdnProductId
            },
                    dataType: "json",
                    asnc: false,
                    success: function (data) {
                    if ($("#lblGSTExemptStatus").val() != "" && $("#lblGSTExemptStatus").val() == "Unregistered Vendor") {
                        if (hdnCustomerStateId.val() == hdnBillingStateId.val()) {
                            $row.find("#hdnCGSTPerc").val(0);
                            $row.find("#hdnSGSTPerc").val(0);
                            $row.find("#hdnIGSTPerc").val(0);
                        }
                        else {
                            $row.find("#hdnCGSTPerc").val(0);
                            $row.find("#hdnSGSTPerc").val(0);
                            $row.find("#hdnIGSTPerc").val(0);
                    }

                    }
                    else {
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
    }, 2000);


}

    function ReCalculateNetValues() {
    var basicValue = 0;
    var taxValue = 0;

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

            if (parseFloat(IGSTPerc) > 0) {
                IGSTAmount = ((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(IGSTPerc)) / 100;
        }

            $row.find("#tdIGSTAmount").html(IGSTAmount.toFixed(2));
            $row.find("#tdIGST_Perc").html(IGSTPerc);
            $row.find("#hdnIGSTAmount").val(IGSTAmount.toFixed(2));
            $row.find("#hdnIGSTPerc").val(IGSTPerc);
            if (totalPrice != undefined) {
                var itemTotal = parseFloat(totalPrice - discountAmount +IGSTAmount);
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



    $("#txtBasicValue").val(basicValue.toFixed(2));
    $("#txtGrossValue").val(parseFloat(parseFloat(basicValue) + parseFloat(taxValue) + parseFloat(freightValue) + parseFloat(freightCGST_Amount) + parseFloat(freightSGST_Amount) + parseFloat(freightIGST_Amount) + parseFloat(loadingValue) + parseFloat(loadingCGST_Amount) + parseFloat(loadingSGST_Amount) + parseFloat(loadingIGST_Amount) + parseFloat(insuranceValue) + parseFloat(insuranceCGST_Amount) + parseFloat(insuranceSGST_Amount) + parseFloat(insuranceIGST_Amount)).toFixed(2));
    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(taxValue) + parseFloat(freightValue) + parseFloat(freightCGST_Amount) + parseFloat(freightSGST_Amount) + parseFloat(freightIGST_Amount) + parseFloat(loadingValue) + parseFloat(loadingCGST_Amount) + parseFloat(loadingSGST_Amount) + parseFloat(loadingIGST_Amount) + parseFloat(insuranceValue) + parseFloat(insuranceCGST_Amount) + parseFloat(insuranceSGST_Amount) + parseFloat(insuranceIGST_Amount)).toFixed(0));
    var grossValue = $("#txtGrossValue").val();
    var str = grossValue.split('.');
    var roundVal = parseFloat("0." + str[1]);

    var round_Amt;
    if (roundVal == '0.00') {
        round_Amt = roundVal;
    }
    else if (roundVal < 0.50) {

        round_Amt = "-" +roundVal;
    }
    else {
        var roundAmt = parseInt(100 - str[1]);
        if (roundAmt > 0 && roundAmt < 10) {
            round_Amt = "0.0" +roundAmt;
        }
        else {
            round_Amt = "0." +roundAmt;
    }

    }
    $("#txtRoundOfValue").val(round_Amt);

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
    function ShowHideProductModel() {
        //  $("#AddProductModel").modal();
    CheckMasterPermission($("#hdnRoleId").val(), 13, 'AddProductModel');
}
    function BindProductDetail(productId) {
    $.ajax({
            type: "GET",
            asnc: false,
            url: "../Product/GetProductDetail",
            data: { productid: productId
    },
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
    //open master vendor pop up----------
    function OpenVendorMasterPopup() {
        //$("#AddNewVendor").modal();
    CheckMasterPermission($("#hdnRoleId").val(), 35, 'AddNewVendor');
}

    function BindTermsDescriptions() {
    var termTemplateId = 1011;
    var piTermList = [];
    if (termTemplateId != undefined && termTemplateId != "" && termTemplateId != "0") {
        var data = { termTemplateId: termTemplateId
    };

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
    //Check Pop Up Master Permissions by User Role, Master Id
    function CheckMasterPermission(RoleId, InterfaceId, ModalId) {
    var IsAuthorized = false;
    var AccessMode = 1;
    $.ajax({
            type: "GET",
            url: "../Role/CheckMasterPermission",
            data: { roleId: RoleId, interfaceId: InterfaceId, accessMode: AccessMode
    },
            dataType: "json",
            asnc: true,
            success: function (data) {
            if (data != null) {
                if (data == true) {
                    IsAuthorized = true;
                    if (IsAuthorized == true) {
                        $("#" +ModalId).modal();
                }
                }
                else {
                    ShowModel('Alert', 'You are not authorized for this action.');
            }
            }
    },
            error: function (Result) {
            ShowModel('Alert', 'Problem in Request');
    }
    });

}


    //Check Pop Up Master Permissions by User Role, Master Id
    function CheckMasterPermission(RoleId, InterfaceId, ModalId) {
    var IsAuthorized = false;
    var AccessMode = 1;
    $.ajax({
            type: "GET",
            url: "../Role/CheckMasterPermission",
            data: { roleId: RoleId, interfaceId: InterfaceId, accessMode: AccessMode
    },
            dataType: "json",
            asnc: true,
            success: function (data) {
            if (data != null) {
                if (data == true) {
                    IsAuthorized = true;
                    if (IsAuthorized == true) {
                        $("#" +ModalId).modal();
                }
                }
                else {
                    ShowModel('Alert', 'You are not authorized for this action.');
            }
            }
    },
            error: function (Result) {
            ShowModel('Alert', 'Problem in Request');
    }
    });

}

    function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../PurchaseInvoice/AddEditPI?accessMode=1";
    }
}


    function BindCountryList() {
    $.ajax({
            type: "GET",
            url: "../City/GetCountryList",
            data: "{}",
            dataType: "json",
            asnc: false,
            success: function (data) {
            $("#ddlCountryOfOrigin,#ddlCountryOfConsignee").append($("<option></option>").val(0).html("-Select Country-"));
            $.each(data, function (i, item) {

                $("#ddlCountryOfOrigin,#ddlCountryOfConsignee").append($("<option></option>").val(item.CountryId).html(item.CountryName));
            });
    },
            error: function (Result) {
            $("#ddlCountryOfOrigin,#ddlCountryOfConsignee").append($("<option></option>").val(0).html("-Select Country-"));
    }
    });
}

    function BindPurchaseUOMList() {
    $.ajax({
            type: "GET",
            url: "../Product/GetUOMList",
            data: "{}",
            dataType: "json",
            asnc: false,
            success: function (data) {
            $("#ddlNoOfPkgsUnit,#ddlGrossWtUnit").append($("<option></option>").val(0).html("-Select UOM-"));
            $.each(data, function (i, item) {
                $("#ddlNoOfPkgsUnit,#ddlGrossWtUnit").append($("<option></option>").val(item.UOMId).html(item.UOMName));
            });
    },
            error: function (Result) {
            $("#ddlNoOfPkgsUnit,#ddlGrossWtUnit").append($("<option></option>").val(0).html("-Select UOM-"));
    }
    });
    }

    function OpenPOSearchPopup() {
        if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
            ShowModel("Alert", "Please Select Company Branch");
            return false;
        }
        $("#divPOList").html("");
        $("#SearchPOModel").modal();

    }
    function SearchPO() {
        var txtSearchPONo = $("#txtSearchPONo");
        var txtVendorName = $("#txtSearchVendorName");

        var txtRefNo = $("#txtSearchRefNo");
        var txtSearchCreatedBy = $("#txtSearchCreatedBy");
        var txtFromDate = $("#txtSearchFromDate");
        var txtToDate = $("#txtSearchToDate");
        var ddlCompanyBranch = $("#ddlCompanyBranch");
        var requestData = {
            poNo: txtSearchPONo.val().trim(), vendorName: txtVendorName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(),
            approvalStatus: "Approved", displayType: "Popup", CreatedByUserName: txtSearchCreatedBy.val(),
            companyBranch: ddlCompanyBranch.val(), poType:"Import"
        };
        $.ajax({
            url: "../PurchaseInvoiceImport/GetPurchaseOrderList",
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
    function SelectPO(poId, poNo, poDate, vendorId, vendorCode, vendorName) {
        GetPODetail(poId);
        
        $("#btnAddNewVendor").hide();
        $("#btnAddPopupProduct").hide();
        $("#txtVendorName").attr('disabled', true);
        $("#txtConsigneeName").attr('disabled', true);
        $("#ddlCompanyBranch").attr('disabled', true);
        $("#ddlInvoiceValueCurrency").attr('disabled', true);

        setTimeout(
function () {
    var poProducts = [];
    GetPOProductList(poProducts, poId);
}, 2000);
       
        $("#SearchPOModel").modal('hide');

    }
    function GetPODetail(poId) {
        $.ajax({
            type: "GET",
            asnc: false,
            url: "../PO/GetPODetail",
            data: { poId: poId },
            dataType: "json",
            success: function (data) {
                $("#hdnPOId").val(poId);
                $("#txtPONo").val(data.PONo);
                $("#txtPODate").val(data.PODate);
                $("#ddlInvoiceValueCurrency").val(data.CurrencyCode);
                $("#ddlExchangecurrency").val("INR");
                
                $("#ddlForeigncurrency").val(data.CurrencyCode);

               $("#txtForeigncurrency").val(1);
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
                $("#txtBPinCode").val(data.PinCode);

                $("#txtBTINNo").val(data.TINNo);

                $("#txtBGSTNo").val(data.GSTNo);

                $("#txtSCity").val(data.ShippingCity);
                $("#ddlSCountry").val(data.ShippingCountryId);
                $("#ddlSState").val(data.ShippingStateId);
                $("#txtSPinCode").val(data.ShippingPinCode);
                $("#txtSGSTNo").val(data.ConsigneeGSTNo);

                $("#txtVendorAddress").val(data.BillingAddress);
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
                $("#txtExchangeRate").val(data.CurrencyConversionRate);
                if (data.ReverseChargeApplicable == true) {
                    $("#chkReverseChargeApplicable").prop("checked", true);
                    changeReverseChargeStatus();
                }
                $("#txtReverseChargeAmount").val(data.ReverseChargeAmount);
                BindConsigneeBranchList(data.ConsigneeId);
                
            },
            error: function (Result) {
                ShowModel("Alert", "Problem in Request");
            }
        });

    }
    function GetPOProductList(poProducts, poId) {
        var hdnPOId = $("#hdnPOId");
        var requestData = { poProducts: poProducts, poId: poId };
        $.ajax({
            url: "../PurchaseInvoiceImport/GetPOProductList",
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
               // ReCalculateNetValues();
                ShowHideProductPanel(2);
            }
        });
    }
