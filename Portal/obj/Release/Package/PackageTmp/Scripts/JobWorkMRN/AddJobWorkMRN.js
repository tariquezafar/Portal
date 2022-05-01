$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    
    $("#txtJobWorkMRNNo").attr('readOnly', true);
    $("#txtJobWorkMRNDate").attr('readOnly', true);
    $("#txtJobWorkNo").attr('readOnly', true); 
   // $("#txtJobWorkMRNTime").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);    
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtMRNProductCode").attr('readOnly', true);
    $("#txtMRNUOMName").attr('readOnly', true);
    $("#txtMRNProductShortDesc").attr('readOnly', true); 
    $("#txtMRNProductHSNCode").attr('readOnly', true);
    $("#txtSearchFromDate,#txtSearchToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

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

    $("#txtJobWorkMRNDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $('#txtJobWorkMRNTime').datetimepicker({
        format: 'D-MMM-YYYY hh:mm a'
    });

    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnJobWorkMRNId = $("#hdnJobWorkMRNId");
    if (hdnJobWorkMRNId.val() != "" && hdnJobWorkMRNId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetJobWorkMRNDetail(hdnJobWorkMRNId.val());
           var jobWorkMRNProducts = [];
           GetJobWorkMRNProductList(jobWorkMRNProducts);
       }, 1000);
       


        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("#ddlCompanyBranch,#ddlJobWorkStatus").attr('disabled', true);
            $("#chkAllPrintChasis").attr('disabled', true);
            $(".chkChasis").attr('disabled', true);
            $(".editonly").hide();
            if ($(".editonly").hide()) {
                $('#lblWorkOrder').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
            }
        }
        else {
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
    $("#ddlCompanyBranch,#ddlSearchCompanyBranch").val(0);
    $("#ddlCompanyBranch,#ddlSearchCompanyBranch").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");
            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
                $("#ddlCompanyBranch").attr('disabled', true);
            }

        },
        error: function (Result) {
            $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}

