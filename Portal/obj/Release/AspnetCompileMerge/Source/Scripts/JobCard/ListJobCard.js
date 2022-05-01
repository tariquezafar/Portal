
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
   
    window.location.href = "../JobCard/ListJobCard";

 
    
}
function SearchJobCard() {
    var txtJobCardNo = $("#txtJobCardNo");
    var txtCustomerName = $("#txtCustomerName");
    var ddlApprovalStatus = $("#ddlApprovalStatus");
    var txtModelName = $("#txtModelName");
    var txtEngineNo = $("#txtEngineNo");
    var txtRegNo = $("#txtRegNo");
    var txtKeyNo = $("#txtKeyNo");   
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    
    var requestData = {
        jobCardNo: txtJobCardNo.val().trim(),
        customerName: txtCustomerName.val(),
        approvalStatus: ddlApprovalStatus.val(),
        modelName: txtModelName.val().trim(),     
        engineNo: txtEngineNo.val().trim(),
        regNo: txtRegNo.val(),
        keyNo: txtKeyNo.val().trim(), 
        fromDate: txtFromDate.val(),
        toDate: txtToDate.val()
       
    };
    $.ajax({
        url: "../JobCard/GetJobCardList",
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
