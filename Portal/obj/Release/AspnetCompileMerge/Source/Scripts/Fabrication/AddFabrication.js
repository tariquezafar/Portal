$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtWorkOrderDate").attr('disabled', true);
    $("#txtProductName").attr('readOnly', true);
    $("#txtPendingQuantity").attr('readOnly', true);
    $("#txtFabricationNo").attr('readOnly', true);
    $("#txtFabricationDate").attr('readOnly', true);
    $("#txtWorkOrderNo").attr('readOnly', true);
    $("#txtWorkOrderDate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtProducedQuantity").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtProductShortDesc").attr('readOnly', true);
    $("#txtworkOrderQuantity").attr('readOnly', true);
    
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

    //$("#txtFabricationDate").datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat: 'dd-M-yy',
    //    yearRange: '-10:+100',
    //    minDate: '0',
    //    onSelect: function (selected) {

    //    }
    //});

    $("#txtFabricationDate").datepicker({
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
            $("#txtFabricationDate").datepicker("option", "minDate", selected);
        }
    });


    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnfabricationId = $("#hdnfabricationId");
    if (hdnfabricationId.val() != "" && hdnfabricationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetFabricationDetail(hdnfabricationId.val());
           var fabricationChasisSerialProducts = [];
           GetFabricationChasisSerialProductList(fabricationChasisSerialProducts);
           $("#ddlCompanyBranch").attr('disabled', true);
       }, 1000);
        /*Start fabrication Chasis serails*/
        var fabricationChasisSerials = [];
        GetFabricationChasisSerials(fabricationChasisSerials);
        /*End fabrication Chasis serails*/


        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
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
        /*Start Chasis Products serails*/

        var productSerialProducts = [];
        GetProductSerialProductList(productSerialProducts);

        //var printChasisDetailProducts = [];
        //GetPrintChasisProducts(printChasisDetailProducts);
        /*End Chasis Products serails*/

    }

    var fabricationProducts = [];
    GetFabricationProductList(fabricationProducts);

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

