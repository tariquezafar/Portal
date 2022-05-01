$(document).ready(function () {
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnGLId = $("#hdnGLId");
    if (hdnGLId.val() != "" && hdnGLId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
          function () {
              GetGLDetail(hdnGLId.val());
              //GetGLDetailOpening(glId, data.CompanyBranchId)

          }, 2000);
        
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkIsbook").attr('disabled', true);
            $("#chkIsbranchGL").attr('disabled', true);
            $("#chkIsDebtorGL").attr('disabled', true);
            $("#chkIsCreditorGL").attr('disabled', true);
            $("#chkIsTaxGL").attr('disabled', true);
            $("#chkIsbook").attr('disabled', true);
            $("#chkIsPostGL").attr('disabled', true);
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
 
});
  
BindGLMainGroupList();
BindSLTypeList(); 

function BindGLMainGroupList(GLMainGroupId) {
    var glType = $("#ddlGLType option:selected").val(); 
    $("#ddlGLMainGroupId").val(0);
    $("#ddlGLMainGroupId").html("");
    $("#ddlGLSubGroupId").val(0); 

    if (glType != undefined && glType != "" && glType != "0") {
        var data = { glType: glType };
        $.ajax({
            type: "GET",
            url: "../GLSubGroup/GetGLMainGroupTypeList",
            data: data,
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlGLMainGroupId").append($("<option></option>").val(0).html("-Select GLMainGroup-"));
                $.each(data, function (i, item) { 
                    $("#ddlGLMainGroupId").append($("<option></option>").val(item.GLMainGroupId).html(item.GLMainGroupName));
                });
                $("#ddlGLMainGroupId").val(GLMainGroupId); 
            },
            error: function (Result) {
                $("#ddlGLMainGroupId").append($("<option></option>").val(0).html("-Select GLMainGroup-"));
            }
        });
    }
    else { 
        $("#ddlGLMainGroupId").append($("<option></option>").val(0).html("-Select GL Sub Group-"));
    }
}

function BindGLSubGroupList(GLSubGroupId) { 
    var GLMainGroupId = $("#ddlGLMainGroupId option:selected").val(); 
    $("#ddlGLSubGroupId").val(0);
    $("#ddlGLSubGroupId").html(""); 
    if (GLMainGroupId != undefined && GLMainGroupId != "" && GLMainGroupId != "0") {
        var data = { MainGroupId: GLMainGroupId };
        $.ajax({
            type: "GET",
            url: "../GL/GetGLSubGroupList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlGLSubGroupId").append($("<option></option>").val(0).html("-Select GLSubGroup-"));
                $.each(data, function (i, item) {

                    $("#ddlGLSubGroupId").append($("<option></option>").val(item.GLSubGroupId).html(item.GLSubGroupName));
                });
                $("#ddlGLSubGroupId").val(GLSubGroupId);
            },
            error: function (Result) {
                $("#ddlGLSubGroupId").append($("<option></option>").val(0).html("-Select GLSubGroup-"));
            }
        });
    }
    else {

        $("#ddlGLSubGroupId").append($("<option></option>").val(0).html("-Select GL Sub Group-"));
    }

} 

