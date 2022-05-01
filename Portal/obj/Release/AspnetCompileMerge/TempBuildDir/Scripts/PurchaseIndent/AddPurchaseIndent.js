$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    BindCompanyBranchList();
    $("#txtIndentNo").attr('readOnly', true);
    $("#txtIndentDate").attr('readOnly', true);
    $("#txtCustomerCode").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);

    $("#txtRejectedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtAvailableStock").attr('readOnly', true);

    var ddlCompanyBranch = $("#ddlCompanyBranch");
    
    $("#txtIndentByUser").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Lead/GetUserAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, companyBranchID: ddlCompanyBranch.val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FullName, value: item.UserName, UserId: item.UserId, MobileNo: item.MobileNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtIndentByUser").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtIndentByUser").val(ui.item.label);
            $("#hdnUserId").val(ui.item.UserId);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtIndentByUser").val("");
                $("#hdnUserId").val("");
                ShowModel("Alert", "Please select User from List")
            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.value + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};




    $("#txtCustomerName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Quotation/GetCustomerAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.CustomerName, value: item.CustomerId, primaryAddress: item.PrimaryAddress, code: item.CustomerCode, gSTNo: item.GSTNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtCustomerName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtCustomerName").val(ui.item.label);
            $("#hdnCustomerId").val(ui.item.value);
            $("#txtCustomerCode").val(ui.item.code);

            BindCustomerBranchList(ui.item.value);

            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtCustomerName").val("");
                $("#hdnCustomerId").val("0");
                $("#txtCustomerCode").val("");
                ShowModel("Alert", "Please select Customer from List")

            }
            return false;
        }

    })
  .autocomplete("instance")._renderItem = function (ul, item) {
      return $("<li>")
        .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.primaryAddress + "</div>")
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
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode, CGST_Perc: item.CGST_Perc, SGST_Perc: item.SGST_Perc, IGST_Perc: item.IGST_Perc, HSN_Code: item.HSN_Code };
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

    $("#txtIndentDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {

        }
    });

    

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPurchaseIndentId = $("#hdnPurchaseIndentId");
    if (hdnPurchaseIndentId.val() != "" && hdnPurchaseIndentId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPurchaseIndentDetail(hdnPurchaseIndentId.val());
       }, 2000);

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $("#divAddProduct").hide();
            $("#btnAddNew").hide();
            $(".editonly").hide();
           
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

    var indentProducts = [];
    GetIndentProductList(indentProducts);

});

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

function GetProductDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productid: productId },
        dataType: "json",
        success: function (data) {
            //$("#txtPrice").val(data.SalePrice);
            $("#txtUOMName").val(data.PurchaseUOMName);
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
    var hdnIndentProductDetailId = $("#hdnIndentProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");

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
        if (parseFloat(txtQuantity.val().trim()) <= 0) {
            ShowModel("Alert", "Quantity should be greater than 0")
            txtQuantity.focus();
            return false;
        }
        else {
            ShowModel("Alert", "Please enter Quantity")
            txtQuantity.focus();
            return false;
        }
    }


    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    var indentProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var indentProductDetailId = $row.find("#hdnIndentProductDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnQuantity").val();

        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var indentProduct = {
                    IndentProductDetailId: indentProductDetailId,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity,

                };
                indentProductList.push(indentProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;

            }
            else if (hdnIndentProductDetailId.val() == indentProductDetailId && hdnSequenceNo.val() == sequenceNo) {
                var indentProduct = {
                    IndentProductDetailId: hdnIndentProductDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim()

                };
                indentProductList.push(indentProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var indentProductAddEdit = {
            IndentProductDetailId: hdnIndentProductDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),

        };
        indentProductList.push(indentProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetIndentProductList(indentProductList);
    }

}
function EditProductRow(obj) {

    var hdnAccessMode = $("#hdnAccessMode");
    if (hdnAccessMode.val() == "3") {
        ShowModel("Alert", "You can't modify in view mode.")
        return false;
    }
    else {
        var row = $(obj).closest("tr");
        var indentProductDetailId = $(row).find("#hdnIndentProductDetailId").val();
        var sequenceNo = $(row).find("#hdnSequenceNo").val();
        var productId = $(row).find("#hdnProductId").val();
        var productName = $(row).find("#hdnProductName").val();
        var productCode = $(row).find("#hdnProductCode").val();
        var productShortDesc = $(row).find("#hdnProductShortDesc").val();
        var uomName = $(row).find("#hdnUOMName").val();
        var quantity = $(row).find("#hdnQuantity").val();

        $("#txtProductName").val(productName);
        $("#hdnIndentProductDetailId").val(indentProductDetailId);
        $("#hdnSequenceNo").val(sequenceNo);
        $("#hdnProductId").val(productId);
        $("#txtProductCode").val(productCode);
        $("#txtProductShortDesc").val(productShortDesc);
        $("#txtUOMName").val(uomName);
        $("#txtQuantity").val(quantity);
        $("#btnAddProduct").hide();
        $("#btnUpdateProduct").show();
        ShowHideProductPanel(1);
    }
}
function RemoveProductRow(obj) {
    var hdnAccessMode = $("#hdnAccessMode");
    if (hdnAccessMode.val() == "3") {
        ShowModel("Alert", "You can't modify in view mode.")
        return false;
    }
    else {
        if (confirm("Do you want to remove selected Product?")) {
            var row = $(obj).closest("tr");
            ShowModel("Alert", "Product Removed from List.");
            row.remove();


        }
    }
}

