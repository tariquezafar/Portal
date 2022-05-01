
$(document).ready(function () { 
    BindProductMainGroupList(); 
    //SearchProductSubGroup();
});
 
function BindProductMainGroupList() {
    $.ajax({
        type: "GET",
        url: "../Product/GetProductMainGroupList",
        data: "{}",
        dataType: "json",
        asnc: true,
        success: function (data) {
            $("#ddlProductMainGroup").append($("<option></option>").val(0).html("-Select Main Group-"));
            $.each(data, function (i, item) {
                $("#ddlProductMainGroup").append($("<option></option>").val(item.ProductMainGroupId).html(item.ProductMainGroupName));
            });
        },
        error: function (Result) {
            $("#ddlProductMainGroup").append($("<option></option>").val(0).html("-Select Main Group-"));
        }
    });
}



function GetProductSubGroupDetail(productSubgroupId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ProductSubGroup/GetProductSubGroupDetail",
        data: { productsubgroupId: productsubgroupId },
        dataType: "json",
        success: function (data) {
            $("#txtProductSubGroupName").val(data.ProductSubGroupName);
            $("#txtProductSubGroupCode").val(data.ProductSubGroupCode); 
            if (data.ProductSubGroup_Status == true) {
                $("#chkstatus").attr("checked", true);
            }
            else {
                $("#chkstatus").attr("checked", false);
            }
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}
 

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
 
function ClearFields()
{

    //$("#hdnProductSubGroupId").val("0");
    //$("#txtProductSubGroupName").val("");
    //$("#txtProductSubGroupCode").val("");
    //$("#ddlProductMainGroup").val("0");
    //$("#ddlStatus").val("");
    window.location.href = "../ProductSubGroup/ListProductSubGroup";
 
    
}
function SearchProductSubGroup() {  
    var txtProductSubGroupName = $("#txtProductSubGroupName");
    var txtProductSubGroupCode = $("#txtProductSubGroupCode");
    var ddlProductMainGroup = $("#ddlProductMainGroup");
    var hdnProductSubGroupId = $("#hdnProductSubGroupId");
    var ddlStatus = $("#ddlStatus");
    var requestData = { 
        productSubGroupName: txtProductSubGroupName.val().trim(),
        productSubGroupCode: txtProductSubGroupCode.val().trim(),
        productMainGroupId: ddlProductMainGroup.val(),
        Status: ddlStatus.val()
    };
    $.ajax({
        url: "../ProductSubGroup/GetProductSubGroupList",
        data: requestData,
        dataType: "html",
        asnc: true,
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
