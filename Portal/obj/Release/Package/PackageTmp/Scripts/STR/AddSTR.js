$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtSTRNo").attr('readOnly', true);
    $("#txtSTRDate").attr('readOnly', true);
    $("#txtSTRDate").css('cursor', 'pointer');

    $("#txtSTNNo").attr('readOnly', true);
    $("#txtSTNDate").attr('readOnly', true);
    $("#txtDispatchRefDate").attr('readOnly', true);
    $("#txtGRDate").attr('readOnly', true);

    $("#txtLRDate").attr('readOnly', true);
    $("#txtRefDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtDiscountAmount").attr('readOnly', true);
    $("#txtTotalPrice").attr('readOnly', true);
    $("#txtBasicValue").attr('readOnly', true);
    $("#txtTaxPercentage").attr('readOnly', true);
    $("#txtTaxAmount").attr('readOnly', true);
    $("#txtTotalValue").attr('readOnly', true);

    BindDocumentTypeList();
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
           // CalculateTotalCharges();
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

    $("#txtChallanDate,#txtLRDate,#txtDispatchRefDate,#txtGRDate").datepicker({
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

    $("#txtSTRDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {

        }
    });
 var hdnAccessMode = $("#hdnAccessMode");
 var hdnstrId = $("#hdnstrId");
 if (hdnstrId.val() != "" && hdnstrId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetSTRDetail(hdnstrId.val());
           var sTNChasisProductSerialDetailViewModel = [];
           GetSTRSTNProductChasisNo(sTNChasisProductSerialDetailViewModel, hdnstrId.val(), 2);
       }, 2000);
        //var vendord = $("#hdnCustomerId").val();
    //    BindCustomerBranchList(customerId);
      

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

    var strProducts = [];
    GetSTRProductList(strProducts);
    var strDocuments = [];
    GetSTRDocumentList(strDocuments);

    BindFromWareBranchList(0);
    BindToWareBranchList(0);
    BindFromLocationListPopUp(0);
    BindToLocationListPopUp(0);
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

function BindFromWareBranchList(companyId) {
    $("#ddlFromWarehouse").val(0);
    $("#ddlFromWarehouse").html("");
    $.ajax({
        type: "GET",
        url: "../STN/GetCompanyBranchList",
        data: { companyId: companyId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlFromWarehouse").append($("<option></option>").val(0).html("-Select From Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlFromWarehouse").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
          
        },
        error: function (Result) {
            $("#ddlFromWarehouse").append($("<option></option>").val(0).html("-Select From Company Branch-"));
        }
    });
}
function BindToWareBranchList(companyId) {
    $("#ddlToWarehouse").val(0);
    $("#ddlToWarehouse").html("");
    $.ajax({
        type: "GET",
        url: "../STN/GetCompanyBranchList",
        data: { companyId: companyId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlToWarehouse").append($("<option></option>").val(0).html("-Select To Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlToWarehouse").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");
            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlToWarehouse").val(hdnSessionCompanyBranchId.val());
                $("#ddlToWarehouse").attr('disabled', true);
            }
           
        },
        error: function (Result) {
            $("#ddlToWarehouse").append($("<option></option>").val(0).html("-Select To Company Branch-"));
        }
    });
}


function RemoveDocumentRow(obj) {
    if (confirm("Do you want to remove selected Document?")) {
        var row = $(obj).closest("tr");
        var STRDocId = $(row).find("#hdnSTRDocId").val();
        ShowModel("Alert", "Document Removed from List.");
        row.remove();
    }
}

