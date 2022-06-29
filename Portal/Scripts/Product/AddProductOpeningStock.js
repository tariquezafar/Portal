
$(document).ready(function () {
    BindFinYearList();
    BindCompanyBranch();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnOpeningTrnd = $("#hdnOpeningTrnd");
    $("#txtProductCode").attr('readOnly', true);
    $("#txtProductShortDesc").attr('readOnly', true);
    if (hdnOpeningTrnd.val() != "" && hdnOpeningTrnd.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        setTimeout(

        function () {
            GetProductOpeningDetail(hdnOpeningTrnd.val());
        }, 2000);
        
        
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
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
 

    $("#txtProductName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Product/GetProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            $("#hdnProductId").val(ui.item.value);
            $("#txtProductShortDesc").val(ui.item.desc);
            $("#txtProductCode").val(ui.item.code);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductName").val("");
                $("#hdnProductId").val("0");
                $("#txtProductShortDesc").val("");
                $("#txtProductCode").val("");
                ShowModel("Alert", "Please select Product from List")

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


function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
function BindFinYearList()
{
    $.ajax({
        type: "GET",
        url: "../Product/GetFinYearList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlFinYear").append($("<option></option>").val(0).html("-Select Fin. Year-"));
            $.each(data, function (i, item) {
                $("#ddlFinYear").append($("<option></option>").val(item.FinYearId).html(item.FinYearDesc));
            });
        },
        error: function (Result) {
            $("#ddlFinYear").append($("<option></option>").val(0).html("-Select Fin. Year-"));
        }
    });
}

function BindCompanyBranch() {
    $.ajax({
        type: "GET",
        url: "../ProductOpeningStock/GetCompanyBranchList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlcompanybranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            $.each(data, function (i, item) {
                $("#ddlcompanybranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
            });
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");

            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlcompanybranch").val(hdnSessionCompanyBranchId.val());
                BindBranchLocation();
                $("#ddlcompanybranch").attr('disabled', true);
            }
        },
        error: function (Result) {
            $("#ddlcompanybranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}




function GetProductOpeningDetail(openingTrnId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../ProductOpeningStock/GetProductOpeningDetail",
        data: { openingTrnId: openingTrnId },
        dataType: "json",
        success: function (data) {
            $("#hdnProductId").val(data.ProductId);
            $("#txtProductName").val(data.ProductName);
            $("#txtProductCode").val(data.ProductCode);
            $("#txtProductShortDesc").val(data.ProductShortDesc);
            $("#ddlFinYear").val(data.FinYearId);
            $("#ddlcompanybranch").val(data.CompanyBranchId);
            $("#txtOpeningQty").val(data.OpeningQty);
            $("#hdnLocationId").val(data.LocationId);
            BindBranchLocation();
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    
}
function SaveData()
{
    var hdnOpeningTrnd = $("#hdnOpeningTrnd");
    var hdnProductId = $("#hdnProductId");

    var txtProductName = $("#txtProductName");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");

    var txtOpeningQty = $("#txtOpeningQty");
    var ddlFinYear = $("#ddlFinYear");
    var ddlcompanybranch = $("#ddlcompanybranch");
 
 
    if (txtProductName.val().trim() == "")
    {
        ShowModel("Alert","Please enter Product Name")
        txtProductName.focus();
        return false;
    }

    if (hdnProductId.val() == "" || hdnProductId.val() == "0") {
        ShowModel("Alert", "Please select Product from list")
        txtProductName.focus();
        return false;
    }
 
    if (ddlFinYear.val() == "" || ddlFinYear.val() == "0") {
        ShowModel("Alert", "Please select Financial Year")
        ddlFinYear.focus();
        return false;
    }
    if (ddlcompanybranch.val() == "" || ddlcompanybranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        ddlcompanybranch.focus();
        return false;
    }
    if ($("#ddlLocation").val() == "" || $("#ddlLocation").val() == "0") {
        ShowModel("Alert", "Please select Branch Location.")
        ddlcompanybranch.focus();
        return false;
    }
    if (txtOpeningQty.val() == "") {
        ShowModel("Alert", "Please enter Product Opening Qty")
        txtOpeningQty.focus();
        return false;
    }
    var productOpeningViewModel = {
        OpeningTrnId: hdnOpeningTrnd.val(), ProductId: hdnProductId.val(),
        FinYearId: ddlFinYear.val(), CompanyBranchId: ddlcompanybranch.val(), OpeningQty: txtOpeningQty.val(),
        LocationId: $("#ddlLocation").val()
    };
    var accessMode = 1;//Add Mode
    if (hdnOpeningTrnd.val() != null && hdnOpeningTrnd.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { productOpeningViewModel: productOpeningViewModel };
    $.ajax({
        url: "../ProductOpeningStock/AddEditProductOpening?AccessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status=="SUCCESS")
            {
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
         function () {
             window.location.href = "../ProductOpeningStock/ListProductOpening";
         }, 2000);
                $("#btnSave").show();
                $("#btnUpdate").hide();
            }
            else
            {
                ShowModel("Error", data.message)
            }
        },
        error: function (err) {
            ShowModel("Error", err)
        }
    });
}
function ShowModel(headerText,bodyText)
{
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);
}
function ClearFields()
{
    $("#hdnOpeningTrnd").val("0");
    $("#hdnAccessMode").val("3");
    $("#hdnProductId").val("0");
    $("#txtProductName").val("");
    $("#txtProductCode").val("");
    $("#txtProductShortDesc").val("");
    $("#ddlFinYear").val("0");
    $("#ddlcompanybranch").val("0");
    $("#txtOpeningQty").val("0");
    
}

function BindBranchLocation() {

    if ($("#ddlcompanybranch").val() != "0" && $("#ddlcompanybranch").val() != "") {
        BranchId = $("#ddlcompanybranch").val();
        $.ajax({
            type: "GET",
            url: "../Fabrication/GetBranchLocationList",
            data: { companyBranchID: BranchId },
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlLocation").append($("<option></option>").val(0).html("-Select Branch Location-"));
                $.each(data, function (i, item) {
                    $("#ddlLocation").append($("<option></option>").val(item.LocationId).html(item.LocationName));
                });
                if ($("#hdnLocationId").val() != "0") {
                    $("#ddlLocation").val($("#hdnLocationId").val());
                }

            },
            error: function (Result) {
                $("#ddlLocation").append($("<option></option>").val(0).html("-Select Branch Location-"));
            }
        });
    }
    else {
        $("#ddlLocation").html('');
        $("#ddlLocation").append($("<option></option>").val(0).html("-Select Branch Location-"));
    }
   
}
