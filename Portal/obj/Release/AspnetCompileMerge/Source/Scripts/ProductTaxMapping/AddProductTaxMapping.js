$(document).ready(function () {
    BindProductMainGroupList();
    BindStateList(0);
    hdnMappingId = $("#hdnMappingId") ;
    var hdnAccessMode = $("#hdnAccessMode");
   
    if (hdnMappingId.val() != "" && hdnMappingId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetProductStateTaxDetail(hdnMappingId.val());
       }, 2000);
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
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

    $("#txtTaxName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../PO/GetTaxAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.TaxName, value: item.TaxId, percentage: item.TaxPercentage };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtTaxName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtTaxName").val(ui.item.label);
            $("#hdnTaxId").val(ui.item.value);
            $("#txtTaxPercentage").val(ui.item.percentage);

            if (parseFloat($("#txtBasicValue").val()) > 0) {
                var taxAmount = (parseFloat($("#txtBasicValue").val()) * (parseFloat($("#txtTaxPercentage").val()) / 100));
                $("#txtTaxAmount").val(Math.round(taxAmount).toFixed(2));
            }
            else {
                $("#txtTaxAmount").val("0");
            }
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtTaxName").val("");
                $("#hdnTaxId").val("0");
                $("#txtTaxPercentage").val("0");
                $("#txtTaxAmount").val("0");
                ShowModel("Alert", "Please select Tax from List")

            }
            return false;
        }

    })
});
function GetProductStateTaxDetail(mappingId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ProductTaxMapping/GetProductStateTaxDetail",
        data: { mappingId: mappingId },
        dataType: "json",
        success: function (data) {
            $("#ddlProductMainGroup").val(data.ProductMainGroupId);
            BindProductSubGroupList(data.ProductSubGroupId);
            $("#ddlProductSubGroup").val(data.ProductSubGroupId);
            $("#ddlState").val(data.StateId);
            $("#txtTaxName").val(data.TaxName);
            $("#hdnTaxId").val(data.TaxId);
            $("#chkstatus").val(data.status == 1 ? true : false);
           
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
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
function BindStateList(stateId) {
    var countryId =parseInt("1");
    $("#ddlState").val(0);
    $("#ddlState").html("");
    if (countryId != undefined && countryId != "" && countryId != "0") {
        var data = { countryId: countryId };
        $.ajax({
            type: "GET",
            url: "../Company/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                $("#ddlState").val(stateId);
            },
            error: function (Result) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
    }
    else {

        $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    }

}

function SaveData() {
    var hdnMappingId = $("#hdnMappingId");
    var ddlProductMainGroup = $("#ddlProductMainGroup");
    var ddlProductSubGroup = $("#ddlProductSubGroup");
    var ddlState = $("#ddlState");
    var hdnTaxId = $("#hdnTaxId");
    var hdnCompanyId = $("#hdnCompanyId");
    var hdnCreatedBy = $("#hdnCreatedBy");
    var txtTaxName= $("#txtTaxName");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (ddlProductMainGroup.val().trim() == "0") {
        ShowModel("Alert", "Please select Product Main Group name")
        ddlProductMainGroup.focus();
        return false;
    }
    if (ddlProductSubGroup.val().trim() == "0") {
        ShowModel("Alert", "Please select Product  Sub Group name")
        ddlProductSubGroup.focus();
        return false;
    }
    if (ddlState.val().trim() == "0") {
        ShowModel("Alert", "Please select state name")
        ddlState.focus();
        return false;
    }
    if (txtTaxName.val().trim() == "") {

        ShowModel("Alert", "Please enter Tax name")
        txtTaxName.focus();
        return false;
    }
    var productSubCategoryStateTaxMappingViewModel = {
        MappingId: hdnMappingId.val(),
        ProductSubGroupId:ddlProductSubGroup.val(),
        StateId: ddlState.val(),
        TaxId:hdnTaxId.val(),
        CompanyId: hdnCompanyId.val(),
        MappingStatus: chkstatus,
        CreatedBy:hdnCreatedBy.val()
    };
    var requestData = {productSubCategoryStateTaxMapping: productSubCategoryStateTaxMappingViewModel };
    $.ajax({
        url: "../ProductTaxMapping/AddEditProductTaxMapping",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
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
    $("#hdnMappingId").val("0");
    $("#ddlProductMainGroup").val("0");
    $("#ddlProductSubGroup").val("0");
    $("#ddlState").val("0");
    $("#txtTaxName").val("");
    $("#chkStatus").attr("checked", true);
}

