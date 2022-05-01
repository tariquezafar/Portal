
$(document).ready(function () {

   
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnHSNID = $("#hdnHSNID");
    if (hdnHSNID.val() != "" && hdnHSNID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetHSNDetail(hdnHSNID.val());
        
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true); 
            $("#chkstatus").attr('disabled', true);
        }
        else
        {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else
    {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }
   
        


});
$(".alpha-only").on("input", function () {
    var regexp = /[^a-zA-Z]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".alpha-space-only").on("input", function () {
    var regexp = /[^a-zA-Z\s]+$/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".numeric-only").on("input", function () {
    var regexp = /\D/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});
$(".alpha-numeric-only").on("input", function () {
    var regexp = /[^a-zA-Z0-9]/g;
    if ($(this).val().match(regexp)) {
        $(this).val($(this).val().replace(regexp, ''));
    }
});


function GetHSNDetail(hSNID) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../HSN/GetHSNDetail",
        data: { hSNID: hSNID },
        dataType: "json",
        success: function (data) {          
            $("#hdnHSNID").val(data.HSNID);
            $("#txtHSNCode").val(data.HSNCode);
            $("#txtCGSTPerc").val(data.CGST_Perc);
            $("#txtSGSTPerc").val(data.SGST_Perc);
            $("#txtIGSTPerc").val(data.IGST_Perc);
            if (data.HSN_Status == true) {
                $("#chkstatus").attr("checked", true);
            }
            else {
                $("#chkstatus").attr("checked", false);
            }
         
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    
}

function SaveData()
{
    var txtHSNCode = $("#txtHSNCode");
    var hdnHSNID = $("#hdnHSNID");
    var txtCGSTPerc = $("#txtCGSTPerc");
    var txtSGSTPerc = $("#txtSGSTPerc");
    var txtIGSTPerc = $("#txtIGSTPerc");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    if (txtHSNCode.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter HSN Code.")
        txtHSNCode.focus();
        return false;
    }
    if (txtCGSTPerc.val().trim() == "" || txtCGSTPerc.val().trim() == "0") {
        ShowModel("Alert", "Please Enter CGST (%)")
        txtCGSTPerc.focus();
        return false;
    }
    if (txtSGSTPerc.val().trim() == "" || txtSGSTPerc.val().trim() == "0") {
        ShowModel("Alert", "Please Enter SGST (%)")
        txtSGSTPerc.focus();
        return false;
    }
    
    var hSNViewModel = {
        HSNID: hdnHSNID.val(),
        HSNCode: txtHSNCode.val().trim(),
        CGST_Perc: txtCGSTPerc.val().trim(),
        SGST_Perc: txtSGSTPerc.val().trim(),
        IGST_Perc: txtIGSTPerc.val().trim(),
        HSN_Status: chkstatus
        
    };
    var accessMode = 1;//Add Mode
    if (hdnHSNID.val() != null && hdnHSNID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { hSNViewModel: hSNViewModel };
    $.ajax({
        url: "../HSN/AddEditHSN?AccessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify( requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status=="SUCCESS")
            {
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
               function () {
                   window.location.href = "../HSN/ListHSN";
               }, 2000);
                $("#btnSave").show();
                $("#btnUpdate").hide();
            }
            else
            {
                ShowModel("Error", data.message)
            }
        },
        error: function (err) {
            ShowModel("Error", err)
        }
    });

}
function ShowModel(headerText,bodyText)
{
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields()
{
    $("#hdnHSNID").val("0");
    $("#txtHSNCode").val("");
    $("#txtCGSTPerc").val("");
    $("#txtSGSTPerc").val("");
    $("#txtIGSTPerc").val("");
    $("#chkstatus").prop("checked", true);
    
}
