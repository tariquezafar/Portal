$(document).ready(function () {
   
    $("#txtEmployeeClearanceNo").attr('readOnly', true);
    $("#txtEmployee").attr('readOnly', true);
    $("#ddlClearanceTemplate").attr('disabled', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    
    BindClearanceTemplateList();
    BindSeparationClearList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnEmployeeClearanceId = $("#hdnEmployeeClearanceId");
    if (hdnEmployeeClearanceId.val() != "" && hdnEmployeeClearanceId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetEmployeeClearanceProcessDetail(hdnEmployeeClearanceId.val());
       }, 2000);
         

        if (hdnAccessMode.val() == "3") {
            $("#btnUpdate").hide();
            
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
        }
        else {
            $("#btnUpdate").show();
        }
    }
    else {
        $("#btnUpdate").hide();
        
    }

    var employeeClearanceProcesses = [];
    GetEmployeeClearances(employeeClearanceProcesses);
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

function GetEmployeeClearances(employeeClearanceProcesses) {
    var hdnEmployeeClearanceId = $("#hdnEmployeeClearanceId");
    var requestData = { employeeClearanceProcesses: employeeClearanceProcesses, employeeClearanceId: hdnEmployeeClearanceId.val() };
    $.ajax({
        url: "../EmployeeClearanceProcess/GetEmployeeClearances",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divClearanceProcessList").html("");
            $("#divClearanceProcessList").html(err);
        },
        success: function (data) {
            $("#divClearanceProcessList").html("");
            $("#divClearanceProcessList").html(data);
            ShowHideClearanceTemplateDetailPanel(2);
        }
    });
}

function BindSeparationClearList() {
    $.ajax({
        type: "GET",
        url: "../ClearanceTemplate/GetSeparationClearListForClearanceTemplate",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSeparationClearList").append($("<option></option>").val(0).html("Select Separation Clear List"));
            $.each(data, function (i, item) {
                $("#ddlSeparationClearList").append($("<option></option>").val(item.SeparationClearListId).html(item.SeparationClearListName));
            });
        },
        error: function (Result) {
            $("#ddlSeparationClearList").append($("<option></option>").val(0).html("Select Separation Clear List"));
        }
    });
}
function BindClearanceTemplateList() {
    $.ajax({
        type: "GET",
        url: "../EmployeeClearanceProcess/GetClearanceTemplateList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlClearanceTemplate").append($("<option></option>").val(0).html("Select Clearance Template"));
            $.each(data, function (i, item) {
                $("#ddlClearanceTemplate").append($("<option></option>").val(item.ClearanceTemplateId).html(item.ClearanceTemplateName));
            });
        },
        error: function (Result) {
            $("#ddlClearanceTemplate").append($("<option></option>").val(0).html("Select Clearance Template"));
        }
    });
}

