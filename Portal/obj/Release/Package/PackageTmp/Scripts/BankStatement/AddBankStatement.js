$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtBankStatementDate").attr('readOnly', true);
    $("#txtBankStatementNo").attr('readOnly', true);
    $("#txtBankBranch").attr('readOnly', true);
    $("#txtBankStatementFromDate").attr('readOnly', true);
    $("#txtBankStatementToDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);

    $("#txtBankStatementDate,#txtBankStatementFromDate,#txtBankStatementToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $("#txtBankBookName").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../SO/GetBookAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term, bookType: "B" },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.BookName, value: item.BookId, branch: item.BankBranch, ifsc: item.IFSC };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtBankBookName").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtBankBookName").val(ui.item.label);
            $("#hdnBankBookId").val(ui.item.value);
            $("#txtBankBranch").val(ui.item.branch);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtBankBookName").val("");
                $("#hdnBankBookId").val(0);
                $("#txtBankBranch").val("");
                ShowModel("Alert", "Please select Bank from List")

            }
            return false;
        }

    })
    .autocomplete("instance")._renderItem = function (ul, item) {
    return $("<li>")
      .append("<div><b>" + item.label + " || " + item.branch + "</b><br>" + item.ifsc + "</div>")
      .appendTo(ul);
};

    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnBankStatementId = $("#hdnBankStatementId");
    if (hdnBankStatementId.val() != "" && hdnBankStatementId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetBankStatementDetail(hdnBankStatementId.val());
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
    else
    {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
        $(".editonly").show();
    }
    var bankStatements = [];
    GetBankStatementList(bankStatements);

 
 
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


function GetBankStatementDetail(bankStatementId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../BankStatement/GetBankStatementDetail",
        data: { bankStatementId: bankStatementId },
        dataType: "json",
        success: function (data) {
            $("#txtBankStatementNo").val(data.BankStatementNo);
        
            $("#txtBankStatementDate").val(data.BankStatementDate);
            $("#txtBankBookName").val(data.BankBookName);
            $("#hdnBankBookId").val(data.BankBookId);
            $("#txtBankBranch").val(data.BankBranch);
            BindCompanyBranchList(data.CompanyBranchId);
            //$("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtBankStatementFromDate").val(data.BankStatementFromDate);
            $("#txtBankStatementFromDate").val(data.BankStatementFromDate);
            $("#txtBankStatementToDate").val(data.BankStatementToDate);
            $("#ddlCompanyBranch").val(data.CompanyBranch)
            $("#ddlBankStatementStatus").val(data.BankStatementStatus);
            if (data.BankStatementStatus == "Final") {
                $("#btnUpdate").hide();
                $("#btnReset").hide();
                $("input").attr('readOnly', true);
                $("textarea").attr('readOnly', true);
                $("select").attr('disabled', true); 
                $(".editonly").hide();
            }
            $("#divCreated").show();
            $("#txtCreatedBy").val(data.CreatedByName);
            $("#txtCreatedDate").val(data.CreatedDate);
            if (data.ModifiedByName != "") {
                $("#divModified").show();
                $("#txtModifiedBy").val(data.ModifiedByName);
                $("#txtModifiedDate").val(data.ModifiedDate);
            }

            $("#txtRemarks").val(data.Remarks);
           
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
            if (branchId != null)
            {
                $("#ddlCompanyBranch").val(branchId);
            }
            var hdnSessionCompanyBranchId = $("#hdnSessionCompanyBranchId");
            var hdnSessionUserID = $("#hdnSessionUserID");

            if (hdnSessionCompanyBranchId.val() != "0" && hdnSessionUserID.val() != "2") {
                $("#ddlCompanyBranch").val(hdnSessionCompanyBranchId.val());
                $("#ddlCompanyBranch").attr('disabled', true);
            }

        },
        error: function (Result) {
            $("#ddlCompanyBranch").append($("<option></option>").val(0).html("-Select Location-"));
        }
    });
}

function ShowHideDocumentPanel(action) {
    if (action == 1) {
        $(".documentsection").show();
    }
    else {
        $(".documentsection").hide();
       
        $("#btnAddDocument").show();
        $("#btnUpdateDocument").hide();
    }
}

function SaveBankStatement() {
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
            
        }
        else {

            ShowModel("Alert", "Please Add The Bank Statement File !")
            return false;

        }

    } else {

        ShowModel("Alert", "FormData is not supported.")
        return "";
    }



    $.ajax({
        url: "../BankStatement/SaveBankStatementDetail",
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
            //ShowHideProductSerialPanel(2);
        }
    });
}

