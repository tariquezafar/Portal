$(document).ready(function () {

    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
        }
    });
    //SearchPayrollTds();
});
function ClearFields() {   
    //$("#ddlCategory").val("0");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());
    window.location.href = "../PayrollTdsSlab/ListPayrollTds";


}
function SearchPayrollTds() {
   
    var txtProductMainGroupName = $("#txtFromDate");
    var txtProductMainGroupCode = $("#txtToDate");
    var ddlCategory = $("#ddlCategory");

    var requestDataSearch = {
        txtProductMainGroupName: txtProductMainGroupName.val().trim(),
        txtProductMainGroupCode: txtProductMainGroupCode.val().trim(),
        ddlCategory: ddlCategory.val()
    };

    $.ajax({
        url: "../PayrollTdsSlab/GetPayrollTdsList",
        data: requestDataSearch,
        dataType: "html",
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