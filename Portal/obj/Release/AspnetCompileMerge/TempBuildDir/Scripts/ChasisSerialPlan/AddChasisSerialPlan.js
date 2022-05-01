$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtChasisSerialPlanNo").attr('readOnly', true);
    $("#txtChasisSerialPlanDate").attr('readOnly', true);
    $("#txtChasisFixedValue").attr('readOnly', true);
    $("#txtMotorFixedValue").attr('readOnly', true);
    $("#txtManufactureLocation").attr('readOnly', true);
    $("#txtLastIncrement").attr('readOnly', true);
    $("#txtLastMonth").attr('readOnly', true);
    $("#txtLastYear").attr('readOnly', true);
    //$("#btnSave").hide(); 
    BindCompanyBranchList();
    GetLastIncrmentNo();
    $("#txtChasisSerialPlanDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {

        }
    });
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnChasisSerialPlanID = $("#hdnChasisSerialPlanID");
    if (hdnChasisSerialPlanID.val() != "" && hdnChasisSerialPlanID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
          
           GetChasisSerialPlanDetail(hdnChasisSerialPlanID.val());
           var chasisPlanDetailProducts = [];
           GetChasisPlanDetailProductList(chasisPlanDetailProducts);
       }, 1000);

        setTimeout(
      function () {
          GetChasisModelProduct();
      }, 1100);



        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
           
            $("#ddlPrintOption").attr('disabled', false);

            setTimeout(
      function () {
          $(".txtQTY").prop('readOnly', true);
          $(".txtQTYNEW").prop('readOnly', true);
      }, 1200);
          
            $(".editonly").hide();
            $("#GenerateChasisMotor").hide();
           // $(".txtQTY").attr('readOnly', true);
         
            if ($(".editonly").hide()) {
                $('#lblWorkOrder').removeClass("col-sm-3 control-label").addClass("col-sm-4 control-label");
            }
        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
            $(".editonly").show();
        }
    }
    else {
        $("#btnSave").hide();
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


function GetChasisPlanDetailProductList(chasisPlanDetailProducts) {
    var hdnChasisSerialPlanID = $("#hdnChasisSerialPlanID");
    var requestData = { ChasisSerialPlanDetails: chasisPlanDetailProducts, chasisSerialPlanID: hdnChasisSerialPlanID.val() };
    $.ajax({
        url: "../ChasisSerialPlan/GetChasisSerialPlanProductList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#GenerateChasisMotroSerial").html("");
            $("#GenerateChasisMotroSerial").html(err);
        },
        success: function (data) {
            $("#GenerateChasisMotroSerial").html("");
            $("#GenerateChasisMotroSerial").html(data);           
        }
    });
}

