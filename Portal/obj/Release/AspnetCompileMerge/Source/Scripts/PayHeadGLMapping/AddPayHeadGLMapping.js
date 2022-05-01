$(document).ready(function () {
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnPayHeadMappingId = $("#hdnPayHeadMappingId");
  
    if (hdnPayHeadMappingId.val() != "" && hdnPayHeadMappingId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetPayHeadGLMappingDetail(hdnPayHeadMappingId.val());
    
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
            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else {       
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
   
function GetPayHeadGLMappingDetail(payHeadMappingId) {
    $.ajax({
        type: "GET",
        asnc: true,
        url: "../PayHeadGLMapping/GetPayHeadGLMappingDetail",
        data: { payHeadMappingId: payHeadMappingId },
        dataType: "json",
        success: function (data) {           
            $("#txtPayHeadName").val(data.PayHeadName);
            $("#hdnPayHeadMappingId").val(data.PayHeadMappingId);
            $("#txtTaxGLHead").val(data.TaxGLHead);
            $("#txtTaxSLHead").val(data.TaxSLHead);
            $("#hdnTaxGLCode").val(data.GLCode);
            $("#hdnTaxSLCode").val(data.SLCode);
            $("#hdnTaxGLId").val(data.GLId);
            $("#hdnTaxSLId").val(data.SLId);
           
            $("#ddlCompanyBranch").val(data.CompanyBranch);
            if (data.PayHeadGLMapping_Status == true) {
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
    var txtPayHeadName = $("#txtPayHeadName");
    var hdnPayHeadMappingId = $("#hdnPayHeadMappingId");
    var hdnTaxGLId = $("#hdnTaxGLId");
    var txtGLCode = $("#hdnTaxGLCode");
    var hdnTaxSLId = $("#hdnTaxSLId");
    var txtTaxSLCode = $("#hdnTaxSLCode"); 
    var hdnTaxId = $("#hdnTaxId");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;


    if (txtPayHeadName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Pay Head GL Mapping Name")
        txtPayHeadName.focus();
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

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }
 
    var payHeadGLMappingViewModel = {
        PayHeadMappingId: hdnPayHeadMappingId.val(),
        PayHeadName: txtPayHeadName.val().trim(),
        GLId: hdnTaxGLId.val(),
        SLId: hdnTaxSLId.val(),
        GLCode: txtGLCode.val().trim(),
        SLCode: txtTaxSLCode.val().trim(), 
        PayHeadGLMapping_Status: chkstatus,
        CompanyBranch: ddlCompanyBranch.val()
    };

    var accessMode = 1;//Add Mode
    if (hdnPayHeadMappingId.val() != null && hdnPayHeadMappingId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { payHeadGLMappingViewModel: payHeadGLMappingViewModel };
    $.ajax({
        url: "../PayHeadGLMapping/AddEditPayHeadGLMapping?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                if (data.status == "SUCCESS") {
                    ShowModel("Alert", data.message);
                    ClearFields();
                    setTimeout(
                      function () {
                          window.location.href = "../PayHeadGLMapping/AddEditPayHeadGLMapping?payHeadMappingId=" + data.trnId + "&AccessMode=3";
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
        $("#hdnPayHeadMappingId").val("0");
        $("#txtPayHeadName").val("");
        $("#txtTaxGLHead").val(""); 
        $("#txtTaxSLHead").val(""); 
        $("#hdnTaxGLCode").val("");
        $("#hdnTaxSLCode").val("");
        $("#chkstatus").prop("checked", true); 
    }
