
$(document).ready(function () {
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);

    BindPackingListType();
    BindProductSubGroup();
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    BindCompanyBranchList();

    //$("#txtFromDate").val("01-Jan-2018");
    //$("#txtToDate").val("31-Dec-2019");
   
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
    //$("#txtPMBNo").val("");
    //$("#ddlPackingListType").val("0");
    //$("#ddlProductSubGroup").val("0");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val()); 
    window.location.href = "../PackingMaterialBOM/ListPackingMaterialBOM";

}
function SearchPackingMaterialBOM() {
    var txtPMBNo = $("#txtPMBNo");
    var ddlPackingListType = $("#ddlPackingListType");
    var ddlProductSubGroup = $("#ddlProductSubGroup");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = { pMBNo: txtPMBNo.val().trim(), packingListTypeId: ddlPackingListType.val(), productSubGroupId: ddlProductSubGroup.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), companyBranchId: 0 };
    $.ajax({
        url: "../PackingMaterialBOM/GetPackingMaterialBOMList",
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
function Export() {
    var divList = $("#divList");
    ExporttoExcel(divList);
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
            $("#ddlPackingListType").append($("<option></option>").val(0).html("-Select Packing List Type-"));
            $.each(data, function (i, item) {
                $("#ddlPackingListType").append($("<option></option>").val(item.PackingListTypeID).html(item.PackingListTypeName));
            });
        },
        error: function (Result) {
            $("#ddlPackingListType").append($("<option></option>").val(0).html("-Select Packing List Type-"));
        }
    });
}

/*Bind DropDown For Product Sub Group */
function BindProductSubGroup() {

    $("#ddlProductSubGroup").val(0);
    $("#ddlProductSubGroup").html("");
    $.ajax({
        type: "GET",
        url: "../PackingMaterialBOM/GetProductSubGroupListForPMB",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Product Sub Group-"));
            $.each(data, function (i, item) {
                $("#ddlProductSubGroup").append($("<option></option>").val(item.ProductSubGroupId).html(item.ProductSubGroupName));
            });
        },
        error: function (Result) {
            $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Product Sub Group-"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}