function BindSLTypeList() {
    $.ajax({
        type: "GET",
        url: "../Voucher/GetSLTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $.each(data, function (i, item) {
                $("#ddlSLtypeId").append($("<option></option>").val(item.SLTypeId).html(item.SLTypeName));
            });
        },
        error: function (Result) {
            $("#ddlSLtypeId").append($("<option></option>").val(0).html("NA"));
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

 
function CalculateTotalOpeningBalance() {
    var openingBalanceDebit = $("#txtOpeningBalanceDebit").val();
    var openingBalanceCredit = $("#txtOpeningBalanceCredit").val();
    openingBalanceDebit = openingBalanceDebit == "" ? 0 : openingBalanceDebit;
    openingBalanceCredit = openingBalanceCredit == "" ? 0 : openingBalanceCredit;
    var openingBalance = parseFloat(openingBalanceDebit) - parseFloat(openingBalanceCredit);
    $("#txtOpeningBalance").val(openingBalance);
}

function GetGLDetail(glId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../GL/GetGLDetail",
        data: { GLId: glId },
        dataType: "json",
        success: function (data) {

            $("#hdnGLId").val(glId);
            $("#txtGLCode").val(data.GLCode);
            $("#txtGLHead").val(data.GLHead);
            $("#ddlGLType").val(data.GLType);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);

          

            BindGLMainGroupList(data.GLMainGroupId);
            $("#ddlGLMainGroupId").val(data.GLMainGroupId);
            
            BindGLSubGroupList(data.GLSubGroupId, data.GLMainGroupId);
            $("#ddlGLSubGroupId").val(data.GLSubGroupId);
            $("#ddlSLtypeId").val(data.SLTypeId);

            data.IsBookGL == true ? $("#chkIsbook").attr("checked", true) : $("#chkIsbook").attr("checked", false);
            
            data.IsBranchGL == true ? $("#chkIsbranchGL").attr("checked", true) : $("#chkIsbranchGL").attr("checked", false);
            data.IsDebtorGL == true ? $("#chkIsDebtorGL").attr("checked", true) : $("#chkIsDebtorGL").attr("checked", false);
            data.IsCreditorGL == true ? $("#chkIsCreditorGL").attr("checked", true) : $("#chkIsCreditorGL").attr("checked", false);
            data.IsTaxGL == true ? $("#chkIsTaxGL").attr("checked", true) : $("#chkIsTaxGL").attr("checked", false);
            data.IsPostGL == true ? $("#chkIsPostGL").attr("checked", true) : $("#chkIsPostGL").attr("checked", false);
            if (data.IsPostGL == true) {
                $("#chkIsPostGL").attr("disabled", false);
            }
            else {
                $("#chkIsPostGL").attr("disabled", true);
            }
          
            $("#txtOpeningBalanceDebit").val(data.OpeningBalanceDebit);
            $("#txtOpeningBalanceCredit").val(data.OpeningBalanceCredit);
            $("#txtOpeningBalance").val(data.OpeningBalance);

            if (data.GLStatus == true) {
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

function GetGLDetailOpening(glId, companyBranchId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../GL/GetGLDetailOpening",
        data: { GLId: glId, companyBranchId: companyBranchId },
        dataType: "json",
        success: function (data) {

            $("#hdnGLId").val(glId);
            $("#txtOpeningBalanceDebit").val(data.OpeningBalanceDebit);
            $("#txtOpeningBalanceCredit").val(data.OpeningBalanceCredit);
            $("#txtOpeningBalance").val(data.OpeningBalance);

          
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}



function BindGLMainGroupList(GLMainGroupId) {
    var glType = $("#ddlGLType option:selected").val();
    $("#ddlGLMainGroupId").val(0);
    $("#ddlGLMainGroupId").html("");
    $("#ddlGLSubGroupId").val(0);

    if (glType != undefined && glType != "" && glType != "0") {
        var data = { glType: glType };
        $.ajax({
            type: "GET",
            url: "../GLSubGroup/GetGLMainGroupTypeList",
            data: data,
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlGLMainGroupId").append($("<option></option>").val(0).html("-Select GL Main Group-"));
                $.each(data, function (i, item) {
                    $("#ddlGLMainGroupId").append($("<option></option>").val(item.GLMainGroupId).html(item.GLMainGroupName));
                });
                $("#ddlGLMainGroupId").val(GLMainGroupId);
            },
            error: function (Result) {
                $("#ddlGLMainGroupId").append($("<option></option>").val(0).html("-Select GL Main Group-"));
            }
        });
    }
    else {
        $("#ddlGLMainGroupId").append($("<option></option>").val(0).html("-Select GL Main Group-"));
    }
}


function BindGLSubGroupList(glSubGroupId, selectedGLMainGroupId) {

    var glMainGroupId = $("#ddlGLMainGroupId option:selected").val();
    if (glMainGroupId == undefined || glMainGroupId == 0)
    {
        glMainGroupId = selectedGLMainGroupId;

    }
    $("#ddlGLSubGroupId").val(0);
    $("#ddlGLSubGroupId").html("");
    if (glMainGroupId != undefined && glMainGroupId != "" && glMainGroupId != "0") {
        var data = { MainGroupId: glMainGroupId };
        $.ajax({
            type: "GET",
            url: "../GL/GetGLSubGroupList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlGLSubGroupId").append($("<option></option>").val(0).html("-Select GL Sub Group-"));
                $.each(data, function (i, item) {

                    $("#ddlGLSubGroupId").append($("<option></option>").val(item.GLSubGroupId).html(item.GLSubGroupName));
                });
                $("#ddlGLSubGroupId").val(glSubGroupId);
            },
            error: function (Result) {
                $("#ddlGLSubGroupId").append($("<option></option>").val(0).html("-Select GL Sub Group-"));
            }
        });
    }
    else {

        $("#ddlGLSubGroupId").append($("<option></option>").val(0).html("-Select GL Sub Group-"));
    }

}
function EnableDisablePostGLCheck(obj)
{ 
    var controlName=$(obj).attr("Id");
    
    switch (controlName)
    {
        case "chkIsbook":
            $("#chkIsPostGL").attr("disabled", true);
            $("#chkIsPostGL").attr("checked", false);
            break;
        case "chkIsDebtorGL":
            $("#chkIsPostGL").attr("disabled", false);
            $("#chkIsPostGL").attr("checked", false);
            break;
        case "chkIsCreditorGL":
            $("#chkIsPostGL").attr("disabled", false);
            $("#chkIsPostGL").attr("checked", false);
            break;
        case "chkIsbranchGL":
            $("#chkIsPostGL").attr("disabled", true);
            $("#chkIsPostGL").attr("checked", false);
            break;
        case "chkIsTaxGL":
            $("#chkIsPostGL").attr("disabled", true);
            $("#chkIsPostGL").attr("checked", false);
            break;
       
    }
    

}

function BindSLTypeList() {
    $.ajax({
        type: "GET",
        url: "../Voucher/GetSLTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $.each(data, function (i, item) {
                $("#ddlSLtypeId").append($("<option></option>").val(item.SLTypeId).html(item.SLTypeName));
            });
        },
        error: function (Result) {
            $("#ddlSLtypeId").append($("<option></option>").val(0).html("NA"));
        }
    });
}





