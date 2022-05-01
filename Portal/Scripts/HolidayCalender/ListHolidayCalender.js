$(document).ready(function () {
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        maxDate: '0D',
        onSelect: function (selected) {
        }
    });
    BindCalenderList();
    BindHolidayTypeIdList();
    //$('#tblCompanyList').paging({ limit: 2 });
    // SearchHolidayCalender();
    BindCompanyBranchList();
  
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

function ClearFields() {
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
    $("#txtActivityDescription").val("");
    $("#ddlCalender").val("0");
    $("#ddlHolidayType").val("0");
    $("#ddlStatus").val("");
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
function BindHolidayTypeIdList() {
    $.ajax({
        type: "GET",
        url: "../HolidayCalender/GetHolidayTypeIdList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlHolidayType").append($("<option></option>").val(0).html("-Select Holiday Type-"));
            $.each(data, function (i, item) {
                $("#ddlHolidayType").append($("<option></option>").val(item.HolidayTypeId).html(item.HolidayTypeName));
            });
        },
        error: function (Result) {
            $("#ddlHolidayType").append($("<option></option>").val(0).html("-Select Holiday Type-"));
        }
    });
}


function SearchHolidayCalender() {
    var hdnHolidayCalenderId = $("#hdnHolidayCalenderId");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate"); 
    var ddlCalender = $("#ddlCalender");
    var ddlHolidayType = $("#ddlHolidayType");
    var ddlStatus = $("#ddlStatus");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = {  
        CalenderId: ddlCalender.val(),
        HolidayTypeId: ddlHolidayType.val(),
        fromDate: txtFromDate.val(),
        toDate: txtToDate.val(),
        Status: ddlStatus.val(),
        companyBranchId: ddlCompanyBranch.val()
    }; 
    $.ajax({
        url: "../HolidayCalender/GetHolidayCalenderList",
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