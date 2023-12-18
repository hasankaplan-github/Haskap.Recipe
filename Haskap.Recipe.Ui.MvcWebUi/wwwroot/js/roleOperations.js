let roleTable = null;


function showRoleUpdatePermissionsModal(roleId) {
    $.ajax({
        type: "GET",
        url: '/Role/LoadUpdatePermissionsViewComponent', //'@Url.Action("LoadCreateReservationViewComponent", "Schedule")',
        data: {
            roleId: roleId
        }
    }).done(function (result, status, xhr) {
        $("#roleUpdatePermissionsModalContent").html(result);
    });

    roleUpdatePermissionsModal = new bootstrap.Modal(document.getElementById(modal.roleUpdatePermissionsModalId), wizardModalOptions);
    roleUpdatePermissionsModal.show();
}

function hideRoleUpdatePermissionsModal() {
    roleUpdatePermissionsModal.hide();
}