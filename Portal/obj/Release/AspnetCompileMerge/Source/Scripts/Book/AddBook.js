$(document).ready(function () {
    $("#txtGLHead").attr('disabled', true);
    BindCompanyBranchList();
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnBookId = $("#hdnBookId");

    if (hdnBookId.val() != "" && hdnBookId.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0") {
        GetBookDetail(hdnBookId.val());
     
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
     $("#ddlBookType").focus();

    $("#txtGLHead").autocomplete({ 
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "../Book/GetBookGLAutoCompleteList",
                type: "GET",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.GLHead, value: item.GLCode };
                    }))
                }
            })
        },
        focus: function (event, ui) {
            $("#txtGLHead").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtGLHead").val(ui.item.label);
            $("#txtBookName").val(ui.item.label);
            $("#hdnGLCode").val(ui.item.value); 
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#txtGLHead").val("");
                $("#hdnGLCode").val("0"); 
                ShowModel("Alert", "Please select GL Head from List") 
            }
            return false;
        }

    })
    .autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
          .append("<div><b>" + item.label + " || " + item.value + "</b></div>")
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
  

function GetBookDetail(bookId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Book/GetBookDetail",
        data: { bookId: bookId },
        dataType: "json",
        success: function (data) {
            $("#txtBookName").val(data.BookName);
            $("#txtBookCode").val(data.BookCode);
            $("#ddlBookType").val(data.BookType);
            $("#hdnGLCode").val(data.GLCode);
            $("#txtBankBranch").val(data.BankBranch);
            $("#txtBankAccountNo").val(data.BankAccountNo);
            $("#txtIFSCCode").val(data.IFSC);
            $("#txtOverDraftLimit").val(data.OverDraftLimit);

            $("#ddlCompanyBranch").val(data.CompanyBranchId);

            if (data.GLHead != "") {
                $("#txtGLHead").attr("disabled", false);
                $("#txtGLHead").val(data.GLHead);
                   }
            if (data.Book_Status == true) {
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
    var txtBookName = $("#txtBookName");
    var txtBookCode = $("#txtBookCode");
    var ddlBookType = $("#ddlBookType");
    var txtGLCode = $("#hdnGLCode");
    var txtBankBranch = $("#txtBankBranch");
    var txtBankAccountNo = $("#txtBankAccountNo");
    var txtIFSCCode = $("#txtIFSCCode");
    var txtOverDraftLimit = $("#txtOverDraftLimit"); 
    var hdnBookId = $("#hdnBookId");
    var ddlCompanyBranch = $("#ddlCompanyBranch");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;

    //if (txtGLCode.val() == "Bank" || txtGLCode.val()== "Cash") { 
    //    txtGLCodeVal = txtGLCode.val();
        
    //}
   
    if (txtBookName.val().trim() == "") {
        ShowModel("Alert", "Please Enter Book Name")
        txtBookName.focus();
        return false;
    }
    if (txtBookCode.val().trim() == "") {
        ShowModel("Alert", "Please Enter Book Code")
        txtBookCode.focus();
        return false;
    }
    if (txtBookCode.val().length != 2) {
        ShowModel("Alert", "Please Enter Book Code Maximum 2 Word")
        txtBookCode.focus();
        return false;
    }
  
    if (ddlBookType.val().trim() == 0) {
        ShowModel("Alert", "Please Select Book Type")
        ddlBookType.focus();
        return false;
    }  
    if (ddlCompanyBranch.val() == "0" || ddlCompanyBranch.val()=="") {
        ShowModel("Alert", "Please Select Company Branch")
        ddlCompanyBranch.focus();
        return false;
    }
    

    var bookViewModel = {
        BookId: hdnBookId.val(),
        BookName: txtBookName.val().trim(),
        BookCode: txtBookCode.val().trim(),
        BookType: ddlBookType.val().trim(),
        GLCode: txtGLCode.val(),
        BankBranch: txtBankBranch.val().trim(),
        BankAccountNo: txtBankAccountNo.val().trim(),
        IFSC: txtIFSCCode.val().trim(),
        OverDraftLimit: txtOverDraftLimit.val().trim(),
        CompanyBranchId:ddlCompanyBranch.val(),
        Book_Status: chkstatus,
        AdCode: ''
    };
    var accessMode = 1;//Add Mode
    if (hdnBookId.val() != null && hdnBookId.val() != 0) {
        accessMode = 2;//Edit Mode
    }
    var requestData = { bookViewModel: bookViewModel };
    $.ajax({
        url: "../Book/AddEditBook?accessMode=" + accessMode + "",
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
                           window.location.href = "../Book/ListBook";
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