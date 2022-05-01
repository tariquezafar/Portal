$(document).ready(function () { 
    $("#txtFromDate").attr('readOnly', true);
    $("#txtToDate").attr('readOnly', true);
    $("#txtFromDate,#txtToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) { 
        }
    });
    BindSeparationCategoryList();
    BindSeparationClearList(); 
    BindDepartmentList();
    $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));

    var hdnAccessMode = $("#hdnAccessMode");

    var hdnClearanceTemplateId = $("#hdnClearanceTemplateId");
    if (hdnClearanceTemplateId.val() != "" && hdnClearanceTemplateId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetClearanceTemplateDetail(hdnClearanceTemplateId.val());
       }, 2000);
         

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $("#btnAddNewTax").hide();
           
            
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

    $("#txtClearanceByUser").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Lead/GetUserAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FullName, value: item.UserName, UserId: item.UserId, MobileNo: item.MobileNo };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtClearanceByUser").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtClearanceByUser").val(ui.item.label);
            $("#hdnClearanceByUserId").val(ui.item.UserId);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtClearanceByUser").val("");
                $("#hdnClearanceByUserId").val("");
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

    var templateDetails = [];
    GetClearanceTemplateDetailList(templateDetails);

  
 
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

function BindDepartmentList() {
    $.ajax({
        type: "GET",
        url: "../Employee/GetDepartmentList",
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
function BindDesignationList(designationId) {
    var departmentId = $("#ddlDepartment option:selected").val();
    $("#ddlDesignation").val(0);
    $("#ddlDesignation").html("");
    if (departmentId != undefined && departmentId != "" && departmentId != "0") {
        var data = { departmentId: departmentId };
        $.ajax({
            type: "GET",
            url: "../Employee/GetDesignationList",
            data: data,
            asnc: false,
            dataType: "json",
            success: function (data) {
                $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
                $.each(data, function (i, item) {
                    $("#ddlDesignation").append($("<option></option>").val(item.DesignationId).html(item.DesignationName));
                });
                $("#ddlDesignation").val(designationId);
            },
            error: function (Result) {
                $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
            }
        });
    }
    else {

        $("#ddlDesignation").append($("<option></option>").val(0).html("-Select Designation-"));
    }
}


function GetClearanceTemplateDetailList(templateDetails) {
    var hdnClearanceTemplateId = $("#hdnClearanceTemplateId");
    var requestData = { details: templateDetails, clearancetemplateId: hdnClearanceTemplateId.val() };
    $.ajax({
        url: "../ClearanceTemplate/GetClearanceTemplateDetailList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divClearanceTemplateDetailList").html("");
            $("#divClearanceTemplateDetailList").html(err);
        },
        success: function (data) {
            $("#divClearanceTemplateDetailList").html("");
            $("#divClearanceTemplateDetailList").html(data);
            ShowHideClearanceTemplateDetailPanel(2);
        }
    });
}


function GetClearanceTemplateDetail(clearancetemplateId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../ClearanceTemplate/GetClearanceTemplateDetail",
        data: { clearancetemplateId: clearancetemplateId },
        dataType: "json",
        success: function (data) {
            $("#txtClearanceTemplateName").val(data.ClearanceTemplateName); 
            $("#hdnClearanceTemplateId").val(data.ClearanceTemplateId);
            $("#ddlDepartment").val(data.DepartmentId);
            BindDesignationList(data.DesignationId);
            $("#ddlSeparationCategory").val(data.SeparationCategoryId);
            if (data.ClearanceTemplate_Status == true) {
                $("#chkstatus").attr("checked", true);
            }
            else {
                $("#chkstatus").attr("checked", false);
            }
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByUserName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByUserName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByUserName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }
             
            $("#btnAddNew").show();
            $("#btnPrint").show();
            $("#btnEmail").show(); 
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    }); 
}

