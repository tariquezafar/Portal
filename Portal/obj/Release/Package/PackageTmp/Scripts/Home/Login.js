$(document).ready(function () {

});
function ValidateLogin()
{
    var txtUserName = $("#txtUserName");
    var txtPassword = $("#txtPassword");
    if(txtUserName.val().trim()=="")
    {
        ShowModel("Login Alert", "Please enter User name/ Email Id")
        txtUserName.focus();
        return false;
    }
    if (txtPassword.val().trim() == "") {
        ShowModel("Login Alert", "Please enter Password")
        txtPassword.focus();
        return false;
    }
    var requestData = { userName: txtUserName.val(), password :txtPassword.val()};
    $.ajax({
        url: "../Home/Login",
        type: "POST",
        dataType: "json",
        data: requestData,
        success: function (data) {
            if (data.status=="SUCCESS")
            {
                CheckEmployeeLogin(data.Email);
                if (parseInt(data.RoleId) == 1)
                {
                    window.location.href = "../Company/SuperAdminDashboard";
                }
                else
                {
                    window.location.href = "../Dashboard/ModuleDashboard";
                }
                
            }
            else
            {
                $("#closebtn").focus();
                ShowModel("Login Alert", data.message)
               
            }
        },
        error: function (err) {
            ShowModel("Login Alert", err)
           
        }
    });

}

function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
function SendMail() {  
    var txtForgotEmail = $("#txtForgotEmail");
    if (txtForgotEmail.val().trim() == "") {
        alert("Please enter Email Id");
        txtForgotEmail.focus();
        return false;
    }
    if (!ValidEmailCheck(txtForgotEmail.val().trim())) {
        alert("Please enter Valid Email Address");
        txtForgotEmail.focus();
        return false;
    }
    var requestData = { userEmail: txtForgotEmail.val().trim() };
    $.ajax({
        url: "../Home/SendPasswordMail",
        type: "POST",
        dataType: "json",
        data: requestData,
        success: function (data) {
            if (data.status == "SUCCESS") {
                alert(data.message);
                ClearFields();
            }
            else {
                alert(data.message)
            }
        },
        error: function (err) {
            alert(err)
        }
    });
}


function ClearFields() { 
    $("#txtOldPassword").val("");
    $("#txtNewPassword").val("");
    $("#txtForgotEmail").val("");
    $("#txtConfirmNewPassword").val("");
}

function ShowModel(headerText,bodyText)
{
    $("#loginModel").modal(); 
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);
  

}

function CheckEmployeeLogin(emailId) {
    var EmployeeId = 0;
    var requestData = { emailId: emailId };
    $.ajax({
        url: "../Home/GetEmployeeIdByEmailId",
        data: requestData,
        dataType: "json",
        type: "GET",
        asnc: true,
        error: function (err) {
            alert(err);
            EmployeeId = 0;
        },
        success: function (data) {           
            EmployeeId = data.EmployeeId;
            if (EmployeeId != 0)
            {
                window.location.href = "../Dashboard/EssDashboard";
            }
        }
    });
}
 