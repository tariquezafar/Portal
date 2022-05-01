$(document).ready(function () {  
    $("#txtExitInterviewNo").attr('readOnly', true);
    $("#txtEmployee").attr('readOnly', true);
    
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
 
    BindSaparationApplicationList();
    $("#txtExitInterviewDate").attr('readOnly', true);
    $("#txtExitInterviewDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        minDate:0,
        onSelect: function (selected) { 
        }
    });

    $("#txtInterviewByUser").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Lead/GetUserAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, companyBranchID :10004},
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FullName, value: item.UserName, UserId: item.UserId, MobileNo: item.MobileNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtInterviewByUser").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtInterviewByUser").val(ui.item.label);
            $("#hdnInterviewByUserId").val(ui.item.UserId);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtInterviewByUser").val("");
                $("#hdnInterviewByUserId").val("");
                ShowModel("Alert", "Please select User from List")
            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.value + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};

     


 
    
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnExitInterviewId = $("#hdnExitInterviewId");
    if (hdnExitInterviewId.val() != "" && hdnExitInterviewId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetExitInterviewDetail(hdnExitInterviewId.val());
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
 
function BindSaparationApplicationList() {
    $.ajax({
        type: "GET",
        url: "../ExitInterview/GetSeparationApplicationForExitInterviewList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSeparationApplication").append($("<option></option>").val(0).html("-Select Separation Application-"));
            $.each(data, function (i, item) {
                $("#ddlSeparationApplication").append($("<option></option>").val(item.ApplicationId).html(item.ApplicationNo));
            });
        },
        error: function (Result) {
            $("#ddlSeparationApplication").append($("<option></option>").val(0).html("-Select Separation Application-"));
        }
    });
}
   
function GetExitInterviewDetail(exitinterviewId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ExitInterview/GetExitInterviewDetail",
        data: { exitinterviewId: exitinterviewId },
        dataType: "json",
        success: function (data) {
            $("#txtExitInterviewNo").val(data.ExitInterviewNo);
            $("#hdnExitInterviewId").val(data.ExitInterviewId);
            $("#txtExitInterviewDate").val(data.ExitInterviewDate);
            $("#ddlSeparationApplication").val(data.ApplicationId);
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#hdnInterviewByUserId").val(data.InterviewByUserId);
            $("#txtInterviewByUser").val(data.InterviewByUserName); 
            $("#txtInterviewRemarks").val(data.InterviewRemarks);
            $("#txtInterviewDescription").val(data.InterviewDescription);  
            $("#ddlInterviewStatus").val(data.InterviewStatus);
            
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByName);
            $("#txtCreatedDate").val(data.CreatedDate);

            if (data.ModifiedByName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            } 
            $("#btnAddNew").show();
             
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
function SaveData() {
    var txtExitInterviewNo = $("#txtExitInterviewNo");
    var hdnExitInterviewId = $("#hdnExitInterviewId");
    var txtExitInterviewDate = $("#txtExitInterviewDate");
    var ddlSeparationApplication = $("#ddlSeparationApplication");
    var txtEmployee = $("#txtEmployee");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var txtInterviewDescription = $("#txtInterviewDescription");
    
    var txtInterviewByUser = $("#txtInterviewByUser");
    var hdnInterviewByUserId = $("#hdnInterviewByUserId");
    var txtInterviewRemarks = $("#txtInterviewRemarks");
    var ddlInterviewStatus = $("#ddlInterviewStatus");
   
    if (txtExitInterviewDate.val() == "") {
        ShowModel("Alert", "Please select Exit Interview Date")
        return false;  
    }
    if (ddlSeparationApplication.val() == "" || ddlSeparationApplication.val() == "0") {
        ShowModel("Alert", "Please select Separation Application")
        return false;
    }
    if (txtEmployee.val() == "" || txtEmployee.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false; 
    }
    if (hdnEmployeeId.val() == "" || hdnEmployeeId.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false;
    }
    
    if (txtInterviewByUser.val() == "" || txtInterviewByUser.val() == "0") {
        ShowModel("Alert", "Please select User")
        return false;
    }
    if (hdnInterviewByUserId.val() == "" || hdnInterviewByUserId.val() == "0") {
        ShowModel("Alert", "Please select User")
        return false;
    }
    if (txtInterviewDescription.val().trim() == "") {
        ShowModel("Alert", "Please Enter Interview Description")
        txtRemark.focus();
        return false;
    }
    
    var exitinterviewViewModel = {
        ExitInterviewId: hdnExitInterviewId.val(),
        ExitInterviewNo: txtExitInterviewNo.val().trim(),
        ExitInterviewDate: txtExitInterviewDate.val(),
        ApplicationId: ddlSeparationApplication.val(),
        EmployeeId: hdnEmployeeId.val(),
        InterviewDescription: txtInterviewDescription.val().trim(),
        InterviewByUserId: hdnInterviewByUserId.val(),
        InterviewRemarks: txtInterviewRemarks.val(),
        InterviewStatus: ddlInterviewStatus.val() 
    };
     
    var accessMode = 1;//Add Mode
    if (hdnExitInterviewId.val() != null && hdnExitInterviewId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { exitinterviewViewModel: exitinterviewViewModel };
    $.ajax({
        url: "../ExitInterview/AddEditExitInterview?accessMode=" + accessMode + "",
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
                      window.location.href = "../ExitInterview/ListExitInterview";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../ExitInterview/AddEditExitInterview";
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
    $("#txtExitInterviewNo").val("");
    $("#hdnExitInterviewId").val("0");
    $("#txtExitInterviewDate").val("");
    $("#ddlSeparationApplication").val("0");
    $("#hdnEmployeeId").val("0");
    $("#txtEmployee").val("");
    $("#txtInterviewDescription").val("");
    $("#hdnInterviewByUserId").val("0");
    $("#txtInterviewRemarks").val("");
    $("#txtInterviewByUser").val(""); 
    $("#ddlInterviewStatus").val("Final");
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
 
function GetSeparationApplicationDetail() {
    var applicationId = $("#ddlSeparationApplication option:selected").val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SeparationApplication/GetSeparationApplicationDetail",
        data: { applicationId: applicationId },
        dataType: "json",
        success: function (data) {
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#txtEmployee").val(data.EmployeeName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}
 
 
 