function GetJobWorkMRNProductList(jobWorkMRNProducts) {
    var hdnJobWorkMRNId = $("#hdnJobWorkMRNId");
    var requestData = { jobWorkMRNProducts: jobWorkMRNProducts, jobWorkMRNId: hdnJobWorkMRNId.val() };
    $.ajax({
        url: "../JobWorkMRN/GetJobWorkMRNProductList",
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
            //ShowHideProductPanel(2);
            ShowHideProductMRNPanel(2);
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
    var hdnWorkOrderQuantity = $("#hdnWorkOrderQuantity");    
    var hdntotalRecivedFabQuantity = $("#hdnTotalRecivedFabQuantity");
    var hdnPendingQuantity = $("#hdnPendingQuantity");
  

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
    if (parseFloat(hdnPendingQuantity.val()) < parseFloat(txtQuantity.val())) {
        ShowModel("Alert", "Recived Quantity cannot be greater than Pending Quantity")
        txtQuantity.focus();
        return false;
    }
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    var fabricationProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var fabricationDetailId = $row.find("#hdnFabricationDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnRecivedQuantity").val();
        var pendingQuantity = $row.find("#hdnPendingQunatity").val();
        var totalRecivedFabQuantity = $row.find("#hdnTotalRecivedFabQuantity").val();
        var workOrderQuantity = $row.find("#hdnWorkOrderQuantity").val();
       

        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var fabricationProduct = {
                    FabricationDetailId: fabricationDetailId,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity,
                    PendingQuantity: pendingQuantity,
                    TotalRecivedFabQuantity: totalRecivedFabQuantity,
                    RecivedQuantity: quantity,
                    WorkOrderQuantity: workOrderQuantity,
                    

                };
                fabricationProductList.push(fabricationProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;

            }
            else if (hdnSequenceNo.val() == sequenceNo) {
                var fabricationProduct = {
                    FabricationDetailId: hdnFabricationDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),                 
                    TotalRecivedFabQuantity: hdntotalRecivedFabQuantity.val(),
                    RecivedQuantity: txtQuantity.val().trim(),
                    PendingQuantity: hdnPendingQuantity.val(),
                    WorkOrderQuantity: hdnWorkOrderQuantity.val(),
                  
                };
                fabricationProductList.push(fabricationProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var fabricationProductAddEdit = {
            FabricationDetailId: hdnFabricationDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            TotalRecivedFabQuantity: hdntotalRecivedFabQuantity.val(),
            RecivedQuantity: txtQuantity.val().trim(),
            PendingQuantity: hdnPendingQuantity.val(),
            WorkOrderQuantity: hdnWorkOrderQuantity.val(),
                   
        };
        fabricationProductList.push(fabricationProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetFabricationProductList(fabricationProductList);
    }

}
function EditProductRow(obj) {
    var row = $(obj).closest("tr");
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var fabricationDetailId = $(row).find("#hdnFabricationDetailId").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var Quantity = $(row).find("#hdnQuantity").val();
    var recivedQuantity = $(row).find("#hdnRecivedQuantity").val();
    var workOrderquantity = $(row).find("#hdnWorkOrderQuantity").val();
    var pendingQunatity = $(row).find("#hdnPendingQunatity").val();
    var totalRecivedFabQuantity = $(row).find("#hdnTotalRecivedFabQuantity").val();
    

    $("#txtProductName").val(productName);
    $("#hdnFabricationDetailId").val(fabricationDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);
    $("#txtQuantity").val(recivedQuantity);
    $("#txtPendingQuantity").val(pendingQunatity);
    $("#hdnPendingQuantity").val(pendingQunatity);
    $("#hdnQuantity").val(Quantity);
    
    $("#hdnWorkOrderQuantity").val(workOrderquantity);
    $("#hdnTotalRecivedFabQuantity").val(totalRecivedFabQuantity);



    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();

    ShowHideProductPanel(1);
}


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
        var hsn = $row.find("#hdnMRNProductHSNCode").val();
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
                    UomId: hdnUOMId,
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
                    UomId: hdnMRNuomId.val(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    Weight: txtMRNWeight.val(),
                    ProductHSNCode: txtMRNProductHSNCode.val().trim()

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
            ProductHSNCode: txtMRNProductHSNCode.val()
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

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var fabricationDetailId = $(row).find("#hdnFabricationDetailId").val();
        var productId = $(row).find("#hdnProductId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
        $('#tblPrintChasis tr:not(:has(th))').each(function (i, row) {
            var $row = $(row);
            var hdnProductID = $row.find("#hdnProductID").val();
            var chkPrintChasis = $(this).find("#chkPrintChasis");
            if (hdnProductID != undefined) {
                if (hdnProductID == productId) {
                    var row = $(this).closest("tr");
                    $(row).find(".hdnProductID").val(0);
                    $(row).find(".spnProductName").text("");
                    $(this).find(".chkChasis").prop('checked', false);
                }
            }
        });
    }
}

function GetJobWorkMRNDetail(jobWorkMRNId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../JobWorkMRN/GetJobWorkMRNDetail",
        data: { jobWorkMRNId: jobWorkMRNId },
        dataType: "json",
        success: function (data) {
            $("#txtJobWorkMRNNo").val(data.JobWorkMRNNo);
            $("#hdnJobWorkMRNId").val(data.JobWorkMRNId);
            $("#txtJobWorkMRNDate").val(data.JobWorkMRNDate);
            $("#txtJobWorkMRNTime").val(data.JobWorkMRNTime);
            $("#txtJobWorkNo").val(data.JobWorkNo);
            $("#hdnJobWorkId").val(data.JobWorkId);         
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#ddlApprovalStatus").val(data.JobWorkMRNStatus);
            if (data.JobWorkMRNStatus == "Final")
            {               
                $("#btnUpdate").hide();
                $(".editonly").hide();
                if ($(".editonly").hide()) {
                    $('#lblWorkOrder').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
                }
            }           
            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);
            $("#txtRemarks3").val(data.Remarks3);
            $("#txtRemarks4").val(data.Remarks4);
            $("#txtRemarks5").val(data.Remarks5);
            $("#txtRemarks6").val(data.Remarks6);
            $("#txtRemarks7").val(data.Remarks7);
            var jobOrderMrnProducts = [];
            GetJobOrderProductList(jobOrderMrnProducts);
            $("#btnAddNew").show();
            $("#btnPrint").show();

       

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}

function SaveData() {
    var txtJobWorkMRNNo = $("#txtJobWorkMRNNo");
    var hdnJobWorkMRNId = $("#hdnJobWorkMRNId");
    var txtJobWorkMRNDate = $("#txtJobWorkMRNDate");
    var txtJobWorkMRNTime = $("#txtJobWorkMRNTime");
    var hdnJobWorkId = $("#hdnJobWorkId");
    var txtJobWorkNo = $("#txtJobWorkNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtRemarks1 = $("#txtRemarks1");
    var txtRemarks2 = $("#txtRemarks2");
    var txtRemarks3 = $("#txtRemarks3");
    var txtRemarks4 = $("#txtRemarks4");
    var txtRemarks5 = $("#txtRemarks5");
    var txtRemarks6 = $("#txtRemarks6");
    var txtRemarks7 = $("#txtRemarks7");
   
    if (hdnJobWorkId.val() == "" || hdnJobWorkId.val() == 0) {
        ShowModel("Alert", "Please Select Job Work No.")
        txtJobWorkNo.focus();
        return false;
    }
    if (txtJobWorkMRNTime.val() == "" || txtJobWorkMRNTime.val() == 0) {
        ShowModel("Alert", "Please select  Job Work MRN Time")
        ddlCompanyBranch.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == 0) {
        ShowModel("Alert", "Please select  Company Branch")
        ddlCompanyBranch.focus();
        return false;
    }

   

    var jobWorkMRNViewModel = {
        JobWorkMRNId: hdnJobWorkMRNId.val(),
        JobWorkMRNNo: txtJobWorkMRNNo.val().trim(),
        JobWorkMRNDate: txtJobWorkMRNDate.val().trim(),
        JobWorkMRNTime: txtJobWorkMRNTime.val().trim(),
        JobWorkId: hdnJobWorkId.val(),
        JobWorkNo: txtJobWorkNo.val().trim(),
        CompanyBranchId: ddlCompanyBranch.val().trim(),
        JobWorkMRNStatus: ddlApprovalStatus.val(),
        Remarks1: txtRemarks1.val(),
        Remarks2: txtRemarks2.val(),
        Remarks3: txtRemarks3.val(),
        Remarks4: txtRemarks4.val(),
        Remarks5: txtRemarks5.val(),
        Remarks6: txtRemarks6.val(),
        Remarks7: txtRemarks7.val(),
        
    };

    var jobWorkMRNProductList = [];
    $('#tblMRNProductList tr').each(function (i, row) {
        var $row = $(row);
        var hdnMRNJobWorkProductInDetailId = $row.find("#hdnMRNJobWorkProductInDetailId").val();
        var productId = $row.find("#hdnMRNProductId").val();       
        var UomId = $row.find("#hdnMRNUOMId").val();
        var quantity = $row.find("#hdnMRNQuantity").val();
        var weight = $row.find("#hdnMRNWeight").val();
        if (productId != undefined) {
            
                var jobWorkMRNProduct = {
                    FabricationDetailId: hdnMRNJobWorkProductInDetailId,
                    ProductId: productId,                  
                    UomId: UomId,
                    Quantity: quantity,
                    Weight: weight,
                };
                jobWorkMRNProductList.push(jobWorkMRNProduct);
        }
    });

    if (jobWorkMRNProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product.")
        return false;
    }
  
    var accessMode = 1;//Add Mode
    if (hdnJobWorkMRNId.val() != null && hdnJobWorkMRNId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { jobWorkMRNViewModel: jobWorkMRNViewModel, jobWorkMRNProducts: jobWorkMRNProductList };
    $.ajax({
        url: "../JobWorkMRN/AddEditJobWorkMRN?AccessMode=" + accessMode + "",
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
                       window.location.href = "../JobWorkMRN/AddEditJobWorkMRN?jobWorkMRNId=" + data.trnId + "&AccessMode=3";
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

    $("#txtJobWorkMRNNo").val("");
    $("#hdnJobWorkMRNId").val("0");
    $("#txtJobWorkNo").val("");
    $("#hdnJobWorkId").val("0");
    $("#txtJobWorkMRNDate").val($("#hdnCurrentDate").val());
    $("#ddlApprovalStatus").val("Drafft");
    $("#ddlCompanyBranch").val("0");
    $("#txtRemarks1").val("");
    $("#txtRemarks2").val("");
    $("#txtRemarks3").val("");
    $("#txtRemarks4").val("");
    $("#txtRemarks5").val("");
    $("#txtRemarks6").val("");
    $("#txtRemarks7").val("");
   
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
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();



    }
}

function OpenWorkOrderSearchPopup() {
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

   
    $("#SearchWordOrderModel").modal();

}

function SearchJobOrder() {
    var txtSearchJobWorkNo = $("#txtSearchJobWorkNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
   
    var requestData = { jobWorkNo: txtSearchJobWorkNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../JobWorkMRN/GetJobWorkMRNJobOrderList",
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
function SelectJobOrder(jobWorkId, jobWorkNo,companyBranchId) {
    $("#txtJobWorkNo").val(jobWorkNo);
    $("#hdnJobWorkId").val(jobWorkId);  
    $("#ddlCompanyBranch").val(companyBranchId);
    var jobOrderMrnProducts = [];
    GetJobOrderProductList(jobOrderMrnProducts);
    var jobWorkINProducts = [];
    GetJobWorkINProductList(jobWorkINProducts);
    $("#SearchWordOrderModel").modal('hide');
}
function GetJobOrderProductList(jobOrderMrnProducts, workOrderId) {
    var hdnJobWorkId = $("#hdnJobWorkId");
    var requestData = { jobOrderMrnProducts: jobOrderMrnProducts, jobOrderId: hdnJobWorkId.val() };
    $.ajax({
        url: "../JobWorkMRN/GetJobWorkMRNJobOrderProductList",
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
        
            //ShowHideProductPanel(2);

        }
    });
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


function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../JobWorkMRN/AddEditJobWorkMRN";
    }
}

function GetJobWorkINProductList(jobWorkINProducts) {
    var hdnjobWorkId = $("#hdnJobWorkId");
    var requestData = { jobWorkProducts: jobWorkINProducts, jobworkId: hdnjobWorkId.val() };
    $.ajax({
        url: "../JobWorkMRN/GetJobWorkProductInList",
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