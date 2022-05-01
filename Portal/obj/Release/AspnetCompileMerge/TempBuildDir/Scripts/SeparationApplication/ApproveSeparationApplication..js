$(document).ready(function () {
    $("#txtApplicationNo").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtRejectedDate").attr('readOnly', true); 
    $("#ddlCompanyBranch").attr('readOnly', true);
    $("#txtApplicationDate").attr('readOnly', true);
    

    $("#txtApplicationDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    var ddlCompanyBranch = $("#ddlCompanyBranch");
    $("#txtEmployee").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, departmentId: $("#txtEmployee").val(), companyBranchId: ddlCompanyBranch.val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.FirstName, value: item.EmployeeId, EmployeeCode: item.EmployeeCode, MobileNo: item.MobileNo
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtEmployee").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtEmployee").val(ui.item.label);
            $("#hdnEmployeeId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtEmployee").val("");
                $("#hdnEmployeeId").val("0");
                ShowModel("Alert", "Please select Employee from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.EmployeeCode + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};
    BindSaparationCategoryList();
    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnApplicationId.val() != "" && hdnApplicationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetSeparationApplicationDetail(hdnApplicationId.val());
       }, 2000);



        if (hdnAccessMode.val() == "3") { 
            $("#btnUpdate").hide(); 
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);


        }
        else {
            
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);

            $("#ddlApplicationStatus").attr('disabled', false);
            $("#txtRejectedReason").attr('disabled', true);
            $("#txtRejectedReason").attr('readOnly', false);

            $("#btnUpdate").show();
        }
    }
    else {
         
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }

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

function BindSaparationCategoryList() {
    $.ajax({
        type: "GET",
        url: "../SeparationApplication/GetSeparationCategoryForSeparationApplicationList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSeparationCategory").append($("<option></option>").val(0).html("-Select Separation Category-"));
            $.each(data, function (i, item) {

                $("#ddlSeparationCategory").append($("<option></option>").val(item.SeparationCategoryId).html(item.SeparationCategoryName));
            });
        },
        error: function (Result) {
            $("#ddlSeparationCategory").append($("<option></option>").val(0).html("-Select Separation Category-"));
        }
    });
}

function GetSeparationApplicationDetail(applicationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SeparationApplication/GetSeparationApplicationDetail",
        data: { applicationId: applicationId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicationNo").val(data.ApplicationNo);
            $("#hdnApplicationId").val(data.ApplicationId);
            $("#txtApplicationDate").val(data.ApplicationDate);
            $("#ddlSeparationCategory").val(data.SeparationCategoryId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#txtRemark").val(data.Remarks);
            $("#txtReason").val(data.Reason);
            $("#ddlApplicationStatus").val(data.ApplicationStatus == "Final" ? "" : data.ApplicationStatus);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);

            if (data.RejectReason != "") {
                $("#divReject").show();
                $("#txtRejectedReason").val(data.RejectReason);
                $("#txtRejectedDate").val(data.RejectDate);
            }
            $("#btnAddNew").show();

        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() { 
    var hdnApplicationId = $("#hdnApplicationId");
    var ddlApplicationStatus = $("#ddlApplicationStatus");
    var txtRejectedReason = $("#txtRejectedReason");
    if (ddlApplicationStatus.val() == "0" || ddlApplicationStatus.val() == "") {
        ShowModel("Alert", "Please select Approved/ Rejdcted")
        ddlApplicationStatus.focus();
        return false;
    } 

    if (txtRejectedReason.val() == "Rejected" && txtRejectedReason.val() == "") {
        ShowModel("Alert", "Please Enter Rejection Reason")
        txtRejectedReason.focus();
        return false;
    }
    var separationapplicationViewModel = {
        ApplicationId: hdnApplicationId.val(),
        ApplicationStatus: ddlApplicationStatus.val(),
        RejectReason: txtRejectedReason.val().trim()
    };
    var accessMode = 1;//Add Mode
    if (hdnApplicationId.val() != null && hdnApplicationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { separationapplicationViewModel: separationapplicationViewModel };
    $.ajax({
        url: "../SeparationApplication/ApproveSeparationApplication?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                setTimeout(
                   function () {
                       window.location.href = "../SeparationApplication/ListSeparationApplicationApproval";
                   }, 2000);


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
    $("#txtApplicationNo").val("");
    $("#hdnApplicationId").val("0");
    $("#txtApplicationDate").val("");
    $("#ddlSeparationCategory").val("0");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployee").val("");
    $("#txtRemark").val("");
    $("#txtReason").val("");
    $("#ddlApplicationStatus").val("Final");
    $("#btnSave").show();
    $("#btnUpdate").hide();
    $("#ddlCompanyBranch").val("0");

}


function EnableDisableRejectReason() {
    var approvalStatus = $("#ddlApplicationStatus option:selected").text();
    if (approvalStatus == "Rejected") {
        $("#txtRejectedReason").attr("disabled", false);
    }
    else {
        $("#txtRejectedReason").attr("disabled", true);
        $("#txtRejectedReason").val("");
    }
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