$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtCarryForwardNo").attr('readOnly', true);
    $("#txtCarryForwardDate").attr('readOnly', true);
    BindCompanyBranchList();
    $("#txtCarryForwardDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnCarryForwardID = $("#hdnCarryForwardID");
    if (hdnCarryForwardID.val() != "" && hdnCarryForwardID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {          
           GetCarryForwardChasisDetail(hdnCarryForwardID.val());
       }, 1000);

        var carryForwardChasisDetails = [];
        GetcarryForwardChasisProducts(carryForwardChasisDetails);



        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);       
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

function GetcarryForwardChasisProducts(carryForwardChasisDetails) {
    var hdnCarryForwardID = $("#hdnCarryForwardID");
    var requestData = { carryForwardChasisDetails: carryForwardChasisDetails, carryForwardID: hdnCarryForwardID.val() };
    $.ajax({
        url: "../CarryForwardChasis/GetCarryForwardChasisProducts",
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


function GetCarryForwardChasisProductList(carryForwardChasisDetails) {
    var PrevddlYear = $("#PrevddlYear");
    if (PrevddlYear.val() == "" || PrevddlYear.val() == "0") {
        ShowModel("Alert", "Please Select Previous Year.")
        PrevddlYear.focus();
        $("#PrevddlMonth").val("0");
        return false;
    }
    var PrevddlMonth = $("#PrevddlMonth");
    if (PrevddlMonth.val() == "" || PrevddlMonth.val() == "0") {
        ShowModel("Alert", "Please Select Previous Month.")
        PrevddlMonth.focus();
        return false;
    }
    var requestData = { carryForwardChasisDetails: carryForwardChasisDetails, prevYear: PrevddlYear.val(), prevMonth: PrevddlMonth.val() };
    $.ajax({
        url: "../CarryForwardChasis/GetCarryForwardChasisProductList",
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
function GetCarryForwardChasisDetail(carryForwardID) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../CarryForwardChasis/GetCarryForwardChasisDetail",
        data: { carryForwardID: carryForwardID },
        dataType: "json",
        success: function (data) {
            $("#txtCarryForwardNo").val(data.CarryForwardNo);
            $("#hdnCarryForwardID").val(data.CarryForwardID);
            $("#txtCarryForwardDate").val(data.CarryForwardDate);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#PrevddlYear").val(data.PrevChasisYear);
            $("#PrevddlMonth").val(data.ChasisMonth);
            $("#CarryddlYear").val(data.CarryForwardYear);
            $("#CarryddlMonth").val(data.CarryMonth);
            $("#ddlApprovalStatus").val(data.ApprovalStatus);
            if (data.ApprovalStatus == "Final")
            {                
                $("#btnUpdate").hide();
                $("#btnReset").hide();
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);
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

//**********--INSERT CODE FOR Carry Forward Chasis BY DHEERAJ KUMAR--**********
function SaveData() {
    var txtCarryForwardNo = $("#txtCarryForwardNo");
    var hdnCarryForwardID = $("#hdnCarryForwardID");
    var txtCarryForwardDate = $("#txtCarryForwardDate"); 
    var PrevddlYear = $("#PrevddlYear");
    var PrevddlMonth = $("#PrevddlMonth");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var CarryddlYear = $("#CarryddlYear");
    var CarryddlMonth = $("#CarryddlMonth");
    var ddlApprovalStatus = $("#ddlApprovalStatus");

    var twoDigatYear = CarryddlYear.val().slice(-2);

    if (PrevddlYear.val() == "" || PrevddlYear.val() == "0") {
        ShowModel("Alert", "Please Select Previous Year.")
        PrevddlYear.focus();
        return false;
    }
    if (PrevddlMonth.val() == "" || PrevddlMonth.val() == "0") {
        ShowModel("Alert", "Please Select Previous Month.")
        PrevddlMonth.focus();
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Comapany Branch.")
        ddlCompanyBranch.focus();
        return false;
    }
    if (CarryddlYear.val() == "" || CarryddlYear.val() == "0") {
        ShowModel("Alert", "Please Select Carry Forward Year.")
        CarryddlYear.focus();
        return false;
    }
    if (CarryddlMonth.val() == "" || CarryddlMonth.val() == "0") {
        ShowModel("Alert", "Please Select Carry Forward Month.")
        CarryddlMonth.focus();
        return false;
    }
    if (PrevddlYear.val() > CarryddlYear.val()) {
        ShowModel("Alert", "Carry Forward Year grater than or equal to Previous year.")
        PrevddlYear.focus();
        return false;
    }
    if (CarryddlYear.val() <= PrevddlYear.val())
    {
        if (PrevddlMonth.val() >= CarryddlMonth.val()) {
            ShowModel("Alert", "Carry Forward Month grater than Previous Month.")
            CarryddlMonth.focus();
            return false;
        }
    }
   
    



    
    var carryForwardChasisDetailViewModel = [];
    $('#tblCarryForward tr:not(:has(th))').each(function (i, row) {
        var firstStr = "";
        var secondstr = "";
        var thiredStr = "";
        var firstStrMotor = "";
        var secondStrMotor = "";
        var thiredStrMotor = "";
        var finalStrMotor = "";
        var $row = $(row);        
        var hdnChasisSerialDetailID = $row.find("#hdnChasisSerialDetailID").val();
        var hdnChasisModelID = $row.find("#hdnChasisModelID").val();
        var hdnChasisSerialNo = $row.find("#hdnChasisSerialNo").val();
        var hdnMotorNo = $row.find("#hdnMotorNo").val();

        if (hdnChasisSerialNo != undefined) {
            
                  firstStr = hdnChasisSerialNo.substring(0, 7);
                  secondstr = hdnChasisSerialNo.substring(9, 10);
                  thiredStr = hdnChasisSerialNo.substring(12, hdnChasisSerialNo.length);
                 
                  secondStrMotor = hdnMotorNo.substring(9, 10);
                  if (secondStrMotor=="S")
                  {        
                      firstStrMotor = hdnMotorNo.substring(0, 7);
                      thiredStrMotor = hdnMotorNo.substring(9, hdnMotorNo.length);
                      //finalStrMotor = firstStrMotor + "" + twoDigatYear + "" + thiredStrMotor;
                      finalStrMotor = firstStrMotor + "" + thiredStrMotor
                  }
                  else
                  {                     
                      firstStrMotor = hdnMotorNo.substring(0, 8);
                      thiredStrMotor = hdnMotorNo.substring(10, hdnMotorNo.length);
                      //finalStrMotor = firstStrMotor + "" + twoDigatYear + "" + thiredStrMotor;
                      finalStrMotor = firstStrMotor + "" + thiredStrMotor;
                  }

                 
                

                var carryForwardChasisproduct = {
                    ChasisSerialDetailID: hdnChasisSerialDetailID,
                    ChasisSerialDetailID:hdnChasisSerialDetailID,
                    ChasisModelID: hdnChasisModelID,
                    ChasisSerialNo: firstStr +""+CarryddlMonth.val()+""+ secondstr +""+twoDigatYear+""+thiredStr.trim(),
                    MotorNo: hdnMotorNo
                };
                carryForwardChasisDetailViewModel.push(carryForwardChasisproduct);
        }
    });
    if (carryForwardChasisDetailViewModel.length == 0) {
        ShowModel("Alert", "Chasis/Motor No not Available.")
        return false;
    }

    var carryForwardChasissViewModel = {
        CarryForwardID: hdnCarryForwardID.val(),
        CarryForwardNo: txtCarryForwardNo.val().trim(),
        CarryForwardDate: txtCarryForwardDate.val().trim(),    
        PrevChasisYear: PrevddlYear.val(),
        PrevChasisMonth: PrevddlMonth.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
        CarryForwardYear: CarryddlYear.val(),
        CarryForwardMonth: CarryddlMonth.val(),
        ApprovalStatus: ddlApprovalStatus.val()
    };
    var accessMode = 1;//Add Mode
    if (hdnCarryForwardID.val() != null && hdnCarryForwardID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { carryForwardChasissViewModel: carryForwardChasissViewModel, carryForwardChasisDetailViewModel: carryForwardChasisDetailViewModel };
    $.ajax({
        url: "../CarryForwardChasis/AddEditCarryForwardChasis?accessMode=" + accessMode + "",
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
                       window.location.href = "../CarryForwardChasis/AddEditCarryForwardChasis?carryForwardID=" + data.trnId + "&AccessMode=3";
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
    $("#txtCarryForwardNo").val("");
    $("#hdnCarryForwardID").val("0");
    $("#ddlCompanyBranch").val("0");
    $("#txtCarryForwardDate").val($("#hdnCurrentDate").val());
    $("#PrevddlYear").val("0");
    $("#PrevddlMonth").val("0");
    $("#CarryddlYear").val("0");
    $("#CarryddlMonth").val("0");
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
        window.location.href = "../CarryForwardChasis/AddEditCarryForwardChasis";
    }
}
function selectCarryForwardDropDownlist() {
    var prevddlMonth = $("#PrevddlMonth").val();
    if (parseFloat(prevddlMonth) > 0 && parseFloat(prevddlMonth) < 9)
    {
        $("#CarryddlMonth").val("0" + (parseFloat(prevddlMonth) + 1));
        $("#CarryddlMonth").prop("disabled", true);
    }
    else if (parseFloat(prevddlMonth) > 8 && parseFloat(prevddlMonth) < 12)
    {
        $("#CarryddlMonth").val((parseFloat(prevddlMonth) + 1));
        $("#CarryddlMonth").prop("disabled", true);
    }
    else {
        $("#CarryddlMonth").val( "0"+ 1);
        $("#CarryddlMonth").prop("disabled", true);
    }
}