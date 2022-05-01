$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtPendingQuantity").attr('readOnly', true);
    $("#txtFinishedGoodProcessNo").attr('readOnly', true);
    $("#txtFinishedGoodProcessDate").attr('readOnly', true);
    $("#txtWorkOrderNo").attr('readOnly', true);
    $("#txtWorkOrderQuantity").attr('readOnly', true);
    $("#txtPaintProcessNo").attr('readOnly', true);
    $("#txtPaintProcessQuantity").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtAssemblingProcessNo").attr('readOnly', true);
    $("#txtAssemblingProcessQuantity").attr('readOnly', true);
   
    $("#txtWorkOrderDate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtProductShortDesc").attr('readOnly', true); 
    $("#txtAssemblyProcessDate").attr('readOnly', true);
    $("#txtSearchFromDate,#txtSearchToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

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

    $("#txtFinishedGoodProcessDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    BindCompanyBranchList();
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnFinishedGoodProcessId = $("#hdnFinishedGoodProcessId");
    if (hdnFinishedGoodProcessId.val() != "" && hdnFinishedGoodProcessId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetFinishedGoodProcessDetail(hdnFinishedGoodProcessId.val());
           var finishedProcessChasisSerialProducts = [];
           GetFinishedProcessChasisSerialProductList(finishedProcessChasisSerialProducts);
           $("#ddlCompanyBranch").attr('disabled', true);
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
            if ($(".editonly").hide()) {
                $('#lblWorkOrder').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
            }
            $("#txtCancelReason").attr('readOnly', false);
            
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
        var productSerialProducts = [];
        GetProductSerialProductList(productSerialProducts);
       
    }

    var finishedGoodProcessProducts = [];
    GetFinishedGoodProcessProductList(finishedGoodProcessProducts);
    
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

function GetFinishedProcessChasisSerialProductList(finishedProcessChasisSerialProducts) {
    var hdnFinishedGoodProcessId = $("#hdnFinishedGoodProcessId");
    var requestData = { finishedProcessChasisSerialProducts: finishedProcessChasisSerialProducts, finishedGoodProcessId: hdnFinishedGoodProcessId.val() };
    $.ajax({
        url: "../FinishedGoodProcess/GetFinishedGoodProductSerialProductList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divChasisSerial").html("");
            $("#divChasisSerial").html(err);
        },
        success: function (data) {
            $("#divChasisSerial").html("");
            $("#divChasisSerial").html(data);
            chkedChechbox();
        }
    });
}
function chkedChechbox() {
    $('#tblPrintChasis tr').each(function (i, row) {
        var $row = $(row);
        var hdnChked = $row.find("#hdnChked").val();
        var chkPrintChasis = $(this).find("#chkPrintChasis");
        if (hdnChked == 1) {
            chkPrintChasis.attr("checked", true)
        }


    });
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
                $("#ddlSearchCompanyBranch,#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
                $("#ddlSearchCompanyBranch,#ddlCompanyBranch").attr('disabled', true);
            }
        },
        error: function (Result) {
            $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}

