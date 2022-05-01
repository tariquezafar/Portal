$(document).ready(function () {
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
    //$("#txtGLHead").val("");
    //$("#txtGLCode").val("");
    //$("#ddlGLType").val("0");
    //$("#ddlGLMainGroupId").val("0");
    //$("#ddlGLSubGroupId").val("0");
    //$("#ddlSLtypeId").val("0");
    window.location.href = "../GL/ListGL";

}

function SearchGL() {
    var txtGLCode = $("#txtGLCode");
    var ddlGLType = $("#ddlGLType");
    var txtGLHead = $("#txtGLHead");
    //var hdnGLMainGroupId = $("#hdnGLMainGroupId");
    var ddlGLMainGroupId = $("#ddlGLMainGroupId");

    var ddlGLSubGroupId = $("#ddlGLSubGroupId");
    var ddlSLtypeId = $("#ddlSLtypeId");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    var requestData = {
        GLHead: txtGLHead.val().trim(),
        GLCode: txtGLCode.val().trim(),
        GLType: ddlGLType.val(),
        GLMainGroupId: ddlGLMainGroupId.val(),
        GLSubGroupId: ddlGLSubGroupId.val(),
        SLTypeId: ddlSLtypeId.val(),
        CompanyBranchId: 0

    };

    $.ajax({
        url: "../GL/GetGLList",
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
BindGLMainGroupList();
BindGLSubGroupList();
BindSLTypeList();

function BindGLMainGroupList() {
        $.ajax({
            type: "GET",
            url: "../GLSubGroup/GetGLMainGroupList",
            data: "{}",
            dataType: "json",
            asnc: false,
            success: function (data) {
                $("#ddlGLMainGroupId").append($("<option></option>").val(0).html("-Select GLMainGroup-"));
                $.each(data, function (i, item) {

                    $("#ddlGLMainGroupId").append($("<option></option>").val(item.GLMainGroupId).html(item.GLMainGroupName));
                });
            },
            error: function (Result) {
                $("#ddlGLMainGroupId").append($("<option></option>").val(0).html("-Select GLMainGroup-"));
            }
        });
    }

function BindGLSubGroupList(GLMainGroupId) {

        var GLMainGroupId = $("#ddlGLMainGroupId option:selected").val();

        $("#ddlGLSubGroupId").val(0);
        $("#ddlGLSubGroupId").html("");

        if (GLMainGroupId != undefined && GLMainGroupId != "" && GLMainGroupId != "0") {
            var data = { MainGroupId: GLMainGroupId };
            $.ajax({
                type: "GET",
                url: "../GL/GetGLSubGroupList",
                data: data,
                asnc: false,
                dataType: "json",
                success: function (data) {
                    $("#ddlGLSubGroupId").append($("<option></option>").val(0).html("-Select GLMainGroup-"));
                    $.each(data, function (i, item) {

                        $("#ddlGLSubGroupId").append($("<option></option>").val(item.GLSubGroupId).html(item.GLSubGroupName));
                    });
                },
                error: function (Result) {
                    $("#ddlGLSubGroupId").append($("<option></option>").val(0).html("-Select GLMainGroup-"));
                }
            });
        }
        else {

            $("#ddlGLSubGroupId").append($("<option></option>").val(0).html("-Select GLMainGroup-"));
        }

    }


function BindSLTypeList() {
        $.ajax({
            type: "GET",
            url: "../Voucher/GetSLTypeList",
            data: "{}",
            dataType: "json",
            asnc: false,
            success: function (data) {
                $.each(data, function (i, item) {
                    $("#ddlSLtypeId").append($("<option></option>").val(item.SLTypeId).html(item.SLTypeName));
                });
            },
            error: function (Result) {
                $("#ddlSLtypeId").append($("<option></option>").val(0).html("NA"));
            }
        });
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

function GenerateReportParameters() {

    var url = "../GL/GLReoprt?GLHead=" + $("#txtGLHead").val() + "&GLCode=" + $("#txtGLCode").val() + "&GLType=" + $("#ddlGLType").val() + "&GLMainGroupId=" + $("#ddlGLMainGroupId").val() + "&GLSubGroupId=" + $("#ddlGLSubGroupId").val() + "&SLTypeId=" + $("#ddlSLtypeId").val() + "&reportType=PDF";
    $('#btnPdf').attr('href', url);
    var url = "../GL/GLReoprt?GLHead=" + $("#txtGLHead").val() + "&GLCode=" + $("#txtGLCode").val() + "&GLType=" + $("#ddlGLType").val() + "&GLMainGroupId=" + $("#ddlGLMainGroupId").val() + "&GLSubGroupId=" + $("#ddlGLSubGroupId").val() + "&SLTypeId=" + $("#ddlSLtypeId").val() + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

}
//Add Company Branch In User InterFAce So Binding Process By Dheeraj
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
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
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("Select Company Branch"));
        }
    });
}
//End Code