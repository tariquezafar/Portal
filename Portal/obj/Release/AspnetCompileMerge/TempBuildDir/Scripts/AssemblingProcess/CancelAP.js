$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });

    $("#txtPaintProcessDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {
            
        }
    });
    $("#txtAssemblingProcessDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            $("#txtAssemblingProcessDate").datepicker("option", "minDate", selected);
        }
    });


    $("#txtPendingQuantity").attr('readOnly', true);
    $("#txtAssemblingProcessNo").attr('readOnly', true);
    $("#txtAssemblingProcessDate").attr('readOnly', true);
    $("#txtWorkOrderNo").attr('readOnly', true);
    $("#txtWorkOrderQuantity").attr('readOnly', true);
    $("#txtPaintProcessNo").attr('readOnly', true);
    $("#txtPaintProcessQuantity").attr('readOnly', true);
    $("#txtWorkOrderDate").attr('readOnly', true);  
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtProductShortDesc").attr('readOnly', true);
    $("#txtPaintProcessDate").attr('readOnly', true);
   
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

    $("#txtAssemblingProcessDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $("#txtWorkOrderDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {

        }
    });
    $("#txtSODate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            $("#txtWorkOrderDate").datepicker("option", "minDate", selected);
        }
    });

    BindCompanyBranchList();
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnAssemblingProcessId = $("#hdnAssemblingProcessId");
    if (hdnAssemblingProcessId.val() != "" && hdnAssemblingProcessId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetAssemblingProcessDetail(hdnAssemblingProcessId.val());
           var assemblingProcessChasisSerialProducts = [];
           GetAssemblingProcessChasisSerialProductList(assemblingProcessChasisSerialProducts);
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

    var assemblingProcessProducts = [];
    GetAssemblingProcessProductList(assemblingProcessProducts);
    
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

function GetAssemblingProcessChasisSerialProductList(assemblingProcessChasisSerialProducts) {
    var hdnAssemblingProcessId = $("#hdnAssemblingProcessId");
    var requestData = { assemblingProcessChasisSerialProducts: assemblingProcessChasisSerialProducts, assemblingProcessId: hdnAssemblingProcessId.val() };
    $.ajax({
        url: "../AssemblingProcess/GetAssemblingProcessProductSerialProductList",
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

function GetAssemblingProcessProductList(assemblingProcessProducts) {
    var hdnAssemblingProcessId = $("#hdnAssemblingProcessId");
    var requestData = { assemblingProcessProducts: assemblingProcessProducts, assemblingProcessId: hdnAssemblingProcessId.val() };
    $.ajax({
        url: "../AssemblingProcess/GetAssemblingProcessProductList",
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
    var hdnAssemblingProcessDetailId = $("#hdnAssemblingProcessDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");

    var hdnTotalPaintQuantity = $("#hdnTotalPaintQuantity");
    var hdnTotalRecivedAssembledQuantity = $("#hdnTotalRecivedAssembledQuantity");
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

    var assemblingProcessProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var assemblingProcessDetailId = $row.find("#hdnAssemblingProcessDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();      
        var quantity = $row.find("#hdnRecivedQuantity").val();
        var pendingQuantity = $row.find("#hdnPendingQunatity").val();
        var totalRecivedAssembledQuantity = $row.find("#hdnTotalRecivedAssembledQuantity").val();
        var totalPaintQuantity = $row.find("#hdnTotalPaintQuantity").val();
        
        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var assemblingProcessProduct = {
                    AssemblingProcessDetailId: assemblingProcessDetailId,
                    SequenceNo:sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity,
                    PendingQuantity: pendingQuantity,
                    RecivedQuantity: quantity,
                    TotalPaintQuantity: totalPaintQuantity,
                    TotalRecivedAssembledQuantity: totalRecivedAssembledQuantity,
                  
                };
                assemblingProcessProductList.push(assemblingProcessProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
                
            }
            else if (hdnAssemblingProcessDetailId.val() == assemblingProcessDetailId && hdnSequenceNo.val() == sequenceNo)
            {
                var assemblingProcessProduct = {
                    AssemblingProcessDetailId: hdnAssemblingProcessDetailId.val(),
                    SequenceNo:hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    RecivedQuantity: txtQuantity.val().trim(),
                    PendingQuantity: hdnPendingQuantity.val(),
                    TotalPaintQuantity: hdnTotalPaintQuantity.val(),
                    TotalRecivedAssembledQuantity: hdnTotalRecivedAssembledQuantity.val(),
                };
                assemblingProcessProductList.push(assemblingProcessProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var assemblingProcessProductAddEdit = {
            AssemblingProcessDetailId: hdnAssemblingProcessDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            TotalPaintQuantity: hdnTotalPaintQuantity.val(),
            RecivedQuantity: txtQuantity.val().trim(),
            PendingQuantity: hdnPendingQuantity.val(),
            TotalRecivedAssembledQuantity: hdnTotalRecivedAssembledQuantity.val(),
          };
        assemblingProcessProductList.push(assemblingProcessProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetAssemblingProcessProductList(assemblingProcessProductList);
    }

}
function EditProductRow(obj) {
    var row = $(obj).closest("tr");
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var assemblingProcessDetailId = $(row).find("#hdnAssemblingProcessDetailId").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var Quantity = $(row).find("#hdnQuantity").val();
    var recivedQuantity = $(row).find("#hdnRecivedQuantity").val();
    var paintQuantity = $(row).find("#hdnTotalPaintQuantity").val();
    var pendingQunatity = $(row).find("#hdnPendingQunatity").val();
    var totalRecivedAssembledQuantity = $(row).find("#hdnTotalRecivedAssembledQuantity").val();
    
    $("#txtProductName").val(productName);
    $("#hdnAssemblingProcessDetailId").val(assemblingProcessDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);
   
    $("#txtQuantity").val(recivedQuantity);
    $("#txtPendingQuantity").val(pendingQunatity);
    $("#hdnPendingQuantity").val(pendingQunatity);
    $("#hdnQuantity").val(Quantity);

    $("#hdnTotalPaintQuantity").val(paintQuantity);
    $("#hdnTotalRecivedAssembledQuantity").val(totalRecivedAssembledQuantity);
    
    
    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    
    ShowHideProductPanel(1);
}

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var assemblingProcessDetailId = $(row).find("#hdnAssemblingProcessDetailId").val();
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
function GetAssemblingProcessDetail(assemblingProcessId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../AssemblingProcess/GetAssemblingProcessDetail",
        data: { assemblingProcessId: assemblingProcessId },
        dataType: "json",
        success: function (data) {
            chekedstatus = "";
            $("#txtAssemblingProcessNo").val(data.AssemblingProcessNo);
            $("#hdnAssemblingProcessId").val(data.AssemblingProcessId);
            $("#txtAssemblingProcessDate").val(data.AssemblingProcessDate);
            $("#txtWorkOrderNo").val(data.WorkOrderNo);
            $("#hdnWorkOrderId").val(data.WorkOrderId);
            $("#txtWorkOrderQuantity").val(data.TotalQuantity);


            $("#txtPaintProcessNo").val(data.PaintProcessNo);
            $("#hdnPaintProcessId").val(data.PaintProcessId);
            $("#txtPaintProcessQuantity").val(data.PaintProcessQuantity);

            $("#ddlCompanyBranch").val(data.CompanyBranchId);            
            $("#ddlAssemblingProcessStatus").val(data.AssemblingProcessStatus);
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            if (data.AssemblingProcessStatus == "Final")
            {
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
                chekedstatus = data.AssemblingProcessStatus;
                $("#txtCancelReason").attr('readOnly', false);
            }
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

function CancelAP() {

    if ($("#txtCancelReason").val() == " ") {
        ShowModel("Alert", "Please Enter Cancel Reason.")
        txtCancelReason.focus();
        return false;
    }

    if (confirm("Are You Sure to Cancel this Assembling Process ?")) {

        var hdnAssemblingProcessId = $("#hdnAssemblingProcessId");
        var txtAssemblingProcessNo = $("#txtAssemblingProcessNo");
        var txtCancelReason = $("#txtCancelReason");

     

        var ddlCompanyBranch = $("#ddlCompanyBranch");
        var requestData = {
            assemblingProcessId: hdnAssemblingProcessId.val(),
            assemblingProcessNo: txtAssemblingProcessNo.val().trim(),
            companyBranchId: ddlCompanyBranch.val(),
            cancelReason: txtCancelReason.val().trim()
        };
        $.ajax({
            url: "../AssemblingProcess/CancelAP",
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
                           window.location.href = "../AssemblingProcess/ListAssemblingProcess";
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
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields() {

    $("#txtAssemblingProcessNo").val("");
    $("#hdnAssemblingProcessId").val("0");
    $("#txtWorkOrderNo").val("");
    $("#hdnWorkOrderId").val("0");
    $("#txtAssemblingProcessDate").val($("#hdnCurrentDate").val());
    $("#ddlAssemblingProcessStatus").val("Final");
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
    if ($("#ddlCompanyBranch").val() == "0" || $("#ddlCompanyBranch").val() == "") {
        ShowModel("Alert", "Please Select Company Branch");
        return false;
    }
    $("#divList").html("");
    $("#SearchWordOrderModel").modal();

}


function SearchWorkOrder() {
    var txtWorkOrderNo = $("#txtSearchWorkOrderNo");
    var ddlCompanyBranch = $("#ddlSearchCompanyBranch");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var requestData = { workOrderNo: txtWorkOrderNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../AssemblingProcess/GetAssemblingProcessWorkOrderList",
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
function SelectWorkOrder(workOrderId, workOrderNo, workOrderDate, quantity,companyBranchId) {
    $("#txtWorkOrderNo").val(workOrderNo);
    $("#hdnWorkOrderId").val(workOrderId);
    $("#txtWorkOrderDate").val(workOrderDate);
    $("#txtWorkOrderQuantity").val(quantity);
    $("#ddlCompanyBranch").val(companyBranchId);
    $("#ddlCompanyBranch").attr('disabled', true);
    var assemblingProcessProducts = [];
    GetWorkOrderProductList(assemblingProcessProducts);
    $("#SearchWordOrderModel").modal('hide');
}
function GetWorkOrderProductList(assemblingProcessProducts, workOrderId) {
    var hdnWorkOrderId = $("#hdnWorkOrderId");
    var requestData = { assemblingProcessProducts: assemblingProcessProducts, workOrderId: hdnWorkOrderId.val() };
    $.ajax({
        url: "../AssemblingProcess/GetWorkOrderProductList",
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
    $("#divListPaintProcess").html("");
    $("#SearchPaintProcessModel").modal();

}

function SearchPaintProcess() {
    var txtSearchPaintProcessNo = $("#txtSearchPaintProcessNo");
    var ddlCompanyBranch = $("#ddlCompanyBranch");//$("#ddlSearchCompanyBranch");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");
    var requestData = { paintProcessNo: txtSearchPaintProcessNo.val().trim(), companyBranchId: ddlCompanyBranch.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../AssemblingProcess/GetAssemblingProcessPaintProcessList",
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
function SelectPaintProcess(paintProcessId, paintProcessNo, workOrderId, workOrderNo, paintProcessquantity, workOrderquantity, companyBranchId, paintProcessDate) {

    $("#hdnPaintProcessId").val(paintProcessId);
    $("#txtPaintProcessNo").val(paintProcessNo); 
    $("#hdnWorkOrderId").val(workOrderId);
    $("#txtWorkOrderNo").val(workOrderNo);
    $("#txtPaintProcessQuantity").val(paintProcessquantity);
    $("#txtWorkOrderQuantity").val(workOrderquantity);
    $("#ddlCompanyBranch").val(companyBranchId);
    $("#ddlCompanyBranch").attr('disabled', true);
    $("#txtPaintProcessDate").val(paintProcessDate);
    var assemblingProcessProducts = [];
    GetPaintProcessProductList(assemblingProcessProducts);
    GetProducedQuantity();
    $("#SearchPaintProcessModel").modal('hide');

    $("#txtAssemblingProcessDate").datepicker("option", "minDate", requisitionDate);
}
function GetPaintProcessProductList(assemblingProcessProducts, workOrderId) {
    var hdnPaintProcessId = $("#hdnPaintProcessId");
    var requestData = { assemblingProcessProducts: assemblingProcessProducts, paintProcessId: hdnPaintProcessId.val() };
    $.ajax({
        url: "../AssemblingProcess/GetPaintProcessProductList",
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
        url: "../AssemblingProcess/GetAssemblingProducedQuantityAgainstWorkOrder",
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
        url: "../AssemblingProcess/GetProductSerialProductList",
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