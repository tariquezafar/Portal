$(document).ready(function () {
    BindDepartmentList();
    //$('#tblCompanyList').paging({ limit: 2 });
    //SearchDesignation();
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
    $("#txtDesignationName").val("");
    $("#txtDesignationCode").val("");
    $("#ddlDepartment").val("0");
    $("#ddlStatus").val("");
}
function BindDepartmentList() {
    $.ajax({
        type: "GET",
        url: "../Designation/GetDepartmentList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlDepartment").append($("<option></option>").val(0).html("-Select Department-"));
            $.each(data, function (i, item) {
                $("#ddlDepartment").append($("<option></option>").val(item.DepartmentId).html(item.DepartmentName));
            });
        },
        error: function (Result) {
            $("#ddlDepartment").append($("<option></option>").val(0).html("-Select Department-"));
        }
    });
}


function SearchDesignation() {
    var hdnDesignationId = $("#hdnDesignationId");
    var txtDesignationName = $("#txtDesignationName");
    var txtDesignationCode = $("#txtDesignationCode");
    var ddlDepartment = $("#ddlDepartment");
    var ddlStatus = $("#ddlStatus");
    var requestData = {
        DesignationName: txtDesignationName.val().trim(),
        DesignationCode: txtDesignationCode.val().trim(),
        DepartmentId: ddlDepartment.val(),
        Status: ddlStatus.val()
    }; 
    $.ajax({
        url: "../Designation/GetDesignationList",
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
