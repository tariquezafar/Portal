$(document).ready(function () { 
    //SearchTax();
    //$('#tblCompanyList').paging({ limit: 2 });   
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
    //$("#hdnTaxId").val("0");
    //$("#txtTaxName").val("");
    //$("#txtTaxPercentage").val("");
    //$("#ddlTaxType").val("0");
    //$("#ddlTaxSubType").val("0");
    //$("#txtGLCode").val("");
    //$("#txtTaxSLCode").val("");
    //$("#ddlStatus").val(""); 
    window.location.href = "../Tax/ListTax";

}
 
 
function SearchTax() {
    var txtTaxName = $("#txtTaxName"); 
    var ddlTaxType = $("#ddlTaxType");
    var ddlTaxSubType = $("#ddlTaxSubType"); 
    var ddlStatus = $("#ddlStatus");
    var requestData = { 
        taxName: txtTaxName.val().trim(), 
        taxType: ddlTaxType.val().trim(),
        taxSubType: ddlTaxSubType.val().trim(), 
        status: ddlStatus.val()
    }; 
    $.ajax({
        url: "../Tax/GetTaxList",
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