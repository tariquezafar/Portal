$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    }); 
    $("#txtWorkOrderNo").attr('readOnly', true);
    $("#txtWorkOrderDate").attr('readOnly', true);
    $("#txtTargetFromDate").attr('readOnly', true);
    $("#txtTargetToDate").attr('readOnly', true);
    $("#txtAssemblyType").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtProductShortDesc").attr('readOnly', true);
    $("#txtSODate").attr('readOnly', true);
    
    
    $("#txtProductName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../WorkOrder/GetProductAutoCompleteBOMList",
                //url: "../Product/GetProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode, AssemblyType :item.AssemblyType};
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
            $("#txtAssemblyType").val(ui.item.AssemblyType);
            GetProductDetail(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductName").val("");
                $("#hdnProductId").val("0");
                $("#txtProductShortDesc").val("");
                $("#txtProductCode").val("");
                $("#txtAssemblyType").val("");
                $("#txtQuantity").val("");
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





    $("#txtWorkOrderDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {

        }
    });
    $("#txtTargetFromDate,#txtTargetToDate,#txtSearchFromDate,#txtSearchToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',      
        onSelect: function (selected) {

        }
    });

    BindCompanyBranchList();
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnWorkOrderId = $("#hdnWorkOrderId");
    if (hdnWorkOrderId.val() != "" && hdnWorkOrderId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetWorkOrderDetail(hdnWorkOrderId.val());
       }, 2000);



        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
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
    $("#txtCancelReason").attr('readOnly', false);
    var workOrderProducts = [];
    GetWorkOrderProductList(workOrderProducts);
    
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
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}

