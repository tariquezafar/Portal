
$(document).ready(function () {
    $("#txtPreviousFinYearCode").attr('readOnly', true);
    $("#txtCurrentFinYearCode").attr('readOnly', true);
    GetFinYearDetail();
    BindCompanyBranchList();
    $("#btnSave").hide();
    
});

function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
}

function ClearFields()
{
    $("#txtAssetTypeName").val("");
    $("#ddlStatus").val("");
}

function GetFinYearDetail() {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SLBalanceTransfer/GetFinYearDetail",
        data: { },
        dataType: "json",
        success: function (data) {
            $("#txtPreviousFinYearCode").val(data.PreviousFinYearCode);
            $("#txtCurrentFinYearCode").val(data.CurrentFinYearCode);
            $("#hdnStartDate").val(data.StartDate);
            $("#hdnEndDate").val(data.EndDate);
            $("#hdnPreviousFinYearId").val(data.PreviousFinYearId);
            $("#hdnCurrentFinYearID").val(data.CurrentFinYearID);
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

function GetSLBalanceTransfer() {   
    var hdnEndDate = $("#hdnEndDate");
    var hdnPreviousFinYearId = $("#hdnPreviousFinYearId");
    var requestData = {
        finYearId: hdnPreviousFinYearId.val(),        
        endDate: hdnEndDate.val()
    };
    $.ajax({
        url: "../SLBalanceTransfer/GetSLBalanceTransfer",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divList").html("");
            $("#divList").html(err);
        },
        success: function (data) {
            $("#divList").html("");
            $("#divList").html(data);
            $("#btnSave").show();
        }
    });
}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function SaveData() {
    if (confirm("Are you sure to Transfer SL Closing Balance.")) {
        var hdnTransferId = $("#hdnSLTransferId");
        var hdnPreviousFinYearId = $("#hdnPreviousFinYearId");
        var hdnCurrentFinYearID = $("#hdnCurrentFinYearID");
        var ddlCompanyBranch = $("#ddlCompanyBranch");

        if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
            ShowModel("Alert", "Please select Company Branch")
            ddlCompanyBranch.focus();
            return false;
        }


        var transferClosingBalanceViewModel = {
            FromFinYearID: hdnPreviousFinYearId.val(),
            ToFinYearID: hdnCurrentFinYearID.val(),
            CompanyBranchId: ddlCompanyBranch.val(),
        };

        var sLDetailViewModelList = [];

        $('#tblFinYearProductList tr').each(function (i, row) {
            var $row = $(row);
            var gLId = $row.find("#hdnGLId").val();
            var sLId = $row.find("#hdnSLId").val();

            var closingBalanceDebit = $row.find("#hdnClosingBalanceDebit").val();
            var closingBalanceCredit = $row.find("#hdnClosingBalanceCredit").val();
            if (gLId != undefined) {

                var sLDetailOpening = {

                    GLId: gLId,
                    SLId: sLId,
                    OpeningBalanceDebit: closingBalanceDebit,
                    OpeningBalanceCredit: closingBalanceCredit
                };
                sLDetailViewModelList.push(sLDetailOpening);
            }
        });



        var accessMode = 1;//Add Mode
        if (hdnTransferId.val() != null && hdnTransferId.val() != 0) {
            accessMode = 2;//Edit Mode
        }



        var requestData = { sLtransferClosingBalanceViewModel: transferClosingBalanceViewModel, sLDetailViewModel: sLDetailViewModelList };
        $.ajax({
            url: "../SLBalanceTransfer/SLBalanceTransfer?AccessMode=" + accessMode + "",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (data) {
                if (data.message == "SL Closing Balance Transfer Successfully") {
                    ShowModel("Alert", data.message);

                    setTimeout(
                function () {
                    window.location.href = "../SLBalanceTransfer/ListSLBalanceTransfer";
                }, 2000)
                   
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
    var url = "../BalanceTransfer/GenerateTransferReport?companyBranchId=" + $("#ddlCompanyBranch").val() + "&fromDate=" + $("#hdnStartDate").val() + "&toDate=" + $("#hdnEndDate").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    url = "../BalanceTransfer/GenerateTransferReport?companyBranchId=" + $("#ddlCompanyBranch").val() + "&fromDate=" + $("#hdnStartDate").val() + "&toDate=" + $("#hdnEndDate").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

}

function ReversedClosingTransfer() {
    if (confirm("Are you sure to Reverse Transfer Closing Balance.")) {
        
        var hdnCurrentFinYearID = $("#hdnCurrentFinYearID");

        var requestData = {
            toFinYearID: hdnCurrentFinYearID.val()          
        };
        $.ajax({
            url: "../BalanceTransfer/ReversedClosingTransfer",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (data) {
                if (data.message == "Balance Transfer Reversed Created Successfully") {
                    ShowModel("Alert", data.message);
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
}