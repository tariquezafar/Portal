
$(document).ready(function () {
  
});


function ClearFields()
{
  
    window.location.href = "../HSN/ListHSN";
}
function SearchManufacturer() {
    var txtHSNCode = $("#txtHSNCode");
    var ddlStatus = $("#ddlStatus");
    
    var requestData = {       
        hSNCode: txtHSNCode.val().trim(),
        hsnStatus: ddlStatus.val()
    };
    $.ajax({
        url: "../HSN/GetHSNList",
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
        }
    });
}