function GetFabricationChasisSerialProductList(fabricationChasisSerialProducts) {
    var hdnfabricationId = $("#hdnfabricationId");
    var requestData = { fabricationChasisSerialProducts: fabricationChasisSerialProducts, fabricationId: hdnfabricationId.val() };
    $.ajax({
        url: "../Fabrication/GetFabricationChasisSerialProductList",
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

function GetFabricationProductList(fabricationProducts) {
    var hdnfabricationId = $("#hdnfabricationId");
    var requestData = { fabricationProducts: fabricationProducts, fabricationId: hdnfabricationId.val() };
    $.ajax({
        url: "../Fabrication/GetFabricationProductList",
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
    var hdnFabricationDetailId = $("#hdnFabricationDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
  
    var hdnSequenceNo = $("#hdnSequenceNo");
    var hdnWorkOrderQuantity = $("#hdnWorkOrderQuantity");    
    var hdntotalRecivedFabQuantity = $("#hdnTotalRecivedFabQuantity");
    var hdnPendingQuantity = $("#hdnPendingQuantity");
    var hdnIsSerializedProduct = $("#hdnIsSerializedProduct");
  

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
        var isSerializedProduct = $row.find("#hdnIsSerializedProduct").val();
       

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
                    IsSerializedProduct: isSerializedProduct,
                    

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
                    IsSerializedProduct: hdnIsSerializedProduct.val(),
                  
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
            IsSerializedProduct: hdnIsSerializedProduct.val(),
                   
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
    var isSerializedProduct = $(row).find("#hdnIsSerializedProduct").val();
    

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
    $("#hdnIsSerializedProduct").val(isSerializedProduct);
    $("#hdnWorkOrderQuantity").val(workOrderquantity);
    $("#hdnTotalRecivedFabQuantity").val(totalRecivedFabQuantity);



    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();

    ShowHideProductPanel(1);
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
var chekedstatus = "";
function GetFabricationDetail(fabricationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Fabrication/GetFabricationDetail",
        data: { fabricationId: fabricationId },
        dataType: "json",
        success: function (data) {
            $("#txtFabricationNo").val(data.FabricationNo);
            $("#hdnfabricationId").val(data.FabricationId);
            $("#txtFabricationDate").val(data.FabricationDate);
            $("#txtWorkOrderDate").val(data.WorkOrderDate);

            $("#txtFabricationDate").datepicker("option", "minDate", data.WorkOrderDate);

            $("#txtWorkOrderNo").val(data.WorkOrderNo);
            $("#hdnWorkOrderId").val(data.WorkOrderId);
            $("#txtWorkOrderQuantity").val(data.WorkOrderQuantity);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#ddlFabricationStatus").val(data.FabricationStatus);
            if (data.FabricationStatus == "Final")
            {
                chekedstatus = data.FabricationStatus;
                //$(".chkChasis").attr('disabled', true);
                $("#btnUpdate").hide();
                $(".editonly").hide();
                $("#btnReset").hide();
                if ($(".editonly").hide()) {
                    $('#lblWorkOrder').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
                }
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);
            }
            //$("#divCreated").show();
            //$("#txtCreatedBy").val(data.CreatedByUserName);
            //$("#txtCreatedDate").val(data.CreatedDate);
            //if (data.ModifiedByUserName != "") {
            //    $("#divModified").show();
            //    $("#txtModifiedBy").val(data.ModifiedByUserName);
            //    $("#txtModifiedDate").val(data.ModifiedDate);
            //}

            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);
            $("#btnAddNew").show();
            $("#btnPrint").show();
            GetProducedQuantity();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}

function SaveData() {
    var txtFabricationNo = $("#txtFabricationNo");
    var hdnfabricationId = $("#hdnfabricationId");
    var txtFabricationDate = $("#txtFabricationDate");
    var txtWorkOrderNo = $("#txtWorkOrderNo");
    var hdnWorkOrderId = $("#hdnWorkOrderId");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlFabricationStatus = $("#ddlFabricationStatus");
    var txtRemarks1 = $("#txtRemarks1");
    var txtRemarks2 = $("#txtRemarks2");

    if (hdnWorkOrderId.val() == "" || hdnWorkOrderId.val() == 0) {
        ShowModel("Alert", "Please select Work Order No.")
        txtWorkOrderNo.focus();
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == 0) {
        ShowModel("Alert", "Please select  Location")
        ddlCompanyBranch.focus();
        return false;
    }

    var fabricationViewModel = {
        FabricationId: hdnfabricationId.val(),
        FabricationNo: txtFabricationNo.val().trim(),
        FabricationDate: txtFabricationDate.val().trim(),
        WorkOrderId: hdnWorkOrderId.val(),
        WorkOrderNo: txtWorkOrderNo.val().trim(),
        CompanyBranchId: ddlCompanyBranch.val().trim(),
        FabricationStatus: ddlFabricationStatus.val(),
        Remarks1: txtRemarks1.val(),
        Remarks2: txtRemarks2.val()
    };
    var totalQuantity = 0;
    var checkedFlag = false;
    var fabricationProductList = [];

    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var fabricationDetailId = $row.find("#hdnFabricationDetailId").val();
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
                var fabricationProduct = {
                    FabricationDetailId: fabricationDetailId,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity
                };
                fabricationProductList.push(fabricationProduct);
           
        }
    });

    if (checkedFlag == false) {
        ShowModel("Alert", "Please Select at least 1 Product as the Received Quantity")
        return false;
    }

    if (fabricationProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product.")
        return false;
    }




    /*Get Print Chasis Modal from Pop Up*/
    var printChasisList = [];
    $('#tblPrintChasis tr').each(function (i, row) {
        var $row = $(row);
        var productId = $row.find("#hdnProductID").val();
        var chasisSerialNo = $row.find("#hdnChasisSerialNo").val();
        var motorNo = $row.find("#hdnMotorNo").val();     
        var chkPrintChasis = $row.find("#chkPrintChasis").is(':checked') ? true : false;
        if (productId != undefined) {
            if (chkPrintChasis == true) {
                var printChasisProduct = {
                    ProductId: productId,
                    ChasisSerialNo: chasisSerialNo.trim(),
                    MotorNo: motorNo.trim()
                };
                printChasisList.push(printChasisProduct);
            }
        }
    });

   
        var flag = true;  
        $('#tblProductList tr:not(:has(th))').each(function (i, row) {
            var count = 0;
            var $row = $(row);
            var productId = $row.find("#hdnProductId").val();
            var quantity = $row.find("#hdnRecivedQuantity").val();
            var isSerializedProduct = $row.find("#hdnIsSerializedProduct").val();

            if (productId != undefined) {
                if (isSerializedProduct == "True")
                {
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
    /*Get Print Chasis Modal from Pop Up*/
    var txtTotalQuantity = $("#txtWorkOrderQuantity");
    var txtProducedQuantity = $("#txtProducedQuantity");

    if (parseFloat(txtProducedQuantity.val()) > 0) {
        // var remainigQuantity = parseFloat(txtTotalQuantity.val()) - parseFloat(txtProducedQuantity.val());
        if (parseFloat(totalQuantity) + parseFloat(txtProducedQuantity.val()) > parseFloat(txtTotalQuantity.val())) {
            ShowModel("Alert", "Produced Quantity should not be grater than work order Quantity.")
            return false;
        }

    }
    if (parseFloat(txtTotalQuantity.val()) > 0) {

        if (parseFloat(totalQuantity) > parseFloat(txtTotalQuantity.val())) {
            ShowModel("Alert", "Product Quantity not be grater than work order Quantity.")
            return false;
        }
    }
    else {

        ShowModel("Alert", "Please Select Work Order Quantity.")
        return false;
    }
    //if (fabricationProductList.length == 0) {
    //    ShowModel("Alert", "Please Select at least 1 Product.")
    //    return false;
    //}
    var accessMode = 1;//Add Mode
    if (hdnfabricationId.val() != null && hdnfabricationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { fabricationViewModel: fabricationViewModel, fabricationProducts: fabricationProductList, fabricationChasisSerialProducts: printChasisList };
    $.ajax({
        url: "../Fabrication/AddEditFabrication?accessMode=" + accessMode + "",
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
                       window.location.href = "../Fabrication/AddEditFabrication?fabricationId=" + data.trnId + "&AccessMode=3";
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

    $("#txtFabricationNo").val("");
    $("#hdnfabricationId").val("0");
    $("#txtWorkOrderNo").val("");
    $("#hdnWorkOrderId").val("0");
    $("#txtFabricationDate").val($("#hdnCurrentDate").val());
    $("#ddlFabricationStatus").val("Final");
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
    if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
        ShowModel("Alert", "Please Select Company Branch");
        return false;
    }
    $("#divList").html("");
    $("#SearchWordOrderModel").modal();

}

function SearchWorkOrder() {
    var txtWorkOrderNo = $("#txtSearchWorkOrderNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var requestData = { workOrderNo: txtWorkOrderNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../Fabrication/GetFabricationWorkOrderList",
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
    var fabricationProducts = [];
    GetWorkOrderProductList(fabricationProducts);

    $("#txtFabricationDate").datepicker("option", "minDate", workOrderDate);
    GetProducedQuantity();
    //var printChasisDetailProducts = [];
    //GetPrintChasisProducts(printChasisDetailProducts);
    


    
    $("#SearchWordOrderModel").modal('hide');
}
function GetWorkOrderProductList(fabricationProducts, workOrderId) {
    var hdnWorkOrderId = $("#hdnWorkOrderId");
    var requestData = { fabricationProducts: fabricationProducts, workOrderId: hdnWorkOrderId.val() };
    $.ajax({
        url: "../Fabrication/GetWorkOrderProductList",
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
        url: "../Fabrication/GetFabricationProducedQuantityAgainstWorkOrder",
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

function GetPrintChasisProducts(printChasisDetailProducts) {
    var requestData = { ChasisSerialPlanDetails: printChasisDetailProducts };
    $.ajax({
        url: "../Fabrication/GetPrintChasisSerials",
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

function GetFabricationChasisSerials(fabricationChasisSerials) {
    var requestData = { fabricationChasisSerialsDetails: fabricationChasisSerials };
    $.ajax({
        url: "../Fabrication/GetFabricationChasisSerials",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divFabricationChasisSerial").html("");
            $("#divFabricationChasisSerial").html(err);
        },
        success: function (data) {
            $("#divFabricationChasisSerial").html("");
            $("#divFabricationChasisSerial").html(data);
        }
    });
}

function GetProductSerialProductList(productSerialProducts) {
    var requestData = {};
    $.ajax({
        url: "../Fabrication/GetProductSerialProductList",
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
function OpenPrintPopup(productId, productName, quantity, IsSerializedProduct) {

    if (IsSerializedProduct =="False")
    {
        ShowModel("Alert", "This Product is not Serialized")
        return false;
    }
    else {

    
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
                }
                else if (hdnProductID == productId) {

                    chkPrintChasis.attr('disabled', false);
                }
                else if (hdnProductID != productId && hdnProductID != 0) {
                    chkPrintChasis.attr('disabled', true);
                }
            }
        });


        if (chekedstatus == "Final" && $("#hdnfabricationId").val() != "0") {
            $(".chkChasis").attr('disabled', true);
        }
    }
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

function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../Fabrication/AddEditFabrication";
    }
}

//////*************************End Code****************************/////////////////////////