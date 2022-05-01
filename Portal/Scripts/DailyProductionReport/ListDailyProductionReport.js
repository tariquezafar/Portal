$(document).ready(function () {

    $("#txtSearchFromDate,#txtSearchToDate").attr('readOnly', true);


    $("#txtSearchFromDate,#txtSearchToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
});

function ClearFields() {
    window.location.href = "../DailyProductionReport/ListDailyProductionReport";
}


function OpenPrintPopup() {
    if ($("#ddlProcessType").val() == null)
    {
        ShowModel("Alert", "Please select Process Type")
        return false;
    }
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
    var url = "../DailyProductionReport/DailyProductionReport?productName=" + $("#txtProductName").val() + "&processType=" + $("#ddlProcessType").val() + "&fromDate=" + $("#txtSearchFromDate").val() + "&toDate=" + $("#txtSearchToDate").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    var url = "../DailyProductionReport/DailyProductionReport?productName=" + $("#txtProductName").val() + "&processType=" + $("#ddlProcessType").val() + "&fromDate=" + $("#txtSearchFromDate").val() + "&toDate=" + $("#txtSearchToDate").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

}


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
                    return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode };
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
       
        $("#txtProductCode").val(ui.item.code);
        
        return false;
    },
    change: function (event, ui) {
        if (ui.item == null) {
            $("#txtProductName").val("");
            $("#hdnProductId").val("0");           
            $("#txtProductCode").val("");           
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



function OpenPrintPopup1() {
    if ($("#ddlProcessType").val() == null) {
        ShowModel("Alert", "Please select Process Type")
        return false;
    }
    $("#printModel1").modal();
    GenerateReportParameters1();
}

function ShowHidePrintOption1() {
    var reportOption = $("#ddlPrintOption1").val();
    if (reportOption == "PDF") {
        $("#btnPdf1").show();
        $("#btnExcel1").hide();
    }
    else if (reportOption == "Excel") {
        $("#btnExcel1").show();
        $("#btnPdf1").hide();
    }
}


function GenerateReportParameters1() {
    var url = "../DailyProductionReport/DailyProductionDetailReport?productName=" + $("#txtProductName").val() + "&processType=" + $("#ddlProcessType").val() + "&fromDate=" + $("#txtSearchFromDate").val() + "&toDate=" + $("#txtSearchToDate").val() + "&chassisno=" + $("#txtChasisNo").val() + "&reportType=PDF";
    $('#btnPdf1').attr('href', url);
    var url = "../DailyProductionReport/DailyProductionDetailReport?productName=" + $("#txtProductName").val() + "&processType=" + $("#ddlProcessType").val() + "&fromDate=" + $("#txtSearchFromDate").val() + "&toDate=" + $("#txtSearchToDate").val() + "&chassisno=" + $("#txtChasisNo").val() + "&reportType=Excel";
    $('#btnExcel1').attr('href', url);

}
function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}