$(document).ready(function () {
    BindCompanyBranchList();

    $("#txtDispatchNo").attr('readOnly', true);
    $("#txtDispatchDate").attr('readOnly', true);
    $("#txtDispatchPlanNo").attr('readOnly', true);
    $("#txtDispatchPlanDate").attr('readOnly', true);

    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#btnAddSOProduct").hide();
    $("#btnApprove").hide();

  
    $("#txtDispatchDate").datepicker({
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
    var hdnDispatchID = $("#hdnDispatchID");
    if (hdnDispatchID.val() != "" && hdnDispatchID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
            function () {
                GetDispatchDetail(hdnDispatchID.val());
                GetDispatchProductList(hdnDispatchID.val());
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

function OpenDispatchPopup() {
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select company branch.")
        ddlCompanyBranch.focus();
        return false;
    } 
    $("#DispatchModel").modal();
}

function SearchDispatch() {
    var txtSearchDispatchPlanNo = $("#txtSearchDispatchPlanNo");
    var txtSearchCustomerName = $("#txtSearchCustomerName");   
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = {
        dispatchPlanNo: txtSearchDispatchPlanNo.val(),
        customerName: txtSearchCustomerName.val().trim(),
        companyBranchId :1,
        fromDate: txtFromDate.val(),
        toDate: txtToDate.val(),
        approvalStatus: "Approve"
    };
    $.ajax({
        url: "../Dispatch/GetDispatchPlanList",
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

function SelectDispatchPlan(dispatchPlanID, dispatchPlanNo, dispatchPlanDate) {
    $("#txtDispatchPlanNo").val(dispatchPlanNo);
    $("#hdnDispatchPlanID").val(dispatchPlanID);
    $("#txtDispatchPlanDate").val(dispatchPlanDate);

    $("#DispatchModel").modal('hide');
    var sOProductViewModellst = [];
    GetCustomerSOProductList(sOProductViewModellst, dispatchPlanID, true);
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
        url: "../Dispatch/GetCustomerSOProductList",
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
    var hdnDispatchID = $("#hdnDispatchID");
    var txtDispatchDate = $("#txtDispatchDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var hdnDispatchPlanID = $("#hdnDispatchPlanID");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    

   
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select company branch.")
        ddlCompanyBranch.focus();
        return false;
    }

  
    if (ddlApprovalStatus.val() == "" || ddlApprovalStatus.val() == "0" || ddlApprovalStatus.val() == null) {
        ShowModel("Alert", "Please select approve status.")
        ddlApprovalStatus.focus();
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdnDispatchID.val() != null && hdnDispatchID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var dispatchPlanViewModel = {
        DispatchID: hdnDispatchID.val(),
        DispatchDate: txtDispatchDate.val(),
        CompanyBranchID: ddlCompanyBranch.val(), 
        DispatchPlanID: hdnDispatchPlanID.val(),    
        ApprovalStatus: ddlApprovalStatus.val()
    };

    var dispatchPlanProductDetails = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var sOId = $row.find("#hdnSOId").val();
        var productId = $row.find("#hdnProductId").val();
        var quantity = $row.find("#hdnQuantity").val();
        var priority = $row.find("#hdnPriority").val();

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
        dispatchViewModel: dispatchPlanViewModel,
        dispatchProductDetails: dispatchPlanProductDetails
    };
    $.ajax({
        url: "../Dispatch/AddEditDispatch?accessMode=" + accessMode + "",
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
                        window.location.href = "../Dispatch/ListDispatch";
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

function GetDispatchDetail(dispatchID) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Dispatch/GetDispatchDetail",
        data: { dispatchID: dispatchID },
        dataType: "json",
        success: function (data) {
            $("#hdnDispatchID").val(data.DispatchID);
            $("#txtDispatchNo").val(data.DispatchNo);
            $("#txtDispatchDate").val(data.DispatchDate);
            $("#hdnDispatchPlanID").val(data.DispatchPlanID);
            $("#txtDispatchPlanNo").val(data.DispatchPlanNo);
            $("#txtDispatchPlanDate").val(data.DispatchPlanDate);
            $("#ddlCompanyBranch").val(data.CompanyBranchID);
            $("#ddlApprovalStatus").val(data.ApprovalStatus);           
            $("#btnAddNew").show();
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}


function GetDispatchProductList(dispatchID) {
    var requestData = {
        dispatchID: dispatchID
    };
    $.ajax({
        url: "../Dispatch/GetDispatchProductList",
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








