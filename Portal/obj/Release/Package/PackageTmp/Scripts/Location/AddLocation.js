$(document).ready(function () {  
 
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);  
    BindCompanyBranchList();
   
 
   
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnLocationId = $("#hdnLocationId");
    if (hdnLocationId.val() != "" && hdnLocationId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetLocationDetail(hdnLocationId.val());
       }, 2000); 

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $("#chkstatus").attr('disabled', true);
            $("#chkIsStoreLocation").attr('disabled', true);
            
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
 
 
function GetLocationDetail(locationId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Location/GetLocationDetail",
        data: { locationId: locationId },
        dataType: "json",
        success: function (data) {
            $("#txtLocationName").val(data.LocationName);
            $("#hdnLocationId").val(data.LocationId);
            $("#txtLocationCode").val(data.LocationCode);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);  
            if (data.IsStoreLocation == true) {
                $("#chkIsStoreLocation").attr("checked", true);
            }
            else {
                $("#chkIsStoreLocation").attr("checked", false);
            }
            if (data.LocationStatus == true) {
                $("#chkstatus").attr("checked", true);
            }
            else {
                $("#chkstatus").attr("checked", false);
            }
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
    var txtLocationName = $("#txtLocationName");
    var hdnLocationId = $("#hdnLocationId");
    var txtLocationCode = $("#txtLocationCode");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var chkIsStoreLocation = $("#chkIsStoreLocation").is(':checked') ? true : false;
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    if (txtLocationName.val() == "") {
        ShowModel("Alert", "Please Enter Location Name")
        return false;  
    }  
    if (txtLocationCode.val() == "") {
        ShowModel("Alert", "Please Enter Location Code")
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }

    var locationViewModel = {
        LocationId: hdnLocationId.val(),
        LocationName: txtLocationName.val().trim(),
        LocationCode: txtLocationCode.val().trim(),
        CompanyBranchId: ddlCompanyBranch.val(),
        IsStoreLocation: chkIsStoreLocation,
        LocationStatus: chkstatus
    }; 

    var accessMode = 1;//Add Mode
    if (hdnLocationId.val() != null && hdnLocationId.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { locationViewModel: locationViewModel };
    $.ajax({
        url: "../Location/AddEditLocation?AccessMode=" + accessMode + "",
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
                       //window.location.href = "../Location/AddEditLocation?LocationId=" + data.trnId + "&AccessMode=2";
                       window.location.href = "../Location/ListLocation";
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
    $("#txtLocationName").val("");
    $("#hdnLocationId").val("0");
    $("#txtLocationCode").val("");
    $("#txtCompanyBranch").val("0");
    $("#chkIsStoreLocation").prop("checked", true);
    $("#chkstatus").prop("checked", true);
    $("#btnSave").show();
    $("#btnUpdate").hide();


}
 
 
 
 
 