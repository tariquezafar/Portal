$(document).ready(function () {
    
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnTaxId = $("#hdnTaxId");
  
    if (hdnTaxId.val() != "" && hdnTaxId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetAdditionalTaxDetail(hdnTaxId.val());
    
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true); 
            $("#ddlFormTypeDesc").prop('disabled', true);
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
    $("#txtTaxName").focus();

    $("#txtTaxGLHead").autocomplete({
        minLength: 0,

        source: function (request, response) {
            $.ajax({
                url: "../Tax/GetGLAutoCompleteListForTax",
                type: "GET",
                dataType: "json", 
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.GLHead, value: item.GLId, code: item.GLCode};
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtTaxGLHead").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtTaxGLHead").val(ui.item.label);
            $("#hdnTaxGLId").val(ui.item.value);
            $("#hdnTaxGLCode").val(ui.item.code);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtTaxGLHead").val("");
                $("#hdnTaxGLId").val("0");
                $("#hdnTaxGLCode").val("0");
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


    $("#txtTaxSLHead").autocomplete({
        minLength: 0,

        source: function (request, response) {
            $.ajax({
                url: "../Tax/GetSLAutoCompleteListForTax",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.SLHead, value: item.SLId, code: item.SLCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtTaxSLHead").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtTaxSLHead").val(ui.item.label);
            $("#hdnTaxSLId").val(ui.item.value);
            $("#hdnTaxSLCode").val(ui.item.code);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtTaxSLHead").val("");
                $("#hdnTaxSLId").val("0");
                $("#hdnTaxSLCode").val("0");
                ShowModel("Alert", "Please select SL from List")

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
 

 
//$(".alpha-only").on("keydown", function (event) {
//    // Allow controls such as backspace
//    var arr = [8, 16, 17, 20, 35, 36, 37, 38, 39, 40, 45, 46];

//    // Allow letters
//    for (var i = 65; i <= 90; i++) {
//        arr.push(i);
//    }

//    // Prevent default if not in array
//    if (jQuery.inArray(event.which, arr) === -1) {
//        event.preventDefault();
//    }
//});

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
 
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
   
function GetAdditionalTaxDetail(taxId) {
    $.ajax({
        type: "GET",
        asnc: true,
        url: "../AdditionalTax/GetAddtionalTaxDetail",
        data: { taxId: taxId },
        dataType: "json",
        success: function (data) {
            $("#txtLeadCode").val(data.LeadCode);
            $("#txtTaxName").val(data.AddTaxName); 
            $("#txtTaxGLHead").val(data.TaxGLHead);
            $("#txtTaxSLHead").val(data.TaxSLHead);
            $("#hdnTaxGLCode").val(data.GLCode);
            $("#hdnTaxSLCode").val(data.SLCode);
            $("#hdnTaxGLId").val(data.GLId);
            $("#hdnTaxSLId").val(data.SLId);
            if (data.AdditionalTax_Status == true) {
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
function changeESINoStatus() {
    if ($("#chkCFormApplicable").is(':checked')) {
        $("#txtWithCFormTaxPercentage").attr("disabled", false);
        $("#ddlFormTypeDesc").attr("disabled", false);
    }
    else {
        $("#txtWithCFormTaxPercentage").attr("disabled", true);
        $("#txtWithCFormTaxPercentage").val("");
        $("#ddlFormTypeDesc").attr("disabled", true);
        $("#ddlFormTypeDesc").val("0");
    }
}
function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function SaveData() {
    var txtTaxName = $("#txtTaxName"); 
    var hdnTaxGLId = $("#hdnTaxGLId"); 
    var txtGLCode = $("#hdnTaxGLCode");
    var hdnTaxSLId = $("#hdnTaxSLId");
    var txtTaxSLCode = $("#hdnTaxSLCode"); 
    var hdnTaxId = $("#hdnTaxId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;


    if (txtTaxName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Tax Name")
        txtTaxName.focus();
        return false;
    }
    if (txtGLCode.val() == "" || txtGLCode.val() == 0) {
        ShowModel("Alert", "Please Select GL Head")
        txtGLCode.focus();
        return false;
    }
    if (txtTaxSLCode.val() == "" || txtTaxSLCode.val() == 0) {
        ShowModel("Alert", "Please Select SL Head")
        txtTaxSLCode.focus();
        return false;
    }
 
    var additionaltaxViewModel = {
        AddTaxId: hdnTaxId.val(),
        AddTaxName: txtTaxName.val().trim(), 
        GLId: hdnTaxGLId.val(),
        SLId: hdnTaxSLId.val(),
        GLCode: txtGLCode.val().trim(),
        SLCode: txtTaxSLCode.val().trim(), 
        AdditionalTax_Status: chkstatus 
    };
    var accessMode = 1;//Add Mode
    if (hdnTaxId.val() != null && hdnTaxId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { additionaltaxViewModel: additionaltaxViewModel };
    $.ajax({
        url: "../AdditionalTax/AddEditAdditionalTax?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                if (accessMode == 2) {
                    setTimeout(
                       function () {
                           window.location.href = "../AdditionalTax/ListAdditionalTax";
                       }, 1000);

                }
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
        $("#hdnTaxId").val("0"); 
        $("#txtTaxName").val(""); 
        $("#txtTaxGLHead").val(""); 
        $("#txtTaxSLHead").val(""); 
        $("#hdnTaxGLCode").val("");
        $("#hdnTaxSLCode").val("");
        $("#chkstatus").prop("checked", true); 
    }
