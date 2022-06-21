$(document).ready(function () {
    BindServiceEngineerList();
    BindSDealerList();
    BindDocumentTypeList();
    $("#txtCustomerName").attr('readOnly', true);
    $("#txtCustomerEmail").attr('readOnly', true);
    $("#txtCustomerAddress").attr('readOnly', true);
    $("#txtInvoiceDate").attr('readOnly', true);

    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtCustomerMobile").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../ComplaintService/GetCustomerMobileAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.MobileNo, value: item.CustomerId, CustomerName: item.CustomerName, Email: item.Email, PrimaryAddress: item.PrimaryAddress };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtCustomerMobile").val(ui.item.label);


            return false;
        },
        select: function (event, ui) {
            $("#txtCustomerMobile").val(ui.item.label);
            $("#hdnCustomerId").val(ui.item.value);
            $("#txtCustomerName").val(ui.item.CustomerName);
            $("#txtCustomerEmail").val(ui.item.Email);
            $("#txtCustomerAddress").val(ui.item.PrimaryAddress);
            // GetCustomerDetailByMobile(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtCustomerMobile").val("");
                $("#hdnCustomerId").val("0");
                ShowModel("Alert", "Please select Mobile from List")

            }
            return false;
        }

    })
        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<div><b>" + item.label + " </div>")
                .appendTo(ul);
        };

    $("#txtProductName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../ComplaintService/GetComplaintProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ProductName, value: item.Productid, code: item.ProductCode, warrantyStartDate: item.WarrantyStartDate, WarrantyEndDate: item.WarrantyEndDate };
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
            $("#txtProductCode").val(ui.item.code);
            $("#txtProductWarrantyStartDate").val(ui.item.warrantyStartDate);
            $("#txtWarrantyEndDate").val(ui.item.WarrantyEndDate);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductName").val("");
                $("#hdnProductId").val("0");
                $("#txtProductCode").val("");
                $("#txtProductWarrantyStartDate").val("");
                $("#txtWarrantyEndDate").val("");
                ShowModel("Alert", "Please select Product from List")

            }
            return false;
        }

    })
        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<div><b>" + item.label + "</div>")
                .appendTo(ul);
        };

    $("#txtComplaintDate").datepicker({
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

    BindCompanyBranchList()
    BindComplaintTypeList();


    var hdnAccessMode = $("#hdnAccessMode");
    var hdncomplaintServiceId = $("#hdncomplaintServiceId");
    if (hdncomplaintServiceId.val() != "" && hdncomplaintServiceId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
            function () {
                GetComplaintServiceDetail(hdncomplaintServiceId.val());
            }, 3000);

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("#FileUpload1").attr('disabled', true);
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true);
        }
        else if (hdnAccessMode.val() == "4") {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").hide();
            $("#divRemarks").show();
            $("#FileUpload1").attr('disabled', true);
            $("input").attr('readOnly', true);
            $("#chkStatus").attr('disabled', true);
            $("#ddlEnquiryType").attr('disabled', true);
            $("#ddlComplaintMode").attr('disabled', true);
            $("#ddlCompanyBranch").attr('disabled', true);
            $("#ddlServiceEngineer").attr('disabled', true);
            $("#ddlDealer").attr('disabled', true);
            $("#ddlStatus").find('option:contains("Open")').hide();
            $("#ddlStatus").find('option:contains("Closed")').hide();
            $("#ddlStatus").val("0");
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
    var complaintProductList = [];
    GetComplaintServiceProductList(complaintProductList);
    var complaintDocuments = [];
    GetComplaintDocumentList(complaintDocuments);



});
$(".numeric-only").on("input", function () {
    var regexp = /\D/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});

function BindComplaintTypeList() {
    $.ajax({
        type: "GET",
        url: "../Product/GetProductTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlProductType").append($("<option></option>").val(0).html("-Select Type-"));
            $.each(data, function (i, item) {
                $("#ddlProductType").append($("<option></option>").val(item.ProductTypeId).html(item.ProductTypeName));
            });
        },
        error: function (Result) {
            $("#ddlProductType").append($("<option></option>").val(0).html("-Select Type-"));
        }
    });
}