function AddClearanceTemplateDetail(action) {
    var taxEntrySequence = 0;
    var flag = true;
    var hdnTaxSequenceNo = $("#hdnTaxSequenceNo"); 
    var hdnClearanceTemplateDetailId = $("#hdnClearanceTemplateDetailId");
    var hdnSeparationClearListId = $("#hdnSeparationClearListId");
    var ddlSeparationClearList = $("#ddlSeparationClearList"); 
    var hdnClearanceByUserId = $("#hdnClearanceByUserId");
    var txtClearanceByUser = $("#txtClearanceByUser");
     
   
    if (ddlSeparationClearList.val() == "" || ddlSeparationClearList.val()==0) {
        ShowModel("Alert", "Please Select Separation Clear List") 
        return false;
    }
    if (txtClearanceByUser.val() == "" || txtClearanceByUser.val() == 0) {
        ShowModel("Alert", "Please Select User")
        return false;
    }
    
    var tamplateDetailList = [];
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        taxEntrySequence = 1;
    }
    $('#tblClearanceTemplateDetailList tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var clearancetemplatedetailId = $row.find("#hdnClearanceTemplateDetailId").val(); 
        var separationclearlistId = $row.find("#hdnSeparationClearListId").val();
        var separationclearlistName = $row.find("#hdnSeparationClearListName").val(); 
        var clearancebyuserId = $row.find("#hdnClearanceByUserId").val();
        var clearancebyuserName = $row.find("#hdnClearanceByUser").val();
       
        
        if (separationclearlistId != undefined) {
            if (action == 1 || (hdnTaxSequenceNo.val() != taxSequenceNo)) { 
                if (separationclearlistId == ddlSeparationClearList.val() && clearancebyuserId == hdnClearanceByUserId.val()) {
                    ShowModel("Alert", "Separation Clear List with Same User already added!!!")
                    flag = false;
                    return false;
                } 
                var tamplateDetail = {
                    ClearanceTemplateDetailId: clearancetemplatedetailId,
                    TaxSequenceNo:taxSequenceNo, 
                    SeparationClearListId: separationclearlistId,
                    SeparationClearListName: separationclearlistName, 
                    ClearanceByUserId: clearancebyuserId,
                    ClearanceByUserName: clearancebyuserName,
                };
                tamplateDetailList.push(tamplateDetail);
                taxEntrySequence = parseInt(taxEntrySequence) + 1;

            }
        
            else if (hdnClearanceTemplateDetailId.val() == clearancetemplatedetailId && hdnTaxSequenceNo.val() == taxSequenceNo)
          {
                var tamplateDetail = {
                    ClearanceTemplateDetailId: hdnClearanceTemplateDetailId.val(),
                TaxSequenceNo: hdnTaxSequenceNo.val(), 
                SeparationClearListId: ddlSeparationClearList.val(),
                SeparationClearListName: $("#ddlSeparationClearList option:selected").text(),
                ClearanceByUserId: hdnClearanceByUserId.val(),
                ClearanceByUserName: txtClearanceByUser.val().trim(),
             
            };
                tamplateDetailList.push(tamplateDetail);
            hdnTaxSequenceNo.val("0");
        }
    }   
    });
    if (action == 1 && (hdnTaxSequenceNo.val() == "" || hdnTaxSequenceNo.val() == "0")) {
        hdnTaxSequenceNo.val(taxEntrySequence);
    }
    if (action == 1)
        {
        var tamplateDetailAddEdit = {
            ClearanceTemplateDetailId: hdnClearanceTemplateDetailId.val(),
            TaxSequenceNo: hdnTaxSequenceNo.val(), 
            SeparationClearListId: ddlSeparationClearList.val(),
            SeparationClearListName: $("#ddlSeparationClearList option:selected").text(),
            ClearanceByUserId: hdnClearanceByUserId.val(),
            ClearanceByUserName: txtClearanceByUser.val().trim(),
        };
        tamplateDetailList.push(tamplateDetailAddEdit);
       hdnTaxSequenceNo.val("0");
    }
    if (flag == true)
        {
        GetClearanceTemplateDetailList(tamplateDetailList);
    }
    
}


