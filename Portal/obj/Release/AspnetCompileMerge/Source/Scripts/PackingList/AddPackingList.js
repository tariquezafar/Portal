$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });

 
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
  
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
    BindCompanyBranchList();
    BindSubGroupList();
    BindPackingListType();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPackingListId = $("#hdnPackingListID");
    if (hdnPackingListId.val() != "" && hdnPackingListId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPackingListDetail(hdnPackingListId.val());
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

    var packingListProducts = [];
    GetPackingListProductList(packingListProducts);

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

function BindSubGroupList() {

    $("#ddlSubGroupType").val(0);
    $("#ddlSubGroupType").html("");
    $.ajax({
        type: "GET",
        url: "../PackingList/GetProductSubGroup",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSubGroupType").append($("<option></option>").val(0).html("-Select SubGroup-"));
            $.each(data, function (i, item) {
                $("#ddlSubGroupType").append($("<option></option>").val(item.ProductSubGroupId).html(item.ProductSubGroupName));
            });
        },
        error: function (Result) {
            $("#ddlSubGroupType").append($("<option></option>").val(0).html("-Select SubGroup-"));
        }
    });
}

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
            $("#ddlPackingListType").append($("<option></option>").val(0).html("-Select PackingType-"));
            $.each(data, function (i, item) {
                $("#ddlPackingListType").append($("<option></option>").val(item.PackingListTypeID).html(item.PackingListTypeName));
            });
        },
        error: function (Result) {
            $("#ddlPackingListType").append($("<option></option>").val(0).html("-Select PackingType-"));
        }
    });
}

