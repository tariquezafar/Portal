$(document).ready(function () {
    //$("#ddlMonth").append($("<option></option>").val(0).html("-Select Month-"));
    $("#ddlYear").append($("<option></option>").val(0).html("-Select Year-"));
   // BindMonth();
    BindYear();
   // GenerateReportParameters();
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
function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}


function BindMonth()
{
    var date = new Date();
    date.setMonth(date.getMonth() + 1);
    var months = 12;
    var monthNames = ["January", "February", "March", "April", "May", "June",
      "July", "August", "September", "October", "November", "December"
    ];
    var select = document.getElementById('ddlMonth');
    var html = '';
    for (var i = 0; i <= months; i++) {
        var m = date.getMonth();
        html += '<option value="' + i + '">' + monthNames[m] + '</option>'
        date.setMonth(date.getMonth() + 1);
    }
    select.innerHTML = html;
   // $('#ddlMonth option[value=' + date.getMonth() + ']').attr('selected', true);
}

function BindYear()
{
    var i = 0;
    for (i = 2000; i <= 2100; i++) {
        $("#ddlYear").append("<option value='" + i + "'>" + i + "</option>")
    }
    var d = new Date(),
    year = d.getFullYear();
    $('#ddlYear option[value=' + year + ']').attr('selected', true);
}

$('#body').on('change', '#ddlMonth', function () {
    GenerateReportParameters();
});

$('#body').on('change', '#ddlYear', function () {
    GenerateReportParameters();
});


function GenerateReportParameters() {
    var ddlMonth = $("#ddlMonth");
    var ddlYear = $("#ddlYear");
    if (ddlMonth.val() == "0")
    {
        ShowModel('Alert', 'Please select month');
        ddlMonth.focus();
        return false;
    }
    if (ddlYear.val() == "0") {
        ShowModel('Alert', 'Please select year');
        ddlYear.focus();
        return false;
    }
    //var url = "../EmployeeAttendance/EmployeeAttendanceReport?month=" + $("#ddlMonth").val() + "&year=" + $("#ddlYear").val() + "&reportType=PDF";

    var url = "../EmployeeAttendance/EmployeeAttendanceReport?month=" + $("#ddlMonth").val() + "&year=" + $("#ddlYear").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    var url = "../EmployeeAttendance/EmployeeAttendanceReport?month=" + $("#ddlMonth").val() + "&year=" + $("#ddlYear").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);


//    $('#lnkGenerateReport').attr({
//        href: url,
//        target: '_blank' });
                           
}


function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}

function ClearFields() {

    $("#txtCustomerSiteMRNNo").val("");
    $("#hdncustomerSiteMrnId").val("0");
    $("#txtCustomerSiteMRNDate").val($("#hdnCurrentDate").val());
    $("#hdnVendorId").val("0");
    $("#txtVendorName").val("");
    $("#txtVendorCode").val("");
    $("#txtInvoiceNo").val("");
    $("#txtInvoiceDate").val("");
    $("#txtInvoiceId").val("0"); 

    $("#txtSContactPerson").val("");
    $("#txtSAddress").val("");
    $("#txtSCity").val("");
    $("#ddlSCountry").val("0");
    $("#ddlSState").val("0");
    $("#txtSPinCode").val("");
    $("#txtSTINNo").val("");
    $("#txtSEmail").val("");
    $("#txtSMobileNo").val("");
    $("#txtSEmail").val("");
    $("#txtSFax").val(""); 

    $("#ddlCompanyBranch").val("0");

    $("#ddlApprovalStatus").val("Draft");


    $("#txtDispatchRefNo").val("");
    $("#txtDispatchRefDate").val(""); 
    $("#txtLRNo").val("");
    $("#txtLRDate").val("");
    $("#txtTransportVia").val("");
    $("#txtNoOfPackets").val("");
    $("#txtRemarks1").val("");
    $("#txtRemarks2").val("");

    $("#btnSave").show();
    $("#btnUpdate").hide();

    


}



function BindCustomerBranchList(customerBranchId) {
    var customerId = $("#hdnCustomerId").val();
    $("#ddlCustomerBranch").val(0);
    $("#ddlCustomerBranch").html("");
    if (customerId != undefined && customerId != "" && customerId != "0") {
        var data = { customerId: customerId };
        $.ajax({
            type: "GET",
            url: "../SO/GetCustomerBranchList",
            data: data,
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
                $.each(data, function (i, item) {
                    $("#ddlCustomerBranch").append($("<option></option>").val(item.CustomerBranchId).html(item.BranchName));
                });
                $("#ddlCustomerBranch").val(customerBranchId);
            },
            error: function (Result) {
                $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
            }
        });
    }
    else {
        $("#ddlCustomerBranch").append($("<option></option>").val(0).html("-Select Branch-"));
    }
}

function GetCustomerBranchListByID(customerId, customerBranchId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Project/GetCustomerBranchListByID",
        data: { customerId: customerId },
        dataType: "json",
        success: function (data) {

            $("#ddlCustomerType").append($("<option></option>").val(0).html("-Select Customer Branch-"));
            $.each(data, function (i, item) {

                $("#ddlCustomerBranch").append($("<option></option>").val(item.CustomerBranchId).html(item.BranchName));
            });
            if (customerBranchId != null && customerBranchId != "") {
                $("#ddlCustomerBranch").val(customerBranchId);
            }
        },
        error: function (Result) {
            $("#ddlCustomerType").append($("<option></option>").val(0).html("-Select Customer Branch-"));
        }
    });


}


/*Function Started here for Local Site MRN*/

function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}

function OpenPrintPopup() {
    $("#printModel").modal();
    GenerateReportParameters();
}
function ShowHidePrintOption() {
    var reportOption = $("#ddlPrintOption").val();
    if (reportOption == "PDF") {
        $("#btnPdf").show();
        $("#btnExcel").hide();
    }
    else if (reportOption == "Excel") {
        $("#btnExcel").show();
        $("#btnPdf").hide();
    }
}