function GetSTRDocumentList(strDocuments) {
    var hdnstrId = $("#hdnstrId");
    var requestData = { strDocuments: strDocuments, strId: hdnstrId.val() };
    $.ajax({
        url: "../STR/GetSTRSupportingDocumentList",
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
        url: "../STR/SaveSupportingDocument",
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
                var hdnSTRDocId = $("#hdnSTRDocId");
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



                var strDocumentList = [];
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    docEntrySequence = 1;
                }
                $('#tblDocumentList tr').each(function (i, row) {
                    var $row = $(row);
                    var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
                    var hdnSTRDocId = $row.find("#hdnSTRDocId").val();
                    var documentTypeId = $row.find("#hdnDocumentTypeId").val();
                    var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
                    var documentName = $row.find("#hdnDocumentName").val();
                    var documentPath = $row.find("#hdnDocumentPath").val();

                    if (hdnSTRDocId != undefined) {
                        if ((hdnDocumentSequence.val() != documentSequenceNo)) {

                            var strDocument = {
                                STRDocId: hdnSTRDocId,
                                DocumentSequenceNo: documentSequenceNo,
                                DocumentTypeId: documentTypeId,
                                DocumentTypeDesc: documentTypeDesc,
                                DocumentName: documentName,
                                DocumentPath: documentPath
                            };
                            strDocumentList.push(strDocument);
                            docEntrySequence = parseInt(docEntrySequence) + 1;
                        }
                        else if (hdnSTRDocId.val() == sTNDocId && hdnDocumentSequence.val() == documentSequenceNo) {
                            var strDocument = {
                                STRDocId: hdnSTRDocId.val(),
                                DocumentSequenceNo: hdnDocumentSequence.val(),
                                DocumentTypeId: ddlDocumentType.val(),
                                DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                                DocumentName: newFileName,
                                DocumentPath: newFileName
                            };
                            strDocumentList.push(strDocument);
                        }
                    }
                });
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    hdnDocumentSequence.val(docEntrySequence);
                }

                var strDocumentAddEdit = {
                    STRDocId: hdnSTRDocId.val(),
                    DocumentSequenceNo: hdnDocumentSequence.val(),
                    DocumentTypeId: ddlDocumentType.val(),
                    DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                    DocumentName: newFileName,
                    DocumentPath: newFileName
                };
                strDocumentList.push(strDocumentAddEdit);
                hdnDocumentSequence.val("0");

                GetSTRDocumentList(strDocumentList);



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
        $("#hdnSTRDocId").val("0");
        $("#FileUpload1").val("");

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


function BindFromLocationListPopUp(companyId) {
    $("#ddlFromLocation").val(0);
    $("#ddlFromWarehouse").html("");
    $.ajax({
        type: "GET",
        url: "../STN/GetCompanyBranchList",
        data: { companyId: companyId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlFromLocation").append($("<option></option>").val(0).html("-Select From Location-"));
            $.each(data, function (i, item) {
                $("#ddlFromLocation").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlFromLocation").append($("<option></option>").val(0).html("-Select To Location-"));
        }
    });
}
function BindToLocationListPopUp(companyId) {
    $("#ddlToLocation").val(0);
    $("#ddlToLocation").html("");
    $.ajax({
        type: "GET",
        url: "../STN/GetCompanyBranchList",
        data: { companyId: companyId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlToLocation").append($("<option></option>").val(0).html("-Select To Location-"));
            $.each(data, function (i, item) {
                $("#ddlToLocation").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlToLocation").append($("<option></option>").val(0).html("-Select To Location-"));
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
function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtProductName = $("#txtProductName");
    var hdnSTRProductDetailId = $("#hdnSTRProductDetailId");
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

    var stnProductList = [];
    $('#tblSTNProductList tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var strProductDetailId = $row.find("#hdnSTRProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var totalprice = $row.find("#hdnTotalPrice").val();
        if (strProductDetailId != undefined) {
            if (action == 1 || hdnSTRProductDetailId.val() != strProductDetailId) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var stnProduct = {
                    SequenceNo: sequenceNo,
                    STRProductDetailId: strProductDetailId,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Price: price,
                    Quantity: quantity,
                    TotalPrice: totalprice
                    
                };
                stnProductList.push(stnProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnProductId.val() == productId && hdnSequenceNo.val() == sequenceNo) {
                var stnProductAddEdit = {
                    SequenceNo: hdnSequenceNo.val(),
                    STRProductDetailId: hdnSTRProductDetailId.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    Price: txtPrice.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    TotalPrice: txtTotalPrice.val()
                   

                };
                stnProductList.push(stnProductAddEdit);
                hdnSequenceNo.val("0");

            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }

    if (action == 1) {
        var stnProductAddEdit = {
            SequenceNo: hdnSequenceNo.val(),
            STRProductDetailId: hdnSTRProductDetailId.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            Price: txtPrice.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            TotalPrice: txtTotalPrice.val()
        };
        stnProductList.push(stnProductAddEdit);
        hdnSequenceNo.val("0");

    }

    if (flag == true) {
        GetSTRProductList(stnProductList);
    }
}
function GetSTRProductList(strProducts) {
    var hdnstrId = $("#hdnstrId");
    var requestData = { strProducts: strProducts, strId: hdnstrId.val() };
    $.ajax({
        url: "../STR/GetSTRProductList",
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


function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var strProductDetailId = $(row).find("#hdnSTRProductDetailId").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var price = $(row).find("#hdnPrice").val();
    var quantity = $(row).find("#hdnQuantity").val();
    var uomname = $(row).find("#hdnUOMName").val();
    var totalprice = $(row).find("#hdnTotalPrice").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();

    $("#txtProductName").val(productName);
    $("#hdnSTRProductDetailId").val(strProductDetailId);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtPrice").val(price);
    $("#txtQuantity").val(quantity);
    $("#txtUOMName").val(uomname);
    $("#txtTotalPrice").val(totalprice);    
    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    $("#hdnSequenceNo").val(sequenceNo);
    ShowHideProductPanel(1);
}

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var challanProductDetailId = $(row).find("#hdnChallanProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();

    }
}
function CalculateTotalCharges() {
    var price = $("#txtPrice").val();
    var quantity = $("#txtQuantity").val();
    price = price == "" ? 0 : price;
    quantity = quantity == "" ? 0 : quantity;
    var totalPrice = parseFloat(price) * parseFloat(quantity);
    $("#txtTotalPrice").val((totalPrice).toFixed(2));


}
function CalculateGrossandNetValues() {
    var basicValue = 0;
    var taxValue = 0;
    $('#tblSTNProductList tr').each(function (i, row) {
        var $row = $(row);
        var strProductDetailId = $row.find("#hdnSTRProductDetailId").val();
        var totalPrice = $row.find("#hdnTotalPrice").val();
        if (strProductDetailId != undefined) {
            basicValue += parseFloat(totalPrice);
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

    $("#txtBasicValue").val(basicValue.toFixed(2));

    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue) + parseFloat(freightValue) + parseFloat(loadingValue)).toFixed(2));
}

function SaveData() {
    var hdnstrId = $("#hdnstrId");
    var txtSTRNo = $("#txtSTRNo");
    var txtSTRDate = $("#txtSTRDate");
    var txtSTNNo = $("#txtSTNNo");
    var hdnSTNId = $("#hdnSTNId");
    var txtSTNDate = $("#txtSTNDate");
    var txtGRNo = $("#txtGRNo");
    var txtGRDate = $("#txtGRDate");

    var ddlFromWarehouse = $("#ddlFromWarehouse");
    var ddlToWarehouse = $("#ddlToWarehouse");
    var txtSContactPerson = $("#txtSContactPerson");
  
    var txtDispatchRefNo = $("#txtDispatchRefNo");
    var txtDispatchRefDate = $("#txtDispatchRefDate"); 
    var txtLRNo = $("#txtLRNo");
    var txtLRDate = $("#txtLRDate");
    var txtTransportVia = $("#txtTransportVia");
    var txtNoOfPackets = $("#txtNoOfPackets");

    var txtBasicValue = $("#txtBasicValue");
    var txtFreightValue = $("#txtFreightValue");
    var txtLoadingValue = $("#txtLoadingValue");
    var txtTotalValue = $("#txtTotalValue");

    var txtRemarks1 = $("#txtRemarks1");
    var txtRemarks2 = $("#txtRemarks2");


    var ddlApprovalStatus = $("#ddlApprovalStatus");

    if (ddlFromWarehouse.val().trim() == "" || ddlFromWarehouse.val().trim() == "0")
    {
        ShowModel("Alert", "Please Select From Loacton")
        ddlFromWarehouse.focus();
        return false;
    }
    if (ddlToWarehouse.val().trim() == "" || ddlToWarehouse.val().trim() == "0") {
        ShowModel("Alert", "Please Select To Loacton")
        ddlToWarehouse.focus();
        return false;
    }
    if (ddlFromWarehouse.val() == ddlToWarehouse.val())
    {
        ShowModel("Alert", "From Loacton and TO Location should not Same")
        ddlFromWarehouse.focus();
        return false;

    }
   



   

    var STRViewModel = {
        STRId: hdnstrId.val(),
        STRNo: txtSTRNo.val().trim(),
        STRDate: txtSTRDate.val().trim(),
        STNId:hdnSTNId.val(),
        STNNo: txtSTNNo.val().trim(),
        STNDate:txtSTNDate.val().trim(),
        GrNo: txtGRNo.val().trim(),
        GrDate:txtGRDate.val().trim(),
        ContactPerson: txtSContactPerson.val().trim(),
        FromWarehouseId: ddlFromWarehouse.val(),
        ToWarehouseId:ddlToWarehouse.val(),
        DispatchRefNo: txtDispatchRefNo.val().trim(),
        DispatchRefDate: txtDispatchRefDate.val(),
        LRNo: txtLRNo.val().trim(),
        LRDate: txtLRDate.val(),
        TransportVia: txtTransportVia.val().trim(),
        NoOfPackets: txtNoOfPackets.val(),
        BasicValue: txtBasicValue.val(),
        TotalValue: txtTotalValue.val(),
        FreightValue: txtFreightValue.val(),
        LoadingValue:txtLoadingValue.val(),
        Remarks1: txtRemarks1.val(),
        Remarks2: txtRemarks2.val(),
        ApprovalStatus: ddlApprovalStatus.val()
    };

    var strProductList = [];
    $('#tblSTNProductList tr').each(function (i, row) {
        var $row = $(row); 
        var strProductDetailId = $row.find("#hdnSTRProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var total = $row.find("#hdnTotalPrice").val();
        if (strProductDetailId != undefined) {

            var stnProduct = {
                STRProductDetailId: strProductDetailId,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc,
                UOMName: uomName,
                Price: price,
                Quantity: quantity,
                TotalPrice:total
            };
            strProductList.push(stnProduct);
        }
    });
    if (strProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product.")
        return false;
    }


    var strDocumentList = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var sTRDocId = $row.find("#hdnSTRDocId").val();
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        if (sTRDocId != undefined) {
            var strDocument = {
                STRDocId: sTRDocId,
                DocumentSequenceNo: documentSequenceNo,
                DocumentTypeId: documentTypeId,
                DocumentTypeDesc: documentTypeDesc,
                DocumentName: documentName,
                DocumentPath: documentPath
            };
            strDocumentList.push(strDocument);
        }

    });


    var strProductChasisList = [];
    $('#tblStnAllProductChasisList tr').each(function (i, row) {
        var $row = $(row);
      
        var productId = $row.find("#hdnProductIdAll").val();
        var chasisNo = $row.find("#hdnChasisNoAll").val();

        if (productId != undefined) {

            var stnProductChasis = {
                ProductId: productId,               
                ChasisNo: chasisNo
            };
            strProductChasisList.push(stnProductChasis);
        }
    });
    

    var flagStatus = true;
    $('#tblSTNProductList tr:not(:has(th))').each(function (i, row) {
        var count = 0;
        var $row = $(row);
        var checkproductId = 0;
        var productId = $row.find("#hdnProductId").val();
        var quantity = $row.find("#hdnQuantity").val();
        if (productId != undefined) {
           
            $('#tblStnAllProductChasisList tr:not(:has(th))').each(function (i, row) {
                    var $row = $(row);
                    var productIdAll = $row.find("#hdnProductIdAll").val();
                    if (productIdAll != undefined) {
                        if (productId == productIdAll) {
                            count = count + 1;
                            checkproductId = productIdAll;
                        }
                    }

                   

                });
               
            if (count != quantity && checkproductId == productId) {
                flagStatus = false;
               
            }
        }

       
    });
    if (flagStatus == false) {
        ShowModel("Alert", "Number of Chassis serial No selected is not equal to the Product Quantity")
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdnstrId.val() != null && hdnstrId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var accessMode = 1;//Add Mode
    if (hdnstrId.val() != null && hdnstrId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = {
        strViewModel: STRViewModel,
        strProductDetailViewModel: strProductList,
        strDocuments: strDocumentList,
        strProductChasisList: strProductChasisList
    };
    $.ajax({
        url: "../STR/AddEditSTR?AccessMode=" + accessMode + "",
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
                       window.location.href = "../STR/AddEditSTR?strId=" + data.trnId + "&AccessMode=3";
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

    $("#txtSTRNo").val("");
    $("#hdnstrId").val("0");
    $("#txtSTNDate").val($("#hdnCurrentDate").val());
    $("#ddlFromWarehouse").val("0");
    $("#ddlToWarehouse").val("0");
    $("#txtVendorCode").val("");
    $("#txtSTNNo").val("");
    $("#hdnSTNId").val("0");
    
    $("#ddlApprovalStatus").val("Draft");

    $("#txtSContactPerson").val("");
    $("#txtDispatchRefNo").val("");
    $("#txtDispatchRefDate").val(""); 
    $("#txtLRNo").val("");
    $("#txtLRDate").val("");
    $("#txtTransportVia").val("");
    $("#txtNoOfPackets").val("");
    $("#txtRemarks1").val("");
    $("#txtRemarks2").val("");
    $("#txtBasicValue").val("");
    $("#txtFreightValue").val("");
    $("#txtLoadingValue").val("");
    $("#txtTotalValue").val("");
    $("#txtRemarks1").val("");
    $("#txtRemarks2").val("");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
function ShowHideProductPanel(action) {
    if (action == 1) {
        $(".productsection").show();
    }
    else {
        $(".productsection").hide();
        $("#txtProductName").val("");
        $("#hdnProductId").val("0");
        $("#hdnSTRProductDetailId").val("0");
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtPrice").val("");
        $("#txtQuantity").val("");
        $("#txtUOMName").val("");
        $("#txtTotalPrice").val("");
        $("#btnAddProduct").show(); 
        $("#btnUpdateProduct").hide();
    }
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

function OpenInvoiceSearchPopup() {
    var ddlFromWarehouse = $("#ddlFromWarehouse");
    var ddlToWarehouse = $("#ddlToWarehouse");
    var hdnSessionUserID = $("#hdnSessionUserID");
    if (ddlFromWarehouse.val() == "" || ddlFromWarehouse.val() == "0") {
        ShowModel("Alert", "Please select Form Company Branch")
        return false;
    }
    if (hdnSessionUserID.val() == "2")
    {
        if (ddlToWarehouse.val() == "" || ddlToWarehouse.val() == "0") {
            ShowModel("Alert", "Please select To Company Branch")
            return false;
        }
    }

        
   
 

    

    
   
    
    $("#SearchInvoiceModel").modal();
   
}

function SearchSTN() {
    var txtSTNNo = $("#txtSearchSTNNo");
    var txtSearchGRNo = $("#txtSearchGRNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var ddlFromLocation = $("#ddlFromWarehouse");
    var ddlToLocation = $("#ddlToWarehouse");

    var requestData = { stnNo: txtSTNNo.val().trim(), glNo: txtSearchGRNo.val().trim(), fromLocation: ddlFromLocation.val(), toLocation: ddlToLocation.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../STR/GetSTNList",
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
function SelectSTN(stnId, stnNo, stnDate,grNo, fromLocation,toLocation,totalValue) {
    $("#txtSTNNo").val(stnNo);
    $("#hdnSTNId").val(stnId);
    $("#txtSTNDate").val(stnDate);
    $("#txtGRNo").val(grNo);
    $("#ddlFromWarehouse").val(fromLocation);
    $("#ddlToWarehouse").val(toLocation);
    $("#txtTotalValue").val(totalValue);
    $("#SearchInvoiceModel").modal('hide');

    var stnProductList = [];
    GetSTNProductList(stnProductList, stnId);


    var sTNChasisProductSerialDetailViewModel = [];
    GetSTRSTNProductChasisNo(sTNChasisProductSerialDetailViewModel, stnId,1);

   // GetVendorDetail(vendorId);
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

function GetSTRDetail(strId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../STR/GetSTRDetail",
        data: { strId: strId },
        dataType: "json",
        success: function (data) {
            $("#txtSTRNo").val(data.STRNo);
            $("#txtSTRDate").val(data.STRDate);
            $("#txtSTNNo").val(data.STNNo),
            $("#hdnSTNId").val(data.STNId),
            $("#txtSTNDate").val(data.STNDate),
            $("#txtGRNo").val(data.GRNo),
            $("#txtGRDate").val(data.GRDate),
          
            $("#ddlFromWarehouse").val(data.FromWarehouseId);
          
            $("#ddlToWarehouse").val(data.ToWarehouseId);
            $("#txtDispatchRefNo").val(data.DispatchRefNo);
            $("#txtDispatchRefDate").val(data.DispatchRefDate);
            $("#txtLRNo").val(data.LRNo);
            $("#txtLRDate").val(data.LRDate);
            $("#txtTransportVia").val(data.TransportVia);
            $("#txtNoOfPackets").val(data.NoOfPackets);

            $("#txtFreightValue").val(data.FreightValue);
            $("#txtLoadingValue").val(data.LoadingValue);
            $("#txtTotalValue").val(data.TotalValue);

            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);

            $("#ddlApprovalStatus").val(data.ApprovalStatus);


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


        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });


}

function GetSTNProductList(stnProducts,stnId) {
    
    var requestData = { stnProducts: stnProducts, stnId: stnId };
    $.ajax({
        url: "../STR/GetSTRSTNProductList",
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

function GetSTRSTNProductChasisNo(sTNChasisProductSerialDetailViewModel, stnId, mode) {

    var requestData = {
        sTNChasisProductSerialDetailViewModel: sTNChasisProductSerialDetailViewModel,
        stnId: stnId,
        mode: mode
    };
    $.ajax({
        url: "../STR/GetSTRSTNProductChasisNo",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divProductChasisList").html("");
            $("#divProductChasisList").html(err);
        },
        success: function (data) {
            $("#divProductChasisList").html("");
            $("#divProductChasisList").html(data);
        }
    });
}

function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../STR/AddEditSTR";
    }
}

///////*************For Only Allow Decimal Values For Two Digits Values******************///////

////////**************Created By Dheeraj**********////////////////////

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

//////*************************End Code****************************/////////////////////////

///***In Work Order Sale Order Functionlatity*****Discussion By HARI Sir Date==16-01-2018**//////////////////////