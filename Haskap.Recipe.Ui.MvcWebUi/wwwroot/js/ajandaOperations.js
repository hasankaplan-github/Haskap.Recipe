var calendar = null;
var changeReservationDateCalendar = {};
let searchResultTable = null;


const eventColor = {
    temporaryColor: 'bg-secondary',
    permanentColor: 'bg-primary',
    holidayColor: 'bg-danger',
    specialDayColor: 'bg-secondary'
};



function showCreateReservationModalIfDateRangeIsAvailable(startDateTime, endDateTime) {
    let antiforgeryToken = $("#antiforgeryToken").val();

    $.ajax({
        type: 'POST',
        headers: {
            'RequestVerificationToken': antiforgeryToken
        },
        url: '/Schedule/ReservationDateOverlaps', //'@Url.Action("ReservationDateOverlaps", "Schedule")',
        data: {
            startDateTime: startDateTime,
            endDateTime: endDateTime
        }
    }).done(function (data, status, xhr) {
        if (data.overlaps) {
            Swal.fire(
                'Hata',
                'Seçtiğiniz saat aralığı başka rezervasyonlarla çakışmaktadır. Yeni saat aralığı seçiniz!',
                'error'
            );
            return;
        }

        showCreateReservationModal(startDateTime, endDateTime);
    });
}

function showCreateReservationModal(startDateTime, endDateTime) {
    $.ajax({
        type: "GET",
        url: '/Schedule/LoadCreateReservationViewComponent', //'@Url.Action("LoadCreateReservationViewComponent", "Schedule")',
        data: {
            startDateTime: startDateTime,
            endDateTime: endDateTime
        }
    }).done(function (result, status, xhr) {
        $("#createReservationModalContent").html(result);
    });

    createReservationModal = new bootstrap.Modal(document.getElementById(modal.createReservationModalId), wizardModalOptions);
    createReservationModal.show();
}


function hideEditReservationModal() {
    editReservationModal.hide();
}

function showEditReservationModal(reservationId) {
    $.ajax({
        type: "GET",
        url: '/Reservation/LoadEditReservationViewComponent', //'@Url.Action("LoadEditReservationViewComponent", "Reservation")',
        data: {
            reservationId: reservationId
        }
    }).done(function (result, status, xhr) {
        $("#editReservationModalContent").html(result);
    });

    editReservationModal = new bootstrap.Modal(document.getElementById(modal.editReservationModalId), wizardModalOptions);
    editReservationModal.show();
}

function hideCreateReservationModal() {
    createReservationModal.hide();
}

function loadEventOptionsModalContent(reservationId) {
    $.ajax({
        type: "GET",
        url: '/Reservation/LoadEventOptionsViewComponent', //'@Url.Action("LoadEventOptionsViewComponent", "Reservation")',
        data: {
            reservationId: reservationId
        }
    }).done(function (result, status, xhr) {
        $("#eventOptionsModalContent").html(result);
    });
}

function showEventOptionsModal(reservationId) {
    loadEventOptionsModalContent(reservationId);

    eventOptionsModal = new bootstrap.Modal(document.getElementById(modal.eventOptionsModalId), wizardModalOptions);
    eventOptionsModal.show();
}

function hideEventOptionsModal() {
    eventOptionsModal.hide();
}

function hidePaymentsModal() {
    paymentsModal.hide();
}

function loadPaymentsModalContent(reservationId) {
    $.ajax({
        type: "GET",
        url: '/Reservation/LoadPaymentsViewComponent', // '@Url.Action("LoadPaymentsViewComponent", "Reservation")',
        data: {
            reservationId: reservationId
        }
    }).done(function (result, status, xhr) {
        $("#paymentsModalContent").html(result);
    });
}

function showPaymentsModal(reservationId) {
    loadPaymentsModalContent(reservationId);

    paymentsModal = new bootstrap.Modal(document.getElementById(modal.paymentsModalId), wizardModalOptions);
    paymentsModal.show();
}

function hideCateringsModal() {
    cateringsModal.hide();
}

