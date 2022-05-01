
$(document).ready(function () {
    $("#txtStartDate").attr('readonly', true);
    $("#txtEndDate").attr('readonly', true);
    $("#txtStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        yearRange: '-100:+100',
        onSelect: function (selected) {
            var d = selected.split('-')[0];
            var m = MonthValue(selected.split('-')[1]);
            var y = selected.split('-')[2];

            var dt = new Date(parseInt(y) + 1, parseInt(m) - 1, parseInt(d));
            dt.setDate(dt.getDate() - 1);
            var yy = dt.getFullYear();
            var mm = dt.getMonth() + 1;
            var dd = dt.getDate();
            var toDate = new Date(yy, dt.getMonth(), dd);
            var someFormattedDate = padLeft(dd) + '-' + ValueToMonth(mm) + '-' + yy;
            $('#txtEndDate').datepicker('option', 'minDate', dt);
            $('#txtEndDate').datepicker('option', 'maxDate', dt);
            $('#txtEndDate').val(someFormattedDate);
            
        }
    });


    $("#txtEndDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        onSelect: function (selected) {
        }
    });
    var hdnAccessMode = $("#hdnAccessMode");
    var hdnFinYearID = $("#hdnFinYearID");
    if (hdnFinYearID.val() != "" && hdnFinYearID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        GetFinYearDetail(hdnFinYearID.val());
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("select").attr('disabled', true);
        }
        else
        {
            $("#txtStartDate").attr('disabled', true);
            $("#txtEndDate").attr('disabled', true);
            $("#btnSave").hide();
            $("#btnUpdate").show();
            $("#btnReset").hide();
        }
    }
    else
    {
        $("#btnSave").show();
        $("#btnUpdate").hide();
        $("#btnReset").show();
    }
    $("#txtStartDate").focus();
        


});
function padLeft(str) {
    if (parseInt(str) < 10) {
        return '0' + str;
    }
    else { return str; }
}
function MonthValue(str) {
    switch (str) {
        case "Jan": return 1;
        case "Feb": return 2;
        case "Mar": return 3;
        case "Apr": return 4;
        case "May": return 5;
        case "Jun": return 6;
        case "Jul": return 7;
        case "Aug": return 8;
        case "Sep": return 9;
        case "Oct": return 10;
        case "Nov": return 11;
        case "Dec": return 12;
        default: return 1;

    }

}
function ValueToMonth(value) {
    switch (value) {
        case 1: return "Jan";
        case 2: return "Feb";
        case 3: return "Mar";
        case 4: return "Apr";
        case 5: return "May";
        case 6: return "Jun";
        case 7: return "Jul";
        case 8: return "Aug";
        case 9: return "Sep";
        case 10: return "Oct";
        case 11: return "Nov";
        case 12: return "Dec";
        default: return "Jan";

    }

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
function SaveData()
{
    var txtStartDate = $("#txtStartDate");
    var hdnFinYearID = $("#hdnFinYearID");
    var txtEndDate = $("#txtEndDate");
    var chkStatus = $("#chkStatus");


    if (txtStartDate.val().trim() == "")
    {
        ShowModel("Alert","Please select Start Period")
        txtStartDate.focus();
        return false;
    }
    if (txtEndDate.val().trim() == "") {
        ShowModel("Alert", "Please select End Period")
        txtEndDate.focus();
        return false;
    }
    var finYearStatus = true;
    if (chkStatus.prop("checked") == true)
    { finYearStatus = true; }
    else
    { finYearStatus = false; }
        
    var finYearViewModel = {
        FinYearId: hdnFinYearID.val(), StartDate: txtStartDate.val().trim(), EndDate: txtEndDate.val().trim(), FinYearStatus: finYearStatus
    };

    var accessMode = 1;//Add Mode
    if (hdnFinYearID.val() != null && hdnFinYearID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { finYearViewModel: finYearViewModel };
    $.ajax({
        url: "../FinYear/AddEditFinYear?AccessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status=="SUCCESS")
            {
                ShowModel("Alert", data.message);
                ClearFields();
                setTimeout(
             function () {
                 window.location.href = "../FinYear/ListFinYear";
             }, 2000);
                $("#btnSave").show();
                $("#btnUpdate").hide();
            }
                          else
            {
                ShowModel("Error", data.message)
            }
        },
        error: function (err) {
            ShowModel("Error", err)
        }
    });

}
function ShowModel(headerText,bodyText)
{
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields()
{
    $("#txtStartDate").val("");
    $("#txtEndDate").val("");
    $("#hdnFinYearID").val("0");
     $("#hdnAccessMode").val("0");
     $("#chkStatus").attr("checked", true);
    
}


function GetFinYearDetail(finYearId) {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../FinYear/GetFinYearDetail",
        data: { finYearId: finYearId },
        dataType: "json",
        success: function (data) {
            $("#hdnFinYearID").val(data.hdnFinYearID)
            $("#txtStartDate").val(data.StartDate);
            $("#txtEndDate").val(data.EndDate);

            if (data.FinYearStatus == true) {
                $("#chkStatus").attr("checked", true);
            }
            else {
                $("#chkStatus ").attr("checked", false);
            }
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });
}