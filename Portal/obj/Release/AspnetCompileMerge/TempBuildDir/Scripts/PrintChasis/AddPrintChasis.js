$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtPrintNo").attr('readOnly', true);
    $("#txtPrintDate").attr('readOnly', true);  
    BindCompanyBranchList();
    $("#txtPrintDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPrintID = $("#hdnPrintID");
    if (hdnPrintID.val() != "" && hdnPrintID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {          
           GetPrintChasisDetail(hdnPrintID.val());          
       }, 1000);

        var printChasisDetailProducts = [];
        GetPrintChasisProducts(printChasisDetailProducts);



        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            setTimeout(
      function () {
          $("#chkAllPrintChasis").attr('disabled', true);
          $(".indent").attr('disabled', true);
      }, 1200);
           
            $(".editonly").hide();           
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
    //var printChasisDetails = [];
    //GetPrintChasisProductList(printChasisDetails);
   
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

function GetPrintChasisProducts(printChasisDetailProducts) {
    var hdnPrintID = $("#hdnPrintID");
    var requestData = { ChasisSerialPlanDetails: printChasisDetailProducts, printID: hdnPrintID.val() };
    $.ajax({
        url: "../PrintChasis/GetPrintChasisProducts",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#DivChasisNo").html("");
            $("#DivChasisNo").html(err);
        },
        success: function (data) {
            $("#DivChasisNo").html("");
            $("#DivChasisNo").html(data);
            chkedChechbox();
        }
    });
}
function chkedChechbox() {
    $('#tblPrintChasis tr').each(function (i, row) {
        var $row = $(row);
        var hdnChked = $row.find("#hdnPrintStatus").val();
        var chkPrintChasis = $(this).find("#chkPrintChasis");
        if (hdnChked == 1) {
            chkPrintChasis.attr("checked", true)
        }


    });
}

function GetPrintChasisProductList(printChasisDetails) {
    var ddlMonth = $("#ddlMonth");
    if (ddlMonth.val() == "" || ddlMonth.val() == "0") {
        ShowModel("Alert", "Please Select Month.")
        $("#ddlCompanyBranch").val(0);
        ddlMonth.focus();
        return false;
    }

    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Comapany Branch.")
        ddlCompanyBranch.focus();
        return false;
    }    
    var requestData = { printChasisDetails: printChasisDetails, companyBranchId: ddlCompanyBranch.val(), chasisMonth: ddlMonth.val() };
    $.ajax({
        url: "../PrintChasis/GetPrintChasisProductList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#DivChasisNo").html("");
            $("#DivChasisNo").html(err);
        },
        success: function (data) {
            $("#DivChasisNo").html("");
            $("#DivChasisNo").html(data);
        }
    });
}
function GetPrintChasisDetail(printID) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PrintChasis/GetPrintChasisDetail",
        data: { printID: printID },
        dataType: "json",
        success: function (data) {
            $("#txtPrintNo").val(data.PrintNo);
            $("#hdnPrintID").val(data.PrintID);
            $("#txtPrintDate").val(data.PrintDate);           
            $("#ddlCompanyBranch").val(data.CompanyBranchId);                            
            $("#ddlPrintChasisStatus").val(data.ApprovalStatus);
            if (data.ApprovalStatus == "Final")
            {
                $("select").attr('disabled', true);
                $("#btnUpdate").hide();
                $("#btnReset").hide();
                $("#chkAllPrintChasis").attr('disabled', true);
                $(".indent").attr('disabled', true);
            }
            $("#btnPrint").show();
            $("#btnAddNew").show();
           
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
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
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}

//**********--INSERT CODE FOR Print Chasis BY DHEERAJ KUMAR--**********
function SaveData() {
    var txtPrintNo = $("#txtPrintNo");
    var hdnPrintID = $("#hdnPrintID");
    var txtPrintDate = $("#txtPrintDate");   
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlPrintChasisStatus = $("#ddlPrintChasisStatus");
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Company Branch.")
        ddlCompanyBranch.focus();
        return false;
    }
    var printChasisViewModel = {
        PrintID: hdnPrintID.val(),
        PrintNo: txtPrintNo.val().trim(),
        PrintDate: txtPrintDate.val().trim(),
        CompanyBranchId: ddlCompanyBranch.val(),
        ApprovalStatus: ddlPrintChasisStatus.val()
    };


    var checkedFlag = false;
    var printChasisDetailViewModel = [];
    $('#tblPrintChasis tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var hdnChasisSerialNo = $row.find("#hdnChasisSerialNo").val();
        var hdnMotorNo = $row.find("#hdnMotorNo").val();
        var chkPrintChasis = $row.find("#chkPrintChasis").is(':checked') ? true : false;            
        if (hdnChasisSerialNo != undefined) {
            if (chkPrintChasis == true)
            {
                checkedFlag = true;
                var printChasisproduct = {
                    ChasisSerialNo: hdnChasisSerialNo.trim(),
                    MotorNo: hdnMotorNo.trim()
                  
                };
                printChasisDetailViewModel.push(printChasisproduct);
            }         
          
        }
    });

    if (checkedFlag == false)
    {
        ShowModel("Alert", "Please Checked at least One chasis Serial No.")
        return false;
    }
    var accessMode = 1;//Add Mode
    if (hdnPrintID.val() != null && hdnPrintID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { printChasisViewModel: printChasisViewModel, printChasisDetailViewModel: printChasisDetailViewModel };
    $.ajax({
        url: "../PrintChasis/AddEditPrintChasis?accessMode=" + accessMode + "",
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
                       window.location.href = "../PrintChasis/AddEditPrintChasis?printID=" + data.trnId + "&AccessMode=3";
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
//*********--END CODE--************
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields() {
    $("#txtPrintNo").val("");
    $("#hdnPrintID").val("0");
    $("#ddlCompanyBranch").val("0");
    $("#txtPrintDate").val($("#hdnCurrentDate").val());    
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
function GetChasisModelProduct() {

    var hdnChasisSerialPlanID = $("#hdnChasisSerialPlanID").val();
    

    var requestData = { chasisSerialPlanID: hdnChasisSerialPlanID };
    $.ajax({
        url: "../ChasisSerialPlan/GetChasisModelProductList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divProductModel").html("");
            $("#divProductModel").html(err);
        },
        success: function (data) {
            $("#divProductModel").html("");
            $("#divProductModel").html(data);
            GetQuantity();
        }
    });
}
function OpenPrintPopup() {
    $("#printModel").modal();
    GenerateReportParameters();
}
function ShowHidePrintOption() {
    var reportOption = $("#ddlPrintOption").val();
    if (reportOption == "PDF") {
        $("#btnPdf").show();
        $("#btnExcel").hide();
    }
    else if (reportOption == "Excel") {
        $("#btnExcel").show();
        $("#btnPdf").hide();
    }
}
function GenerateReportParameters() {
    var url = "../ChasisSerialPlan/ChasisSerialPlanPrint?chasisSerialPlanID=" + $("#hdnChasisSerialPlanID").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    var url = "../ChasisSerialPlan/ChasisSerialPlanPrint?chasisSerialPlanID=" + $("#hdnChasisSerialPlanID").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

}
function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../PrintChasis/AddEditPrintChasis";
    }
}