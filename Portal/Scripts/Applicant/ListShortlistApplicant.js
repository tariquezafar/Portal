$(document).ready(function () {
    BindResumeSourceList();
    BindPositionList();
 
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);

    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });
    setTimeout(
    function () {
        SearchApplicant();
    }, 1000);
  
});
function BindResumeSourceList() {
    $("#ddlResumeSource").val(0);
    $("#ddlResumeSource").html("");
    $.ajax({
        type: "GET",
        url: "../Applicant/GetResumeSourceList",
        data: {},
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlResumeSource").append($("<option></option>").val(0).html("-Select Resume Source-"));
            $.each(data, function (i, item) {
                $("#ddlResumeSource").append($("<option></option>").val(item.ResumeSourceId).html(item.ResumeSourceName));
            });
        },
        error: function (Result) {
            $("#ddlResumeSource").append($("<option></option>").val(0).html("-Select Resume Source-"));
        }
    });
}
function BindPositionList() {

    $("#ddlDesignation").val(0);
    $("#ddlDesignation").html("");

    $.ajax({
        type: "GET",
        url: "../Applicant/GetAllDesignationList",
        data: {},
        asnc: false,
        dataType: "json",
        success: function (data) {
            $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
            $.each(data, function (i, item) {
                $("#ddlDesignation").append($("<option></option>").val(item.DesignationId).html(item.DesignationName));
            });
        },
        error: function (Result) {
            $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
        }
    });


}

function ClearFields() {
    $("#txtApplicantNo").val("");
    $("#txtProjectNo").val("");
    $("#txtFirstName").val("");
    $("#txtLastName").val("");
    $("#ddlResumeSource").val("0");
    $("#ddlDesignation").val("0");
    $("#ddlApplicantStatus").val("0");
    $("#txtFromDate").val($("#hdnFromDate").val());
    $("#txtToDate").val($("#hdnToDate").val());
}
function SearchApplicant() {
    var txtApplicantNo = $("#txtApplicantNo");
    var txtProjectNo = $("#txtProjectNo");
    var txtFirstName = $("#txtFirstName");
    var txtLastName = $("#txtLastName");
    var ddlResumeSource = $("#ddlResumeSource");
    var ddlDesignation = $("#ddlDesignation");
    var ddlApplicantStatus = $("#ddlApplicantStatus");

    var txtFromDate = $("#txtFromDate");
    var txtToDate = $("#txtToDate");
    var requestData = {
        applicantNo: txtApplicantNo.val().trim(), projectNo: txtProjectNo.val().trim(),
        firstName: txtFirstName.val().trim(), lastName: txtLastName.val(), resumeSource: ddlResumeSource.val(),
        designation: ddlDesignation.val(), fromDate: txtFromDate.val(), toDate: txtToDate.val(),
        applicantShortlistStatus: ddlApplicantStatus.val()
    };
    $.ajax({
        url: "../Applicant/GetShortlistApplicantList",
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