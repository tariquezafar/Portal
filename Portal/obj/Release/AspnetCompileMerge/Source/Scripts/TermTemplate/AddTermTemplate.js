$(document).ready(function () {
    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnTermTemplateId = $("#hdnTermTemplateId");
    if (hdnTermTemplateId.val() != "" && hdnTermTemplateId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {

        GetTermTemplateDetail(hdnTermTemplateId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true);
            $(".editonly").hide();
            $("#ddlCompanyBranch").attr('readOnly', true);
        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
            $(".editonly").show();
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
        $(".editonly").show();
    }
    var termTemplates = [];
    GetTermTemplateDetailList(termTemplates); 
    $("#txtTermTemplateName").focus(); 
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
function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

} 
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
 

function GetTermTemplateDetail(termtemplateId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../TermsTemplate/GetTermTemplateDetail",
        data: { termtemplateId: termtemplateId },
        dataType: "json",
        success: function (data) {
            $("#txtTermTemplateName").val(data.TermTempalteName);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);

            if (data.TermTemplate_Status == true) {
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
 

function GetTermTemplateDetailList(termTemplates) {
    var hdnTermTemplateId = $("#hdnTermTemplateId");
    var requestData = { termTemplates: termTemplates, termtemplateId: hdnTermTemplateId.val() };
    $.ajax({
        url: "../TermsTemplate/GetTermTemplateDetailList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divTermTemplateList").html("");
            $("#divTermTemplateList").html(err);
        },
        success: function (data) {
            $("#divTermTemplateList").html("");
            $("#divTermTemplateList").html(data);
            ClearTermTemplateFields();
            $("#showTermTemplate").hide();
        }
    });
}

function ClearTermTemplateFields() {
    var hdnAccessMode = $("#hdnAccessMode");
    $("#hdnTrnId").val("0");
    $("#ddlCompanyBranch").val("0");
    $("#txtTermTemplateDesc").val("");
    $("#btnAddTermTemplateDesc").show();
    $("#btnAddTermTemplateDetail").show();
    if (hdnAccessMode.val() == "3") {
        $(".editonly").hide();
    }
    $("#btnUpdateTermTemplateDetail").hide();
} 
function AddTermTemplate(action) {
    var termEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtTermTemplateDesc = $("#txtTermTemplateDesc");

    var hdnTrnId = $("#hdnTrnId");
    var chkStatusDesc = $("#chkStatusDesc").is(':checked') ? true : false;
    if (txtTermTemplateDesc.val().trim() == "") {
        ShowModel("Alert", "Please Enter Term Template Desc")
        txtTermTemplateDesc.focus();
        return false;
    }
    
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        termEntrySequence = 1;
    }
  

    var termTemplateDetailList = [];
    $('#tblTermTemplateDetailList tr').each(function (i, row) {
        var $row = $(row);
      
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var trnId = $row.find("#hdnTrnId").val();
        var hdnTermStatus = $row.find("#hdnTermStatus").val() == "Active" ? true : false;
       
        var termtemplateDesc = $row.find("#hdnTermTemplateDesc").val(); 
        if (trnId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {   
                var termTemplate = { 
                    TrnId: trnId,
                    SequenceNo: sequenceNo,
                    TermsDesc: termtemplateDesc,
                    Term_Status: hdnTermStatus
                };
                termTemplateDetailList.push(termTemplate);
                termEntrySequence = parseInt(termEntrySequence) + 1;
            }
            else if (hdnTrnId.val() == trnId && hdnSequenceNo.val() == sequenceNo) {
                var termTemplate = { 
                    SequenceNo: hdnSequenceNo.val(), 
                    TrnId: hdnTrnId.val(),
                    TermsDesc: txtTermTemplateDesc.val().trim(),
                    Term_Status: chkStatusDesc

                };
                termTemplateDetailList.push(termTemplate);
                hdnSequenceNo.val("0");
            } 

            }
         
    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(termEntrySequence);
    }
    if (action == 1) {
        var termTemplateAddEdit = {
            SequenceNo: hdnSequenceNo.val(),
            TrnId: hdnTrnId.val(),
             
            TermsDesc: txtTermTemplateDesc.val().trim(),
            Term_Status: chkStatusDesc
        };
        termTemplateDetailList.push(termTemplateAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetTermTemplateDetailList(termTemplateDetailList);
    }
}
function EditTermTemplateRow(obj) { 
    var row = $(obj).closest("tr"); 
    var trnId = $(row).find("#hdnTrnId").val();
    var termtemplateDesc = $(row).find("#hdnTermTemplateDesc").val();
    var hdnTermStatus = $(row).find("#hdnTermStatus").val() == "Active" ? true : false;
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    $("#txtTermTemplateDesc").val(termtemplateDesc);
    $("#hdnTrnId").val(trnId);
    $("#hdnSequenceNo").val(sequenceNo);
    if (hdnTermStatus == true) {
       // $("#chkStatusDesc").is(':checked') = true;
        $('#chkStatusDesc').prop('checked', true);
    }
    else {
       // $("#chkStatusDesc").is(':checked') = false;
        $('#chkStatusDesc').prop('checked', false);
    }
    
    $("#showTermTemplate").show();
    $("#btnAddTermTemplateDetail").hide();
    $("#btnUpdateTermTemplateDetail").show();
}

function RemoveTermTemplateRow(obj) {
    if (confirm("Do you want to remove selected Term Template Desc?")) {
        var row = $(obj).closest("tr");
        var trnId = $(row).find("#hdnTrnId").val();

        $.ajax({
            type: "POST",
            url: "../TermsTemplate/RemoveTermTemplate",
            data: { trnId: trnId }
        }).success(function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                row.remove();
            }
            else {
                ShowModel("Error", data.message)
            }

        }).error(function (err) {

            ShowModel("Error", err)
        });
    }
}
function ClearTermTemplateDetailFields() {
    $("#hdnTrnId").val("0");
    $("#hdnTermTemplateDesc").val("");
    $("#btnAddTermTemplateDetail").show();
    $("#btnUpdateTermTemplateDetail").hide();
  
}

function HideTermTemplateFields() {
    $("#showTermTemplate").hide();
}
function GetCustomerDetail(customerId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Customer/GetCustomerDetail",
        data: { customerId: customerId },
        dataType: "json",
        success: function (data) {
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtCustomerName").val(data.CustomerName);
            $("#txtContactPersonName").val(data.ContactPersonName);
            $("#txtDesignation").val(data.Designation);
            $("#txtEmail").val(data.Email);
            $("#txtMobileNo").val(data.MobileNo);
            $("#txtContactNo").val(data.ContactNo);
            $("#txtFax").val(data.Fax);
            $("#txtPrimaryAddress").val(data.PrimaryAddress);
            $("#txtCity").val(data.City);
            $("#ddlCountry").val(data.CountryId);
            BindPrimaryStateList(data.StateId);
            $("#ddlState").val(data.StateId);
            $("#txtPinCode").val(data.PinCode);
            $("#txtCSTNo").val(data.CSTNo);
            $("#txtTINNo").val(data.TINNo);
            $("#txtPANNo").val(data.PANNo);
            $("#txtGSTNo").val(data.GSTNo);
            $("#txtExciseNo").val(data.ExciseNo);
            $("#ddlCustomerType").val(data.CustomerTypeId);
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#txtEmployeeName").val(data.EmployeeName);
            $("#txtCreditLimit").val(data.CreditLimit);
            $("#txtCreditDays").val(data.CreditDays);

            if (data.Customer_Status == true) {
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
    var txtTermTemplateName = $("#txtTermTemplateName");
    var hdnTermTemplateId = $("#hdnTermTemplateId");
    var chkStatus = $("#chkStatus").is(':checked') ? true : false;
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (txtTermTemplateName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Term Template Name")
        txtTermTemplateName.focus();
        return false;
    }
     
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    var termtemplateViewModel = {
        TermTemplateId: hdnTermTemplateId.val(),
        TermTempalteName: txtTermTemplateName.val().trim(),
        TermTemplate_Status: chkStatus,
        CompanyBranchId: ddlCompanyBranch.val()
    }; 

    var termTemplateDetailList = [];
    $('#tblTermTemplateDetailList tr').each(function (i, row) {
        var $row = $(row);
        var trnId = $row.find("#hdnTrnId").val();
        var hdnTermStatus = $row.find("#hdnTermStatus").val() == "Active" ? true : false;
        var termtemplateDesc = $row.find("#hdnTermTemplateDesc").val();
        if (trnId != undefined) { 
            var termTemplate = {  
                    TrnId: trnId,
                    TermsDesc: termtemplateDesc,
                    Term_Status: hdnTermStatus
                };
                termTemplateDetailList.push(termTemplate);
 
        }
    });

    var accessMode = 1;//Add Mode
    if (hdnTermTemplateId.val() != null && hdnTermTemplateId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { termtemplateViewModel: termtemplateViewModel, termtemplateDetail: termTemplateDetailList };
    $.ajax({
        url: "../TermsTemplate/AddEditTermTemplate?accessMode=" + accessMode + "",
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
                   window.location.href = "../TermsTemplate/ListTermTemplate";
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
    $("#txtTermTemplateName").val("");
    $("#hdnTermTemplateId").val("0"); 
    $("#chkStatus").prop("checked", true);
    $("#btnSave").show();
    $("#btnUpdate").hide(); 
    $("#divTermTemplateList").html("");
    $("#ddlCompanyBranch").val("0");

}
function ClearAllFields() {
    $("#txtTermTemplateName").val("");
    $("#hdnTermTemplateId").val("0");
    $("#hdntrnId").val("0");
    $("#txtTermTemplateDesc").val(""); 
    $("#chkStatus").prop("checked", true);
    $("#chkStatusDesc").prop("checked", true);
    $("#showTermTemplate").hide();
  

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