function AddProduct(action) {
    var productEntrySequence = 0;
    var flag = true;
    var txtProductName = $("#txtProductName");
    var hdnFabricationDetailId = $("#hdnFabricationDetailId");
    var hdnProductId = $("#hdnProductId");
    var txtProductCode = $("#txtProductCode");
    var txtProductShortDesc = $("#txtProductShortDesc");
    var txtUOMName = $("#txtUOMName");
    var txtQuantity = $("#txtQuantity");
    var hdnSequenceNo = $("#hdnSequenceNo");

    if (txtProductName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Product Name")
        txtProductName.focus();
        return false;
    }
    if (hdnProductId.val().trim() == "" || hdnProductId.val().trim() == "0") {
        ShowModel("Alert", "Please select Product from list")
        hdnProductId.focus();
        return false;
    }
    if (txtQuantity.val().trim() == "" || txtQuantity.val().trim() == "0" || parseFloat(txtQuantity.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter Quantity")
        txtQuantity.focus();
        return false;
    }
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    var fabricationProductList = [];
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var fabricationDetailId = $row.find("#hdnFabricationDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var productCode = $row.find("#hdnProductCode").val();
        var productShortDesc = $row.find("#hdnProductShortDesc").val();
        var uomName = $row.find("#hdnUOMName").val();
        var quantity = $row.find("#hdnQuantity").val();
        
        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added!!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }

                var fabricationProduct = {
                    FabricationDetailId: fabricationDetailId,
                    SequenceNo:sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,
                    ProductShortDesc: productShortDesc,
                    UOMName: uomName,
                    Quantity: quantity
                  
                };
                fabricationProductList.push(fabricationProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
                
            }
            else if (hdnSequenceNo.val() == sequenceNo)
            {
                var fabricationProduct = {
                    FabricationDetailId: hdnFabricationDetailId.val(),
                    SequenceNo:hdnSequenceNo.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),
                    ProductShortDesc: txtProductShortDesc.val().trim(),
                    UOMName: txtUOMName.val().trim(),
                    Quantity: txtQuantity.val().trim()
                };
                fabricationProductList.push(fabricationProduct);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var fabricationProductAddEdit = {
            FabricationDetailId: hdnFabricationDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            ProductShortDesc: txtProductShortDesc.val().trim(),
            UOMName: txtUOMName.val().trim(),
            Quantity: txtQuantity.val().trim()
          };
        fabricationProductList.push(fabricationProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetFabricationProductList(fabricationProductList);
    }

}
function EditProductRow(obj) {
    var row = $(obj).closest("tr");
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var fabricationDetailId = $(row).find("#hdnFabricationDetailId").val();   
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var productShortDesc = $(row).find("#hdnProductShortDesc").val();
    var uomName = $(row).find("#hdnUOMName").val();
    var quantity = $(row).find("#hdnQuantity").val();
    
    $("#txtProductName").val(productName);
    $("#hdnFabricationDetailId").val(fabricationDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    $("#txtProductCode").val(productCode);
    $("#txtProductShortDesc").val(productShortDesc);
    $("#txtUOMName").val(uomName);
    $("#txtQuantity").val(quantity);
    
    
    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    
    ShowHideProductPanel(1);
}

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var fabricationDetailId = $(row).find("#hdnFabricationDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
    }
}

function GetChasisSerialPlanDetail(chasisSerialPlanID) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ChasisSerialPlan/GetChasisSerialPlanDetail",
        data: { chasisSerialPlanID: chasisSerialPlanID },
        dataType: "json",
        success: function (data) {
            $("#txtChasisSerialPlanNo").val(data.ChasisSerialPlanNo);
            $("#hdnChasisSerialPlanID").val(data.ChasisSerialPlanID);
            $("#txtChasisSerialPlanDate").val(data.ChasisSerialPlanDate);
            $("#ddlYear").val(data.ChasisYear);
            $("#ddlMonth").val(data.Month);
            $("#hdnYear").val(data.ChasisYear);
            $("#hdnMonth").val(data.Month);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            GetManufactorLocationCode();
            $("#txtLastIncrement").val(data.LastIncreamentNo);                  
            $("#ddlChasisSerialPlanStatus").val(data.ApprovalStatus);
            if (data.ApprovalStatus == "Final")
            {                
                $("#btnUpdate").hide();
                $("#GenerateChasisMotor").hide();
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);
                $("#ddlPrintOption").attr('disabled', false);
                setTimeout(
      function () {
          
          $(".txtQTY").prop('readOnly', true);
          $(".txtQTYNEW").prop('readOnly', true);
      }, 1200);
                
            }
            $("#btnGenerateCheckListPrint").show();
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
                GetManufactorLocationCode();
            }
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}

function checkedCarryForwardTrueOrFalse() {
    $('#tblProductList tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var hdnCarryForwardTrueORNot = $row.find("#hdnCarryForwardTrueORNot").val();
        var chkPrintChasis = $(this).find("#txtQTYNEW");
        if (hdnCarryForwardTrueORNot == "True") {
            chkPrintChasis.prop("readonly", true);
        }


    });
}

//--INSERT CODE FOR CHASIS SERIAL PLAN BY DHEERAJ KUMAR

