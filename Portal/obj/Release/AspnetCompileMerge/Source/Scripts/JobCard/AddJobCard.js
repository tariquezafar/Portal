$(document).ready(function () {
     $("#tabs").tabs({
        collapsible: true
    });
    $("#txtJobCardNo").attr('readOnly', true);
    $("#txtJobCardDate").attr('readOnly', true);
    $("#txtDateOfSale").attr('readOnly', true);
    $("#txtCustomerName").attr('readOnly', true);
    $("#txtPreSeviceDate").attr('readOnly', true);


    $("#txtProductCode").attr('readOnly', true);
    $("#txtDiscountAmount").attr('readOnly', true);
    $("#txtCGSTPercAmount").attr('readOnly', true);
    $("#txtSGSTPercAmount").attr('readOnly', true);
    $("#txtTotalPrice").attr('readOnly', true);
   
    $("#txtJobCardDate,#txtDateOfSale,#txtPreSeviceDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $("#txtCustomerCode").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SaleInvoice/GetCustomerAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.CustomerName, value: item.CustomerId, primaryAddress: item.PrimaryAddress, code: item.CustomerCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtCustomerName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtCustomerName").val(ui.item.label);
            $("#hdnCustomerId").val(ui.item.value);
            $("#txtCustomerCode").val(ui.item.code);
           
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtCustomerName").val("");
                $("#hdnCustomerId").val("0");
                $("#txtCustomerCode").val("");
                ShowModel("Alert", "Please select Customer from List")
            }
            return false;
        }

    })
 .autocomplete("instance")._renderItem = function (ul, item) {
     return $("<li>")
       .append("<div><b>" + item.code + " || " + item.label + "</b><br>" + "</div>")
       .appendTo(ul);
 };

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
                        return {
                            label: item.ProductName,
                            value: item.Productid,
                           
                        };
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
           
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtProductName").val("");
                $("#hdnProductId").val("0");
                
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

    $("#txtProductNameJob").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Product/GetProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.ProductName, value: item.Productid,
                            desc: item.ProductShortDesc, code: item.ProductCode,
                            CGST_Perc: item.CGST_Perc,
                            SGST_Perc: item.SGST_Perc,
                            SalePrice: item.SalePrice

                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtProductNameJob").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtProductNameJob").val(ui.item.label);
            $("#hdnProductIdJob").val(ui.item.value);
            $("#txtProductCode").val(ui.item.code);            
            $("#txtCGSTPerc").val(ui.item.CGST_Perc);
            $("#txtSGSTPerc").val(ui.item.SGST_Perc);
            $("#txtPrice").val(ui.item.SalePrice);
            
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
                $("#txtPrice").val(0);
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

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnJobCardId = $("#hdnJobCardId");

    if (hdnJobCardId.val() != "" && hdnJobCardId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetJobCardDetail(hdnJobCardId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#ddlFormTypeDesc").prop('disabled', true);
            $("#chkstatus").prop('disabled', true);
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

    

    var jobCardProducts = [];
    GetJobCardProductList(jobCardProducts);
    var jobCardProductList = [];
    GetJobCardProduct(jobCardProductList);
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

$(".numeric-dot-only").on("input", function () {
    var regexp = /[^\d.]|\.(?=.*\.)/g;
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
function GetJobCardDetail(jobCardId) {
    
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../JobCard/GetJobCardDetail",
        data: { jobCardId: jobCardId },
        dataType: "json",
        success: function (data) {
          
            $("#hdnJobCardId").val(data.JobCardID);
            $("#txtJobCardNo").val(data.JobCardNo);
            $("#txtJobCardDate").val(data.JobCardDate);
            $("#txtTimeInhours").val(data.TimeIn);
            $("#txtTimeInMinutes").val(data.TimeInMinute);
            $("#txtDeliveryTime").val(data.DeliveryTime);
            $("#txtDeliveryTimeMinutes").val(data.DeliveryTimeMinute);
            $("#hdnCustomerId").val(data.CustomerID);
            $("#txtCustomerName").val(data.CustomerName);
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtModelName").val(data.ModelName);
            $("#txtRegNo").val(data.RegNo);
            $("#txtFrameNo").val(data.FrameNo);
            $("#txtEngineNo").val(data.EngineNo);
            $("#txtDateOfSale").val(data.DateOfSale);
            $("#txtKMSCovered").val(data.KMSCovered);
            $("#txtCouponNo").val(data.CouponNo);
            $("#txtFuelLevel").val(data.FuelLevel);
            $("#txtEngineOilLevel").val(data.EngineOilLevel);
            $("#txtKeyNo").val(data.KeyNo);
            $("#txtBatteryMakeNo").val(data.BatteryMakeNo);
            $("#txtDamage").val(data.Damage);
            $("#txtAccessories").val(data.Accessories);
            $("#ddlTypeOfService").val(data.TypeOfService);
            $("#txtESTDeliveryTime").val(data.EstimationDeliveryTime);
            $("#txtESTDeliveryTimeMinutes").val(data.EstimationDeliveryTimeMinute);
            $("#txCostOfRepair").val(data.EstimationCostOfRepair);
            $("#txtCostOfParts").val(data.EstimationCostOfParts);
            $("#txtPreJobCardNo").val(data.PreJobCardNo);
            $("#txtPreSeviceDate").val(data.PreSeviceDate);
            $("#txtPreKey").val(data.PreKey);
            $("#txtMahenicName").val(data.MahenicName);
            $("#txtStartTime").val(data.StartTime);
            $("#txtClosingTime").val(data.ClosingTime);
            $("#txtStartTimeMinute").val(data.StartTimeMinute);
            $("#txtClosingTimeMinute").val(data.ClosingTimeMinute);
            $("#ddlApprovalStatus").val(data.ApprovalStatus);

            $("#txtVehicleNo").val(data.VehicleNo);
            $("#txtChassisNo").val(data.ChassisNo);

            $("#btnAddNew").show();
            $("#btnPrint").show();
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}
function GetJobCardProductList(jobCardProducts) {
    var hdnJobCardId = $("#hdnJobCardId");
    var requestData = { jobCardProducts: jobCardProducts, jobCardId: hdnJobCardId.val() };
    $.ajax({
        url: "../JobCard/GetJobCardProductList",
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

            $("#txtCustComplaintObservation").val("");
            $("#txtSupervisorAdvice").val("");
            $("#txtAmountEstimated").val("");

        }
    });
}
function GetJobCardProduct(jobCardProductList) {
    var hdnJobCardId = $("#hdnJobCardId");
    var requestData = { jobCardProductList: jobCardProductList, jobCardId: hdnJobCardId.val() };
    $.ajax({
        url: "../JobCard/GetJobCardProduct",
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
            $("#JobdivProductList").html("");
            $("#JobdivProductList").html(data);

            ShowHideProductPanel(2);

        }
    });
}

function AddProduct(action) {

    var productEntrySequence = 0;
    var flag = true;

    var hdnJobCardDetailID = $("#hdnJobCardDetailID");
    var hdnSequenceNo = $("#hdnSequenceNo");
    var txtCustComplaintObservation = $("#txtCustComplaintObservation");
    var txtSupervisorAdvice = $("#txtSupervisorAdvice");
    var txtAmountEstimated = $("#txtAmountEstimated");

    var hdnProductId = $("#hdnProductId");
    var txtProductName = $("#txtProductName");
    var txtServiceItemName = $("#txtServiceItemName");
    var hdnServiceItemID = $("#hdnServiceItemID");
 
   
    var jobCardProductList = [];
    $('#tblProductList tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var jobCardDetailID = $row.find("#hdnJobCardDetailID").val();
        var sequenceNo = $row.find("#hdnSequenceNo").val();
        var custComplaintObservation = $row.find("#hdnCustComplaintObservation").val();
        var supervisorAdvice = $row.find("#hdnSupervisorAdvice").val();
        var amountEstimated = $row.find("#hdnAmountEstimated").val();

        var productID = $row.find("#hdnProductID").val();
        var productName = $row.find("#hdnProductName").val();
        var serviceItemID = $row.find("#hdnServiceItemID").val();
        var serviceItemName = $row.find("#hdnServiceItemName").val();
        
        if (custComplaintObservation != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

               

                var jobCardProduct = {
                    JobCardDetailID: jobCardDetailID,
                    SequenceNo: sequenceNo,
                    CustComplaintObservation: custComplaintObservation,
                    SupervisorAdvice: supervisorAdvice,
                    AmountEstimated: amountEstimated,
                    ProductID: productID,
                    ProductName: productName,
                    ServiceItemName: serviceItemName,
                    ServiceItemID: serviceItemID,
                  
                };



                jobCardProductList.push(jobCardProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnProductId.val() == productId && hdnSequenceNo.val() == sequenceNo) {

                var jobCardAddEdit = {

                    JobCardDetailID: hdnJobCardDetailID.val(),
                    SequenceNo: hdnSequenceNo.val(),
                    CustComplaintObservation: txtCustComplaintObservation.val(),
                    SupervisorAdvice: txtSupervisorAdvice.val(),
                    AmountEstimated: txtAmountEstimated.val(),
                    ProductID: hdnProductId.val(),
                    ProductName: txtProductName.val(),
                    ServiceItemName: txtServiceItemName.val(),
                    ServiceItemID: hdnServiceItemID.val(),

                  
                };
                jobCardProductList.push(jobCardAddEdit);
                hdnSequenceNo.val("0");
            }
        }

    });


    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {

        var jobCardAddEdit = {
            JobCardDetailID: hdnJobCardDetailID.val(),
            SequenceNo: hdnSequenceNo.val(),
            CustComplaintObservation: txtCustComplaintObservation.val(),
            SupervisorAdvice: txtSupervisorAdvice.val(),
            AmountEstimated: txtAmountEstimated.val(),
            ProductID: hdnProductId.val(),
            ProductName: txtProductName.val(),
            ServiceItemName: txtServiceItemName.val(),
            ServiceItemID: hdnServiceItemID.val(),
        };
        jobCardProductList.push(jobCardAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetJobCardProductList(jobCardProductList);
    }

}
function SaveData() {
    var hdnJobCardId = $("#hdnJobCardId");
    var txtJobCardNo = $("#txtJobCardNo");
    var txtJobCardDate = $("#txtJobCardDate");
    var txtTimeInhours = $("#txtTimeInhours");
    var txtTimeInMinutes = $("#txtTimeInMinutes");
    var txtDeliveryTime = $("#txtDeliveryTime");
    var txtDeliveryTimeMinutes = $("#txtDeliveryTimeMinutes");
    var hdnCustomerId = $("#hdnCustomerId");
    var txtModelName = $("#txtModelName");
    var txtRegNo = $("#txtRegNo");
    var txtFrameNo = $("#txtFrameNo");
    var txtEngineNo = $("#txtEngineNo");
    var txtDateOfSale = $("#txtDateOfSale");
    var txtKMSCovered = $("#txtKMSCovered");
    var txtCouponNo = $("#txtCouponNo");
    var txtFuelLevel = $("#txtFuelLevel");
    var txtEngineOilLevel = $("#txtEngineOilLevel");
    var txtKeyNo = $("#txtKeyNo");
    var txtBatteryMakeNo = $("#txtBatteryMakeNo");
    var txtDamage = $("#txtDamage");
    var txtAccessories = $("#txtAccessories");
    var ddlTypeOfService = $("#ddlTypeOfService");
    var txtESTDeliveryTime = $("#txtESTDeliveryTime");
    var txtESTDeliveryTimeMinutes = $("#txtESTDeliveryTimeMinutes");
    var txCostOfRepair = $("#txCostOfRepair");
    var txtCostOfParts = $("#txtCostOfParts");
    var txtPreJobCardNo = $("#txtPreJobCardNo");
    var txtPreSeviceDate = $("#txtPreSeviceDate");
    var txtPreKey = $("#txtPreKey");
    var txtMahenicName = $("#txtMahenicName");
    var txtStartTime = $("#txtStartTime");
    var txtClosingTime = $("#txtClosingTime");

    var txtStartTimeMinute = $("#txtStartTimeMinute");
    var txtClosingTimeMinute = $("#txtClosingTimeMinute");

    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtVehicleNo = $("#txtVehicleNo");
    var txtChassisNo = $("#txtChassisNo");

    var jobCardViewModel = {

        JobCardID: hdnJobCardId.val(),
        JobCardDate: txtJobCardDate.val(),
        TimeIn: txtTimeInhours.val() + ":" + txtTimeInMinutes.val(),
        DeliveryTime: txtDeliveryTime.val()+ ":" + txtDeliveryTimeMinutes.val(),
        CustomerID: hdnCustomerId.val(),
        ModelName: txtModelName.val(),
        RegNo: txtRegNo.val(),
        FrameNo: txtFrameNo.val().trim(),
        EngineNo: txtEngineNo.val(),
        DateOfSale: txtDateOfSale.val(),
        KMSCovered: txtKMSCovered.val().trim(),
        CouponNo: txtCouponNo.val(),
        FuelLevel: txtFuelLevel.val(),
        EngineOilLevel: txtEngineOilLevel.val().trim(),
        KeyNo: txtKeyNo.val(),
        BatteryMakeNo: txtBatteryMakeNo.val(),
        Damage: txtDamage.val().trim(),
        Accessories: txtAccessories.val(),
        TypeOfService: ddlTypeOfService.val(),
        EstimationDeliveryTime: txtESTDeliveryTime.val() + ":" + txtESTDeliveryTimeMinutes.val(),
        EstimationCostOfRepair: txCostOfRepair.val(),
        EstimationCostOfParts: txtCostOfParts.val(),
        PreJobCardNo: txtPreJobCardNo.val().trim(),
        PreSeviceDate: txtPreSeviceDate.val(),
        PreKey: txtPreKey.val().trim(),
        MahenicName: txtMahenicName.val(),
        StartTime: txtStartTime.val() + ":" + txtStartTimeMinute.val(),
        ClosingTime: txtClosingTime.val().trim() + ":" + txtClosingTimeMinute.val(),
        ApprovalStatus: ddlApprovalStatus.val(),
        VehicleNo: txtVehicleNo.val(),
        ChassisNo: txtChassisNo.val(),
       
    }

    var jobCardDetailViewModel = [];
    $('#tblProductList tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var jobCardDetailID = $row.find("#hdnJobCardDetailID").val();
        var productId = $row.find("#hdnProductID").val();
        var productName = $row.find("#hdnProductName").val();
        var serviceItemID = $row.find("#hdnServiceItemID").val();
        var serviceItemName = $row.find("#hdnServiceItemName").val();
        var custComplaintObservation = $row.find("#hdnCustComplaintObservation").val();
        var supervisorAdvice = $row.find("#hdnSupervisorAdvice").val();
        var amountEstimated = $row.find("#hdnAmountEstimated").val();
        if (productId != undefined) {

            var jobcardProduct = {

                ProductID: productId,
                ServiceItemID:serviceItemID,
                ServiceItemName: serviceItemName,
                CustComplaintObservation: custComplaintObservation,
                SupervisorAdvice: supervisorAdvice,
                AmountEstimated: amountEstimated,
            };
            jobCardDetailViewModel.push(jobcardProduct);
        }
    });

    var jobProductList = [];
    $('#tblProductList1 tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);       
        var productId = $row.find("#hdnProductIdJob").val();
        var price = $row.find("#hdnPrice").val();
        var quantity = parseFloat($row.find("#hdnQuantity").val()).toFixed(2);
        var discountPerc = $row.find("#hdnDiscountPerc").val();
        var discountAmount = $row.find("#hdnDiscountAmount").val();      
        var cGSTPerc = $row.find("#hdnCGSTPerc").val();
        var cGSTPercAmount = $row.find("#hdnCGSTAmount").val();
        var sGSTPerc = $row.find("#hdnSGSTPerc").val();
        var sGSTPercAmount = $row.find("#hdnSGSTAmount").val();
        var totalPrice = $row.find("#hdnTotalPrice").val();
       
        if (productId != undefined) {

            var jobProduct = {
                ProductId: productId,             
                Price: price,
                Quantity: quantity,
                DiscountPercentage: discountPerc,
                DiscountAmount: discountAmount,
                CGST_Perc: cGSTPerc,
                CGST_Amount: cGSTPercAmount,
                SGST_Perc: sGSTPerc,
                SGST_Amount: sGSTPercAmount,
                TotalPrice: totalPrice,
               
            };
            jobProductList.push(jobProduct);
        }
    });

    var requestData = {
        jobCardViewModel: jobCardViewModel,
        jobCardProductDetailViewModel: jobCardDetailViewModel,
        jobProductList: jobProductList
    };
    $.ajax({
        url: "../JobCard/AddEditJobCard",
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
                       window.location.href = "../JobCard/AddEditJobCard?jobCardId=" + data.trnId + "&AccessMode=3";
                   }, 2000);
                $("#btnSave").hide();
                $("#btnUpdateProduct").hide();
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
   
    $("#hdnJobCardId").val("0");
    $("#txtJobCardNo").val("");
    $("#txtJobCardDate").val($("#hdnCurrentDate").val());
    $("#txtTimeInhours").val("");
    $("#txtTimeInMinutes").val("");
    $("#txtDeliveryTime").val("");
    $("#txtDeliveryTimeMinutes").val("");
    $("#hdnCustomerId").val("0");
    $("#txtModelName").val("");
    $("#txtRegNo").val("");
    $("#txtFrameNo").val("");
    $("#txtEngineNo").val("");
    $("#txtDateOfSale").val($("#hdnCurrentDate").val());
    $("#txtKMSCovered").val("");
    $("#txtCouponNo").val("");
    $("#txtFuelLevel").val("");
    $("#txtEngineOilLevel").val("");
    $("#txtKeyNo").val("");
    $("#txtBatteryMakeNo").val("");
    $("#txtDamage").val("");
    $("#txtAccessories").val("");
    $("#ddlTypeOfService").val("");
    $("#txtESTDeliveryTime").val("");
    $("#txtESTDeliveryTimeMinutes").val("");
    $("#txCostOfRepair").val("");
    $("#txtCostOfParts").val("");
    $("#txtPreJobCardNo").val("");
    $("#txtPreSeviceDate").val("");
    $("#txtPreKey").val("");
    $("#txtMahenicName").val("");
    $("#txtStartTime").val("");
    $("#txtClosingTime").val("");
    $("#ddlApprovalStatus").val("");

}
//open master Customer pop up----------
function ShowHideProductModel() {
     $("#AddProductModel").modal();
    //CheckMasterPermission($("#hdnRoleId").val(), 13, 'AddProductModel');
}

function OpenCustomerMasterPopup(){
    
    $("#AddNewCustomer").modal();

}
function BindProductDetail(productId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Product/GetProductDetail",
        data: { productid: productId },
        dataType: "json",
        success: function (data) {
            $("#txtProductNameJob").val(data.ProductName);
            $("#hdnProductIdJob").val(data.Productid);
            $("#txtProductShortDesc").val(data.ProductShortDesc);
            $("#txtProductCode").val(data.ProductCode);
            $("#txtHSN_Code").val(data.HSN_Code);           
                $("#txtCGSTPerc").val(data.CGST_Perc);
                $("#txtSGSTPerc").val(data.SGST_Perc);
                //$("#txtIGSTPerc").val();
               
            
            

            $("#txtPrice").val(data.PurchasePrice);
            $("#txtQuantity").val("");
            $("#txtUOMName").val(data.UOMName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}


function AddProductGrid(action) {
    var productEntrySequence = 0;
    var flag = true;
    var hdnSequenceNo = $("#hdnJobSequenceNo");
    var txtProductName = $("#txtProductNameJob");
    var hdnJobCardDetailID = $("#hdnJobCardDetailID");
    var hdnProductId = $("#hdnProductIdJob");
    var txtProductCode = $("#txtProductCode");
    var txtQuantity = $("#txtQuantity");
    var txtPrice = $("#txtPrice");        
    var txtDiscountPerc = $("#txtDiscountPerc");
    var txtDiscountAmount = $("#txtDiscountAmount");  
    var txtCGSTPerc = $("#txtCGSTPerc");
    var txtCGSTPercAmount = $("#txtCGSTPercAmount");
    var txtSGSTPerc = $("#txtSGSTPerc");
    var txtSGSTPercAmount = $("#txtSGSTPercAmount");  
    var txtTotalPrice = $("#txtTotalPrice");

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
    if (txtPrice.val().trim() == "" || txtPrice.val().trim() == "0" || parseFloat(txtPrice.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter Price")
        txtPrice.focus();
        return false;
    }
    if (txtQuantity.val().trim() == "" || txtQuantity.val().trim() == "0" || parseFloat(txtQuantity.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter Quantity")
        txtQuantity.focus();
        return false;
    }
   

    if (txtDiscountPerc.val().trim() != "" && (parseFloat(txtDiscountPerc.val().trim()) < 0 || parseFloat(txtDiscountPerc.val().trim()) > 100)) {
        ShowModel("Alert", "Please enter correct discount %")
        txtDiscountPerc.focus();
        return false;
    }

    if (txtTotalPrice.val().trim() == "" || txtTotalPrice.val().trim() == "0" || parseFloat(txtTotalPrice.val().trim()) <= 0) {
        ShowModel("Alert", "Please enter correct Price and Quantity")
        txtQuantity.focus();
        return false;
    }
    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        productEntrySequence = 1;
    }

    var jobCardProductList = [];
    $('#tblProductList1 tr:not(:has(th))').each(function (i, row) {
        var $row = $(row);
        var jobCardDetailID = $row.find("#hdnJobCardDetailID").val();
        var sequenceNo = $row.find("#hdnJobSequenceNo").val();
        var productId = $row.find("#hdnProductIdJob").val();
        var productName = $row.find("#txtProductNameJob").val();
        var productCode = $row.find("#hdnProductCode").val();     
        var price = $row.find("#hdnPrice").val();
        var quantity = $row.find("#hdnQuantity").val();
        var discountPerc = $row.find("#hdnDiscountPerc").val();
        var discountAmount = $row.find("#hdnDiscountAmount").val();
        var cGSTPerc = $row.find("#hdnCGSTPerc").val();
        var cGSTPercAmount = $row.find("#hdnCGSTAmount").val();
        var sGSTPerc = $row.find("#hdnSGSTPerc").val();
        var sGSTPercAmount = $row.find("#hdnSGSTAmount").val();      
        var totalPrice = $row.find("#hdnTotalPrice").val();
       

        if (productId != undefined) {
            if (action == 1 || (hdnSequenceNo.val() != sequenceNo)) {

                if (productId == hdnProductId.val()) {
                    ShowModel("Alert", "Product already added !!!")
                    txtProductName.focus();
                    flag = false;
                    return false;
                }
               
                var jobCardProduct = {
                    JobCardDetailID: jobCardDetailID,
                    SequenceNo: sequenceNo,
                    ProductId: productId,
                    ProductName: productName,
                    ProductCode: productCode,                   
                    Price: price,
                    Quantity: quantity,
                    DiscountPercentage: discountPerc,
                    DiscountAmount: discountAmount,                   
                    TotalPrice: totalPrice,                   
                    CGST_Perc: cGSTPerc,
                    CGST_Amount: cGSTPercAmount,
                    SGST_Perc: sGSTPerc,
                    SGST_Amount: sGSTPercAmount,                                    
                };



                jobCardProductList.push(jobCardProduct);
                productEntrySequence = parseInt(productEntrySequence) + 1;
            }
            else if (hdnProductId.val() == productId && hdnSequenceNo.val() == sequenceNo) {
                var JobCardProductAddEdit = {
                    SequenceNo: hdnSequenceNo.val(),
                    JobCardDetailID: hdnJobCardDetailID.val(),
                    ProductId: hdnProductId.val(),
                    ProductName: txtProductName.val().trim(),
                    ProductCode: txtProductCode.val().trim(),                  
                    Price: txtPrice.val().trim(),
                    Quantity: txtQuantity.val().trim(),
                    DiscountPercentage: txtDiscountPerc.val().trim(),
                    DiscountAmount: txtDiscountAmount.val().trim(),                                                      
                    CGST_Perc: txtCGSTPerc.val(),
                    CGST_Amount: txtCGSTPercAmount.val(),
                    SGST_Perc: txtSGSTPerc.val(),
                    SGST_Amount: txtSGSTPercAmount.val(),
                    TotalPrice: txtTotalPrice.val().trim(),
                  
                };
                jobCardProductList.push(JobCardProductAddEdit);
                hdnSequenceNo.val("0");
            }
        }

    });


    if (action == 1 && (hdnSequenceNo.val() == "" || hdnSequenceNo.val() == "0")) {
        hdnSequenceNo.val(productEntrySequence);
    }
    if (action == 1) {

        var JobCardProductAddEdit = {
            SequenceNo: hdnSequenceNo.val(),
            JobCardDetailID: hdnJobCardDetailID.val(),
            ProductId: hdnProductId.val(),
            ProductName: txtProductName.val().trim(),
            ProductCode: txtProductCode.val().trim(),
            Price: txtPrice.val().trim(),
            Quantity: txtQuantity.val().trim(),
            DiscountPercentage: txtDiscountPerc.val().trim(),
            DiscountAmount: txtDiscountAmount.val().trim(),
            CGST_Perc: txtCGSTPerc.val(),
            CGST_Amount: txtCGSTPercAmount.val(),
            SGST_Perc: txtSGSTPerc.val(),
            SGST_Amount: txtSGSTPercAmount.val(),
            TotalPrice: txtTotalPrice.val().trim(),
        };
        jobCardProductList.push(JobCardProductAddEdit);
        hdnSequenceNo.val("0");
    }
    if (flag == true) {
        GetJobCardProduct(jobCardProductList);
    }

}

function EditProductRowGrid(obj) {

    var row = $(obj).closest("tr");
    var JobCardDetailID = $(row).find("#hdnJobCardDetailID").val();
    var sequenceNo = $(row).find("#hdnJobSequenceNo").val();
    var productId = $(row).find("#hdnProductIdJob").val();
    var productName = $(row).find("#txtProductNameJob").val();
    var productCode = $(row).find("#hdnProductCode").val();
    var price = $(row).find("#hdnPrice").val();
    var quantity = $(row).find("#hdnQuantity").val();
    var discountPerc = $(row).find("#hdnDiscountPerc").val();
    var discountAmount = $(row).find("#hdnDiscountAmount").val();   
    var totalPrice = $(row).find("#hdnTotalPrice").val(); 
    var cGSTPerc = $(row).find("#hdnCGSTPerc").val();
    var cGSTAmount = $(row).find("#hdnCGSTAmount").val();
    var sGSTPerc = $(row).find("#hdnSGSTPerc").val();
    var sGSTAmount = $(row).find("#hdnSGSTAmount").val();
   


    $("#hdnJobSequenceNo").val(sequenceNo);
    $("#txtProductNameJob").val(productName);
    $("#hdnJobCardDetailID").val(JobCardDetailID);
    $("#hdnProductIdJob").val(productId);
    $("#txtProductCode").val(productCode);  
    $("#txtPrice").val(price);
    $("#txtQuantity").val(quantity);
    $("#txtDiscountPerc").val(discountPerc);
    $("#txtDiscountAmount").val(discountAmount);   
    $("#txtTotalPrice").val(totalPrice);    
    $("#txtCGSTPerc").val(cGSTPerc);
    $("#txtCGSTPercAmount").val(cGSTAmount);
    $("#txtSGSTPerc").val(sGSTPerc);
    $("#txtSGSTPercAmount").val(sGSTAmount);
     
    $("#btnAddProduct").hide();
    $("#btnUpdateProduct").show();
    $("#txtProductNameJob").attr('readOnly', true);
    //ShowHideProductPanel(1);


}
function RemoveProductRowGrid(obj) {
    if (confirm("Do you want to remove selected Product?")) {
        var row = $(obj).closest("tr");      
        ShowModel("Alert", "Product Removed from List.");
        row.remove();
       
    }
}

function ShowHideProductPanel(action) {
    $("#txtProductNameJob").attr('readOnly', false);
        $("#txtProductNameJob").val("");
        $("#hdnProductIdJob").val("0");
        $("#hdnSIProductDetailId").val("0");
       
        $("#txtProductCode").val("");
        $("#txtProductShortDesc").val("");
        $("#txtPrice").val("");
       
        $("#txtQuantity").val("");
        $("#txtDiscountPerc").val("");
        $("#txtDiscountAmount").val("");      
        $("#txtTotalPrice").val("");
        $("#txtHSN_Code").val("");
        $("#txtCGSTPerc").val("");
        $("#txtCGSTPercAmount").val("");
        $("#txtSGSTPerc").val("");
        $("#txtSGSTPercAmount").val("");      
        $("#btnAddProduct").show();
        $("#btnUpdateProduct").hide();
   
}

function CalculateTotalCharges() {
    var quantity = $("#txtQuantity").val();
    var price = $("#txtPrice").val();
    var discountPerc = $("#txtDiscountPerc").val();
    var CGSTPerc = $("#txtCGSTPerc").val();
    var SGSTPerc = $("#txtSGSTPerc").val();
  
    var discountAmount = 0;
    var CGSTAmount = 0;
    var SGSTAmount = 0;
    var IGSTAmount = 0;  
    price = price == "" ? 0 : price;
    quantity = quantity == "" ? 0 : quantity;

    var totalPrice = parseFloat(price) * parseFloat(quantity);
    if (parseFloat(discountPerc) > 0) {
        discountAmount = ((parseFloat(totalPrice) * parseFloat(discountPerc)) / 100).toFixed(2)
    }
    $("#txtDiscountAmount").val(discountAmount);


    if (parseFloat(CGSTPerc) > 0) {

        CGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(CGSTPerc)) / 100).toFixed(2);
    }
    $("#txtCGSTPercAmount").val(CGSTAmount);


    if (parseFloat(SGSTPerc) > 0) {

        SGSTAmount = (((parseFloat(totalPrice) - parseFloat(discountAmount)) * parseFloat(SGSTPerc)) / 100).toFixed(2);

    }
    $("#txtSGSTPercAmount").val(SGSTAmount);
    $("#txtTotalPrice").val((parseFloat(totalPrice) - parseFloat(discountAmount) + parseFloat(CGSTAmount) +
        parseFloat(SGSTAmount) ).toFixed(2));

}