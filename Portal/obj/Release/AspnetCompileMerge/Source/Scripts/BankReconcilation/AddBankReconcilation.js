$(document).ready(function () {
    $("#tabs").tabs({
        collapsible: true
    });
    $("#txtBankReconcilationDate").attr('readOnly', true);
    $("#txtBankReconcilationNo").attr('readOnly', true);
    $("#txtBankBranch").attr('readOnly', true);
    $("#txtBankReconcilationFromDate").attr('readOnly', true);
    $("#txtBankReconcilationToDate").attr('readOnly', true);
    $("#txtCreatedBy").attr('readOnly', true);
    $("#txtCreatedDate").attr('readOnly', true);
    
    $("#txtModifiedBy").attr('readOnly', true);
    $("#txtModifiedDate").attr('readOnly', true);
    $("#txtBookClosingBalance").attr('readOnly', true);
    $("#txtStatementClosingBalance").attr('readOnly', true);
    $("#txtBankStatementListTotal1").attr('readOnly', true);
    $("#txtBankStatementListTotal2").attr('readOnly', true);
    $("#txtBankStatementListTotal3").attr('readOnly', true);
    $("#txtBankStatementListTotal4").attr('readOnly', true);

    $("#txtBankReconcilationDate,#txtBankReconcilationFromDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {

        }
    });

    $("#txtBankReconcilationToDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-10:+100',
        onSelect: function (selected) {
            var txtBankReconcilationFromDate = $("#txtBankReconcilationFromDate").val();
            var txtBankReconcilationToDate = $("#txtBankReconcilationToDate").val(); //2013-09-10

            if (Date.parse(txtBankReconcilationToDate) < Date.parse(txtBankReconcilationFromDate)) {
                ShowModel("Alert", "To Date Should not be greater then From Date");
                $("#txtBankReconcilationToDate").val("");
                return false;
            }
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
            GetBankBalance();
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
    var hdnbankRecoID = $("#hdnbankRecoID");
    if (hdnbankRecoID.val() != "" && hdnbankRecoID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        setTimeout(
       function () {
           GetBankReconcilationDetail(hdnbankRecoID.val());
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
    //var bankStatements = [];
    //GetBankStatementList(bankStatements);

 
 
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


function GetBankReconcilationDetail(bankRecoID) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../BankReconcilation/GetBankReconcilationDetail",
        data: { bankRecoID: bankRecoID },
        dataType: "json",
        success: function (data) {
            $("#txtBankReconcilationNo").val(data.BankRecoNo);
            $("#txtBankReconcilationDate").val(data.BankRecoDate);
            $("#txtBankBookName").val(data.BankBookName);
            $("#hdnBankBookId").val(data.BankBookId);
            $("#txtBankBranch").val(data.BankBranch);
            BindCompanyBranchList(data.CompanyBranchId);
            //$("#ddlCompanyBranch").val(data.CompanyBranchId);
            $("#txtBankReconcilationFromDate").val(data.BankRecoFromDate);
            $("#txtBankReconcilationToDate").val(data.BankRecoToDate);
            $("#txtBookClosingBalance").val(data.BookClosingBalance);
            $("#ddlBookClosingBalance").val(data.BookClosingRemarks);
            $("#txtStatementClosingBalance").val(data.StatementClosingBalance);
            $("#ddlStatementClosingBalance").val(data.StatementClosingRemarks);
            $("#txtRemarks").val(data.Remarks);
            $("#ddlCompanyBranch").val(data.CompanyBranch)
            $("#ddlBankRecoStatus").val(data.BankRecoStatus);
            if (data.BankRecoStatus == "Final") {
                $("#btnUpdate").hide();
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
            $("#btnAddNew").show();
            $("#btnPrint").show();
            GetBankStatementList();
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



function GetBankStatementList() {
    $("#spnOnDate").html($("#txtBankReconcilationToDate").val());
    var hdnBankBookId = $("#hdnBankBookId");
    var txtBankBookName = $("#txtBankBookName");
    var txtBankReconcilationFromDate = $("#txtBankReconcilationFromDate");
    var txtBankReconcilationToDate = $("#txtBankReconcilationToDate");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    if (hdnBankBookId.val() == "0")
    {
        ShowModel("Alert", "Please Select Bank Name");
        txtBankBookName.focus();
        return false;
    }
    if (txtBankBookName.val() == "")
    {
        ShowModel("Alert", "Please Select Bank Name");
        txtBankBookName.focus();
        return false;
    }
    var requestData1 = { bankBookId: hdnBankBookId.val(), fromDate: txtBankReconcilationFromDate.val(), toDate: txtBankReconcilationToDate.val(), companyBranchId: ddlCompanyBranch.val(),trnType:"CINB" };
    $.ajax({
        url: "../BankReconcilation/GetBankReconcilationList1",
        cache: false,
        data: JSON.stringify(requestData1),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divBankStatementList1").html("");
            $("#divBankStatementList1").html(err);
        },
        success: function (data) {
            $("#divBankStatementList1").html("");
            $("#divBankStatementList1").html(data);
            //ShowHideProductPanel(2);
        }
    });


    var requestData2 = { bankBookId: hdnBankBookId.val(), fromDate: txtBankReconcilationFromDate.val(), toDate: txtBankReconcilationToDate.val(), companyBranchId: ddlCompanyBranch.val(), trnType: "CRNB" };
    $.ajax({
        url: "../BankReconcilation/GetBankReconcilationList2",
        cache: false,
        data: JSON.stringify(requestData2),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divBankStatementList2").html("");
            $("#divBankStatementList2").html(err);
        },
        success: function (data) {
            $("#divBankStatementList2").html("");
            $("#divBankStatementList2").html(data);
            ;
        }
    });

    var requestData3 = { bankBookId: hdnBankBookId.val(), fromDate: txtBankReconcilationFromDate.val(), toDate: txtBankReconcilationToDate.val(), companyBranchId: ddlCompanyBranch.val(), trnType: "CRNBK" };
    $.ajax({
        url: "../BankReconcilation/GetBankReconcilationList3",
        cache: false,
        data: JSON.stringify(requestData3),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divBankStatementList3").html("");
            $("#divBankStatementList3").html(err);
        },
        success: function (data) {
            $("#divBankStatementList3").html("");
            $("#divBankStatementList3").html(data);
            ;
        }
    });

    var requestData4 = { bankBookId: hdnBankBookId.val(), fromDate: txtBankReconcilationFromDate.val(), toDate: txtBankReconcilationToDate.val(), companyBranchId: ddlCompanyBranch.val(), trnType: "CDNBK" };
    $.ajax({
        url: "../BankReconcilation/GetBankReconcilationList4",
        cache: false,
        data: JSON.stringify(requestData4),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divBankStatementList4").html("");
            $("#divBankStatementList4").html(err);
        },
        success: function (data) {
            $("#divBankStatementList4").html("");
            $("#divBankStatementList4").html(data);
            ;
        }
    });

  
    
    GetBankClosingBalance();
    setInterval(function () {
        CalculateStatementList();
        CalculateSummary();
    }, 2000);
    
}

function CalculateStatementList()
{
    var totalValue1 = 0;
    var totalValue2 = 0;
    var totalValue3 = 0;
    var totalValue4 = 0;

    $('#tblBankReconcilationList1 tr').each(function (i, row) {
        var $row = $(row);
        var hdnNarration = $row.find("#hdnNarration").val();
        if (hdnNarration != undefined) {
            var amountList1 = $row.find("#hdnAmount").val();
            if (amountList1 != undefined) {
                totalValue1 = parseFloat(totalValue1) + parseFloat(amountList1);
             }
        }
    });

    txtBankStatementListTotal1 = $("#txtBankStatementListTotal1");
    txtBankStatementListTotal1.val(parseFloat(totalValue1).toFixed(2));
    if (totalValue1 > 0) {
        $(".totalSeciotn1").show();
        $("#spnCheqeusIssuedTotal").html(parseFloat(totalValue1).toFixed(2));
        $("#hdnCheqeusIssuedTotal").val(parseFloat(totalValue1).toFixed(2));
    }
    else {
        $(".totalSeciotn1").hide();
    }
    $('#tblBankReconcilationList2 tr').each(function (i, row) {
        var $row = $(row);
        var hdnNarration = $row.find("#hdnNarration").val();
        if (hdnNarration != undefined) {
            var amountList2 = $row.find("#hdnAmount").val();
            if (amountList2 != undefined) {
                totalValue2 = parseFloat(totalValue2) + parseFloat(amountList2);
            }
        }
    });
    txtBankStatementListTotal2 = $("#txtBankStatementListTotal2");
    txtBankStatementListTotal2.val(parseFloat(totalValue2).toFixed(2));
     if (totalValue2 > 0) {
         $(".totalSeciotn2").show();
         $("#spnCheqeusReceivedTotal").html(parseFloat(totalValue2).toFixed(2));
         $("#hdnCheqeusReceivedTotal").val(parseFloat(totalValue2).toFixed(2));
     } else {
         $(".totalSeciotn2").hide();
     }
    $('#tblBankReconcilationList3 tr').each(function (i, row) {
        var $row = $(row);
        var hdnNarration = $row.find("#hdnNarration").val();
        if (hdnNarration != undefined) {
            var amountList3 = $row.find("#hdnAmount").val();
            if (amountList3 != undefined) {
                totalValue3 = parseFloat(totalValue3) + parseFloat(amountList3);
            }
        }
    });

    txtBankStatementListTotal3 = $("#txtBankStatementListTotal3");
    txtBankStatementListTotal3.val(parseFloat(totalValue3).toFixed(2));
    if (totalValue3 > 0) {
        $(".totalSeciotn3").show();
        $("#spnCheqeusReceivedBankTotal").html(parseFloat(totalValue3).toFixed(2));
        $("#hdnCheqeusReceivedBankTotal").val(parseFloat(totalValue3).toFixed(2));
    }
    else {
        $(".totalSeciotn3").hide();
    }
    $('#tblBankReconcilationList4 tr').each(function (i, row) {
        var $row = $(row);
        var hdnNarration = $row.find("#hdnNarration").val();
        if (hdnNarration != undefined) {
            var amountList4 = $row.find("#hdnAmount").val();
            if (amountList4 != undefined) {
                totalValue4 = parseFloat(totalValue4) + parseFloat(amountList4);
            }
        }
    });

    txtBankStatementListTotal4 = $("#txtBankStatementListTotal4");
    txtBankStatementListTotal4.val(parseFloat(totalValue4).toFixed(2));
   
  if (totalValue4 > 0) {
      $(".totalSeciotn4").show();
      $("#spnCheqeusDebitedBankTotal").html(parseFloat(totalValue4).toFixed(2));
      $("#hdnCheqeusDebitedBankTotal").val(parseFloat(totalValue4).toFixed(2));
  }
  else {
      $(".totalSeciotn4").hide();
  }

  

  
}

function  CalculateSummary()
{
    var totalSummary = 0;
    var txtBookClosingBalance = $("#txtBookClosingBalance").val();
    var hdnCheqeusIssuedTotal = $("#hdnCheqeusIssuedTotal").val();
    var hdnCheqeusReceivedTotal = $("#hdnCheqeusReceivedTotal").val();
    var hdnCheqeusReceivedBankTotal = $("#hdnCheqeusReceivedBankTotal").val();
    var hdnCheqeusDebitedBankTotal = $("#hdnCheqeusDebitedBankTotal").val();
    totalSummary = parseFloat(txtBookClosingBalance) + parseFloat(hdnCheqeusIssuedTotal);
    totalSummary = parseFloat(totalSummary) - parseFloat(hdnCheqeusReceivedTotal);
    totalSummary = parseFloat(totalSummary) + parseFloat(hdnCheqeusReceivedBankTotal);
    totalSummary = parseFloat(totalSummary) - parseFloat(hdnCheqeusDebitedBankTotal);
    $("#spnSummaryTotal").html(parseFloat(totalSummary).toFixed(2))
    $("#hdnSummaryTotal").val(parseFloat(totalSummary).toFixed(2))
}
function SaveData() {
    var hdnbankRecoID = $("#hdnbankRecoID");
    var txtBankReconcilationDate = $("#txtBankReconcilationDate");
    var txtBankBookName = $("#txtBankBookName");
    var hdnBankBookId = $("#hdnBankBookId");
    var txtBankReconcilationFromDate = $("#txtBankReconcilationFromDate");
    var txtBankReconcilationToDate = $("#txtBankReconcilationToDate");
    var txtBookClosingBalance = $("#txtBookClosingBalance");
    var ddlBookClosingBalance = $("#ddlBookClosingBalance");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var txtRemarks = $("#txtRemarks");
    var txtStatementClosingBalance = $("#txtStatementClosingBalance");
    var ddlStatementClosingBalance = $("#ddlStatementClosingBalance");
    var ddlBankRecoStatus = $("#ddlBankRecoStatus");
   
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

    if (txtBankReconcilationFromDate.val() == "") {
        ShowModel("Alert", "Please select Bank Reconcilation FromDate");
        txtBankReconcilationFromDate.focus();
        return false;

    }

    if (txtBankReconcilationToDate.val() == "") {
        ShowModel("Alert", "Please select Bank Reconcilation ToDate")
        txtBankReconcilationToDate.focus();
        return false;
    }

    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }
    
  var bankRecoViewModel = {
      BankRecoID: hdnbankRecoID.val(),
      BankRecoDate: txtBankReconcilationDate.val(),
      BankBookId: hdnBankBookId.val(),
      BankRecoFromDate: txtBankReconcilationFromDate.val(),
      BankRecoToDate: txtBankReconcilationToDate.val(),
      BookClosingBalance:txtBookClosingBalance.val(),
      BookClosingRemarks:ddlBookClosingBalance.val(),
      StatementClosingBalance:txtStatementClosingBalance.val(),
      StatementClosingRemarks: ddlStatementClosingBalance.val(),
      Remarks: txtRemarks.val(),
      CompanyBranchId: ddlCompanyBranch.val(),
      BankRecoStatus: ddlBankRecoStatus.val()
  };
  hdnCheqeusIssuedTotal= $("#hdnCheqeusIssuedTotal");
  hdnCheqeusReceivedTotal= $("#hdnCheqeusReceivedTotal");
  hdnCheqeusReceivedBankTotal=$("#hdnCheqeusReceivedBankTotal");
  hdnCheqeusDebitedBankTotal = $("#hdnCheqeusDebitedBankTotal");
  hdnSummaryTotal = $("#hdnSummaryTotal");
  var bankRecoSummaryViewModel = {
      BookId:hdnBankBookId.val(),
      AsOnDate:txtBankReconcilationToDate.val(),
      BookClosingBalance:txtBookClosingBalance.val(),
      CheqeusIssuedButNotPresentedInBankAmt:hdnCheqeusIssuedTotal.val(),
      ChequesReceivedButNotInBankAmt:hdnCheqeusReceivedTotal.val(),
      ChequeReceivedInBankButNotInBooksAmt:hdnCheqeusReceivedBankTotal.val(),
      ChequeDebitedPaidByBankButNotInBookAmt:hdnCheqeusDebitedBankTotal.val(),
      BankStatementClosingBalanceAmt:txtStatementClosingBalance.val(),
      ClosingBalAsPerBankStatementAmt: hdnSummaryTotal.val(),
      ClosingBalOfBankAsPerBankReco:hdnSummaryTotal.val()
  };

  var bankRecoDetailList = [];
    
  $('#tblBankReconcilationList1 tr').each(function (i, row) {
      var $row = $(row);
      var bankRecoDetailId = $row.find("#hdnBankRecoDetailId").val();
      var narration = $row.find("#hdnNarration").val();
      var refNo = $row.find("#hdnRefNo").val();
      var refDate = $row.find("#hdnRefDate").val();
      var chequeRefNo = $row.find("#hdnChequeRefNo").val();
      var amount = $row.find("#hdnAmount").val();
      var trnType = $row.find("#hdnTrnType").val();
      if (narration != undefined) {
          var bankRecoDetail = {
              BankRecoDetailId: bankRecoDetailId,
              ChequeNumber: chequeRefNo,
              BankRecoNarration: narration,
              RefNo: refNo,
              RefDate: refDate,
              Amount: amount,
              TrnType: trnType
          };
          bankRecoDetailList.push(bankRecoDetail);
      }

  });

  $('#tblBankReconcilationList2 tr').each(function (i, row) {
      var $row = $(row);
      var bankRecoDetailId = $row.find("#hdnBankRecoDetailId").val();
      var narration = $row.find("#hdnNarration").val();
      var refNo = $row.find("#hdnRefNo").val();
      var refDate = $row.find("#hdnRefDate").val();
      var chequeRefNo = $row.find("#hdnChequeRefNo").val();
      var amount = $row.find("#hdnAmount").val();
      var trnType = $row.find("#hdnTrnType").val();
      if (narration != undefined) {
          var bankRecoDetail = {
              BankRecoDetailId: bankRecoDetailId,
              ChequeNumber: chequeRefNo,
              BankRecoNarration: narration,
              RefNo: refNo,
              RefDate: refDate,
              Amount: amount,
              TrnType: trnType
          };
          bankRecoDetailList.push(bankRecoDetail);
      }

  });

  $('#tblBankReconcilationList3 tr').each(function (i, row) {
      var $row = $(row);
      var bankRecoDetailId = $row.find("#hdnBankRecoDetailId").val();
      var narration = $row.find("#hdnNarration").val();
      var refNo = $row.find("#hdnRefNo").val();
      var refDate = $row.find("#hdnRefDate").val();
      var chequeRefNo = $row.find("#hdnChequeRefNo").val();
      var amount = $row.find("#hdnAmount").val();
      var trnType = $row.find("#hdnTrnType").val();
      if (narration != undefined) {
          var bankRecoDetail = {
              BankRecoDetailId: bankRecoDetailId,
              ChequeNumber: chequeRefNo,
              BankRecoNarration: narration,
              RefNo: refNo,
              RefDate: refDate,
              Amount: amount,
              TrnType: trnType
          };
          bankRecoDetailList.push(bankRecoDetail);
      }

  });

  $('#tblBankReconcilationList4 tr').each(function (i, row) {
      var $row = $(row);
      var bankRecoDetailId = $row.find("#hdnBankRecoDetailId").val();
      var narration = $row.find("#hdnNarration").val();
      var refNo = $row.find("#hdnRefNo").val();
      var refDate = $row.find("#hdnRefDate").val();
      var chequeRefNo = $row.find("#hdnChequeRefNo").val();
      var amount = $row.find("#hdnAmount").val();
      var trnType = $row.find("#hdnTrnType").val();
      if (narration!= undefined)
      {
          var bankRecoDetail = {
              BankRecoDetailId:bankRecoDetailId,
              ChequeNumber:chequeRefNo,
              BankRecoNarration:narration,
              RefNo:refNo,
              RefDate:refDate,
              Amount:amount,
              TrnType: trnType
          };
          bankRecoDetailList.push(bankRecoDetail);
      }

  });

 
  var accessMode = 1;//Add Mode
  if (hdnbankRecoID.val() != null && hdnbankRecoID.val() != 0) {
      accessMode = 2;//Edit Mode
  }

  var requestData = { bankReconcilation: bankRecoViewModel, bankReconcilationDetails: bankRecoDetailList, bankReconcilationSummary: bankRecoSummaryViewModel };
    $.ajax({
        url: "../BankReconcilation/AddEditBankReconcilation?accessMode=" + accessMode + "",
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
                       window.location.href = "../BankReconcilation/AddEditBankReconcilation?bankRecoID=" + data.trnId + "&AccessMode=2";
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
    $("#hdnbankRecoID").val("0");
    $("#txtBankReconcilationDate").val($("#hdnCurrentDate").val());
    $("#txtBankBookName").val("");
    $("#hdnBankBookId").val("0");
    $("#txtBankReconcilationFromDate").val($("#hdnBankReconcilationFromDate").val());
    $("#txtBankReconcilationToDate").val($("#hdnBankReconcilationToDate").val());
    $("#txtBookClosingBalance").val("");
    $("#ddlBookClosingBalance").val("0");
    $("#ddlCompanyBranch").val("");
    $("#txtRemarks").val("");;
    $("#txtStatementClosingBalance").val("");;
    $("#ddlStatementClosingBalance").val("");;
    $("#ddlBankRecoStatus").val("0");;

}


function GetBankBalance() {
    $("#txtBookClosingBalance").val(0);
    var voucherId =0;
    var bookId = $("#hdnBankBookId").val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Voucher/GetBookBalance",
        data: { voucherId: voucherId, bookId: bookId },
        dataType: "json",
        success: function (data) {
            $("#txtBookClosingBalance").val(parseFloat(data).toFixed(2));
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
            $("#txtBookClosingBalance").val(0);
        }
    });
}

function GetBankClosingBalance() {
    $("#txtStatementClosingBalance").val(0);
    var bookId = $("#hdnBankBookId").val();
    var bankReconcilationFromDate = $("#txtBankReconcilationFromDate").val();
    var bankReconcilationToDate = $("#txtBankReconcilationToDate").val();
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../BankReconcilation/GetBankClosingBalance",
        data: { bookId: bookId, fromDate: bankReconcilationFromDate, ToDate: bankReconcilationToDate },
        dataType: "json",
        success: function (data) {
            $("#txtStatementClosingBalance").val(parseFloat(data).toFixed(2));
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
            $("#txtStatementClosingBalance").val(0);
        }
    });


}