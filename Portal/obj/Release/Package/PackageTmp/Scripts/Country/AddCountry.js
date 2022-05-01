
$(document).ready(function () { 
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnCountryID = $("#hdnCountryID");
    if (hdnCountryID.val() != "" && hdnCountryID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetCountryDetail(hdnCountryID.val());
        
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
    $("#txtCountryName").focus();
        


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


function GetCountryDetail(countryId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../Country/GetCountryDetail",
        data: { countryId: countryId },
        dataType: "json",
        success: function (data) {
            $("#txtCountryName").val(data.CountryName);
            $("#txtCountryCode").val(data.CountryCode);
            if (data.Country_Status == true) {
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
    var txtCountryName = $("#txtCountryName");
   var hdnCountryID = $("#hdnCountryID");
    var txtCountryCode = $("#txtCountryCode");  
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtCountryName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Country Name")
        txtCountryName.focus();
        return false;
    }
    if (txtCountryCode.val().trim() == "") {
        ShowModel("Alert", "Please Enter Country Short Code")
        txtCountryCode.focus();
        return false;
    }

    if (txtCountryCode.val().length != 3)
    {
        ShowModel("Alert", "Please Enter Country Code Maximum 3 Word ")
        txtCountryCode.focus();
        return false; 
    }
     
    
    var countryViewModel = {
        CountryId: hdnCountryID.val(),
        CountryName: txtCountryName.val().trim(),
        CountryCode: txtCountryCode.val().trim(),
        Country_Status: chkstatus
        
    };
    var requestData = { countryViewModel: countryViewModel };
    $.ajax({
        url: "../Country/AddEditCountry",
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
 
    $("#txtCountryName").val("");
    $("#txtCountryCode").val("");
    $("#chkstatus").prop("checked", true);
    
}
