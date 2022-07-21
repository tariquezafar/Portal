$(document).ready(function () {  
    BindProductTypeList();
    BindProductMainGroupList();
    BindSaleUOMList();
    BindPurchaseUOMList();
    BindCompanyBranchList();
    //$("#txtVendorCode").attr("readonly", true);
    //$("#txtProductCode").attr("readonly", true);
    //$("#txtWarrantyInMonth").attr("readonly", true);
    //$("#txtCGSTPercentage").attr("readonly", true);
    //$("#txtSGSTPercentage").attr("readonly", true);
    //$("#txtIGSTPercentage").attr("readonly", true);
    $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnProductId = $("#hdnProductId");
    if (hdnProductId.val() != "" && hdnProductId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        setTimeout(
        function () {
            GetProductDetail(hdnProductId.val());
        }, 2000); 
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide(); 
            $("#FileUpload1").attr('disabled', true);
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true); 
        }
        else
        {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else
    {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }
    $("#txtProductName").focus();

    $("#txtHSNCode").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../HSN/GetHSNAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.HSNCode,
                            value: item.HSNID,
                            CGST_Perc: item.CGST_Perc,
                            SGST_Perc: item.SGST_Perc,
                            IGST_Perc: item.IGST_Perc,                                                 
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtHSNCode").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtHSNCode").val(ui.item.label);
            $("#hdnHSNID").val(ui.item.value);
            $("#txtCGSTPercentage").val(ui.item.CGST_Perc);
            $("#txtSGSTPercentage").val(ui.item.SGST_Perc);
            $("#txtIGSTPercentage").val(ui.item.IGST_Perc);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtHSNCode").val("");
                $("#hdnHSNID").val("0");
                $("#txtCGSTPercentage").val(0);
                $("#txtSGSTPercentage").val(0);
                $("#txtIGSTPercentage").val(0);
               
                ShowModel("Alert", "Please select HSN Code from List")

            }
            return false;
        }

    })
    .autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
          .append("<div><b>" + item.label + " || " + item.label + "</b></div>")
          .appendTo(ul);
    };
    $("#txtVendorName").autocomplete({
        minLength: 2,
        delay:0,
        source: function (request, response) {
            $.ajax({
                url: "../PO/GetVendorAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.VendorName,
                            value: item.VendorId,
                            Address: item.Address,
                            code: item.VendorCode
                        };
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

    $("#txtSizeDesc").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Product/GetSizeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, productMainGroupId: $("#ddlProductMainGroup").val(), productSubGroupId: $("#ddlProductSubGroup").val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.SizeDesc, value: item.SizeId, SizeCode: item.SizeCode
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtSizeDesc").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtSizeDesc").val(ui.item.label);
            $("#hdnSizeId").val(ui.item.value);
            $("#hdnSizeCode").val(ui.item.SizeCode);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtSizeDesc").val("");
                $("#hdnSizeId").val("0");
                $("#hdnSizeCode").val("0");
                ShowModel("Alert", "Please select Size from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.SizeCode + "</b></div>")
      .appendTo(ul);
};

 
    $("#txtBrandName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Product/GetManufacturerAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {term: request.term},
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.ManufacturerName, value: item.ManufacturerId, ManufacturerCode: item.ManufacturerCode
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtBrandName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtBrandName").val(ui.item.label);
            $("#hdnManufacturerId").val(ui.item.value);
            $("#hdnManufacturerCode").val(ui.item.ManufacturerCode);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtBrandName").val("");
                $("#hdnManufacturerId").val("0");
                $("#hdnManufacturerCode").val("0");
                ShowModel("Alert", "Please select Manufacturer from List")

            }
            return false;
        }

    })
    .autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
          .append("<div><b>" + item.label + " || " + item.ManufacturerCode + "</b></div>")
          .appendTo(ul);
    };
    
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
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
}; 

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
function BindProductTypeList()
{
    $.ajax({
        type: "GET",
        url: "../Product/GetProductTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlProductType").append($("<option></option>").val(0).html("-Select Type-"));
            $.each(data, function (i, item) {
                $("#ddlProductType").append($("<option></option>").val(item.ProductTypeId).html(item.ProductTypeName));
            });
        },
        error: function (Result) {
            $("#ddlProductType").append($("<option></option>").val(0).html("-Select Type-"));
        }
    });
}
function BindProductMainGroupList() {
    $.ajax({
        type: "GET",
        url: "../Product/GetProductMainGroupList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlProductMainGroup").append($("<option></option>").val(0).html("-Select Main Group-"));
            $.each(data, function (i, item) {
                $("#ddlProductMainGroup").append($("<option></option>").val(item.ProductMainGroupId).html(item.ProductMainGroupName + "(" + item.ProductMainGroupCode+")"));
            });
        },
        error: function (Result) {
            $("#ddlProductMainGroup").append($("<option></option>").val(0).html("-Select Main Group-"));
        }
    });
}
function BindProductSubGroupList(productSubGroupId) {
    var productMainGroupId = $("#ddlProductMainGroup option:selected").val();
    $("#ddlProductSubGroup").val(0);
    $("#ddlProductSubGroup").html("");
    if (productMainGroupId != undefined && productMainGroupId != "" && productMainGroupId != "0") {
        var data = { productMainGroupId: productMainGroupId };
        $.ajax({
            type: "GET",
            url: "../Product/GetProductSubGroupList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
                $.each(data, function (i, item) {
                    $("#ddlProductSubGroup").append($("<option></option>").val(item.ProductSubGroupId).html(item.ProductSubGroupName + "(" + item.ProductSubGroupCode+")"));
                });
                $("#ddlProductSubGroup").val(productSubGroupId);
            },
            error: function (Result) {
                $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
            }
        });
    }
    else {

        $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
    }

}
function BindSaleUOMList() {
    $.ajax({
        type: "GET",
        url: "../Product/GetUOMList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlUOM").append($("<option></option>").val(0).html("-Select Sale UOM-"));
            $.each(data, function (i, item) {
                $("#ddlUOM").append($("<option></option>").val(item.UOMId).html(item.UOMName));
            });
        },
        error: function (Result) {
            $("#ddlUOM").append($("<option></option>").val(0).html("-Select Sale UOM-"));
        }
    });
}

