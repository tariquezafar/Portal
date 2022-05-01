$(document).ready(function () {
    BindCompanyBranchList();
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnEssEmployeeId.val() != "0") {
        $("#txtEmployee").attr('readOnly', true);
        $("#txtEmployee").val(hdnEssEmployeeName.val());
        $("#hdnEmployeeId").val(hdnEssEmployeeId.val());
    }
    $("#txtApplicationNo").attr('readOnly', true);
    $("#txtApplicationDate").attr('readOnly', true);
    if (hdnApplicationId.val() != "0") {        
        $("#txtCreatedBy").attr('readOnly', true);
        $("#txtCreatedDate").attr('readOnly', true);
        $("#txtModifiedBy").attr('readOnly', true);
        $("#txtModifiedDate").attr('readOnly', true);
        $("#txtRejectedDate").attr('readOnly', true);

        $("#txtAdvanceStartDate").attr('readOnly', true);
        $("#txtAdvanceEndDate").attr('readOnly', true);
    }
    $("#txtAdvanceStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            var fromDate = $('#txtAdvanceStartDate').datepicker('getDate');
            fromDate.setDate(fromDate.getDate() + 0);
            $('#txtAdvanceEndDate').datepicker('option', 'minDate', fromDate);
        }
    }); 

    $("#txtApplicationDate,#txtAdvanceEndDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $("#txtEmployee").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Employee/GetEmployeeAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term, departmentId: $("#txtEmployee").val(), designationId: $("#txtEmployee").val()
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
    BindAdvanceTypeList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnApplicationId.val() != "" && hdnApplicationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeAdvanceAppDetail(hdnApplicationId.val());
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
function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

} 
 
function BindAdvanceTypeList() {
    $.ajax({
        type: "GET",
        url: "../EmployeeAdvanceApp/GetAdvanceTypeForEmpolyeeAdvanceAppList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlAdvanceType").append($("<option></option>").val(0).html("-Select Advance Type-"));
            $.each(data, function (i, item) {

                $("#ddlAdvanceType").append($("<option></option>").val(item.AdvanceTypeId).html(item.AdvanceTypeName));
            });
        },
        error: function (Result) {
            $("#ddlAdvanceType").append($("<option></option>").val(0).html("-Select Advance Type-"));
        }
    });
}
   
