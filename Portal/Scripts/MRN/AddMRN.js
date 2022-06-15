$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtInvoiceDate").attr('disabled', true);
    $("#txtMRNDate").css('cursor', 'pointer');
   
    $("#txtMRNNo").attr('readOnly', true);
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

 

    $("#txtTaxName").autocomplete({
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

  

    $("#txtMRNDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {
           
        }
    });
    $("#txtInvoiceDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            $("#txtMRNDate").datepicker("option", "minDate", selected);
        }
    });

 var hdnAccessMode = $("#hdnAccessMode");
    var hdnmrnId = $("#hdnmrnId");
    if (hdnmrnId.val() != "" && hdnmrnId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetMRNDetail(hdnmrnId.val());
           
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

    var mrnProducts = [];
    GetMRNProductList(mrnProducts);
    var mrnDocuments = [];
    GetMRNDocumentList(mrnDocuments);
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

function GetMRNDocumentList(mrnDocuments) {
    var hdnmrnId = $("#hdnmrnId");
    var requestData = { mrnDocuments: mrnDocuments, mrnId: hdnmrnId.val() };
    $.ajax({
        url: "../MRN/GetMRNSupportingDocumentList",
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
        url: "../MRN/SaveSupportingDocument",
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

                GetMRNDocumentList(mrnDocumentList);



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
    
    if (txtReceivedQuantity.val().trim() == "" || txtReceivedQuantity.val().trim() == "0" || parseFloat(txtReceivedQuantity.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter Received Quantity")
        txtReceivedQuantity.focus();
        return false;
    }

    if (txtAcceptQuantity.val().trim() == "") {
        ShowModel("Alert", "Please enter Accept Quantity")
        txtAcceptQuantity.focus();
        return false;
    }
    var mrnProductList = [];
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

                if (productId == hdnProductId.val() && parseFloat(price) == parseFloat(txtPrice.val())) {
                    ShowModel("Alert", "Product already added with same Price!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }
                if (productId == hdnProductId.val() && parseFloat(price) != parseFloat(txtPrice.val())) {
                    if (confirm("You have allready Added this Product with Different Price. Do you want to add it?")) {
                        var mrnProduct = {
                            MRNProductDetailId: mrnProductDetailId,
                            SequenceNo: sequenceNo,
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
                    }
                    else {
                        flag = false;
                        return false;
                    }
                }
                var mrnProduct = {
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
                mrnProductList.push(mrnProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnMRNProductDetailId.val() == mrnProductDetailId && hdnSequenceNo.val() == sequenceNo)
            {
                var mrnProduct = {
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
                mrnProductList.push(mrnProduct);
                hdnSequenceNo.val("0");
            }
        }
    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var mrnProductAddEdit = {
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
        mrnProductList.push(mrnProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetMRNProductList(mrnProductList);
    }
    
}
function GetMRNProductList(mrnProducts) {
    var hdnmrnId = $("#hdnmrnId");
    var requestData = { mrnProducts: mrnProducts, mrnId: hdnmrnId.val() };
    $.ajax({
        url: "../MRN/GetMRNProductList",
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
    var txtMRNNo = $("#txtMRNNo");
    var hdnmrnId = $("#hdnmrnId");
    var txtMRNDate = $("#txtMRNDate");
    var txtGRNo = $("#txtGRNo") ;
    var txtGRDate = $("#txtGRDate");
    var hdnInvoiceId = $("#hdnInvoiceId");
    var txtInvoiceNo = $("#txtInvoiceNo");
    var hdnVendorId = $("#hdnVendorId");
    var txtVendorName = $("#txtVendorName");

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
  
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Received At Location")
        ddlCompanyBranch.focus();
        return false;
    }
     

    var MRNViewModel = {
        MRNId: hdnmrnId.val(),
        MRNNo: txtMRNNo.val().trim(),
        MRNDate: txtMRNDate.val().trim(),
        GrNo: txtGRNo.val().trim() == "" ? "0" : txtGRNo.val().trim(),
        GrDate:txtGRDate.val().trim(),
        InvoiceId: hdnInvoiceId.val().trim(),
        InvoiceNo: txtInvoiceNo.val().trim(),
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
    var accessMode = 1;//Add Mode
    if (hdnmrnId.val() != null && hdnmrnId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var mrnProductList = [];
    var productSelected = false;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
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

        if (productId != undefined) {
            productSelected = true;
            var mrnProduct = {
                MRNProductDetailId: mrnProductDetailId,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc,
                UOMName: uomName,
                Price: price,
                Quantity: quantity,
                ReceivedQuantity: receivedQuantity,
                AcceptQuantity: acceptQuantity,
                RejectQuantity:rejectQuantity
            };
            mrnProductList.push(mrnProduct);
        }
    });


    var mrnDocumentList = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var mrnDocId = $row.find("#hdnMrnDocId").val();
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        if (mrnDocId != undefined) {
            var mrnDocument = {
                MRNDocId: mrnDocId,
                DocumentSequenceNo: documentSequenceNo,
                DocumentTypeId: documentTypeId,
                DocumentTypeDesc: documentTypeDesc,
                DocumentName: documentName,
                DocumentPath: documentPath
            };
            mrnDocumentList.push(mrnDocument);
        }

    });


    if (productSelected == false) {
        ShowModel("Alert", "Please select at least one Product")
        ddlCompanyBranch.focus();
        return false; 
    }
    
    var requestData = { mrnViewModel: MRNViewModel, mrnProducts: mrnProductList, mrnDocuments: mrnDocumentList };
    $.ajax({
        url: "../MRN/AddEditMRN?AccessMode=" + accessMode + "",
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
                       window.location.href = "../MRN/AddEditMRN?mrnId=" + data.trnId + "&AccessMode=3";
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

    //window.location.href = "../MRN/AddEditMRN";

    var id = $("#hdnSessionUserID").val();

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

    if (id == 2) {
        $("#ddlCompanyBranch").val(0);
    }

    


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
    $("#divInvoiceList").html("");
    $("#divInvoiceList").html(err);
}

function SearchInvoice() {
    var txtInvoiceNo = $("#txtSearchInvoiceNo");
    var txtVendorName = $("#txtSearchVendorName");
    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { piNo: txtInvoiceNo.val().trim(), vendorName: txtVendorName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: "Final", displayType: "Popup",companyBranch: ddlCompanyBranch.val() };
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
    //$("#txtInvoiceNo").val(invoiceNo);
    //$("#txtInvoiceDate").val(invoiceDate);
    //$("#hdnVendorId").val(vendorId);
    //$("#txtVendorCode").val(vendorCode);
    //$("#txtVendorName").val(vendorName);
    //GetVendorDetail(vendorId);
    $("#SearchInvoiceModel").modal('hide');
   
    $("#ddlCompanyBranch").attr('disabled', true);
    $("#txtVendorName").attr('disabled', true);
    
    $("#txtMRNDate").datepicker("option", "minDate", invoiceDate);
    
    GetPIDetail(invoiceId);
    var piProducts = [];
    GetPIProductList(piProducts, invoiceId);
}
function GetPIDetail(invoiceId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PurchaseInvoice/GetPIDetail",
        data: { invoiceId: invoiceId },
        dataType: "json",
        success: function (data) {
            $("#hdnInvoiceId").val(invoiceId);
            $("#txtInvoiceNo").val(data.InvoiceNo);
            $("#txtInvoiceDate").val(data.InvoiceDate);
            $("#hdnVendorId").val(data.VendorId);
            $("#txtVendorCode").val(data.VendorCode);
            $("#txtVendorName").val(data.VendorName);
            $("#txtRemarks1").val(data.Remarks);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
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
function GetMRNDetail(mrnId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../MRN/GetMRNDetail",
        data: { mrnId: mrnId },
        dataType: "json",
        success: function (data) {
            $("#txtMRNNo").val(data.MRNNo);
            $("#txtMRNDate").val(data.MRNDate);
            $("#txtGRNo").val(data.GRNo),
            $("#txtGRDate").val(data.GRDate),
            $("#txtInvoiceNo").val(data.InvoiceNo);
            $("#hdnInvoiceId").val(data.InvoiceId);
            $("#txtInvoiceDate").val(data.InvoiceDate);

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
        ShowModel("Alert", "Accept Quantity Should be less than  or equal Received Quantity!!!");
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

function ResetPage() {
    if (confirm("Are you Sure to Reset the page")) {
        window.location.href = "../MRN/AddEditMRN";
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