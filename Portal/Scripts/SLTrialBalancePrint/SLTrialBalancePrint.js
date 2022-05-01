
$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtAsOnDate").attr('readOnly', true);    
    $("#txtAsOnDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            GenerateReportParameters();
        }
    });
    
    BindSLTypeList();
    $("#txtGLHead").attr('readOnly', true);
    $("#txtGLHead").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SubLedgerPrint/GetGLAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, slTypeId: $("#ddlSLType").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.GLHead, value: item.GLId, code: item.GLCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtGLHead").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtGLHead").val(ui.item.label);
            $("#hdnGLId").val(ui.item.value);
            GenerateReportParameters();
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtGLHead").val("");
                $("#hdnGLId").val("0");
                ShowModel("Alert", "Please select GL from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b></div>")
      .appendTo(ul);
};


    var url = "../SLTrialBalancePrint/SLTrialBalanceReport?asOnDate=" + $("#txtAsOnDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../SLTrialBalancePrint/SLTrialBalanceReport?asOnDate=" + $("#txtAsOnDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
    $('#btnPrintExcel').attr('href', url);
});


function ClearFields() {
    $("#ddlSLType").val("0");
    $("#txtGLHead").val("ALL");
    $("#hdnGLId").val("0");
    $("#txtAsOnDate").val($("#hdnFromDate").val());
    var url = "../SLTrialBalancePrint/SLTrialBalanceReport?asOnDate=" + $("#txtAsOnDate").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../SLTrialBalancePrint/SLTrialBalanceReport?asOnDate=" + $("#txtAsOnDate").val() + "&reportType=Excel";
    $('#btnPrintExcel').attr('href', url);

}
function BindSLTypeList() {
    $.ajax({
        type: "GET",
        url: "../SL/GetSLTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSLType").append($("<option></option>").val(0).html("-Select SL Type-"));
            $.each(data, function (i, item) {
                $("#ddlSLType").append($("<option></option>").val(item.SLTypeId).html(item.SLTypeName));
            });
        },
        error: function (Result) {
            $("#ddlSLType").append($("<option></option>").val(0).html("-Select SL Type-"));
        }
    });
}
function EnableDisableGLSL(obj) {
    var ddlSLType = $(obj);
    var txtGLHead = $("#txtGLHead");
    var hdnGLId = $("#hdnGLId");
    
    if (ddlSLType.val() == "" || ddlSLType.val() == "0") {
        $("#txtGLHead").attr('readOnly', true);
        txtGLHead.val("All");
        hdnGLId.val("0");
        
    }
    else {
        $("#txtGLHead").attr('readOnly', false);
    }
}
function SearchBankVoucher() {
    var ddlSLType = $("#ddlSLType");
    var hdnGLId = $("#hdnGLId");
    var txtFromDate = $("#txtAsOnDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    
    var requestData = { slTypeId: ddlSLType.val(), glId: hdnGLId.val(), asOnDate: txtFromDate.val(), companyBranchId: ddlCompanyBranch.val() };

    
    $.ajax({
        url: "../SLTrialBalancePrint/SLTrialBalanceGenerate",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                GenerateReportParameters();
                ShowPrintModel();
                
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
function ShowPrintModel() {
    $("#printModel").modal();
    
}
function GenerateReportParameters() {
    
    var url = "../SLTrialBalancePrint/SLTrialBalanceReport?asOnDate=" + $("#txtAsOnDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
        $('#btnPrintPDF').attr('href', url);
        url = "../SLTrialBalancePrint/SLTrialBalanceReport?asOnDate=" + $("#txtAsOnDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
        $('#btnPrintExcel').attr('href', url);
    
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("All Company Branch"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("All Company Branch"));
        }
    });
}
//End Code