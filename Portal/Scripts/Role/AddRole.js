$(document).ready(function () {
    BindCountryList();
    BindCompanyBranchList();
    BindCustomerTypeList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnRoleId = $("#hdnRoleId");
    if (hdnRoleId.val() != "" && hdnRoleId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetRoleDetail(hdnRoleId.val());
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true); 
            $("#chkstatus").attr('disabled', true); 
            $("#chkisadmin").attr('disabled', true);
            $("#ddlCompanyBranch").attr('readOnly', true);
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
    $("#txtRoleName").focus();
 


});
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
function BindCountryList() {
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
 

function GetRoleDetail(roleId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../Role/GetRoleDetail",
        data: { roleId: roleId },
        dataType: "json",
        success: function (data) { 
            $("#txtRoleName").val(data.RoleName);
            $("#txtRoleDescription").val(data.RoleDesc);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            if (data.IsAdmin == true) {
                $("#chkisadmin").attr("checked", true);
            }
            else {
                $("#chkisadmin").attr("checked", false);
            }
            if (data.Role_Status == true) {
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

function SaveData()
{
    var txtRoleName = $("#txtRoleName");
    var hdnRoleId = $("#hdnRoleId");
    var txtRoleDescription = $("#txtRoleDescription");
    var chkisadmin = $("#chkisadmin").is(':checked') ? true : false;
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var UserTypeId = $("#ddlUserType");

    if (txtRoleName.val().trim() == ""){
        ShowModel("Alert","Please Enter Role Name")
        txtRoleName.focus();
        return false;
    }
    if (txtRoleDescription.val().trim() == "") {
        ShowModel("Alert", "Please enter Role Description")
        txtRoleDescription.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Location")
        return false;
    }

    var roleViewModel = {
        RoleId: hdnRoleId.val(),
        RoleName: txtRoleName.val().trim(), 
        RoleDesc: txtRoleDescription.val().trim(),
        IsAdmin: chkisadmin, 
        Role_Status: chkstatus,
        CompanyBranchId: ddlCompanyBranch.val(),
        UserTypeId: UserTypeId.val() == "" || UserTypeId.val() == "0" ? 0 : parseInt(UserTypeId.val())
    }; 

    var accessMode = 1;//Add Mode
    if (hdnRoleId.val() != null && hdnRoleId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData ={ roleViewModel: roleViewModel };
    $.ajax({
        url: "../Role/AddEditRole?accessMode=" + accessMode + "",
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
                      window.location.href = "../Role/ListRole";
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
    $("#hdnRoleId").val("0");
    $("#txtRoleName").val("");
    $("#txtRoleDescription").val("");
    $("#ddlCompanyBranch").val(0);
    
    $("#chkisadmin").prop("checked", false);
    $("#chkstatus").prop("checked", true);
     
}
function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
}
document.onkeypress = stopRKey;

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