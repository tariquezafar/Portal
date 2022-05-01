$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnLoanTypeID = $("#hdnLoanTypeID");
    if (hdnLoanTypeID.val() != "" && hdnLoanTypeID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetLoanTypeDetail(hdnLoanTypeID.val());
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
    $("#txtLoanTypeName").focus(); 
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


function GetLoanTypeDetail(loantypeId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../LoanType/GetLoanTypeDetail",
        data: { loantypeId: loantypeId },
        dataType: "json",
        success: function (data) {
            $("#txtLoanTypeName").val(data.LoanTypeName);
            $("#txtLoanInterestRate").val(data.LoanInterestRate);
            $("#txtInterestCalcOn").val(data.InterestCalcOn); 
            if (data.LoanType_Status == true) {
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
    var txtLoanTypeName = $("#txtLoanTypeName");
    var txtLoanInterestRate = $("#txtLoanInterestRate");
    var txtInterestCalcOn = $("#txtInterestCalcOn");
    var hdnLoanTypeID = $("#hdnLoanTypeID");
  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtLoanTypeName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Loan Type Name")
        txtLoanTypeName.focus();
        return false;
    }   
    if (txtLoanInterestRate.val().trim() == "") {
        ShowModel("Alert", "Please Enter Loan Interest Rate")
        txtLoanInterestRate.focus();
        return false;
    }
    if (txtInterestCalcOn.val().trim() == "") {
        ShowModel("Alert", "Please Enter Interest Calc On")
        txtInterestCalcOn.focus();
        return false;
    }
    var loantypeViewModel = { 
        LoanTypeId: hdnLoanTypeID.val(),
        LoanTypeName: txtLoanTypeName.val().trim(),
        LoanInterestRate: txtLoanInterestRate.val(),
        InterestCalcOn:txtInterestCalcOn.val().trim(),
        LoanType_Status: chkstatus
        
    };
   
    var accessMode = 1;//Add Mode
    if (hdnLoanTypeID.val() != null && hdnLoanTypeID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { loantypeViewModel: loantypeViewModel };
    $.ajax({
        url: "../LoanType/AddEditLoanType?accessMode=" + accessMode + "",
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
                      window.location.href = "../LoanType/ListLoanType";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../LoanType/AddEditLoanType?accessMode=1";
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
function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function ShowModel(headerText,bodyText)
{
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields()
{ 
    $("#txtLoanTypeName").val("");
    $("#txtLoanInterestRate").val("");
    $("#txtInterestCalcOn").val("");
    $("#chkstatus").prop("checked", true);
    
}
