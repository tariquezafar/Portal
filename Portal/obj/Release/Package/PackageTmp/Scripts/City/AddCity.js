$(document).ready(function () {
    $("#ddlCountry").html("");
    BindCountryList();
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
   
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnCityId = $("#hdnCityId");
    
    
    if (hdnCityId.val() != "" && hdnCityId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetCityDetail(hdnCityId.val());
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
        BindCountryList();
    }
    $("#txtCityName").focus();
 


});

function BindCountryList() {
    
    $.ajax({
        type: "GET",
        url: "../City/GetCountryList",
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



function BindStateList(stateId) {

    var countryId = $("#ddlCountry option:selected").val();
    //alert(stateId);
    $("#ddlState").val(0);
    $("#ddlState").html("");
    if (countryId != undefined && countryId != "" && countryId != "0") {
        var data = { countryId: countryId };
        $.ajax({
            type: "GET",
            url: "../City/GetStateList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {

                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
                $.each(data, function (i, item) {
                    $("#ddlState").append($("<option></option>").val(item.StateId).html(item.StateName));
                });
                $("#ddlState").val(stateId);

            },
            error: function (Result) {
                $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
            }
        });
    }
    else {

        $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
    }

}
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
 

function GetCityDetail(cityId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../City/GetCityDetail",
        data: { cityId: cityId },
        dataType: "json",
        success: function (data) { 
            $("#txtCityName").val(data.CityName);
            $("#txtPopulation").val(data.Population);
            $("#txtArea").val(data.Area);
            $("#txtRailway").val(data.Railway);
            $("#txtAirport").val(data.Airport);
            $("#txtPointOfInterest").val(data.PointOfInterest);
            $("#ddlCityClass").val(data.CityClass);
            $("#txtPopFactor").val(data.PopFactor);
            $("#ddlCountry").val(data.CountryId);
            BindStateList(data.StateId);
            $("#ddlState").val(data.StateId);
            $("#chkstatus").val(data.CityStatus);
            
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
    
}

function SaveData()
{
    var txtCityName = $("#txtCityName");
    var hdnCityId = $("#hdnCityId"); 
    //var txtStateCode = $("#txtStateCode"); 
    var ddlCountry = $("#ddlCountry");
    var ddlState = $("#ddlState");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    var txtPopulation = $("#txtPopulation");
    var txtArea = $("#txtArea");
    var txtRailway = $("#txtRailway");
    var txtAirport = $("#txtAirport");
    var txtPointOfInterest = $("#txtPointOfInterest");
    var ddlCityClass = $("#ddlCityClass");
    var txtPointOfInterest = $("#txtPointOfInterest");
    var txtPopFactor = $("#txtPopFactor");

    if (ddlCountry.val().trim() == "0") {
        ShowModel("Alert", "Please select country name")
        ddlCountry.focus();
        return false;
    }
    if (ddlState.val().trim() == "0") {

        ShowModel("Alert", "Please select state name")
        ddlState.focus();
        return false;
    }
      if (txtCityName.val().trim() == "")
      {
          
        ShowModel("Alert","Please enter city name")
        txtCityName.focus();
        return false;
      }
     
      if (ddlCityClass.val() == "0")
      {
          ShowModel("Alert", "Please Enter City Class")
          ddlCityClass.focus();
          return false;
         
      }
      var population=parseFloat($("#txtPopulation").val());
      var railway=parseFloat($("#txtRailway").val());
      var airport=parseFloat($("#txtAirport").val());
      var poi =parseFloat( $("#txtPointOfInterest").val());
      var popfactor=parseFloat($("#txtPopFactor").val());
      var area=parseFloat($("#txtArea").val());
      var cityClass= parseFloat(ddlCityClass.val());
        
      var effectivePopulation=population+population*((railway*2/100)+(airport*3/100)+(poi*5/100));
      var vehicles  =(((effectivePopulation/area*100)/popfactor)/cityClass);
      var dearshipnos=parseFloat(Math.round(area/50));
      var perdealer = parseFloat(vehicles/dearshipnos);

     var cityViewModel = {
         CityId: hdnCityId.val(),
         CityName: txtCityName.val().trim(),
         StateId: ddlState.val(),
         Population:txtPopulation.val(),
         Area:txtArea.val(),
         Railway:txtRailway.val(),
         Airport:txtAirport.val(),
         PointOfInterest:txtPointOfInterest.val(),
         CityClass:ddlCityClass.val(),
         PopFactor:txtPopFactor.val(),
         EffectivePopulation:effectivePopulation,
         Vehicles:vehicles,
         PerDealar:perdealer,
         DealershipsNos:dearshipnos,
         CityStatus: chkstatus
    };
    var requestData ={cityViewModel:cityViewModel };
    $.ajax({
        url: "../City/AddEditCity",
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
     $("#txtCityName").val("");
     $("#hdnCityID").val("0");
     $("#hdnAccessMode").val("0");
     $("#ddlState").val("0");
     $("#ddlCountry").val("0");
     $("#chkstatus").prop('checked', false);
     $("#txtPopulation").val("");
     $("#txtArea").val("");
     $("#txtRailway").val("");
     $("#txtAirport").val("");
     $("#txtPointOfInterest").val("");
     $("#ddlCityClass").val("");
     $("#txtPopFactor").val("");
    
}
