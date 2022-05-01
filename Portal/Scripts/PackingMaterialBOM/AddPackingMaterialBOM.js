$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtPMBNo").attr('readOnly', true);
    $("#txtPMBDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtProductCode").attr('readOnly', true);
    $("#txtUOMName").attr('readOnly', true);
    $("#txtProductShortDesc").attr('readOnly', true);


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


    $("#txtPMBDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    BindPackingListType();
    BindProductSubGroup();
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPMBId = $("#hdnPMBId");
    if (hdnPMBId.val() != "" && hdnPMBId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPackingMaterialBOMDetail(hdnPMBId.val());
       }, 3000);

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
    var pMBProducts = [];
    GetPackingMaterialBOMProductList(pMBProducts);

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

/*Add Products*/
function AddProduct(action) {
    $("#txtProductName").attr('readOnly', false);
    var productEntrySequence = 0;
    var flag = true;
    var txtProductName = $("#txtProductName");
    var hdnPMBProductDetailId = $("#hdnPMBProductDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");

    if (txtProductName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Product Name");
        txtProductName.focus();
        return false;
    }
    if (hdnProductId.val().trim() == "" || hdnProductId.val().trim() == "0") {
        ShowModel("Alert", "Please select Product from list")
        hdnProductId.focus();
        return false;
    }
    if (txtQuantity.val().trim() == "" || txtQuantity.val().trim() == "0") {
        ShowModel("Alert", "Please enter Quantity.")
        txtQuantity.focus();
        return false;
    }
    if (txtQuantity.val().trim() != "" && parseFloat(txtQuantity.val().trim()) <= 0) {
        ShowModel("Alert", "Quantity should be greater than zero.")
        txtQuantity.focus();
        return false;
    }
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    var pMBProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var pMBProductDetailId = $row.find("#hdnPMBProductDetailId").val();
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

                var pMBProduct = {
                    PMBProductDetailId: pMBProductDetailId,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity

                };
                pMBProductList.push(pMBProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;

            }
            else if (hdnPMBProductDetailId.val() == pMBProductDetailId && hdnSequenceNo.val() == sequenceNo) {
                var pMBProduct = {
                    PMBProductDetailId: hdnPMBProductDetailId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim()
                };
                pMBProductList.push(pMBProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var pMBProductAddEdit = {
            PMBProductDetailId: hdnPMBProductDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim()
        };
        pMBProductList.push(pMBProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetPackingMaterialBOMProductList(pMBProductList);
    }

}

/*Edit product from product list on edit button*/
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
    $("#txtProductName").attr('readOnly', true);
    $("#txtProductName").val(productName);
    $("#hdnWorkOrderProductDetailId").val(workOrderProductDetailId);
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

/*Remove product from product list on delete button*/
function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var workOrderProductDetailId = $(row).find("#hdnWorkOrderProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
    }
}

/*Save form data with products */
function SaveData() {
    var txtPMBNo = $("#txtPMBNo");
    var hdnPMBId = $("#hdnPMBId");
    var txtPMBDate = $("#txtPMBDate");
    var ddlPackingListType = $("#ddlPackingListType");
    var ddlProductSubGroup = $("#ddlProductSubGroup");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (ddlPackingListType.val() == "" || ddlPackingListType.val() == 0) {
        ShowModel("Alert", "Please select  Packing list type");
        return false;
    }

    if (ddlProductSubGroup.val() == "" || ddlProductSubGroup.val() == 0) {
        ShowModel("Alert", "Please select Product Sub Group.");
        return false;
    }

    var packingMaterialBOMViewModel = {
        PMBId: hdnPMBId.val(),
        PMBNo: txtPMBNo.val().trim(),
        PMBDate: txtPMBDate.val().trim(),
        PackingListTypeId: ddlPackingListType.val(),
        ProductSubGroupid: ddlProductSubGroup.val(),
        CompanyBranchId:0
    };

    var pMBProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var PMBProductDetailId = $row.find("#hdnPMBProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnQuantity").val();



        if (PMBProductDetailId != undefined) {

            var packingMaterialBOMProducts = {
                PMBProductDetailId: PMBProductDetailId,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc,
                UOMName: uomName,
                Quantity: quantity
            };
            pMBProductList.push(packingMaterialBOMProducts);
        }
    });

    if (pMBProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product.")
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdnPMBId.val() != null && hdnPMBId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { packingMaterialBOMViewModel: packingMaterialBOMViewModel, packingMaterialBOMProducts: pMBProductList, CompanyBranchId:0 };
    $.ajax({
        url: "../PackingMaterialBOM/AddEditPackingMaterialBOM?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../PackingMaterialBOM/ListPackingMaterialBOM";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PackingMaterialBOM/AddEditPackingMaterialBOM";
                    }, 2000);
                }
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

/*Show pop up for messages */
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}

/*Clear form fields after insert/update */
function ClearFields() {
    $("#txtPMBNo").val("");
    $("#hdnPMBId").val("0");
    $("#txtPMBDate").val($("#hdnCurrentDate").val());
    $("#ddlPackingListType").val(0);
    $("#btnSave").show();
    $("#btnUpdate").hide();
    ShowHideProductPanel(2);
    var pMBProducts = [];
    GetPackingMaterialBOMProductList(pMBProducts);
}

/*Get Product Details by product id */
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

/*Show hide product add/update form */
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

/*Bind DropDown For Packing List Type */
function BindPackingListType() {

    $("#ddlPackingListType").val(0);
    $("#ddlPackingListType").html("");
    $.ajax({
        type: "GET",
        url: "../PackingList/GetAllPackingListType",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPackingListType").append($("<option></option>").val(0).html("-Select Packing List Type-"));
            $.each(data, function (i, item) {
                $("#ddlPackingListType").append($("<option></option>").val(item.PackingListTypeID).html(item.PackingListTypeName));
            });
        },
        error: function (Result) {
            $("#ddlPackingListType").append($("<option></option>").val(0).html("-Select PackingType-"));
        }
    });
}

/*Bind DropDown For Product Sub Group */
function BindProductSubGroup() {

    $("#ddlProductSubGroup").val(0);
    $("#ddlProductSubGroup").html("");
    $.ajax({
        type: "GET",
        url: "../PackingMaterialBOM/GetProductSubGroupListForPMB",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Product Sub Group-"));
            $.each(data, function (i, item) {
                $("#ddlProductSubGroup").append($("<option></option>").val(item.ProductSubGroupId).html(item.ProductSubGroupName));
            });
        },
        error: function (Result) {
            $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Product Sub Group-"));
        }
    });
}

/*Get Packing Material BOM Detail By Id */
function GetPackingMaterialBOMDetail(pMBId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PackingMaterialBOM/PackingMaterialBOMDetail",
        data: { pMBId: pMBId },
        dataType: "json",
        success: function (data) {
            $("#hdnPMBId").val(data.PMBId);
            $("#txtPMBNo").val(data.PMBNo);
            $("#txtPMBDate").val(data.PMBDate);
            $("#ddlPackingListType").val(data.PackingListTypeId);
            $("#ddlProductSubGroup").val(data.ProductSubGroupid);
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            $("#ddlCompanyBranchId").val(0);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            $("#btnAddNew").show();
            $("#btnPrint").show();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

/*Get Packing Material BOM Products By Id */
function GetPackingMaterialBOMProductList(pMBProducts) {
    var hdnPMBId = $("#hdnPMBId");
    var requestData = { packingMaterialBOMProducts: pMBProducts, pMBId: hdnPMBId.val() };
    $.ajax({
        url: "../PackingMaterialBOM/GetPackingMaterialBOMProductList",
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}