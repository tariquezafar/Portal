$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtQualityCheckNo").attr('readOnly', true);
    $("#txtQualityCheckDate").attr('readOnly', true);
    $("#txtGateInNo").attr('readOnly', true);
    $("#txtPoNo").attr('readOnly', true);
   
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtProductName").attr('readOnly', true);
    
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
    $("#txtPendingQuantity").attr('readOnly', true); 
    $("#txtQuantity").attr('readOnly', true);
   
    
    
    BindCompanyBranchList();
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

    $("#txtQualityCheckDate,#txtDispatchRefDate,#txtGRDate,#txtDispatchRefDate,#txtGateInDate,#txtPODate").datepicker({
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
 var hdnQualityCheckId = $("#hdnQualityCheckId");
 if (hdnQualityCheckId.val() != "" && hdnQualityCheckId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetQualityCheckDetail(hdnQualityCheckId.val());
           
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

 var qualityCheckProducts = [];
 GetQualityCheckProductList(qualityCheckProducts);
 var qualityCheckDocuments = [];
 GetQualityCheckDocumentList(qualityCheckDocuments);
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

function GetQualityCheckDocumentList(qualityCheckDocuments) {
    var hdnQualityCheckId = $("#hdnQualityCheckId");
    var requestData = { qualityCheckDocuments: qualityCheckDocuments, qualityCheckId: hdnQualityCheckId.val() };
    $.ajax({
        url: "../QualityCheck/GetQualityCheckSupportingDocumentList",
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
function GetQualityCheckProductList(qualityCheckProducts) {
    var hdnQualityCheckId = $("#hdnQualityCheckId");
    var requestData = { qualityCheckProducts: qualityCheckProducts, qualityCheckId: hdnQualityCheckId.val() };
    $.ajax({
        url: "../QualityCheck/GetQualityCheckProductList",
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
        url: "../QualityCheck/SaveSupportingDocument",
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
                var hdnQualityCheckDocId = $("#hdnQualityCheckDocId");
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
                    var hdnQualityCheckDocId = $row.find("#hdnQualityCheckDocId").val();
                    var documentTypeId = $row.find("#hdnDocumentTypeId").val();
                    var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
                    var documentName = $row.find("#hdnDocumentName").val();
                    var documentPath = $row.find("#hdnDocumentPath").val();

                    if (hdnQualityCheckDocId != undefined) {
                        if ((hdnDocumentSequence.val() != documentSequenceNo)) {
                            var mrnDocument = {
                                QualityCheckDocId: hdnQualityCheckDocId,
                                DocumentSequenceNo: documentSequenceNo,
                                DocumentTypeId: documentTypeId,
                                DocumentTypeDesc: documentTypeDesc,
                                DocumentName: documentName,
                                DocumentPath: documentPath
                            };
                            mrnDocumentList.push(mrnDocument);
                            docEntrySequence = parseInt(docEntrySequence) + 1;
                        }
                        else if (hdnQualityCheckDocId.val() == mRNDocId && hdnDocumentSequence.val() == documentSequenceNo) {
                            var mrnDocument = {
                                QualityCheckDocId: hdnQualityCheckDocId.val(),
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
                    QualityCheckDocId: hdnQualityCheckDocId.val(),
                    DocumentSequenceNo: hdnDocumentSequence.val(),
                    DocumentTypeId: ddlDocumentType.val(),
                    DocumentTypeDesc: $("#ddlDocumentType option:selected").text(),
                    DocumentName: newFileName,
                    DocumentPath: newFileName
                };
                mrnDocumentList.push(mrnDocumentAddEdit);
                hdnDocumentSequence.val("0");

                GetQualityCheckDocumentList(mrnDocumentList);
              


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
function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtProductName = $("#txtProductName");
    var hdnQualityCheckDetailId = $("#hdnQualityCheckDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtPrice = $("#txtPrice");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var txtReceivedQuantity = $("#txtReceivedQuantity");
    var txtAcceptQuantity = $("#txtAcceptQuantity");
    var txtRejectQuantity = $("#txtRejectQuantity");
    var txtPendingQuantity = $("#txtPendingQuantity");
    var hdnTotalRecQuantity = $("#hdnTotalRecQuantity");

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
    if (parseFloat(txtReceivedQuantity.val()) > parseFloat(txtPendingQuantity.val())) {
        ShowModel("Alert", "Received Quantity cannot be grater than Pending Quantiy.")
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
        var qualityCheckDetailId = $row.find("#hdnQualityCheckDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnRemarks").val();
        var uomName = $row.find("#hdnUOMName").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var receivedQuantity = $row.find("#hdnReceivedQuantity").val();
        var acceptQuantity = $row.find("#hdnAcceptQuantity").val();
        var rejectQuantity = $row.find("#hdnRejectQuantity").val();
        var totalRecQuantity = $row.find("#hdnTotalRecQuantity").val();
        var pendingQuantity = $row.find("#hdnPendingQuantity").val();

        if (productName != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var mrnProduct = {
                    QualityCheckDetailId: qualityCheckDetailId,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    RejectRemakrs: productShortDesc,
                    UOMName: uomName,                  
                    Quantity: quantity,
                    ReceivedQuantity: receivedQuantity,
                    AcceptQuantity: acceptQuantity,
                    RejectQuantity: rejectQuantity,
                    TotalRecQuantity: totalRecQuantity,
                    PendingQuantity: pendingQuantity

                };
                mrnProductList.push(mrnProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnQualityCheckDetailId.val() == qualityCheckDetailId && hdnSequenceNo.val() == sequenceNo) {
                var mrnProduct = {
                    QualityCheckDetailId: hdnQualityCheckDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    RejectRemakrs: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    ReceivedQuantity: txtReceivedQuantity.val().trim(),
                    AcceptQuantity: txtAcceptQuantity.val().trim(),
                    RejectQuantity: txtRejectQuantity.val().trim(),
                    TotalRecQuantity: hdnTotalRecQuantity.val(),
                    PendingQuantity: txtPendingQuantity.val().trim()
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
            QualityCheckDetailId: hdnQualityCheckDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            RejectRemakrs: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            ReceivedQuantity: txtReceivedQuantity.val().trim(),
            AcceptQuantity: txtAcceptQuantity.val().trim(),
            RejectQuantity: txtRejectQuantity.val().trim(),
            TotalRecQuantity: hdnTotalRecQuantity.val(),
            PendingQuantity: txtPendingQuantity.val().trim()
        };
        mrnProductList.push(mrnProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetQualityCheckProductList(mrnProductList);
    }

}



function EditProductRow(obj) {
    var row = $(obj).closest("tr");
    var qualityCheckDetailId = $(row).find("#hdnQualityCheckDetailId").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnRemarks").val();
    var uomName = $(row).find("#hdnUOMName").val(); 
    var quantity = $(row).find("#hdnQuantity").val();
    var receivedQuantity = $(row).find("#hdnReceivedQuantity").val();
    var acceptQuantity = $(row).find("#hdnAcceptQuantity").val();
    var rejectQuantity = $(row).find("#hdnRejectQuantity").val();
    var totalRecQuantity = $(row).find("#hdnTotalRecQuantity").val();
    var pendingQuantity = $(row).find("#hdnPendingQuantity").val();

    $("#txtProductName").val(productName);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnQualityCheckDetailId").val(qualityCheckDetailId);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);   
    $("#txtQuantity").val(quantity);

    $("#txtRejectQuantity").val(rejectQuantity);
    $("#txtAcceptQuantity").val(acceptQuantity);
    $("#txtReceivedQuantity").val(receivedQuantity);
    $("#txtPendingQuantity").val(pendingQuantity);
    $("#hdnPendingQuantity").val(pendingQuantity);
    $("#hdnTotalRecQuantity").val(totalRecQuantity);

    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    ShowHideProductPanel(1);
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



function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var challanProductDetailId = $(row).find("#hdnChallanProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();

    }
}


function SaveData() {
    var hdnQualityCheckId = $("#hdnQualityCheckId")
    var txtQualityCheckDate = $("#txtQualityCheckDate");
    var txtQualityCheckNo = $("#txtQualityCheckNo")
    var txtGateInNo = $("#txtGateInNo");
    var hdnGateInId = $("#hdnGateInId");
    var txtPoNo = $("#txtPoNo");
    var hdnPOId = $("#hdnPOId");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
   

    var txtRemarks = $("#txtRemarks");
    var txtRejectRemark = $("#txtRejectRemark");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
  
    if (hdnGateInId.val() == "" || hdnGateInId.val() == "0") {
        ShowModel("Alert", "Please Select Gate In No.")
        txtQualityCheckNo.focus();
        return false;
    }
  
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Received At Location")
        ddlCompanyBranch.focus();
        return false;
    }
     

    var qcViewModel = {
        QualityCheckId: hdnQualityCheckId.val(),
        QualityCheckDate:txtQualityCheckDate.val().trim(),
        GateInId: hdnGateInId.val(),
        GateInNo: txtGateInNo.val().trim(),               
        POId: hdnPOId.val(),       
        CompanyBranchId: ddlCompanyBranch.val(),       
        Remarks: txtRemarks.val(),
        RejectRemarks: txtRejectRemark.val(),
        ApprovalStatus: ddlApprovalStatus.val()
    };

    var qcProductList = [];
    var productSelected = false;
    var recQTY = false;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var qualityCheckDetailId = $row.find("#hdnQualityCheckDetailId").val();
        var productId = $row.find("#hdnProductId").val();                  
        var quantity = $row.find("#hdnQuantity").val();
        var receivedQuantity = $row.find("#hdnReceivedQuantity").val();
        var acceptQuantity = $row.find("#hdnAcceptQuantity").val();
        var rejectQuantity = $row.find("#hdnRejectQuantity").val();
        var remarks = $row.find("#hdnRemarks").val();

        if (productId != undefined) {
            productSelected = true;
            if (parseFloat(receivedQuantity) > 0) {
                recQTY = true;
            }
            var qcProduct = {
                QualityCheckDetailId: qualityCheckDetailId,
                ProductId: productId,                                             
                Quantity: quantity,
                ReceivedQuantity: receivedQuantity,
                AcceptQuantity: acceptQuantity,
                RejectQuantity: rejectQuantity,
                RejectRemakrs: remarks,
            };
            qcProductList.push(qcProduct);
        }
    });

    var qcDocumentList = [];
    $('#tblDocumentList tr').each(function (i, row) {
        var $row = $(row);
        var qualityCheckDocId = $row.find("#hdnQualityCheckDocId").val();
        var documentSequenceNo = $row.find("#hdnDocumentSequenceNo").val();
        var documentTypeId = $row.find("#hdnDocumentTypeId").val();
        var documentTypeDesc = $row.find("#hdnDocumentTypeDesc").val();
        var documentName = $row.find("#hdnDocumentName").val();
        var documentPath = $row.find("#hdnDocumentPath").val();

        if (qualityCheckDocId != undefined) {
            var qcDocument = {
                QualityCheckDocId: qualityCheckDocId,
                DocumentSequenceNo: documentSequenceNo,
                DocumentTypeId: documentTypeId,
                DocumentTypeDesc: documentTypeDesc,
                DocumentName: documentName,
                DocumentPath: documentPath
            };
            qcDocumentList.push(qcDocument);
        }

    });

    if (productSelected == false) {
        ShowModel("Alert", "Please select at least one Product")
        ddlCompanyBranch.focus();
        return false; 
    }
    if (recQTY == false) {
        ShowModel("Alert", "Please select at least one Product Recived Quantity")       
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdnQualityCheckId.val() != null && hdnQualityCheckId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { qcViewModel: qcViewModel, qcProductList: qcProductList, qcDocumentList: qcDocumentList };
    $.ajax({
        url: "../QualityCheck/AddEditQualityCheck?AccessMode=" + accessMode + "",
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
                       window.location.href = "../QualityCheck/AddEditQualityCheck?qualityCheckId=" + data.trnId + "&AccessMode=3";
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

    $("#txtQualityCheckNo").val("");
    $("#hdnQualityCheckId").val("0");
    $("#txtQualityCheckDate").val($("#hdnCurrentDate").val());
    $("#txtGateInNo").val("");
    $("#hdnGateInId").val("0");
    $("#txtPoNo").val("");
    $("#hdnPOId").val("0");   
    $("#ddlCompanyBranch").val("0");

    $("#txtGateInDate").val("");

    $("#txtRemarks").val("");
    $("#txtRejectRemark").val("");
    ;

    $("#ddlApprovalStatus").val("Draft");
   
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
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    $("#SearchInvoiceModel").modal();

}

function SearchGate() {
    var txtSearchGateInNO = $("#txtSearchGateInNO");
    var txtSearchPONO = $("#txtSearchPONO");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { gateInNO: txtSearchGateInNO.val().trim(), pONO: txtSearchPONO.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: "Final", displayType: "Popup",companyBranch: ddlCompanyBranch.val() };
    $.ajax({
        url: "../QualityCheck/GetQualityCheckGateInList",
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
function SelectGateIN(gateInId, gateInNo, gateInDate, poId, pono,companyBranchId) {
    $("#SearchInvoiceModel").modal('hide');

    $("#hdnGateInId").val(gateInId);
    $("#txtGateInNo").val(gateInNo);
    $("#txtGateInDate").val(gateInDate);

    $("#hdnPOId").val(poId);
    $("#txtPoNo").val(pono);
    $("#ddlCompanyBranch").val(companyBranchId);       
    var giProducts = [];
    GetGateInProduct(giProducts, gateInId);
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
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function GetGateInProduct(giProducts, gateInId) {

    var requestData = { giProducts: giProducts, gateInId: gateInId };
    $.ajax({
        url: "../QualityCheck/GetQualityCheckGateInProductList",
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
function GetQualityCheckDetail(qualityCheckId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../QualityCheck/GetQualityCheckDetail",
        data: { qualityCheckId: qualityCheckId },
        dataType: "json",
        success: function (data) {
            $("#hdnQualityCheckId").val(data.QualityCheckId);
            $("#txtQualityCheckNo").val(data.QualityCheckNo);
            $("#txtQualityCheckDate").val(data.QualityCheckDate); 
            $("#hdnGateInId").val(data.GateInId);
            $("#txtGateInNo").val(data.GateInNo);
            $("#txtGateInDate").val(data.GateInDate);          
            $("#txtPoNo").val(data.PONo);
            $("#hdnPOId").val(data.POId);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtRemarks").val(data.Remarks);
            $("#txtRejectRemark").val(data.RejectRemarks);
          

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

function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../QualityCheck/AddEditQualityCheck";
    }
}