
$(document).ready(function () {
    //SearchAssemblyList();
    BindCompanyBranchList();
    $("#txtCopyToAssemblyName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../ProductBOM/GetProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, assemblyType: "" },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtCopyToAssemblyName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtCopyToAssemblyName").val(ui.item.label);
            $("#hdnCopyToAssemblyId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtCopyToAssemblyName").val("");
                $("#hdnCopyToAssemblyId").val("0");
                alert("Please select Main Assembly / Sub Assembly from List");

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.desc + "</div>")
      .appendTo(ul);
};
  
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
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};

function ClearFields()
{
    //$("#txtProductName").val("");
    //$("#ddlAssemblyType").val("0");
    window.location.href = "../ProductBOM/ListProductBOM";

    
    
}
function SearchAssemblyList() {

    var txtProductName = $("#txtProductName");
    var ddlAssemblyType = $("#ddlAssemblyType");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var requestData = { assemblyName: txtProductName.val().trim(), assemblyType: ddlAssemblyType.val(), companyBranchId:0 };
    $.ajax({
        url: "../ProductBOM/GetAssemblyList",
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



function GetAssemblyBOMList(obj)
{
    var id = $(obj).attr("id");
    var assemblyId = id.split('_')[1];
    if ($("#divAssemblyBOM_" + assemblyId).css('display') == 'none') {
        $("#divAssemblyBOM_" + assemblyId).show();
    }
    else
    {
        $("#divAssemblyBOM_" + assemblyId).hide();
        return false;
    }
        
    var requestData = { assemblyId: assemblyId };
    $.ajax({
        url: "../ProductBOM/GetAssemblyBOMList",
        data: requestData,
        dataType: "html",
        asnc: true,
        type: "GET",
        error: function (err) {
            $("#divAssemblyBOM_" + assemblyId).html("");
            $("#divAssemblyBOM_" + assemblyId).html(err);
        },
        success: function (data) {
            $("#divAssemblyBOM_" + assemblyId).html("");
            $("#divAssemblyBOM_" + assemblyId).html(data);

        }
    });
}

function CopyAssemblyBOMList(obj) {
    var id = $(obj).attr("id");
    var assemblyId = id.split('_')[1];
    $("#hdnCopyFromAssemblyId").val(assemblyId);
    ShowCopyModel();


}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ShowCopyModel() {
    $("#copyModel").modal();
    
}
function CopyBOM() {
    var hdnCopyFromAssemblyId = $("#hdnCopyFromAssemblyId");
    var hdnCopyToAssemblyId = $("#hdnCopyToAssemblyId");
    var txtCopyToAssemblyName = $("#txtCopyToAssemblyName");

    if (txtCopyToAssemblyName.val().trim() == "") {
        alert("Please enter Main Assemby/ Sub Assembly Name");
        txtCopyToAssemblyName.focus();
        return false;
    }
    if (hdnCopyToAssemblyId.val() == "" || hdnCopyToAssemblyId.val() == "0") {
        alert("Please select Main Assemby/ Sub Assembly from list");
        txtCopyToAssemblyName.focus();
        return false;
    }
    if (hdnCopyFromAssemblyId.val() == "" || hdnCopyFromAssemblyId.val() == "0") {
        alert("Please choose Assembly to be copied");
        return false;
    }


   
    if (hdnCopyFromAssemblyId.val() == hdnCopyToAssemblyId.val()) {
        alert("Assembly Copy from and Assembly Copy To cannot be same");
        txtCopyToAssemblyName.focus();
        return false;
    }

    
    var requestData = { copyFromAssemblyId: hdnCopyFromAssemblyId.val(), copyToAssemblyId: hdnCopyToAssemblyId.val() };
    $.ajax({
        url: "../ProductBOM/CopyProductBOM",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                alert(data.message);
                $("#hdnCopyFromAssemblyId").val("0");
                $("#hdnCopyToAssemblyId").val("0");
                $("#txtCopyToAssemblyName").val("");
            }
            else {
                alert(data.message);
            }
        },
        error: function (err) {
            alert(err);
        }
    });

}

function CancelAssemblyBOMList(obj) {
    var id = $(obj).attr("id");
    var assemblyId = id.split('_')[1];
   
    var requestData = { assemblyId: assemblyId };
    $.ajax({
        url: "../ProductBOM/GetAssemblyBOMList",
        data: requestData,
        dataType: "html",
        asnc: true,
        type: "GET",
        error: function (err) {
            $("#divAssemblyBOM_" + assemblyId).html("");
            $("#divAssemblyBOM_" + assemblyId).html(err);
        },
        success: function (data) {
            $("#divAssemblyBOM_" + assemblyId).html("");
            $("#divAssemblyBOM_" + assemblyId).html(data);

        }
    });
}

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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}