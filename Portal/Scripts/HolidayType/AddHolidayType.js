$(document).ready(function () {
    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnHolidayTypeID = $("#hdnHolidayTypeID");
    if (hdnHolidayTypeID.val() != "" && hdnHolidayTypeID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetHolidayTypeDetail(hdnHolidayTypeID.val());
        
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
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
    $("#txtHolidayTypeName").focus();
        


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


function GetHolidayTypeDetail(holidaytypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../HolidayType/GetHolidayTypeDetail",
        data: { holidaytypeId: holidaytypeId },
        dataType: "json",
        success: function (data) {
            $("#txtHolidayTypeName").val(data.HolidayTypeName);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            if (data.HolidayType_Status == true) {
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
    var txtHolidayTypeName = $("#txtHolidayTypeName");
    var hdnHolidayTypeID = $("#hdnHolidayTypeID");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtHolidayTypeName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Holiday Type Name")
        txtHolidayTypeName.focus();
        return false;
    }
  
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    var holidaytypeViewModel = { 
        HolidayTypeId: hdnHolidayTypeID.val(),
        HolidayTypeName: txtHolidayTypeName.val().trim(),
        HolidayType_Status: chkstatus,
        CompanyBranchId: ddlCompanyBranch.val()
    };
    var requestData = { holidaytypeViewModel: holidaytypeViewModel };
    var accessMode = 1;//Add Mode
    if (hdnHolidayTypeID.val() != null && hdnHolidayTypeID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    $.ajax({
        url: "../HolidayType/AddEditHolidayType?accessMode=" + accessMode + "",
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
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../HolidayType/ListHolidayType";
                  }, 2000);
                }
                else {
                    setTimeout(
                   function () {
                       window.location.href = "../HolidayType/AddEditHolidayType?accessMode=1";
                   }, 2000);
                }
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
    $("#txtHolidayTypeName").val(""); 
    $("#chkstatus").prop("checked", true);
    $("#ddlCompanyBranch").val("");
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