function GetEmployeeAdvanceAppDetail(applicationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeAdvanceApp/GetEmployeeAdvanceAppDetail",
        data: { applicationId: applicationId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicationNo").val(data.ApplicationNo);
            $("#hdnApplicationId").val(data.ApplicationId);
            $("#txtApplicationDate").val(data.ApplicationDate); 
            $("#ddlAdvanceType").val(data.AdvanceTypeId);
            $("#hdnEmployeeId").val(data.EmployeeId);
           // $("#hdnEssEmployeeId").val(data.EmployeeId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#txtAdvanceAmount").val(data.AdvanceAmount);
            $("#txtAdvanceStartDate").val(data.AdvanceStartDate);
            $("#txtAdvanceEndDate").val(data.AdvanceEndDate);
            $("#txtAdvanceInstallmentAmount").val(data.AdvanceInstallmentAmount);
            $("#txtAdvanceReason").val(data.AdvanceReason);
            $("#ddlAdvanceStatus").val(data.AdvanceStatus); 
            $("#ddlCompanyBranch").val(data.companyBranch);
            if (data.RejectReason != "") { 
                $("#divReject").show();
                $("#txtRejectReason").val(data.RejectReason);
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
    var txtApplicationNo = $("#txtApplicationNo");
    var hdnApplicationId = $("#hdnApplicationId");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var txtApplicationDate = $("#txtApplicationDate");
    var txtEmployee = $("#txtEmployee");
    var txtAdvanceAmount = $("#txtAdvanceAmount"); 
    var ddlAdvanceType = $("#ddlAdvanceType"); 
    var txtAdvanceStartDate = $("#txtAdvanceStartDate");
    var txtAdvanceEndDate = $("#txtAdvanceEndDate");
    var txtAdvanceInstallmentAmount = $("#txtAdvanceInstallmentAmount"); 
    var txtAdvanceReason = $("#txtAdvanceReason"); 
    var ddlAdvanceStatus = $("#ddlAdvanceStatus");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
    var ddlCompanyBranch= $("#ddlCompanyBranch");
    var EmployeeId;
    if (hdnEssEmployeeId.val() != "0") {
        EmployeeId = hdnEssEmployeeId.val();
    }
    else {
        EmployeeId = hdnEmployeeId.val();
    }
    if (txtApplicationDate.val() == "") {
        ShowModel("Alert", "Please select Application Date")
        return false;  
    } 
    if (txtEmployee.val() == "" || txtEmployee.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false; 
    } 
    if (ddlAdvanceType.val() == "" || ddlAdvanceType.val() == "0") {
        ShowModel("Alert", "Please select Advance Type")
        return false;
    }
    if (txtEmployee.val().trim() == "") {
        ShowModel("Alert", "Please Enter Employee Name")
        txtEmployee.focus();
        return false;
    }
    if (txtAdvanceAmount.val().trim() == "") {
        ShowModel("Alert", "Please Enter Advance Amount Name")
        txtAdvanceAmount.focus();
        return false;
    }
    if (txtAdvanceStartDate.val().trim() == "") {
        ShowModel("Alert", "Please Enter Advance Start Date")
        txtAdvanceStartDate.focus();
        return false;
    }
    if (txtAdvanceEndDate.val().trim() == "") {
        ShowModel("Alert", "Please Enter Advance End Date")
        txtAdvanceEndDate.focus();
        return false;
    }
    if (txtAdvanceInstallmentAmount.val().trim() == "") {
        ShowModel("Alert", "Please Enter Advance Installment Amount")
        txtAdvanceInstallmentAmount.focus();
        return false;
    }
    if (txtAdvanceReason.val().trim() == "") {
        ShowModel("Alert", "Please Enter Advance Reason")
        txtAdvanceReason.focus();
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    var accessMode = 1;//Add Mode
    if (hdnApplicationId.val() != null && hdnApplicationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }


    var employeeadvanceappViewModel = {
        ApplicationId: hdnApplicationId.val(),
        ApplicationNo: txtApplicationNo.val().trim(),
        ApplicationDate: txtApplicationDate.val(),
        AdvanceTypeId: ddlAdvanceType.val(),
        EmployeeId: EmployeeId,
        AdvanceAmount: txtAdvanceAmount.val(),
        AdvanceStartDate: txtAdvanceStartDate.val(),
        AdvanceEndDate: txtAdvanceEndDate.val(),
        AdvanceInstallmentAmount: txtAdvanceInstallmentAmount.val(),
        AdvanceReason: txtAdvanceReason.val(), 
        AdvanceStatus: ddlAdvanceStatus.val(),
        companyBranch: ddlCompanyBranch.val()
    };
     
    

    var requestData = { employeeadvanceApplicationViewModel: employeeadvanceappViewModel };
    $.ajax({
        url: "../EmployeeAdvanceApp/AddEditEmployeeAdvanceApp?accessMode=" + accessMode + "",
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
                      window.location.href = "../EmployeeAdvanceApp/ListEmployeeAdvanceApp";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                       // window.location.href = "../EmployeeAdvanceApp/AddEditEmployeeAdvanceApp";
                        window.location.href = "../EmployeeAdvanceApp/AddEditEmployeeAdvanceApp?applicationId=" + data.trnId + "&AccessMode=3";
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
    $("#ddlAdvanceType").val("0");   
    $("#txtAdvanceAmount").val("");
    $("#txtAdvanceInstallmentAmount").val("");
    $("#txtAdvanceReason").val(""); 
    $("#ddlAdvanceStatus").val("Draft");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
 
 
 
 
 