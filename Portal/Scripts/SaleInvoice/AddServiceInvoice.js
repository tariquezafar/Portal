        $(document).ready(function () {


    $("#tabs").tabs({
        collapsible: true
    });
            BindCompanyBranchList();
            BindPaymentMode();
    $(".expand").on("click", function () {
        // $(this).next().slideToggle(200);
        $expand = $(this).find(">:first-child");

        if ($expand.text() == "+") {
            $expand.text("-");
        } else {
            $expand.text("+");
        }
    });
    $(".showdata").hide();
    $("#ddlBranchType").attr('disabled', true);
    $("#txtSINo").attr('readOnly', true);
    $("#txtSIDate").attr('readOnly', true);
    $("#txtSONo").attr('readOnly', true);
    $("#txtSODate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);

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
    $("#txtBranchStock").attr('readOnly', true);

    $("#txtTotalPrice").attr('readOnly', true);
    $("#txtBasicValue").attr('readOnly', true);

    $("#txtTaxPercentage").attr('readOnly', true);
    $("#txtTaxAmount").attr('readOnly', true);
    $("#txtTotalValue").attr('readOnly', true);
    $("#txtGrossValue").attr('readOnly', true);
    $("#txtRoundOfValue").attr('readOnly', true);
    $("#txtBiltyDate").attr('readOnly', true);

    $("#txtRTORegCGST_Amt").attr('readOnly', true);
    $("#txtRTORegSGST_Amt").attr('readOnly', true);
    $("#txtRTORegIGST_Amt").attr('readOnly', true);
    $("#txtChasisProductName").attr('readOnly', true);

    $("#txtLabourDiscountAmount").attr('readOnly', true);
    $("#txtCGSTPercAmountLabour").attr('readOnly', true);
    $("#txtSGSTPercAmountLabour").attr('readOnly', true);
    $("#txtIGSTPercAmountLabour").attr('readOnly', true);
    $("#txtTotalPriceLabour").attr('readOnly', true);

    $("#txtFromDateJobCard").attr('readOnly', true);
    $("#txtToDateJobCard").attr('readOnly', true);


    $("#txtFromDateJobCard,#txtToDateJobCard").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $("#txtChasisSerial").autocomplete({



        minLength: 4,
        delay: 0,

        source: function (request, response) {
            var ddlCompanyBranch = $("#ddlCompanyBranch");

            if (ddlCompanyBranch.val() == "0" || ddlCompanyBranch.val() == 0 || ddlCompanyBranch.val() == "") {
                ShowModel("Alert", "Please select Company Branch !.");
                return false;

            }


            $.ajax({
                url: "../ChasisSerialMapping/GetChasisSerialAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, companyBranchId: $("#ddlCompanyBranch").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ChasisSerialNo, value: item.ProductId, motorNo: item.MotorNo, controllerNo: item.ControllerNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtChasisSerial").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtChasisSerial").val(ui.item.label);
            $("#hdnProductId").val(ui.item.value);
            GetProductChasisSerialDetail(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null && $("#ddlCompanyBranch").val() != "0" && $("#ddlCompanyBranch").val() != "") {
                $("#txtChasisSerial").val("");
                $("#hdnProductId").val(0);
                ShowModel("Alert", "Please select Product Chasis Serial from List")

            }
            return false;
        }

    })
 .autocomplete("instance")._renderItem = function (ul, item) {
     return $("<li>")
       .append("<div><b>" + item.label + "</div>")
       .appendTo(ul);
 };

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
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode, CGST_Perc: item.CGST_Perc, SGST_Perc: item.SGST_Perc, IGST_Perc: item.IGST_Perc, HSN_Code: item.HSN_Code, IsSerializedProduct: item.IsSerializedProduct };
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
            $("#hdnIsSerializedProduct").val(ui.item.IsSerializedProduct);
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

    $("#txtLabourCode").autocomplete({
        minLength: 0,
        delay: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Product/GetProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.ProductName, value: item.Productid,
                            desc: item.ProductShortDesc, code: item.ProductCode,
                            CGST_Perc: item.CGST_Perc, SGST_Perc: item.SGST_Perc,
                            IGST_Perc: item.IGST_Perc,
                            SalePrice: item.SalePrice
                           
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtLabourCode").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtLabourCode").val(ui.item.label);
            $("#hdnLabourID").val(ui.item.value);
            $("#txtDescription").val(ui.item.desc);           
            $("#txtCGSTPercLabour").val(ui.item.CGST_Perc);
            $("#txtSGSTPercLabour").val(ui.item.SGST_Perc);
            $("#txtLabourRate").val(ui.item.SalePrice);
            $("#txtIGSTPerc").val(0);                              
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
                $("#txtLabourRate").val(0);
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

    $("#txtLabourProductName").autocomplete({
        minLength: 2,
        delay:0,
        source: function (request, response) {
            $.ajax({
                url: "../Product/GetProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.ProductName, value: item.Productid
                           

                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtLabourProductName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtLabourProductName").val(ui.item.label);
            $("#hdnLabourProductID").val(ui.item.value);
            
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtLabourProductName").val("");
                $("#hdnLabourProductID").val("0");
               
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
                data: { term: request.term, bookType: "B", companyBranchId: 0 },
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


    //Bind Sales Employee 


    $("#txtSalesEmployeeName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            ddlCompanyBranch = $("#ddlCompanyBranch");
            if (ddlCompanyBranch.val() == "0" || ddlCompanyBranch.val() == 0 || ddlCompanyBranch.val() == "") {
                ShowModel("Alert", "Please select Company Branch !.");
                return false;

            }

            $.ajax({
                url: "../SaleInvoice/GetEmployeeDepartmentWiseAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, companybrachId: $("#ddlCompanyBranch").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FirstName, value: item.EmployeeId, primaryAddress: item.PAddress, code: item.EmployeeCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtSalesEmployeeName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtSalesEmployeeName").val(ui.item.label);
            $("#hdnSaleEmployeeId").val(ui.item.value);


            return false;
        },
        change: function (event, ui) {
            if (ui.item == null && $("#ddlCompanyBranch").val() != "0" && $("#ddlCompanyBranch").val() != "") {
                $("#txtSalesEmployeeName").val("");
                $("#hdnSaleEmployeeId").val("0");

                ShowModel("Alert", "Please select Sales Employee Name from List");

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.primaryAddress + "</div>")
      .appendTo(ul);
};

    //End Sale Employee Name



    $("#txtSIDate,#txtRefDate,#txtBiltyDate").datepicker({
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


    BindCurrencyList();
    BindTermTemplateList();
    BindDocumentTypeList();
    BindCompanyBranchTypeList();
    BindPackingListType();


    var hdnAccessMode = $("#hdnAccessMode");
    var hdnSIId = $("#hdnSIId");
    if (hdnSIId.val() != "" && hdnSIId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetSaleInvoiceDetail(hdnSIId.val());
           $("#ddlCompanyBranch").attr('disabled', true);
       }, 3000);

        var customerId = $("#hdnCustomerId").val();
        BindCustomerBranchList(customerId);



        if (hdnAccessMode.val() == "3") {
            //$("#btnSave").hide();
            $(".btnAddEdit").hide();
            $(".btnReset").hide();
            $(".btnUpdate").hide();
            $(".btnReset").hide();
            //$("#btnUpdate").hide();
            //$("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $("#ddlPrintOption").attr('disabled', false);
            $(".editonly").hide();
            $("#txtSIDate").attr('readOnly', true);
            if ($(".editonly").hide()) {
                $('#lblSODate,#lblPartyCode,#lblConsigneeCode').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
            }
            $("#btnReCalculate").hide();
            $("#chkReverseChargeApplicable").attr('disabled', true);
            $("#chkSamePermanentAddress").attr('disabled', true);

        }
        else {
            //$("#btnSave").hide();
            //$("#btnUpdate").show();
            //$("#btnReset").show();
            $(".btnAddEdit").hide();
            $(".btnUpdate").show();
            $(".btnReset").show();
        }
    }
    else {
        $("#btnSave").show();
        //$("#btnUpdate").hide();
        $(".btnUpdate").hide();
        $("#btnReset").show();

        if ($("#hdnCustomerId").val() != "0") {
            GetCustomerDetail($("#hdnCustomerId").val());

        }
    }

    var saleinvoiceProducts = [];
    GetSaleInvoiceProductList(saleinvoiceProducts);
    var saleinvoiceTaxes = [];
    GetSaleInvoiceTaxList(saleinvoiceTaxes);
    var saleinvoiceTerms = [];
    GetSaleInvoiceTermList(saleinvoiceTerms);
    var saleProductSerials = [];
    GetSaleProductSerialList(saleProductSerials);


    var siDocuments = [];
    GetSIDocumentList(siDocuments);

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
                if (item.CurrencyCode == "INR") {
                    $("#ddlCurrency").append($("<option selected></option>").val(item.CurrencyCode).html(item.CurrencyName));
                }
                else {
                    $("#ddlCurrency").append($("<option></option>").val(item.CurrencyCode).html(item.CurrencyName));
                }

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
            //$("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
                $(":input#ddlCompanyBranch").trigger('change');
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}

function BindPaymentMode() {
    $("#ddlSaleType").val("");
    $("#ddlSaleType").html("");
    $.ajax({
        type: "GET",
        url: "../PaymentMode/GetPaymentMode",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            //$("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlSaleType").append($("<option></option>").val(item.PaymentModeName).html(item.PaymentModeName));
                
            });
            
        },
        error: function (Result) {
            $("#ddlSaleType").append($("<option></option>").val(0).html("-Select Payment Mode-"));
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
    BindCompanyBranchTypeListOnChange();
}

function BindCustomerBranchList(customerId) {
    $("#ddlBCustomerBranch").val(0);
    $("#ddlBCustomerBranch").html("");
    customerId = customerId == null ? 0 : customerId;
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
//function BindTermTemplateList() {
//    $.ajax({
//        type: "GET",
//        url: "../SaleInvoice/GetTermTemplateList",
//        data: "{}",
//        dataType: "json",
//        asnc: false,
//        success: function (data) {
//            $("#ddlTermTemplate").append($("<option></option>").val(0).html("-Select Terms Template-"));
//            $.each(data, function (i, item) {
//                if (item.termTemplateId == 1011)
//                {
//                    $("#ddlTermTemplate option:selected").append($("<option></option>").val(item.TermTemplateId).html(item.TermTempalteName));
//                    BindTermsDescriptions();
//                }
//                else
//                {
//                    $("#ddlTermTemplate option:selected").append($("<option></option>").val(item.TermTemplateId).html(item.TermTempalteName));
//                    BindTermsDescriptions();
//                }
//                });
//        },
//        error: function (Result) {
//            $("#ddlTermTemplate").append($("<option></option>").val(0).html("-Select Terms Template-"));
//        }
//    });
//}

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
function GetSIDocumentList(siDocuments) {
    var hdnSIId = $("#hdnSIId");
    var requestData = { siDocuments: siDocuments, siId: hdnSIId.val() };
    $.ajax({
        url: "../SaleInvoice/GetSISupportingDocumentList",
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
            
            var totalsum = 0;
            $('#tblDocumentList tr:not(:has(th))').each(function (i, row) {
                var $row = $(row);          
                var totalAmount = $row.find("#hdnTotalAmount").val();

                if(parseFloat (totalAmount)>0)
                {
                    totalsum += totalAmount;
                }

            });
            var grossValue = $("#txtGrossValue").val();
            $("#txtGrossValue").val(parseFloat(totalsum) + parseFloat(grossValue));

            var txtTotalValue= $("#txtTotalValue").val();
            $("#txtTotalValue").val(parseFloat(totalsum) + parseFloat(txtTotalValue));
            //if ($("#hdnConsigneeId").val() != "0")
            //{
            //    ReCalculateNetValues();
            //}
            
           
      
            ShowHideDocumentPanel(2);
        }
    });
}
function SaveDocument() {


    var docEntrySequence = 0;
    var hdnDocumentSequence = $("#hdnDocumentSequence");
    var ddlDocumentType = $("#ddlDocumentType");
    var hdnSaleInvoiceDocId = $("#hdnSaleInvoiceDocId");
    var FileUpload1 = $("#FileUpload2");



    var txtLabourCode = $("#txtLabourCode");
    var txtDescription = $("#txtDescription");
    var txtLabourtDesc = $("#txtLabourtDesc");
    var txtLabourRate = $("#txtLabourRate");
    var txtLabourDiscountPerc = $("#txtLabourDiscountPerc");
    var txtLabourDiscountAmount = $("#txtLabourDiscountAmount");
    var txtCGSTPercLabour = $("#txtCGSTPercLabour");
    var txtCGSTPercAmountLabour = $("#txtCGSTPercAmountLabour");
    var txtSGSTPercLabour = $("#txtSGSTPercLabour");
    var txtSGSTPercAmountLabour = $("#txtSGSTPercAmountLabour");
    var txtIGSTPercLabour = $("#txtIGSTPercLabour");
    var txtIGSTPercAmountLabour = $("#txtIGSTPercAmountLabour");
    var txtTotalPriceLabour = $("#txtTotalPriceLabour");
    var txtLabourProductName = $("#txtLabourProductName");
    var hdnLabourProductID = $("#hdnLabourProductID");

    if (txtLabourRate.val() == "" || txtLabourRate.val() == "0") {
        ShowModel("Alert", "Please Enter Labour Rate.")
        txtLabourRate.focus();
        return false;
    }





    var siDocuments = [];
    if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
        docEntrySequence = 1;
    }
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var hdnSaleInvoiceDocId = $row.find("#hdnSaleInvoiceDocId").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();


        var labourCode = $row.find("#hdnLabourCode").val();
        var description = $row.find("#hdnDescription").val();
        var cGST_Perc = $row.find("#hdnCGST_Perc").val();
        var cGST_Amount = $row.find("#hdnCGST_Amount").val();
        var sGST_Perc = $row.find("#hdnSGST_Perc").val();
        var sGST_Amount = $row.find("#hdnSGST_Amount").val();
        var labourRate = $row.find("#hdnLabourRate").val();

        var iGST_Perc = $row.find("#hdnIGST_Perc").val();
        var iGST_Amount = $row.find("#hdnIGST_Amount").val();
        var totalAmount = $row.find("#hdnTotalAmount").val();
        var discountPerc = $row.find("#hdnDiscountPerc").val();
        var discountAmount = $row.find("#hdnDiscountAmount").val();
        var labourProductID = $row.find("#hdnLabourProductID").val();
        var labourProductName = $row.find("#hdnLabourProductName").val();


        if (hdnSaleInvoiceDocId != undefined) {
            if ((hdnDocumentSequence.val() != documentSequenceNo)) {

                var siDocument = {
                    SaleInvoiceDocId: hdnSaleInvoiceDocId,
                    DocumentSequenceNo: documentSequenceNo,
                    DocumentTypeId: documentTypeId,
                    DocumentTypeDesc: documentTypeDesc,
                    DocumentName: "dffd",
                    DocumentPath: "dffd",
                    LabourRate: labourRate,
                    LabourCode: labourCode,
                    Description: description,
                    CGST_Perc: cGST_Perc,
                    CGST_Amount: cGST_Amount,
                    SGST_Perc: sGST_Perc,
                    SGST_Amount: sGST_Amount,
                    IGST_Perc: iGST_Perc,
                    IGST_Amount: iGST_Amount,
                    TotalAmount: totalAmount,
                    DiscountPerc: discountPerc,
                    DiscountAmount: discountAmount,
                    ProductId: labourProductID,
                    ProductName: labourProductName,

                };
                siDocuments.push(siDocument);
                docEntrySequence = parseInt(docEntrySequence) + 1;
            }
            else if (hdnSaleInvoiceDocId.val() == saleInvoiceDocId && hdnDocumentSequence.val() == documentSequenceNo) {
                var siDocument = {
                    SaleInvoiceDocId: hdnSaleInvoiceDocId.val(),
                    DocumentSequenceNo: hdnDocumentSequence.val(),
                    DocumentTypeId: ddlDocumentType.val(),
                    DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                    DocumentName: "dffd",
                    DocumentPath: "dffd",
                    LabourCode: txtLabourCode.val(),
                    Description: txtDescription.val(),
                    LabourRate: txtLabourRate.val(),
                    CGST_Perc: txtCGSTPercLabour.val(),
                    CGST_Amount: txtCGSTPercAmountLabour.val(),
                    SGST_Perc: txtSGSTPercLabour.val(),
                    SGST_Amount: txtSGSTPercAmountLabour.val(),
                    IGST_Perc: txtIGSTPercLabour.val(),
                    IGST_Amount: txtIGSTPercAmountLabour.val(),
                    TotalAmount: txtTotalPriceLabour.val(),
                    DiscountPerc: txtLabourDiscountPerc.val(),
                    DiscountAmount: txtLabourDiscountAmount.val(),
                    ProductId: hdnLabourProductID.val(),
                    ProductName: txtLabourProductName.val(),
                };
                siDocuments.push(siDocument);
            }
        }
    });
    if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
        hdnDocumentSequence.val(docEntrySequence);
    }

    var siDocumentAddEdit = {
        SaleInvoiceDocId: hdnSaleInvoiceDocId.val(),
        DocumentSequenceNo: hdnDocumentSequence.val(),
        DocumentTypeId: ddlDocumentType.val(),
        DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
        DocumentName: "dffd",
        DocumentPath: "dffd",
        LabourCode: txtLabourCode.val(),
        Description: txtDescription.val(),
        LabourRate: txtLabourRate.val(),
        CGST_Perc: txtCGSTPercLabour.val(),
        CGST_Amount: txtCGSTPercAmountLabour.val(),
        SGST_Perc: txtSGSTPercLabour.val(),
        SGST_Amount: txtSGSTPercAmountLabour.val(),
        IGST_Perc: txtIGSTPercLabour.val(),
        IGST_Amount: txtIGSTPercAmountLabour.val(),
        TotalAmount: txtTotalPriceLabour.val(),
        DiscountPerc: txtLabourDiscountPerc.val(),
        DiscountAmount: txtLabourDiscountAmount.val(),
        ProductId: hdnLabourProductID.val(),
        ProductName: txtLabourProductName.val(),
    };
    siDocuments.push(siDocumentAddEdit);
    hdnDocumentSequence.val("0");

    GetSIDocumentList(siDocuments);



}

function ShowHideDocumentPanel(action) {
    if (action == 1) {
        $(".documentsection").show();
    }
    else {
        $(".documentsection").hide();
        $("#ddlDocumentType").val("0");
        $("#hdnSaleInvoiceDocId").val("0");
        $("#FileUpload1").val("");


        $("#txtLabourCode").val("");
        $("#txtDescription").val("");
        $("#txtLabourtDesc").val("");
        $("#txtLabourRate").val("");
        $("#txtDescription").val("");
        $("#txtLabourDiscountPerc").val("");
        $("#txtLabourDiscountAmount").val("");
        $("#txtCGSTPercLabour").val("");
        $("#txtCGSTPercAmountLabour").val("");
        $("#txtSGSTPercLabour").val("");
        $("#txtSGSTPercAmountLabour").val("");
        $("#txtIGSTPercLabour").val("");
        $("#txtIGSTPercAmountLabour").val("");
        $("#txtTotalPriceLabour").val("");

        $("#txtLabourProductName").val("");
        $("#hdnLabourProductID").val("");

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
        var hdnDeliveryChallanDocId = $(row).find("#hdnDeliveryChallanDocId").val();
        ShowModel("Alert", "Document Removed from List.");
        row.remove();
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
    $('#tblTermList tr').each(function (i, row) {
        var $row = $(row);
        var invoiceTermDetailId = $row.find("#hdnSITermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (termDesc != undefined) {
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
    var taxEntrySequence = 0;
    var hdnTaxSequenceNo = $("#hdnTaxSequenceNo");
    var txtBasicValue = $("#txtBasicValue");
    var txtTaxName = $("#txtTaxName");
    var hdnSITaxDetailId = $("#hdnSITaxDetailId");
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

    var saleinvoiceTaxList = [];
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        taxEntrySequence = 1;
    }

    $('#tblTaxList tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var invoiceTaxDetailId = $row.find("#hdnSITaxDetailId").val();
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
                    return false;
                }

                var saleinvoiceTax = {
                    InvoiceTaxDetailId: invoiceTaxDetailId,
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
                saleinvoiceTaxList.push(saleinvoiceTax);
                taxEntrySequence = parseInt(taxEntrySequence) + 1;
            }
            else if (hdnSITaxDetailId.val() == invoiceTaxDetailId && hdnTaxSequenceNo.val() == taxSequenceNo) {
                var saleinvoiceTax = {
                    SITaxDetailId: hdnSITaxDetailId.val(),
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
                saleinvoiceTaxList.push(saleinvoiceTax);
                hdnTaxSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        hdnTaxSequenceNo.val(taxEntrySequence);
    }
    if (action == 1) {
        var saleinvoiceTaxAddEdit = {
            SITaxDetailId: hdnSITaxDetailId.val(),
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
        saleinvoiceTaxList.push(saleinvoiceTaxAddEdit);
        hdnTaxSequenceNo.val("0");
    }
    GetSaleInvoiceTaxList(saleinvoiceTaxList);

}
function EditTaxRow(obj) {
    var row = $(obj).closest("tr");
    var saleinvoiceTaxDetailId = $(row).find("#hdnSITaxDetailId").val();
    var taxSequenceNo = $(row).find("#hdnTaxSequenceNo").val();
    var taxId = $(row).find("#hdnTaxId").val();
    var taxName = $(row).find("#hdnTaxName").val();
    var taxPercentage = $(row).find("#hdnTaxPercentage").val();
    var taxAmount = $(row).find("#hdnTaxAmount").val();

    $("#txtTaxName").val(taxName);
    $("#hdnSITaxDetailId").val(saleinvoiceTaxDetailId);
    $("#hdnTaxSequenceNo").val(taxSequenceNo);
    $("#hdnTaxId").val(taxId);
    $("#txtTaxPercentage").val(taxPercentage);
    $("#txtTaxAmount").val(taxAmount);

    var surchargeName_1 = $(row).find("#hdnSurchargeName_1").val();
    var surchargePercentage_1 = $(row).find("#hdnSurchargePercentage_1").val();
    var surchargeAmount_1 = $(row).find("#hdnSurchargeAmount_1").val();

    var surchargeName_2 = $(row).find("#hdnSurchargeName_2").val();
    var surchargePercentage_2 = $(row).find("#hdnSurchargePercentage_2").val();
    var surchargeAmount_2 = $(row).find("#hdnSurchargeAmount_2").val();

    var surchargeName_3 = $(row).find("#hdnSurchargeName_3").val();
    var surchargePercentage_3 = $(row).find("#hdnSurchargePercentage_3").val();
    var surchargeAmount_3 = $(row).find("#hdnSurchargeAmount_3").val();

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
        var saleinvoiceTaxDetailId = $(row).find("#hdnSITaxDetailId").val();
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
    var hdnSIProductDetailId = $("#hdnSIProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtPrice = $("#txtPrice");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var txtAvailableStock = $("#txtAvailableStock");
    var txtBranchStock = $("#txtBranchStock");
    var txtDiscountPerc = $("#txtDiscountPerc");
    var txtDiscountAmount = $("#txtDiscountAmount");
    var txtProductTaxName = $("#txtProductTaxName");
    var hdnProductTaxId = $("#hdnProductTaxId");
    var hdnProductTaxPerc = $("#hdnProductTaxPerc");
    var txtProductTaxAmount = $("#txtProductTaxAmount");
    var txtTotalPrice = $("#txtTotalPrice");
    var hdnIsSerializedProduct = $("#hdnIsSerializedProduct");

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
    //if (parseFloat(txtQuantity.val().trim()) > parseFloat(txtBranchStock.val().trim())) {
    //    ShowModel("Alert", "Invoice Quantity cannot be greater than Branch Stock Quantity")
    //    txtQuantity.focus();
    //    return false;
    //}

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

    var saleinvoiceProductList = [];
    $('#tblProductList tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var invoiceProductDetailId = $row.find("#hdnSIProductDetailId").val();
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
        var isSerializedProduct = $row.find("#hdnIsSerializedProduct").val();

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
                    if (confirm("You have already Added This Product with Different Price. Do you want to add it?")) {
                        var saleinvoiceProduct = {
                            InvoiceProductDetailId: invoiceProductDetailId,
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
                            HSN_Code: hsn_Code,
                            IsSerializedProduct: isSerializedProduct

                        };
                    }
                    else {
                        flag = false;
                        return false;
                    }

                }


                var saleinvoiceProduct = {
                    InvoiceProductDetailId: invoiceProductDetailId,
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
                    HSN_Code: hsn_Code,
                    IsSerializedProduct: isSerializedProduct
                };



                saleinvoiceProductList.push(saleinvoiceProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnProductId.val() == productId && hdnSequenceNo.val() == sequenceNo) {
                var saleinvoiceProductAddEdit = {
                    SequenceNo: hdnSequenceNo.val(),
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
                    HSN_Code: txtHSN_Code.val(),
                    IsSerializedProduct: hdnIsSerializedProduct.val()
                };
                saleinvoiceProductList.push(saleinvoiceProductAddEdit);
                hdnSequenceNo.val("0");
            }
        }

    });


    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {

        var saleinvoiceProductAddEdit = {
            SequenceNo: hdnSequenceNo.val(),
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
            HSN_Code: txtHSN_Code.val(),
            IsSerializedProduct: hdnIsSerializedProduct.val()
        };
        saleinvoiceProductList.push(saleinvoiceProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetSaleInvoiceProductList(saleinvoiceProductList);
    }

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
        discountAmount = ((parseFloat(totalPrice) * parseFloat(discountPerc)) / 100).toFixed(2)
    }
    $("#txtDiscountAmount").val(discountAmount);





    if (parseFloat(CGSTPerc) > 0) {

        CGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(CGSTPerc)) / 100).toFixed(2);
    }
    $("#txtCGSTPercAmount").val(CGSTAmount);

    //var amt = getTwoDecimalPlace(CGSTAmount);






    if (parseFloat(SGSTPerc) > 0) {

        SGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(SGSTPerc)) / 100).toFixed(2);

    }
    $("#txtSGSTPercAmount").val(SGSTAmount);


    if (parseFloat(IGSTPerc) > 0) {

        IGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(IGSTPerc)) / 100).toFixed(2);
    }
    $("#txtIGSTPercAmount").val(IGSTAmount);


    if (parseFloat(productTaxPerc) > 0) {
        taxAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(productTaxPerc)) / 100).toFixed(2);
    }
    $("#txtProductTaxAmount").val(taxAmount.toFixed(2));

    if (parseFloat(taxAmount) > 0 && parseFloat(productSurchargePerc_1) > 0) {
        productSurchargeAmount_1 = ((parseFloat(taxAmount) * parseFloat(productSurchargePerc_1)) / 100).toFixed(2);
    }
    $("#txtProductSurchargeAmount_1").val(productSurchargeAmount_1.toFixed(2));

    if (parseFloat(taxAmount) > 0 && parseFloat(productSurchargePerc_2) > 0) {
        productSurchargeAmount_2 = ((parseFloat(taxAmount) * parseFloat(productSurchargePerc_2)) / 100).toFixed(2);
    }
    $("#txtProductSurchargeAmount_2").val(productSurchargeAmount_2.toFixed(2));

    if (parseFloat(taxAmount) > 0 && parseFloat(productSurchargePerc_3) > 0) {
        productSurchargeAmount_3 = ((parseFloat(taxAmount) * parseFloat(productSurchargePerc_3)) / 100).toFixed(2);
    }
    $("#txtProductSurchargeAmount_3").val(productSurchargeAmount_3.toFixed(2));
    totalTaxAmount = parseFloat(taxAmount) + parseFloat(productSurchargeAmount_1) + parseFloat(productSurchargeAmount_2) + parseFloat(productSurchargeAmount_3);
    $("#txtProductTotalTaxAmount").val(totalTaxAmount.toFixed(2));

    $("#txtTotalPrice").val((parseFloat(totalPrice) - parseFloat(discountAmount) +
        parseFloat(taxAmount) + parseFloat(CGSTAmount) +
        parseFloat(SGSTAmount) + parseFloat(IGSTAmount) +
        parseFloat(productSurchargeAmount_1) +
       parseFloat(productSurchargeAmount_2) +
        parseFloat(productSurchargeAmount_3)));

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


    var rTORegValue = $("#txtRTOReg").val() == "" ? "0" : $("#txtRTOReg").val();
    var rTORegCGST_Amt = $("#txtRTORegCGST_Amt").val() == "" ? "0" : $("#txtRTORegCGST_Amt").val();
    var rTORegSGST_Amt = $("#txtRTORegSGST_Amt").val() == "" ? "0" : $("#txtRTORegSGST_Amt").val();
    var rTORegIGST_Amt = $("#txtRTORegIGST_Amt").val() == "" ? "0" : $("#txtRTORegIGST_Amt").val();

    var vehicleInsurance = $("#txtVehicleInsurance").val() == "" ? "0" : $("#txtVehicleInsurance").val();


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

    if (parseFloat(rTORegValue) <= 0) {
        rTORegValue = 0;
    }
    if (parseFloat(rTORegCGST_Amt) <= 0) {
        rTORegCGST_Amt = 0;
    }
    if (parseFloat(rTORegSGST_Amt) <= 0) {
        rTORegSGST_Amt = 0;
    }
    if (parseFloat(rTORegIGST_Amt) <= 0) {
        rTORegIGST_Amt = 0;
    }
    if (parseFloat(vehicleInsurance) <= 0) {
        vehicleInsurance = 0;
    }


    $("#txtBasicValue").val(parseFloat(basicValue).toFixed(2));

    $("#txtGrossValue").val(parseFloat(parseFloat(basicValue)
        + parseFloat(taxValue)
        + parseFloat(freightValue)
        + parseFloat(freightCGST_Amt)
        + parseFloat(freightSGST_Amt)
        + parseFloat(freightIGST_Amt)
        + parseFloat(loadingValue)
        + parseFloat(loadingCGST_Amt)
        + parseFloat(loadingSGST_Amt)
        + parseFloat(loadingIGST_Amt)
        + parseFloat(insuranceValue)
        + parseFloat(insuranceCGST_Amt)
        + parseFloat(insuranceSGST_Amt)
        + parseFloat(insuranceIGST_Amt)
        + parseFloat(rTORegValue)
        + parseFloat(rTORegCGST_Amt)
        + parseFloat(rTORegSGST_Amt)
        + parseFloat(rTORegIGST_Amt)
        + parseFloat(vehicleInsurance)).toFixed(2));


    var grossValue = $("#txtGrossValue").val();
    var str = grossValue.split('.');
    var roundVal = parseFloat("0." + str[1]);
    var round_Amt;
    if (roundVal == '0.00') {
        round_Amt = roundVal;
    }
    else if (roundVal < 0.50) {
        round_Amt = "-" + roundVal;
    }
    else {
        var roundAmt = parseInt(100 - str[1]);
        if (roundAmt > 0 && roundAmt < 10) {
            round_Amt = "0.0" + roundAmt;
        }
        else {
            round_Amt = "0." + roundAmt;
        }
    }
    $("#txtRoundOfValue").val(round_Amt);
    $("#txtTotalValue").val(Math.round(parseFloat(parseFloat(basicValue)
        + parseFloat(taxValue)
        + parseFloat(freightValue)
        + parseFloat(freightCGST_Amt)
        + parseFloat(freightSGST_Amt)
        + parseFloat(freightIGST_Amt)
        + parseFloat(loadingValue)
        + parseFloat(loadingCGST_Amt)
        + parseFloat(loadingSGST_Amt)
        + parseFloat(loadingIGST_Amt)
        + parseFloat(insuranceValue)
        + parseFloat(insuranceCGST_Amt)
        + parseFloat(insuranceSGST_Amt)
        + parseFloat(insuranceIGST_Amt)
        + parseFloat(rTORegValue)
        + parseFloat(rTORegCGST_Amt)
        + parseFloat(rTORegSGST_Amt)
        + parseFloat(rTORegIGST_Amt)
        + parseFloat(vehicleInsurance)
        ).toFixed(2)));
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

function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var siProductDetailId = $(row).find("#hdnSIProductDetailId").val();
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
    var isSerializedProduct = $(row).find("#hdnIsSerializedProduct").val();


    $("#hdnSequenceNo").val(sequenceNo);
    $("#txtProductName").val(productName);
    $("#hdnSIProductDetailId").val(siProductDetailId);
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
    $("#hdnIsSerializedProduct").val(isSerializedProduct);

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


    GetProductAvailableStock(productId);
    GetProductAvailableStockBranchWise(productId);
    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();

    ShowHideProductPanel(1);


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

            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#ddlCurrency").val(data.CurrencyCode);
            $("#ddlInvoiceType").val(data.InvoiceType);

            $("#hdnCustomerId").val(data.CustomerId);
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtCustomerName").val(data.CustomerName);

            $("#hdnConsigneeId").val(data.ConsigneeId);
            $("#txtConsigneeCode").val(data.ConsigneeCode);
            $("#txtConsigneeName").val(data.ConsigneeName);


            $("#txtSalesEmployeeName").val(data.SaleEmployeeName);
            $("#hdnSaleEmployeeId").val(data.SaleEmpId);


            $("#txtBContactPerson").val(data.ContactPerson);
            $("#txtBAddress").val(data.BillingAddress);
            $("#txtBCity").val(data.City);
            $("#ddlBCountry").val(data.CountryId);
            $("#ddlBState").val(data.StateId);
            $("#txtBPinCode").val(data.PinCode);
            $("#txtBTINNo").val(data.TINNo);
            $("#ddlSaleType").val(data.SaleType);
            $("#txtBEmail").val(data.Email);
            $("#txtBMobileNo").val(data.MobileNo);
            $("#txtBContactNo").val(data.ContactNo);
            $("#txtBFax").val(data.Fax);
            $("#txtBGSTNo").val(data.GSTNo);
            //$("#ddlApprovalStatus").val(data.ApprovalStatus);
            $("#btnPrintForm").show();
            $(".approvalStatus").val(data.ApprovalStatus);
            if (data.ApprovalStatus == "Final") {
                $("#btnUpdate").hide();

                $("#btnPrintForm").show();
                $(".editonly").hide();
                $("#btnReset").hide();
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);
                if ($(".editonly").hide()) {
                    $('#lblSODate,#lblPartyCode,#lblConsigneeCode').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
                }
                $("#btnReCalculate").hide();
                $("#chkReverseChargeApplicable").attr('disabled', true);
                $("#chkSamePermanentAddress").attr('disabled', true);
                $("#ddlPrintOption").attr('disabled', false);
            }
            $("#txtSContactPerson").val(data.ShippingContactPerson);
            $("#txtSAddress").val(data.ShippingBillingAddress);
            $("#txtSCity").val(data.ShippingCity);
            $("#ddlSCountry").val(data.ShippingCountryId);
            $("#ddlSState").val(data.ShippingStateId);
            $("#txtSPinCode").val(data.ShippingPinCode);
            $("#txtSTINNo").val(data.ShippingTINNo);
            $("#txtSGSTNo").val(data.ShippingTINNo);

            $("#txtSEmail").val(data.ShippingEmail);
            $("#txtSMobileNo").val(data.ShippingMobileNo);
            $("#txtSContactNo").val(data.ShippingContactNo);
            $("#txtSFax").val(data.ShippingFax);

            $("#txtRefNo").val(data.RefNo);
            $("#txtRefDate").val(data.RefDate);

            $("#txtPayToBookName").val(data.PayToBookName);
            $("#hdnPayToBookId").val(data.PayToBookId);
            $("#txtPayToBookBranch").val(data.PayToBookBranch);

            $("#txtRemarks1").val(data.Remarks);

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

            $("#txttransportname").val(data.TransportName);
            $("#txtVehicleNo").val(data.VehicleNo);
            $("#txtBiltyNo").val(data.BiltyNo);
            $("#txtBiltyDate").val(data.BiltyDate);

            $("#txtRTOReg").val(data.RtoRegsValue);
            $("#txtRTORegCGST_Amt").val(data.RtoRegsCGST_Amt);
            $("#hdnRTORegCGST_Perc").val(data.RtoRegsCGST_Perc);
            $("#txtRTORegSGST_Amt").val(data.RtoRegsSGST_Amt);
            $("#hdnRTORegSGST_Perc").val(data.RtoRegsSGST_Perc);
            $("#txtRTORegIGST_Amt").val(data.RtoRegsIGST_Amt);
            $("#hdnRTORegIGST_Perc").val(data.RtoRegsIGST_Perc);
            $("#txtVehicleInsurance").val(data.VehicleInsuranceValue);


            $("#txtIdTypeName").val(data.IdtypeName);
            $("#txtIDTypeValue").val(data.IdtypeValue);
            $("#txtAdharcardNo").val(data.AdharcardNo);
            $("#txtPancard").val(data.Pancard);
            $("#txtHypothecationBy").val(data.HypothecationBy);
            $("#txtEwayBillNo").val(data.EwayBillNo);


            $("#txtRTOReg").val(data.RtoRegsValue);
            $("#txtRTORegCGST_Amt").val(data.RtoRegsCGST_Amt);
            $("#hdnRTORegCGST_Perc").val(data.RtoRegsCGST_Perc);
            $("#txtRTORegSGST_Amt").val(data.RtoRegsSGST_Amt);
            $("#hdnRTORegSGST_Perc").val(data.RtoRegsSGST_Perc);
            $("#txtRTORegIGST_Amt").val(data.RtoRegsIGST_Amt);
            $("#hdnRTORegIGST_Perc").val(data.RtoRegsIGST_Perc);
            $("#txtVehicleInsurance").val(data.VehicleInsuranceValue);

            if (data.BranchType == "Showroom") {
                $(".showdata").show();
            }
            else {
                $(".showdata").hide();
            }






            if (data.ReverseChargeApplicable == true) {
                $("#chkReverseChargeApplicable").prop("checked", true);
                changeReverseChargeStatus();
            }
            $("#txtReverseChargeAmount").val(data.ReverseChargeAmount);

            GetBranchStateId();
            //CalculateLoadingTotalCharges();
            //CalculateFreightTotalCharges();
            //CalculateInsuranceTotalCharges();
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
    var txtSINo = $("#txtSINo");
    var hdnSIId = $("#hdnSIId");
    var txtSIDate = $("#txtSIDate");
    var hdnSOId = $("#hdnSOId");
    var txtSONo = $("#txtSONo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlCurrency = $("#ddlCurrency");
    var hdnCustomerId = $("#hdnCustomerId");
    var txtCustomerName = $("#txtCustomerName");
    var hdnConsigneeId = $("#hdnConsigneeId");
    var txtConsigneeName = $("#txtConsigneeName");
    var ddlInvoiceType = $("#ddlInvoiceType");
    var txtBContactPerson = $("#txtBContactPerson");
    var txtBAddress = $("#txtBAddress");
    var txtBCity = $("#txtBCity");
    var ddlBCountry = $("#ddlBCountry");
    var ddlBState = $("#ddlBState");
    var txtBPinCode = $("#txtBPinCode");
    var txtBTINNo = $("#txtBTINNo");
    var txtBEmail = $("#txtBEmail");
    var txtBMobileNo = $("#txtBMobileNo");
    var txtBContactNo = $("#txtBContactNo");
    var txtBFax = $("#txtBFax");
    var txtBGSTNo = $("#txtBGSTNo");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtSContactPerson = $("#txtSContactPerson");
    var txtSAddress = $("#txtSAddress");
    var txtSCity = $("#txtSCity");
    var ddlSCountry = $("#ddlSCountry");
    var ddlSState = $("#ddlSState");
    var txtSPinCode = $("#txtSPinCode");

    var txtSTINNo = $("#txtSTINNo");
    var txtSGSTNo = $("#txtSGSTNo");
    var txtSEmail = $("#txtSEmail");
    var txtSMobileNo = $("#txtSMobileNo");
    var txtSContactNo = $("#txtSContactNo");
    var txtSFax = $("#txtSFax");


    var txtRefNo = $("#txtRefNo");
    var txtRefDate = $("#txtRefDate");
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
    var hdnPayToBookId = $("#hdnPayToBookId");

    var txtRemarks = $("#txtRemarks1");
    var ddlPackingListType = $("#ddlPackingListType");
    var txtRoundOfValue = $("#txtRoundOfValue");
    var txtGrossValue = $("#txtGrossValue");
    var ddlSaleType = $("#ddlSaleType");


    var txttransportname = $("#txttransportname");
    var txtVehicleNo = $("#txtVehicleNo");
    var txtBiltyNo = $("#txtBiltyNo");
    var txtBiltyDate = $("#txtBiltyDate");

    var txtRTOReg = $("#txtRTOReg");
    var txtRTORegCGST_Amt = $("#txtRTORegCGST_Amt");
    var hdnRTORegCGST_Perc = $("#hdnRTORegCGST_Perc");
    var txtRTORegSGST_Amt = $("#txtRTORegSGST_Amt");
    var hdnRTORegSGST_Perc = $("#hdnRTORegSGST_Perc");
    var txtRTORegIGST_Amt = $("#txtRTORegIGST_Amt");
    var hdnRTORegIGST_Perc = $("#hdnRTORegIGST_Perc");
    var txtVehicleInsurance = $("#txtVehicleInsurance");

    var txtIdTypeName = $("#txtIdTypeName");
    var txtIDTypeValue = $("#txtIDTypeValue");
    var txtAdharcardNo = $("#txtAdharcardNo");
    var txtPancard = $("#txtPancard");
    var txtHypothecationBy = $("#txtHypothecationBy");
    var txtEwayBillNo = $("#txtEwayBillNo");
    var hdnSaleEmployeeId = $("#hdnSaleEmployeeId");

    var IdTypeName1 = "";
    var IDTypeValue1 = "";
    var AdharcardNo1 = "";
    var Pancard1 = "";
    var HypothecationBy1 = "";

    if (txtCustomerName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Party Name")
        txtCustomerName.focus();
        return false;
    }
    if (ddlInvoiceType.val() == "" || ddlInvoiceType.val() == "0") {
        ShowModel("Alert", "Please select InvoiceType from list")
        return false;

    }
    if (hdnCustomerId.val() == "" || hdnCustomerId.val() == "0") {
        ShowModel("Alert", "Please select Party from list")
        txtCustomerName.focus();
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

    //if (txtBAddress.val().trim() == "") {
    //    ShowModel("Alert", "Please Enter Party billing Address")
    //    txtBAddress.focus();
    //    return false;
    //}

    //if (txtBCity.val() =="0" || txtBCity.val() ==null) {
    //    ShowModel("Alert", "Please enter billing city")
    //    txtBCity.focus();
    //    return false;
    //}
    //if (txtSAddress.val() == "" || txtSAddress.val()==null) {
    //    ShowModel("Alert", "Please Enter Party shipping Address")
    //    txtSAddress.focus();
    //    return false;
    //}
    //if (txtSCity.val().trim() == "") {
    //    ShowModel("Alert", "Please enter shipping city")
    //    txtSCity.focus();
    //    return false;
    //}

    if (txtHypothecationBy.val() != null || txtHypothecationBy.val() != '') {
        HypothecationBy1 = txtHypothecationBy.val();
    }

    var CompanyBranchType = $("#ddlBranchType option:selected").text();


    if (CompanyBranchType == 'Showroom') {

        var AdharcardNo = txtAdharcardNo.val();
        if (txtAdharcardNo.val() != '' && AdharcardNo.length != 12) {
            ShowModel("Alert", "Please enter 12 digit Adharcard No.")
            return false;

        }
        var Pancard = txtPancard.val();
        if (txtPancard.val() != '' && Pancard.length != 10) {
            ShowModel("Alert", "Please enter 10 digit Pancard No.")
            return false;
        }
        //return false;
    }

    //if (hdnPayToBookId.val() == "" || hdnPayToBookId.val() == "0") {
    //    ShowModel("Alert", "Please select Bank Name from list")
    //    $("#txtPayToBookName").focus();
    //    return false;
    //}

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
    if (txtLoadingCGST_Amt.val() == "" || parseFloat(txtLoadingCGST_Amt.val()) == 0 || txtLoadingCGST_Amt.val() == null) {
        loadingCGSTPerc = 0;
    }
    else {
        loadingCGSTPerc = hdnLoadingCGST_Perc.val();
    }

    var loadingSGSTPerc = 0;
    if (txtLoadingSGST_Amt.val() == "" || parseFloat(txtLoadingSGST_Amt.val()) == 0 || txtLoadingSGST_Amt.val() == null) {
        loadingSGSTPerc = 0;
    }
    else {
        loadingSGSTPerc = hdnLoadingSGST_Perc.val();
    }

    var loadingIGSTPerc = 0;
    if (txtLoadingIGST_Amt.val() == "" || parseFloat(txtLoadingIGST_Amt.val()) == 0 || txtLoadingIGST_Amt.val() == null) {
        loadingIGSTPerc = 0;
    }
    else {
        loadingIGSTPerc = hdnLoadingIGST_Perc.val();
    }

    var freightCGSTPerc = 0;
    if (txtFreightCGST_Amt.val() == "" || parseFloat(txtFreightCGST_Amt.val()) == 0 || txtFreightCGST_Amt.val() == null) {
        freightCGSTPerc = 0;
    }
    else {
        freightCGSTPerc = hdnFreightCGST_Perc.val();
    }

    var freightSGSTPerc = 0;
    if (txtFreightSGST_Amt.val() == "" || parseFloat(txtFreightSGST_Amt.val()) == 0 || txtFreightSGST_Amt.val() == null) {
        freightSGSTPerc = 0;
    }
    else {
        freightSGSTPerc = hdnFreightSGST_Perc.val();
    }

    var freightIGSTPerc = 0;
    if (txtFreightIGST_Amt.val() == "" || parseFloat(txtFreightIGST_Amt.val()) == 0 || txtFreightIGST_Amt.val() == null) {
        freightIGSTPerc = 0;
    }
    else {
        freightIGSTPerc = hdnFreightIGST_Perc.val();
    }

    var insuranceCGSTPerc = 0;
    if (txtInsuranceCGST_Amt.val() == "" || parseFloat(txtInsuranceCGST_Amt.val()) == 0 || txtInsuranceCGST_Amt.val() == null) {
        insuranceCGSTPerc = 0;
    }
    else {
        insuranceCGSTPerc = hdnInsuranceCGST_Perc.val();
    }

    var insuranceSGSTPerc = 0;
    if (txtInsuranceSGST_Amt.val() == "" || parseFloat(txtInsuranceSGST_Amt.val()) == 0 || txtInsuranceSGST_Amt.val() == null) {
        insuranceSGSTPerc = 0;
    }
    else {
        insuranceSGSTPerc = hdnInsuranceSGST_Perc.val();
    }

    var insuranceIGSTPerc = 0;
    if (txtInsuranceIGST_Amt.val() == "" || parseFloat(txtInsuranceIGST_Amt.val()) == 0 || txtInsuranceIGST_Amt.val() == null) {
        insuranceIGSTPerc = 0;
    }
    else {
        insuranceIGSTPerc = hdnInsuranceIGST_Perc.val();
    }

    var rTORegIGSTPerc = 0;
    if (txtRTORegIGST_Amt.val() == "" || parseFloat(txtRTORegIGST_Amt.val()) == 0 || txtRTORegIGST_Amt.val() == null) {
        rTORegIGSTPerc = 0;
    }
    else {
        rTORegIGSTPerc = hdnRTORegIGST_Perc.val();
    }

    var rTORegCGSTPerc = 0;
    if (txtRTORegCGST_Amt.val() == "" || parseFloat(txtRTORegCGST_Amt.val()) == 0 || txtRTORegCGST_Amt.val() == null) {
        rTORegCGSTPerc = 0;
    }
    else {
        rTORegCGSTPerc = hdnRTORegCGST_Perc.val();
    }

    var rTOSGSTPerc = 0;
    if (txtRTORegSGST_Amt.val() == "" || parseFloat(txtRTORegSGST_Amt.val()) == 0 || txtRTORegSGST_Amt.val() == null) {
        rTOSGSTPerc = 0;
    }
    else {
        rTOSGSTPerc = hdnRTORegSGST_Perc.val();
    }

    var rTORegVal = 0;
    if (txtRTOReg.val() == "" || parseFloat(txtRTOReg.val()) == 0 || txtRTOReg.val() == null) {
        rTORegVal = 0;
    }
    else {
        rTORegVal = txtRTOReg.val();
    }

    var rTORegCGST_Amt = 0;
    if (txtRTORegCGST_Amt.val() == "" || parseFloat(txtRTORegCGST_Amt.val()) == 0 || txtRTORegCGST_Amt.val() == null) {
        rTORegCGST_Amt = 0;
    }
    else {
        rTORegCGST_Amt = txtRTORegCGST_Amt.val();
    }

    var rTORegSGST_Amt = 0;
    if (txtRTORegSGST_Amt.val() == "" || parseFloat(txtRTORegSGST_Amt.val()) == 0 || txtRTORegSGST_Amt.val() == null) {
        rTORegSGST_Amt = 0;
    }
    else {
        rTORegSGST_Amt = txtRTORegSGST_Amt.val();
    }
    var rTORegIGST_Amt = 0;
    if (txtRTORegIGST_Amt.val() == "" || parseFloat(txtRTORegIGST_Amt.val()) == 0 || txtRTORegIGST_Amt.val() == null) {
        rTORegIGST_Amt = 0;
    }
    else {
        rTORegIGST_Amt = txtRTORegIGST_Amt.val();
    }

    var vehicleInsurance = 0;
    if (txtVehicleInsurance.val() == "" || parseFloat(txtVehicleInsurance.val()) == 0 || txtVehicleInsurance.val() == null) {
        vehicleInsurance = 0;
    }
    else {
        vehicleInsurance = txtVehicleInsurance.val();
    }






    //if (txttransportname.val().trim() == "") {
    //    ShowModel("Alert", "Please enter transport name")
    //    txttransportname.focus();
    //    return false;
    //}
    //if (txtVehicleNo.val().trim() == "") {
    //    ShowModel("Alert", "Please enter Vehicle No")
    //    txtVehicleNo.focus();
    //    return false;
    //}
    //if (txtBiltyNo.val().trim() == "") {
    //    ShowModel("Alert", "Please enter bilty no")
    //    txtBiltyNo.focus();
    //    return false;
    //}
    //if (txtBiltyDate.val().trim() == "") {
    //    ShowModel("Alert", "Please select bilty date ")
    //    txtBiltyDate.focus();
    //    return false;
    //}

    var saleinvoiceViewModel = {
        InvoiceId: hdnSIId.val(),
        InvoiceNo: txtSINo.val().trim(),
        InvoiceDate: txtSIDate.val().trim(),
        InvoiceType: ddlInvoiceType.val(),
        SOId: hdnSOId.val().trim(),
        SONo: txtSONo.val().trim(),
        CompanyBranchId: ddlCompanyBranch.val().trim(),
        CurrencyCode: ddlCurrency.val().trim(),
        CustomerId: hdnCustomerId.val().trim(),
        CustomerName: txtCustomerName.val().trim(),
        ConsigneeId: hdnConsigneeId.val().trim(),
        ConsigneeName: txtConsigneeName.val().trim(),
        ContactPerson: txtBContactPerson.val().trim(),
        BillingAddress: txtBAddress.val().trim(),
        City: txtBCity.val().trim(),
        StateId: ddlBState.val(),
        CountryId: ddlBCountry.val(),
        PinCode: txtBPinCode.val().trim(),
        TINNo: txtBTINNo.val().trim(),
        Email: txtBEmail.val().trim(),
        MobileNo: txtBMobileNo.val().trim(),
        ContactNo: txtBContactNo.val().trim(),
        Fax: txtBFax.val().trim(),
        GSTNo: txtBGSTNo.val().trim(),
        ApprovalStatus: ddlApprovalStatus.val(),
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
        RefNo: txtRefNo.val().trim(),
        RefDate: txtRefDate.val(),
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
        PayToBookId: hdnPayToBookId.val(),
        Remarks: txtRemarks.val(),
        RoundOfValue: txtRoundOfValue.val(),
        GrossValue: txtGrossValue.val(),
        SaleType: ddlSaleType.val(),
        TransportName: txttransportname.val(),
        VehicleNo: txtVehicleNo.val(),
        BiltyNo: txtBiltyNo.val(),
        BiltyDate: txtBiltyDate.val(),
        RtoRegsValue: rTORegVal,//txtRTOReg.val(),
        RtoRegsCGST_Amt: rTORegCGST_Amt,//txtRTORegCGST_Amt.val(),
        RtoRegsSGST_Amt: rTORegSGST_Amt,//txtRTORegSGST_Amt.val(),
        RtoRegsIGST_Amt: rTORegIGST_Amt,//txtRTORegIGST_Amt.val(),
        RtoRegsCGST_Perc: rTORegCGSTPerc,
        RtoRegsSGST_Perc: rTOSGSTPerc,
        RtoRegsIGST_Perc: rTORegIGSTPerc,
        VehicleInsuranceValue: vehicleInsurance,// txtVehicleInsurance.val(),
        IdtypeName: txtIdTypeName.val(),
        IdtypeValue: txtIDTypeValue.val(),
        Pancard: txtPancard.val(),
        AdharcardNo: txtAdharcardNo.val(),
        HypothecationBy: HypothecationBy1,
        EwayBillNo: txtEwayBillNo.val(),
        SaleEmpId: hdnSaleEmployeeId.val(),
        SaleInvoiceType: "Service"
    };

    var SIProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var invoiceProductDetailId = $row.find("#hdnSIProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = parseFloat($row.find("#hdnQuantity").val()).toFixed(2);
        var discountPerc = $row.find("#hdnDiscountPerc").val();
        var discountAmount = $row.find("#hdnDiscountAmount").val();
        var productTaxId = $row.find("#hdnProductTaxId").val();
        var productTaxPerc = $row.find("#hdnProductTaxPerc").val();
        var productTaxAmount = $row.find("#hdnProductTaxAmount").val();
        var productTaxName = $row.find("#hdnProductTaxName").val();

        var productSurchargeName_1 = $row.find("#hdnProductSurchargeName_1").val();
        var productSurchargePercentage_1 = $row.find("#hdnProductSurchargePercentage_1").val();
        var productSurchargeAmount_1 = $row.find("#hdnProductSurchargeAmount_1").val();

        var productSurchargeName_2 = $row.find("#hdnProductSurchargeName_2").val();
        var productSurchargePercentage_2 = $row.find("#hdnProductSurchargePercentage_2").val();
        var productSurchargeAmount_2 = $row.find("#hdnProductSurchargeAmount_2").val();

        var productSurchargeName_3 = $row.find("#hdnProductSurchargeName_3").val();
        var productSurchargePercentage_3 = $row.find("#hdnProductSurchargePercentage_3").val();
        var productSurchargeAmount_3 = $row.find("#hdnProductSurchargeAmount_3").val();

        var totalPrice = $row.find("#hdnTotalPrice").val();
        var cGSTPerc = $row.find("#hdnCGSTPerc").val();
        var cGSTPercAmount = $row.find("#hdnCGSTAmount").val();
        var sGSTPerc = $row.find("#hdnSGSTPerc").val();
        var sGSTPercAmount = $row.find("#hdnSGSTAmount").val();
        var iGSTPerc = $row.find("#hdnIGSTPerc").val();
        var iGSTPercAmount = $row.find("#hdnIGSTAmount").val();
        var hsn_Code = $row.find("#hdnHSN_Code").val();

        if (productId != undefined) {

            var saleinvoiceProduct = {
                ProductId: productId,
                ProductShortDesc: productShortDesc,
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
            SIProductList.push(saleinvoiceProduct);
        }
    });


    var saleinvoiceTaxList = [];
    $('#tblTaxList tr').each(function (i, row) {
        var $row = $(row);
        var invoiceTaxDetailId = $row.find("#hdnSITaxDetailId").val();
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
            var saleinvoiceTax = {
                InvoiceTaxDetailId: invoiceTaxDetailId,
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
            saleinvoiceTaxList.push(saleinvoiceTax);
        }

    });

    var saleinvoiceTermList = [];
    $('#tblTermList tr').each(function (i, row) {
        var $row = $(row);
        var invoiceTermDetailId = $row.find("#hdnSITermDetailId").val();
        var termDesc = $row.find("#hdnTermDesc").val();
        var termSequence = $row.find("#hdnTermSequence").val();

        if (termDesc != undefined) {
            var saleinvoiceTerm = {
                InvoiceTermDetailId: invoiceTermDetailId,
                TermDesc: termDesc,
                TermSequence: termSequence
            };
            saleinvoiceTermList.push(saleinvoiceTerm);
        }

    });


    var saleInvoiceProductSerialDetail = [];

    var flag = true;
    $('#tblProductSerialDetail tr').each(function (i, row) {
        var $row = $(row);
        //var invoiceTermDetailId = $row.find("#hdnSITermDetailId").val();
        var hdnProductId = $row.find("#hdnProductId").val();
        var refSerial1 = $row.find("#hdnRefSerial1").val();
        var refSerial2 = $row.find("#hdnRefSerial2").val();
        var refSerial3 = $row.find("#hdnRefSerial3").val();
        var refSerial4 = $row.find("#hdnRefSerial4").val();
        var hdnPackingListTypeId = $row.find("#hdnPackingListTypeId").val();

        if (hdnPackingListTypeId != undefined) {
            if (ddlApprovalStatus.val() == "Final" && hdnPackingListTypeId == 0) {
                flag = false;
            }

        }

        if (refSerial1 != undefined) {
            var saleInvoiceProductSerials = {
                ProductId: hdnProductId,
                RefSerial1: refSerial1,
                RefSerial2: refSerial2,
                RefSerial3: refSerial3,
                RefSerial4: refSerial4,
                PackingListTypeID: hdnPackingListTypeId

            };
            saleInvoiceProductSerialDetail.push(saleInvoiceProductSerials);
        }

    });

    if (flag == false) {
        ShowModel("Alert", "Please Select Packing List Type")
        return false;
    }



    //if (ddlApprovalStatus.val() == "Final" || ddlApprovalStatus.val() == "FINAL") {
    //    var confirmMessageValue = confirm("Sale Invoice has been finalized . Are you sure you want to save it .");
    //    if (confirmMessageValue == false) {
    //        return false;
    //    }
    //}

    var siDocumentList = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var saleInvoiceDocId = $row.find("#hdnSaleInvoiceDocId").val();
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        var labourCode = $row.find("#hdnLabourCode").val();
        var description = $row.find("#hdnDescription").val();
        var labourRate = $row.find("#hdnLabourRate").val();
        var discountPerc = $row.find("#hdnDiscountPerc").val();
        var discountAmount = $row.find("#hdnDiscountAmount").val();
        var cGST_Perc = $row.find("#hdnCGST_Perc").val();
        var cGST_Amount = $row.find("#hdnCGST_Amount").val();
        var sGST_Perc = $row.find("#hdnSGST_Perc").val();
        var sGST_Amount = $row.find("#hdnSGST_Amount").val();

        var iGST_Perc = $row.find("#hdnIGST_Perc").val();
        var iGST_Amount = $row.find("#hdnIGST_Amount").val();
        var totalAmount = $row.find("#hdnTotalAmount").val();
        var labourProductID = $row.find("#hdnLabourProductID").val();

        if (saleInvoiceDocId != undefined) {
            var siDocument = {
                SaleInvoiceDocId: saleInvoiceDocId,
                DocumentSequenceNo: documentSequenceNo,
                DocumentTypeId: documentTypeId,
                DocumentTypeDesc: documentTypeDesc,
                DocumentName: documentName,
                DocumentPath: documentPath,
                LabourCode:labourCode,
                Description:description,
                LabourRate:labourRate,
                DiscountPerc:discountPerc,
                DiscountAmount:discountAmount,
                CGST_Perc:cGST_Perc,
                CGST_Amount:cGST_Amount,
                SGST_Perc:sGST_Perc,
                SGST_Amount:sGST_Amount,
                IGST_Perc:iGST_Perc,
                IGST_Amount:iGST_Amount,
                TotalAmount: totalAmount,
                ProductId: labourProductID,
            };
            siDocumentList.push(siDocument);
        }

    });

    var accessMode = 1;//Add Mode
    if (hdnSIId.val() != null && hdnSIId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var flag = true;

    $('#tblProductList tr:not(:has(th))').each(function (i, row) {
        var count = 0;
        var matchQTY = 0;
        var $row = $(row);
        var productId = $row.find("#hdnProductId").val();
        var quantity = $row.find("#hdnQuantity").val();
        var isSerializedProduct = $row.find("#hdnIsSerializedProduct").val();

        if (productId != undefined) {
            if (isSerializedProduct.toUpperCase() == "TRUE") {
                $('#tblProductList tr:not(:has(th))').each(function (i, row) {
                    var $row = $(row);
                    var newproductId = $row.find("#hdnProductId").val();
                    var newquantity = $row.find("#hdnQuantity").val();
                    var newisSerializedProduct = $row.find("#hdnIsSerializedProduct").val();
                    if (productId == newproductId && newisSerializedProduct.toUpperCase() == "TRUE") {
                        matchQTY = parseFloat(matchQTY) + parseFloat(newquantity);
                    }

                });
                $('#tblProductSerialDetail tr:not(:has(th))').each(function (i, row) {
                    var $row = $(row);
                    var hdnProductId = $row.find("#hdnProductId").val();
                    if (hdnProductId != undefined) {
                        if (productId == hdnProductId) {
                            count = count + 1;
                        }
                    }
                });
                if (count != matchQTY) {
                    flag = false;
                }
            }


        }
    });

    //if (flag == false) {
    //    ShowModel("Alert", "Number of Chassis Serial Selected is not equal to the Product Quantity.")
    //    return false;
    //}
    var requestData = { saleinvoiceViewModel: saleinvoiceViewModel, siProducts: SIProductList, saleinvoiceTaxes: saleinvoiceTaxList, saleinvoiceTerms: saleinvoiceTermList, saleInvoiceProductSerialDetail: saleInvoiceProductSerialDetail, siDocuments: siDocumentList };

    $.ajax({
        url: "../SaleInvoice/AddEditSaleInvoice?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
                   function () {
                       window.location.href = "../SaleInvoice/AddEditSaleInvoice?siId=" + data.trnId + "&AccessMode=3";
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

    $("#txtSINo").val("");
    $("#hdnSIId").val("0");
    $("#txtSIDate").val($("#hdnCurrentDate").val());

    $("#hdnCustomerId").val("0");
    $("#txtCustomerName").val("");
    $("#txtCustomerCode").val("");
    $("#hdnConsigneeId").val("0");
    $("#txtConsigneeName").val("");
    $("#txtConsigneeCode").val("");

    $("#hdnSOId").val("0");
    $("#txtSONo").val("");
    $("#txtSODate").val("");
    $("#ddlApprovalStatus").val("Draft");
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
    $("#txtBGSTNo").val("");
    $("#ddlSCustomerBranch").val("0");
    $("#ddlCurrency").val("0");
    $("#ddlCompanyBranch").val("0");
    $("#ddlInvoiceType").val("0");
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
    $("#txtSGSTNo").val("");
    $("#txtRefNo").val("");
    $("#txtRefDate").val("");
    $('#tblProductList tbody').html('');
    $("#hdnPayToBookId").val("0");
    $("#txtPayToBookName").val("");
    $("#txtPayToBookBranch").val("");

    $("#txtBasicValue").val("");
    $("#txtFreightValue").val("");
    $("#txtInsuranceValue").val("");
    $("#txtLoadingValue").val("");
    $("#txtTotalValue").val("");
    $("#txtRemarks").val("");
    $("#txtLoadingIGST_Amt").val("");
    $("#txtFreightIGST_Amt").val("");
    $("#txtInsuranceIGST_Amt").val("");
    $("#txtReverseChargeAmount").val("");
    $("#txtGrossValue").val("");
    $("txtRoundOfValue").val("");

    $("#chkSamePermanentAddress").prop('checked', false);
    $("#chkReverseChargeApplicable").prop('checked', false);

    $('#tblProductList tbody').html('');
    $('#tblProductSerialDetail tbody').html('');
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
            $("#txtBGSTNo").val(data.GSTNo);
            $("#txtPancard").val(data.PANNo);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    BindCustomerBranchList(customerId)
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
            $("#txtQuantity").val("0");
            $("#txtPrice").val(data.SalePrice);
            $("#txtUOMName").val(data.UOMName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    GetProductAvailableStock(productId);
    GetProductAvailableStockBranchWise(productId)
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


function GetProductAvailableStockBranchWise(productId) {
    var companyBranchID = $("#ddlCompanyBranch").val();

    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductAvailableStock",
        data: { productid: productId, companyBranchId: companyBranchID, trnId: 0, trnType: "Invoice" },
        dataType: "json",
        success: function (data) {
            $("#txtBranchStock").val(data);
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
    }
    else {
        $(".productsection").hide();
        $("#txtProductName").val("");
        $("#hdnProductId").val("0");
        $("#hdnSIProductDetailId").val("0");
        $("#hdnIsSerializedProduct").val("0");
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
        $("#txtHSN_Code").val("");
        $("#txtCGSTPerc").val("");
        $("#txtCGSTPercAmount").val("");
        $("#txtSGSTPerc").val("");

        $("#txtSGSTPercAmount").val("");
        $("#txtIGSTPerc").val("");
        $("#txtIGSTPercAmount").val("");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
    }
}

function validateStateSelection(action) {
    var hdnCustomerStateId = $("#ddlSState");
    var hdnBillingStateId = $("#hdnBillingStateId");

    if (hdnCustomerStateId.val() == "0") {
        ShowModel("Alert", "Please Select Consignee Name.")
        return false;
    }
    else if (hdnBillingStateId.val() == "0") {
        ShowModel("Alert", "Please Select Billing Location")
        return false;
    }
    ShowHideProductPanel(action);
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
    if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
        ShowModel("Alert", "Please Select Company Branch");
        return false;
    }
    $("#divSOList").html("");
    $("#SearchQuotationModel").modal();

}
function SearchSaleOrder() {
    var txtSearchSONo = $("#txtSearchSONo");
    var txtCustomerName = $("#txtSearchCustomerName");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { soNo: txtSearchSONo.val().trim(), customerName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: "Final", displayType: "Popup", companyBranchId: ddlCompanyBranch.val() };
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
function SelectSO(soId, soNo, soDate, customerId, customerCode, customerName, customerGSTNo, consigneeId, consigneeName, consigneeCode, consigneeGSTNo, companyBranchId) {
    $("#txtSONo").val(soNo);
    $("#hdnSOId").val(soId);
    $("#txtSODate").val(soDate);
    $("#hdnCustomerId").val(customerId);
    $("#txtCustomerCode").val(customerCode);
    $("#txtCustomerName").val(customerName);
    $("#txtBTINNo").val(customerGSTNo);
    $("#txtBGSTNo").val(customerGSTNo);

    $("#hdnConsigneeId").val(consigneeId);
    $("#txtConsigneeName").val(consigneeName);
    $("#txtConsigneeCode").val(consigneeCode);
    $("#txtSTINNo").val(consigneeGSTNo);

    $("#ddlCompanyBranch").val(companyBranchId);
    $("#ddlCompanyBranch").attr('disabled', true);
    GetCustomerDetail(customerId);
    $("#txtCustomerName").attr('disabled', true);
    $("#txtConsigneeName").attr('disabled', true);
    $(".selectpo").hide();
    GetSODetail(soId);
    $("#SearchQuotationModel").modal('hide');

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
        if ($("#txtBGSTNo").val().trim() != "") {
            $("#txtSGSTNo").val($("#txtBGSTNo").val().trim());
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
        $("#txtSGSTNo").val("");
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
            $("#txtSGSTNo").val(data.GSTNo);
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
            $("#txtBGSTNo").val(data.GSTNo);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function OpenPrintPopup() {
    $("#printModel").modal();
}

function ShowHidePrintOption() {
    var reportOption = $("#ddlPrintOption").val();
    if (reportOption == "Original") {
        $("#btnPrintOriginal").show();
        $("#btnPrintDuplicate,#btnPrintTriplicate,#btnPrintQuadruplicate").hide();
    }
    else if (reportOption == "Duplicate") {
        $("#btnPrintDuplicate").show();
        $("#btnPrintOriginal,#btnPrintTriplicate,#btnPrintQuadruplicate").hide();
    }
    else if (reportOption == "Triplicate") {
        $("#btnPrintTriplicate").show();
        $("#btnPrintOriginal,#btnPrintDuplicate,#btnPrintQuadruplicate").hide();
    }
    else if (reportOption == "Quadruplicate") {
        $("#btnPrintQuadruplicate").show();
        $("#btnPrintOriginal,#btnPrintDuplicate,#btnPrintTriplicate").hide();
    }

}

function SendMail() {
    var hdnSIId = $("#hdnSIId");
    var requestData = { siId: hdnSIId.val(), reportType: "PDF" };
    $.ajax({
        url: "../SaleInvoice/SaleInvoiceMail",
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
function GetSODetail(soId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SO/GetSODetail",
        data: { soId: soId },
        dataType: "json",
        success: function (data) {

            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            GetBranchStateId();
            $("#ddlCurrency").val(data.CurrencyCode);

            $("#txtBContactPerson").val(data.ContactPerson);
            $("#txtBAddress").val(data.BillingAddress);
            $("#txtBCity").val(data.City);
            $("#ddlBCountry").val(data.CountryId);
            $("#ddlBState").val(data.StateId);
            $("#txtBPinCode").val(data.PinCode);
            $("#txtBTINNo").val(data.TINNo);
            $("#txtBGSTNo").val(data.GSTNo);
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
            $("#txtSGSTNo").val(data.ShippingTINNo);

            $("#txtSEmail").val(data.ShippingEmail);
            $("#txtSMobileNo").val(data.ShippingMobileNo);
            $("#txtSContactNo").val(data.ShippingContactNo);
            $("#txtSFax").val(data.ShippingFax);

            $("#txtPayToBookName").val(data.PayToBookName);
            $("#hdnPayToBookId").val(data.PayToBookId);
            $("#txtPayToBookBranch").val(data.PayToBookBranch);

            $("#txtRefNo").val(data.RefNo);
            $("#txtRefDate").val(data.RefDate);

            $("#txtBasicValue").val(data.BasicValue);
            $("#txtGrossValue").val(data.TotalValue);
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

            $("#txtRTOReg").val(data.RtoRegsValue);
            $("#txtRTORegCGST_Amt").val(data.RtoRegsCGST_Amt);
            $("#hdnRTORegCGST_Perc").val(data.RtoRegsCGST_Perc);
            $("#txtRTORegSGST_Amt").val(data.RtoRegsSGST_Amt);
            $("#hdnRTORegSGST_Perc").val(data.RtoRegsSGST_Perc);
            $("#txtRTORegIGST_Amt").val(data.RtoRegsIGST_Amt);
            $("#hdnRTORegIGST_Perc").val(data.RtoRegsIGST_Perc);
            $("#txtVehicleInsurance").val(data.VehicleInsuranceValue);


            $("#txtIdTypeName").val(data.IdtypeName);
            $("#txtIDTypeValue").val(data.IdtypeValue);
            $("#txtAdharcardNo").val(data.AdharcardNo);
            $("#txtPancard").val(data.Pancard);
            $("#txtHypothecationBy").val(data.HypothecationBy);

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

    var soProducts = [];
    GetSOProductList(soProducts, soId);
    var soTaxes = [];
    GetSOTaxList(soTaxes, soId);

}
function GetSOProductList(soProducts, soId) {

    var requestData = { soProducts: soProducts, soId: soId };
    $.ajax({
        url: "../SaleInvoice/GetSISOProductList",
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

function ShowHideProductSerialPanel(action) {

    if (action == 1) {
        //       $(".productserialsection").show();
        $("#txtChasisSerial").val("")
        $("#txtChasisProductName").val("");
        $("#hdnProductId").val("0");
        $("#ddlPackingListType").val("0");
    }
    /*else {
        $(".productserialsection").hide();
       
        $("#FileUpload1").val("");

        $("#btnAddDocument").show();
        $("#btnUpdateDocument").hide();
    }*/
}

function SaveProductSerial() {

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
            //fileData.append("PackingTypeListId", ddlPackingListType.val());
        }


    } else {

        ShowModel("Alert", "FormData is not supported.")
        return "";
    }



    $.ajax({
        url: "../SaleInvoice/SaveProductSerialDetail",
        type: "POST",
        asnc: false,
        contentType: false,
        processData: false,
        data: fileData,
        error: function (err) {

        },
        success: function (data) {
            $("#divProductSerialList").html("");
            $("#divProductSerialList").html(data);
            BindPackingListType();

            ShowHideProductSerialPanel(2);

        }
    });
}

function AddProductSerial() {
    var ProductSerialList = [];
    GetSaleProductSerialList(ProductSerialList);
}

function GetSaleProductSerialList(saleInvoiceProductSerialDetails) {
    var hdnSIId = $("#hdnSIId");
    var requestData = { saleSerials: saleInvoiceProductSerialDetails, saleinvoiceId: hdnSIId.val() };
    $.ajax({
        url: "../SaleInvoice/GetSaleInvoiceProductSerialDetailList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divProductSerialList").html("");
            $("#divProductSerialList").html(err);
        },
        success: function (data) {
            $("#divProductSerialList").html("");
            $("#divProductSerialList").html(data);
            //BindPackingListType();                          
            //ShowHideProductSerialPanel(2);
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
        var hdnBillingStateId = $("#hdnBillingStateId");
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
                TotalGST = CGSTAmount + SGSTAmount;
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

var checkedFlag = true;
function SetGSTPercentageInProduct() {
    checkedFlag = true;
    var hdnCustomerStateId = $("#ddlSState");
    var hdnBillingStateId = $("#hdnBillingStateId");

    if (hdnCustomerStateId.val() == "0") {
        ShowModel("Alert", "Please Select Consignee Name.")
        checkedFlag = false;
        return false;
    }
    else if (hdnBillingStateId.val() == "0") {
        ShowModel("Alert", "Please Select Billing Location.")
        checkedFlag = false;
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
        ShowModel("Alert", "Please Select Consignee Name.")
        return false;
    }
    else if (hdnBillingStateId.val() == "0") {
        ShowModel("Alert", "Please Select Billing Location.")
        return false;
    }


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

                    CGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(CGSTPerc)) / 100).toFixed(2);
                }
                $row.find("#tdCGSTAmount").html(CGSTAmount);
                $row.find("#tdCGST_Perc").html(CGSTPerc);
                $row.find("#hdnCGSTAmount").val(CGSTAmount);
                $row.find("#hdnCGSTPerc").val(CGSTPerc);

                if (parseFloat(SGSTPerc) > 0) {
                    SGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(SGSTPerc)) / 100).toFixed(2);
                }

                $row.find("#tdSGSTAmount").html(SGSTAmount);
                $row.find("#tdSGST_Perc").html(SGSTPerc);
                $row.find("#hdnSGSTAmount").val(SGSTAmount);
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
                    IGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(IGSTPerc)) / 100).toFixed(2);
                }

                $row.find("#tdIGSTAmount").html(IGSTAmount);
                $row.find("#tdIGST_Perc").html(IGSTPerc);
                $row.find("#hdnIGSTAmount").val(IGSTAmount);
                $row.find("#hdnIGSTPerc").val(IGSTPerc);



            }

            if (totalPrice != undefined) {
                var itemTotal = parseFloat(parseFloat(totalPrice) - parseFloat(discountAmount) + parseFloat(CGSTAmount) + parseFloat(SGSTAmount) + parseFloat(IGSTAmount));
                basicValue += itemTotal;
                $row.find("#tdTotalPrice").val(itemTotal);
                $row.find("#hdnTotalPrice").val(itemTotal);

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

    var totalsum = 0;
    $('#tblDocumentList tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var totalAmount = $row.find("#hdnTotalAmount").val();

        if (parseFloat(totalAmount) > 0) {
            totalsum += totalAmount;
        }

    });

    var freightValue = $("#txtFreightValue").val() == "" ? "0" : $("#txtFreightValue").val();

    var loadingValue = $("#txtLoadingValue").val() == "" ? "0" : $("#txtLoadingValue").val();

    var insuranceValue = $("#txtInsuranceValue").val() == "" ? "0" : $("#txtInsuranceValue").val();

    var rTORegValue = $("#txtRTOReg").val() == "" ? "0" : $("#txtRTOReg").val();

    var vehicleInsurance = $("#txtVehicleInsurance").val() == "" ? "0" : $("#txtVehicleInsurance").val();

    if (parseFloat(freightValue) <= 0) {
        freightValue = 0;
    }
    if (parseFloat(loadingValue) <= 0) {
        loadingValue = 0;
    }
    if (parseFloat(insuranceValue) <= 0) {
        insuranceValue = 0;
    }

    if (parseFloat(rTORegValue) <= 0) {
        rTORegValue = 0;
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

    var rTORegCGST_Perc = maxCGSTPerc;
    var rTORegSGST_Perc = maxSGSTPerc;
    var rTORegIGST_Perc = maxIGSTPerc;

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

        rTORegCGST_Perc = rTORegCGST_Perc == "" ? 0 : rTORegCGST_Perc;
        rTORegSGST_Perc = rTORegSGST_Perc == "" ? 0 : rTORegSGST_Perc;
        rTORegIGST_Perc = 0;
        $("#hdnRTORegCGST_Perc").val(rTORegCGST_Perc);
        $("#hdnRTORegSGST_Perc").val(rTORegSGST_Perc);
        $("#hdnRTORegIGST_Perc").val(rTORegIGST_Perc);


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

        rTORegCGST_Perc = 0;
        rTORegSGST_Perc = 0;
        rTORegIGST_Perc = rTORegIGST_Perc == "" ? 0 : rTORegIGST_Perc;
        $("#hdnRTORegCGST_Perc").val(rTORegCGST_Perc);
        $("#hdnRTORegSGST_Perc").val(rTORegSGST_Perc);
        $("#hdnRTORegIGST_Perc").val(rTORegIGST_Perc);


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

    var rTORegCGST_Amount = 0;
    var rTORegSGST_Amount = 0;
    var rTORegIGST_Amount = 0;

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

    if (parseFloat(rTORegCGST_Perc) > 0) {
        rTORegCGST_Amount = (parseFloat(rTORegValue) * parseFloat(rTORegCGST_Perc)) / 100;
    }
    if (parseFloat(rTORegSGST_Perc) > 0) {
        rTORegSGST_Amount = (parseFloat(rTORegValue) * parseFloat(rTORegSGST_Perc)) / 100;
    }
    if (parseFloat(rTORegIGST_Perc) > 0) {
        rTORegIGST_Amount = (parseFloat(rTORegValue) * parseFloat(rTORegIGST_Perc)) / 100;
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

    $("#txtRTORegCGST_Amt").val(rTORegCGST_Amount.toFixed(2));
    $("#txtRTORegSGST_Amt").val(rTORegSGST_Amount.toFixed(2));
    $("#txtRTORegIGST_Amt").val(rTORegIGST_Amount.toFixed(2));



    $("#txtBasicValue").val(basicValue.toFixed(2));
    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue)
        + parseFloat(taxValue)
        + parseFloat(freightValue)
        + parseFloat(freightCGST_Amount)
        + parseFloat(freightSGST_Amount)
        + parseFloat(freightIGST_Amount)
        + parseFloat(loadingValue)
        + parseFloat(loadingCGST_Amount)
        + parseFloat(loadingSGST_Amount)
        + parseFloat(loadingIGST_Amount)
        + parseFloat(insuranceValue)
        + parseFloat(insuranceCGST_Amount)
        + parseFloat(insuranceSGST_Amount)
        + parseFloat(insuranceIGST_Amount)
        + parseFloat(rTORegValue)
        + parseFloat(rTORegCGST_Amount)
        + parseFloat(rTORegSGST_Amount)
        + parseFloat(rTORegIGST_Amount)
        + parseFloat(totalsum)
        + parseFloat(vehicleInsurance)).toFixed(0));

    $("#txtGrossValue").val(parseFloat(parseFloat(basicValue)
       + parseFloat(taxValue)
       + parseFloat(freightValue)
        + parseFloat(freightCGST_Amount)
        + parseFloat(freightSGST_Amount)
        + parseFloat(freightIGST_Amount)
        + parseFloat(loadingValue)
        + parseFloat(loadingCGST_Amount)
        + parseFloat(loadingSGST_Amount)
        + parseFloat(loadingIGST_Amount)
        + parseFloat(insuranceValue)
        + parseFloat(insuranceCGST_Amount)
        + parseFloat(insuranceSGST_Amount)
        + parseFloat(insuranceIGST_Amount)
        + parseFloat(rTORegValue)
        + parseFloat(rTORegCGST_Amount)
        + parseFloat(rTORegSGST_Amount)
        + parseFloat(rTORegIGST_Amount)
       + parseFloat(vehicleInsurance)
       + parseFloat(totalsum)
       ).toFixed(2));


    var grossValue = $("#txtGrossValue").val();
    var str = grossValue.split('.');
    var roundVal = parseFloat("0." + str[1]);
    var round_Amt;
    if (roundVal == '0.00') {
        round_Amt = roundVal;
    }
    else if (roundVal < 0.50) {
        round_Amt = "-" + roundVal;
    }
    else {
        var roundAmt = parseInt(100 - str[1]);
        if (roundAmt > 0 && roundAmt < 10) {
            round_Amt = "0.0" + roundAmt;
        }
        else {
            round_Amt = "0." + roundAmt;
        }
    }
    $("#txtRoundOfValue").val(round_Amt);
    $("#txtTotalValue").val(Math.round(parseFloat(parseFloat(basicValue)
        + parseFloat(taxValue)
        + parseFloat(freightValue)
        + parseFloat(freightCGST_Amount)
        + parseFloat(freightSGST_Amount)
        + parseFloat(freightIGST_Amount)
        + parseFloat(loadingValue)
        + parseFloat(loadingCGST_Amount)
        + parseFloat(loadingSGST_Amount)
        + parseFloat(loadingIGST_Amount)
        + parseFloat(insuranceValue)
        + parseFloat(insuranceCGST_Amount)
        + parseFloat(insuranceSGST_Amount)
        + parseFloat(insuranceIGST_Amount)
        + parseFloat(rTORegValue)
        + parseFloat(rTORegCGST_Amount)
        + parseFloat(rTORegSGST_Amount)
        + parseFloat(rTORegIGST_Amount)
        + parseFloat(vehicleInsurance)
        + parseFloat(totalsum)
        ).toFixed(2)));

}

function ExecuteSave() {
    SetGSTPercentageInProduct();
    setTimeout(
function () {
    if (checkedFlag == true) {
        SaveData();
    }

}, 2000);

}

function BindPackingListType() {
    $(".ddlPackingListType").val(0);
    $(".ddlPackingListType").html("");
    var hdnSIId = $("#hdnSIId").val();
    $.ajax({
        type: "GET",
        url: "../PackingList/GetAllPackingListType",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $(".ddlPackingListType").append($("<option></option>").val(0).html("-Select PackingType-"));
            $.each(data, function (i, item) {
                if (item.PackingListTypeID == 2) {
                    $(".ddlPackingListType").append($("<option selected></option>").val(item.PackingListTypeID).html(item.PackingListTypeName));
                }
                else {
                    $(".ddlPackingListType").append($("<option></option>").val(item.PackingListTypeID).html(item.PackingListTypeName));
                }

            });
            binddropdown();
            if (hdnSIId == 0) {
                $(".ddlPackingListType").val($(".ddlPackingListType option:eq(2)").val());
            }

        },
        error: function (Result) {
            $(".ddlPackingListType").append($("<option></option>").val(0).html("-Select PackingType-"));
        }
    });

}

function ShowHideProductModel() {
    // $("#AddProductModel").modal();
    CheckMasterPermission($("#hdnRoleId").val(), 13, 'AddProductModel');
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

function binddropdown() {
    $('#tblProductSerialDetail tr').each(function (i, row) {
        var $row = $(row);
        var hdnPackingListTypeId = $row.find("#hdnPackingListTypeId").val();
        var ddlPackingListType = $(row).find("#ddlPackingListType");
        ddlPackingListType.val(hdnPackingListTypeId);

    });
}
//open master Customer pop up----------
function OpenCustomerMasterPopup() {
    CheckMasterPermission($("#hdnRoleId").val(), 32, 'AddNewCustomer');
}
//open master Consignee pop up----------
function OpenConsigneeMasterPopup() {

    CheckMasterPermission($("#hdnRoleId").val(), 32, 'AddNewConsignee');
}

//function ResetPage() {
//    if (confirm("Are you sure to reset the page")) {
//        window.location.href = "../SaleInvoice/AddEditSaleInvoice";
//    }
//}

function ResetPage1() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../SaleInvoice/AddEditSaleInvoice";
    }

}

function ResetPage2() {
    if (confirm("Are you sure to reset the page")) {
        $("#ddlTermTemplate").val("0");
        $("#txtTermDesc").val("");
        $('#tblTermList').html();
        $('#tblTermList tbody').html();
        $('#divTermList').hide();
        return false;
    }
}

function ResetPage3() {
    if (confirm("Are you sure to reset the page")) {
        //$("#tblProductSerialDetail").html("");
        //$('#tblProductSerialDetail tbody').html("");
        $("#tblProductSerialDetail > tbody").empty();
        //$('#divProductSerialList').hide();
        return false;
    }
}

function ResetPage4() {
    if (confirm("Are you sure to reset the page")) {
        $("#ddlDocumentType").val(0);
        //$("#tblDocumentList").html("");
        //$('#tblDocumentList tbody').html();
        $("#tblDocumentList > tbody").empty();
        //$('#divDocumentList').hide();
        return false;
    }
}

function ShowHideProductPurchaseModel() {
    var hdnProductId = $("#hdnProductId");
    if (hdnProductId.val() == "0") {
        alert("Please Select Product First");
    }
    else {
        $("#PurchaseProductModel").modal();
        SearchProductPurchase();
    }

}

function SearchProductPurchase() {
    var hdnProductId = $("#hdnProductId");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    //var requestData = { };
    $.ajax({
        url: "../SaleInvoice/GetPorductSaleList",
        data: { productId: hdnProductId.val(), companyBranch: ddlCompanyBranch.val() },
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#DivSeePreviousPurchaseProduct").html("");
            $("#DivSeePreviousPurchaseProduct").html(err);
        },
        success: function (data) {
            $("#DivSeePreviousPurchaseProduct").html("");
            $("#DivSeePreviousPurchaseProduct").html(data);
        }
    });
}

function BindTermsDescriptions() {
    var termTemplateId = 1011;
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

//Check Pop Up Master Permissions by User Role, Master Id
function CheckMasterPermission(RoleId, InterfaceId, ModalId) {
    var IsAuthorized = false;
    var AccessMode = 1;
    $.ajax({
        type: "GET",
        url: "../Role/CheckMasterPermission",
        data: { roleId: RoleId, interfaceId: InterfaceId, accessMode: AccessMode },
        dataType: "json",
        asnc: true,
        success: function (data) {
            if (data != null) {
                if (data == true) {
                    IsAuthorized = true;
                    if (IsAuthorized == true) {
                        $("#" + ModalId).modal();
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

function BindCompanyBranchTypeList() {

    $("#ddlBranchType").val(0);
    $("#ddlBranchType").html("");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var CompanyBranchID = 0;
    if (ddlCompanyBranch.val() != null) {
        CompanyBranchID = ddlCompanyBranch.val();
    }

    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchType",
        data: { companyBranchId: CompanyBranchID },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlBranchType").append($("<option></option>").val(0).html("-Select Branch Type-"));
            $.each(data, function (i, item) {

                $("#ddlBranchType").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchType));
            });


        },
        error: function (Result) {
            $("#ddlBranchType").append($("<option></option>").val(0).html("-Select Branch Type-"));
        }
    });
}

function BindCompanyBranchTypeListOnChange() {

    $("#ddlBranchType").val(0);
    $("#ddlBranchType").html("");
    var CompanyBranchID = $("#ddlCompanyBranch option:selected").val();
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchType",
        data: { companyBranchId: CompanyBranchID },
        dataType: "json",
        asnc: false,
        success: function (data) {

            $.each(data, function (i, item) {

                $("#ddlBranchType").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchType));
                if (item.BranchType == "Showroom") {
                    $(".showdata").show();
                }
                else {
                    $(".showdata").hide();
                    $("#txtIdTypeName").val("");
                    $("#txtIDTypeValue").val("");
                    $("#txtAdharcardNo").val("");
                    $("#txtPancard").val("");
                }
            });


        },
        error: function (Result) {
            $("#ddlBranchType").append($("<option></option>").val(0).html("-Select Branch Type-"));
        }
    });
}

function CalculateRTORegistrationTotalCharges() {

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



    var rTORegValue = $("#txtRTOReg").val();
    var rTORegCGST_Perc = maxCGSTPerc;
    var rTORegSGST_Perc = maxSGSTPerc;
    var rTORegIGST_Perc = maxIGSTPerc;


    rTORegValue = rTORegValue == "" ? 0 : rTORegValue;
    if (hdnBillingStateId == ddlSState) {
        rTORegCGST_Perc = rTORegCGST_Perc == "" ? 0 : rTORegCGST_Perc;
        rTORegSGST_Perc = rTORegSGST_Perc == "" ? 0 : rTORegSGST_Perc;
        rTORegIGST_Perc = 0;
        $("#hdnRTORegCGST_Perc").val(rTORegCGST_Perc);
        $("#hdnRTORegSGST_Perc").val(rTORegSGST_Perc);
        $("#hdnRTORegIGST_Perc").val(rTORegIGST_Perc);
    }
    else {
        rTORegCGST_Perc = 0;
        rTORegSGST_Perc = 0;
        rTORegIGST_Perc = rTORegIGST_Perc == "" ? 0 : rTORegIGST_Perc;
        $("#hdnRTORegCGST_Perc").val(rTORegCGST_Perc);
        $("#hdnRTORegSGST_Perc").val(rTORegSGST_Perc);
        $("#hdnRTORegIGST_Perc").val(rTORegIGST_Perc);
    }

    var rTORegCGST_Amount = 0;
    var rTORegSGST_Amount = 0;
    var rTORegIGST_Amount = 0;


    if (parseFloat(rTORegCGST_Perc) > 0) {
        rTORegCGST_Amount = (parseFloat(rTORegValue) * parseFloat(rTORegCGST_Perc)) / 100;
    }
    if (parseFloat(rTORegSGST_Perc) > 0) {
        rTORegSGST_Amount = (parseFloat(rTORegValue) * parseFloat(rTORegSGST_Perc)) / 100;
    }
    if (parseFloat(rTORegIGST_Perc) > 0) {
        rTORegIGST_Amount = (parseFloat(rTORegValue) * parseFloat(rTORegIGST_Perc)) / 100;
    }

    $("#txtRTORegCGST_Amt").val(rTORegCGST_Amount.toFixed(2));
    $("#txtRTORegSGST_Amt").val(rTORegSGST_Amount.toFixed(2));
    $("#txtRTORegIGST_Amt").val(rTORegIGST_Amount.toFixed(2));
}

function GetProductChasisSerialDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productid: productId },
        dataType: "json",
        success: function (data) {
            $("#txtChasisProductName").val(data.ProductName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function AddProductChasisSerialDetail(action) {
    var txtChasisSerial = $("#txtChasisSerial");
    var hdnProductId = $("#hdnProductId");
    var ddlPackingListType = $("#ddlPackingListType");

    var txtChasisProductName = $("#txtChasisProductName");
    if (txtChasisSerial.val().trim() == "") {
        ShowModel("Alert", "Please Enter Product Chasis Serial")
        txtChasisSerial.focus();
        return false;
    }
    if (ddlPackingListType.val() == "0") {
        ShowModel("Alert", "Please Select Packing List Type")
        ddlPackingListType.focus();
        return false;
    }
    var rowCount = $('#tblProductList tr').length;
    if (rowCount == 1) {
        ShowModel("Alert", "Please Add Product");
        return false;
    }

    var productChasisSerialList = [];
    $('#tblProductSerialDetail tr').each(function (i, row) {
        var $row = $(row);
        var hdnFileName = $row.find("#hdnFileName").val();
        var hdnProductId = $row.find("#hdnProductId").val();
        var hdnProductName = $row.find("#hdnProductName").val();
        var hdnPackingListTypeId = $row.find("#hdnPackingListTypeId").val();
        var hdnPackingListTypeName = $row.find("#hdnPackingListTypeName").val();
        var hdnRefSerial1 = $row.find("#hdnRefSerial1").val();
        var hdnRefSerial2 = $row.find("#hdnRefSerial2").val();
        var hdnRefSerial3 = $row.find("#hdnRefSerial3").val();
        var hdnRefSerial4 = $row.find("#hdnRefSerial4").val();
        if (hdnProductId != undefined) {
            if (action == 1) {
                if (hdnRefSerial1 == txtChasisSerial.val()) {
                    ShowModel("Alert", "Chasis Serial already added")
                    return false;
                }
                var saleInvoiceChasisSerial = {
                    InvoiceId: 0,
                    ProductId: hdnProductId,
                    ProductName: hdnProductName,
                    RefSerial1: hdnRefSerial1,
                    RefSerial2: hdnRefSerial2,
                    RefSerial3: hdnRefSerial3,
                    RefSerial4: hdnRefSerial4,
                    PackingListTypeID: hdnPackingListTypeId,
                    PackingListTypeName: hdnPackingListTypeName
                };
                productChasisSerialList.push(saleInvoiceChasisSerial);
            }
        }

    });

    var saleInvoiceChasisSerial = {
        InvoiceId: 0,
        ProductId: hdnProductId.val(),
        ProductName: txtChasisProductName.val(),
        RefSerial1: txtChasisSerial.val(),
        RefSerial2: "",
        RefSerial3: "",
        RefSerial4: "",
        PackingListTypeID: ddlPackingListType.val(),
        PackingListTypeName: $("#ddlPackingListType option:selected").text()
    };

    productChasisSerialList.push(saleInvoiceChasisSerial);
    GetSaleProductSerialList(productChasisSerialList);
    $("#txtChasisSerial").val("");
    $("#hdnProductId").val("0");
    $("#txtChasisProductName").val("");
}

function RemoveProductSerialRow(obj) {
    if (confirm("Do You Want to Remove Selected Product Serial ?")) {
        var row = $(obj).closest("tr");
        var saleinvoiceProductDetailId = $(row).find("#hdnRefSerial1").val();
        ShowModel("Alert", "Product Serial Removed from List.");
        row.remove();
    }
}


function CalculateTotalLabourCharges() {
    var price = $("#txtLabourRate").val();
    var discountPerc = $("#txtLabourDiscountPerc").val();
    var CGSTPerc = $("#txtCGSTPercLabour").val();
    var SGSTPerc = $("#txtSGSTPercLabour").val();
    var IGSTPerc = $("#txtIGSTPercLabour").val();

    var discountAmount = 0;
    var CGSTAmount = 0;
    var SGSTAmount = 0;
    var IGSTAmount = 0;
    var totalTaxAmount = 0;
    price = price == "" ? 0 : price;

    var totalPrice = parseFloat(price);
    if (parseFloat(discountPerc) > 0) {
        discountAmount = ((parseFloat(totalPrice) * parseFloat(discountPerc)) / 100).toFixed(2)
    }
    $("#txtLabourDiscountAmount").val(discountAmount);





    if (parseFloat(CGSTPerc) > 0) {

        CGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(CGSTPerc)) / 100).toFixed(2);
    }
    $("#txtCGSTPercAmountLabour").val(CGSTAmount);


    if (parseFloat(SGSTPerc) > 0) {

        SGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(SGSTPerc)) / 100).toFixed(2);

    }
    $("#txtSGSTPercAmountLabour").val(SGSTAmount);


    if (parseFloat(IGSTPerc) > 0) {

        IGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(IGSTPerc)) / 100).toFixed(2);
    }
    $("#txtIGSTPercAmountLabour").val(IGSTAmount);



    $("#txtTotalPriceLabour").val((parseFloat(totalPrice) - parseFloat(discountAmount) + parseFloat(CGSTAmount) +
        parseFloat(SGSTAmount) + parseFloat(IGSTAmount)).toFixed(2));

}
function OpenJobSearchPopup() {

    $("#divJCList").html("");
    $("#SearchJobCard").modal();

}
function SearchJobCard() {
    var txtJobCardNo = $("#txtSearchJobCardNo");
    var txtCustomerName = $("#txtCustomerNameJobCar");
    var ddlApprovalStatus = $("#ddlApprovalStatusJobCard");
    var txtModelName = $("#txtModelName");
    var txtEngineNo = $("#txtEngineNo");
    var txtRegNo = $("#txtRegNo");
    var txtKeyNo = $("#txtKeyNo");
    var txtFromDate = $("#txtFromDateJobCard");
    var txtToDate = $("#txtToDateJobCard");

    var requestData = {
        jobCardNo: txtJobCardNo.val().trim(),
        customerName: txtCustomerName.val(),
        approvalStatus: 'Final',
        modelName: txtModelName.val().trim(),
        engineNo: txtEngineNo.val().trim(),
        regNo: txtRegNo.val(),
        keyNo: txtKeyNo.val().trim(),
        fromDate: txtFromDate.val(),
        toDate: txtToDate.val()

    };
    $.ajax({
        url: "../SaleInvoice/GetJobCardList",
        data: requestData,
        dataType: "html",
        asnc: true,
        type: "GET",
        error: function (err) {
            $("#divJCList").html("");
            $("#divJCList").html(err);
        },
        success: function (data) {
            $("#divJCList").html("");
            $("#divJCList").html(data);

        }
    });
}
function SelectJobCard(jobCardID, jobCardNo, jobCardDate, customerID, customerCode, customerName) {
    $("#txtSONo").val(jobCardNo);
    $("#hdnSOId").val(jobCardID);
    $("#txtSODate").val(jobCardDate);
    $("#hdnCustomerId").val(customerID);
    $("#txtCustomerCode").val(customerCode);
    $("#txtCustomerName").val(customerName);

    $("#hdnConsigneeId").val(customerID);
    $("#txtConsigneeName").val(customerName);
    $("#txtConsigneeCode").val(customerCode);

    GetCustomerDetail(customerID);
    GetConsigneeDetail(customerID);
    $("#txtCustomerName").attr('disabled', true);
    $("#txtConsigneeName").attr('disabled', true);
    var saleinvoiceProducts = [];
    GetSaleInvoiceJobCardProductList(saleinvoiceProducts, jobCardID);
    $("#SearchJobCard").modal('hide');

   

}
function GetSaleInvoiceJobCardProductList(saleinvoiceProducts, jobCardID) {
 
    var requestData = { saleinvoiceProducts: saleinvoiceProducts, jobCardID: jobCardID };
    $.ajax({
        url: "../SaleInvoice/GetSaleInvoiceJobCardProductList",
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