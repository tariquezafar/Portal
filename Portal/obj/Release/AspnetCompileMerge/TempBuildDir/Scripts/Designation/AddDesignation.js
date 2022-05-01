$(document).ready(function () {
    BindDepartmentList();
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnDesignationId = $("#hdnDesignationId");
    if (hdnDesignationId.val() != "" && hdnDesignationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetDesignationDetail(hdnDesignationId.val());
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
    $("#txtDesignationName").focus();
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

function GetDesignationDetail(designationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Designation/GetDesignationDetail",
        data: { designationId: designationId },
        dataType: "json",
        success: function (data) {
            $("#txtDesignationName").val(data.DesignationName);
            $("#txtDesignationCode").val(data.DesignationCode);
            $("#ddlDepartment").val(data.DepartmentId);
            if (data.DesignationStatus == true) {
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
    var txtDesignationName = $("#txtDesignationName");
    var txtDesignationCode = $("#txtDesignationCode");
    var ddlDepartment = $("#ddlDepartment");
    var hdnDesignationId = $("#hdnDesignationId");
    var chkStatus = $("#chkStatus").is(':checked') ? true : false;

    if (txtDesignationName.val().trim() == "") {
        ShowModel("Alert", "Please enter Designation Name")
        txtDesignationName.focus();
        return false;
    }
    if (txtDesignationCode.val().trim() == "") {
        ShowModel("Alert", "Please enter Designation Code")
        txtDesignationCode.focus();
        return false;
    }
    if (ddlDepartment.val() == 0) {
        ShowModel("Alert", "Please Select Department Id")
        ddlDepartment.focus();
        return false;
    }
    
    var designationViewModel = {
        DesignationId: hdnDesignationId.val(),
        DesignationName: txtDesignationName.val().trim(),
        DesignationCode: txtDesignationCode.val().trim(),
        DepartmentId: ddlDepartment.val().trim(),
        DesignationStatus: chkStatus
    };
    var accessMode = 1;//Add Mode
    if (hdnDesignationId.val() != null && hdnDesignationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { designationViewModel: designationViewModel };
    $.ajax({
        url: "../Designation/AddEditDesignation?accessMode=" + accessMode + "",
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
                      window.location.href = "../Designation/ListDesignation";
                  }, 2000);
                }
                else {

                    setTimeout(
                    function () {
                        window.location.href = "../Designation/AddEditDesignation?accessMode=1";
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
    $("#hdnDesignationId").val("0");
    $("#txtDesignationName").val("");
    $("#txtDesignationCode").val("");
    $("#ddlDepartment").val("0"); 
    $("#chkStatus").prop("checked", true);

}
