$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtTargetNo").attr('readOnly', true);
    $("#txtTargetDate").attr('readOnly', true);
    $("#txtTargetFromDate").attr('readOnly', true);
    $("#txtTargetToDate").attr('readOnly', true);
    $("#txtDesignation").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtVehicles").attr('readOnly', true);
    $("#txtDealershipsNos").attr('readOnly', true);
    $("#txtPerDealar").attr('readOnly', true);

    $("#txtProductName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Product/GetProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode, CGST_Perc: item.CGST_Perc, SGST_Perc: item.SGST_Perc, IGST_Perc: item.IGST_Perc, HSN_Code: item.HSN_Code, IsSerializedProduct: item.IsSerializedProduct };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            $("#hdnProductId").val(ui.item.value);
            $("#txtProductShortDesc").val(ui.item.desc);
            $("#txtProductCode").val(ui.item.code);
            $("#txtHSN_Code").val(ui.item.HSN_Code);
            $("#hdnIsSerializedProduct").val(ui.item.IsSerializedProduct);
            var ddlSState = $("#ddlSState");
            var hdnBillingStateId = $("#hdnBillingStateId");
            if (ddlSState.val() == hdnBillingStateId.val()) {
                $("#txtCGSTPerc").val(ui.item.CGST_Perc);
                $("#txtSGSTPerc").val(ui.item.SGST_Perc);
                $("#txtIGSTPerc").val(0);
                $("#txtCGSTPercAmount").val(0);
                $("#txtSGSTPercAmount").val(0);
                $("#txtIGSTPercAmount").val(0);
            }
            else {
                $("#txtCGSTPerc").val(0);
                $("#txtSGSTPerc").val(0);
                $("#txtIGSTPerc").val(ui.item.IGST_Perc);
                $("#txtCGSTPercAmount").val(0);
                $("#txtSGSTPercAmount").val(0);
                $("#txtIGSTPercAmount").val(0);
            }
            GetProductDetail(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductName").val("");
                $("#hdnProductId").val("0");
                $("#txtProductShortDesc").val("");
                $("#txtProductCode").val("");
                $("#txtHSN_Code").val("");
                $("#txtCGSTPerc").val(0);
                $("#txtSGSTPerc").val(0);
                $("#txtIGSTPerc").val(0);
                $("#txtCGSTPercAmount").val(0);
                $("#txtSGSTPercAmount").val(0);
                $("#txtIGSTPercAmount").val(0);
                ShowModel("Alert", "Please select Product from List")
             }
            return false;
        }

    })
 .autocomplete("instance")._renderItem = function (ul, item) {
     return $("<li>")
       .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.desc + "</div>")
       .appendTo(ul);
 };

    var ddlCompanyBranch = $("#ddlCompanyBranch");
    //if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
    //    ShowModel("Alert", "Please select Company Branch")
    //    return false;
    //}


    $("#txtEmployeeName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SaleTarget/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term,companyBranchId: ddlCompanyBranch.val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.FirstName, value: item.EmployeeId, EmployeeCode: item.EmployeeCode, MobileNo: item.MobileNo, DesignationId: item.DesignationId
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtEmployeeName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtEmployeeName").val(ui.item.label);
            $("#hdnEmployeeId").val(ui.item.value);
            $("#hdnDesignationId").val(ui.item.DesignationId);
            GetDesignationDetail(ui.item.DesignationId);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtEmployeeName").val("");
                $("#hdnEmployeeId").val("0");

                ShowModel("Alert", "Please select Employee from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.EmployeeCode + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};

    $("#txtCityName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SaleTarget/GetCityAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term,
                    stateId: $("#ddlStateList").val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.CityName, value: item.CityId, vehicles: item.Vehicles, PerDealar: item.PerDealar, DealershipsNos: item.DealershipsNos,
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtCityName").val(ui.item.label);
            $("#txtVehicles").val(ui.item.vehicles);
            $("#txtDealershipsNos").val(ui.item.DealershipsNos);
            $("#txtPerDealar").val(ui.item.PerDealar);

            return false;
        },
        select: function (event, ui) {
            $("#txtCityName").val(ui.item.label);
            $("#hdnCityId").val(ui.item.value);
            $("#txtDealershipsNos").val(ui.item.DealershipsNos);
            $("#txtPerDealar").val(ui.item.PerDealar);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtCityName").val("");
                $("#hdnCityId").val("0");
                $("#txtDealershipsNos").val("");
                $("#txtPerDealar").val("");
                ShowModel("Alert", "Please select City from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.vehicles + "</b></div>")
      .appendTo(ul);
};


    $("#txtTargetDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {

        }
    });
    $("#txtTargetFromDate,#txtTargetToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

   
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnTargetId = $("#hdnTargetId");
    if (hdnTargetId.val() != "" && hdnTargetId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetSaleTargetDetail(hdnTargetId.val());
       }, 2000);



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

        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
        }

    var targetDetails = [];
    GetTargetDetailList(targetDetails);

    BindCompanyBranchList();
    BindStateList(0);
    BindTargetTypeList(0);
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