function SaveData() {
    //GetTotalQuantity();
    var txtChasisSerialPlanNo = $("#txtChasisSerialPlanNo");
    var hdnChasisSerialPlanID = $("#hdnChasisSerialPlanID");
    var txtChasisSerialPlanDate = $("#txtChasisSerialPlanDate");
    var ddlYear = $("#ddlYear");
    var ddlMonth = $("#ddlMonth");
    var hdnYear = $("#hdnYear");
    var hdnMonth = $("#hdnMonth");
    var ddlCompanyBranch = $("#ddlCompanyBranch");   
    var ddlChasisSerialPlanStatus = $("#ddlChasisSerialPlanStatus");
    var txtManufactureLocation = $("#txtManufactureLocation");
    if (ddlYear.val() == "" || ddlYear.val() == "0") {
        ShowModel("Alert", "Please select Year.")
        ddlYear.focus();
        return false;
    }


    if (ddlMonth.val() == "" || ddlMonth.val() == "0") {
        ShowModel("Alert", "Please select Month.")
        ddlMonth.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Company Branch.")
        ddlCompanyBranch.focus();
        return false;
    }

    if (hdnYear.val() != "0" && hdnMonth.val() != "0")
    {
        if(hdnYear.val()!=ddlYear.val() || hdnMonth.val() !=ddlMonth.val() )
        {
            ShowModel("Alert", "Please Generate Chasis Serial first")            
            return false;
        }

    }


    if (txtManufactureLocation.val() == "") {
        ShowModel("Alert", "Manufacture Location Can't be Blank.")
        txtManufactureLocation.focus();
        return false;
    }


    var checkedFlag = true;
    var totalQuantity = 0;
    var chasisSerialplanModelProductList = [];
    $('#tblProductList tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var hdnChasisModelID = $row.find("#hdnChasisModelID").val();
        var hdnLastIncreamentNo = $row.find("#hdnLastIncreamentNo").val();
        var txtQTY = $row.find("#txtQTY").val();
        var hdnQtyValue = $row.find("#hdnQtyValue").val();
        if (hdnChasisModelID != undefined) {
            if (parseFloat(txtQTY) > 0) {
                totalQuantity += parseFloat(txtQTY);
               
            }
        }
        if (txtQTY == "")
        {
            txtQTY = 0;
        }
         if (parseFloat(txtQTY) != parseFloat(hdnQtyValue)) {
                checkedFlag = false;
         }

        if (hdnChasisModelID != undefined) {
            var chasisPlanModelProduct = {
                ChasisModelID: hdnChasisModelID,
                QtyProduced: txtQTY,
                LastIncreamentNo: parseFloat(txtQTY) + parseFloat(hdnLastIncreamentNo)
            };
            chasisSerialplanModelProductList.push(chasisPlanModelProduct);
        }
    });


    if (checkedFlag == false) {
        ShowModel("Alert", "Please Generate Chasis Serial first")
        return false;
    }
    var chasisSerialPlanViewModel = {
        ChasisSerialPlanID: hdnChasisSerialPlanID.val(),
        ChasisSerialPlanNo: txtChasisSerialPlanNo.val().trim(),
        ChasisSerialPlanDate: txtChasisSerialPlanDate.val().trim(),
        ChasisYear: ddlYear.val(),
        ChasisMonth: ddlMonth.val(),
        LastIncreamentNo: 0,
        CompanyBranchId:ddlCompanyBranch.val(),
        ApprovalStatus: ddlChasisSerialPlanStatus.val()      
    };
  
    var chasisSerialplanDetailProductList = [];
    $('#myTable tr:not(:has(th))').each(function (i, row) {     
        var currentRow = $(this).closest("tr");
        var chasisSerialNo = currentRow.find("td:eq(0)").text();
        var motorNo = currentRow.find("td:eq(1)").text();
        var hdnChasisModelID = currentRow.find("#hdnChasisModelID").val();

        if (chasisSerialNo != undefined) {
            var chasisSerialPlanDetailViewModel = {
                ChasisModelID: hdnChasisModelID,
                ChasisSerialNo: chasisSerialNo.trim(),
                MotorNo: motorNo.trim()
            };
            chasisSerialplanDetailProductList.push(chasisSerialPlanDetailViewModel);
        }
    });

    if (chasisSerialplanDetailProductList.length == 0) {
        ShowModel("Alert", "Please Generate at least one Chasis/Motor No.")
        return false;
    }
  
    var accessMode = 1;//Add Mode
    if (hdnChasisSerialPlanID.val() != null && hdnChasisSerialPlanID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    

    var requestData = { chasisSerialPlanViewModel: chasisSerialPlanViewModel, chasisSerialPlanDetailProducts: chasisSerialplanDetailProductList, ChasisSerialModelDetailViewModels: chasisSerialplanModelProductList };
    $.ajax({
        url: "../ChasisSerialPlan/AddEditChasisSerialPlan?accessMode=" + accessMode + "",
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
                       window.location.href = "../ChasisSerialPlan/AddEditChasisSerialPlan?chasisSerialPlanID=" + data.trnId + "&AccessMode=3";
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
//--END CODE
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields() {

    $("#txtChasisSerialPlanNo").val("");
    $("#hdnChasisSerialPlanID").val("0");
    $("#ddlYear").val("0");
    $("#ddlMonth").val("0");
    $("#txtChasisSerialPlanDate").val($("#hdnCurrentDate").val());
    $("#ddlChasisSerialPlanStatus").val("Final");
    $("#txtLastIncrement").val("");
    $("#hdnYear").val("0");
    $("#hdnMonth").val("0");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}

function OpenWorkOrderSearchPopup() {
    $("#SearchWordOrderModel").modal();

}

function GetChasisModelProduct() {
    var ddlYear = $("#ddlYear").val();

    if (ddlYear == "" || ddlYear == "0") {
        ShowModel("Alert", "Please select Year.")
        return false;
    }

    var hdnChasisSerialPlanID = $("#hdnChasisSerialPlanID").val();
    var chasisserialPlanStatus = $("#ddlChasisSerialPlanStatus").val();
    var ddlMonth = $("#ddlMonth").val();
    var requestData = { chasisSerialPlanID: hdnChasisSerialPlanID, status: chasisserialPlanStatus, year: ddlYear, month: ddlMonth};
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
            checkedCarryForwardTrueOrFalse();
        }
    });
}

