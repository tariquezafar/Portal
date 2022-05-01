
$(document).ready(function () {
    $("#txtExpiryDate").attr('readonly', true);
    $("#txtExpiryDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        minDate: 0,
        onSelect: function (selected) {
        }
    });
    BindCompanyList();
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnUserID = $("#hdnUserID");
    if (hdnUserID.val() != "" && hdnUserID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetUserDetail(hdnUserID.val());
        
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true);
            
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
    $("#txtUserName").focus();
        


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
function checkPhone(el) {
    var ex = /^[0-9]+\-?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
function BindCompanyList()
{
    $.ajax({
        type: "GET",
        url: "../User/GetCompanyList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlCompany").append($("<option></option>").val(0).html("-Select Company-"));
            $.each(data, function (i, item) {
                $("#ddlCompany").append($("<option></option>").val(item.CompanyId).html(item.CompanyName));
            });
        },
        error: function (Result) {
            $("#ddlCompany").append($("<option></option>").val(0).html("-Select Company-"));
        }
    });
}
function GetUserDetail(userId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../User/GetUserDetail",
        data: { userId: userId },
        dataType: "json",
        success: function (data) {
            $("#txtUserName").val(data.UserName);
            $("#txtFullName").val(data.FullName);
            $("#txtPhone").val(data.MobileNo);
            $("#txtEmail").val(data.Email);
            $("#txtPassword").val(data.Password);
            $("#txtConfirmPassword").val(data.Password);
            $("#ddlCompany").val(data.CompanyId);
            $("#txtExpiryDate").val(data.ExpiryDate);
            if (data.UserStatus)
            {
                $("#chkStatus").attr("checked",true);
            }
            else
            {
                $("#chkStatus").attr("checked", false);
            }
            
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    
}
function SaveData()
{
    var txtUserName = $("#txtUserName");
    var hdnUserID = $("#hdnUserID");
    var txtFullName = $("#txtFullName");
    var txtEmail = $("#txtEmail");
    var txtPhone = $("#txtPhone");
    var txtPassword = $("#txtPassword");
    var txtConfirmPassword = $("#txtConfirmPassword");
    var ddlCompany = $("#ddlCompany");
    var txtExpiryDate = $("#txtExpiryDate");
    var chkStatus = $("#chkStatus");


    if (txtUserName.val().trim() == "")
    {
        ShowModel("Alert","Please enter Login ID")
        txtUserName.focus();
        return false;
    }
    if (txtUserName.val().length<5) {
        ShowModel("Alert", "Please enter valid Login ID")
        txtUserName.focus();
        return false;
    }
    if (!ValidEmailCheck(txtUserName.val().trim())) {
        ShowModel("Alert", "Please enter Valid Email Id as Login ID")
        txtEmail.focus();
        return false;
    }
    if (txtFullName.val().trim() == "") {
        ShowModel("Alert", "Please enter User Full Name")
        txtFullName.focus();
        return false;
    }
  
    if (txtPhone.val().trim() == "") {
        ShowModel("Alert", "Please enter Phone No.")
        txtPhone.focus();
        return false;
    }
    if (txtPhone.val().trim().length<8) {
        ShowModel("Alert", "Please enter Valid Contact Phone Number")
        txtPhone.focus();
        return false;
    }
    if (txtEmail.val().trim() == "") {
        ShowModel("Alert", "Please enter Email Address")
        txtEmail.focus();
        return false;
    }
    if (!ValidEmailCheck(txtEmail.val().trim()))
    {
        ShowModel("Alert", "Please enter Valid Email Address")
        txtEmail.focus();
        return false;
    }
    if (txtPassword.val().trim() == "") {
        ShowModel("Alert", "Please enter Password")
        txtPassword.focus();
        return false;
    }
    if (txtPassword.val().length < 6) {
        ShowModel("Alert", "Please enter at least 6 character long Password")
        txtPassword.focus();
        return false;
    }
    if (txtConfirmPassword.val().trim() == "") {
        ShowModel("Alert", "Please enter Confirm Password")
        txtConfirmPassword.focus();
        return false;
    }
    if (txtConfirmPassword.val().trim() != txtPassword.val().trim()) {
        ShowModel("Alert", "Please enter Confirm Password same as Password")
        txtConfirmPassword.focus();
        return false;
    }
    if (ddlCompany.val() == "" || ddlCompany.val() == "0") {
        ShowModel("Alert", "Please select User Company")
        ddlCompany.focus();
        return false;
    }
    if (txtExpiryDate.val().trim() == "") {
        ShowModel("Alert", "Please select User Expiry Date")
        txtExpiryDate.focus();
        return false;
    }
    var userStatus = true;
    if (chkStatus.prop("checked") == true)
    { userStatus = true; }
    else
    { userStatus = false; }
        
    var userViewModel = {
        UserId: hdnUserID.val(), UserName: txtUserName.val().trim(), FullName: txtFullName.val().trim(),
        MobileNo: txtPhone.val().trim(), Email: txtEmail.val().trim(), Password: txtPassword.val().trim(), CompanyId: ddlCompany.val(), 
        ExpiryDate: txtExpiryDate.val().trim(), UserStatus: userStatus
    };
    var requestData = { userViewModel: userViewModel };
    $.ajax({
        url: "../User/AddEditPrimaryUser",
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
    $("#txtUserName").val("");
    $("#hdnUserID").val("0");
     $("#hdnAccessMode").val("0");
     $("#txtFullName").val("");
     $("#txtEmail").val("");
     $("#txtPhone").val("");
    
     $("#txtPassword").val("");
     $("#txtConfirmPassword").val("");
     $("#ddlCompany").val("0");
    $("#txtExpiryDate").val("");
    $("#chkStatus").attr("checked", true);
    
}
