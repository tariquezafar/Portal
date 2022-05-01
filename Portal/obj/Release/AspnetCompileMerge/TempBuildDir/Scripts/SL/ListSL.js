$(document).ready(function () {
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
    //$("#txtSLHead").val("");
    //$("#txtSLCode").val("");
    //$("#ddlSLTypeId").val("0");
    //$("#ddlCostCenterId").val("0"); 
    //$("#ddlStatus").val("");
    window.location.href = "../SL/ListSL";


}

function SearchSL() {
    var txtSLHead = $("#txtSLHead");
    var ddlSLTypeId = $("#ddlSLTypeId");
    var ddlCostCenterId = $("#ddlCostCenterId");
    var ddlStatus = $("#ddlStatus");
    var txtSLCode = $("#txtSLCode");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var requestData = {
        SLHead: txtSLHead.val().trim(),
        SLCode: txtSLCode.val().trim(),
        SLTypeId: ddlSLTypeId.val(),
        CostCenterId: ddlCostCenterId.val(),
        Status: ddlStatus.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
       

    };

    $.ajax({
        url: "../SL/GetSLList",
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
   
    BindSLTypeList();
    BindCostCenterList();
 

    function BindCostCenterList() {
        $.ajax({
            type: "GET",
            url: "../SL/GetCostCenterList",
            data: "{}",
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlCostCenterId").append($("<option></option>").val(0).html("-Select Cost Center Name-"));
                $.each(data, function (i, item) {
                    $("#ddlCostCenterId").append($("<option></option>").val(item.CostCenterId).html(item.CostCenterName));
                });
            },
            error: function (Result) {
                $("#ddlCostCenterId").append($("<option></option>").val(0).html("Select Cost Center Name"));
            }
        });
    }

    function BindSLTypeList() {
        $.ajax({
            type: "GET",
            url: "../SL/GetSLTypeList",
            data: "{}",
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlSLTypeId").append($("<option></option>").val(0).html("-Select SL Type-"));
                $.each(data, function (i, item) {
                    $("#ddlSLTypeId").append($("<option></option>").val(item.SLTypeId).html(item.SLTypeName));
                });
            },
            error: function (Result) {
                $("#ddlSLTypeId").append($("<option></option>").val(0).html("NA"));
            }
        });
    }

    function Export() {
        var divList = $("#divList");
        ExporttoExcel(divList);

    }
    //Add Company Branch In User InterFAce So Binding Process By Dheeraj
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
    //End Code