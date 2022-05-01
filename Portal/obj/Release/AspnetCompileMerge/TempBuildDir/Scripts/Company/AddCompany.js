
$(document).ready(function () {
    BindCountryList();
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnCompanyID = $("#hdnCompanyID");
    if (hdnCompanyID.val()!="" && hdnCompanyID.val()!="0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetCompanyDetail(hdnCompanyID.val());
        
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
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
    $("#txtCompanyName").focus();
        


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

function checkPhone(el) {
    var ex = /^[0-9]+\-?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
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
function BindCountryList()
{
    $.ajax({
        type: "GET",
        url: "../Company/GetCountryList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCountry").append($("<option></option>").val(0).html("-Select Country-"));
            $.each(data, function (i, item) {

                $("#ddlCountry").append($("<option></option>").val(item.CountryId).html(item.CountryName));
            });
        },
        error: function (Result) {
            $("#ddlCountry").append($("<option></option>").val(0).html("-Select Country-"));
        }
    });
}
function GetCompanyDetail(companyId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../Company/GetCompanyDetail",
        data: { companyId: companyId },
        dataType: "json",
        success: function (data) {
            $("#txtCompanyName").val(data.CompanyName);
            $("#txtCompanyCode").val(data.CompanyCode);
            $("#txtContactPersonName").val(data.ContactPersonName);
            $("#txtPhone").val(data.Phone);
            $("#txtEmail").val(data.Email);
            $("#txtFax").val(data.Fax);
            $("#txtWebSite").val(data.Website);
            $("#txtAddress").val(data.Address);
            $("#txtCity").val(data.City);
            $("#ddlCountry").val(data.CountryId);
            BindStateList(data.State);
            $("#ddlState").val(data.State);
            $("#txtPinCode").val(data.ZipCode);
            $("#txtCompanyDesc").val(data.CompanyDesc);
            $("#txtPANNo").val(data.PANNo);
            $("#txtTINNo").val(data.TINNo);
            $('#txtGSTNo').val(data.TanNo);
            $('#txtAnnualTurnOver').val(data.AnnualTurnover);
            $("#txtServiceTaxNo").val(data.ServiceTaxNo);
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    
}
function BindStateList(stateId) {
    var countryId = $("#ddlCountry option:selected").val();
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
    else
    {
        
        $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    }
    
}
function SaveData()
{
    var txtCompanyName = $("#txtCompanyName");
    var hdnCompanyID = $("#hdnCompanyID");
    var txtCompanyCode = $("#txtCompanyCode");
    var txtContactPersonName = $("#txtContactPersonName");
    var txtPhone = $("#txtPhone");
    var txtEmail = $("#txtEmail");
    var txtFax = $("#txtFax");
    var txtWebSite = $("#txtWebSite");
    var txtAddress = $("#txtAddress");
    var txtCity = $("#txtCity");
    var ddlCountry = $("#ddlCountry");
    var ddlState = $("#ddlState");
    var txtPinCode = $("#txtPinCode");
    var txtCompanyDesc = $("#txtCompanyDesc");
    var txtPANNo = $("#txtPANNo");
    var txtTINNo = $("#txtTINNo");
    var txtGSTNo = $("#txtGSTNo");
    var txtAnnualTurnOver = $("#txtAnnualTurnOver");
    var txtServiceTaxNo = $("#txtServiceTaxNo"); 

    if (txtCompanyName.val().trim() == "")
    {
        ShowModel("Alert","Please enter Company Name")
        txtCompanyName.focus();
        return false;
    }
    if (txtCompanyCode.val().trim() == "") {
        ShowModel("Alert", "Please enter Company Short Code")
        txtCompanyCode.focus();
        return false;
    }
    if (txtContactPersonName.val().trim() == "") {
        ShowModel("Alert", "Please enter Contact Person Name")
        txtContactPersonName.focus();
        return false;
    }
    if (txtPhone.val().trim() == "") {
        ShowModel("Alert", "Please enter Contact Phone Number")
        txtPhone.focus();
        return false;
    }
    if (txtPhone.val().trim().length<10) {
        ShowModel("Alert", "Please enter Valid Contact Phone Number")
        txtPhone.focus();
        return false;
    }
    if (txtEmail.val().trim() == "") {
        ShowModel("Alert", "Please enter Email Address")
        txtEmail.focus();
        return false;
    }
    if (ddlCountry.val().trim() == "0") {
        ShowModel("Alert", "Please Select Country")
        ddlCountry.focus();
        return false;
    }
    if (ddlState.val().trim() == "0") {
        ShowModel("Alert", "Please Select State")
        ddlState.focus();
        return false;
    }
    if (txtCity.val() == "") {
        ShowModel("Alert", "Please Enter City")
        txtCity.focus();
        return false;
    }
    //if (txtGSTNo.val().trim() == "") {
    //    ShowModel("Alert", "Please Enter GST No.")
    //    txtGSTNo.focus();
    //    return false; 
    //}
    if (!ValidEmailCheck(txtEmail.val().trim()))
    {
        ShowModel("Alert", "Please enter Valid Email Address")
        txtEmail.focus();
        return false;
    } 
    
    var companyViewModel = {
        CompanyId: hdnCompanyID.val(),
        CompanyName: txtCompanyName.val().trim(),
        ContactPersonName: txtContactPersonName.val().trim(),
        Phone: txtPhone.val().trim(),
        Email: txtEmail.val().trim(),
        Fax: txtFax.val().trim(),
        Website: txtWebSite.val().trim(),
        Address: txtAddress.val().trim(),
        City: txtCity.val().trim(),
        State: ddlState.val(),
        CountryId: ddlCountry.val(),
        ZipCode: txtPinCode.val().trim(),
        CompanyDesc: txtCompanyDesc.val().trim(),
        PANNo: txtPANNo.val().trim(),
        TINNo: txtTINNo.val().trim(),
        TanNo: txtGSTNo.val().trim(),
        ServiceTaxNo: txtServiceTaxNo.val().trim(),
        CompanyCode: txtCompanyCode.val().trim(),
        AnnualTurnOver: txtAnnualTurnOver.val()
    };
    var requestData ={ companyViewModel: companyViewModel };
    $.ajax({
        url: "../Company/AddEditCompany",
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
    $("#txtGSTNo").val("");
    $("#txtCompanyName").val("");
    $("#hdnCompanyID").val("0");
    $("#hdnAccessMode").val("0");
    $("#txtCompanyCode").val("");
    $("#txtContactPersonName").val("");
    $("#txtPhone").val("");
    $("#txtEmail").val("");
    $("#txtFax").val("");
    $("#txtWebSite").val("");
    $("#txtAddress").val("");
    $("#txtCity").val("");
    $("#ddlCountry").val("0");
    $("#ddlState").val("0");
    $("#txtPinCode").val("");
    $("#txtCompanyDesc").val("");
    $("#txtAnnualTurnOver").val("");
    $("#txtPANNo").val("");
    $("#txtTINNo").val("");
    $("#txtServiceTaxNo").val("");
    
}
