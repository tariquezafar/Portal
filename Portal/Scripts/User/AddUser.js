
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
    BindRoleList();
    BindCompanyBranchList();
    BindCustomerTypeList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnUserID = $("#hdnUserID");
    if (hdnUserID.val() != "" && hdnUserID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        setTimeout(
             function () {
                 GetUserDetail(hdnUserID.val());
             }, 1000);
        
        
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
           
            $("#FileUpload1").attr('disabled', true);
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true);
            $("#txtExpiryDate").prop('disabled', true);
            
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
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
function checkPhone(el) {
    var ex = /^[0-9]+\-?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function BindRoleList()
{
    $.ajax({
        type: "GET",
        url: "../User/GetRoleList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlRole").append($("<option></option>").val(0).html("-Select Role-"));
            $.each(data, function (i, item) {
                $("#ddlRole").append($("<option></option>").val(item.RoleId).html(item.RoleName));
            });
        },
        error: function (Result) {
            $("#ddlRole").append($("<option></option>").val(0).html("-Select Role-"));
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
            $("#ddlRole").val(data.RoleId);
            $("#txtExpiryDate").val(data.ExpiryDate);

            $("#ddlCompanyBranch").val(data.CompanyBranchId);

            if (data.UserPicName == "") {
                $("#btnRemoveImg").hide();
            }
            if (data.UserPicName) {
                $("#btnRemoveImg").show(); 
            }

            $("#imgUserPic").attr("src", "../Images/UserImages/" + data.UserPicName);

            //var d = new Date();
            //var strDate = d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate();
            //if (data.UserStatus && data.ExpiryDate >= strDate)
            //{
            //    $("#chkStatus").attr("checked", false);
            //}
            //else
            //{
            //    $("#chkStatus").attr("checked", true);
            //}
            
            if (data.UserStatus)
            {
                $("#chkStatus").attr("checked", true);
            }
            else
            {
                $("#chkStatus").attr("checked", false);
            }
            var hdnAccessMode = $("#hdnAccessMode");
            if (hdnAccessMode.val() == "3") {
                $("#btnRemoveImg").hide();
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
    var ddlRole = $("#ddlRole");
    var txtExpiryDate = $("#txtExpiryDate");

    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var chkStatus = $("#chkStatus");
    var UserType = $("#ddlUserType");
    var DealerId = $("#ddlDealer");


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
    if (ddlRole.val() == "" || ddlRole.val() == "0") {
        ShowModel("Alert", "Please select User Role")
        ddlRole.focus();
        return false;
    }
    if (txtExpiryDate.val().trim() == "") {
        ShowModel("Alert", "Please select User Expiry Date")
        txtExpiryDate.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    if (UserType.val() != "0" && (DealerId.val() == "" || DealerId.val() == "0")) {
        ShowModel("Alert", "Please select Dealer")
        return false;
    }


    var userStatus = true;

    var accessMode = 1;//Add Mode
    if (hdnUserID.val() != null && hdnUserID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    if (chkStatus.prop("checked") == true)
    { userStatus = true; }
    else
    { userStatus = false; }
        
    var userViewModel = {
        UserId: hdnUserID.val(), UserName: txtUserName.val().trim(), FullName: txtFullName.val().trim(),
        MobileNo: txtPhone.val().trim(), Email: txtEmail.val().trim(), Password: txtPassword.val().trim(), RoleId: ddlRole.val(),
        ExpiryDate: txtExpiryDate.val().trim(), CompanyBranchId: ddlCompanyBranch.val(), UserStatus: userStatus, FK_CustomerId: parseInt(DealerId.val())
    };
    var requestData = { userViewModel: userViewModel };
    $.ajax({
        url: "../User/AddEditUser?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status=="SUCCESS")
            {
                SaveUserImage(data.trnId);
                if ($("#hdnRemoveImage").val() == 1)
                {
                    RemoveImage();
                }
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
                function () {
                    window.location.href = "../User/ListUser";
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
     $("#txtUserName").val("");
     $("#hdnUserID").val("0");
     $("#hdnAccessMode").val("0");
     $("#txtFullName").val("");
     $("#txtEmail").val("");
     $("#txtPhone").val("");
     $("#txtPassword").val("");
     $("#txtConfirmPassword").val("");
     $("#ddlRole").val("0");
     $("#txtExpiryDate").val("");
     $("#btnRemoveImg").hide(); 
    $("#chkStatus").attr("checked", true);
    document.getElementById('FileUpload1').value = "";
    document.getElementById('imgUserPic').src = "";
    
}
function ShowImagePreview(input) {
    var fname = input.value;
    var ext = fname.split(".");
    var x = ext.length;
    var extstr = ext[x - 1].toLowerCase();
    if (extstr == 'jpg' || extstr == 'jpeg' || extstr == 'png' || extstr == 'gif') {
    }
    else {
        alert("File doesnt match png, jpg or gif");
        input.focus();
        input.value = "";
        return false;
    }
    if (typeof (FileReader) != "undefined") {
        if (input.files && input.files[0]) {
            if (input.files[0].name.length < 1) {
            }
            else if (input.files[0].size > 5000000) {
                input.files[0].name.length = 0;
                alert("File is too big");
                input.value = "";
                return false;
            }
            else if (input.files[0].type != 'image/png' && input.files[0].type != 'image/jpg' && !input.files[0].type != 'image/gif' && input.files[0].type != 'image/jpeg') {
                input.files[0].name.length = 0;
                alert("File doesnt match png, jpg or gif");
                input.value = "";
                return false;
            }
            var reader = new FileReader();
            reader.onload = function (e) {
                $("#imgUserPic").prop('src', e.target.result)
                    .width(150)
                    .height(150);
            };
            reader.readAsDataURL(input.files[0]);
            if ($("#FileUpload1").val() != '') {
                $("#btnRemoveImg").show();
            }
            else {
                $("#btnRemoveImg").hide();

            }
          

        }
    }
    else {
        alert("This browser does not support FileReader.");
        input.value = "";
        //return false;
    }
    
}

function ConfirmRemoveImage()
{
    if (confirm("Do you want to remove selected Image?")) {
        $("#hdnRemoveImage").val(1);
        document.getElementById('FileUpload1').value = "";
        document.getElementById('imgUserPic').src = "";
        $("#btnRemoveImg").hide();
    }
}
 

function RemoveImage() {  
            var hdnUserID = $("#hdnUserID");
            var requestData = { userId: hdnUserID.val() };
            $.ajax({
                url: "../User/RemoveImage",
                cache: false,
                type: "POST",
                dataType: "json",
                data: JSON.stringify(requestData),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status == "SUCCESS") {
                        ShowModel("Alert", data.message);
                        document.getElementById('FileUpload1').value = "";
                        document.getElementById('imgUserPic').src = "";
                        $("#btnRemoveImg").hide();
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
function SaveUserImage(userId) {
    if (parseInt(userId) <= 0)
    {
        ShowModel("Alert", "User ID not available")
        return false;
    }

    if (window.FormData !== undefined) {
        var uploadfile = document.getElementById('FileUpload1');
        var fileData = new FormData();
        if (uploadfile.value != '') {
            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;
            
            if (uploadfile.files[0].size > 5000000) {
                uploadfile.files[0].name.length = 0;
                ShowModel("Alert", "File is too big")
                uploadfile.value = "";
                return false;
            }

            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
        }
        fileData.append("userId", userId);

    } else {
        
        ShowModel("Alert", "FormData is not supported.")
    }

    $.ajax({
        url: "../User/UpdateUserPic",
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        error: function () {
            ShowModel("Alert", "An error occured")
            return;
        },
        success: function (result) {
            if (result.status == "SUCCESS") {
                document.getElementById('FileUpload1').value = "";
                document.getElementById('imgUserPic').src = "";
            }
            else {
                ShowModel("Alert", result.message);
            }
        }
    });
}
function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
}
//Add Company Branch In User InterFAce So Binding Process By Dheeraj
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

function BindCustomerTypeList() {
    $.ajax({
        type: "GET",
        url: "../Customer/GetCustomerTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlUserType").append($("<option></option>").val(0).html("-Select User Type-"));
            $.each(data, function (i, item) {
                if (item.CustomerTypeDesc.toLowerCase() == "dealer") {
                    $("#ddlUserType").append($("<option></option>").val(item.CustomerTypeId).html(item.CustomerTypeDesc));
                }
            });
        },
        error: function (Result) {
            $("#ddlUserType").append($("<option></option>").val(0).html("-Select User Type-"));
        }
    });
}

function BindDealer() {
    if ($("#ddlUserType").val() != "" && $("#ddlUserType").val() != "0") {
        BindCustomerByType(parseInt($("#ddlUserType").val()));
    }
    else {
        $("#ddlDealer").html('');
        $("#ddlDealer").append($("<option></option>").val(0).html("-Select Dealer-"));
        $("#dvDealer").hide();
    }
}
function BindCustomerByType(customerTypeId) {
    $.ajax({
        type: "GET",
        url: "../Customer/BindCustomerByType",
        data: { customerTypeId: customerTypeId },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlDealer").append($("<option></option>").val(0).html("-Select Dealer-"));
            $.each(data, function (i, item) {

                $("#ddlDealer").append($("<option></option>").val(item.CustomerId).html(item.CustomerName + "(" + item.CustomerCode+")"));
                
            });

            $("#dvDealer").show();
        },
        error: function (Result) {
            $("#ddlDealer").append($("<option></option>").val(0).html("-Select Dealer-"));
            $("#dvDealer").hide();
        }
    });
}
//End Code
document.onkeypress = stopRKey;