$(document).ready(function () {
    $("#ddlEmailTemplateType").html("");
   
    BindEmailTemplateList();
    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnEmailTemplateId = $("#hdnEmailTemplateId");


    if (hdnEmailTemplateId.val() != "" && hdnEmailTemplateId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetEmailTemplateDetail(hdnEmailTemplateId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("#ddlCompanyBranch").attr('readOnly', true);
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").prop('disabled', true);
            $("#txtEditor").prop('disabled', true);
            statusbar
            
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
});

function BindEmailTemplateList() {
    
    $.ajax({
        type: "GET",
        url: "../EmailTemplate/GetEmailTemplateTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlEmailTemplateType").append($("<option></option>").val(0).html("-Select Email Template-"));
            $.each(data, function (i, item) {

                $("#ddlEmailTemplateType").append($("<option></option>").val(item.EmailTemplateTypeId).html(item.EmailTemplateName));
            });
        },
        error: function (Result) {
            $("#ddlEmailTemplateType").append($("<option></option>").val(0).html("-Select Email Template-"));
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
function GetEmailTemplateDetail(emailTemplateId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmailTemplate/GetEmailTemplateDetail",
        data: { emailTemplateId: emailTemplateId },
        dataType: "json",
        success: function (data) {
            $("#txtEmailTemplateName").val(data.EmailTemplateSubject);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#ddlEmailTemplateType").val(data.EmailTemplateTypeId);
            $("#txtEditor").Editor("setText", data.EmailTemplateDesc);
            //$("#txtEditor").Editor(data.EmailTemplateDesc);
            //var editorObj = $("#txtEditor").data('wysihtml5');
            //var editor = editorObj.editor;
            //editor.setValue(data.EmailTemplateDesc);
            //txtEditor.Editor("setText");
           
         
            if (data.EmailTemplateStatus == true) {
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

function SaveData() {
    var txtEmailTemplateName = $("#txtEmailTemplateName");
    var hdnEmailTemplateId = $("#hdnEmailTemplateId");
    var ddlEmailTemplateType = $("#ddlEmailTemplateType");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var txtEditor = $("#txtEditor");
    
    var chkstatus = $("#chkStatus").is(':checked') ? true : false;
    if (txtEmailTemplateName.val().trim() == "") {

        ShowModel("Alert", "Please enter Email Template Name")
        txtEmailTemplateName.focus();
        return false;
    }

    if (ddlEmailTemplateType.val().trim() == "0") {
        ShowModel("Alert", "Please select Email Template Type")
        ddlCountry.focus();
        return false;
    }
    if (txtEditor.Editor("getText") == "")
    {
        ShowModel("Alert", "Please Enter Email Description");
        txtEditor.focus();
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }


    var accessMode = 1;//Add Mode
    if (hdnEmailTemplateId.val() != null && hdnEmailTemplateId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var emailTemplateViewModel = {
        EmailTemplateId: hdnEmailTemplateId.val(),
        EmailTemplateSubject:txtEmailTemplateName.val().trim(),
        EmailTemplateTypeId: ddlEmailTemplateType.val(),
        EmailTemplateDesc:txtEditor.Editor("getText").trim(),
        EmailTemplateStatus: chkstatus,
        CompanyBranchId: ddlCompanyBranch.val(),
    };
    var requestData = { emailTemplateViewModel: emailTemplateViewModel };
    $.ajax({
        url: "../EmailTemplate/AddEditEmailTemplate?accessMode=" + accessMode + "",
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
                      window.location.href = "../EmailTemplate/ListEmailTemplate";
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
function ShowModel(headerText,bodyText)
{
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields() {
    $("#txtEmailTemplateName").val("");
    $("#hdnEmailTemplateId").val("0");
    $("#hdnAccessMode").val("0");
    $("#ddlEmailTemplateType").val("0");
    $("#txtEditor").Editor("setText", "");
    $("#chkStatus").prop('checked', false);
    $("#ddlCompanyBranch").val("0");

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