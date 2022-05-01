$(document).ready(function () {
    //$('#tblCompanyList').paging({ limit: 2 });
    //SearchSL();
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
    $("#txtGLCode").val("");
    $("#ddlGLType").val("0");
    $("#ddlGLMainGroupId").val("0");
    $("#ddlGLSubGroupId").val("0");
    $("#ddlSLtypeId").val("0");
}

function SearchSL() {
    var txtSLCode = $("#txtSLCode");
    var ddlSLTypeId = $("#ddlSLTypeId");
    var ddlCostCenterId = $("#ddlCostCenterId");
    var ddlStatus = $("#ddlStatus");
    
    var requestData = {
        SLCode: txtSLCode.val().trim(),
        SLTypeId: ddlSLTypeId.val(),
        CostCenterId: ddlCostCenterId.val(),
        Status: ddlStatus.val(),
       

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
                $("#ddlSLTypeId").append($("<option></option>").val(0).html("-Select Sl Type-"));
                $.each(data, function (i, item) {
                    $("#ddlSLTypeId").append($("<option></option>").val(item.SLTypeId).html(item.SLTypeName));
                });
            },
            error: function (Result) {
                $("#ddlSLTypeId").append($("<option></option>").val(0).html("NA"));
            }
        });
    }

