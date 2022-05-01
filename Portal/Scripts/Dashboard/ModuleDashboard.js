$(document).ready(function () {
    var hdnCurrentFinyearId = $("#hdnCurrentFinyearId");

    BindFinYearList(hdnCurrentFinyearId.val());
    $("#ddlFinYear").val(hdnCurrentFinyearId);
  //  BindCalender();
    GetStickyNotesDetails();
    GetQautationCountList();
    GetSOCountList();
    GetSICountList();
    GetEssEmployeeDetail();
    GetDashboardThought();
    GetSISumByUserId();
    GetTodayPISumAmount();
    $('#txtStickyNoteMessage').bind('blur', function () {
        SaveStickyNotesData();
    });
});

function BindFinYearList(selectedFinYear) {
    $.ajax({
        type: "GET",
        url: "../Product/GetFinYearList",
        data: "{}",
        dataType: "json",
        asnc: false,
        success: function (data) {
            $.each(data, function (i, item) {
                $("#ddlFinYear").append($("<option></option>").val(item.FinYearId).html(item.FinYearDesc));
            });
            $("#ddlFinYear").val(selectedFinYear);
        },
        error: function (Result) {

        }
    });
}

function SetFinancialYearSession() {
    var finYearId = $("#ddlFinYear option:selected").val();
    var data = { finYearId: finYearId };
    $.ajax({
        type: "POST",
        url: "../Dashboard/SetFinancialYear",
        data: data,
        asnc: false,
        success: function (data) {
            GetQautationCountList();
            GetSOCountList();
            GetSICountList();

        },
        error: function (Result) {

        }
    });
}

function GetQautationCountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetQuatationCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivQuatationCount").html("");
            $("#DivQuatationCount").html(err);

        },
        success: function (data) {
            $("#DivQuatationCount").html("");
            $("#DivQuatationCount").html(data);

            // var ctx1 = document.getElementById("barcanvasQuatation").getContext("2d");
            //window.myBar = new Chart(ctx1,
            //    {
            //        type: 'bar',
            //        data: barChartData,
            //        options:
            //            {
            //                title:
            //                {
            //                    display: true,
            //                    text: ""
            //                },
            //                responsive: true,
            //                maintainAspectRatio: true
            //            }
            //    });


        }
    });
}

function GetSOCountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetSOCountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivSOCount").html("");
            $("#DivSOCount").html(err);

        },
        success: function (data) {
            $("#DivSOCount").html("");
            $("#DivSOCount").html(data);

            //   var ctx1 = document.getElementById("barcanvasSaleOrder").getContext("2d");
            //window.myBar = new Chart(ctx1,
            //    {
            //        type: 'bar',
            //        data: barChartData,
            //        options:
            //            {
            //                title:
            //                {
            //                    display: true,
            //                    text: ""
            //                },
            //                responsive: true,
            //                maintainAspectRatio: true
            //            }
            //    });


        }
    });
}

function GetSICountList() {

    var requestData = {};
    $.ajax({
        url: "../Dashboard/GetSICountList",
        data: requestData,
        dataType: "html",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#DivSICount").html("");
            $("#DivSICount").html(err);

        },
        success: function (data) {
            $("#DivSICount").html("");
            $("#DivSICount").html(data);

            //  var ctx1 = document.getElementById("barcanvasSI").getContext("2d");
            //window.myBar = new Chart(ctx1,
            //    {
            //        type: 'bar',
            //        data: barChartData,
            //        options:
            //            {
            //                title:
            //                {
            //                    display: true,
            //                    text: ""
            //                },
            //                responsive: true,
            //                maintainAspectRatio: true
            //            }
            //    });


        }
    });
}

function GetEssEmployeeDetail() {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Dashboard/GetESSEmployeeDetail",
        data: {},
        dataType: "json",
        success: function (data) {
            $("#userName").val(data.FirstName);
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }
    });

}

function GetDashboardThought() {
    $.ajax({
        url: "../Dashboard/GetDashboardThoughtList",
        type: "GET",
        error: function (err) {
            $("#carousel-example-generic").html("");
            $("#carousel-example-generic").html(err);
        },
        success: function (data) {
            $("#carousel-example-generic").html("");
            $("#carousel-example-generic").html(data);
        }
    });
}