function GetTargetDetailList(targetDetails) {
    var hdnTargetId = $("#hdnTargetId");
    var requestData = { targetDetails: targetDetails, targetId: hdnTargetId.val() };
    $.ajax({
        url: "../SaleTarget/GetSaleTargetDetailList",
        cache:false,
        data:JSON.stringify(requestData),
        dataType:"html",
        contentType:"application/json; charset=utf-8",
        type:"POST",
        error:function (err) {
            $("#divTargetList").html("");
            $("#divTargetList").html(err);
        },
        success:function (data) {
            $("#divTargetList").html("");
            $("#divTargetList").html(data);
            ShowHideProductPanel(2);
        }
    });
}

function AddTargetDeatil(action) {
    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnSequenceNo");
    var hdnTargetDetailId = $("#hdnTargetDetailId");
    var txtDesignation = $("#txtDesignation");
    var ddlTargetType = $("#ddlTargetType");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var txtEmployeeName = $("#txtEmployeeName");
    var hdnDesignationId = $("#hdnDesignationId");
    var hdnProductId = $("#hdnProductId");
    var ddlStateList = $("#ddlStateList");
    var hdnCityId = $("#hdnCityId");
    var txtVehicles = $("#txtVehicles");
    var txtQuantity = $("#txtQuantity");
    var txtTargetAmount = $("#txtTargetAmount");
    var txtProductName = $("#txtProductName");
    var txtCityName = $("#txtCityName");
    var txtDealershipsNos = $("#txtDealershipsNos");
    var txtTargetDealershipsNos = $("#txtTargetDealershipsNos");
    var txtPerDealar = $("#txtPerDealar");
    if (txtEmployeeName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Employee Name")
        txtEmployeeName.focus();
        return false;
    }
    if (hdnEmployeeId.val().trim() == "" || hdnEmployeeId.val().trim() == "0") {
        ShowModel("Alert", "Please select Employee from list")
        txtEmployeeName.focus();
        return false;
    }
    //if (ddlTargetType.val() == "0") {
    //    ShowModel("Alert", "Please Select Target Type");
    //    ddlTargetType.focus();
    //    return false;
    //}
    //if (txtProductName.val().trim() == "") {
    //    ShowModel("Alert", "Please Enter Product Name")
    //    txtProductName.focus();
    //    return false;
    //}
    
    //if (hdnProductId.val().trim() == "" || hdnProductId.val().trim() == "0") {
    //    ShowModel("Alert", "Please select Product from list")
    //    hdnProductId.focus();
    //    return false;
    //}
    
    if (ddlStateList.val().trim() == "0") {
        ShowModel("Alert", "Please Select State")
        return false;
    }
    if (txtCityName.val() == "") {
        ShowModel("Alert", "Please Enter City ")
        return false;
    }

    if (hdnCityId.val().trim() == "" || hdnCityId.val().trim() == "0") {
        ShowModel("Alert", "Please Select City from list")
        hdnCityId.focus();
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

    var targetList = [];
    $('#tblTargetDetailList tr').each(function (i, row) {
        var $row = $(row);
        var targetDetailId = $row.find("#hdnTargetDetailId").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var productId = $row.find("#hdnProductId").val();
        var productName = $row.find("#hdnProductName").val();
        var employeeId = $row.find("#hdnEmployeeId").val();
        var employeeName = $row.find("#hdnEmployeeName").val();
        var targetTypeId = $row.find("#hdnTargetTypeId").val();
        var targetTypeName = $row.find("#hdnTargetTypeName").val();
        var stateId = $row.find("#hdnStateId").val();
        var stateName = $row.find("#hdnStateName").val();
        var cityId = $row.find("#hdnCityId").val();
        var cityName = $row.find("#hdnCityName").val();
        var quantity = $row.find("#hdnQuantity").val();
        var designationId = $row.find("#hdnDesignationId").val();
        var designationName = $row.find("#hdnDesignationName").val();
        var vehicles = $row.find("#hdnVehicles").val();
        var targetAmount = $row.find("#hdnTargetAmount").val();
        var perDealar = $row.find("#hdnPerDealar").val();
        var dealershipsNos = $row.find("#hdnDealershipsNos").val();
        var targetDealershipsNos = $row.find("#hdnTargetDealershipsNos").val();
        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                //if (productId == hdnProductId.val()) {
                //    ShowModel("Alert", "Product already added!!!")
                //    txtProductName.focus();
                //    flag = false;
                //    return false;
                //}
                var targetAddEdit = {
                    TargetDetailId: targetDetailId,
                    SequenceNo: sequenceNo,
                    EmpId: employeeId,
                    EmployeeName:employeeName,
                    DesignationId: designationId,
                    DesignationName: designationName,
                    TargetTypeId: targetTypeId,
                    TargetTypeName: targetTypeName,
                    ProductId:productId,
                    ProductName: productName,
                    StateId:stateId,
                    StateName:stateName,
                    CityId:cityId,
                    CityName:cityName,
                    Vehicles:vehicles,
                    TargetQty: quantity,
                    Amount: targetAmount,
                    PerDealar:perDealar,
                    DealershipsNos:dealershipsNos,
                    TargetDealershipsNos: targetDealershipsNos
                };
                targetList.push(targetAddEdit);
                productEntrySequence = parseInt(productEntrySequence) + 1;

            }
            else if (hdnTargetDetailId.val() == targetDetailId && hdnSequenceNo.val() == sequenceNo) {
                 var targetAddEdit = {
                         TargetDetailId: hdnTargetDetailId.val(),
                         SequenceNo: hdnSequenceNo.val(),
                         EmpId: hdnEmployeeId.val(),
                         EmployeeName:txtEmployeeName.val(),
                         DesignationId: hdnDesignationId.val(),
                         DesignationName: txtDesignation.val(),
                         TargetTypeId: ddlTargetType.val(),
                         TargetTypeName:$("#ddlTargetType option:selected").text(),
                         ProductId: hdnProductId.val(),
                         ProductName: txtProductName.val().trim(),
                         StateId: ddlStateList.val(),
                         StateName: $("#ddlStateList option:selected").text(),
                         CityId: hdnCityId.val(),
                         CityName: txtCityName.val(),
                         Vehicles: txtVehicles.val(),
                         TargetQty: txtQuantity.val().trim(),
                         Amount: txtTargetAmount.val(),
                         PerDealar: txtPerDealar.val(),
                         DealershipsNos: txtDealershipsNos.val(),
                         TargetDealershipsNos: txtTargetDealershipsNos.val()
                     };
                 targetList.push(targetAddEdit);
                hdnSequenceNo.val("0");
            }
        }

    });
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {
        var targetAddEdit = {
            TargetDetailId: hdnTargetDetailId.val(),
            SequenceNo: hdnSequenceNo.val(),
            EmpId: hdnEmployeeId.val(),
            EmployeeName:txtEmployeeName.val(),
            DesignationId: hdnDesignationId.val(),
            DesignationName: txtDesignation.val(),
            TargetTypeId: ddlTargetType.val(),
            TargetTypeName: $("#ddlTargetType option:selected").text(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            StateId: ddlStateList.val(),
            StateName: $("#ddlStateList option:selected").text(),
            CityId: hdnCityId.val(),
            CityName: txtCityName.val(),
            Vehicles: txtVehicles.val(),
            TargetQty: txtQuantity.val(),
            Amount: txtTargetAmount.val(),
            PerDealar: txtPerDealar.val(),
            DealershipsNos: txtDealershipsNos.val(),
            TargetDealershipsNos:txtTargetDealershipsNos.val()
        };
        targetList.push(targetAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetTargetDetailList(targetList);
    }

}
function EditProductRow(obj) {

    var row = $(obj).closest("tr");
    var targetDetailId = $(row).find("#hdnTargetDetailId").val();
    var sequenceNo = $(row).find("#hdnSequenceNo").val();
    var employeeId = $(row).find("#hdnEmployeeId").val();
    var employeeName = $(row).find("#hdnEmployeeName").val();
    var productId = $(row).find("#hdnProductId").val();
    var productName = $(row).find("#hdnProductName").val();
    var targetTypeId = $(row).find("#hdnTargetTypeId").val();
    var targetTypeName = $(row).find("#hdnTargetTypeName").val();
    var stateId = $(row).find("#hdnStateId").val();
    var stateName = $(row).find("#hdnStateName").val();
    var cityId = $(row).find("#hdnCityId").val();
    var cityName = $(row).find("#hdnCityName").val();
    var frequency = $(row).find("#hdnFrequency").val();
    var designationId = $(row).find("#hdnDesignationId").val();
    var designationName = $(row).find("#hdnDesignationName").val();
    var vehicles = $(row).find("#hdnVehicles").val();
    var quantity = $(row).find("#hdnQuantity").val();
    var targetAmount = $(row).find("#hdnTargetAmount").val();
    var perDealar = $(row).find("#hdnPerDealar").val();
    var dealershipsNos = $(row).find("#hdnDealershipsNos").val();
    var targetDealershipsNos = $(row).find("#hdnTargetDealershipsNos").val();
    $("#txtProductName").val(productName);
    $("#hdnTargetDetailId").val(targetDetailId);
    $("#hdnSequenceNo").val(sequenceNo);
    $("#hdnProductId").val(productId);
    $("#txtEmployeeName").val(employeeName);
    $("#hdnEmployeeId").val(employeeId);
    $("#txtDesignation").val(designationName);
    $("#hdnDesignationId").val(designationId);
    BindTargetTypeList(targetTypeId);
    //$("#ddlTargetType").val(targetTypeId);
    $("#ddlStateList").val(stateId);
    $("#txtCityName").val(cityName);
    $("#hdnCityId").val(cityId);
    $("#txtVehicles").val(vehicles);
    $("#txtQuantity").val(quantity);
    $("#txtTargetAmount").val(targetAmount);
    $("#txtDealershipsNos").val(dealershipsNos);
    $("#txtTargetDealershipsNos").val(targetDealershipsNos);
    $("#txtPerDealar").val(perDealar);


    $("#btnAddTargetDetail").hide();
    $("#btnUpdateTargetDetail").show();

    ShowHideProductPanel(1);
}

function RemoveProductRow(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");
        var workOrderProductDetailId = $(row).find("#hdnWorkOrderProductDetailId").val();
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
    }
}

