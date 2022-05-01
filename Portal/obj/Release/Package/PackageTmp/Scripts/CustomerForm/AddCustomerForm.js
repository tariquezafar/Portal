$(document).ready(function () {
   
    $("#txtPaymentNo").attr('readOnly', true);
    $("#txtPaymentDate").attr('readOnly', true); 
    $("#txtRefDate").attr('readOnly', true);
    $("#txtSearchFromDate").attr('readOnly', true);
    $("#txtSearchToDate").attr('readOnly', true);
    $("#txtInvoiceNo").attr('readOnly', true);
    $("#txtInvoiceDate").attr('readOnly', true);
    $("#txtValueDate").attr('readOnly', true);
    $("#txtCustomerCode").attr('readOnly', true);    
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true);
     
    $("#txtCustomerName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../CustomerPayment/GetCustomerAutoCompleteList",
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
        .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.primaryAddress + "</div>")
        .appendTo(ul);
  };

  
    

    $("#txtPaymentDate,#txtRefDate,#txtValueDate").datepicker({
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
    var hdnCustomerFormTrnId = $("#hdnCustomerFormTrnId");
    if (hdnCustomerFormTrnId.val() != "" && hdnCustomerFormTrnId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
           function () {
               GetCustomerFormDetail(hdnCustomerFormTrnId.val());
       }, 2000);
        $(function () {
            $('input#txtCustomerName').blur();
        });
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

  

    //$("#txtCustomerName").focus();

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
    var txtCustomerName = $("#txtSearchCustomerName");
    var txtRefNo = $("#txtSearchRefNo");
    var txtFromDate = $("#txtSearchFromDate");
    var txtToDate = $("#txtSearchToDate");

    var requestData = { saleinvoiceNo: txtInvoiceNo.val().trim(), customerName: txtCustomerName.val().trim(), refNo: txtRefNo.val().trim(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), displayType: "Popup", approvalStatus: "Final" };
    $.ajax({
        url: "../CustomerForm/GetSaleInvoiceList",
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
function SelectInvoice(invoiceId, saleinvoiceNo, invoiceDate, customerId, customerCode, customerName) {
    $("#txtInvoiceNo").val(saleinvoiceNo);
    $("#hdnInvoiceId").val(invoiceId);
    $("#txtInvoiceDate").val(invoiceDate);
    $("#hdnCustomerId").val(customerId);
    $("#txtCustomerCode").val(customerCode);
    $("#txtCustomerName").val(customerName);
    $("#SearchInvoiceModel").modal('hide'); 
   
}







 
function GetCustomerFormDetail(customerFormTrnId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../CustomerForm/GetCustomerFormDetail",
        data: { customerFormTrnId: customerFormTrnId },
        dataType: "json",
        success: function (data) { 
            $("#txtInvoiceNo").val(data.InvoiceNo);
            $("#hdnInvoiceId").val(data.InvoiceId);
            $("#txtInvoiceDate").val(data.InvoiceDate);
            $("#hdnCustomerId").val(data.CustomerId);
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#txtCustomerName").val(data.CustomerName);
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
            $("#btnAddNew").show();
            $("#btnPrint").show();
            if (data.CustomerForm_Status == true) {
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


$(function () {
    $("#dialogForQuotationNo").dialog({
        autoOpen: false,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
    });

    $("#opener").on("click", function () {
        $("#dialogForQuotationNo").dialog("open");
    });
});
function SaveData() {    
    var hdnCustomerFormTrnId = $("#hdnCustomerFormTrnId");   
    var hdnInvoiceId = $("#hdnInvoiceId");
    var txtInvoiceNo = $("#txtInvoiceNo");
    var hdnCustomerId = $("#hdnCustomerId");  
    var txtRefNo = $("#txtRefNo");
    var txtRefDate = $("#txtRefDate"); 
    var ddlFormStatus = $("#ddlFormStatus");
    var ddlFormType = $("#ddlFormType");
    var txtAmount = $("#txtAmount");   
    var txtRemarks = $("#txtRemarks");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtInvoiceNo.val().trim() == "") {
        ShowModel("Alert", "Please select Invoice No from list")
        txtInvoiceNo.focus();
        return false;
    }
    if (hdnCustomerId.val() == "" || hdnCustomerId.val() == "0") {
        ShowModel("Alert", "Please select Party from list")
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

    var customerformViewModel = {
        CustomerFormTrnId: hdnCustomerFormTrnId.val(),       
        InvoiceId: hdnInvoiceId.val().trim(),        
        CustomerId: hdnCustomerId.val().trim(),       
        RefNo: txtRefNo.val().trim(),
        RefDate: txtRefDate.val(),
        FormStatus: ddlFormStatus.val(),
        FormTypeId: ddlFormType.val(),
        Amount: txtAmount.val(),       
        Remarks: txtRemarks.val(),
        CustomerForm_Status: chkstatus
    };
    var accessMode = 1;//Add Mode
    if (hdnCustomerFormTrnId.val() != null && hdnCustomerFormTrnId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { customerformViewModel: customerformViewModel };
    $.ajax({
        url: "../CustomerForm/AddEditCustomerForm?AccessMode=" + accessMode + "",
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
                       window.location.href = "../CustomerForm/AddEditCustomerForm?customerFormTrnId=" + data.trnId + "&AccessMode=2";
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

    $("#txtPaymentNo").val("");
    $("#hdnPaymentTrnId").val("0");
    $("#txtPaymentDate").val($("#hdnCurrentDate").val());
    $("#hdnInvoiceId").val("0");
    $("#txtCustomerName").val("");
    $("#txtInvoiceNo").val("");
    $("#txtCustomerCode").val("");  
   
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());


    $("#txtRefNo").val("");
    $("#txtRefDate").val("");

    $("#btnSave").show();
    $("#btnUpdate").hide();


}
 
 
 