function SaveData(action) {
    var hdnGLId = $("#hdnGLId");
    var txtGLCode = $("#txtGLCode");
    var txtGLHead = $("#txtGLHead");
    var ddlGLType = $("#ddlGLType");
    var ddlGLMainGroupId = $("#ddlGLMainGroupId");
    var ddlGLSubGroupId = $("#ddlGLSubGroupId");
    var ddlSLtypeId = $("#ddlSLtypeId");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var chkIsbook = $("#chkIsbook").is(":checked") ? true : false;
    var chkIsbranchGL = $("#chkIsbranchGL").is(":checked") ? true : false;
    var chkIsDebtorGL = $("#chkIsDebtorGL").is(":checked") ? true : false;
    var chkIsCreditorGL = $("#chkIsCreditorGL").is(":checked") ? true : false;
    var chkIsTaxGL = $("#chkIsTaxGL").is(":checked") ? true : false;
    var chkIsPostGL = $("#chkIsPostGL").is(":checked") ? true : false; 

   var txtOpeningBalanceDebit = $("#txtOpeningBalanceDebit");
   var txtOpeningBalanceCredit = $("#txtOpeningBalanceCredit");
   var txtOpeningBalance = $("#txtOpeningBalance");

   var chkstatus = $("#chkstatus").is(":checked") ? true : false;


   if (txtGLCode.val().trim() == "") {
        ShowModel("Alert", "Please enter GL Code")
        txtGLCode.focus();
        return false;
   }

   if (txtGLHead.val().trim() == "")
   {
       ShowModel("Alert", "Please enter GL Head")
       txtGLHead.focus();
       return false;
   }

   if (ddlGLType.val()== "0") {
       ShowModel("Alert", "Please Select GL Type")
       ddlGLType.focus();
       return false;
   }

   if (ddlGLMainGroupId.val() == "0") {
        ShowModel("Alert", "Please Select GL Main Group")
        ddlGLMainGroupId.focus();
        return false;
   }

   if (ddlGLSubGroupId.val() == "0") {
       ShowModel("Alert", "Please Select GL Sub Group")
       ddlGLSubGroupId.focus();
       return false;
   }
   
   
   //if (ddlCompanyBranch.val() == "0" || ddlCompanyBranch.val() == "") {
   //    ShowModel("Alert", "Please Select Company Branch")
   //    ddlCompanyBranch.focus();
   //    return false;
   //}


    var glViewModel = {
        GLId: hdnGLId.val(),
        GLCode : txtGLCode.val().trim(),
        GLHead:txtGLHead.val().trim(),
        GLType:ddlGLType.val(),
        GLSubGroupId:ddlGLSubGroupId.val(),
        SLTypeId:ddlSLtypeId.val(),
        IsBookGL:chkIsbook,
        IsBranchGL:chkIsbranchGL,
        IsDebtorGL:chkIsDebtorGL,
        IsCreditorGL:chkIsCreditorGL,
        IsTaxGL: chkIsTaxGL,
        IsPostGL:chkIsPostGL,
        GLStatus: chkstatus
    };

    var glDetailViewModel = {
        OpeningBalanceDebit: txtOpeningBalanceDebit.val(),
        OpeningBalanceCredit: txtOpeningBalanceCredit.val(),
        OpeningBalance: txtOpeningBalance.val(),
        CompanyBranchId:10004 
    };
    var accessMode = 1;//Add Mode
    if (hdnGLId.val() != null && hdnGLId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { glViewModel: glViewModel, glDetailViewModel: glDetailViewModel };
    $.ajax({
        url: "../GL/AddEditGL?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                if (accessMode == 2)
                {
                    window.location.href = "../GL/ListGL";
                }
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
    $("#hdnGLId").val("0");
    $("#txtGLCode").val("");
    $("#txtGLHead").val("");
    $("#ddlGLType").val("0");
    $("#ddlGLMainGroupId").val("0");
    $("#ddlGLSubGroupId").val("0");
    $("#ddlSLtypeId").val("0");

    $("#chkIsbook").prop("checked", false);
    $("#chkIsbranchGL").prop("checked", false);
    $("#chkIsDebtorGL").prop("checked", false);
    $("#chkIsCreditorGL").prop("checked",false);
    $("#chkIsTaxGL").prop("checked",false);
    $("#chkIsPostGL").prop("checked", false);
    
    $("#txtOpeningBalanceDebit").val("");
    $("#txtOpeningBalanceCredit").val("");
    $("#txtOpeningBalance").val("");

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