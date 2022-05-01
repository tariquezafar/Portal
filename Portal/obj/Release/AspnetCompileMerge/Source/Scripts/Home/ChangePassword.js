$(document).ready(function () {

});
 
function SaveData() {
   
    var hdnUserID = $("#hdnUserID");
    var txtOldPassword = $("#txtOldPassword");
    var txtNewPassword = $("#txtNewPassword");
    var txtConfirmNewPassword = $("#txtConfirmNewPassword"); 

    if (txtOldPassword.val().trim() == "") { 
        ShowModel("Alert", "Please enter old Password")
        txtOldPassword.focus();
        return false;
    }

    if (txtNewPassword.val().trim() == "") {
        ShowModel("Alert", "Please enter New Password")
        txtNewPassword.focus();
        return false;
    }
      
    if (txtNewPassword.val().length < 6) {
        ShowModel("Alert", "Please enter at least 6 character long Password")
        txtNewPassword.focus();
        return false;
    } 
 
    if (txtConfirmNewPassword.val().trim() == "") {
        ShowModel("Alert", "Please enter Confirm Password")
        txtConfirmNewPassword.focus();
        return false;
    }
    if (txtConfirmNewPassword.val().trim() != txtNewPassword.val().trim()) {
        ShowModel("Alert", "Please enter Confirm Password same as Password")
        txtConfirmNewPassword.focus();
        return false;
    }
    
   
    var userViewModel = { 
        Password: txtNewPassword.val().trim(), oldPassword: txtOldPassword.val().trim(), confirmPassword: txtConfirmNewPassword.val().trim()
    };
    var requestData = { userViewModel: userViewModel };
    $.ajax({
        url: "../Home/ChangePassword",
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
function ClearFields() { 
    $("#txtOldPassword").val("");
    $("#txtNewPassword").val(""); 
    $("#txtConfirmNewPassword").val("");
}

function ShowModel(headerText,bodyText)
{ 
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
 