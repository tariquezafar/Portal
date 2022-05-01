$(document).ready(function () {
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnEssEmployeeId.val() != "0") {
        $("#txtEmployee").attr('readOnly', true);
        $("#txtEmployee").val(hdnEssEmployeeName.val());
        $("#hdnEmployeeId").val(hdnEssEmployeeId.val());
    }
    $("#txtApplicationNo").attr('readOnly', true);   
    $("#txtApplicationDate").attr('readOnly', true);   
    $("#txtApplicationDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        minDate: 0,
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


    BindEmployeeClaimApplicationTypeList();
    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnApplicationId.val() != "" && hdnApplicationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeClaimApplicationDetail(hdnApplicationId.val());
       }, 2000);

        

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
        

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
 
function BindEmployeeClaimApplicationTypeList() {
    $.ajax({
        type: "GET",
        url: "../EmployeeClaimApplication/GetEmployeeClaimApplicationTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlClaimType").append($("<option></option>").val(0).html("Select Claim Type"));
            $.each(data, function (i, item) {
                $("#ddlClaimType").append($("<option></option>").val(item.ClaimTypeId).html(item.ClaimTypeName));
            });
        },
        error: function (Result) {
            $("#ddlClaimType").append($("<option></option>").val(0).html("Select Claim Type"));
        }
    });
}
   
function GetEmployeeClaimApplicationDetail(applicationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeClaimApplication/GetEmployeeClaimApplicationDetail",
        data: { applicationId: applicationId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicationNo").val(data.ApplicationNo);
            $("#hdnApplicationId").val(data.ApplicationId);
            $("#txtApplicationDate").val(data.ApplicationDate);
            $("#ddlClaimType").val(data.ClaimTypeId);
            $("#hdnEmployeeId").val(data.EmployeeId);
           // $("#hdnEssEmployeeId").val(data.EmployeeId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#hdnEmployeeId").val(data.EmployeeId);          
            $("#txtClaimReason").val(data.ClaimReason);
            $("#txtClaimAmount").val(data.ClaimAmount);
            $("#ddlClaimStatus").val(data.ClaimStatus);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#btnAddNew").show();

             
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

function SaveData() {
    var txtApplicationNo = $("#txtApplicationNo");
    var hdnApplicationId = $("#hdnApplicationId");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var txtApplicationDate = $("#txtApplicationDate");
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var txtEmployee = $("#txtEmployee");
    var ddlClaimType = $("#ddlClaimType");
    var txtClaimAmount = $("#txtClaimAmount");   
    var txtClaimReason = $("#txtClaimReason");
    var ddlClaimStatus = $("#ddlClaimStatus");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var EmployeeId;
    if (hdnEssEmployeeId.val() != "0") {
        EmployeeId = hdnEssEmployeeId.val();
    }
    else {
        EmployeeId = hdnEmployeeId.val();
    }
    
    if (txtEmployee.val() == "" || txtEmployee.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false;

    } 
    if (ddlClaimType.val() == "" || ddlClaimType.val() == "0") {
        ShowModel("Alert", "Please select Claim Type")
        return false;
    }
 
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }


    var employeeClaimApplicationViewModel = {
        ApplicationId: hdnApplicationId.val(),
        ApplicationNo: txtApplicationNo.val().trim(),
        ApplicationDate: txtApplicationDate.val(),
        ClaimTypeId: ddlClaimType.val(),
        EmployeeId: EmployeeId,
        ClaimReason: txtClaimReason.val(),
        ClaimStatus: ddlClaimStatus.val(),
        ClaimAmount: txtClaimAmount.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
    };
     
    var accessMode = 1;//Add Mode
    if (hdnApplicationId.val() != null && hdnApplicationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { employeeClaimApplicationViewModel: employeeClaimApplicationViewModel };
    $.ajax({
        url: "../EmployeeClaimApplication/AddEditEmployeeClaimApplication?accessMode=" + accessMode + "",
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
                      window.location.href = "../EmployeeClaimApplication/ListEmployeeClaimApplication";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../EmployeeClaimApplication/AddEditEmployeeClaimApplication";
                    }, 2000);
                }
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
    $("#ddlAssetType").val("0");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployee").val("");   
    $("#txtAssetReason").val("");
    $("#ddlAssetStatus").val("Draft");
    $("#btnSave").show();
    $("#btnUpdate").hide();
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
 
 