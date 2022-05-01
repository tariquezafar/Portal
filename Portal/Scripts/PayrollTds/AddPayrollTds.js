$(document).ready(function () {
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPayrollTdsid = $("#hdnPayrollTdsid");
    if (hdnPayrollTdsid.val() != "" && hdnPayrollTdsid.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetPayrollTdsDetails(hdnPayrollTdsid.val());
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
    
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
        }
    });
     
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


function GetPayrollTdsDetails(PayrollTdsid) {
    
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PayrollTdsSlab/GetPayrollTdsDetails",
        data: { PayrollTdsid: PayrollTdsid },
        dataType: "json",
        success: function (data) {
           
            $("#txtFromDate").val(data.FromDate);
            $("#txtToDate").val(data.ToDate);
            $("#txtFromAmount").val(data.FromAmount);
            $("#txtToAmount").val(data.ToAmount);
            $("#ddlCategory").val(data.Category);
            $("#txtTDSPercentage").val(data.TDSPerc);
            $("#txtCessPerc").val(data.CessPerc);
            $("#txtSurcharegePerc").val(data.SurcharegePerc);
            $("#txtSurchargePerc2").val(data.SurchargePerc2);
            $("#txtSurchargePerc3").val(data.SurchargePerc3);
            $("#txtYearlyDiscount").val(data.YearlyDiscount);
            $("#txtMonthlyDiscount").val(data.MonthlyDiscount); 
            $("#ddlCompanyBranch").val(data.CompanyBranchid);
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}



function SaveData() {
    var hdnPayrollTdsid = $("#hdnPayrollTdsid");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var txtFromAmount = $("#txtFromAmount");
    var txtToAmount = $("#txtToAmount");
    var ddlCategory = $("#ddlCategory");
    var txtTDSPercentage = $("#txtTDSPercentage");
    var txtCessPerc = $("#txtCessPerc");
    var txtSurcharegePerc = $("#txtSurcharegePerc");
    var txtSurchargePerc2 = $("#txtSurchargePerc2");
    var txtSurchargePerc3 = $("#txtSurchargePerc3");
    var txtYearlyDiscount = $("#txtYearlyDiscount");
    var txtMonthlyDiscount = $("#txtMonthlyDiscount");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (txtFromDate.val().trim() == "") {
         ShowModel("Alert", "Please Select From Date")
        txtFromDate.focus();
        return false;
    }
    if (txtToDate.val().trim() == "") {
        ShowModel("Alert", "Please Select To Date")
        txtToDate.focus();
        return false;
    }
    if (txtFromAmount.val().trim() == "") {
        ShowModel("Alert", "Please Enter From Amount")
        txtToAmount.focus();
        return false;
    }
    
    if (txtToAmount.val().trim() == "") {
        ShowModel("Alert", "Please Enter To Amount")
        txtToAmount.focus();
        return false;
    }
    if (ddlCategory.val() == "" || ddlCategory.val() == "0") {
        ShowModel("Alert", "Please Select Category")
        ddlCategory.focus();
        return false;
    }
    if (txtCessPerc.val().trim() == "") {
        ShowModel("Alert", "Please Enter Cess Percentage")
        txtCessPerc.focus();
        return false;
    }
    
    if (txtTDSPercentage.val().trim() == "") {
        ShowModel("Alert", "Please Enter TDS Percentage")
        txtTDSPercentage.focus();
        return false;
    }
   
    
    if (txtYearlyDiscount.val().trim() == "") {
        ShowModel("Alert", "Please Enter Yearly Discount")
        txtYearlyDiscount.focus();
        return false;
    }
    if (txtMonthlyDiscount.val().trim() == "") {
        ShowModel("Alert", "Please Enter Monthly Discount")
        txtMonthlyDiscount.focus();
        return false;
    }

    if (txtFromDate.val().trim() > txtToDate.val().trim()) {
        ShowModel("Alert", "From date can't be grater then to date")
        txtFromDate.focus();
        txtToDate.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }
    var SurcharegePerc = txtSurcharegePerc.val().trim() == "" ? "0" : txtSurcharegePerc.val().trim();
    var SurchargePerc2 = txtSurchargePerc2.val().trim() == "" ? "0" : txtSurchargePerc2.val().trim();
    var SurchargePerc3 = txtSurchargePerc3.val().trim() == "" ? "0" : txtSurchargePerc3.val().trim();
    var PayrollTdsSlabViewModel = {
        TdsSlabid: hdnPayrollTdsid.val(),
        Companyid:0,
        FromDate :txtFromDate.val().trim(),
        ToDate :txtToDate.val().trim(),
        FromAmount:txtFromAmount.val().trim(),
        ToAmount :txtToAmount.val().trim(),
        Category :ddlCategory.val(),
        TDSPerc :txtTDSPercentage.val().trim(),
        CessPerc :txtCessPerc.val().trim(),
        SurcharegePerc: SurcharegePerc,
        SurchargePerc2: SurchargePerc2,
        SurchargePerc3: SurchargePerc3,
        YearlyDiscount :txtYearlyDiscount.val(),
        MonthlyDiscount: txtMonthlyDiscount.val().trim(),
        CreatedBy:"",
        CreatedDate:"",
        Modifiedby:"",
        ModifiedDate: "",
        CompanyBranchid: ddlCompanyBranch.val()

   };
    
    var accessMode = 1;//Add Mode
    if (hdnPayrollTdsid.val() != null && hdnPayrollTdsid.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { payRollTdsViewModel: PayrollTdsSlabViewModel };
  
    $.ajax({
        url: "../PayrollTdsSlab/AddEditPayrollTdsSlab?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../PayrollTdsSlab/ListPayrollTds";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../PayrollTdsSlab/AddEditPayrollTdsSlab";
                    }, 2000);
                }
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

function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}

function ClearFields() {
    $("#hdnPayrollTdsid").val(0);
    $("#txtFromDate").val("");
    $("#txtToDate").val("");
    $("#txtFromAmount").val("");
    $("#txtToAmount").val("");
    $("#ddlCategory").val("");
    $("#txtTDSPercentage").val("");
    $("#txtCessPerc").val("");
    $("#txtSurcharegePerc").val("");
    $("#txtSurchargePerc2").val("");
    $("#txtSurchargePerc3").val("");
    $("#txtYearlyDiscount").val("");
    $("#txtMonthlyDiscount").val("");
}
