$(document).ready(function () {
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnCostCenterId = $("#hdnCostCenterId");
    if (hdnCostCenterId.val() != "" && hdnCostCenterId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
       GetCostCenterDetail(hdnCostCenterId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            
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
    $("#txtCostCenterName").focus();
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


function GetCostCenterDetail(costcenterId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../CostCenter/GetCostCenterDetail",
        data: { costcenterId: costcenterId },
        dataType: "json",
        success: function (data) {
            $("#txtCostCenterName").val(data.CostCenterName);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            if (data.CostCenter_Status == true) {
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
    var txtCostCenterName = $("#txtCostCenterName");
    var hdnCostCenterId = $("#hdnCostCenterId");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (txtCostCenterName.val().trim() == "") {
        ShowModel("Alert", "Please enter Cost Center Name")
        txtCostCenterName.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "0" || ddlCompanyBranch.val() == "") {
        ShowModel("Alert", "Please Select Company Branch")
        ddlCompanyBranch.focus();
        return false;
    }
    var costcenterViewModel = {
        CostCenterId: hdnCostCenterId.val(),
        CostCenterName: txtCostCenterName.val().trim(),
        CompanyBranchId: ddlCompanyBranch.val(),
        CostCenter_Status: chkstatus
    };
    var accessMode = 1;//Add Mode
    if (hdnCostCenterId.val() != null && hdnCostCenterId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { costcenterViewModel: costcenterViewModel };
    $.ajax({
        url: "../CostCenter/AddEditCostCenter?accessMode=" + accessMode + "",
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
    $("#hdnCostCenterId").val("0");
    $("#txtCostCenterName").val(""); 
    $("#chkstatus").prop("checked", true);

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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
        }
    });
}
//End Code