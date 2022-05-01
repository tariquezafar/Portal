$(document).ready(function () {
    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnSettingId = $("#hdnSettingId");
    if (hdnSettingId.val() != "" && hdnSettingId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetUserEmailSettingDetail(hdnSettingId.val());

       }, 1000);
       if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").prop('disabled', true);
            $("#chkEnableSsl").prop('disabled', true);
            $("#ddlCompanyBranch").attr('readOnly', true);
            
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


    
    $("#txtUserName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../UserEmailSetting/GetUserEmailAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FullName, value: item.UserName, UserId: item.UserId, MobileNo: item.MobileNo,Email:item.Email };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtUserName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtUserName").val(ui.item.label);
            $("#hdnUserId").val(ui.item.UserId);
            $("#txtSmtpDisplayName").val(ui.item.label);
            $("#txtEmail").val(ui.item.value)
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtUserName").val("");
                $("#hdnUserId").val("");
                ShowModel("Alert", "Please select User from List")
            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.value + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};
});
$(".alpha-numeric-only").on("input", function () {
    var regexp = /[^a-zA-Z0-9]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});

function GetUserEmailSettingDetail(settingId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../UserEmailSetting/GetUserEmailSettingDetail",
        data: { settingId: settingId },
        dataType: "json",
        success: function (data) {
            $("#txtUserName").val(data.FullName);
            $("#txtEmail").val(data.SmtpUser);
            $("#txtSmtpServer").val(data.SmtpServer);
            $("#txtSmtpPort").val(data.SmtpPort);
            $("#txtSmtpDisplayName").val(data.SmtpDisplayName);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            if (data.UserStatus) {
                $("#chkStatus").attr("checked", true);
            }
            else {
                $("#chkStatus").attr("checked", false);
            }


        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

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
function SaveData() {
        var hdnSettingId = $("#hdnSettingId");
        var txtUserName = $("#txtUserName");
        var hdnUserId = $("#hdnUserId");
        var txtEmail = $("#txtEmail");
        var txtPassword = $("#txtPassword");
        var txtConfirmPassword = $("#txtConfirmPassword");
        var txtSmtpServer = $("#txtSmtpServer");
        var chkEnableSsl = $("#chkEnableSsl").is(':checked') ? true : false;
        var txtSmtpPort = $("#txtSmtpPort");
        var txtSmtpDisplayName = $("#txtSmtpDisplayName");
        var chkstatus = $("#chkStatus").is(':checked') ? true : false;
        var ddlCompanyBranch = $("#ddlCompanyBranch");

        if (txtUserName.val().trim() == "") {
            ShowModel("Alert", "Please Enter User Name")
            txtCompanyName.focus();
            return false;
        }
        if (txtEmail.val().trim() == "") {
            ShowModel("Alert", "Please Enter Email")
            txtEmail.focus();
            return false;
        }
        if (!ValidEmailCheck(txtEmail.val().trim())) {
            ShowModel("Alert", "Please Enter Valid Email Address")
            txtEmail.focus();
            return false;
        }
      
        if (txtPassword.val().trim() == "") {
            ShowModel("Alert", "Please Enter Password")
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
    
        if (txtSmtpServer.val().trim() == 0) {
            ShowModel("Alert", "Please Enter SMTP Server")
            txtSmtpServer.focus();
            return false;
        }
        if (txtSmtpPort.val().trim() == "") {
            ShowModel("Alert", "Please Enter SMTP Port")
            txtSmtpPort.focus();
            return false;
        }
        if (txtSmtpDisplayName.val().trim() == "") {
            ShowModel("Alert", "Please Enter SMTP Port")
            txtSmtpDisplayName.focus();
            return false;
        }

        if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
            ShowModel("Alert", "Please select Company Branch")
            return false;
        }


        var accessMode = 1;//Add Mode
        if (hdnSettingId.val() != null && hdnSettingId.val() != 0) {
            accessMode = 2;//Edit Mode
        }
   
        var userEmailSettingViewModel = {
            SettingId: hdnSettingId.val(),
            UserId: hdnUserId.val().trim(),
            SmtpUser: txtEmail.val().trim(),
            SmtpPass: txtPassword.val().trim(),
            SmtpServer: txtSmtpServer.val().trim(),
            EnableSsl: chkEnableSsl,
            SmtpPort: txtSmtpPort.val().trim(),
            SmtpDisplayName: txtSmtpDisplayName.val().trim(),
            UserStatus: chkstatus,
            CompanyBranchId: ddlCompanyBranch.val(),
         };
      
        var requestData = { userEmailSettingViewModel:userEmailSettingViewModel};
        $.ajax({
            url: "../UserEmailSetting/AddEditUserEmailSetting?accessMode=" + accessMode + "",
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
                        window.location.href = "../UserEmailSetting/ListUserEmailSetting";
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
        $("#hdnUserEmailSettingId").val("0");
        $("#txtUserName").val("");
        $("#hdnUserId").val("0");
        $("#txtEmail").val("");
        $("#txtPassword").val("");
        $("#txtConfirmPassword").val("");
        $("#txtSmtpServer").val("webmail.htssolutions.org");
        $("#chkEnableSsl").prop("checked", false);
        $("#txtSmtpPort").val("25");
        $("#txtSmtpDisplayName").val("erpsupport");
        $("#chkstatus").prop("checked", true);
        $("#txtSmtpServer").val("");
        $("#ddlCompanyBranch").val(0);
       
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