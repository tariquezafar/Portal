$(document).ready(function () {
    //$('#tblCompanyList').paging({ limit: 2 });
    SearchCity(); 
    BindCountryList();
    $("#ddlState").append($("<option></option>").val(0).html("-Select State-"));
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

//$("#ddlCountry").change(function () {
//    BindStateList($("#ddlCountry").val());
//});

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
 
function ClearFields()
{
    $("#txtCityName").val("");
    BindCountryList();
    $("#ddlState").val("0");
    $("#ddlCountry").val("0");
}

function SearchCity() { 
    var txtCityName = $("#txtCityName");
    var ddlState = $("#ddlState");
    var ddlCountry = $("#ddlCountry"); 
    var requestData = { cityName: txtCityName.val().trim(), stateId:ddlState.val(), countryId: ddlCountry.val() };
    $.ajax({
        url: "../City/GetCityList",
        data: requestData,
        dataType: "html",
        type: "GET",
        error: function (err) {
            $("#divList").html("");
            $("#divList").html(err);
            alert(err);
        },
        success: function (data) {
            $("#divList").html("");
            $("#divList").html(data);

        }
    });
}
