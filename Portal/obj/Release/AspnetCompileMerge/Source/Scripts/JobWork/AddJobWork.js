$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtjobWorkDate").css('cursor', 'pointer')
    $("#txtJobWorkNo").attr('readOnly', true);
    $("#txtjobWorkDate").attr('readOnly', true);
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
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode};
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

    BindCompanyBranchList();
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnjobWorkId = $("#hdnjobWorkId");
    if (hdnjobWorkId.val() != "" && hdnjobWorkId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetJobWorkDetail(hdnjobWorkId.val());
       }, 3000);



        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("#ddlCompanyBranch,#ddlJobWorkStatus").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
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

    var jobWorkProducts = [];
    GetJobWorkProductList(jobWorkProducts);
    var jobWorkMRNProducts = [];
    GetJobWorkMRNProductList(jobWorkMRNProducts);
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
function GetJobWorkProductList(jobOrderProducts) {
    var hdnjobWorkId = $("#hdnjobWorkId");
    var requestData = { jobWorkProducts: jobOrderProducts, jobworkId: hdnjobWorkId.val() };
    $.ajax({
        url: "../JobWork/GetJobWorkProductList",
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
    var hdnJobWorkProductDetailId = $("#hdnJobWorkProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtTotalValue = $("#txtTotalValue");
    var txtNatureOfProcessing = $("#txtNatureOfProcessing");
    var txtIdentificationMarks = $("#txtIdentificationMarks");
    var txtScrapPerc = $("#txtScrapPerc");
    var txtProductHSNCode = $("#txtProductHSNCode");



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
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    if (txtScrapPerc.val() != "") {
        if ((parseFloat(txtScrapPerc.val().trim()) < 0) || (txtScrapPerc.val() > 99)) {
            ShowModel("Alert", "Please Enter the correct Scrap Percentage between 0 to 99.")
            txtScrapPerc.focus();
            return false;
        }
    }
    



    var jobWorkProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var jobWorkProductDetailId = $row.find("#hdnJobWorkProductDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnQuantity").val();
        var natureOfProcessing = $row.find("#hdnNatureOfProcessing").val();
        var identificationMarks = $row.find("#hdnIdentificationMarks").val();
        var totalValue = $row.find("#hdnTotalValue").val();
        var scrapPerc = $row.find("#hdnScrapPerc").val();
        var productHSNCode = $row.find("#hdnProductHSNCode").val();

        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var jobWorkProduct = {
                                    
                    JobWorkProductDetailId: jobWorkProductDetailId,
                    SequenceNo:sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity,
                    TotalValue:totalValue,
                    ScrapPerc:scrapPerc,
                    NatureOfProcessing:natureOfProcessing,
                    IdentificationMarks: identificationMarks,
                    ProductHSNCode: productHSNCode
                };
                jobWorkProductList.push(jobWorkProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
                
            }
            else if (hdnSequenceNo.val()==sequenceNo)
            {
                var jobWorkProduct = {
                    JobWorkProductDetailId: hdnJobWorkProductDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    TotalValue: txtTotalValue.val(),
                    ScrapPerc: txtScrapPerc.val().trim(),
                    NatureOfProcessing: txtNatureOfProcessing.val().trim(),
                    IdentificationMarks: txtIdentificationMarks.val().trim(),
                    ProductHSNCode: txtProductHSNCode.val().trim()
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
            JobWorkProductDetailId: hdnJobWorkProductDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            TotalValue: txtTotalValue.val(),
            ScrapPerc: txtScrapPerc.val().trim(),
            NatureOfProcessing: txtNatureOfProcessing.val().trim(),
            IdentificationMarks: txtIdentificationMarks.val().trim(),
            ProductHSNCode: txtProductHSNCode.val().trim()
          };
        jobWorkProductList.push(jobWorkProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetJobWorkProductList(jobWorkProductList);
    }

}

function EditProductRow(obj) {

    var $row = $(obj).closest("tr");
    var jobWorkProductDetailId = $row.find("#hdnJobWorkProductDetailId").val();
    var sequenceNo = $row.find("#hdnSequenceNo").val();
    var productId = $row.find("#hdnProductId").val();
    var productName = $row.find("#hdnProductName").val();
    var productCode = $row.find("#hdnProductCode").val();
    var productShortDesc = $row.find("#hdnProductShortDesc").val();
    var uomName = $row.find("#hdnUOMName").val();
    var quantity = $row.find("#hdnQuantity").val();
    
    var natureOfProcessing = $row.find("#hdnNatureOfProcessing").val();
    var identificationMarks = $row.find("#hdnIdentificationMarks").val();
    var totalValue = $row.find("#hdnTotalValue").val();
    var scrapPerc = $row.find("#hdnScrapPerc").val();

    $("#txtProductName").val(productName);
    $("#hdnJobWorkProductDetailId").val(jobWorkProductDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);
    $("#txtQuantity").val(quantity);
    $("#txtTotalValue").val(totalValue);
    $("#txtNatureOfProcessing").val(natureOfProcessing);
    $("#txtIdentificationMarks").val(identificationMarks);
    $("#txtScrapPerc").val(scrapPerc);

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


function GetJobWorkDetail(jobWorkId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../JobWork/GetJobWorkDetail",
        data: { jobWorkId: jobWorkId },
        dataType: "json",
        success: function (data) {
            $("#txtJobWorkNo").val(data.JobWorkNo);
            $("#txtjobWorkDate").val(data.JobWorkDate);
            $("#txtJobWorkTime").val(data.JobWorkTime);
            $("#txtVendorName").val(data.VendorName);
            $("#hdnVendorId").val(data.VendorId);
            $("#txtVendorCode").val(data.VendorCode);
            $("#txtDestination").val(data.Destination);
            $("#txtMotorVehicleNo").val(data.MotorVehicleNo);
            $("#txtTransportName").val(data.TransportName);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#ddlJobWorkStatus").val(data.JobWorkStatus);

            if (data.JobWorkStatus == "Final")
            {
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

function SaveData() {
    var txtJobWorkNo = $("#txtJobWorkNo");
    var hdnjobWorkId = $("#hdnjobWorkId");
    var txtjobWorkDate = $("#txtjobWorkDate");
    var txtJobWorkTime = $("#txtJobWorkTime");
    var txtVendorName = $("#txtVendorName");
    var hdnVendorId = $("#hdnVendorId");
    var txtDestination = $("#txtDestination");
    var txtMotorVehicleNo = $("#txtMotorVehicleNo");
    var txtTransportName = $("#txtTransportName");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlJobWorkStatus = $("#ddlJobWorkStatus");
    var txtRemarks1 = $("#txtRemarks1");
    var txtRemarks2 = $("#txtRemarks2");
    if (txtjobWorkDate.val() == "" || txtjobWorkDate.val() == 0) {
        ShowModel("Alert", "Please select Job Work Date")
        txtjobWorkDate.focus();
        return false;
    }
    if (txtJobWorkTime.val() == "" || txtJobWorkTime.val() == 0) {
        ShowModel("Alert", "Please select Job Work Time")
        txtJobWorkTime.focus();
        return false;
    }
    if (txtVendorName.val() == "" || hdnVendorId.val() == 0) {
        ShowModel("Alert", "Please select Vendor Name")
        txtVendorName.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == 0) {
        ShowModel("Alert", "Please select Company Branch")
        ddlCompanyBranch.focus();
        return false;
    }

    var jobWorkViewModel = {
         JobWorkId:hdnjobWorkId.val(),
         JobWorkDate:txtjobWorkDate.val().trim(),
         JobWorkTime: txtJobWorkTime.val().trim(),
         VendorId:hdnVendorId.val(),
         Destination:txtDestination.val().trim(),
         MotorVehicleNo:txtMotorVehicleNo.val().trim(),
         TransportName:txtTransportName.val().trim(),
         CompanyBranchId: ddlCompanyBranch.val().trim(),
         Remarks1: txtRemarks1.val(),
         Remarks2: txtRemarks2.val(),
         JobWorkStatus:ddlJobWorkStatus.val()
    };

    var jobWorkProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var jobWorkProductDetailId = $row.find("#hdnJobWorkProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var natureOfProcessing = $row.find("#hdnNatureOfProcessing").val();
        var identificationMarks = $row.find("#hdnIdentificationMarks").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnQuantity").val();
        var totalValue = $row.find("#hdnTotalValue").val();
        var scrapPerc = $row.find("#hdnScrapPerc").val();
        

        if (productId != undefined) {

            var jobWorkProduct = {
                JobWorkProductDetailId: jobWorkProductDetailId,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc,
                UOMName: uomName,
                Quantity: quantity,
                TotalValue:totalValue,
                ScrapPerc:scrapPerc,
                NatureOfProcessing:natureOfProcessing,
                IdentificationMarks: identificationMarks
             };
            jobWorkProductList.push(jobWorkProduct);
        }
    });
    var jobWorkINProductList = [];
    $('#tblMRNProductList tr').each(function (i, row) {
        var $row = $(row);
        var jobWorkProductDetailId = $row.find("#hdnMRNJobWorkProductInDetailId").val();
        var productId = $row.find("#hdnMRNProductId").val();
        var hdnUOMId = $row.find("#hdnMRNUOMId").val();
        var quantity = $row.find("#hdnMRNQuantity").val();
        var weight = $row.find("#hdnMRNWeight").val();

        if (productId != undefined) {
                var jobWorkMRNProduct = {
                    JobWorkProductInDetailId: jobWorkProductDetailId,
                    ProductId: productId,
                    Quantity: quantity,
                    Weight: weight,
                    UomId: hdnUOMId
                };
                jobWorkINProductList.push(jobWorkMRNProduct);
            }
    });

    if (jobWorkProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product.")
        return false;
    }
    if (jobWorkINProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 MRN Product.")
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdnjobWorkId.val() != null && hdnjobWorkId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { jobWorkViewModel: jobWorkViewModel, jobWorkProducts: jobWorkProductList, jobWorkINProducts: jobWorkINProductList };
    $.ajax({
        url: "../JobWork/AddEditJobWork?AccessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                if (ddlJobWorkStatus.val().trim() == "Final") {
                    setTimeout(
                       function () {
                           window.location.href = "../JobWork/AddEditJobWork?jobWorkId=" + data.trnId + "&AccessMode=3";
                       }, 2000);
                }
                else
                {
                    setTimeout(
                       function () {
                           window.location.href = "../JobWork/AddEditJobWork?jobWorkId=" + data.trnId + "&AccessMode=2";
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

    $("#txtJobWorkNo").val("");
    $("#hdnjobWorkId").val("0");
    $("#txtjobWorkDate").val($("#hdnCurrentDate").val());
    $("#txtJobWorkTime").val($("").val());
    $("#txtVendorName").val("");
    $("#hdnVendorId").val("0");
    $("#txtVendorCode").val("");
    $("#txtDestination").val("");
    $("#txtMotorVehicleNo").val("");
    $("#txtTransportName").val("");
    $("#ddlCompanyBranch").val("0");
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
        $("#txtNatureOfProcessing").val("");
        $("#txtIdentificationMarks").val("");
        $("#txtScrapPerc").val("");
        $("#txtProductHSNCode").val("");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
        


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
        window.location.href = "../JobWork/AddEditJobWork";
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