
$(document).ready(function () { 
    BindProductTypeList();
   // SearchProductGLMapping();
    $("#txtGLHead").autocomplete({
        minLength: 0,

        source: function (request, response) {
            $.ajax({
                url: "../ProductGLMapping/GetGLAutoCompleteListForProductGLMapping",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.GLHead, value: item.GLId, code: item.GLCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtGLHead").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtGLHead").val(ui.item.label);
            $("#hdnGLId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtGLHead").val("");
                $("#hdnGLId").val("0");
                ShowModel("Alert", "Please select GL from List")

            }
            return false;
        }

    })
 .autocomplete("instance")._renderItem = function (ul, item) {
     return $("<li>")
       .append("<div><b>" + item.label + " || " + item.code + "</b></div>")
       .appendTo(ul);
 };
});
 
function BindProductTypeList() {
    $.ajax({
        type: "GET",
        url: "../Product/GetProductTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlProductType").append($("<option></option>").val(0).html("-Select Type-"));
            $.each(data, function (i, item) {
                $("#ddlProductType").append($("<option></option>").val(item.ProductTypeId).html(item.ProductTypeName));
            });
        },
        error: function (Result) {
            $("#ddlProductType").append($("<option></option>").val(0).html("-Select Type-"));
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
    //$("#ddlProductType").val("0");
    //$("#txtGLHead").val(""); 
    //$("#ddlStatus").val("");
    window.location.href = "../ProductGLMapping/ListProductGLMapping";

 
    
}
function SearchProductGLMapping() {  
    var ddlProductSubGroup = $("#ddlProductType");
    var ddlState = $("#ddlState");   
    var txtGLHead = $("#txtGLHead");
    var hdnGLId = $("#hdnGLId");
    var ddlStatus = $("#ddlStatus");
    var requestData = { 
        productsubgroupid: ddlProductSubGroup.val(), 
        glId: hdnGLId.val(),
    };
    $.ajax({
        url: "../ProductGLMapping/GetProductGLMappingList",
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
