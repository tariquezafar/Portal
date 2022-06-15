$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    BindCompanyBranchList();
    $("#txtReturnedDate").css('cursor', 'pointer')
    $("#txtReturnedNo").attr('readOnly', true);
    $("#txtReturnedDate").attr('readOnly', true);
    $("#txtSaleInvoiceNo").attr('readOnly', true);
    $("#txtInvoicePackingListNo").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtProductShortDesc").attr('readOnly', true);
    $("#txtVendorCode").attr('readOnly', true);
    $("#txtMRNUOMName").attr('readOnly', true);
    $("#txtMRNProductCode").attr('readOnly', true);
    $("#txtProductHSNCode").attr('readOnly', true);
    $("#txtMRNProductHSNCode").attr('readOnly', true);


    $("#txtQuantity").attr('readOnly', true);
    $("#txtWarrantyPeriodMonth").attr('readOnly', true);
    $("#txtWarrantyStartDate").attr('readOnly', true);
    $("#txtWarrantyEndDate").attr('readOnly', true);

    
    $("#txtProductName").autocomplete({
        

        minLength: 0,
        source: function (request, response) {
            var warrantyId = $("#hdnWarrantyID").val();
            var ddlWarrantyStatus = $("#ddlWarrantyStatus").val();
            $.ajax({
                url: "../Return/GetProductAutoCompleteWarrantyList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, warrantyId: warrantyId,warrantyStatus: ddlWarrantyStatus},
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.ProductName, value: item.Productid, code: item.ProductCode, quantity: item.Quantity, warrantyPeriodMonth: item.WarrantyPeriodMonth,
                            warrantyStartDate: item.WarrantyStartDate, WarrantyEndDate: item.WarrantyEndDate
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
            $("#txtProductCode").val(ui.item.code);

            $("#txtQuantity").val(ui.item.quantity);
            $("#txtWarrantyPeriodMonth").val(ui.item.warrantyPeriodMonth);
            $("#txtWarrantyStartDate").val(ui.item.warrantyStartDate);
            $("#txtWarrantyEndDate").val(ui.item.WarrantyEndDate);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductName").val("");
                $("#hdnProductId").val("0");
                $("#txtProductShortDesc").val("");
                $("#txtProductCode").val("");

                $("#txtQuantity").val("");
                $("#txtWarrantyPeriodMonth").val("");
                $("#txtWarrantyStartDate").val("");
                $("#txtWarrantyEndDate").val("");
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


    $("#txtMRNProductName").autocomplete({
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
            $("#txtMRNProductName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtMRNProductName").val(ui.item.label);
            $("#hdnMRNProductId").val(ui.item.value);
            $("#txtMRNProductShortDesc").val(ui.item.desc);
            $("#txtMRNProductCode").val(ui.item.code);
            GetProductMRNDetail(ui.item.value);
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

    $("#txtjobWorkDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    $('#txtJobWorkTime').datetimepicker({
        format: 'D-MMM-YYYY hh:mm a'
   });

    
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnReturnedID = $("#hdnReturnedID");
    if (hdnReturnedID.val() != "" && hdnReturnedID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           $("#ddlCompanyBranch").attr('disabled', true);
           GetReturnDetail(hdnReturnedID.val());
       }, 3000);

        $("#ddlWarrantyStatus").attr('disabled', true);

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("#ddlCompanyBranch,#ddlJobWorkStatus").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $(".editonly").hide(); 
            $("#ddlApprovalStatus").attr('disabled', true);
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

    var returnProducts = [];
    GetReturnProductList(returnProducts);
    //var jobWorkMRNProducts = [];
    //GetJobWorkMRNProductList(jobWorkMRNProducts);
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
/* Job Order */
function GetReturnProductList(returnProducts, complaintId) {
    var hdnReturnedID = $("#hdnReturnedID");
    var ddlCompanyBranch = $("#ddlCompanyBranch").val();
    var requestData = { returnProducts: returnProducts, returnedID: hdnReturnedID.val(), complaintId: complaintId, companyBranch: ddlCompanyBranch };
    $.ajax({
        url: "../Return/GetReturnProductList",
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

/* Job Order  AddProduct */
function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var txtProductName = $("#txtProductName");
    var hdnReturnedDetailID = $("#hdnReturnedDetailID");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");   
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");

    var txtWarrantyPeriodMonth = $("#txtWarrantyPeriodMonth");
    var txtWarrantyStartDate = $("#txtWarrantyStartDate");
    var txtWarrantyEndDate = $("#txtWarrantyEndDate");
    var txtReplacementQuantity = $("#txtReplacementQuantity");
    var txtReturnedQTY = $("#txtReturnedQTY");
    
    var ddlStatus = $("#ddlStatus");
    var txtRemarks = $("#txtRemarks");

    var ddlWarrantyStatus = $("#ddlWarrantyStatus");




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
    if (txtReplacementQuantity.val().trim() == "" || txtReplacementQuantity.val().trim() == "0" || parseFloat(txtReplacementQuantity.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter Replacement Quantity")
        txtReplacementQuantity.focus();
        return false;
    }

    if (ddlWarrantyStatus.val() == 1) {
        if (parseFloat(txtReplacementQuantity.val()) > parseFloat(txtQuantity.val())) {
            ShowModel("Alert", "Replace Quantity Cannot be grater than Quantity")
            txtReplacementQuantity.focus();
            return false;
        }
    }

    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

   


    var returnProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var returnedDetailID = $row.find("#hdnReturnedDetailID").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();       
        var quantity = $row.find("#hdnQuantity").val();

        var warrantyPeriodMonth = $row.find("#hdnWarrantyPeriodMonth").val();
        var warrantyStartDate = $row.find("#hdnWarrantyStartDate").val();
        var warrantyEndDate = $row.find("#hdnWarrantyEndDate").val();
        var replacedQTY = $row.find("#hdnReplacedQTY").val();
        var returnedQTY = $row.find("#hdnReturnedQTY").val();
        
        var status = $row.find("#hdnStatus").val();
        var remarks = $row.find("#hdnRemarks").val();
     

        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var returnProduct = {
                                    
                    ReturnedDetailID: returnedDetailID,
                    SequenceNo:sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,                   
                    Quantity: quantity,
                    WarrantyPeriodMonth: warrantyPeriodMonth,
                    WarrantyStartDate: warrantyStartDate,
                    WarrantyEndDate: warrantyEndDate,
                    ReplacedQTY: replacedQTY,
                    ReturnedQTY: returnedQTY,
                    Status: status,
                    Remarks: remarks
                   
                };
                returnProductList.push(returnProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
                
            }
            else if (hdnSequenceNo.val()==sequenceNo)
            {
                var returnProduct = {
                    ReturnedDetailID: hdnReturnedDetailID.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),                   
                    Quantity: txtQuantity.val().trim(),
                    WarrantyPeriodMonth: txtWarrantyPeriodMonth.val(),
                    WarrantyStartDate: txtWarrantyStartDate.val().trim(),
                    WarrantyEndDate: txtWarrantyEndDate.val().trim(),
                    ReplacedQTY: txtReplacementQuantity.val().trim(),
                    ReturnedQTY: txtReturnedQTY.val().trim(),
                    Status: ddlStatus.val().trim(),
                    Remarks: txtRemarks.val().trim()
                   
               

                };
                returnProductList.push(returnProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var returnProductAddEdit = {
            ReturnedDetailID: hdnReturnedDetailID.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            Quantity: txtQuantity.val().trim(),
            WarrantyPeriodMonth: txtWarrantyPeriodMonth.val(),
            WarrantyStartDate: txtWarrantyStartDate.val().trim(),
            WarrantyEndDate: txtWarrantyEndDate.val().trim(),
            ReplacedQTY: txtReplacementQuantity.val().trim(),
            ReturnedQTY: txtReturnedQTY.val().trim(),
            Status: ddlStatus.val().trim(),
            Remarks: txtRemarks.val().trim()
          };
        returnProductList.push(returnProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetReturnProductList(returnProductList);
    }

}

function EditProductRow(obj) {

    var $row = $(obj).closest("tr");
    var returnedDetailID = $row.find("#hdnReturnedDetailID").val();
    var sequenceNo = $row.find("#hdnSequenceNo").val();
    var productId = $row.find("#hdnProductId").val();
    var productName = $row.find("#hdnProductName").val();
    var productCode = $row.find("#hdnProductCode").val();   
    var quantity = $row.find("#hdnQuantity").val();
    
    var warrantyPeriodMonth = $row.find("#hdnWarrantyPeriodMonth").val();
    var warrantyStartDate = $row.find("#hdnWarrantyStartDate").val();
    var warrantyEndDate = $row.find("#hdnWarrantyEndDate").val();
    var replacedQTY = $row.find("#hdnReplacedQTY").val();

    var returnedQTY = $row.find("#hdnReturnedQTY").val();
    var status = $row.find("#hdnStatus").val();
    var remarks = $row.find("#hdnRemarks").val();

    $("#txtProductName").val(productName);
    $("#hdnReturnedDetailID").val(returnedDetailID);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);    
    $("#txtQuantity").val(quantity);
    $("#txtReturnedQTY").val(returnedQTY);

    $("#txtWarrantyPeriodMonth").val(warrantyPeriodMonth);
    $("#txtWarrantyStartDate").val(warrantyStartDate);
    $("#txtWarrantyEndDate").val(warrantyEndDate);
    $("#txtReplacementQuantity").val(replacedQTY);

    $("#ddlStatus").val(status);
    $("#txtRemarks").val(remarks);

    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    
    ShowHideProductPanel(1);
}

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var workOrderProductDetailId = $(row).find("#hdnWorkOrderProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
    }
}


function GetJobWorkMRNProductList(jobOrderProducts) {
    var hdnjobWorkId = $("#hdnjobWorkId");
    var requestData = { jobWorkProducts: jobOrderProducts, jobworkId: hdnjobWorkId.val() };
    $.ajax({
        url: "../JobWork/GetJobWorkProductInList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divProductMRNList").html("");
            $("#divProductMRNList").html(err);
        },
        success: function (data) {
            $("#divProductMRNList").html("");
            $("#divProductMRNList").html(data);
            ShowHideProductMRNPanel(2);
        }
    });
}


/* Job Order MRN  AddProduct */
function AddMRNProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var txtProductName = $("#txtMRNProductName");
    var hdnJobWorkProductDetailId = $("#hdnJobWorkMRNProductDetailId");
    var hdnProductId = $("#hdnMRNProductId");
    var txtProductCode = $("#txtMRNProductCode");
    var txtProductShortDesc = $("#txtMRNProductShortDesc");
    var txtUOMName = $("#txtMRNUOMName");
    var hdnMRNuomId = $("#hdnMRNuomId");
    var txtQuantity = $("#txtMRNQuantity");
    var hdnSequenceNo = $("#hdnMRNSequenceNo");
    var txtMRNWeight = $("#txtMRNWeight");
    var txtMRNProductHSNCode = $("#txtMRNProductHSNCode");
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
        ShowModel("Alert", "Please enter Quantity")
        txtQuantity.focus();
        return false;
    }

    if (txtMRNWeight.val().trim() == "" || txtMRNWeight.val().trim() == "0" || parseFloat(txtMRNWeight.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter Weight")
        txtMRNWeight.focus();
        return false;
    }

    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    var jobWorkProductList = [];
    $('#tblMRNProductList tr').each(function (i, row) {
        var $row = $(row);
        var jobWorkProductDetailId = $row.find("#hdnMRNJobWorkProductInDetailId").val();
        var sequenceNo = $row.find("#hdnMRNSequenceNo").val();
        var productId = $row.find("#hdnMRNProductId").val();
        var productName = $row.find("#hdnMRNProductName").val();
        var productCode = $row.find("#hdnMRNProductCode").val();
        var productShortDesc = $row.find("#hdnMRNProductShortDesc").val();
        var hdnUOMId = $row.find("#hdnMRNUOMId").val();
        var uomName = $row.find("#hdnMRNUOMName").val();
        var quantity = $row.find("#hdnMRNQuantity").val();
        var weight = $row.find("#hdnMRNWeight").val();
        var hsn=$row.find("#hdnMRNProductHSNCode").val();
        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var jobWorkMRNProduct = {

                    JobWorkProductInDetailId: jobWorkProductDetailId,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UomId:hdnUOMId,
                    UOMName: uomName,
                    Quantity: quantity,
                    Weight: weight,
                    ProductHSNCode: hsn
                };
                jobWorkProductList.push(jobWorkMRNProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;

            }
            else if (hdnSequenceNo.val() == sequenceNo) {
                var jobWorkProduct = {
                    JobWorkProductInDetailId: hdnJobWorkProductDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UomId:hdnMRNuomId.val(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    Weight: txtMRNWeight.val(),
                    ProductHSNCode:txtMRNProductHSNCode.val().trim()
                  
                };
                jobWorkProductList.push(jobWorkProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var jobWorkProductAddEdit = {
            JobWorkProductInDetailId: hdnJobWorkProductDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UomId: hdnMRNuomId.val(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            Weight: txtMRNWeight.val(),
            ProductHSNCode:txtMRNProductHSNCode.val()
        };
        jobWorkProductList.push(jobWorkProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetJobWorkMRNProductList(jobWorkProductList);
    }

}

function EditMRNProductRow(obj) {
    var $row = $(obj).closest("tr");
    var jobWorkProductDetailId = $row.find("#hdnMRNJobWorkProductInDetailId").val();
    var sequenceNo = $row.find("#hdnMRNSequenceNo").val();
    var productId = $row.find("#hdnMRNProductId").val();
    var productName = $row.find("#hdnMRNProductName").val();
    var productCode = $row.find("#hdnMRNProductCode").val();
    var productShortDesc = $row.find("#hdnMRNProductShortDesc").val();
    var hdnUOMId = $row.find("#hdnMRNUOMId").val();
    var uomName = $row.find("#hdnMRNUOMName").val();
    var quantity = $row.find("#hdnMRNQuantity").val();
    var weight = $row.find("#hdnMRNWeight").val();
    var mRNUOMId = $row.find("#hdnMRNUOMId").val();
    $("#txtMRNProductName").val(productName);
    $("#hdnJobWorkMRNProductDetailId").val(jobWorkProductDetailId);
    $("#hdnMRNSequenceNo").val(sequenceNo);
    $("#hdnMRNProductId").val(productId);
    $("#txtMRNProductCode").val(productCode);
    $("#txtMRNProductShortDesc").val(productShortDesc);
    $("#txtMRNUOMName").val(uomName);
    $("#txtMRNQuantity").val(quantity);
    $("#txtMRNWeight").val(weight);
    $("#hdnMRNuomId").val(mRNUOMId);

    $("#btnAddMRNProduct").hide();
    $("#btnUpdateMRNProduct").show();

    ShowHideProductMRNPanel(1);
}

function RemoveMRNProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
    }
}


function GetReturnDetail(returnedID) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Return/GetReturnDetail",
        data: { returnedID: returnedID },
        dataType: "json",
        success: function (data) {
            $("#hdnReturnedID").val(data.ReturnedID);
            $("#txtReturnedNo").val(data.ReturnedNo);
            $("#txtReturnedDate").val(data.ReturnedDate);

            $("#hdnSaleInvoiceID").val(data.InvoiceID);
            $("#hdnWarrantyID").val(data.WarrantyID);
            $("#txtSaleInvoiceNo").val(data.InvoiceNo); 
            $("#txtInvoicePackingListNo").val(data.InvoicePackingListNo);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#ddlApprovalStatus").val(data.ApprovalStatus);
            $("#ddlWarrantyStatus").val(data.Warranty);

            if (data.ApprovalStatus == "Final")
            {
                $(".editonly").hide();
                $("#btnReset").hide();
                $("#btnUpdate").hide();
            }
            
           
            $("#btnAddNew").show();
            $("#btnPrint").show();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}

function SaveData() {
    var txtReturnedNo = $("#txtReturnedNo");
    var hdnReturnedID = $("#hdnReturnedID");
    var txtReturnedDate = $("#txtReturnedDate");
    var txtSaleInvoiceNo = $("#txtSaleInvoiceNo");
    var hdnSaleInvoiceID = $("#hdnSaleInvoiceID");    
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var ddlWarrantyStatus = $("#ddlWarrantyStatus");

  
    if (txtReturnedDate.val() == "" || txtReturnedDate.val() == 0) {
        ShowModel("Alert", "Please select Returned Date")
        txtReturnedDate.focus();
        return false;
    }
   
    if (txtSaleInvoiceNo.val() == "" || txtSaleInvoiceNo.val() == 0) {
        ShowModel("Alert", "Please select Sale Invoice No")
        txtSaleInvoiceNo.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == 0) {
        ShowModel("Alert", "Please select Company Branch")
        ddlCompanyBranch.focus();
        return false;
    }

    if (ddlWarrantyStatus.val() == "" || ddlWarrantyStatus.val() == 0) {
        ShowModel("Alert", "Please select Warranty Status");
        ddlWarrantyStatus.focus();
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdnReturnedID.val() != null && hdnReturnedID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var ddlWarrantyStatusText = $("#ddlWarrantyStatus option:selected").text();
    var returnViewModel = {
         ReturnedID: hdnReturnedID.val(),
         ReturnedDate: txtReturnedDate.val().trim(),       
         InvoiceID: hdnSaleInvoiceID.val(),
         InvoiceNo: txtSaleInvoiceNo.val().trim(),        
         CompanyBranchId: ddlCompanyBranch.val().trim(),         
         ApprovalStatus: ddlApprovalStatus.val(),
         Warranty: ddlWarrantyStatusText
    };

    var returnProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var ReturnedDetailID = $row.find("#hdnReturnedDetailID").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var quantity = $row.find("#hdnQuantity").val();

        var replacedQTY = $row.find("#hdnReplacedQTY").val();
        var returnedQTY = $row.find("#hdnReturnedQTY").val();
        var status = $row.find("#hdnStatus").val();
        var remarks = $row.find("#hdnRemarks").val();
            
        if (productId != undefined) {

            var returnProduct = {
                ReturnedDetailID: ReturnedDetailID,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,               
                Quantity: quantity,
                ReplacedQTY: replacedQTY,
                ReturnedQTY:returnedQTY,
                Status: status,
                Remarks: remarks
                
             };
            returnProductList.push(returnProduct);
        }
    });
   
    if (returnProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product.")
        return false;
    }
  
  

    var requestData = { returnViewModel: returnViewModel, returnedProducts: returnProductList };
    $.ajax({
        url: "../Return/AddEditReturn?AccessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                if (ddlApprovalStatus.val().trim() == "Final") {
                    setTimeout(
                       function () {
                           window.location.href = "../Return/AddEditReturn?returnedID=" + data.trnId + "&AccessMode=3";
                       }, 2000);
                }
                else
                {
                    setTimeout(
                       function () {
                           window.location.href = "../Return/AddEditReturn?returnedID=" + data.trnId + "&AccessMode=2";
                       }, 2000);
                }

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

    $("#txtReturnedNo").val("");
    $("#hdnReturnedID").val("0");
    $("#txtReturnedDate").val($("#hdnCurrentDate").val());
    $("#txtSaleInvoiceNo").val("");
    $("#hdnSaleInvoiceID").val("0");
    $("#hdnWarrantyID").val("0");
    $("#txtInvoicePackingListNo").val("");   
    $("#txtRemarks1").val("");
    $("#txtRemarks2").val("");
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
            $("#txtProductHSNCode").val(data.HSN_Code);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

function GetProductMRNDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productid: productId },
        dataType: "json",
        success: function (data) {
            $("#txtMRNUOMName").val(data.UOMName);
            $("#hdnMRNuomId").val(data.UOMId);
            $("#txtMRNProductHSNCode").val(data.HSN_Code);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function ShowHideProductPanel(action) {
    if (action == 1) {

        var ddlWarrantyStatus = $("#ddlWarrantyStatus");
        if (ddlWarrantyStatus.val() == "" || ddlWarrantyStatus.val() == 0) {
            ShowModel("Alert", "Please select Warranty Status")
            ddlWarrantyStatus.focus();
            return false;
        }
        else
        {
            $("#ddlWarrantyStatus").attr('disabled', true);
        }
      
        $(".productsection").show();
    }
    else {
        $(".productsection").hide();
        $("#txtProductName").val("");
        $("#hdnProductId").val("0");
        $("#hdnWorkOrderProductDetailId").val("0");
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtUOMName").val("");
        $("#txtQuantity").val("");
        $("#txtTotalValue").val("");
        $("#txtWarrantyPeriodMonth").val("");
        $("#txtWarrantyStartDate").val("");
        $("#txtWarrantyEndDate").val("");
        $("#txtReplacementQuantity").val("");

        $("#ddlStatus").val("Closed");
        $("#txtRemarks").val("");

        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();


        var rowCount = $('#tblProductList tr').length;
        if(rowCount==1)
        {
            $("#ddlWarrantyStatus").attr('disabled', false);
        }
        


    }
}

function ShowHideProductMRNPanel(action) {
    if (action == 1) {
        $(".productmrnsection").show();
    }
    else {
        $(".productmrnsection").hide();
        $("#hdnMRNSequenceNo").val("0");
        $("#txtMRNProductName").val("");
        $("#hdnMRNProductId").val("0");
        $("#hdnJobWorkMRNProductDetailId").val("0");
        $("#txtMRNProductCode").val("");
        $("#txtMRNProductShortDesc").val("");
        $("#txtMRNUOMName").val("");
        $("#txtMRNQuantity").val("");
        $("#txtMRNWeight").val("");
        $("#btnAddMRNProduct").show();
        $("#btnUpdateMRNProduct").hide();
    }
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
function ResetPage() {
    if (confirm("Are you Sure to Reset the page")) {
        window.location.href = "../Return/AddEditReturn";
    }
}

function OpenWorkOrderSearchPopup() {
    if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
        ShowModel("Alert", "Please Select Company Branch");
        return false;
    }
    $("#divList").html("");
    $("#SearchWordOrderModel").modal();

}

function OpenWorkOrderComplaintSearchPopup() {
    debugger
    if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
        ShowModel("Alert", "Please Select Company Branch");
        return false;
    }
    $("#divComplaintList").html("");
    $("#SearchComplaintWordOrderModel").modal();

}

function SearchSI() {
    var txtSearchSaleInvoiceNo = $("#txtSearchSaleInvoiceNo");
    var txtSearchInvoicePackingListNo = $("#txtSearchInvoicePackingListNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { saleInvoiceNo: txtSearchSaleInvoiceNo.val().trim(), invoicePackingListNo: txtSearchInvoicePackingListNo.val().trim(), companyBranchId: ddlCompanyBranch.val() };
    $.ajax({
        url: "../Return/GetSaleInvoiceReturnList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divList").html("");
            $("#divList").html(err);
        },
        success: function (data) {
            $("#divList").html("");
            $("#divList").html(data);
        }
    });
}

function SearchCI() {
    var txtSearchComplaintInvoiceNo = $("#txtSearchComplaintInvoiceNo");
    var txtSearchCustomerMobileNo = $("#txtSearchCustomerMobileNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { complaintInvoiceNo: txtSearchComplaintInvoiceNo.val().trim(), customerMobileNo: txtSearchCustomerMobileNo.val().trim(), companyBranchId: ddlCompanyBranch.val() };
    $.ajax({
        url: "../Return/GetComplaintInvoiceReturnList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divComplaintList").html("");
            $("#divComplaintList").html(err);
        },
        success: function (data) {
            $("#divComplaintList").html("");
            $("#divComplaintList").html(data);
        }
    });
}

function SelectSI(warrantyID, invoiceId, invoiceNo, invoicePackingListNo ) {
    $("#hdnWarrantyID").val(warrantyID);
    $("#hdnSaleInvoiceID").val(invoiceId);
    $("#txtSaleInvoiceNo").val(invoiceNo);
    $("#txtInvoicePackingListNo").val(invoicePackingListNo);
    $("#SearchWordOrderModel").modal('hide');
    $("#ddlCompanyBranch").attr('disabled', true);
}

function SelectCI(complaintId) {
    $("#SearchComplaintWordOrderModel").modal('hide');
    $("#ddlCompanyBranch").attr('disabled', true);
    GetReturnProductList(null, complaintId);

}