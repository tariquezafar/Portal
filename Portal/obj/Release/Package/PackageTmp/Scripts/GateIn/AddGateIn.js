$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtGateNo").attr('readOnly', true);
    $("#txtPONo").attr('readOnly', true);
    $("#txtMRNDate").attr('readOnly', true);
    $("#txtGRDate").attr('readOnly', true);
    $("#txtInvoiceNo").attr('readOnly', true);
    $("#txtInvoiceDate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtDispatchRefDate").attr('readOnly', true);
    $("#txtLRDate").attr('readOnly', true);
    $("#txtVendorCode").attr('readOnly', true);
    $("#txtRefDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true); 
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtDiscountAmount").attr('readOnly', true);
    $("#txtTaxAmount").attr('readOnly', true);
    $("#txtTotalPrice").attr('readOnly', true);
    $("#txtBasicValue").attr('readOnly', true);
    $("#txtTaxPercentage").attr('readOnly', true);
    $("#txtTaxAmount").attr('readOnly', true);
    $("#txtTotalValue").attr('readOnly', true);
    $("#txtRejectQuantity").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtGateInDate").attr('readOnly', true);
    $("#txtPODate").attr('readOnly', true);
   
   
    
    
    BindCompanyBranchList();
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

    $("#txtLRDate,#txtDispatchRefDate,#txtGRDate,#txtDispatchRefDate,#txtGateInDate,#txtPODate").datepicker({
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
    $("#txtMRNDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {

        }
    });
 var hdnAccessMode = $("#hdnAccessMode");
 var hdngateinId = $("#hdngateinId");
 if (hdngateinId.val() != "" && hdngateinId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetGateInDetail(hdngateinId.val());
           
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
            $("#btnSearchInvoice").attr("onclick", "");
            if ($(".editonly").hide()) {
                $('#lblInvoiceDate').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
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

    var gateProducts = [];
    GetGateInProductList(gateProducts);
    var gateDocuments = [];
    GetGateInDocumentList(gateProducts);
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
function RemoveDocumentRow(obj) {
    if (confirm("Do you want to remove selected Document?")) {
        var row = $(obj).closest("tr");
        var poDocId = $(row).find("#hdnPODocId").val();
        ShowModel("Alert", "Document Removed from List.");
        row.remove();
    }
}

function GetGateInDocumentList(gateDocuments) {
    var hdngateinId = $("#hdngateinId");
    var requestData = { gateDocuments: gateDocuments, gateInId: hdngateinId.val() };
    $.ajax({
        url: "../GateIn/GetGateInSupportingDocumentList",
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
    if ($("#ddlDocumentType").val() == "0")
    {
        ShowModel("Alert", "Please Select Document Type.")
        $("#FileUpload1").val('');
        return "";       
    }
    else
    {
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
        
    }
   
    $.ajax({
        url: "../GateIn/SaveSupportingDocument",
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
                var hdnMrnDocId = $("#hdnMrnDocId");
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



                var mrnDocumentList = [];
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    docEntrySequence = 1;
                }
                $('#tblDocumentList tr').each(function (i, row) {
                    var $row = $(row);
                    var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
                    var hdnMrnDocId = $row.find("#hdnMrnDocId").val();
                    var documentTypeId = $row.find("#hdnDocumentTypeId").val();
                    var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
                    var documentName = $row.find("#hdnDocumentName").val();
                    var documentPath = $row.find("#hdnDocumentPath").val();

                    if (hdnMrnDocId != undefined) {
                        if ((hdnDocumentSequence.val() != documentSequenceNo)) {
                            var mrnDocument = {
                                MRNDocId: hdnMrnDocId,
                                DocumentSequenceNo: documentSequenceNo,
                                DocumentTypeId: documentTypeId,
                                DocumentTypeDesc: documentTypeDesc,
                                DocumentName: documentName,
                                DocumentPath: documentPath
                            };
                            mrnDocumentList.push(mrnDocument);
                            docEntrySequence = parseInt(docEntrySequence) + 1;
                        }
                        else if (hdnMrnDocId.val() == mRNDocId && hdnDocumentSequence.val() == documentSequenceNo) {
                            var mrnDocument = {
                                MRNDocId: hdnMrnDocId.val(),
                                DocumentSequenceNo: hdnDocumentSequence.val(),
                                DocumentTypeId: ddlDocumentType.val(),
                                DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                                DocumentName: newFileName,
                                DocumentPath: newFileName
                            };
                            mrnDocumentList.push(mrnDocument);
                        }
                    }
                });
                if ((hdnDocumentSequence.val() == "" || hdnDocumentSequence.val() == "0")) {
                    hdnDocumentSequence.val(docEntrySequence);
                }

                var mrnDocumentAddEdit = {
                    MRNDocId: hdnMrnDocId.val(),
                    DocumentSequenceNo: hdnDocumentSequence.val(),
                    DocumentTypeId: ddlDocumentType.val(),
                    DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                    DocumentName: newFileName,
                    DocumentPath: newFileName
                };
                mrnDocumentList.push(mrnDocumentAddEdit);
                hdnDocumentSequence.val("0");

                GetGateInDocumentList(mrnDocumentList);



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
        $("#hdnMrnDocId").val("0");
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


function GetProductDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productid: productId },
        dataType: "json",
        success: function (data) {
            $("#txtPrice").val(data.PurchasePrice);
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
    var hdnMRNProductDetailId = $("#hdnMRNProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc"); 
    var txtPrice = $("#txtPrice");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var txtReceivedQuantity = $("#txtReceivedQuantity");
    var txtAcceptQuantity = $("#txtAcceptQuantity");
    var txtRejectQuantity = $("#txtRejectQuantity");

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
        ShowModel("Alert", "Please enter Order Quantity")
        txtQuantity.focus();
        return false;
    }
    
    //if (txtReceivedQuantity.val().trim() == "" || txtReceivedQuantity.val().trim() == "0" || parseFloat(txtReceivedQuantity.val().trim()) <= 0) {
    //    ShowModel("Alert", "Please enter Received Quantity")
    //    txtReceivedQuantity.focus();
    //    return false;
    //}

    //if (txtAcceptQuantity.val().trim() == "") {
    //    ShowModel("Alert", "Please enter Accept Quantity")
    //    txtAcceptQuantity.focus();
    //    return false;
    //}
    var gateProductList = [];
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var mrnProductDetailId = $row.find("#hdnMRNProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var receivedQuantity = $row.find("#hdnReceivedQuantity").val();
        var acceptQuantity = $row.find("#hdnAcceptQuantity").val();
        var rejectQuantity = $row.find("#hdnRejectQuantity").val();
        if (productName != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var gateProduct = {
                    MRNProductDetailId: mrnProductDetailId,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Price: price,
                    Quantity: quantity,
                    ReceivedQuantity:receivedQuantity,
                    AcceptQuantity: acceptQuantity,
                    RejectQuantity:rejectQuantity
                };
                gateProductList.push(gateProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnMRNProductDetailId.val() == mrnProductDetailId && hdnSequenceNo.val() == sequenceNo)
            {
                var gateProduct = {
                    MRNProductDetailId: hdnMRNProductDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    Price: txtPrice.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    ReceivedQuantity: txtReceivedQuantity.val().trim(),
                    AcceptQuantity: txtAcceptQuantity.val().trim(),
                    RejectQuantity: txtRejectQuantity.val().trim()
                };
                gateProductList.push(gateProduct);
                hdnSequenceNo.val("0");
            }
        }
    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var gateProductAddEdit = {
            MRNProductDetailId: hdnMRNProductDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            Price: txtPrice.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            ReceivedQuantity: txtReceivedQuantity.val().trim(),
            AcceptQuantity: txtAcceptQuantity.val().trim(),
            RejectQuantity: txtRejectQuantity.val().trim()
        };
        gateProductList.push(gateProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetGateInProductList(gateProductList);
    }
    
}
function GetGateInProductList(gateProducts) {
    var hdngateinId = $("#hdngateinId");
    var requestData = { gateProducts: gateProducts, gateInId: hdngateinId.val() };
    $.ajax({
        url: "../GateIn/GetGateInProductList",
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
    var challanProductDetailId = $(row).find("#hdnMRNProductDetailId").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var price = $(row).find("#hdnPrice").val();
    var quantity = $(row).find("#hdnQuantity").val();
    var receivedQuantity = $(row).find("#hdnReceivedQuantity").val();
    var acceptQuantity = $(row).find("#hdnAcceptQuantity").val();
    var rejectQuantity = $(row).find("#hdnRejectQuantity").val();
    +
    $("#txtProductName").val(productName);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnMRNProductDetailId").val(challanProductDetailId);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);
    $("#txtPrice").val(price);
    $("#txtQuantity").val(quantity);

    $("#txtRejectQuantity").val(rejectQuantity);
    $("#txtAcceptQuantity").val(acceptQuantity);
    $("#txtReceivedQuantity").val(receivedQuantity);
  
    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
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


function SaveData() {
    var txtGateNo = $("#txtGateNo");
    var hdngateinId = $("#hdngateinId");
    var txtGateInDate = $("#txtGateInDate");
    var txtGRNo = $("#txtGRNo") ;
    var txtGRDate = $("#txtGRDate");
    var hdnVendorId = $("#hdnVendorId");
    var txtVendorName = $("#txtVendorName");
    var hdnPOId = $("#hdnPOId");
    var txtPONo = $("#txtPONo");
    var txtPODate = $("#txtPODate");
    var txtSAddress = $("#txtSAddress");
    var txtSCity = $("#txtSCity");
    var ddlSCountry = $("#ddlSCountry");
    var ddlSState = $("#ddlSState");
    var txtSPinCode = $("#txtSPinCode");
    var txtSTINNo = $("#txtSTINNo");
    var txtSEmail = $("#txtSEmail");
    var txtSMobileNo = $("#txtSMobileNo");
    var txtSContactNo = $("#txtSContactNo");
    var txtSFax = $("#txtSFax");
    var txtSContactPerson = $("#txtSContactPerson");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtDispatchRefNo = $("#txtDispatchRefNo") == "" ? "0" : $("#txtDispatchRefNo");
    var txtDispatchRefDate = $("#txtDispatchRefDate") == "" ? "0" : $("#txtDispatchRefDate");
    var txtLRNo = $("#txtLRNo") == "" ? "0" : $("#txtLRNo");
    var txtLRDate = $("#txtLRDate") == "" ? "0" : $("#txtLRDate");
    var txtTransportVia = $("#txtTransportVia") == "" ? "0" : $("#txtTransportVia");
    var txtNoOfPackets = $("#txtNoOfPackets") == "" ? "0" : $("#txtNoOfPackets");
    var txtRemarks1 = $("#txtRemarks1") == "" ? "" : $("#txtRemarks1");
    var txtRemarks2 = $("#txtRemarks2") == "" ? "" : $("#txtRemarks2");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Received At Location")
        ddlCompanyBranch.focus();
        return false;
    }
    if (txtVendorName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Vendor Name")
        txtVendorName.focus();
        return false;
    }
    if (hdnVendorId.val() == "" || hdnVendorId.val() == "0") {
        ShowModel("Alert", "Please select Vendor from list")
        txtCustomerName.focus();
        return false;
    }

    var pDate = new Date(txtGateInDate.val());
    var poDate = new Date(txtPODate.val());
    if (pDate < poDate) {
        ShowModel("Alert", "Gate In Date not be Less than PO Date.")
        return false;
    }
  
     

    var GateInViewModel = {
        GateInId: hdngateinId.val(),
        GateInNo: txtGateNo.val().trim(),
        GateInDate: txtGateInDate.val().trim(),
        GrNo: txtGRNo.val().trim() == "" ? "0" : txtGRNo.val().trim(),
        GrDate: txtGRDate.val().trim(),
        POId: hdnPOId.val(),
        PONo: txtPONo.val().trim(),
        VendorId: hdnVendorId.val().trim(),
        VendorName: txtVendorName.val().trim(),
        ContactPerson: txtSContactPerson.val().trim(),
        ShippingContactPerson: txtSContactPerson.val().trim(),
        ShippingBillingAddress: txtSAddress.val().trim(),
        ShippingCity: txtSCity.val().trim(),
        ShippingStateId: ddlSState.val(),
        ShippingCountryId: ddlSCountry.val(),
        ShippingPinCode: txtSPinCode.val().trim(),
        ShippingTINNo: txtSTINNo.val().trim(),
        ShippingEmail: txtSEmail.val().trim(),
        ShippingMobileNo: txtSMobileNo.val().trim(),
        ShippingContactNo: txtSContactNo.val().trim(),
        ShippingFax: txtSFax.val().trim(),
        CompanyBranchId: ddlCompanyBranch.val(),
        DispatchRefNo: txtDispatchRefNo.val().trim(),
        DispatchRefDate: txtDispatchRefDate.val(),
        LRNo: txtLRNo.val().trim(),
        LRDate: txtLRDate.val(),
        TransportVia: txtTransportVia.val().trim(),
        NoOfPackets: txtNoOfPackets.val(),
        Remarks1: txtRemarks1.val(),
        Remarks2: txtRemarks2.val(),
        ApprovalStatus: ddlApprovalStatus.val()
    };

    var gateinProductList = [];
    var productSelected = false;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var gateInProductDetailId = $row.find("#hdnGateInProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var receivedQuantity = $row.find("#hdnReceivedQuantity").val();
        var acceptQuantity = $row.find("#hdnAcceptQuantity").val();
        var rejectQuantity = $row.find("#hdnRejectQuantity").val();

        if (productId != undefined) {
            productSelected = true;
            var gateinProduct = {
                GateInProductDetailId: gateInProductDetailId,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc,
                UOMName: uomName,
                Price: price,
                Quantity: quantity,
                ReceivedQuantity: receivedQuantity,
                AcceptQuantity: acceptQuantity,
                RejectQuantity: rejectQuantity
                
            };
            gateinProductList.push(gateinProduct);
        }
    });


    var gateinDocumentList = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var mrnDocId = $row.find("#hdnMrnDocId").val();
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        if (mrnDocId != undefined) {
            var gateinDocument = {
                MRNDocId: mrnDocId,
                DocumentSequenceNo: documentSequenceNo,
                DocumentTypeId: documentTypeId,
                DocumentTypeDesc: documentTypeDesc,
                DocumentName: documentName,
                DocumentPath: documentPath
            };
            gateinDocumentList.push(gateinDocument);
        }

    });


    if (productSelected == false) {
        ShowModel("Alert", "Please select at least one Product")
        ddlCompanyBranch.focus();
        return false; 
    }

    var accessMode = 1;//Add Mode
    if (hdngateinId.val() != null && hdngateinId.val() != 0) {
        accessMode = 2;//Edit Mode
    }


    var requestData = { gateinViewModel: GateInViewModel, gateinProducts: gateinProductList, gateinDocuments: gateinDocumentList };
    $.ajax({
        url: "../GateIn/AddEditGateIn?AccessMode=" + accessMode + "",
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
                       window.location.href = "../GateIn/AddEditGateIn?gateinId=" + data.trnId + "&AccessMode=3";
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

    $("#ddlCompanyBranch").val("0");

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
    if (action == 1) {
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
        $("#txtReceivedQuantity").val("");
        $("#txtAcceptQuantity").val("");
        $("#txtRejectQuantity").val("");
        $("#txtUOMName").val("");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
    }
}


function OpenInvoiceSearchPopup() {


    if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
        ShowModel("Alert", "Please Select Company Branch");
        return false;
    }

    $("#SearchInvoiceModel").modal();

}

function SearchPO() {
    var txtSearchPONo = $("#txtSearchPONo");
    var txtVendorName = $("#txtSearchVendorName");
    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { poNo: txtSearchPONo.val().trim(), vendorName: txtVendorName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: "Final", displayType: "Popup",companyBranch: ddlCompanyBranch.val() };
    $.ajax({
        url: "../GateIn/GetGateInPOList",
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
function SelectPO(poId, pono, poDate, vendorId, vendorCode, vendorName) {
    $("#SearchInvoiceModel").modal('hide');

    GetPODetail(poId);
    var poProducts = [];
    GetPOProductList(poProducts, poId);
}
function GetPODetail(poId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PO/GetPODetail",
        data: { poId: poId },
        dataType: "json",
        success: function (data) {
            $("#txtPONo").val(data.PONo);
            $("#txtPODate").val(data.PODate);
            $("#hdnPOId").val(poId);
            $("#hdnVendorId").val(data.VendorId);
            $("#txtVendorCode").val(data.VendorCode);
            $("#txtVendorName").val(data.VendorName);
            $("#txtRemarks1").val(data.Remarks);
            $("#txtVendorName").attr('readOnly', true);
           
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function GetPOProductList(poProducts, poId) {

    var requestData = { poProducts: poProducts, poId: poId };
    $.ajax({
        url: "../GateIn/GetPOGateInProductList",
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

            // CalculateGrossandNetValues();
            ShowHideProductPanel(2);
        }
    });
}
function GetPIProductList(piProducts, invoiceId) {
    
    var requestData = { piProducts: piProducts, invoiceId: invoiceId };
    $.ajax({
        url: "../PurchaseInvoice/GetPIMRNProductList",
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

           // CalculateGrossandNetValues();
            ShowHideProductPanel(2);
        }
    });
}



function GetVendorDetail(vendorId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Vendor/GetVendorDetail",
        data: { vendorId: vendorId },
        dataType: "json",
        success: function (data) {
            $("#txtSAddress").val(data.Address);
            $("#txtSCity").val(data.City);
            $("#ddlSCountry").val(data.CountryId);
            $("#ddlSState").val(data.StateId);
            $("#txtSPinCode").val(data.PinCode);
            $("#txtSTINNo").val(data.TINNo);
            $("#txtSEmail").val(data.Email);
            $("#txtSMobileNo").val(data.MobileNo);
            $("#txtSContactNo").val(data.ContactNo);
            $("#txtSFax").val(data.Fax);
            $("#txtSContactPerson").val(data.ContactPersonName);
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function GetGateInDetail(gateinId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../GateIn/GetGateInDetail",
        data: { gateinId: gateinId },
        dataType: "json",
        success: function (data) {
            $("#txtGateNo").val(data.GateInNo);
            $("#txtGateInDate").val(data.GateInDate);
            $("#txtGRNo").val(data.GRNo);
            $("#txtGRDate").val(data.GRDate);
            $("#txtPONo").val(data.PONo);
            $("#hdnPOId").val(data.POId);

            $("#hdnVendorId").val(data.VendorId);
            $("#txtVendorCode").val(data.VendorCode);
            $("#txtVendorName").val(data.VendorName);

            $("#ddlApprovalStatus").val(data.ApprovalStatus)
            if (data.ApprovalStatus == "Final") {
                $("#btnUpdate").hide();
                $(".editonly").hide();
                $("#btnReset").hide();
                if ($(".editonly").hide()) {
                    $('#lblInvoiceDate').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
                }
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);
            }

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

            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtDispatchRefNo").val(data.DispatchRefNo);
            $("#txtDispatchRefDate").val(data.DispatchRefDate);
            $("#txtLRNo").val(data.LRNo);
            $("#txtLRDate").val(data.LRDate);
            $("#txtTransportVia").val(data.TransportVia);
            $("#txtNoOfPackets").val(data.NoOfPackets);


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

function CalculateQuantity()
{
    var totalquantity = $("#txtReceivedQuantity").val();
    var acceptquantity = $("#txtAcceptQuantity").val();
    total = totalquantity == "" ? 0 : totalquantity;
    accept = acceptquantity == "" ? 0 : acceptquantity;
    if (parseFloat(accept) > parseFloat(total))
    {
        ShowModel("Alert", "Accept Quantity Should be less than or equal Received Quantity!!!");
        $("#txtAcceptQuantity").val("0");
        $("#txtRejectQuantity").val(totalquantity);
        $("#txtAcceptQuantity").focus();
        return false;
      
    }
    else
    {
        var rejectquantity = (parseFloat(total) - parseFloat(accept)).toFixed(2);
        $("#txtRejectQuantity").val(parseFloat(rejectquantity).toFixed(2));
    }
    
}

$("body").on("keyup", "#tblProductList .receivedQuantity", function () {

    var acceptQuantity = $(this).closest("tr").find("#txtAcceptQuantity").val();
    var receivedQuantity = $(this).closest("tr").find("#txtReceivedQuantity").val() == "" ? "0" : $(this).closest("tr").find("#txtReceivedQuantity").val();
    var pendingQuantity = $(this).closest("tr").find("#hdnPendingQuantity").val();
    $(this).closest("tr").find("#hdnReceivedQuantity").val(receivedQuantity);
    $(this).closest("tr").find(".acceptQuantity").val(receivedQuantity);

  
        if (parseFloat(receivedQuantity) > parseFloat(pendingQuantity)) {
            ShowModel("Alert", "Received Quantity Should be less than or equal Pending Quantity!!!");
            $(this).closest("tr").find("#txtReceivedQuantity").val("0");
            $(this).closest("tr").find("#hdnReceivedQuantity").val("0");
            $(this).closest("tr").find(".acceptQuantity").val("0");
            return false;
        }
        //else {
        //    ShowModel("Alert", "Received Quantity Should be less than or equal Pending Quantity!!!");
        //    $(this).closest("tr").find("#txtAcceptQuantity").val("0");
        //    $(this).closest("tr").find("#hdnAcceptQuantity").val("0");
        //    $(this).closest("tr").find("#txtReceivedQuantity").val("0");
        //    $(this).closest("tr").find("#hdnReceivedQuantity").val("0");
        //    $(this).closest("tr").find("#txtRejectQuantity").val("0");
        //    $(this).closest("tr").find("#hdnRejectQuantity").val("0");
        //    return false;
        //}
 
    //if (parseFloat(receivedQuantity) >= parseFloat(acceptQuantity)) {
    //    if (receivedQuantity != 0 || receivedQuantity != "") {
    //        $(this).closest("tr").find("#txtRejectQuantity").val(parseFloat(receivedQuantity - acceptQuantity));
    //        $(this).closest("tr").find("#hdnRejectQuantity").val(parseFloat(receivedQuantity - acceptQuantity));
    //    }
    //}
    //else
    //{
    //    ShowModel("Alert", "Accept Quantity Should be less than or equal Received Quantity!!!");
    //    $(this).closest("tr").find("#txtAcceptQuantity").val("0");
    //    $(this).closest("tr").find("#hdnAcceptQuantity").val("0");
    //    $(this).closest("tr").find("#txtRejectQuantity").val("0");
    //    $(this).closest("tr").find("#hdnRejectQuantity").val("0");
    //    return false;
    //}
});

$("body").on("keyup", "#tblProductList .acceptQuantity", function () {

    var acceptQuantity = $(this).closest("tr").find(".acceptQuantity").val();
    var receivedQuantity = $(this).closest("tr").find(".receivedQuantity").val();
    if (parseFloat(acceptQuantity) > parseFloat(receivedQuantity)) {
        ShowModel("Alert", "Accept Quantity Should be less than or equal Received Quantity!!!");
        $(this).closest("tr").find("#txtAcceptQuantity").val("0");
        $(this).closest("tr").find("#txtRejectQuantity").val("0");
        $(this).closest("tr").find("#hdnRejectQuantity").val("0");
        return false;
                
    }
    else
    {
        $(this).closest("tr").find("#txtRejectQuantity").val(parseFloat(receivedQuantity - acceptQuantity));
        $(this).closest("tr").find("#hdnRejectQuantity").val(parseFloat(receivedQuantity - acceptQuantity));
        $(this).closest("tr").find("#hdnAcceptQuantity").val(acceptQuantity);


    }

});

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

function SearchInvoice() {
    var txtInvoiceNo = $("#txtSearchInvoiceNo");
    var txtVendorName = $("#txtSearchVendorName");
    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { piNo: txtInvoiceNo.val().trim(), vendorName: txtVendorName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: "Final", displayType: "Popup" };
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