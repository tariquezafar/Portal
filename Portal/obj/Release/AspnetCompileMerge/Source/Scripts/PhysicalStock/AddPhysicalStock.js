$(document).ready(function () {

    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtPhysicalAsOnDate").attr('readOnly', true);
    $("#txtPhysicalAsOnDate").css('cursor', 'pointer');
    $("#txtPhysicalStockNo").attr('readOnly', true);
    $("#txtPhysicalStockDate").attr('readOnly', true);
    $("#txtPhysicalStockDate").css('cursor', 'pointer');
    $("#txtPhysicalAsOnDate,#txtPhysicalStockDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        maxDate: '0',
        onSelect: function (selected) {

        }
    });



    GenerateReportParameters();
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnPhysicalStockID = $("#hdnPhysicalStockID");
    if (hdnPhysicalStockID.val() != "" && hdnPhysicalStockID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetPhysicalStockDetail(hdnPhysicalStockID.val());
       }, 2000);

        if (hdnAccessMode.val() == "3") {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
            $("select").attr('disabled', true);
            $(".editonly").hide();

        }
        else {
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").show();
            $(".editonly").show();
        }

    }
    else {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
        $(".editonly").show();
    }
    var physicalStockProductDetails = [];
    GetPhysicalStockList(physicalStockProductDetails);




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


function GetPhysicalStockDetail(physicalStockID) {

    $.ajax({
        type: "GET",
        asnc: false,
        url: "../PhysicalStock/GetPhysicalStockDetail",
        data: { physicalStockID: physicalStockID },
        dataType: "json",
        success: function (data) {

            $("#txtPhysicalStockNo").val(data.PhysicalStockNo);
            $("#hdnPhysicalStockID").val(data.PhysicalStockID);
            $("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtPhysicalStockDate").val(data.PhysicalStockDate);
            $("#txtPhysicalAsOnDate").val(data.PhysicalAsOnDate);
            $("#ddlPhysicalStockStatus").val(data.ApprovalStatus);
            // $("#txtProductName").val(data.TransferProductName);
            //  $("#hdnTransferProductId").val(data.TransferProductID);
            if (data.ApprovalStatus == "Final") {
                $("#btnUpdate").hide();
                $("#btnReset").hide();
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true);
                $(".editonly").hide();
            }


            $("#btnAddNew").show();
            $("#btnPrint").show();



        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });



}


function BindCompanyBranchList(branchId) {
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
            if (branchId != null) {
                $("#ddlCompanyBranch").val(branchId);
            }

        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}

function ShowHideDocumentPanel(action) {
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please Select Company Branch")
        return false;
    }

    if (action == 1) {
        $(".documentsection").show();
    }
    else {
        $(".documentsection").hide();

        $("#btnAddDocument").show();
        $("#btnUpdateDocument").hide();
    }
}

function SavePhisicalStock() {

    if (window.FormData !== undefined) {
        var uploadfile = document.getElementById('FileUpload1');
        var fileData = new FormData();
        if (uploadfile.value != '') {

            var fileUpload = $("#FileUpload1").get(0);
            var files = fileUpload.files;

            if (uploadfile.files[0].size > 50000000) {
                uploadfile.files[0].name.length = 0;
                ShowModel("Alert", "File is too big")
                uploadfile.value = "";
                return "";
            }

            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            fileData.append("companyBranchId", $("#ddlCompanyBranch").val());
            fileData.append("todate", $("#txtPhysicalAsOnDate").val());

        }
        else {

            ShowModel("Alert", "Please Add The Phisical Stock File !")
            return false;

        }

    } else {

        ShowModel("Alert", "FormData is not supported.")
        return "";
    }



    $.ajax({
        url: "../PhysicalStock/SavePhysicalStockDetail",
        type: "POST",
        asnc: false,
        contentType: false,
        processData: false,
        data: fileData,
        error: function (err) {

        },
        success: function (data) {
            $("#divBankStatementList").html("");
            $("#divBankStatementList").html(data);
        }
    });



    function GetProductDetail(productId, obj) {
        $.ajax({
            type: "GET",
            asnc: false,
            url: "../Product/GetAutoCompleteProductDetail",
            data: { productid: productId },
            dataType: "json",
            success: function (data) {
                $(obj).closest('tr').find('.txtProductName').val(data.SalePrice);
                $(obj).closest('tr').find('.hdnProductId').val(data.UOMName);
            },
            error: function (Result) {
                ShowModel("Alert", "Problem in Request");
            }
        });

    }
}