function showCateringsModal(reservationId) {
    $.ajax({
        type: "GET",
        url: '/Reservation/LoadCateringsViewComponent', // '@Url.Action("LoadCateringsViewComponent", "Reservation")',
        data: {
            reservationId: reservationId
        }
    }).done(function (result, status, xhr) {
        $("#cateringsModalContent").html(result);
    });

    cateringsModal = new bootstrap.Modal(document.getElementById(modal.cateringsModalId), wizardModalOptions);
    cateringsModal.show();
}



$('#' + modal.changeReservationDateCalendarModalId).on('shown.bs.modal', function () {
    changeReservationDateCalendar.render();
})

function showChangeReservationDateCalendarModal(reservationId) {
    $.ajax({
        type: "GET",
        url: '/Schedule/LoadChangeReservationDateViewComponent', // '@Url.Action("LoadChangeReservationDateViewComponent", "Schedule")',
        data: {
            reservationId: reservationId
        }
    }).done(function (result, status, xhr) {
        $("#changeReservationDateCalendarModalContent").html(result);

        let calendarEl = document.getElementById('changeReservationDateCalendar');
        changeReservationDateCalendar = new FullCalendar.Calendar(calendarEl, {
            timeZone: 'local',
            initialView: 'dayGridMonth',
            showNonCurrentDates: false,
            fixedWeekCount: false,
            locale: 'tr',
            contentHeight: 500,
            headerToolbar: {
                start: '', // will normally be on the left. if RTL, will be on the right
                center: 'title',
                end: 'dayGridMonth today prev,next' // will normally be on the right. if RTL, will be on the left
            },

            //callbacks
            datesSet: function (dateInfo) {
                clearEventsForChangeReservationDateCalendar();
                if (changeReservationDateCalendar.view.type == 'dayGridMonth') {
                    changeReservationDateCalendar.setOption('selectable', false);
                    setDayGridMonthViewEventsForChangeReservationDateCalendar(dateInfo.startStr, dateInfo.endStr, reservationId);
                }
                else if (changeReservationDateCalendar.view.type == 'timeGridDay') {
                    changeReservationDateCalendar.setOption('selectable', true);
                    setTimeGridDayViewEventsForChangeReservationDateCalendar(dateInfo.startStr, reservationId);
                }
            },
            dateClick: function (dateClickInfo) {
                if (changeReservationDateCalendar.view.type == 'dayGridMonth') {
                    changeReservationDateCalendar.changeView('timeGridDay', dateClickInfo.dateStr);
                }
            },
            //eventClick: function (eventClickInfo) {
            //    if (changeReservationDateCalendar.view.type == 'timeGridDay') {
            //        let reservationId = eventClickInfo.event.id;
            //        showEventOptionsModal(reservationId);
            //    }
            //},
            select: function (selectionInfo) {
                if (selectionInfo.allDay == false) {
                    approveNewReservationDateIfDateRangeIsAvailable(selectionInfo.startStr, selectionInfo.endStr, reservationId);
                }
            },
            eventContent: function (arg) {
                if (changeReservationDateCalendar.view.type == "dayGridMonth" || arg.event.allDay) {
                    return { html: arg.event.title };
                }
            },
            selectOverlap: function (event) {
                return event.rendering !== 'background';
            }
        });
        //changeReservationDateCalendar.render();

        changeReservationDateCalendarModal = new bootstrap.Modal(document.getElementById(modal.changeReservationDateCalendarModalId), wizardModalOptions);
        changeReservationDateCalendarModal.show();
    });
}

function clearEventsForChangeReservationDateCalendar() {
    let events = changeReservationDateCalendar.getEvents();
    for (let i = 0; i < events.length; i++) {
        events[i].remove();
    }
}


function setDayGridMonthViewEventsForChangeReservationDateCalendar(startDate, endDate, reservationId) {
    let antiforgeryToken = $("#antiforgeryToken").val();

    $.ajax({
        type: 'POST',
        headers: {
            'RequestVerificationToken': antiforgeryToken
        },
        url: '/Schedule/GetDayGridMonthEventsForChangeReservationDateCalendar', //'@Url.Action("GetSpecialDaysInDateRange", "Schedule")',
        data: {
            startDateTime: startDate,
            endDateTime: endDate,
            reservationId: reservationId
        }
    }).done(function (data, status, xhr) {
        $.each(data, function (index, value) {
            let event = {
                //id: value.id,
                title: value.title,
                allDay: true,
                start: value.startDateTime,
                //end: value.reservationDate,
                classNames: [eventColor.permanentColor],
                display: 'background'
            }

            if (value.isPublicHoliday) {
                event.classNames = [eventColor.holidayColor];
            } else if (value.isSpecialDay) {
                event.classNames = [eventColor.specialDayColor];
            }

            changeReservationDateCalendar.addEvent(event);
        });
    });
}

