$(document).ready(function () {
    BindRoleList();
    BindCompanyBranchList();
   $("#ddlRole").focus();
 


});
function refreshPage() {
    window.location.reload();
}
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
function BindRoleList() {
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
function GetRoleMappingDetail() {
    var ddlRole = $("#ddlRole");
    var ddlModule = $("#ddlModule");
    var ddlTrnType = $("#ddlTrnType");
    //var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (ddlRole.val() == "0" || ddlRole.val() == "") {
        ShowModel("Alert", "Please Select Role")
        ddlRole.focus();
        return false;
    }
    if (ddlModule.val() == "0" || ddlModule.val() == "") {
        ShowModel("Alert", "Please Select Module")
        ddlModule.focus();
        return false;
    }
    if (ddlTrnType.val() == "0" || ddlTrnType.val() == "") {
        ShowModel("Alert", "Please Select Transaction Type")
        ddlTrnType.focus();
        return false;
    }

    //savedata:
    //    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
    //        ShowModel("Alert", "Please select Company Branch")
    //        return false;
    //    }

    var requestData = { interfaceType: ddlModule.val(), interfaceSubType: ddlTrnType.val(), roleId: ddlRole.val() };

    $.ajax({
        url: "../Role/GetRoleUIMapping",
        data: requestData,
        dataType: "html",
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

function SaveData()
{
    var ddlRole = $("#ddlRole");
    if (ddlRole.val() == "" || ddlRole.val() == "0") {
        ShowModel("Alert", "Please select Role")
        ddlRole.focus();
        return false;
    }

    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }


    var mappingStatus = false;

    var roleUIMappingList = [];
    //$('#tblRoleList > tr').each(function (row) {
    $('.mapping-list tr').each(function (i, row) {
        var $row = $(row);
        var interfaceId = $row.find("#hdnInterfaceId").val();
        var addAccess = $row.find("#chkAddAccess").is(':checked') ? true : false;
        var editAccess = $row.find("#chkEditAccess").is(':checked') ? true : false;
        var viewAccess = $row.find("#chkViewAccess").is(':checked') ? true : false;
        var cancelAccess = $row.find("#chkCancelAccess").is(':checked') ? true : false;
        var reviseAccess = $row.find("#chkReviseAccess").is(':checked') ? true : false;
        //var companyBranchId = $row.find("#ddlCompanyBranch").val();
        //var companyBranchId = $row.find("#ddlCompanyBranch").is(':checked') ? true : false;
        //var interfaceId = $('#tblRoleList > tr:eq(' + row + ')').find("#hdnInterfaceId").val();
        //var addAccess =  $('#tblRoleList > tr:eq(' + row + ')').find("#chkAddAccess").is(':checked') ? true : false;
        //var editAccess = $('#tblRoleList > tr:eq(' + row + ')').find("#chkEditAccess").is(':checked') ? true : false;
        //var viewAccess = $('#tblRoleList > tr:eq(' + row + ')').find("#chkViewAccess").is(':checked') ? true : false;
        
        if (interfaceId !=undefined) {
                var roleUIMapping = {
                    RoleId: ddlRole.val(),
                    InterfaceId: interfaceId,
                    AddAccess: addAccess,
                    EditAccess: editAccess,
                    ViewAccess: viewAccess,
                    cancelAccess: cancelAccess,
                    reviseAccess: reviseAccess,
                    CompanyBranchId: ddlCompanyBranch.val()
                };
                roleUIMappingList.push(roleUIMapping);
                mappingStatus = true;
            }
        });
    if (mappingStatus == false)
        {
            ShowModel("Alert", "Please map atleast one UI permission")
            return false;
        }
        var requestData ={ roleUIMappingList: roleUIMappingList };
    $.ajax({
        url: "../Role/AddEditRoleUIMapping",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.status=="SUCCESS")
            {
                ShowModel("Alert", data.message);
               // ClearFields();
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
    $("#hdnRoleUIMappingId").val("0");
    $("#ddlRole").val("0");
    $("#ddlModule").val("0");
    $("#ddlTrnType").val("0");
    $('input:checkbox').prop('checked', false);
    $("#ddlCompanyBranch").val("0");

     
}
function CheckAllAddAccess(obj)
{
    $('.AddAccess').prop('checked', obj.checked);
    
}
function CheckAllEditAccess(obj) {
    $('.EditAccess').prop('checked', obj.checked);

}
function CheckAllViewAccess(obj) {
    $('.ViewAccess').prop('checked', obj.checked);

} 
function CheckAllCancelAccess(obj) {
  $('.CancelAccess').prop('checked', obj.checked);
}
function CheckAllReviseAccess(obj) {
    $('.ReviseAccess').prop('checked', obj.checked);

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