function GetWorkOrderProductList(workOrderProducts) {
    var hdnWorkOrderId = $("#hdnWorkOrderId");
    var requestData = { workOrderProducts: workOrderProducts, workOrderId: hdnWorkOrderId.val() };
    $.ajax({
        url: "../WorkOrder/GetWorkOrderProductList",
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
    var hdnWorkOrderProductDetailId = $("#hdnWorkOrderProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtAssemblyType = $("#txtAssemblyType");

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

    var workOrderProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var workOrderProductDetailId = $row.find("#hdnWorkOrderProductDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnQuantity").val();
        var assemblyType = $row.find("#hdnAssemblyType").val();
        
        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }
                if (assemblyType != txtAssemblyType.val()) {
                    ShowModel("Alert", " Work Order With Different Assembly Types products cannot be created.!!!")
                    txtAssemblyType.focus();
                    flag = false;
                    return false;
                }

                var workOrderProduct = {
                    WorkOrderProductDetailId: workOrderProductDetailId,
                    SequenceNo:sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity,
                    AssemblyType: assemblyType,
                  
                };
                workOrderProductList.push(workOrderProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
                
            }
            else if (hdnWorkOrderProductDetailId.val() == workOrderProductDetailId && hdnSequenceNo.val()==sequenceNo)
            {
                var workOrderProduct = {
                    WorkOrderProductDetailId: hdnWorkOrderProductDetailId.val(),
                    SequenceNo:hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    AssemblyType: txtAssemblyType.val().trim(),
                };
                workOrderProductList.push(workOrderProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var workOrderProductAddEdit = {
            WorkOrderProductDetailId: hdnWorkOrderProductDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            AssemblyType: txtAssemblyType.val().trim(),
          };
        workOrderProductList.push(workOrderProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetWorkOrderProductList(workOrderProductList);
    }

}
function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var workOrderProductDetailId = $(row).find("#hdnWorkOrderProductDetailId").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var quantity = $(row).find("#hdnQuantity").val();
    var assemblyType = $(row).find("#hdnAssemblyType").val();
    
    $("#txtProductName").val(productName);
    $("#hdnWorkOrderProductDetailId").val(workOrderProductDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);
    $("#txtQuantity").val(quantity);
    $("#txtAssemblyType").val(assemblyType);
    
    
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

function GetWorkOrderDetail(workOrderId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../WorkOrder/GetWorkOrderDetail",
        data: { workOrderId: workOrderId },
        dataType: "json",
        success: function (data) {
            $("#txtWorkOrderNo").val(data.WorkOrderNo);
            $("#txtWorkOrderDate").val(data.WorkOrderDate);
            $("#txtTargetFromDate").val(data.TargetFromDate);
            $("#txtTargetToDate").val(data.TargetToDate);
            
            $("#hdnSOId").val(data.SOId);
            $("#txtSONo").val(data.SONo);
            $("#txtSODate").val(data.SODate);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            
            $("#ddlApprovalStatus").val(data.WorkOrderStatus);
            if (data.WorkOrderStatus == "Final")
            {
                $(".editonly").hide();
                $("#btnReset").hide();
                $("#btnUpdate").hide();
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);
                
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
            $("#txtCancelReason").attr('readOnly', false);
            $("#btnAddNew").show();
            $("#btnPrint").show();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}

function CancelWO() {
    var hdnWorkOrderId = $("#hdnWorkOrderId");
    var txtWorkOrderNo = $("#txtWorkOrderNo");
    var txtCancelReason = $("#txtCancelReason");
    var requestData = {
        workOrderId: hdnWorkOrderId.val(),
        workOrderNo: txtWorkOrderNo.val().trim(),
        cancelReason: txtCancelReason.val().trim()
    };
    $.ajax({
        url: "../WorkOrder/CancelWO",
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
                       window.location.href = "../WorkOrder/ListWorkOrder";
                   }, 2000);

                $("#btnCancel").hide();
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
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields() {

    $("#txtWorkOrderNo").val("");
    $("#hdnWorkOrderId").val("0");
    $("#txtWorkOrderDate").val($("#hdnCurrentDate").val());
    $("#txtTargetFromDate").val($("#hdnCurrentDate").val());
    $("#txtTargetToDate").val($("#hdnCurrentDate").val());

    $("#ddlApprovalStatus").val("Final");
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
        $("#txtAssemblyType").val("");
        $("#txtProductShortDesc").val("");
        $("#txtUOMName").val("");
        $("#txtQuantity").val("");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
        


    }
}

function show() {
    let el = document.querySelector('input[id="txtQuantity"]');
    el.addEventListener('keypress', (event) => {
        let k = event.key,
            t = isNaN(k),
            sc = ['Backspace'].indexOf(k) === -1,
            d = k === '.', dV = el.value.indexOf('.') > -1,
            m = k === '-', mV = el.value.length > 0;

        if ((t && sc) && ((d && dV) || (m && dV) || (m && mV) || ((t && !d) && (t && !m)))) { event.preventDefault(); }
    }, false);
    el.addEventListener('paste', (event) => {
        if (event.clipboardData.types.indexOf('text/html') > -1) {
            if (isNaN(event.clipboardData.getData('text'))) { event.preventDefault(); }
        }
    }, false);
}
function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../WorkOrder/AddEditWorkOrder";
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
function OpenSOSearchPopup() {
    $("#SearchQuotationModel").modal();

}
function SearchSaleOrder() {
    var txtSearchSONo = $("#txtSearchSONo");
    var txtCustomerName = $("#txtSearchCustomerName");

    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { soNo: txtSearchSONo.val().trim(), customerName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: "Final", displayType: "Popup" };
    $.ajax({
        url: "../WorkOrder/GetSaleOrderList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divSOList").html("");
            $("#divSOList").html(err);
        },
        success: function (data) {
            $("#divSOList").html("");
            $("#divSOList").html(data);
        }
    });
}
function SelectSO(soId, soNo, soDate, companyBranchId) {
    $("#txtSONo").val(soNo);
    $("#hdnSOId").val(soId);
    $("#txtSODate").val(soDate);
    $("#ddlCompanyBranch").val(companyBranchId);
    GetSODetail(soId);
    var soProducts = [];
    GetSOProductList(soProducts);

    $("#SearchQuotationModel").modal('hide');

}
function GetSODetail(soId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SO/GetSODetail",
        data: { soId: soId },
        dataType: "json",
        success: function (data) {
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

function GetSOProductList(soProducts) {
    var hdnSOId = $("#hdnSOId");
    var requestData = { soProducts: soProducts, soId: hdnSOId.val() };
    $.ajax({
        url: "../WorkOrder/GetWOSOProductList",
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
        }
    });
}

//////********END CODE********///////