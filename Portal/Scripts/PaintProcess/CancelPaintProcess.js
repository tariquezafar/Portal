$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#btnAddNew").hide();
    $("#btnPrint").hide();
   
    $("#btnAddNewProduct").attr('disabled', true);
    $("#erp-btn").attr('disabled', true);
    $("#txtRemarks1").attr('readOnly', true);
    $("#txtRemarks2").attr('readOnly', true);
    $("#txtWorkOrderDate").attr('disabled', true);
    $("#btnSearchWorkOrder").attr('disabled', true);
    $("#txtAdjQTY").attr('readOnly', true);
    $("#txtPendingQuantity").attr('readOnly', true);
    $("#txtPaintProcessNo").attr('readOnly', true);
    $("#txtPaintProcessDate").attr('readOnly', true);
    $("#txtWorkOrderNo").attr('readOnly', true);
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

    //$("#txtPaintProcessDate").datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat: 'dd-M-yy',
    //    yearRange: '-10:+100',
    //    onSelect: function (selected) {

    //    }
    //});


    $("#txtPaintProcessDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {
            // $("#txtRequisitionDate").datepicker("option", "maxDate", selected);
        }
    });
    $("#txtWorkOrderDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            $("#txtPaintProcessDate").datepicker("option", "minDate", selected);
        }
    });

    BindCompanyBranchList();
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPaintProcessId = $("#hdnPaintProcessId");
    if (hdnPaintProcessId.val() != "" && hdnPaintProcessId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPaintProcessDetail(hdnPaintProcessId.val());
           var paintProcessChasisSerialProducts = [];
           GetPaintProcessChasisSerialProductList(paintProcessChasisSerialProducts);
           $("#ddlCompanyBranch").attr('disabled', true);
       }, 1000);



        if (hdnAccessMode.val() == "4") {
           // $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', false);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $(".editshow").hide();
            if ($(".editshow").hide()) {
                $('#lblWorkOrder').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
            }
        }
        else {
           // $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
            $(".editshow").show();
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
        $(".editshow").show();
        var productSerialProducts = [];
        GetProductSerialProductList(productSerialProducts);
    }

    var paintProcessProducts = [];
    GetPaintProcessProductList(paintProcessProducts);

    $("#txtRemarks1").attr('readOnly', true);
    $("#txtRemarks2").attr('readOnly', true);

    $(".editshow").attr('disabled', true);
    
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
function GetPaintProcessChasisSerialProductList(paintProcessChasisSerialProducts) {
    var hdnPaintProcessId = $("#hdnPaintProcessId");
    var requestData = { paintProcessChasisSerialProducts: paintProcessChasisSerialProducts, paintProcessId: hdnPaintProcessId.val() };
    $.ajax({
        url: "../PaintProcess/GetPaintProcessProductSerialProductList",
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
        if (hdnChked == 1)
        {
            chkPrintChasis.attr("checked",true)
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
            $("#ddlCompanyBranch,#ddlSearchCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}

function GetPaintProcessProductList(paintProcessProducts) {
    var hdnPaintProcessId = $("#hdnPaintProcessId");
    var requestData = { paintProcessProducts: paintProcessProducts, paintProcessId: hdnPaintProcessId.val()};
    $.ajax({
        url: "../PaintProcess/GetPaintProcessProductList",
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
    var hdnPaintProcessDetailId = $("#hdnPaintProcessDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");
    var hdnWorkOrderQuantity = $("#txtWOQTY");
    var hdntotalRecivedFabQuantity = $("#hdnTotalRecivedFabQuantity");
    var hdnPendingQuantity = $("#hdnPendingQuantity");
    var txtPendingQuantity = $("#txtPendingQuantity");
    var ddlAdjustment = $("#ddlAdjustment");
    var hdnRepQTY = $("#hdnRepQTY");
    var newProduct = $("#hdnNewProduct");
    if (action == 1) {
        newProduct.val("NEW")
    }
    else {
        newProduct.val("0")
    }


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
        ShowModel("Alert", "Please enter recived Quantity")
        txtQuantity.focus();
        return false;
    }

    if (ddlAdjustment.val() == "" || ddlAdjustment.val() == "0") {
        ShowModel("Alert", "Please select Adjustment Product.")
        ddlAdjustment.focus();
        return false;
    }

    if ($("#txtWOQTY").val() == "" || parseFloat($("#txtWOQTY").val()) == 0) {
        ShowModel("Alert", "Please Enter Work order Quantity.")
        $("#txtWOQTY").val().focus();
        return false;
    }
    if (txtPendingQuantity.val() != "0" && hdnPendingQuantity.val() != "0") {
        if (parseFloat(txtPendingQuantity.val()) < parseFloat(txtQuantity.val())) {
            ShowModel("Alert", "Recived Quantity cannot be greater than Pending Quantity")
            txtQuantity.focus();
            return false;
        }
    }
    if (parseFloat(txtQuantity.val()) > parseFloat($("#txtAdjQTY").val())) {
        ShowModel("Alert", "Recived Quantity cannot be greater than Adjustment Quantity")
        txtQuantity.focus();
        return false;
    }
    if (parseFloat(txtQuantity.val()) > parseFloat($("#txtWOQTY").val())) {
        ShowModel("Alert", "Recived Quantity cannot be greater than Work Quantity")
        txtQuantity.focus();
        return false;
    }

    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    var paintProcessProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var paintProcessDetailId = $row.find("#hdnPaintProcessDetailId").val();
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
        var adjProductId = $row.find("#hdnAdjProductId").val();
        var repQTY = $row.find("#hdnRepQTY").val();
        var hdnNewProduct = $row.find("#hdnNewProduct").val();

        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }
                if (ddlAdjustment.val() == productId) {
                    var paintProcessProduct = {
                        PaintProcessDetailId: paintProcessDetailId,
                        SequenceNo: sequenceNo,
                        ProductId: productId,
                        ProductName: productName,
                        ProductCode: productCode,
                        ProductShortDesc: productShortDesc,
                        UOMName: uomName,
                        Quantity: quantity,
                        PendingQuantity: pendingQuantity - (txtQuantity.val()),
                        TotalPaintQuantity: totalRecivedFabQuantity,
                        RecivedQuantity: quantity,
                        WorkOrderQuantity: workOrderQuantity - (txtQuantity.val()),
                        AdjProductId: adjProductId,
                        RepQTY: repQTY,
                        NewProduct: hdnNewProduct,

                    };
                    paintProcessProductList.push(paintProcessProduct);
                }
                else {
                    var paintProcessProduct = {
                        PaintProcessDetailId: paintProcessDetailId,
                        SequenceNo: sequenceNo,
                        ProductId: productId,
                        ProductName: productName,
                        ProductCode: productCode,
                        ProductShortDesc: productShortDesc,
                        UOMName: uomName,
                        Quantity: quantity,
                        PendingQuantity: pendingQuantity,
                        TotalPaintQuantity: totalRecivedFabQuantity,
                        RecivedQuantity: quantity,
                        WorkOrderQuantity: workOrderQuantity,
                        AdjProductId: adjProductId,
                        RepQTY: repQTY,
                        NewProduct: hdnNewProduct,

                    };
                    paintProcessProductList.push(paintProcessProduct);
                }


                productEntrySequence = parseInt(productEntrySequence) + 1;

            }
            else if (hdnPaintProcessDetailId.val() == paintProcessDetailId && hdnSequenceNo.val() == sequenceNo) {
                var paintProcessProduct = {
                    PaintProcessDetailId: hdnPaintProcessDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    TotalPaintQuantity: hdntotalRecivedFabQuantity.val(),
                    RecivedQuantity: txtQuantity.val().trim(),
                    PendingQuantity: hdnPendingQuantity.val(),
                    WorkOrderQuantity: hdnWorkOrderQuantity.val(),
                    AdjProductId: ddlAdjustment.val(),
                    RepQTY: hdnRepQTY.val(),
                    NewProduct: newProduct.val(),
                };
                paintProcessProductList.push(paintProcessProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var paintProcessProductAddEdit = {
            PaintProcessDetailId: hdnPaintProcessDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            TotalPaintQuantity: hdntotalRecivedFabQuantity.val(),
            RecivedQuantity: txtQuantity.val().trim(),
            PendingQuantity: hdnPendingQuantity.val(),
            WorkOrderQuantity: hdnWorkOrderQuantity.val(),
            AdjProductId: ddlAdjustment.val(),
            RepQTY: hdnRepQTY.val(),
            NewProduct: newProduct.val(),
        };
        paintProcessProductList.push(paintProcessProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetPaintProcessProductList(paintProcessProductList);
    }

}
function EditProductRow(obj) {
   // $("#txtWOQTY").attr('readOnly', true);
    var row = $(obj).closest("tr");
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var paintProcessDetailId = $(row).find("#hdnPaintProcessDetailId").val();
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
    var repQTY = $(row).find("#hdnRepQTY").val();
    var hdnNewProduct = $(row).find("#hdnNewProduct").val();
    var hdnAdjProductId = $(row).find("#hdnAdjProductId").val();

    $("#txtProductName").val(productName);
    $("#hdnPaintProcessDetailId").val(paintProcessDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    
    if (productId!=hdnAdjProductId && hdnNewProduct == "NEW" && hdnAdjProductId!=0) {
        $('#tblProductList tr').each(function (i, row) {
            var $row = $(row);
            var hdnProductId = $row.find("#hdnProductId").val();
            var workOrderQuantity = $row.find("#hdnWorkOrderQuantity").val();
            var hdnPendingQunatity = $row.find("#hdnPendingQunatity").val();
            if (hdnProductId != undefined) {
                if (hdnProductId == hdnAdjProductId) {
                    $(row).find("#hdnWorkOrderQuantity").val(parseFloat(workOrderQuantity) + parseFloat(recivedQuantity));
                    $(row).find("#hdnPendingQunatity").val(parseFloat(hdnPendingQunatity) + parseFloat(recivedQuantity));
                }
            }
        });
    }
    if (hdnNewProduct == "NEW") {
        $("#ddlAdjustment").val(hdnAdjProductId);
         GetProdutwiseQty();
    }
    else {
        $("#ddlAdjustment").val(productId);
        GetProdutwiseQty();
    }
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);

    $("#txtQuantity").val(recivedQuantity);
    $("#txtPendingQuantity").val(pendingQunatity);
    $("#hdnPendingQuantity").val(pendingQunatity);
    $("#hdnQuantity").val(Quantity);
    $("#hdnRepQTY").val(repQTY);


    $("#hdnNewProduct").val(hdnNewProduct);

    $("#txtWOQTY").val(workOrderquantity);
    $("#hdnWorkOrderQuantity").val(workOrderquantity);
    $("#hdnTotalRecivedFabQuantity").val(totalRecivedFabQuantity);


    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();


    ShowHideProductPanel(1);
}

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var paintProcessDetailId = $(row).find("#hdnPaintProcessDetailId").val();
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
function GetPaintProcessDetail(paintProcessId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PaintProcess/GetPaintProcessDetail",
        data: { paintProcessId: paintProcessId },
        dataType: "json",
        success: function (data) {
            chekedstatus = "";
            $("#txtPaintProcessNo").val(data.PaintProcessNo);
            $("#hdnPaintProcessId").val(data.PaintProcessId);
            $("#txtPaintProcessDate").val(data.PaintProcessDate);
            $("#txtWorkOrderNo").val(data.WorkOrderNo);
            $("#txtWorkOrderQuantity").val(data.TotalQuantity);

            $("#txtWorkOrderDate").val(data.WorkOrderDate);

            $("#txtPaintProcessDate").datepicker("option", "minDate", data.WorkOrderDate);

            $("#hdnWorkOrderId").val(data.WorkOrderId);          
            $("#ddlCompanyBranch").val(data.CompanyBranchId);            
            $("#ddlPaintProcessStatus").val(data.PaintProcessStatus);
           // $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
               // $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }

            if (data.PaintProcessStatus == "Final")
            {

                chekedstatus = data.PaintProcessStatus;
                $(".editshow").hide();
                $("#btnReset").hide();
                $("#btnUpdate").hide();
                $("input").attr('readOnly', true);
               
                $("select").attr('disabled', true);              
                if ($(".editshow").hide()) {
                    $('#lblWorkOrder').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
                }
               
            }

            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);
            $("#btnAddNew").hide();
            $("#btnPrint").hide();
            GetProducedQuantity();
            BindWorkOrderProductList(data.WorkOrderId);
            

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}

function SaveData() {
    if ($("#txtCancelReason").val() == " ") {
        ShowModel("Alert", "Please Enter Cancel Reason.")
        txtCancelReason.focus();
        return false;
    }
    if (confirm("Are You Sure to Cancel this Assembling Process ?")) {
        var hdnPaintProcessId = $("#hdnPaintProcessId");
        var txtCancelReason = $("#txtCancelReason");


        var accessMode = 1;//Add Mode
        if (hdnPaintProcessId.val() != null && hdnPaintProcessId.val() != 0) {
            accessMode = 2;//Edit Mode
        }

        var requestData = { paintProcessId: hdnPaintProcessId.val(), cancelReason: txtCancelReason.val() };
        $.ajax({
            url: "../PaintProcess/CancelPaintProcess",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
                    ShowModel("Alert", data.message);
                  
                    setTimeout(
                       function () {
                           window.location.href = "../PaintProcess/ListPaintProcess";
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
}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

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
        $("#hdnWorkOrderQuantity").val("0");
        $("#hdnTotalRecivedFabQuantity").val("0");
        $("#hdnPendingQuantity").val("0");
        $("#txtPendingQuantity").val(""); 
        $("#hdnQuantity").val("0");
        $("#txtWOQTY").val("");
        $("#txtAdjQTY").val("");
        $("#ddlAdjustment").val("0");
        $("#txtWOQTY").attr('readOnly', false);


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

function SearchWorkOrder() {
    var txtWorkOrderNo = $("#txtSearchWorkOrderNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch"); //$("#ddlSearchCompanyBranch");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var requestData = { workOrderNo: txtWorkOrderNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../PaintProcess/GetPaintProcessWorkOrderList",
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
    $("#ddlCompanyBranch").attr('disabled', true);
    var paintProcessProducts = [];
    GetWorkOrderProductList(paintProcessProducts);

    $("#txtPaintProcessDate").datepicker("option", "minDate", workOrderDate);

    GetProducedQuantity();
    BindWorkOrderProductList(workOrderId);
    $("#SearchWordOrderModel").modal('hide');
}
function GetWorkOrderProductList(paintProcessProducts, workOrderId) {
    var hdnWorkOrderId = $("#hdnWorkOrderId");
    var requestData = { paintProcessProducts: paintProcessProducts, workOrderId: hdnWorkOrderId.val() };
    $.ajax({
        url: "../PaintProcess/GetWorkOrderProductList",
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

function GetProducedQuantity() {
    var workOrderId = $("#hdnWorkOrderId").val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PaintProcess/GetPaintProcessProducedQuantityAgainstWorkOrder",
        data: { workOrderId: workOrderId },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "GET",
        error: function (err) {
        },
        success: function (data) {
            $("#txtProducedQuantity").val(data);

        }
    });
}

function OpenPrintPopup(productId,productName,quantity) {
    if (quantity == 0) {
        ShowModel("Alert", "Please Enter Quantity first")
        return false;

    }

    else {
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
                    // $('.chkChasis:checked').attr('disabled', false);
                }
                else if (hdnProductID == productId) {
                    //$('.chkChasis:checked').attr('disabled', false);
                    chkPrintChasis.attr('disabled', false);
                }
                else if (hdnProductID != productId && hdnProductID != 0) {
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
        url: "../PaintProcess/GetProductSerialProductList",
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
function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../PaintProcess/AddEditPaintProcess";
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



function BindWorkOrderProductList(workOrderId) {
    //var workOrderId = $("$hdnWorkOrderId").val();
    $("#ddlAdjustment").val(0);
    $("#ddlAdjustment").html("");
    $.ajax({
        type: "GET",
        url: "../PaintProcess/GetWOProductList",
        data: { workOrderId: workOrderId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlAdjustment").append($("<option></option>").val(0).html("Select Adjustment Product"));
            $.each(data, function (i, item) {
                $("#ddlAdjustment").append($("<option></option>").val(item.ProductId).html(item.ProductName));
            });
        },
        error: function (Result) {
            $("#ddlAdjustment").append($("<option></option>").val(0).html("Select Adjustment Product"));
        }
    });
}
function GetProdutwiseQty() {
    var ddlAdjustment = $("#ddlAdjustment").val();
    if (ddlAdjustment != "" && ddlAdjustment!= "0")
    {
        $('#tblProductList tr').each(function (i, row) {
            var $row = $(row);
            var productId = $row.find("#hdnProductId").val();
            var workOrderQuantity = $row.find("#hdnWorkOrderQuantity").val();          
            if (productId != undefined) {                
                if (ddlAdjustment == productId) {
                    $("#txtAdjQTY").val(workOrderQuantity);
                }
            }
        });
    }
    else {
        $("#txtAdjQTY").val("");
    }
  
}