function GetSaleTargetDetail(targetId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SaleTarget/GetSaleTargetDetail",
        data: { targetId: targetId },
        dataType: "json",
        success: function (data) {
            $("#txtTargetNo").val(data.TargetNo);
            $("#txtTargetDate").val(data.TargetDate);
            $("#txtTargetFromDate").val(data.TargetFromDate);
            $("#txtTargetToDate").val(data.TargetToDate);

            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#ddlFrequency").val(data.Frequency);
            $("#ddlApprovalStatus").val(data.TargetStatus);
            if (data.TargetStatus == "Final") {
                $(".editonly").hide();
                $("#btnReset").hide();
                $("#btnUpdate").hide();
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);

            }
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
            $("#txtRemarks1").val(data.Remarks);
            $("#btnAddNew").show();
            $("#btnPrint").show();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}

function SaveData() {
    var txtTargetNo = $("#txtTargetNo");
    var hdnTargetId = $("#hdnTargetId");
    var txtTargetDate = $("#txtTargetDate");
    var txtTargetFromDate = $("#txtTargetFromDate");
    var txtTargetToDate = $("#txtTargetToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtRemarks = $("#txtRemarks1");
    var ddlFrequency = $("#ddlFrequency");
    

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == 0) {
        ShowModel("Alert", "Please select  Location")
        return false;
    }


    if (Date.parse(txtTargetFromDate.val()) > Date.parse(txtTargetToDate.val())) {
        ShowModel("Alert", "Target To Date Not be less than Target From date.")
        return false;
    }

    if (ddlFrequency.val() == "0")
    {
        ShowModel("Alert", "Please select Frequency");
        ddlFrequency.focus();
        return false;
    }
    var target = 
    {
        TargetId:hdnTargetId.val(),
        TargetDate:txtTargetDate.val(),
        TargetFromDate:txtTargetFromDate.val(),
        TargetToDate:txtTargetToDate.val(),
        CompanyBranchId:ddlCompanyBranch.val(),
        Frequency:ddlFrequency.val(),
        Remarks:txtRemarks.val(),
        TargetStatus: ddlApprovalStatus.val(),
        Status:1
    };
 
    var targetDetailList = [];
    $('#tblTargetDetailList tr').each(function (i, row) {
        var $row = $(row);
        var targetDetailId = $row.find("#hdnTargetDetailId").val();
        var productId = $row.find("#hdnProductId").val();
        var employeeId = $row.find("#hdnEmployeeId").val();
        var targetTypeId = $row.find("#hdnTargetTypeId").val();
        var stateId = $row.find("#hdnStateId").val();
        var cityId = $row.find("#hdnCityId").val();
        var targetQty = $row.find("#hdnQuantity").val();

        var designationId = $row.find("#hdnDesignationId").val();
        var vehicles = $row.find("#hdnVehicles").val();
        var targetAmount = $row.find("#hdnTargetAmount").val();
        var perDealar = $row.find("#hdnPerDealar").val();
        var dealershipsNos = $row.find("#hdnDealershipsNos").val();
        var targetDealershipsNos = $row.find("#hdnTargetDealershipsNos").val();
        if (targetDetailId != undefined) {

            var targetDetails = {
                EmpId:employeeId,
                DesignationId:designationId,
                ProductId:productId,
                StateId:stateId,
                CityId:cityId,
                Vehicles:vehicles,
                TargetQty:targetQty,
                Amount: targetAmount,
                TargetTypeId:targetTypeId,
                TargetDealershipsNos:targetDealershipsNos
            };
            targetDetailList.push(targetDetails);
        }
    });

    if (targetDetailList.length == 0) {
        ShowModel("Alert", "Please Enter at least 1 Target Detail")
        return false;
    }
    var accessMode = 1;//Add Mode
    if (hdnTargetId.val() != null && hdnTargetId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { targetViewModel: target, targetDetails: targetDetailList };
    $.ajax({
        url: "../SaleTarget/AddEditSaleTarget?AccessMode=" + accessMode + "",
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
                       window.location.href = "../SaleTarget/AddEditSaleTarget?saleTargetId=" + data.trnId + "&AccessMode=3";
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

    $("#txtTargetNo").val("");
    $("#hdnTargetId").val("0");
    $("#txtTargetDate").val($("#hdnCurrentDate").val());
    $("#txtTargetFromDate").val($("#hdnCurrentDate").val());
    $("#txtTargetToDate").val($("#hdnCurrentDate").val());

    $("#ddlApprovalStatus").val("Final");
    $("#ddlCompanyBranch").val("0");
    $("#txtRemarks1").val("");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}

function GetProductDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productid: productId },
        dataType: "json",
        success: function (data) {
            $("#txtUOMName").val(data.UOMName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}


function ShowHideProductPanel(action) {
    if (action == 1) {
        $(".productsection").show();
        BindTargetTypeList(0);
    }
    else {
        $(".productsection").hide();
        $("#txtProductName").val("");
        $("#hdnProductId").val("0");
        $("#hdnTargetDetailId").val("0");
        $("#txtEmployeeName").val("");
        $("#hdnEmployeeId").val("0");
        $("#txtDesignation").val("");
        $("#hdnDesignationId").val("0");
        $("#ddlTargetType").val("0");
        $("#ddlStateList").val("0");
        $("#ddlTargetType").val("0");
        $("#txtCityName").val("");
        $("#hdnCityId").val("0");
        $("#txtVehicles").val("");
        $("#txtQuantity").val("");
        $("#txtDealershipsNos").val("");
        $("#txtTargetDealershipsNos").val("");
        $("#txtPerDealar").val("");
        $("#txtTargetAmount").val("");
        $("#txtDealershipsNos").val("");
        $("#txtTargetDealershipsNos").val("");
        $("#txtPerDealar").val("");
        $("#ddlFrequency").val("0");
        $("#btnAddTargetDetail").show();
        $("#btnUpdateTargetDetail").hide();
        BindTargetTypeList(0);
      }
}

function show() {
    let el = document.querySelector('input[id="txtQuantity"]');
    el.addEventListener('keypress', (event) => {
        let k = event.key,
            t = isNaN(k),
            sc = ['Backspace'].indexOf(k) === -1,
            d = k === '.', dV = el.value.indexOf('.') > -1,
            m = k === '-', mV = el.value.length > 0;

        if ((t && sc) && ((d && dV) || (m && dV) || (m && mV) || ((t && !d) && (t && !m)))) { event.preventDefault(); }
    }, false);
    el.addEventListener('paste', (event) => {
        if (event.clipboardData.types.indexOf('text/html') > -1) {
            if (isNaN(event.clipboardData.getData('text'))) { event.preventDefault(); }
        }
    }, false);
}

function ResetPage() {
    if (confirm("Are you sure to reset the page")) {
        window.location.href = "../SaleTarget/AddEditSaleTarget";
    }
}


///////*************For Only Allow Decimal Values For Two Digits Values******************///////

////////**************Created By Dheeraj**********////////////////////

$('.Quantity').keypress(function (event) {
    var $this = $(this);
    if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
       ((event.which < 48 || event.which > 57) &&
       (event.which != 0 && event.which != 8))) {
        event.preventDefault();
    }

    var text = $(this).val();
    if ((event.which == 46) && (text.indexOf('.') == -1)) {
        setTimeout(function () {
            if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
            }
        }, 1);
    }

    if ((text.indexOf('.') != -1) &&
        (text.substring(text.indexOf('.')).length > 2) &&
        (event.which != 0 && event.which != 8) &&
        ($(this)[0].selectionStart >= text.length - 2)) {
        event.preventDefault();
    }
});

$('.Quantity').bind("paste", function (e) {
    var text = e.originalEvent.clipboardData.getData('Text');
    if ($.isNumeric(text)) {
        if ((text.substring(text.indexOf('.')).length > 3) && (text.indexOf('.') > -1)) {
            e.preventDefault();
            $(this).val(text.substring(0, text.indexOf('.') + 3));
        }
    }
    else {
        e.preventDefault();
    }
});


function OpenSOSearchPopup() {
    $("#SearchQuotationModel").modal();

}
function SearchSaleOrder() {
    var txtSearchSONo = $("#txtSearchSONo");
    var txtCustomerName = $("#txtSearchCustomerName");

    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { soNo: txtSearchSONo.val().trim(), customerName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: "Final", displayType: "Popup" };
    $.ajax({
        url: "../WorkOrder/GetSaleOrderList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divSOList").html("");
            $("#divSOList").html(err);
        },
        success: function (data) {
            $("#divSOList").html("");
            $("#divSOList").html(data);
        }
    });
}
function SelectSO(soId, soNo, soDate, companyBranchId) {
    $("#txtSONo").val(soNo);
    $("#hdnSOId").val(soId);
    $("#txtSODate").val(soDate);
    $("#ddlCompanyBranch").val(companyBranchId);
    GetSODetail(soId);
    var soProducts = [];
    GetSOProductList(soProducts);

    $("#SearchQuotationModel").modal('hide');

}
function GetSODetail(soId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SO/GetSODetail",
        data: { soId: soId },
        dataType: "json",
        success: function (data) {
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

function GetSOProductList(soProducts) {
    var hdnSOId = $("#hdnSOId");
    var requestData = { soProducts: soProducts, soId: hdnSOId.val() };
    $.ajax({
        url: "../WorkOrder/GetWOSOProductList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divProductList").html("");
            $("#divProductList").html(err);
        },
        success: function (data) {
            $("#divProductList").html("");
            $("#divProductList").html(data);
        }
    });
}
function GetDesignationDetail(designationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Designation/GetDesignationDetail",
        data: { designationId: designationId },
        dataType: "json",
        success: function (data) {
            $("#txtDesignation").val(data.DesignationName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function BindStateList(stateId) {
    $("#ddlStateList").val(0);
    $("#ddlStateList").html("");
        var data = {};
        $.ajax({
            type: "GET",
            url: "../SaleTarget/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlStateList").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlStateList").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                if (stateId > 0) {
                    $("#ddlStateList").val(stateId);
                }
            },
            error: function (Result) {
                $("#ddlStateList").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
}

function ChangeState()
{
    $("#txtCityName").val("");
    $("#hdnCityId").val(0);
}

function BindTargetTypeList(targetTypeId) {
    $("#ddlTargetType").val(0);
    $("#ddlTargetType").html("");
    var data = {};
    $.ajax({
        type: "GET",
        url: "../SaleTarget/GetTargetTypeList",
        data: data,
        asnc: false,
        dataType: "json",
        success: function (data) {
            $("#ddlTargetType").append($("<option></option>").val(0).html("-Select Target Type-"));
            $.each(data, function (i, item) {
                $("#ddlTargetType").append($("<option></option>").val(item.TargetTypeId).html(item.TargetName));
            });
            if (targetTypeId > 0) {
                $("#ddlTargetType").val(targetTypeId);
            }
        },
        error: function (Result) {
            $("#ddlTargetType").append($("<option></option>").val(0).html("-Select Target Type-"));
        }
    });
}