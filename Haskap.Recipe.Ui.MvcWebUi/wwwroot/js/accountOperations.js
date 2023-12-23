let searchResultTable = null;


function showUpdatePermissionsModal(userId) {
    $.ajax({
        type: "GET",
        url: '/Account/LoadUpdatePermissionsViewComponent', //'@Url.Action("LoadCreateReservationViewComponent", "Schedule")',
        data: {
            userId: userId
        }
    }).done(function (result, status, xhr) {
        $("#userUpdatePermissionsModalContent").html(result);
    });

    userUpdatePermissionsModal = new bootstrap.Modal(document.getElementById(modal.userUpdatePermissionsModalId), wizardModalOptions);
    userUpdatePermissionsModal.show();
}

function hideUserUpdatePermissionsModal() {
    userUpdatePermissionsModal.hide();
}

function showUpdateRolesModal(userId) {
    $.ajax({
        type: "GET",
        url: '/Account/LoadUpdateRolesViewComponent', //'@Url.Action("LoadCreateReservationViewComponent", "Schedule")',
        data: {
            userId: userId
        }
    }).done(function (result, status, xhr) {
        $("#userUpdateRolesModalContent").html(result);
    });

    userUpdateRolesModal = new bootstrap.Modal(document.getElementById(modal.userUpdateRolesModalId), wizardModalOptions);
    userUpdateRolesModal.show();
}

function hideUserUpdateRolesModal() {
    userUpdateRolesModal.hide();
}
