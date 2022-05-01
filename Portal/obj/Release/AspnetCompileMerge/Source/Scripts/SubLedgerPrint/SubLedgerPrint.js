
$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            GenerateReportParameters();
        }
    });
    BindSLTypeList();
    $("#txtGLHead,#txtSLHead").attr('readOnly', true);
    $("#txtGLHead").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SubLedgerPrint/GetGLAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, slTypeId: $("#ddlSLType").val()},
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

    $("#txtSLHead").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SubLedgerPrint/GetSLAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, slTypeId: $("#ddlSLType").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.SLHead, value: item.SLId, code: item.SLCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtSLHead").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtSLHead").val(ui.item.label);
            $("#hdnSLId").val(ui.item.value);
            GenerateReportParameters();
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtSLHead").val("All");
                $("#hdnSLId").val("0");
                ShowModel("Alert", "Please select SL from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b></div>")
      .appendTo(ul);
};



    var url = "../SubLedgerPrint/SubLedgerWoFySingleGLReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../SubLedgerPrint/SubLedgerWoFySingleGLReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
    $('#btnPrintExcel').attr('href', url);
});


function ClearFields() {
    $("#ddlSLType").val("0");
    $("#txtSLHead").val("ALL");
    $("#hdnSLId").val("0");
    $("#txtGLHead").val("ALL");
    $("#hdnGLId").val("0");
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
    var url = "../SubLedgerPrint/SubLedgerWoFySingleGLReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=PDF";
    $('#btnPrintPDF').attr('href', url);
    url = "../SubLedgerPrint/SubLedgerWoFySingleGLReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&reportType=Excel";
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
function EnableDisableGLSL(obj)
{
    var ddlSLType = $(obj);
    var txtGLHead = $("#txtGLHead");
    var hdnGLId = $("#hdnGLId");
    var txtSLHead = $("#txtSLHead");
    var hdnSLId = $("#hdnSLId");

    if (ddlSLType.val()=="" || ddlSLType.val()=="0")
    {
        $("#txtGLHead,#txtSLHead").attr('readOnly', true);
        txtGLHead.val("All");
        hdnGLId.val("0");
        txtSLHead.val("All");
        hdnSLId.val("0");
    }
    else
    {
        $("#txtGLHead,#txtSLHead").attr('readOnly', false);
    }
}
function SearchBankVoucher() {
    var ddlSLType = $("#ddlSLType");
    var hdnGLId = $("#hdnGLId");
    var hdnSLId = $("#hdnSLId");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (ddlSLType.val() == undefined || ddlSLType.val() == "" || ddlSLType.val() == "0") {
        ShowModel("Alert", "Please select SL Type First")
        return false;
    }

    if (hdnGLId.val() == undefined || hdnGLId.val() == "") {
        ShowModel("Alert", "Please select correct GL")
        return false;

    }
    if (hdnSLId.val() == undefined || hdnSLId.val() == "") {
        ShowModel("Alert", "Please select correct SL")
        return false;
    }

    var requestData = { slTypeId: ddlSLType.val(), glId: hdnGLId.val(), slId: hdnSLId.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), companyBranchId: ddlCompanyBranch.val() };
    $.ajax({
        url: "../SubLedgerPrint/SubLedgerGenerate",
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
    if ($("#hdnGLId").val() != "0" && $("#hdnGLId").val() != "") {
        var url = "../SubLedgerPrint/SubLedgerWoFySingleGLReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
        $('#btnPrintPDF').attr('href', url);
        url = "../SubLedgerPrint/SubLedgerWoFySingleGLReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
        $('#btnPrintExcel').attr('href', url);
    }
    else
    {
        var url = "../SubLedgerPrint/SubLedgerWoFyAllGLReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
        $('#btnPrintPDF').attr('href', url);
        url = "../SubLedgerPrint/SubLedgerWoFyAllGLReport?fromDate=" + $("#txtFromDate").val() + "&toDate=" + $("#txtToDate").val() + "&CompanyBranch=" + $("#ddlCompanyBranch  option:selected").text() + "&CompanyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
        $('#btnPrintExcel').attr('href', url);
    }
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