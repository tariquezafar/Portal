$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    BindCompanyBranchList();
    $("#txtProductCode").attr('readOnly', true);
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




    BindCountryList(); 
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnVendorId = $("#hdnVendorId");
    if (hdnVendorId.val() != "" && hdnVendorId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetVendorDetail(hdnVendorId.val());
       }, 1000);

       

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true);
            $("#chkGSTExempt").attr('disabled', true); 
        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
            $("#txtVendorCode").attr('readOnly', true);
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }


    var vendorProducts = [];
    GetVendorProductList(vendorProducts);

    $("#txtVendorName").focus();


    $("#txtEmployeeName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, departmentId: 0, designationId: 0 },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.FirstName, value: item.EmployeeId, EmployeeCode: item.EmployeeCode, MobileNo: item.MobileNo
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtEmployeeName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtEmployeeName").val(ui.item.label);
            $("#hdnEmployeeId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtEmployeeName").val("");
                $("#hdnEmployeeId").val("0");
                ShowModel("Alert", "Please select Employee from List")

            }
            return false;
        }

    })
//.autocomplete("instance")._renderItem = function (ul, item) {
   // return $("<li>")
     // .append("<div><b>" + item.label + " || " + item.EmployeeCode + "</b><br>" + item.MobileNo + "</div>")
     // .appendTo(ul);
//};


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

function GetPAN() {
    var pan = $("#txtGSTNo").val().slice(2).slice(0,-3);
    $("#txtPANNo").val(pan);
}

 
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
function BindCountryList() {
    $.ajax({
        type: "GET",
        url: "../Company/GetCountryList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCountry").append($("<option></option>").val(0).html("-Select Country-"));
            $.each(data, function (i, item) {

                $("#ddlCountry").append($("<option></option>").val(item.CountryId).html(item.CountryName));
            });
        },
        error: function (Result) {
            $("#ddlCountry").append($("<option></option>").val(0).html("-Select Country-"));
        }
    });
}


function BindPrimaryStateList(stateId) {
    var countryId = $("#ddlCountry option:selected").val();
    $("#ddlState").val(0);
    $("#ddlState").html("");
    if (countryId != undefined && countryId != "" && countryId != "0") {
        var data = { countryId: countryId };
        $.ajax({
            type: "GET",
            url: "../Company/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlState").append($("<option></option>").val(item.StateId).html(item.StateName + "(" + item.StateCode+")"));
                });
                $("#ddlState").val(stateId);
            },
            error: function (Result) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
    }
    else {

        $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    }

}

