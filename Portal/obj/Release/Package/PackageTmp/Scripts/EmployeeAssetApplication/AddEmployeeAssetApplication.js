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
                    term: request.term, departmentId: $("#txtEmployee").val(),companyBranchId: ddlCompanyBranch.val()
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


    BindEmployeeAssetApplicationTypeList();
    BindCompanyBranchList();

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnApplicationId.val() != "" && hdnApplicationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeAssetApplicationDetail(hdnApplicationId.val());
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
 
function BindEmployeeAssetApplicationTypeList() {
    $.ajax({
        type: "GET",
        url: "../EmployeeAssetApplication/GetEmployeeAssetApplicationTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlAssetType").append($("<option></option>").val(0).html("Select Asset Type"));
            $.each(data, function (i, item) {
                $("#ddlAssetType").append($("<option></option>").val(item.AssetTypeId).html(item.AssetTypeName));
            });
        },
        error: function (Result) {
            $("#ddlAssetType").append($("<option></option>").val(0).html("Select Asset Type"));
        }
    });
}
   
function GetEmployeeAssetApplicationDetail(applicationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeAssetApplication/GetEmployeeAssetApplicationDetail",
        data: { applicationId: applicationId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicationNo").val(data.ApplicationNo);
            $("#hdnApplicationId").val(data.ApplicationId);
            $("#txtApplicationDate").val(data.ApplicationDate);
            $("#ddlAssetType").val(data.AssetTypeId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#hdnEmployeeId").val(data.EmployeeId);
           // $("#hdnEssEmployeeId").val(data.EmployeeId);
            $("#txtAssetReason").val(data.AssetReason);
            $("#ddlAssetStatus").val(data.ApplicationStatus);       
            $("#btnAddNew").show();
            $("#ddlCompanyBranch").val(data.CompanyBranchId);

             
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

function SaveData() {
    var txtApplicationNo = $("#txtApplicationNo");
    var hdnApplicationId = $("#hdnApplicationId");
    var txtApplicationDate = $("#txtApplicationDate");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var hdnEssEmployeeId = $("#hdnEssEmployeeId");
    var txtEmployee = $("#txtEmployee");
    var ddlAssetType = $("#ddlAssetType");   
    var txtAssetReason = $("#txtAssetReason");
    var ddlAssetStatus = $("#ddlAssetStatus");
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
    if (ddlAssetType.val() == "" || ddlAssetType.val() == "0") {
        ShowModel("Alert", "Please select Asset Type")
        return false;
    }
 
    var employeeAssetApplicationViewModel = {
        ApplicationId: hdnApplicationId.val(),
        ApplicationNo: txtApplicationNo.val().trim(),
        ApplicationDate: txtApplicationDate.val(),
        AssetTypeId: ddlAssetType.val(),
        EmployeeId: EmployeeId,
        AssetReason: txtAssetReason.val(),
        ApplicationStatus: ddlAssetStatus.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
    };
     
    var accessMode = 1;//Add Mode
    if (hdnApplicationId.val() != null && hdnApplicationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }


    var requestData = { employeeAssetApplicationViewModel: employeeAssetApplicationViewModel };
    $.ajax({
        url: "../EmployeeAssetApplication/AddEditEmployeeAssetApplication?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
                   function () {
                       window.location.href = "../EmployeeAssetApplication/AddEditEmployeeAssetApplication?applicationId=" + data.trnId + "&essEmployeeId=" + hdnEssEmployeeId.val() + "&essEmployeeName=" + hdnEssEmployeeName.val() + "&AccessMode=3";
                   }, 2000);

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
 
 
 