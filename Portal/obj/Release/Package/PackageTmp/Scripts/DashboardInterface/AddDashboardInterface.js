
$(document).ready(function () { 
  
    BindCompanyBranchList();


    var hdnAccessMode = $("#hdnAccessMode");
    var hdnitemId = $("#hdnitemId");
    if (hdnitemId.val() != "" && hdnitemId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetDashboardInterfaceDetail(hdnitemId.val());
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
        }
        else
        {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
        }
    }
    else
    {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }
    $("#txtProductName").focus();
        


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
 
 

 
 

function GetDashboardInterfaceDetail(itemId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../DashboardInterface/GetDashboardInterfaceDetail",
        data: { itemId: itemId },
        dataType: "json",
        success: function (data) {
            $("#txtItemName").val(data.ItemName);
            $("#txtItemDescription").val(data.ItemDescription);
            $("#ddlModuleName").val(data.ModuleName);
            $("#txtContainerNo").val(data.ContainerNo);
            $("#txtContainerName").val(data.ContainerName);
            $("#txtSequenceNo").val(data.SequenceNo);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);


            if (data.Status == true) {
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

function SaveData() {

    var txtItemName=$("#txtItemName");
    var txtItemDescription=$("#txtItemDescription");
    var ddlModuleName=$("#ddlModuleName");
    var txtContainerNo=$("#txtContainerNo");
    var txtContainerName=$("#txtContainerName");
    var txtSequenceNo=$("#txtSequenceNo");
    var ddlCompanyBranch=$("#ddlCompanyBranch");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    var hdnitemId = $("#hdnitemId");
    
    if (txtItemName.val().trim() == "") {
        ShowModel("Alert", "Please enter Item Name")
        txtItemName.focus();
        return false;
    }
    if (txtItemDescription.val().trim() == "") {
        ShowModel("Alert", "Please enter Item Description")
        txtItemDescription.focus();
        return false;
    }

    if (ddlModuleName.val() == "0") {
        ShowModel("Alert", "Please Select Module Name")
        ddlModuleName.focus();
        return false;
    }

    if (txtContainerNo.val().trim() == "") {
        ShowModel("Alert", "Please enter Container No.")
        txtContainerNo.focus();
        return false;
    }

    if (txtContainerName.val().trim() == "") {
        ShowModel("Alert", "Please enter Container Name.")
        txtContainerName.focus();
        return false;
    }

    if (txtSequenceNo.val().trim() == "") {
        ShowModel("Alert", "Please enter Sequence No.")
        txtSequenceNo.focus();
        return false;
    }



    
    if (ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Company Branch")
        ddlCompanyBranch.focus();
        return false;
    } 

   

    var dashboardinterfaceViewModel = {
        ItemId: hdnitemId.val(),
        ItemName: txtItemName.val().trim(),
        ItemDescription: txtItemDescription.val().trim(),
        ModuleName: ddlModuleName.val(),
        ContainerNo: txtContainerNo.val(),
        ContainerName: txtContainerName.val(),
        Status: chkstatus,
        CompanyBranchId: ddlCompanyBranch.val(),
        SequenceNo: txtSequenceNo.val()

      
    };
    var accessMode = 1;//Add Mode
    if (hdnitemId.val() != null && hdnitemId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { dashboardinterfaceViewModel: dashboardinterfaceViewModel };
    $.ajax({
        url: "../DashboardInterface/AddEditDashboardInterface?AccessMode=" + accessMode + "",
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
                window.location.href = "../DashboardInterface/AddEditDashboardInterface?itemId=" + data.trnId + "&AccessMode=3";
                
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

    $("#txtItemName").val("");
    $("#txtItemDescription").val("");
    $("#ddlModuleName").val("0");
    $("#txtContainerNo").val("");
    $("#txtContainerName").val("");
    $("#txtSequenceNo").val("");
    $("#ddlCompanyBranch").val("0");
    $("#chkstatus").prop("checked", true);

    
    

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
            //var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            //var hdnSessionUserID = $("#hdnSessionUserID");
            //if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
            //    $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
            //    $("#ddlCompanyBranch").attr('disabled', true);
            //    $(":input#ddlCompanyBranch").trigger('change');
            //}
        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Company Branch-"));
        }
    });
}