function setTimeGridDayViewEventsForChangeReservationDateCalendar(startDate, reservationId) {
    let antiforgeryToken = $("#antiforgeryToken").val();

    $.ajax({
        type: 'POST',
        headers: {
            'RequestVerificationToken': antiforgeryToken
        },
        url: '/Schedule/GetTimeGridDayEventsForChangeReservationDateCalendar', //'@Url.Action("GetSpecialDaysInDay", "Schedule")',
        data: {
            dayDateTime: startDate,
            reservationId: reservationId
        }
    }).done(function (data, status, xhr) {
        if (data.specialDayEvent) {
            let specialDayEvent = {
                //id: value.id,
                title: data.specialDayEvent.title,
                allDay: true,
                start: data.specialDayEvent.startDateTime,
                //end: value.reservationDate,
                classNames: [eventColor.specialDayColor],
                display: 'background'
            }

            if (data.specialDayEvent.isPublicHoliday) {
                specialDayEvent.classNames = [eventColor.holidayColor];
            }

            changeReservationDateCalendar.addEvent(specialDayEvent);
        }

        $.each(data.reservations, function (index, value) {
            let event = {
                id: value.id,
                title: value.customerFirstName + ' ' + value.customerLastName,
                //allDay: true,
                start: value.reservationStartDateTime,
                end: value.reservationEndDateTime,
                classNames: [eventColor.permanentColor],
                //display: 'background'
            }

            if (value.isTemporary) {
                event.classNames = [eventColor.temporaryColor];
            }

            //dateInfo.view.calendar.addEvent(event);
            changeReservationDateCalendar.addEvent(event);
        });
    });
}

function approveNewReservationDateIfDateRangeIsAvailable(startDate, endDate, reservationId) {
    let reservationStartDate = new Date(startDate);
    let reservationEndDate = new Date(endDate);

    Swal.fire({
        icon: 'question',
        title: 'Yeni tarih: ' + reservationStartDate.getDate() + '-' + (reservationStartDate.getMonth() + 1) + '-' + reservationStartDate.getFullYear() + '</br>' +
            'Yeni başlangıç saati: ' + ("0" + reservationStartDate.getHours()).slice(-2) + ':' + ("0" + reservationStartDate.getMinutes()).slice(-2) + '</br>' +
            'Yeni bitiş saati: ' + ("0" + reservationEndDate.getHours()).slice(-2) + ':' + ("0" + reservationEndDate.getMinutes()).slice(-2) + '</br>',
        //showDenyButton: true,
        showCancelButton: true,
        cancelButtonText: 'İptal',
        confirmButtonText: 'Değiştir'
        //denyButtonText: `Don't save`,
    }).then(function (result) {
        if (result.isDenied || result.isDismissed) {
            return;
        }

        let antiforgeryToken = $("#antiforgeryToken").val();

        $.ajax({
            type: "POST",
            headers: {
                'RequestVerificationToken': antiforgeryToken
            },
            url: '/Schedule/ChangeReservationDate', // '@Url.Action("ChangeReservationDate", "Schedule")',
            data: {
                reservationId: reservationId,
                newStartDateTime: startDate,
                newEndDateTime: endDate
            }
        }).done(function (result, status, xhr) {
            Swal.fire(
                'Başarılı',
                'Reservasyon tarihi başarıyla değiştirildi.',
                'success'
            );

            hideChangeReservationDateCalendarModal();
            showEventOptionsModal(reservationId);

            if (calendar) {
                let event = calendar.getEventById(reservationId);
                event.setDates(startDate, endDate);
            } else if (searchResultTable) {
                searchResultTable.draw();
            }
        });
    });
}

function hideChangeReservationDateCalendarModal() {
    changeReservationDateCalendarModal.hide();
}