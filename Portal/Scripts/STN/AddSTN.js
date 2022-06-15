$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    BindFromWareBranchList();
    BindToWareBranchList(0);
    $("#txtSTNDate").css('cursor', 'pointer');

    $("#txtDispatchRefDate").attr('readOnly', true);
    $("#txtSTNNo").attr('readOnly', true);
    $("#txtSTNDate").attr('readOnly', true);
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
    $("#txtGRDate").attr('readOnly', true);
    $("#txtBranchStock").attr('readOnly', true);
    BindDocumentTypeList();



    $("#txtProductNameSearch").autocomplete({
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
            $("#txtProductNameSearch").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtProductNameSearch").val(ui.item.label);
            $("#hdnProductIdSerach").val(ui.item.value);

            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductNameSearch").val("");
                $("#hdnProductIdSerach").val("0");
                ShowModel("Alert", "Please select Product from List")

            }
            return false;
        }

    })

    ////////////





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
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode, IsSerializedProduct: item.IsSerializedProduct };
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
            $("#hdnSTNIsSerializedProduct").val(ui.item.IsSerializedProduct)
            $("#txtQuantity").val("");
            var hdnSTNIsSerializedProduct = $("#hdnSTNIsSerializedProduct");

            if (ui.item.IsSerializedProduct == true) {
                $("#ChasisDetail").show();
            }
            else {
                $("#ChasisDetail").hide();
            }

            GetProductDetail(ui.item.value);
            GetProductAvailableStockBranchWise(ui.item.value);


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

    $("#txtChallanDate,#txtLRDate,#txtDispatchRefDate,#txtSTNDate,#txtGRDate").datepicker({
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
    var hdnstnId = $("#hdnstnId");
    if (hdnstnId.val() != "" && hdnstnId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetSTNDetail(hdnstnId.val());

       }, 2000);
        //var vendord = $("#hdnCustomerId").val();
        //    BindCustomerBranchList(customerId);
        $("#ddlFromWarehouse").attr('disabled', true);

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $("#btnAddNewProduct").hide();
            $(".editonly").hide();

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

    var stnProducts = [];
    GetSTNProductList(stnProducts);

    var stnDocuments = [];
    GetSTNDocumentList(stnDocuments);
    GetProductChasisSerialNo();


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

function BindFromWareBranchList() {
    $("#ddlFromWarehouse").val(0);
    $("#ddlFromWarehouse").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlFromWarehouse").append($("<option></option>").val(0).html("Select Company Branch"));
            $.each(data, function (i, item) {
                $("#ddlFromWarehouse").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");

            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlFromWarehouse").val(hdnSessionCompanyBranchId.val());
                $("#ddlFromWarehouse").attr('disabled', true);
            }
        },
        error: function (Result) {
            $("#ddlFromWarehouse").append($("<option></option>").val(0).html("Select Company Branch"));
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
            $("#ddlToWarehouse").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlToWarehouse").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });


        },
        error: function (Result) {
            $("#ddlToWarehouse").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}

function RemoveDocumentRow(obj) {
    if (confirm("Do you want to remove selected Document?")) {
        var row = $(obj).closest("tr");
        var STNDocId = $(row).find("#hdnSTNDocId").val();
        ShowModel("Alert", "Document Removed from List.");
        row.remove();
    }
}

function GetSTNDocumentList(stnDocuments) {
    var hdnstnId = $("#hdnstnId");
    var requestData = { stnDocuments: stnDocuments, stnId: hdnstnId.val() };
    $.ajax({
        url: "../STN/GetSTNSupportingDocumentList",
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
        url: "../STN/SaveSupportingDocument",
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
                var hdnSTNDocId = $("#hdnSTNDocId");
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



                var stnDocumentList = [];
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    docEntrySequence = 1;
                }
                $('#tblDocumentList tr').each(function (i, row) {
                    var $row = $(row);
                    var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
                    var hdnSTNDocId = $row.find("#hdnSTNDocId").val();
                    var documentTypeId = $row.find("#hdnDocumentTypeId").val();
                    var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
                    var documentName = $row.find("#hdnDocumentName").val();
                    var documentPath = $row.find("#hdnDocumentPath").val();

                    if (hdnSTNDocId != undefined) {
                        if ((hdnDocumentSequence.val() != documentSequenceNo)) {

                            var stnDocument = {
                                STNDocId: hdnSTNDocId,
                                DocumentSequenceNo: documentSequenceNo,
                                DocumentTypeId: documentTypeId,
                                DocumentTypeDesc: documentTypeDesc,
                                DocumentName: documentName,
                                DocumentPath: documentPath
                            };
                            stnDocumentList.push(stnDocument);
                            docEntrySequence = parseInt(docEntrySequence) + 1;
                        }
                        else if (hdnSTNDocId.val() == sTNDocId && hdnDocumentSequence.val() == documentSequenceNo) {
                            var stnDocument = {
                                STNDocId: hdnSTNDocId.val(),
                                DocumentSequenceNo: hdnDocumentSequence.val(),
                                DocumentTypeId: ddlDocumentType.val(),
                                DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                                DocumentName: newFileName,
                                DocumentPath: newFileName
                            };
                            stnDocumentList.push(stnDocument);
                        }
                    }
                });
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    hdnDocumentSequence.val(docEntrySequence);
                }

                var stnDocumentAddEdit = {
                    STNDocId: hdnSTNDocId.val(),
                    DocumentSequenceNo: hdnDocumentSequence.val(),
                    DocumentTypeId: ddlDocumentType.val(),
                    DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                    DocumentName: newFileName,
                    DocumentPath: newFileName
                };
                stnDocumentList.push(stnDocumentAddEdit);
                hdnDocumentSequence.val("0");

                GetSTNDocumentList(stnDocumentList);



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
        $("#hdnSTNDocId").val("0");
        $("#FileUpload1").val("");

        $("#btnAddDocument").show();
        $("#btnUpdateDocument").hide();
    }
}

function BindDocumentTypeList() {
    $.ajax({
        type: "GET",
        url: "../PO/GetModuleDocumentTypeList",
        data: { employeeDoc: "Inventory" },
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

    $("#ddlFromWarehouse").attr('readOnly', true);


    var flag = true;
    var productEntrySequence = 0;
    var Countchasisno = 0;
    var hdnSTNIsSerializedProduct = $("#hdnSTNIsSerializedProduct");
    var hdnProductId = $("#hdnProductId");
    var txtQuantity = $("#txtQuantity");


    if (hdnSTNIsSerializedProduct.val() == 'true' || hdnSTNIsSerializedProduct.val() == 'True') {
        $('#tblStnAllProductChasisList tr').each(function (i, row) {
            var $row = $(row);
            var hdnProductIdAll = $row.find("#hdnProductIdAll").val();
            var hdnChasisNoAll = $row.find("#hdnChasisNoAll").val();
            if (hdnProductIdAll != undefined) {

                if (hdnProductIdAll == hdnProductId.val()) {
                    Countchasisno++;


                }

            }
        });


        if (Countchasisno != txtQuantity.val()) {
            ShowModel("Alert", "Chasis No. not equal to product quantity")
            return false;
        }


    }




    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtProductName = $("#txtProductName");
    var hdnSTNProductDetailId = $("#hdnSTNProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtPrice = $("#txtPrice");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var txtTotalPrice = $("#txtTotalPrice");
    var hdnSTNIsSerializedProduct = $("#hdnSTNIsSerializedProduct");
    var txtBranchStock = $("#txtBranchStock");
    if (txtProductName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Product Name")
        txtProductName.focus();
        return false;
    }

    if (txtQuantity.val().trim() == "") {
        ShowModel("Alert", "Please Enter Product Quantity")
        txtQuantity.focus();
        return false;
    }

    if (hdnProductId.val().trim() == "" || hdnProductId.val().trim() == "0") {
        ShowModel("Alert", "Please select Product from list")
        hdnProductId.focus();
        return false;
    }
    //if (txtPrice.val().trim() == "" || txtPrice.val().trim() == "0" || parseFloat(txtPrice.val().trim()) <= 0) {
    //    ShowModel("Alert", "Please enter Price")
    //    txtPrice.focus();
    //    return false;
    //}


    if (parseFloat(txtQuantity.val().trim()) > parseFloat(txtBranchStock.val().trim())) {
        ShowModel("Alert", "Product Quantity cannot be greater than Branch Stock Quantity")
        txtQuantity.focus();
        return false;
    }



    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    if (txtQuantity.val().trim() == "" || txtQuantity.val().trim() == "0" || parseFloat(txtQuantity.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter Quantity")
        txtQuantity.focus();
        return false;
    }

    var stnProductList = [];
    $('#tblSTNProductList tr').each(function (i, row) {

        var $row = $(row);
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var stnProductDetailId = $row.find("#hdnSTNProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var totalprice = $row.find("#hdnTotalPrice").val();
        var hdnIsSerializedProduct = $row.find("#hdnIsSerializedProduct").val();
        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!");
                    txtProductName.focus();
                    flag = false;
                    return false;
                }


                var stnProduct = {
                    SequenceNo: sequenceNo,
                    STNProductDetailId: stnProductDetailId,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Price: price,
                    Quantity: quantity,
                    TotalPrice: totalprice,
                    IsSerializedProduct: hdnIsSerializedProduct

                };
                stnProductList.push(stnProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }




            else if (hdnProductId.val() == productId && hdnSequenceNo.val() == sequenceNo) {
                var stnProductAddEdit = {
                    SequenceNo: hdnSequenceNo.val(),
                    STNProductDetailId: hdnSTNProductDetailId.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    Price: txtPrice.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    TotalPrice: txtTotalPrice.val(),
                    IsSerializedProduct: hdnSTNIsSerializedProduct.val()

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
            STNProductDetailId: hdnSTNProductDetailId.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            Price: txtPrice.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            TotalPrice: txtTotalPrice.val(),
            IsSerializedProduct: hdnSTNIsSerializedProduct.val()
        };
        stnProductList.push(stnProductAddEdit);
        hdnSequenceNo.val("0");

    }

    if (flag == true) {
        GetSTNProductList(stnProductList);
    }

}
function GetSTNProductList(stnProducts) {
    var hdnstnId = $("#hdnstnId");
    var requestData = { stnProducts: stnProducts, stnId: hdnstnId.val() };
    $.ajax({
        url: "../STN/GetSTNProductList",
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
    var stnProductDetailId = $(row).find("#hdnSTNProductDetailId").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var price = $(row).find("#hdnPrice").val();
    var quantity = $(row).find("#hdnQuantity").val();
    var uomname = $(row).find("#hdnUOMName").val();
    var totalprice = $(row).find("#hdnTotalPrice").val();
    var isSerializedProduct = $(row).find("#hdnIsSerializedProduct").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var editonly = $(row).find(".editonly");



    // $(row).find(".editonly").attr("disabled", true);

    editonly.hide();
    if (isSerializedProduct == 'True' || isSerializedProduct == 'true') {
        $("#ChasisDetail").show();
    }
    else {
        $("#ChasisDetail").hide();
    }


    $("#txtProductName").val(productName);
    $("#hdnSTNProductDetailId").val(stnProductDetailId);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtPrice").val(price);
    $("#txtQuantity").val(quantity);
    $("#txtUOMName").val(uomname);
    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    $("#hdnSTNIsSerializedProduct").val(isSerializedProduct);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#txtTotalPrice").val(totalprice);


    GetProductAvailableStockBranchWise(productId);


    ShowHideProductPanel(1);
    // RemoveProductChasisNo(productId);
}

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var allProductrow = $('#tblSTNProductList').find('tr').length;
        var challanProductDetailId = $(row).find("#hdnChallanProductDetailId").val();
        var hdnProductId = $(row).find("#hdnProductId").val();
        if (allProductrow == 2) {
            $("#ddlFromWarehouse").attr('readOnly', false);


        }

        RemoveProductChasisNo(hdnProductId);
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
        var stnProductDetailId = $row.find("#hdnSTNProductDetailId").val();
        var totalPrice = $row.find("#hdnTotalPrice").val();
        if (stnProductDetailId != undefined) {
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
    var txtSTNNo = $("#txtSTNNo");
    var hdnstnId = $("#hdnstnId");
    var txtSTNDate = $("#txtSTNDate");
    var txtGRNo = $("#txtGRNo");
    var txtGRDate = $("#txtGRDate");
    var hdnInvoiceId = $("#hdnInvoiceId");
    var txtInvoiceNo = $("#txtInvoiceNo");
    var ddlFromWarehouse = $("#ddlFromWarehouse");
    var ddlToWarehouse = $("#ddlToWarehouse");
    var txtSContactPerson = $("#txtSContactPerson");
    var ddlApprovalStatus = $("#ddlApprovalStatus");


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
    if (ddlFromWarehouse.val().trim() == "" || ddlFromWarehouse.val().trim() == "0") {
        ShowModel("Alert", "Please Select From Loacton")
        ddlFromWarehouse.focus();
        return false;
    }
    if (ddlToWarehouse.val().trim() == "" || ddlToWarehouse.val().trim() == "0") {
        ShowModel("Alert", "Please Select To Loacton")
        ddlToWarehouse.focus();
        return false;
    }
    if (ddlFromWarehouse.val() == ddlToWarehouse.val()) {
        ShowModel("Alert", "From Location and To Location should not Same")
        ddlFromWarehouse.focus();
        return false;

    }




    var STNViewModel = {
        STNId: hdnstnId.val(),
        STNNo: txtSTNNo.val().trim(),
        STNDate: txtSTNDate.val().trim(),
        GrNo: txtGRNo.val().trim(),
        GrDate: txtGRDate.val().trim(),
        InvoiceId: hdnInvoiceId.val().trim(),
        InvoiceNo: txtInvoiceNo.val().trim(),
        ContactPerson: txtSContactPerson.val().trim(),
        FromWarehouseId: ddlFromWarehouse.val(),
        ToWarehouseId: ddlToWarehouse.val(),
        DispatchRefNo: txtDispatchRefNo.val().trim(),
        DispatchRefDate: txtDispatchRefDate.val(),
        LRNo: txtLRNo.val().trim(),
        LRDate: txtLRDate.val(),
        TransportVia: txtTransportVia.val().trim(),
        NoOfPackets: txtNoOfPackets.val(),
        BasicValue: txtBasicValue.val(),
        TotalValue: txtTotalValue.val(),
        FreightValue: txtFreightValue.val(),
        LoadingValue: txtLoadingValue.val(),
        Remarks1: txtRemarks1.val(),
        Remarks2: txtRemarks2.val(),
        ApprovalStatus: ddlApprovalStatus.val()
    };

    var stnProductList = [];

    $('#tblSTNProductList tr').each(function (i, row) {
        var $row = $(row);
        var stnProductDetailId = $row.find("#hdnSTNProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var total = $row.find("#hdnTotalPrice").val();
        if (stnProductDetailId != undefined) {

            var stnProduct = {
                STNProductDetailId: stnProductDetailId,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc,
                UOMName: uomName,
                Price: price,
                Quantity: quantity,
                TotalPrice: total
            };
            stnProductList.push(stnProduct);
        }
    });


    if (stnProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product.")
        return false;
    }


    var stnDocumentList = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var sTNDocId = $row.find("#hdnSTNDocId").val();
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        if (sTNDocId != undefined) {
            var stnDocument = {
                STNDocId: sTNDocId,
                DocumentSequenceNo: documentSequenceNo,
                DocumentTypeId: documentTypeId,
                DocumentTypeDesc: documentTypeDesc,
                DocumentName: documentName,
                DocumentPath: documentPath
            };
            stnDocumentList.push(stnDocument);
        }

    });


    var stnProductChasisList = [];

    $('#tblStnAllProductChasisList tr').each(function (i, row) {

        var $row = $(row);
        var hdnSrnoAll = $row.find("#hdnSrnoAll").val();
        var hdnProductIdAll = $row.find("#hdnProductIdAll").val();
        var hdnChasisNoAll = $row.find("#hdnChasisNoAll").val();
        var hdnProductNameAll = $row.find("#hdnProductNameAll").val();
        var hdnPriceAll = $row.find("#hdnPriceAll").val();

        if (hdnProductIdAll != undefined) {
            var stnProductChasis = {
                SequenceNo: "0",
                ProductName: hdnProductNameAll,
                RefSerial1: hdnChasisNoAll,
                ProductId: hdnProductIdAll,
                Price: hdnPriceAll,
                Status: "Draft",
                StnId: '0',
                STNProductDetailId: "0",
                Remarks: ""

            };
            stnProductChasisList.push(stnProductChasis);


        }
    });



    var accessMode = 1;//Add Mode
    if (hdnstnId.val() != null && hdnstnId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { stnViewModel: STNViewModel, stnProductDetailViewModel: stnProductList, stnDocuments: stnDocumentList, stnChasisProductSerialDetail: stnProductChasisList };
    $.ajax({
        url: "../STN/AddEditSTN?AccessMode=" + accessMode + "",
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
                       window.location.href = "../STN/AddEditSTN?stnId=" + data.trnId + "&AccessMode=3";
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
    $("#txtMRNNo").val("");
    $("#hdnmrnId").val("0");
    $("#txtMRNDate").val($("#hdnCurrentDate").val());
    $("#hdnVendorId").val("0");
    $("#txtVendorName").val("");
    $("#txtVendorCode").val("");
    $("#txtInvoiceNo").val("");
    $("#txtInvoiceDate").val("");
    $("#txtInvoiceId").val("0");
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
    $("#ddlApprovalStatus").val("Draft");
    $("#txtDispatchRefNo").val("");
    $("#txtDispatchRefDate").val("");
    $("#txtLRNo").val("");
    $("#txtLRDate").val("");
    $("#txtTransportVia").val("");
    $("#txtNoOfPackets").val("");
    $("#txtRemarks1").val("");
    $("#txtRemarks2").val("");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
function ShowHideProductPanel(action) {
    var hdnAccessMode = $("#hdnAccessMode");
    if (action == 1) {
        var ddlFromWarehouse = $("#ddlFromWarehouse");
        if (ddlFromWarehouse.val().trim() == "" || ddlFromWarehouse.val().trim() == "0") {
            ShowModel("Alert", "Please Select From Company Branch !.")
            ddlFromWarehouse.focus();
            return false;
        }

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
        $("#hdnMRNProductDetailId").val("0");
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtPrice").val("");
        $("#txtQuantity").val("");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
        $("#txtBranchStock").val("");
        $("#txtTotalPrice").val("");

        if (hdnAccessMode.val() == "3" || hdnAccessMode.val() == "4") {
            $(".editonly").hide();
        }
        else {
            $(".editonly").show();
        }
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
    $("#SearchInvoiceModel").modal();

}

function SearchInvoice() {
    var txtInvoiceNo = $("#txtSearchInvoiceNo");
    var txtVendorName = $("#txtSearchVendorName");
    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { piNo: txtInvoiceNo.val().trim(), vendorName: txtVendorName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val() };
    $.ajax({
        url: "../MRN/GetPurchaseInvoiceList",
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
function SelectInvoice(invoiceId, invoiceNo, invoiceDate, vendorId, vendorCode, vendorName) {
    $("#txtInvoiceNo").val(invoiceNo);
    $("#hdnInvoiceId").val(invoiceId);
    $("#txtInvoiceDate").val(invoiceDate);
    $("#hdnVendorId").val(vendorId);
    $("#txtVendorCode").val(vendorCode);
    $("#txtVendorName").val(vendorName);
    $("#SearchInvoiceModel").modal('hide');
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

function GetSTNDetail(stnId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../STN/GetSTNDetail",
        data: { stnId: stnId },
        dataType: "json",
        success: function (data) {
            $("#txtSTNNo").val(data.STNNo);
            $("#txtSTNDate").val(data.STNDate);
            $("#txtGRNo").val(data.GRNo),
            $("#txtGRDate").val(data.GRDate),
            $("#txtSContactPerson").val(data.ContactPerson);
            // BindFromWareBranchList(0);
            $("#ddlFromWarehouse").val(data.FromWarehouseId);
            // BindToWareBranchList(0);



            $("#ddlApprovalStatus").val(data.ApprovalStatus);

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
function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../STN/AddEditSTN";
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

function OpenSOSearchPopup() {

    var hdnSTNProductDetailId = $("#hdnSTNProductDetailId");
    var txtProductName = $("#txtProductName");
    var txtQuantity = $("#txtQuantity");
    var hdnProductId = $("#hdnProductId");
    var txtProductNameSearch = $("#txtProductNameSearch");

    if (txtProductName.val() == "0" || txtProductName.val() == "") {
        ShowModel("Alert", "Please Enter Product Name");
        txtProductName.focus();
        return false;


    }

    if (txtQuantity.val() == "0" || txtQuantity.val() == "") {
        ShowModel("Alert", "Please Enter Product Quantity");
        txtQuantity.focus();
        return false;

    }
    $("#txtProductNameSearch").attr('readOnly', true);
    $("#hdnProductIdSerach").val(hdnProductId.val());
    $("#txtProductNameSearch").val(txtProductName.val());


    $("#divChasisSerialList").html("");
    $("#SearchChasisModel").modal();
}

function SearchProductChasisSerialNo() {


    var hdnProductId = $("#hdnProductIdSerach");
    var hdnProductIdSerach = $("#hdnProductIdSerach");
    var txtQuantity = $("#txtQuantity");
    var txtProductNameSearch = $("#txtProductNameSearch");
    var txtChasisNo = $("#txtChasisNo");
    var modeValue = 1;
    var ddlCompanyBranch = $("#ddlFromWarehouse");

    if (txtQuantity.val() == "0" || txtQuantity.val() == "") {
        ShowModel("Alert", "Please Enter Product Quantity");
        txtQuantity.focus();
        return false;

    }


    var requestData = { productId: hdnProductIdSerach.val(), chasisSerialNo: txtChasisNo.val(), mode: modeValue, companyBranch: ddlCompanyBranch.val() };
    $.ajax({
        url: "../STN/GetSTNProductChasisSerialNoList",
        data: requestData,
        dataType: "html",
        type: "POST",
        error: function (err) {
            $("#divChasisSerialList").html("");
            $("#divChasisSerialList").html(err);
        },
        success: function (data) {
            $("#divChasisSerialList").html("");
            $("#divChasisSerialList").html(data);

            CheckSerialNos();

        }
    });
}


function CheckSerialNos() {

    $('#tblStnProductChasisList tr').each(function (i, row) {
        var $row = $(row);
        var hdnProductIdPopup = $row.find("#hdnProductIdPopup").val();
        var hdnChasisNoPopup = $row.find("#hdnChasisNoPopup").val();
        var chkStatus = $row.find("#chkStatus");
        var trcol = $row.find(".trCol");
        if (hdnProductIdPopup != undefined) {
            $('#tblStnAllProductChasisList tr').each(function (i, row) {
                var $row = $(row);
                var hdnProductIdAll = $row.find("#hdnProductIdAll").val();
                var hdnChasisNoAll = $row.find("#hdnChasisNoAll").val();
                if (hdnProductIdAll != undefined) {
                    if (hdnProductIdAll == hdnProductIdPopup && hdnChasisNoPopup == hdnChasisNoAll) {
                        chkStatus.attr("checked", true);


                    }
                }
            });
        }
    });
}

function AddChasisSerialPopup() {
  
    var stnProducts = [];
    var action = 1;
    var countchasisno = 0;
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");
    var hdnProductId = $("#hdnProductId");

    //Show Edit,Delete button in Product List//

    $('#tblSTNProductList tr').each(function (i, row) {
        var $row = $(row);
        var editonly = $(row).find(".editonly");
        editonly.show();
    });

    // Upper tblStnProductChasisList
    var countpopupchasisno = 0;
    var messageFlag = false;
    $('#tblStnProductChasisList tr').each(function (i, row) {
        var $row = $(row);
        var hdnSrnoPopup = $row.find("#hdnSrnoPopup").val();
        var hdnProductIdPopup = $row.find("#hdnProductIdPopup").val();
        var chkStatus = $row.find("#chkStatus").is(':checked') ? true : false;
        if (chkStatus == true) {
            countpopupchasisno++;
        }
    });
    if (txtQuantity.val() != countpopupchasisno) {
        ShowModel("Alert", "Selected Chasis No not equal to Product Qty.");
        return false;
    }

    $('#tblStnAllProductChasisList tr').each(function (i, row) {
        var $row = $(row);
        var hdnSrnoAll = $row.find("#hdnSrnoAll").val();
        var hdnProductIdAll = $row.find("#hdnProductIdAll").val();
        var hdnChasisNoAll = $row.find("#hdnChasisNoAll").val();
        var hdnProductNameAll = $row.find("#hdnProductNameAll").val();
        var hdnPriceAll = $row.find("#hdnPriceAll").val();
        var hdnIsSerializedProductAll = $row.find("#hdnIsSerializedProductAll").val();
        if (hdnProductIdAll != undefined) {
            var stnProduct = {
                SequenceNo: hdnSrnoAll,
                ProductName: hdnProductNameAll,
                RefSerial1: hdnChasisNoAll,
                StnId: '0',
                ProductId: hdnProductIdAll,
                Status: "Draft",
                Price: hdnPriceAll,
                IsSerializedProduct: hdnIsSerializedProductAll
            };
            stnProducts.push(stnProduct);
        }
    });

    $('#tblStnProductChasisList tr').each(function (i, row) {
        var $row = $(row);
        var hdnSrnoPopup = $row.find("#hdnSrnoPopup").val();
        var hdnProductIdPopup = $row.find("#hdnProductIdPopup").val();
        var hdnChasisNoPopup = $row.find("#hdnChasisNoPopup").val();
        var hdnProductNamePopup = $row.find("#hdnProductNamePopup").val();
        var hdnPricePopup = $row.find("#hdnPricePopup").val();
        var hdnIsSerializedProductPopup = $row.find("#hdnIsSerializedProductPopup").val();
        var chkStatus = $row.find("#chkStatus").is(':checked') ? true : false;
        if (chkStatus == true) {
            if (hdnProductIdPopup != undefined) {
                if (txtQuantity.val() <= countpopupchasisno) {
                    var stnProduct = {
                        SequenceNo: hdnSrnoPopup,
                        ProductName: hdnProductNamePopup,
                        RefSerial1: hdnChasisNoPopup,
                        StnId: '0',
                        ProductId: hdnProductIdPopup,
                        Status: "Draft",
                        Price: hdnPricePopup,
                        IsSerializedProduct: hdnIsSerializedProductPopup
                    };
                    stnProducts.push(stnProduct);
                }

            }
        }

    });

   

    var requestData = { stnProductList: stnProducts };
    $.ajax({
        url: "../STN/GetSTNProductChasisSerialNo",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divChasisSerialAllProductList").html("");
            $("#divChasisSerialAllProductList").html(err);
            $("#divChasisSerialAllProductListView").html("");
        },
        success: function (data) {
            $("#divChasisSerialAllProductList").html("");
            $("#divChasisSerialAllProductList").html(data);
            $("#divChasisSerialAllProductListView").html("");
            if (hdnSequenceNo.val() != 0) {
                AddProduct(2);
            }
            else {
                AddProduct(1);
            }
        }
    });
}

function RemoveProductChasisNo(productid) {
    $('#tblStnAllProductChasisList tr').each(function (i, row) {

        var $row = $(row);
        var hdnProductIdAll = $row.find("#hdnProductIdAll").val();
        var trcol = $row.find(".trCol");

        if (hdnProductIdAll == productid) {
            trcol.remove();
        }
    });


}




function GetProductChasisSerialNo() {


    var hdnstnId = $("#hdnstnId");
    var modeValue = 2;


    var requestData = { stnId: hdnstnId.val(), mode: modeValue };
    $.ajax({
        url: "../STN/GetSTNProductChasisNoView",
        data: requestData,
        dataType: "html",
        type: "POST",
        error: function (err) {
            $("#divChasisSerialAllProductListView").html("");
            $("#divChasisSerialAllProductListView").html(err);
            $("#divChasisSerialAllProductList").html("");
        },
        success: function (data) {
            $("#divChasisSerialAllProductList").html("");
            $("#divChasisSerialAllProductListView").html("");
            $("#divChasisSerialAllProductListView").html(data);
        }
    });
}


function GetProductAvailableStockBranchWise(productId) {
    var companyBranchID = $("#ddlFromWarehouse").val();

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

//////*************************End Code****************************/////////////////////////

