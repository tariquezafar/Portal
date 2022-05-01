$(document).ready(function () {
    BindCountryList();
    function BindCountryList() {
        $.ajax({
            type: "GET",
            url: "../Company/GetCountryList",
            data: "{}",
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlCountry").append($("<option></option>").val(0).html("-Select Country-"));
                $.each(data, function (i, item) {

                    $("#ddlCountry").append($("<option></option>").val(item.CountryId).html(item.CountryName));
                });
            },
            error: function (Result) {
                $("#ddlCountry").append($("<option></option>").val(0).html("-Select Country-"));
            }
        });
    }
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnStateId = $("#hdnStateId");
    if (hdnStateId.val() != "" && hdnStateId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetStateDetail(hdnStateId.val());
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
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
    $("#txtStateName").focus();
 


});
//$(".alpha-only").on("keydown", function (event) {
//    // Allow controls such as backspace
//    var arr = [8, 16, 17, 20, 35, 36, 37, 38, 39, 40, 45, 46];

//    // Allow letters
//    for (var i = 65; i <= 90; i++) {
//        arr.push(i);
//    }

//    // Prevent default if not in array
//    if (jQuery.inArray(event.which, arr) === -1) {
//        event.preventDefault();
//    }
//});

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
 

function GetStateDetail(stateId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../State/GetStateDetail",
        data: { stateId: stateId },
        dataType: "json",
        success: function (data) { 
            $("#txtStateName").val(data.StateName);
            $("#txtStateCode").val(data.StateCode); 
            $("#ddlCountry").val(data.CountryId);
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    
}

function SaveData()
{
    var txtStateName = $("#txtStateName");
    var hdnStateId = $("#hdnStateId"); 
    var txtStateCode = $("#txtStateCode"); 
    var ddlCountry = $("#ddlCountry");
   
    

    if (txtStateName.val().trim() == "")
    {
        ShowModel("Alert","Please enter State Name")
        txtStateName.focus();
        return false;
    }
    if (txtStateCode.val().trim() == "" ) {
        ShowModel("Alert", "Please enter State Short Code")
        txtStateCode.focus();
        return false;
    }

    if (txtStateCode.val().length != 3) {
        ShowModel("Alert", "Please Enter State Code Maximum 3 Word ")
        txtStateCode.focus();
        return false;
    }
    if (ddlCountry.val() == "" || ddlCountry.val() == 0) {
        ShowModel("Alert", "Please Select Country ")
        ddlCountry.focus();
        return false;
    }

    var stateViewModel = {
        StateId: hdnStateId.val(), StateName: txtStateName.val().trim(), StateCode: txtStateCode.val().trim(),
        CountryId: ddlCountry.val() 
    };
    var requestData ={ stateViewModel: stateViewModel };
    $.ajax({
        url: "../State/AddEditState",
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
     $("#txtStateName").val("");
     $("#txtStateCode").val("");
     $("#hdnStateID").val("0");
     $("#hdnAccessMode").val("0");
     $("#ddlCountry").val("0");
    
}