function GetVendorDetail(vendorId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Vendor/GetVendorDetail",
        data: { vendorId: vendorId },
        dataType: "json",
        success: function (data) {
            $("#txtVendorCode").val(data.VendorCode);
            $("#txtVendorName").val(data.VendorName);
            $("#txtContactPersonName").val(data.ContactPersonName); 
            $("#txtEmail").val(data.Email);
            $("#txtMobileNo").val(data.MobileNo);
            $("#txtContactNo").val(data.ContactNo);
            $("#txtFax").val(data.Fax);
            $("#txtAddress").val(data.Address);
            $("#txtCity").val(data.City);
            $("#ddlCountry").val(data.CountryId);
            BindPrimaryStateList(data.StateId);
            $("#ddlState").val(data.StateId);
            $("#txtPinCode").val(data.PinCode);
            $("#txtCSTNo").val(data.CSTNo);
            $("#txtTINNo").val(data.TINNo);
            $("#txtPANNo").val(data.PANNo);
            $("#txtGSTNo").val(data.GSTNo);
            $("#txtExciseNo").val(data.ExciseNo);
            $("#txtCreditLimit").val(data.CreditLimit);
            $("#txtCreditDays").val(data.CreditDays);
            $("#txtAnnualTurnOver").val(data.AnnualTurnover);
            //alert(data.CompanyBranchId);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);

            if (data.IsComposition) {
                $("#chkComposition").attr("checked", true);
            }
            else {
                $("#chkComposition").attr("checked", false);
            }
            if (data.GST_Exempt) {
                $("#chkGSTExempt").attr("checked", true);
            }
            else {
                $("#chkGSTExempt").attr("checked", false);
            }
            if (data.Vendor_Status == true) {
                $("#chkStatus").attr("checked", true);
            }
            else {
                $("#chkStatus").attr("checked", false);
            }

            if (data.IsTCS) {
                $("#chkIsTCS").attr("checked", true);
            }
            else {
                $("#chkIsTCS").attr("checked", false);
            }
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}


function SaveData() {
    var txtVendorName = $("#txtVendorName");
    var hdnVendorId = $("#hdnVendorId");
    var txtVendorCode = $("#txtVendorCode");
    var txtContactPersonName = $("#txtContactPersonName"); 
    var txtEmail = $("#txtEmail");
    var txtMobileNo = $("#txtMobileNo");
    var txtContactNo = $("#txtContactNo"); 
    var txtFax = $("#txtFax");
    var txtAddress = $("#txtAddress");
    var txtCity = $("#txtCity");
    var ddlCountry = $("#ddlCountry");
    var ddlState = $("#ddlState");
    var txtPinCode = $("#txtPinCode");
    var txtCSTNo = $("#txtCSTNo");
    var txtTINNo = $("#txtTINNo");
    var txtPANNo = $("#txtPANNo");
    var txtGSTNo = $("#txtGSTNo");
    var txtExciseNo = $("#txtExciseNo");
    var txtCreditLimit = $("#txtCreditLimit");
    var txtCreditDays = $("#txtCreditDays");
    var txtAnnualTurnOver = $("#txtAnnualTurnOver");
    
    var chkStatus = $("#chkStatus").is(':checked') ? true : false;
    var chkGSTExempt = $("#chkGSTExempt");
    var chkComposition = $("#chkComposition").is(':checked') ? true : false;

    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (txtVendorName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Vendor Name")
        txtVendorName.focus();
        return false;
    }
    //if (txtVendorCode.val().trim() == "") {
    //    ShowModel("Alert", "Please Enter Vendor Code")
    //    txtVendorCode.focus();
    //    return false;
    //}
    if (txtContactPersonName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Contact Person name")
        txtContactPersonName.focus();
        return false;
    }

    if (txtEmail.val().trim()!="" && !ValidEmailCheck(txtEmail.val().trim())) {
        ShowModel("Alert", "Please enter Valid Email Id")
        txtEmail.focus();
        return false;
    }

    if (txtMobileNo.val().trim() == "") {
        ShowModel("Alert", "Please enter Mobile No.")
        txtMobileNo.focus();
        return false;
    }
    if (txtMobileNo.val().trim().length < 10) {
        ShowModel("Alert", "Please enter valid Mobile No.")
        txtMobileNo.focus();
        return false;
    }
     
    if (txtAddress.val().trim() == "") {
        ShowModel("Alert", "Please enter Address")
        txtAddress.focus();
        return false;
    }
    if (txtCity.val().trim() == "") {
        ShowModel("Alert", "Please enter City")
        txtCity.focus();
        return false;
    }
    if (ddlCountry.val() == "" || ddlCountry.val() == "0") {
        ShowModel("Alert", "Please select Country")
        ddlCountry.focus();
        return false;
    }
    if (ddlState.val() == "" || ddlState.val() == "0") {
        ShowModel("Alert", "Please select State")
        ddlState.focus();
        return false;
    }

    

    //if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
    //    ShowModel("Alert", "Please select Comapmny Branch")
    //    ddlCompanyBranch.focus();
    //    return false;
    //}

        var GSTExempt = true;
        if (chkGSTExempt.prop("checked") == true)
        { GSTExempt = true; }
        else
        { GSTExempt = false; }
     
    var vendorViewModel = {
        VendorId: hdnVendorId.val(),
        VendorCode: txtVendorCode.val().trim(),
        VendorName: txtVendorName.val().trim(),
        ContactPersonName: txtContactPersonName.val().trim(), 
        Email: txtEmail.val().trim(),
        MobileNo: txtMobileNo.val().trim(),
        ContactNo: txtContactNo.val(),
        Fax: txtFax.val().trim(),
        Address: txtAddress.val().trim(),
        City: txtCity.val().trim(),
        StateId: ddlState.val(),
        CountryId: ddlCountry.val(),
        PinCode: txtPinCode.val().trim(),
        CSTNo: txtCSTNo.val().trim(),
        TINNo: txtTINNo.val().trim(),
        PANNo: txtPANNo.val().trim(),
        GSTNo: txtGSTNo.val().trim(),
        ExciseNo: txtExciseNo.val().trim(),
        CreditLimit: txtCreditLimit.val().trim(),
        CreditDays: txtCreditDays.val().trim(),
        Vendor_Status: chkStatus,
        AnnualTurnOver: txtAnnualTurnOver.val(),
        GST_Exempt: GSTExempt,
        IsComposition: chkComposition,
        CompanyBranchId: 0,
        StateCode: $('#ddlState option:selected').text().substring($('#ddlState option:selected').text().indexOf('(') + 1, $('#ddlState option:selected').text().indexOf(')')),
        IsTCS: $("#chkIsTCS").is(':checked') ? true : false
    };

    var vendorProductList = [];
    $('#tblVendorProductList tr').each(function (i, row) {
        var $row = $(row);
        var mappingId = $row.find("#hdnMappingId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        if (mappingId != undefined) {
            var vendorProduct = {
                MappingId: mappingId,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc
            };
            vendorProductList.push(vendorProduct);
        }

    });
    var accessMode = 1;//Add Mode
    if (hdnVendorId.val() != null && hdnVendorId.val() != 0) {
        accessMode = 2;//Edit Mode
    }


    var requestData = { vendorViewModel: vendorViewModel, vendorProducts: vendorProductList };
    $.ajax({
        url: "../Vendor/AddEditVendor?AccessMode=" + accessMode + "",
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
                   window.location.href = "../Vendor/ListVendor";
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
function GetVendorProductList(vendorProducts) {
    var hdnVendorId = $("#hdnVendorId");
    var requestData = { vendorProducts: vendorProducts, vendorId: hdnVendorId.val() };
    $.ajax({
        url: "../Vendor/GetVendorProductList",
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
            ClearProductFields();
        }
    });
}
function ClearProductFields() {
    $("#hdnMappingId").val("0");
    $("#txtProductName").val("");
    $("#hdnProductId").val("0");
    $("#txtProductCode").val("");
    $("#txtProductShortDesc").val("");
    $("#btnAddProduct").show();
    $("#btnUpdateProduct").hide();
}
function AddProduct(action) {
    var txtProductName = $("#txtProductName");
    var hdnMappingId = $("#hdnMappingId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");

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

    var vendorProductList = [];
    $('#tblVendorProductList tr').each(function (i, row) {
        var $row = $(row);
        var mappingId = $row.find("#hdnMappingId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();

        if (mappingId != undefined) {
            if (action == 1 || hdnMappingId.val() != mappingId) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already mapped!!!")
                    txtProductName.focus();
                    return false;
                }

                var vendorProduct = {
                    MappingId: mappingId,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc
                };
                vendorProductList.push(vendorProduct);
            }
        }

    });

    var vendorProductAddEdit = {
        MappingId: hdnMappingId.val(),
        ProductId: hdnProductId.val(),
        ProductName: txtProductName.val().trim(),
        ProductCode: txtProductCode.val().trim(),
        ProductShortDesc: txtProductShortDesc.val().trim()
    };
    vendorProductList.push(vendorProductAddEdit);
    GetVendorProductList(vendorProductList);

}
function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var mappingId = $(row).find("#hdnMappingId").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();

    $("#txtProductName").val(productName);
    $("#hdnMappingId").val(mappingId);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);

    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
}

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var mappingId = $(row).find("#hdnMappingId").val();

        $.ajax({
            type: "POST",
            url: "../Customer/RemoveVendorProduct",
            data: { mappingId: mappingId }
        }).success(function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                row.remove();
            }
            else {
                ShowModel("Error", data.message)
            }

        }).error(function (err) {

            ShowModel("Error", err)
        });
    }
}




function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields() { 
    $("#txtVendorName").val("");
    $("#hdnVendorId").val("0");
    $("#txtVendorCode").val("");
    $("#txtContactPersonName").val("");
    $("#txtDesignation").val("");
    $("#txtEmail").val("");
    $("#txtMobileNo").val("");
    $("#txtContactNo").val("");
    $("#txtFax").val("");
    $("#txtAddress").val("");
    $("#txtCity").val("");
    $("#ddlCountry").val("0");
    $("#ddlState").val("0");
    $("#txtPinCode").val("");
    $("#txtCSTNo").val("");
    $("#txtTINNo").val("");
    $("#txtPANNo").val("");
    $("#txtGSTNo").val("");
    $("#txtExciseNo").val("");
    $("#txtCreditLimit").val("");
    $("#txtCreditDays").val("");
    $("#txtAnnualTurnOver").val("");
    $("#chkStatus").prop("checked", true);
    $("#chkGSTExempt").prop("checked", true);
    $("#btnSave").show();
    $("#btnUpdate").hide();
    $("#divProductList").html("");
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
