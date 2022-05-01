
$(document).ready(function () {
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdntdssectionID = $("#hdntdssectionID");
    if (hdntdssectionID.val() != "" && hdntdssectionID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetTDSSectionDetail(hdntdssectionID.val());
        
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
    $("#txtCountryName").focus();
        


});


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


function GetTDSSectionDetail(tdsSectionId) {
        $.ajax({
        type: "GET",
        asnc:false,
        url: "../TDSSection/GetTDSsectionDetail",
        data: { tdsSectionId: tdsSectionId },
        dataType: "json",
        success: function (data) {
            $("#txtSectionName").val(data.SectionName);
            $("#txtSectionDescription").val(data.SectionDesc);
            $("#txtSectionMAXValue").val(data.SectionMaxValue); 
            $("#ddlCompanyBranch").val(data.CompanyBranch);
            if (data.TDSSetion_Status == true) {
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
   
    var txtSectionName = $("#txtSectionName");
    var hdntdssectionID = $("#hdntdssectionID");
    var txtSectionDescription = $("#txtSectionDescription");
    var txtSectionMAXValue = $("#txtSectionMAXValue");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (txtSectionName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Section Name")
        txtSectionName.focus();
        return false;
    }
    if (txtSectionDescription.val().trim() == "") {
        ShowModel("Alert", "Please Enter Section Description")
        txtSectionDescription.focus();
        return false;
    }
    if (txtSectionMAXValue.val().trim() == "") {
        ShowModel("Alert", "Please Enter Section MAX Value")
        txtSectionMAXValue.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }
    
    var tdsSectionViewModel = {
        SectionId: hdntdssectionID.val(),
        SectionName: txtSectionName.val().trim(),
        SectionDesc: txtSectionDescription.val().trim(),
        SectionMaxValue:txtSectionMAXValue.val(),
        TDSSetion_Status: chkstatus,
        CompanyBranch: ddlCompanyBranch.val()
        
    };

    var accessMode = 1;//Add Mode
    if (hdntdssectionID.val() != null && hdntdssectionID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { tdsSectionViewModel: tdsSectionViewModel };
    $.ajax({
        url: "../TDSSection/AddEditTDSSection?accessMode=" + accessMode + "",
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
                      window.location.href = "../TDSSection/ListTDSSection";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../TDSSection/AddEditTDSSection";
                    }, 2000);
                }
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
 
    $("#txtSectionName").val("");
    $("#txtSectionDescription").val("");
    $("#txtSectionMAXValue").val("");
    $("#chkstatus").prop("checked", true);
    
}
