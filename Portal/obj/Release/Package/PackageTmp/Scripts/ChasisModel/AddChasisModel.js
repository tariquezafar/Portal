
$(document).ready(function () { 
    BindProductSubGroupList();
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnChasisModelID = $("#hdnChasisModelID");
    if (hdnChasisModelID.val() != "" && hdnChasisModelID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetChasisModelDetail(hdnChasisModelID.val());
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
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
 
 
function BindProductSubGroupList() {
    $.ajax({
        type: "GET",
        url: "../ChasisModel/GetChasisModelSubGroupList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
            $.each(data, function (i, item) {
                $("#ddlProductSubGroup").append($("<option></option>").val(item.ProductSubGroupId).html(item.ProductSubGroupName));
            });
        },
        error: function (Result) {
            $("#ddlProductSubGroup").append($("<option></option>").val(0).html("-Select Sub Group-"));
        }
    });
}
 
 

function GetChasisModelDetail(chasisModelID) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ChasisModel/GetChasisModelDetail",
        data: { chasisModelID: chasisModelID },
        dataType: "json",
        success: function (data) {
            $("#txtChasisModelName").val(data.ChasisModelName);
            $("#ddlProductSubGroup").val(data.ProductSubGroupId);
            $("#txtChasisModelCode").val(data.ChasisModelCode);
            $("#txtMotorModelCode").val(data.MotorModelCode);
            $("#hdnChasisModelID").val(data.ChasisModelID);
           
            $("#ddlCompanyBranch").val(0);
            if (data.ChasisModelStatus == true) {
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

function SaveData() {
    var txtChasisModelName = $("#txtChasisModelName");
    var ddlProductSubGroup = $("#ddlProductSubGroup");
    var txtChasisModelCode = $("#txtChasisModelCode");
    var txtMotorModelCode = $("#txtMotorModelCode");      
    var hdnChasisModelID = $("#hdnChasisModelID");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (txtChasisModelName.val().trim() == "") {
        ShowModel("Alert", "Please enter Chasis Model Name")
        txtChasisModelName.focus();
        return false;
    }
    
    if (ddlProductSubGroup.val() == "0") {
        ShowModel("Alert", "Please Select Product Sub Group")
        ddlProductSubGroup.focus();
        return false;
    } 

    if (txtChasisModelCode.val().trim() == "") {
        ShowModel("Alert", "Please enter Chasis Model Code")
        txtChasisModelCode.focus();
        return false;
    }

    if (txtMotorModelCode.val().trim() == "") {
        ShowModel("Alert", "Please enter Motor Code")
        txtMotorModelCode.focus();
        return false;
    }

    var chasisModelViewModel = {
        ChasisModelID: hdnChasisModelID.val(),
        ProductSubGroupId:ddlProductSubGroup.val(),
        ChasisModelName: txtChasisModelName.val().trim(),
        ChasisModelCode: txtChasisModelCode.val().trim(),
        MotorModelCode:  txtMotorModelCode.val().trim(),     
        ChasisModelStatus: chkstatus,
        CompanyBranchId:0
    };

    var accessMode = 1;//Add Mode
    if (hdnChasisModelID.val() != null && hdnChasisModelID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { chasisModelViewModel: chasisModelViewModel };
    $.ajax({
        url: "../ChasisModel/AddEditChasisModel?accessMode=" + accessMode + "",
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
                window.location.href = "../ChasisModel/AddEditChasisModel";
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
    $("#hdnChasisModelID").val("0");
    $("#txtChasisModelName").val("");
    $("#txtChasisModelCode").val("");
    $("#txtMotorModelCode").val("");
    $("#ddlProductSubGroup").val("0");
    $("#chkstatus").prop("checked", true);

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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}
