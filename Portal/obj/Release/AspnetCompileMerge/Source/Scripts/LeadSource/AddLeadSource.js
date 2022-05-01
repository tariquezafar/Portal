$(document).ready(function () {
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnLeadSourceId = $("#hdnLeadSourceId");
    if (hdnLeadSourceId.val() != "" && hdnLeadSourceId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetLeadSourceDetail(hdnLeadSourceId.val());
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
    $("#txtLeadSourceName").focus();



});
//$(".alpha-only").on("keydown", function (event) {
//    // Allow controls such as backspace
//    var arr = [8, 16, 17, 20, 35, 36, 37, 38, 39, 40, 45, 46];



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


function GetLeadSourceDetail(leadsourceId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../LeadSource/GetLeadSourceDetail",
        data: { leadsourceId: leadsourceId },
        dataType: "json",
        success: function (data) {
            $("#txtLeadSourceName").val(data.LeadSourceName); 
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            if (data.LeadSource_Status == true) {
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
    var txtLeadSourceName = $("#txtLeadSourceName");
    var hdnLeadSourceId = $("#hdnLeadSourceId");
    var ddlCompanyBranch=$("#ddlCompanyBranch");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtLeadSourceName.val().trim() == "") {
        ShowModel("Alert", "Please enter Lead Source Name")
        txtLeadSourceName.focus();
        return false;
    }

    if (ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Comopany Branch")
        ddlCompanyBranch.focus();
        return false;
    }


   

    var leadsourceViewModel = {
        LeadSourceId: hdnLeadSourceId.val(),
        LeadSourceName: txtLeadSourceName.val().trim(),
        LeadSource_Status: chkstatus,
        CompanyBranchId:ddlCompanyBranch.val()

    };
    var accessMode = 1;//Add Mode
    if (hdnLeadSourceId.val() != null && hdnLeadSourceId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { leadsourceViewModel: leadsourceViewModel };
    $.ajax({
        url: "../LeadSource/AddEditLeadSource?accessMode=" + accessMode + "",
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
                    window.location.href = "../LeadSource/ListLeadSource";
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
    $("#hdnLeadSourceId").val("0");
    $("#txtLeadSourceName").val("");
    $("#chkstatus").prop("checked", true);

}
