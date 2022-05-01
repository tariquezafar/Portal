$(document).ready(function () {
    //$("#txtPaymentNo").attr('readOnly', true);
    //$("#txtPaymentDate").attr('readOnly', true); 
    //$("#txtRefDate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    //$("#txtInvoiceNo").attr('readOnly', true);
    //$("#txtInvoiceDate").attr('readOnly', true);
    //$("#txtValueDate").attr('readOnly', true);
    //$("#txtCustomerCode").attr('readOnly', true);    
    //$("#txtCreatedBy").attr('readOnly', true);
    //$("#txtCreatedDate").attr('readOnly', true);
    //$("#txtModifiedBy").attr('readOnly', true);
    //$("#txtModifiedDate").attr('readOnly', true);
    //$("#txtRejectedDate").attr('readOnly', true);
     
    $("#txtVendorName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../VendorPayment/GetVendorAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.VendorName, value: item.VendorId, Address: item.Address, code: item.VendorCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtVendorName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtVendorName").val(ui.item.label);
            $("#hdnVendorId").val(ui.item.value);
           // $("#txtVendorCode").val(ui.item.code);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtVendorName").val("");
                $("#hdnVendorId").val("0");
             //   $("#txtVendorCode").val("");
                ShowModel("Alert", "Please select Vendor from List")

            }
            return false;
        }

    })
  .autocomplete("instance")._renderItem = function (ul, item) {
      return $("<li>")
        .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.Address + "</div>")
        .appendTo(ul);
  };

  
    $("#txtRefDate,#txtInvoiceDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $("#txtSearchFromDate,#txtSearchToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    BindFormType();
  

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnVendorFormTrnId = $("#hdnVendorFormTrnId");
    if (hdnVendorFormTrnId.val() != "" && hdnVendorFormTrnId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
           function () {
               GetVendorFormDetail(hdnVendorFormTrnId.val());
       }, 2000);
         
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            //$("input").attr('readOnly', true);
            //$("textarea").attr('readOnly', true);
            //$("select").attr('disabled', true);
            //$("#chkstatus").attr('disabled', true);
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

function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function OpenInvoiceSearchPopup() {
    $("#SearchInvoiceModel").modal(); 
}
function SearchInvoice() {
    var txtInvoiceNo = $("#txtSearchInvoiceNo");
    var txtCustomerName = $("#txtSearchVendorName");
    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { piNo: txtInvoiceNo.val().trim(), vendorName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), approvalStatus: "0", displayType:"" };
    $.ajax({
        url: "../VendorPayment/GetPIList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divInvoiceList").html("");
            $("#divInvoiceList").html(err);
        },
        success: function (data) {
            $("#divInvoiceList").html("");
            $("#divInvoiceList").html(data);
        }
    });
}
function SelectInvoice(invoiceId, saleinvoiceNo, invoiceDate, vendorId, vendorCode, vendorName) {
    $("#txtInvoiceNo").val(saleinvoiceNo);
    $("#hdnInvoiceId").val(invoiceId);
    $("#txtInvoiceDate").val(invoiceDate);
    $("#hdnVendorId").val(vendorId);
    //$("#txtVendorCode").val(customerCode);
    $("#txtVendorName").val(vendorName);
    $("#SearchInvoiceModel").modal('hide'); 
   
}

 
function GetVendorFormDetail(vendorFormTrnId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../VendorForm/GetVendorFormDetail",
        data: { vendorFormTrnId: vendorFormTrnId },
        dataType: "json",
        success: function (data) { 
            $("#txtInvoiceNo").val(data.InvoiceNo);
            $("#hdnInvoiceId").val(data.InvoiceId);
            $("#txtInvoiceDate").val(data.InvoiceDate);
            $("#hdnVendorId").val(data.VendorId);
            //$("#txtCustomerCode").val(data.CustomCode);
            $("#txtVendorName").val(data.VendorName);
            $("#ddlFormStatus").val(data.FormStatus);
            $("#ddlFormType").val(data.FormTypeId);
            $("#txtRefNo").val(data.RefNo);
            $("#txtRefDate").val(data.RefDate);
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            } 
            $("#txtAmount").val(data.Amount);           
            $("#txtRemarks").val(data.Remarks); 
            $("#btnAddNew").hide();
            $("#btnPrint").hide();
         
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

function BindFormType() {
    $.ajax({
        type: "GET",
        url: "../CustomerForm/GetFormTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlFormType").append($("<option></option>").val(0).html("-Select Form Type Desc-"));
            $.each(data, function (i, item) {
                $("#ddlFormType").append($("<option></option>").val(item.FormTypeId).html(item.FormTypeDesc));
            });
        },
        error: function (Result) {
            $("#ddlFormType").append($("<option></option>").val(0).html("-Select Form Type Desc-"));
        }
    });
}

function SaveData() {    
    var hdnVendorFormTrnId = $("#hdnVendorFormTrnId");
    var hdnInvoiceId = $("#hdnInvoiceId");
    var txtInvoiceNo = $("#txtInvoiceNo");
    var hdnVendorId = $("#hdnVendorId");
    var txtRefNo = $("#txtRefNo");
    var txtRefDate = $("#txtRefDate"); 
    var ddlFormStatus = $("#ddlFormStatus");
    var ddlFormType = $("#ddlFormType");
    var txtAmount = $("#txtAmount");   
    var txtRemarks = $("#txtRemarks");
    //var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtInvoiceNo.val().trim() == "") {
        ShowModel("Alert", "Please select Invoice No from list")
        txtInvoiceNo.focus();
        return false;
    }
    if (hdnVendorId.val() == "" || hdnVendorId.val() == "0") {
        ShowModel("Alert", "Please select vendor from list")
        txtCustomerName.focus();
        return false;
    }
   
   
    if (ddlFormStatus.val() == "" || ddlFormStatus.val() == "0") {
        ShowModel("Alert", "Please select Form Status")
        ddlFormStatus.focus();
        return false;
    }

    if (ddlFormType.val() == "" || ddlFormType.val() == "0") {
        ShowModel("Alert", "Please select Form Type Desc")
        ddlFormType.focus();
        return false;
    }

    var vendorformViewModel = {
        vendorFormTrnId: hdnVendorFormTrnId.val(),
        InvoiceId: hdnInvoiceId.val().trim(),        
        VendorId: hdnVendorId.val().trim(),
        RefNo: txtRefNo.val().trim(),
        RefDate: txtRefDate.val(),
        FormStatus: ddlFormStatus.val(),
        FormTypeId: ddlFormType.val(),
        Amount: txtAmount.val(),       
        Remarks: txtRemarks.val()
       
    };
     
    var requestData = { vendorFormViewModel: vendorformViewModel };
    $.ajax({
        url: "../VendorForm/AddEditVendorForm",
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
                       window.location.href = "../VendorForm/ListVendorForm";
                   }, 3000);

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

    $("#txtInvoiceNo").val("");
    $("#hdnVendorFormTrnId").val("0");
    $("#txtInvoiceDate").val("");
    $("#hdnInvoiceId").val("0");
    $("#txtVendorName").val("");
    $("#txtInvoiceNo").val("");
    $("#txtvendorCode").val("");  
   
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());


    $("#txtRefNo").val("");
    $("#txtRefDate").val("");

    $("#btnSave").show();
    $("#btnUpdate").hide();


}
 
 
 