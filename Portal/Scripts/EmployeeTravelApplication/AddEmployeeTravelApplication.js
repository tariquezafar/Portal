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
    if (hdnApplicationId.val() != "0") {
        $("#txtCreatedBy").attr('readOnly', true);
        $("#txtCreatedDate").attr('readOnly', true);
        $("#txtModifiedBy").attr('readOnly', true);
        $("#txtModifiedDate").attr('readOnly', true);
        $("#txtRejectedDate").attr('readOnly', true);

        $("#txtApplicationDate").attr('readOnly', true);
        $("#txtTravelStartDate").attr('readOnly', true);
        $("#txtTravelEndDate").attr('readOnly', true);
    }

    $("#txtTravelStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        minDate: 0,
        onSelect: function (selected) {

        }
    });
    $("#txtApplicationDate,#txtTravelEndDate").datepicker({
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
    BindTravelTypeList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnApplicationId = $("#hdnApplicationId");
    if (hdnApplicationId.val() != "" && hdnApplicationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeTravelAppDetail(hdnApplicationId.val());
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
 
function BindTravelTypeList() {
    $.ajax({
        type: "GET",
        url: "../EmployeeTravelApp/GetTravelTypeForEmpolyeeTravelAppList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlTravelType").append($("<option></option>").val(0).html("-Select Travel Type-"));
            $.each(data, function (i, item) {

                $("#ddlTravelType").append($("<option></option>").val(item.TravelTypeId).html(item.TravelTypeName));
            });
        },
        error: function (Result) {
            $("#ddlTravelType").append($("<option></option>").val(0).html("-Select Travel Type-"));
        }
        
    });
}


function GetEmployeeTravelAppDetail(applicationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeTravelApp/GetEmployeTravelAppDetail",
        data: { applicationId: applicationId },
        dataType: "json",
        success: function (data) {
            $("#txtApplicationNo").val(data.ApplicationNo);
            $("#hdnApplicationId").val(data.ApplicationId);
            $("#txtApplicationDate").val(data.ApplicationDate);
            $("#ddlTravelType").val(data.TravelTypeId);
            $("#hdnEmployeeId").val(data.EmployeeId);
            //$("#hdnEssEmployeeId").val(data.EmployeeId);
            $("#txtEmployee").val(data.EmployeeName); 
            $("#txtTravelStartDate").val(data.TravelStartDate);
            $("#txtTravelEndDate").val(data.TravelEndDate);
            $("#txtTravelReason").val(data.TravelReason);
            $("#ddlTravelStatus").val(data.TravelStatus);
            $("#txtTravelDestination").val(data.TravelDestination);
            
 
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
    var ddlTravelType = $("#ddlTravelType");
    var txtTravelDestination = $("#txtTravelDestination");
    var txtTravelStartDate = $("#txtTravelStartDate");
    var txtTravelEndDate = $("#txtTravelEndDate");
    var txtTravelReason = $("#txtTravelReason");
    var ddlTravelStatus = $("#ddlTravelStatus");
    var hdnEssEmployeeName = $("#hdnEssEmployeeName");
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
    if (ddlTravelType.val() == "" || ddlTravelType.val() == "0") {
        ShowModel("Alert", "Please select Travel Type")
        return false;
    } 
  
    if (txtTravelStartDate.val().trim() == "") {
        ShowModel("Alert", "Please Enter Travel Start Date")
        txtTravelStartDate.focus();
        return false;
    }
    if (txtTravelEndDate.val().trim() == "") {
        ShowModel("Alert", "Please Enter Travel End Date")
        txtTravelEndDate.focus();
        return false;
    }
    if (txtTravelDestination.val().trim() == "") {
        ShowModel("Alert", "Please Enter Travel Destination")
        txtTravelDestination.focus();
        return false;
    }
   
    if (txtTravelReason.val().trim() == "") {
        ShowModel("Alert", "Please Enter Travel Reason")
        txtTravelReason.focus();
        return false;
    }
    

    var employeetravelappViewModel = {
        ApplicationId: hdnApplicationId.val(),
        ApplicationNo: txtApplicationNo.val().trim(),
        ApplicationDate: txtApplicationDate.val(),
        TravelTypeId: ddlTravelType.val(),
        EmployeeId: EmployeeId,
        TravelStartDate: txtTravelStartDate.val(),
        TravelEndDate: txtTravelEndDate.val(),
        TravelReason: txtTravelReason.val(),
        TravelDestination: txtTravelDestination.val(),
        TravelStatus: ddlTravelStatus.val()
    };
    var accessMode = 1;//Add Mode
    if (hdnApplicationId.val() != null && hdnApplicationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    

    var requestData = { employeetravelApplicationViewModel: employeetravelappViewModel };
    $.ajax({
        url: "../EmployeeTravelApp/AddEditEmployeeTravelApp?accessMode=" + accessMode + "",
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
                      window.location.href = "../EmployeeTravelApp/ListEmployeeTravelApp";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../EmployeeTravelApp/AddEditEmployeeTravelApp";
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
    $("#ddlTravelType").val("0");    
    $("#txtTravelDestination").val("");   
    $("#txtTravelReason").val("");
    $("#ddlTravelStatus").val("Draft");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
 
 


 
 