$(document).ready(function () {
    BindCompanyBranchList();
   

    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    $("#txtFromDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

            var tt = document.getElementById('txtFromDate').value;

            var date = new Date(tt);
            var newdate = new Date(date);

            newdate.setDate(newdate.getDate() + 180);

            var dd = newdate.getDate();
            var mm = newdate.getMonth() + 1;
            var y = newdate.getFullYear();
           
            var someFormattedDate =dd+'-'+mm+'-'+y;
            //var someFormattedDate = mm + '/' + dd + '/' + y;
            document.getElementById('txtToDate').value = someFormattedDate;
           
            //var tt = $("#txtFromDate").val();
          
            //tt.setDate(tt.getDay() + 3);
            //var date = new Date(tt);
            //var newdate = new Date(date);
            //$("#txtToDate").datepicker("option", "minDate", newdate);


            ////newdate.setDate(newdate.getDate() + 3);
            //////$("#txtToDate").setDate($("#txtFromDate").val() + 3);
            ////$("#txtToDate").setDate($("#txtFromDate") + 3);
        }
    }); 
});


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
    //$("#txtResourceRequisitionNo").val("");
    //$("#txtJobOpeningNo").val("");
    //$("#txtJobTitle").val("");
    //$("#txtJobPortalRefNo").val("");
    //$("#ddlJobStatus").val("0");
    //$("#txtFromDate").val($("#hdnFromDate").val());
    //$("#txtToDate").val($("#hdnToDate").val());

    window.location.href = "../JobOpening/ListJobOpening";
}
function SearchJobOpenings() {
    var txtResourceRequisitionNo = $("#txtResourceRequisitionNo");
    var txtJobOpeningNo = $("#txtJobOpeningNo");
    var txtJobPortalRefNo = $("#txtJobPortalRefNo");
    var txtJobTitle = $("#txtJobTitle");
    var ddlJobStatus = $("#ddlJobStatus");
    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = {
        jobOpeningNo: txtJobOpeningNo.val().trim(), requisitionNo: txtResourceRequisitionNo.val().trim(), jobPortalRefNo: txtJobPortalRefNo.val().trim(),
        jobTitle: txtJobTitle.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(), jobStatus: ddlJobStatus.val(),companyBranch: ddlCompanyBranch.val()};
    $.ajax({
        url: "../JobOpening/GetJobOpeningList",
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
function Export() {
    var divList = $("#divList");
    ExporttoExcel(divList);
}