function GetFinishedGoodProcessProductList(finishedGoodProcessProducts) {
    var hdnFinishedGoodProcessId = $("#hdnFinishedGoodProcessId");
    var requestData = { finishedGoodProcessProducts: finishedGoodProcessProducts, finishedGoodProcessId: hdnFinishedGoodProcessId.val() };
    $.ajax({
        url: "../FinishedGoodProcess/GetFinishedGoodProcessProductList",
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

function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var txtProductName = $("#txtProductName");
    var hdnFinishedGoodProcessDetailId = $("#hdnFinishedGoodProcessDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");

    var hdnTotalAssembledQuantity = $("#hdnTotalAssembledQuantity");
    var hdnTotalRecivedFinishedGoodQuantity = $("#hdnTotalRecivedFinishedGoodQuantity");
    var hdnPendingQuantity = $("#hdnPendingQuantity");

    var hdnIsThirdPartyProduct = $("#hdnIsThirdPartyProduct");

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

    var finishedGoodProcessProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var finishedGoodProcessDetailId = $row.find("#hdnFinishedGoodProcessDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnRecivedQuantity").val();
        var pendingQuantity = $row.find("#hdnPendingQunatity").val();
        var totalRecivedFinishedGoodQuantity = $row.find("#hdnTotalRecivedFinishedGoodQuantity").val();
        var totalAssembledQuantity = $row.find("#hdnTotalAssembledQuantity").val();
        var isThirdPartyProduct = $row.find("#hdnIsThirdPartyProduct").val();
        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var finishedGoodProcessProduct = {
                    FinishedGoodProcessDetailId: finishedGoodProcessDetailId,
                    SequenceNo:sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity,
                    PendingQuantity: pendingQuantity,
                    RecivedQuantity: quantity,
                    TotalAssembledQuantity: totalAssembledQuantity,
                    TotalRecivedFinishedGoodQuantity: totalRecivedFinishedGoodQuantity,
                    IsThirdPartyProduct: isThirdPartyProduct,
                  
                };
                finishedGoodProcessProductList.push(finishedGoodProcessProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
                
            }
            else if (hdnFinishedGoodProcessDetailId.val() == finishedGoodProcessDetailId && hdnSequenceNo.val() == sequenceNo)
            {
                var finishedGoodProcessProduct = {
                    FinishedGoodProcessDetailId: hdnFinishedGoodProcessDetailId.val(),
                    SequenceNo:hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    RecivedQuantity: txtQuantity.val().trim(),
                    PendingQuantity: hdnPendingQuantity.val(),
                    TotalAssembledQuantity: hdnTotalAssembledQuantity.val(),
                    TotalRecivedFinishedGoodQuantity: hdnTotalRecivedFinishedGoodQuantity.val(),
                    IsThirdPartyProduct: hdnIsThirdPartyProduct.val(),
                };
                finishedGoodProcessProductList.push(finishedGoodProcessProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var finishedGoodProcessProductAddEdit = {
            FinishedGoodProcessDetailId: hdnFinishedGoodProcessDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            RecivedQuantity: txtQuantity.val().trim(),
            PendingQuantity: hdnPendingQuantity.val(),
            TotalAssembledQuantity: hdnTotalAssembledQuantity.val(),
            TotalRecivedFinishedGoodQuantity: hdnTotalRecivedFinishedGoodQuantity.val(),
            IsThirdPartyProduct: hdnIsThirdPartyProduct.val(),
          };
        finishedGoodProcessProductList.push(finishedGoodProcessProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetFinishedGoodProcessProductList(finishedGoodProcessProductList);
    }

}
function EditProductRow(obj) {
    var row = $(obj).closest("tr");
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var finishedGoodProcessDetailId = $(row).find("#hdnFinishedGoodProcessDetailId").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var Quantity = $(row).find("#hdnQuantity").val();
    var recivedQuantity = $(row).find("#hdnRecivedQuantity").val();
    var assembledQuantity = $(row).find("#hdnTotalAssembledQuantity").val();
    var pendingQunatity = $(row).find("#hdnPendingQunatity").val();
    var totalRecivedFinishedGoodQuantity = $(row).find("#hdnTotalRecivedFinishedGoodQuantity").val();
    var isThirdPartyProduct = $(row).find("#hdnIsThirdPartyProduct").val();

    $("#txtProductName").val(productName);
    $("#hdnFinishedGoodProcessDetailId").val(finishedGoodProcessDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);


    $("#hdnIsThirdPartyProduct").val(isThirdPartyProduct);

    $("#txtQuantity").val(recivedQuantity);
    $("#txtPendingQuantity").val(pendingQunatity);
    $("#hdnPendingQuantity").val(pendingQunatity);
    $("#hdnQuantity").val(Quantity);

    $("#hdnTotalAssembledQuantity").val(assembledQuantity);
    $("#hdnTotalRecivedFinishedGoodQuantity").val(totalRecivedFinishedGoodQuantity);
    
    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    
    ShowHideProductPanel(1);
}

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var finishedGoodProcessDetailId = $(row).find("#hdnFinishedGoodProcessDetailId").val();
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
var chekedstatus = "";
function GetFinishedGoodProcessDetail(finishedGoodProcessId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../FinishedGoodProcess/GetFinishedGoodProcessDetail",
        data: { finishedGoodProcessId: finishedGoodProcessId },
        dataType: "json",
        success: function (data) {
            $("#txtFinishedGoodProcessNo").val(data.FinishedGoodProcessNo);
            $("#hdnFinishedGoodProcessId").val(data.FinishedGoodProcessId);
            $("#txtFinishedGoodProcessDate").val(data.FinishedGoodProcessDate);
            $("#txtWorkOrderNo").val(data.WorkOrderNo);
            $("#hdnWorkOrderId").val(data.WorkOrderId);
            $("#txtWorkOrderQuantity").val(data.TotalQuantity);
            $("#txtAssemblingProcessNo").val(data.AssemblingProcessNo);
            $("#hdnAssemblingProcessId").val(data.AssemblingProcessId);           
            $("#ddlCompanyBranch").val(data.CompanyBranchId);            
            $("#ddlFinishedGoodProcessStatus").val(data.FinishedGoodProcessStatus);
            //$("#divCreated").show();
            //$("#txtCreatedBy").val(data.CreatedByUserName);
            //$("#txtCreatedDate").val(data.CreatedDate);
            //if (data.ModifiedByUserName != "") {
            //    $("#divModified").show();
            //    $("#txtModifiedBy").val(data.ModifiedByUserName);
            //    $("#txtModifiedDate").val(data.ModifiedDate);
            //}
            if (data.FinishedGoodProcessStatus == "Final")
            {
                chekedstatus = data.FinishedGoodProcessStatus;
                $(".editonly").hide();
                $("#btnUpdate").hide();
                $("#btnReset").hide();
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);
                if ($(".editonly").hide()) {
                    $('#lblWorkOrder').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
                }
                $(".chkPrintChasis").attr('disabled', true);
                $("#txtCancelReason").attr('readOnly', false);
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
    var txtFinishedGoodProcessNo = $("#txtFinishedGoodProcessNo");
    var hdnFinishedGoodProcessId = $("#hdnFinishedGoodProcessId");
    var txtFinishedGoodProcessDate = $("#txtFinishedGoodProcessDate");
    var txtAssemblingProcessNo = $("#txtAssemblingProcessNo");
    var hdnAssemblingProcessId = $("#hdnAssemblingProcessId");
    var txtWorkOrderNo = $("#txtWorkOrderNo");
    var hdnWorkOrderId = $("#hdnWorkOrderId");    
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlFinishedGoodProcessStatus = $("#ddlFinishedGoodProcessStatus");
    var txtRemarks1 = $("#txtRemarks1");
    var txtRemarks2 = $("#txtRemarks2");
    
    if (hdnAssemblingProcessId.val() == "" || hdnAssemblingProcessId.val() == 0) {
        ShowModel("Alert", "Please Select Assembling Process No.")
        txtAssemblingProcessNo.focus();
        return false;
    }


    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == 0) {
        ShowModel("Alert", "Please select  Location")
        ddlCompanyBranch.focus();
        return false;
    }

    var finishedGoodProcessViewModel = {
        FinishedGoodProcessId: hdnFinishedGoodProcessId.val(),
        FinishedGoodProcessNo: txtFinishedGoodProcessNo.val().trim(),
        FinishedGoodProcessDate: txtFinishedGoodProcessDate.val().trim(),
        AssemblingProcessId: hdnAssemblingProcessId.val(),
        AssemblingProcessNo: txtAssemblingProcessNo.val().trim(),
        WorkOrderId: hdnWorkOrderId.val(),
        WorkOrderNo: txtWorkOrderNo.val().trim(),              
        CompanyBranchId: ddlCompanyBranch.val().trim(),
        FinishedGoodProcessStatus: ddlFinishedGoodProcessStatus.val(),
        Remarks1: txtRemarks1.val(),
        Remarks2: txtRemarks2.val()
    };


    var finishedGoodProcessChasisSerialList = [];
    $('#tblPrintChasis tr').each(function (i, row) {
        var $row = $(row);
        var productId = $row.find("#hdnProductID").val();
        var chasisSerialNo = $row.find("#hdnChasisSerialNo").val();
        var motorNo = $row.find("#hdnMotorNo").val();
        var chkPrintChasis = $row.find("#chkPrintChasis").is(':checked') ? true : false;
        if (productId != undefined) {
            if (chkPrintChasis == true) {
                var finishedGoodProcessChasisSerial = {
                    ProductId: productId,
                    ChasisSerialNo: chasisSerialNo.trim(),
                    MotorNo: motorNo.trim()
                };
                finishedGoodProcessChasisSerialList.push(finishedGoodProcessChasisSerial);
            }
        }
    });

    var totalQuantity = 0;
    var checkedFlag = false;
    var finishedGoodProcessProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var finishedGoodProcessDetailId = $row.find("#hdnFinishedGoodProcessDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnRecivedQuantity").val();
        if (productId != undefined) {
            if (parseFloat(quantity) > 0) {
                totalQuantity += parseFloat(quantity);
                checkedFlag = true;

            }
            if (ddlFinishedGoodProcessStatus.val() == "Final") {
                if (parseFloat(quantity) > 0) {
                    var finishedGoodProcessProduct = {
                        FinishedGoodProcessDetailId: finishedGoodProcessDetailId,
                        ProductId: productId,
                        ProductName: productName,
                        ProductCode: productCode,
                        ProductShortDesc: productShortDesc,
                        UOMName: uomName,
                        Quantity: quantity
                    };
                    finishedGoodProcessProductList.push(finishedGoodProcessProduct);
                }
            }
            else {
                var finishedGoodProcessProduct = {
                    FinishedGoodProcessDetailId: finishedGoodProcessDetailId,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity
                };
                finishedGoodProcessProductList.push(finishedGoodProcessProduct);
            }
        }
    });

    if (finishedGoodProcessProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product.")
        return false;
    }

    if (checkedFlag == false) {
        ShowModel("Alert", "Please Select at least one Product as the Received Quantity.")
        return false;
    }
    var flag = true;
   
        $('#tblProductList tr:not(:has(th))').each(function (i, row) {
            var count = 0;
            var $row = $(row);
            var productId = $row.find("#hdnProductId").val();
            var quantity = $row.find("#hdnRecivedQuantity").val();
            var hdnIsThirdPartyProduct = $row.find("#hdnIsThirdPartyProduct").val();

            if (hdnIsThirdPartyProduct.toUpperCase() != "TRUE") {
                if (productId != undefined) {

                    $('#tblPrintChasis tr:not(:has(th))').each(function (i, row) {
                        var $row = $(row);
                        var printproductId = $row.find("#hdnProductID").val();
                        if (printproductId != undefined) {
                            if (productId == printproductId) {
                                count = count + 1;
                            }
                        }
                    });
                    if (count != quantity) {
                        flag = false;
                    }

                }
            }
        });
   
     if (flag == false) {
        ShowModel("Alert", "Number of Chassis/Motor serial selected is not equal to the Received Quantity.")
        return false;
    }

    if (finishedGoodProcessProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product.")
        return false;
    }
    var accessMode = 1;//Add Mode
    if (hdnFinishedGoodProcessId.val() != null && hdnFinishedGoodProcessId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = {
        finishedGoodProcessViewModel: finishedGoodProcessViewModel,
        finishedGoodProcessProducts: finishedGoodProcessProductList, finishedGoodProcessChasisSerialList: finishedGoodProcessChasisSerialList
    };
    $.ajax({
        url: "../FinishedGoodProcess/AddEditFinishedGoodProcess?accessMode=" + accessMode + "",
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
                       window.location.href = "../FinishedGoodProcess/AddEditFinishedGoodProcess?finishedGoodProcessId=" + data.trnId + "&AccessMode=3";
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

    $("#txtFinishedGoodProcessNo").val("");
    $("#hdnFinishedGoodProcessId").val("0");
    $("#txtWorkOrderNo").val("");
    $("#hdnWorkOrderId").val("0");
    $("#txtFinishedGoodProcessDate").val($("#hdnCurrentDate").val());
    $("#ddlFinishedGoodProcessStatus").val("Final");
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
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

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
    $("#SearchWordOrderModel").modal();

}

function SearchWorkOrder() {
    var txtWorkOrderNo = $("#txtSearchWorkOrderNo");
    var ddlCompanyBranch = $("#ddlSearchCompanyBranch");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var requestData = { workOrderNo: txtWorkOrderNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../FinishedGoodProcess/GetFinishedGoodProcessWorkOrderList",
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
function SelectWorkOrder(workOrderId, workOrderNo, workOrderDate, quantity, companyBranchId) {
    $("#txtWorkOrderNo").val(workOrderNo);
    $("#hdnWorkOrderId").val(workOrderId);
    $("#txtWorkOrderDate").val(workOrderDate);
    $("#txtWorkOrderQuantity").val(quantity);
    $("#ddlCompanyBranch").val(companyBranchId);
    var finishedGoodProcessProducts = [];
    GetWorkOrderProductList(finishedGoodProcessProducts);
    $("#SearchWordOrderModel").modal('hide');
}
function GetWorkOrderProductList(finishedGoodProcessProducts, workOrderId) {
    var hdnWorkOrderId = $("#hdnWorkOrderId");
    var requestData = { finishedGoodProcessProducts: finishedGoodProcessProducts, workOrderId: hdnWorkOrderId.val() };
    $.ajax({
        url: "../FinishedGoodProcess/GetWorkOrderProductList",
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


function OpenPaintProcessSearchPopup() {
    if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
        ShowModel("Alert", "Please Select Company Branch");
        return false;
    }
    $("#divListPaintProcess").html("")
    $("#SearchPaintProcessModel").modal();

}

function SearchAssemblingProcess() {
    var txtAssemblingProcessNo = $("#txtSearchAssemblingProcessNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");//$("#ddlSearchCompanyBranch");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var requestData = { assemblingProcessNo: txtAssemblingProcessNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../FinishedGoodProcess/GetFinishedGoodAssemblingProcessList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divListPaintProcess").html("");
            $("#divListPaintProcess").html(err);
        },
        success: function (data) {
            $("#divListPaintProcess").html("");
            $("#divListPaintProcess").html(data);
        }
    });
}
function SelectAssemblingProcess(assemblingProcessId, assemblingProcessNo, paintProcessNo, workOrderId, workOrderNo, paintProcessquantity, workOrderquantity, companyBranchId,assemblingProcessDate) {
    $("#hdnAssemblingProcessId").val(assemblingProcessId);
    $("#txtAssemblingProcessNo").val(assemblingProcessNo);
    $("#txtPaintProcessNo").val(paintProcessNo); 
    $("#hdnWorkOrderId").val(workOrderId);
    $("#txtWorkOrderNo").val(workOrderNo);
    $("#txtPaintProcessQuantity").val(paintProcessquantity);
    $("#txtAssemblingProcessQuantity").val(paintProcessquantity);
    $("#txtWorkOrderQuantity").val(workOrderquantity);
    $("#ddlCompanyBranch").val(companyBranchId);
    $("#ddlCompanyBranch").attr('disabled', true);
    $("#txtAssemblyProcessDate").val(assemblingProcessDate);
    var assemblingProcessProducts = [];
    GetPaintProcessProductList(assemblingProcessProducts);
    $("#SearchPaintProcessModel").modal('hide');
}
function GetPaintProcessProductList(assemblingProcessProducts, workOrderId) {
    var hdnAssemblingProcessId = $("#hdnAssemblingProcessId");
    var requestData = { assemblingProcessProducts: assemblingProcessProducts, assemblingProcessId: hdnAssemblingProcessId.val() };
    $.ajax({
        url: "../FinishedGoodProcess/GetAssemblingProcessProductList",
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
            var productSerialProducts = [];
            GetProductSerialProductList(productSerialProducts);
            ShowHideProductPanel(2);
        }
    });
}

function OpenPrintPopup(productId, productName, quantity) {

    if (quantity == 0) {
        ShowModel("Alert", "Please Enter Quantity first")
        return false;

    }

    else
    {

    $("#hdnTempProductID").val(productId);
    $("#hdnTempProductName").val(productName);
    $("#hdnTempQuantity").val(quantity);
    $("#SearchChasisSerialModel").modal();

    $('#tblPrintChasis tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var hdnProductID = $row.find("#hdnProductID").val();
        var chkPrintChasis = $(this).find("#chkPrintChasis");
        if (hdnProductID != undefined) {
            if (hdnProductID == 0) {
                chkPrintChasis.attr('disabled', false);             
            }
            else if (hdnProductID == productId) {
             
                chkPrintChasis.attr('disabled', false);
            }
            else if (hdnProductID != productId && hdnProductID != 0) {
                chkPrintChasis.attr('disabled', true);
            }
        }
    });
    $('#tblPrintChasis tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var hdnMatchProductID = $row.find("#hdnMatchProductID").val();
        var chkPrintChasis = $(this).find("#chkPrintChasis");
        if (hdnMatchProductID != undefined) {
            if (hdnMatchProductID == 0) {
                chkPrintChasis.attr('disabled', false);
            }
            else if (hdnMatchProductID == productId) {

                chkPrintChasis.attr('disabled', false);
            }
            else if (hdnMatchProductID != productId && hdnMatchProductID != 0) {
                chkPrintChasis.attr('disabled', true);
            }
        }
    });

    if (chekedstatus == "Final") {
        $(".chkChasis").attr('disabled', true);
    }
    }


}
function GetProductSerialProductList(productSerialProducts) {
    var requestData = {};
    $.ajax({
        url: "../FinishedGoodProcess/GetProductSerialProductList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divChasisSerial").html("");
            $("#divChasisSerial").html(err);
        },
        success: function (data) {
            $("#divChasisSerial").html("");
            $("#divChasisSerial").html(data);


        }
    });
}

function CancelFGP() {
    if ($("#txtCancelReason").val() == " ") {
        ShowModel("Alert", "Please Enter Cancel Reason.")
        txtCancelReason.focus();
        return false;
    }
    if (confirm("Are You Sure to Cancel this Finished Good Process ?")) {

        var hdnFinishedGoodProcessId = $("#hdnFinishedGoodProcessId");
        var txtFinishedGoodProcessNo = $("#txtFinishedGoodProcessNo");
        var txtCancelReason = $("#txtCancelReason");
        var ddlCompanyBranch = $("#ddlCompanyBranch");

        var finishedGoodProcessProductList = [];
        $('#tblProductList tr').each(function (i, row) {
            var $row = $(row);
            var finishedGoodProcessDetailId = $row.find("#hdnFinishedGoodProcessDetailId").val();
            var productId = $row.find("#hdnProductId").val();
            var productName = $row.find("#hdnProductName").val();
            var productCode = $row.find("#hdnProductCode").val();
            var productShortDesc = $row.find("#hdnProductShortDesc").val();
            var uomName = $row.find("#hdnUOMName").val();
            var quantity = $row.find("#hdnRecivedQuantity").val();
            if (productId != undefined) {
                
                        var finishedGoodProcessProduct = {
                            FinishedGoodProcessDetailId: finishedGoodProcessDetailId,
                            ProductId: productId,
                            ProductName: productName,
                            ProductCode: productCode,
                            ProductShortDesc: productShortDesc,
                            UOMName: uomName,
                            Quantity: quantity
                        };
                        finishedGoodProcessProductList.push(finishedGoodProcessProduct);                                  
            }
        });

        var requestData = {
            finishedGoodProcessId: hdnFinishedGoodProcessId.val(),
            finishedGoodProcessNo: txtFinishedGoodProcessNo.val().trim(),
            companyBranchId: ddlCompanyBranch.val(),
            cancelReason: txtCancelReason.val().trim(),
            finishedGoodProcessProducts: finishedGoodProcessProductList
        };
        $.ajax({
            url: "../FinishedGoodProcess/CancelFGP",
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
                           window.location.href = "../FinishedGoodProcess/ListFinishedGoodProcess";
                       }, 2000);

                    $("#btnAP").hide();
                    $("#btnList").show();
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
}