$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtDispatchPlanDate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#btnAddSOProduct").hide();
    $("#btnApprove").hide();

    $("#txtCustomerName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../DispatchPlan/GetCustomerAutoCompletewithSaleOrderList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        debugger;
                        return {
                            label: item.Text,
                            value: item.ValueInt,
                            CustomerName: item.Text,

                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtCustomerName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#hdnCustomerId").val(ui.item.value);
            $("#txtCustomerName").val(ui.item.label);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtCustomerName").val("");
                $("#hdnCustomerId").val("0");
                ShowModel("Alert", "Please select customer.")

            }
            return false;
        }

    })
        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<div><b>" + item.label + " </div>")
                .appendTo(ul);
        };



    $("#txtDispatchPlanDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) { }
    });

    $("#txtSearchFromDate,#txtSearchToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });


    var hdnAccessMode = $("#hdnAccessMode");
    var hdnDispatchPlanID = $("#hdnDispatchPlanID");
    if (hdnDispatchPlanID.val() != "" && hdnDispatchPlanID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
            function () {
                GetDispatchPlanDetail(hdnDispatchPlanID.val());
                var sOProductViewModellst = [];
                GetCustomerSOProductList(sOProductViewModellst, hdnDispatchPlanID.val(), true);
            }, 3000);

        if (hdnAccessMode.val() == "3" || hdnAccessMode.val() == "4") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("#btnSearchSO").hide();
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true);
            if (hdnAccessMode.val() == "4") {
                $("#btnApprove").show();
                $("#ddlApprovalStatus").attr('disabled', false);
            }
            
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

});
$(".numeric-only").on("input", function () {
    var regexp = /\D/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});

function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}

function OpenSOPopup() {
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var hdnCustomerId = $("#hdnCustomerId");
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select company branch.")
        ddlCompanyBranch.focus();
        return false;
    }

    if (hdnCustomerId.val() == "" || hdnCustomerId.val() == "0") {
        ShowModel("Alert", "Please select customer.")
        txtCustomerName.focus();
        return false;
    }
    $("#SOModel").modal();
}

function SearchSO() {
    var hdnCustomerId = $("#hdnCustomerId");
    var txtSearchSONo = $("#txtSearchSONo");
    var txtSearchQuotationNo = $("#txtSearchQuotationNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = {
        customerID: hdnCustomerId.val(),
        soNo: txtSearchSONo.val().trim(),
        quotationNo: txtSearchQuotationNo.val().trim(),
        fromDate: txtFromDate.val(),
        toDate: txtToDate.val(),
        companyBranchId: 1,
    };
    $.ajax({
        url: "../DispatchPlan/GetCustomerSOList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divSOList").html("");
            $("#divSOList").html(err);
        },
        success: function (data) {
            $("#btnAddSOProduct").show();
            $("#divSOList").html("");
            $("#divSOList").html(data);
        }
    });
}

function SelectSO() {
    var soIds = "";
    var mappingStatus = false;
    $('.mapping-list tr').each(function (i, row) {
        debugger;
        var $row = $(row);
        var addAccess = $row.find("#chkAddAccess").is(':checked') ? true : false;
        if (addAccess) {
            soIds += $row.find("#hdnSOId").val() + ",";
            mappingStatus = true;
        }
    });
    if (!mappingStatus) {
        ShowModel("Alert", "Please select at least one SO No");
        return false;
    }
   
    $("#SOModel").modal('hide');
    var sOProductViewModellst = [];
    GetCustomerSOProductList(sOProductViewModellst, soIds, false);
}

function CheckAllCheckBoxAccess(obj) {
    $('.AddAccess').prop('checked', obj.checked);
}

function GetCustomerSOProductList(sOProductViewModellst, sOIds, isDispatchPlan) {
    var requestData = {
        sOProductViewModellst: sOProductViewModellst,
        sOIds: sOIds,
        isDispatchPlan: isDispatchPlan
    };
    $.ajax({
        url: "../DispatchPlan/GetCustomerSOProductList",
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

function SaveData() {
    var hdnDispatchPlanID = $("#hdnDispatchPlanID");
    var txtDispatchPlanDate = $("#txtDispatchPlanDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var hdnCustomerId = $("#hdnCustomerId");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    

   
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select company branch.")
        ddlCompanyBranch.focus();
        return false;
    }

    if (hdnCustomerId.val() == "" || hdnCustomerId.val() == "0") {
        ShowModel("Alert", "Please select customer.")
        txtCustomerName.focus();
        return false;
    }
    if (ddlApprovalStatus.val() == "" || ddlApprovalStatus.val() == "0" || ddlApprovalStatus.val() == null) {
        ShowModel("Alert", "Please select approve status.")
        ddlApprovalStatus.focus();
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdnDispatchPlanID.val() != null && hdnDispatchPlanID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var dispatchPlanViewModel = {
        DispatchPlanID: hdnDispatchPlanID.val(),
        DispatchPlanDate: txtDispatchPlanDate.val(),
        CustomerID: hdnCustomerId.val(),
        CompanyBranchID: ddlCompanyBranch.val(),
        approvalStatus: ddlApprovalStatus.val()
    };

    var dispatchPlanProductDetails = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);

        var sOId = $row.find("#hdnSOId").val();
        var productId = $row.find("#hdnProductId").val();
        var quantity = $row.find("#hdnQuantity").val();
        var priority = $row.find(".Priority").val();

        if (productId != undefined) {
            var dispatchProduct = {
                SOId: sOId,
                ProductId: productId,         
                Quantity: quantity,
                Priority: priority
            };
            dispatchPlanProductDetails.push(dispatchProduct);
        }
    });

    var rowCount = $('#tblProductList tr').length;
    if (rowCount == 1) {
        alert("Please add at least one product.");
        return false;
    }

    var requestData = {
        dispatchPlanViewModel: dispatchPlanViewModel,
        dispatchPlanProductDetails: dispatchPlanProductDetails
    };
    $.ajax({
        url: "../DispatchPlan/AddEditDispatchPlan?accessMode=" + accessMode + "",
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
                        window.location.href = "../DispatchPlan/ListDispatchPlan";
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


function ClearFields() {
    $("#hdnDispatchPlanID").val("0");
    $("#txtDispatchPlanNo").val("");
    $("#txtDispatchPlanDate").val("");
    $("#ddlCompanyBranch").val("0");
    $("#hdnCustomerId").val("0");
}

function GetDispatchPlanDetail(dispatchPlanID) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../DispatchPlan/GetDispatchPlanDetail",
        data: { dispatchPlanID: dispatchPlanID },
        dataType: "json",
        success: function (data) {
            $("#hdnDispatchPlanID").val(data.DispatchPlanID);
            $("#txtDispatchPlanNo").val(data.DispatchPlanNo);
            $("#txtDispatchPlanDate").val(data.DispatchPlanDate);
            $("#ddlCompanyBranch").val(data.CompanyBranchID);
            $("#hdnCustomerId").val(data.CustomerID);
            $("#txtCustomerName").val(data.CustomerName);
            if (data.ApprovalStatus == "Final" && $("#hdnAccessMode").val() == "4") {
                $("#ddlApprovalStatus").val("0");
            }
            else {
                $("#ddlApprovalStatus").val(data.ApprovalStatus);
            }
            


            $("#btnAddNew").show();
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}