function BindServiceEngineerList() {
    $.ajax({
        type: "GET",
        url: "../Employee/GetDesignationByDepartmentID",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlServiceEngineer").append($("<option></option>").val(0).html("Select Service Engineer"));
            $.each(data, function (i, item) {
                $("#ddlServiceEngineer").append($("<option></option>").val(item.EmployeeId).html(item.EmployeeName));
            });
        },
        error: function (Result) {
            $("#ddlServiceEngineer").append($("<option></option>").val(0).html("Select Service Engineer"));
        }
    });
}

function BindSDealerList() {
    $.ajax({
        type: "GET",
        url: "../ComplaintService/GetCustomerTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlDealer").append($("<option></option>").val(0).html("Select Dealer"));
            $.each(data, function (i, item) {
                $("#ddlDealer").append($("<option></option>").val(item.ValueInt).html(item.Text));
            });
        },
        error: function (Result) {
            $("#ddlDealer").append($("<option></option>").val(0).html("Select Service Engineer"));
        }
    });
}


function GetCustomerDetailByMobile(customerId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Customer/GetCustomerDetail",
        data: { customerId: customerId },
        dataType: "json",
        success: function (data) {
            $("#txtCustomerAddress").val(data.PrimaryAddress);
            $("#txtCustomerName").val(data.CustomerName);

            $("#txtCustomerEmail").val(data.CustomerEmail);

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}
function OpenInvoiceSearchPopup() {
    $("#SearchInvoiceModel").modal();

}
function SearchInvoice() {
    var txtInvoiceNo = $("#txtSearchInvoiceNo");
    var txtCustomerName = $("#txtSearchCustomerName");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { saleinvoiceNo: txtInvoiceNo.val().trim(), customerName: txtCustomerName.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "", approvalStatus: "Final" };
    $.ajax({
        url: "../ComplaintService/GetChallanSaleInvoiceList",
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
    //$("#hdnCustomerId").val(customerId);
    //$("#txtCustomerCode").val(customerCode);
    //$("#txtCustomerName").val(customerName);
    //$("#hdnConsigneeId").val(consigneeId);
    //$("#txtConsigneeCode").val(consigneeCode);
    //$("#txtConsigneeName").val(consigneeName);
    $("#SearchInvoiceModel").modal('hide');
    //GetConsigneeDetail(consigneeId);
    //GetSaleInvoiceDetail(invoiceId);
    var complaintServiceProducts = [];
    GetComplaintServiceSIProductList(complaintServiceProducts, invoiceId);
    //var saleinvoiceTaxes = [];
    //GetSaleInvoiceTaxList(saleinvoiceTaxes, invoiceId);
    //var saleinvoiceTerms = [];
    //GetSaleInvoiceTermList(saleinvoiceTerms, invoiceId);
}
function GetComplaintServiceSIProductList(complaintServiceProducts, invoiceId) {

    var requestData = { saleinvoiceProducts: complaintServiceProducts, saleinvoiceId: invoiceId };
    $.ajax({
        url: "../ComplaintService/GetComplaintServiceSIProductList",
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

function AddProduct(action) {

    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");
    var hdnComplaintProductDetailId = $("#hdnComplaintProductDetailId");
    var txtProductName = $("#txtProductName");
    var txtProductCode = $("#txtProductCode");
    var hdnMappingId = $("#hdnMappingId");
    var hdnProductId = $("#hdnProductId");
    var txtProductWarrantyStartDate = $("#txtProductWarrantyStartDate");
    var txtWarrantyEndDate = $("#txtWarrantyEndDate");
    var txtProductRemarks = $("#txtProductRemarks");
    var txtQuantity = $("#txtQuantity");

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

    if (txtQuantity.val().trim() == "" || txtQuantity.val().trim() == "0" ||  txtQuantity.val() <= 0) {
        ShowModel("Alert", "Please enter the quantity")
        txtQuantity.focus();
        return false;
    }

    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }
    var complaintProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var complaintProductDetailID = $row.find("#hdnComplaintProductDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var mappingId = $row.find("#hdnMappingId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productstartdate = $row.find("#hdnProductWarrantyStartDate").val();
        var productenddate = $row.find("#hdnProductWarrantyEndDate").val();
        var remarks = $row.find("#hdnRemarks").val();
        var quantity = $row.find("#hdnQuantity").val();

        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already mapped!!!")
                    txtProductName.focus();
                    return false;
                }

                var complaintProduct = {
                    ComplaintProductDetailID: complaintProductDetailID,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    MappingId: mappingId,
                    ProductName: productName,
                    ProductCode: productCode,
                    WarrantyStartDate: productstartdate,
                    WarrantyEndDate: productenddate,
                    Remarks: remarks,
                    Quantity: quantity
                };
                complaintProductList.push(complaintProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnSequenceNo.val() == sequenceNo) {
                var complaintProduct = {
                    ComplaintProductDetailID: hdnComplaintProductDetailId.val(),
                    MappingId: hdnMappingId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    WarrantyStartDate: txtProductWarrantyStartDate.val(),
                    WarrantyEndDate: txtWarrantyEndDate.val(),
                    Remarks: txtProductRemarks.val().trim(),
                    Quantity: txtQuantity.val(),

                };
                complaintProductList.push(complaintProduct);
                hdnSequenceNo.val("0");

            }
        }
    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var complaintProductAddEdit = {
            ComplaintProductDetailID: hdnComplaintProductDetailId.val(),
            MappingId: hdnMappingId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            WarrantyStartDate: txtProductWarrantyStartDate.val(),
            WarrantyEndDate: txtWarrantyEndDate.val(),
            Remarks: txtProductRemarks.val().trim(),
            Quantity: txtQuantity.val(),

        };

        complaintProductList.push(complaintProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetComplaintServiceProductList(complaintProductList);
    }


}
function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var complaintProductDetailId = $(row).find("#hdnComplaintProductDetailId").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var mappingId = $(row).find("#hdnMappingId").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var remarks = $(row).find("#hdnRemarks").val();
    var quantity = $(row).find("#hdnQuantity").val();


    $("#txtProductName").val(productName);
    $("#hdnComplaintProductDetailId").val(complaintProductDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnMappingId").val(mappingId);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductRemarks").val(remarks);
    $("#txtQuantity").val(quantity);
    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    ShowHideProductPanel(1);

}

function RemoveProductRow(obj) {

    var hdnAccessMode = $("#hdnAccessMode");
    if (hdnAccessMode.val() == "3") {
        ShowModel("Alert", "You can't modify in view mode.")
        return false;
    }
    else {


        if (confirm("Do you want to remove selected Product?")) {
            var row = $(obj).closest("tr");
            var SequenceNo = $(row).find("#hdnSequenceNo").val();
            var hdnProductId = $(row).find("#hdnProductId").val();
            var mappingId = $(row).find("#hdnMappingId").val();
            ShowModel("Alert", "Product Removed from List.");
            row.remove();

        }

    }
}

function GetComplaintServiceProductList(complaintProduct) {
    var hdncomplaintServiceId = $("#hdncomplaintServiceId");
    var requestData = { complaintServiceProducts: complaintProduct, complaintServiceId: hdncomplaintServiceId.val() };
    $.ajax({
        url: "../ComplaintService/GetComplaintServiceProductList",
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
            ShowHideProductPanel(2);
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
        $("#txtProductCode").val("");
        $("#txtProductRemarks").val("");
        $("#txtQuantity").val("");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
    }
}

function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}

function SaveData() {
    var hdncomplaintServiceId = $("#hdncomplaintServiceId");
    var txtComplaintNo = $("#txtComplaintNo");
    var txtComplaintDate = $("#txtComplaintDate");
    var ddlEnquiryType = $("#ddlEnquiryType");
    var ddlComplaintMode = $("#ddlComplaintMode");
    var txtCustomerMobile = $("#txtCustomerMobile");
    var txtCustomerName = $("#txtCustomerName");
    var txtCustomerEmail = $("#txtCustomerEmail");
    var txtCustomerAddress = $("#txtCustomerAddress");
    var txtInvoiceNo = $("#txtInvoiceNo");
    var txtComplaintDescription = $("#txtComplaintDescription");
    var chkStatus = $("#chkStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlServiceEngineer = $("#ddlServiceEngineer");
    var ddlDealer = $("#ddlDealer");
    var txtInvoiceDate = $("#txtInvoiceDate");
    var ddlStatus = $("#ddlStatus");
    var hdnAccessMode = $("#hdnAccessMode");
    var txtRemarks = $("#txtRemarks");

    if (ddlEnquiryType.val() == "" || ddlEnquiryType.val() == "0") {
        ShowModel("Alert", "Please Select Enquiry Type")
        ddlEnquiryType.focus();
        return false;
    }

    if (ddlStatus.val() == "" || ddlStatus.val() == "0") {
        ShowModel("Alert", "Please Select Complaint Status")
        ddlStatus.focus();
        return false;
    }
    if (hdnAccessMode.val() == "4") {
        if (txtRemarks.val().trim() == "") {
            ShowModel("Alert", "Please enter Remarks")
            txtRemarks.focus();
            return false;
        }
    }

    if (ddlComplaintMode.val() == "" || ddlComplaintMode.val() == "0") {
        ShowModel("Alert", "Please Select Complaint Mode")
        ddlComplaintMode.focus();
        return false;
    }

    if (txtCustomerMobile.val().trim() == "") {
        ShowModel("Alert", "Please Select Customer Mobile No.")
        txtCustomerMobile.focus();
        return false;
    }

    if (txtComplaintDescription.val().trim() == "") {
        ShowModel("Alert", "Please Enter Complaint Description.")
        txtComplaintDescription.focus();
        return false;
    }


    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Company Branch")
        ddlCompanyBranch.focus();
        return false;
    }

    if (ddlServiceEngineer.val() == "" || ddlServiceEngineer.val() == "0") {
        ShowModel("Alert", "Please select service engineer")
        ddlServiceEngineer.focus();
        return false;
    }

    if (ddlDealer.val() == "" || ddlDealer.val() == "0") {
        ShowModel("Alert", "Please select dealer")
        ddlDealer.focus();
        return false;
    }



    var accessMode = 1;//Add Mode
    if (hdncomplaintServiceId.val() != null && hdncomplaintServiceId.val() != 0) {
        accessMode = 2;//Edit Mode
    }


    var Status = true;
    if (chkStatus.prop("checked") == true) { productStatus = true; }
    else { productStatus = false; }


    var complaintServiceViewModel = {
        ComplaintId: hdncomplaintServiceId.val(),
        ComplaintDate: txtComplaintDate.val().trim(),
        InvoiceNo: txtInvoiceNo.val(),
        EnquiryType: ddlEnquiryType.val(),
        ComplaintMode: ddlComplaintMode.val(),
        CustomerMobile: txtCustomerMobile.val().trim(),
        CustomerName: txtCustomerName.val().trim(),
        CustomerEmail: txtCustomerEmail.val().trim(),
        CustomerAddress: txtCustomerAddress.val().trim(),
        ComplaintDescription: txtComplaintDescription.val().trim(),
        ComplaintService_Status: Status,
        BranchID: ddlCompanyBranch.val(),
        EmployeeID: ddlServiceEngineer.val(),
        DealerID: ddlDealer.val(),
        InvoiceDate: txtInvoiceDate.val(),
        ComplaintStatus: ddlStatus.val(),
        Remarks: txtRemarks.val()
    };

    var complaintProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var complaintProductDetailID = $row.find("#hdnComplaintProductDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var mappingId = $row.find("#hdnMappingId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var remarks = $row.find("#hdnRemarks").val();
        var quantity = $row.find("#hdnQuantity").val();

        if (productId != undefined) {
            var complaintProduct = {
                ComplaintProductDetailID: complaintProductDetailID,
                SequenceNo: sequenceNo,
                ProductId: productId,
                MappingId: mappingId,
                ProductName: productName,
                ProductCode: productCode,
                Remarks: remarks,
                Quantity: quantity
            };
            complaintProductList.push(complaintProduct);
        }
    });

    var complaintDocumentList = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var ComplaintDocId = $row.find("#hdnSODocId").val();
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        if (ComplaintDocId != undefined) {
            var complaintDocument = {
                ComplaintDocId: ComplaintDocId,
                DocumentSequenceNo: documentSequenceNo,
                DocumentTypeId: documentTypeId,
                DocumentTypeDesc: documentTypeDesc,
                DocumentName: documentName,
                DocumentPath: documentPath
            };
            complaintDocumentList.push(complaintDocument);
        }

    });

    var rowCount = $('#tblProductList tr').length;
    if (rowCount == 1) {
        alert("Please add at least one product.");
        return false;
    }

    var requestData = { complaintServiceViewModel: complaintServiceViewModel, complaintProducts: complaintProductList, complaintDocuments: complaintDocumentList };
    $.ajax({
        url: "../ComplaintService/AddEditComplaintService?accessMode=" + accessMode + "",
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
                        window.location.href = "../ComplaintService/ListComplaintService";
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
    $("#hdncomplaintServiceId").val("0");
    $("#txtComplaintNo").val("");
    $("#txtComplaintDate").val("");
    $("#ddlEnquiryType").val("0");
    $("#ddlDealer").val("0");
    $("#ddlServiceEngineer").val("0");
    $("#ddlComplaintMode").val("0");
    $("#ddlCompanyBranch").val("0");
    $("#txtCustomerMobile").val("0");
    $("#txtCustomerName").val("");
    $("#txtCustomerEmail").val("");
    $("#txtCustomerAddress").val("");
    $("#txtComplaintDescription").val("");
    $("#txtInvoiceNo").val("");
    $("#txtProductName").val("");
    $("#txtProductCode").val("");
    $("#txtProductRemarks").val("");
    $("#txtInvoiceNo").val("");
    $("#chkStatus").attr("checked", true);
    $("#txtInvoiceDate").val("");
    $("#ddlStatus").val("0");
    
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}

function GetComplaintServiceDetail(ComplaintId) {
    var hdnAccessMode = $("#hdnAccessMode").val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ComplaintService/GetComplaintServiceDetail",
        data: { ComplaintId: ComplaintId },
        dataType: "json",
        success: function (data) {
            $("#txtComplaintNo").val(data.ComplaintNo);
            $("#txtComplaintDate").val(data.ComplaintDate);
            $("#txtInvoiceNo").val(data.InvoiceNo);
            $("#ddlEnquiryType").val(data.EnquiryType);
            $("#ddlComplaintMode").val(data.ComplaintMode);
            $("#txtCustomerMobile").val(data.CustomerMobile);
            $("#txtCustomerName").val(data.CustomerName);
            $("#txtCustomerEmail").val(data.CustomerEmail);
            $("#txtCustomerAddress").val(data.CustomerAddress);
            $("#txtComplaintDescription").val(data.ComplaintDescription);
            $("#ddlCompanyBranch").val(data.BranchID);
            $("#ddlServiceEngineer").val(data.EmployeeID);
            $("#ddlDealer").val(data.DealerID);
            $("#chkStatus").val(data.status);
            $("#txtInvoiceDate").val(data.InvoiceDate);
            if (hdnAccessMode != "4") {
                $("#ddlStatus").val(data.ComplaintStatus);
            }
            if (data.status == "Final") {
                $(".editonly").hide();
                $("#btnReset").hide();
                $("#btnUpdate").hide();
            }

            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);

            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);
            $("#btnAddNew").show();
            $("#btnPrint").show();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
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
        $("#FileUpload2").val("");

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

function SaveDocument() {
    if ($("#ddlDocumentType").val() == "0") {
        ShowModel("Alert", "Please Select document type")
        return false;
    }
    if (window.FormData !== undefined) {
        var uploadfile = document.getElementById('FileUpload2');
        var fileData = new FormData();
        if (uploadfile.value != '') {
            var fileUpload = $("#FileUpload2").get(0);
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
        url: "../ComplaintService/SaveSupportingDocument",
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
                var FileUpload1 = $("#FileUpload2");

                if (ddlDocumentType.val() == "" || ddlDocumentType.val() == "0") {
                    ShowModel("Alert", "Please select Document Type")
                    ddlDocumentType.focus();
                    return false;
                }

                if (FileUpload1.val() == undefined || FileUpload1.val() == "") {
                    ShowModel("Alert", "Please select File To Upload")
                    return false;
                }
                var complaintDocumentList = [];
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

                            var complaintDocument = {
                                ComplaintDocId: hdnSODocId,
                                DocumentSequenceNo: documentSequenceNo,
                                DocumentTypeId: documentTypeId,
                                DocumentTypeDesc: documentTypeDesc,
                                DocumentName: documentName,
                                DocumentPath: documentPath
                            };
                            complaintDocumentList.push(complaintDocument);
                            docEntrySequence = parseInt(docEntrySequence) + 1;
                        }
                        else if (hdnSODocId == ComplaintDocId && hdnDocumentSequence.val() == documentSequenceNo) {
                            var complaintDocument = {
                                ComplaintDocId: hdnSODocId,
                                DocumentSequenceNo: hdnDocumentSequence.val(),
                                DocumentTypeId: ddlDocumentType.val(),
                                DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                                DocumentName: newFileName,
                                DocumentPath: newFileName
                            };
                            complaintDocumentList.push(complaintDocument);
                        }
                    }
                });
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    hdnDocumentSequence.val(docEntrySequence);
                }

                var complaintDocumentAddEdit = {
                    ComplaintDocId: hdnSODocId.val(),
                    DocumentSequenceNo: hdnDocumentSequence.val(),
                    DocumentTypeId: ddlDocumentType.val(),
                    DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                    DocumentName: newFileName,
                    DocumentPath: newFileName
                };
                complaintDocumentList.push(complaintDocumentAddEdit);
                hdnDocumentSequence.val("0");

                GetComplaintDocumentList(complaintDocumentList);



            }
            else {
                ShowModel("Alert", result.message);
            }
        }
    });
}

function GetComplaintDocumentList(complaintDocuments) {
    var hdnComplaintServiceIdID = $("#hdncomplaintServiceId");
    var requestData = { complaintDocuments: complaintDocuments, complaintID: hdnComplaintServiceIdID.val() };
    $.ajax({
        url: "../ComplaintService/GetComplaintSupportingDocumentList",
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

function RemoveDocumentRow(obj) {
    if (confirm("Do you want to remove selected Document?")) {
        var row = $(obj).closest("tr");
        var poDocId = $(row).find("#hdnPODocId").val();
        ShowModel("Alert", "Document Removed from List.");
        row.remove();
    }
}

function ExecuteSave() {

    setTimeout(
        function () {
            SaveData();
        }, 2000);
    SetGSTPercentageInProduct();
}

//open master Customer pop up----------
function OpenCustomerMasterPopup() {
    CheckMasterPermission($("#hdnRoleId").val(), 32, 'AddNewCustomer');
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


('.Quantity').keypress(function (event) {
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