var table = "";
var html = "";

function GetTotalQuantity() {
    var ddlYear = $("#ddlYear").val();
    var ddlMonth = $("#ddlMonth");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtManufactureLocation = $("#txtManufactureLocation"); 
    var txtChasisFixedValue = $("#txtChasisFixedValue");
    var txtMotorFixedValue = $("#txtMotorFixedValue");
   
    var count = 1;
    var tablecount = 0;
    var quantity = 0;
   
    var twoDiYear = ddlYear.slice(-2);

    if (ddlYear == "" || ddlYear == "0") {
        ShowModel("Alert", "Please select Year.")
        return false;
    }


    if (ddlMonth.val() == "" || ddlMonth.val() == "0") {
        ShowModel("Alert", "Please select Month.")
        ddlMonth.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Company Branch.")
        ddlCompanyBranch.focus();
        return false;
    }

    if (txtManufactureLocation.val() == "") {
        ShowModel("Alert", "Manufacture Location Can't be Blank.")
        txtManufactureLocation.focus();
        return false;
    }

    


    var quantityCount = 0;
    var checkedflag = true;
    tablecount = $("#tblProductList tr").length - 1;
    $('#tblProductList tr:not(:has(th))').each(function (i, row) {
        var chasisModelCode = "";
        var motorModelCode = "";
        var chasisMotorNo = "";
        var chasisSerialNo = "";
        if (count == 1) {         
            count = count + 1;
            quantity = 0
            $("#GenerateChasisMotroSerial").empty();
        }
        else {            
            count = count + 1;

        }
        var $row = $(row);
        var hdnChasisModelCode = $row.find("#hdnChasisModelCode").val();
        var hdnMotorModelCode = $row.find("#hdnMotorModelCode").val();
        var txtQTY = $row.find(".txtQTY").val();
        var chasisModelID = $row.find("#hdnChasisModelID").val();
        var lastIncrement = $row.find("#hdnLastIncreamentNo").val();        
        if (txtQTY != undefined) {

            if (txtQTY == "")
            {
                $row.find("#hdnQtyValue").val("0");
            }
            else {
                $row.find("#hdnQtyValue").val(txtQTY);
            }
           
            if (txtQTY > 0) {
                quantity = txtQTY;
                quantityCount = parseFloat(quantityCount) + parseFloat(txtQTY);
               
            }

            if(parseFloat(txtQTY)>9999)
            {
                checkedflag = false;
            }
        }
        if (hdnChasisModelCode != undefined && hdnMotorModelCode != undefined && txtQTY != "" && txtQTY != 0) {
            if (hdnChasisModelCode != "0" && hdnMotorModelCode != "0") {
                chasisModelCode = hdnChasisModelCode;
                var chasisSerialNo = (txtChasisFixedValue.val() + "" + ddlMonth.val() + "" + chasisModelCode + "" + twoDiYear + "" + txtManufactureLocation.val());
                motorModelCode = hdnMotorModelCode;
                if (motorModelCode == 1) {
                    //chasisMotorNo = (txtMotorFixedValue.val() + "" + motorModelCode + "" + twoDiYear);--- Not Need For AutoGenerated Motor No Only Fixed Value As per Discussion HAri Sir and Prabhaker Sir By Dheeraj 
                    chasisMotorNo = txtMotorFixedValue.val();//Only Fixed Values
                }
                else {

                    //chasisMotorNo = (txtMotorFixedValue.val() + "" + twoDiYear + "" + motorModelCode);--- Not Need For AutoGenerated Motor No Only Fixed Value As per Discussion HAri Sir and Prabhaker Sir By Dheeraj 
                    chasisMotorNo = txtMotorFixedValue.val();//Only Fixed Values
                }
                generateRow(quantity, chasisSerialNo, chasisMotorNo, lastIncrement, chasisModelID);

            }
        }

        else {
        }
        tablecount = tablecount - 1;
        if (tablecount == 0) {
            $('#GenerateChasisMotroSerial').append('<table id="myTable"><thead><tr><th style="text-align:center; background:rgba(0, 0, 0, 0) linear-gradient(#375CB0, #6e96e2) repeat scroll 0 0; padding:8px; color:#fff;">Chasis Serial No.</th><th style="text-align:center; background:rgba(0, 0, 0, 0) linear-gradient(#375CB0, #6e96e2) repeat scroll 0 0; padding:8px; color:#fff;">Moto No.</th></tr></thead><tbody>' + html + '</tbody></table>');
            html = "";
        }

    });

    if(quantityCount==0)
    {
        ShowModel("Alert", "Please Enter at least one model Quantity.")
        $("#myTable").remove();
        html = "";
        return false;
      
    }
    if (checkedflag == false) {
        ShowModel("Alert", "Quantity cannot be grater than 9999.")
        $("#myTable").remove();
        html = "";
        return false;

    }
    var hdnChasisSerialPlanID = $("#hdnChasisSerialPlanID");
    if(quantityCount!=0 && hdnChasisSerialPlanID.val()=="0" )
    {
        $("#btnSave").show();
    }
    if (quantityCount != 0 && hdnChasisSerialPlanID.val()!= "0") {
        $("#btnUpdate").show();
    }

}
function generateRow(quantity, chasisSerialNo, chasisMotorNo, lastIncrement, chasisModelID)
{ 
    $("#myTable").remove();
    $("#foo").append("<div id='GenerateChasisMotroSerial'>hello world</div>")
 
    for (i = 1; i <= quantity; i++) {


        var string = "" + (parseFloat(lastIncrement) + i);
        var pad = "0000";
        n = pad.substring(0, pad.length - string.length) + string;

        html += "<tr class='bar'>";        
        html += "<td>";
        html += "<input type='hidden' id='hdnChasisModelID' value='" + chasisModelID + "' /> " + chasisSerialNo + (n)
        html += "</td>";
        html += "<td>";
        // html += "" + chasisMotorNo + (n); Not Need For AutoGenerated Motor No Only Fixed Value As per Discussion HAri Sir and Prabhaker Sir By Dheeraj 
        html += "" + chasisMotorNo ;//--Only Fixed Values
        html += "</td>";
        html += "</tr>";
    }

    $("#hdnYear").val($("#ddlYear").val());
    $("#hdnMonth").val($("#ddlMonth").val());
   
}