function GetPhysicalStockList(physicalStockProductDetails) {
    var hdnPhysicalStockID = $("#hdnPhysicalStockID");
    var requestData = { physicalStockProductDetail: physicalStockProductDetails, physicalStockID: hdnPhysicalStockID.val() };
    $.ajax({
        url: "../PhysicalStock/GetPhysicalStockDetailList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divBankStatementList").html("");
            $("#divBankStatementList").html(err);
        },
        success: function (data) {
            $("#divBankStatementList").html("");
            $("#divBankStatementList").html(data);
        }
    });
}

function SaveData() {
    var hdnPhysicalStockID = $("#hdnPhysicalStockID");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtPhysicalStockDate = $("#txtPhysicalStockDate");
    var txtPhysicalAsOnDate = $("#txtPhysicalAsOnDate");
    var ddlPhysicalStockStatus = $("#ddlPhysicalStockStatus")
    var fileUpload1 = $("#FileUpload1")
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch from list")
        return false;
    }
    if (fileUpload1.val() == "" ) {
        ShowModel("Alert", "Please Upload The Physical Stock File")
        return false;
    }

    var physicalStockViewModel = {
        PhysicalStockID: hdnPhysicalStockID.val(),
        PhysicalStockDate: txtPhysicalStockDate.val().trim(),
        PhysicalAsOnDate: txtPhysicalAsOnDate.val().trim(),
        CompanyBranchId: ddlCompanyBranch.val(),
        ApprovalStatus: ddlPhysicalStockStatus.val()
    };

    var physicalStockDetailList = [];
    var data = 0, phyqun = 0;
    //var hdnProductid=0, hdnTransferProductId=0;
    $('#tblPhysicalDetail tr').each(function (i, row) {

        var $row = $(row);
        var hdnProductid = $row.find("#hdnProductid").val();
        var hdnProductName = $row.find("#hdnProductName").val();
        var hdnProductMainGroupId = $row.find("#hdnProductMainGroupId").val();
        var hdnProductSubGroupId = $row.find("#hdnProductSubGroupId").val();
        var hdnUOMId = $row.find("#hdnUOMId").val();
        var hdnProductTypeId = $row.find("#hdnProductTypeId").val();
        var hdnPhysicalQTY = $row.find("#hdnPhysicalQTY").val();
        var hdnSystemQTY = $row.find("#hdnSystemQTY").val();
        var hdnDiffQTY = $row.find("#hdnDiffQTY").val();
        var hdnProductCode = $row.find("#hdnProductCode").val();
        var hdnTransferProductId = $row.find("#hdnTransferProductId").val();



        if (hdnProductid != undefined) {
            if (parseInt(hdnProductid) == parseInt(hdnTransferProductId)) {
                data = 1;
                ShowModel("Alert", "Product Name and Transfer To can't be same from Physical stock list");
                return false;

            }
            else {
                data = 0;
            }

            //if (parseInt(hdnPhysicalQTY) == 0) {
            //    phyqun = 1;
            //    ShowModel("Alert", "Physical Quantity can't 0");
            //    return false;

            //}
            //else {
            //    phyqun = 0;
            //}



            var physicalStock = {
                Productid: hdnProductid,
                ProductName: hdnProductName,
                ProductMainGroupId: hdnProductMainGroupId,
                ProductSubGroupId: hdnProductSubGroupId,
                UOMId: hdnUOMId,
                ProductTypeId: hdnProductTypeId,
                PhysicalQTY: hdnPhysicalQTY,
                SystemQTY: hdnSystemQTY,
                DiffQTY: hdnDiffQTY,
                ProductCode: hdnProductCode,
                TransferTo: hdnTransferProductId

            };


            physicalStockDetailList.push(physicalStock);
        }
    });

    if (data == 1) {
        ShowModel("Alert", "Product Name and Transfer To can't be same from Physical stock list");
        return false;

    }
    //if (phyqun == 1) {
    //    ShowModel("Alert", "Physical Quantity can't zero input");
    //    return false;

    //}



    var requestData = { physicalStockViewModel: physicalStockViewModel, physicalStockProductDetails: physicalStockDetailList };
    $.ajax({
        url: "../PhysicalStock/AddEditPhysicalStock",
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
                       window.location.href = "../PhysicalStock/AddEditPhysicalStock?physicalStockID=" + data.trnId + "&AccessMode=2";
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
    $("#hdnPhysicalStockID").val("0");
    $("#txtPhysicalStockDate").val($("#hdnCurrentDate").val());
    $("#txtPhysicalAsOnDate").val($("#hdnCurrentDate").val());
    $("#ddlCompanyBranch").val("0");
    $("#ddlPhysicalStockStatus").val("0");

}

function GenerateReportParameters() {

    var companyBranchId = $("#ddlCompanyBranch").val()

    var url = "../PhysicalStock/PhysicalStockReport?companyBranchId=" + companyBranchId + "&reportType=Excel";
    $('#btnExcel').attr('href', url);

}