function GetEmployeeClearanceProcessDetail(employeeClearanceId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../EmployeeClearanceProcess/GetEmployeeClearanceProcessDetail",
        data: { employeeClearanceId: employeeClearanceId },
        dataType: "json",
        success: function (data) {


            $("#txtEmployeeClearanceNo").val(data.EmployeeClaaranceNo);
            $("#hdnEmployeeClearanceId").val(data.EmployeeClearanceId);
            $("#txtEmployee").val(data.EmployeeName);
            $("#hdnEmployeeId").val(data.EmployeeId);
            $("#ddlClearanceTemplate").val(data.ClearanceTemplateId);

            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
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
    
    var hdnEmployeeClearanceId = $("#hdnEmployeeClearanceId");
    var txtEmployee = $("#txtEmployee");
    var hdnEmployeeId = $("#hdnEmployeeId");
    var ddlClearanceTemplate = $("#ddlClearanceTemplate");
    
    if (txtEmployee.val().trim() == "") {
        ShowModel("Alert", "Please Enter Employee Name")
        return false;
    }
    if (hdnEmployeeId.val() == "" || hdnEmployeeId.val() == "0") {
        ShowModel("Alert", "Please select Employee")
        return false;
    }
    if (ddlClearanceTemplate.val() == "" || ddlClearanceTemplate.val() == "0") {
        ShowModel("Alert", "Please select Clearance Template")
        return false;
    } 
    var employeeClearanceProcessViewModel = {
        EmployeeClearanceId: hdnEmployeeClearanceId.val(),
        EmployeeId: hdnEmployeeId.val(),
        ClearanceTemplateId: ddlClearanceTemplate.val(),
        ClearanceFinalStatus: "Pending"
       
    }; 
     

    var employeeClearanceProcessDetailList = [];
    $('#tblClearanceTemplate tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var employeeClearanceDetailId = $row.find("#hdnEmployeeClearanceDetailId").val();
        var separationClearListId = $row.find("#hdnSeparationClearListId").val();
        var clearanceByUserId = $row.find("#hdnClearanceByUserId").val();
         var clearanceStatus = $row.find("#hdnClearanceStatus").val();
        var clearanceRemarks = $row.find("#hdnClearanceRemarks").val();
        
        if (employeeClearanceDetailId != undefined) {
            var employeeClearanceProcessDetail= {
                EmployeeClearanceDetailId: employeeClearanceDetailId,
                SeparationClearListId: separationClearListId,
                ClearanceByUserId: clearanceByUserId,
                ClearanceStatus: clearanceStatus,
                ClearanceRemarks: clearanceRemarks
            };
            employeeClearanceProcessDetailList.push(employeeClearanceProcessDetail);
        } 
    });

    var accessMode = 1;//Add Mode
    if (hdnEmployeeClearanceId.val() != null && hdnEmployeeClearanceId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { employeeClearanceProcess: employeeClearanceProcessViewModel, employeeClearanceProcessDetails: employeeClearanceProcessDetailList };
    $.ajax({
        url: "../EmployeeClearanceProcess/AddEditEmployeeClearance?accessMode=" + accessMode + "",
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
                      window.location.href = "../EmployeeClearanceProcess/ListEmployeeClearance";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../EmployeeClearanceProcess/AddEditEmployeeClearance";
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
    $("#txtEmployeeClearanceNo").val("");
    $("#hdnEmployeeClearanceId").val("0");
    $("#txtEmployee").val("");
    $("#hdnEmployeeId").val("0");
    $("#ddlClearanceTemplate").val("0");
    $("#btnUpdate").hide(); 
}
 
function EditClearanceTemplateDetailRow(obj) {
    var row = $(obj).closest("tr");
    var employeeClearanceDetailId = $(row).find("#hdnEmployeeClearanceDetailId").val();
    var taxSequenceNo = $(row).find("#hdnTaxSequenceNo").val();
    var separationClearListId = $(row).find("#hdnSeparationClearListId").val();
    var clearanceByUserId = $(row).find("#hdnClearanceByUserId").val();
    var clearanceByUserName = $(row).find("#hdnClearanceByUserName").val();
    var clearanceStatus = $(row).find("#hdnClearanceStatus").val();
    var clearanceRemarks = $(row).find("#hdnClearanceRemarks").val();

    $("#hdnEmployeeClearanceDetailId").val(employeeClearanceDetailId);
    $("#hdnTaxSequenceNo").val(taxSequenceNo);
    $("#ddlSeparationClearList").val(separationClearListId);

    $("#txtClearanceByUser").val(clearanceByUserName);
    $("#hdnClearanceByUserId").val(clearanceByUserId);

    $("#ddlClearanceStatus").val(clearanceStatus);
    $("#txtClearanceRemarks").val(clearanceRemarks);

    $("#btnUpdateClearanceDetail").show();
    ShowHideClearanceTemplateDetailPanel(1);

}
function AddClearanceTemplateDetail(action) {
    var taxEntrySequence = 0;
    var flag = true;
    var hdnTaxSequenceNo = $("#hdnTaxSequenceNo");
    var ddlSeparationClearList = $("#ddlSeparationClearList");
    var hdnEmployeeClearanceDetailId = $("#hdnEmployeeClearanceDetailId");

    var txtClearanceByUser = $("#txtClearanceByUser");
    var hdnClearanceByUserId = $("#hdnClearanceByUserId");

    var ddlClearanceStatus = $("#ddlClearanceStatus");
    var txtClearanceRemarks = $("#txtClearanceRemarks");



    if (ddlSeparationClearList.val() == "" || ddlSeparationClearList.val() == 0) {
        ShowModel("Alert", "Please Select Separation Clear List")
        return false;
    }
    if (txtClearanceByUser.val() == "" || txtClearanceByUser.val() == 0) {
        ShowModel("Alert", "Please Select User")
        return false;
    }

    if (ddlClearanceStatus.val() == "" || ddlClearanceStatus.val() == 0) {
        ShowModel("Alert", "Please Select Clearance Status")
        return false;
    }
    if (txtClearanceRemarks.val() == "" ) {
        ShowModel("Alert", "Please enter Clearance Remarks")
        return false;
    }


    var employeeClearanceList = [];
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        taxEntrySequence = 1;
    }
    $('#tblClearanceTemplate tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var employeeClearanceDetailId = $row.find("#hdnEmployeeClearanceDetailId").val();
        var separationClearListId = $row.find("#hdnSeparationClearListId").val();
        var separationClearListName= $row.find("#hdnSeparationClearListName").val();
        var clearanceByUserName = $row.find("#hdnClearanceByUserName").val();
        var clearanceByUserId = $row.find("#hdnClearanceByUserId").val();
        var clearanceStatus = $row.find("#hdnClearanceStatus").val();
        var clearanceRemarks = $row.find("#hdnClearanceRemarks").val();


        if (employeeClearanceDetailId != undefined) {
            if (action == 1 || (hdnTaxSequenceNo.val() != taxSequenceNo)) {
                
                var employeeClearance= {
                    EmployeeClearanceDetailId: employeeClearanceDetailId,
                    TaxSequenceNo: taxSequenceNo,
                    SeparationClearListId: separationClearListId,
                    SeparationClearListName: separationClearListName,
                    ClearanceByUserId: clearanceByUserId,
                    ClearanceByUserName: clearanceByUserName,
                    ClearanceStatus: clearanceStatus,
                    ClearanceRemarks: clearanceRemarks
                };
                employeeClearanceList.push(employeeClearance);
                taxEntrySequence = parseInt(taxEntrySequence) + 1;

            }

            else if (hdnEmployeeClearanceDetailId.val() == employeeClearanceDetailId && hdnTaxSequenceNo.val() == taxSequenceNo) {
                var employeeClearance = {
                    EmployeeClearanceDetailId: hdnEmployeeClearanceDetailId.val(),
                    TaxSequenceNo: hdnTaxSequenceNo.val(),
                    SeparationClearListId: ddlSeparationClearList.val(),
                    SeparationClearListName: $("#ddlSeparationClearList option:selected").text(),
                    ClearanceByUserId: hdnClearanceByUserId.val(),
                    ClearanceByUserName: txtClearanceByUser.val(),
                    ClearanceStatus: ddlClearanceStatus.val(),
                    ClearanceRemarks: txtClearanceRemarks.val()
                };
                employeeClearanceList.push(employeeClearance);
                hdnTaxSequenceNo.val("0");
            }
        }
    });
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        hdnTaxSequenceNo.val(taxEntrySequence);
    }
    if (action == 1) {
        var employeeClearanceAddEdit = {
            EmployeeClearanceDetailId: hdnEmployeeClearanceDetailId.val(),
            TaxSequenceNo: hdnTaxSequenceNo.val(),
            SeparationClearListId: ddlSeparationClearList.val(),
            SeparationClearListName: $("#ddlSeparationClearList option:selected").text(),
            ClearanceByUserId: hdnClearanceByUserId.val(),
            ClearanceByUserName: txtClearanceByUser.val(),
            ClearanceStatus: ddlClearanceStatus.val(),
            ClearanceRemarks: txtClearanceRemarks.val()
        };
        employeeClearanceList.push(employeeClearanceAddEdit);
        hdnTaxSequenceNo.val("0");
    }
    if (flag == true) {
        GetEmployeeClearances(employeeClearanceList);
    }

}


function ShowHideClearanceTemplateDetailPanel(action) {
    if (action == 1) {
        $(".clearancetemplatedetailsection").show();
    }
    else {
        $(".clearancetemplatedetailsection").hide();
        $("#txtClearanceByUser").val("");
        $("#hdnClearanceByUserId").val("0");
        $("#ddlSeparationClearList").val("0");
        $("#ddlClearanceStatus").val("Pending");
        $("#txtClearanceRemarks").val("");
        $("#btnUpdateClearanceDetail").hide();
    }
}