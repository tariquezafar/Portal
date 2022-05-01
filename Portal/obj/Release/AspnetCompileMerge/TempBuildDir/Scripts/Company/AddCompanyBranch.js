
$(document).ready(function () {
    BindCountryList();
    $("#chkstatus").prop("checked", true);
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-")); 
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnCompanyBranchId = $("#hdnCompanyBranchId");
    if (hdnCompanyBranchId.val() != "" && hdnCompanyBranchId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetCompanyBranchDetail(hdnCompanyBranchId.val());
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
function BindCountryList() {
    $.ajax({
        type: "GET",
        url: "../Company/GetCountryList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCountry,#ddlBCountry").append($("<option></option>").val(0).html("-Select Country-"));
            $.each(data, function (i, item) {

                $("#ddlCountry,#ddlBCountry").append($("<option></option>").val(item.CountryId).html(item.CountryName));
            });
        },
        error: function (Result) {
            $("#ddlCountry,#ddlBCountry").append($("<option></option>").val(0).html("-Select Country-"));
        }
    });
}
function GetCompanyBranchDetail(CompanyBranchId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../CompanyBranch/GetCompanyBranchDetail",
        data: { CompanyBranchId: CompanyBranchId },
        dataType: "json",
        success: function (data) {
            $("#txtCompanyBranchName").val(data.BranchName);          
            $("#txtBContactPersonName").val(data.ContactPersonName);
            $("#txtBDesignation").val(data.Designation);
            $("#txtBEmail").val(data.Email);
            $("#txtBFax").val(data.Fax);
            $("#txtBMobileNo").val(data.MobileNo); 
            $("#txtBContactNo").val(data.ContactNo);                   
            $("#txtBAddress").val(data.PrimaryAddress);
            $("#txtBCity").val(data.City);
            $("#ddlBCountry").val(data.CountryId);
            BindBranchStateList(data.StateId);
            $("#ddlBState").val(data.StateId);
            $("#txtBPinCode").val(data.PinCode);
            $("#txtBCSTNo").val(data.CSTNo);
            $("#txtBTINNo").val(data.PANNo);
            $("#txtBTINNo").val(data.TINNo);
            $("#txtBPANNo").val(data.PANNo); 
            $("#txtBGSTNo").val(data.GSTNo);
            $("#txtAnnualTurnOver").val(data.AnnualTurnover);
            $("#txtCompanyBranch").val(data.CompanyBranchCode);
            $("#txtManufactorLocationCode").val(data.ManufactorLocationCode);

            if (data.CompanyBranch_Status == true) {
                $("#chkstatus").prop("checked", true);
            }
            else {
                $("#chkstatus").prop("checked", false);
            }

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function BindBranchStateList(stateId) {
    var countryId = $("#ddlBCountry option:selected").val();
    $("#ddlBState").val(0);
    $("#ddlBState").html("");
    if (countryId != undefined && countryId != "" && countryId != "0") {
        var data = { countryId: countryId };
        $.ajax({
            type: "GET",
            url: "../Company/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlBState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlBState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                $("#ddlBState").val(stateId);
            },
            error: function (Result) {
                $("#ddlBState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
    }
    else {

        $("#ddlBState").append($("<option></option>").val(0).html("-Select State-"));
    }

}

function SaveData() { 
    var hdnCompanyBranchId = $("#hdnCompanyBranchId");
    var txtCompanyBranchName = $("#txtCompanyBranchName");
    var txtBContactPersonName = $("#txtBContactPersonName");
    var txtBDesignation = $("#txtBDesignation");
    var txtBEmail = $("#txtBEmail");
    var txtBMobileNo = $("#txtBMobileNo");
    var txtEmail = $("#txtEmail");
    var txtMobileNo = $("#txtMobileNo");
    var txtBContactNo = $("#txtBContactNo");
    var txtBFax = $("#txtBFax");
    var txtBAddress = $("#txtBAddress");
    var txtBCity = $("#txtBCity");
    var ddlBCountry = $("#ddlBCountry");
    var ddlBState = $("#ddlBState");
    var txtBPinCode = $("#txtBPinCode");
    var txtBCSTNo = $("#txtBCSTNo");
    var txtBTINNo = $("#txtBTINNo");
    var txtBPANNo = $("#txtBPANNo");
    var txtBGSTNo = $("#txtBGSTNo");
    var txtAnnualTurnOver = $("#txtAnnualTurnOver")
    var txtCompanyBranch = $("#txtCompanyBranch")
    var txtManufactorLocationCode = $("#txtManufactorLocationCode")
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtCompanyBranchName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Company Branch Name")
        txtCompanyBranchName.focus();
        return false;
    }
    if (txtBMobileNo.val() == "") {
        ShowModel("Alert", "Please Enter Mobile No")
        txtBCity.focus();
        return false;
    }

    if (txtBMobileNo.val().trim().length < 10) {
        ShowModel("Alert", "Please enter valid Mobile No.")
        txtMobileNo.focus();
        return false;
    }

    if (txtBAddress.val().trim() == "") {
        ShowModel("Alert", "Please Enter Address")
        txtBAddress.focus();
        return false;
    }
    if (txtBCity.val().trim() == "") {
        ShowModel("Alert", "Please Enter City name")
        txtBCity.focus();
        return false;
    } 
    
    //if (txtEmail.val().trim() != "" && !ValidEmailCheck(txtEmail.val().trim())) {
    //    ShowModel("Alert", "Please enter Valid Email Id")
    //    txtEmail.focus();
    //    return false;
    //}
    if (ddlBCountry.val() == "" || ddlBCountry.val() == "0") {
        ShowModel("Alert", "Please select Primary Country")
        ddlBCountry.focus();
        return false;
    }
    if (ddlBState.val() == "" || ddlBState.val() == "0") {
        ShowModel("Alert", "Please select Primary State")
        ddlBState.focus();
        return false;
    }
    
    if (txtCompanyBranch.val().trim() == "") {
        ShowModel("Alert", "Please Enter Company Branch")
        txtCompanyBranch.focus();
        return false;
    }
    //if (txtBGSTNo.val() == "") {
    //    ShowModel("Alert", "Please Enter GST Number")
    //    txtBGSTNo.focus();
    //    return false;
    //}

    if (txtManufactorLocationCode.val() == "") {
        ShowModel("Alert", "Please Enter Manufactor Location Code")
        txtManufactorLocationCode.focus();
        return false;
    }

    
    var accessMode = 1;//Add Mode
    if (hdnCompanyBranchId.val() != null && hdnCompanyBranchId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var companyBranch = {
        CompanyBranchId: hdnCompanyBranchId.val(),
        BranchName: txtCompanyBranchName.val(),
        ContactPersonName: txtBContactPersonName.val(),
        Designation: txtBDesignation.val(),
        Email: txtBEmail.val(),
        CompanyBranch_Status: chkstatus,
        MobileNo: txtBMobileNo.val(),
        ContactNo: txtBContactNo.val(),
        Fax: txtBFax.val(),
        PrimaryAddress: txtBAddress.val(),
        City: txtBCity.val(),
        StateId: ddlBState.val(),
        CountryId: ddlBCountry.val(),
        PinCode: txtBPinCode.val(),
        CSTNo: txtBCSTNo.val(),
        TINNo: txtBTINNo.val(),
        PANNo: txtBPANNo.val(),
        GSTNo: txtBGSTNo.val(),
        AnnualTurnOver: txtAnnualTurnOver.val(),
        CompanyBranchCode: txtCompanyBranch.val(),
        ManufactorLocationCode: txtManufactorLocationCode.val()


        };
    var requestData = { companyBranchViewModel: companyBranch };
    $.ajax({
        url: "../CompanyBranch/AddEditCompanyBranch?accessMode=" + accessMode + "",
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
                    window.location.href = "../CompanyBranch/ListCompanyBranch";
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
    $("#txtCompanyBranchName").val("");
    $("#hdnCompanyBranchId").val("0");
    $("#txtBContactPersonName").val("");
    $("#txtBDesignation").val("");
    $("#txtBEmail").val("");
    $("#txtBMobileNo").val("");
    $("#txtBContactNo").val("");
    $("#txtBFax").val("");
    $("#txtBAddress").val("");
    $("#txtBCity").val("");
    $("#ddlBCountry").val("0");
    $("#ddlBState").val("0");
    $("#txtBPinCode").val("");
    $("#txtBCSTNo").val("");
    $("#txtAnnualTurnOver").val(""); 
    $("#txtBTINNo").val("");
    $("#txtBPANNo").val("");
    $("#txtBGSTNo").val("");
    $("#chkstatus").prop("checked", true);
}