function EditClearanceTemplateDetailRow(obj) {
    var row = $(obj).closest("tr");
    var clearancetemplatedetailId = $(row).find("#hdnClearanceTemplateDetailId").val();
    var taxSequenceNo = $(row).find("#hdnTaxSequenceNo").val();
    var separationclearlistId = $(row).find("#hdnSeparationClearListId").val();
    var clearancebyuserId = $(row).find("#hdnClearanceByUserId").val();
    var clearancebyuserName = $(row).find("#hdnClearanceByUser").val(); 
    $("#ddlSeparationClearList").val(separationclearlistId);
    $("#hdnClearanceTemplateDetailId").val(clearancetemplatedetailId);
    $("#hdnTaxSequenceNo").val(taxSequenceNo);
    $("#hdnClearanceByUserId").val(clearancebyuserId);
    $("#txtClearanceByUser").val(clearancebyuserName);
    $("#btnAddAppraisalTemplateDetail").hide();
    $("#btnUpdateClearanceAppraisalTemplateDetail").show();
    ShowHideClearanceTemplateDetailPanel(1);
   
} 
function RemoveClearanceTemplateDetailRow(obj) {
    if (confirm("Do you want to remove selected Clearance Template Detail?")) {
        var row = $(obj).closest("tr");
        var templateDetailId = $(row).find("#hdnClearanceTemplateDetailId").val();
        ShowModel("Alert", "Clearance Template Detail Removed from List.");
        row.remove();  
    }
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


function BindSeparationCategoryList() {
    $.ajax({
        type: "GET",
        url: "../SeparationApplication/GetSeparationCategoryForSeparationApplicationList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlSeparationCategory").append($("<option></option>").val(0).html("Select Separation Category"));
            $.each(data, function (i, item) {
                $("#ddlSeparationCategory").append($("<option></option>").val(item.SeparationCategoryId).html(item.SeparationCategoryName));
            });
        },
        error: function (Result) {
            $("#ddlSeparationCategory").append($("<option></option>").val(0).html("Select Separation Category"));
        }
    });
} 

 
function SaveData() {
    var txtClearanceTemplateName = $("#txtClearanceTemplateName");
    var hdnClearanceTemplateId = $("#hdnClearanceTemplateId");
    var ddlDepartment = $("#ddlDepartment");
    var ddlDesignation = $("#ddlDesignation");
    var ddlSeparationCategory = $("#ddlSeparationCategory"); 
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtClearanceTemplateName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Clearance Template Name")
        txtClearanceTemplateName.focus();
        return false;
    }
    if (ddlDepartment.val() == "" || ddlDepartment.val() == "0") {
        ShowModel("Alert", "Please select Department")
        ddlDepartment.focus();
        return false;
    }
    if (ddlDesignation.val() == "" || ddlDesignation.val() == "0") {
        ShowModel("Alert", "Please select Designation")
        ddlDesignation.focus();
        return false;
    }
    if (ddlSeparationCategory.val() == "" || ddlSeparationCategory.val() == "0") {
        ShowModel("Alert", "Please select Separation Category")
        ddlSeparationCategory.focus();
        return false;
    }
    var clearancetemplateViewModel = {
        ClearanceTemplateId: hdnClearanceTemplateId.val(),
        ClearanceTemplateName: txtClearanceTemplateName.val().trim(),
        DepartmentId: ddlDepartment.val(),
        DesignationId: ddlDesignation.val(),
        SeparationCategoryId: ddlSeparationCategory.val(),
        ClearanceTemplate_Status: chkstatus, 
    }; 
    var accessMode = 1;//Add Mode
    if (hdnClearanceTemplateId.val() != null && hdnClearanceTemplateId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var itemSelectionStatus = false;
    var tamplateDetailList = []; 
    $('#tblClearanceTemplateDetailList tr').each(function (i, row) {
        var $row = $(row);
        var taxSequenceNo = $row.find("#hdnTaxSequenceNo").val();
        var clearancetemplatedetailId = $row.find("#hdnClearanceTemplateDetailId").val(); 
        var separationclearlistId = $row.find("#hdnSeparationClearListId").val();
        var separationclearlistName = $row.find("#hdnSeparationClearListName").val(); 
        var clearancebyuserId = $row.find("#hdnClearanceByUserId").val();
        var clearancebyuserName = $row.find("#hdnClearanceByUser").val();
        if (separationclearlistId != undefined) { 
            var tamplateDetail = {
                ClearanceTemplateDetailId: clearancetemplatedetailId,
                TaxSequenceNo:taxSequenceNo, 
                SeparationClearListId: separationclearlistId,
                SeparationClearListName: separationclearlistName, 
                ClearanceByUserId: clearancebyuserId,
                ClearanceByUserName: clearancebyuserName,
            };
            tamplateDetailList.push(tamplateDetail);
            itemSelectionStatus = true;
        }
    }); 
    
    if (itemSelectionStatus==false) {
        ShowModel("Alert", "Please select atleast single Separation Clear List")
        return false;
    }

    var requestData = { clearancetemplateViewModel: clearancetemplateViewModel, templateDetails: tamplateDetailList };
    $.ajax({
        url: "../ClearanceTemplate/AddEditClearanceTemplate?accessMode=" + accessMode + "",
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
                      window.location.href = "../ClearanceTemplate/ListClearanceTemplate";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../ClearanceTemplate/AddEditClearanceTemplate";
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
    $("#txtClearanceTemplateName").val("");
    $("#hdnClearanceTemplateId").val("0");
    $("#ddlDepartment").val("0"); 
    $("#ddlDesignation").val("0");
    $("#ddlSeparationCategory").val("0"); 
    $("#chkstatus").val(""); 
    $("#btnSave").show();
    $("#btnUpdate").hide(); 
}
 
function ShowHideClearanceTemplateDetailPanel(action) {
    if (action == 1) {
        $(".clearancetemplatedetailsection").show();
    }
    else {
        $(".clearancetemplatedetailsection").hide();
        $("#txtClearanceByUser").val("");
        $("#hdnClearanceByUserId").val("0");
        $("#hdnSeparationClearListId").val("0");
        $("#ddlSeparationClearList").val("0");
        
        $("#btnAddAppraisalTemplateDetail").show();
        $("#btnUpdateClearanceAppraisalTemplateDetail").hide();
    }
}


 
 