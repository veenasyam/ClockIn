$(document).ready(function () {
    var start = new Date;

    const status = {
        Start: 'Start',
        Stop: 'Stop'
    }

    const stype = {
        Shift: 'Shift',
        Break: 'Break',
        Lunch: 'Lunch'
    }

    $('.clock').click(function (e) {
        

        var sClockInType = $(this).find(".curr-status").text();
        var sShiftType = $(this).attr('data-sfttype');
        var sDisplayType = sClockInType == status.Start ? status.Stop : status.Start;

        ////$(this).find(".curr-status").text(sDisplayType);
       
        if ($(this).attr("class").indexOf("clockInactive") > 0) {
            e.preventDefault();
            return;
        }

        $.ajax({
            type: 'POST',
            url: 'Home/Index',
            data: {
                ShiftType: sShiftType,
                ClockInType: sClockInType,
                DisplayClockInType: sDisplayType
            },
            success: function (data) {
                window.location.reload(true);
            }
        });

        ////if ((sShiftType == stype.Shift) && (sClockInType == status.Stop)) {
        ////    $('.clock').addClass("clockInactive").removeClass("clockActive");
        ////}
    });


});