function GetIndentProductList(indentProducts) {
    var hdnPurchaseIndentId = $("#hdnPurchaseIndentId");
    var requestData = { purchaseIndentProducts: indentProducts, indentId: hdnPurchaseIndentId.val() };
    $.ajax({
        url: "../PurchaseIndent/GetPurchaseIndentProductList",
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

function ShowHideProductPanel(action) {
    if (action == 1) {
        $(".productsection").show();
    }
    else {
        $(".productsection").hide();
        $("#txtProductName").val("");
        $("#hdnProductId").val("0");
        $("#hdnIndentProductDetailId").val("0");
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtUOMName").val("");
        $("#txtQuantity").val("");
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
    }
}

function BindCustomerBranchList(customerBranchId) {
    var customerId = $("#hdnCustomerId").val();
    $("#ddlCustomerBranch").val(0);
    $("#ddlCustomerBranch").html("");
    if (customerId != undefined && customerId != "" && customerId != "0") {
        var data = { customerId: customerId };
        $.ajax({
            type: "GET",
            url: "../SO/GetCustomerBranchList",
            data: data,
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
                $.each(data, function (i, item) {
                    $("#ddlCustomerBranch").append($("<option></option>").val(item.CustomerBranchId).html(item.BranchName));
                });
                $("#ddlCustomerBranch").val(customerBranchId);
            },
            error: function (Result) {
                $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
            }
        });
    }
    else {
        $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
    }
}

function SaveData() {

    var hdnPurchaseIndentId = $("#hdnPurchaseIndentId");
    var txtIndentDate = $("#txtIndentDate");
    var ddlIndentType = $("#ddlIndentType");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var hdnCustomerId = $("#hdnCustomerId");
    var hdnUserId = $("#hdnUserId");
    var ddlCustomerBranch = $("#ddlCustomerBranch");
    var txtIndentByUser = $("#txtIndentByUser");
    var txtRemarks1 = $("#txtRemarks1");
    var txtRemarks2 = $("#txtRemarks2");
    var ddlIndentStatus = $("#ddlIndentStatus");
    if (ddlIndentType.val() == "0" || ddlIndentType.val() == "") {
        ShowModel("Alert", "Please Select Indent Type")
        ddlIndentType.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "0" || ddlCompanyBranch.val() == "") {
        ShowModel("Alert", "Please Select Company Branch")
        ddlCompanyBranch.focus();
        return false;
    }
    var requisitionId = 0;
    var requisitionNo = "";
    //if (hdnUserId.val() == "0" || hdnUserId.val() == "" || txtIndentByUser.val() == "") {
    //    ShowModel("Alert", "Enter Indent By User Name ");
    //    txtIndentByUser.focus();
    //    return false;
    //}
    var purchaseIndentViewModel = {
        IndentId: hdnPurchaseIndentId.val(),
        IndentDate: txtIndentDate.val().trim(),
        RequisitionId: requisitionId,
        RequisitionNo: requisitionNo,
        IndentType: ddlIndentType.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
        IndentByUserId: hdnUserId.val().trim(),
        CustomerId: hdnCustomerId.val().trim(),
        CustomerBranchId: ddlCustomerBranch.val(),
        Remarks1: txtRemarks1.val(),
        Remarks2: txtRemarks2.val(),
        IndentStatus: ddlIndentStatus.val()
    };

    var purchaseIndentProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var indentProductDetailId = $row.find("#hdnIndentProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var quantity = $row.find("#hdnQuantity").val();
        if (indentProductDetailId != undefined) {
            var indentProduct = {
                ProductId: productId,
                ProductShortDesc: productShortDesc,
                Quantity: quantity

            };
            purchaseIndentProductList.push(indentProduct);
        }
    });

    if (purchaseIndentProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product.")
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdnPurchaseIndentId.val() != null && hdnPurchaseIndentId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { purchaseIndentViewModel: purchaseIndentViewModel, purchaseIndentProducts: purchaseIndentProductList };
    $.ajax({
        url: "../PurchaseIndent/AddEditPurchaseIndent?accessMode=" + accessMode + "",
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
                        window.location.href = "../PurchaseIndent/AddEditPurchaseIndent?purchaseIndentId=" + data.trnId + "&AccessMode=3";
                    }, 2000);
                //ClearFields();
                //if (accessMode == 2) {
                //    setTimeout(
                //  function () {
                //      window.location.href = "../PurchaseIndent/ListPurchaseIndent";
                //  }, 2000);
                //}
                //else {
                //    setTimeout(
                //    function () {
                //        window.location.href = "../PurchaseIndent/AddEditPurchaseIndent";
                //    }, 2000);
                //}
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
    $("#hdnPurchaseIndentId").val("0");
    $("#txtIndentDate").val($("#hdnCurrentDate").val());
    $("#ddlIndentType").val("0");
    $("#ddlCompanyBranch").val("0");
    $("#hdnCustomerId").val("0");
    $("#hdnUserId").val("0");
    $("#ddlCustomerBranch").val("0");
    $("#txtRemarks1").val("");
    $("#txtRemarks2").val("");
    $("#ddlIndentStatus").val("0");
    $("#btnSave").show();
    $("#btnUpdate").hide();
}
function GetPurchaseIndentDetail(indentId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PurchaseIndent/GetPurchaseIndentDetail",
        data: { indentId: indentId },
        dataType: "json",
        success: function (data) {
            $("#txtIndentNo").val(data.IndentNo);
            $("#txtIndentDate").val(data.IndentDate);
            $("#ddlIndentType").val(data.IndentType)
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtIndentByUser").val(data.FullName);
            $("#hdnUserId").val(data.IndentByUserId);
            $("#hdnCustomerId").val(data.CustomerId);
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtCustomerName").val(data.CustomerName);
            BindCustomerBranchList(data.CustomerBranchId);
            $("#ddlCustomerBranch").val(data.CustomerBranchId);


            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);
            $("#ddlIndentStatus").val(data.IndentStatus);
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }

            if (data.ApprovedByUserName != "") {
                $("#divApproved").show();
                $("#txtApprovedBy").val(data.ApprovedByUserName);
                $("#txtApprovedDate").val(data.ApprovedDate);
            }

            if (data.RejectedByUserName != "") {
                $("#divRejected").show();
                $("#txtRejectedBy").val(data.RejectedByUserName);
                $("#txtRejectedDate").val(data.RejectedDate);
                $("#divReject").show();
                $("#txtRejectReason").val(data.RejectedReason)
            }
          
            $("#btnAddNew").show();
            $("#btnPrint").show();           
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function BindLocationList(LocationId) {
    var companyBranchID = $("#ddlCompanyBranch option:selected").val();
    $("#ddlLocation").val(0);
    $("#ddlLocation").html("");
    $.ajax({
        type: "GET",
        url: "../SIN/GetFromLocationList",
        data: { companyBranchID: companyBranchID },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlLocation").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlLocation").append($("<option></option>").val(item.LocationId).html(item.LocationName));
            });

            $("#ddlLocation").val(LocationId);
        },
        error: function (Result) {
            $("#ddlLocation").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}

function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
