$(document).ready(function () {
    BindCompanyBranchList();
    $("#txtRoasterStartDate").attr('readOnly', true);

    BindDepartmentList();
    BindShiftList();
    var hdnAccessMode = $("#hdnAccessMode"); 
    var hdnRoasterID = $("#hdnRoasterID");
    if (hdnRoasterID.val() != "" && hdnRoasterID.val() != "0" && hdnAccessMode.val() != "" && hdnAccessMode.val() != "0")
    {
        setTimeout(
        function () {
            GetRoasterDetail(hdnRoasterID.val());
        }, 1000);

     
        if (hdnAccessMode.val() == "3")
        {
            $("#btnSave").hide();
            $("#btnUpdate").hide();
            $("#btnReset").hide();
            $("input").attr('readOnly', true);
            $("textarea").attr('readOnly', true);
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
    $("#txtRoasterName").focus();
    $("#txtRoasterStartDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd-M-yy',
        minDate: '0D',
        onSelect: function (selected) {
        }
    });
    var weeks = [];
    GetRoasterWeekList(weeks);

});



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
function BindDepartmentList() {
    $.ajax({
        type: "GET",
        url: "../Employee/GetDepartmentList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $("#ddlDepartment").append($("<option></option>").val(0).html("-Select Department-"));
            $.each(data, function (i, item) {

                $("#ddlDepartment").append($("<option></option>").val(item.DepartmentId).html(item.DepartmentName));
            });
        },
        error: function (Result) {
            $("#ddlDepartment").append($("<option></option>").val(0).html("-Select Department-"));
        }
    });
}
function BindShiftList() {
    $.ajax({
        type: "GET",
        url: "../Roaster/GetShiftList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $(".shiftDDl").append($("<option></option>").val(0).html("-Select Shift-"));
            $.each(data, function (i, item) {

                $(".shiftDDl").append($("<option></option>").val(item.ShiftId).html(item.ShiftName));
            });
        },
        error: function (Result) {
            $(".shiftDDl").append($("<option></option>").val(0).html("-Select Shift-"));
        }
    });
}
function GetRoasterDetail(roasterId) {
    $.ajax({
        type: "GET",
        asnc:false,
        url: "../Roaster/GetRoasterDetail",
        data: { roasterId: roasterId },
        dataType: "json",
        success: function (data) {
            $("#txtRoasterName").val(data.RoasterName);
            $("#txtRoasterDesc").val(data.RoasterDesc);
            $("#txtRoasterStartDate").val(data.RoasterStartDate);
            $("#txtRoasterType").val(data.RoasterType);
            $("#txtRoasterRemark").val(data.Remarks);
            $("#ddlDepartment").val(data.DepartmentId);
            $("#txtNoOfWeek").val(data.NoOfWeeks);
            $("#CompanyBranchId").val(data.CompanyBranchId);

            if (data.RoasterStatus == true) {
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

function SaveData()
{
    var txtRoasterName = $("#txtRoasterName");
    var hdnRoasterID = $("#hdnRoasterID");
    var txtRoasterDesc = $("#txtRoasterDesc");
    var txtRoasterStartDate = $("#txtRoasterStartDate");
    var txtRoasterType = $("#txtRoasterType");
    var txtRoasterRemark = $("#txtRoasterRemark");
    var ddlDepartment = $("#ddlDepartment");
    var txtNoOfWeek = $("#txtNoOfWeek");
    var chkstatus = $("#chkstatus").is(':checked') ? true : false;
    var ddlCompanyBranch = $("#ddlCompanyBranch");

    if (txtRoasterName.val().trim() == "")
    {
        ShowModel("Alert", "Please Enter Roaster Name")
        txtRoasterName.focus();
        return false;
    }
    if (txtRoasterStartDate.val().trim() == "") {
        ShowModel("Alert", "Please Enter Start Date")
        txtRoasterStartDate.focus();
        return false;
    }
    if (txtRoasterType.val().trim() == "") {
        ShowModel("Alert", "Please Enter Roaster Type")
        txtRoasterType.focus();
        return false;
    } 
    if (ddlDepartment.val().trim() == "" || ddlDepartment.val().trim() == "0") {
        ShowModel("Alert", "Please Select Department")
        ddlDepartment.focus();
        return false;
    }
    if (txtNoOfWeek.val() == "" || txtNoOfWeek.val() == "0") {
        ShowModel("Alert", "Please Select No Of Week")
        txtNoOfWeek.focus();
        return false;
    }
    if (ddlCompanyBranch.val() == "" || ddlCompanyBranch.val() == "0") {
        ShowModel("Alert", "Please select Company Branch")
        return false;
    }
    var roasterViewModel = {
        RoasterId: hdnRoasterID.val(),
        RoasterName: txtRoasterName.val().trim(),
        RoasterDesc: txtRoasterDesc.val().trim(),
        RoasterStartDate : txtRoasterStartDate.val(),
        RoasterType: txtRoasterType.val().trim(),
        Remarks: txtRoasterRemark.val().trim(),
        DepartmentId: ddlDepartment.val(),
        NoOfWeeks: txtNoOfWeek.val(), 
        RoasterStatus: chkstatus,
        CompanyBranchId: ddlCompanyBranch.val()
        
    };
    var weekList = [];
    var iWeekCount = 0;
    $('#tblWeekList tr').each(function (i, row) {
        var $row = $(row);
        var weekSequenceNo = $row.find("#hdnWeekSequenceNo").val();
        var roasterWeekId = $row.find("#hdnRoasterWeekId").val();
        var weekNo = $row.find("#hdnWeekNo").val();
        var mon_ShiftId = $row.find("#hdnMon_ShiftId").val();
        var tue_ShiftId = $row.find("#hdnTue_ShiftId").val();
        var wed_ShiftId = $row.find("#hdnWed_ShiftId").val();
        var thu_ShiftId = $row.find("#hdnThu_ShiftId").val();
        var fri_ShiftId = $row.find("#hdnFri_ShiftId").val();
        var sat_ShiftId = $row.find("#hdnSat_ShiftId").val();
        var sun_ShiftId = $row.find("#hdnSun_ShiftId").val();
        if (weekNo != undefined) {
            var week = {
                RoasterWeekId: roasterWeekId,
                WeekSequenceNo: weekSequenceNo,
                WeekNo: weekNo,
                Mon_ShiftId: mon_ShiftId,
                Tue_ShiftId: tue_ShiftId,
                Wed_ShiftId: wed_ShiftId,
                Thu_ShiftId: thu_ShiftId,
                Fri_ShiftId: fri_ShiftId,
                Sat_ShiftId: sat_ShiftId,
                Sun_ShiftId: sun_ShiftId
                };
            weekList.push(week);
            iWeekCount = parseInt(iWeekCount) + 1;
        }

    });

    if (parseInt(txtNoOfWeek.val()) != parseInt(iWeekCount))
    {
        ShowModel("Alert", "Please Select Shift for " + txtNoOfWeek.val() + " week(s)");
        return false;

    }
    var accessMode = 1;//Add Mode
    if (hdnRoasterID.val() != null && hdnRoasterID.val() != 0) {
        accessMode = 2;//Edit Mode
    }

    var requestData = { roasterViewModel: roasterViewModel, weeks: weekList };
    $.ajax({
        url: "../Roaster/AddEditRoaster?accessMode=" + accessMode + "",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify( requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                ShowModel("Alert", data.message);
                ClearFields();
                if (accessMode == 2) {
                    setTimeout(
                  function () {
                      window.location.href = "../Roaster/ListRoaster";
                  }, 2000);
                }
                else {
                    setTimeout(
                    function () {
                        window.location.href = "../Roaster/AddEditRoaster";
                    }, 2000);
                }
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
function ShowModel(headerText,bodyText)
{
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}
function ClearFields()
{ 
    $("#txtRoasterName").val("");
    $("#hdnRoasterID").val("0");
    $("#txtRoasterDesc").val("");
    $("#txtRoasterStartDate").val("");
    $("#txtRoasterType").val("0");
    $("#txtRoasterRemark").val("");
    $("#ddlDepartment").val("0");
    $("#txtNoOfWeek").val("0");
    
    $("#chkstatus").prop("checked", true);
    $("#divWeekList").html("");
}
function GetRoasterWeekList(weeks) {
    var hdnRoasterID = $("#hdnRoasterID");
    var requestData = { weeks: weeks, roasterId: hdnRoasterID.val() };
    $.ajax({
        url: "../Roaster/GetRoasterWeekList",
        cache: false,
        data: JSON.stringify(requestData),
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        error: function (err) {
            $("#divWeekList").html("");
            $("#divWeekList").html(err);
        },
        success: function (data) {
            $("#divWeekList").html("");
            $("#divWeekList").html(data);

            ShowHideWeekPanel(2);
        }
    });
}

function AddWeek(action) {
    var weekEntrySequence = 0;
    var hdnWeekSequenceNo = $("#hdnWeekSequenceNo");
    var hdnRoasterWeekId = $("#hdnRoasterWeekId");
    var ddlWeekNo = $("#ddlWeekNo");
    var ddlShift_Mon = $("#ddlShift_Mon");
    var ddlShift_Tue = $("#ddlShift_Tue");
    var ddlShift_Wed = $("#ddlShift_Wed");
    var ddlShift_Thu = $("#ddlShift_Thu");
    var ddlShift_Fri = $("#ddlShift_Fri");
    var ddlShift_Sat = $("#ddlShift_Sat");
    var ddlShift_Sun = $("#ddlShift_Sun");

    if (ddlWeekNo.val() == "" || ddlWeekNo.val() == "0") {
        ShowModel("Alert", "Please select Week No.")
        return false;
    }

    if (ddlShift_Mon.val() == "" || ddlShift_Mon.val() == "0") {
        ShowModel("Alert", "Please select Monday Shift")
        return false;
    }
    if (ddlShift_Tue.val() == "" || ddlShift_Tue.val() == "0") {
        ShowModel("Alert", "Please select Tuesday Shift")
        return false;
    }
    if (ddlShift_Wed.val() == "" || ddlShift_Wed.val() == "0") {
        ShowModel("Alert", "Please select Wednesday Shift")
        return false;
    }
    if (ddlShift_Thu.val() == "" || ddlShift_Thu.val() == "0") {
        ShowModel("Alert", "Please select Thursday Shift")
        return false;
    }
    if (ddlShift_Fri.val() == "" || ddlShift_Fri.val() == "0") {
        ShowModel("Alert", "Please select Friday Shift")
        return false;
    }
    if (ddlShift_Sat.val() == "" || ddlShift_Sat.val() == "0") {
        ShowModel("Alert", "Please select Saturday Shift")
        return false;
    }
    if (ddlShift_Sun.val() == "" || ddlShift_Sun.val() == "0") {
        ShowModel("Alert", "Please select Sunday Shift")
        return false;
    }

    var weekList = [];
    if (action == 1 && (hdnWeekSequenceNo.val() == "" || hdnWeekSequenceNo.val() == "0")) {
        weekEntrySequence = 1;
    }
    $('#tblWeekList tr').each(function (i, row) {
        var $row = $(row);
        var weekSequenceNo = $row.find("#hdnWeekSequenceNo").val();
        var roasterWeekId = $row.find("#hdnRoasterWeekId").val();
        var weekNo = $row.find("#hdnWeekNo").val();
        var mon_ShiftName = $row.find("#hdnMon_ShiftName").val();
        var mon_ShiftId = $row.find("#hdnMon_ShiftId").val();
        var tue_ShiftName = $row.find("#hdnTue_ShiftName").val();
        var tue_ShiftId = $row.find("#hdnTue_ShiftId").val();
        var wed_ShiftName = $row.find("#hdnWed_ShiftName").val();
        var wed_ShiftId = $row.find("#hdnWed_ShiftId").val();
        var thu_ShiftName = $row.find("#hdnThu_ShiftName").val();
        var thu_ShiftId = $row.find("#hdnThu_ShiftId").val();
        var fri_ShiftName = $row.find("#hdnFri_ShiftName").val();
        var fri_ShiftId = $row.find("#hdnFri_ShiftId").val();
        var sat_ShiftName = $row.find("#hdnSat_ShiftName").val();
        var sat_ShiftId = $row.find("#hdnSat_ShiftId").val();
        var sun_ShiftName = $row.find("#hdnSun_ShiftName").val();
        var sun_ShiftId = $row.find("#hdnSun_ShiftId").val();


        if (weekNo != undefined) {
            if (action == 1 || (hdnWeekSequenceNo.val() != weekSequenceNo)) {

                if (weekNo == ddlWeekNo.val()) {
                    ShowModel("Alert", "This Week no. roaster already added!!!")
                    return false;
                }
                var week = {
                    RoasterWeekId: roasterWeekId,
                    WeekSequenceNo: weekSequenceNo,
                    WeekNo: weekNo,
                    Mon_ShiftId: mon_ShiftId,
                    Mon_ShiftName: mon_ShiftName,
                    Tue_ShiftId: tue_ShiftId,
                    Tue_ShiftName: tue_ShiftName,
                    Wed_ShiftId: wed_ShiftId,
                    Wed_ShiftName: wed_ShiftName,
                    Thu_ShiftId: thu_ShiftId,
                    Thu_ShiftName: thu_ShiftName,
                    Fri_ShiftId: fri_ShiftId,
                    Fri_ShiftName: fri_ShiftName,
                    Sat_ShiftId: sat_ShiftId,
                    Sat_ShiftName: sat_ShiftName,
                    Sun_ShiftId: sun_ShiftId,
                    Sun_ShiftName: sun_ShiftName
                };
                weekList.push(week);
                weekEntrySequence = parseInt(weekEntrySequence) + 1;
            }
            else if (hdnWeekSequenceNo.val() == weekSequenceNo) {
                var weekAddEdit = {
                    RoasterWeekId: hdnRoasterWeekId.val(),
                    WeekSequenceNo: hdnWeekSequenceNo.val(),
                    WeekNo: ddlWeekNo.val(),
                    Mon_ShiftId: ddlShift_Mon.val(),
                    Mon_ShiftName: $("#ddlShift_Mon option:selected").text(),
                    Tue_ShiftId: ddlShift_Tue.val(),
                    Tue_ShiftName: $("#ddlShift_Tue option:selected").text(),
                    Wed_ShiftId: ddlShift_Wed.val(),
                    Wed_ShiftName: $("#ddlShift_Wed option:selected").text(),
                    Thu_ShiftId: ddlShift_Thu.val(),
                    Thu_ShiftName: $("#ddlShift_Thu option:selected").text(),
                    Fri_ShiftId: ddlShift_Fri.val(),
                    Fri_ShiftName: $("#ddlShift_Fri option:selected").text(),
                    Sat_ShiftId: ddlShift_Sat.val(),
                    Sat_ShiftName: $("#ddlShift_Sat option:selected").text(),
                    Sun_ShiftId: ddlShift_Sun.val(),
                    Sun_ShiftName: $("#ddlShift_Sun option:selected").text()
                };
                weekList.push(weekAddEdit);
                hdnWeekSequenceNo.val("0");
            }
        }
    });
    if (action == 1 && (hdnWeekSequenceNo.val() == "" || hdnWeekSequenceNo.val() == "0")) {
        hdnWeekSequenceNo.val(weekEntrySequence);
    }
    if (action == 1) {
        var weekAddEdit = {
            RoasterWeekId: hdnRoasterWeekId.val(),
            WeekSequenceNo: hdnWeekSequenceNo.val(),
            WeekNo: ddlWeekNo.val(),
            Mon_ShiftId: ddlShift_Mon.val(),
            Mon_ShiftName: $("#ddlShift_Mon option:selected").text(),
            Tue_ShiftId: ddlShift_Tue.val(),
            Tue_ShiftName: $("#ddlShift_Tue option:selected").text(),
            Wed_ShiftId: ddlShift_Wed.val(),
            Wed_ShiftName: $("#ddlShift_Wed option:selected").text(),
            Thu_ShiftId: ddlShift_Thu.val(),
            Thu_ShiftName: $("#ddlShift_Thu option:selected").text(),
            Fri_ShiftId: ddlShift_Fri.val(),
            Fri_ShiftName: $("#ddlShift_Fri option:selected").text(),
            Sat_ShiftId: ddlShift_Sat.val(),
            Sat_ShiftName: $("#ddlShift_Sat option:selected").text(),
            Sun_ShiftId: ddlShift_Sun.val(),
            Sun_ShiftName: $("#ddlShift_Sun option:selected").text()
        };
        weekList.push(weekAddEdit);
        hdnWeekSequenceNo.val("0");
    }
    GetRoasterWeekList(weekList);
}

function EditWeekRow(obj) {
    var row = $(obj).closest("tr");
    var weekSequenceNo = $(row).find("#hdnWeekSequenceNo").val();
    var roasterWeekId = $(row).find("#hdnRoasterWeekId").val();
    var weekNo = $(row).find("#hdnWeekNo").val();
    var mon_ShiftId = $(row).find("#hdnMon_ShiftId").val();
    var tue_ShiftId = $(row).find("#hdnTue_ShiftId").val();
    var wed_ShiftId = $(row).find("#hdnWed_ShiftId").val();
    var thu_ShiftId = $(row).find("#hdnThu_ShiftId").val();
    var fri_ShiftId = $(row).find("#hdnFri_ShiftId").val();
    var sat_ShiftId = $(row).find("#hdnSat_ShiftId").val();
    var sun_ShiftId = $(row).find("#hdnSun_ShiftId").val();

    $("#hdnWeekSequenceNo").val(weekSequenceNo);
    $("#hdnRoasterWeekId").val(roasterWeekId);
    $("#ddlWeekNo").val(weekNo);
    $("#ddlShift_Mon").val(mon_ShiftId);
    $("#ddlShift_Tue").val(tue_ShiftId);
    $("#ddlShift_Wed").val(wed_ShiftId);
    $("#ddlShift_Thu").val(thu_ShiftId);
    $("#ddlShift_Fri").val(fri_ShiftId);
    $("#ddlShift_Sat").val(sat_ShiftId);
    $("#ddlShift_Sun").val(sun_ShiftId);

    $("#btnAddWeek").hide();
    $("#btnUpdateWeek").show();
    ShowHideWeekPanel(1);
}
function CopyWeekRow(obj) {
    var row = $(obj).closest("tr");
    var mon_ShiftId = $(row).find("#hdnMon_ShiftId").val();
    var tue_ShiftId = $(row).find("#hdnTue_ShiftId").val();
    var wed_ShiftId = $(row).find("#hdnWed_ShiftId").val();
    var thu_ShiftId = $(row).find("#hdnThu_ShiftId").val();
    var fri_ShiftId = $(row).find("#hdnFri_ShiftId").val();
    var sat_ShiftId = $(row).find("#hdnSat_ShiftId").val();
    var sun_ShiftId = $(row).find("#hdnSun_ShiftId").val();

    $("#ddlShift_Mon").val(mon_ShiftId);
    $("#ddlShift_Tue").val(tue_ShiftId);
    $("#ddlShift_Wed").val(wed_ShiftId);
    $("#ddlShift_Thu").val(thu_ShiftId);
    $("#ddlShift_Fri").val(fri_ShiftId);
    $("#ddlShift_Sat").val(sat_ShiftId);
    $("#ddlShift_Sun").val(sun_ShiftId);

    $("#btnAddWeek").show();
    $("#btnUpdateWeek").hide();
    ShowHideWeekPanel(1);
}

function RemoveWeekRow(obj) {
    if (confirm("Do you want to remove selected Week No. Roaster?")) {
        var row = $(obj).closest("tr");
        var weekNo = $(row).find("#hdnWeekNo").val();
        ShowModel("Alert", "Week Roaster Removed from List.");
        row.remove();

    }
}

function ShowHideWeekPanel(action) {
    if (action == 1) {
        $(".weeksection").show();
    }
    else {
        $(".weeksection").hide();
        $("#hdnWeekSequenceNo").val(0);
        $("#hdnRoasterWeekId").val(0);
        $("#ddlWeekNo").val(0);
        $("#ddlShift_Mon").val(0);
        $("#ddlShift_Tue").val(0);
        $("#ddlShift_Wed").val(0);
        $("#ddlShift_Thu").val(0);
        $("#ddlShift_Fri").val(0);
        $("#ddlShift_Sat").val(0);
        $("#ddlShift_Sun").val(0);

        $("#btnAddWeek").show();
        $("#btnUpdateWeek").hide();
    }
}