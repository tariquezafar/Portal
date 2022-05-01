
$(document).ready(function () {
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
    var hdnTotalSaleReturnList = $("#hdnTotalSaleReturnList").val();
    if (hdnTotalSaleReturnList == "true")
    {
        SearchSaleReturn();
    }

    BindCompanyBranchList();
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

function ClearFields() {
    //$("#txtSaleReturnNo").val("");
    //$("#txtCustomerName").val("");
    //$("#txtDispatchRefNo").val("");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    //$("#ddlApprovalStatus").val("0");
    //$("#txtSearchCreatedBy").val("");
    window.location.href = "../SaleReturn/ListSaleReturn";
    
}
function SearchSaleReturn() {
    var txtSaleReturnNo = $("#txtSaleReturnNo");
    var txtCustomerName = $("#txtCustomerName");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtDispatchRefNo = $("#txtDispatchRefNo");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var txtSearchCreatedBy = $("#txtSearchCreatedBy");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var requestData = { saleReturnNo: txtSaleReturnNo.val().trim(), customerName: txtCustomerName.val().trim(), dispatchrefNo: txtDispatchRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: ddlApprovalStatus.val(), CreatedByUserName: txtSearchCreatedBy.val(), companyBranchId: ddlCompanyBranch.val() };
    $.ajax({
        url: "../SaleReturn/GetSaleReturnList",
        data: requestData,
        dataType: "html",
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
function OpenSaleReturnPopup(saleReturnId) {
  
    if (confirm("Do you want to Cancel Sale Return?")) {
        $("#hdnSaleReturnID").val(saleReturnId);
        $("#SearchSaleReturn").modal();
    }

 

}
function CancelSaleReturn() {
   
    var saleReturnId = $("#hdnSaleReturnID").val();
    var cancelReason = $("#txtCancelReason").val();

    if (cancelReason == "") {
        alert("Please Enter Cancel Reason..");       
        return false;
    }
    $("#SearchSaleReturn").hide();
    var requestData = { saleReturnID: saleReturnId, cancelReason: cancelReason };
    $.ajax({
        url: "../SaleReturn/CancelSaleReturn",
        data: requestData,
        dataType: "JSON",
        type: "POST",
        success: function (data) {
            if (data.status == "SUCCESS") {
              
                ShowModel("Alert", data.message);

               
                setTimeout(
                  function () {
                      window.location.href = "../SaleReturn/ListSaleReturn";
                  }, 2000);
                            
               
            }
            else {
                ShowModel("Error", data.message);
            }
        },
        error: function (err) {
        ShowModel("Error", err.responseText)
    }
    });
}

function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}