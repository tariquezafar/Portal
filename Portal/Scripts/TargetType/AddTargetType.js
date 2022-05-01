
$(document).ready(function () {
    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdntargetTypeID = $("#hdntargetTypeID");
    if (hdntargetTypeID.val() != "" && hdntargetTypeID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetTargetTypeDetail(hdntargetTypeID.val());
        
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true); 
            $("#chkstatus").attr('disabled', true);
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
    $("#txtCountryName").focus();
        


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


function GetTargetTypeDetail(targettypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../TargetType/GetTargetTypeDetail",
        data: { targettypeId: targettypeId },
        dataType: "json",
        success: function (data) {
            $("#txtTargetName").val(data.TargetName);
            $("#txtTargetDescription").val(data.TargetDesc);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
           
            if (data.Status == true) {
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
    var txtTargetName = $("#txtTargetName");
    var hdntargetTypeID = $("#hdntargetTypeID");
    var txtTargetDescription = $("#txtTargetDescription");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtTargetName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Target Type Name")
        txtTargetName.focus();
        return false;
    }
    if (txtTargetDescription.val().trim() == "") {
        ShowModel("Alert", "Please Enter Target Type Description")
        txtTargetDescription.focus();
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdntargetTypeID.val() != null && hdntargetTypeID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
     
    
    var targetTypeViewModel = {
        TargetTypeId: hdntargetTypeID.val(),
        TargetName: txtTargetName.val().trim(),
        TargetDesc: txtTargetDescription.val().trim(),
        TargetType_Status: chkstatus,
        CompanyBranchId: ddlCompanyBranch.val()      
    };
    var requestData = { targetTypeViewModel: targetTypeViewModel };
    $.ajax({
        url: "../TargetType/AddEditTargetType?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify( requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status=="SUCCESS")
            {
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
               function () {
                   window.location.href = "../TargetType/ListTargetType";
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
    $("#ddlCompanyBranch").val(0);
    $("#txtTargetName").val("");
    $("#txtTargetDescription").val("");
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}