function GetBankStatementList(bankStatements) {
    var hdnBankStatementId = $("#hdnBankStatementId");
    var requestData = { bankStatementDetails: bankStatements, bankStatementId: hdnBankStatementId.val() };
    $.ajax({
        url: "../BankStatement/GetBankStatementDetailList",
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
            //ShowHideProductPanel(2);
        }
    });
}

function SaveData() {
    var hdnBankStatementId = $("#hdnBankStatementId");
    var txtBankBookName = $("#txtBankBookName");
    var hdnBankBookId = $("#hdnBankBookId");
    var txtBankStatementDate = $("#txtBankStatementDate");
    var txtBankBranch = $("#txtBankBranch");
    var txtBankStatementFromDate = $("#txtBankStatementFromDate");
    var txtBankStatementToDate = $("#txtBankStatementToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtRemarks = $("#txtRemarks");
    var ddlBankStatementStatus = $("#ddlBankStatementStatus");
    var fileUpload = $("#FileUpload1");
   
    if (txtBankBookName.val().trim() == "")  {
        ShowModel("Alert", "Please Select Bank Name")
        txtBankBookName.focus();
        return false;
    }

    if (hdnBankBookId.val().trim() == "" || hdnBankBookId.val().trim()=="0") {
        ShowModel("Alert", "Please Select Bank Name")
        txtBankBookName.focus();
        return false;
    }

    if (txtBankStatementFromDate.val() == "") {
        ShowModel("Alert", "Please select Bank Statement FromDate");
        txtBankStatementFromDate.focus();
        return false;

    }

    if (txtBankStatementToDate.val() == "") {
        ShowModel("Alert", "Please select Bank Statement ToDate")
        txtBankStatementToDate.focus();
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch from list")
        return false;
    }
    if (fileUpload.val() == "" || fileUpload.val() == "0") {
        ShowModel("Alert", "Please Add The Bank Statement File !")
        fileUpload.focus();
        return false;
    }
  
  var bankStatementViewModel = {
      BankStatementID: hdnBankStatementId.val(),
      BankStatementDate: txtBankStatementDate.val().trim(),
      BankBookId: hdnBankBookId.val(),
      BankBranch: txtBankBranch.val().trim(),
      BankStatementFromDate: txtBankStatementFromDate.val().trim(),
      BankStatementToDate: txtBankStatementToDate.val().trim(),
      Remarks: txtRemarks.val().trim(),
      CompanyBranchId: ddlCompanyBranch.val(),
      BankStatementStatus: ddlBankStatementStatus.val()
  };

    var bankStatementDetailList = [];
    $('#tblBankStatementDetail tr').each(function (i, row) {
        var $row = $(row);
        var transactionDate = $row.find("#hdnTransactionDate").val();
        var chequeNumber = $row.find("#hdnChequeNumber").val();
        var withdrawal = $row.find("#hdnWithdrawal").val();
        var deposit = $row.find("#hdnDeposit").val();
        var balance = $row.find("#hdnBalance").val();
        var narration = $row.find("#hdnNarration").val();
        var bankStatementDetailId = $row.find("#hdnBankStatementDetailId").val();
        if (transactionDate != undefined) {

            var bankStatement = {
                BankStatementDetailId: bankStatementDetailId,
                TransactionDate: transactionDate,
                ChequeNumber: chequeNumber,
                Withdrawal: withdrawal,
                Deposit: deposit,
                Balance: balance,
                Narration:narration
            };
            bankStatementDetailList.push(bankStatement);
        }
    });

    var accessMode = 1;//Add Mode
    if (hdnBankStatementId.val() != null && hdnBankStatementId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    if (bankStatementDetailList.length == 0)
    {
        ShowModel("Alert", "Please Add The Bank Statement File !");
        return false;
    }
    var requestData = { bankStatementViewModel: bankStatementViewModel,bankStatementDetails: bankStatementDetailList};
    $.ajax({
        url: "../BankStatement/AddEditBankStatement?accessMode=" + accessMode + "",
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
                       window.location.href = "../BankStatement/AddEditBankStatement?bankStatementID=" + data.trnId + "&AccessMode=3";
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
    $("#hdnBankStatementId").val("0");
    $("#txtBankStatementDate").val($("#hdnCurrentDate").val());
    $("#txtBankBookName").val("");
    $("#hdnBankBookId").val("0");
    $("#txtBankBranch").val("");
    $("#ddlCompanyBranch").val("0");
    $("#txtRemarks").val("");
    $("#ddlBankRecoStatus").val("0");

}
