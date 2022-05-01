$(document).ready(function () {
    $("#txtNormalApprovedByUser").prop('disabled', true);
    $("#txtRevisedApprovedByUser").prop('disabled', true);
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnSOSettingId = $("#hdnSOSettingId");
    if (hdnSOSettingId.val() != "" && hdnSOSettingId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetSOSettingDetail(hdnSOSettingId.val());
     
        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
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
    $("#txtNormalApprovedByUser").focus();


    $("#txtNormalApprovedByUser").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../QuotationSettings/GetUserAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.UserName, value: item.UserId, FullName: item.FullName, MobileNo: item.MobileNo
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtNormalApprovedByUser").val(ui.item.FullName);
            return false;
        },
        select: function (event, ui) {
            $("#txtNormalApprovedByUser").val(ui.item.FullName);
            $("#hdnNormalApprovalByUserId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtNormalApprovedByUser").val("");
                $("#hdnNormalApprovalByUserId").val("0");
                ShowModel("Alert", "Please select User from List")

            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.FullName + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};




    $("#txtRevisedApprovedByUser").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SOSettings/GetUserAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: {
                    term: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.UserName, value: item.UserId, FullName: item.FullName, MobileNo: item.MobileNo
                        };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtRevisedApprovedByUser").val(ui.item.FullName);
            return false;
        },
        select: function (event, ui) {
            $("#txtRevisedApprovedByUser").val(ui.item.FullName);
            $("#hdnRevisedApprovalByUserId").val(ui.item.value);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtRevisedApprovedByUser").val("");
                $("#hdnRevisedApprovalByUserId").val("0");
                ShowModel("Alert", "Please select User from List") 
            }
            return false;
        }

    })
.autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.FullName + "</b><br>" + item.MobileNo + "</div>")
      .appendTo(ul);
};
     

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
 
function ValidEmailCheck(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};
  

function GetSOSettingDetail(sosettingId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../SOSettings/GetSOSettingDetail",
        data: { sosettingId: sosettingId },
        dataType: "json",
        success: function (data) { 
            if (data.NormalApprovalRequired == true) {
                $("#chkNormalApprovalRequired").attr("checked", true);
                $("#txtNormalApprovedByUser").attr("disabled", false);
            }
            else {
                $("#chkNormalApprovalRequired").attr("checked", false);
                $("#txtNormalApprovedByUser").attr("disabled", true);
            }  
            if (data.RevisedApprovalRequired == true) {
                $("#chkRevisedApprovalRequired").attr("checked", true);
                $("#txtRevisedApprovedByUser").attr("disabled", false);
            }
            else {
                $("#chkRevisedApprovalRequired").attr("checked", false);
                $("#txtRevisedApprovedByUser").attr("disabled", true);
            }
            $("#txtNormalApprovedByUser").val(data.NormalApprovalByUserName);
            $("#hdnNormalApprovalByUserId").val(data.NormalApprovalByUserId);
            $("#txtRevisedApprovedByUser").val(data.RevisedApprovalByUserName);
            $("#hdnRevisedApprovalByUserId").val(data.RevisedApprovalByUserId);
           
            if (data.SOSetting_Status == true) {
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
function changeNormalApproval() { 
    if ($("#chkNormalApprovalRequired").is(':checked')) {
        $("#txtNormalApprovedByUser").attr("disabled", false);
    }
    else {
        $("#txtNormalApprovedByUser").attr("disabled", true);
        $("#txtNormalApprovedByUser").val("");
    }
} 
function changeRevisedApproval() { 
    if ($("#chkRevisedApprovalRequired").is(':checked')) {
        $("#txtRevisedApprovedByUser").attr("disabled", false);
    }
    else {
        $("#txtRevisedApprovedByUser").attr("disabled", true);
        $("#txtRevisedApprovedByUser").val("");
    }
} 

function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}
function SaveData() {
    var hdnSOSettingId = $("#hdnSOSettingId");
    var chkNormalApprovalRequired = $("#chkNormalApprovalRequired").is(':checked') ? true : false;
    var chkRevisedApprovalRequired = $("#chkRevisedApprovalRequired").is(':checked') ? true : false;
    var hdnNormalApprovalByUserId = $("#hdnNormalApprovalByUserId");
    var hdnRevisedApprovalByUserId = $("#hdnRevisedApprovalByUserId");
    var txtNormalApprovedByUser = $("#txtNormalApprovedByUser");
    var txtRevisedApprovedByUser = $("#txtRevisedApprovedByUser");

    
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

  

    if (chkNormalApprovalRequired == true) {
        if (txtNormalApprovedByUser.val().trim() == "" || hdnNormalApprovalByUserId.val() == "0") {
            ShowModel("Alert", "Please Enter Normal Approved User Name")
            txtNormalApprovedByUser.focus();
            return false;
        } 
    }

    if (chkRevisedApprovalRequired == true) {
        if (txtRevisedApprovedByUser.val().trim() == "" || hdnRevisedApprovalByUserId.val() == "0") {
            ShowModel("Alert", "Please Enter Revised Approved User Name")
            txtRevisedApprovedByUser.focus();
            return false;
        } 
    }
     
    var sosettingViewModel = {
        SOSettingId: hdnSOSettingId.val(),
        NormalApprovalRequired: chkNormalApprovalRequired,
        NormalApprovalByUserId: hdnNormalApprovalByUserId.val(),
        RevisedApprovalRequired: chkRevisedApprovalRequired,
        RevisedApprovalByUserId: hdnRevisedApprovalByUserId.val(),
        SOSetting_Status: chkstatus 
    };
    var requestData = { sosettingViewModel: sosettingViewModel };
    $.ajax({
        url: "../SOSettings/AddEditSOSetting",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
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
       $("#hdnSOSettingId").val("0");
       $("#chkNormalApprovalRequired").prop("checked", false);
       $("#chkRevisedApprovalRequired").prop("checked", false);
       $("#txtNormalApprovedByUser").attr("disabled",true);
       $("#txtRevisedApprovedByUser").attr("disabled", true);
       $("#txtNormalApprovedByUser").val("");
       $("#txtRevisedApprovedByUser").val("");
       $("#hdnNormalApprovalByUserId").val("0");
       $("#hdnRevisedApprovalByUserId").val("0");
       $("#chkstatus").prop("checked", true); 
    }
