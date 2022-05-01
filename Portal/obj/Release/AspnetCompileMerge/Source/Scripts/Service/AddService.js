$(document).ready(function () {
    debugger;
    BindProductTypeList();

    $("#txtServiceNo").attr('readOnly', true);
    $("#txtServiceDate").attr('readOnly', true);



    $("#txtServiceDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnServicesId = $("#hdnServiceId");
    if (hdnServicesId.val() != "" && hdnServicesId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetServicesDetail(hdnServicesId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true);
        }
        else {
            $("#btnSave").hide();
            $("#btnUpdateProduct").show();
            $("#btnReset").show();
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }
    $("#txtProductName").autocomplete({
        minLength: 0,
        source: function (request, response) {

            var ddlProductType = $("#ddlProductType");

            if (ddlProductType.val() == "0" || ddlProductType.val() == 0 || ddlProductType.val() == "") {
                ShowModel("Alert", "Please select Services!.");
                return false;

            }


            $.ajax({
                url: "../Product/GetProductTypeBYProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, productTypeId: ddlProductType.val() },
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

    var serviceProducts = [];
    GetServiceProductList(serviceProducts);


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


function GetServicesDetail(serviceId) {
    debugger;
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Service/GetServiceDetail",
        data: { serviceId: serviceId },
        dataType: "json",
        success: function (data) {
            $("#hdnServiceId").val(data.ServiceId);
            $("#txtServiceNo").val(data.ServiceNo);
            $("#txtServiceDate").val(data.ServiceDate);
            $("#ddlApprovalStatus").val(data.ApprovalStatus);
            
           
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function SaveData() {
    var hdnServiceId = $("#hdnServiceId");
    var txtServiceNo = $("#txtServiceNo");
    var txtServiceDate = $("#txtServiceDate");

    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var servicViewModel = {
        ServiceId: hdnServiceId.val(),
        ServiceNo: txtServiceNo.val().trim(),
        ServiceDate: txtServiceDate.val().trim(),
        ApprovalStatus: ddlApprovalStatus.val(),
    }
   
    var serviceViewModel = [];
    $('#tblProductServiceList tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var serviceItemId = $row.find("#hdnServiceItemId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var serviceItemName = $row.find("#hdnServiceItemName").val();
        var notes = $row.find("#hdnNotes").val();
        var ddlProductType = $row.find("#ddlProductType").val();
       
        if (productId != undefined) {

            var serviceProduct = {
                ProductId: productId,
                ServiceItemName: serviceItemName,
                Notes: notes,
                ProductTypeID: ddlProductType,
            };
            serviceViewModel.push(serviceProduct);
        }
    });

    var requestData = { servicViewModel: servicViewModel, serviceViewModel: serviceViewModel };
    $.ajax({
        url: "../Service/AddEditService",
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
                       window.location.href = "../Service/AddEditService?serviceId=" + data.trnId + "&AccessMode=3";
                   }, 2000);
                $("#btnSave").hide();
                $("#btnUpdateProduct").hide();
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
    $("#hdnServicesId").val("0");
    $("#txtServicesName").val("");
    $("#chkStatus").prop("checked", true);

}
function AddProduct(action) {

    var productEntrySequence = 0;
    var flag = true;

    var hdnServiceItemId = $("#hdnServiceItemId");
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtProductName = $("#txtProductName");
    var txtServiceItem = $("#txtServiceItem");
    var hdnProductId = $("#hdnProductId");

    var ddlProductType = $("#ddlProductType");
    var txtNotes = $("#txtNotes");

  
    if (txtProductName.val().trim() == "") {
        ShowModel("Alert", "Please Select Product Name")
        txtProductName.focus();
        return false;
    }
    if (hdnProductId.val().trim() == "" || hdnProductId.val().trim() == "0") {
        ShowModel("Alert", "Please select Product Name")
        hdnProductId.focus();
        return false;
    }
    
    if (txtServiceItem.val().trim() == "") {
        ShowModel("Alert", "Please Enter Service Item Name.")
        txtServiceItem.focus();
        return false;
    }
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    var serviceProductList = [];
    $('#tblProductServiceList tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var serviceItemId = $row.find("#hdnServiceItemId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var serviceItemName = $row.find("#hdnServiceItemName").val();
        var notes = $row.find("#hdnNotes").val();
        var productType = $row.find("#ddlProductType").val();
       

        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (serviceItemName == txtServiceItem.val() && productId == hdnProductId.val()) {
                    ShowModel("Alert", "Service Item Name already added !!!")
                    txtServiceItem.focus();
                    flag = false;
                    return false;
                }
             
                var serviceProduct = {
                    ServiceItemId: serviceItemId,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ServiceItemName: serviceItemName,
                    Notes: notes,
                    ProductTypeID: productType,
                };



                serviceProductList.push(serviceProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnProductId.val() == productId && hdnSequenceNo.val() == sequenceNo) {
                var serviceProductAddEdit = {

                    ServiceItemId: hdnServiceItemId.val(),
                    SequenceNo: hdnSequenceNo.val(),                    
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ServiceItemName: txtServiceItem.val().trim(),
                    Notes: txtNotes.val().trim(),
                    ProductTypeID: ddlProductType.val(),
                  
                };
                serviceProductList.push(serviceProductAddEdit);
                hdnSequenceNo.val("0");
            }
        }

    });


    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {

        var serviceProductAddEdit = {
            ServiceItemId: hdnServiceItemId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ServiceItemName: txtServiceItem.val().trim(),
            Notes: txtNotes.val().trim(),
            ProductTypeID: ddlProductType.val(),
        };
        serviceProductList.push(serviceProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetServiceProductList(serviceProductList);
    }

}
function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var serviceItemId = $(row).find("#hdnServiceItemId").val();    
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var serviceItemName = $(row).find("#hdnServiceItemName").val();
    var notes = $(row).find("#hdnNotes").val();
    var ddlProductType = $(row).find("#ddlProductType").val();
   

    $("#hdnSequenceNo").val(sequenceNo);
    $("#txtProductName").val(productName);
    $("#hdnServiceItemId").val(serviceItemId);
    $("#hdnProductId").val(productId);

    $("#txtServiceItem").val(serviceItemName);
    $("#txtNotes").val(notes);
    $("#ddlProductType").val(ddlProductType);
   
     
    $("#btnAdd").hide();
    $("#btnUpdateProduct").show();

   

}
function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var hdnServiceItemId = $(row).find("#hdnServiceItemId").val();
        alert("Product Removed from List.");
        row.remove();

    }
}
function GetServiceProductList(serviceProducts) {
    var hdnServiceId = $("#hdnServiceId");
    var requestData = { serviceProducts: serviceProducts, serviceId: hdnServiceId.val() };
    $.ajax({
        url: "../Service/GetServiceProductList",
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

            $("#txtServiceItem").val("");
            $("#txtNotes").val("");
           
        }
    });
}

function BindProductTypeList() {
    $.ajax({
        type: "GET",
        url: "../Services/GetServiceList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlProductType").append($("<option></option>").val(0).html("-Select Type-"));
            $.each(data, function (i, item) {
                $("#ddlProductType").append($("<option></option>").val(item.ServicesId).html(item.ServicesName));
            });
        },
        error: function (Result) {
            $("#ddlProductType").append($("<option></option>").val(0).html("-Select Type-"));
        }
    });
}