function GetPackingListProductList(packingListProducts) {
    var hdnPackingListID = $("#hdnPackingListID");
    var requestData = { packingListProducts: packingListProducts, packingListId: hdnPackingListID.val() };
    $.ajax({
        url: "../PackingList/GetPackingListProductList",
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
    var hdnPackingListDetailedID = $("#hdnPackingListDetailedID");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");
    var ddlIsComplimentary = $("#ddlIsComplimentary");

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

    var packingListProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var packingListDetailedID = $row.find("#hdnPackingListDetailedID").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnQuantity").val();
        var isComplimentary = $row.find("#hdnIsComplimentary").val();
        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var packingListProduct = {
                    PackingListDetailId: packingListDetailedID,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity,
                    IsComplimentary: isComplimentary

                };
                packingListProductList.push(packingListProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;

            }
            else if (hdnPackingListDetailedID.val() == packingListDetailedID && hdnSequenceNo.val() == sequenceNo) {
                var packingListProduct = {
                    PackingListDetailId: packingListDetailedID,
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    IsComplimentary: ddlIsComplimentary.val()
                };
                packingListProductList.push(packingListProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var packingListProductAddEdit = {
            PackingListDetailedID: hdnPackingListDetailedID.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim(),
            IsComplimentary: ddlIsComplimentary.val(),
        };
        packingListProductList.push(packingListProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetPackingListProductList(packingListProductList);
    }

}
function EditProductRow(obj) {
    var row = $(obj).closest("tr");
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var PackingListDetailedId = $(row).find("#hdnPackingListDetailedID").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var quantity = $(row).find("#hdnQuantity").val();  
    $("#txtProductName").val(productName);
    $("#hdnPackingListDetailedID").val(PackingListDetailedId);
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

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var paintProcessDetailId = $(row).find("#hdnPaintProcessDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
    }
}

function RemoveProductRowCheckbox() {
    var CountRow=0;
    if (confirm("Do you want to remove selected Product?")) {
       


        $('#tblProductList tr').each(function (i, row) {
            var $row = $(row);
            var packingListDetailedID = $row.find("#hdnPackingListDetailedID").val();
            var chkDeleteRow = $row.find("#chkDeleteRow").is(':checked') ? true : false;
            if (packingListDetailedID != undefined && chkDeleteRow == true) {
                CountRow++;
                row.remove();
            }
        });

        if (CountRow > 0) {
            ShowModel("Alert", "Product Removed from List.");
        }
        else
        {
            ShowModel("Alert", "Please Select At least One Product from List.");
        }
        
    }
}



function GetPackingListDetail(packingListId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PackingList/GetPackingListDetail",
        data: { packingListId: packingListId },
        dataType: "json",
        success: function (data) {
            $("#ddlPackingListType").val(data.PackingListTypeID);
            $("#hdnPackingListID").val(data.PackingListID);
            $("#ddlSubGroupType").val(data.ProductSubGroupId);
            $("#ddlPackingListType").val(data.PackingListTypeID);
            $("#txtPackingListName").val(data.PackingListName);
            $("#txtPackingListDescription").val(data.PackingListDescription);
            $("#ddlPackingListStatus").val(data.PackingListStatus);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            
            $("#ddlSubGroupType").attr('readOnly', true);
            $("#ddlPackingListType").attr('readOnly', true);
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
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

function SaveData() {

    var hdnPackingListId = $("#hdnPackingListID");
    var ddlPackingListType = $("#ddlPackingListType");
    var ddlSubGroupType = $("#ddlSubGroupType");
    var txtPackingListName = $("#txtPackingListName");
    var txtPackingListDescription = $("#txtPackingListDescription");
    var ddlPackingListStatus = $("#ddlPackingListStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (ddlPackingListType.val() == "" || ddlPackingListType.val() == 0) {
        ShowModel("Alert", "Please select Packing List Type ");
        ddlPackingListType.focus();
        return false;
    }

    if (ddlSubGroupType.val() == "" || ddlSubGroupType.val() == 0) {
        ShowModel("Alert", "Please select  Sub Group Type")
        ddlSubGroupType.focus();
          return false;
      }
  
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch.")
        ddlCompanyBranch.focus();
        return false;
    }

    var packingListViewModel = {
        PackingListId: hdnPackingListId.val(),
        PackingListTypeID: ddlPackingListType.val().trim(),
        ProductSubGroupId: ddlSubGroupType.val().trim(),
        PackingListName: txtPackingListName.val(),
        PackingListDescription: txtPackingListDescription.val(),
        PackingListStatus: ddlPackingListStatus.val(),
        CompanyBranchId: ddlCompanyBranch.val()
    };

   

    var packingListProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var packingListDetailedID = $row.find("#hdnPackingListDetailedID").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnQuantity").val();
        var isComplementry = $row.find("#hdnIsComplimentary").val();
        if (packingListDetailedID != undefined) {

            var packingListProduct = {

                PackingListDetailId: packingListDetailedID,
                //SequenceNo:hdnSequenceNo.val(),
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc,
                UOMName: uomName,
                Quantity: quantity,
                IsComplimentary: isComplementry
            };
            packingListProductList.push(packingListProduct);
        }
    });
    var accessMode = 1;//Add Mode
    if (hdnPackingListId.val() != null && hdnPackingListId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    if (packingListProductList.length == "0") {
        ShowModel("Alert", "Please Add Product in List");
        return false;
    }

    var requestData = { packingListViewModel: packingListViewModel, packingListProducts: packingListProductList };
    $.ajax({
        url: "../PackingList/AddEditPackingList?accessMode=" + accessMode + "",
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
                        window.location.href = "../PackingList/AddEditPackingList?packingListId=" + data.trnId + "&AccessMode=3";
                       //window.location.href = "../PackingList/ListPackingList";
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

    $("#ddlPackingListType").val("0");
    $("#ddlSubGroupType").val("0");
    $("#txtPackingListName").val("");
    $("#txtPackingListDescription").val("");
    $("#ddlPackingListStatus").val("Final");
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
//   GetPackingListBomProductList(packingListProducts);

function GetPackingListBomProductList() {

    var ddlSubGroupType = $("#ddlSubGroupType");
    var packingListProducts = [];
    if (ddlSubGroupType.val() != "" || ddlSubGroupType.val() != 0) {
        var hdnProductSubGroupId = ddlSubGroupType.val();

        var requestData = { productSubGroupId: hdnProductSubGroupId };
        $.ajax({
            url: "../PackingList/GetPackingListBOMProductList",
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
/*--------End------------*/


