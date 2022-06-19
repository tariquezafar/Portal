$(document).ready(function () {
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnDashBoardContainerID = $("#hdnDashBoardContainerID");
    if (hdnDashBoardContainerID.val() != "" && hdnDashBoardContainerID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
       
        Getdashboardcontainerdetail(hdnDashBoardContainerID.val());

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
    $("#txtDashBoardContainerName").focus();

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
  

function Getdashboardcontainerdetail(hdnDashBoardContainerID) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../DashBoardContainer/GetDashboardContainerDetail",
        data: { dashBoardContainerID: hdnDashBoardContainerID },
        dataType: "json",
        success: function (data) {
            $("#txtDashBoardContainerName").val(data.ContainerName);
            $("#txtDashBoardContainerDisplayName").val(data.ContainerDisplayName);
            $("#txtContainerNo").val(data.ContainterNo);
            $("#txtTotalItem").val(data.TotalItem);
            $("#ddlModule").val(data.ModuleName);
           

            if (data.ContainerName != "") {
                $("#txtDashBoardContainerName").attr("disabled", false);
                $("#txtDashBoardContainerDisplayName").val(data.ContainerDisplayName);
                   }
            //if (data.Book_Status == true) {
            //    $("#chkstatus").attr("checked", true);
            //}
            //else {
            //    $("#chkstatus").attr("checked", false);
            //}
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
         
    });
}
function changeBookType() {
    var ddlBookType = $("#ddlBookType").val(); 
    if (ddlBookType != "0" && ddlBookType != undefined) { 
        if (ddlBookType == 'B' || ddlBookType == 'C') {
            $("#txtGLHead").attr("disabled", false);
        }
        else  {
            $("#txtGLHead").attr("disabled", true); 
        }
    }
   
} 


function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }

}

function SaveData() {
    var txtDashBoardContainerName = $("#txtDashBoardContainerName");
    var txtDashBoardContainerDisplayName = $("#txtDashBoardContainerDisplayName");
    var txtContainerNo = $("#txtContainerNo");
    var txtTotalItem = $("#txtTotalItem");   
    var hdnDashBoardContainerID = $("#hdnDashBoardContainerID");
    var ddlModule = $("#ddlModule");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    //if (txtGLCode.val() == "Bank" || txtGLCode.val()== "Cash") { 
    //    txtGLCodeVal = txtGLCode.val();
        
    //}
   
    if (txtDashBoardContainerName.val().trim() == "") {
        ShowModel("Alert", "Please Enter DashBoard Container Name")
        txtDashBoardContainerName.focus();
        return false;
    }
    if (txtDashBoardContainerDisplayName.val().trim() == "") {
        ShowModel("Alert", "Please Enter DashBoard Container Display Name.")
        txtDashBoardContainerDisplayName.focus();
        return false;
    }

    if (txtContainerNo.val().trim() == "") {
        ShowModel("Alert", "Please Enter DashBoard Container Number.")
        txtContainerNo.focus();
        return false;
    }
    
    if (parseInt( txtContainerNo.val().trim()) <= 0) {
        ShowModel("Alert", "DashBoard Container Number cannot be 0.")
        txtContainerNo.focus();
        return false;
    }

    
    if (txtTotalItem.val().trim() == "") {
        ShowModel("Alert", "Please Enter Total Item in a Container.")
        txtContainerNo.focus();
        return false;
    }
    
    if (parseInt( txtContainerNo.val().trim()) <= 0) {
        ShowModel("Alert", "DashBoard Container Number cannot be 0")
        txtContainerNo.focus();
        return false;
    }

      
    if (ddlModule.val().trim() == 0) {
        ShowModel("Alert", "Please Select Module Name.")
        ddlModule.focus();
        return false;
    }  

   
    //if (ddlCompanyBranch.val() == "0" || ddlCompanyBranch.val()=="") {
    //    ShowModel("Alert", "Please Select Company Branch")
    //    ddlCompanyBranch.focus();
    //    return false;
    //}
    

    var dashboardContainerViewModel = {
        DashboardContainterID: hdnDashBoardContainerID.val(),
        ContainerName: txtDashBoardContainerName.val().trim(),
        ContainerDisplayName: txtDashBoardContainerDisplayName.val().trim(),
        ContainterNo: txtContainerNo.val().trim(),
        TotalItem:txtTotalItem.val().trim(),        
        ModuleName: ddlModule.val().trim(),
        
    };
    var accessMode = 1;//Add Mode
    if (hdnDashBoardContainerID.val() != null && hdnDashBoardContainerID.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { dashboardContainerViewModel: dashboardContainerViewModel };
    $.ajax({
        url: "../DashBoardContainer/AddEditDashboardContainer?accessMode=" + accessMode + "",
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
                           window.location.href = "../DashboardContainer/ListDashboardContainer";
                       }, 1000);

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
        $("#hdnBookId").val("0"); 
        $("#txtBookName").val("");
        $("#txtBookCode").val("");
        $("#ddlBookType").val("0");
        $("#hdnGLCode").val("");
        $("#txtGLHead").val("");
        $("#txtBankBranch").val("");
        $("#txtBankAccountNo").val("");
        $("#txtIFSCCode").val("");
        $("#txtOverDraftLimit").val("");
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