
$(document).ready(function () { 
    BindProductMainGroupList();   
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnProductSubGroupId = $("#hdnProductSubGroupId");
    if (hdnProductSubGroupId.val() != "" && hdnProductSubGroupId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetProductSubGroupDetail(hdnProductSubGroupId.val());
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
        }
        else
        {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else
    {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }
    $("#txtProductName").focus();
        


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
 
 

function GetProductSubGroupDetail(productsubgroupId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ProductSubGroup/GetProductSubGroupDetail",
        data: { productsubgroupId: productsubgroupId },
        dataType: "json",
        success: function (data) {
            $("#txtProductSubGroupName").val(data.ProductSubGroupName);
            $("#txtProductSubGroupCode").val(data.ProductSubGroupCode);
            $("#ddlProductMainGroup").val(data.ProductMainGroupId);
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

function SaveData() {
    var txtProductSubGroupName = $("#txtProductSubGroupName");
    var txtProductSubGroupCode = $("#txtProductSubGroupCode");
    var ddlProductMainGroup = $("#ddlProductMainGroup");
    var hdnProductSubGroupId = $("#hdnProductSubGroupId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    
    if (txtProductSubGroupName.val().trim() == "") {
        ShowModel("Alert", "Please enter Product Sub Group Name")
        txtProductSubGroupName.focus();
        return false;
    }
    
    if (ddlProductMainGroup.val() == "0") {
        ShowModel("Alert", "Please Select Product Main Group")
        ddlProductMainGroup.focus();
        return false;
    } 

    if (txtProductSubGroupCode.val().trim() == "") {
        ShowModel("Alert", "Please enter Product Sub Group Code")
        txtProductSubGroupCode.focus();
        return false;
    }

    var productsubgroupViewModel = {
        ProductSubGroupId: hdnProductSubGroupId.val(),
        productSubGroupName: txtProductSubGroupName.val().trim(),
        productSubGroupCode: txtProductSubGroupCode.val().trim(),
        productMainGroupId: ddlProductMainGroup.val(),
        ProductSubGroup_Status: chkstatus
    };
    var accessMode = 1;//Add Mode
    if (hdnProductSubGroupId.val() != null && hdnProductSubGroupId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { productsubgroupViewModel: productsubgroupViewModel };
    $.ajax({
        url: "../ProductSubGroup/AddEditProductSubGroup?AccessMode=" + accessMode + "",
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
                window.location.href = "../ProductSubGroup/ListProductSubGroup";
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
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);
}
function ClearFields() {
    $("#hdnProductSubGroupId").val("0");
    $("#txtProductSubGroupName").val("");
    $("#txtProductSubGroupCode").val("");
    $("#ddlProductMainGroup").val("0");
    $("#chkstatus").prop("checked", true);

}
