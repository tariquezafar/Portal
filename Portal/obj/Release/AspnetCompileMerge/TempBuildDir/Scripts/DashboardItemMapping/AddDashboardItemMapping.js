$(document).ready(function () {

    BindCompanyBranchList();
    //BindDashboardContainerList();
    BindRoleList()

    var hdnAccessMode = $("#hdnAccessMode");
    var hdnDashBoardContainerID = $("#hdnDashBoardItemMappingID");
    if (hdnDashBoardContainerID.val() != "" && hdnDashBoardContainerID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
       
        debugger
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
  

function DisplayDashboardContainerDetail() {
   
    debugger
    //hari 2018 new
    var ddlDashboardContainer = $("#ddlDashboardContainer").val();
    
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../DashboardItemMapping/GetDashboardContainerDetail",
        data: { dashBoardContainerID: ddlDashboardContainer },
        dataType: "json",
        success: function (data) {
                $("#txtDashBoardContainerDisplayName").val(data.ContainerDisplayName);
            $("#txtTotalItemInContainer").val(data.TotalItem);

        },
        error: function (err) {
            ShowModel("Error", err)

        //error: function (Result) {
        //    ShowModel("Alert", "Problem in Request");


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

    debugger

    var ddlModule = $("#ddlModule");
    var ddlDashboardContainer = $("#ddlDashboardContainer");
    var ddlRole = $("#ddlRole");
    var ddlCompanyBranch = $("#ddlCompanyBranch");            
   
    if (ddlModule.val().trim() == 0) {
        ShowModel("Alert", "Please Select Module Name.")
        ddlModule.focus();
        return false;
    }

    if (ddlDashboardContainer.val().trim() == 0) {
        ShowModel("Alert", "Please Select Dashboard Container No.")
        ddlDashboardContainer.focus();
        return false;
    }

    if (ddlRole.val().trim() == 0) {
        ShowModel("Alert", "Please Select Role ")
        ddlRole.focus();
        return false;
    }


    if (ddlCompanyBranch.val().trim() == 0) {
        ShowModel("Alert", "Please Select Branch ")
        ddlCompanyBranch.focus();
        return false;
    }
    

   
    var ddlModule = $("#ddlModule");
    var ddlDashboardContainer = $("#ddlDashboardContainer");
    var ddlRole = $("#ddlRole");
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    

    var dashboardItemMappingList = [];
    $("#tblDashboardItemMappingList tr:not(:has(th))").each(function (i, row) {
        var $row = $(row);

        var dashboardItemMappingID = $row.find("#hdnDashboardItemMappingID").val();
        var dashboardItemId = $row.find("#hdnDashboardItemId").val();
        var mappingStatus = $row.find("#chkMappingStatus").is(':checked') ? true : false;
        var containerNo = $row.find("#hdnContainerNo").val();
              
        
        if (dashboardItemMappingID != undefined) {
            var dashboardItemMapping = {
                DashboardItemMappingID: dashboardItemMappingID,
                DashboardItemId: dashboardItemId,
                MappingStatus: mappingStatus,
                ContainerNo:containerNo
            };
            dashboardItemMappingList.push(dashboardItemMapping);
            //mappingStatus = true;
        }
    });

           
    


    var accessMode = 1;//Add Mode
    //if (hdnDashBoardContainerID.val() != null && hdnDashBoardContainerID.val() != 0) {
    //    accessMode = 2;//Edit Mode
    //}
    var requestData = { dashboardItemMappingList: dashboardItemMappingList, moduleName: ddlModule.val().trim(),
                       containerID: ddlDashboardContainer.val().trim(),
                       roleId: ddlRole.val().trim(),
                        companyBranchID: ddlCompanyBranch.val().trim() };
    $.ajax({
        url: "../DashboardItemMapping/AddEditDashDashboardItemMapping?accessMode=" + accessMode + "",
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
                           window.location.href = "../DashboardItemMapping/AddEditDashDashboardItemMapping";
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
    debugger
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



//
function BindDashboardContainerList() {
    debugger

    $("#ddlDashboardContainer").val(0);
    $("#ddlDashboardContainer").html("");
    var moduleName = $("#ddlModule").val();
    

    $.ajax({
        type: "GET",
        url: "../DashboardItemMapping/BindDashboardContainerList",
        data: { moduleName: moduleName },
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlDashboardContainer").append($("<option></option>").val(0).html("Select Dashboard Container"));
            $.each(data, function (i, item) {
                $("#ddlDashboardContainer").append($("<option></option>").val(item.DashboardContainterID).html(item.ContainerName));
            });
            //var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            //var hdnSessionUserID = $("#hdnSessionUserID");

            //if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
            //    $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
            //    $("#ddlCompanyBranch").attr('disabled', true);
            //}
        },
        error: function (Result) {
            $("#ddlDashboardContainer").append($("<option></option>").val(0).html("Select Dashboard Container"));
        }
    });
}



function GetDashboardItemMappingDetail() {
    debugger
       

    var moduleName = $("#ddlModule").val();
    var ddlDashboardContainer = $("#ddlDashboardContainer").val();    
    var ddlCompanyBranch = $("#ddlCompanyBranch").val();
    var ddlrole = $("#ddlRole").val();
    
    if (moduleName.trim() == 0) {
        ShowModel("Alert", "Please Select Module Name.")
        ddlModule.focus();
        return false;
    }

    if (ddlDashboardContainer.trim() == 0) {
        ShowModel("Alert", "Please Select Dashboard Container No.")
        ddlDashboardContainer.focus();
        return false;
    }

    if (ddlrole.trim() == 0) {
        ShowModel("Alert", "Please Select Role ")
        ddlrole.focus();
        return false;
    }


    if (ddlCompanyBranch.trim() == 0) {
        ShowModel("Alert", "Please Select Branch ")
        ddlCompanyBranch.focus();
        return false;
    }







    var requestData ={
        moduleName: moduleName,
        containerID : ddlDashboardContainer,
        roleId :ddlrole,
        companyBranchID: ddlCompanyBranch}
    $.ajax({
       
        url: "../DashboardItemMapping/GetDashboardItemMappingDetail",
        data: requestData,
        dataType: "html",
        type: "GET",       
        error: function (err) {
            $("#divList").html("");
            $("#divList").html(err);
        },
        success: function (data) {
            $("#divList").html("");
            $("#divList").html(data);
        }
    });
}




function BindRoleList() {
    $.ajax({
        type: "GET",
        url: "../User/GetRoleList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlRole").append($("<option></option>").val(0).html("-Select Role-"));
            $.each(data, function (i, item) {
                $("#ddlRole").append($("<option></option>").val(item.RoleId).html(item.RoleName));
            });
        },
        error: function (Result) {
            $("#ddlRole").append($("<option></option>").val(0).html("-Select Role-"));
        }
    });
}
//End Code


