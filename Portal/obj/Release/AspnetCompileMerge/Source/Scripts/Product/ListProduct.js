
$(document).ready(function () {
    BindCompanyBranchListA();
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
        }
    });
    BindProductTypeList();
    BindProductMainGroupList();
    BindCompanyBranchList();

    var hdnPendingStatus = $("#hdnPendingStatus");
    if (hdnPendingStatus.val() == "Pending") {
        SearchProductReorder();
    }
    $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
    setTimeout(
  function () {
      var hdntodayproduct = $("#hdntodayproduct");
      if (hdntodayproduct.val() == "true" || hdntodayproduct.val() == "today")
      {
          SearchProduct();
      }   
  }, 1000);

});
function BindCompanyBranchListA() {
    $("#ddlCompanyBranchA").val(0);
    $("#ddlCompanyBranchA").html("");
    $.ajax({
        type: "GET",
        url: "../DeliveryChallan/GetCompanyBranchList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCompanyBranchA").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranchA").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}
function BindProductTypeList() {
    $.ajax({
        type: "GET",
        url: "../Product/GetProductTypeList",
        data: "{}",
        dataType: "json",
        asnc: true,
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
        asnc: true,
        success: function (data) {
            $("#ddlProductMainGroup").append($("<option></option>").val(0).html("-Select Main Group-"));
            $.each(data, function (i, item) {
                $("#ddlProductMainGroup").append($("<option></option>").val(item.ProductMainGroupId).html(item.ProductMainGroupName));
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
                    $("#ddlProductSubGroup").append($("<option></option>").val(item.ProductSubGroupId).html(item.ProductSubGroupName));
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
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};

function ClearFields()
{
    //$("#txtProductName").val("");
    //$("#txtProductCode").val("");
    //$("#txtProductShortDesc").val("");
    //$("#txtProductFullDesc").val("");
    //$("#ddlProductType").val("0");
    //$("#ddlProductMainGroup").val("0");
    //$("#ddlAssemblyType").val("0");
    //$("#txtBrandName").val("");
    //$("#hdntodayproduct").val("0");
    //$("#hdntodayproduct").val("0");
    //$("#hdnPendingStatus").val("0");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    //$("#hdntodayproduct").val("0");

    window.location.href = "../Product/ListProductReorderQuantity";
    
}
function SearchProduct() {
    var txtProductName = $("#txtProductName");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtProductFullDesc = $("#txtProductFullDesc");
    var ddlProductType = $("#ddlProductType");
    var ddlProductMainGroup = $("#ddlProductMainGroup");
    var ddlProductSubGroup = $("#ddlProductSubGroup");
    var ddlAssemblyType = $("#ddlAssemblyType");
    var txtBrandName = $("#txtBrandName");
    var hdntodayproduct = $("#hdntodayproduct");
    var hdncurrentDate = $("#hdncurrentDate");

    var txtModelName = $("#txtModelName");
    var txtHSNCode = $("#txtHSNCode");
    var txtCC = $("#txtCC");

    var txtVendorName = $("#txtVendorName");
    var txtVendorCode = $("#txtVendorCode");
    var txtLocalName = $("#txtLocalName");
    var txtCompatibility = $("#txtCompatibility");
    var ddlCompanyBranch = $("#ddlCompanyBranchA");

    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var formdate = "";
    var todate = "";
    if (hdntodayproduct.val() == "today")
    {
        formdate = hdncurrentDate.val();
        todate = hdncurrentDate.val();
    }
    else {
        formdate = txtFromDate.val();
        todate = txtToDate.val();
    }
    var requestData = {
        productName: txtProductName.val().trim(),
        productCode: txtProductCode.val().trim(),
        productShortDesc: txtProductShortDesc.val().trim(),
        productFullDesc: txtProductFullDesc.val().trim(),
        productTypeId: ddlProductType.val(),
        productMainGroupId: ddlProductMainGroup.val(),
        productSubGroupId: ddlProductSubGroup.val(),
        assemblyType: ddlAssemblyType.val(),
        brandName: txtBrandName.val().trim(),
        fromDate: formdate,
        toDate: todate,
        modelName: txtModelName.val().trim(),
        hsnCode: txtHSNCode.val().trim(),
        cc: txtCC.val().trim(),
        vendorName: txtVendorName.val().trim(),
        vendorCode: txtVendorCode.val().trim(),
        localName: txtLocalName.val().trim(),
        compatibility: txtCompatibility.val().trim(),
        companyBranchId: ddlCompanyBranch.val()
    };
    $.ajax({
        url: "../Product/GetProductList",
        data: requestData,
        dataType: "html",
        asnc: true,
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

function SearchProductReorder() {

    var txtProductName = $("#txtProductName");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtProductFullDesc = $("#txtProductFullDesc");
    //var ddlProductType = $("#ddlProductType");
    //var ddlProductMainGroup = $("#ddlProductMainGroup");
    //var ddlAssemblyType = $("#ddlAssemblyType");
    //var txtBrandName = $("#txtBrandName");
    //productName: txtProductName.val().trim(), productCode: txtProductCode.val().trim(), productShortDesc: txtProductShortDesc.val().trim(), productFullDesc: txtProductFullDesc.val().trim(), productTypeId: ddlProductType.val(), productMainGroupId: ddlProductMainGroup.val(), assemblyType: ddlAssemblyType.val(), brandName: txtBrandName.val().trim() 
    var requestData = { productName: txtProductName.val().trim(), productCode: txtProductCode.val().trim(), productShortDesc: txtProductShortDesc.val().trim(), productFullDesc: txtProductFullDesc.val().trim() };
    $.ajax({
        url: "../Product/GetProductReorderQuantityList",
        data: requestData,
        dataType: "html",
        asnc: true,
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

function Export() {
    var divList = $("#divList");
    ExporttoExcel(divList);
}
//----Code For Raise Indent---Add Field Company Branch Disscussion Anil Sir//
function SaveData() {
    var hdnCustomerId = $("#hdnCustomerId");
    var hdnCustomerBranchId = $("#hdnCustomerBranchId");
    var hdnIndentByUserId = $("#hdnIndentByUserId");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var hdnCurrentDate = $("#hdnCurrentDate");

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        ddlCompanyBranch.focus();
        return false;
    }
    var purchaseIndentViewModel = {
        IndentId: 0,
        IndentDate: hdnCurrentDate.val(),
        IndentType: "PO",
        CompanyBranchId: ddlCompanyBranch.val(),
        IndentByUserId: hdnIndentByUserId.val(),
        CustomerId: hdnCustomerId.val(),
        CustomerBranchId: hdnCustomerBranchId.val(),
        Remarks1: "Auto Indent From Inventory Dashboard!!",
        Remarks2: "Auto Indent From Inventory Dashboard!!",
        IndentStatus: "Draft"
    };

    var purchaseIndentProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var indentProductDetailId = $row.find("#hdnIndentProductDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var quantity = $row.find("#hdnBalanceQuantity").val();
        var chkIndent = $row.find("#chkIndent").prop("checked");

        if (indentProductDetailId != undefined) {
            if (quantity > 0 && chkIndent == true) {
                var indentProduct = {
                    ProductId: productId,
                    ProductShortDesc: productShortDesc,
                    Quantity: quantity
                };
                purchaseIndentProductList.push(indentProduct);
            }
        }
    });
    if (purchaseIndentProductList.length == 0) {
        ShowModel("Alert", "Please Select at least 1 Product for Indent");
        return false;
    }

    if (purchaseIndentProductList.length > 0) {
        var requestData = { purchaseIndentViewModel: purchaseIndentViewModel, purchaseIndentProducts: purchaseIndentProductList };
        $.ajax({
            url: "../PurchaseIndent/AddEditPurchaseIndent",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
                    ShowModel("Message", data.indentMessage);
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
        }
    });
}
///----------end code---------////////

function UpdateOnlineCode(productId) {
    $.ajax({
        url: "../Product/UpdateAndCancelOnlineCode",
        data: { productId: productId, status:"Yes" },
        dataType: "text",
        asnc: true,
        type: "Get",
        error: function (err) {           
        },
        success: function (data) {
            SearchProduct();
        }
    });
}