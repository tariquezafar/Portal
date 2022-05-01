$(document).ready(function () {

    BindCompanyBranchList();
    BindSLTypeList();
    BindCostCenterList();
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnSLId = $("#hdnSLId");
    if (hdnSLId.val() != "" && hdnSLId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
          function () {
              GetSLDetail(hdnSLId.val());
          }, 1000);
        
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
    
    
    $("#txtPostingGLId").autocomplete({
        minLength: 0,
     
        source: function (request, response) {
            $.ajax({
                url: "../SL/GetPostingGLAutoCompleteList",
                type: "GET",
                dataType: "json", 
                data: { term: request.term, slTypeId: $("#ddlSLTypeId").val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.GLHead, value: item.GLId, code: item.GLCode};
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#ddlPostingGLId").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtPostingGLId").val(ui.item.label);
            $("#hdntxtPostingGLId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtPostingGLId").val("");
                $("#hdntxtPostingGLId").val("0");
                ShowModel("Alert", "Please select GL from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.code + "</b></div>")
      .appendTo(ul);
}; 

});




function BindSLTypeList() {
    $.ajax({
        type: "GET",
        url: "../SL/GetSLTypeList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $.each(data, function (i, item) {
                $("#ddlSLTypeId").append($("<option></option>").val(item.SLTypeId).html(item.SLTypeName));
            });
        },
        error: function (Result) {
            $("#ddlSLTypeId").append($("<option></option>").val(0).html("NA"));
        }
    });
}

function BindCostCenterList() {
    $.ajax({
        type: "GET",
        url: "../SL/GetCostCenterList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $.each(data, function (i, item) {
                $("#ddlCostCenterId").append($("<option></option>").val(item.CostCenterId).html(item.CostCenterName));
            });
        },
        error: function (Result) {
            $("#ddlCostCenterId").append($("<option></option>").val(0).html("Select Cost Center Name"));
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

function GetSLDetail(slId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SL/GetSLDetail",
        data: { sLId: slId },
        dataType: "json",
        success: function (data) {
            $("#hdnSLId").val(slId);
            $("#txtSLCode").val(data.SLCode);
            $("#txtSLHead").val(data.SLHead);
            $("#txtRefCode").val(data.RefCode);
            $("#ddlSLTypeId").val(data.SLTypeId);
            $("#txtPostingGLId").val(data.GLHead);
            $("#hdntxtPostingGLId").val(data.PostingGLId);
            $("#ddlCostCenterId").val(data.CostCenterId);

            $("#ddlCompanyBranch").val(data.CompanyBranchId);

            if (data.SL_Status == true) {
                $("#chkstatus").attr("checked", true);
            }
            else {
                $("#chkstatus").attr("checked", false);
            }
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}

function SaveData(action) {
    var hdnSLId = $("#hdnSLId");
    var txtSLCode = $("#txtSLCode");
    var txtSLHead = $("#txtSLHead");
    var txtRefCode = $("#txtRefCode");
    var ddlSLTypeId = $("#ddlSLTypeId"); 
    var hdntxtPostingGLId = $("#hdntxtPostingGLId");
    var ddlCostCenterId = $("#ddlCostCenterId");
    var chkstatus = $("#chkstatus").is(":checked") ? true : false;

    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (txtSLCode.val().trim() == "") {
        ShowModel("Alert", "Please enter SL Code")
        txtSLCode.focus();
        return false;
   }

    if (txtSLHead.val().trim() == "") {
       ShowModel("Alert", "Please enter SL Head")
       txtSLHead.focus();
       return false;
    }
    if (txtRefCode.val().trim() == "") {
        ShowModel("Alert", "Please enter Ref. Code")
        txtRefCode.focus();
        return false;
    }

    if (ddlSLTypeId.val().trim() == "0") {
       ShowModel("Alert", "Please Select SL Type")
       ddlSLTypeId.focus();
       return false;
   }

    if (ddlCostCenterId.val().trim() == "0") {
        ShowModel("Alert", "Please Select Cost Center")
        ddlCostCenterId.focus();
        return false;
   }
    if (ddlCompanyBranch.val() == "0" || ddlCompanyBranch.val() == "") {
        ShowModel("Alert", "Please Select Company Branch")
        ddlCompanyBranch.focus();
        return false;
    }
    var slViewModel = { 
        SLId:hdnSLId.val(),
        SLCode:txtSLCode.val(),
        SLHead:txtSLHead.val(),
        RefCode:txtRefCode.val(),
        SLTypeId: ddlSLTypeId.val(),
        PostingGLId: hdntxtPostingGLId.val(),
        CostCenterId: ddlCostCenterId.val(),
        CompanyBranchId: ddlCompanyBranch.val(),
        SL_Status: chkstatus,
    };

    var accessMode = 1;//Add Mode
    if (hdnSLId.val() != null && hdnSLId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { sLViewModel: slViewModel };
    $.ajax({
        url: "../SL/AddEditSL?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                if (accessMode == 2)
                {
                    window.location.href = "../SL/ListSL";
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
    $("#hdnSLId").val("0");
    $("#txtSLCode").val("");
    $("#txtSLHead").val("");
    $("#txtRefCode").val("");
    $("#ddlSLTypeId").val("0");
    $("#hdntxtPostingGLId").val("0");
    $("#txtPostingGLId").val("");
    $("#ddlCostCenterId").val("0");
   $("#chkstatus").prop("checked", true);

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