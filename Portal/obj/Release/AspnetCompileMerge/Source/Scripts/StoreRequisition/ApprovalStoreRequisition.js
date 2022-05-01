$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtFromDate").css('cursor', 'pointer');
    $("#txtToDate").css('cursor', 'pointer');

    $("#txtRequisitionNo").attr('readOnly', true);
    $("#txtRequisitionDate").attr('readOnly', true);
    $("#txtCustomerCode").attr('readOnly', true);
    $("#txtWorkOrderNo").attr('readOnly', true);
    $("#txtWorkOrderDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);
    $("#ddlRequisitionType").attr('readOnly', true);
    $("#ddlCompanyBranch").attr('readOnly', true);
    $("#ddlLocation").attr('readOnly', true);
    $("#txtCustomerName").attr('readOnly', true);
    $("#ddlCustomerBranch").attr('readOnly', true);
    $("#txtRequisitionByUser").attr('readOnly', true);
    $("#txtRemarks1").attr('readOnly', true);
    $("#txtRemarks2").attr('readOnly', true);
    $("#txtRejectedReason").attr("disabled", true);
    $("#txtTotalValue").attr('readOnly', true);

    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnRequisitionId = $("#hdnRequisitionId");
    if (hdnRequisitionId.val() != "" && hdnRequisitionId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetStoreRequisitionDetail(hdnRequisitionId.val());
       }, 1000);

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
         
        }
     
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        

       
    }

    var requisitionProducts = [];
    GetRequisitionProductList(requisitionProducts);
  
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
            $.each(data, function (i, item) {
                $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}





function GetRequisitionProductList(requisitionProducts) {
    var hdnRequisitionId = $("#hdnRequisitionId");
    var requestData = { storeRequisitionProducts: requisitionProducts, requisitionId: hdnRequisitionId.val() };
    $.ajax({
        url: "../StoreRequisition/GetStoreRequisitionProductApprovalList",
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
            CalculateGrossandNetValues();
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
        $("#hdnProductDetailId").val("0");
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtUOMName").val("");
        $("#txtQuantity").val("");
    }
}
function BindLocationList(locationId) {
    var companyBranchID = $("#ddlCompanyBranch option:selected").val();
    $("#ddlLocation").val(0);
    $("#ddlLocation").html("");
    $.ajax({
        type: "GET",
        url: "../StoreRequisition/GetBranchLocationList",
        data: { companyBranchID: companyBranchID },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlLocation").append($("<option></option>").val(0).html("-Select Department-"));
            $.each(data, function (i, item) {
                $("#ddlLocation").append($("<option></option>").val(item.LocationId).html(item.LocationName));
            });

            $("#ddlLocation").val(locationId);
        },
        error: function (Result) {
            $("#ddlLocation").append($("<option></option>").val(0).html("-Select Department-"));
        }
    });
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
            
            var hdnRequisitionId = $("#hdnRequisitionId");
            var ddlRequisitionApprovel = $("#ddlRequisitionApprovel");
            var txtRejectedReason = $("#txtRejectedReason");

            if (ddlRequisitionApprovel.val() == "0" || ddlRequisitionApprovel.val() == undefined) {
                ShowModel("Alert", "Please select Store Requisition Approvel Status")
                return false;

            }
            if (ddlRequisitionApprovel.val() == "Rejected") {
                if (txtRejectedReason.val() == "") {
                    ShowModel("Alert", "Please Enter Store Requisition Rejected Reason")
                    return false;

                }
            }

            var storeRequisitionViewModel = {
                RequisitionId: hdnRequisitionId.val(),
                ApprovalStatus: ddlRequisitionApprovel.val(),
                RejectedReason: txtRejectedReason.val()
            };

        var requestData = {storeRequisitionViewModel:storeRequisitionViewModel};
        $.ajax({
            url: "../StoreRequisition/ApprovalStoreRequisition",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
                    ShowModel("Alert", data.message);
                    //ClearFields();
                    setTimeout(
                       function () {
                           window.location.href = "../StoreRequisition/ApprovalStoreRequisition?storeRequisitionId=" + hdnRequisitionId.val() + "&AccessMode=2";
                       }, 1000);

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
    $("#hdnRequisitionId").val("0");
    $("#txtRequisitionDate").val($("#hdnCurrentDate").val());
    $("#ddlRequisitionType").val("0");
    $("#ddlCompanyBranch").val("0");
    $("#hdnCustomerId").val("0");
    $("#hdnUserId").val("0");
    $("#ddlCustomerBranch").val("0");
    $("#txtRemarks1").val("");
    $("#txtRemarks2").val("");
    $("#ddlRequisitionStatus").val("0");
    $("#btnSave").show();
    $("#btnUpdate").hide();
}

function GetStoreRequisitionDetail(requisitionId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../StoreRequisition/GetStoreRequisitionDetail",
        data: { requisitionId: requisitionId },
        dataType: "json",
        success: function (data) {
            $("#txtRequisitionNo").val(data.RequisitionNo);
            $("#txtRequisitionDate").val(data.RequisitionDate);
            $("#txtWorkOrderNo").val(data.WorkOrderNo);
            $("#hdnWorkOrderId").val(data.WorkOrderId);
            $("#txtWorkOrderDate").val(data.WorkOrderDate);
            $("#ddlRequisitionType").val(data.RequisitionType)
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            BindLocationList(data.LocationId);
            $("#ddlLocation").val(data.LocationId);

            $("#txtRequisitionByUser").val(data.FullName);
            $("#hdnUserId").val(data.RequisitionByUserId);
            $("#hdnCustomerId").val(data.CustomerId);
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtCustomerName").val(data.CustomerName);
            BindCustomerBranchList(data.CustomerBranchId);
            $("#ddlCustomerBranch").val(data.CustomerBranchId);
            $("#txtRemarks1").val(data.Remarks1);
            $("#txtRemarks2").val(data.Remarks2);
            $("#ddlRequisitionStatus").val(data.RequisitionStatus);
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            //$("#btnUpdate").hide();

            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }

           

            if (data.ApprovalStatus == "Approved") {
                $("#ddlRequisitionApprovel").val(data.ApprovalStatus);
                $("#btnUpdate").hide();
            }
            else if (data.ApprovalStatus == "Rejected")
            {
                $("#ddlRequisitionApprovel").val(data.ApprovalStatus);
                $("#txtRejectedReason").val(data.RejectedReason);
            }
            else
            {
                $("#ddlRequisitionApprovel").val("0");
            }
         
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function EnableDisableRejectReason() {
    var approvalStatus = $("#ddlRequisitionApprovel option:selected").text();
    if (approvalStatus == "Rejected") {
        $("#DivRejected").show();
        $("#txtRejectedReason").attr("disabled", false);
    }
    else {
        $("#txtRejectedReason").attr("disabled", true);
        $("#txtRejectedReason").val("");
        $("#DivRejected").hide();
    }
}

function CalculateGrossandNetValues() {
    var basicValue = 0;
    var taxValue = 0;
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        
        var totalPrice = $row.find("#hdnTotalPrice").val();
        if (totalPrice != undefined) {
            basicValue += parseFloat(totalPrice);
        }

    });
    $("#txtTotalValue").val(parseFloat(parseFloat(basicValue)).toFixed(2));
}
