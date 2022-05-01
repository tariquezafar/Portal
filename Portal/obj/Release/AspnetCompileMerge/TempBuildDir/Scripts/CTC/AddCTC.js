$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnCTCId = $("#hdnCTCId");
    BindDesignationList();
    if (hdnCTCId.val() != "" && hdnCTCId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetCTCDetail(hdnCTCId.val());
        
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
    $("#txtClaimTypeName").focus(); 
   

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
 
function BindDesignationList() {
        $.ajax({
            type: "GET",
            url: "../CTCType/GetDesignationList",
            data: "{}",
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
                $.each(data, function (i, item) {
                    $("#ddlDesignation").append($("<option></option>").val(item.DesignationId).html(item.DesignationName));
                }); 
            },
            error: function (Result) {
                $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
            }
        });
    }
   
function GetCTCDetail(cTCId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../CTCType/GetCTCDetail",
        data: { cTCId: cTCId },
        dataType: "json",
        success: function (data) {

            
            $("#txtBasicPay").val(data.Basic);
            $("#txtHraPercentage").val(data.HRAPerc);
            $("#txtHRAAmount").val(data.HRAAmount);
            $("#txtConveyanceAllow").val(data.Conveyance);
            $("#txtMedicalAllow").val(data.Medical);
            $("#txtChildEduAllow").val(data.ChildEduAllow);
            $("#txtLTA").val(data.LTA);
            $("#txtSpecialAllow").val(data.SpecialAllow);
            $("#txtOtherAllow").val(data.OtherAllow);
            $("#txtGrossSalary").val(data.GrossSalary);
            $("#txtEmployeePF").val(data.EmployeePF);
            $("#txtEmployeeESI").val(data.EmployeeESI);
            $("#txtProfessionalTax").val(data.ProfessionalTax);
            $("#txtNetSalary").val(data.NetSalary);
            $("#txtEmployerPF").val(data.EmployerPF);
            $("#txtEmployerESI").val(data.EmployerESI);
            $("#txtMonthlyCTC").val(data.MonthlyCTC);
            $("#txtYearlyCTC").val(data.YearlyCTC);
           
            $("#ddlDesignation").val(data.DesignationId);
            if (data.CTC_Status == true) {
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
    var ddlDesignation = $("#ddlDesignation");
    var hdnCTCId = $("#hdnCTCId");
    var txtBasicPay = $("#txtBasicPay");
    var txtHraPercentage = $("#txtHraPercentage");
    var txtHRAAmount = $("#txtHRAAmount");
    var txtConveyanceAllow = $("#txtConveyanceAllow");
    var txtMedicalAllow = $("#txtMedicalAllow");
    var txtChildEduAllow = $("#txtChildEduAllow");
    var txtLTA = $("#txtLTA");
    var txtSpecialAllow = $("#txtSpecialAllow");
    var txtOtherAllow = $("#txtOtherAllow");
    var txtGrossSalary = $("#txtGrossSalary");
    var txtEmployeePF = $("#txtEmployeePF");
    var txtEmployeeESI = $("#txtEmployeeESI");
    var txtProfessionalTax = $("#txtProfessionalTax"); 
    var txtNetSalary = $("#txtNetSalary");
    var txtEmployerPF = $("#txtEmployerPF");
    var txtEmployerESI = $("#txtEmployerESI");
    var txtMonthlyCTC = $("#txtMonthlyCTC"); 
    var txtYearlyCTC = $("#txtYearlyCTC");
    
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (ddlDesignation.val() == "" || ddlDesignation.val() == "0") {
        ShowModel("Alert", "Please select Designation")
        ddlDesignation.focus();
        return false;
    } 
    
    if (txtBasicPay.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Basic Pay")
        txtBasicPay.focus();
        return false;
    }
    if (txtHraPercentage.val().trim() == "") {
        ShowModel("Alert", "Please Enter Hra Percentage")
        txtHraPercentage.focus();
        return false;
    }
    if (txtNetSalary.val().trim() == "") {
        ShowModel("Alert", "Please Enter Net Salary")
        txtNetSalary.focus();
        return false;
    }
    if (txtMonthlyCTC.val().trim() == "") {
        ShowModel("Alert", "Please Enter Monthly Salary")
        txtMonthlyCTC.focus();
        return false;
    }
    if (txtYearlyCTC.val().trim() == "") {
        ShowModel("Alert", "Please Enter Yearly CTC")
        txtYearlyCTC.focus();
        return false;
    }
    var cTCViewModel = {
        DesignationId:ddlDesignation.val(),
        CTCId: hdnCTCId.val(),
        Basic: txtBasicPay.val().trim(),
        HRAPerc: txtHraPercentage.val().trim(),
        HRAAmount: txtHRAAmount.val().trim(),
        Conveyance: txtConveyanceAllow.val().trim(),
        Medical: txtMedicalAllow.val().trim(),
        ChildEduAllow: txtChildEduAllow.val().trim(),
        LTA: txtLTA.val().trim(),
        SpecialAllow: txtSpecialAllow.val().trim(),
        OtherAllow: txtOtherAllow.val().trim(),
        GrossSalary: txtGrossSalary.val().trim(),
        EmployeePF: txtEmployeePF.val().trim(),
        EmployeeESI: txtEmployeeESI.val().trim(),
        ProfessionalTax: txtProfessionalTax.val().trim(),
        NetSalary: txtNetSalary.val().trim(),
        EmployerPF: txtEmployerPF.val().trim(),
        EmployerESI: txtEmployerESI.val().trim(),
        MonthlyCTC: txtMonthlyCTC.val().trim(),
        YearlyCTC: txtYearlyCTC.val().trim(),
        CTC_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnCTCId.val() != null && hdnCTCId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { cTCViewModel: cTCViewModel };
    $.ajax({
        url: "../CTCType/AddEditCTC?AccessMode=" + accessMode + "",
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
                $("#btnSave").show();
                $("#btnUpdate").hide();
                if(accessMode==2)
                {
                    setTimeout(
                  function () {
                      window.location.href = "../CTCType/ListCTC";
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
    $("#ddlDesignation").val("0");
    $("#txtBasicPay").val("");
    $("#txtHraPercentage").val("");
    $("#txtHRAAmount").val("");
    $("#txtConveyanceAllow").val("");
    $("#txtMedicalAllow").val("");
    $("#txtChildEduAllow").val("");
    $("#txtLTA").val("");
    $("#txtSpecialAllow").val("");
    $("#txtOtherAllow").val("");
    $("#txtGrossSalary").val("");
    $("#txtEmployeePF").val("");
    $("#txtEmployeeESI").val("");
    $("#txtMonthlyCTC").val("");
    $("#txtProfessionalTax").val("");
    $("#txtNetSalary").val("");
    $("#txtEmployerPF").val("");
    $("#txtEmployerESI").val("");
    $("#txtMonthlyCTC").val("");
    $("#txtYearlyCTC").val("");
    $("#chkstatus").prop("checked", true);
    
}
function CalculatateBasicPay() { 
   var basicpay = parseInt($("#txtBasicPay").val());
   var percentagecvalue = parseInt($("#txtHraPercentage").val());
   if (isNaN(basicpay) || isNaN(percentagecvalue)) {
       var basicpay = 0;
       var percentagecvalue = 0;
   }
   else {
       var percentageofbasicpay = (basicpay * percentagecvalue) / 100;
       var result = parseFloat(percentageofbasicpay);
       $("#txtHRAAmount").val(result);
   }
  
}