function GetManufactorLocationCode() {
   
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ChasisSerialPlan/GetManufactorLocationCode",
        data: { companyBranchId: ddlCompanyBranch.val() },
        dataType: "json",
        success: function (data) {
            $("#txtManufactureLocation").val(data);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}
function GetLastIncrmentNo() {
    //var ddlYear = $("#ddlYear");
    //var ddlMonth = $("#ddlMonth");
    //var hdnChasisSerialPlanID = $("#hdnChasisSerialPlanID");

    //if (ddlYear.val() == "" || ddlYear.val() == "0") {
    //    ShowModel("Alert", "Please select Year.")
    //    ddlYear.focus();
    //    return false;
    //}
    //if (ddlMonth.val() == "" || ddlMonth.val() == "0") {
    //    ShowModel("Alert", "Please select Month.")
    //    ddlMonth.focus();
    //    return false;
    //}
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ChasisSerialPlan/GetLastIncrement",
        data: {},
        dataType: "json",
        success: function (data) {           
            $("#txtLastYear").val(data.ChasisYear);
            $("#txtLastMonth").val(data.Month);
            $("#txtLastIncrement").val(data.LastIncreamentNo);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
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

function GetQuantity() {
    $('#tblProductList tr').each(function (i, row) {
        var $row = $(row);
        var hdnQtyValue = $row.find("#hdnQtyValue").val();
        var txtQTY = $(row).find("#txtQTY");
        txtQTY.val(hdnQtyValue);

    });
}

function getCalCulateQTY() {
    $('#tblProductList tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var hdnCarryForwardQTY = $row.find("#hdnCarryForwardQTY").val();
        var txtQTYNEW = $(row).find("#txtQTYNEW").val();
        if (parseFloat(txtQTYNEW) > 0)
        {

        
        if (parseFloat(txtQTYNEW) > parseFloat(hdnCarryForwardQTY))
        {
            $(row).find(".txtQTY").val(parseFloat(txtQTYNEW) - parseFloat(hdnCarryForwardQTY).toFixed(1));
        }
        //else if (parseFloat(txtQTYNEW) <= parseFloat(hdnCarryForwardQTY)) {
        //    alert("Quantity Cannot be less than or Equal to Carry Forward Quantity.")
        //    $(row).find(".txtQTY").val(0);
        //    $(row).find(".txtQTYNEW").val(0);
        //    return false;
           
        //}
        }
        else {
            $(row).find(".txtQTY").val(0);
            $(row).find(".txtQTYNEW").val(0);
        }
        

    });
}
