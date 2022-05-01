
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
});
 

 
function ClearFields()
{
   
    window.location.href = "../Service/GetServiceList";

 
    
}
function SearchService() {
    var txtServiceNo = $("#txtServiceNo");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    
    var requestData = { 
        serviceNo: txtServiceNo.val().trim(),
        approvalStatus: ddlApprovalStatus.val(),
        fromDate: txtFromDate.val(),
        toDate: txtToDate.val()
       
    };
    $.ajax({
        url: "../Service/GetServiceList",
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
