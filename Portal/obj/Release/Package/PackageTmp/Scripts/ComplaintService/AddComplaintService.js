$(document).ready(function () {

    $("#txtCustomerName").attr('readOnly', true);
    $("#txtCustomerEmail").attr('readOnly', true);
    $("#txtCustomerAddress").attr('readOnly', true);

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
            //GetProductDetail(ui.item.value);
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
        }, 2000);

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("#FileUpload1").attr('disabled', true);
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true);
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
    GetComplaintServiceSIProductList(complaintServiceProducts,invoiceId);
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

        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already mapped!!!")
                    txtProductName.focus();
                    return false;
                }

                var complaintProduct = {
                    ComplaintProductDetailID:complaintProductDetailID,                
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    MappingId:mappingId,
                    ProductName: productName,
                    ProductCode: productCode,
                    WarrantyStartDate: productstartdate,
                    WarrantyEndDate:productenddate,
                   Remarks:remarks
                };
                complaintProductList.push(complaintProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnSequenceNo.val() == sequenceNo) {
                var complaintProduct = {
                    ComplaintProductDetailID :hdnComplaintProductDetailId.val(),
                    MappingId: hdnMappingId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    WarrantyStartDate: txtProductWarrantyStartDate.val(),
                    WarrantyEndDate:txtWarrantyEndDate.val(),
                    Remarks:txtProductRemarks.val().trim(),
                   

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


        $("#txtProductName").val(productName);
        $("#hdnComplaintProductDetailId").val(complaintProductDetailId);
        $("#hdnSequenceNo").val(sequenceNo);
        $("#hdnMappingId").val(mappingId);
        $("#hdnProductId").val(productId);
        $("#txtProductCode").val(productCode);
        $("#txtProductRemarks").val(remarks);
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

    if (ddlEnquiryType.val() == "" || ddlEnquiryType.val() == "0") {
        ShowModel("Alert", "Please Select Enquiry Type")
        ddlEnquiryType.focus();
        return false;
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
    
    if (txtInvoiceNo.val().trim() == "") {
        ShowModel("Alert", "Please Select Invoice No")
        txtInvoiceNo.focus();
        return false; 
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Company Branch")
        ddlCompanyBranch.focus();
        return false; 
    }
    
    var accessMode = 1;//Add Mode
    if (hdncomplaintServiceId.val() != null && hdncomplaintServiceId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
   

    var Status = true;
    if (chkStatus.prop("checked") == true)
    { productStatus = true; }
    else
    { productStatus = false; }


    var complaintServiceViewModel = {
        ComplaintId:hdncomplaintServiceId.val(),
        ComplaintDate: txtComplaintDate.val().trim(),
        InvoiceNo:txtInvoiceNo.val(),
        EnquiryType: ddlEnquiryType.val(),
        ComplaintMode: ddlComplaintMode.val(),
        CustomerMobile: txtCustomerMobile.val().trim(),
        CustomerName: txtCustomerName.val().trim(),
        CustomerEmail: txtCustomerEmail.val().trim(),
        CustomerAddress: txtCustomerAddress.val().trim(),
        ComplaintDescription: txtComplaintDescription.val().trim(),
        ComplaintService_Status: Status,
        BranchID: ddlCompanyBranch.val(),
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

        if (productId != undefined) {          
                var complaintProduct = {
                    ComplaintProductDetailID:complaintProductDetailID,                
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    MappingId:mappingId,
                    ProductName: productName,
                    ProductCode: productCode,
                    Remarks:remarks
                };
                complaintProductList.push(complaintProduct);
            }
        });

        var requestData = { complaintServiceViewModel: complaintServiceViewModel,complaintProducts: complaintProductList };
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
            $("#chkStatus").val(data.status);

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

function ExecuteSave() {

    setTimeout(
function () {
    SaveData();
}, 2000);
    SetGSTPercentageInProduct();
}