
$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtAssemblyName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../ProductBOM/GetProductAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, assemblyType: $("#ddlAssemblyType").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ProductName, value: item.Productid, desc: item.ProductShortDesc, code: item.ProductCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtAssemblyName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtAssemblyName").val(ui.item.label);
            $("#hdnAssemblyId").val(ui.item.value);
            
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtAssemblyName").val("");
                $("#hdnAssemblyId").val("0");              
                ShowModel("Alert", "Please select Assembly Name")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b><br>" + item.desc + "</div>")
      .appendTo(ul);
};
  
});

function ClearFields()
{
    $("#txtProductName").val("");
    $("#ddlAssemblyType").val("0");
    
    
}
function SearchAssemblyList() {

    var txtProductName = $("#txtProductName");
    var ddlAssemblyType = $("#ddlAssemblyType");
    
    var requestData = { assemblyName: txtProductName.val().trim(), assemblyType: ddlAssemblyType.val() };
    $.ajax({
        url: "../ProductBOM/GetAssemblyList",
        data: requestData,
        dataType: "html",
        asnc: true,
        type: "GET",
        error: function (err) {
            $("#divList").html("");
            $("#divList").html(err);
        },
        success: function (data) {
            $("#divList").html("");
            $("#divList").html(data);

        }
    });
}

function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function OpenPrintPopup() {
    $("#printModel").modal();
    GenerateReportParameters();
   
}
function ShowHidePrintOption() {
    var reportOption = $("#ddlPrintOption2").val();
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
    var url = "../ProductBOM/Report?assemblyId=" + $("#hdnAssemblyId").val() + "&assemblyType=" + $("#ddlAssemblyType").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    url = "../ProductBOM/Report?assemblyId=" + $("#hdnAssemblyId").val() + "&assemblyType=" + $("#ddlAssemblyType").val() + "&companyBranchId=" + $("#ddlCompanyBranch").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);
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
