$(document).ready(function () {
    BindHolidayTypeIdList();
    BindCompanyBranchList();
    BindCalenderList();
    $("#ddlHolidayType").append($("<option></option>").val(0).html("-Select Holiday Type-"));

    $("#txtHolidayDate").attr('readOnly', true);  
    $("#txtHolidayDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        minDate: '0D',
        onSelect: function (selected) {
        }
    });
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnHolidayCalenderId = $("#hdnHolidayCalenderId");
    if (hdnHolidayCalenderId.val() != "" && hdnHolidayCalenderId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetHolidayCalenderDetail(hdnHolidayCalenderId.val());
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkStatus").attr('disabled', true);
            
        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }
    $("#txtActivityDate").focus();
   
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
 
function BindHolidayTypeIdList(companyBranchId) {

    var companyBranchId = $("#ddlCompanyBranch option:selected").val();
    $("#ddlHolidayType").val(0);
    $("#ddlHolidayType").html("");
    if (companyBranchId != undefined && companyBranchId != "" && companyBranchId != "0") {
        var data = { companyBranchId: companyBranchId };
        $.ajax({
            type: "GET",
            url: "../HolidayCalender/GetHolidayTypeIdList",
            data:data,
            dataType: "json",
            asnc: false,
            success: function(data) {
                $("#ddlHolidayType").append($("<option></option>").val(0).html("-Select Holiday Type-"));
                $.each(data, function (i, item) {
                    $("#ddlHolidayType").append($("<option></option>").val(item.HolidayTypeId).html(item.HolidayTypeName));
                });
                $("#ddlHolidayType").val(0);
            },
            error: function (Result) {
                $("#ddlHolidayType").append($("<option></option>").val(0).html("-Select Holiday Type-"));
            }
        });
    }
}

    function BindCalenderList() {
        $.ajax({
            type: "GET",
            url: "../ActivityCalender/GetCalenderList",
            data: "{}",
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlCalender").append($("<option></option>").val(0).html("-Select Calender-"));
                $.each(data, function (i, item) {
                    $("#ddlCalender").append($("<option></option>").val(item.CalenderId).html(item.CalenderName));
                });
            },
            error: function (Result) {
                $("#ddlCalender").append($("<option></option>").val(0).html("-Select Calender-"));
            }
        });
    }
    function GetHolidayCalenderDetail(holidaycalenderId) {
        $.ajax({
            type: "GET",
            asnc: false,
            url: "../HolidayCalender/GetHolidayCalenderDetail",
            data: { holidaycalenderId: holidaycalenderId },
            dataType: "json",
            success: function (data) {
                $("#txtHolidayDate").val(data.HolidayDate);
                $("#txtHolidayDescription").val(data.HolidayDescription);
                $("#ddlHolidayType").val(data.HolidayTypeId);
                $("#ddlCalender").val(data.CalenderId);
                $("#ddlCompanyBranch").val(data.CompanyBranchId);
                BindHolidayTypeIdList(data.companyBranchId);
                if (data.HolidayStatus == true) {
                    $("#chkStatus").attr("checked", true);
                }
                else {
                    $("#chkStatus").attr("checked", false);
                }
            },
            error: function (Result) {
                ShowModel("Alert", "Problem in Request");
            }
        });
    }

    function SaveData() {
        var txtHolidayDate = $("#txtHolidayDate");
        var txtHolidayDescription = $("#txtHolidayDescription");
        var ddlCalender = $("#ddlCalender");
        var ddlHolidayType = $("#ddlHolidayType");
        var hdnHolidayCalenderId = $("#hdnHolidayCalenderId");
        var chkStatus = $("#chkStatus").is(':checked') ? true : false;
        var ddlCompanyBranch = $("#ddlCompanyBranch");

        if (txtHolidayDate.val() == "") {
            ShowModel("Alert", "Please select Holiday Date")
            txtHolidayDate.focus();
            return false;
        }
        if (ddlCalender.val() == 0) {
            ShowModel("Alert", "Please Select Calender")
            ddlCalender.focus();
            return false;
        }
        if (ddlHolidayType.val() == 0) {
            ShowModel("Alert", "Please Select Holiday Type")
            ddlHolidayType.focus();
            return false;
        }

        if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
            ShowModel("Alert", "Please select Company Branch")
            return false;
        }

        var holidaycalenderViewModel = {
            HolidayCalenderId: hdnHolidayCalenderId.val(),
            HolidayDate: txtHolidayDate.val(),
            HolidayDescription: txtHolidayDescription.val().trim(),
            CalenderId: ddlCalender.val(),
            HolidayTypeId: ddlHolidayType.val(),
            HolidayStatus: chkStatus,
            CompanyBranchId: ddlCompanyBranch.val(),
        };
        var accessMode = 1;//Add Mode
        if (hdnHolidayCalenderId.val() != null && hdnHolidayCalenderId.val() != 0) {
            accessMode = 2;//Edit Mode
        }

        var requestData = { holidaycalenderViewModel: holidaycalenderViewModel };
        $.ajax({
            url: "../HolidayCalender/AddEditHolidayCalender?accessMode=" + accessMode + "",
            cache: false,
            type: "POST",
            dataType: "json",
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (data) {
                if (data.status == "SUCCESS") {
                    ShowModel("Alert", data.message);
                    ClearFields();
                    if (accessMode == 2) {
                        setTimeout(
                      function () {
                          window.location.href = "../HolidayCalender/ListHolidayCalender";
                      }, 2000);
                    }
                    else {

                        setTimeout(
                        function () {
                            window.location.href = "../HolidayCalender/AddEditHolidayCalender?accessMode=3";
                        }, 2000);
                    }
                    $("#btnSave").show();
                    $("#btnUpdate").hide();
                }
                else {
                    ShowModel("Error", data.message)
                }
            },
            error: function (err) {
                ShowModel("Error", err)
            }
        });

    }
    function ShowModel(headerText, bodyText) {
        $("#alertModel").modal();
        $("#modelHeader").html(headerText);
        $("#modelText").html(bodyText);
    }
    function ClearFields() {
        $("#hdnHolidayCalenderId").val("0");
        $("#txtHolidayDate").val($("#hdnCurrentDate").val());
        $("#txtHolidayDescription").val("");
        $("#ddlCalender").val("0");
        $("#ddlHolidayType").val("0");
        $("#chkStatus").prop("checked", true);
        $("#ddlCompanyBranch").val("0");

    }


    function BindCompanyBranchList() {
        $("#ddlCompanyBranch").val(0);
        $("#ddlCompanyBranch").html("");
        $.ajax({
            type: "GET",
            url: "../DeliveryChallan/GetCompanyBranchList",
            data: {},
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
                $.each(data, function (i, item) {
                    $("#ddlCompanyBranch").append($("<option></option>").val(item.CompanyBranchId).html(item.BranchName));
                });
                var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
                var hdnSessionUserID = $("#hdnSessionUserID");
                if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                    $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
                    $("#ddlCompanyBranch").attr('disabled', true);
                }
            },
            error: function (Result) {
                $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
            }
        });
    }
