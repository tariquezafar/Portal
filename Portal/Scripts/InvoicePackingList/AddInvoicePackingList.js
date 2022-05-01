$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtInvoicePackingListNo").attr('readOnly', true);
    $("#txtInvoicePackingListDate").attr('readOnly', true);
    $("#txtInvoiceNo").attr('readOnly', true);
    $("#txtInvoiceDate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtCustomerName").attr('readOnly', true);
    $("#txtCustomerCode").attr('readOnly', true);   
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtAvailableStock").attr('readOnly', true);
    BindPackingListType();
    BindCompanyBranchList();
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
                        return {
                            label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc,
                            code: item.ProductCode, IsWarrantyProduct: item.IsWarrantyProduct, WarrantyInMonth: item.WarrantyInMonth
                        };
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
            //$("#txtProductShortDesc").val(ui.item.desc);
            $("#txtProductCode").val(ui.item.code);
            $("#hdnIsWarrantyProduct").val(ui.item.IsWarrantyProduct);
            $("#hdnWarrantyInMonth").val(ui.item.WarrantyInMonth);
            
            GetProductDetail(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductName").val("");
                $("#hdnProductId").val("0");
                //$("#txtProductShortDesc").val("");
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

$("#txtInvoicePackingListDate").datepicker({
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

    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnInvoicePackingListId = $("#hdnInvoicePackingListId");
    if (hdnInvoicePackingListId.val() != "" && hdnInvoicePackingListId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {

           $("#ddlCompanyBranch").attr('disabled', true);
           GetInvoicePackingListDetail(hdnInvoicePackingListId.val());
       }, 1000);
       if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $(".editonly").hide();
            $("#ddlPrintOption").attr('disabled', false);
        }
       else {
           $("#ddlPackingListType").attr('disabled', true);
           $("#btnSearchInvoice").hide();
           $("#btnSearchInvoiceDisabled").show();
           
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
    }

    var invoicePackingListProducts = [];
    GetInvoicePackingListProductList(invoicePackingListProducts);
    

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
function BindPackingListType() {

    $("#ddlPackingListType").val(0);
    $("#ddlPackingListType").html("");
    $.ajax({
        type: "GET",
        url: "../PackingList/GetAllPackingListType",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPackingListType").append($("<option></option>").val(0).html("-Select Packing Type-"));
            $.each(data, function (i, item) {
                $("#ddlPackingListType").append($("<option></option>").val(item.PackingListTypeID).html(item.PackingListTypeName));
            });
        },
        error: function (Result) {
            $("#ddlPackingListType").append($("<option></option>").val(0).html("-Select Packing Type-"));
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
function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");

    var txtProductName = $("#txtProductName");
    var hdnInvoicePackingListProductDetailId = $("#hdnInvoicePackingListProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var txtAvailableStock = $("#txtAvailableStock");
    
    var ddlPackingProductType = $("#ddlPackingProductType");
    var hdnIsWarrantyProduct = $("#hdnIsWarrantyProduct");
    var hdnWarrantyInMonth = $("#hdnWarrantyInMonth");

    if (txtProductName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Product Name")
        return false;
    }
    if (hdnProductId.val().trim() == "" || hdnProductId.val().trim() == "0") {
        ShowModel("Alert", "Please select Product from list")
        return false;
    }
    if (txtQuantity.val().trim() == "" || txtQuantity.val().trim() == "0" || parseFloat(txtQuantity.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter Quantity")
        txtQuantity.focus();
        return false;
    }
    if ((ddlPackingProductType.val().toUpperCase() == "COMPLIMENTARY" || ddlPackingProductType.val().toUpperCase() == "BILLED" || ddlPackingProductType.val().toUpperCase() == "REPLACEMENT" ) && parseFloat(txtQuantity.val().trim()) > parseFloat(txtAvailableStock.val().trim())) {
        ShowModel("Alert", "Challan Quantity cannot be greater than Available Stock Quantity")
        return false;
    }
    
    var invoicePackingListProducts = [];
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var invoicePackingListProductDetailId = $row.find("#hdnInvoicePackingListProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnQuantity").val();
        var packingProductType = $row.find("#hdnPackingProductType").val();

        var isWarrantyProduct = $row.find("#hdnIsWarrantyProduct").val();
        var warrantyInMonth = $row.find("#hdnWarrantyInMonth").val();


        if (productName != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val() && packingProductType==ddlPackingProductType.val()) {
                    ShowModel("Alert", "Product already added with same packing product type!!!")
                    flag = false;
                    return false;
                }

                var invoicePackingListProduct = {
                    InvoicePackingListProductDetailId: invoicePackingListProductDetailId,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity,
                    PackingProductType: packingProductType,
                    IsWarrantyProduct:isWarrantyProduct,
                    warrantyInMonth:warrantyInMonth
                };
                invoicePackingListProducts.push(invoicePackingListProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnProductId.val() == productId && hdnSequenceNo.val() == sequenceNo) {
                var invoicePackingListProduct = {
                    InvoicePackingListProductDetailId: hdnInvoicePackingListProductDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    PackingProductType: ddlPackingProductType.val(),
                    IsWarrantyProduct: hdnIsWarrantyProduct.val(),
                    warrantyInMonth: hdnWarrantyInMonth.val()
                };
                invoicePackingListProducts.push(invoicePackingListProduct);
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var invoicePackingListProductAddEdit = {
            InvoicePackingListProductDetailId: hdnInvoicePackingListProductDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            PackingProductType: ddlPackingProductType.val(),
            IsWarrantyProduct: hdnIsWarrantyProduct.val(),
            warrantyInMonth: hdnWarrantyInMonth.val()
        };

        invoicePackingListProducts.push(invoicePackingListProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true)
        {
        GetInvoicePackingListProductList(invoicePackingListProducts);
    }

}
function GetInvoicePackingListProductList(invoicePackingListProducts) {
    var hdnInvoicePackingListId = $("#hdnInvoicePackingListId");
    var requestData = { invoicePackingListProducts: invoicePackingListProducts, invoicePackingListId: hdnInvoicePackingListId.val() };
    $.ajax({
        url: "../InvoicePackingList/GetInvoicePackingListProductList",
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
function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var invoicePackingListProductDetailId = $(row).find("#hdnInvoicePackingListProductDetailId").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var quantity = $(row).find("#hdnQuantity").val();
    var packingProductType = $(row).find("#hdnPackingProductType").val();

    var IsWarrantyProduct = $(row).find("#hdnIsWarrantyProduct").val();
    var WarrantyInMonth = $(row).find("#hdnWarrantyInMonth").val();


    $("#txtProductName").val(productName);
    $("#hdnInvoicePackingListProductDetailId").val(invoicePackingListProductDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);
    $("#txtQuantity").val(quantity);
    $("#ddlPackingProductType").val(packingProductType);

    $("#hdnIsWarrantyProduct").val(IsWarrantyProduct);
    $("#hdnWarrantyInMonth").val(WarrantyInMonth);

    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    GetProductAvailableStock(productId);
    ShowHideProductPanel(1);
}

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var invoicePackingListProductDetailId = $(row).find("#hdnInvoicePackingListProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
    }
}

function GetInvoicePackingListDetail(invoicePackingListId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../InvoicePackingList/GetInvoicePackingListDetail",
        data: { invoicePackingListId: invoicePackingListId },
        dataType: "json",
        success: function (data) {
            $("#txtInvoicePackingListNo").val(data.InvoicePackingListNo);
            $("#txtInvoicePackingListDate").val(data.InvoicePackingListDate);
            $("#txtInvoiceNo").val(data.InvoiceNo);
            $("#hdnInvoiceId").val(data.InvoiceId);
            $("#txtInvoiceDate").val(data.InvoiceDate);
            
            $("#hdnCustomerId").val(data.CustomerId);
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtCustomerName").val(data.CustomerName);
            $("#ddlPackingListType").val(data.PackingListTypeID);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtRemarks").val(data.Remarks);

            $("#ddlApprovalStatus").val(data.InvoicePackingListStatus)
    
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }

        
         
            $("#btnAddNew").show();
            $("#btnPrint").show(); 
            $("#btnGenerateCheckListPrint").show();


        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
           
    
}
function SaveData() {
    var hdnInvoicePackingListId = $("#hdnInvoicePackingListId");
    var txtInvoicePackingListDate = $("#txtInvoicePackingListDate");
    var hdnInvoiceId = $("#hdnInvoiceId");
    var txtInvoiceNo = $("#txtInvoiceNo");
    var hdnCustomerId = $("#hdnCustomerId");
    var txtCustomerName = $("#txtCustomerName");
    var ddlPackingListType = $("#ddlPackingListType");
    var txtRemarks = $("#txtRemarks");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    
    
    
    if (txtInvoiceNo.val().trim() == "") {
        ShowModel("Alert", "Please select Invoice No")
        return false;
    }
    if (hdnInvoiceId.val() == "" || hdnInvoiceId.val() == "0") {
           ShowModel("Alert", "Please select invoice No")
           return false;
     }

    
    if (ddlPackingListType.val() == "" || ddlPackingListType.val() == "0") {
        ShowModel("Alert", "Please select Packing List Type")
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch.")
        ddlCompanyBranch.focus();
        return false;
    }
    
    var accessMode = 1;//Add Mode
    if (hdnInvoicePackingListId.val() != null && hdnInvoicePackingListId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var invoicePackingListViewModel = {
        InvoicePackingListId: hdnInvoicePackingListId.val(),
        InvoicePackingListDate: txtInvoicePackingListDate.val().trim(),
        InvoiceId: hdnInvoiceId.val().trim(),
        InvoiceNo: txtInvoiceNo.val().trim(),
        PackingListTypeID:ddlPackingListType.val(),
        Remarks: txtRemarks.val().trim(),
        InvoicePackingListStatus: ddlApprovalStatus.val(),
        CompanyBranchId:ddlCompanyBranch.val()
    };
    var productCountStatus = false;
    var invoicePackingListProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var invoicePackingListProductDetailId = $row.find("#hdnInvoicePackingListProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnQuantity").val();
        
        var packingProductType = $row.find("#hdnPackingProductType").val();
        var hdnIsWarrantyProduct = $row.find("#hdnIsWarrantyProduct").val();
        var hdnWarrantyInMonth = $row.find("#hdnWarrantyInMonth").val();

        if (productId != undefined) {

            var invoicePackingListProduct = {
                InvoicePackingListProductDetailId: invoicePackingListProductDetailId,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc,
                UOMName: uomName,
                Quantity: quantity,
                PackingProductType: packingProductType,
                IsWarrantyProduct: hdnIsWarrantyProduct,
                WarrantyInMonth: hdnWarrantyInMonth
            };
            invoicePackingListProductList.push(invoicePackingListProduct);
            productCountStatus = true;
        }
    });

    if (productCountStatus == false)
    {
        ShowModel("Alert", "At least single Product should be selected!!!");
        return false;
    }

 

  

    var requestData = { invoicePackingListViewModel: invoicePackingListViewModel, invoicePackingListProducts: invoicePackingListProductList };
    $.ajax({
        url: "../InvoicePackingList/AddEditInvoicePackingList?AccessMode=" + accessMode + "",
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
                       window.location.href = "../InvoicePackingList/AddEditInvoicePackingList?invoicePackingListId=" + data.trnId + "&AccessMode=3";
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

    $("#txtInvoicePackingListNo").val("");
    $("#hdnInvoicePackingListId").val("0");
    $("#txtInvoicePackingListDate").val($("#hdnCurrentDate").val());
    $("#hdnCustomerId").val("0");
    $("#txtCustomerName").val("");
    $("#txtCustomerCode").val("");
    $("#txtInvoiceNo").val("");
    $("#txtInvoiceDate").val("");
    $("#txtInvoiceId").val("0"); 
    $("#ddlApprovalStatus").val("Draft");
    $("#txtRemarks").val("");
    $("#ddlPackingListType").val("0");

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
        data: { productid: productId, companyBranchId: 0 , trnId: 0 , trnType: "DC" },
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
        if ($("#ddlPackingListType").val() == "0") {
            ShowModel("Alert", "Please  Select Packing List Type");
            return false;
        }
        else {
            $(".productsection").show();
        }
    }
    else {
        $(".productsection").hide();
        $("#txtProductName").val("");
        $("#hdnProductId").val("0");
        $("#hdnInvoicePackingListProductDetailId").val("0");
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtAvailableStock").val("");
        $("#txtUOMName").val("");
        $("#txtQuantity").val("");
        $("#ddlPackingProductType").val("Parts");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
        
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
        url: "../InvoicePackingList/GetPLSaleInvoiceList",
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

    $("#ddlCompanyBranch").attr('disabled', true);
    $("#SearchInvoiceModel").modal('hide');
    
    GetSaleInvoiceProductList($("#hdnInvoiceId").val(), $("#ddlPackingListType").val());
    
}
function BindPackingListProducts()
{
    GetSaleInvoiceProductList($("#hdnInvoiceId").val(), $("#ddlPackingListType").val());
}
 
function GetSaleInvoiceProductList(invoiceId, packingListTypeId) {
    if (invoiceId == undefined || invoiceId == "" || invoiceId == "0")
    {
        invoiceId = 0;
    }
    if (packingListTypeId == undefined || packingListTypeId == "" || packingListTypeId == "0") {
        packingListTypeId = 0;
    }
    

    var requestData = { invoiceId: invoiceId, packingListTypeId: packingListTypeId };
    $.ajax({
        url: "../InvoicePackingList/GetPackingListTypeProductList",
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
function OpenPrintPopup() {
    $("#printModel").modal();
    GenerateReportParameters();
}
function ShowHidePrintOption() {
    var reportOption = $("#ddlPrintOption").val();
    if (reportOption == "PDF") {
        $("#btnPdf").show();
        $("#btnExcel").hide();
    }
    else if (reportOption == "Excel") {
        $("#btnExcel").show();
        $("#btnPdf").hide();
    }
}
function GenerateReportParameters() {

    var url = "../InvoicePackingList/CheckListReport?invoiceID=" + $("#hdnInvoiceId").val() + "&packingListTypeid=" + $("#ddlPackingListType").val() + "&invoicepackingListId=" + $("#hdnInvoicePackingListId").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    var url = "../InvoicePackingList/CheckListReport?invoiceID=" + $("#hdnInvoiceId").val() + "&packingListTypeid=" + $("#ddlPackingListType").val() + "&invoicepackingListId=" + $("#hdnInvoicePackingListId").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

}

function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../InvoicePackingList/AddEditInvoicePackingList";
    }
}