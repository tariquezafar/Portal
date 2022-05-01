$(document).ready(function () {   
    BindProductTypeList();
    hdnMappingId = $("#hdnMappingId") ;
    var hdnAccessMode = $("#hdnAccessMode");
   
    if (hdnMappingId.val() != "" && hdnMappingId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetProductGLDetail(hdnMappingId.val());
       }, 1000);
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").prop('disabled', true);
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
function GetProductGLDetail(mappingId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ProductGLMapping/GetProductGLDetail",
        data: { mappingId: mappingId },
        dataType: "json",
        success: function (data) {
            $("#ddlProductType").val(data.ProductSubGroupId);
            $("#txtGLHead").val(data.GLHead);
            $("#hdnGLId").val(data.GLId);
            $("#ddlGLType").val(data.GLType);
            if (data.MappingStatus == true) {
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
function BindProductMainGroupList() {
    $.ajax({
        type: "GET",
        url: "../Product/GetProductMainGroupList",
        data: "{}",
        dataType: "json",
        asnc: false,
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
function BindProductSubGroupList(productSubGroupId) {
    var productMainGroupId = $("#ddlProductMainGroup option:selected").val();
    $("#ddlProductSubGroup").val(0);
    $("#ddlProductSubGroup").html("");
    if (productMainGroupId != undefined && productMainGroupId != "" && productMainGroupId != "0") {
        var data = { productMainGroupId: productMainGroupId };
        $.ajax({
            type: "GET",
            url: "../Product/GetProductSubGroupList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
                $.each(data, function (i, item) {
                    $("#ddlProductSubGroup").append($("<option></option>").val(item.ProductSubGroupId).html(item.ProductSubGroupName));
                });
                $("#ddlProductSubGroup").val(productSubGroupId);
            },
            error: function (Result) {
                $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
            }
        });
    }
    else {

        $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
    }

}
 

function SaveData() {
    var hdnMappingId = $("#hdnMappingId");   
    var ddlProductSubGroup = $("#ddlProductType");
    var hdnGLId = $("#hdnGLId");
    var hdnCompanyId = $("#hdnCompanyId");
    var hdnCreatedBy = $("#hdnCreatedBy");
    var txtGLHead = $("#txtGLHead");
    var ddlGLType = $("#ddlGLType");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;    
    if (ddlProductSubGroup.val().trim() == "0") {
        ShowModel("Alert", "Please select Product Type")
        ddlProductSubGroup.focus();
        return false;
    }
    
    if (txtGLHead.val().trim() == "") {

        ShowModel("Alert", "Please enter GL Head")
        txtGLHead.focus();
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdnMappingId.val() != null && hdnMappingId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var productGLMappingViewModel = {
        MappingId: hdnMappingId.val(),
        ProductSubGroupId:ddlProductSubGroup.val(), 
        GLId:hdnGLId.val(),
        CompanyId: hdnCompanyId.val(),
        GLType:ddlGLType.val(),
        MappingStatus: chkstatus,
        CreatedBy:hdnCreatedBy.val()
    };
    var requestData = {productGLMapping: productGLMappingViewModel };
    $.ajax({
        url: "../ProductGLMapping/AddEditProductGLMapping",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                if (data.message != "Product GL Mapping already exist with same Product Sub Group")
                {
                    ClearFields();
                    setTimeout(
                function () {
                    window.location.href = "../ProductGLMapping/ListProductGLMapping";
                }, 2000);
                    $("#btnSave").show();
                    $("#btnUpdate").hide();
                }               
               
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
    $("#hdnMappingId").val("0");
    $("#ddlProductType").val("0");
    $("#txtGLHead").val("");
    $("#chkStatus").attr("checked", true);
}

function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
}
document.onkeypress = stopRKey;