$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtUINNo").attr("disabled", true);
  // $("#txtDueDate").attr('readOnly', true);
    //  $("#txtReminderDate").attr('readOnly', true);
    $(".showSaleEmpName").hide();
    BindCountryList();
    BindCustomerTypeList();
    BindCompanyBranchList();
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    $("#ddlBState").append($("<option></option>").val(0).html("-Select State-"));


    $("#txtAssignFollowUp").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Lead/GetUserAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FullName, value: item.UserName, UserId: item.UserId, MobileNo: item.MobileNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtAssignFollowUp").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtAssignFollowUp").val(ui.item.label);
            $("#hdnUserId").val(ui.item.UserId);
            $("#hdnFollowUpByUserName").val(ui.item.label);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtAssignlead").val("");
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


    //Bind Sales Employee 


    $("#txtSalesEmployeeName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            

            $.ajax({
                url: "../SaleInvoice/GetEmployeeDepartmentWiseAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, companybrachId:0 },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FirstName, value: item.EmployeeId, primaryAddress: item.PAddress, code: item.EmployeeCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtSalesEmployeeName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtSalesEmployeeName").val(ui.item.label);
            $("#hdnSaleEmployeeId").val(ui.item.value);


            return false;
        },
        //change: function (event, ui) {
        //    if (ui.item == null) {
        //        $("#txtSalesEmployeeName").val("");
        //        $("#hdnSaleEmployeeId").val("0");

        //        ShowModel("Alert", "Please select Sales Employee Name from List");

        //    }
        //    return false;
        //}

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.primaryAddress + "</div>")
      .appendTo(ul);
};

    //End Sale Employee Name

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

  

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnCustomerId = $("#hdnCustomerId");
    if (hdnCustomerId.val() != "" && hdnCustomerId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetCustomerDetail(hdnCustomerId.val());
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
            $("#txtCustomerCode").attr('readOnly', true);
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }

    var customerBranchs = [];
    GetCustomerBranchList(customerBranchs);

    var customerProducts = [];
    GetCustomerProductList(customerProducts);

    var followUps = [];
    GetFollowUpList(followUps);
    

    $("#txtCustomerName").focus(); 

    $("#txtEmployeeName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, departmentId: 0, designationId: 0
                },
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
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.EmployeeCode + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};

    BindFollowUpActivityTypeList();
    BindLeadStatusList();
});
$("#txtReminderDate").datetimepicker({
    format: 'd-M-Y h:i a',
    formatTime: 'h:i a',
});
$("#txtDueDate").datetimepicker({
    format: 'd-M-Y h:i a',
    formatTime: 'h:i a',
    minDate: 0,
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
function checkPhone(el) {
    var ex = /^[0-9]+\-?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
$("#chkUIN").on
function BindCustomerTypeList() {
    $.ajax({
        type: "GET",
        url: "../Customer/GetCustomerTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCustomerType").append($("<option></option>").val(0).html("-Select Customer Type-"));
            $.each(data, function (i, item) {

                $("#ddlCustomerType").append($("<option></option>").val(item.CustomerTypeId).html(item.CustomerTypeDesc));
            });
        },
        error: function (Result) {
            $("#ddlCustomerType").append($("<option></option>").val(0).html("-Select Customer Type-"));
        }
    });
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
            $("#ddlCountry,#ddlBCountry").append($("<option></option>").val(0).html("-Select Country-"));
            $.each(data, function (i, item) {

                $("#ddlCountry,#ddlBCountry").append($("<option></option>").val(item.CountryId).html(item.CountryName));
            });
        },
        error: function (Result) {
            $("#ddlCountry,#ddlBCountry").append($("<option></option>").val(0).html("-Select Country-"));
        }
    });
}
function RemoveRow(obj) {
    if (confirm("Do you want to remove selected Branch?")) {
        var row = $(obj).closest("tr");
        var customerBranchId = $(row).find("#hdnCustomerBranchId").val();

        $.ajax({
            type: "POST",
            url: "../Customer/RemoveCustomerBranch",
            data: { customerBranchId: customerBranchId }
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
function EditRow(obj) { 
    var row = $(obj).closest("tr");
    var customerBranchId = $(row).find("#hdnCustomerBranchId").val();
    var branchSequenceNo = $(row).find("#hdnSequenceNo").val();
    var branchName = $(row).find("#hdnBranchName").val();
    var bAddress = $(row).find("#hdnBAddress").val();
    var bCity = $(row).find("#hdnBCity").val();
    var bStateId = $(row).find("#hdnBStateId").val();
    var bStateName = $(row).find("#hdnBStateName").val();
    var bCountryId = $(row).find("#hdnBCountryId").val();
    var bPinCode = $(row).find("#hdnBPinCode").val();
    var bCSTNo = $(row).find("#hdnBCSTNo").val();
    var bTINNo = $(row).find("#hdnBTINNo").val();
    var bPANNo = $(row).find("#hdnBPANNo").val();
    var bGSTNo = $(row).find("#hdnBGSTNo").val();
    var bContactPersonName = $(row).find("#hdnBContactPersonName").val();
    var bDesignation = $(row).find("#hdnBDesignation").val();
    var bEmail = $(row).find("#hdnBEmail").val();
    var bMobileNo = $(row).find("#hdnBMobileNo").val();
    var bContactNo = $(row).find("#hdnBContactNo").val();
    var bFax = $(row).find("#hdnBFax").val();
    var annualTurnOverBranch = $(row).find("#hdnAnnualTurnover").val();

    $("#hdnCustomerBranchId").val(customerBranchId);
    $("#txtBranchName").val(branchName);
    $("#txtBContactPersonName").val(bContactPersonName);
    $("#txtBDesignation").val(bDesignation);
    $("#hdnBranchSequenceNo").val(branchSequenceNo);
    $("#txtBEmail").val(bEmail);
    $("#txtBMobileNo").val(bMobileNo);
    $("#txtBContactNo").val(bContactNo);
    $("#txtBFax").val(bFax);
    $("#txtBAddress").val(bAddress);
    $("#txtBCity").val(bCity);
    $("#ddlBCountry").val(bCountryId);
    BindBranchStateList(bStateId);
    $("#ddlBState").val(bStateId);
    $("#txtBPinCode").val(bPinCode);
    $("#txtBCSTNo").val(bCSTNo);
    $("#txtBTINNo").val(bTINNo);
    $("#txtBPANNo").val(bPANNo);
    $("#txtBGSTNo").val(bGSTNo);
    $("#txtAnnualTurnOverBranch").val(annualTurnOverBranch);
    $("#btnAddBranch").hide();
    $("#btnUpdateBranch").show();

}
function AddBranch(action) {
    var taxEntrySequence = 0;
    var flag = true;
    var hdnBranchSequenceNo = $("#hdnBranchSequenceNo");
    var txtBranchName = $("#txtBranchName");
    var hdnCustomerBranchId = $("#hdnCustomerBranchId"); 
    var txtBContactPersonName = $("#txtBContactPersonName");
    var txtBDesignation = $("#txtBDesignation");
    var txtBEmail = $("#txtBEmail");
    var txtBMobileNo = $("#txtBMobileNo");
    var txtBContactNo = $("#txtBContactNo");

    var txtBFax = $("#txtBFax");
    var txtBAddress = $("#txtBAddress");
    var txtBCity = $("#txtBCity");
    var ddlBCountry = $("#ddlBCountry");
    var ddlBState = $("#ddlBState");
    var txtBPinCode = $("#txtBPinCode");
    var txtBCSTNo = $("#txtBCSTNo");
    var txtBTINNo = $("#txtBTINNo");
    var txtBPANNo = $("#txtBPANNo");
    var txtBGSTNo = $("#txtBGSTNo");
    var txtAnnualTurnOverBranch = $("#txtAnnualTurnOverBranch");

    if (txtBranchName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Branch Name")
        txtBranchName.focus();
        return false;
    }
    if (txtBEmail.val().trim() != "" && !ValidEmailCheck(txtBEmail.val().trim())) {
        ShowModel("Alert", "Please enter Valid Email Id")
        txtBEmail.focus();
        return false;
    }

    if (txtBMobileNo.val().trim() != "" && txtBMobileNo.val().trim().length < 10) {
        ShowModel("Alert", "Please enter valid Mobile No.")
        txtBMobileNo.focus();
        return false;
    } 

    if (txtBAddress.val().trim() == "") {
        ShowModel("Alert", "Please enter Branch Address")
        txtBAddress.focus();
        return false;
    }
    if (txtBCity.val().trim() == "") {
        ShowModel("Alert", "Please enter Branch City")
        txtBCity.focus();
        return false;
    }
    if (ddlBCountry.val() == "" || ddlBCountry.val() == "0") {
        ShowModel("Alert", "Please select Branch Country")
        ddlBCountry.focus();
        return false;
    }
    if (ddlBState.val() == "" || ddlBState.val() == "0") {
        ShowModel("Alert", "Please select Branch State")
        ddlBState.focus();
        return false;
    }


    var customerBranchList = [];
    if (action == 1 && (hdnBranchSequenceNo.val() == "" || hdnBranchSequenceNo.val() == "0")) {
        taxEntrySequence = 1;
    }
    $('#tblCustomerBranchList tr').each(function (i, row) { 
        var $row = $(row);
        var branchSequenceNo = $row.find("#hdnSequenceNo").val();
        var customerBranchId = $row.find("#hdnCustomerBranchId").val();
        var branchName = $row.find("#hdnBranchName").val();
        var bAddress = $row.find("#hdnBAddress").val();
        var bCity = $row.find("#hdnBCity").val();
        var bStateId = $row.find("#hdnBStateId").val();
        var bStateName = $row.find("#hdnBStateName").val();
        var bCountryId = $row.find("#hdnBCountryId").val();
        var bPinCode = $row.find("#hdnBPinCode").val();
        var bCSTNo = $row.find("#hdnBCSTNo").val();
        var bTINNo = $row.find("#hdnBTINNo").val();
        var bPANNo = $row.find("#hdnBPANNo").val();
        var bGSTNo = $row.find("#hdnBGSTNo").val();
        var bContactPersonName = $row.find("#hdnBContactPersonName").val();
        var bDesignation = $row.find("#hdnBDesignation").val();
        var bEmail = $row.find("#hdnBEmail").val();
        var bMobileNo = $row.find("#hdnBMobileNo").val();
        var bContactNo = $row.find("#hdnBContactNo").val();
        var bFax = $row.find("#hdnBFax").val();
        var annualTurnOverBranch = $row.find("#hdnAnnualTurnover").val();

        if (customerBranchId != undefined) {
            if (action == 1 || (hdnBranchSequenceNo.val() != branchSequenceNo)) {

                if (branchName == txtBranchName.val().trim() && bAddress == txtBAddress.val().trim()) {
                    ShowModel("Alert", "Branch Name with same Address already exists!!!")
                    ddlBCountry.focus();
                    return false;
                }

                var customerBranch = {
                    CustomerBranchId: customerBranchId,
                    SequenceNo: branchSequenceNo,
                    BranchName: branchName,
                    ContactPersonName: bContactPersonName,
                    Designation: bDesignation,
                    Email: bEmail,
                    MobileNo: bMobileNo,
                    ContactNo: bContactNo,
                    Fax: bFax,
                    PrimaryAddress: bAddress,
                    City: bCity,
                    StateId: bStateId,
                    StateName: bStateName,
                    CountryId: bCountryId,
                    PinCode: bPinCode,
                    CSTNo: bCSTNo,
                    TINNo: bTINNo,
                    PANNo: bPANNo,
                    GSTNo: bGSTNo,
                    AnnualTurnOver: annualTurnOverBranch

                };
                customerBranchList.push(customerBranch);
                taxEntrySequence = parseInt(taxEntrySequence) + 1;
            }
            else if (hdnCustomerBranchId.val() == customerBranchId && hdnBranchSequenceNo.val() == branchSequenceNo) {
                var customerBranchAdd = {
                    CustomerBranchId: hdnCustomerBranchId.val(),
                    BranchName: txtBranchName.val().trim(),
                    SequenceNo: hdnBranchSequenceNo.val(),
                    ContactPersonName: txtBContactPersonName.val().trim(),
                    Designation: txtBDesignation.val().trim(),
                    Email: txtBEmail.val().trim(),
                    MobileNo: txtBMobileNo.val().trim(),
                    ContactNo: txtBContactNo.val().trim(),
                    Fax: txtBFax.val().trim(),
                    PrimaryAddress: txtBAddress.val().trim(),
                    City: txtBCity.val().trim(),
                    StateId: ddlBState.val(),
                    StateName: $("#ddlBState option:selected").text(),
                    CountryId: ddlBCountry.val(),
                    PinCode: txtBPinCode.val().trim(),
                    CSTNo: txtBCSTNo.val().trim(),
                    TINNo: txtBTINNo.val().trim(),
                    PANNo: txtBPANNo.val().trim(),
                    GSTNo: txtBGSTNo.val(),
                    AnnualTurnOver: txtAnnualTurnOverBranch.val()
                };
                customerBranchList.push(customerBranchAdd);
                hdnBranchSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnBranchSequenceNo.val() == "" || hdnBranchSequenceNo.val() == "0")) {
        hdnBranchSequenceNo.val(taxEntrySequence);
    }
    if (action == 1)
    {
    var customerBranchAddEdit = {
        CustomerBranchId: hdnCustomerBranchId.val(),
        BranchName: txtBranchName.val().trim(),
        SequenceNo: hdnBranchSequenceNo.val(),
        ContactPersonName: txtBContactPersonName.val().trim(),
        Designation: txtBDesignation.val().trim(),
        Email: txtBEmail.val().trim(),
        MobileNo: txtBMobileNo.val().trim(),
        ContactNo: txtBContactNo.val().trim(),
        Fax: txtBFax.val().trim(),
        PrimaryAddress: txtBAddress.val().trim(),
        City: txtBCity.val().trim(),
        StateId: ddlBState.val(),
        StateName: $("#ddlBState option:selected").text(),
        CountryId: ddlBCountry.val(),
        PinCode: txtBPinCode.val().trim(),
        CSTNo: txtBCSTNo.val().trim(),
        TINNo: txtBTINNo.val().trim(),
        PANNo: txtBPANNo.val().trim(),
        GSTNo: 0,
        AnnualTurnOver: txtAnnualTurnOverBranch.val()

    };
    customerBranchList.push(customerBranchAddEdit);
    hdnBranchSequenceNo.val("0");
    }
    if (flag == true) {
        GetCustomerBranchList(customerBranchList);
    }
}
function ClearBranchFields() {
    $("#hdnCustomerBranchId").val("0");
    $("#txtBranchName").val("");
    $("#txtBContactPersonName").val("");
    $("#txtBDesignation").val("");
    $("#txtBEmail").val("");
    $("#txtBMobileNo").val("");
    $("#txtBContactNo").val("");
    $("#txtBFax").val("");
    $("#txtBAddress").val("");
    $("#txtBCity").val("");
    $("#ddlBCountry").val("0");
    $("#ddlBState").val("0");
    $("#txtBPinCode").val("");
    $("#txtBCSTNo").val("");
    $("#txtBTINNo").val("");
    $("#txtBPANNo").val("");
    $("#txtAnnualTurnOverBranch").val("");
    $("#txtBGSTNo").val("");
    $("#btnAddBranch").show();
    $("#btnUpdateBranch").hide();
}
function GetCustomerBranchList(customerBranchs) {
    var hdnCustomerId = $("#hdnCustomerId");
    var requestData = { customerBranchs: customerBranchs, customerId: hdnCustomerId.val() };
    $.ajax({
        url: "../Customer/GetCustomerBranchList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divBranchList").html("");
            $("#divBranchList").html(err);
        },
        success: function (data) {
            $("#divBranchList").html("");
            $("#divBranchList").html(data);
            ClearBranchFields();
        }
    });
}


function GetCustomerProductList(customerProducts) {
    var hdnCustomerId = $("#hdnCustomerId");
    var requestData = { customerProducts: customerProducts, customerId: hdnCustomerId.val() };
    $.ajax({
        url: "../Customer/GetCustomerProductList",
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
    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");
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
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }
    var customerProductList = [];
    $('#tblCustomerProductList tr').each(function (i, row) {
        var $row = $(row);
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var mappingId = $row.find("#hdnMappingId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();

        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already mapped!!!")
                    txtProductName.focus();
                    return false;
                }

                var customerProduct = {
                    MappingId: mappingId,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc
                };
                customerProductList.push(customerProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnMappingId.val() == mappingId && hdnSequenceNo.val() == sequenceNo) {
                var customerProduct = {
                    MappingId: hdnMappingId.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),

                };
                customerProductList.push(customerProduct);
                hdnSequenceNo.val("0");

            }
        }
    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var customerProductAddEdit = {
            MappingId: hdnMappingId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim()
        };
  
    customerProductList.push(customerProductAddEdit);
    hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetCustomerProductList(customerProductList);
    }
}
function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var mappingId = $(row).find("#hdnMappingId").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();

    $("#txtProductName").val(productName);
    $("#hdnSequenceNo").val(sequenceNo);
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
            url: "../Customer/RemoveCustomerProduct",
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
                    $("#ddlState").append($("<option></option>").val(item.StateId).html(item.StateName));
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
function BindBranchStateList(stateId) {
    var countryId = $("#ddlBCountry option:selected").val();
    $("#ddlBState").val(0);
    $("#ddlBState").html("");
    if (countryId != undefined && countryId != "" && countryId != "0") {
        var data = { countryId: countryId };
        $.ajax({
            type: "GET",
            url: "../Company/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlBState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlBState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                $("#ddlBState").val(stateId);
            },
            error: function (Result) {
                $("#ddlBState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
    }
    else {

        $("#ddlBState").append($("<option></option>").val(0).html("-Select State-"));
    }

}

function GetCustomerDetail(customerId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Customer/GetCustomerDetail",
        data: { customerId: customerId },
        dataType: "json",
        success: function (data) {
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtCustomerName").val(data.CustomerName);
            $("#txtContactPersonName").val(data.ContactPersonName);
            $("#txtDesignation").val(data.Designation);
            $("#txtEmail").val(data.Email);
            $("#txtMobileNo").val(data.MobileNo);
            $("#txtContactNo").val(data.ContactNo);
            $("#txtFax").val(data.Fax);
            $("#txtPrimaryAddress").val(data.PrimaryAddress);
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
           // BindCustomerTypeList();
            $("#ddlCustomerType").val(data.CustomerTypeId);
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#txtEmployeeName").val(data.EmployeeName);
            $("#txtAnnualTurnOver").val(data.AnnualTurnover);
            $("#txtCreditLimit").val(data.CreditLimit);
            $("#txtCreditDays").val(data.CreditDays);

            var CustomerType = $("#ddlCustomerType option:selected").text();
            if (CustomerType == 'Dealer' || CustomerType == 'Distributor') {
                $(".showSaleEmpName").show();
                $("#hdnSaleEmployeeId").val(data.SaleEmpId);
                $("#txtSalesEmployeeName").val(data.SaleEmployeeName);

            }
            else {
                $(".showSaleEmpName").hide();
            }

            if (data.GST_Exempt) {
                $("#chkGSTExempt").attr("checked", true);
            }
            else {
                $("#chkGSTExempt").attr("checked", false);
            }
            if (data.Customer_Status == true) {
                $("#chkStatus").attr("checked", true);
                
            }
            else {
                $("#chkStatus").attr("checked", false);
            }
            if (data.IsComposition) {
                $("#chkComposition").attr("checked", true);
            }
            else {
                $("#chkComposition").attr("checked", false);
            }
            if (data.IsUIN) {
                $("#chkUIN").attr("checked", true);
                $("#txtUINNo").attr("disabled", false);
                $("#txtUINNo").val(data.UINNo);
            }
            else {
                $("#chkUIN").attr("checked", false);
                $("#txtUINNo").attr("disabled", true);
                $("#txtUINNo").val("");
            }
            BindCompanyBranchList();
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}


function SaveData() {
   
    var txtCustomerName = $("#txtCustomerName");
    var hdnCustomerId = $("#hdnCustomerId");
    var txtCustomerCode = $("#txtCustomerCode");
    var txtContactPersonName = $("#txtContactPersonName");
    var txtDesignation = $("#txtDesignation");
    var txtEmail = $("#txtEmail");
    var txtMobileNo = $("#txtMobileNo");
    var txtContactNo = $("#txtContactNo");

    var txtFax = $("#txtFax");
    var txtPrimaryAddress = $("#txtPrimaryAddress");
    var txtCity = $("#txtCity");
    var ddlCountry = $("#ddlCountry");
    var ddlState = $("#ddlState");
    var txtPinCode = $("#txtPinCode");
    var txtCSTNo = $("#txtCSTNo");
    var txtTINNo = $("#txtTINNo");
    var txtPANNo = $("#txtPANNo");
    var txtGSTNo = $("#txtGSTNo");
    var txtExciseNo = $("#txtExciseNo");
    var ddlCustomerType = $("#ddlCustomerType");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var txtCreditLimit = $("#txtCreditLimit");
    var txtCreditDays = $("#txtCreditDays");
    var txtAnnualTurnOver = $("#txtAnnualTurnOver");

    var chkStatus = $("#chkStatus").is(':checked') ? true : false;
    var chkGSTExempt = $("#chkGSTExempt");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var hdnSaleEmployeeId = $("#hdnSaleEmployeeId");

    if (txtCustomerName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Customer Name")
        txtCustomerName.focus();
        return false;
    }
    if (txtCustomerCode.val().trim() == "") {
        ShowModel("Alert", "Please Enter Customer Code")
        txtCustomerCode.focus();
        return false;
    }
    if (txtContactPersonName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Contact Person name")
        txtContactPersonName.focus();
        return false;
    } 

    if (txtEmail.val().trim() != "" && !ValidEmailCheck(txtEmail.val().trim())) {
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
    if (txtPrimaryAddress.val().trim() == "") {
        ShowModel("Alert", "Please enter Primary Address")
        txtPrimaryAddress.focus();
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
    if (ddlCustomerType.val() == "" || ddlCustomerType.val() == "0") {
        ShowModel("Alert", "Please select Customer Type")
        ddlCustomerType.focus();
        return false;
    }

    
   
    var GSTExempt = true;
    if (chkGSTExempt.prop("checked") == true)
    { GSTExempt = true; }
    else
    { GSTExempt = false; }
    var chkComposition = $("#chkComposition");
    var Composition = true;
    if (chkComposition.prop("checked") == true)
    { Composition = true; }
    else
    { Composition = false; }

    var chkUIN = $("#chkUIN");
    var UIN = true;
    if (chkUIN.prop("checked") == true)
    { UIN = true; }
    else
    { UIN = false; }
   
    txtUINNo = $("#txtUINNo");

   
   

    var customerViewModel = {
        CustomerId: hdnCustomerId.val(),
        CustomerCode: txtCustomerCode.val().trim(),
        CustomerName: txtCustomerName.val().trim(),
        ContactPersonName: txtContactPersonName.val().trim(),
        Designation: txtDesignation.val().trim(),
        Email: txtEmail.val().trim(),
        MobileNo: txtMobileNo.val().trim(),
        ContactNo: txtContactNo.val(),
        Fax: txtFax.val().trim(),
        PrimaryAddress: txtPrimaryAddress.val().trim(),
        City: txtCity.val().trim(),
        StateId: ddlState.val(),
        CountryId: ddlCountry.val(),
        PinCode: txtPinCode.val().trim(),
        CSTNo: txtCSTNo.val().trim(),
        TINNo: txtTINNo.val().trim(),
        PANNo: txtPANNo.val().trim(),
        GSTNo: txtGSTNo.val().trim(),
        ExciseNo: txtExciseNo.val().trim(),
        EmployeeId: hdnEmployeeId.val(),
        CustomerTypeId: ddlCustomerType.val(),
        CreditLimit: txtCreditLimit.val().trim(),
        CreditDays: txtCreditDays.val().trim(),
        Customer_Status: chkStatus,
        AnnualTurnOver:txtAnnualTurnOver.val(),
        GST_Exempt: GSTExempt,
        IsComposition:Composition,
        IsUIN:UIN,
        UINNo: txtUINNo.val(),
        CompanyBranchId: 0,
        SaleEmpId: hdnSaleEmployeeId.val()

    };

    var customerBranchList = [];
    $('#tblCustomerBranchList tr').each(function (i, row) {
        var $row = $(row);
        var customerBranchId = $row.find("#hdnCustomerBranchId").val();
        var branchName = $row.find("#hdnBranchName").val();
        var bAddress = $row.find("#hdnBAddress").val();
        var bCity = $row.find("#hdnBCity").val();
        var bStateId = $row.find("#hdnBStateId").val();
        var bStateName = $row.find("#hdnBStateName").val();
        var bCountryId = $row.find("#hdnBCountryId").val();
        var bPinCode = $row.find("#hdnBPinCode").val();
        var bCSTNo = $row.find("#hdnBCSTNo").val();
        var bTINNo = $row.find("#hdnBTINNo").val();
        var bPANNo = $row.find("#hdnBPANNo").val();
        var bGSTNo = $row.find("#hdnBGSTNo").val();
        var bContactPersonName = $row.find("#hdnBContactPersonName").val();
        var bDesignation = $row.find("#hdnBDesignation").val();
        var bEmail = $row.find("#hdnBEmail").val();
        var bMobileNo = $row.find("#hdnBMobileNo").val();
        var bContactNo = $row.find("#hdnBContactNo").val();
        var bFax = $row.find("#hdnBFax").val();
        var annualTurnOverBranch = $row.find("#hdnAnnualTurnover").val();

        if (customerBranchId != undefined) {

            var customerBranch = {
                CustomerBranchId: customerBranchId,
                BranchName: branchName,
                ContactPersonName: bContactPersonName,
                Designation: bDesignation,
                Email: bEmail,
                MobileNo: bMobileNo,
                ContactNo: bContactNo,
                Fax: bFax,
                PrimaryAddress: bAddress,
                City: bCity,
                StateId: bStateId,
                StateName: bStateName,
                CountryId: bCountryId,
                PinCode: bPinCode,
                CSTNo: bCSTNo,
                TINNo: bTINNo,
                PANNo: bPANNo,
                GSTNo: bGSTNo,
                AnnualTurnOver:annualTurnOverBranch

            };
            customerBranchList.push(customerBranch);
        }

    });

    var customerProductList = [];
    $('#tblCustomerProductList tr').each(function (i, row) {
        var $row = $(row);
        var mappingId = $row.find("#hdnMappingId").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        if (mappingId != undefined) {
            var customerProduct = {
                MappingId: mappingId,
                ProductId: productId,
                ProductName: productName,
                ProductCode: productCode,
                ProductShortDesc: productShortDesc
            };
            customerProductList.push(customerProduct);
        }

    });

    var customerFollowUpList = [];
    $('#tblLeadList tr').each(function (i, row) {
        var $row = $(row);
        var CustomerFollowUpId = $row.find("#hdnCustomerFollowUpId").val();
        var FollowUpActivityTypeId = $row.find("#hdnFollowUpActivityTypeId").val();
        var FollowUpActivityTypeName = $row.find("#hdnFollowUpActivityTypeName").val();
        var FollowUpDueDateTime = $row.find("#hdnFollowUpDueDateTime").val();
        var FollowUpReminderDateTime = $row.find("#hdnFollowUpReminderDateTime").val();
        var FollowUpRemarks = $row.find("#hdnFollowUpRemarks").val();
        var Priority = $row.find("#hdnPriority").val();
        var PriorityName = $row.find("#hdnPriorityName").val();
        var FollowUpStatusId = $row.find("#hdnFollowUpStatusId").val();
        var FollowUpStatusName = $row.find("#hdnFollowUpStatusName").val();
        var FollowUpStatusReason = $row.find("#hdnFollowUpStatusReason").val();
        var FollowUpByUserId = $row.find("#hdnFollowUpByUserId").val();
        var hdnFollowUpCreatedBy = $row.find("#hdnFollowUpCreatedBy").val();
        var followUpByUserName = $row.find("#hdnFollowUpByUserName").val();
        var hdnLeadCreatedBy = $row.find("#hdnLeadCreatedBy").val();

        if (CustomerFollowUpId != undefined) {
                    var customerFollowup = {
                    CustomerFollowUpId: CustomerFollowUpId,
                    FollowUpActivityTypeId: FollowUpActivityTypeId,
                    FollowUpActivityTypeName: FollowUpActivityTypeName,
                    FollowUpDueDateTime: FollowUpDueDateTime,
                    FollowUpReminderDateTime: FollowUpReminderDateTime,
                    FollowUpRemarks: FollowUpRemarks,
                    Priority: Priority,
                    PriorityName: PriorityName,
                    FollowUpStatusId: FollowUpStatusId,
                    FollowUpStatusName: FollowUpStatusName,
                    FollowUpStatusReason: FollowUpStatusReason,
                    FollowUpByUserId: FollowUpByUserId,
                    FollowUpByUserName: followUpByUserName
                };
                customerFollowUpList.push(customerFollowup); 
        }

    });

    var accessMode = 1;//Add Mode
    if (hdnCustomerId.val() != null && hdnCustomerId.val() != 0) {
        accessMode = 2;//Edit Mode
    }


    if (txtGSTNo.val() != '' && txtGSTNo.val().length != 15) {
        var confirmMessageGSTNo = confirm("WARNING : The GSTN No. entered is less than 15 character. Are you sure you want to save it?");
        if (confirmMessageGSTNo == false) {
            return false;
        }

        else
        {
            var requestData = { customerViewModel: customerViewModel, customerBranchs: customerBranchList, customerProducts: customerProductList, customerFollowUps: customerFollowUpList };
            $.ajax({
                url: "../Customer/AddEditCustomer?AccessMode=" + accessMode + "",
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
                             window.location.href = "../Customer/ListCustomer";
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
    }

    else {
        var requestData = { customerViewModel: customerViewModel, customerBranchs: customerBranchList, customerProducts: customerProductList, customerFollowUps: customerFollowUpList };
        $.ajax({
            url: "../Customer/AddEditCustomer?AccessMode=" + accessMode + "",
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
                         window.location.href = "../Customer/ListCustomer";
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


   

}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields() {

    $("#txtCustomerName").val("");
    $("#hdnCustomerId").val("0");
    $("#txtCustomerCode").val("");
    $("#txtContactPersonName").val("");
    $("#txtDesignation").val("");
    $("#txtEmail").val("");
    $("#txtMobileNo").val("");
    $("#txtContactNo").val("");
    $("#txtFax").val("");
    $("#txtPrimaryAddress").val("");
    $("#txtCity").val("");
    $("#ddlCountry").val("0");
    $("#ddlState").val("0");
    $("#txtPinCode").val("");
    $("#txtCSTNo").val("");
    $("#txtTINNo").val("");
    $("#txtPANNo").val("");
    $("#txtGSTNo").val("");
    $("#txtExciseNo").val(""); 
    $("#ddlCustomerType").val("0");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployeeName").val("");
    $("#txtCreditLimit").val("0");
    $("#txtCreditDays").val("0");
    $("#chkStatus").prop("checked", true);
    $("#btnSave").show();
    $("#btnUpdate").hide();
    $("#divBranchList").html("");
    $("#divProductList").html("");
    $("#divFollowUpList").html("");
    $("#txtAnnualTurnOver").val(""); 
    $("#chkGSTExempt").prop("checked", true);
}


function CompareDate(date1, date2) {
    var fdate = date1.split(/[+=;:-]/);
    var d1 = fdate[0];
    var m1 = MonthValue(fdate[1]);
    var y1 = fdate[2].split(' ')[0];
    var h1 = fdate[2].split(' ')[1];
    var i1 = fdate[3].split(' ')[0];
    var a1 = fdate[3].split(' ')[1];
    var dt1 = new Date(parseInt(y1), parseInt(m1) - 1, parseInt(d1), parseInt(h1), parseInt(i1));

    var sdate = date2.split(/[+=;:-]/);
    var d2 = sdate[0];
    var m2 = MonthValue(sdate[1]);
    var y2 = sdate[2].split(' ')[0];
    var h2 = sdate[2].split(' ')[1];
    var i2 = sdate[3].split(' ')[0];
    var a2 = sdate[3].split(' ')[1];
    var dt2 = new Date(parseInt(y2), parseInt(m2) - 1, parseInt(d2), parseInt(h2), parseInt(i2));
    var firstDate = new Date(dt1);
    var secondDate = new Date(dt2);
    if (firstDate > secondDate) {
        ShowModel("Alert", "Due Date should be greater Reminder Date ")
        return false;
    }
    if (firstDate == secondDate && a1 == a2) {
        ShowModel("Alert", "Due Date should not equal Reminder Date ")
        return false;
    }

}
function ValueToMonth(value) {
    switch (value) {
        case 1: return "Jan";
        case 2: return "Feb";
        case 3: return "Mar";
        case 4: return "Apr";
        case 5: return "May";
        case 6: return "Jun";
        case 7: return "Jul";
        case 8: return "Aug";
        case 9: return "Sep";
        case 10: return "Oct";
        case 11: return "Nov";
        case 12: return "Dec";
        default: return "Jan";
    }
}
function MonthValue(str) {
    switch (str) {
        case "Jan": return 1;
        case "Feb": return 2;
        case "Mar": return 3;
        case "Apr": return 4;
        case "May": return 5;
        case "Jun": return 6;
        case "Jul": return 7;
        case "Aug": return 8;
        case "Sep": return 9;
        case "Oct": return 10;
        case "Nov": return 11;
        case "Dec": return 12;
        default: return 1;

    }

}
function padLeft(str) {
    if (parseInt(str) < 10) {
        return '0' + str;
    }
    else { return str; }
}

function GetFollowUpList(customerFollowUps) {
    var hdnCustomerId = $("#hdnCustomerId");
    var requestData = { customerFollowUps: customerFollowUps, custId: hdnCustomerId.val() };
    $.ajax({
        url: "../Customer/GetCustomerFollowUpList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divFollowUpList").html("");
            $("#divFollowUpList").html(err);
        },
        success: function (data) {
            $("#divFollowUpList").html("");
            $("#divFollowUpList").html(data);
            ClearFollowUpFields();
        }
    });
}


function AddFollowUp(action) {
    var taxEntrySequence = 0;
    var flag = true;
    var hdnFollowUpSequenceNo = $("#hdnFollowUpSequenceNo");

    var hdnFollowUpId = $("#hdnFollowUpId");
    var ddlActivityType = $("#ddlActivityType");
    var txtDueDate = $("#txtDueDate");
    var txtReminderDate = $("#txtReminderDate");
    var txtRemarks = $("#txtRemarks");
    var ddlPriority = $("#ddlPriority");
    var ddlFollowUpStatus = $("#ddlFollowUpStatus");
    var txtFollowUpStatusReason = $("#txtFollowUpStatusReason");
    var txtAssignlead = $("#txtAssignlead");
    var hdnUserId = $("#hdnUserId");
    var hdnFollowUpByUserName = $("#hdnFollowUpByUserName");
    var hdnCreatedDate = $("#hdnCreatedDate");
    var hdnModifyDate = $("#hdnModifyDate");
    var hdnLoginUserId = $("#hdnLoginUserId");

    if (ddlActivityType.val().trim() == "" || ddlActivityType.val() == "0") {
        ShowModel("Alert", "Please Select Activity Type")
        ddlActivityType.focus();
        return false;
    }
    if (txtDueDate.val().trim() == "") {
        ShowModel("Alert", "Please select due date")
        txtDueDate.focus();
        return false;
    }

    if (txtReminderDate.val().trim() == "") {
        ShowModel("Alert", "Please select reminder date")
        txtReminderDate.focus();
        return false;
    }

    //if (CompareDate(txtReminderDate.val(), txtdueDate.val()) == false) {
    //    txtReminderDate.focus();
    //    return false;
    //}
    if (ddlPriority.val() == "" || ddlPriority.val() == "0") {
        ShowModel("Alert", "Please select Priority")
        ddlPriority.focus();
        return false;
    }
    if (ddlFollowUpStatus.val() == "" || ddlFollowUpStatus.val() == "0") {
        ShowModel("Alert", "Please select Lead Status")
        ddlFollowUpStatus.focus();
        return false;
    }
    if (txtRemarks.val() == "") {
        ShowModel("Alert", "Please Enter Remarks")
        txtRemarks.focus();
        return false;
    }
    if (txtAssignlead.val() == "" || txtAssignlead.val() == "0") {
        ShowModel("Alert", "Please Select Assign Lead")
        txtAssignlead.focus();
        return false;
    }
    if (txtFollowUpStatusReason.val() == "") {
        ShowModel("Alert", "Please Enter Follow Up Status Reason")
        txtFollowUpStatusReason.focus();
        return false;
    }



    var followupList = [];
    if (action == 1 && (hdnFollowUpSequenceNo.val() == "" || hdnFollowUpSequenceNo.val() == "0")) {
        taxEntrySequence = 1;
    }
    $('#tblLeadList tr').each(function (i, row) {
        var $row = $(row);
        var FollowUpSequenceNo = $row.find("#hdnFollowUpSequenceNo").val();
        var CustomerFollowUpId = $row.find("#hdnCustomerFollowUpId").val();
        var FollowUpActivityTypeId = $row.find("#hdnFollowUpActivityTypeId").val();
        var FollowUpActivityTypeName = $row.find("#hdnFollowUpActivityTypeName").val();
        var FollowUpDueDateTime = $row.find("#hdnFollowUpDueDateTime").val();
        var FollowUpReminderDateTime = $row.find("#hdnFollowUpReminderDateTime").val();
        var FollowUpRemarks = $row.find("#hdnFollowUpRemarks").val();
        var Priority = $row.find("#hdnPriority").val();
        var PriorityName = $row.find("#hdnPriorityName").val();
        var FollowUpStatusId = $row.find("#hdnFollowUpStatusId").val();
        var FollowUpStatusName = $row.find("#hdnFollowUpStatusName").val();
        var FollowUpStatusReason = $row.find("#hdnFollowUpStatusReason").val();
        var FollowUpByUserId = $row.find("#hdnFollowUpByUserId").val();
        var hdnFollowUpCreatedBy = $row.find("#hdnFollowUpCreatedBy").val();
        var followUpByUserName = $row.find("#hdnFollowUpByUserName").val();
        var FollowUpCreatedBy = $row.find("#hdnFollowUpCreatedBy").val();

        if (CustomerFollowUpId != undefined) {
            if (action == 1 || (hdnFollowUpSequenceNo.val() != FollowUpSequenceNo)) {
                //if (branchName == txtBranchName.val().trim() && bAddress == txtBAddress.val().trim()) {
                //    ShowModel("Alert", "Branch Name with same Address already exists!!!")
                //    ddlBCountry.focus();
                //    return false;
                //} 
                var customerFollowup = {
                    CustomerFollowUpId: CustomerFollowUpId,
                    FollowUpSequenceNo: FollowUpSequenceNo,
                    FollowUpActivityTypeId: FollowUpActivityTypeId,
                    FollowUpActivityTypeName: FollowUpActivityTypeName,
                    FollowUpDueDateTime: FollowUpDueDateTime,
                    FollowUpReminderDateTime: FollowUpReminderDateTime,
                    FollowUpRemarks: FollowUpRemarks,
                    Priority: Priority,
                    PriorityName: PriorityName,
                    FollowUpStatusId: FollowUpStatusId,
                    FollowUpStatusName: FollowUpStatusName,
                    FollowUpStatusReason: FollowUpStatusReason,
                    FollowUpByUserId: FollowUpByUserId,
                    FollowUpByUserName: followUpByUserName,
                    FollowUpCreatedBy: FollowUpCreatedBy
                };
                followupList.push(customerFollowup); 
                taxEntrySequence = parseInt(taxEntrySequence) + 1;
            }
            else if (hdnFollowUpId.val() == CustomerFollowUpId && hdnFollowUpSequenceNo.val() == FollowUpSequenceNo) {
                var customerFollowUpAdd = { 
                    FollowUpSequenceNo: hdnFollowUpSequenceNo.val(),
                    CustomerFollowUpId: hdnFollowUpId.val(),
                    FollowUpActivityTypeName: $("#ddlActivityType option:selected").text(),
                    FollowUpActivityTypeId: $("#ddlActivityType").val(),
                    FollowUpDueDateTime: txtDueDate.val(),
                    FollowUpReminderDateTime: txtReminderDate.val(),
                    FollowUpRemarks: txtRemarks.val(),
                    PriorityName: $("#ddlPriority option:selected").text(),
                    Priority: ddlPriority.val(),
                    FollowUpStatusId: ddlFollowUpStatus.val(),
                    FollowUpStatusName: $("#ddlFollowUpStatus option:selected").text(),
                    FollowUpStatusReason: txtFollowUpStatusReason.val(),
                    FollowUpByUserId: hdnUserId.val(),
                    FollowUpByUserName: hdnFollowUpByUserName.val(),
                    CreatedBy: hdnLoginUserId.val()
                };
                followupList.push(customerFollowUpAdd);
                hdnFollowUpSequenceNo.val("0");
            }



        }

    });
    if (action == 1 && (hdnFollowUpSequenceNo.val() == "" || hdnFollowUpSequenceNo.val() == "0")) {
        hdnFollowUpSequenceNo.val(taxEntrySequence);
    }
    if (action == 1) {
        var followUpAddEdit = {
            FollowUpSequenceNo: hdnFollowUpSequenceNo.val(),
            CustomerFollowUpId: hdnFollowUpId.val(),
            FollowUpActivityTypeName: $("#ddlActivityType option:selected").text(),
            FollowUpActivityTypeId: $("#ddlActivityType").val(),
            FollowUpDueDateTime: txtDueDate.val(),
            FollowUpReminderDateTime: txtReminderDate.val(),
            FollowUpRemarks: txtRemarks.val(),
            PriorityName: $("#ddlPriority option:selected").text(),
            Priority: ddlPriority.val(),
            FollowUpStatusId: ddlFollowUpStatus.val(),
            FollowUpStatusName: $("#ddlFollowUpStatus option:selected").text(),
            FollowUpStatusReason: txtFollowUpStatusReason.val(),
            FollowUpByUserId: hdnUserId.val(),
            FollowUpByUserName: hdnFollowUpByUserName.val(),
            CreatedBy: hdnLoginUserId.val()
        };
   
    var requestData = { customerFollowUps: followUpAddEdit };
    $.ajax({
        url: "../Customer/CustomerFollowUpValidation",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            ShowModel("Error", err)
            return false;
        },
        success: function (data) {
            if (data.status == "SUCCESS") {
                followupList.push(followUpAddEdit);
                GetFollowUpList(followupList);
            }
            else {
                ShowModel("Error", data.message)
                return false;
            }

        }
    }); 

    followupList.push(followUpAddEdit);
    hdnFollowUpSequenceNo.val("0");
}
if (flag == true) {
    GetFollowUpList(followupList);
}


    
}
function EditFollowUpRow(obj) {

    var $row = $(obj).closest("tr");
    var followUpSequenceNo = $row.find("#hdnFollowUpSequenceNo").val();
    var hdnCustomerFollowUpId = $row.find("#hdnCustomerFollowUpId").val();
    var followUpActivityTypeId = $row.find("#hdnFollowUpActivityTypeId").val();
    var hdnFollowUpActivityTypeName = $row.find("#hdnFollowUpActivityTypeName").val();
    var followUpDueDateTime = $row.find("#hdnFollowUpDueDateTime").val();
    var followUpReminderDateTime = $row.find("#hdnFollowUpReminderDateTime").val();
    var followUpRemarks = $row.find("#hdnFollowUpRemarks").val();
    var priority = $row.find("#hdnPriority").val();
    var hdnPriorityName = $row.find("#hdnPriorityName").val();
    var leadStatusId = $row.find("#hdnFollowUpStatusId").val();
    var hdnLeadStatusName = $row.find("#hdnFollowUpStatusName").val();
    var leadStatusReason = $row.find("#hdnFollowUpStatusReason").val();
    var leadFollowUpId = $row.find("#hdnFollowUpByUserId").val();
    var followUpByUserName = $row.find("#hdnFollowUpByUserName").val();
    $("#hdnFollowUpId").val(hdnCustomerFollowUpId);
   
    $("#ddlActivityType").val(followUpActivityTypeId);
    $("#hdnFollowUpSequenceNo").val(followUpSequenceNo);
    $("#txtReminderDate").val(followUpReminderDateTime);
    $("#ddlPriority").val(priority);
    $("#ddlFollowUpStatus").val(leadStatusId);
    $("#txtAssignFollowUp").val(followUpByUserName);
    $("#txtDueDate").val(followUpDueDateTime);
    $("#txtRemarks").val(followUpRemarks);
    $("#txtFollowUpStatusReason").val(leadStatusReason);
    $("#btnAddFollowUp").hide();
    $("#btnUpdateFollowUp").show();
}

function ClearFollowUpFields() {
    $("#hdnFollowUpId").val("0");
    $("#ddlActivityType").val("0");
    $("#txtDueDate").val("");
    $("#txtReminderDate").val("");
    $("#txtRemarks").val("");
    $("#ddlPriority").val("0");
    $("#ddlFollowUpStatus").val("0");
    $("#txtFollowUpStatusReason").val("");
    $("#txtAssignFollowUp").val("");
    $("#btnAddFollowUp").show();
    $("#btnUpdateFollowUp").hide();
}
function BindFollowUpActivityTypeList() {
    $.ajax({
        type: "GET",
        url: "../Lead/GetFollowUpActivityTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlActivityType").append($("<option></option>").val(0).html("-Select Activity Type-"));
            $.each(data, function (i, item) {

                $("#ddlActivityType").append($("<option></option>").val(item.FollowUpActivityTypeId).html(item.FollowUpActivityTypeName));
            });
        },
        error: function (Result) {
            $("#ddlActivityType").append($("<option></option>").val(0).html("-Select Activity Type-"));
        }
    });
}

function BindLeadStatusList() {
    $.ajax({
        type: "GET",
        url: "../Lead/GetLeadStatusList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlFollowUpStatus").append($("<option></option>").val(0).html("-Select Lead Status-"));
            $.each(data, function (i, item) {

                $("#ddlFollowUpStatus").append($("<option></option>").val(item.LeadStatusId).html(item.LeadStatusName));
            });
        },
        error: function (Result) {
            $("#ddlFollowUpStatus").append($("<option></option>").val(0).html("-Lead Status-"));
        }
    });

}

function changeUINStatus() {
    if ($("#chkUIN").is(':checked')) {
        $("#txtUINNo").attr("disabled", false);
    }
    else {
        $("#txtUINNo").attr("disabled", true);
        $("#txtUINNo").val("");
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

function GetddlCustomerType()
{
    var CustomerType = $("#ddlCustomerType option:selected").text();
    if(CustomerType=='Dealer' || CustomerType=='Distributor')
    {
        $(".showSaleEmpName").show();
    }
    else
    {
        $("#txtSalesEmployeeName").val("");
        $("#hdnSaleEmployeeId").val("0");
        $(".showSaleEmpName").hide();
    }
}