function BindPurchaseUOMList() {
    $.ajax({
        type: "GET",
        url: "../Product/GetUOMList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlPurchaseUOM").append($("<option></option>").val(0).html("-Select Purchase UOM-"));
            $.each(data, function (i, item) {
                $("#ddlPurchaseUOM").append($("<option></option>").val(item.UOMId).html(item.UOMName));
            });
        },
        error: function (Result) {
            $("#ddlPurchaseUOM").append($("<option></option>").val(0).html("-Select Purchase UOM-"));
        }
    });
}
 
function ShowHideWarrentyStatus() {
    if ($("#chkIsWarrantyProduct").is(':checked')) {
        $("#txtWarrantyInMonth").attr("readonly", false);
    }
    else {
        $("#chkIsWarrantyProduct").attr("readonly", true);
        $("#txtWarrantyInMonth").val("");
    }
}
function GetProductDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productId: productId },
        dataType: "json",
        success: function (data) { 
            $("#txtProductName").val(data.ProductName);
            $("#txtProductCode").val(data.ProductCode);
            $("#txtProductShortDesc").val(data.ProductShortDesc);
            $("#txtProductFullDesc").val(data.ProductFullDesc);
            $("#ddlProductType").val(data.ProductTypeId);
            $("#ddlProductMainGroup").val(data.ProductMainGroupId);
            BindProductSubGroupList(data.ProductSubGroupId);
            $("#ddlProductSubGroup").val(data.ProductSubGroupId);
            $("#ddlAssemblyType").val(data.AssemblyType);
            $("#ddlUOM").val(data.UOMId);
            $("#ddlPurchaseUOM").val(data.PurchaseUOMId);
            $("#txtPurchasePrice").val(data.PurchasePrice);
            $("#txtSalePrice").val(data.SalePrice);
            $("#txtSizeDesc").val(data.SizeDesc);
            $("#hdnSizeId").val(data.SizeId);
            $("#hdnSizeCode").val(data.SizeCode);
            $("#txtLength").val(data.Length);
            $("#txtBrandName").val(data.ManufacturerName);
            $("#hdnManufacturerId").val(data.ManufacturerId);
            $("#hdnManufacturerCode").val(data.ManufacturerCode);
            $("#txtRackNo").val(data.RackNo);

            $("#txtLocalName").val(data.LocalName);
            $("#txtCompatibility").val(data.Compatibility);


            $("#hdnHSNID").val(data.HSNID);
            $("#txtModelName").val(data.ModelName);
            $("#txtCC").val(data.CC);




            if (data.IsSerializedProduct) {
                $("#chkIsSerializedProduct").attr("checked", true);
               
            }
            else {
                $("#chkIsSerializedProduct").attr("checked", false);

            }

            if (data.IsWarrantyProduct) {
                $("#chkIsWarrantyProduct").attr("checked", true);
                $("#txtWarrantyInMonth").val(data.WarrantyInMonth);
                $("#txtWarrantyInMonth").attr("readonly", false);
            }
            else {
                $("#chkIsWarrantyProduct").attr("checked", false);
                $("#txtWarrantyInMonth").val("");
                $("#txtWarrantyInMonth").attr("readonly", true);
            }
            
            $("#txtMinOrderQty").val(data.MinOrderQty);
            $("#txtReOrderQty").val(data.ReOrderQty);
            $("#imgUserPic").attr("src", "../Images/ProductImages/" + data.ProductPicName);        
            if (data.Product_Status) {
                $("#chkStatus").attr("checked", true);
            }
            else {
                $("#chkStatus").attr("checked", false);
            }

            $("#txtCGSTPercentage").val(data.CGST_Perc);
            $("#txtSGSTPercentage").val(data.SGST_Perc);
            $("#txtIGSTPercentage").val(data.IGST_Perc);
            $("#txtHSNCode").val(data.HSN_Code);
            $("#ddlColourCode").val(data.ColourCode);
            $("#hdnVendorId").val(data.VendorId);
            $("#ddlVehicleType").val(data.VehicleType);
            $("#txtVendorName").val(data.VendorName);
            $("#txtVendorCode").val(data.VendorCode);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            
            if (data.GST_Exempt) {
                $("#chkGSTExempt").attr("checked", true);
            }
            else {
                $("#chkGSTExempt").attr("checked", false);
            }
            if (data.IsNonGST) {
                $("#chkNonGST").attr("checked", true);
            }
            else {
                $("#chkNonGST").attr("checked", false);
            }
            if (data.IsZeroRated)
            {
                $("#chkZeroRated").attr("checked", true);
            }
            else {
                $("#chkZeroRated").attr("checked", false);
            }
            if (data.IsNilRated) {
                $("#chkNilRated").attr("checked", true);
            }
            else
            {
                $("#chkNilRated").attr("checked", false);
            }


            if (data.IsThirdPartyProduct) {
                $("#chkThirdPartyProduct").attr("checked", true);
            }
            else {
                $("#chkThirdPartyProduct").attr("checked", false);
            }

            if (data.OnlineProduct) {
                $("#chkOnlineProduct").attr("checked", true);
            }
            else {
                $("#chkOnlineProduct").attr("checked", false);
            }

            if (data.ProductPicName =="") {
                $("#btnRemoveImg").hide();
            }
            if (data.ProductPicName) {
                $("#btnRemoveImg").show(); 
            } 
            var hdnAccessMode = $("#hdnAccessMode");
            if (hdnAccessMode.val() == "3") {
                $("#btnRemoveImg").hide(); 
            }
            $("#txtMRP").val(data.MRP);
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() {
    
    var hdnProductId = $("#hdnProductId");
    var txtProductName = $("#txtProductName");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtProductFullDesc = $("#txtProductFullDesc");
    var ddlProductType = $("#ddlProductType");
    var ddlProductMainGroup = $("#ddlProductMainGroup");
    var ddlProductSubGroup = $("#ddlProductSubGroup");
    var ddlAssemblyType = $("#ddlAssemblyType");
    var ddlUOM = $("#ddlUOM");
    var ddlPurchaseUOM = $("#ddlPurchaseUOM");
    var txtPurchasePrice = $("#txtPurchasePrice");
    var txtSalePrice = $("#txtSalePrice");
    var chkIsSerializedProduct = $("#chkIsSerializedProduct"); 

    var txtSizeDesc = $("#txtSizeDesc");
    var hdnSizeId = $("#hdnSizeId");  
    var txtLength = $("#txtLength"); 

    var txtBrandName = $("#txtBrandName");
    var hdnManufacturerId = $("#hdnManufacturerId"); 

    var txtReOrderQty = $("#txtReOrderQty");
    var txtMinOrderQty = $("#txtMinOrderQty");
    var chkStatus = $("#chkStatus");

    var txtCGSTPercentage = $("#txtCGSTPercentage");
    var txtSGSTPercentage = $("#txtSGSTPercentage");
    var txtIGSTPercentage = $("#txtIGSTPercentage");
    var txtHSNCode = $("#txtHSNCode");
    var chkGSTExempt = $("#chkGSTExempt");
    var ddlColourCode = $("#ddlColourCode");
    var chkNonGST = $("#chkNonGST");
    var chkIsWarrantyProduct = $("#chkIsWarrantyProduct");
    var txtWarrantyInMonth = $("#txtWarrantyInMonth");
    var chkNilRated = $("#chkNilRated");
    var chkZeroRated = $("#chkZeroRated");

    var chkThirdPartyProduct = $("#chkThirdPartyProduct");
    var txtRackNo = $("#txtRackNo");

    var txtModelName = $("#txtModelName");
    var txtCC = $("#txtCC");
    var hdnHSNID = $("#hdnHSNID");
    var hdnVendorId = $("#hdnVendorId");
    var ddlVehicleType = $("#ddlVehicleType");
    var localName = $("#txtLocalName");
    var compatibility = $("#txtCompatibility");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    
    var chkOnlineProduct = $("#chkOnlineProduct");
    
    if (txtProductName.val().trim() == "") {
        ShowModel("Alert", "Please enter Product Name")
        txtProductName.focus();
        return false;
    }
    //if (txtProductCode.val().trim() == "") {
    //    ShowModel("Alert", "Please enter Product Code")
    //    txtProductCode.focus();
    //    return false;
    //}
    //if (txtProductCode.val().length < 2) {
    //    ShowModel("Alert", "Please enter at least 2 character long Product Code")
    //    txtProductCode.focus();
    //    return false;
    //}
    if (ddlProductType.val() == "" || ddlProductType.val() == "0") {
        ShowModel("Alert", "Please select Product Type")
        ddlProductType.focus();
        return false;
    }
    if (ddlProductMainGroup.val() == "" || ddlProductMainGroup.val() == "0") {
        ShowModel("Alert", "Please select Product Main Group")
        ddlProductMainGroup.focus();
        return false;
    }
    if (ddlProductSubGroup.val() == "" || ddlProductSubGroup.val() == "0") {
        ShowModel("Alert", "Please select Product Sub Group")
        ddlProductSubGroup.focus();
        return false;
    }
    if (ddlAssemblyType.val() == "" || ddlAssemblyType.val() == "0") {
        ShowModel("Alert", "Please select Assembly Type")
        ddlAssemblyType.focus();
        return false;
    }
    if (ddlUOM.val() == "" || ddlUOM.val() == "0") {
        ShowModel("Alert", "Please select Sale Unit of Measurement (UOM)")
        ddlUOM.focus();
        return false;
    }
    if (ddlPurchaseUOM.val() == "" || ddlPurchaseUOM.val() == "0") {
        ShowModel("Alert", "Please select Purchase Unit of Measurement (UOM)")
        ddlPurchaseUOM.focus();
        return false;
    }

    if (ddlColourCode.val() == "" || ddlColourCode.val() == "0") {
        ShowModel("Alert", "Please select Color Code.")
        ddlColourCode.focus();
        return false;
    }
    //if (txtReOrderQty.val().trim() == "" || txtReOrderQty.val().trim() == "0") {
    //    ShowModel("Alert", "Please enter Reorder Qty.")
    //    txtReOrderQty.focus();
    //    return false;
    //}
    var productStatus = true;
    if (chkStatus.prop("checked") == true)
    { productStatus = true; }
    else
    { productStatus = false; }

    var GSTExempt = false;
    if (chkGSTExempt.prop("checked") == true)
    { GSTExempt = true; }
    else
    { GSTExempt = false; }

    var NonGST = false;
    if (chkNonGST.prop("checked") == true)
    { NonGST = true; }
    else
    { NonGST = false; }

    var WarrantyProduct = false;
    if (chkIsWarrantyProduct.prop("checked") == true)
    { WarrantyProduct = true; }
    else
    { WarrantyProduct = false; }

    var serializedProduct = true;
    if (chkIsSerializedProduct.prop("checked") == true)
    { serializedProduct = true; }
    else
    { serializedProduct = false; }

    var serializedProduct = true;
    if (chkIsSerializedProduct.prop("checked") == true)
    { serializedProduct = true; }
    else
    { serializedProduct = false; }

    var purchasePrice = txtPurchasePrice.val().trim() == "" ? "0" : txtPurchasePrice.val().trim();
    var salePrice = txtSalePrice.val().trim() == "" ? "0" : txtSalePrice.val().trim();
    var reOrderQty = txtReOrderQty.val().trim() == "" ? "0" : txtReOrderQty.val().trim();
    var minOrderQty = txtMinOrderQty.val().trim() == "" ? "0" : txtMinOrderQty.val().trim();

    var nilRated = false;
    if (chkNilRated.prop("checked") == true)
    { nilRated = true; }
    else
    { nilRated = false; }

    var zeroRated = false;
    if (chkZeroRated.prop("checked") == true)
    { zeroRated = true; }
    else
    { zeroRated = false; }

    var thirdPartyProduct = false;
    if (chkThirdPartyProduct.prop("checked") == true)
    { thirdPartyProduct = true; }
    else
    { thirdPartyProduct = false; }

    var onlineProduct = false;
    if (chkOnlineProduct.prop("checked") == true)
    { onlineProduct = true; }
    else
    { onlineProduct = false; }


    var productViewModel = {
        Productid: hdnProductId.val(), ProductName: txtProductName.val().trim(), ProductCode: txtProductCode.val().trim(),
        ProductShortDesc: txtProductShortDesc.val().trim(), ProductFullDesc: txtProductFullDesc.val().trim(), ProductTypeId: ddlProductType.val(),
        ProductMainGroupId: ddlProductMainGroup.val(), ProductSubGroupId: ddlProductSubGroup.val(), AssemblyType: ddlAssemblyType.val().trim(),
        UOMId: ddlUOM.val().trim(), PurchaseUOMId: ddlPurchaseUOM.val().trim(), PurchasePrice: purchasePrice, SalePrice: salePrice, LocalTaxRate: 0, CentralTaxRate: 0, OtherTaxRate: 0,
        IsSerializedProduct: serializedProduct,
        BrandName: txtBrandName.val().trim(), SizeId: hdnSizeId.val(), Length: txtLength.val().trim(),
        ManufacturerId: hdnManufacturerId.val(),
        ReOrderQty: reOrderQty, MinOrderQty: minOrderQty, Product_Status: productStatus,
        CGST_Perc: txtCGSTPercentage.val().trim(), SGST_Perc: txtSGSTPercentage.val().trim(),
        IGST_Perc: txtIGSTPercentage.val().trim(), HSN_Code: txtHSNCode.val().trim(), GST_Exempt: GSTExempt,
        ColourCode: ddlColourCode.val().trim(),
        IsNonGST:NonGST,
        IsWarrantyProduct:WarrantyProduct,
        WarrantyInMonth: txtWarrantyInMonth.val().trim(),
        IsNilRated: nilRated,
        IsZeroRated: zeroRated,
        IsThirdPartyProduct: thirdPartyProduct,
        RackNo: txtRackNo.val().trim(),

        ModelName: txtModelName.val(),
        CC: txtCC.val(),
        HSNID: hdnHSNID.val().trim(),
        VendorId :hdnVendorId.val(),
        VehicleType: ddlVehicleType.val(),
        LocalName: localName.val(),
        Compatibility: compatibility.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
        OnlineProduct: onlineProduct,
        ManufactureCode: $("#hdnManufacturerCode").val(),
        MRP: $("#txtMRP").val(),
        ProductMainGroupCode: $('#ddlProductMainGroup option:selected').text().substring($('#ddlProductMainGroup option:selected').text().indexOf('(') + 1, $('#ddlProductMainGroup option:selected').text().indexOf(')')),
        ProductSubGroupCode: $('#ddlProductSubGroup option:selected').text().substring($('#ddlProductSubGroup option:selected').text().indexOf('(') + 1, $('#ddlProductSubGroup option:selected').text().indexOf(')')),
};
    var accessMode = 1;//Add Mode
    if (hdnProductId.val() != null && hdnProductId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { productViewModel: productViewModel };
    $.ajax({
        url: "../Product/AddEditProduct?AccessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                SaveProductImage(data.trnId);
                if ($("#hdnRemoveImage").val() == 1) {
                    RemoveImage();
                }
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
                  function () {
                      window.location.href = "../Product/ListProduct";
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
function ShowModel(headerText,bodyText)
{
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText); 
}
function ClearFields()
{
    $("#hdnProductId").val("0");
    $("#txtProductName").val("");
    $("#txtProductCode").val("");
    $("#txtProductShortDesc").val("");
    $("#txtProductFullDesc").val("");
    $("#ddlProductType").val("0");
    $("#ddlProductMainGroup").val("0");
    $("#ddlProductSubGroup").val("0");
    $("#ddlAssemblyType").val("0");
    $("#ddlUOM").val("0");
    $("#ddlPurchaseUOM").val("0"); 
    $("#txtPurchasePrice").val("");
    $("#txtSalePrice").val("");
    $("#txtLocalTaxRate").val("");
    $("#txtCentralTaxRate").val("");
    $("#txtOtherTaxRate").val("");
    $("#txtBrandName").val("");
    $("#txtSizeDesc").val("");
    $("#hdnSizeId").val("0");
    $("#hdnSizeCode").val("0"); 
    $("#txtLength").val(""); 
    $("#txtBrandName").val("");
    $("#hdnManufacturerId").val("0");
    $("#hdnManufacturerCode").val("0");
    $("#chkIsSerializedProduct").attr("checked", false);
    $("#txtMinOrderQty").val("");
    $("#txtCGSTPercentage").val("");
    $("#txtSGSTPercentage").val("");
    $("#txtIGSTPercentage").val("");
    $("#txtHSNCode").val("");
    $("#chkGSTExempt").attr("checked", false); 
    $("#txtReOrderQty").val("");
    $("#chkStatus").attr("checked", true);
    $("#btnRemoveImg").hide();
    document.getElementById('FileUpload1').value = "";
    document.getElementById('imgUserPic').src = "";
}
function ShowImagePreview(input) { 
    var fname = input.value;
    var ext = fname.split(".");
    var x = ext.length;
    var extstr = ext[x - 1].toLowerCase();
    if (extstr == 'jpg' || extstr == 'jpeg' || extstr == 'png' || extstr == 'gif') {
       
    }
    else { 
        alert("File doesnt match png, jpg or gif");
        input.focus();
        input.value = "";
        return false;
    }
    if (typeof (FileReader) != "undefined") {
        if (input.files && input.files[0]) {
            if (input.files[0].name.length < 1) {
            }
            else if (input.files[0].size > 50000000) {
                input.files[0].name.length = 0;
                alert("File is too big");
                input.value = "";
                return false;
            }
            else if (input.files[0].type != 'image/png' && input.files[0].type != 'image/jpg' && !input.files[0].type != 'image/gif' && input.files[0].type != 'image/jpeg') {
                input.files[0].name.length = 0;
                alert("File doesnt match png, jpg or gif");
                input.value = "";
                return false;
            }
            var reader = new FileReader();
            reader.onload = function (e) {
                $("#imgUserPic").prop('src', e.target.result)
                    .width(150)
                    .height(150);
             
            };
            reader.readAsDataURL(input.files[0]);
            
            if ($("#FileUpload1").val() != '') {
                $("#btnRemoveImg").show();
            }
            else { 
                $("#btnRemoveImg").hide(); 
            }
           
       
        }
    }
    else {
        
        alert("This browser does not support FileReader.");
        input.value = "";
        //return false;
    } 
}
function SaveProductImage(productId) {
    if (parseInt(productId) <= 0) {
        ShowModel("Alert", "Product ID not available")
        return false;
    } 
    if (window.FormData !== undefined) {
        var uploadfile = document.getElementById('FileUpload1');
        var fileData = new FormData();
        if (uploadfile.value != '') {
           
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;

            if (uploadfile.files[0].size > 50000000) {
                uploadfile.files[0].name.length = 0;
                ShowModel("Alert", "File is too big")
                uploadfile.value = "";
                return false;
            }

            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
        }
        fileData.append("productId", productId);

    } else {

        ShowModel("Alert", "FormData is not supported.")
    }

    $.ajax({
        url: "../Product/UpdateProductPic",
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        error: function () {
            ShowModel("Alert", "An error occured")
            return;
        },
        success: function (result) {
            if (result.status == "SUCCESS") {
              
                document.getElementById('FileUpload1').value = "";
                document.getElementById('imgUserPic').src = "";
            }
            else {
                ShowModel("Alert", result.message);
            }
        }
    });
}
 
function BlankPerCentagechkNonGST() {
    if ($("#chkNonGST").is(':checked')) {
            if (confirm("Are you sure to reset GST Tax Fields ?.")) {
                $("#txtSGSTPercentage").val("");
                $("#txtIGSTPercentage").val("");
                $("#txtCGSTPercentage").val("");

                $("#txtSGSTPercentage").attr('readOnly', true);
                $("#txtIGSTPercentage").attr('readOnly', true);
                $("#txtCGSTPercentage").attr('readOnly', true);

                $("#chkGSTExempt").attr("checked", false);
                $("#chkNilRated").attr("checked", false);
            }
     
        
    }
    else {
        $("#txtSGSTPercentage").attr('readOnly', false);
        $("#txtIGSTPercentage").attr('readOnly', false);
        $("#txtCGSTPercentage").attr('readOnly', false);
    }
   
}
function BlankPerCentagechkGSTExempt() {
    if ($("#chkGSTExempt").is(':checked')) {
        if (confirm("Are you sure to reset GST Tax Fields ?.")) {
            $("#txtSGSTPercentage").val("");
            $("#txtIGSTPercentage").val("");
            $("#txtCGSTPercentage").val("");

            $("#txtSGSTPercentage").attr('readOnly', true);
            $("#txtIGSTPercentage").attr('readOnly', true);
            $("#txtCGSTPercentage").attr('readOnly', true);

          
            $("#chkNonGST").attr("checked", false);
            $("#chkNilRated").attr("checked", false);
        }
       


    }
    else {
        $("#txtSGSTPercentage").attr('readOnly', false);
        $("#txtIGSTPercentage").attr('readOnly', false);
        $("#txtCGSTPercentage").attr('readOnly', false);
    }

}

function BlankNilRatedGSTExempt() {
    if ($("#chkNilRated").is(':checked')) {
        if (confirm("Are you sure to reset GST Tax Fields ?.")) {
            $("#txtSGSTPercentage").val("");
            $("#txtIGSTPercentage").val("");
            $("#txtCGSTPercentage").val("");

            $("#txtSGSTPercentage").attr('readOnly', true);
            $("#txtIGSTPercentage").attr('readOnly', true);
            $("#txtCGSTPercentage").attr('readOnly', true);

            $("#chkGSTExempt").attr("checked", false);
            $("#chkNonGST").attr("checked", false);
        }



    }
    else {
        $("#txtSGSTPercentage").attr('readOnly', false);
        $("#txtIGSTPercentage").attr('readOnly', false);
        $("#txtCGSTPercentage").attr('readOnly', false);
    }

}

function ConfirmRemoveImage() {
    if (confirm("Do you want to remove selected Image?")) {
        $("#hdnRemoveImage").val(1);
        document.getElementById('FileUpload1').value = "";
        document.getElementById('imgUserPic').src = "";
        $("#btnRemoveImg").hide();
    }
}
function RemoveImage() {  
            var hdnProductId = $("#hdnProductId");
            var requestData = { productId: hdnProductId.val() };
            $.ajax({
                url: "../Product/RemoveImage",
                cache: false,
                type: "POST",
                dataType: "json",
                data: JSON.stringify(requestData),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status == "SUCCESS") {
                        ShowModel("Alert", data.message);
                        document.getElementById('FileUpload1').value = "";
                        document.getElementById('imgUserPic').src = "";
                        $("#btnRemoveImg").hide();
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
function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
}

function OpenVendorMasterPopup() {
     $("#AddNewVendor").modal();
   
}
function GetVendorDetail(vendorId) {
    //$.ajax({
    //    type: "GET",
    //    asnc: false,
    //    url: "../Vendor/GetVendorDetail",
    //    data: { vendorId: vendorId },
    //    dataType: "json",
    //    success: function (data) {
            

    //    },
    //    error: function (Result) {
    //        ShowModel("Alert", "Problem in Request");
    //    }
    //});

}
document.onkeypress = stopRKey;