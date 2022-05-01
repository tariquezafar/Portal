
$(document).ready(function () {
   
    $("#txtWorkOrderNo").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../WorkOrder/GetWorkOrderAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.WorkOrderNo, value: item.WorkOrderId };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtWorkOrderNo").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtWorkOrderNo").val(ui.item.label);
            $("#hdnWorkOrderID").val(ui.item.value);
            GenerateReportParameters();
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtWorkOrderNo").val("");
                $("#hdnWorkOrderID").val("0");
                ShowModel("Alert", "Please select Work Order No.")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + "</div>")
      .appendTo(ul);
};
    GenerateReportParameters();
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

function GenerateReportParameters() {   
    var url = "../WorkOrder/WIPReport?workOrderid=" + $("#hdnWorkOrderID").val() + "&reportType=PDF";
    $('#btnExport').attr('href', url);
    var url = "../WorkOrder/WIPReport?workOrderid=" + $("#hdnWorkOrderID").val() + "&reportType=PDF";
    $('#btnExcel').attr('href', url);


}