function BindCalender() {
    var d = new Date();
    var strDate = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
    var events = [];
    $('#divCalendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,basicWeek,basicDay'
        },
        defaultDate: strDate, //strDate
        navLinks: true, // can click day/week names to navigate views
        editable: true,
        height: 320,
        timeFormat: 'H:mm',
        eventLimit: true, // allow "more" link when too many events
        events: function (start, end, timezone, callback) {
            $.ajax({
                type: "GET",
                asnc: false,
                url: "../Dashboard/GetHolidayandActivityDetails",
                success: function (data) {
                    if (data != null) {
                        $(data).each(function () {
                            events.push({
                                title: $(this).attr('title'),
                                start: $(this).attr('start'),
                                end: $(this).attr('end'),
                                tooltip: $(this).attr('title'),
                                id: $(this).attr('id')
                            });
                        });
                        callback(events);
                        //events.push(data);
                        //callback(events);
                    }
                },
                error: function (Result) {
                    ShowModel("Alert", "Problem in Request");
                }

            });
        },
        eventMouseover: function (data, event, view) {

            tooltip = '<div class="tooltiptopicevent" style="width:auto;height:auto;background:#337ab7;position:absolute;z-index:10001;padding:10px 10px 10px 10px ;color:white;  line-height: 200%;">' + 'Event ' + ': ' + data.title + '</div>'; //'</br>' + 'Date ' + ': ' + data.start +
            $("body").append(tooltip);
            $(this).mouseover(function (e) {
                $(this).css('z-index', 10000);
                $('.tooltiptopicevent').fadeIn('500');
                $('.tooltiptopicevent').fadeTo('10', 1.9);
            }).mousemove(function (e) {
                $('.tooltiptopicevent').css('top', e.pageY + 10);
                $('.tooltiptopicevent').css('left', e.pageX + 20);
            });


        },
        eventMouseout: function (data, event, view) {
            $(this).css('z-index', 8);

            $('.tooltiptopicevent').remove();

        }
        //events: [
        //    {
        //        title: 'All Day Event',
        //        start: '2017-08-01'
        //    },
        //    {
        //        title: 'Long Event',
        //        start: '2017-08-07',
        //        end: '2017-08-10'
        //    },
        //    {
        //        id: 999,
        //        title: 'Repeating Event',
        //        start: '2017-07-09T16:00:00'
        //    },
        //    {
        //        id: 999,
        //        title: 'Repeating Event',
        //        start: '2017-07-16T16:00:00'
        //    },
        //    {
        //        title: 'Conference',
        //        start: '2017-09-11',
        //        end: '2017-09-13'
        //    },
        //    {
        //        title: 'Meeting',
        //        start: '2017-07-12T10:30:00',
        //        end: '2017-07-12T12:30:00'
        //    },
        //    {
        //        title: 'Lunch',
        //        start: '2017-07-12T12:00:00'
        //    },
        //    {
        //        title: 'Meeting',
        //        start: '2017-07-12T14:30:00'
        //    },
        //    {
        //        title: 'Happy Hour',
        //        start: '2017-07-12T17:30:00'
        //    },
        //    {
        //        title: 'Dinner',
        //        start: '2017-07-12T20:00:00'
        //    },
        //    {
        //        title: 'Birthday Party',
        //        start: '2017-07-13T07:00:00'
        //    },
        //    {
        //        title: 'Click for Google',
        //        url: 'http://google.com/',
        //        start: '2017-07-28'
        //    }
        //]
    });
}
function SaveStickyNotesData() {
    //var hdnUserId = $("hdnUserId");
    var hdnStickyNoteId = $("#hdnStickyNoteId");
    var StickyNotesMessage = $("#txtStickyNoteMessage");
    var stickyNotesViewModel = {
        StickyNoteMessage: StickyNotesMessage.val(),
        StickyNoteId: hdnStickyNoteId.val()
    };
    var requestData = { stickyNotesViewModel: stickyNotesViewModel };
    $.ajax({
        url: "../Dashboard/AddEditStickyNotes",
        cache: false,
        type: "POST",
        dataType: "json",
        data: JSON.stringify(requestData),
        contentType: 'application/json',
        success: function (data) {
            if (data.status == "SUCCESS") {
                // ShowModel("Alert", data.message);
                $("#txtStickyNoteMessage").val("");
                $("#hdnStickyNoteId").val(data.StickyNoteId);
                GetStickyNotesDetails();
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

function GetStickyNotesDetails() {
    $.ajax({
        type: "GET",
        asnc: false,
        url: "../Dashboard/GetStickyNotesDetails",
        success: function (data) {
            if (data != null) {
                $("#txtStickyNoteMessage").val(data.StickyNoteMessage);
                $("#hdnStickyNoteId").val(data.StickyNoteId);
            }
        },
        error: function (Result) {
            ShowModel("Alert", "Problem in Request");
        }

    });
}

function ShowModel(headerText, bodyText) {
    $("#alertModel").modal();
    $("#modelHeader").html(headerText);
    $("#modelText").html(bodyText);

}

function GetSISumByUserId() {

    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetSISumByUser",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#lblSiSumAmount").html("");
            //$("#lblSiSumAmount").html(err);

        },
        success: function (data) {
            // $("#lblSiSumAmount").html("");
            $("#lblSiSumAmount").html(data.SITotalAmountSum);


        }
    });
}

function GetTodayPISumAmount() {
    var requestData = { userId: 0, selfOrTeam: "SELF" };
    $.ajax({
        url: "../Dashboard/GetTodayPISumAmount",
        data: requestData,
        dataType: "json",
        type: "POST",
        asnc: false,
        error: function (err) {
            $("#lblTodayPISum").html("");
            $("#lblTodayPISum").html(err);

        },
        success: function (data) {
            $("#lblTodayPISum").html("");
            $("#lblTodayPISum").html(data.TodayPISumAmount);
        }
    });
}